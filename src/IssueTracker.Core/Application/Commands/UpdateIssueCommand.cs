using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IssueTracker.Core.Domain.Issue.Exceptions;
using IssueTracker.Core.Domain.Issue.Models;
using IssueTracker.CQRS.Commands;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Application.Commands
{
    public sealed class UpdateIssueCommand : Command
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; } = 5;
    }

    public class UpdateIssueCommandValidator : AbstractValidator<UpdateIssueCommand>
    {
        public UpdateIssueCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Priority).InclusiveBetween(1, 5);
        }
    }

    public sealed class UpdateIssueCommandHandler : ICommandHandler<UpdateIssueCommand>
    {
        private readonly IIssueAggregateRepository _issues;

        public UpdateIssueCommandHandler(IIssueAggregateRepository issues)
        {
            _issues = issues;
        }

        public async Task<IEnumerable<IEvent>> Handle(UpdateIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = await _issues.GetByIdAsync(request.AggregateId, cancellationToken);

            if (issue == null)
            {
                throw new IssueException($"Issue {request.AggregateId} not found.");
            }

            issue.SetTitle(request.Title);
            issue.SetDescription(request.Description);
            issue.SetPriority(request.Priority);

            return issue.Events;
        }
    }
}
