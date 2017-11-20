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
            return string.Join("/", new string[] { endpoint.Name.GetServiceDirectory(), Uri.EscapeDataString(endpoint.ToUri().ToString()) });
        }


    }
}
