using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscoveryBuilder
    {
        IServiceCollection ServiceCollection { get; }

    }
}
