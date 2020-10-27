using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IProxyRoute
    {
        void Handle(RouteContext context, RouteResult result);
    }
}
