using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceProxyGenerator
    {
        T CreateServiceProxy<T>(ServiceEndpoint endpoint);
    }
}
