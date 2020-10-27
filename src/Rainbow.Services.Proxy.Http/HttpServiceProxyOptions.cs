using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class HttpServiceProxyOptions
    {
        public string Suffix { get; set; } = "Service";
        public bool IsFormatter { get; set; } = true;
        //public bool IsWebApi { get; set; } = true;
        //public bool IsAction { get; set; } = false;

        public string LoadBalancer { get; set; } = "RollPollingLoadBalancer";
    }
}
