using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery.Consul
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
        public Uri Address { get; set; } = new Uri("http://localhost:8500/");

        public TimeSpan WaitTime { get; set; } = new TimeSpan(0, 1, 0);

    }
}
