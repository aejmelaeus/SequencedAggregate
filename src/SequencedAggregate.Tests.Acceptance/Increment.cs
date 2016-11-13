using System;

namespace SequencedAggregate.Tests.Acceptance
{
    public class Increment
    {
        internal Increment(Guid id)
        {
            Id = id;
        }

        internal Guid Id { get; }
        internal int IncrementValue { get; private set; }

        internal void Apply(Incremented incremented)
        {
            IncrementValue++;
        }
    }
}