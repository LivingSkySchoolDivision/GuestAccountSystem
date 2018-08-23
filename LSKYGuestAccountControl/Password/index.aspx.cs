using LSKYGuestAccountControl.Static;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace LSKYGuestAccountControl.Password
{
    public partial class index : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            StringBuilder outPut = new StringBuilder();
            for (int x = 0; x < 5; x++)
            {
                outPut.Append(Authentication.GenerateGuestPassword());
                outPut.Append("<BR>");
            }

            lblPassword.Text = Authentication.GenerateGuestPassword();
            lblPasswords.Text = outPut.ToString();
        }
    }
}