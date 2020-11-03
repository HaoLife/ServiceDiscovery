using Microsoft.Extensions.Logging;
using Rainbow.Services.Discovery;
using Rainbow.Services.Proxy.Http.Attributes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.Services.Proxy.Http
{
    public class HttpDispatchServiceProxy : DispatchProxy
    {
        private HttpServiceProxyProvider _provider;
        //private ILogger _logger;
        private ServiceProxyDescriptor _descriptor;
        private ILoadBalancer _loadBalancer;

        internal static TService CreateProxy<TService>(HttpServiceProxyProvider provider, ServiceProxyDescriptor descriptor)
        {
            TService proxy = DispatchProxy.Create<TService, HttpDispatchServiceProxy>();
            if (proxy == null)
            {
                throw new Exception("创建代理服务异常");
            }

            var loadBalancer = string.IsNullOrEmpty(descriptor.LoadBalancer)
                ? provider.Options.LoadBalancer : descriptor.LoadBalancer;

            var channelProxy = (HttpDispatchServiceProxy)(object)proxy;
            channelProxy._provider = provider;
            channelProxy._descriptor = descriptor;

            channelProxy._loadBalancer =
                provider.LoadBalancers.Any(a => string.Compare(a.GetType().Name, loadBalancer, true) == 0) ?
                provider.LoadBalancers.First(a => string.Compare(a.GetType().Name, loadBalancer, true) == 0)
                : provider.LoadBalancers.First();

            return proxy;
        }



        private RouteValueDictionary GetParam(MethodInfo targetMethod, object[] args)
        {

            var paramDict = new RouteValueDictionary();

            var parms = targetMethod
                .GetParameters()
                .Select((a, i) => new { Paramert = a, Value = args[i] })
                .ToList();

            parms.ForEach(a => paramDict.Add(a.Paramert.Name, a.Value));

            return paramDict;
        }

        private RouteValueDictionary GetQuery(MethodInfo targetMethod, object[] args)
        {

            var queryDict = new RouteValueDictionary();

            var parms = targetMethod
                .GetParameters()
                .Select((a, i) => new { Paramert = a, Value = args[i] })
                .ToList();


            //设置请求的url参数值
            var querys = parms
                .Where(a => a.Paramert.GetCustomAttributes<HttpProxyQueryAttribute>().Any())
                .Select(a => new { Paramert = a.Paramert, Value = a.Value, Attribute = a.Paramert.GetCustomAttribute<HttpProxyQueryAttribute>() })
                .ToList();

            if (querys.Any())
            {
                if (querys.Count == 1)
                {
                    var item = querys.First();

                    if (item.Paramert.ParameterType.IsClass)
                    {
                        //如果是空，强制用无内容代替
                        queryDict = new RouteValueDictionary(item.Value ?? new { });
                    }
                    else
                    {
                        var name = string.IsNullOrEmpty(item.Attribute.Name) ? item.Paramert.Name : item.Attribute.Name;
                        queryDict.Add(name, item.Value);
                    }
                }
                else
                {
                    foreach (var item in querys)
                    {
                        if (item.Paramert.ParameterType.IsClass) continue;

                        var name = string.IsNullOrEmpty(item.Attribute.Name) ? item.Paramert.Name : item.Attribute.Name;
                        queryDict.Add(name, item.Value);
                    }
                }
            }
            else
            {
                foreach (var item in parms)
                {
                    if (item.Paramert.ParameterType.IsClass) continue;

                    queryDict.Add(item.Paramert.Name, item.Value);

                }
            }

            return queryDict;
        }

        private object GetBody(MethodInfo targetMethod, object[] args)
        {
            var parms = targetMethod
                .GetParameters()
                .Select((a, i) => new { Paramert = a, Value = args[i] })
                .ToList();


            //设置请求的url参数值
            var bodys = parms
                .Where(a => a.Paramert.GetCustomAttributes<HttpProxyQueryAttribute>().Any())
                .Select(a => new { Paramert = a.Paramert, Value = a.Value, Attribute = a.Paramert.GetCustomAttribute<HttpProxyQueryAttribute>() })
                .ToList();

            if (bodys.Any())
            {
                if (bodys.Count != 1) throw new Exception("body 参数不能拥有多个");

                var item = bodys.First();

                return item.Value;
            }
            else
            {
                foreach (var item in parms)
                {
                    //获取第一个class作为body
                    if (item.Paramert.ParameterType.IsClass)
                    {
                        return item.Value;
                    }

                }
            }

            return null;
        }


        private RouteValueDictionary GetRoute(MethodInfo targetMethod, object[] args)
        {

            var proxyName = _descriptor.ProxyType.Name;

            //这里有不同的处理方式，1通过特性处理，2通过契约处理
            if (_provider.Options.IsFormatter && _descriptor.ProxyType.IsInterface && _descriptor.ProxyType.Name.StartsWith("I"))
            {
                proxyName = _descriptor.ProxyType.Name.Substring(1);
            }
            if (proxyName.EndsWith(_provider.Options.Suffix))
            {
                proxyName = proxyName.Substring(0, proxyName.Length - _provider.Options.Suffix.Length);
            }

            return new RouteValueDictionary() {
                { HttpProxyDefaults.ProxyName, proxyName.ToLower() },
                { HttpProxyDefaults.MethodName, targetMethod.Name.ToLower() },
            };

        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var endpoint = this._loadBalancer.Get(_provider.Discovery, _descriptor.ServiceName);

            var uriBuilder = endpoint.ToUriBuilder();


            var paramDict = this.GetParam(targetMethod, args);
            var queryDict = this.GetQuery(targetMethod, args);
            var routeDict = this.GetRoute(targetMethod, args);

            var body = this.GetBody(targetMethod, args);



            var routeContext = new RouteContext()
            {
                Descriptor = _descriptor,
                TargetMethod = targetMethod,
                Route = routeDict,
                Parameter = paramDict,
                Options = _provider.Options,
            };
            var result = new RouteResult();

            foreach (var route in this._provider.Routes)
            {
                route.Handle(routeContext, result);
            }

            if (result.MethodRoute.StartsWith("/"))
            {
                uriBuilder.PathCombine(uriBuilder.Path, result.MethodRoute);
            }
            else
            {
                uriBuilder.PathCombine(uriBuilder.Path, result.ProxyRoute, result.MethodRoute);
            }

            var httpClient = new HttpClient();
            var inputContext = new InvokeInputContext(uriBuilder.Uri.ToString(), result.HttpMethod, result.ContentType, queryDict, body);

            foreach (var formater in this._provider.Formatters)
            {
                if (formater.CanWrite(inputContext))
                {
                    formater.Write(inputContext);
                    break;
                }
            }

            var response = httpClient.SendAsync(inputContext.Request).GetAwaiter().GetResult();


            var formaterContext = new InvokeOutputContext(response, targetMethod);

            foreach (var formater in this._provider.Formatters)
            {
                if (formater.CanRead(formaterContext))
                {
                    formater.Read(formaterContext);
                    break;
                }
            }
            return formaterContext.Result;
        }


    }
}
