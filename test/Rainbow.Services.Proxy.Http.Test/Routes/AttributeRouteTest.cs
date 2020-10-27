using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rainbow.Services.Proxy.Attributes;
using Rainbow.Services.Proxy.Http.Routes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Test.Routes
{
    [TestClass]
    public class AttributeRouteTest
    {
        [HttpProxyRoute("api/{proxy}")]
        public interface ITestService
        {
            [HttpProxyGet]
            List<string> Get(string key);

            [HttpProxyGet("{id}")]
            string Get(int id);
        }

        [TestMethod]
        public void TestHandle()
        {
            var route = new AttributeRoute();
            var context = new RouteContext()
            {
                TargetMethod = typeof(ITestService).GetMethod(nameof(ITestService.Get), new Type[] { typeof(string) }),
                Args = new object[] { },
                Descriptor = new ServiceProxyDescriptor() { ProxyType = typeof(ITestService), ServiceName = "samples", },
                Options = new HttpServiceProxyOptions() { }
            };
            var result = new RouteResult();
            route.Handle(context, result);
            Assert.AreEqual(result.ProxyRoute, "api/test");
            Assert.AreEqual(result.MethodRoute, "");
            Assert.AreEqual(result.HttpMethod, "GET");

        }
    }
}
