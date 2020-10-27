using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyModel;
using Rainbow.Services.Proxy;
using Rainbow.Services.Proxy.Attributes;


namespace Microsoft.Extensions.DependencyInjection
{
    public static class AutoAttributeExtensions
    {
        public static ServiceProxyBuilder AddAutoProxy(this ServiceProxyBuilder builder)
        {

            var dependencyContext = DependencyContext.Default;

            List<Assembly> assemblys = DependencyContext.Default.RuntimeLibraries
                .SelectMany(p => p.GetDefaultAssemblyNames(dependencyContext))
                .Select(Assembly.Load)
                .ToList();

            var proxys = assemblys
                .SelectMany(a => a.GetTypes())
                .Where(a => a.GetCustomAttributes<ProxyAttribute>().Any())
                .ToList();

            var descriptors = proxys
                .Select(a => new { type = a, attr = a.GetCustomAttribute<ProxyAttribute>() })
                .Select(a => new ServiceProxyDescriptor()
                {
                    ServiceName = a.attr.Service,
                    LoadBalancer = a.attr.LoadBalancer,
                    ProviderName = a.attr.ProviderName,
                    ProxyType = a.type,
                })
                .ToList();

            descriptors.ForEach(a => builder.AddProxy(a));


            return builder;
        }
    }
}