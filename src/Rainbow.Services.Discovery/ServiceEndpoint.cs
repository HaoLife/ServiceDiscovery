using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
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
        public ServiceEndpoint(string name, string protocol, string host, int port, string path)
        {

            this.Name = name;
            this.Protocol = protocol;
            this.Host = host;
            this.Port = port;
            this.Path = path;
        }

        public string Name { get; set; }

        public string Protocol { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string Path { get; set; }

    }
}
