using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using LSKYGuestAccountControl.ExtensionMethods;
using LSKYGuestAccountControl.Model;
using LSKYGuestAccountControl.Repositories;
using LSKYGuestAccountControl.Static;

namespace LSKYGuestAccountControl
{
    public partial class Template : System.Web.UI.MasterPage
    {
        /// <summary>
        /// Stops the processing of the current page, and redirects to the login page (URL is specified in a string at the top of this file)
        /// </summary>
        public void RedirectToLogin()
        {
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Redirect(Request.Url.GetLeftPart(UriPartial.Authority) + HttpContext.Current.Request.ApplicationPath + Settings.LoginURL);
            Response.OutputStream.Flush();
            Response.OutputStream.Close();
            Response.End();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            // If "Logout" or "Logoff" are in the querystring, log the current session off
            if ((Request.QueryString.AllKeys.Contains("logoff")) || (Request.QueryString.AllKeys.Contains("logout")))
            {
                LoginSessionRepository loginRepository = new LoginSessionRepository();
                string foundUserSessionID = loginRepository.GetSessionIDFromCookies(Request);
                if (!string.IsNullOrEmpty(foundUserSessionID))
                {
                    loginRepository.Expire(foundUserSessionID);
                    RedirectToLogin();
                }

            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
            LoginSessionRepository loginRepository = new LoginSessionRepository();
            string foundUserSessionID = loginRepository.GetSessionIDFromCookies(Request);

            LoginSession currentUser = null;

            if (!string.IsNullOrEmpty(foundUserSessionID))
            {
                // A cookie exists, lets see if it corresponds to a valid session ID
                currentUser = loginRepository.LoadIfValid(foundUserSessionID,
                    Request.ServerVariables["REMOTE_ADDR"], Request.ServerVariables["HTTP_USER_AGENT"]);
            }

            // If there is no logged in user, redirect to login page
            if (currentUser == null)
            {
                string CurrentURL = Request.Url.AbsoluteUri;
                string LoginURL = Request.Url.GetLeftPart(UriPartial.Authority) +
                                  HttpContext.Current.Request.ApplicationPath + Settings.LoginURL;

                // If the application is running in the root, we dont need to include the application path
                if (HttpContext.Current.Request.ApplicationPath == "/")
                {
                    LoginURL = Request.Url.GetLeftPart(UriPartial.Authority) + Settings.LoginURL;
                }
                if (!
                    (CurrentURL.ToLower().Equals(LoginURL.ToLower()))
                    )
                {
                    RedirectToLogin();
                }
            }
            else
            {
                Response.Write("<!-- Logged in: " + currentUser.Username + " -->");
                Response.Write("<!-- Is in valid group: " + Authentication.GetValidUserGroups(Settings.Domain, currentUser.Username).ToCommaSeparatedString() + "-->");
            }

        }
    }
}