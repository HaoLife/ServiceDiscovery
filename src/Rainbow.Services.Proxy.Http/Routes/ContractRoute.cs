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
                var proxyName = context.Descriptor.ProxyType.Name;

                //这里有不同的处理方式，1通过特性处理，2通过契约处理
                if (context.Options.IsFormatter && context.Descriptor.ProxyType.IsInterface && context.Descriptor.ProxyType.Name.StartsWith("I"))
                {
                    proxyName = context.Descriptor.ProxyType.Name.Substring(1);
                }
                if (proxyName.EndsWith(context.Options.Suffix))
                {
                    proxyName = proxyName.Substring(0, proxyName.Length - context.Options.Suffix.Length);
                }

                result.ProxyResult = true;
                result.ProxyRoute = proxyName;
            }


            if (!result.MethodResult)
            {
                var httpMethod = _methods.Where(a => context.TargetMethod.Name.ToUpper().StartsWith(a)).FirstOrDefault() ?? "POST";
                var methodName = context.TargetMethod.Name.ToLower();

                result.MethodResult = true;
                result.MethodRoute = methodName;
                result.HttpMethod = httpMethod;
            }
        }

    }
}
