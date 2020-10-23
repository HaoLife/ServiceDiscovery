using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http
{
    public interface IContentFormatter
    {
        bool CanRead(IOutputContext context);

        void Read(IOutputContext context);


        bool CanWrite(IInputContext context);

        void Write(IInputContext context);
    }
}
