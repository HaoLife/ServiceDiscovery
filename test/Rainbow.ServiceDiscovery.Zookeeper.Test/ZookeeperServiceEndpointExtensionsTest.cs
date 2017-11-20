using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rainbow.ServiceDiscovery.Zookeeper.Test
{
    public class ZookeeperServiceEndpointExtensionsTest
    {
        [Fact]
        public void DirectoryExtensionsPathEqueal()
        {
            var uri = new Uri("http://127.0.0.1/api");
            var endpoint = new ServiceEndpoint("service", uri);
            var path = ZookeeperServiceEndpointExtensions.ToPath(endpoint);
            var source = $"/{ZookeeperDefaults.NameService}/{endpoint.Name}/{Uri.EscapeDataString(uri.ToString())}";
            Assert.Equal(path, source);
        }
    }
}
