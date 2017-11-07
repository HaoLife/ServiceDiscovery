using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public interface IServiceProxyGenerator
    {
        T CreateServiceProxy<T>(ServiceEndpoint endpoint);
    }
}
