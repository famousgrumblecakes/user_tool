/*
 Cheap powershell trick to give the users we're making o365 licenses (probably doesn't work?)
 */

using System.Management.Automation;
using System.Security;

namespace ConsoleApp2
{
    class createO365
    {
        public createO365(Credentials creds, string displayname, string upn, string fname, string lname, string hm)
        {

            //Convert our sinful unsecure string to a secure securestring
            var secure = new SecureString();
            foreach (char c in creds.password)
            {
                secure.AppendChar(c);
            }
            var secureCreds = new PSCredential(creds.username + "@viatechpub.com", secure);

            using (PowerShell ps = PowerShell.Create())
            {
                ps.AddCommand("Connect-MsolService");
                ps.AddParameter("Credentials", secureCreds);
                ps.Invoke();

                ps.AddCommand("New-MsolUser");
                ps.AddParameter("DisplayName", displayname);
                ps.AddParameter("FirstName", fname);
                ps.AddParameter("LastName", lname);
                ps.AddParameter("UserPrincipalName", upn);
                ps.AddParameter("UsageLocation", "US");
                ps.AddParameter("Password", "Temp123!");
                ps.AddParameter("LicenseAssignment", hm + ":ENTERPRISEPACK");

                ps.Invoke();
            }

        }

    }
}
