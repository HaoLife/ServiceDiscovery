using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rainbow.ServiceDiscovery.Test
{

    public class ServiceEndpointTest
    {

        [Fact]
        public void ServiceEndpointEqual()
        {
            var serviceEndpoint = new ServiceEndpoint("service", "http://127.0.0.1:8080/api");
            Assert.Equal(serviceEndpoint.Protocol, "http");
            Assert.Equal(serviceEndpoint.HostName, "127.0.0.1");
            Assert.Equal(serviceEndpoint.Port, 8080);
            Assert.Equal(serviceEndpoint.Path, "/api");
            Assert.Equal(serviceEndpoint.Name, "service");
        }


        [Fact]
        public void ServiceEndpointUriEqual()
        {
            var serviceEndpoint = new ServiceEndpoint("service", new Uri("http://127.0.0.1:8080/api"));
            Assert.Equal(serviceEndpoint.Protocol, "http");
            Assert.Equal(serviceEndpoint.HostName, "127.0.0.1");
            Assert.Equal(serviceEndpoint.Port, 8080);
            Assert.Equal(serviceEndpoint.Path, "/api");
            Assert.Equal(serviceEndpoint.Name, "service");
        }


        [Fact]
        public void ServiceEndpointDefaultHostEqual()
        {
            var serviceEndpoint = new ServiceEndpoint("service", "http://127.0.0.1/api");
            Assert.Equal(serviceEndpoint.Protocol, "http");
            Assert.Equal(serviceEndpoint.HostName, "127.0.0.1");
            Assert.Equal(serviceEndpoint.Port, 80);
            Assert.Equal(serviceEndpoint.Path, "/api");
            Assert.Equal(serviceEndpoint.Name, "service");
        }

        [Fact]
        public void ServiceEndpointHttpsEqual()
        {
            var serviceEndpoint = new ServiceEndpoint("service", "https://127.0.0.1/api");
            Assert.Equal(serviceEndpoint.Protocol, "https");
            Assert.Equal(serviceEndpoint.HostName, "127.0.0.1");
            Assert.Equal(serviceEndpoint.Port, 443);
            Assert.Equal(serviceEndpoint.Path, "/api");
            Assert.Equal(serviceEndpoint.Name, "service");
        }

        [Fact]
        public void ServiceEndpointPathEqual()
        {
            var serviceEndpoint = new ServiceEndpoint("service", "https://127.0.0.1");
            Assert.Equal(serviceEndpoint.Protocol, "https");
            Assert.Equal(serviceEndpoint.HostName, "127.0.0.1");
            Assert.Equal(serviceEndpoint.Port, 443);
            Assert.Equal(serviceEndpoint.Path, "/");
            Assert.Equal(serviceEndpoint.Name, "service");
        }
    }
}
