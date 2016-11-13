using System;

namespace SequencedAggregate.Tests.Acceptance
{
    internal class User
    {
        public User(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
        public string Email { get; private set; }

        public void Apply(UserEmailUpdated @event)
        {
            Email = @event.NewEmail;
        }
    }
}
