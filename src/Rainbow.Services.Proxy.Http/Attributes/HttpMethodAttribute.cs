using System;

namespace Rainbow.Services.Proxy.Http.Attributes
{


    [AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = false)]
    public class HttpMethodAttribute : Attribute
    {
        public string HttpMethod { get; set; }
        public string Template { get; set; }
    }
}