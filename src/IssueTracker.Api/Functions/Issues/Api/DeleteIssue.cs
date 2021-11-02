using System;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using IssueTracker.Core.Application.Commands;
using IssueTracker.CQRS.Domain.Models;
using IssueTracker.CQRS.Domain.Models.Factories;

namespace IssueTracker.Api.Functions.Issues.Api
{
    public class DeleteIssue
    {
        private readonly ILogger _logger;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public DeleteIssue(ILoggerFactory loggerFactory, ICommandDocumentFactory commandDocumentFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(IssueTracker));
            _commandDocumentFactory = commandDocumentFactory;
        }

        [FunctionName(nameof(DeleteIssue))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "delete", Route = "issues/{id}")]
            HttpRequest req,
            [CosmosDB("%CosmosConfig:DatabaseName%", "commands-col", ConnectionStringSetting = "CosmosConfig:ConnectionString")]
            IAsyncCollector<CommandDocument> commands,
            string id,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Deleting issue.");
                var command = new DeleteIssueCommand
                {
                    AggregateId = id
                };
                await commands.AddAsync(_commandDocumentFactory.CreateCommand(command), cancellationToken);

                return new NoContentResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                throw;
            }
        }
    }
}
