using System;
using System.Collections.Generic;

namespace SequencedAggregate
{
    public abstract class SequencedAggregateBase
    {
        public string AggregateId { get; }

        protected SequencedAggregateBase(string aggregateId)
        {
            AggregateId = aggregateId;
        }

        private readonly Dictionary<long, List<IDomainEvent>> _uncommitedEvents = new Dictionary<long, List<IDomainEvent>>();
        private readonly Dictionary<Type, Action<IDomainEvent>> _routes = new Dictionary<Type, Action<IDomainEvent>>();

        protected abstract void RegisterTransitions();

        protected void RegisterTransition<T>(Action<T> transition) where T : class
        {
            _routes.Add(typeof(T), o => transition(o as T));
        }

        protected void RaiseEvent(IDomainEvent domainEvent)
        {
            ApplyEvent(domainEvent);

            

            //_uncommitedEvents.Add(domainEvent);
        }

        protected void RaiseEvent(IDomainEvent domainEvent, long sequenceAnchor)
        {
            if (_uncommitedEvents.ContainsKey(sequenceAnchor))
            {
                
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
