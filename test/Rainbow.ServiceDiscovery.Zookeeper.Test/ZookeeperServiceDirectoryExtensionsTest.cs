using System;
using Xunit;

namespace Rainbow.ServiceDiscovery.Zookeeper.Test
{
    public class ZookeeperServiceDirectoryExtensionsTest
    {
        [Fact]
        public void GetServiceDirectoryEqual()
        {
            var dir = ZookeeperServiceNodeExtensions.GetServiceDirectory("service");
            var source = $"/{ZookeeperDefaults.NameService}/service";
            Assert.Equal(dir, source);
        }

        [Fact]
        public void GetServiceNameByPathEqual()
        {
            var path = $"/{ZookeeperDefaults.NameService}/service";
            var service = ZookeeperServiceNodeExtensions.GetServiceNameByPath(path);

            Assert.Equal(service, "service");
        }
    }
}
