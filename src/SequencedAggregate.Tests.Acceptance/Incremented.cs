using System;

namespace SequencedAggregate.Tests.Acceptance
{
    internal class Incremented : TestEventBase
    {
        public Guid MessageId { get; set; }
    }
}