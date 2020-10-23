using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscovery
    {
        IEnumerable<IServiceDiscoveryProvider> Providers { get; }
        IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName);

    }
}
