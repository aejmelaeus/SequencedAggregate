using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Unit
{
    [TestFixture]
    public class SequencerTests
    {
        [Test]
        public void Sequence_WithSomeEvents_SortedByAnchorThenByIndex()
        {
            var firstAnchorFirstIndex = new TestEvent();
            var firstAnchorSecondIndex = new TestEvent();
            var secondAnchorFirstIndex = new TestEvent();
            var secondAnchorSecondIndex = new TestEvent();

            var se11 = new SequencedEvent
            {
                DomainEvent = firstAnchorFirstIndex,
                SequenceAnchor = 1,
                SequenceIndex = 1
            };

            var se12 = new SequencedEvent
            {
                DomainEvent = firstAnchorSecondIndex,
                SequenceAnchor = 1,
                SequenceIndex = 2
            };

            var se21 = new SequencedEvent
            {
                DomainEvent = secondAnchorFirstIndex,
                SequenceAnchor = 2,
                SequenceIndex = 1
            };

            var se22 = new SequencedEvent
            {
                DomainEvent = secondAnchorSecondIndex,
                SequenceAnchor = 2,
                SequenceIndex = 2
            };

            var sequencedEvents = new List<SequencedEvent>
            {
                se22,
                se12,
                se21,
                se11
            };

            var result = Sequencer.Sequence(sequencedEvents).ToList();

            Assert.That(result[0], Is.EqualTo(firstAnchorFirstIndex));
            Assert.That(result[1], Is.EqualTo(firstAnchorSecondIndex));
            Assert.That(result[2], Is.EqualTo(secondAnchorFirstIndex));
            Assert.That(result[3], Is.EqualTo(secondAnchorSecondIndex));
        }
    }
}