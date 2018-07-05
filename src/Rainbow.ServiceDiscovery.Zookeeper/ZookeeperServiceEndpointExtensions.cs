using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public static class ZookeeperServiceEndpointExtensions
    {
        public static string ToPath(this IServiceEndpoint endpoint)
        {
            return string.Join("/", new string[] { endpoint.ToDirectory(), Uri.EscapeDataString(endpoint.ToUri().ToString()) });
        }
        
        public static string ToDirectory(this IServiceEndpoint endpoint)
        {
            return $"/{ZookeeperDefaults.NameService}/{endpoint.Name}";
        }

    }
}
