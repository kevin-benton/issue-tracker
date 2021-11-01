using System;
using System.Collections.Generic;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

using FluentValidation;

using IssueTracker.Core.Application.Commands;
using IssueTracker.Core.Domain.Issue.Events;
using IssueTracker.CQRS.Extensions;

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

            builder.Services.AddCqrs(new List<Type>
            {
                typeof(IssueCreatedEvent)
            });

            builder.Services.AddTransient<IValidator<CreateIssueCommand>, CreateIssueCommandValidator>();
        }
    }
}
