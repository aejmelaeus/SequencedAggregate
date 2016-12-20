using System.Collections.Generic;

namespace SequencedAggregate
{
    public interface IProjectionRepository<in TEventBase>
    {
        TView Read<TView>(string id) where TView : class;
        void Rebuild<TView>(string id);
        void Update(string id, IEnumerable<TEventBase> events);
    }
}