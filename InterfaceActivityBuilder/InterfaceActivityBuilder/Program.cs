using System;
using System.Configuration;

namespace InterfaceActivityBuilder
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = "C:\\Users\\ezozkme\\Desktop\\Book2.xlsx";

            string proxyAssemblyName = ConfigurationSettings.AppSettings["ProxyAssemblyName"] ?? throw new Exception("ProxyAssemblyName configuration key not found in App.Config file.");

            ICanonicResolver resolver = new ClaroCanonicResolver(path);
            resolver.Resolve();
        }
    }
}
