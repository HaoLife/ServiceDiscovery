using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceSubscriberFactory : IServiceSubscriberFactory
    {
        private readonly IZookeeperRegistryClient _zkClient;
        private readonly IServiceEndpointStore _serviceEndpointStore;
        public ZookeeperServiceSubscriberFactory(IZookeeperRegistryClient zkClient, IServiceEndpointStore serviceEndpointStore)
        {
            this._zkClient = zkClient;
            this._serviceEndpointStore = serviceEndpointStore;
        }

        public IServiceSubscriber CreateSubscriber(SubscribeDescribe subscribeDescribe)
        {
            return new ZookeeperServiceSubscriber(_zkClient, _serviceEndpointStore, subscribeDescribe);
        }
    }
}
