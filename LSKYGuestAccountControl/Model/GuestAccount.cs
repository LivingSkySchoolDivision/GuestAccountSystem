using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl
{
    public class GuestAccount
    {
        public string GivenName { get; set; }
        public string SN { get; set; }
        public string sAMAccountName { get; set; }
        public string DN { get; set; }
        public DateTime Expires { get; set; }
        public string Comment { get; set; }
        public string Description { get; set; }
        public bool IsEnabled { get; set; }
        public bool IsExpired { get; set; }

        public override string ToString()
        {
            return this.DN;
        }
    }
}