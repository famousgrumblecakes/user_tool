using System;
using System.Collections.Generic;
using System.Linq;


namespace ConsoleApp2
{
    class Credentials
    {

        public string username { get; set; }
        public string password { get; }

        public string get_the_password()
        {
            Queue<char> pwd = new Queue<char>();
            while (true)
            {
                ConsoleKeyInfo i = Console.ReadKey(true);
                if (i.Key == ConsoleKey.Enter)
                {
                    break;
                }
                else if (i.Key == ConsoleKey.Backspace)
                {
                    if (pwd.Count != 0)
                    {
                        pwd.Dequeue();
                        Console.Write("\b \b");
                    }
                }
                else
                {
                    pwd.Enqueue(i.KeyChar);
                    Console.Write("*");
                }
            }
            char[] pwdArr = pwd.ToArray<char>();
            string tmp = new string(pwdArr);
            
            return tmp;
        }

        public Credentials()
        {
            Console.Write("Your " + Environment.UserDomainName.ToLower() + " username: ");
            username = Console.ReadLine();
            Console.Write("Your password: ");
            password = get_the_password();
            Console.WriteLine();
        }

    }

}
