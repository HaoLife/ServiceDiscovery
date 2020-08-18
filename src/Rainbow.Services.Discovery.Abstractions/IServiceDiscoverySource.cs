using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscoverySource
    {
        IServiceDiscoveryProvider Build(IServiceProvider privider);
    }
}
