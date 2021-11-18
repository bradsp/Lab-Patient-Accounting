using System;
using System.Text;

namespace RFClassLibrary
{
    /// <summary>
    /// 
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static StringBuilder AppendFixed(this StringBuilder sb, int length, string value)
        {
            if (String.IsNullOrWhiteSpace(value))
                return sb.Append(String.Empty.PadRight(length));

            if (value.Length <= length)
                return sb.Append(value.PadRight(length));
            else
                return sb.Append(value.Substring(0, length));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="length"></param>
        /// <param name="value"></param>
        /// <param name="rest"></param>
        /// <returns></returns>
        public static StringBuilder AppendFixed(this StringBuilder sb, int length, string value, out string rest)
        {
            rest = String.Empty;

            if (String.IsNullOrWhiteSpace(value))
                return sb.AppendFixed(length, value);

            if (value.Length > length)
                rest = value.Substring(length);

            return sb.AppendFixed(length, value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sb"></param>
        /// <param name="text"></param>
        /// <param name="delim"></param>
        /// <param name="writeEmptyString"></param>
        public static void AppendDelim(this StringBuilder sb, string text, string delim = ", ", bool writeEmptyString = true)
        {
            if (writeEmptyString || !String.IsNullOrWhiteSpace(text))
            {
                if (sb.Length != 0)
                {
                    sb.Append(delim + text);
                }
                else
                {
                    sb.Append(text);
                }
            }
        }
    }
}
