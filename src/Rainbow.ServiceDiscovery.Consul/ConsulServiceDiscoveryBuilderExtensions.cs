using Microsoft.Extensions.Configuration;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Rainbow.ServiceDiscovery.Consul;

namespace Rainbow.ServiceDiscovery
{
    public static class ConsulServiceDiscoveryBuilderExtensions
    {
        public static IServiceDiscoveryBuilder AddConsul(this IServiceDiscoveryBuilder builder, IConfiguration configuration)
        {
            var source = new ConsulServiceDiscoverySource(configuration);
            builder.ServiceCollection.AddSingleton<IServiceDiscoveryProvider>(source.Build);
            return builder;
        }
    }
}
