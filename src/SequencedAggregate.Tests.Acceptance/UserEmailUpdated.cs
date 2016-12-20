namespace SequencedAggregate.Tests.Acceptance
{
    internal class UserEmailUpdated : TestEventBase
    {
        public string Id { get; set; }
        public string NewEmail { get; set; }
        public long SequenceAnchor { get; set; }
    }
}