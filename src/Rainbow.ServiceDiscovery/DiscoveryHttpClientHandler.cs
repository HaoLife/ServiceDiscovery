using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Rainbow.ServiceDiscovery
{
    public class DiscoveryHttpClientHandler : DiscoveryHttpClientHandlerBase
    {
        private IServiceDiscovery _client;
        private ILogger<DiscoveryHttpClientHandler> _logger;

        public DiscoveryHttpClientHandler(IServiceDiscovery client, ILogger<DiscoveryHttpClientHandler> logger = null) : base()
        {
            if (client == null)
            {
                throw new ArgumentNullException(nameof(client));
            }

            _client = client;
            _logger = logger;
        }

        public override Uri LookupService(Uri current)
        {
            _logger?.LogDebug("LookupService({0})", current.ToString());
            if (!current.IsDefaultPort)
            {
                return current;
            }

            var instances = _client.GetEndpoints(current.Host);
            if (instances.Count() > 0)
            {
                int indx = _random.Next(instances.Count());
                current = new Uri(instances.ElementAt(indx).ToUri(), current.PathAndQuery);
            }
            _logger?.LogDebug("LookupService() returning {0} ", current.ToString());
            return current;

        }
    }
}
