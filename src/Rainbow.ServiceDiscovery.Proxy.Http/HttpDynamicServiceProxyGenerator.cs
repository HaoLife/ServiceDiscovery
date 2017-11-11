using Rainbow.ServiceDiscovery.Abstractions;
using Rainbow.ServiceDiscovery.Proxy.Abstractions;
using Rainbow.ServiceDiscovery.Proxy.Http.Formatters;
using Rainbow.ServiceDiscovery.Proxy.Http.Routes;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using System.Net;
using System.IO;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http
{
    public class HttpDynamicServiceProxyGenerator : IServiceProxyGenerator
    {
        private List<IOutputFormatter> _formaters;
        private List<IHttpRouteReader> _routeReaders;

        public HttpDynamicServiceProxyGenerator()
        {
            Init();
        }

        private void Init()
        {
            this._routeReaders = this.InitRouteReaders().ToList();
            this._formaters = this.InitOutputFormaters().ToList();
        }

        public virtual IEnumerable<IHttpRouteReader> InitRouteReaders()
        {
            var result = new List<IHttpRouteReader>();
            result.Add(new WebApiHttpRouteReader());
            result.Add(new AttributeHttpRouteReader());

            return result;
        }

        public virtual IEnumerable<IOutputFormatter> InitOutputFormaters()
        {
            var result = new List<IOutputFormatter>();
            result.Add(new KVOutputFormatter());
            result.Add(new JsonOutputFormatter());

            return result;
        }

        public T CreateServiceProxy<T>(ServiceEndpoint endpoint)
        {
            var proxy = new HttpDynamicProxy<T>(this, endpoint);

            return DispatchProxy.Create<T, HttpDynamicProxy<T>>();
        }

        public virtual RouteContext BuildRouteContext(ServiceInvokeContext context)
        {
            RouteContext routeContext = new RouteContext();
            routeContext.InvokeContext = context;
            foreach (var reader in this._routeReaders)
            {
                reader.Handle(routeContext);
            }

            return routeContext;

        }

        public virtual object Handle(ServiceInvokeContext context)
        {
            RouteContext routeContext = this.BuildRouteContext(context);

            UriBuilder builder = new UriBuilder(context.ServiceEndpoint.ToUri());

            builder.Path = string.Join("/", builder.Path.Substring(1), routeContext.ServicePath, routeContext.ActionPath);


            var isGet = string.Compare(routeContext.Method, "GET", true) == 0;

            IInputFormatterContext inputFormaterContext = new InvokeInputFormaterContext(routeContext);
            foreach (var formater in this._formaters)
            {
                if (formater.CanWrite(inputFormaterContext))
                {
                    formater.Write(inputFormaterContext);
                    break;
                }
            }

            if (isGet && inputFormaterContext.Result != null)
            {
                builder.Query = inputFormaterContext.Result;
            }


            var request = WebRequest.Create(builder.Uri);
            request.Method = routeContext.Method;

            if (!isGet && inputFormaterContext.Result != null)
            {
                request.ContentType = inputFormaterContext.ContentType;
                request.ContentLength = inputFormaterContext.Result.Length;
                Stream dataStream = request.GetRequestStream();
                byte[] byteArray = Encoding.UTF8.GetBytes(inputFormaterContext.Result);
                dataStream.Write(byteArray, 0, byteArray.Length);
                dataStream.Close();
            }


            using (WebResponse response = request.GetResponse())
            {
                IOutputFormatterContext formaterContext = new HttpOutputFormatterContext(response, context.Method);

                foreach (var formater in this._formaters)
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

        private class HttpDynamicProxy<T> : DispatchProxy
        {
            private readonly Type _proxyType;
            private readonly HttpDynamicServiceProxyGenerator _generator;
            private readonly ServiceEndpoint _endpoint;
            public HttpDynamicProxy(HttpDynamicServiceProxyGenerator generator, ServiceEndpoint endpoint)
            {
                _proxyType = typeof(T);
                _generator = generator;
                _endpoint = endpoint;
            }

            protected override object Invoke(MethodInfo targetMethod, object[] args)
            {
                ServiceInvokeContext context = new ServiceInvokeContext(this._endpoint, _proxyType, targetMethod, args);
                var result = this._generator.Handle(context);
                return result;
            }
        }
    }
}
