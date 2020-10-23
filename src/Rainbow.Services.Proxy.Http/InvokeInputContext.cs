using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public class InvokeInputContext : IInputContext
    {
        public InvokeInputContext(MethodInfo method, object[] args, string contentType)
        {
            this.Paramters = method.GetParameters();
            this.Args = args;
            this.ContentType = contentType;
        }
        public object[] Args { get; set; }

        public ParameterInfo[] Paramters { get; set; }

        public string ContentType { get; set; }

        public string Result { get; set; }
    }
}
