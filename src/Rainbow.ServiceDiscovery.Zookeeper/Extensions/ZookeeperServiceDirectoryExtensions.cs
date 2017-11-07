using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public static class ZookeeperServiceDirectoryExtensions
    {
        private static string _nameService = "nameservice";

        public static string GetServiceDirectory(this string serviceName)
        {
            return string.Format("/{0}/{1}", _nameService, serviceName);
        }

        public static string GetServiceNameByPath(this string path)
        {
            var nodes = path.Split('/');
            if (nodes.Length != 3)
                return string.Empty;

            if (string.IsNullOrEmpty(nodes[2]))
                return string.Empty;

            return nodes[2];
        }
    }
}
