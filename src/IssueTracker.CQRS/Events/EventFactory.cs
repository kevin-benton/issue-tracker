using AutoMapper;

namespace IssueTracker.CQRS.Events
{
    public interface IEventFactory
    {
        dynamic CreateConcreteEvent(object @event);
    }

    public class EventFactory : IEventFactory
    {
        private readonly IMapper _mapper;

        public EventFactory(IMapper mapper)
        {
            _mapper = mapper;
        }

        public dynamic CreateConcreteEvent(object @event)
        {
            var type = @event.GetType();
            return _mapper.Map(@event, type, type);
        }
    }
}
