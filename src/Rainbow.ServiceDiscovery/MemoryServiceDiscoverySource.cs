using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace Rainbow.ServiceDiscovery
{
    public class MemoryServiceDiscoverySource : IServiceDiscoverySource
    {
        public IConfiguration Configuration { get; set; }


        public MemoryServiceDiscoverySource(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IServiceDiscoveryProvider Build(IServiceProvider privider)
        {
            var factory = privider.GetRequiredService<ILoggerFactory>();
            return new MemoryServiceDiscoveryProvider(this, factory);
        }
    }
}
