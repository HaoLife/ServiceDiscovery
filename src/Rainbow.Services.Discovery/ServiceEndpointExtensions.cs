using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rainbow.Services.Discovery
{
    public static class ServiceEndpointExtensions
    {
        public static UriBuilder ToUriBuilder(this IServiceEndpoint endpoint)
        {
            return new UriBuilder(endpoint.Protocol, endpoint.Host, endpoint.Port, endpoint.Path);
        }
        public static Uri ToUri(this IServiceEndpoint endpoint)
        {
            return endpoint.ToUriBuilder().Uri;
        }
        public static string ToUrl(this IServiceEndpoint endpoint)
        {
            return endpoint.ToUriBuilder().ToString();
        }
    }
}
