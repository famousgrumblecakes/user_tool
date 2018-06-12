using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
namespace ConsoleApp2
{
    //I'm aware this isn't really getting a fqdn
    class digestFQDN
    {
        public string local { get; }
        public string dmn { get; }
        public string tld { get; }

        private string[] digest;

        public digestFQDN()
        {
            digest = IPGlobalProperties.GetIPGlobalProperties().DomainName.Split('.');
            local = System.Environment.MachineName.ToLower();
            dmn = digest.First<string>();
            tld = digest.Last<string>();

            Console.WriteLine("Your current environment: " + local + "." + dmn + "." + tld);
        }
    }
}
