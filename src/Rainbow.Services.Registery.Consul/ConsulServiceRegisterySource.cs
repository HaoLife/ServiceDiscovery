using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery.Consul
{
    public class ConsulServiceRegisterySource : IServiceRegisterySource
    {
        public IConfiguration Configuration { get; set; }


        public ConsulServiceRegisterySource(IConfiguration configuration)
        {
            this.Configuration = configuration;
        }
        public IServiceRegisteryProvider Build(IServiceProvider privider)
        {
            return new ConsulServiceRegisteryProvider(privider, this);
        }
    }
}
