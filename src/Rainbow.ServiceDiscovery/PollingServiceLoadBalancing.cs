using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class PollingServiceLoadBalancing : IServiceLoadBalancing
    {
        private readonly ISubscriberDirectory _subscriberDirectory;
        private ConcurrentDictionary<string, Sequence> _cache = new ConcurrentDictionary<string, Sequence>();

        public PollingServiceLoadBalancing(ISubscriberDirectory subscriberDirectory)
        {
            this._subscriberDirectory = subscriberDirectory;
        }

        public ServiceEndpoint Find(string serviceName)
        {
            var subscriber = this._subscriberDirectory.GetSubscribers().FirstOrDefault(a => a.Name == serviceName);
            if (subscriber == null)
            {
                throw new Exception("没有该服务的订阅" + serviceName);
            }

            var points = subscriber.GetEndpoints();

            if (!points.Any())
            {
                throw new Exception("没有可用的服务" + serviceName);
            }

            var seq = _cache.GetOrAdd(serviceName, new Sequence());
            var index = (int)(seq.Value % points.Count());
            seq.Value++;
            return points.ElementAt(index);
        }


        private class Sequence
        {
            public long Value;
        }
    }
}
