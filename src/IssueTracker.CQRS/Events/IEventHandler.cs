using MediatR;

namespace IssueTracker.CQRS.Events
{
    public interface IEventHandler<in TEvent> : INotificationHandler<TEvent> where TEvent : IEvent
    {
    }
}
