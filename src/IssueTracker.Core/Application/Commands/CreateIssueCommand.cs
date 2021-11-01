using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using FluentValidation;

using IssueTracker.Core.Domain.Issue.Models;
using IssueTracker.CQRS.Commands;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Core.Application.Commands
{
    public sealed class CreateIssueCommand : Command
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Priority { get; set; } = 5;
    }

    public class CreateIssueCommandValidator : AbstractValidator<CreateIssueCommand>
    {
        public CreateIssueCommandValidator()
        {
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
            RuleFor(x => x.Priority).InclusiveBetween(1, 5);
        }
    }

    public sealed class CreateIssueCommandHandler : ICommandHandler<CreateIssueCommand>
    {
        public async Task<IEnumerable<IEvent>> Handle(CreateIssueCommand request, CancellationToken cancellationToken)
        {
            var issue = new IssueAggregate(request.AggregateId, request.Title, request.Description, request.Priority);
            return await Task.FromResult(issue.Events);
        }
    }
}
