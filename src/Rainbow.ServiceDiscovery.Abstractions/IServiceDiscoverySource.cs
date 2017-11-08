using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Abstractions
{
    public interface IServiceDiscoverySource
    {
        IServiceDiscoveryProvider Build(IServiceDiscoveryBuilder builder);
    }
}
