using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public class ServiceRegistery : IServiceRegistery
    {
        private readonly IEnumerable<IServiceRegisteryProvider> providers;
        private ServiceRegisteryApplication application;

        public ServiceRegistery(IEnumerable<IServiceRegisteryProvider> providers,
            IOptionsMonitor<ServiceRegisteryApplication> options)
        {
            this.providers = providers;
            this.application = options.CurrentValue;
            this.Init();
            options.OnChange(a => this.RaiseChanged(a));
        }
        private void RaiseChanged(ServiceRegisteryApplication app)
        {
            this.Deregister();
            this.application = app;
            this.Register();
        }

        protected virtual void Init()
        {
            if (string.IsNullOrEmpty(this.application.Host))
            {
                this.application.Host = this.GetHostAddresss();
            }
        }

        public string GetHostAddresss()
        {
            string hostName = System.Net.Dns.GetHostName();
            var task = System.Net.Dns.GetHostAddressesAsync(hostName);
            task.Wait();

            string address = string.Empty;
            if (task.Result != null && task.Result.Length > 0)
            {
                foreach (var result in task.Result)
                {
                    if (result.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        address = result.ToString();
                        break;
                    }
                }
            }

            return address;
        }

        public void Register()
        {
            foreach (var item in this.providers)
            {
                item.Register(this.application);
                ChangeToken.OnChange(() => item.GetReloadToken(), () => this.Reload(item));
            }
        }

        private void Reload(IServiceRegisteryProvider provider)
        {
            provider.Deregister(this.application);
            provider.Load();
            provider.Register(this.application);
        }

        public void Deregister()
        {
            foreach (var item in this.providers)
            {
                item.Deregister(this.application);
            }
        }
    }
}
