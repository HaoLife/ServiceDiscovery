using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery.Consul
{
    public class ConsulServiceDiscoveryOptions
    {
        public Uri Address { get; set; } = new Uri("http://localhost:8500/");
        public TimeSpan WaitTime { get; set; } = new TimeSpan(0, 1, 0);
    }
}
