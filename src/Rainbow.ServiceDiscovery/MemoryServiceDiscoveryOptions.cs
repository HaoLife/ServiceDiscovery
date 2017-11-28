using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class MemoryServiceDiscoveryOptions
    {
        public MemoryServiceDiscoveryOptions(IConfiguration configuration)
        {
            Configure(configuration);
        }

        private void Configure(IConfiguration configuration)
        {
            ConfigurationBinder.Bind(configuration, this);
        }

        public Dictionary<string, string> Services { get; set; }
    }
}
