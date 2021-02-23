/*
 * 09/14/2006 Rick Crone
 * This RFCObject class is intended primarily as a base class to
 * give our RFClassLibray classes a consitant way to manage error
 * message. It is public however so it could be instantiated if needed.
 * 
 * It may also include some basic functionality that most
 * classes would likely use. 
 * 
 */ 
using System;
using System.Collections.Generic;
using System.Text;
//added
using System.Windows.Forms;
using System.IO; // for File
using System.Reflection;
using System.Drawing; // for creating fonts


namespace RFClassLibrary
{
    /// <summary>
    /// base class for most RFC classes
    /// Rick Crone
    /// </summary>
    public class RFCObject
    {
         
        /// <summary>
        ///09/03/2008 wdk/rgc Added for Handling fatal error 
        /// rgc/wdk 20090609 changed from private to public for use with HL7 class.
        /// </summary>
        static public ERR m_ERR;

        //- static to access via derived classes
        
        /// <summary>
        /// string holds error message (and other) messages
        /// 09/14/2006 Rick Crone
        /// </summary>
        static public string m_strErrMsg;
        /// <summary>
        /// Gets or sets the value in m_strErrMsg
        /// rgc/wdk 20090609 now adds the error to the ERR class's m_dsError dataset via its AddErrorToDataSet();
        /// see ERR's DumpDataSetErrors()
        /// </summary>
        public string propErrMsg
        {
            set
            {
                m_strErrMsg = value;
                try
                {
                    m_ERR.AddErrorToDataSet(string.Format("INFO^{0}",Application.ProductName), m_strErrMsg);
                }
                catch (NullReferenceException)
                {
                    // m_ERR was created by a recordset that has not been updated to pass our m_ERR class
                    // but is derived from RFCObject.
                }
            }
            get
            {
                return m_strErrMsg;
            }
        }


        /// <!--just a temp work string -->
        static public string ms_strWork;
        /// <!--is the class valid - properly initalized -->
        public bool m_bValid;  //10/03/2007 wdk/rgc removed static and refactored
        
      

        /// <summary>
        /// gets the validity value
        /// </summary>
        public bool propValid
        {
            get
            {
                //throw new System.NotImplementedException();
                return m_bValid;// gets the validity value
            }
            set
            {
                //
            }
        }

        /// <summary>
        /// Get's the applications name with version
        /// The Application sets its version in the AssemblyInfo.cs
        /// <code>[assembly: AssemblyFileVersion("1.0.0.0")] // accessed as Application.ProductVersion property</code>
        /// 
        /// 09/19/2007 rgc/wdk updated to add version.
        /// </summary>
        public string propAppName
        {
            get
            {
                string strApp = Application.ProductName + " " + Application.ProductVersion;
                if (Application.ProductName.Trim() == ".Net SqlClient Data Provider")
                {
                    strApp = Assembly.GetExecutingAssembly().FullName;
                }
                   
                return strApp;
            }
            set
            {
            }
        }

        /// <summary>
        /// Get's the applications version and refence information
        /// </summary>
        public string propAppVersionAndRefInfo
        {
            
            get
            {
                string strRetVal;
                string strFullName;
                string strAssemblyNames;
                //Assembly asm =  Assembly.GetExecutingAssembly();
                Assembly asm = Assembly.GetEntryAssembly();//  .GetExecutingAssembly();
                strFullName = string.Format("File Version: {0}", asm.FullName);
               
    
                strAssemblyNames = string.Format("References :");
                AssemblyName[] asmNames = asm.GetReferencedAssemblies();
                foreach (AssemblyName nm in asmNames)
                {
                    strAssemblyNames += "\n";
                    strAssemblyNames += nm.FullName;
                }

            strRetVal = string.Format("{0}\n{1}", strFullName, strAssemblyNames);
            return(strRetVal);   
            }
        }
        
        
        /// <summary>
        ///  will this get a build date or an install date???
        /// 12/06/2006 Rick Crone
        /// </summary>
        public string propAppBuildDate
        {
            get
            {
                //return(File.GetCreationTime(Application.ExecutablePath).ToString());
                // GetLastWriteTime
                return(File.GetLastWriteTime(Application.ExecutablePath).ToString());
                //return Application.ProductName.ToString();
            }
            set
            {
            }
        }

