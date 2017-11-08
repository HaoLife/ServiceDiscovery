using System;
using System.Collections.Generic;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscoveryBuilder
    {
        IList<IServiceDiscoverySource> Sources { get; }

        IServiceDiscoveryBuilder Add(IServiceDiscoverySource source);

        IServiceDiscovery Build();

    }
}
