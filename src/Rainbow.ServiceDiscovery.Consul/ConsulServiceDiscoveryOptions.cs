using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Consul
{
    public class ConsulServiceDiscoveryOptions
    {
        public ConsulServiceDiscoveryOptions(IConfiguration configuration)
        {
            Configure(configuration);
        }

        private void Configure(IConfiguration configuration)
        {
            ConfigurationBinder.Bind(configuration, this);
        }

        public Uri Address { get; set; }
        public bool IsRegister { get; set; }
        public string CheckPath { get; set; }
        public TimeSpan CheckTimeout { get; set; } = new TimeSpan(0, 0, 1);
        public TimeSpan CheckInterval { get; set; } = new TimeSpan(0, 0, 10);
    }
}
