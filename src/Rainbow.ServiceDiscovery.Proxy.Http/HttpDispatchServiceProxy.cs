using Microsoft.Extensions.Logging;
using Rainbow.ServiceDiscovery.Proxy.Http.Attributes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpDispatchServiceProxy : DispatchProxy
    {
        private HttpDispatchServiceProxyFactory _httpDispatchServiceProxyFactory;
        private ILogger _logger;
        protected static Random _random = new Random();
        private HttpServiceInvoke _httpServiceInvoke;
        private Type _type;



        internal static TService CreateProxy<TService>(HttpDispatchServiceProxyFactory httpDispatchServiceProxyFactory)
        {
            TService proxy = DispatchProxy.Create<TService, HttpDispatchServiceProxy>();
            if (proxy == null)
            {
                throw new Exception("创建代理服务异常");
            }

            HttpDispatchServiceProxy channelProxy = (HttpDispatchServiceProxy)(object)proxy;
            channelProxy._httpDispatchServiceProxyFactory = httpDispatchServiceProxyFactory;
            channelProxy._logger = httpDispatchServiceProxyFactory.LoggerFactory.CreateLogger<HttpDispatchServiceProxy>();
            channelProxy._httpServiceInvoke = new HttpServiceInvoke();
            channelProxy._type = typeof(TService);
            return proxy;
        }

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            try
            {
                var serviceAttrs = _type.GetCustomAttributes<HttpServiceAttribute>();
                if (!serviceAttrs.Any())
                {
                    throw new Exception($"没有配置服务标记{nameof(HttpServiceAttribute)},无法使用代理服务");
                }

                var attr = serviceAttrs.FirstOrDefault();

                var instances = this._httpDispatchServiceProxyFactory.ServiceDiscovery.GetEndpoints(attr.ServiceName);

                if (!instances.Any())
                {
                    throw new Exception($"没有找到服务{attr.ServiceName}");
                }
                int index = _random.Next(instances.Count());

                ServiceInvokeContext context = new ServiceInvokeContext(instances.ElementAt(index), _type, targetMethod, args);

                return _httpServiceInvoke.Handle(context);

            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "服务调用失败");
                throw ex;
            }

        }
    }
}
