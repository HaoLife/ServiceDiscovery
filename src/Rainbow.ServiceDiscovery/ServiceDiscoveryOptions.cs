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

        public int Port { get; set; }
        public string Path { get; set; }
        public string UseScheme { get; set; } = "http";
    }
}
