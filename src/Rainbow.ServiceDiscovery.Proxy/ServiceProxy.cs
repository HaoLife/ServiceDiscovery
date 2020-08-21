using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public class ServiceProxy : IServiceProxy
    {
        private readonly IEnumerable<IServiceProxyProvider> providers;
        private readonly IEnumerable<ProxyDescriptor> descriptors;

        public ServiceProxy(IEnumerable<IServiceProxyProvider> providers, IEnumerable<IProxyDescriptorSource> proxyDescriptorSources)
        {
            this.providers = providers;
            this.descriptors = proxyDescriptorSources.SelectMany(a => a.Build()).ToList();
        }


        public T Create<T>()
        {
            var desc = this.descriptors.FirstOrDefault(a => a.ProxyType == typeof(T));
            if (desc == null)
            {
                throw new NotFoundProxyException($"not found type {typeof(T).FullName} ");
            }

            var provider = this.providers.FirstOrDefault(a => a.CanHandle(desc.FactoryType));
            if (provider == null)
            {
                throw new NotFoundProxyException($"not found provider {typeof(T).FullName} ");
            }

            return provider.Create<T>(desc);
        }
    }
}
