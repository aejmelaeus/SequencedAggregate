namespace SequencedAggregate.Tests.Acceptance
{
    internal class UserEmailUpdated
    {
        public string NewEmail { get; set; }
        public long SequenceAnchor { get; set; }
    }
}