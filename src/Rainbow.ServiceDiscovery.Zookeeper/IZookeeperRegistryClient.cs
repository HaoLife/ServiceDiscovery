using Microsoft.Extensions.Primitives;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public interface IZookeeperRegistryClient
    {
        void Publish(ServiceEndpoint endpoint);
        void Unpublish(ServiceEndpoint endpoint);

        IChangeToken GetReloadToken(string serviceName);
        IEnumerable<ServiceEndpoint> GetChildren(string serviceName);
    }
}
