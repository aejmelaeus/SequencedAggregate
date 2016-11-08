using System;
using System.Collections.Generic;

namespace SequencedAggregate
{
    public abstract class SequencedAggregate : SequencedEvents
    {
        public string AggregateId { get; }

        protected SequencedAggregate(string aggregateId)
        {
            AggregateId = aggregateId;
        }

        private readonly Dictionary<Type, Action<IDomainEvent>> _routes = new Dictionary<Type, Action<IDomainEvent>>();

        protected void RegisterTransition<T>(Action<T> transition) where T : class
        {
            _routes.Add(typeof(T), o => transition(o as T));
        }

        protected void RaiseEvent(IDomainEvent domainEvent)
        {
            ApplyEvent(domainEvent);
            AddEvent(domainEvent, DateTime.UtcNow.Ticks);
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
