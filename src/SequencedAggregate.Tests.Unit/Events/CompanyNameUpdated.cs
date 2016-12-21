namespace SequencedAggregate.Tests.Unit.Events
{
    internal class CompanyNameUpdated : EventBase
    {
        public string Id { get; set; }
        public string NewName { get; set; }
    }
}