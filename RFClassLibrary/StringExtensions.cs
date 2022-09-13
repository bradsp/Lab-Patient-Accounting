using System;
using System.Text.RegularExpressions;
using System.Linq;

namespace RFClassLibrary
{
    /// <summary>
    /// Custom extention methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// This is the extension method.
        /// The first parameter takes the "this" modifier and specifies the type for which the method is defined.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string TrimAndReduce(this string str)
        {
            return ConvertWhitespacesToSingleSpaces(str).Trim();
        }

        /// <summary>
        /// Extension method to convert all whitespace in a string to single spaces
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ConvertWhitespacesToSingleSpaces(this string value)
        {
            return Regex.Replace(value, @"\s+", " ");
        }

        /// <summary>
        /// Returns string formatted as an SSN (inserts dashes).
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatSSN(this string str)
        {
            if (str == null)
                return "";
            string temp = str;
            if (str.Length == 9)
            {
                temp = temp.Insert(5, "-");
                temp = temp.Insert(3, "-");
            }
            return temp;
        }

        /// <summary>
        /// Returns string formatted as Phone number. If string is not exactly 10 characters, no formatting is performed.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FormatPhone(this string str)
        {
            if (str == null)
                return "";
            string temp = str;

            if (temp.Length == 10)
            {
                temp = string.Format("({0}) {1}-{2}",
                    temp.Substring(0, 3), temp.Substring(3, 3), temp.Substring(6, 4));
            }
            return temp;
        }

        /// <summary>
        /// Performs a case-insenstive string comparison
        /// </summary>
        /// <param name="source"></param>
        /// <param name="substring"></param>
        /// <returns></returns>
        public static bool ContainsCaseInsensitive(this string source, string substring)
        {
            return source?.IndexOf(substring, System.StringComparison.OrdinalIgnoreCase) > -1;
        }

        /// <summary>
        /// Determines if string is included in a list of items.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="items"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool In(this string item, params string[] items)
        {
            if (items == null)
                throw new ArgumentNullException("items");

            return items.Contains(item);
        }

        /// <summary>
        /// Returns passed string if 1st string is null or empty.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="item2"></param>
        /// <returns></returns>
        public static string Coalesce(this string item, string item2)
        {
            if (string.IsNullOrEmpty(item))
                return item2;
            else
                return item;
        }

        /// <summary>
        /// Date/Time from HL7 Specifications
        ///  TYPE     LEN   DATA NAME   Reference   Notes/Format
        ///  DT        8       Date        2.8.13      YYYY[MM[DD]] 
        ///  TM       18       Time        2.8.39      HH[MM[SS[.S[S[S[S]]]]]][+/-ZZZZ] (not used in our HL7 as of this date)
        ///  TS       26        Time stamp  2.8.42      YYYY[MM[DD[HHMM[SS[.S[S[S[S]]]]]]]][+/-ZZZZ] ^ [degree of precision]
        /// wdk 20090320 
        /// Returns string representation in Sql Date acceptable format
        /// 
        /// for TS date >= 14 returns MM/DD/CCYY HHMM SS
        /// for TS date between 9 and 13 returns MM/DD/CCYY
        /// for TS date or DT date equal 8 returns MM/DD/CCYY
        /// 
        /// Any date that is less than 8 blank is returned.
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ConvertHL7DateToSqlDate(this string str)
        {
            /*
             Date/Time from HL7 Specifications
             TYPE        DATA NAME   Reference   Notes/Format
             DT          Date        2.8.13      YYYY[MM[DD]] 
             TM          Time        2.8.39      HH[MM[SS[.S[S[S[S]]]]]][+/-ZZZZ]
             TS          Time stamp  2.8.42      YYYY[MM[DD[HHMM[SS[.S[S[S[S]]]]]]]][+/-ZZZZ] ^ <degree of precision>
             */

            string strRetVal;
            switch (str.Length)
            {
                case 8:
                case 9:
                case 10:
                case 11:
                case 12:
                case 13:
                {
                    strRetVal = string.Format("{0}/{1}/{2}",
                        str.Substring(4, 2), str.Substring(6, 2), str.Substring(0, 4)); // MM/DD/CCYY for 8

                    break;
                }

                case 14:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 23:
                case 24:
                case 25:
                case 26:  // hl7 timestamp
                {
                    strRetVal = string.Format("{0}/{1}/{2} {3}{4} {5}",
                        str.Substring(4, 2), str.Substring(6, 2), str.Substring(0, 4), // MM/DD/CCYY for 8
                        str.Substring(8, 2), str.Substring(10, 2), str.Substring(12, 2) // HHMM SS 6
                        );
                    break;
                }
                default:
                {
                    strRetVal = string.Empty;
                    break;
                }
            }
            return strRetVal;
        }

    }
}
