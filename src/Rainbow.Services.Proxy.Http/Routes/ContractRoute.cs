using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Routes
{
    public class ContractRoute : IProxyRoute
    {
        private static List<string> _methods = new List<string> { "GET", "POST", "DELETE", "PUT" };


        public void Handle(RouteContext context, RouteResult result)
        {
            if (!result.ProxyResult)
            {
                result.ProxyResult = true;
                result.ProxyRoute = context.Route[HttpProxyDefaults.ProxyName].ToString();
            }



            if (!result.MethodResult)
            {
                var httpMethod = _methods.Where(a => context.TargetMethod.Name.ToUpper().StartsWith(a)).FirstOrDefault() ?? "POST";
                var methodName = context.Route[HttpProxyDefaults.MethodName].ToString();

                result.MethodResult = true;
                result.MethodRoute = context.Options.IsRestful ? "" : methodName;
                result.HttpMethod = httpMethod;
                result.ContentType = string.Compare(httpMethod, "GET", true) == 0 ? "application/x-www-form-urlencoded" : "application/json";
            }
        }

    }
}
