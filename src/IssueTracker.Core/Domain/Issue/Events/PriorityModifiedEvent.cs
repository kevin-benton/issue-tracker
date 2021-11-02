using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IssueTracker.Core.Domain.Issue.ReadModel;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Domain.Issue.Events
{
    public sealed class PriorityModifiedEvent : Event
    {
        public PriorityModifiedEvent(string id)
        {
            AggregateId = id;
        }

        public int Priority { get; set; } = 5;
    }

    public sealed class PriorityModifiedEventHandler : IEventHandler<PriorityModifiedEvent>
    {
        private readonly IIssueRepository _issues;

        public PriorityModifiedEventHandler(IIssueRepository issues)
        {
            _issues = issues;
        }

        public async Task Handle(PriorityModifiedEvent notification, CancellationToken cancellationToken)
        {
            var issue = await _issues.GetAsync(notification.AggregateId, cancellationToken);

            issue.Priority = notification.Priority;
            issue.Updated = notification.Created;
            issue.History.Add(notification);
            issue.History = issue.History.OrderBy(e => e.Created).ToList();

            await _issues.UpdateAsync(issue, cancellationToken);
        }
    }
}
