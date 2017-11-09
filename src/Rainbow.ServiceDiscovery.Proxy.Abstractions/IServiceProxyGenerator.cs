using Rainbow.ServiceDiscovery.Abstractions;
using System;

namespace Rainbow.ServiceDiscovery.Proxy.Abstractions
{
    public interface IServiceProxyGenerator
    {
        T CreateServiceProxy<T>(ServiceEndpoint endpoint);
    }
}
