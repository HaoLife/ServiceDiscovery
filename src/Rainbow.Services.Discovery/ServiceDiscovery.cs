using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rainbow.Services.Discovery
{
    public class ServiceDiscovery : IServiceDiscoveryRoot
    {
        private IList<IServiceDiscoveryProvider> _providers;

        public ServiceDiscovery(IEnumerable<IServiceDiscoveryProvider> providers
            )
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }
            _providers = providers.ToList();
            foreach (var p in providers)
            {
                p.Load();
                ChangeToken.OnChange(() => p.GetReloadToken(), () => p.Load());
            }

        }

        public IEnumerable<IServiceDiscoveryProvider> Providers => _providers;

        public void Reload()
        {
            foreach (var provider in _providers)
            {
                provider.Load();
            }
        }


        public IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName)
        {
            IEnumerable<IServiceEndpoint> result;
            foreach (var item in this._providers.Reverse())
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
