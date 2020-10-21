using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public class ServiceRegisteryBuilder : IServiceRegisteryBuilder
    {
        public IList<IServiceRegisterySource> Sources { get; } = new List<IServiceRegisterySource>();

        public IServiceApplication Application { get; private set; } = ServiceApplication.Default();

        public ServiceRegisteryBuilder Add(IServiceRegisterySource source)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            Sources.Add(source);
            return this;
        }

        public ServiceRegisteryBuilder SetApplication(IServiceApplication application)
        {
            if (application == null)
            {
                throw new ArgumentNullException(nameof(application));
            }
            this.Application = application;
            return this;
        }


        public IServiceRegistery Build()
        {
            var providers = new List<IServiceRegisteryProvider>();
            foreach (var source in Sources)
            {
                var provider = source.Build(this);
                providers.Add(provider);
            }
            return new ServiceRegistery(this, providers);
        }
    }
}
