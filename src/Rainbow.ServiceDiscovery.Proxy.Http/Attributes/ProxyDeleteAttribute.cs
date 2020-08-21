using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class ProxyDeleteAttribute : ProxyHttpAttribute
    {
        public ProxyDeleteAttribute(string path = null) : base("DELETE", path)
        {
        }
    }
}
