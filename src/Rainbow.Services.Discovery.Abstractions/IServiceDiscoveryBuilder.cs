using Microsoft.Extensions.DependencyInjection;
using System;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscoveryBuilder
    {
        IServiceCollection ServiceCollection { get; }

    }
}
