using Microsoft.Extensions.Logging;
using Rainbow.Services.Discovery;
using System;
using System.Collections.Generic;
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

        private static List<string> _methods = new List<string> { "GET", "POST", "DELETE", "PUT" };


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

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var endpoint = this._loadBalancer.Get(_provider.Discovery, _descriptor.ServiceName);

            var uriBuilder = endpoint.ToUriBuilder();

            var proxyName = _descriptor.ProxyType.Name;
            var httpMethod = _methods.Where(a => targetMethod.Name.ToUpper().StartsWith(a)).FirstOrDefault() ?? "";
            var methodName = targetMethod.Name.ToLower();

            if (this._provider.Options.IsFormatter && _descriptor.ProxyType.IsInterface && _descriptor.ProxyType.Name.StartsWith("I"))
            {
                proxyName = _descriptor.ProxyType.Name.Substring(1);
            }
            if (proxyName.EndsWith(this._provider.Options.Suffix))
            {
                proxyName = proxyName.Substring(0, proxyName.Length - this._provider.Options.Suffix.Length);
            }

            List<string> paths = new List<string>();
            if (uriBuilder.Path.Any() && !uriBuilder.Path.EndsWith("/"))
            {
                paths.Add(uriBuilder.Path);
            }
            if (!string.IsNullOrEmpty(this._descriptor.Prefix))
            {
                paths.Add(this._descriptor.Prefix);
            }
            paths.Add(proxyName);

            if (!this._provider.Options.IsWebApi)
            {
                httpMethod = "POST";
                paths.Add(methodName);
            }
            uriBuilder.Path = string.Join("/", paths);

            //这里使用什么输入格式，需要采用契约模式或者特性的方式实现

            var isGet = string.Compare(httpMethod, "GET", true) == 0;
            var httpClient = new HttpClient();
            var contentType = "application/x-www-form-urlencoded";

            if (!isGet)
            {
                contentType = "application/json";
            }
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(contentType));

            var inputContext = new InvokeInputContext(targetMethod, args, contentType);

            foreach (var formater in this._provider.Formatters)
            {
                if (formater.CanWrite(inputContext))
                {
                    formater.Write(inputContext);
                    break;
                }
            }

            HttpResponseMessage response = null;
            if (isGet && inputContext.Result != null)
            {
                uriBuilder.Query = inputContext.Result;

                response = httpClient.GetAsync(uriBuilder.ToString()).GetAwaiter().GetResult();
            }

            if (!isGet && inputContext.Result != null)
            {
                var requestMessage = new HttpRequestMessage();
                requestMessage.Method = new HttpMethod(httpMethod);
                requestMessage.Content = new StringContent(inputContext.Result);
                response = httpClient.SendAsync(requestMessage).GetAwaiter().GetResult();
            }


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
