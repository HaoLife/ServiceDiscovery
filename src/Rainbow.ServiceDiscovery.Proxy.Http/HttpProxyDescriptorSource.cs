using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpProxyDescriptorSource : IProxyDescriptorSource
    {
        private Dictionary<Type, string> mappings { get; set; } = new Dictionary<Type, string>();
        public HttpProxyDescriptorSource()
        {
        }
        public HttpProxyDescriptorSource(IConfiguration configuration, List<Assembly> assemblies)
        {
            var kv = configuration.AsEnumerable().ToList();

            var types = assemblies.SelectMany(a => a.GetTypes()).ToList();

            foreach (var item in kv)
            {
                var type = types.FirstOrDefault(a => a.FullName.Equals(item.Key));
                if (type != null)
                {
                    this.AddService(type, item.Value);
                }
            }


        }

        public void AddService(Type type, string service)
        {
            this.mappings.Add(type, service);
        }

        public IEnumerable<ProxyDescriptor> Build()
        {
            return this.mappings.Select(a => new ProxyDescriptor() { FactoryType = "http", ProxyType = a.Key, ServiceName = a.Value }).ToList();
        }

    }
}
