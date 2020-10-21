using Microsoft.Extensions.Configuration;
using Rainbow.Services.Registery;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ServiceRegisteryBuilderExtensions
    {
        public static ServiceRegisteryBuilder SetApplication(this ServiceRegisteryBuilder builder, IConfiguration configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }


            var app = configuration.Get<ServiceApplication>();
            return builder.SetApplication(app);
        }

    }
}
