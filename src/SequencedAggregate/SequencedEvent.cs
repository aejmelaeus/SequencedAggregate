namespace SequencedAggregate
{
    public class SequencedEvent
    {
        public long SequenceAnchor { get; set; }
        public int SequenceIndex { get; set; }
        public IDomainEvent DomainEvent { get; set; }
    }
}