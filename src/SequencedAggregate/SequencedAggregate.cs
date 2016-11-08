using System;
using System.Collections.Generic;
using System.Linq;

namespace SequencedAggregate
{
    public abstract class SequencedAggregate
    {
        private readonly Dictionary<Type, Action<IDomainEvent>> _routes = new Dictionary<Type, Action<IDomainEvent>>();
        private readonly Dictionary<long, List<IDomainEvent>> _uncommittedEvents = new Dictionary<long, List<IDomainEvent>>();

        protected SequencedAggregate(string aggregateId)
        {
            AggregateId = aggregateId;
        }

        public string AggregateId { get; }

        public Dictionary<long, IEnumerable<IDomainEvent>> UncommittedEvents => _uncommittedEvents.ToDictionary(u => u.Key, u => u.Value.AsEnumerable());

        public void ClearUncommittedEvents()
        {
            _uncommittedEvents.Clear();
        }

        protected void RegisterTransition<T>(Action<T> transition) where T : class
        {
            _routes.Add(typeof(T), o => transition(o as T));
        }

        protected void RaiseEvent(IDomainEvent domainEvent)
        {
            ApplyEvent(domainEvent);
            AddEvent(domainEvent, DateTime.UtcNow.Ticks);
        }

        private void AddEvent(IDomainEvent domainEvent, long sequenceAnchor)
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

        public void ApplyEvent(IDomainEvent domainEvent)
        {
            var domainEventType = domainEvent.GetType();

            if (_routes.ContainsKey(domainEventType))
            {
                _routes[domainEventType](domainEvent);
            }
        }
    }
}
