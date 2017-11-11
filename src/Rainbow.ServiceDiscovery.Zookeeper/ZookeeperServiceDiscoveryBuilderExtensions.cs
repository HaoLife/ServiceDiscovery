using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Rainbow.ServiceDiscovery.Zookeeper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Rainbow.ServiceDiscovery
{
    public static class ZookeeperServiceDiscoveryBuilderExtensions
    {
        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, IConfiguration configuration)
        {
            
            var source = new ZookeeperServiceDiscoverySource(new ZookeeperServiceDiscoveryOptions(configuration));
            builder.ServiceCollection.AddSingleton<IServiceDiscoveryProvider>(source.Build);
            return builder;
        }

    }
}
