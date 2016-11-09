using NEventStore;
using System.Collections.Generic;

namespace SequencedAggregate
{
    public class SequencedNEventStore : ISequencedEventStore
    {
        private readonly IStoreEvents _eventStore;

        public SequencedNEventStore(IStoreEvents eventStore)
        {
            _eventStore = eventStore;
        }

        public void Commit<TEvent>(string id, Dictionary<long, IEnumerable<TEvent>> events) where TEvent : class
        {
            var eventMessages = GetEventMessages(events);
        }

        private IEnumerable<EventMessage> GetEventMessages<TEvent>(Dictionary<long, IEnumerable<TEvent>> events)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerable<TEvent> GetById<TEvent>(string id) where TEvent : class
        {
            using (var stream = _eventStore.OpenStream(id))
            {
                var events = stream.CommittedEvents;
                var sequencedEvents = Sequencer.Sequence<TEvent>(events);
                return sequencedEvents;
            }
        }
    }
}