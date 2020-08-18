using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Rainbow.Services.Registery.Consul
{
    public class ConsulServiceRegisteryProvider : IServiceRegisteryProvider
    {
        private readonly IServiceProvider provider;
        private readonly ConsulServiceRegisterySource _source;
        private ConsulClient _client;
        private ConsulServiceRegisteryOptions _options;

        public ConsulServiceRegisteryProvider(
            IServiceProvider provider,
            ConsulServiceRegisterySource source)
        {
            this.provider = provider;
            this._source = source;

            this.Load();
        }

        public void Load()
        {
            var client = _client;
            this._options = new ConsulServiceRegisteryOptions(_source.Configuration);
            this._client =  new ConsulClient(SetConsulConfig);
            client?.Dispose();
        }

        private void SetConsulConfig(ConsulClientConfiguration config)
        {
            config.Address = _options.Address;
        }

        public IChangeToken GetReloadToken()
        {
            return _source.Configuration.GetReloadToken();
        }


        public void Register(IServiceApplication application)
        {
            var register = new AgentServiceRegistration()
            {
                Address = application.Host,
                Port = application.Port,
                Name = application.Name,
                ID = $"{application.Name}-{application.Protocol}-{application.Host}-{application.Port}",
                Tags = new string[] {
                    $"{application.Name}",
                },
                Meta = new Dictionary<string, string>() {
                    { ConsulDefaults.Protocol, application.Protocol },
                    { ConsulDefaults.Path, application.Path }
                },
            };
            if (_options.Check)
            {
                register.Check = new AgentServiceCheck
                {
                    HTTP = Path.Combine($"{application.Protocol}://{application.Host}:{application.Port}{application.Path}", _options.CheckPath),
                    Interval = _options.CheckInterval,
                    Timeout = _options.CheckTimeout,
                };
            }


            var result = this._client.Agent.ServiceRegister(register).GetAwaiter().GetResult();

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new RegisteryException($"注册失败:{result.StatusCode}");
            }
        }

        public void Deregister(IServiceApplication application)
        {
            var id = $"{application.Name}-{application.Protocol}-{application.Host}-{application.Port}";
            var result = this._client.Agent.ServiceDeregister(id).GetAwaiter().GetResult();

        }

    }
}
