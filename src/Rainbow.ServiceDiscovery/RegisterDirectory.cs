using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Rainbow.ServiceDiscovery
{
    public class RegisterDirectory : IRegisterDirectory
    {
        private readonly IServiceRegisterFactory _registerFactory;
        private readonly List<IServiceRegister> _serviceRegisters;

        public RegisterDirectory(IServiceRegisterFactory registerFactory)
        {
            this._registerFactory = registerFactory;
            this._serviceRegisters = new List<IServiceRegister>();
        }

        public void Add(ServiceEndpoint endpoint)
        {
            if (_serviceRegisters.Any(a => a.ServiceName == endpoint.Name))
            {
                throw new Exception("重复添加注册节点:" + endpoint.Name);
            }
            var register = _registerFactory.CreateRegister(endpoint);
            this._serviceRegisters.Add(register);
        }

        public IEnumerable<IServiceRegister> GetRegisters()
        {
            return this._serviceRegisters;
        }
    }
}
