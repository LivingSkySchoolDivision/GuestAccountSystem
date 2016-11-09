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

        public static bool IsPasswordStrongEnough(string thisPassword)
        {
            bool returnme = false;

            char[] upperCase = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            char[] lowerCase = { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z' };
            char[] numbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
            char[] specialchar = { ' ', '.', ',', '!', '@', '#', '$', '%', '^', '&', '\\', '/', '*', '(', ')', '-', '+', '|', '?', '`', '~' };

            List<string> commonPasswords = new List<string>();
            #region Add some common passwords to the list
            // These are from various sources for the most commonly used passwords from various breaches in the last few years
            // This list could definitely be bigger, but I only have so  much patience. Maybe at some point I will have it look these up from a database table
            commonPasswords.Add("123");
            commonPasswords.Add("1234");
            commonPasswords.Add("12345");
            commonPasswords.Add("123456");
            commonPasswords.Add("1234567");
            commonPasswords.Add("12345678");
            commonPasswords.Add("123456789");
            commonPasswords.Add("password");
            commonPasswords.Add("abc123");
            commonPasswords.Add("qwerty");
            commonPasswords.Add("monkey");
            commonPasswords.Add("letmein");
            commonPasswords.Add("111111");
            commonPasswords.Add("iloveyou");
            commonPasswords.Add("trustno1");
            commonPasswords.Add("123123");
            commonPasswords.Add("password1");
            commonPasswords.Add("000000");
            commonPasswords.Add("123123123");
            commonPasswords.Add("changeme");
            commonPasswords.Add("abc");
            commonPasswords.Add(" ");
            commonPasswords.Add("  ");
            commonPasswords.Add("   ");
            commonPasswords.Add("    ");
            commonPasswords.Add("     ");
            commonPasswords.Add("      ");
            commonPasswords.Add("       ");
            commonPasswords.Add("michael");
            commonPasswords.Add("football");
            commonPasswords.Add("princess");
            commonPasswords.Add("welcome");
            commonPasswords.Add("passw0rd");
            commonPasswords.Add("p4ssw0rd");
            commonPasswords.Add("access");
            commonPasswords.Add("shadow");
            commonPasswords.Add("hunter2");
            #endregion

            if (thisPassword.Trim().Length > 4)
            {
                if (!commonPasswords.Contains(thisPassword.ToLower()))
                {
                    return true;
                }
            }

            return returnme;
        }
    }
}