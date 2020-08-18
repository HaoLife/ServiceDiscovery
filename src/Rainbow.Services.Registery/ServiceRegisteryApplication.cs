using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public class ServiceRegisteryApplication : IServiceApplication
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; } = "http";


        public override string ToString()
        {
            return $"{this.Protocol}://{this.Host}:{this.Port}{this.Path}";
        }
    }
}
