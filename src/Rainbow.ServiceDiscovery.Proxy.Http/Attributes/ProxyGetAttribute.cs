using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class ProxyGetAttribute : ProxyHttpAttribute
    {
        public ProxyGetAttribute(string path = null) : base("GET", path)
        {
        }
    }
}
