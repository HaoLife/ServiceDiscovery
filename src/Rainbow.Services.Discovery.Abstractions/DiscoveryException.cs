using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public class DiscoveryException : Exception
    {
        public DiscoveryException()
        {

        }
        public DiscoveryException(string message) : base(message)
        {

        }
    }
}
