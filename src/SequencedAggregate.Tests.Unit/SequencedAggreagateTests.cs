using System.Linq;
using NUnit.Framework;

namespace SequencedAggregate.Tests.Unit
{
    [TestFixture]
    public class SequencedAggreagateTests
    {
        //[Test]
        //public void AddEvent_WhenCalledWithUnseenSequence_AddedToDictionary()
        //{
        //    // Arrange
        //    const int anchor = 123;
        //    var testEvent = new TestEvent();

        //    var sequencedAggregate = new SequencedEvents();

        //    // Act
        //    sequencedAggregate.AddEvent(testEvent, anchor);

        //    // Assert
        //    Assert.That(sequencedAggregate.UncommittedEvents[anchor].Count, Is.EqualTo(1));
        //    Assert.That(sequencedAggregate.UncommittedEvents[anchor].Contains(testEvent));
        //}

        //[Test]
        //public void AddEvent_WhenCalledWithAlreadySeenSequence_AddedToDictionary()
        //{
        //    // Arrange
        //    const int anchor = 123;
        //    var firstTestEvent = new TestEvent();
        //    var lastTestEvent = new TestEvent();

        //    var sequencedAggregate = new SequencedEvents();

        //    // Act
        //    sequencedAggregate.AddEvent(firstTestEvent, anchor);
        //    sequencedAggregate.AddEvent(lastTestEvent, anchor);

        //    // Assert
        //    Assert.That(sequencedAggregate.UncommittedEvents[anchor].Count, Is.EqualTo(2));
        //    Assert.That(sequencedAggregate.UncommittedEvents[anchor].First(), Is.EqualTo(firstTestEvent));
        //    Assert.That(sequencedAggregate.UncommittedEvents[anchor].Last(), Is.EqualTo(lastTestEvent));
        //}
    }
}