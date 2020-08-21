using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public class InvokeInputFormaterContext : IInputFormatterContext
    {
        public InvokeInputFormaterContext(MethodInfo method, object[] args, string contentType)
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
