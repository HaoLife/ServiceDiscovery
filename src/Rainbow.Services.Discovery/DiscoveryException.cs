using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class DiscoveryException : Exception
    {
        public DiscoveryException(IServiceDiscoveryProvider provider, string message)
            : base($"{provider.GetType().FullName} - {message}")
        {

        }
    }
}
