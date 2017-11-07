using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Routes
{
    public interface IHttpRouteReader
    {
        void Handle(RouteContext route);
    }
}
