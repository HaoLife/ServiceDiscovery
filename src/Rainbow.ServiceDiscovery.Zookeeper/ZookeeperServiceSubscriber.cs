using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceSubscriber : IServiceSubscriber
    {
        private readonly string _serviceName;
        private readonly IZookeeperRegistryClient _zkClient;
        private readonly IServiceEndpointStore _serviceEndpointStore;

        public ZookeeperServiceSubscriber(string serviceName, IZookeeperRegistryClient zkClient, IServiceEndpointStore serviceEndpointStore)
        {
            this._serviceName = serviceName;
            this._zkClient = zkClient;
            this._serviceEndpointStore = serviceEndpointStore;
        }
        public string Name => this._serviceName;

        private void ChangeEndpoint(IEnumerable<ServiceEndpoint> endpoints)
        {
            this._serviceEndpointStore.Set(this.Name, endpoints);

        }

        public void Subscribe()
        {
            var values = _zkClient.Subscribe(new ZookeeperSubscribeNotice(this.Name, ChangeEndpoint));
            ChangeEndpoint(values);

        }

        public IEnumerable<ServiceEndpoint> GetEndpoints()
        {
            return this._serviceEndpointStore.Get(this.Name);
        }
    }
}
