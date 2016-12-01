using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Repositories;

namespace LSKYGuestAccountControl.Log
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LogRepository logRepository = new LogRepository();
                List<LoggedActivation> activationLog = logRepository.GetRecentEntries(500);

                tblLog.Rows.Clear();
                tblLog.Rows.Add(addTableHeadings());
                foreach (LoggedActivation entry in activationLog)
                {
                    tblLog.Rows.Add(addLogEntry(entry));
                }
            }
        }

        private TableHeaderRow addTableHeadings()
        {
            TableHeaderRow returnMe = new TableHeaderRow();
            returnMe.Cells.Add(new TableHeaderCell() { Text = "Date" });
            returnMe.Cells.Add(new TableHeaderCell() { Text = "Time" });
            returnMe.Cells.Add(new TableHeaderCell() { Text = "Guest Account" });
            returnMe.Cells.Add(new TableHeaderCell() { Text = "Requesting User" });
            returnMe.Cells.Add(new TableHeaderCell() { Text = "Reason" });
            returnMe.Cells.Add(new TableHeaderCell() { Text = "IP Address" });
            returnMe.Cells.Add(new TableHeaderCell() { Text = "UserAgent" });
            return returnMe;
        }

        private TableRow addLogEntry(LoggedActivation entry)
        {
            TableRow returnMe = new TableRow();
            returnMe.Cells.Add(new TableCell() { Text = entry.Date.ToShortDateString() });
            returnMe.Cells.Add(new TableCell() { Text = entry.Date.ToShortTimeString() });
            returnMe.Cells.Add(new TableCell() { Text = entry.GuestAccountName });
            returnMe.Cells.Add(new TableCell() { Text = entry.RequestingUser });
            returnMe.Cells.Add(new TableCell() { Text = entry.Reason });
            returnMe.Cells.Add(new TableCell() { Text = entry.IPAddress });
            returnMe.Cells.Add(new TableCell() { Text = entry.UserAgent });
            return returnMe;
        }
    }
}