using org.apache.zookeeper;
using org.apache.zookeeper.data;
using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static org.apache.zookeeper.Watcher.Event;
using static org.apache.zookeeper.ZooDefs;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperRegistryClient : IZookeeperRegistryClient
    {
        private readonly List<ServiceEndpoint> _registerEndpoints;
        private readonly int _sleepMilliseconds = 10000;
        private ZooKeeper _zkClient;
        private readonly IDictionary<string, ServiceDiscoveryReloadToken> _changeTokens = new SortedDictionary<string, ServiceDiscoveryReloadToken>(StringComparer.OrdinalIgnoreCase);

        private readonly ZookeeperServiceDiscoverySource _source;
        public ZookeeperRegistryClient(ZookeeperServiceDiscoverySource source)
        {
            this._source = source;
            this._zkClient = this.BuildZkClient();
            this._registerEndpoints = new List<ServiceEndpoint>();

        }



        public void Publish(ServiceEndpoint endpoint)
        {
            this.Create(endpoint);
            _registerEndpoints.Add(endpoint);
        }

        public void Unpublish(ServiceEndpoint endpoint)
        {
            this.Delete(endpoint);
            _registerEndpoints.RemoveAll(a => a.Name == endpoint.Name);
        }

        public IChangeToken GetReloadToken(string serviceName)
        {

            var directory = serviceName.GetServiceDirectory();

            //订阅变更，如果没有节点，直接抛出异常
            this._zkClient.getChildrenAsync(directory, true).GetAwaiter().GetResult();

            ServiceDiscoveryReloadToken value = new ServiceDiscoveryReloadToken();
            if (_changeTokens.ContainsKey(serviceName))
            {
                _changeTokens[serviceName] = value;
            }
            else
            {
                _changeTokens.Add(serviceName, value);
            }

            return value;
        }

        public IEnumerable<ServiceEndpoint> GetChildren(string serviceName)
        {
            var directory = serviceName.GetServiceDirectory();
            List<ServiceEndpoint> serviceEndpoints = new List<ServiceEndpoint>();

            try
            {

                var children = this._zkClient.getChildrenAsync(directory).GetAwaiter().GetResult();
                foreach (var item in children.Children)
                {
                    var uri = new Uri(Uri.UnescapeDataString(item));
                    ServiceEndpoint se = new ServiceEndpoint(serviceName, uri);
                    serviceEndpoints.Add(se);
                }
            }
            catch (KeeperException.NodeExistsException zkExistsEx)
            {
                //如果节点存在，不处理
            }
            catch (KeeperException.ConnectionLossException zkConnLossEx)
            {
                //连接需要准备时间，在未连接成功前使用会抛出这个异常，进行休眠一会重连吧
                System.Threading.Thread.Sleep(300);
                return GetChildren(serviceName);
            }
            catch (Exception ex)
            {
            }

            return serviceEndpoints;
        }


        private ZooKeeper BuildZkClient()
        {
            return new ZooKeeper(_source.Connection, (int)_source.SessionTimeout.TotalMilliseconds, new SubscribeWatcher(this));
        }



        public void Create(ServiceEndpoint endpoint)
        {
            var path = endpoint.ToPath();
            try
            {
                _zkClient.createAsync(path, null, Ids.OPEN_ACL_UNSAFE, CreateMode.EPHEMERAL).GetAwaiter().GetResult();
            }
            catch (KeeperException.NoNodeException zkNoEx)
            {
                //创建父节点
                var nodes = endpoint.ToPathNodes();
                CreateNodes(nodes);
                Create(endpoint);
            }
            catch (KeeperException.ConnectionLossException zkConnLossEx)
            {
                //连接需要准备时间，在未连接成功前使用会抛出这个异常，进行休眠一会重连吧
                System.Threading.Thread.Sleep(300);
                Create(endpoint);
            }
            catch (KeeperException.NodeExistsException zkExistsEx)
            {
                //如果节点存在，先删除再注册一个，重启时会出现识别延迟的问题，而不重新注册的话，会停掉该节点的。
                _zkClient.deleteAsync(path, -1).GetAwaiter().GetResult();
                Create(endpoint);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Delete(ServiceEndpoint endpoint)
        {

            var path = endpoint.ToPath();
            try
            {
                _zkClient.deleteAsync(path);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        public void ChildrenChange(WatchedEvent @event)
        {
            var serviceName = @event.getPath().GetServiceNameByPath();

            if (string.IsNullOrEmpty(serviceName)) return;

            ServiceDiscoveryReloadToken token;

            if (!_changeTokens.TryGetValue(serviceName, out token)) return;

            token.OnReload();

        }



        public void CreateNodes(IEnumerable<string> nodes)
        {
            foreach (var item in nodes)
            {
                Stat stat = _zkClient.existsAsync(item, false).GetAwaiter().GetResult();
                if (stat != null) continue;
                _zkClient.createAsync(item, null, Ids.OPEN_ACL_UNSAFE, CreateMode.PERSISTENT).GetAwaiter().GetResult();
            }

        }

        private void NodeDisconnection(WatchedEvent @event)
        {
            //写入日志吧
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            Directory.CreateDirectory(dir);

            var file = Path.Combine(dir, "zookeeper.log");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("datetime:" + DateTime.Now.ToString());
            sb.AppendLine("path:" + @event.getPath());
            sb.AppendLine("state:" + @event.getState());
            sb.AppendLine("type:" + @event.get_Type());
            sb.AppendLine("");

            File.AppendAllText(file, sb.ToString());
        }

        private void NodeExpire(WatchedEvent @event)
        {
            this._zkClient = this.BuildZkClient();
            System.Threading.Thread.Sleep(300);

            foreach (var item in this._registerEndpoints)
            {
                this.Create(item);
            }

            foreach (var token in _changeTokens.Values)
            {
                token.OnReload();
            }
        }

        private void Connection(WatchedEvent @event)
        {
            switch (@event.get_Type())
            {
                case EventType.NodeChildrenChanged:
                    ChildrenChange(@event);
                    break;
            }

        }

        public void NodeChangeNotice(WatchedEvent @event)
        {
            switch (@event.getState())
            {
                case KeeperState.Disconnected:
                    NodeDisconnection(@event);
                    break;
                case KeeperState.Expired:
                    NodeExpire(@event);
                    break;
                case KeeperState.SyncConnected:
                    Connection(@event);
                    break;
            }

        }



        private class SubscribeWatcher : Watcher
        {
            private readonly ZookeeperRegistryClient _client;
            public SubscribeWatcher(ZookeeperRegistryClient client)
            {
                this._client = client;
            }

            public override Task process(WatchedEvent @event)
            {
                return Task.Run(() =>
                {
                    this._client.NodeChangeNotice(@event);
                });
            }
        }



    }
}
