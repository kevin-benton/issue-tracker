using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using IssueTracker.Core.Extensions;

[assembly: FunctionsStartup(typeof(IssueTracker.Api.Startup))]

namespace IssueTracker.Api
{
    public class Startup : FunctionsStartup
    {
        public override void Configure(IFunctionsHostBuilder builder)
        {
            builder.Services.AddLogging(config =>
            {
                config.AddFilter(nameof(IssueTracker), LogLevel.Trace);
            });

            builder.Services.AddIssueTracker();
        }
    }
}
