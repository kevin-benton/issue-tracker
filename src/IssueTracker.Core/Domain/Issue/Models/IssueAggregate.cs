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

        public void SetTitle(string title)
        {
            if (Title == title)
            {
                return;
            }

            AddAndApplyEvent(new TitleModifiedEvent(Id)
            {
                Title = title
            });
        }

        public void SetDescription(string description)
        {
            if (Description == description)
            {
                return;
            }

            AddAndApplyEvent(new DescriptionModifiedEvent(Id)
            {
                Description = description
            });
        }

        public void SetPriority(int priority)
        {
            if (Priority == priority)
            {
                return;
            }

            AddAndApplyEvent(new PriorityModifiedEvent(Id)
            {
                Priority = priority
            });
        }

        internal void Apply(IssueCreatedEvent @event)
        {
            Id = @event.AggregateId;
            Title = @event.Title;
            Description = @event.Description;
            Priority = @event.Priority;
        }

        internal void Apply(TitleModifiedEvent @event)
        {
            Title = @event.Title;
            Updated = @event.Created;
        }

        internal void Apply(DescriptionModifiedEvent @event)
        {
            Description = @event.Description;
            Updated = @event.Created;
        }

        internal void Apply(PriorityModifiedEvent @event)
        {
            Priority = @event.Priority;
            Updated = @event.Created;
        }
    }
}
