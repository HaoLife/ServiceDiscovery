using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
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
