using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceEndpoint
    {
        string Name { get; }
        string Protocol { get; }
        string Host { get; }
        int Port { get; }
        string Path { get; }
    }
}
