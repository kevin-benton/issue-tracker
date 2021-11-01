using System;
using System.Collections.Generic;

using Microsoft.Extensions.DependencyInjection;

using IssueTracker.CQRS.Domain.Models.Factories;

namespace IssueTracker.CQRS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCqrs(this IServiceCollection services, IEnumerable<Type> types)
        {
            services.AddAutoMapper(types);

            services.AddScoped<ICommandDocumentFactory, CommandDocumentFactory>();
            services.AddScoped<IEventDocumentFactory, EventDocumentFactory>();
        }
    }
}
