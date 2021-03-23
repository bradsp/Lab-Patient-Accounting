using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace RFClassLibrary
{

    /// <summary>
    /// Extension methods for DateTime
    /// </summary>
    public static class DateTimeExtension
    {
        /// <summary>
        /// Returns a DateTime object based on the provided shorthand expression (T = today's date, T-n (n days ago), T+n (n days ahead))
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static DateTime ParseExpression(this DateTime dt, string expression)
        {
            //expect an expression such as "T" for today, "T-1" for yesterday, "T+1" for tomorrow, etc.

            string pattern = @"([Tt])([+-]\d+)?";
            Regex rgx = new Regex(pattern);
            Match m = rgx.Match(expression);

            if(m.Success)
            {
                if (expression == "T" || expression == "t")
                {
                    dt = DateTime.Today;
                }
                else
                {
                    string oper = m.Groups[2].Value;
                    int days = Convert.ToInt32(m.Groups[2].Value);

                    dt = DateTime.Today.AddDays(days);
                }
            }
            else
            {
                dt = DateTime.MinValue;
            }

            return dt;
        }

        /// <summary>
        /// Tests the provided string to see if it is a valid shorthand expression (T, T-n, T+n). 
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="expression"></param>
        /// <returns>Returns true if it matches.</returns>
        public static bool IsExpression(this DateTime dt, string expression)
        {
            string pattern = @"([Tt])([+-]\d+)?";
            if (Regex.IsMatch(expression, pattern))
                return true;
            else
                return false;

        }

        /// <summary>
        /// Takes a date string and parses the date. 
        /// </summary>
        /// <param name="datestring"></param>
        /// <returns>Valid dates are returned as DateTime. Invalid returns null.</returns>
        public static DateTime? ValidateDateNullable(string datestring)
        {
            DateTime dt;
            if (DateTime.TryParse(datestring, out dt))
                return dt;
            else
                return null;
        }

        /// <summary>
        /// Takes a date string and parses the date.
        /// </summary>
        /// <param name="datestring"></param>
        /// <returns>Valid dates are returned as DateTime. Invalid date returns DateTime.MinValue</returns>
        public static DateTime ValidateDate(string datestring)
        {
            DateTime dt;
            if (DateTime.TryParse(datestring, out dt))
                return dt;
            else
                return DateTime.MinValue;
        }


    }
}
