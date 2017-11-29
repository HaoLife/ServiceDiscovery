using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class HttpServiceAttribute : Attribute
    {
        public HttpServiceAttribute(string serviceName)
        {
            this.ServiceName = serviceName;
        }

        public string ServiceName { get; set; }
    }
}
