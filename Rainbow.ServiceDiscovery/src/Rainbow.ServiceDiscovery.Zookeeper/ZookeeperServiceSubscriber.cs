using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceSubscriber : IServiceSubscriber
    {
        private readonly SubscribeDescribe _subscribeDescribe;
        private readonly IZookeeperRegistryClient _zkClient;
        private readonly IServiceEndpointStore _serviceEndpointStore;

        public ZookeeperServiceSubscriber(IZookeeperRegistryClient zkClient, IServiceEndpointStore serviceEndpointStore, SubscribeDescribe subscribeDescribe)
        {
            this._zkClient = zkClient;
            this._subscribeDescribe = subscribeDescribe;
            this._serviceEndpointStore = serviceEndpointStore;
        }
        public string Name
        {
            get { return _subscribeDescribe.ServiceName; }
        }

        private void ChangeEndpoint(IEnumerable<ServiceEndpoint> endpoints)
        {
            this._serviceEndpointStore.Set(this.Name, endpoints);

        }

        public void Subscribe()
        {
            var values = _zkClient.Subscribe(new ZookeeperSubscribeNotice(_subscribeDescribe, ChangeEndpoint));
            ChangeEndpoint(values);

        }

        public IEnumerable<ServiceEndpoint> GetEndpoints()
        {
            return this._serviceEndpointStore.Get(this.Name);
        }
    }
}
