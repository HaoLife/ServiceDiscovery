//using Rainbow.ServiceDiscovery.Proxy.Http.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rainbow.Services.Proxy.Attributes;

namespace Rainbow.Services.Discovery.Samples.Services
{
    [HttpProxy("samples")]
    //[Service("http://{sample}/weatherforecast)]
    [HttpProxyRoute("api/{proxy}")]
    public interface IWeatherForecastService
    {
        [HttpProxyGet]
        List<WeatherForecast> Get(string key);

        [HttpProxyGet("{id}")]
        WeatherForecast Get(int id);
    }
}
