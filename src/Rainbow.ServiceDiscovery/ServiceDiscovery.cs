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
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();

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

        public bool TryGet(string serviceName, out ServiceEndpoint value)
        {
            value = null;
            foreach (var provider in _providers.Reverse())
            {
                if (provider.TryGet(serviceName, out value))
                {
                    return true;
                }
            }
            return false;
        }
        public ServiceEndpoint GetService(string serviceName)
        {
            ServiceEndpoint result;
            if (!this.TryGet(serviceName, out result))
            {
                throw new Exception("没有找到" + serviceName + "服务");
            }
            return result;
        }


        private void RaiseChanged()
        {
            var previousToken = Interlocked.Exchange(ref _reloadToken, new ServiceDiscoveryReloadToken());
            previousToken.OnReload();
        }

        public void Reload()
        {
            foreach (var provider in _providers)
            {
                provider.Load();
            }
            RaiseChanged();
        }
    }
}
