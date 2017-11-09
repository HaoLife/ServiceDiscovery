using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoverySource : IServiceDiscoverySource
    {
        public IServiceEndpointStore Store { get; set; }
        public IServiceLoadBalancing LoadBalancing { get; set; }

        public string Connection { get; set; }
        public TimeSpan SessionTimeout { get; set; }
        public List<string> Subscribes { get; set; }
        public List<ServiceEndpoint> Registers { get; set; }

        public ZookeeperServiceDiscoverySource()
        {
            this.LoadBalancing = new PollingServiceLoadBalancing();
            this.Store = new MemoryServiceEndpointStore();
        }

        public IServiceDiscoveryProvider Build(IServiceDiscoveryBuilder builder)
        {
            return new ZookeeperServiceDiscoveryProvider(new ZookeeperRegistryClient(this), this);
        }
    }
}
