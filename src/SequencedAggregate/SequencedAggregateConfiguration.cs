using System.Configuration;
using Autofac;

namespace SequencedAggregate
{
    public class SequencedAggregateConfiguration
    {
        private string _eventSourceConnectionString;
        private string _viewRepositoryConnectionString;

        public Module GetModule<TEventBase>() where TEventBase : class, new()
        {
            if (string.IsNullOrEmpty(_eventSourceConnectionString))
            {
                _eventSourceConnectionString = ConfigurationManager.ConnectionStrings["EventStore"].ConnectionString;
            }

            if (string.IsNullOrEmpty(_viewRepositoryConnectionString))
            {
                _viewRepositoryConnectionString = ConfigurationManager.ConnectionStrings["ProjectionRepository"].ConnectionString;
            }

            return new SequencedAggregateModule<TEventBase>(_eventSourceConnectionString, _viewRepositoryConnectionString);
        }

        public SequencedAggregateConfiguration WithEventSourceConnectionString(string connectionString)
        {
            _eventSourceConnectionString = connectionString;

            return this;
        }

        public SequencedAggregateConfiguration WithViewRepositoryConnectionString(string connectionString)
        {
            _viewRepositoryConnectionString = connectionString;

            return this;
        }
    }
}