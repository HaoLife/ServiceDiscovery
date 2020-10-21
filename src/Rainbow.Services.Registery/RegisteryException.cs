using System;
using System.Collections.Generic;
using System.Text;
using System.Transactions;

namespace Rainbow.Services.Registery
{
    public class RegisteryException : Exception
    {
        public RegisteryException(IServiceRegisteryProvider provider, string message)
            : base($"{provider.GetType().FullName} - {message}")
        {

        }
    }
}
