using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Rainbow.ServiceDiscovery.Routes;

namespace Rainbow.ServiceDiscovery.Formatters
{
    public class InvokeInputFormaterContext : IInputFormatterContext
    {
        private readonly ServiceInvokeContext _invokeContext;
        private readonly string _contentType;
        private readonly string[] _ignoreParams;
        public InvokeInputFormaterContext(RouteContext routeContext)
        {
            this._invokeContext = routeContext.InvokeContext;
            this._contentType = routeContext.ContentType;
            this._ignoreParams = routeContext.IgnoreParams.ToArray();
            Init();
        }

        private void Init()
        {
            var paramters = this._invokeContext.Method.GetParameters();
            List<ParameterInfo> resultParamters = new List<ParameterInfo>();
            List<object> resultArgs = new List<object>();
            for (int i = 0; i < paramters.Length; i++)
            {
                if (!this._ignoreParams.Contains(paramters[i].Name))
                {
                    resultArgs.Add(this._invokeContext.Args[i]);
                    resultParamters.Add(paramters[i]);
                }
            }
            this.Args = resultArgs.ToArray();
            this.Paramters = resultParamters.ToArray();
        }

        public object[] Args { get; private set; }
        public System.Reflection.ParameterInfo[] Paramters { get; private set; }


        public string ContentType
        {
            get { return this._contentType; }
        }

        public string Result { get; set; }
    }
}