        /// <summary>
        /// returns the path for the executable file, not including the executable name
        /// </summary>
        public string propAppStartupPath
        {
            get
            {
                return (Application.StartupPath);
            }
        }

        /// <summary>
        /// Complete path to executable includes the name and extention.
        /// 12/06/2006 Rick Crone
        /// </summary>
        public string propAppExecPath
        {
            get
            {
                
                return (Application.ExecutablePath);
            }
        }

        /// <summary>
        /// Complete path to executable includes the name and extention.
        /// 12/06/2006 Rick Crone
        /// </summary>
        public string propAppConfigFile
        {
            get
            {
                return (Application.ExecutablePath + ".config");
            }
        }

        /// <summary>
        /// Empty Constructor to maintain backwards compatability.
        /// 09/03/2008 wdk/rgc
        /// </summary>
        public RFCObject()
        {
        }

        /// <summary>
        /// Constructor for passing the reference to the ERR class
        /// </summary>
        /// <param name="m_err">used to the RFCObject's m_ERR</param>
        public RFCObject(ref ERR m_err)
        {
            m_ERR = m_err;
        }
        /// <summary>
        /// Display a MessageBox in Windows with the m_strErrMsg text in it.
        /// OR - Console display for Console apps!
        /// 01/04/2007 Rick Crone
        /// </summary>
        public void DispErrMsg()
        {
            MessageBox.Show(m_strErrMsg);
            Console.WriteLine(m_strErrMsg);
        }

        /// <summary>
        /// 11/15/2006 added to assist adding results to the sql tables from the mckession interface.
        /// can be used as any sql cleaner from the user when needed.
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        public string SqlClean(string strIn)
        {
	        // 09/09/2004 WDK and RGC attempt to avoid SQL injection attack
            strIn = strIn.Replace("'", "''");
            strIn = strIn.Replace("--", "");
            strIn = strIn.Replace("/*", "");
            strIn = strIn.Replace("*/", "");
            strIn = strIn.Replace(";", ""); // semi colon denotes end of one statement and start of another
        	
	        return strIn;

        }
        /// <summary>
        /// static version of sqlClean
        /// 11/15/2006 added to assist adding results to the sql tables from the mckession interface.
        /// can be used as any sql cleaner from the user when needed.
        /// </summary>
        /// <param name="strIn"></param>
        /// <returns></returns>
        static public string staticSqlClean(string strIn) 
        {
            // 09/09/2004 WDK and RGC attempt to avoid SQL injection attack
            strIn = strIn.Replace("'", "''");
            strIn = strIn.Replace("--", ""); // rest of line is a comment
            strIn = strIn.Replace("/*", ""); // from here to next line is comment
            strIn = strIn.Replace("*/", ""); // end of comment from above
            strIn = strIn.Replace(";", ""); // semi colon denotes end of one statement and start of another

            return strIn;

        }

        /// <summary>
        /// Throws an exception
        /// </summary>
        /// <param name="strErrMsg"></param>
        public static void ThrowException(string strErrMsg)
        {
            Exception e = new Exception(strErrMsg);
            throw e;
        }

