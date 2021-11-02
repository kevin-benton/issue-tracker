using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using IssueTracker.Core.Domain.Issue.Exceptions;
using IssueTracker.Core.Domain.Issue.Models;
using IssueTracker.CQRS.Commands;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Application.Commands
{
    public sealed class DeleteIssueCommand : Command
    {
    }

    public sealed class DeleteIssueCommandHandler : ICommandHandler<DeleteIssueCommand>
    {
        private readonly IIssueAggregateRepository _issues;

        public DeleteIssueCommandHandler(IIssueAggregateRepository issues)
        {
            _issues = issues;
        }

        public async Task<IEnumerable<IEvent>> Handle(DeleteIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = await _issues.GetByIdAsync(request.AggregateId, cancellationToken);

            if (issue == null)
            {
                throw new IssueException($"Issue {request.AggregateId} not found.");
            }

            issue.Delete();

            return issue.Events;
        }
    }
}
