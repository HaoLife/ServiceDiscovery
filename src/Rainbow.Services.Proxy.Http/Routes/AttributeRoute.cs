using Rainbow.Services.Proxy.Http.Attributes;
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
            var dict = new RouteValueDictionary(context.Parameter.Union(context.Route).ToList());


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
                    result.ContentType = attr.ContentType;
                }
            }
        }
    }
}
