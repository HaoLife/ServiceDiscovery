using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Attributes
{
    public class GetServiceAttribute : ServiceMethodAttribute
    {
        public GetServiceAttribute()
            : base("GET")
        {

        }
    }
}
