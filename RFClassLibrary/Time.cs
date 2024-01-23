using System;
using System.ComponentModel;

namespace Utilities
{
    /// <summary>
    /// Summary description for Time.
    /// </summary>
    public class Time
    {
        /// <!-- string to store error message 
        ///   - if this class was dervied from RFCObject
        ///   we wouldn't need this -->
        public static string m_strErrMsg;

        /// <summary>
        /// Empty constructor
        /// </summary>
        public Time()
        {

        }

        //==============================
        /// <summary>
        /// wait(milliseconds) 1000 = 1 sec
        /// 04/01/2003 Rick Crone 
        /// </summary>
        public static void Wait(int milliseconds)
        {
            //string msg = string.Format("Entering wait({0})",milliseconds);
            //WriteLogFile(msg);
            System.Threading.Thread.Sleep(milliseconds);
            //msg = string.Format("Leaving wait({0})",milliseconds);
            //WriteLogFile(msg);
        }
        //===============================
        /// <summary>
        /// returns string with date and time including seconds
        /// 05/08/2003 Rick Crone
        /// </summary>
        public static string Nows()
        {
            DateTime now = DateTime.Now;
            string strRetVal = "Time comming soon!";
            //strRetVal = now.ToString("MM/dd/yy HH:mm:ss:ffff");
            strRetVal = now.ToString("MM/dd/yy HH:mm:ss");
            return (strRetVal);

        }

        /// <summary>
        /// static function to get date time as a string iDays ago
        /// time at 1 min after midnight.
        /// This function developed for use with the QpmReq FORD service.
        /// 04/25/2007 Rick Crone
        /// </summary>
        /// <returns>String with the date and time iDays hours ago</returns>
        public static string DaysAgo(int iDays)
        {
            DateTime dt = DateTime.Now;
            TimeSpan days = new TimeSpan(iDays, 0, 0, 0);
            dt = dt - days;
            string strRetVal = dt.ToString("MM/dd/yy 00:01");
            return strRetVal;
        }


        /// <summary>
        /// returns string with date and time including seconds
        /// static version - no instance needed
        /// 05/08/2003 Rick Crone 
        /// </summary>
        /// <returns>returns string with date and time including seconds</returns>
        public static string SNows()
        {
            DateTime now = DateTime.Now;
            string strRetVal = "Time comming soon!";
            //strRetVal = now.ToString("MM/dd/yy HH:mm:ss:ffff");
            strRetVal = now.ToString("MM/dd/yy HH:mm:ss");
            return (strRetVal);

        }

        /// <summary>
        /// static function - returns string with HL7 timestamp for current time
        /// 07/27/2006 Rick Crone
        /// 03/28/2007 Rick and David new version w/o seconds.
        /// </summary>
        /// <returns>returns string with date and time including seconds in HL7 format</returns>
        public static string HL7TimeStampNoSeconds()
        {
            //throw new System.NotImplementedException();
            DateTime now = DateTime.Now;
            string strRetVal = "Time comming soon!";
            //strRetVal = now.ToString("MM/dd/yy HH:mm:ss:ffff");
            strRetVal = now.ToString("yyyyMMddHHmm");
            return (strRetVal);
        }

        /// <summary>
        /// static function - returns string with HL7 timestamp for current time
        /// 07/27/2006 Rick Crone
        /// </summary>
        /// <returns>returns string with date and time including seconds in HL7 format</returns>
        public static string HL7TimeStamp()
        {
            //throw new System.NotImplementedException();
            DateTime now = DateTime.Now;
            string strRetVal = "Time comming soon!";
            //strRetVal = now.ToString("MM/dd/yy HH:mm:ss:ffff");
            strRetVal = now.ToString("yyyyMMddHHmmss");
            return (strRetVal);
        }

