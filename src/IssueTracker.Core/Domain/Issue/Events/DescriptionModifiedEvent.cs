using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IssueTracker.Core.Domain.Issue.ReadModel;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Domain.Issue.Events
{
    public sealed class DescriptionModifiedEvent : Event
    {
        public DescriptionModifiedEvent(string id)
        {
            AggregateId = id;
        }

        public string Description { get; set; } = string.Empty;
    }

    public sealed class DescriptionModifiedEventHandler : IEventHandler<DescriptionModifiedEvent>
    {
        private readonly IIssueRepository _issues;

        public DescriptionModifiedEventHandler(IIssueRepository issues)
        {
            _issues = issues;
        }

        public async Task Handle(DescriptionModifiedEvent notification, CancellationToken cancellationToken)
        {
            var issue = await _issues.GetAsync(notification.AggregateId, cancellationToken);

            issue.Description = notification.Description;
            issue.History.Add(notification);
            issue.History = issue.History.OrderBy(e => e.Created).ToList();

            await _issues.UpdateAsync(issue, cancellationToken);
        }
    }
}
