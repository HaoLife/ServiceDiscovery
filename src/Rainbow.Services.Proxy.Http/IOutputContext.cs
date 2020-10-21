using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IOutputContext
    {
        long ContentLength { get; }
        string ContentType { get; }

        Type OutType { get; }
        Stream Stream { get; }

        object Result { get; set; }
    }
}
