using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscovery
    {
        ServiceEndpoint GetService(string serviceName);

        bool TryGet(string serviceName, out ServiceEndpoint value);

    }
}
