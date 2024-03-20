using System;
using System.Diagnostics;// for Process
// wdk 05/07/07 for OS's IsFileLocked(FileInfo file)
using System.IO;
			

namespace Utilities
{
	/// <summary>
	/// Summary description for OS.
	/// </summary>
	public class OS : RFCObject
	{
		/// <summary>
		/// empty constructor
		/// </summary>
        public OS()
		{
			
		}

        /// <summary>
        /// This is just for example code as it always returns - getCurrentMethodName
        /// 08/02/2007 Rick Crone
        /// </summary>
        /// <returns>string with current method name</returns>
        public static string getCurrentMethodName()
        {
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(0, true); //To get the calling method name, use 1   instead of 0.method
            System.Reflection.MethodBase mb = sf.GetMethod();
            string methodName = mb != null ? mb.Name : "";
            return methodName;
        }

        /// <summary>
        /// When called from a function this will be that functions name.
        /// 08/02/2007 Rick Crone
        /// </summary>
        /// <returns>string with calling methods name</returns>
        public static string getCallingMedthodName()
        {
            System.Diagnostics.StackFrame sf = new System.Diagnostics.StackFrame(1, true); //To get the calling method name, use 1   instead of 0.method
            System.Reflection.MethodBase mb = sf.GetMethod();
            string methodName = mb != null ? mb.Name : "";
            return methodName;
        }


		/// <summary>
		/// 07/29/2004 Rick Crone - display the contents of a folder
		/// example of strPath parameter:
		///		"C:\\temp\\"
		///		or it could even be
		///		/S 
		///		for Select
		///		/E
		///		for Everything
		/// </summary>
		public static void OpenFolder(string strPath)
		{
			Process p = new Process();
			p.StartInfo.FileName = "explorer.exe";
			p.StartInfo.WorkingDirectory = "C:\\Windows\\";
			p.StartInfo.Arguments = strPath;
			p.Start();
			return;

		}

		/// <summary>
		/// 07/29/2004 Rick Crone
        /// Mod Hist:
        /// 03/07/2007 Rick Crone
        /// Now returns the Process. There are many interesting properties and methods.
        /// This feature added primarly to allow the caller to check the HasExited property.
        /// note: using System.Diagnostics;// for Process
        /// Calling example:
        /// <code>
        ///  OS my_os = new OS();
        ///    Process p;
        ///    p = my_os.Shell("NotePad.exe", @"C\Windows", "");
        ///    while (!p.HasExited)
        ///    {
        ///        //do nothing - no refresh needed here
        ///    }
        /// </code>
        /// 
		/// </summary>
		public Process Shell(string strFileName, string strWorkingDirectory, string strArgumants)
		{
			Process p = new Process();
			p.StartInfo.FileName = strFileName;
			p.StartInfo.WorkingDirectory = strWorkingDirectory;
			p.StartInfo.Arguments = strArgumants;
            try
            {
                p.Start();
            }
            catch (Exception ex)
            {
                m_strErrMsg = ex.Message;
                p = null;
            }
			return(p);
		}

		/// <summary>
		/// 06/02/2005 Rick Crone
		/// </summary>
		public static string GetMachineName()
		{
			string strMachineName;
			strMachineName = string.Format("{0}",Environment.MachineName);
			return strMachineName;
		}
		/// <summary>
		/// 06/15/2005 Rick Crone
		/// </summary>
		public static string GetUserName()
		{
            //SystemInformation.UserName; an alternate way
            string strUserName;
			strUserName = string.Format("{0}",Environment.UserName);
			return strUserName;
		}

        /// <summary>
        /// 12/14/2006 David Kelly
        /// </summary>
        public static string GetAppName()
        {
            RFCObject rfc = new();
            return rfc.propAppName;          
        }


        /// <summary>
        /// wdk 05/07/07 added for checking if the file is locked.
        /// Thanks to Code project's Matt Sclotto.
        /// Intended use is with the XML file creation but will work with any files 
        /// FileInfo's can be new'ed with the string path and file name. i.e
        /// FileInfo fi = new FileInfo(@"C:\temp\david.txt");
        /// Called by 
        /// OS.IsFileLocked(fi);
        /// 
        /// </summary>
        /// <param name="file"></param>
        /// <returns>True if the file is being used by another process. ie being written to</returns>
        #pragma warning disable 168
        public static bool IsFileLocked(FileInfo file)
        {
            bool bRetVal = false; // not locked - 05/24/2007 rgc
            FileStream stream = null;

            try
            {
                stream = file.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
            }
            catch (IOException ex)
            {
                //CS0168
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }
            finally //Control is always passed to the finally block regardless of how the try block exits.
            {
                if (stream != null)
                {
                    stream.Close();
                }
                else
                {
                    //--- added 05/24/2007 rgc ---
                    bRetVal = true; // no stream - nothing for caller to work with
                    //----------------------------
                }
            }

            //file is not locked - or does not exist - either way nothing for caller to use!
            return bRetVal;
        }

    #pragma warning restore 168

    }
}
