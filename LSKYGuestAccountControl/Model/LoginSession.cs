using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Model
{
    public class LoginSession
    {
        public string username { get; set; }
        public string ip { get; set; }
        public string hash { get; set; }
        public string useragent { get; set; }
        public DateTime starts { get; set; }
        public DateTime ends { get; set; }
    }
}