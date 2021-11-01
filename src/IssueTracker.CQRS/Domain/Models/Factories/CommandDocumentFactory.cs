using Newtonsoft.Json;

using IssueTracker.CQRS.Commands;

namespace IssueTracker.CQRS.Domain.Models.Factories
{
    public interface ICommandDocumentFactory
    {
        CommandDocument CreateCommand(ICommand command);
    }

    public class CommandDocumentFactory : ICommandDocumentFactory
    {
        public CommandDocument CreateCommand(ICommand command)
        {
            return new CommandDocument
            {
                Id = command.Id,
                AggregateId = command.AggregateId,
                Type = command.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(command),
                Created = command.Created,
                UserId = command.UserId
            };
        }
    }
}
