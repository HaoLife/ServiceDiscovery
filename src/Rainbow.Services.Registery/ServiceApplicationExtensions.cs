using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.Services.Registery
{
    public static class ServiceApplicationExtensions
    {
        public static UriBuilder ToUriBuilder(this IServiceApplication endpoint)
        {
            return new UriBuilder(endpoint.Protocol, endpoint.Host, endpoint.Port, endpoint.Path);
        }
        public static Uri ToUri(this IServiceApplication endpoint)
        {
            return endpoint.ToUriBuilder().Uri;
        }
        public static string ToUrl(this IServiceApplication endpoint)
        {
            return endpoint.ToUriBuilder().ToString();
        }

        public static string ToUrl(this IServiceApplication endpoint, string path)
        {
            var builder = endpoint.ToUriBuilder();
            return $"{builder.ToString()}" + (builder.Path.EndsWith("/") ? $"{path}" : $"/{path}");
        }
    }
}
