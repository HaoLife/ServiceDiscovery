using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscovery : IServiceDiscovery
    {
        public ServiceEndpoint GetService(string serviceName)
        {
            throw new NotImplementedException();
        }

        public T GetProxy<T>()
        {
            throw new NotImplementedException();
        }
    }
}
