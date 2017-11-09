using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscoveryBuilder : IServiceDiscoveryBuilder
    {
        public ServiceDiscoveryBuilder()
        {

        }

        public IList<IServiceDiscoverySource> Sources { get; } = new List<IServiceDiscoverySource>();

        public IServiceDiscoveryBuilder Add(IServiceDiscoverySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }

        public IServiceDiscovery Build()
        {
            var providers = new List<IServiceDiscoveryProvider>();
            foreach (var source in Sources)
            {
                var provider = source.Build(this);
                providers.Add(provider);
            }
            return new ServiceDiscovery(providers);
        }

    }
}
