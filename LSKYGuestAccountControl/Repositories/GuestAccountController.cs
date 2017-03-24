using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Threading;
using System.Web;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Static;

namespace LSKYGuestAccountControl.Repositories
{
    public class GuestAccountController
    {
        static Random random = new Random();

        private List<GuestAccount> _cache = new List<GuestAccount>();

        private void RefreshCache()
        {
            _cache = new List<GuestAccount>();

            //try
            {
                DirectoryEntry searchRoot = new DirectoryEntry("LDAP://" + Settings.GuestAccountOU);
                DirectorySearcher searcher = new DirectorySearcher(searchRoot);
                searcher.Filter = "(objectClass=user)";
                searcher.Sort = new SortOption("sn", SortDirection.Ascending);
                searcher.PageSize = 1000;
                searcher.PropertiesToLoad.Add("givenName");
                searcher.PropertiesToLoad.Add("sn");
                searcher.PropertiesToLoad.Add("sAMAccountName");
                searcher.PropertiesToLoad.Add("comment");
                searcher.PropertiesToLoad.Add("employeeID");
                searcher.PropertiesToLoad.Add("userAccountControl");
                searcher.PropertiesToLoad.Add("description");
                searcher.PropertiesToLoad.Add("title");

                // This scope returns all users in the given OU, and child OUs
                searcher.SearchScope = SearchScope.OneLevel;

                // This scope returns just users in the given OU
                //searcher.SearchScope = SearchScope.OneLevel;

                SearchResultCollection allUsers = searcher.FindAll();

                //foreach (DirectoryEntry child in directoryObject.Children)
                #region Iterate users
                foreach (SearchResult thisUser in allUsers)
                {
                    DirectoryEntry child = thisUser.GetDirectoryEntry();
                    string DN = child.Path.ToString().Remove(0, 7);

                    #region GivenName
                    String GivenName;
                    if (child.Properties.Contains("givenName"))
                    {
                        GivenName = child.Properties["givenName"].Value.ToString();
                    }
                    else
                    {
                        GivenName = "";
                    }
                    #endregion

                    #region Surname
                    String Surname;
                    if (child.Properties.Contains("sn"))
                    {
                        Surname = child.Properties["sn"].Value.ToString();
                    }
                    else
                    {
                        Surname = "";
                    }
                    #endregion

                    #region AccountName
                    String AccountName;
                    if (child.Properties.Contains("sAMAccountName"))
                    {
                        AccountName = child.Properties["sAMAccountName"].Value.ToString();
                    }
                    else
                    {
                        AccountName = string.Empty;
                    }
                    #endregion

                    #region Comment
                    String Comment;
                    if (child.Properties.Contains("comment"))
                    {
                        Comment = child.Properties["comment"].Value.ToString();
                    }
                    else
                    {
                        Comment = "";
                    }
                    #endregion

                    #region description
                    String Description;
                    if (child.Properties.Contains("description"))
                    {
                        Description = child.Properties["description"].Value.ToString();
                    }
                    else
                    {
                        Description = "";
                    }
                    #endregion

                    #region Title / Expires
                    bool expires = true;
                    if (child.Properties.Contains("title"))
                    {
                        if (!string.IsNullOrEmpty(child.Properties["title"].Value.ToString()))
                        {
                            expires = false;
                        }
                    }
                    #endregion

                    #region userAccountControl
                    bool Enabled;
                    int userAccountControl = Convert.ToInt32(child.Properties["userAccountControl"][0]);
                    bool results = ((userAccountControl & 2) > 0);
                    Enabled = !results;
                    #endregion

                    GuestAccount guestAccount = new GuestAccount()
                    {
                        GivenName = GivenName,
                        SN = Surname,
                        sAMAccountName = AccountName,
                        DN = DN,
                        Comment = Comment,
                        Description = Description,
                        IsEnabled = Enabled,
                        Expires = expires
                    };

                    _cache.Add(guestAccount);
                    child.Close();
                    child.Dispose();
                }
                #endregion
                searchRoot.Close();
                searchRoot.Dispose();
            }
            //catch (DirectoryServicesCOMException e)
            {

            }
        }

