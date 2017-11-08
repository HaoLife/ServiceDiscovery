using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperSubscriberDirectory : ISubscriberDirectory
    {
        private readonly ZookeeperServiceDiscoverySource _source;
        private readonly List<IServiceSubscriber> _serviceSubscriber;

        public ZookeeperSubscriberDirectory(ZookeeperServiceDiscoverySource source)
        {
            this._source = source;
            this._serviceSubscriber = new List<IServiceSubscriber>();
        }

        public void Add(string serviceName)
        {
            if (this._serviceSubscriber.Any(a => serviceName == a.Name))
            {
                throw new Exception("重复添加订阅项");
            }
            var subscriber = new ZookeeperServiceSubscriber(serviceName, this._source.Client, this._source.Store);

            this._serviceSubscriber.Add(subscriber);
        }

        public IEnumerable<IServiceSubscriber> GetSubscribers()
        {
            return this._serviceSubscriber;
        }
    }
}
