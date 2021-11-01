using IssueTracker.Core.Domain.Issue.Models;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Domain.Issue.Events
{
    public sealed class IssueCreatedEvent : Event
    {
        public IssueCreatedEvent(string id)
        {
            AggregateId = id;
        }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; } = 5;
    }
}
