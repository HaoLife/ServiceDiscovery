using Consul;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Rainbow.Services.Discovery.Consul
{
    public class ConsulServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private ConsulClient _client;
        private ConsulServiceDiscoveryOptions _options;
        private ConsulServiceDiscoverySource _source;
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();
        private SortedDictionary<string, IEnumerable<IServiceEndpoint>> _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();
        private ILogger _logger;

        private ulong lastIndex = 0L;

        public ConsulServiceDiscoveryProvider(
            ConsulServiceDiscoverySource source,
            ILoggerFactory loggerFactory)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            this._source = source;
            this._options = new ConsulServiceDiscoveryOptions(source.Configuration);
            this._logger = loggerFactory.CreateLogger<ConsulServiceDiscoveryProvider>();

            ChangeToken.OnChange(source.Configuration.GetReloadToken, RaiseChanged);

            _client = new ConsulClient(SetConsulConfig);

        }


        private void RaiseChanged()
        {
            this._options = new ConsulServiceDiscoveryOptions(this._source.Configuration);
            this._client = new ConsulClient(SetConsulConfig);
            this._reloadToken.OnReload();
        }

        private void SetConsulConfig(ConsulClientConfiguration config)
        {
            config.Address = _options.Address;
        }

        public bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints)
        {
            return _cache.TryGetValue(serviceName, out endpoints);
        }

        public IChangeToken GetReloadToken()
        {
            return _reloadToken;
        }

        private void LoadData()
        {
            _reloadToken = new ServiceDiscoveryReloadToken();
            LoadServices();

            //检测是否变更，如果变更则token取消，重新加载
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        var ck = GetConsuleHealth();
                        _logger.LogDebug($"get health {ck.LastIndex}");
                        if (this.lastIndex != ck.LastIndex)
                        {
                            _logger.LogInformation($"reload get index {ck.LastIndex} current {this.lastIndex}");
                            _reloadToken.OnReload();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"get health error : {ex.Message}");
                        Thread.Sleep(Convert.ToInt32(this._options.WaitTime.TotalMilliseconds));
                    }


                }
            }, TaskCreationOptions.LongRunning);
        }


        public void Load()
        {
            if (this._source.IsAsync)
            {
                this.AsyncLoadData();
            }
            else
            {
                this.LoadData();
            }
        }
        private void AsyncLoadData()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    this.LoadData();
                }
                catch (Exception ex)
                {
                    Thread.Sleep(Convert.ToInt32(this._options.WaitTime.TotalMilliseconds));
                    this.AsyncLoadData();
                }

            });
        }


        private QueryResult<HealthCheck[]> GetConsuleHealth()
        {
            var query = new QueryOptions()
            {
                WaitIndex = this.lastIndex,
                WaitTime = this._options.WaitTime
            };

            var ck = this._client.Health
                .State(HealthStatus.Passing, query)
                .GetAwaiter()
                .GetResult();

            return ck;

        }

        private void LoadServices()
        {
            _logger.LogInformation("load consul discovery");
            var ck = GetConsuleHealth();

            _logger.LogInformation($"load consul index { ck.LastIndex}");

            if (ck.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new DiscoveryException($"consul load error status code :{ck.StatusCode.GetHashCode()}");
            }

            this.lastIndex = ck.LastIndex;


            var ckServices = ck.Response.Where(a => !string.IsNullOrEmpty(a.ServiceID)).ToDictionary(a => a.ServiceID);


            var servicesResponse = this._client.Agent.Services().GetAwaiter().GetResult();
            if (servicesResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new DiscoveryException($"get services error {servicesResponse.StatusCode}");
            }

            List<IServiceEndpoint> endpoints = new List<IServiceEndpoint>();
            foreach (var item in servicesResponse.Response)
            {
                if (!ckServices.ContainsKey(item.Key)) continue;

                endpoints.Add(new ConsulServiceEndpoint(item.Value));

            }

            _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>(endpoints.GroupBy(a => a.Name).ToDictionary(a => a.Key, b => b.AsEnumerable()));

        }


    }
}
