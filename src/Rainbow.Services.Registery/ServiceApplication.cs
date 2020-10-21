using System;
using System.Collections.Generic;
using System.Text;

namespace Rainbow.Services.Registery
{
    public class ServiceApplication : IServiceApplication
    {
        public string Name { get; set; }
        public string Host { get; set; }
        public string Path { get; set; }
        public int Port { get; set; }
        public string Protocol { get; set; }


        public static ServiceApplication Default()
        {
            return new ServiceApplication()
            {

                Name = System.Net.Dns.GetHostName(),
                Host = GetHostAddresss(),
                Protocol = "http",
                Port = 5000,
            };
        }


        private static string GetHostAddresss()
        {
            string hostName = System.Net.Dns.GetHostName();
            var task = System.Net.Dns.GetHostAddressesAsync(hostName);
            task.Wait();

            string address = string.Empty;
            if (task.Result != null && task.Result.Length > 0)
            {
                foreach (var result in task.Result)
                {
                    if (result.AddressFamily.Equals(System.Net.Sockets.AddressFamily.InterNetwork))
                    {
                        address = result.ToString();
                        break;
                    }
                }
            }

            return address;
        }

    }
}
