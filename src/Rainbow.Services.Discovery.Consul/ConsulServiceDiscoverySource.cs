using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery.Consul
{
    public class ConsulServiceDiscoverySource : IServiceDiscoverySource
    {
        public IConfiguration Configuration { get; set; }
        public bool IsAsync { get; set; }
        public bool ReloadOnChange { get; set; }


        public IServiceDiscoveryProvider Build(IServiceProvider services)
        {
            return new ConsulServiceDiscoveryProvider(this, services);
        }
    }
}
