using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class ServiceDiscoveryBuilder : IServiceDiscoveryBuilder
    {
        public IList<IServiceDiscoverySource> Sources { get; } = new List<IServiceDiscoverySource>();

        public ServiceDiscoveryBuilder Add(IServiceDiscoverySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }

        public IServiceDiscovery Build(IServiceProvider services)
        {
            var providers = new List<IServiceDiscoveryProvider>();
            foreach (var source in Sources)
            {
                var provider = source.Build(services);
                providers.Add(provider);
            }
            return new ServiceDiscovery(providers);
        }
    }
}
