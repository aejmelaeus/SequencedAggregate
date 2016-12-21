namespace SequencedAggregate
{
    internal class SqlServerViewRepositoryConfiguration : ISqlServerViewRepositoryConfiguration
    {
        public string ConnectionString { get; }
        public string TableName { get; }

        public SqlServerViewRepositoryConfiguration(string connectionString, string tableName = "")
        {
            ConnectionString = connectionString;
            TableName = tableName;
        }
    }
}