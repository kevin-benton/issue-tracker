using System;
using System.Collections.Generic;

using MediatR;

using IssueTracker.CQRS.Events;

namespace IssueTracker.CQRS.Commands
{
    public interface ICommand : IRequest<IEnumerable<IEvent>>
    {
        string Id { get; set; }
        string AggregateId { get; set; }
        string UserId { get; set; }
        DateTime Created { get; set; }
    }

    public class Command : ICommand
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string AggregateId { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime Created { get; set; } = DateTime.UtcNow;
    }
}
