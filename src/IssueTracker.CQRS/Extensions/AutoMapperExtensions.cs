using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using AutoMapper;

using IssueTracker.CQRS.Events;

namespace IssueTracker.CQRS.Extensions
{
    public static class AutoMapperExtensions
    {
        public static IServiceCollection AddAutoMapper(this IServiceCollection services, IEnumerable<Type> types)
        {
            var autoMapperConfig = new MapperConfiguration(cfg =>
            {
                foreach (var type in types)
                {
                    var eventTypes = type.Assembly.GetTypes()
                        .Where(t => t.GetTypeInfo().IsClass && !t.GetTypeInfo().IsAbstract && typeof(IEvent).IsAssignableFrom(t))
                        .ToList();

                    foreach (var eventType in eventTypes)
                    {
                        cfg.CreateMap(eventType, eventType);
                    }
                }
            });

            services.AddSingleton(sp => autoMapperConfig.CreateMapper());

            return services;
        }
    }
}
