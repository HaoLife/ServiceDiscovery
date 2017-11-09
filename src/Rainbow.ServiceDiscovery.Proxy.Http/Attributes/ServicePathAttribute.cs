using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Attributes
{
    public class ServicePathAttribute : Attribute
    {
        public ServicePathAttribute(string path)
        {
            this.Path = path;
        }

        public string Path { get; set; }
    }
}
