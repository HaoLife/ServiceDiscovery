using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IHttpInputContext
    {
        //object[] Args { get; }
        //ParameterInfo[] Paramters { get; }

        string ContentType { get; }
        string HttpMethod { get; }

        RouteValueDictionary Query { get; }
        object Body { get; }

        HttpRequestMessage Request { get; }
    }
}
