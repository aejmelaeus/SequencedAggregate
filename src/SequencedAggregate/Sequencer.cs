using System;
using System.Linq;
using NEventStore;
using System.Collections.Generic;

namespace SequencedAggregate
{
    public static class Sequencer
    {
        public static IEnumerable<TEvent> Sequence<TEvent>(ICollection<EventMessage> events) where TEvent : class 
        {
            var result = new List<SortableEventMessage<TEvent>>();

            try
            {
                foreach (var eventMessage in events)
                {
                    var anchorIndex = int.Parse(eventMessage.Headers[SequenceConstants.AnchorIndexKey].ToString());
                    var sequenceAnchor = long.Parse(eventMessage.Headers[SequenceConstants.SequenceAnchorKey].ToString());

                    result.Add(new SortableEventMessage<TEvent>
                    {
                        AnchorIndex = anchorIndex,
                        Event = eventMessage.Body as TEvent,
                        SequenceAnchor = sequenceAnchor
                    });
                }
            }
            catch (Exception)
            {
                throw new SequenceException();
            }

            return result.OrderBy(r => r.SequenceAnchor)
                         .ThenBy(r => r.AnchorIndex)
                         .Select(r => r.Event);
        }

        private class SortableEventMessage<TEvent>
        {
            public TEvent Event { get; set; }
            public long SequenceAnchor { get; set; }
            public int AnchorIndex { get; set; }
        }
    }
}