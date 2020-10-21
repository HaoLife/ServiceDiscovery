using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery.Consul
{
    public class ConsulServiceEndpoint : IServiceEndpoint
    {
        public ConsulServiceEndpoint(AgentService agent)
        {
            var protocol = ConsulDefaults.ProtocolValue;
            var path = ConsulDefaults.PathValue;

            if (agent.Meta != null && agent.Meta.ContainsKey(ConsulDefaults.Protocol))
            {
                protocol = agent.Meta[ConsulDefaults.Protocol];
            }
            if (agent.Meta != null && agent.Meta.ContainsKey(ConsulDefaults.Path))
            {
                path = agent.Meta[ConsulDefaults.Path];
            }

            this.Name = agent.Service;
            this.Protocol = protocol;
            this.Host = agent.Address;
            this.Port = agent.Port;
            this.Path = path;
        }
        public string Name { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Path { get; set; }

    }
}
