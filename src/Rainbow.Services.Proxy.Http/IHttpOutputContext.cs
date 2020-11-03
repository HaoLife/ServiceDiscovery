using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IHttpOutputContext
    {
        HttpResponseMessage Response { get; }

        Type OutType { get; }

        object Result { get; set; }
    }
}
