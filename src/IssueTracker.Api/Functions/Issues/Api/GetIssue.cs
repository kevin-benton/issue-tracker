using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IssueTracker.Api.Functions.Issues.Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace IssueTracker.Api.Functions.Issues.Api
{
    public static class GetIssue
    {
        [FunctionName(nameof(GetIssue))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "issues/{id}")]
            HttpRequest req,
            [CosmosDB(
                "%CosmosConfig:DatabaseName%",
                "issues-col",
                ConnectionStringSetting = "CosmosConfig:ConnectionString",
                SqlQuery = "SELECT TOP 1 * FROM c WHERE c.issueId = {id}")] IEnumerable<IssueApiModel> issues,
            ILogger log,
            string id)
        {
            log.LogInformation($"Getting issue with ID {id}");

            var issueApiModels = issues as IssueApiModel[] ?? issues.ToArray();

            if (issueApiModels.Any())
            {
                return new OkObjectResult(issueApiModels.FirstOrDefault());
            }

            return new NotFoundResult();
        }
    }
}