        /// <summary>
        /// Creates From/Thru DateTime Controls on a ToolStrip. Must pass in two ToolStripControlHosts variables.
        /// Passing in the default datetimes is optional. If no value is passed then the 
        ///     From Date will be today minus two days and the
        ///     Thru Date will be today minus one day.
        /// 
        /// From the application call the three lines of code below.
        /// <code><example>
        ///     private void AddDateRangeControls()
        ///     {
        ///         RFClassLibrary.RFCObject.AddDateRangeControls(ref m_dpFrom, ref m_dpThru, "", "");
        ///         tsMain.Items.Insert(7, m_dpFrom);
        ///         tsMain.Items.Insert(9, m_dpThru);
        ///     }
        ///</example></code>
        /// 
        /// 09/21/2007 wdk
        /// </summary>
        /// <param name="dpFrom">The From date control defined in the application</param>
        /// <param name="dpThru">The Thru date control defined in the application</param>
        /// <param name="strFrom">Optional value for setting a date in the from control</param>
        /// <param name="strThru">Optional value for setting a date in the thru control</param>
        public static void CreateDateTimes(ref ToolStripControlHost dpFrom, ref ToolStripControlHost dpThru, string strFrom, string strThru)
        {
            // create the datetime controls for the From and Thru dates
            dpFrom = new ToolStripControlHost(new DateTimePicker());
            ((DateTimePicker)dpFrom.Control).Format = DateTimePickerFormat.Short;
            dpFrom.Control.Width = 95;
  
            dpThru = new ToolStripControlHost(new DateTimePicker());
            ((DateTimePicker)dpThru.Control).Format = DateTimePickerFormat.Short;
            dpThru.Control.Width = 95;


            // Check the from and thru dates passed to see if they are valid.
            DateTime dtResult;
            // FROM control set the value to the default
            dpFrom.Text = DateTime.Now.Subtract(new TimeSpan(2, 0, 0, 0)).ToString("d");
            // if the value passed in is not empty try to use it. If it is not valid the default will be used
            if (strFrom.Length > 0) // we have some type of value
            {
                if (DateTime.TryParse(strFrom, out dtResult))
                {
                    dpFrom.Text = strFrom; // the value is a valid datetime use it.
                }
            }

            // THRU control set the value to the default
            dpThru.Text = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0)).ToString("d");
            // if the value passed in is not empty try to use it. If it is not valid the default will be used
            if (strThru.Length > 0) // we have some type of value
            {
                if (DateTime.TryParse(strThru, out dtResult))
                {
                    dpThru.Text = strThru; // the value is a valid datetime use it.
                }
            }
            
