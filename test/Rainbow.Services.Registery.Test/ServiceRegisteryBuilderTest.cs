using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace Rainbow.Services.Registery.Test
{
    [TestClass]
    public class ServiceRegisteryBuilderTest
    {
        [TestMethod]
        public void Create()
        {
            var build = new ServiceRegisteryBuilder();
            build.SetApplication(new ServiceApplication() { Name = "test", Protocol = "http", Host = "localhost", Path = "", Port = 5000 });

            var register = build.Build();

            Assert.AreEqual(register.Application.Name, "test");
            Assert.AreEqual(register.Application.Protocol, "http");
            Assert.AreEqual(register.Application.Host, "localhost");
            Assert.AreEqual(register.Application.Path, "");
            Assert.AreEqual(register.Application.Port, 5000);

            Assert.AreEqual(register.Providers.Count(), 0);

        }

        [TestMethod]
        public void CreateDefault()
        {
            var build = new ServiceRegisteryBuilder();
            var register = build.Build();

            Assert.AreEqual(register.Application.Name, System.Net.Dns.GetHostName());
            Assert.AreEqual(register.Application.Protocol, "http");
            Assert.AreEqual(register.Application.Path, null);
            Assert.AreEqual(register.Application.Port, 5000);

            Assert.AreEqual(register.Providers.Count(), 0);

        }
    }
}
