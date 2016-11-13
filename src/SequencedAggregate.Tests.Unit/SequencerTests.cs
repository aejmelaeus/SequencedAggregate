using NEventStore;
using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;

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

            var se11 = new EventMessage
            {
                Body = firstAnchorFirstIndex,
                Headers = new Dictionary<string, object>
                {
                    { SequenceConstants.SequenceAnchorKey, 1 }, { SequenceConstants.AnchorIndexKey, 1 }
                }
            };

            var se12 = new EventMessage
            {
                Body = firstAnchorSecondIndex,
                Headers = new Dictionary<string, object>
                {
                    { SequenceConstants.SequenceAnchorKey, 1 }, { SequenceConstants.AnchorIndexKey, 2 }
                }
            };

            var se21 = new EventMessage
            {
                Body = secondAnchorFirstIndex,
                Headers = new Dictionary<string, object>
                {
                    { SequenceConstants.SequenceAnchorKey, 2 }, { SequenceConstants.AnchorIndexKey, 1 }
                }
            };

            var se22 = new EventMessage
            {
                Body = secondAnchorSecondIndex,
                Headers = new Dictionary<string, object>
                {
                    { SequenceConstants.SequenceAnchorKey, 2 }, { SequenceConstants.AnchorIndexKey, 2 }
                }
            };
            
            var sequencedEvents = new List<EventMessage>
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

        [Test]
        public void Sequence_WhenHeaderMissing_ExceptionTrhown()
        {
            var eventMessage = new EventMessage
            {
                Body = new TestEvent(),
                Headers = new Dictionary<string, object>
                {
                    { SequenceConstants.SequenceAnchorKey, 1 }
                }
            };

            var eventMessages = new List<EventMessage> { eventMessage };

            Assert.Throws<SequenceException>(() => { Sequencer.Sequence(eventMessages); });
        }
    }
}