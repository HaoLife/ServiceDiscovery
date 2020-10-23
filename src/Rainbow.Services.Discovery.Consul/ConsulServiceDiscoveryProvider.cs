using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
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
        private ulong lastIndex = 0L;
        private ConsulClient _client;
        private readonly ConsulServiceDiscoverySource source;
        private readonly IServiceProvider services;
        private ConsulServiceDiscoveryOptions _options;
        private SortedDictionary<string, IEnumerable<IServiceEndpoint>> _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>();
        private ILogger _logger;

        public ConsulServiceDiscoveryProvider(ConsulServiceDiscoverySource source, IServiceProvider services)
        {
            this.source = source;
            this.services = services;
            this._logger = services.GetRequiredService<ILoggerFactory>().CreateLogger<ConsulServiceDiscoveryProvider>();

            if (source.ReloadOnChange)
            {
                ChangeToken.OnChange(() => source.Configuration.GetReloadToken(), () => this.Load());
            }
        }

        public void Load()
        {
            this._client?.Dispose();
            this._options = this.source.Configuration.Get<ConsulServiceDiscoveryOptions>();
            this._client = new ConsulClient(SetConsulConfig);

            CatchLoadServices();
            Listening();
        }

        private void SetConsulConfig(ConsulClientConfiguration config)
        {
            config.Address = _options.Address;
        }

        public bool TryGetEndpoints(string serviceName, out IEnumerable<IServiceEndpoint> endpoints)
        {
            return _cache.TryGetValue(serviceName, out endpoints);
        }

        private void Listening()
        {
            //检测是否变更，如果变更则token取消，重新加载
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    try
                    {
                        var ck = GetConsuleHealth();
                        _logger.LogDebug($"listening health index :{ck.LastIndex}");
                        if (this.lastIndex != ck.LastIndex)
                        {
                            _logger.LogInformation($"reload services to index : {ck.LastIndex} current index {this.lastIndex}");
                            LoadServices();
                            break;
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"listening health error : {ex.Message}");
                        Thread.Sleep(Convert.ToInt32(this._options.WaitTime.TotalMilliseconds));
                    }


                }
            }, TaskCreationOptions.LongRunning);
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

        private void CatchLoadServices()
        {
            try
            {
                LoadServices();
            }
            catch (Exception ex)
            {
                if (this.source.IsAsync)
                {
                    _logger.LogInformation(ex.Message);
                    return;
                }
                throw ex;
            }
        }

        private void LoadServices()
        {
            var ck = GetConsuleHealth();


            if (ck.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new DiscoveryException(this, $"consul load error status code :{ck.StatusCode.GetHashCode()}");
            }

            var ckServices = ck.Response.Where(a => !string.IsNullOrEmpty(a.ServiceID)).ToDictionary(a => a.ServiceID);


            var servicesResponse = this._client.Agent.Services().GetAwaiter().GetResult();
            if (servicesResponse.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new DiscoveryException(this, $"get services error {servicesResponse.StatusCode}");
            }

            List<IServiceEndpoint> endpoints = new List<IServiceEndpoint>();
            foreach (var item in servicesResponse.Response)
            {
                if (!ckServices.ContainsKey(item.Key)) continue;

                endpoints.Add(new ConsulServiceEndpoint(item.Value));

            }
            _cache = new SortedDictionary<string, IEnumerable<IServiceEndpoint>>(endpoints.GroupBy(a => a.Name).ToDictionary(a => a.Key, b => b.AsEnumerable()));

            this.lastIndex = ck.LastIndex;
        }

    }
}
