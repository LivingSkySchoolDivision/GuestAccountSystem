using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace LSKYGuestAccountControl.ExtensionMethods
{
    public static class ExtensionMethodsLists
    {
        public static string ToCommaSeparatedString<T>(this List<T> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (T item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }
        public static string ToSemicolenSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(";");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }
        public static string ToCommaSeparatedString(this List<int> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (int item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

        public static string ToCommaSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(", ");
            }

            if (returnMe.Length > 2)
            {
                returnMe.Remove(returnMe.Length - 2, 2);
            }

            return returnMe.ToString();
        }

        public static string ToSemicolenSeparatedString(this List<string> list)
        {
            StringBuilder returnMe = new StringBuilder();

            foreach (string item in list)
            {
                returnMe.Append(item);
                returnMe.Append(";");
            }

            if (returnMe.Length > 1)
            {
                returnMe.Remove(returnMe.Length - 1, 1);
            }

            return returnMe.ToString();
        }
    }
}