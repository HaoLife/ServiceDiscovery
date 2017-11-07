using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public interface IServiceEndpointStore
    {
        void Set(string serviceName, IEnumerable<ServiceEndpoint> endpoints);

        IEnumerable<ServiceEndpoint> Get(string serviceName);

        void Delete(string serviceName);
    }
}
