using Microsoft.Extensions.Primitives;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceSubscriber : IServiceSubscriber
    {
        private readonly IZookeeperRegistryClient _zkClient;
        private readonly IServiceEndpointStore _serviceEndpointStore;

        public ZookeeperServiceSubscriber(string serviceName, IZookeeperRegistryClient zkClient, IServiceEndpointStore serviceEndpointStore)
        {
            this.Name = serviceName;
            this._zkClient = zkClient;
            this._serviceEndpointStore = serviceEndpointStore;
        }
        public string Name { get; }


        public void Subscribe()
        {
            RaiseChanged();
            ChangeToken.OnChange(() => this._zkClient.GetReloadToken(this.Name), () => RaiseChanged());

        }

        public void RaiseChanged()
        {
            var endpoints = this._zkClient.GetChildren(this.Name);
            this._serviceEndpointStore.Set(this.Name, endpoints);
        }

        public IEnumerable<ServiceEndpoint> GetEndpoints()
        {
            return this._serviceEndpointStore.Get(this.Name);
        }
    }
}
