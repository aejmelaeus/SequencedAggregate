using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using SequencedAggregate.Tests.Unit.Events;

namespace SequencedAggregate.Tests.Unit
{
    [TestFixture]
    public class ProjectionBuilderTests
    {
        [Test]
        public void Handle_WhenHandlingCompanyCreatedEvent_UpdateCalledOnRepository()
        {
            // Arrange
            var projectionRepository = new TestCompanyViewRepository();

            var projectionBuilder = new CompanyProjectionBuilder
            {
                ViewRepository = projectionRepository
            };

            const string id = "SomeId";
            const string name = "SomeName";
            const string category = "SomeCategory";

            List<EventBase> events = new List<EventBase>
            {
                new CompanyCreated
                {
                    Name = name,
                    Id = id,
                    Category = category
                }
            };

            // Act
            projectionBuilder.Handle(id, events);

            var view = projectionRepository.Read<CompanyView>(id);

            // Assert
            Assert.That(view.Id, Is.EqualTo(id));
            Assert.That(view.Name, Is.EqualTo(name));
            Assert.That(view.Category, Is.EqualTo(category));
        }

        [Test]
        public void Handle_WhenHandlingCompanyUpdatedEvent_EventAppliedOnExistingCompany()
        {
            const string id = "SomeId";
            const string existingName = "SomeName";
            const string theNewCategory = "TheNewCategory";

            var projectionRepository = new TestCompanyViewRepository();

            projectionRepository.WithExistingView(id, new CompanyView
            {
                Id = id,
                Name = existingName,
                Category = "SomeCategory"
            });

            var projectionBuilder = new CompanyProjectionBuilder
            {
                ViewRepository = projectionRepository
            };

            var events = new List<EventBase>
            {
                new CompanyCategoryUpdated
                {
                    Id = id,
                    NewCategory = theNewCategory
                }
            };

            // Act
            projectionBuilder.Handle(id, events);

            var view = projectionRepository.Read<CompanyView>(id);

            Assert.That(view.Id, Is.EqualTo(id));
            Assert.That(view.Name, Is.EqualTo(existingName));
            Assert.That(view.Category, Is.EqualTo(theNewCategory));
        }

        [Test]
        public void Rebuild_WhenInvoked_AllEventsReplayedOnProjection()
        {
            // Arrange
            const string id = "SomeId";
            const string theNewName = "TheNewName";
            const string theNewCategory = "TheNewCategory";

            var events = new List<EventBase>
            {
                new CompanyCreated
                {
                    Id = id,
                    Name = "SomeName"
                },
                new CompanyNameUpdated
                {
                    Id = id,
                    NewName = theNewName
                },
                new CompanyCategoryUpdated
                {
                    Id = id,
                    NewCategory = theNewCategory
                }
            };

            var projectionRepository = new TestCompanyViewRepository();
            var eventStore = Substitute.For<ISequencedEventStore<EventBase>>();
            eventStore.GetById(id).Returns(events);

            var projectionBuilder = new CompanyProjectionBuilder
            {
                ViewRepository = projectionRepository,
                EventStore = eventStore
            };

            // Act
            projectionBuilder.Rebuild(id);
            var view = projectionRepository.Read<CompanyView>(id);

            // Assert
            Assert.That(view.Id, Is.EqualTo(id));
            Assert.That(view.Name, Is.EqualTo(theNewName));
            Assert.That(view.Category, Is.EqualTo(theNewCategory));
        }

        [Test]
        public void Handle_WhenProjectionDoesNotContainAnyOfTheEvents_CommitNotCalled()
        {
            // Arrange
            const string id = "SomeId";

            var events = new List<EventBase>
            {
                new CompanyCategoryUpdated
                {
                    Id = id,
                    NewCategory = "TheNewCategory"
                }
            };

            var projectionRepository = new TestCompanyViewRepository();

            var projectionBuilder = new CompanyNamesProjectionBuilder
            {
                ViewRepository = projectionRepository
            };

            // Act
            projectionBuilder.Handle(id, events);

            var view = projectionRepository.Read<CompanyNamesView>(id);

            // Assert
            Assert.That(view, Is.Null);
        }
    }
}
