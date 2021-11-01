using FluentValidation;
using IssueTracker.CQRS.Commands;

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
}
