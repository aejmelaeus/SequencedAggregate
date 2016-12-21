namespace SequencedAggregate.Tests.Unit.Events
{
    internal class CompanyCategoryUpdated : EventBase
    {
        public string Id { get; set; }
        public string NewCategory { get; set; }
    }
}