using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Repositories
{
    public class GuestAccountController
    {
        private List<GuestAccount> _cache = new List<GuestAccount>();

        public GuestAccountController()
        {
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

                // This scope returns all users in the given OU, and child OUs
                searcher.SearchScope = SearchScope.Subtree;

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

        public List<GuestAccount> GetAvailableGuestAccounts()
        {
            return _cache.Where(g => !g.IsEnabled).ToList();
        }

        public List<GuestAccount> GetActiveGuestAccounts()
        {
            return _cache.Where(g => g.IsEnabled).ToList();
        }

    }
}