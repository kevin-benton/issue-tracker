using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using FluentValidation;
using Newtonsoft.Json;

using IssueTracker.Core.Application.Commands;
using IssueTracker.CQRS.Domain.Models;
using IssueTracker.CQRS.Domain.Models.Factories;

namespace IssueTracker.Api.Functions.Issues.Api
{
    public class UpdateIssue
    {
        private readonly ILogger _logger;
        private readonly IValidator<UpdateIssueCommand> _validator;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public UpdateIssue(ILoggerFactory loggerFactory, IValidator<UpdateIssueCommand> validator,
            ICommandDocumentFactory commandDocumentFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(IssueTracker));
            _validator = validator;
            _commandDocumentFactory = commandDocumentFactory;
        }

        [FunctionName(nameof(UpdateIssue))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "put", Route = "issues/{id}")]
            HttpRequest req,
            [CosmosDB("%CosmosConfig:DatabaseName%", "commands-col", ConnectionStringSetting = "CosmosConfig:ConnectionString")]
            IAsyncCollector<CommandDocument> commands,
            string id,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Updating issue.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonConvert.DeserializeObject<UpdateIssueCommand>(requestBody) ??
                              new UpdateIssueCommand();

                var validationResult = await _validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(validationResult);
                }

                command.AggregateId = id;
                await commands.AddAsync(_commandDocumentFactory.CreateCommand(command), cancellationToken);

                return new OkResult();
            }
            catch (JsonReaderException)
            {
                return new BadRequestErrorMessageResult("Unable to parse body.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                throw;
            }
        }
    }
}
