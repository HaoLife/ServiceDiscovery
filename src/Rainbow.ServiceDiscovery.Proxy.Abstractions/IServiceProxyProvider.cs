using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public interface IServiceProxyProvider
    {
        bool CanHandle(string type);

        T Create<T>(ProxyDescriptor descriptor);
    }
}
