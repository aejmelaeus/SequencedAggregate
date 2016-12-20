using System;
using System.Collections.Generic;

namespace SequencedAggregate
{
    public interface IProjectionBuilder<in TEventBase> where TEventBase : class
    {
        void Rebuild(string id, IEnumerable<TEventBase> events);
        void Handle(string id, IEnumerable<TEventBase> events);
        Type ViewType { get; }
    }
}