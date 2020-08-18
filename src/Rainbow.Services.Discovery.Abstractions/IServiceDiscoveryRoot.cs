using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscoveryRoot : IServiceDiscovery
    {
        void Reload();
        IEnumerable<IServiceDiscoveryProvider> Providers { get; }
    }
}
