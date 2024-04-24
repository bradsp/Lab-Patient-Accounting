using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Utilities;

/// <summary>
/// Custom extention methods
/// </summary>
public static class StringExtensions
{
    /// <summary>
    /// Returns the leftmost number of characters from a string
    /// </summary>
    /// <param name="str"></param>
    /// <param name="numChar"></param>
    /// <returns></returns>
    public static string Left(this string str, int numChar)
    {
        if (numChar < str.Length)
            return str[0..numChar];
        else
            return str;
    }


    /// <summary>
    /// Returns the right-most number of characters from a string
    /// </summary>
    /// <param name="str"></param>
    /// <param name="numChar"></param>
    /// <returns></returns>
    public static string Right(this string str, int numChar)
    {
        int len = str.Length;

        if (numChar < len)
            return str[(len - numChar)..];
        else
            return str;

    }

    /// <summary>
    /// Reduce multiple whitespace characters to one space and trim string.
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
    /// Extension method to remove all instances of a char in a list of chars.
    /// </summary>
    /// <param name="str"></param>
    /// <param name="charsToRemove"></param>
    /// <returns></returns>
    public static string Filter(this string str, List<char> charsToRemove)
    {
        foreach (char c in charsToRemove)
        {
            str = str.Replace(c.ToString(), string.Empty);
        }

        return str;
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
            temp = $"({temp[..3]}) {temp.Substring(3, 3)}-{temp.Substring(6, 4)}";
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
    /// <summary>
    /// Strips the leading zeros or blanks from the parameter string and returns the results
    /// </summary>
    static public string StripZeros(string str)
    {
        string strRetVal = str;
        int nLen = strRetVal.Length;
        if (nLen == 0)
        {
            return strRetVal;
        }
        for (int i = 0; i < nLen; i++)
        {
            if (strRetVal[i] != '0' || strRetVal[i] == ' ')
            {
                return strRetVal[i..nLen];
            }
        }
        return strRetVal;
    }
    //-----

    /// <summary>
    /// Necessary to put the WINS R1 into a 10 digit date of birth formatted
    /// mm/dd/yyyy 
    /// 
    /// wdk 20100218
    /// </summary>
    /// <param name="strHL7Date"></param>
    /// <returns></returns>
    static public string parseHL7DateStringToSqlStringDate(string strHL7Date)
    {
        string strRetVal = string.Empty;
        if (string.IsNullOrEmpty(strHL7Date))
        {
            return strRetVal;
        }

        string strYear = strHL7Date[..4];
        string strMo = strHL7Date.Substring(4, 2);
        string strDay = strHL7Date.Substring(6, 2);
        strRetVal = $"{strMo}/{strDay}/{strYear}";
        return strRetVal;

    }

    /// <summary>
    /// ParseCityStZip will split a string with City,ST Zip00 into its parts.
    /// 2019-09-21 Bradley Powers
    /// </summary>
    /// <param name="strCityStZip"></param>
    /// <param name="strCity"></param>
    /// <param name="strState"></param>
    /// <param name="strZip"></param>
    /// <returns>true = success
    /// false = error - could not parse
    /// </returns>
    public static bool ParseCityStZip(string strCityStZip, out string strCity, out string strState, out string strZip)
    {
        bool retVal = true; //success
        //clear out hte vars to ensure they are empty if we fail
        strCity = "";
        strState = "";
        strZip = "";
        if (strCityStZip == null) // cannot process null string
            return false;

        int iComma = strCityStZip.IndexOf(',');
        if (iComma > 1)
        {
            strCity = strCityStZip[..iComma];
            strCity.Trim();

            string strWork = strCityStZip[(iComma + 1)..];
            strWork = strWork.Trim();
            int iSpace = strWork.IndexOf(' ');
            if (iSpace > 0)
            {
                strState = strWork[..iSpace];
                strState.Trim();
                strZip = strWork[(iSpace + 1)..];
                strZip.Trim();
            }
            else
            {
                //there is no state or zip - return false for failure
                retVal = false;
            }
        }
        else
        {
            retVal = false;
        }

        return retVal;
    }


    ///<summary>
    /// ParseName will split a string with LastName Generation,FirstName MidName into its parts.
    /// LastName will include generation JR III ect.
    /// 08/02/2006 Rick Crone
    ///</summary>
    ///<returns>
    /// true = success
    /// false = error - could not parse - m_strErrMsg will contain more info on failure
    ///</returns>
    public static bool ParseName(string strName, out string strLastName, out string strFirstName, out string strMidName, out string strSuffix)
    {
        bool bRetVal = true; // success
        //clear the out vars to insure they are empty if we fail
        strLastName = "";
        strFirstName = "";
        strMidName = "";
        strSuffix = "";

        if (strName == null)
        {
            //cannot process a null value - return false
            return false;
        }

        //find comma
        int iComma = strName.IndexOf(',');
        if (iComma > 1)
        {
            strLastName = strName.Substring(0, iComma);
            strLastName.Trim();
            //lastname should have lastname and suffix now
            int iSpace = strLastName.IndexOf(' ');
            if (iSpace > 0) //suffix exists
            {
                strSuffix = strLastName.Substring(iSpace + 1);
                strSuffix.Trim();
                strLastName = strLastName.Substring(0, iSpace);
            }

            string strWork = strName.Substring(iComma + 1);
            strWork = strWork.Trim(); // work should have first and mid now
            iSpace = strWork.IndexOf(' ');
            if (iSpace > 0)
            {
                strFirstName = strWork[..iSpace];
                strMidName = strWork.Substring(iSpace + 1);
            }
            else
            {
                // no middle name or initial
                strFirstName = strWork;
            }
        }
        else
        {
            //m_strErrMsg = "No comma in name. Can NOT parse";
            bRetVal = false;
        }
        return (bRetVal);
    }//end of ParseName()

    /// <summary>
    /// Calls the SSplitString (static version)
    /// 08/27/2007 wdk
    /// </summary>
    /// <param name="strToSplit"></param>
    /// <returns></returns>
    public static string SplitString(string strToSplit)
    {
        return SSplitString(strToSplit);
    }

    /// <summary>
    /// Use this function statically to split a string a near the center as possible. 
    /// Currently used in ViewerPrint for footer text.
    /// If the back half of the string does not contain a space the split is moved forward of center 
    /// to the last space in the first half of the string.
    /// 
    /// 08/28/2007 wdk/rgc
    /// </summary>
    /// <param name="strToSplit"></param>
    /// <returns></returns>
    public static string SSplitString(string strToSplit)
    {
        if (strToSplit.Length <= 0)
        {
            //nothing to split
            return strToSplit;
        }
        string strRetVal = "";
        // remove any carrage returns
        strToSplit = strToSplit.Replace(Environment.NewLine, " "); // take out all of the newlines
        strToSplit = strToSplit.TrimStart(new char[] { ' ' });  // remove spaces from the front.
        strToSplit = strToSplit.TrimEnd(new char[] { ' ' });  // remove spaces from the end.

        int nCenter;
        if (strToSplit.IndexOf(' ') > -1) // we have a space
        {
            nCenter = strToSplit.Length / 2; // find the center of the string. This will round down for strings with an odd number of characters but we don't care.
            while (strToSplit[nCenter] != ' ')
            {
                if (strToSplit.IndexOf(" ", nCenter) > -1)
                {
                    nCenter++;
                }
                else
                {
                    while (strToSplit[nCenter] != ' ')
                    {
                        if (strToSplit.IndexOf(" ", 0, nCenter) > -1)
                        {
                            nCenter--;
                        }
                    }
                }
            }
            strRetVal = string.Format("{0}|{1}", strToSplit.Substring(0, nCenter), strToSplit.Substring(nCenter, strToSplit.Length - nCenter));
        }
        return strRetVal;

    }

    /// <summary>
    /// Converts string to byte array
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static byte[] StrToByteArray(string str)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetBytes(str);
    }
    /// <summary>
    /// Converts byte array to string
    /// </summary>
    /// <param name="byteArray"></param>
    /// <returns></returns>
    public static string ByteArrayToStr(byte[] byteArray)
    {
        System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
        return encoding.GetString(byteArray);
    }
}
