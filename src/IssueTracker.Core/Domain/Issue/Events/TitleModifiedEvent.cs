using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using IssueTracker.Core.Domain.Issue.ReadModel;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Domain.Issue.Events
{
    public sealed class TitleModifiedEvent : Event
    {
        public TitleModifiedEvent(string id)
        {
            AggregateId = id;
        }

        public string Title { get; set; } = string.Empty;
    }

    public sealed class TitleModifiedEventHandler : IEventHandler<TitleModifiedEvent>
    {
        private readonly IIssueRepository _issues;

        public TitleModifiedEventHandler(IIssueRepository issues)
        {
            _issues = issues;
        }

        public async Task Handle(TitleModifiedEvent notification, CancellationToken cancellationToken)
        {
            var issue = await _issues.GetAsync(notification.AggregateId, cancellationToken);

            issue.Title = notification.Title;
            issue.History.Add(notification);
            issue.History = issue.History.OrderBy(e => e.Created).ToList();

            await _issues.UpdateAsync(issue, cancellationToken);
        }
    }
}
