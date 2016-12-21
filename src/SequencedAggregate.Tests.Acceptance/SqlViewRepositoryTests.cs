using System;
using Autofac;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Acceptance
{
    [TestFixture]
    public class SqlViewRepositoryTests : AcceptanceTestsBase<TransactionEventBase>
    {
        [Test]
        public void Commit_WhenItemDoesNotExistInDatabase_ItemCreated()
        {
            // Arrange
            string id = Guid.NewGuid().ToString();
            const string name = "SomeName";
            const string category = "SomeCategory";

            var companyView = new CompanyView
            {
                Id = id,
                Category = category,
                Name = name
            };

            var viewRepository = Container.Resolve<IViewRepository>();

            // Act
            viewRepository.Commit(id, companyView);

            // Assert
            var companyViewFromDb = viewRepository.Read<CompanyView>(id);

            Assert.That(companyViewFromDb.Id, Is.EqualTo(id));
            Assert.That(companyViewFromDb.Name, Is.EqualTo(name));
            Assert.That(companyViewFromDb.Category, Is.EqualTo(category));
        }

        [Test]
        public void Commit_WhenItemExistsInDatabase_ItemUpdated()
        {
            // Arrange
            const string id = "SomeId";
            const string theNewName = "TheNewName";
            const string theNewCategory = "TheNewCategory";

            var companyView = new CompanyView
            {
                Id = id,
                Name = "AName",
                Category = "ACategory"
            };

            var viewRepository = Container.Resolve<IViewRepository>();

            viewRepository.Commit(id, companyView);

            // Act
            var updatedCompanyView = new CompanyView
            {
                Id = id,
                Name = theNewName,
                Category = theNewCategory
            };

            viewRepository.Commit(id, updatedCompanyView);

            // Assert
            var companyViewFromDb = viewRepository.Read<CompanyView>(id);

            Assert.That(companyViewFromDb.Id, Is.EqualTo(id));
            Assert.That(companyViewFromDb.Name, Is.EqualTo(theNewName));
            Assert.That(companyViewFromDb.Category, Is.EqualTo(theNewCategory));
        }
    }
}
