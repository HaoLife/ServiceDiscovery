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
    public interface IWeatherForecastService
    {
        //[ProxyGet]
        List<WeatherForecast> Get();

        WeatherForecast Get(int id);
    }
}
