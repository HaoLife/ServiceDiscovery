using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public class ServiceProxy : IServiceProxy
    {
        private readonly ServiceProxyBuilder builder;

        public IEnumerable<IServiceProxyProvider> Providers { get; }
        private IList<ServiceProxyDescriptor> descriptors => builder.Descriptors;

        public ServiceProxy(ServiceProxyBuilder builder, IEnumerable<IServiceProxyProvider> providers)
        {
            this.builder = builder;
            Providers = providers;
        }

        public T Create<T>()
        {
            var desc = this.descriptors.FirstOrDefault(a => a.ProxyType == typeof(T));
            if (desc == null)
            {
                throw new NotFoundProxyException($"not found type {typeof(T).FullName} ");
            }

            var provider = this.Providers.FirstOrDefault(a => a.CanHandle(desc.ProviderName));
            if (provider == null)
            {
                throw new NotFoundProxyException($"not found provider {typeof(T).FullName} ");
            }

            return provider.Create<T>(desc);
        }
    }
}
