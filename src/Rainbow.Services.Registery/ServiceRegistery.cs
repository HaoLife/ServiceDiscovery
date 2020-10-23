using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace Rainbow.Services.Registery
{
    public class ServiceRegistery : IServiceRegistery
    {
        private readonly ServiceRegisteryBuilder builder;
        public IEnumerable<IServiceRegisteryProvider> Providers { get; }

        public ServiceRegistery(ServiceRegisteryBuilder builder, IEnumerable<IServiceRegisteryProvider> providers)
        {
            this.builder = builder;
            Providers = providers;
        }

        public IServiceApplication Application => this.builder.Application;


        public void Deregister()
        {
            foreach (var item in this.Providers)
            {
                item.Deregister();
            }
        }

        public void Register()
        {
            foreach (var item in this.Providers)
            {
                item.Register();
            }
        }
    }
}
