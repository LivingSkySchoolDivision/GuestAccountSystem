using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using LSKYGuestAccountControl.Static;

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
                    return System.Configuration.ConfigurationManager.AppSettings["allowed_ad_security_groups"].ToString().Split(';').ToList().ConvertAll(g => g.ToLower());
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public static List<string> SecurityGroupsAllowedToMakeBatches
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.AppSettings["allowed_ad_security_groups_for_batches"].ToString().Split(';').ToList().ConvertAll(g => g.ToLower()); ;
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public static List<string> SecurityGroupsBypassLimits
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.AppSettings["allowed_ad_security_groups_bypass_limits"].ToString().Split(';').ToList().ConvertAll(g => g.ToLower()); ;
                }
                catch
                {
                    return new List<string>();
                }
            }
        }
        public static List<string> SecurityGroupsViewLog
        {
            get
            {
                try
                {
                    return System.Configuration.ConfigurationManager.AppSettings["allowed_ad_security_groups_view_log"].ToString().Split(';').ToList().ConvertAll(g => g.ToLower()); ;
                }
                catch
                {
                    return new List<string>();
                }
            }
        }

        public static string GuestAccountOU
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["GuestAccountOU"].Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static string BatchGuestAccountOU
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["BatchGuestAccountOU"].Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static string dbConnectionString_Internal
        {
            get
            {
                try
                {
                    return ConfigurationManager.ConnectionStrings["Internal"].ConnectionString;
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static string LoginURL { get { return "/Login/index.aspx"; } }
        public static string IndexURL { get { return "/index.aspx"; } }

        public static string ApplicationRoot { get { return HttpContext.Current.Request.ApplicationPath; } }

        public static string CookieName { get { return "LSKYSDGUESTCONTROL"; } }
        public static string Domain { get { return "LSKYSD.CA"; } }

        public static string ADUsername
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ADUsername"].Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }
        } 

        public static string ADPassword
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ADPassword"].Trim();
                }
                catch
                {
                    return string.Empty;
                }
            }
        }

        public static int AllowedRequisitionsPerDay
        {
            get
            {
                try
                {
                    return Parsers.ParseInt(ConfigurationManager.AppSettings["AllowedRequisitionsPerDay"].Trim());
                }
                catch
                {
                    return 1;
                }
            }
        }

        public static int MaxBatchSize
        {
            get
            {
                try
                {
                    return Parsers.ParseInt(ConfigurationManager.AppSettings["MaxBatchSize"].Trim());
                }
                catch
                {
                    return 1;
                }
            }
        }
    }
}