        public GuestAccountController()
        {
            RefreshCache();
        }

        public List<GuestAccount> GetAllGuestAccounts()
        {
            RefreshCache();
            return _cache.ToList();
        }

        public List<GuestAccount> GetAvailableGuestAccounts()
        {
            RefreshCache();
            return _cache.Where(g => !g.IsEnabled).ToList();
        }

        public List<GuestAccount> GetActiveGuestAccounts()
        {
            RefreshCache();
            return _cache.Where(g => g.IsEnabled).ToList();
        }

        public List<GuestAccount> GetActiveAccountsRequisitionedBy(LoginSession currentUser)
        {
            return GetActiveGuestAccounts().Where(g => g.Description.Contains(currentUser.Username)).ToList();
        }

        public GuestAccount GetRandomAvailableGuestAccount()
        {
            List<GuestAccount> availableGuestAccounts = GetAvailableGuestAccounts();

            if (availableGuestAccounts.Count > 0)
            {
                return availableGuestAccounts[random.Next(availableGuestAccounts.Count)];
            }
            else
            {
                return null;
            }
        }

        private string GetBatchID()
        {
            return Crypto.GetMD5(DateTime.Now.ToString());
        }

        private void ActivateAccount(GuestAccount account, LoginSession currentUser, string reason, string batchID)
        {
            DirectoryEntry DE = new DirectoryEntry("LDAP://" + account.DN, Settings.ADUsername, Settings.ADPassword);

            // Enable
            DE.Properties["userAccountControl"].Value = (int)DE.Properties["userAccountControl"].Value & ~0x2;

            // Set password
            account.Password = Authentication.GenerateGuestPassword();
            DE.Invoke("SetPassword", new object[] { account.Password });

            // Flag for a password reset
            // DE.Properties["pwdLastSet"].Value = 0;

            // Set comment / description
            string comment = "Last activated by " + currentUser.Username + " on " + DateTime.Now.ToShortDateString() + " at " + DateTime.Now.ToShortTimeString() + " from IP " + currentUser.IPAddress;
            DE.Properties["description"].Value = comment;

            DE.Properties["comment"].Value = currentUser.Username;

            DE.CommitChanges();
            DE.Close();

            // Update our internal list to save having to reload for this information
            account.IsEnabled = true;
            account.LastActivated = DateTime.Now;
            account.Comment = currentUser.Username;

            // Log this
            LogRepository logrepository = new LogRepository();
            logrepository.LogActivation(batchID, account, currentUser, reason);

        }

        public GuestAccount RequisitionAccount(LoginSession currentUser, string reason)
        {
            GuestAccount accountToActivate = GetRandomAvailableGuestAccount();

            if (accountToActivate != null)
            {
                ActivateAccount(accountToActivate, currentUser, reason, GetBatchID());
                return accountToActivate;
            }
            
            throw new Exception("No guest accounts left to activate!");
        }

        public string RequisitionBatch(LoginSession currentUser, string reason, int count)
        {
            // Check to make sure we have enough accounts
            if (count > GetAvailableGuestAccounts().Count())
            {
                throw new Exception("Request to activate " + count + " accounts, but only " + GetAvailableGuestAccounts().Count() + " are available");
            }
            string batchID = GetBatchID();

            List <GuestAccount> batch = new List<GuestAccount>();

            for (int x = 0; x < count; x++)
            {
                GuestAccount accountToActivate = GetRandomAvailableGuestAccount();
                if (accountToActivate != null)
                {
                    ActivateAccount(accountToActivate, currentUser, reason, batchID);
                    batch.Add(accountToActivate);
                }
            }

            return batchID;
        }

    }
}