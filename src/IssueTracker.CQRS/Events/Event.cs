using System;

using MediatR;

using IssueTracker.CQRS.Commands;

namespace IssueTracker.CQRS.Events
{
    public interface IEvent : INotification
    {
        string Id { get; set; }
        string AggregateId { get; set; }
        string CommandId { get; set; }
        string UserId { get; set; }
        DateTime Created { get; set; }

        void Update(ICommand command);
    }

    public class Event : IEvent
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AggregateId { get; set; } = string.Empty;
        public string CommandId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;

        public void Update(ICommand command)
        {
            CommandId = command.Id;
            UserId = command.UserId;
        }
    }
}
