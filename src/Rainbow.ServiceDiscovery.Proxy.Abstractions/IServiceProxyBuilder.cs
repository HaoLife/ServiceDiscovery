using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy
{
    public interface IServiceProxyBuilder
    {
        IServiceCollection ServiceCollection { get; }
    }
}
