using System.Collections.Generic;

namespace SequencedAggregate
{
    public interface ISequencedEventStore
    {
        void Commit<TEvent>(string id, Dictionary<long, IEnumerable<TEvent>> events) where TEvent : class;
        IEnumerable<TEvent> GetById<TEvent>(string id) where TEvent : class;
    }
}