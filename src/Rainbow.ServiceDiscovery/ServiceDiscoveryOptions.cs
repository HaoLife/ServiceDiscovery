using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscoveryOptions
    {
        public ISubscriberDirectory SubscriberDirectory { get; set; }
        public IRegisterDirectory RegisterDirectory { get; set; }
        public IProxyMapper ProxyMapper { get; set; }

    }
}
