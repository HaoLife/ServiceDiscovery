using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.ServiceDiscovery
{
    public abstract class DiscoveryHttpClientHandlerBase : HttpClientHandler
    {
        protected static Random _random = new Random();

        public DiscoveryHttpClientHandlerBase()
        {
        }

        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var current = request.RequestUri;
            try
            {
                request.RequestUri = LookupService(current);
                return await base.SendAsync(request, cancellationToken);
            }
            finally
            {
                request.RequestUri = current;
            }

        }

        public abstract Uri LookupService(Uri current);

    }
}
