using Microsoft.Extensions.Primitives;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscovery : IServiceDiscoveryRoot
    {
        private IList<IServiceDiscoveryProvider> _providers;
        private ServiceDiscoveryReloadToken _changeToken = new ServiceDiscoveryReloadToken();

        public ServiceDiscovery(IList<IServiceDiscoveryProvider> providers)
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _providers = providers;
            foreach (var p in providers)
            {
                p.Load();
                ChangeToken.OnChange(() => p.GetReloadToken(), () => RaiseChanged());
            }

        }

        public IEnumerable<IServiceDiscoveryProvider> Providers => _providers;

        public T GetProxy<T>()
        {
            throw new NotImplementedException();
        }

        public ServiceEndpoint GetService(string serviceName)
        {
            foreach (var provider in _providers.Reverse())
            {
                ServiceEndpoint value;

                if (provider.TryGet(serviceName, out value))
                {
                    return value;
                }
            }

            return null;
        }

        public void Reload()
        {
            foreach (var provider in _providers)
            {
                provider.Load();
            }
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            var previousToken = Interlocked.Exchange(ref _changeToken, new ServiceDiscoveryReloadToken());
            previousToken.OnReload();
        }
    }
}
