using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public class ServiceProxyDescriptor
    {
        public string ServiceName { get; set; }
        public Type ProxyType { get; set; }
        public string ProviderName { get; set; }
        public string LoadBalancer { get; set; }
        public string Prefix { get; set; }
    }
}
