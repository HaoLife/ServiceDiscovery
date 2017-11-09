using Rainbow.ServiceDiscovery.Proxy.Http.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Routes
{
    public class AttributeHttpRouteReader : IHttpRouteReader
    {
        public void Handle(RouteContext route)
        {

            var paths = route.InvokeContext.Service.GetCustomAttributes<ServicePathAttribute>();
            if (paths.Any())
            {
                route.ServicePath = paths.First().Path;
            }
            var actions = route.InvokeContext.Method.GetCustomAttributes<ServiceActionAttribute>();
            if (actions.Any())
            {
                var action = actions.First().Name;

                action = ReplaceAction(action, route);
                route.ActionPath = action;
            }

            var httpMethods = route.InvokeContext.Method.GetCustomAttributes<ServiceMethodAttribute>();
            if (httpMethods.Any())
            {
                route.Method = httpMethods.First().HttpMethod;
            }

            var contentTypes = route.InvokeContext.Method.GetCustomAttributes<ServiceContentTypeAttribute>();
            if (contentTypes.Any())
            {
                route.ContentType = contentTypes.First().Type;
            }

        }


        public virtual string ReplaceAction(string action, RouteContext context)
        {
            var parms = context.InvokeContext.Method.GetParameters();
            for (int i = 0; i < parms.Length; i++)
            {
                if (action.Contains("{" + parms[i].Name + "}"))
                {
                    action = action.Replace("{" + parms[i].Name + "}", context.InvokeContext.Args[i].ToString());
                    context.IgnoreParams.Add(parms[i].Name);
                }

            }
            return action;
        }

    }
}
