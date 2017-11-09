using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Linq;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private readonly ZookeeperServiceDiscoverySource _source;
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();
        public IZookeeperRegistryClient Client { get; }
        public List<IServiceRegister> Registers { get; }
        public List<IServiceSubscriber> Subscribes { get; }


        public ZookeeperServiceDiscoveryProvider(ZookeeperRegistryClient client, ZookeeperServiceDiscoverySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            this._source = source;
            this.Client = client;
            this.Registers = new List<IServiceRegister>();
            this.Subscribes = new List<IServiceSubscriber>();
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        public void Load()
        {
            this.Registers.Clear();
            this.Subscribes.Clear();

            foreach (var item in this._source.Registers)
            {
                var register = new ZookeeperServiceRegister(this.Client, item);
                register.Register();
                this.Registers.Add(register);
            }

            foreach (var item in this._source.Subscribes)
            {
                var subscriber = new ZookeeperServiceSubscriber(item, this.Client, _source.Store);
                subscriber.Subscribe();
                this.Subscribes.Add(subscriber);
            }
        }

        public bool TryGet(string key, out ServiceEndpoint value)
        {
            IServiceSubscriber subscriber = this.Subscribes.FirstOrDefault(a => a.Name == key);
            if (subscriber == null)
            {
                value = null;
                return false;
            }
            return _source.LoadBalancing.TryGet(subscriber, out value);
        }

    }
}
