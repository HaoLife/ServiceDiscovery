using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public interface IZookeeperRegistryClient
    {
        IEnumerable<ServiceEndpoint> Subscribe(ZookeeperSubscribeNotice subscribeNotice);
        void Publish(ServiceEndpoint endpoint);
        void Unpublish(ServiceEndpoint endpoint);
    }
}
