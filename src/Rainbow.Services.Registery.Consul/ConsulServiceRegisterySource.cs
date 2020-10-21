using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery.Consul
{
    public class ConsulServiceRegisterySource : IServiceRegisterySource
    {

        public ConsulServiceRegisterySource(ConsulServiceRegisteryOptions options)
        {
            Options = options;
        }

        public ConsulServiceRegisteryOptions Options { get; }

        public IServiceRegisteryProvider Build(IServiceRegisteryBuilder builder)
        {
            return new ConsulServiceRegisteryProvider(builder, this);
        }
    }
}
