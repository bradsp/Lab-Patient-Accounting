using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Utilities
{
    /// <summary>
    /// DateTime Helper methods
    /// </summary>
    public static class DateTimeHelper
    {
        /// <summary>
        /// Returns a datetime object for yesterday at the same time.
        /// </summary>
        /// <returns>Yesterday date at same time.</returns>
        public static DateTime Yesterday()
        {
            return DateTime.Now.AddDays(-1);
        }

        /// <summary>
        /// Returns a datetime object for the last day of the previous month.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastDayOfPrevMonth()
        {
            return new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddDays(-1);
        }

        /// <summary>
        /// Returns a datetime object for the last day of the current month.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastDayOfCurrentMonth()
        {
            int month = DateTime.Today.Month;

            return new DateTime(DateTime.Today.Year, DateTime.Today.AddMonths(1).Month, 1).AddDays(-1);
        }

    }


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
        /// <returns>A date time with the computed date, or MinValue if not a valid expression.</returns>
        public static DateTime ParseExpression(this DateTime dt, string expression)
        {
            //expect an expression such as "T" for today, "T-1" for yesterday, "T+1" for tomorrow, etc.

            string pattern = @"([Tt])([+-]\d+)?";
            Regex rgx = new Regex(pattern);
            Match m = rgx.Match(expression);

            if (m.Success)
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
            if (DateTime.TryParse(datestring, out DateTime dt))
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
            DateTime myDateTime = DateTime.MinValue;
          
            return myDateTime.ValidateDate(datestring);
        }

        /// <summary>
        /// Takes a date string and parses the date.
        /// </summary>
        /// <param name="datestring"></param>
        /// <returns>Valid dates are returned as DateTime. Invalid date returns null</returns>
        public static DateTime? ValidateDateOrNull(string datestring)
        {
            DateTime myDateTime = DateTime.MinValue;
            if (string.IsNullOrWhiteSpace(datestring))
            {
                return null;
            }
            else
            {
                myDateTime = myDateTime.ValidateDate(datestring);
                if (myDateTime == DateTime.MinValue)
                    return null;
                else
                    return myDateTime;
            }
        }


        /// <summary>
        /// Takes a date string and parses the date.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="datestring"></param>
        /// <returns>Valid dates are returned as DateTime. Invalid date returns DateTime.MinValue</returns>
        public static DateTime ValidateDate(this DateTime dateTime, string datestring)
        {
            if((datestring.Length == 6 || datestring.Length == 8) && !datestring.Contains("/"))
            {
                datestring = datestring.Insert(4, "/");
                datestring = datestring.Insert(2, "/");
            }
            // mmddyyyyhhmm
            if(datestring.Length == 12 && !datestring.Contains("/"))
            {
                datestring = datestring.Insert(10, ":");
                datestring = datestring.Insert(8, " ");
                datestring = datestring.Insert(4, "/");
                datestring = datestring.Insert(2, "/");
            }

            if (DateTime.TryParse(datestring, out DateTime dt))
                return dt;
            else
                return DateTime.MinValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="datestring"></param>
        /// <returns></returns>
        public static DateTime ParseHL7Date(this DateTime dateTime, string datestring)
        {

            if(datestring.Length == 8)
            {
                dateTime = DateTime.ParseExact(datestring, "yyyyMMdd", CultureInfo.InvariantCulture);
            }
            else if(datestring.Length == 12)
            {
                dateTime = DateTime.ParseExact(datestring, "yyyyMMddHHmm", CultureInfo.InvariantCulture);
            }
            else if(datestring.Length == 14)
            {
                dateTime = DateTime.ParseExact(datestring, "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            }
            else
            {
                dateTime = DateTime.MinValue;
            }

            return dateTime;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentDate"></param>
        /// <param name="weeks"></param>
        /// <returns></returns>
        public static DateTime AddWeeks(this DateTime currentDate, int weeks)
        {
            return DateTime.Now.AddDays(weeks * 7);
        }

        /// <summary>
        /// Gets the last date of the month of the DateTime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Gets the last date of the previous month of the Datetime.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static DateTime GetLastDayOfPrevMonth(this DateTime dateTime)
        {
            return new DateTime(dateTime.Year, dateTime.Month, 1).AddDays(-1);
        }


        /// <summary>
        /// Returns datetime corresponding to last day of the month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOfTheMonth(this DateTime date)
        {
            var endOfTheMonth = new DateTime(date.Year, date.Month, 1)
                .AddMonths(1)
                .AddDays(-1);

            return endOfTheMonth;
        }

        /// <summary>
        /// Adds time to existing DateTime
        /// </summary>
        /// <param name="date"></param>
        /// <param name="hour"></param>
        /// <param name="minutes"></param>
        /// <returns></returns>
        public static DateTime AddTime(this DateTime date, int hour, int minutes)
        {
            return date + new TimeSpan(hour, minutes, 0);
        }

        /// <summary>
        /// Inspiration for this extension method was another DateTime extension that determines difference in current time and a DateTime object. That one returned a string and it is more useful for my applications to have a TimeSpan reference instead. That is what I did with this extension method.
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static TimeSpan TimeElapsed(this DateTime date)
        {
            return DateTime.Now - date;
        }

        /// <summary>
        /// Prints out a nullable datetime's value (if its not null) in the string format specified as a parameter. A final parameter is specified for what to print if the nullable datetime was, in fact, null.
        /// </summary>
        /// <param name="dt"></param>
        /// <param name="format"></param>
        /// <param name="nullResult"></param>
        /// <returns></returns>
        public static string NullDateToString(this DateTime? dt, string format = "M/d/yyyy", string nullResult = "")
        {
            if (dt.HasValue)
                return dt.Value.ToString(format);
            else
                return nullResult;
        }

        /// <summary>
        /// Returns datetime corresponding to first day of the month
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime BeginningOfTheMonth(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, 1);
        }

        /// <summary>
        /// Returns datetime corresponding to day beginning
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime BeginningOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day);
        }

        /// <summary>
        /// Returns datetime corresponding to day end
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public static DateTime EndOfTheDay(this DateTime date)
        {
            return new DateTime(date.Year, date.Month, date.Day, 23, 59, 59);
        }

        /// <summary>
        /// Returns whether the given date is the last day of the month.
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static bool IsLastDayOfTheMonth(this DateTime dateTime)
        {
            return dateTime == new DateTime(dateTime.Year, dateTime.Month, 1).AddMonths(1).AddDays(-1);
        }

        /// <summary>
        /// Compares date with start and end dates.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>Return True if date is between start and end dates, otherwise False.</returns>
        public static bool IsBetween(this DateTime? date, DateTime start, DateTime end)
        {
            if (date == null)
                return false;

            return date >= start && date <= end;
        }

        /// <summary>
        /// Compares date with start and end dates.
        /// </summary>
        /// <param name="date"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns>Return True if date is between start and end dates, otherwise False.</returns>
        public static bool IsBetween(this DateTime date, DateTime start, DateTime end)
        {
            return date >= start && date <= end;
        }

        /// <summary>
        /// Parses string returning a nullable DateTime
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static DateTime? Parse(string text)
        {
            DateTime date;
            return DateTime.TryParse(text, out date) ? date : (DateTime?)null;
        }


    }
}
