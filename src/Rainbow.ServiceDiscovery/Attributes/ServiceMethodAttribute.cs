using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Attributes
{
    public class ServiceMethodAttribute : Attribute
    {
        public ServiceMethodAttribute(string httpMethod)
        {
            this.HttpMethod = httpMethod;
        }
        public string HttpMethod { get; private set; }
    }
}
