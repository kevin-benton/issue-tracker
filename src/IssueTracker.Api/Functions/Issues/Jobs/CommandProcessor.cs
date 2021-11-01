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

using IssueTracker.CQRS.Commands;
using IssueTracker.CQRS.Domain.Models;
using IssueTracker.CQRS.Domain.Models.Factories;
using IssueTracker.CQRS.Events;

namespace IssueTracker.Api.Functions.Issues.Jobs
{
    public class CommandProcessor
    {
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IEventFactory _eventFactory;
        private readonly IEventDocumentFactory _eventDocumentFactory;

        public CommandProcessor(ILoggerFactory loggerFactory, IMediator mediator, IEventFactory eventFactory,
            IEventDocumentFactory eventDocumentFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(IssueTracker));
            _mediator = mediator;
            _eventFactory = eventFactory;
            _eventDocumentFactory = eventDocumentFactory;
        }

        [FunctionName(nameof(CommandProcessor))]
        public async Task RunAsync([CosmosDBTrigger(
                databaseName: "%CosmosConfig:DatabaseName%",
                "commands-col",
                ConnectionStringSetting = "CosmosConfig:ConnectionString",
                LeaseCollectionName = "leases-col",
                CreateLeaseCollectionIfNotExists = true)]
            IReadOnlyList<Document> input,
            [CosmosDB("%CosmosConfig:DatabaseName%", "events-col", ConnectionStringSetting = "CosmosConfig:ConnectionString")]
            IAsyncCollector<EventDocument> eventDocuments,
            CancellationToken cancellationToken)
        {
            try
            {
                if (input.Count > 0)
                {
                    var docs = input.Select(d =>
                        JsonConvert.DeserializeObject<CommandDocument>(d.ToString())
                        ?? throw new InvalidOperationException("Unable to parse command."));
                    foreach (var doc in docs.OrderBy(c => c.Created))
                    {
                        var type = Type.GetType(doc.Type);

                        if (type == null)
                        {
                            _logger.LogError("Unable to get command type.");
                            continue;
                        }

                        var command = JsonConvert.DeserializeObject(doc.Data, type);

                        if (command == null)
                        {
                            _logger.LogError("Unable to deserialize command.");
                            continue;
                        }

                        var events = await _mediator.Send((ICommand)command, cancellationToken);

                        foreach (var @event in events)
                        {
                            @event.Update((ICommand)command);
                            var e = _eventFactory.CreateConcreteEvent(@event);
                            await eventDocuments.AddAsync(_eventDocumentFactory.CreateEvent(e), cancellationToken);
                        }
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
