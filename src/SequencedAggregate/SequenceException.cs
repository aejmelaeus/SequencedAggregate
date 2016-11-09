using System;

namespace SequencedAggregate
{
    public class SequenceException : Exception
    {
        public SequenceException() : base("Unable to parse Sequenceinformation from Stream")
        {
            // Nothing here...
        }
    }
}