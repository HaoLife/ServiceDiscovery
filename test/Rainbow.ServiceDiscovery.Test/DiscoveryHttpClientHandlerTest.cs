using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Rainbow.ServiceDiscovery.Test
{
    public class DiscoveryHttpClientHandlerTest
    {

        class TestDiscoveryClient : IServiceDiscovery
        {
            private readonly IServiceEndpoint _instance;

            public TestDiscoveryClient(IServiceEndpoint serviceEndpoint = null)
            {
                this._instance = serviceEndpoint;
            }

            public IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName)
            {
                if (_instance != null)
                {
                    return new List<IServiceEndpoint>() { _instance };
                }
                return new List<IServiceEndpoint>();
            }

            public IServiceEndpoint GetLocalEndpoint()
            {
                throw new NotImplementedException();
            }
        }


        [Fact]
        public void Constructor_ThrowsIfClientNull()
        {
            // Arrange
            IServiceDiscovery client = null;

            // Act and Assert
            var ex = Assert.Throws<ArgumentNullException>(() => new DiscoveryHttpClientHandler(client));
            Assert.Contains(nameof(client), ex.Message);
        }

        [Fact]
        public void LookupService_NonDefaultPort_ReturnsOriginalURI()
        {
            // Arrange
            IServiceDiscovery client = new TestDiscoveryClient();
            DiscoveryHttpClientHandler handler = new DiscoveryHttpClientHandler(client);
            Uri uri = new Uri("http://foo:8080/test");
            // Act and Assert
            var result = handler.LookupService(uri);
            Assert.Equal(uri, result);
        }

        [Fact]
        public void LookupService_DoesntFindService_ReturnsOriginalURI()
        {
            // Arrange
            IServiceDiscovery client = new TestDiscoveryClient();
            DiscoveryHttpClientHandler handler = new DiscoveryHttpClientHandler(client);
            Uri uri = new Uri("http://foo/test");

            // Act and Assert
            var result = handler.LookupService(uri);
            Assert.Equal(uri, result);
        }

        [Fact]
        public void LookupService_FindsService_ReturnsURI()
        {
            // Arrange
            IServiceDiscovery client = new TestDiscoveryClient(new ServiceEndpoint("service", new Uri("http://127.0.0.1:5555")));
            DiscoveryHttpClientHandler handler = new DiscoveryHttpClientHandler(client);
            Uri uri = new Uri("http://service/test/bar/foo?test=1&test2=2");
            // Act and Assert
            var result = handler.LookupService(uri);
            Assert.Equal(new Uri("http://127.0.0.1:5555/test/bar/foo?test=1&test2=2"), result);
        }

    }

}
