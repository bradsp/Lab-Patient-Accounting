using System.Text.RegularExpressions;

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
    }
}
