using System.Configuration;
using Autofac;

namespace SequencedAggregate
{
    public class SequencedAggregateConfiguration
    {
        private string _connectionString;

        public static SequencedAggregateConfiguration Create()
        {
            return new SequencedAggregateConfiguration();
        }

        public Module GetModule<TEventBase>() where TEventBase : class, new()
        {
            if (string.IsNullOrEmpty(_connectionString))
            {
                _connectionString = ConfigurationManager.ConnectionStrings["SequencedAggregate"].ConnectionString;
            }

            var sqlViewRepositoryConfiguration = new SqlServerViewRepositoryConfiguration(_connectionString);
            var sqlViewRepository = new SqlServerViewRepository(sqlViewRepositoryConfiguration);
            sqlViewRepository.CreateProjectionsTable();

            return new SequencedAggregateModule<TEventBase>(_connectionString);
        }

        public SequencedAggregateConfiguration WithConnectionString(string connectionString)
        {
            _connectionString = connectionString;

            return this;
        }
    }
}