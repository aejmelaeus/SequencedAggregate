namespace SequencedAggregate.Tests.Unit.Events
{
    internal class CompanyCreated : EventBase
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
    }
}