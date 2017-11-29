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
        private ZookeeperServiceDiscoverySource _source;
        private ZooKeeper _zkClient;
        private SortedDictionary<string, IEnumerable<IServiceEndpoint>> _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();
        private ILogger _logger;
        private ILoggerFactory _loggerFactory;
        private ZookeeperServiceDiscoveryOptions _options;
        private IServiceDiscovery _serviceDiscovery;

        public ZookeeperServiceDiscoveryProvider(
            ZookeeperServiceDiscoverySource source,
            ILoggerFactory loggerFactory)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            if (loggerFactory == null)
            {
                throw new ArgumentNullException(nameof(loggerFactory));
            }
            this._source = source;
            this._options = new ZookeeperServiceDiscoveryOptions(source.Configuration);
            this._zkClient = new ZooKeeper(_options.Connection, (int)_options.SessionTimeout.TotalMilliseconds, new ZookeeperSubscribeWatcher(this, loggerFactory));
            this._logger = loggerFactory.CreateLogger<ZookeeperServiceDiscoveryProvider>();
            this._loggerFactory = loggerFactory;
            ChangeToken.OnChange(source.Configuration.GetReloadToken, RaiseChanged);
        }

        private void RaiseChanged()
        {
            this._options = new ZookeeperServiceDiscoveryOptions(_source.Configuration);
            this._zkClient.closeAsync().GetAwaiter().GetResult();
            this._zkClient = new ZooKeeper(_options.Connection, (int)_options.SessionTimeout.TotalMilliseconds, new ZookeeperSubscribeWatcher(this, this._loggerFactory));
            Initialize();
        }

        public bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints)
        {
            return _cache.TryGetValue(serviceName, out endpoints);
        }


        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }


        public void Load(IServiceDiscovery serviceDiscovery)
        {
            InitializeParam(serviceDiscovery);
            Initialize();
        }

        private void InitializeParam(IServiceDiscovery serviceDiscovery)
        {
            _serviceDiscovery = serviceDiscovery;
        }

        public virtual void Initialize()
        {
            HandleRegister();
            HandleDiscovery();
        }

        protected virtual void HandleRegister()
        {
            if (this._options.IsRegister)
            {
                Create(_serviceDiscovery);
            }
        }

        public virtual void HandleDiscovery()
        {
            var path = $"/{ZookeeperDefaults.NameService}";
            try
            {
                SortedDictionary<string, IEnumerable<IServiceEndpoint>> cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();

                var node = this._zkClient.getChildrenAsync(path, false).GetAwaiter().GetResult();

                foreach (var item in node.Children)
                {
                    var endpoints = GetZookeeperServiceEndpoints(item);
                    cache.Add(item, endpoints);
                }

                _cache = cache;
            }
            catch (KeeperException.NoNodeException noex)
            {
                _logger.LogInformation(noex, $"{path} - 节点不存在，无法获取子节点，执行创建节点");

                this._zkClient.createAsync(path, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
                HandleDiscovery();
            }
        }

        protected virtual void Create(IServiceDiscovery serviceDiscovery)
        {
            var endpoint = serviceDiscovery.GetLocalEndpoint();
            var path = endpoint.ToPath();
            try
            {
                var result = this._zkClient.createAsync(path, null, Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL).GetAwaiter().GetResult();
            }
            catch (KeeperException.NoNodeException noex)
            {
                _logger.LogInformation(noex, $"{path} - 不能创建节点，目录不存在,执行创建目录");

                CreateNode($"/{ZookeeperDefaults.NameService}");
                CreateNode(endpoint.ToDirectory());
                Create(serviceDiscovery);
            }
            catch (KeeperException.NodeExistsException existsex)
            {
                _logger.LogInformation(existsex, $"{path} - 节点已存在,执行删除节点并重新创建");

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
            if (this._zkClient.existsAsync(node).GetAwaiter().GetResult() == null)
            {
                this._zkClient.createAsync(node, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
            }
        }



        protected virtual List<IServiceEndpoint> GetZookeeperServiceEndpoints(string service)
        {
            List<IServiceEndpoint> endpoints = new List<IServiceEndpoint>();

            var serviceDir = service.GetServiceDirectory();
            var pathChildren = this._zkClient.getChildrenAsync(serviceDir, true).GetAwaiter().GetResult();
            foreach (var address in pathChildren.Children)
            {
                endpoints.Add(new ServiceEndpoint(service, Uri.UnescapeDataString(address)));
            }
            return endpoints;
        }

        public virtual void LoadService(string service)
        {
            if (!_cache.ContainsKey(service))
            {
                _cache.Add(service, new List<IServiceEndpoint>());
            }
            var endpoints = GetZookeeperServiceEndpoints(service);
            _cache[service] = endpoints;
        }



    }



}
