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
        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, string connection, List<string> subscribes)
        {
            return builder.AddZookeeper(connection: connection, timeout: new TimeSpan(0, 0, 30), subscribes: subscribes, serviceEndpoints: null);
        }

        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, string connection, List<string> subscribes, ServiceEndpoint serviceEndpoint)
        {
            return builder.AddZookeeper(connection: connection, timeout: new TimeSpan(0, 0, 30), subscribes: subscribes, serviceEndpoints: new List<ServiceEndpoint>() { serviceEndpoint });
        }

        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, string connection, TimeSpan timeout, List<string> subscribes, ServiceEndpoint serviceEndpoint)
        {
            return builder.AddZookeeper(connection: connection, timeout: timeout, subscribes: subscribes, serviceEndpoints: new List<ServiceEndpoint>() { serviceEndpoint });
        }

        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, string connection, TimeSpan timeout, List<string> subscribes, List<ServiceEndpoint> serviceEndpoints)
        {
            return builder.AddZookeeper((source) =>
            {
                source.SessionTimeout = timeout;
                source.Connection = connection;
                source.Subscribes = subscribes;
                source.Registers = serviceEndpoints;
            });
        }

        public static IServiceDiscoveryBuilder AddZookeeper(this IServiceDiscoveryBuilder builder, Action<ZookeeperServiceDiscoverySource> action)
        {
            var source = new ZookeeperServiceDiscoverySource();
            action(source);
            builder.Add(source);
            return builder;
        }

    }
}
