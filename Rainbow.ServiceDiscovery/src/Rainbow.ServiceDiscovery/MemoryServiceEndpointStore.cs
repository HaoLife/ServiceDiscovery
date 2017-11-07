using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.ServiceDiscovery
{
    public class MemoryServiceEndpointStore : IServiceEndpointStore
    {
        private readonly ConcurrentDictionary<string, List<ServiceEndpoint>> _caches;
        private readonly Task _fileSyncTask;
        private long _currentTicks;
        private long _syncTicks;
        public MemoryServiceEndpointStore()
        {
            this._caches = new ConcurrentDictionary<string, List<ServiceEndpoint>>();
            this._fileSyncTask = BuildFileSyncTask();
        }

        private Task BuildFileSyncTask()
        {
            Task task = new Task(FileSync, TaskCreationOptions.LongRunning);
            task.Start();
            return task;
        }



        //文件同步
        public void FileSync()
        {
            while (true)
            {
                Thread.Sleep(1000);
                if (_currentTicks > _syncTicks)
                {
                    var temp = _currentTicks;
                    var time = DateTime.Now.Ticks;
                    WriteFile();

                    _syncTicks = temp;
                }
            }
        }

        private void WriteFile()
        {
            var file = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "endpoint.cache");
            StringBuilder sb = new StringBuilder();
            foreach (var item in this._caches)
            {
                sb.AppendLine("service:" + item.Key);
                foreach (var endpoint in item.Value)
                {
                    sb.AppendLine(endpoint.Endpoint.ToUri().ToString());
                }
                sb.AppendLine("");
            }

            File.WriteAllText(file, sb.ToString());
        }

        private void TriggerChange()
        {
            this._currentTicks = DateTime.Now.Ticks;
        }

        public void Set(string serviceName, IEnumerable<ServiceEndpoint> endpoints)
        {
            _caches.AddOrUpdate(serviceName, endpoints.ToList(), (key, value) => endpoints.ToList());
            TriggerChange();
        }

        public IEnumerable<ServiceEndpoint> Get(string serviceName)
        {
            return _caches.GetOrAdd(serviceName, (key) => new List<ServiceEndpoint>());
        }

        public void Delete(string serviceName)
        {
            List<ServiceEndpoint> values;
            if (_caches.TryGetValue(serviceName, out values))
            {
                values.Clear();
                TriggerChange();
            }
        }
    }
}
