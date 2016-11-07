using System;
using System.Linq;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Unit
{
    /*
    ** TODO: See if it is Whack to derive from the tested class...
    */

    [TestFixture]
    public class SequencedAggregateBaseTests : SequencedAggregate
    {
        private const string AggregateIdTestValue = "AFancyAggregateId";

        public SequencedAggregateBaseTests() : base(AggregateIdTestValue)
        {
            // Not much here (yet)
        }

        protected override void RegisterTransitions()
        {
            // Nothing here...
        }

        [Test]
        public void AggregateId_WhenCalled_ReturnsValueFromConstructor()
        {
            Assert.That(AggregateId, Is.EqualTo(AggregateIdTestValue));
        }

        [Test]
        public void RaiseEvent_WhenCalled_UncommittedEventContainsEventWithCorrectishSequenceAnchor()
        {
            // Arrange
            var someEvent = new TestEvent();
            var expectedAnchor = DateTime.UtcNow.Ticks;
            
            // Act
            RaiseEvent(someEvent);
            
            // Assert
            var actualAanchor = UncommittedEvents.First().Key;
            var expectedDateTime = new DateTime(expectedAnchor);
            var actualDateTime = new DateTime(actualAanchor);

            Assert.That(expectedDateTime, Is.EqualTo(actualDateTime).Within(TimeSpan.FromMilliseconds(1)));
        }
    }
}
