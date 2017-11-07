using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class SubscriberDirectory : ISubscriberDirectory
    {
        private readonly IServiceSubscriberFactory _serviceSubscriberFactory;
        private readonly List<IServiceSubscriber> _serviceSubscriber;
        public SubscriberDirectory(IServiceSubscriberFactory serviceSubscriberFactory)
        {
            this._serviceSubscriberFactory = serviceSubscriberFactory;
            this._serviceSubscriber = new List<IServiceSubscriber>();
        }

        public void Add(SubscribeDescribe describe)
        {
            if (this._serviceSubscriber.Any(a => describe.ServiceName == a.Name))
            {
                throw new Exception("重复添加订阅项");
            }
            var subscriber = this._serviceSubscriberFactory.CreateSubscriber(describe);

            this._serviceSubscriber.Add(subscriber);
        }

        public IEnumerable<IServiceSubscriber> GetSubscribers()
        {
            return this._serviceSubscriber;
        }
    }
}
