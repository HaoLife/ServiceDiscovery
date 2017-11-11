using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscoveryOptions
    {
        
        public string Name { get; set; }
        public string Url { get; set; }
    }
}
