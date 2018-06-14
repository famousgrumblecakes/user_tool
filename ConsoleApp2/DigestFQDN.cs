using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.NetworkInformation;
namespace ConsoleApp2
{
    //I'm aware this isn't really getting a fqdn
    /*
        made a horrible little string builder to setup the container we pass to PrincipalContext. The goal is to absorb your environment information
        so there's no manual configuration to muck about with.
    */
    class digestFQDN
    {
        public string local { get; }
        public string dmn { get; }
        public string tld { get; }
        public string container { get; }
        private string[] digest;

        public digestFQDN()
        {
            digest = IPGlobalProperties.GetIPGlobalProperties().DomainName.Split('.');
            local = System.Environment.MachineName.ToLower();
            dmn = digest[digest.Length - 2].ToLower();
            tld = digest.Last<string>().ToLower();
            for (int i = 0; i < digest.Length; i++)
            {
                digest[i] = "DC=" + digest[i];
            }
            Console.WriteLine("Your current environment: " + local + "." + dmn + "." + tld);

            for(int i = 0; i < digest.Length; i++)
            {
                container = string.Concat(container, digest[i]);
                if (i+1 < digest.Length)
                {
                    container = string.Concat(container, ",");
                }
            }
            //container = string.Concat("CN=Users" + "," + container);

        }
    }
}


