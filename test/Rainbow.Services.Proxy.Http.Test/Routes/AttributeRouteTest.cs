using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rainbow.Services.Proxy.Http.Attributes;
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
                Route = new RouteValueDictionary(new { proxy = "test", method = nameof(ITestService.Get) }),
                Descriptor = new ServiceProxyDescriptor() { ProxyType = typeof(ITestService), ServiceName = "samples", },
                Parameter = new RouteValueDictionary(new { key = "111" })
            };
            var result = new RouteResult();
            route.Handle(context, result);
            Assert.AreEqual(result.ProxyRoute, "api/test");
            Assert.AreEqual(result.MethodRoute, "");
            Assert.AreEqual(result.HttpMethod, "GET");

        }
    }
}
