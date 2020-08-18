using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscovery
    {
        IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName);

    }
}
