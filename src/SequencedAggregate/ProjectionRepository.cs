using System;
using System.Linq;
using System.Collections.Generic;

namespace SequencedAggregate
{
    internal class ProjectionRepository<TEventBase> : IProjectionRepository<TEventBase> where TEventBase : class
    {
        private readonly IViewRepository _viewRepository;
        private readonly ISequencedEventStore<TEventBase> _eventSource;
        private readonly Dictionary<Type, IProjectionBuilder<TEventBase>> _projectionBuilders;

        public ProjectionRepository(IEnumerable<IProjectionBuilder<TEventBase>> projectionBuilders,
            IViewRepository viewRepository, ISequencedEventStore<TEventBase> eventSource)
        {
            _viewRepository = viewRepository;
            _eventSource = eventSource;
            _projectionBuilders = projectionBuilders.ToDictionary(pb => pb.ViewType);
        }

        public TView Read<TView>(string id) where TView : class
        {
            return _viewRepository.Read<TView>(id);
        }

        public void Rebuild<TView>(string id)
        {
            var viewType = typeof(TView);

            if (_projectionBuilders.ContainsKey(viewType))
            {
                _projectionBuilders[viewType].Rebuild(id);
            }
        }

        public void Update(string id, IEnumerable<TEventBase> events)
        {
            events = events.ToList();

            foreach (var projectionBuilder in _projectionBuilders.Values)
            {
                projectionBuilder.Handle(id, events);
            }
        }
    }
}