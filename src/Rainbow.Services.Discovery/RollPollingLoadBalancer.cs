using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class RollPollingLoadBalancer : ILoadBalancer
    {
        private ConcurrentDictionary<string, Sequence> _catch = new ConcurrentDictionary<string, Sequence>();

        public IServiceEndpoint Get(IServiceDiscovery discovery, string serviceName)
        {
            var endPoint = this.TryGet(discovery, serviceName);
            if (endPoint == null) throw new DiscoveryException($"no {serviceName} service was found.");
            return endPoint;
        }

        public IServiceEndpoint TryGet(IServiceDiscovery discovery, string serviceName)
        {
            var endpoints = discovery.GetEndpoints(serviceName);
            if (!endpoints.Any()) return null;

            var seq = _catch.GetOrAdd(serviceName, new Sequence());
            var value = seq.Next();
            int index = (int)(value & endpoints.Count());
            return endpoints.ElementAt(index);
        }
    }
}
