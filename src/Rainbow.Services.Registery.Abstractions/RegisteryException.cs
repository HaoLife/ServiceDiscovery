using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public class RegisteryException : Exception
    {
        public RegisteryException()
        {

        }
        public RegisteryException(string message) : base(message)
        {

        }
    }
}
