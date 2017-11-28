using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoverySource : IServiceDiscoverySource
    {
        public IConfiguration Configuration { get; set; }


        public ZookeeperServiceDiscoverySource(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IServiceDiscoveryProvider Build(IServiceProvider privider)
        {
            var factory = privider.GetRequiredService<ILoggerFactory>();
            return new ZookeeperServiceDiscoveryProvider(this, factory);
        }
    }
}
