using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using Consul;

namespace Rainbow.ServiceDiscovery.Consul
{
    public class ConsulServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private readonly IConsulClient _client;

        public IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName)
        {
            _client = new ConsulClient();
            throw new NotImplementedException();
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public void Load(IServiceDiscovery serviceDiscovery)
        {
            throw new NotImplementedException();
        }
    }
}
