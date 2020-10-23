using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IInputContext
    {
        object[] Args { get; }
        ParameterInfo[] Paramters { get; }

        string ContentType { get; }

        string Result { get; set; }
    }
}
