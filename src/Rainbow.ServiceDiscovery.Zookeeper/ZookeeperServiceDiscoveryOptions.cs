using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoveryOptions
    {
        public ZookeeperServiceDiscoveryOptions(IConfiguration configuration)
        {
            Configure(configuration);
        }

        private void Configure(IConfiguration configuration)
        {
            ConfigurationBinder.Bind(configuration, this);
        }

        public string Connection { get; set; }
        public TimeSpan SessionTimeout { get; set; }
        public bool IsRegister { get; set; }
    }
}
