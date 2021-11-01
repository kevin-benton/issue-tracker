using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

using ReflectionMagic;

using IssueTracker.CQRS.Events;

namespace IssueTracker.CQRS.Domain
{
    public interface IAggregate
    {
        string Id { get; }
        int Version { get; }
        ReadOnlyCollection<IEvent> Events { get; }

        void LoadFromHistory(IEnumerable<IEvent> events);
    }

    public abstract class Aggregate : IAggregate
    {
        public string Id { get; protected set; }
        public int Version { get; private set; }

        private readonly List<IEvent> _events = new List<IEvent>();
        public ReadOnlyCollection<IEvent> Events => _events.AsReadOnly();

        protected Aggregate()
        {
            Id = Guid.NewGuid().ToString();
        }

        protected Aggregate(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                id = Guid.NewGuid().ToString();
            }

            Id = id;
        }

        public void LoadFromHistory(IEnumerable<IEvent> events)
        {
            var domainEvents = events as IEvent[] ?? events.ToArray();

            foreach (var @event in domainEvents)
            {
                this.AsDynamic().Apply(@event);
            }

            Version = domainEvents.Length;
        }

        protected void AddEvent(IEvent @event)
        {
            _events.Add(@event);
        }

        protected void AddAndApplyEvent(IEvent @event)
        {
            _events.Add(@event);
            this.AsDynamic().Apply(@event);
        }
    }
}
