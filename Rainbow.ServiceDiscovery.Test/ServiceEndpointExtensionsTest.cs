using System;
using Xunit;

namespace Rainbow.ServiceDiscovery.Test
{
    public class ServiceEndpointExtensionsTest
    {
        [Fact]
        public void ServiceEndpointAndUriEqual()
        {
            var sourceUri = new Uri("http://127.0.0.1:8080/api");
            var serviceEndpoint = new ServiceEndpoint("service", sourceUri);
            var uri = ServiceEndpointExtensions.ToUri(serviceEndpoint);
            Assert.Equal(uri, sourceUri);
        }
    }
}
