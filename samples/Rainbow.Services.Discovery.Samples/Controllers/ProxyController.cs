using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Rainbow.Services.Discovery.Samples.Services;
using Rainbow.Services.Proxy;

namespace Rainbow.Services.Discovery.Samples.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProxyController : ControllerBase
    {
        private readonly IServiceProxy proxy;

        public ProxyController(IServiceProxy proxy)
        {
            this.proxy = proxy;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {

            var service = proxy.Create<IWeatherForecastService>();

            var result = service.Get();

            return result;
        }
    }
}
