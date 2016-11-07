using System.Collections.Generic;
using System.Linq;

namespace SequencedAggregate
{
    public class SequencedEvents
    {
        public IDictionary<long, IEnumerable<IDomainEvent>> UncommittedEvents => _uncommittedEvents.ToDictionary(u => u.Key, u => u.Value.AsEnumerable());

        private readonly Dictionary<long, List<IDomainEvent>> _uncommittedEvents = new Dictionary<long, List<IDomainEvent>>();

        public void AddEvent(IDomainEvent domainEvent, long sequenceAnchor)
        {
            if (!_uncommittedEvents.ContainsKey(sequenceAnchor))
            {
                _uncommittedEvents.Add(sequenceAnchor, new List<IDomainEvent> { domainEvent });
            }
            else
            {
                _uncommittedEvents[sequenceAnchor].Add(domainEvent);
            }
        }
    }
}