            //dpFrom.Control.Refresh();
            //dpFrom.Invalidate();
            //tsMain.Items.Insert(7, dpFrom);

//            dpThru.Text = DateTime.Now.Subtract(new TimeSpan(DateTime.Now.Day, 0, 0, 0)).ToString("d");// "01/10/2007";
            //dpThru.Control.Refresh();
            //dpThru.Invalidate();
           // tsMain.Items.Insert(9, dpThru);
          //  tsMain.Refresh();
        }


        /// <summary>
        /// Creates a new font of the size specified.
        /// The default font name is "Arial"  and the size is 12 unless a name is passed in.
        /// 
        /// 10/12/2007 David Kelly
        /// </summary>
        /// <param name="strFontName"></param>
        /// <param name="nFontSize"></param>
        public static Font CreateFont(string strFontName, int nFontSize)
        {
            Font ftRetVal;
            // create the default font.
            ftRetVal = new Font(
               new FontFamily("Arial"),
               12,
               FontStyle.Bold,
               GraphicsUnit.Pixel);

            // Check the name that was passed to see if it is a valid font.
            bool bValidFontName = false;
            if (strFontName.Length > 0)
            {
                foreach (FontFamily ff in FontFamily.Families)
                {
                    if (strFontName == ff.Name)
                    {
                        // the font name is valid so go to work.
                        bValidFontName = true;
                        break;
                    }
                }
            }

            if (!bValidFontName)
            {
                strFontName = "Arial";
            }
            FontFamily fontFamily = new FontFamily(strFontName);
            ftRetVal = new Font(
               fontFamily,
               nFontSize,
               FontStyle.Bold,
               GraphicsUnit.Pixel);

            return ftRetVal;
        }

        /// <summary>
        /// Returns the contents of the file as a string.
        /// If the file cannot be read the returned string will be null, 
        /// and the m_strErrMsg will contain the exception thrown.
        /// 
        /// 02/22/2008 David Kelly
        /// </summary>
        /// <param name="FileName">File Name with path</param>
        /// <returns></returns>
        public static string GetFileContents(string FileName)
        {
            if (FileName.Length == 0)
            {
                m_strErrMsg = "File Name is blank";
                return null;
            }
            StreamReader sr = null;
            string FileContents = null;
            try
            {
                using (FileStream fs = new FileStream(FileName, FileMode.Open, FileAccess.Read))
                {
                    sr = new StreamReader(fs);
                    FileContents = sr.ReadToEnd();
                }
            }
             
            catch (FileLoadException fle)
            {
                m_strErrMsg = new string(fle.Message.ToCharArray());
            }
            catch (FieldAccessException fae)
            {
                m_strErrMsg = new string(fae.Message.ToCharArray());
            }
            catch (DriveNotFoundException dnfe)
            {
                m_strErrMsg = new string(dnfe.Message.ToCharArray());
            }
            catch (FileNotFoundException fnfe)
            {
                m_strErrMsg = new string(fnfe.Message.ToCharArray());
            }
            catch (DirectoryNotFoundException dnfe)
            {
                m_strErrMsg = new string(dnfe.Message.ToCharArray());
            }
            catch (NotSupportedException nse)
            {
                m_strErrMsg = new string(nse.Message.ToCharArray());
            }
            finally
            {
                if (sr != null)
                    sr.Close();
            }              

            return FileContents;
        }

  
        /// <summary>
        /// Dynamic used with instance of the class. For a static version see sfatal(string strErrMsg)
        /// Pass the error messge to be set to this function.
        /// This function logs, emails and shuts down HARD
        /// ie does NOT go thru OnStop()
        /// 
        /// Calling example
        /// RFCObject r = new RFCObject();
        ///  r.fatal(string.Format("ERROR FOUND -- TESTING\r\nCALL STACK: {0}", new System.Diagnostics.StackTrace(true).ToString()));
        /// If m_ERR does not exist when created the default ERR class is created in LIVE using MCLOE GOMCLLIVE.
        /// </summary>
        /// <param name="strErrMsg">Add the call stack to strErrMsg before passing. 
        /// If no error passed m_strErrMsg of RFCObject is used.</param>
        public void fatal(string strErrMsg)
        {
            if (m_ERR == null)
            {
                m_ERR = new ERR(new string[] { "/LIVE", "/MCLOE", "/GOMCLLIVE" });
                
            }
            m_ERR.propErrMsg = strErrMsg.Length == 0 ? m_strErrMsg : strErrMsg;
            m_ERR.ErrorHandler(ERR.ErrLevel.eINFO);// logs
            m_ERR.ErrorHandler(ERR.ErrLevel.eEMAIL); // send an email
            m_ERR.ErrorHandler(ERR.ErrLevel.eFATAL); // stops the app

        }

        /// <summary>
        /// Static version of the Dynamic fatal(string strErrMsg) function.
        /// Pass the error messge to be set to this function.
        /// This function logs, emails and shuts down HARD
        /// ie does NOT go thru OnStop()
        /// 
        /// Calling example
        /// RFCObject.sfatal(string.Format("ERROR FOUND -- TESTING\r\nCALL STACK: {0}", new System.Diagnostics.StackTrace(true).ToString()));
        /// If m_ERR does not exist when created the default ERR class is created in LIVE using MCLOE GOMCLLIVE.
        /// </summary>
        /// <param name="strErrMsg">Add the call stack to strErrMsg before passing. 
        /// If no error passed m_strErrMsg of RFCObject is used.</param>
        static public void sfatal(string strErrMsg)
        {
            if (m_ERR == null)
            {
                m_ERR = new ERR(new string[] {"/LIVE","/MCLOE","/GOMCLLIVE"});

            }
            m_ERR.propErrMsg = strErrMsg.Length == 0 ? m_strErrMsg : strErrMsg;
            m_ERR.ErrorHandler(ERR.ErrLevel.eINFO);// logs
            m_ERR.ErrorHandler(ERR.ErrLevel.eEMAIL); // send an email
            m_ERR.ErrorHandler(ERR.ErrLevel.eFATAL); // stops the app

        }

        /// <summary>
        /// Formats the number of bytes into GB,MB, KB or Bytes
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        static public string FormatBytes(long bytes)
        {
          const int scale = 1024;
          string[] orders = new string[] { "GB", "MB", "KB", "Bytes" };
          long max = (long)Math.Pow(scale, (orders.GetUpperBound(0)));

          foreach (string order in orders)
          {
            if ( bytes > max )
              return string.Format("{0:##.##} {1}", decimal.Divide( bytes, max ), order);

            max /= scale;
          }
          return "0 Bytes";
        }

        /// <summary>
        /// Converts a date time to hl7 date string
        /// </summary>
        /// <param name="strDate">Datetime as string</param>
        /// <returns>string formatted "yyyyMMdd"</returns>
        static public string ConvertDateTimeToHL7Date(string strDate)
        {
            string strRetVal = null;
            if (!string.IsNullOrEmpty(strDate))
            {
                DateTime dtConvert = DateTime.MinValue;
                if (DateTime.TryParse(strDate, out dtConvert))
                {
                    strRetVal = dtConvert.ToString("yyyyMMdd");
                }
            }
            
            return strRetVal;
        }


    } //don't type below this line
}
