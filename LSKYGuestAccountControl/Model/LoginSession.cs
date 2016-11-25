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
        public DateTime SessionStarts { get; set; }
        public DateTime SessionEnds { get; set; }
    }
}