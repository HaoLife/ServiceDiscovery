using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public interface IContentFormatter
    {
        bool CanRead(IOutputFormatterContext context);

        void Read(IOutputFormatterContext context);


        bool CanWrite(IInputFormatterContext context);

        void Write(IInputFormatterContext context);
    }
}
