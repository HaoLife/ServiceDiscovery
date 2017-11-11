using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using System.Linq;
using org.apache.zookeeper;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using static org.apache.zookeeper.ZooDefs;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();
        private readonly ZookeeperServiceDiscoverySource _source;
        private ZooKeeper _zkClient;
        private SortedDictionary<string, List<IServiceEndpoint>> _cache = new SortedDictionary<string, List<IServiceEndpoint>>();
        private ILogger _logger;

        public ZookeeperServiceDiscoveryProvider(
            ZookeeperServiceDiscoverySource source,
            ILoggerFactory loggerFactory)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            this._source = source;
            this._zkClient = new ZooKeeper(source.Options.Connection, (int)source.Options.SessionTimeout.TotalMilliseconds, new SubscribeWatcher(this));
            this._logger = loggerFactory.CreateLogger<ZookeeperServiceDiscoveryProvider>();
        }

        public IEnumerable<IServiceEndpoint> GetEndpoints(string serviceName)
        {
            List<IServiceEndpoint> result;
            if (!_cache.TryGetValue(serviceName, out result))
            {
                return Enumerable.Empty<IServiceEndpoint>();
            }
            return result;
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }


        public void Load(IServiceDiscovery serviceDiscovery)
        {
            if (this._source.Options.IsRegister)
            {
                Create(serviceDiscovery);
            }
            LoadServices();

        }

        private void Create(IServiceDiscovery serviceDiscovery)
        {
            var endpoint = serviceDiscovery.GetLocalEndpoint();
            var path = endpoint.ToPath();
            try
            {
                var result = this._zkClient.createAsync(path, null, Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL).GetAwaiter().GetResult();
            }
            catch (KeeperException.NoNodeException noex)
            {
                CreateNode($"/{ZookeeperDefaults.NameService}");
                CreateNode(endpoint.Name.GetServiceDirectory());
                Create(serviceDiscovery);
            }
            catch (KeeperException.NodeExistsException existsex)
            {
                _zkClient.deleteAsync(path, -1).GetAwaiter().GetResult();
                Create(serviceDiscovery);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "zookeeper 未知异常");
            }

        }

        private void CreateNode(string node)
        {
            this._zkClient.createAsync(node, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
        }

        private void LoadServices()
        {

            var path = $"/{ZookeeperDefaults.NameService}";
            try
            {
                var children = this._zkClient.getChildrenAsync(path, false).GetAwaiter().GetResult();
                _cache.Clear();
                foreach (var item in children.Children)
                {
                    LoadService(item);
                }

            }
            catch (KeeperException.NodeExistsException existsex)
            {
                this._zkClient.createAsync(path, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
                LoadServices();
            }
        }

        private void LoadService(string service)
        {
            List<IServiceEndpoint> endpoints;
            if (!_cache.TryGetValue(service, out endpoints))
            {
                endpoints = new List<IServiceEndpoint>();
                _cache.Add(service, endpoints);
            }
            endpoints.Clear();
            var serviceDir = service.GetServiceDirectory();
            var pathChildren = this._zkClient.getChildrenAsync(serviceDir, true).GetAwaiter().GetResult();
            foreach (var address in pathChildren.Children)
            {
                endpoints.Add(new ServiceEndpoint(service, Uri.UnescapeDataString(address)));
            }
        }


        private void NodeDisconnected(WatchedEvent @event)
        {
            _logger.LogInformation($"zookeeper path:{@event.getPath()} state:{@event.getState()} type:{@event.get_Type()}");
        }

        private void Connection(WatchedEvent @event)
        {
            switch(@event.get_Type())
            {
                case Watcher.Event.EventType.NodeChildrenChanged:
                    ChildrenChange(@event);
                    break;
            }
        }
        private void ChildrenChange(WatchedEvent @event)
        {
            var serviceName = @event.getPath().GetServiceNameByPath();
            if (string.IsNullOrEmpty(serviceName)) return;

            LoadService(serviceName);

        }

        private void NodeChange(WatchedEvent @event)
        {
            switch (@event.getState())
            {
                case Watcher.Event.KeeperState.Disconnected:
                    NodeDisconnected(@event);
                    break;
                case Watcher.Event.KeeperState.Expired:
                    LoadServices();
                    break;
                case Watcher.Event.KeeperState.SyncConnected:
                    Connection(@event);
                    break;
            }
        }


        private class SubscribeWatcher : Watcher
        {
            private readonly ZookeeperServiceDiscoveryProvider _provider;

            public SubscribeWatcher(ZookeeperServiceDiscoveryProvider provider)
            {
                this._provider = provider;
            }

            public override Task process(WatchedEvent @event)
            {
                return Task.Run(() =>
                {
                    this._provider.NodeChange(@event);
                });
            }
        }

    }



}
