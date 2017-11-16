using System;
using Xunit;

namespace Rainbow.ServiceDiscovery.Test
{
    public class ServiceEndpointExtensionsTest
    {
        [Fact]
        public void ServiceEndpointAndUriEqual()
        {
            var serviceEndpoint = new ServiceEndpoint("service", "http://127.0.0.1:8080/api");
            var uri = ServiceEndpointExtensions.ToUri(serviceEndpoint);
            Assert.Equal(uri.Scheme, serviceEndpoint.Protocol);
            Assert.Equal(uri.Host, serviceEndpoint.HostName);
            Assert.Equal(uri.Port, serviceEndpoint.Port);
            Assert.Equal(uri.AbsolutePath, serviceEndpoint.Path);
        }
    }
}
