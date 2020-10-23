using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery.Consul
{
    public class GrpcAgentServiceCheck : AgentServiceCheck
    {
        public string Grpc { get; set; }
        public bool GRPCUseTLS { get; set; }

    }
}
