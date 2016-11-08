using System.Collections.Generic;
using System.Management.Instrumentation;

namespace SequencedAggregate
{
    public interface IDomainEvent
    {
        // Nothing here...
    }

    public interface IDomainRepository
    {
        void Commit<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;
        TResult GetById<TResult>(string id) where TResult : IAggregate, new();
        void AddEvent(IDomainEvent domainEvent, long sequenceAnchor);
    }

    public interface IAggregate
    {
        void ApplyEvent(IDomainEvent domainEvent);
    }

    public abstract class SequencedDomainRepository : IDomainRepository
    {
        public abstract void Commit<TAggregate>(TAggregate aggregate) where TAggregate : IAggregate;

        public abstract TResult GetById<TResult>(string id) where TResult : IAggregate, new();

        public void AddEvent(IDomainEvent domainEvent, long sequenceAnchor)
        {
            throw new System.NotImplementedException();
        }

        protected TResult BuildAggregate<TResult>(IEnumerable<SequencedEvent> events) where TResult : IAggregate, new()
        {
            var result = new TResult();

            var eventsInSequence = Sequencer.Sequence(events);

            foreach (var domainEvent in eventsInSequence)
            {
                result.ApplyEvent(domainEvent);
            }

            return result;
        }
    }
}