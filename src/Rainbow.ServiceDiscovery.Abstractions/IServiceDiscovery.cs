using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscovery
    {
        IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName);
        IServiceEndpoint GetLocalEndpoint();

    }
}
