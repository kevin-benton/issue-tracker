using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

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
    public class CreateIssue
    {
        private readonly ILogger _logger;
        private readonly IValidator<CreateIssueCommand> _validator;
        private readonly ICommandDocumentFactory _commandDocumentFactory;

        public CreateIssue(ILoggerFactory loggerFactory, IValidator<CreateIssueCommand> validator,
            ICommandDocumentFactory commandDocumentFactory)
        {
            _logger = loggerFactory.CreateLogger(nameof(IssueTracker));
            _validator = validator;
            _commandDocumentFactory = commandDocumentFactory;
        }

        [FunctionName(nameof(CreateIssue))]
        public async Task<IActionResult> RunAsync(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = "issues")]
            HttpRequest req,
            [CosmosDB("%CosmosConfig:DatabaseName%", "commands-col", ConnectionStringSetting = "CosmosConfig:ConnectionString")]
            IAsyncCollector<CommandDocument> commands,
            CancellationToken cancellationToken)
        {
            try
            {
                _logger.LogInformation("Creating new issue.");

                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                var command = JsonConvert.DeserializeObject<CreateIssueCommand>(requestBody) ?? new CreateIssueCommand();

                var validationResult = await _validator.ValidateAsync(command, cancellationToken);

                if (!validationResult.IsValid)
                {
                    return new BadRequestObjectResult(validationResult);
                }

                await commands.AddAsync(_commandDocumentFactory.CreateCommand(command), cancellationToken);

                return new CreatedResult(string.Empty, null);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred.");
                throw;
            }
        }
    }
}
