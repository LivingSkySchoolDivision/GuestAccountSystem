﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl
{
    public static class Settings
    {
        public static List<string> SecurityGroupsAllowed
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.AppSettings["allowed_ad_security_groups"].ToString().Split(';').ToList();
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public static string dbConnectionString_Internal = ConfigurationManager.ConnectionStrings["Internal"].ConnectionString;
        
        public static string LoginURL { get { return "/Login/index.aspx"; } }
        public static string IndexURL { get { return "/index.aspx"; } }

        public static string ApplicationRoot { get { return HttpContext.Current.Request.ApplicationPath; } }

        public static string CookieName { get { return "LSKYSDGUESTCONTROL"; } }
        public static string Domain { get { return "LSKYSD.CA"; } }
    }
}