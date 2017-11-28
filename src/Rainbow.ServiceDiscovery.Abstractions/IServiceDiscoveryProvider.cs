using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscoveryProvider
    {
        bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints);
        void Load(IServiceDiscovery serviceDiscovery);
        IChangeToken GetReloadToken();

    }
}
