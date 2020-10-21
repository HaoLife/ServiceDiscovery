using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class ConfigServiceDiscoverySource : IServiceDiscoverySource
    {
        public IConfiguration Configuration { get; set; }

        public bool ReloadOnChange { get; set; }


        public ConfigServiceDiscoverySource(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }

        public IServiceDiscoveryProvider Build(IServiceProvider services)
        {
            return new ConfigServiceDiscoveryProvider(this);
        }
    }
}
