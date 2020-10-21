using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class ConfigServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private readonly ConfigServiceDiscoverySource source;

        private SortedDictionary<string, IEnumerable<IServiceEndpoint>> _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();

        public ConfigServiceDiscoveryProvider(
            ConfigServiceDiscoverySource source)
        {
            this.source = source;

            if (source.ReloadOnChange)
            {
                ChangeToken.OnChange(() => source.Configuration.GetReloadToken(), () => this.Load());
            }
        }


        public void Load()
        {
            var options = this.source.Configuration.Get<ConfigServiceDiscoveryOptions>();

            SortedDictionary<string, IEnumerable<IServiceEndpoint>> cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();
            foreach (var item in options.Services)
            {
                var serviceEndpoint = new ServiceEndpoint(item.Key, item.Value);
                cache.Add(serviceEndpoint.Name, new List<IServiceEndpoint>() { serviceEndpoint });

            }
            _cache = cache;
        }

        public bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints)
        {
            return _cache.TryGetValue(serviceName, out endpoints);
        }
    }
}
