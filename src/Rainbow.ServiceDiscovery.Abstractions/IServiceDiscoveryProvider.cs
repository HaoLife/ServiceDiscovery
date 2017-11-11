using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscoveryProvider
    {
        IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName);
        void Load(IServiceDiscovery serviceDiscovery);
        IChangeToken GetReloadToken();

    }
}
