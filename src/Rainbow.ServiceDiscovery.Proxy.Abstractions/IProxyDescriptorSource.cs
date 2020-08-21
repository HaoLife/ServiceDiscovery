using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public interface IProxyDescriptorSource
    {
        IEnumerable<ProxyDescriptor> Build();

        void AddService(Type type, string service);
    }
}
