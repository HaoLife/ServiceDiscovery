using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class MemoryServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
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
            this._logger = loggerFactory.CreateLogger<MemoryServiceDiscoveryProvider>();
        }

        public IChangeToken GetReloadToken()
        {
            return _source.Configuration.GetReloadToken();
        }

        public void Load()
        {
            this._options = new MemoryServiceDiscoveryOptions(_source.Configuration);

            this._logger.LogInformation($"load {nameof(MemoryServiceDiscoveryProvider)}");
            SortedDictionary<string, IEnumerable<IServiceEndpoint>> cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();
            foreach (var item in _options.Services)
            {
                var serviceEndpoint = new ServiceEndpoint(item.Key, item.Value);
                cache.Add(serviceEndpoint.Name, new List<IServiceEndpoint>() { serviceEndpoint });

                this._logger.LogInformation($"add {item.Key} {item.Value}");
            }
            _cache = cache;
        }

        public bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints)
        {
            return _cache.TryGetValue(serviceName, out endpoints);
        }
    }
}
