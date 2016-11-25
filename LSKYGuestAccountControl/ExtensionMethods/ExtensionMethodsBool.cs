using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LSKYGuestAccountControl.ExtensionMethods
{
    public static class ExtensionMethodsBool
    {
        public static string ToTrueOrFalse(this bool thisBool)
        {
            if (thisBool)
            {
                return "True";
            }
            else
            {
                return "False";
            }
        }

        public static string ToYesOrNoHTML(this bool thisBool)
        {
            if (thisBool)
            {
                return "<span style=\"color: #007700;\">Yes</span>";
            }
            else
            {
                return "<span style=\"color: #770000;\">No</span>";
            }
        }

        public static string ToYesOrNo(this bool thisBool)
        {
            if (thisBool)
            {
                return "Yes";
            }
            else
            {
                return "No";
            }
        }

        public static int ToOneOrZero(this bool thisBool)
        {
            if (thisBool)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}