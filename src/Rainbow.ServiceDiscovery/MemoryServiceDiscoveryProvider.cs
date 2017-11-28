using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace Rainbow.ServiceDiscovery
{
    public class MemoryServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();
        private MemoryServiceDiscoverySource _source;
        private MemoryServiceDiscoveryOptions _options;
        private ILogger _logger;
        private SortedDictionary<string, IEnumerable<IServiceEndpoint>> _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();

        public MemoryServiceDiscoveryProvider(
            MemoryServiceDiscoverySource source,
            ILoggerFactory loggerFactory)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            this._source = source;
            this._options = new MemoryServiceDiscoveryOptions(source.Configuration);
            this._logger = loggerFactory.CreateLogger<MemoryServiceDiscoveryProvider>();

            ChangeToken.OnChange(source.Configuration.GetReloadToken, RaiseChanged);
        }
        private void RaiseChanged()
        {
            this._options = new MemoryServiceDiscoveryOptions(_source.Configuration);
            Initialize();
        }


        protected virtual void Initialize()
        {
            SortedDictionary<string, IEnumerable<IServiceEndpoint>> cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();
            foreach (var item in _options.Services)
            {
                var serviceEndpoint = new ServiceEndpoint(item.Key, item.Value);
                cache.Add(serviceEndpoint.Name, new List<IServiceEndpoint>() { serviceEndpoint });
            }
            _cache = cache;
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        public void Load(IServiceDiscovery serviceDiscovery)
        {
            Initialize();
        }

        public bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints)
        {
            return _cache.TryGetValue(serviceName, out endpoints);
        }
    }
}
