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

namespace Rainbow.ServiceDiscovery.Zookeeper
{
    public class ZookeeperRegistryClient : IZookeeperRegistryClient
    {
        private readonly List<ZookeeperSubscribeNotice> _subscribeFailureNotices;
        private readonly List<ZookeeperSubscribeNotice> _subscribeSuccessNotices;
        private readonly List<ServiceEndpoint> _registerEndpoints;
        private readonly Task _retryTask;
        private readonly int _sleepMilliseconds = 10000;
        //private readonly string _connection;
        //private readonly int _sessionTimeout;
        private ZooKeeper _zkClient;

        private readonly ZookeeperServiceDiscoverySource _source;

        //public ZookeeperRegistryClient(string connection, TimeSpan sessionTimeout)
        //{
        //    this._connection = connection;
        //    this._sessionTimeout = (int)sessionTimeout.TotalMilliseconds;
        //    this._zkClient = this.BuildZkClient();
        //    this._subscribeFailureNotices = new List<ZookeeperSubscribeNotice>();
        //    this._subscribeSuccessNotices = new List<ZookeeperSubscribeNotice>();
        //    this._registerEndpoints = new List<ServiceEndpoint>();
        //    this._retryTask = BuildRetryTask();
        //}
        public ZookeeperRegistryClient(ZookeeperServiceDiscoverySource source)
        {
            this._source = source;
            this._zkClient = this.BuildZkClient();
            this._subscribeFailureNotices = new List<ZookeeperSubscribeNotice>();
            this._subscribeSuccessNotices = new List<ZookeeperSubscribeNotice>();
            this._registerEndpoints = new List<ServiceEndpoint>();
            this._retryTask = BuildRetryTask();

        }

        private ZooKeeper BuildZkClient()
        {
            return new ZooKeeper(_source.Options.Connection, (int)_source.Options.SessionTimeout.TotalMilliseconds, new SubscribeWatcher(this));
        }

        private Task BuildRetryTask()
        {
            Task task = new Task(() =>
            {
                while (true)
                {
                    Thread.Sleep(_sleepMilliseconds);
                    FailureRetry();
                }
            }, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }


        //失败重试
        public void FailureRetry()
        {
            if (!this._subscribeFailureNotices.Any()) return;

            var items = this._subscribeFailureNotices.ToArray();

            foreach (var item in items)
            {
                try
                {
                    var values = this.GetChildren(item.ServiceName);
                    item.Handler(values);
                    //成功后清除,并添加到成功通知
                    this._subscribeFailureNotices.Remove(item);
                    this._subscribeSuccessNotices.Add(item);
                }
                catch (Exception ex)
                {
                    //失败无视
                }
            }

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




        public IEnumerable<ServiceEndpoint> GetChildren(string serviceName)
        {
            var directory = serviceName.GetServiceDirectory();
            List<ServiceEndpoint> serviceEndpoints = new List<ServiceEndpoint>();

            var children = this._zkClient.getChildrenAsync(directory, true).GetAwaiter().GetResult();
            foreach (var item in children.Children)
            {
                var uri = new Uri(Uri.UnescapeDataString(item));
                ServiceEndpoint se = new ServiceEndpoint(serviceName, uri);
                serviceEndpoints.Add(se);
            }
            return serviceEndpoints;
        }

        public void ChildrenChange(WatchedEvent @event)
        {
            var serviceName = @event.getPath().GetServiceNameByPath();

            if (string.IsNullOrEmpty(serviceName)) return;

            var notice = this._subscribeSuccessNotices.FirstOrDefault(a => a.ServiceName == serviceName);
            if (notice == null)
                return;

            try
            {
                var values = this.GetChildren(notice.ServiceName);
                notice.Handler(values);
            }
            catch (KeeperException.ConnectionLossException zkConnLossEx)
            {
                //session 超时了，客户端已经关闭照成的，不要做处理，之后由超时事件做通知，重新创建client，并重新注册和订阅
            }
            catch (Exception ex)
            {
                WriteLog(@event, ex);
            }


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

        private void WriteLog(WatchedEvent @event, Exception ex = null)
        {
            var dir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "log");
            Directory.CreateDirectory(dir);

            var file = Path.Combine(dir, "zookeeper.log");

            StringBuilder sb = new StringBuilder();

            sb.AppendLine("datetime:" + DateTime.Now.ToString());
            sb.AppendLine("path:" + @event.getPath());
            sb.AppendLine("state:" + @event.getState());
            sb.AppendLine("type:" + @event.get_Type());
            if (ex != null)
            {
                sb.AppendLine(ex.Message);
                sb.AppendLine(ex.StackTrace);
            }
            sb.AppendLine("");

            File.AppendAllText(file, sb.ToString());

        }

        private void NodeDisconnection(WatchedEvent @event)
        {
            //写入日志吧
            WriteLog(@event);
        }

        private void NodeExpire(WatchedEvent @event)
        {
            this._zkClient = this.BuildZkClient();
            System.Threading.Thread.Sleep(300);

            foreach (var item in this._registerEndpoints)
            {
                this.Create(item);
            }

            foreach (var item in this._subscribeSuccessNotices)
            {
                var values = this.GetChildren(item.ServiceName);
                item.Handler(values);
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


        public IEnumerable<ServiceEndpoint> Subscribe(ZookeeperSubscribeNotice subscribeNotice)
        {
            try
            {
                var endpoints = this.GetChildren(subscribeNotice.ServiceName);
                _subscribeSuccessNotices.Add(subscribeNotice);
                return endpoints;
            }
            catch (Exception ex)
            {
                _subscribeFailureNotices.Add(subscribeNotice);
                return new List<ServiceEndpoint>();
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
