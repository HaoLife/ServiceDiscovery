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
            : this(name, uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath)
        {

        }
        public ServiceEndpoint(string name, string protocol, string hostname, int port, string path)
        {

            this.Name = name;
            this.Protocol = protocol;
            this.HostName = hostname;
            this.Port = port;
            this.Path = path;
        }

        public string Name { get; set; }

        public string Protocol { get; set; }

        public string HostName { get; set; }

        public int Port { get; set; }

        public string Path { get; set; }

    }
}
