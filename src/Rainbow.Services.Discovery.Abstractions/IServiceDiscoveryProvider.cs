using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscoveryProvider
    {
        bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints);
        void Load();
        IChangeToken GetReloadToken();

    }
}
