using Autofac;

namespace SequencedAggregate
{
    public static class SequencedAggregateConfiguration
    {
        public static Module Configure<TEventBase>() where TEventBase : class, new()
        {
            // TODO: Sql thingy + tests!
            return new SequencedAggregateModule<TEventBase>();
        }
    }
}