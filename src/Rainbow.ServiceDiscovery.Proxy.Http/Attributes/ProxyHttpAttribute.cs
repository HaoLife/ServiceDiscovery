using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class ProxyHttpAttribute : Attribute
    {
        public string Method { get; set; }
        public string Path { get; set; }
        public ProxyHttpAttribute(string method, string path = null)
        {
            this.Method = method;
            this.Path = path;
        }
    }
}
