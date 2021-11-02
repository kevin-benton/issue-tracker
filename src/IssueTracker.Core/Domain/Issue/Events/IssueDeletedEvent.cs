using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IssueTracker.Core.Domain.Issue.ReadModel;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Domain.Issue.Events
{
    public sealed class IssueDeletedEvent : Event
    {
        public IssueDeletedEvent(string id)
        {
            AggregateId = id;
        }
    }

    public sealed class IssueDeletedEventHandler : IEventHandler<IssueDeletedEvent>
    {
        private readonly IIssueRepository _issues;

        public IssueDeletedEventHandler(IIssueRepository issues)
        {
            _issues = issues;
        }

        public async Task Handle(IssueDeletedEvent notification, CancellationToken cancellationToken)
        {
            var issue = await _issues.GetAsync(notification.AggregateId, cancellationToken);

            issue.Updated = notification.Created;
            issue.Deleted = notification.Created;
            issue.History.Add(notification);
            issue.History = issue.History.OrderBy(e => e.Created).ToList();

            await _issues.UpdateAsync(issue, cancellationToken);
        }
    }
}
