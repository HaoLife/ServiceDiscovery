using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Rainbow.Services.Discovery.Consul
{
    public class ConsulServiceDiscoverySource : IServiceDiscoverySource
    {
        public IConfiguration Configuration { get; set; }
        public bool IsAsync { get; set; }


        public ConsulServiceDiscoverySource(IConfiguration configuration, bool isAsync)
        {
            this.Configuration = configuration;
            this.IsAsync = isAsync;
        }

        public IServiceDiscoveryProvider Build(IServiceProvider privider)
        {
            var factory = privider.GetRequiredService<ILoggerFactory>();
            return new ConsulServiceDiscoveryProvider(this, factory);
        }
    }
}
