using Rainbow.Services.Proxy.Attributes;
using Rainbow.Services.Proxy.Http.Patterns;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Routes
{
    public class AttributeRoute : IProxyRoute
    {
        public void Handle(RouteContext context, RouteResult result)
        {
            var dict = new Dictionary<string, string>();

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
            dict.Add(HttpProxyDefaults.ProxyName, proxyName.ToLower());
            dict.Add(HttpProxyDefaults.MethodName, context.TargetMethod.Name.ToLower());

            var param = context.TargetMethod.GetParameters();
            for (var i = 0; i < param.Length; i++)
            {
                var p = param[i];
                dict.Add(p.Name, context.Args[i]?.ToString().ToLower() ?? "");
            }


            if (!result.ProxyResult)
            {
                if (context.Descriptor.ProxyType.GetCustomAttributes<HttpProxyRouteAttribute>().Any())
                {
                    var attr = context.Descriptor.ProxyType.GetCustomAttribute<HttpProxyRouteAttribute>();

                    var pattern = attr.Template;

                    var path = RoutePatternParser.Parse(pattern, dict);


                    result.ProxyResult = true;
                    result.ProxyRoute = path;
                }
            }

            if (!result.MethodResult)
            {
                if (context.TargetMethod.GetCustomAttributes<HttpProxyMethodAttribute>().Any())
                {
                    var attr = context.TargetMethod.GetCustomAttribute<HttpProxyMethodAttribute>();

                    var pattern = attr.Template;

                    var path = RoutePatternParser.Parse(pattern, dict);


                    result.MethodResult = true;
                    result.MethodRoute = path;
                    result.HttpMethod = attr.HttpMethod;
                }
            }
        }
    }
}
