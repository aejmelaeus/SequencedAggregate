using System.Collections.Generic;

namespace SequencedAggregate
{
    public interface IAggregate<TEventBase>
    {
        string Id { get; }
        bool ApplyEvent(TEventBase @event);
        IEnumerable<TEventBase> UncommittedEvents { get; }
        void ClearUncommitedEvents();
    }
}