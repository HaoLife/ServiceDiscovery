using System;

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
