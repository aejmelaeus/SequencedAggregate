using System;
using Autofac;
using System.Linq;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Acceptance
{
    [TestFixture]
    public class SequencedNEventStoreTests : AcceptanceTestsBase<TestEventBase>
    {
        [Test]
        public void Sequencing_WhenOlderEventIsCommitedAfterNewer_OlderEventAppliedBeforeNewer()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();

            var sequencedEventStore = Container.Resolve<ISequencedEventStore<TestEventBase>>();

            const string newestEmail = "bob.spelled.right@test.com";
            const string newEmail = "bop.spelled.wrong@test.com";

            const int earlier = 123;
            const int later = 124;

            var newestEvent = new UserEmailUpdated { Id = userId, NewEmail = newestEmail, SequenceAnchor = later };
            var failedEvent = new UserEmailUpdated { Id = userId, NewEmail = newEmail, SequenceAnchor = earlier};
            
            // Act
            // We commit the second event first, simulating that it has 
            // arrived in wrong order
            sequencedEventStore.CommitEvent(newestEvent.Id, newestEvent.SequenceAnchor, Guid.NewGuid(), newestEvent);
            sequencedEventStore.CommitEvent(failedEvent.Id, failedEvent.SequenceAnchor, Guid.NewGuid(), failedEvent);

            // Assert
            var users = sequencedEventStore.GetById(userId.ToString());
            Assert.That((users.Last() as UserEmailUpdated).NewEmail, Is.EqualTo(newestEmail));
        }

        [Test]
        public void DuplicateCommit_WhenCommittingADuplicateEvent_OtherEventDiscarded()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var sequenceAnchor = 123;

            var incremented = new Incremented { MessageId = messageId };

            var id = Guid.NewGuid();

            var sequencedEventStore = Container.Resolve<ISequencedEventStore<TestEventBase>>();

            // Act
            // We simulate that we are in NServiceBus Handler that has committed the
            // event but somehting else failed and the message is retried.
            // We don't want dublicate commits.
            sequencedEventStore.CommitEvent(id.ToString(), sequenceAnchor, messageId, incremented);
            sequencedEventStore.CommitEvent(id.ToString(), sequenceAnchor, messageId, incremented);

            // Assert
            var increments = sequencedEventStore.GetById(id.ToString());

            Assert.That(increments.Count(), Is.EqualTo(1));
        }
    }
}
