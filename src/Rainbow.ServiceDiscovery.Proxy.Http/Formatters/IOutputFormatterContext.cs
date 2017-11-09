using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery.Proxy.Http.Formatters
{
    public interface IOutputFormatterContext
    {
        long ContentLength { get; }
        string ContentType { get; }

        Type OutType { get; }
        Stream GetStream();

        object Result { get; set; }
    }
}
