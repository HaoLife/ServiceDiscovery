using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    internal static class ZookeeperServiceNodeExtensions
    {

        public static string GetServiceDirectory(this string serviceName)
        {
            return $"/{ZookeeperDefaults.NameService}/{serviceName}";
        }

        public static string GetServiceNameByPath(this string path)
        {
            var nodes = path.Split('/');
            if (nodes.Length >= 3)
                return string.Empty;

            if (string.IsNullOrEmpty(nodes[2]))
                return string.Empty;

            return nodes[2];
        }

    }
}
