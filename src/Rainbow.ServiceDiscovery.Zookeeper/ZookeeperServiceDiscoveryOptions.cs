using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoveryOptions
    {
        public string Connection { get; set; }
        public TimeSpan SessionTimeout { get; set; }
    }
}
