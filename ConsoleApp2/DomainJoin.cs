using System;
using System.DirectoryServices.AccountManagement;


namespace ConsoleApp2
{
    class DomainJoin
    {
        private string uname = null, pwd = null;
        private string container;
        private NewUser nu;
        private string dmn;
        private ContextType ctx;
        string[] group = null;
        bool aduser = false;
        Credentials o365creds;
        digestFQDN location;

        private void join()
        {
            using (PrincipalContext pc = new PrincipalContext(ctx, dmn, container, ContextOptions.Negotiate, uname, pwd))
            {
                try
                {
                    using (UserPrincipal up = new UserPrincipal(pc, nu.log_on_name, "Temp123!", true))
                    {
                        up.Name = nu.log_on_name;
                        up.SamAccountName = up.Name;
                        up.DisplayName = nu.first_name + " " + nu.last_name;
                        up.ExpirePasswordNow();
                        if (aduser == true)
                        {
                            up.GivenName = nu.first_name;
                            up.Surname = nu.last_name;
                            up.UserPrincipalName = nu.UPN;
                        }
                        up.Save();
                        int count = 0;
                        foreach (string i in group)
                        {
                            try
                            {
                                GroupPrincipal gp = GroupPrincipal.FindByIdentity(pc, i);
                                gp.Members.Add(pc, IdentityType.SamAccountName, up.SamAccountName);
                                gp.Save();
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine(e.Message.ToString());
                            }
                        }


//                        createO365 ouser = new createO365(o365creds, up.DisplayName, nu.log_on_name + "@viatechpub.com", nu.first_name, nu.last_name, location.dmn);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message.ToString());
                }
            }
        }

        public DomainJoin(NewUser nup, Credentials creds, digestFQDN l, bool where, string[] g)
        {
            o365creds = creds;
            location = l;
            group = g;
            if (where == false)
            {
                Console.WriteLine("adding local user");
                dmn = System.Environment.MachineName;
                ctx = ContextType.Machine;
                container = null;
            }
            else
            {
                aduser = true;
                Console.WriteLine("adding AD user");
                uname = creds.username;
                pwd = creds.password;
                dmn = location.dmn;

                ctx = ContextType.Domain;
                container = "CN=Users" +",DC="+ dmn + ",DC=" + location.tld;

            }
            nu = nup;
            Console.WriteLine("Adding to:" + dmn);
            join();
        }
    }
}
