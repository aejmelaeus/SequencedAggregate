using System;
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

        public void CommitEvents(string id, IEnumerable<object> events)
        {
            long sequenceAnchor = DateTime.UtcNow.Ticks;
            Guid commitId = Guid.NewGuid();

            CommitEvents(id, sequenceAnchor, commitId, events);
        }

        public void CommitEvents(string id, long sequenceAnchor, Guid commitId , IEnumerable<object> events)
        {
            using (var stream = _eventStore.OpenStream(id))
            {
                var eventMessags = EventMessages.Parse(sequenceAnchor, events);

                foreach (var eventMessage in eventMessags)
                {
                    stream.Add(eventMessage);
                }

                stream.CommitChanges(Guid.NewGuid());
            }
        }

        public IEnumerable<object> GetById(string id)    
        {
            using (var stream = _eventStore.OpenStream(id))
            {
                var events = stream.CommittedEvents;
                var sequencedEvents = Sequencer.Sequence(events);
                return sequencedEvents;
            }
        }
    }
}