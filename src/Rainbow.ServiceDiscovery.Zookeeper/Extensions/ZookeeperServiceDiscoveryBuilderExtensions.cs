using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Rainbow.ServiceDiscovery.Zookeeper;
using Microsoft.Extensions.Configuration;

namespace Rainbow.ServiceDiscovery
{
    public static class ZookeeperServiceDiscoveryBuilderExtensions
    {
        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, ZookeeperServiceDiscoveryOptions options, Action<ZookeeperServiceDiscoverySource> action)
        {
            var source = new ZookeeperServiceDiscoverySource(options);
            action(source);
            builder.Add(source);
            return builder;
        }
    }
}
