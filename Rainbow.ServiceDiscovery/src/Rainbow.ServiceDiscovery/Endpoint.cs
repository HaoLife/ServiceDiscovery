﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class Endpoint
    {
        public Endpoint(string protocol, string ip, int port, string path)
        {
            this.Protocol = protocol;
            this.Ip = ip;
            this.Port = port;
            this.Path = path;
        }

        public string Protocol { get; private set; }
        public string Ip { get; private set; }
        public int Port { get; private set; }
        public string Path { get; private set; }

        public override string ToString()
        {
            return string.Format("{0}_{1}_{2}_{3}", Protocol, Ip, Port, Path.Replace("/", "$"));
        }

        public Uri ToUri()
        {
            return new Uri(string.Format("{0}://{1}:{2}{3}", Protocol, Ip, Port, Path));
        }

        public static Endpoint ToEndpoint(Uri uri)
        {
            return new Endpoint(uri.Scheme, uri.Host, uri.Port, uri.AbsolutePath);
        }

    }
}
