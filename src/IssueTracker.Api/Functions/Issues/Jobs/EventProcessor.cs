using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Azure.Documents;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;

using MediatR;
using Newtonsoft.Json;

using IssueTracker.CQRS.Domain.Models;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Api.Functions.Issues.Jobs
{
    public class EventProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;

        public EventProcessor(ILoggerFactory loggerFactory, IMediator mediator)
        {
            _logger = loggerFactory.CreateLogger(nameof(IssueTracker));
            _mediator = mediator;
        }

        [FunctionName(nameof(EventProcessor))]
        public async Task RunAsync([CosmosDBTrigger(
                databaseName: "%CosmosConfig:DatabaseName%",
                "events-col",
                ConnectionStringSetting = "CosmosConfig:ConnectionString",
                LeaseCollectionName = "leases-col",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input,
            CancellationToken cancellationToken)
        {
            try
            {
                if (input.Count > 0)
                {
                    var docs = input.Select(d =>
                        JsonConvert.DeserializeObject<EventDocument>(d.ToString())
                        ?? throw new InvalidOperationException("Unable to parse event."));
                    foreach (var doc in docs.OrderBy(c => c.Created))
                    {
                        var type = Type.GetType(doc.Type);

                        if (type == null)
                        {
                            _logger.LogError("Unable to get event type.");
                            continue;
                        }

                        var @event = JsonConvert.DeserializeObject(doc.Data, type);

                        if (@event == null)
                        {
                            _logger.LogError("Unable to deserialize event.");
                            continue;
                        }

                        await _mediator.Publish((IEvent)@event, cancellationToken);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                throw;
            }
        }
    }
}
