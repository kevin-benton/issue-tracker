using System.Collections.Generic;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

using IssueTracker.Api.Functions.Issues.Api.Models;

namespace IssueTracker.Api.Functions.Issues.Api
{
    public static class GetAllIssues
    {
        [FunctionName(nameof(GetAllIssues))]
        public static IActionResult Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "issues")]
            HttpRequest req,
            [CosmosDB(
                "%CosmosConfig:DatabaseName%",
                "issues-col",
                ConnectionStringSetting = "CosmosConfig:ConnectionString",
                SqlQuery = "SELECT * FROM c ORDER BY c.issueId")] IEnumerable<IssueApiModel> issues,
            ILogger log)
        {
            log.LogInformation($"Getting all issues");
            return new OkObjectResult(issues);
        }
    }
}
