using Rainbow.Services.Proxy;
using Rainbow.Services.Proxy.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.Configuration
{
    public static class ServiceProxyBuilderExtensions
    {
        public static ServiceProxyBuilder AddHttp(this ServiceProxyBuilder builder, IConfiguration configuration = null, bool reloadOnChange = false)
        {
            return builder.AddHttp((c) =>
            {
                c.Configuration = configuration;
                c.ReloadOnChange = reloadOnChange;
            });
        }

        public static ServiceProxyBuilder AddHttp(this ServiceProxyBuilder builder, Action<HttpServiceProxySource> action)
        {
            var source = new HttpServiceProxySource();
            action?.Invoke(source);

            return builder.Add(source);
        }
    }
}
