using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.Static
{
    public static class Helpers
    {
        public static string SanitizeInput(string thisString)
        {
            // todo: actually make this sanitize stuff
            return thisString.Trim();
        }
    }
}