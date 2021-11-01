using System;
using System.Collections.Generic;
using System.Reflection;

using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using FluentValidation;

using IssueTracker.Core.Application.Commands;
using IssueTracker.Core.Domain.Issue.Events;
using IssueTracker.Core.Domain.Issue.ReadModel;
using IssueTracker.Core.Settings;
using IssueTracker.CQRS.Extensions;

namespace IssueTracker.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddIssueTracker(this IServiceCollection services)
        {
            services.AddCqrs(new List<Type>
            {
                typeof(IssueCreatedEvent)
            }, new List<Assembly>
            {
                Assembly.GetAssembly(typeof(CreateIssueCommandHandler)),
                Assembly.GetAssembly(typeof(IssueCreatedEventHandler))
            });

            services.AddTransient<IValidator<CreateIssueCommand>, CreateIssueCommandValidator>();

            services.AddOptions<CosmosConfig>()
                .Configure<IConfiguration>((settings, configuration) =>
                {
                    configuration.GetSection(nameof(CosmosConfig)).Bind(settings);
                });

            services.AddSingleton<ICosmosConfig, CosmosConfig>(
                _ => _.GetRequiredService<IOptions<CosmosConfig>>().Value);

            services.AddSingleton(_ =>
            {
                var config = _.GetRequiredService<ICosmosConfig>();

                return new CosmosClient(config.ConnectionString, new CosmosClientOptions
                {
                    ConnectionMode = ConnectionMode.Direct,
                    SerializerOptions = new CosmosSerializationOptions
                    {
                        PropertyNamingPolicy = CosmosPropertyNamingPolicy.CamelCase
                    }
                });
            });

            services.AddScoped<IIssueRepository, IssueRepository>();
        }
    }
}
