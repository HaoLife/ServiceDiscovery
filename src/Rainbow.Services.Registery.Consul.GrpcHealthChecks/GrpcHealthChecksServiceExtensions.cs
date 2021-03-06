﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
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
        public static IServiceCollection AddGrpcHealthChecks(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            // HealthServiceImpl is designed to be a singleton
            services.TryAddSingleton<GrpcHealthService>();

            return services;
        }
    }
}
