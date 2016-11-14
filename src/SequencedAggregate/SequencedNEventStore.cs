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

        public void CommitEvents<TEvent>(string id, IEnumerable<TEvent> events) where TEvent : class
        {
            var sequenceAnchor = DateTime.UtcNow.Ticks;
            var commitId = Guid.NewGuid();

            CommitEvents(id, sequenceAnchor, commitId, events);
        }

        public void CommitEvents<TEvent>(string id, long sequenceAnchor, Guid commitId, IEnumerable<TEvent> events) where TEvent : class
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