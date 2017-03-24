using System;
using System.Collections.Generic;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Web;
using LSKYGuestAccountControl.ExtensionMethods;
using LSKYGuestAccountControl.Model;

namespace LSKYGuestAccountControl.Static
{
    public static class Authentication
    {
        static Random random = new Random();

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

        public static UserPermissionResponse GetUserPermissions(string domain, string username)
        {
            bool canLogin = false;
            bool canBypassLimits = false;
            bool canUseBatches = false;
            bool canViewLog = false;

            List<string> usersGroupMembership = GetUsersGroups(domain, username);
            
            // Check if the user can even log in first
            foreach (string group in Settings.SecurityGroupsAllowed)
            {
                if (usersGroupMembership.Contains(group))
                {
                    canLogin = true;
                }
            }

            // If the user can't login at this point, don't even bother checking any further
            if (!canLogin)
            {
                return new UserPermissionResponse()
                {
                    CanUserUseSystem = false,
                    CanUserBypassLimits = false,
                    CanUserCreateBatches = false,
                    CanUserViewLog = false
                };
            }

            // Check if the user can bypass limits
            foreach (string group in Settings.SecurityGroupsBypassLimits)
            {
                if (usersGroupMembership.Contains(group))
                {
                    canBypassLimits = true;
                }
            }

            // Check if the user can use batches
            foreach (string group in Settings.SecurityGroupsAllowedToMakeBatches)
            {
                if (usersGroupMembership.Contains(group))
                {
                    canUseBatches = true;
                }
            }

            // Check if the user can view logs
            foreach (string group in Settings.SecurityGroupsViewLog)
            {
                if (usersGroupMembership.Contains(group))
                {
                    canViewLog = true;
                }
            }

            return new UserPermissionResponse()
            {
                CanUserUseSystem = canLogin,
                CanUserBypassLimits = canBypassLimits,
                CanUserCreateBatches = canUseBatches,
                CanUserViewLog = canViewLog
            };
        }
        
        /// <summary>
        /// Returns a list of AD groups a user is a member of
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        private static List<string> GetUsersGroups(string domain, string username)
        {
            List<string> returnMe = new List<string>();

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                using (UserPrincipal user = UserPrincipal.FindByIdentity(pc, username))
                {
                    if (user != null)
                    {
                        PrincipalSearchResult<Principal> groups = user.GetAuthorizationGroups();

                        foreach (Principal p in groups)
                        {
                            if (p is GroupPrincipal)
                            {
                                returnMe.Add(p.SamAccountName.ToLower());
                            }
                        }
                    }
                }
            }

            return returnMe;
        }

        /// <summary>
        /// Returns a list of an AD group's members
        /// </summary>
        /// <param name="domain"></param>
        /// <param name="groupName"></param>
        /// <returns></returns>
        private static List<string> GetADGroupMembers(string domain, string groupName)
        {
            List<string> returnMe = new List<string>();

            using (PrincipalContext pc = new PrincipalContext(ContextType.Domain, domain))
            {
                using (GroupPrincipal grp = GroupPrincipal.FindByIdentity(pc, IdentityType.Name, groupName))
                {
                    if (grp != null)
                    {
                        foreach (Principal p in grp.GetMembers(true))
                        {
                            returnMe.Add(p.SamAccountName.ToLower());
                        }
                    }
                }
            }
            return returnMe;
        }
        public static string GenerateGuestPassword()
        {
            List<string> adjectives = new List<string>()
            {
                "orange",
                "bright",
                "hello",
                "tomorrow",
                "elephant",
                "red",
                "blue",
                "green",
                "purple",
                "violet",
                "grape",
                "future",
                "fancy",
                "nice",
                "skinny",
                "short",
                "tiny",
                "big",
                "noisy",
                "prickly",
                "cold",
                "cute",
                "funny",
                "fuzzy",
                "groovy",
                "great",
                "giant",
                "huge",
                "lucky",
                "odd",
                "royal",
                "smart",
                "swift",
                "strong",
                "ultra",
                "violet",
                "wacky",
                "wild",
                "wise",
                "saturday",
                "sunday",
                "monday",
                "tuesday",
                "wednesday",
                "thursday",
                "friday",
                "january",
                "february",
                "march",
                "april",
                "may",
                "june",
                "july",
                "august",
                "september",
                "october",
                "november",
                "december",
                "pizza",
                "space",
                "radioactive",

            };

            List<string> nouns = new List<string>()
            {
                "basket",
                "kitten",
                "puppy",
                "dragon",
                "music",
                "flower",
                "forest",
                "music",
                "summer",
                "winter",
                "spring",
                "autumn",
                "weather",
                "wagon",
                "mountain",
                "captain",
                "chicken",
                "circus",
                "doctor",
                "river",
                "sky",
                "ocean",
                "sandwich",
                "pizza",
                "salad",
                "picture",
                "hat",
                "violin",
                "breakfast",
                "lunch",
                "dinner",
                "piano",
                "telephone",
                "banana",
                "apple",
                "soup",
                "food",
                "bird",
                "thing",
                "apple",
                "bread",
                "cake",
                "lemon",
                "cookie",
            };
            
            return adjectives[random.Next(adjectives.Count)] + "-" + nouns[random.Next(nouns.Count)];
        }
    }
}