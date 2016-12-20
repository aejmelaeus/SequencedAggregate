using System.Transactions;

namespace SequencedAggregate
{
    internal class AggregateRepository<TEventBase> : IAggregateRepository<TEventBase> where TEventBase : class, new()
    {
        private readonly ISequencedEventStore<TEventBase> _eventSource;
        private readonly IProjectionRepository<TEventBase> _projectionRepository;

        public AggregateRepository(ISequencedEventStore<TEventBase> eventSource, IProjectionRepository<TEventBase> projectionRepository)
        {
            _eventSource = eventSource;
            _projectionRepository = projectionRepository;
        }

        public TAggregate Read<TAggregate>(string id) where TAggregate : IAggregate<TEventBase>, new()
        {
            var aggreagate = new TAggregate();

            var events = _eventSource.GetById(id);

            foreach (var @event in events)
            {
                aggreagate.ApplyEvent(@event);
            }

            return aggreagate;
        }

        public void Commit(IAggregate<TEventBase> aggregate)
        {
            using (var transactionScope = new TransactionScope())
            {
                _eventSource.CommitEvents(aggregate.Id, aggregate.UncommittedEvents);

                _projectionRepository.Update(aggregate.Id, aggregate.UncommittedEvents);

                transactionScope.Complete();
            }
        }
    }
}