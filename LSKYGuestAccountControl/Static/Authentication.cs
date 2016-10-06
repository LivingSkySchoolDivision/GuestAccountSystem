using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Static
{
    public static class Authentication
    {
        public static string GetServerName(HttpRequest Request)
        {
            return Request.ServerVariables["SERVER_NAME"].ToString().Trim();
        }

        /// <summary>
        /// Validates a username and password against Active Directory and returns true if they match.
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool ValidateADCredentials(string domain, string username, string password)
        {
            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                return pc.ValidateCredentials(username, password);
            }
        }

        /// <summary>
        /// Parse a username, in case a user enters their domain in the username. We hard code the domain elsewhere, so strip it out if it was entered here.
        /// </summary>
        /// <param name="givenUsername"></param>
        /// <returns></returns>
        public static string ParseUsername(string givenUsername)
        {
            return givenUsername.ToLower().Replace("@lskysd.ca", "").Replace(@"lskysd\", "");
        }
    }
}