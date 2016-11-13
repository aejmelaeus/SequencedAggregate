using System;

namespace SequencedAggregate.Tests.Acceptance
{
    internal class UserRepository
    {
        private readonly ISequencedEventStore _sequencedEventStore;

        public UserRepository(ISequencedEventStore sequencedEventStore)
        {
            _sequencedEventStore = sequencedEventStore;
        }

        internal User GetById(Guid id)
        {
            var user = new User(id);

            var events = _sequencedEventStore.GetById(id.ToString());

            foreach (var @event in events)
            {
                user.Apply(@event as UserEmailUpdated);
            }

            return user;
        }

        internal void CommitEvent(Guid id, long sequenceAnchor, UserEmailUpdated @event)
        {
            _sequencedEventStore.CommitEvents(id.ToString(), sequenceAnchor, Guid.NewGuid(), new []{ @event });
        }
    }
}