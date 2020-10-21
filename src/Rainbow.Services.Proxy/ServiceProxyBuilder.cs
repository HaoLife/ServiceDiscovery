using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public class ServiceProxyBuilder : IServiceProxyBuilder
    {
        public IList<IServiceProxySource> Sources { get; } = new List<IServiceProxySource>();

        public IList<ServiceProxyDescriptor> Descriptors { get; } = new List<ServiceProxyDescriptor>();

        public IList<ILoadBalancer> LoadBalancers { get; } = new List<ILoadBalancer>();

        public ServiceProxyBuilder Add(IServiceProxySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }

        public ServiceProxyBuilder AddProxy(ServiceProxyDescriptor descriptor)
        {
            if (descriptor == null)
            {
                throw new ArgumentNullException(nameof(descriptor));
            }

            Descriptors.Add(descriptor);
            return this;
        }

        public ServiceProxyBuilder AddLoadBalancer(ILoadBalancer loadBalancer)
        {
            if (loadBalancer == null)
            {
                throw new ArgumentNullException(nameof(loadBalancer));
            }

            LoadBalancers.Add(loadBalancer);
            return this;
        }

        public IServiceProxy Build(IServiceProvider services)
        {
            var providers = new List<IServiceProxyProvider>();
            foreach (var source in Sources)
            {
                var provider = source.Build(this, services);
                providers.Add(provider);
            }
            return new ServiceProxy(this, providers);
        }
    }
}