        /// <summary>
        /// static function - returns string with HL7 timestamp for the DateTime passed in.
        /// 10/11/2006 Rick Crone
        /// </summary>
        /// <returns>returns string with date and time including seconds in HL7 format</returns>
        public static string DateTimeToHL7TimeString(DateTime dt)
        {
            //throw new System.NotImplementedException();
            string strRetVal = dt.ToString("yyyyMMddHHmmss");
            return (strRetVal);
        }
        /// <summary>
        /// static function - returns string with HL7 timestamp for the object passed in.
        /// This overload allows caller to pass in object without having to cast to DateTime
        /// 10/11/2006 Rick Crone
        /// </summary>
        /// <returns>returns string with date and time including seconds in HL7 format</returns>

        public static string DateTimeToHL7TimeString(object o)
        {
            //throw new System.NotImplementedException();
            DateTime dt = (DateTime)o;
            string strRetVal = dt.ToString("yyyyMMddHHmmss");
            return (strRetVal);

        }
        /// <summary>
        /// Returns a string with the HL7 Date (only - no time).
        /// Useful for DOB.
        /// Rick Crone 10/11/2006
        /// </summary>
        /// <param name="o">an object that can be cast to a DateTime</param>
        /// <returns></returns>
        public static string DateTimeToHL7DateString(object o)
        {
            //throw new System.NotImplementedException();
            DateTime dt = (DateTime)o;
            string strRetVal = dt.ToString("yyyyMMdd");
            return (strRetVal);
        }
        /// <summary>
        /// Converts a string in HL7 format ccyymmddhhmmss  or ccyymmdd to a DateTime value.
        /// Be sure to check the return value... if the string can NOT be parsed
        /// the DateTime will be now.
        /// 08/26/2008 wdk modified to handle 12 digit date (Meditech dates - 200808261313)
        /// 12/11/2007 wdk/rgc modified to handle 8 digit date
        /// 
        ///  08/30/2006 Rick Crone
        /// </summary>
        /// <param name="strHL7DateTime"> format:ccyymmddhhmmss</param>
        /// <param name="dt">out var to hold the DateTime value</param>
        /// <returns>
        /// true // success
        /// false // false
        /// </returns>
        public static bool StringToHL7Time(string strHL7DateTime, out DateTime dt)
        {
            bool bRetVal = false;
            dt = DateTime.Now;
            string strWork;// to reformat string so it can be parsed
            if (strHL7DateTime.Length == 14)
            {
                //01234567890123
                //ccyymmddhhmmss
                strWork = string.Format("{0}/{1}/{2} {3}:{4}:{5}",
                                strHL7DateTime.Substring(4, 2),
                                 strHL7DateTime.Substring(6, 2),
                                  strHL7DateTime.Substring(0, 4),
                                   strHL7DateTime.Substring(8, 2),
                                    strHL7DateTime.Substring(10, 2),
                                     strHL7DateTime.Substring(12));

                try
                {
                    dt = DateTime.Parse(strWork);
                    bRetVal = true;
                }
                catch (FormatException)
                {
                    bRetVal = false;
                    m_strErrMsg = "FormatException in StringToHL7Time()";

                }
            }

            // 08/26/2008 wdk 12 digit dates
            //200808261313
            if (strHL7DateTime.Length == 12)
            {
                //01234567890123
                //ccyymmddhhmmss
                strWork = string.Format("{0}/{1}/{2} {3}:{4}",
                                strHL7DateTime.Substring(4, 2),
                                 strHL7DateTime.Substring(6, 2),
                                  strHL7DateTime.Substring(0, 4),
                                   strHL7DateTime.Substring(8, 2),
                                    strHL7DateTime.Substring(10));
                try
                {
                    dt = DateTime.Parse(strWork);
                    bRetVal = true;
                }
                catch (FormatException)
                {
                    bRetVal = false;
                    m_strErrMsg = "FormatException in StringToHL7Time()";
                }
            }

            // 12/11/2007 wdk/rgc convert 8 digit date
            if (strHL7DateTime.Length == 8)
            {
                //01234567
                //ccyymmdd
                strWork = string.Format("{0}/{1}/{2} ",
                                strHL7DateTime.Substring(4, 2),
                                 strHL7DateTime.Substring(6, 2),
                                  strHL7DateTime.Substring(0, 4));

                try
                {
                    dt = DateTime.Parse(strWork);
                    bRetVal = true;
                }
                catch (FormatException)
                {
                    bRetVal = false;
                    m_strErrMsg = "FormatException in StringToHL7Time()";

                }
            }
            // end of 12/11/2007 mod
            return (bRetVal);
        }

