using System.Threading;
using System.Threading.Tasks;

using IssueTracker.Core.Domain.Issue.ReadModel;
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

    public sealed class IssueCreatedEventHandler : IEventHandler<IssueCreatedEvent>
    {
        private readonly IIssueRepository _issues;

        public IssueCreatedEventHandler(IIssueRepository issues)
        {
            _issues = issues;
        }

        public async Task Handle(IssueCreatedEvent notification, CancellationToken cancellationToken)
        {
            var totalCount = await _issues.CountAllAsync(cancellationToken);

            await _issues.CreateAsync(new ReadModel.Issue
            {
                Id = notification.AggregateId,
                IssueId = (totalCount + 1).ToString(),
                Title = notification.Title,
                Description = notification.Description,
                Priority = notification.Priority
            }, cancellationToken);
        }
    }
}
