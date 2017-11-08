using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public interface IServiceDiscoveryRoot : IServiceDiscovery
    {
        void Reload();
        IEnumerable<IServiceDiscoveryProvider> Providers { get; }
    }
}
