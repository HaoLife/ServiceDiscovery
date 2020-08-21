using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class ProxyPutAttribute : ProxyHttpAttribute
    {
        public ProxyPutAttribute(string path = null) : base("PUT", path)
        {
        }
    }
}
