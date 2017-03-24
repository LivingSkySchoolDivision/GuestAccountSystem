using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.ExtensionMethods;
using LSKYGuestAccountControl.Repositories;

namespace LSKYGuestAccountControl.JSON
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GuestAccountController guestRepo = new GuestAccountController();
            List<GuestAccount> allGuestAccounts = guestRepo.GetAllGuestAccounts();

            Response.Clear();
            Response.ContentEncoding = Encoding.UTF8;
            Response.ContentType = "application/json; charset=utf-8";
            Response.Write("{\n");
            Response.Write("\"TotalGuestAccounts\" : " + allGuestAccounts.Count() + ",\n");
            Response.Write("\"TotalActive\" : " + allGuestAccounts.Count(g => g.IsEnabled) + ",\n");
            Response.Write("\"ActiveAndDontExpire\" : " + allGuestAccounts.Count(g => g.IsEnabled && !g.Expires) + ",\n");
            Response.Write("\"TotalAvailable\" : " + allGuestAccounts.Count(g => !g.IsEnabled) + ",\n");
            Response.Write("\"Active\": [\n");

            int counter = 0;
            foreach (GuestAccount guest in allGuestAccounts.Where(g => g.IsEnabled))
            {
                Response.Write("{\n");
                Response.Write("\"UserName\": \"" + guest.sAMAccountName + "\",\n");
                Response.Write("\"RequestedBy\": \"" + guest.Comment + "\",\n");
                Response.Write("\"Expires\": \"" + guest.Expires.ToYesOrNo() + "\"\n");
                Response.Write("}\n");
                counter++;
                if (counter < allGuestAccounts.Count(g => g.IsEnabled))
                {
                    Response.Write(",");
                }
            }

            Response.Write("]\n");
            Response.Write("}\n");
            Response.End();
        }
    }
}