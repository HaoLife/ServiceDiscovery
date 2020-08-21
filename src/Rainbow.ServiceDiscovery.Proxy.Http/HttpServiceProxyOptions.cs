using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpServiceProxyOptions
    {
        public HttpServiceProxyOptions()
        {

        }
        public HttpServiceProxyOptions(IConfiguration configuration)
        {
            Configure(configuration);
        }

        private void Configure(IConfiguration configuration)
        {
            ConfigurationBinder.Bind(configuration, this);
        }

        public string Suffix { get; set; } = "Service";
        public bool IsFormatter { get; set; } = true;

    }
}
