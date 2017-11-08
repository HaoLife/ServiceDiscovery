using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoverySource : IServiceDiscoverySource
    {
        public ZookeeperServiceDiscoveryOptions Options { get; }
        public IZookeeperRegistryClient Client { get; }
        public IServiceEndpointStore Store { get; }
        public ZookeeperSubscriberDirectory SubscriberDirectory { get; }
        public IRegisterDirectory RegisterDirectory { get; }
        public IProxyMapper ProxyMapper { get; }
        public IServiceProxyGenerator ProxyGenerator { get; }
        public IServiceLoadBalancing LoadBalancing { get; set; }

        public ZookeeperServiceDiscoverySource(ZookeeperServiceDiscoveryOptions options)
        {
            this.Options = options;
            this.Client = new ZookeeperRegistryClient(this);
            this.Store = new MemoryServiceEndpointStore();
            this.SubscriberDirectory = new ZookeeperSubscriberDirectory(this);
            this.RegisterDirectory = new ZookeeperRegisterDirectory(this);
            this.ProxyMapper = new ProxyMapper();
            this.ProxyGenerator = new HttpDynamicServiceProxyGenerator();
            this.LoadBalancing = new PollingServiceLoadBalancing(this.SubscriberDirectory);
        }


        public IServiceDiscoveryProvider Build(IServiceDiscoveryBuilder builder)
        {
            return new ZookeeperServiceDiscoveryProvider(this);
        }
    }
}
