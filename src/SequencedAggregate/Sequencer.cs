using System.Collections.Generic;
using System.Linq;

namespace SequencedAggregate
{
    public static class Sequencer
    {
        public static IEnumerable<IDomainEvent> Sequence(IEnumerable<SequencedEvent> sequencedEvents)
        {
            return sequencedEvents.OrderBy(se => se.SequenceAnchor)
                                  .ThenBy(se => se.SequenceIndex)
                                  .Select(se => se.DomainEvent);
        }
    }
}