using System;
using System.Collections.Generic;

namespace SequencedAggregate
{
    public abstract class AggregateBase<TEventBase> : IAggregate<TEventBase> where TEventBase : class
    {
        public abstract string Id { get; }
        private readonly List<TEventBase> _uncommittedEvents = new List<TEventBase>();
        private readonly Dictionary<Type, Action<TEventBase>> _routes = new Dictionary<Type, Action<TEventBase>>();

        protected void RegisterTransition<T>(Action<T> transition) where T : class
        {
            _routes.Add(typeof(T), e => transition(e as T));
        }

        protected void RaiseEvent(TEventBase @event)
        {
            if (ApplyEvent(@event))
            {
                _uncommittedEvents.Add(@event);
            }
        }

        public bool ApplyEvent(TEventBase @event)
        {
            var eventType = @event.GetType();
            if (_routes.ContainsKey(eventType))
            {
                _routes[eventType](@event);
                return true;
            }
            return false;
        }

        public IEnumerable<TEventBase> UncommittedEvents => _uncommittedEvents;

        public void ClearUncommitedEvents()
        {
            _uncommittedEvents.Clear();
        }
    }
}