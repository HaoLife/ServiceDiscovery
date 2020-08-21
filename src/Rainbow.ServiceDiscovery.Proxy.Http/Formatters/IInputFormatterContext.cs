using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public interface IInputFormatterContext
    {
        object[] Args { get; }
        ParameterInfo[] Paramters { get; }

        string ContentType { get; }

        string Result { get; set; }
    }
}
