using Rainbow.ServiceDiscovery.Abstractions;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Primitives;
using Consul;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading;

namespace Rainbow.ServiceDiscovery.Consul
{
    public class ConsulServiceDiscoveryProvider : IServiceDiscoveryProvider
    {
        private readonly IConsulClient _client;
        private ConsulServiceDiscoveryOptions _options;
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();
        private CancellationTokenSource _cts = new CancellationTokenSource();
        private IServiceDiscovery _serviceDiscovery;
        private SortedDictionary<string, List<IServiceEndpoint>> _cache = new SortedDictionary<string, List<IServiceEndpoint>>();
        private ILogger _logger;

        public ConsulServiceDiscoveryProvider(
            ConsulServiceDiscoverySource source,
            ILoggerFactory loggerFactory)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }
            //this._source = source;
            this._options = new ConsulServiceDiscoveryOptions(source.Configuration);
            //this._zkClient = new ZooKeeper(_options.Connection, (int)_options.SessionTimeout.TotalMilliseconds, new SubscribeWatcher(this));
            this._logger = loggerFactory.CreateLogger<ConsulServiceDiscoveryProvider>();

            //ChangeToken.OnChange(source.Configuration.GetReloadToken, RaiseChanged);

            _client = new ConsulClient(SetConsulConfig);
        }

        private void SetConsulConfig(ConsulClientConfiguration config)
        {
            config.Address = _options.Address;
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

            _serviceDiscovery = serviceDiscovery;
            Load();
        }

        private void Load()
        {
            if (this._options.IsRegister)
            {
                Create(_serviceDiscovery);
            }
            LoadServices();
        }

        private void Create(IServiceDiscovery serviceDiscovery)
        {
            var endpoint = serviceDiscovery.GetLocalEndpoint();
            var checkUri = endpoint.ToUri();
            var builder = new UriBuilder(checkUri);
            builder.Path = string.IsNullOrEmpty(endpoint.Path) || endpoint.Path == "/" ? _options.CheckPath : $"{builder.Path}/{_options.CheckPath}";
            var register = new AgentServiceRegistration()
            {
                Address = endpoint.HostName,
                Port = endpoint.Port,
                Name = endpoint.Name,
                ID = $"{endpoint.Name}-{endpoint.Protocol}-{endpoint.HostName}-{endpoint.Port}-{endpoint.Path}",
                Checks = new AgentServiceCheck[] {
                    new AgentServiceCheck{
                        HTTP=builder.Uri.ToString(),
                        Interval=new TimeSpan(0,0,10),
                        Timeout=new TimeSpan(0,0,1),
                    }
                }
            };
            var result = this._client.Agent.ServiceRegister(register, _cts.Token).GetAwaiter().GetResult();
        }

        private void LoadServices()
        {
            var result = this._client.Agent.Services().GetAwaiter().GetResult();
            foreach (var item in result.Response.Values)
            {
                this._logger.LogInformation($"{item.Address}");
            }
        }



    }
}
