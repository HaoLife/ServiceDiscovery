using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public interface IServiceDiscoveryBuilder
    {
        IList<IServiceDiscoverySource> Sources { get; }
        IServiceDiscovery Build(IServiceProvider services);
    }
}
