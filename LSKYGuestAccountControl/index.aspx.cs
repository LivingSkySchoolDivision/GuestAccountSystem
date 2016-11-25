using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.Repositories;
using LSKYGuestAccountControl.Static;

namespace LSKYGuestAccountControl
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            GuestAccountController repository = new GuestAccountController();

            for (int x = 0; x <= 100; x++)
            {
                Response.Write("<br>" + Authentication.GenerateGuestPassword());
            }

            Response.Write("<BR><B>Available guest accounts</b>");
            foreach (GuestAccount g in repository.GetAvailableGuestAccounts())
            {
                Response.Write("<BR>" + g.DN);
            }

            Response.Write("<BR><B>Active guest accounts</b>");
            foreach (GuestAccount g in repository.GetActiveGuestAccounts())
            {
                Response.Write("<BR>" + g.DN);
            }
        }
    }
}