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
            var sequenceAnchor = DateTime.UtcNow.Ticks;
            var commitId = Guid.NewGuid();

            CommitEvents(id, sequenceAnchor, commitId, events);
        }

        public void CommitEvents(string id, long sequenceAnchor, Guid commitId, IEnumerable<object> events)
        {
            using (var stream = _eventStore.OpenStream(id))
            {
                var eventMessags = EventMessages.Parse(sequenceAnchor, events);

                foreach (var eventMessage in eventMessags)
                {
                    stream.Add(eventMessage);
                }

                try
                {
                    stream.CommitChanges(commitId);
                }
                catch (DuplicateCommitException)
                {
                    // Nothing here... 
                    // We want just to swallow the exception, since we know that the event
                    // is already committed and every one can live happily ever after.
                }
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