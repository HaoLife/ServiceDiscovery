using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Test
{
    [TestClass]
    public class UriBuilderExtensionsTest
    {
        [TestMethod]
        public void TestPathCombine()
        {
            UriBuilder builder = new UriBuilder("http://wwww.test.com/path/");

            builder.PathCombine("/api/test", "handle");

            Assert.AreEqual(builder.Path, "/path/api/test/handle");

        }
    }
}
