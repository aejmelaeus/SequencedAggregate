namespace SequencedAggregate
{
    internal interface ISqlServerViewRepositoryConfiguration
    {
        string ConnectionString { get; }
        string TableName { get; }
    }
}