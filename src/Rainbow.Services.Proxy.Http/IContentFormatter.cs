using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IContentFormatter
    {
        bool CanRead(IHttpOutputContext context);

        void Read(IHttpOutputContext context);


        bool CanWrite(IHttpInputContext context);

        void Write(IHttpInputContext context);
    }
}
