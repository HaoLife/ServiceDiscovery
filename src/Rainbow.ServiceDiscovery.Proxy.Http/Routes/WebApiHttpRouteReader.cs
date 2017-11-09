using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Routes
{
    public class WebApiHttpRouteReader : IHttpRouteReader
    {
        private readonly string[] _methodNames;
        private static string _serviceSuffix = "Service";
        private static string _servicePrefix = "I";

        public WebApiHttpRouteReader()
        {
            this._methodNames = new string[] { "GET", "POST", "PUT", "DELETE" };
        }

        public void Handle(RouteContext route)
        {

            route.ServicePath = route.InvokeContext.Service.Name;
            if (route.ServicePath.EndsWith(_serviceSuffix, StringComparison.InvariantCultureIgnoreCase))
            {
                route.ServicePath = route.ServicePath.Substring(0, route.ServicePath.Length - _serviceSuffix.Length);
            }

            if (route.ServicePath.StartsWith(_servicePrefix, StringComparison.InvariantCultureIgnoreCase))
            {
                route.ServicePath = route.ServicePath.Substring(_servicePrefix.Length);
            }

            route.ActionPath = route.InvokeContext.Method.Name;

            foreach (var item in this._methodNames)
            {
                if (route.InvokeContext.Method.Name.StartsWith(item, StringComparison.InvariantCultureIgnoreCase))
                {
                    route.Method = item;
                    break;
                }
            }
            var parms = route.InvokeContext.Method.GetParameters();
            switch (parms.Length)
            {
                case 1:
                    route.ContentType = ResultType(parms.First(), route);
                    break;
                default:
                    route.ContentType = ResultKV();
                    break;
            }



        }

        private string ResultKV()
        {
            return "application/x-www-form-urlencoded";
        }

        private string ResultJson()
        {
            return "application/json";
        }

        private string ResultType(ParameterInfo paramType, RouteContext route)
        {
            if (paramType.ParameterType.IsValueType || string.Compare(route.Method, "GET", true) == 0)
                return ResultKV();
            return ResultJson();
        }
    }
}
