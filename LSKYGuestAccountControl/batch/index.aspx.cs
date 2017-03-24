using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Repositories;
using LSKYGuestAccountControl.Static;

namespace LSKYGuestAccountControl.Batch
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            lblMaxBatchSize.Text = Settings.MaxBatchSize.ToString();

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
                if (!currentUser.CanViewLogs)
                {
                    redirectToIndex();
                }
            }

            if (!IsPostBack)
            {
                tblControls.Visible = true;
                tblIndexInstructions.Visible = true;

                GuestAccountController guestRepo = new GuestAccountController();
                int availableAccounts = guestRepo.GetAvailableGuestAccounts().Count();

                lblAvailableGuestAccounts.Text = availableAccounts.ToString();

                int maxBatchSize = Settings.MaxBatchSize;
                if (availableAccounts < maxBatchSize)
                {
                    maxBatchSize = availableAccounts;
                }


                drpBatchCount.Items.Clear();
                for (int x = 2; x <= Settings.MaxBatchSize; x++)
                {
                    drpBatchCount.Items.Add(new ListItem()
                    {
                        Text = x.ToString(),
                        Value = x.ToString()
                    });
                }
            } 
        }

        public void redirectToIndex()
        {
            string IndexURL = Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + Settings.IndexURL;
            Response.Clear();
            Response.Write("<html>");
            Response.Write("<meta http-equiv=\"refresh\" content=\"0; url=" + IndexURL + "\">");
            Response.Write("<div style=\"padding: 5px; text-align: center; font-size: 10pt; font-family: sans-serif;\">You do not have access to this page... redirecting... <a href=\"" + IndexURL + "\">Click here if you are not redirected automatically</a></div>");
            Response.Write("</html>");
            Response.End();
        }
        private TableRow accountRow(GuestAccount guest)
        {
            TableRow newRow = new TableRow();

            newRow.Cells.Add(new TableCell() { Text = guest.sAMAccountName });
            newRow.Cells.Add(new TableCell() { Text = guest.Password });

            return newRow;
        }

        public void redirectToInfoPage(string batchID)
        {
            string infoURL = Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + "/Batch/Batchinfo.aspx?batchid=" + batchID;
            Response.Clear();
            Response.Write("<html>");
            Response.Write("<meta http-equiv=\"refresh\" content=\"0; url=" + infoURL + "\">");
            Response.Write("<div style=\"padding: 5px; text-align: center; font-size: 10pt; font-family: sans-serif;\">Redirecting...<a href=\"" + infoURL + "\">Click here if you are not redirected automatically</a></div>");
            Response.Write("</html>");
            Response.End();
        }

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
                // Parse the number
                int count = Parsers.ParseInt(drpBatchCount.SelectedValue);

                if (count > 0)
                {
                    GuestAccountController guestrepo = new GuestAccountController();
                    string batchID = guestrepo.RequisitionBatch(currentUser, txtReason.Text, count);

                    // Wait a few seconds
                    System.Threading.Thread.Sleep(1000 * 3);

                    // Redirect to the batch info page
                    redirectToInfoPage(batchID);
                }
            }
        }
    }
}