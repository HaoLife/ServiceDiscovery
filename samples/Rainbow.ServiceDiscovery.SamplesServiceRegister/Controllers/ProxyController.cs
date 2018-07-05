using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rainbow.ServiceDiscovery.Abstractions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using Rainbow.ServiceDiscovery.Proxy.Http;
using Rainbow.ServiceDiscovery.SamplesServiceRegister.Services;

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister.Controllers
{
    [Produces("application/json")]
    [Route("api/Proxy")]
    public class ProxyController : Controller
    {
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly ILoggerFactory _loggerFactory;
        private readonly IConfiguration _config;

        public ProxyController(IServiceDiscovery serviceDiscovery, ILoggerFactory loggerFactory, IConfiguration config)
        {
            this._serviceDiscovery = serviceDiscovery;
            this._config = config;
            this._loggerFactory = loggerFactory;
        }

        // GET: api/Proxy
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var appName = this._config.GetValue<string>("service:Application:name");
            var a = this._serviceDiscovery.GetEndpoints(appName);
            return new string[] { "value1", "value2" };
        }

        // GET: api/Proxy/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            var factory = new HttpDispatchServiceProxyFactory(_serviceDiscovery, _loggerFactory);
            var service = factory.CreateProxy<IValuesService>();

            return service.Get(id);
        }
        
        // POST: api/Proxy
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }
        
        // PUT: api/Proxy/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }
        
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
