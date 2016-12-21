using System;
using Autofac;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Acceptance
{
    public class AcceptanceTestsBase<TEventBase> where TEventBase : class, new()
    {
        protected IContainer _container;

        private readonly string _connectionString = Environment.GetEnvironmentVariables().Contains("APPVEYOR")
            ? @"Server=(local)\SQL2014;Initial Catalog=SequencedAggregate;User ID=sa;Password=Password12!"
            : @"Data Source=SE-UTV28172; Initial Catalog=SequencedAggregate; Integrated Security=True";

        [OneTimeSetUp]
        public void OneTimeSetUp()
        {
            var bldr = new ContainerBuilder();

            var module = SequencedAggregateConfiguration
                .Create()
                .WithEventSourceConnectionString(_connectionString)
                .WithViewRepositoryConnectionString(_connectionString)
                .GetModule<TEventBase>();

            bldr.RegisterModule(module);

            _container = bldr.Build();
        }
    }
}