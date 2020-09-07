using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Rainbow.ServiceDiscovery.Proxy.Http.Formatters;
using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpServiceProxyProvider : IServiceProxyProvider
    {
        private HttpServiceProxySource _source;

        public HttpServiceProxyProvider(
            HttpServiceProxySource source,
            ILoggerFactory loggerFactory,
            IServiceDiscovery serviceDiscovery,
            ILoadBalancer loadBalancer)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            this._source = source;
            this.LoggerFactory = loggerFactory;
            this.Discovery = serviceDiscovery;
            LoadBalancer = loadBalancer;
            this.ContentFormatters = this.InitContentFormaters();

        }

        public ILoggerFactory LoggerFactory { get; set; }
        public IServiceDiscovery Discovery { get; set; }
        public ILoadBalancer LoadBalancer { get; }
        public List<IContentFormatter> ContentFormatters { get; private set; }

        public HttpServiceProxyOptions Options => this._source.Options;

        public bool CanHandle(string type)
        {
            return !string.IsNullOrEmpty(type) && type.ToLower().Equals("http");
        }

        public T Create<T>(ProxyDescriptor descriptor)
        {
            return HttpDispatchServiceProxy.CreateProxy<T>(this, descriptor);
        }


        public virtual List<IContentFormatter> InitContentFormaters()
        {
            var result = new List<IContentFormatter>();
            result.Add(new KVContentFormatter());
            result.Add(new TextContentFormatter());
            result.Add(new JsonContentFormatter());
            return result;
        }
    }
}
