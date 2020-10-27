using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rainbow.Services.Proxy.Http.Patterns;
using Rainbow.Services.Proxy.Http.Routes;
using System.Collections.Generic;

namespace Rainbow.Services.Proxy.Http.Test.Patterns
{
    [TestClass]
    public class RoutePatternParserTest
    {
        [TestMethod]
        public void TestParse()
        {
            var dict = new Dictionary<string, string> {
                { "proxy","test" },
                { "method","handle" }
            };
            var result = RoutePatternParser.Parse("/{proxy}/{method}", dict);

            Assert.AreEqual(result, "/test/handle");

        }
    }
}
