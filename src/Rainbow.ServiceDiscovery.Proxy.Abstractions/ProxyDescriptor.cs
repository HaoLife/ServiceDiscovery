using System;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public class ProxyDescriptor
    {
        public string ServiceName { get; set; }
        public Type ProxyType { get; set; }
        public string FactoryType { get; set; }
    }
}
