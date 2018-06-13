using System;
using System.Linq;

namespace ConsoleApp2
{
    class NewUser
    {
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string UPN { get; }
        public string log_on_name { get; set; }
        digestFQDN home;

        string generate_valid_log_on_name(string f, string l)
        {
            f = f.ToCharArray().First<char>().ToString();
            string lon = f.ToLower() + l.ToLower();
            Console.WriteLine("User Log-On Name: " + lon);
            return lon;
        }

        string generate_valid_UPN(string lon)
        {
            string upn = lon +"@" + home.dmn + "."+home.tld;
            Console.WriteLine("User UPN: " + upn);
            return upn;
        }

        public NewUser(digestFQDN netinfo, string full_name)
        {
            home = netinfo;
            string[] nameTokens = full_name.Split(' ');

            first_name = nameTokens.First<string>();
            last_name = nameTokens.Last<string>();

            log_on_name = generate_valid_log_on_name(first_name, last_name);
            UPN = generate_valid_UPN(log_on_name);
        }
    }
}
