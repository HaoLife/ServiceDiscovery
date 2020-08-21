using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public interface IServiceProxySource
    {
        IServiceProxyProvider Build(IServiceProvider provider);
    }
}
