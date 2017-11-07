using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Attributes
{
    public class ServiceActionAttribute : Attribute
    {
        public ServiceActionAttribute()
            : this(string.Empty)
        {

        }
        public ServiceActionAttribute(string name)
        {
            this.Name = name;
        }
        public string Name { get; set; }
    }
}
