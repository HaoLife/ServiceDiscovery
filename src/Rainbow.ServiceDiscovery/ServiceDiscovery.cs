using Microsoft.Extensions.Options;
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
        private ServiceDiscoveryOptions _options;
        private IDisposable _changeTokenRegistration;

        public ServiceDiscovery(
            IOptionsChangeTokenSource<ServiceDiscoveryOptions> token
            , IOptionsMonitor<ServiceDiscoveryOptions> options
            , IEnumerable<IServiceDiscoveryProvider> providers
            )
        {
            if (providers == null)
            {
                throw new ArgumentNullException(nameof(providers));
            }

            _changeTokenRegistration = options.OnChange(RefreshOptions);
            RefreshOptions(options.CurrentValue);

            _providers = providers.ToList();
            foreach (var p in providers)
            {
                p.Load(this);
                ChangeToken.OnChange(() => p.GetReloadToken(), () => RaiseChanged());
            }

        }

        public IEnumerable<IServiceDiscoveryProvider> Providers => _providers;

        private void RefreshOptions(ServiceDiscoveryOptions options)
        {
            this._options = options;
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
                provider.Load(this);
            }
            RaiseChanged();
        }


        public IServiceEndpoint GetLocalEndpoint()
        {
            return new ServiceEndpoint(this._options.Name, this._options.Url);
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
