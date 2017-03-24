using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Repositories;

namespace LSKYGuestAccountControl.Batch
{
    public partial class BatchInfo : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // get batch ID from querystring
            string batchID = Request.QueryString["batchid"];
            litHeading.Text = "<p style=\"text-align: center;\">Missing Batch ID</p>";

            if (!string.IsNullOrEmpty(batchID))
            {
                LogRepository logRepo = new LogRepository();
                List<LoggedActivation> batchActivations = logRepo.GetBatchID(batchID);

                if (batchActivations.Count > 0)
                {
                    tblAccounts.Visible = true;

                    litHeading.Text = "<p><b>Batch:</b> " + batchID + "<br><b>Accounts in this batch:</b> " + batchActivations.Count + "</p>";

                    tblAccounts.Rows.Clear();
                    tblAccounts.Rows.Add(addHeadingRow());
                    foreach (LoggedActivation guest in batchActivations)
                    {
                        tblAccounts.Rows.Add(addAccountrow(guest));
                    }
                }
            }
        }

        private TableHeaderRow addHeadingRow()
        {
            TableHeaderRow newRow = new TableHeaderRow();
            
            newRow.Cells.Add(new TableHeaderCell() { Text = "Activation date" });
            newRow.Cells.Add(new TableHeaderCell() { Text = "Guest account" });
            newRow.Cells.Add(new TableHeaderCell() { Text = "Password" });
            newRow.Cells.Add(new TableHeaderCell() { Text = "Requested by" });

            return newRow;
        }

        private TableRow addAccountrow(LoggedActivation guest)
        {
            TableRow newRow = new TableRow();

            newRow.Cells.Add(new TableCell() { Text = guest.Date.ToString() });
            newRow.Cells.Add(new TableCell() { Text = guest.GuestAccountName });
            newRow.Cells.Add(new TableCell() { Text = guest.Password });
            newRow.Cells.Add(new TableCell() { Text = guest.RequestingUser });

            return newRow;
        }
    }
}