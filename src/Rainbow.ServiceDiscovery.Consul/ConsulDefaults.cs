using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Consul
{
    public static class ConsulDefaults
    {
        public static readonly string Path = "path";
        public static readonly string Protocol = "protocol";

        public static readonly string ProtocolValue = "http";
        public static readonly string PathValue = "";

        public static readonly string HealthStatusPassing = "passing";
    }
}
