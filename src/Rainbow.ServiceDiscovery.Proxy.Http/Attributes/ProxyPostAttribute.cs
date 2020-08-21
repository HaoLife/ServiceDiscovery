using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class ProxyPostAttribute : ProxyHttpAttribute
    {
        public ProxyPostAttribute(string path = null) : base("POST", path)
        {
        }
    }
}
