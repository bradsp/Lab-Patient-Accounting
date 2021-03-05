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
                if (expression == "T")
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
        /// Tests the provided string to see if it is a valid shorthand expression (T, T-n, T+n). Returns true if it matches.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static bool IsExpression(this DateTime dt, string expression)
        {
            string pattern = @"(Tt)([+-]\d+)?";
            if (Regex.IsMatch(expression, pattern))
                return true;
            else
                return false;

        }

    }
}
