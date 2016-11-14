using System;
using NEventStore;
using NUnit.Framework;
using System.Collections.Generic;
using NEventStore.Persistence.Sql.SqlDialects;

namespace SequencedAggregate.Tests.Acceptance
{
    [TestFixture]
    public class AcceptanceTests
    {
        [Test]
        public void Sequencing_WhenOlderEventIsCommitedAfterNewer_OlderEventAppliedBeforeNewer()
        {
            // Arrange
            var storeEvents = GetStoreEvents();

            var userId = Guid.NewGuid();

            var sequencedEventStore = new SequencedNEventStore(storeEvents);
            var userRepository = new UserRepository(sequencedEventStore);

            const string newestEmail = "bob.spelled.right@test.com";
            const string newEmail = "bop.spelled.wrong@test.com";

            const int earlier = 123;
            const int later = 124;

            var newestEvent = new UserEmailUpdated { NewEmail = newestEmail, SequenceAnchor = later };
            var failedEvent = new UserEmailUpdated { NewEmail = newEmail, SequenceAnchor = earlier};
            
            // Act
            // We commit the second event first, simulating that it has 
            // arrived in wrong order
            userRepository.CommitEvent(userId, newestEvent.SequenceAnchor, newestEvent);
            userRepository.CommitEvent(userId, failedEvent.SequenceAnchor, failedEvent);
            
            // Assert
            var user = userRepository.GetById(userId);
            Assert.That(user.Email, Is.EqualTo(newestEmail));
        }

        [Test]
        public void DuplicateCommit_WhenCommittingADuplicateEvent_OtherEventDiscarded()
        {
            // Arrange
            var messageId = Guid.NewGuid();
            var sequenceAnchor = 123;

            var incremented = new Incremented { MessageId = messageId };
 
            var storeEvents = GetStoreEvents();

            var id = Guid.NewGuid();

            var sequencedEventStore = new SequencedNEventStore(storeEvents);
            var incrementRepository = new IncrementRepository(sequencedEventStore);
            
            // Act
            // We simulate that we are in NServiceBus Handker that has committed the
            // event but somehting else failed and the message is retried.
            // We don't want dublicate commits.
            sequencedEventStore.CommitEvents(id.ToString(), sequenceAnchor, messageId, new List<Incremented> { incremented });
            sequencedEventStore.CommitEvents(id.ToString(), sequenceAnchor, messageId, new List<Incremented> { incremented });

            // Assert
            var increment = incrementRepository.GetById(id);

            Assert.That(increment.IncrementValue, Is.EqualTo(1));
        }

        private static IStoreEvents GetStoreEvents()
        {
            string connectionString = Environment.GetEnvironmentVariables().Contains("APPVEYOR")
                ? @"Server=(local)\SQL2014;Initial Catalog=Acceptance;User ID=sa;Password=Password12!"
                : @"Server=(local)\SQLEXPRESS; Initial Catalog=SequencedAggregate.Acceptance; Integrated Security=True";

            return Wireup.Init()
                .UsingSqlPersistence("SequencedAggregate", "System.Data.SqlClient", connectionString)
                    .WithDialect(new MsSqlDialect())
                    .InitializeStorageEngine()
                .UsingJsonSerialization()
                    .Compress()
                .Build();
        }
    }
}
