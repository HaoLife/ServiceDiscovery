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
        private IConsulClient _client;
        private ConsulServiceDiscoveryOptions _options;
        private ServiceDiscoveryReloadToken _reloadToken = new ServiceDiscoveryReloadToken();
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
            this._options = new ConsulServiceDiscoveryOptions(source.Configuration);
            this._logger = loggerFactory.CreateLogger<ConsulServiceDiscoveryProvider>();

            ChangeToken.OnChange(source.Configuration.GetReloadToken, RaiseChanged);

            _client = new ConsulClient(SetConsulConfig);

        }

        private void RaiseChanged()
        {
            _client.Dispose();
            _client = new ConsulClient(SetConsulConfig);
            Load();
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
                Tags = new string[] {
                    $"{ConsulDefaults.Path}-{endpoint.Path}",
                    $"{ConsulDefaults.Protocol}-{endpoint.Protocol}"
                },
                Check = new AgentServiceCheck
                {
                    HTTP = builder.Uri.ToString(),
                    Interval = _options.CheckInterval,
                    Timeout = _options.CheckTimeout,
                }
            };
            var result = this._client.Agent.ServiceRegister(register).GetAwaiter().GetResult();

            _logger.LogDebug($"执行注册服务:{Newtonsoft.Json.JsonConvert.SerializeObject(_options)}");

        }

        private void LoadServices()
        {
            var catalog = this._client.Catalog.Services().GetAwaiter().GetResult();
            foreach (var item in catalog.Response.Where(a => a.Key != "consul"))
            {
                LoadService(item.Key);
            }

        }

        private void LoadService(string service)
        {
            List<IServiceEndpoint> endpoints = new List<IServiceEndpoint>();
            var serviceEntry = this._client.Health.Service(service, null, true).GetAwaiter().GetResult();

            foreach (var item in serviceEntry.Response)
            {
                if (!item.Checks.All(a => a.Status.Status == ConsulDefaults.HealthStatusPassing)) continue;

                var protocol = ConsulDefaults.ProtocolValue;
                var path = ConsulDefaults.PathValue;
                if (item.Service.Tags.Where(a => a.StartsWith($"{ConsulDefaults.Path}-")).Any())
                {
                    protocol = item.Service.Tags.Where(a => a.StartsWith($"{ConsulDefaults.Path}-")).FirstOrDefault().Substring(ConsulDefaults.Path.Length + 1);
                }
                if (item.Service.Tags.Where(a => a.StartsWith($"{ConsulDefaults.PathValue}-")).Any())
                {
                    path = item.Service.Tags.Where(a => a.StartsWith($"{ConsulDefaults.PathValue}-")).FirstOrDefault().Substring(ConsulDefaults.Path.Length + 1);
                }

                endpoints.Add(new ServiceEndpoint(item.Service.Service, protocol, item.Service.Address, item.Service.Port, path));
            }

            if (_cache.ContainsKey(service))
            {
                _cache[service] = endpoints;
            }
            else
            {
                _cache.Add(service, endpoints);
            }
        }






    }
}
