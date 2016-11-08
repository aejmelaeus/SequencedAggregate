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
        private string _testEventValue;

        public SequencedAggregateBaseTests() : base(AggregateIdTestValue)
        {
            RegisterTransition<TestEvent>(Apply);
        }

        public void Apply(TestEvent testEvent)
        {
            _testEventValue = testEvent.Value;
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

            Assert.That(expectedDateTime, Is.EqualTo(actualDateTime).Within(TimeSpan.FromMilliseconds(100)));
        }

        [Test]
        public void RaiseEvent_WhenCalled_EventIsApplied()
        {
            // Arrange
            const string testEventValue = "ItShouldBeAppliedWhenRaised";
            var testEvent = new TestEvent { Value = testEventValue };

            // Act
            RaiseEvent(testEvent);

            // Assert
            Assert.That(_testEventValue, Is.EqualTo(testEventValue));
        }

        [Test]
        public void ClearUncommittedEvents_WhenInvoked_UncommittedEventsCleared()
        {
            // Arrange
            var testEvent = new TestEvent();

            RaiseEvent(testEvent);
            
            // Act
            ClearUncommittedEvents();

            // Assert
            Assert.That(UncommittedEvents.Count, Is.EqualTo(0));
        }
    }
}
