using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using Rainbow.Services.Discovery;
using System;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpServiceProxySource : IServiceProxySource
    {
        public HttpServiceProxyOptions Options { get; set; } = new HttpServiceProxyOptions();
        private IConfiguration _configuration;

        public HttpServiceProxySource()
        {

        }
        public HttpServiceProxySource(IConfiguration configuration)
        {
            this._configuration = configuration;
            this.Options = new HttpServiceProxyOptions(configuration);
            ChangeToken.OnChange(configuration.GetReloadToken, RaiseChanged);
        }

        private void RaiseChanged()
        {
            this.Options = new HttpServiceProxyOptions(_configuration);
        }

        public IServiceProxyProvider Build(IServiceProvider provider)
        {
            var factory = provider.GetRequiredService<ILoggerFactory>();
            var discovery = provider.GetRequiredService<IServiceDiscovery>();
            return new HttpServiceProxyProvider(this, factory, discovery);
        }
    }
}
