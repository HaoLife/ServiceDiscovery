using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class ServiceDiscovery : IServiceDiscovery
    {
        public IEnumerable<IServiceDiscoveryProvider> Providers { get; }

        public ServiceDiscovery(IEnumerable<IServiceDiscoveryProvider> providers)
        {
            Providers = providers;
            this.Init();
        }

        private void Init()
        {
            foreach(var item in this.Providers)
            {
                item.Load();
            }
        }

        public IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName)
        {
            IEnumerable<IServiceEndpoint> result;
            foreach (var item in this.Providers.Reverse())
            {
                if (item.TryGetEndpoints(serviceName, out result))
                {
                    return result;
                }
            }

            return Enumerable.Empty<IServiceEndpoint>();
        }
    }
}
