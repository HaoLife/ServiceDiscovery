using Grpc.HealthCheck;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Rainbow.Services.Registery.Consul.GrpcHealthChecks;
using System;


namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extension methods for the gRPC health checks services.
    /// </summary>
    public static class GrpcHealthChecksServiceExtensions
    {
        /// <summary>
        /// Adds gRPC health check services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection"/> for adding services.</param>
        /// <returns>An instance of <see cref="IHealthChecksBuilder"/> from which health checks can be registered.</returns>
        public static IHealthChecksBuilder AddGrpcHealthChecks(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // HealthServiceImpl is designed to be a singleton
            services.TryAddSingleton<HealthServiceImpl>();

            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHealthCheckPublisher, GrpcHealthChecksPublisher>());

            return services.AddHealthChecks();
        }
    }
}
