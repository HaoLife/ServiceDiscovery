using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class PollingServiceLoadBalancing : IServiceLoadBalancing
    {
        private ConcurrentDictionary<string, Sequence> _cache = new ConcurrentDictionary<string, Sequence>();

        public bool TryGet(IServiceSubscriber subscriber, out ServiceEndpoint endpoint)
        {
            endpoint = null;
            var points = subscriber.GetEndpoints();
            if (!points.Any())
            {
                return false;
            }

            var seq = _cache.GetOrAdd(subscriber.Name, new Sequence());
            var index = (int)(seq.Value % points.Count());
            seq.Value++;
            endpoint = points.ElementAt(index);

            return true;
        }

        private class Sequence
        {
            public long Value;
        }
    }
}
