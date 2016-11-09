using System.Collections.Generic;
using NEventStore;

namespace SequencedAggregate
{
    public static class EventMessages
    {
        public static IEnumerable<EventMessage> Parse<TEvent>(Dictionary<long, IEnumerable<TEvent>> events)
        {
            return null;
        }
    }
}