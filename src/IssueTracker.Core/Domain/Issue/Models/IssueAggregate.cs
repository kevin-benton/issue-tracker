using System;

using IssueTracker.Core.Domain.Issue.Events;
using IssueTracker.Core.Domain.Issue.Exceptions;
using IssueTracker.CQRS.Domain;

namespace IssueTracker.Core.Domain.Issue.Models
{
    public sealed class IssueAggregate : Aggregate
    {
        public IssueAggregate()
        {
        }

        public IssueAggregate(string id, string title, string description, int priority = 5) : base(id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                throw new IssueException("Issue must have valid ID.");
            }

            if (string.IsNullOrWhiteSpace(title))
            {
                throw new IssueException("Issue must have non-empty title.");
            }

            if (string.IsNullOrWhiteSpace(description))
            {
                throw new IssueException("Issue must have non-empty description.");
            }

            AddAndApplyEvent(new IssueCreatedEvent(id)
            {
                Title = title,
                Description = description,
                Priority = priority
            });
        }

        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; } = 5;
        public DateTime Created { get; set; } = DateTime.UtcNow;
        public DateTime Updated { get; set; } = DateTime.UtcNow;
        public DateTime? Deleted { get; set; }

        internal void Apply(IssueCreatedEvent @event)
        {
            Id = @event.AggregateId;
            Title = @event.Title;
            Description = @event.Description;
            Priority = @event.Priority;
        }
    }
}
