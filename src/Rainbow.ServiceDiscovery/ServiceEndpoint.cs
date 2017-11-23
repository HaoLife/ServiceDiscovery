using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceEndpoint : IServiceEndpoint
    {
        public ServiceEndpoint(string name, string url)
            : this(name, new Uri(url))
        {

        }

        public ServiceEndpoint(string name, Uri uri)
        {
            this.Name = name;
            this.Protocol = uri.Scheme;
            this.HostName = uri.Host;
            this.Port = uri.Port;
            this.Path = uri.AbsolutePath;
        }

        public string Name { get; set; }

        public string Protocol { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string Path { get; set; }

    }
}
