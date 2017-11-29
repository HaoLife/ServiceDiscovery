using Rainbow.ServiceDiscovery.Proxy.Http.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rainbow.ServiceDiscovery.SamplesServiceRegister.Services
{
    [HttpService("test")]
    [ServicePath("api/values")]
    public interface IValuesService
    {
        IEnumerable<string> Get();
        string Get(int id);
        void Post(string value);
        void Put(int id, string value);
        void Delete(int id);
    }
}
