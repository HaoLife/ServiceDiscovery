using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Proxy.Attributes;
using Rainbow.ServiceDiscovery.Proxy.Http.Attributes;
using Rainbow.ServiceDiscovery.Proxy.Http.Formatters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpDispatchServiceProxy : DispatchProxy
    {
        private HttpServiceProxyProvider _httpServiceProxyProvider;
        private ILogger _logger;
        protected static Random _random = new Random();
        private ProxyDescriptor _descriptor;
        private static List<string> _methods = new List<string> { "GET", "POST", "DELETE", "PUT" };



        internal static TService CreateProxy<TService>(HttpServiceProxyProvider httpServiceProxyProvider, ProxyDescriptor descriptor)
        {
            TService proxy = DispatchProxy.Create<TService, HttpDispatchServiceProxy>();
            if (proxy == null)
            {
                throw new Exception("创建代理服务异常");
            }

            HttpDispatchServiceProxy channelProxy = (HttpDispatchServiceProxy)(object)proxy;
            channelProxy._httpServiceProxyProvider = httpServiceProxyProvider;
            channelProxy._logger = httpServiceProxyProvider.LoggerFactory.CreateLogger<HttpDispatchServiceProxy>();
            channelProxy._descriptor = descriptor;
            return proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var endpoint =
                this._httpServiceProxyProvider.LoadBalancer.Get(_httpServiceProxyProvider.Discovery, _descriptor.ServiceName);


            UriBuilder builder = new UriBuilder($"{endpoint.Protocol}://{endpoint.Host}:{endpoint.Port}");

            var servicePath = _descriptor.ProxyType.Name;
            var methodPath = "";
            var method = "POST";


            var serviceAttr = _descriptor.ProxyType.GetCustomAttribute<ServiceRouteAttribute>();
            if (serviceAttr != null)
            {
                servicePath = serviceAttr.Path;
            }
            else
            {
                if (this._httpServiceProxyProvider.Options.IsFormatter && _descriptor.ProxyType.IsInterface && _descriptor.ProxyType.Name.StartsWith("I"))
                {
                    servicePath = _descriptor.ProxyType.Name.Substring(1);
                }
                if (servicePath.EndsWith(this._httpServiceProxyProvider.Options.Suffix))
                {
                    servicePath = servicePath.Substring(0, servicePath.Length - this._httpServiceProxyProvider.Options.Suffix.Length);
                }
            }


            var methodAttr = targetMethod.GetCustomAttribute<ProxyHttpAttribute>();
            if (methodAttr != null)
            {
                method = methodAttr.Method;
                methodPath = string.IsNullOrEmpty(methodAttr.Path) ? methodPath : methodAttr.Path;
            }
            else
            {
                foreach (var item in _methods)
                {
                    if (targetMethod.Name.ToUpper().StartsWith(item))
                    {
                        method = item;
                        break;
                    }
                }
            }
            var path = endpoint.Path.Last().Equals('/') ? endpoint.Path.Substring(0, endpoint.Path.Length - 1) : endpoint.Path;


            builder.Path = $"{path}/{servicePath}/{methodPath}";

            var isGet = string.Compare(method, "GET", true) == 0;

            var request = WebRequest.Create(builder.Uri);
            request.Method = method;

            if (isGet)
            {
                request.ContentType = "application/x-www-form-urlencoded";
            }
            else
            {
                request.ContentType = "application/json";
            }

            var inputContext = new InvokeInputFormaterContext(targetMethod, args, request.ContentType);


            foreach (var formater in this._httpServiceProxyProvider.ContentFormatters)
            {
                if (formater.CanWrite(inputContext))
                {
                    formater.Write(inputContext);
                    break;
                }
            }

            if (isGet && inputContext.Result != null)
            {
                builder.Query = inputContext.Result;
            }

            if (!isGet && inputContext.Result != null)
            {
                request.ContentType = inputContext.ContentType;
                request.ContentLength = inputContext.Result.Length;
                Stream dataStream = request.GetRequestStream();
                byte[] byteArray = Encoding.UTF8.GetBytes(inputContext.Result);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }


            using (WebResponse response = request.GetResponse())
            {
                IOutputFormatterContext formaterContext = new InvokeOutputFormaterContext(response, targetMethod);

                foreach (var formater in this._httpServiceProxyProvider.ContentFormatters)
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
}
