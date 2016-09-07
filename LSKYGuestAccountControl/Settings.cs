using System;
using System.Collections.Generic;
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
    }
}