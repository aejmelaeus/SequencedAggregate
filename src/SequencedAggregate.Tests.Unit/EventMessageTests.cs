using System.Linq;
using NUnit.Framework;
using System.Collections.Generic;
using SequencedAggregate.Tests.Unit.Events;

namespace SequencedAggregate.Tests.Unit
{
    [TestFixture]
    public class EventMessageTests
    {
        [Test]
        public void Parse_WithTwoEvents_BothGetAnchorAndFirstGetZeroIndexAndSecondGetsOneIndex()
        {
            // Arrange
            const long sequenceAnchor = 123;

            var firstEvent = new TestEvent();
            var secondEvent = new TestEvent();

            var events = new List<object> { firstEvent, secondEvent };

            // Act
            var result = EventMessages.Parse(sequenceAnchor, events).ToList();

            // Assert
            Assert.That(result.Count, Is.EqualTo(2));

            var firstAnchor = result.First().Headers[SequenceConstants.SequenceAnchorKey];
            var secondAnchor = result.Last().Headers[SequenceConstants.SequenceAnchorKey];

            var firstIndex = result.First().Headers[SequenceConstants.AnchorIndexKey];
            var secondIndex = result.Last().Headers[SequenceConstants.AnchorIndexKey];

            Assert.That(long.Parse(firstAnchor.ToString()), Is.EqualTo(sequenceAnchor));
            Assert.That(long.Parse(secondAnchor.ToString()), Is.EqualTo(sequenceAnchor));

            Assert.That(int.Parse(firstIndex.ToString()), Is.EqualTo(0));
            Assert.That(int.Parse(secondIndex.ToString()), Is.EqualTo(1));
        }
    }
}