using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery.Consul
{
    public class ConsulServiceRegisteryOptions
    {
        public ConsulServiceRegisteryOptions()
        {

        }
        public ConsulServiceRegisteryOptions(IConfiguration configuration)
        {
            ConfigurationBinder.Bind(configuration, this);
        }

        public Uri Address { get; set; } = new Uri("http://localhost:8500/");
        public bool Check { get; set; } = false;
        public string CheckPath { get; set; }
        public TimeSpan CheckTimeout { get; set; } = new TimeSpan(0, 0, 10);
        public TimeSpan CheckInterval { get; set; } = new TimeSpan(0, 0, 10);

    }
}
