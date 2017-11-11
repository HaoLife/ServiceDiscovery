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


        public static string[] ToPathNodes(this ServiceEndpoint endpoint)
        {
            var nodes = endpoint.Name.GetServiceDirectory().Split('/').Skip(1).ToArray();
            var paths = new string[nodes.Length];
            for (int i = 0; i < nodes.Length; i++)
            {
                paths[i] = "/" + string.Join("/", nodes.Take(i + 1));
            }

            return paths;
        }
    }
}
