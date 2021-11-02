using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IssueTracker.Core.Settings;
using IssueTracker.CQRS.Domain.Models;
using IssueTracker.CQRS.Events;
using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using Newtonsoft.Json;

namespace IssueTracker.Core.Domain.Issue.Models
{
    public interface IIssueAggregateRepository
    {
        Task<IssueAggregate?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
    }

    public class IssueAggregateRepository : IIssueAggregateRepository
    {
        private const string EventsCollection = "events-col";

        private readonly Container _container;

        public IssueAggregateRepository(CosmosClient client, ICosmosConfig config)
        {
            _container = client.GetContainer(config.DatabaseName, EventsCollection);
        }

        public async Task<IssueAggregate?> GetByIdAsync(string id, CancellationToken cancellationToken)
        {
            var it = _container.GetItemLinqQueryable<EventDocument>(true)
                .ToFeedIterator();

            var eventList = new List<EventDocument>();

            while (it.HasMoreResults)
            {
                var results = await it.ReadNextAsync(cancellationToken);
                eventList.AddRange(results);
            }

            var databaseEvents = eventList.Select(@event =>
                {
                    var type = Type.GetType(@event.Type);
                    if (type == null)
                    {
                        throw new InvalidOperationException("Can't decipher event type.");
                    }

                    return (IEvent)JsonConvert.DeserializeObject(@event.Data, type)!;
                });
            var events = databaseEvents as IEvent[] ?? databaseEvents.ToArray();

            if (!events.Any())
            {
                return null;
            }

            var aggregate = Activator.CreateInstance<IssueAggregate>();
            aggregate.LoadFromHistory(events);
            return aggregate;
        }
    }
}
