using System;
using System.Linq;
using Autofac;
using NEventStore;
using NEventStore.Persistence.Sql.SqlDialects;

namespace SequencedAggregate
{
    internal class SequencedAggregateModule<TEventBase> : Module where TEventBase : class, new()
    {
        protected override void Load(ContainerBuilder bldr)
        {
            var asseblies = AppDomain.CurrentDomain.GetAssemblies();

            bldr.RegisterType<SqlServerViewRepository>()
                .As<IViewRepository>();

            bldr.RegisterType<ProjectionRepository<TEventBase>>()
                .As<IProjectionRepository<TEventBase>>();

            bldr.RegisterType<AggregateRepository<TEventBase>>()
                .As<IAggregateRepository<TEventBase>>();

            bldr.RegisterAssemblyTypes(asseblies)
                .Where(a => a.GetInterfaces().Any(i => i.IsAssignableFrom(typeof(IProjectionBuilder<TEventBase>))))
                .AsImplementedInterfaces()
                .PropertiesAutowired();

            bldr.RegisterInstance(GetEventSource())
                .As<IStoreEvents>();
        }

        private static IStoreEvents GetEventSource()
        {
            return Wireup
                .Init()
                .UsingSqlPersistence("EventSource")
                    .WithDialect(new MsSqlDialect())
                    .EnlistInAmbientTransaction()
                .InitializeStorageEngine()
                    .UsingJsonSerialization()
                    .Compress()
                .Build();
        }
    }
}