        /// <summary>
        /// static member function to Convert a DateTime into a string.
        /// Formated "MM/dd/yy HH:mm"
        /// </summary>
        /// <param name="dtToConvert">DateTime</param>
        /// <returns></returns>
        static public string SDateTimeToString(DateTime dtToConvert)
        {
            return dtToConvert.ToString("MM/dd/yy HH:mm");
        }


        /// <summary>
        /// static member function to Convert an object that can be casted
        /// into a DateTime into a string.
        /// Formated "MM/dd/yy HH:mm"
        /// </summary>
        static public string SDateTimeToString(object o)
        {
            DateTime dtToConvert = (DateTime)o;
            return dtToConvert.ToString("MM/dd/yy HH:mm");
        }

        private static int[] monthDay = new int[12] { 31, -1, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
        /// <summary>
        /// returns the age on the date of service.
        /// </summary>
        /// <param name="dtDob"></param>
        /// <param name="dtThru"></param>
        /// <returns></returns>
        static public string GetAge(DateTime dtDob, DateTime dtThru)
        {
            int year;
            int month;
            int day;
            int increment = 0;
            if (dtDob.Day > dtThru.Day)
            {
                increment = monthDay[dtDob.Month - 1];
            }
            if (increment == -1)
            {
                increment = 28;
                if (DateTime.IsLeapYear(dtDob.Year))
                {
                    increment = 29;
                }
            }
            day = dtThru.Day - dtDob.Day;
            if (increment != 0)
            {
                day = (dtThru.Day + increment) - dtDob.Day;
                increment = 1;
            }

            if ((dtDob.Month + increment) > dtThru.Month)
            {
                month = (dtThru.Month + 12) - (dtDob.Month + increment);
                increment = 1;
            }
            else
            {
                month = (dtThru.Month) - (dtDob.Month + increment);
                increment = 0;
            }
            year = dtThru.Year - (dtDob.Year + increment);
            return (string.Format("{0} Years, {1} Months, {2} Days", year, month, day));

        }

        /// <summary>
        /// Computes the age from the strDOB and the strDateToComputeFrom. Useful for calculating
        /// the age of the individual on a collection date. If the strDateToComputeFrom is empty
        /// then the age is calculated from the current datetime.
        /// 
        /// Returns the age and the units in the string array if successful
        /// strARetVal[0] = 9
        /// strARetVal[1] = MO
        /// Returns an error message 
        /// strARetVal[0] = "ERROR"
        /// strARetVal[1] = "Could not convert {0} to a datetime." strDOB or strDateToComputeFrom
        /// </summary>
        /// <param name="strDOB"></param>
        /// <param name="strDateToComputeFrom"></param>
        /// <returns></returns>
        static public string[] GetAge(string strDOB, string strDateToComputeFrom)
        {
            string[] strARetVal = new string[2];
            if (strDateToComputeFrom.Length == 0)
            {
                strDateToComputeFrom = DateTime.Now.ToString("d");
            }
            DateTime dtDOB = DateTime.MinValue;
            if (!DateTime.TryParse(strDOB, out dtDOB))
            {
                strARetVal[0] = "ERROR";
                strARetVal[1] = string.Format("Could not convert {0} to a datetime", strDOB);
                return strARetVal;
            }
            DateTime dtFrom = DateTime.MinValue;
            if (!DateTime.TryParse(strDateToComputeFrom, out dtFrom))
            {
                strARetVal[0] = "ERROR";
                strARetVal[1] = string.Format("Could not convert {0} to a datetime", strDateToComputeFrom);
                return strARetVal;
            }
            TimeSpan tSpan = new TimeSpan();
            tSpan = dtFrom - dtDOB;

            if (tSpan.Days >= 365)
            {
                strARetVal[0] = Math.Floor((double)tSpan.Days / 365).ToString();
                strARetVal[1] = "YR";
            }
            else
            {
                if (tSpan.Days >= 30)
                {
                    strARetVal[0] = Math.Floor((double)tSpan.Days / 30).ToString();
                    strARetVal[1] = "MO";
                }
                else
                {
                    strARetVal[0] = tSpan.Days.ToString();
                    strARetVal[1] = "DY";
                }
            }

            return strARetVal;
        }

        /// <summary>
        /// wdk 20090420 Converts either a datetime or string to HL7 date without time
        /// </summary>
        /// <param name="o">String as MM/dd/CCYY or MM/dd/YY (converts to Y2K)
        ///                 datetime as CCYYMMDD or YYMMDD (converts to Y2K</param>
        /// <returns>formatted input as CCYYMMDD or ERROR if the object is not convertable or is blank</returns>
        public static string ConvertToHL7DateString(object o)
        {
            string strRetVal = "ERROR";
            string strTemp = string.Empty;
            if (string.IsNullOrEmpty(o.ToString()))
            {
                return strRetVal;
            }
            try
            {
                if (o.GetType() == typeof(string))
                {


                }
                if (o.GetType() == typeof(DateTime))
                {
                    strTemp = ((DateTime)o).ToString("s"); // returns CCYY-MM-DDTHH:mm:ss
                    strRetVal = strTemp.Split(new char[] { 'T' })[0].Replace("-", "");
                    // date passed in with only a 2 digit year not 4
                    if (strTemp.Substring(0, 2) == "00")
                    {
                        int nParseYearFromDate = int.Parse(strTemp.Substring(2, 2));
                        int nParseYearFromNow = int.Parse(DateTime.Now.Year.ToString().Substring(2, 2));
                        if (nParseYearFromDate > nParseYearFromNow)
                        {
                            nParseYearFromDate += 1900;
                            strRetVal = string.Format("{0}{1}{2}",
                                                    nParseYearFromDate.ToString(),
                                                       ((DateTime)o).Month,
                                                            ((DateTime)o).Day);
                        }
                        else
                        {
                            nParseYearFromDate += 2000;
                            // now if the month is greater and the year is the same use last century
                            if (((DateTime)o).Month == DateTime.Now.Month)
                            {
                                if (((DateTime)o).Day > DateTime.Now.Day)
                                {
                                    nParseYearFromDate -= 100;
                                }
                            }
                            if (((DateTime)o).Month < DateTime.Now.Month)
                            {
                                nParseYearFromDate -= 100;
                            }
                            strRetVal = string.Format("{0}{1}{2}",
                                                    nParseYearFromDate.ToString(),
                                                       ((DateTime)o).Month,
                                                            ((DateTime)o).Day);
                        }
                    }
                }
            }
            catch (InvalidCastException)
            {
                strRetVal = "ERROR";
            }
            return strRetVal;
        }

        /// <summary>
        /// converts datetimes and strings to a formatted HL7 date or datetime depending on bReturnTime varialbe
        /// You should ensure the date or datetime is valid before calling this method.
        /// 
        /// rgc/wdk 20090506
        /// </summary>
        /// <param name="o">DateTime or string</param>
        /// <param name="bReturnTime">Returns the time appended to the date string if true</param>
        /// <returns></returns>
        public static string ConvertObjectToDateTimeString(object o, bool bReturnTime)
        {
            string strRetval = string.Empty;
            //TypeDescriptor.GetConverter(dt).ConvertFrom(myStr));
            if (o.GetType() == typeof(string))
            {
                strRetval =
               ((DateTime)TypeDescriptor.GetConverter(new DateTime()).
                                                ConvertFrom(o)).ToString("yyyyMMdd");
                if (bReturnTime)
                {
                    strRetval = ((DateTime)TypeDescriptor.GetConverter(new DateTime()).
                                                    ConvertFrom(o)).ToString("yyyyMMddhhmmss");
                }
            }
            if (o.GetType() == typeof(DateTime))
            {
                strRetval = ((DateTime)o).ToString("yyyyMMdd");
                if (bReturnTime)
                {
                    strRetval = ((DateTime)o).ToString("yyyyMMddhhmmss");
                }
            }
            return strRetval;

        }
    }
}
