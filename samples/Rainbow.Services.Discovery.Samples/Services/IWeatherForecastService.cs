using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rainbow.Services.Proxy.Http.Attributes;

namespace Rainbow.Services.Discovery.Samples.Services
{
    [HttpProxy("samples")]
    [HttpProxyRoute("api/{proxy}")]
    public interface IWeatherForecastService
    {
        //[HttpProxyGet]
        List<WeatherForecast> Get([HttpProxyQuery]string key);

        [HttpProxyGet("{id}")]
        WeatherForecast Get(int id);
    }
}
