using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Attributes
{
    public class ServiceRouteAttribute : Attribute
    {
        public string Path { get; set; }

    }
}
