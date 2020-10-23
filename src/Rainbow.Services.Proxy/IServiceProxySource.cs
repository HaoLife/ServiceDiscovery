using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy
{
    public interface IServiceProxySource
    {
        IServiceProxyProvider Build(IServiceProxyBuilder builder, IServiceProvider services);
    }
}
