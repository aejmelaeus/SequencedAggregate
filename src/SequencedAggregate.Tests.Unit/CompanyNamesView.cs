using System.Collections.Generic;

namespace SequencedAggregate.Tests.Unit
{
    internal class CompanyNamesView
    {
        public string Id { get; set; }
        public List<string> Names { get; set; } = new List<string>();
    }
}