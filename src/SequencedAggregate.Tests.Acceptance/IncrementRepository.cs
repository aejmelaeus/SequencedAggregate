using System;

namespace SequencedAggregate.Tests.Acceptance
{
    internal class IncrementRepository
    {
        private readonly ISequencedEventStore _sequencedEventStore;

        public IncrementRepository(ISequencedEventStore sequencedEventStore)
        {
            _sequencedEventStore = sequencedEventStore;
        }

        internal Increment GetById(Guid id)
        {
            var increment = new Increment(id);

            var events = _sequencedEventStore.GetById<Incremented>(id.ToString());

            foreach (var @event in events)
            {
                increment.Apply(@event);
            }

            return increment;
        }
    }
}