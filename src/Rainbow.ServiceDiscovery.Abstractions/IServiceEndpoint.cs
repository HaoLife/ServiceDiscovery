using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceEndpoint
    {
        string Name { get; }
        string Protocol { get; }
        string HostName { get; }
        int Port { get; }
        string Path { get; }
    }
}
