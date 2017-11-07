using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Routes
{
    public class RouteContext
    {
        public RouteContext()
        {
            this.IgnoreParams = new List<string>();
        }
        public ServiceInvokeContext InvokeContext { get; set; }

        public string ServicePath { get; set; }
        public string ActionPath { get; set; }
        public string Method { get; set; }

        public List<string> IgnoreParams { get; set; }

        public string ContentType { get; set; }

    }
}
