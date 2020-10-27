using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Proxy.Http.Patterns
{
    [Serializable]
    public sealed class RoutePatternException : Exception
    {
        public RoutePatternException(string pattern, string message)
            : base(message)
        {
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            Pattern = pattern;
        }

        public string Pattern { get; }

    }
}
