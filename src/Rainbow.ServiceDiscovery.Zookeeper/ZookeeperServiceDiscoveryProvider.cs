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

        public ZookeeperServiceDiscoveryProvider(ZookeeperServiceDiscoverySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            this._source = source;
        }

        public T GetProxy<T>()
        {

            var mapping = this._source.ProxyMapper.GetMappings().Where(a => a.MapType == typeof(T)).FirstOrDefault();
            if (mapping == null) throw new Exception("没有可映射的服务" + typeof(T).Name);
            ServiceEndpoint endpoint;
            if(!this.TryGet(mapping.ServiceName, out endpoint))
            {
                throw new Exception("没有找到服务");
            }
            //创建代理服务
            return this._source.ProxyGenerator.CreateServiceProxy<T>(endpoint);
        }

        public IChangeToken GetReloadToken()
        {
            throw new NotImplementedException();
        }

        public void Load()
        {
            foreach (var item in this._source.RegisterDirectory.GetRegisters())
            {
                item.Register();
            }

            foreach (var item in this._source.SubscriberDirectory.GetSubscribers())
            {
                item.Subscribe();
            }

        }

        public bool TryGet(string key, out ServiceEndpoint value)
        {
            try
            {
                value = this._source.LoadBalancing.Find(key);
                return true;
            }
            catch (Exception ex)
            {
                value = null;
                return false;
            }
        }
    }
}
