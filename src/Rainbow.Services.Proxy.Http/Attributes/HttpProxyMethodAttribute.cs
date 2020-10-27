using System;

namespace Rainbow.Services.Proxy.Attributes
{


    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class HttpProxyMethodAttribute : Attribute
    {
        public HttpProxyMethodAttribute(string httpMethod, string template = "")
        {
            this.HttpMethod = httpMethod;
            this.Template = template;
        }

        public string HttpMethod { get; set; }
        public string Template { get; set; }
    }
}