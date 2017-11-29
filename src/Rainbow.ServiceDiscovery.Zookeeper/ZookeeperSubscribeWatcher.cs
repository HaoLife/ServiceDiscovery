using Microsoft.Extensions.Logging;
using org.apache.zookeeper;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    internal class ZookeeperSubscribeWatcher : Watcher
    {
        private readonly ZookeeperServiceDiscoveryProvider _provider;
        private readonly ILogger _logger;

        public ZookeeperSubscribeWatcher(ZookeeperServiceDiscoveryProvider provider, ILoggerFactory loggerFactory)
        {
            this._provider = provider;
            this._logger = loggerFactory.CreateLogger<ZookeeperSubscribeWatcher>();
        }

        public override Task process(WatchedEvent @event)
        {
            return Task.Run(() =>
            {
                this.NodeChange(@event);
            });
        }

        protected virtual void NodeDisconnected(WatchedEvent @event)
        {
            _logger.LogInformation($"zookeeper path:{@event.getPath()} state:{@event.getState()} type:{@event.get_Type()}");
        }

        protected virtual void NodeChange(WatchedEvent @event)
        {
            switch (@event.getState())
            {
                case Watcher.Event.KeeperState.Disconnected:
                    NodeDisconnected(@event);
                    break;
                case Watcher.Event.KeeperState.Expired:
                    this._provider.Initialize();
                    break;
                case Watcher.Event.KeeperState.SyncConnected:
                    Connection(@event);
                    break;
            }
        }


        protected virtual void Connection(WatchedEvent @event)
        {
            switch (@event.get_Type())
            {
                case Watcher.Event.EventType.NodeChildrenChanged:
                    ChildrenChange(@event);
                    break;
            }
        }


        protected virtual void ChildrenChange(WatchedEvent @event)
        {
            var serviceName = @event.getPath().GetServiceNameByPath();
            if (string.IsNullOrEmpty(serviceName)) return;

            this._provider.LoadService(serviceName);
        }
    }
}
