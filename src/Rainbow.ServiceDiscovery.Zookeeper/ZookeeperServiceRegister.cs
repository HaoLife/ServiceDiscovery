using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceRegister : IServiceRegister
    {
        private readonly ServiceEndpoint _serviceEndpoint;
        private readonly IZookeeperRegistryClient _zkClient;

        public ZookeeperServiceRegister(IZookeeperRegistryClient zkClient, ServiceEndpoint serviceEndpoint)
        {
            this._serviceEndpoint = serviceEndpoint;
            this._zkClient = zkClient;
        }
        public string ServiceName
        {
            get { return _serviceEndpoint.Name; }
        }

        public void Register()
        {
            this._zkClient.Publish(_serviceEndpoint);
        }

        public void Deregister()
        {
            this._zkClient.Unpublish(_serviceEndpoint);
        }
    }
}
