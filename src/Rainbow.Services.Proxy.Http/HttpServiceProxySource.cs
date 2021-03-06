﻿using Microsoft.Extensions.Configuration;
using Rainbow.Services.Proxy.Http.Formatters;
using System;
using System.Collections.Generic;

namespace Rainbow.Services.Proxy.Http
{
    public class HttpServiceProxySource : IServiceProxySource
    {
        public IConfiguration Configuration { get; set; }
        public bool ReloadOnChange { get; set; }

        public List<IContentFormatter> Formatters { get; set; } = new List<IContentFormatter>()
        {
            new KVContentFormatter(),
            new TextContentFormatter(),
            new JsonContentFormatter(),
        };

        public IServiceProxyProvider Build(IServiceProxyBuilder builder, IServiceProvider services)
        {
            return new HttpServiceProxyProvider(builder, this, services);
        }
    }
}
