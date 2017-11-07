using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class ServiceDiscoveryStarter : IServiceDiscoveryStarter
    {
        private readonly ServiceDiscoveryOptions _options;

        public ServiceDiscoveryStarter(ServiceDiscoveryOptions options)
        {
            this._options = options;
        }

        public void Bootup()
        {
            foreach (var item in this._options.RegisterDirectory.GetRegisters())
            {
                item.Register();
            }

            foreach (var item in this._options.SubscriberDirectory.GetSubscribers())
            {
                item.Subscribe();
            }

        }

        public void Close()
        {
            foreach (var item in this._options.RegisterDirectory.GetRegisters())
            {
                item.Deregister();
            }
        }
    }
}
