using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;

namespace Rainbow.ServiceDiscovery.Consul
{
    public class ConsulServiceDiscoverySource : IServiceDiscoverySource
    {
        public IConfiguration Configuration { get; set; }


        public ConsulServiceDiscoverySource(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IServiceDiscoveryProvider Build(IServiceProvider privider)
        {
            var factory = privider.GetRequiredService<ILoggerFactory>();
            return new ConsulServiceDiscoveryProvider(this, factory);
        }
    }
}
