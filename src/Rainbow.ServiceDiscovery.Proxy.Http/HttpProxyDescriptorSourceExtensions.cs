using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using Rainbow.ServiceDiscovery.Proxy;
using Rainbow.ServiceDiscovery.Proxy.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class HttpProxyDescriptorSourceExtensions
    {

        public static IProxyDescriptorSource AddHttpProxy(this IServiceCollection services)
        {
            var source = new HttpProxyDescriptorSource();
            services.AddSingleton<IProxyDescriptorSource>(source);
            return source;
        }

        public static IProxyDescriptorSource AddHttpProxy(this IServiceCollection services, IConfiguration configuration)
        {
            var dependencyContext = DependencyContext.Load(Assembly.GetEntryAssembly());
            List<Assembly> assemblies = dependencyContext.RuntimeLibraries
                .SelectMany(p => p.GetDefaultAssemblyNames(dependencyContext))
                .Select(Assembly.Load)
                .ToList();
            var source = new HttpProxyDescriptorSource(configuration, assemblies);
            services.AddSingleton<IProxyDescriptorSource>(source);

            return source;
        }


        public static IProxyDescriptorSource AddHttpProxy(this IServiceCollection services, IConfiguration configuration, Assembly assembly)
        {
            var source = new HttpProxyDescriptorSource(configuration, new List<Assembly>() { assembly });
            services.AddSingleton<IProxyDescriptorSource>(source);

            return source;
        }
    }
}
