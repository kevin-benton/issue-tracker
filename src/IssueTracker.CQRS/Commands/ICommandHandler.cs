using System.Collections.Generic;

using MediatR;

using IssueTracker.CQRS.Events;

namespace IssueTracker.CQRS.Commands
{
    public interface ICommandHandler<in TCommand>
        : IRequestHandler<TCommand, IEnumerable<IEvent>> where TCommand : ICommand
    {
    }
}
