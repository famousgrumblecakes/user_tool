using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices.AccountManagement;
using System.Security;
using System.Security.Permissions;
using System.Net;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            string full_name = "nothing";
            bool where = false;
            bool multiadd = false;
            bool prompt_for_user = true;
            string filename = null;
            string[] group = {"Users"};
            Credentials creds = null;
            digestFQDN netinfo = new digestFQDN();

            if (args.Length == 0)
            {
                Console.WriteLine("run with -? for settings");
            }
            else
            {
                int count = 0;
                foreach (string i in args)
                {
                    if (i == "-?")
                    {
                        Console.WriteLine("usage: executable.exe -u FirstName LastName\n" +
                            "-?         show settings\n" +
                            "-u         New User's Name\n" +
                            "-d         Make a domain user    Default: local\n" +
                            "-b         read user list from a file\n" +
                            "-g         {GroupName1 GroupName2 Groupname3}   Specify a user group  Default: Users\n" +
                            "-#         Make Administrator");

                        Environment.Exit(0);
                    }
                    if (i == "-u")
                    {
                        try
                        {
                            full_name = args[count + 1] + " " + args[count + 2];
                            prompt_for_user = false;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message.ToString() + "- Did you specify a full name?");
                        }

                    }
                    if (i == "-l")
                    {
                        where = false;
                    }
                    if(i == "-d")
                    {
                        creds = new Credentials();
                        where = true;

                    }
                    if (i == "-b")
                    {
                        try
                        {
                            filename = args[count + 1];
                            multiadd = true;
                            prompt_for_user = false;
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message.ToString() + "- Did you specify a filename?");
                        }
                    }
                    if (i == "-g")
                    {
                        Console.WriteLine("-g detected");
                        Queue<String> groupqueue = new Queue<string>();
                        groupqueue.Enqueue("Users");
                        for (int groupcount = count + 1; groupcount < args.Length; groupcount++)
                        {
                            if (args[groupcount].ToCharArray().First<char>() == '-')
                            {
                                break;
                            }
                            else
                            {
                                groupqueue.Enqueue(args[groupcount]);
                            }

                        }
                        group = groupqueue.ToArray();
                    }
                    if (i == "-#")
                    {
                        group[0] = "Administrators";
                    }
                    count++;
                }
            }

            if (prompt_for_user == true)
            {
                Console.Write("User full name: ");
                full_name = Console.ReadLine();
            }
            if (multiadd == false)
            {
                NewUser user = new NewUser(netinfo, full_name);
                DomainJoin dj = new DomainJoin(user, creds, netinfo, where, group);
            }
            else //Add users from a file (will not make administrators)
            {
                FileStream fs = System.IO.File.OpenRead(filename);
                StreamReader sr = new StreamReader(fs);

                while (!sr.EndOfStream)
                {
                    NewUser user = new NewUser(netinfo, sr.ReadLine());
                    DomainJoin dj = new DomainJoin(user, creds, netinfo, where, group);
                }
            }
        }
    }
}

