using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Model
{
    public class LoginSession
    {
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string Thumbprint { get; set; }
        public string UserAgent { get; set; }

        public bool CanUseBatches { get; set; }
        public bool CanBypassLimits { get; set; }
        public bool CanViewLogs { get; set; }

        public DateTime SessionStarts { get; set; }
        public DateTime SessionEnds { get; set; }

        public int ActivationLimit
        {
            get
            {
                if (this.CanBypassLimits)
                {
                    return int.MaxValue;
                }
                else
                {
                    return Settings.AllowedRequisitionsPerDay;
                }
            }
        }

    }
}