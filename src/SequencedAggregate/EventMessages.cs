using NEventStore;
using System.Collections.Generic;

namespace SequencedAggregate
{
    internal static class EventMessages
    {
        internal static IEnumerable<EventMessage> Parse(long sequenceAnchor, IEnumerable<object> events)
        {
            var result = new List<EventMessage>();

            var index = 0;

            foreach (var @event in events)
            {
                var eventMessage = new EventMessage
                {
                    Body = @event,
                    Headers = new Dictionary<string, object>
                    {
                        { SequenceConstants.SequenceAnchorKey, sequenceAnchor },
                        { SequenceConstants.AnchorIndexKey, index }
                    }
                };

                index++;

                result.Add(eventMessage);
            }

            return result;
        }
    }
}