using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Rainbow.ServiceDiscovery.Abstractions;
using Microsoft.Extensions.Configuration;

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly IServiceDiscovery _serviceDiscovery;
        private readonly IConfiguration _config;
        public ValuesController(IServiceDiscovery serviceDiscovery, IConfiguration config)
        {
            this._serviceDiscovery = serviceDiscovery;
            this._config = config;
        }

        // GET api/values
        [HttpGet]
        public IEnumerable<string> Get()
        {
            var appName = this._config.GetValue<string>("service:Application:name");
            var a = this._serviceDiscovery.GetEndpoints(appName);
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
