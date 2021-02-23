using System;

namespace RFClassLibrary
{
    /// <summary>
    /// String Manipulation Class
    /// </summary>
    public static class Str
    {

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
                    return strRetVal.Substring(i, nLen - i);
                }
            }
            return strRetVal;
        }
        //-----

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        static public string WhoAmI()
        {
            string strUserName;
            strUserName = System.Security.Principal.WindowsIdentity.GetCurrent().Name.ToString();
            //UserName.Text = a;
            //MessageBox.Show(a.ToString());
            return (strUserName);
        }
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

            string strYear = strHL7Date.Substring(0, 4);
            string strMo = strHL7Date.Substring(4, 2);
            string strDay = strHL7Date.Substring(6, 2);
            strRetVal = string.Format("{0}/{1}/{2}", strMo, strDay, strYear);
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
                strCity = strCityStZip.Substring(0, iComma);
                strCity.Trim();

                string strWork = strCityStZip.Substring(iComma + 1);
                strWork = strWork.Trim();
                int iSpace = strWork.IndexOf(' ');
                if (iSpace > 0)
                {
                    strState = strWork.Substring(0, iSpace);
                    strState.Trim();
                    strZip = strWork.Substring(iSpace + 1);
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
                    strFirstName = strWork.Substring(0, iSpace);
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
    } // don't type below this line
}
