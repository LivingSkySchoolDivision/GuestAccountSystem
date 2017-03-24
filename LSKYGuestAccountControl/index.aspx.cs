using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Repositories;
using LSKYGuestAccountControl.Static;

namespace LSKYGuestAccountControl
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblAllowedRequisitionsPerDay.Text = Settings.AllowedRequisitionsPerDay.ToString();
            lblAllowedRequisitionsPerDay2.Text = Settings.AllowedRequisitionsPerDay.ToString();

            if (!IsPostBack)
            {
                tblControls.Visible = true;
                tblIndexInstructions.Visible = true;
                tblNewAccountInfo.Visible = false;
                tblNewAccountInstructions.Visible = false;
            }

            // Get the current user
            LoginSessionRepository loginRepository = new LoginSessionRepository();
            string foundUserSessionID = loginRepository.GetSessionIDFromCookies(Request);
            LoginSession currentUser = null;
            if (!string.IsNullOrEmpty(foundUserSessionID))
            {
                // A cookie exists, lets see if it corresponds to a valid session ID
                currentUser = loginRepository.LoadIfValid(foundUserSessionID,
                    Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
            }

            if (currentUser != null)
            {
                // Find any guest accounts that the logged in user has already requisitions
                GuestAccountController guestRepo = new GuestAccountController();
                List<GuestAccount> alreadyProvisionedGuestAccounts = guestRepo.GetActiveAccountsRequisitionedBy(currentUser);

                if (alreadyProvisionedGuestAccounts.Count > 0)
                {
                    lblCount.Text = "<div class=\"already_active_text\">You have already activated " + alreadyProvisionedGuestAccounts.Count + " of a maximum of " + Settings.AllowedRequisitionsPerDay + " guest account(s) today</div>";
                }

                if ((alreadyProvisionedGuestAccounts.Count >= Settings.AllowedRequisitionsPerDay) && (!currentUser.CanBypassLimits))
                {
                    tblControls.Visible = false;
                    tblNewAccountInfo.Visible = false;
                    tblNewAccountInstructions.Visible = false;
                    tblTooMany.Visible = true;
                }

                if (alreadyProvisionedGuestAccounts.Count > 0)
                {
                    tblActiveAccounts.Visible = true;
                    tblActiveAccounts.Rows.Clear();
                    tblActiveAccounts.Rows.Add(alreadyActiveHeadings());
                    LogRepository logRepo = new LogRepository();
                    List<LoggedActivation> provisionedAccounts = logRepo.GetActivationsToday(currentUser);

                    // Make a list of active usernames that we can compare to
                    List<string> activeUsernames = alreadyProvisionedGuestAccounts.Select(g => g.sAMAccountName).ToList();
                    List<string> alreadyDisplayed = new List<string>();
                    foreach (LoggedActivation guest in provisionedAccounts.OrderByDescending(g => g.Date))
                    {
                        if (activeUsernames.Contains(guest.GuestAccountName))
                        {
                            if (!alreadyDisplayed.Contains(guest.GuestAccountName))
                            {
                                alreadyDisplayed.Add(guest.GuestAccountName);
                                tblActiveAccounts.Rows.Add(alreadyActiveRow(guest));
                            }
                        }
                    }
                }

            }
            
        }

        private TableHeaderRow alreadyActiveHeadings()
        {
            TableHeaderRow newRow = new TableHeaderRow();
            
            newRow.Cells.Add(new TableHeaderCell() { Text = "Username" });
            newRow.Cells.Add(new TableHeaderCell() { Text = "Password" });
            newRow.Cells.Add(new TableHeaderCell() { Text = "Reason" });
            newRow.Cells.Add(new TableHeaderCell() { Text = "Expires" });

            return newRow;
        }

        private TableRow alreadyActiveRow(LoggedActivation guest)
        {
            TableRow newRow = new TableRow();
            
            newRow.Cells.Add(new TableCell() { Text = guest.GuestAccountName });
            newRow.Cells.Add(new TableCell() { Text = guest.Password });
            newRow.Cells.Add(new TableCell() { Text = guest.Reason });
            string expiresString = DateTime.Today.AddDays(1).AddMinutes(-1).ToString();
            newRow.Cells.Add(new TableCell() { Text = expiresString });

            return newRow;
        }

        private static int errorBorderWidth = 0;

        protected void btnActivate_OnClick(object sender, EventArgs e)
        {
            // Get the current user
            LoginSessionRepository loginRepository = new LoginSessionRepository();
            string foundUserSessionID = loginRepository.GetSessionIDFromCookies(Request);
            LoginSession currentUser = null;
            if (!string.IsNullOrEmpty(foundUserSessionID))
            {
                // A cookie exists, lets see if it corresponds to a valid session ID
                currentUser = loginRepository.LoadIfValid(foundUserSessionID,
                    Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
            }

            if (currentUser != null)
            {
                // Check to make sure that they've enterd a reason
                
                GuestAccountController guestrepo = new GuestAccountController();

                GuestAccount activatedAccount = guestrepo.RequisitionAccount(currentUser, txtReason.Text.Trim());

                if (activatedAccount != null)
                {
                    lblUsername.Text = activatedAccount.sAMAccountName;
                    lblPassword.Text = activatedAccount.Password;
                    lblExpires.Text = DateTime.Today.AddDays(1).AddMinutes(-1).ToString();

                    tblControls.Visible = false;
                    tblIndexInstructions.Visible = false;
                    tblNewAccountInfo.Visible = true;
                    tblNewAccountInstructions.Visible = true;
                    tblActiveAccounts.Visible = false;
                }
            }
        }
    }
}