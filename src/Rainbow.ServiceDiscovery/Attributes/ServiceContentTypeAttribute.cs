using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Attributes
{
    public class ServiceContentTypeAttribute: Attribute
    {
        public ServiceContentTypeAttribute(string type)
        {
            this.Type = type;
        }

        public string Type { get; set; }
    }
}
