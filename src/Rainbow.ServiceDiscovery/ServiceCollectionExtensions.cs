using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Rainbow.ServiceDiscovery.Abstractions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace Rainbow.ServiceDiscovery
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDiscovery(this IServiceCollection services, IConfiguration configuration, Action<IServiceDiscoveryBuilder> configure)
        {
            services.Configure<ServiceDiscoveryOptions>(configuration);

            services.AddSingleton<IServiceDiscovery, ServiceDiscovery>();

            var builder = new ServiceDiscoveryBuilder(services);

            configure(builder);

            return services;
        }

    }
}
