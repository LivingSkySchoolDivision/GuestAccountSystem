using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Model
{
    public class LoggedActivation
    {
        public DateTime Date { get; set; }
        public string GuestAccountName { get; set; }
        public string RequestingUser { get; set; }
        public string Reason { get; set; }
        public string IPAddress { get; set; }
        public string UserAgent { get; set; }
    }
}