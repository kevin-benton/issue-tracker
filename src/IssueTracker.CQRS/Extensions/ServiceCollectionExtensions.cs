using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using MediatR;

using IssueTracker.CQRS.Domain.Models.Factories;
using IssueTracker.CQRS.Events;

namespace IssueTracker.CQRS.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void AddCqrs(this IServiceCollection services, IEnumerable<Type> types,
            IEnumerable<Assembly> assemblies)
        {
            services.AddMediatR(assemblies.ToArray());

            services.AddAutoMapper(types);

            services.AddScoped<IEventFactory, EventFactory>();
            services.AddScoped<ICommandDocumentFactory, CommandDocumentFactory>();
            services.AddScoped<IEventDocumentFactory, EventDocumentFactory>();
        }
    }
}
