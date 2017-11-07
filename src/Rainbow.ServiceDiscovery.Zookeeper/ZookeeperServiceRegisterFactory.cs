using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceRegisterFactory : IServiceRegisterFactory
    {
        private readonly IZookeeperRegistryClient _zkClient;
        public ZookeeperServiceRegisterFactory(IZookeeperRegistryClient zkClient)
        {
            this._zkClient = zkClient;
        }

        public IServiceRegister CreateRegister(ServiceEndpoint endpoint)
        {
            return new ZookeeperServiceRegister(_zkClient, endpoint);
        }
    }
}
