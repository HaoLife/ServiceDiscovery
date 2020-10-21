using Consul;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Rainbow.Services.Registery.Consul
{
    public class ConsulServiceRegisteryProvider : IServiceRegisteryProvider
    {
        private readonly IServiceRegisteryBuilder builder;
        private readonly ConsulServiceRegisterySource source;
        private ConsulClient _client;

        public ConsulServiceRegisteryProvider(IServiceRegisteryBuilder builder, ConsulServiceRegisterySource source)
        {
            this.builder = builder;
            this.source = source;
        }

        public void Deregister()
        {
            var application = builder.Application;
            var id = $"{application.Name}-{application.Protocol}-{application.Host}-{application.Port}";
            var result = this._client.Agent.ServiceDeregister(id).GetAwaiter().GetResult();
        }

        public void Register()
        {
            this._client?.Dispose();

            this._client = new ConsulClient(SetConsulConfig);

            var application = builder.Application;
            var options = this.source.Options;


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
                    { ConsulDefaults.Protocol,application.Protocol },
                    { ConsulDefaults.Path, application.Path }
                },
            };
            var checks = new List<GrpcAgentServiceCheck>();
            if (options.HttpCheck)
            {
                checks.Add(new GrpcAgentServiceCheck
                {
                    HTTP = application.ToUrl(options.CheckPath),
                    Interval = options.CheckInterval,
                    Timeout = options.CheckTimeout,
                }
                );
            }
            if (options.GrpcCheck)
            {
                var host = string.IsNullOrEmpty(options.GrpcHost) ? application.Host : options.GrpcHost;
                // grpc 
                checks.Add(new GrpcAgentServiceCheck()
                {
                    Grpc = $"{host}:{application.Port}{application.Path}",
                    GRPCUseTLS = options.GrpcTls,
                    Interval = options.CheckInterval,
                    Timeout = options.CheckTimeout,
                });
            }

            register.Checks = checks.ToArray();


            var result = this._client.Agent.ServiceRegister(register).GetAwaiter().GetResult();

            if (result.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new RegisteryException(this, $"注册失败:{result.StatusCode}");
            }


        }
        private void SetConsulConfig(ConsulClientConfiguration config)
        {
            config.Address = source.Options.Address;
        }
    }
}
