using Newtonsoft.Json;

using IssueTracker.CQRS.Events;

namespace IssueTracker.CQRS.Domain.Models.Factories
{
    public interface IEventDocumentFactory
    {
        EventDocument CreateEvent(IEvent @event);
    }

    public class EventDocumentFactory : IEventDocumentFactory
    {
        public EventDocument CreateEvent(IEvent @event)
        {
            return new EventDocument
            {
                Id = @event.Id,
                AggregateId = @event.AggregateId,
                CommandId = @event.CommandId,
                Type = @event.GetType().AssemblyQualifiedName,
                Data = JsonConvert.SerializeObject(@event),
                Created = @event.Created,
                UserId = @event.UserId
            };
        }
    }
}
