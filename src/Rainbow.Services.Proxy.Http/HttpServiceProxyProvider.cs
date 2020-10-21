using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class HttpServiceProxyProvider : IServiceProxyProvider
    {
        private readonly IServiceProxyBuilder builder;
        private readonly HttpServiceProxySource source;
        public HttpServiceProxyOptions Options { get; private set; }
        public List<IContentFormatter> Formatters => source.Formatters;
        public IList<ILoadBalancer> LoadBalancers => builder.LoadBalancers;
        public IServiceDiscovery Discovery { get; private set; }


        public HttpServiceProxyProvider(IServiceProxyBuilder builder, HttpServiceProxySource source, IServiceProvider services)
        {
            this.builder = builder;
            this.source = source;
            this.Discovery = services.GetRequiredService<IServiceDiscovery>();

            this.Load();
            if (source.ReloadOnChange && source.Configuration != null)
            {
                ChangeToken.OnChange(() => source.Configuration.GetReloadToken(), () => this.Load());
            }
        }

        public bool CanHandle(string type)
        {
            return !string.IsNullOrEmpty(type) && type.ToLower().Equals(ProxyDefaults.ProviderName);
        }

        public T Create<T>(ServiceProxyDescriptor descriptor)
        {
            return HttpDispatchServiceProxy.CreateProxy<T>(this, descriptor);
        }

        public void Load()
        {
            this.Options = source.Configuration?.Get<HttpServiceProxyOptions>() ?? new HttpServiceProxyOptions();

        }
    }
}
