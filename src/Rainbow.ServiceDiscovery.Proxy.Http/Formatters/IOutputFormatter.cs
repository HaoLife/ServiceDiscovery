using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public interface IOutputFormatter
    {
        bool CanRead(IOutputFormatterContext context);

        void Read(IOutputFormatterContext context);


        bool CanWrite(IInputFormatterContext context);

        void Write(IInputFormatterContext context);
        
    }
}
