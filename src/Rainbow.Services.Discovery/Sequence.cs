using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Rainbow.Services.Discovery
{
    public class Sequence
    {
        private long _value = -1;
        public long CurrentValue => _value;

        public long Next()
        {
            return Interlocked.Increment(ref _value);
        }
    }
}
