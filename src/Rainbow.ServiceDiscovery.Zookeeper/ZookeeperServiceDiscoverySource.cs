using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoverySource : IServiceDiscoverySource
    {
        public ZookeeperServiceDiscoveryOptions Options { get; }

        public ZookeeperServiceDiscoverySource(ZookeeperServiceDiscoveryOptions options)
        {
            this.Options = options;
        }

        public IServiceDiscoveryProvider Build(IServiceProvider privider)
        {
            var factory = privider.GetRequiredService<ILoggerFactory>();
            return new ZookeeperServiceDiscoveryProvider(this, factory);
        }
    }
}
