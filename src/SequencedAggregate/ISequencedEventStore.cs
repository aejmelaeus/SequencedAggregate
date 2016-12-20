using System;
using System.Collections.Generic;

namespace SequencedAggregate
{
    /// <summary>
    /// Implements commit and get methods for sequenced event handling.
    /// </summary>
    public interface ISequencedEventStore<TEventBase> where TEventBase : class
    {
        /// <summary>
        /// Commits events to the stream. SequenceAnchor are Ticks from DateTime.UtcNow 
        /// and the CommitId is a new Guid.
        /// </summary>
        /// <param name="id">The stream id</param>
        /// <param name="events">The events</param>
        void CommitEvents(string id, IEnumerable<TEventBase> events);

        /// <summary>
        /// Commits events to the stream.
        /// Use this method if you are reacting to events from other bounded contexts.
        /// </summary>
        /// <param name="id">The stream id</param>
        /// <param name="sequenceAnchor">The sequence anchor when the event occured</param>
        /// <param name="commitId">The commit id, for example NServiceBus Message Id</param>
        /// <param name="events">The events</param>
        void CommitEvents(string id, long sequenceAnchor, Guid commitId, IEnumerable<TEventBase> events);

        /// <summary>
        /// Returns the stream ordered by sequence anchor and by index within one anchor
        /// </summary>
        /// <param name="id">The stream id</param>
        /// <returns>The events</returns>
        IEnumerable<TEventBase> GetById(string id);
    }
}