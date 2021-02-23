//Documentation:
// 12/27/2006
// Purpose:
// 
// Consistant methodology for handling errors in both applications and services. 
// 
// The main way to use this class is to call ErrorHandler(ErrLevel el).
// 
// Depending on the error severity level ErrorHandler() can upgrade the error to the error so that some
// notification gets posted in the following order if possible 
//     LogFile - Writes to a logfile Can be specified in the applications config file or defaults to the application's directory
//     DataBase - writes to the data_ErrLog table Can only be used if specifed in the app's config file
//     Email - Sends email to the applications EmailTo (from apps config file) or default if none assigned or file doesn't exist
//     Fatal Error will cause application to exit (try's to display a console writeline or message box for notification of problem.
// 
// 
// This class needs one argument (string[] args) to operate fully.
// The paramaters is:
//     /LIVE or /TEST  (If not /LIVE defaults to /TEST Even with no paramater) (Capitalization is important)
// Any additional parameters passes are ignored.
// 
// Additional configuration comes from Applications Configuration file which may contain the following KEYS
 //<add key="LiveEMailTo" value="david.kelly@wth.org;rick.crone@wth.org"/>
 //<add key="TestEMailTo" value="david.kelly@wth.org;rick.crone@wth.org"/>
    
 //<add key="LiveERRDataBaseServer" value="MCL03" />
 //<add key="LiveERRDataBase" value="GOMCLLIVE" />
 //<add key="TestERRDataBaseServer" value="MCL03" />
 //<add key="TestERRDataBase" value="GOMCLTEST" />
 //<add key="LiveERRLogTextFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_LIVE.log" />
  //<add key="LiveERRLogSaveFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_LIVE.log" />
  //<add key="TestERRLogTextFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_TEST.log" />
  //<add key="TestERRLogSaveFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_TEST.log" />    <add key="LiveEMailTo" value="david.kelly@wth.org;rick.crone@wth.org"/>
  //<add key="TestEMailTo" value="david.kelly@wth.org;rick.crone@wth.org"/>
    
  //<add key="LiveERRDataBaseServer" value="MCL03" />
  //<add key="LiveERRDataBase" value="GOMCLLIVE" />
  //<add key="TestERRDataBaseServer" value="MCL03" />
  //<add key="TestERRDataBase" value="GOMCLTEST" />

  //<add key="LiveERRLogTextFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_LIVE.log" />
  //<add key="LiveERRLogSaveFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_LIVE.log" />
  //<add key="TestERRLogTextFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_TEST.log" />
  //<add key="TestERRLogSaveFile" value="C:\HMS\HMS_HL7_RESULTS_PARSE_TEST.log" />
// 
// 
// The table should be created with the following SQL script 
//****** Object:  Table [dbo].[data_ErrLog]    Script Date: 12/27/2006 10:43:31 AM ******/
//if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[data_ErrLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
//drop table [dbo].[data_ErrLog]
//GO

//****** Object:  Table [dbo].[data_ErrLog]    Script Date: 12/27/2006 10:43:32 AM ******/
//CREATE TABLE [dbo].[data_ErrLog] (
//    [rowguid] [uniqueidentifier] NOT NULL ,
//    [deleted] [bit] NOT NULL ,
//    [uri] [numeric](18, 0) IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
//    [App_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
//    [Error_Msg] [varchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
//    [Error_Level] [int] NOT NULL ,
//    [Stack_Trace] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
//    [mod_date] [datetime] NOT NULL ,
//    [mod_user] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
//    [mod_host] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
//    [mod_prg] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
//) ON [PRIMARY]
//GO

//ALTER TABLE [dbo].[data_ErrLog] WITH NOCHECK ADD 
//    CONSTRAINT [PK_data_ErrLog] PRIMARY KEY  CLUSTERED 
//    (
//        [rowguid]
//    ) WITH  FILLFACTOR = 90  ON [PRIMARY] 
//GO

//ALTER TABLE [dbo].[data_ErrLog] WITH NOCHECK ADD 
//    CONSTRAINT [DF_data_ErrLog_rowguid] DEFAULT (newid()) FOR [rowguid],
//    CONSTRAINT [DF_data_ErrLog_deleted] DEFAULT (0) FOR [deleted],
//    CONSTRAINT [DF_data_ErrLog_App_Name] DEFAULT ('') FOR [App_Name],
//    CONSTRAINT [DF_data_ErrLog_Error_Msg] DEFAULT ('') FOR [Error_Msg],
//    CONSTRAINT [DF_data_ErrLog_Error_Level] DEFAULT (0) FOR [Error_Level],
//    CONSTRAINT [DF_data_ErrLog_mod_date] DEFAULT (getdate()) FOR [mod_date],
//    CONSTRAINT [DF_data_ErrLog_mod_user] DEFAULT (suser_sname()) FOR [mod_user],
//    CONSTRAINT [DF_data_ErrLog_mod_host] DEFAULT (host_name()) FOR [mod_host],
//    CONSTRAINT [DF_data_ErrLog_mod_prg] DEFAULT (app_name()) FOR [mod_prg]
//GO


//exec sp_addextendedproperty N'MS_Description', null, N'user', N'dbo', N'table', N'data_ErrLog', N'column', N'Error_Level'


//GO


//
using System;
using System.Collections.Generic;
using System.Text;
// ADDED
using System.IO;
using System.Configuration;
using System.Data;



namespace RFClassLibrary
{
    /// <summary>
    /// Modifications:
    /// 07/16/2007 wdk 
    ///     added m_strFile and prop's to get and set the public variable the logfile for help in finding the
    ///     log file created.
    /// Documentation:
    /// 
    /// 12/27/2006
    /// Purpose:
    /// 
    /// Consistant methodology for handling errors in both applications and services. 
    /// 
    /// The main way to use this class is to call ErrorHandler(ErrLevel el).
    /// 
    /// Depending on the error severity level ErrorHandler() can upgrade the error to the error so that some
    /// notification gets posted in the following order if possible 
    ///     LogFile - Writes to a logfile Can be specified in the applications config file or defaults to the application's directory
    ///     DataBase - writes to the data_ErrLog table Can only be used if specifed in the app's config file
    ///     Email - Sends email to the applications EmailTo (from apps config file) or default if none assigned or file doesn't exist
    ///     Fatal Error will cause application to exit (try's to display a console writeline or message box for notification of problem.
    /// 
    /// 
    /// This class needs one argument (string[] args) to operate fully.
    /// Use the empty constructor if you are using the application's command line paramaters 
    /// The paramaters is:
    ///     /LIVE or /TEST  (If not /LIVE defaults to /TEST Even with no paramater) (Capitalization is important)
    /// Any additional parameters passes are ignored.
    /// 
    /// Additional configuration comes from Applications Configuration file which may contain the following KEYS
    /// SEE source code for the configuration example!   
    /// 
    /// The table should be created with the following SQL script 
    ///****** Object:  Table [dbo].[data_ErrLog]    Script Date: 12/27/2006 10:43:31 AM ******/
    ///if exists (select * from dbo.sysobjects where id = object_id(N'[dbo].[data_ErrLog]') and OBJECTPROPERTY(id, N'IsUserTable') = 1)
    ///drop table [dbo].[data_ErrLog]
    ///GO
    ///
    ///****** Object:  Table [dbo].[data_ErrLog]    Script Date: 12/27/2006 10:43:32 AM ******/
    ///CREATE TABLE [dbo].[data_ErrLog] (
    ///    [rowguid] [uniqueidentifier] NOT NULL ,
    ///    [deleted] [bit] NOT NULL ,
    ///    [uri] [numeric](18, 0) IDENTITY (1, 1) NOT FOR REPLICATION  NOT NULL ,
    ///    [App_Name] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
    ///    [Error_Msg] [varchar] (1024) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
    ///    [Error_Level] [int] NOT NULL ,
    ///    [Stack_Trace] [varchar] (8000) COLLATE SQL_Latin1_General_CP1_CI_AS NULL ,
    ///    [mod_date] [datetime] NOT NULL ,
    ///    [mod_user] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
    ///    [mod_host] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL ,
    ///    [mod_prg] [varchar] (50) COLLATE SQL_Latin1_General_CP1_CI_AS NOT NULL 
    ///) ON [PRIMARY]
    ///GO
    ///
    ///ALTER TABLE [dbo].[data_ErrLog] WITH NOCHECK ADD 
    ///    CONSTRAINT [PK_data_ErrLog] PRIMARY KEY  CLUSTERED 
    ///    (
    ///        [rowguid]
    ///    ) WITH  FILLFACTOR = 90  ON [PRIMARY] 
    ///GO
    ///
    ///ALTER TABLE [dbo].[data_ErrLog] WITH NOCHECK ADD 
    ///    CONSTRAINT [DF_data_ErrLog_rowguid] DEFAULT (newid()) FOR [rowguid],
    ///    CONSTRAINT [DF_data_ErrLog_deleted] DEFAULT (0) FOR [deleted],
    ///    CONSTRAINT [DF_data_ErrLog_App_Name] DEFAULT ('') FOR [App_Name],
    ///    CONSTRAINT [DF_data_ErrLog_Error_Msg] DEFAULT ('') FOR [Error_Msg],
    ///    CONSTRAINT [DF_data_ErrLog_Error_Level] DEFAULT (0) FOR [Error_Level],
    ///    CONSTRAINT [DF_data_ErrLog_mod_date] DEFAULT (getdate()) FOR [mod_date],
    ///    CONSTRAINT [DF_data_ErrLog_mod_user] DEFAULT (suser_sname()) FOR [mod_user],
    ///    CONSTRAINT [DF_data_ErrLog_mod_host] DEFAULT (host_name()) FOR [mod_host],
    ///    CONSTRAINT [DF_data_ErrLog_mod_prg] DEFAULT (app_name()) FOR [mod_prg]
    ///GO
    ///
    ///
    ///exec sp_addextendedproperty N'MS_Description', null, N'user', N'dbo', N'table', N'data_ErrLog', N'column', N'Error_Level'
    ///
    ///
    ///GO
    /// </summary>
    public class ERR : RFCObject
    {
        /// <summary>
        /// Obsolete no long switchable wdk 20100716
        /// </summary>
        public string m_strServerToMonitor = null;
        /// <summary>
        /// This dataset contains the error_type, err_msg, err_mod_app, err_mod_time, err_mod_host, and err_mod_user
        /// 
        /// </summary>
        public DataSet m_dsErrors = null;

        /// <summary>
        /// enum for request level to handle error
        /// this class can elevate error level if the requested level fails
        /// </summary>
        public enum ErrLevel {
            ///<summary>Show on the screen either Console.WriteLine or MessageBox only</summary>
            eDISPLAY = 0,
            /// <summary>/// EHS Errors in PV1 or PID Log using this. cannot write the WPat record </summary>
            eERR_PAT = 10,
            /// <summary> EHS Errors in ORC segment cannot write the Wreq record </summary>
            eERR_REQ = 20,
            /// <summary>
            /// EHS Errors in OBR segment cannot write the Worders record</summary>
            eERR_ORD = 30,
            /// <summary> EHS Errors in IN1 segment cannot write the wins record</summary>
            eERR_INS = 40,
            /// <summary>EHS Errors in DG1 segment cannot write the gaurantor portion of the wins record</summary>
            eERR_GAUR = 50,
            ///<summary>Log error to text file (LogFile)</summary>
            eINFO = 99,
            ///<summary>Log error to DataBase (table data_ErrLog)</summary>
            eERR_DATABASE = 100,
            ///<summary>Email Error</summary>
            eEMAIL = 200,
            ///<summary>Fatal Exit program</summary>
            eFATAL = 255 
        };
        
	

        DBAccess m_dbAccess;
        
        /// <summary>
        /// ERR class's instance of EMail
        /// </summary>
        public EMail m_Email;
        
        /// <summary>
        /// ERR class's instance of LogFile
        /// </summary>
        public LogFile m_Logfile;
        private string m_strLogFileName;
        /// <summary>
        /// 07/16/2007 wdk 
        /// Set or gets the logfile created in the init.
        /// </summary>
        public string propLogFileName
        {
            get { return m_strLogFileName; }
            set { m_strLogFileName = value; }
        }
         
        string m_strConfigFile;

        private bool m_bIsLive;
        /// <summary>
        /// This property is read only for the ERR class's IsLive boolean value. For use when
        /// creating Local record sets in applications/services that instantiate the ERR class.
        /// </summary>
        public bool propIsLive
        {
            get { return m_bIsLive; }
        }

        /// <summary>
        /// /// Use the empty constructor if you are using the application's command line paramaters 
        /// The paramaters are 
        ///     1. /LIVE or /TEST (Capitalization is important)
        ///     2. /SERVER  (Where SERVER is the SQL server where the data_ErrLog table resides ex: /MCL03)
        ///     3. /DATABASE (DATABASE on the SQL SERVER above  where the data_ErrLog table resides ex: /GOMCLTEST)
        /// </summary>
        [Obsolete ("Do not create the error class with this constructor. Use the ERR(string[] args) constructor following the guidelines it provides.")]
        public ERR()
        {
            // command line paramaters zero argument is Application name not needed for ConstructorCode()
            string[] args = Environment.CommandLine.Split(new char[] { '/' });
            string[] myArgs = new string[5];
            //int[] myIntArray = new int[5] { 1, 2, 3, 4, 5 };

            for (int i = 0; i <= args.GetUpperBound(0); i++)
            {
                if (i != 0)
                {
                    myArgs[i - 1] = ("/"+args[i]); // split removes the '/' need to add it back
                }
            }
            
            ConstructorCode(myArgs);
        }

        /// <summary>
        /// Use this constructor to work with services as the command line arguments don't seem to be available?
        /// The paramaters are 
        ///     1. /LIVE or /TEST (Capitalization is important)
        ///     2. /SERVER  (Where SERVER is the SQL server where the data_ErrLog table resides ex: /MCL03)
        ///     3. /DATABASE (DATABASE on the SQL SERVER above  where the data_ErrLog table resides ex: /GOMCLTEST)
        /// </summary>
        /// <param name="args">The paramaters are 
        ///     1. /LIVE or /TEST (Capitalization is important)
        ///     2. /SERVER  (Where SERVER is the SQL server where the data_ErrLog table resides ex: /MCL03)
        ///     3. /DATABASE (DATABASE on the SQL SERVER above  where the data_ErrLog table resides ex: /GOMCLTEST)
        ///</param>
        public ERR(string[] args)
        {
            
            ConstructorCode(args);
        }

        //public ERR(object[] args)
        //{
        //    string[] sArgs = new string[3];
        //    args.CopyTo(sArgs, 0);
        //    ConstructorCode(sArgs);
        //}

        private void ConstructorCode(string[] args)
        {
            
            if (args.GetUpperBound(0) == -1)
            {
                m_bIsLive = false;
            }
            else
            {
                //m_bIsLive = args[0] == "/LIVE" ? true : false;
                m_bIsLive = args[0].Contains("/LIVE"); // has a space being passed in so string does not match exact
                if (!m_bIsLive) // 08/08/2008 wdk added to check for /MCLLIVE being passed by .Net applications
                {
                    m_bIsLive = args[0].Contains("/MCLLIVE"); // 08/08/2008 wdk not using ("LIVE") because of MCLARCLIVE
                }
                // 07/17/2008 wdk now handled in the constructor
                //foreach (string str in args) //06/23/2008 wdk added foreach loop to handle 1st arg being the file name in the new applications with configs
                //{
                //    if (str == "/MCLLIVE")
                //    {
                //        m_bIsLive = true;
                //        break;
                //    }
                //}
            }

            m_Email = new EMail(); // InitLogfile needs the email should it fail
            if (args.GetLength(0) > 3)
            {
                m_strServerToMonitor = args[3].ToString().Remove(0,1);//.Replace("//","");
            }
            InitLogFile();

            m_bValid = true; // assume success
            m_strConfigFile = propAppConfigFile;
            
            InitDataBase(args);

            // rgc/wdk 20090512 New dataset for error list added
            CreateErrorDataSet();
        }

        private void CreateErrorDataSet()
        {
            m_dsErrors = new DataSet("ERRORS_DATASET");
            m_dsErrors.Tables.Add("ERRORS");
            m_dsErrors.Tables["ERRORS"].Columns.Add("ERR_TYPE"); // 
            m_dsErrors.Tables["ERRORS"].Columns.Add("TEST_MNEM");
            m_dsErrors.Tables["ERRORS"].Columns.Add("ERR_MSG"); // this is the actual error reported to ERR class
            m_dsErrors.Tables["ERRORS"].Columns.Add("ERR_MOD_APP"); // this should be the App with the version
            m_dsErrors.Tables["ERRORS"].Columns.Add("ERR_MOD_TIME"); // when this occured Date and time
            m_dsErrors.Tables["ERRORS"].Columns.Add("ERR_MOD_HOST"); // Machine used
            m_dsErrors.Tables["ERRORS"].Columns.Add("ERR_MOD_USER"); // application user
            
        }
        /// <summary>
        /// Adds an error message to the dataset with a test mnemonic
        /// Use overload for not passing the test mnenoic
        /// 
        /// rgc/wdk 20090512
        /// 
        /// wdk 20091001 For HL7 format errors set the strTestMnem to "FORMAT" when adding this allows NTE's to be created for each error in the message when cancelling the message.
        /// </summary>
        /// <param name="strErrorType">LMRP, MUTEX, SANCPHY etc</param>
        /// <param name="strErrorMsg">actual message</param>
        /// <param name="strTestMnem">Test to add to the ERROR dataset</param>
        /// <returns></returns>
        public int AddErrorToDataSet(string strErrorType,string strTestMnem, string strErrorMsg)
        {
            int iRetVal = 0;
            if (strErrorType.Substring(0, 4) != "INFO")
            {
                m_dsErrors.Tables["ERRORS"].Rows.Add(new string[] { strErrorType, strTestMnem, strErrorMsg, OS.GetAppName(), DateTime.Now.ToString(), OS.GetMachineName(), OS.GetUserName() });
            }
            iRetVal = m_dsErrors.Tables["ERRORS"].Rows.Count;
            return iRetVal;
        }


        /// <summary>
        /// Adds an error message to the dataset. This adds the record with a blank test mnemonic
        /// Use overload for passing the test mnenoic
        /// 
        /// rgc/wdk 20090512
        /// </summary>
        /// <param name="strErrorType">LMRP, MUTEX, SANCPHY etc</param>
        /// <param name="strErrorMsg">actual message</param>
        /// <returns></returns>
        public int AddErrorToDataSet(string strErrorType,  string strErrorMsg)
        {
            int iRetVal = 0;
            if (strErrorType.Substring(0,4) != "INFO")
            {
                m_dsErrors.Tables["ERRORS"].Rows.Add(new string[] { strErrorType, "", strErrorMsg, OS.GetAppName(), DateTime.Now.ToString(), OS.GetMachineName(), OS.GetUserName() });           
            }
            iRetVal = m_dsErrors.Tables["ERRORS"].Rows.Count;
            return iRetVal;
        }

       /// <summary>
       /// wdk 20090403 Modified args[] handling. 
       /// Some applications are passing a null as args[2]. If args[1] and args[2] are not null
       ///  use them if args[0] and args[1] are not null use them instead.
       /// Also changed the calls to null and empty to use string.IsNullOrEmpty()
       /// </summary>
       /// <param name="args"></param>
        private void InitDataBase(string[] args)
        {
            
            // wdk 20090403 mod to keep backwards compatability
            if (string.IsNullOrEmpty(args[2]) && !string.IsNullOrEmpty(args[0]))
            {
                m_dbAccess = new DBAccess(args[0], args[1], "data_ErrLog");
            }
            else
            {
                m_dbAccess = new DBAccess(args[1], args[2], "data_ErrLog");
            }
            

            if (m_dbAccess.m_bValid)
            {
                //if (m_dbAccess.propDB == "GOMCLLIVE")
                //{
                //    System.Diagnostics.Debugger.Launch();
                //}
            m_Logfile.WriteLogFile(string.Format("INITDATABASE created m_dbAccess.\r\nProperties DATABASE SERVER: {0}, DATABASE {1}, TABLE {2}",
                    m_dbAccess.propDataSrc, m_dbAccess.propDB, m_dbAccess.propTable));
                return;
            }
            // not valid from above so try to use the config file for the application to get the error database.
            try
            {
                //// read config file for database properties
                m_dbAccess.propDataSrc =
                    System.Configuration.ConfigurationManager.AppSettings[string.Format("{0}ERRDataBaseServer", m_bIsLive ? "Live" : "Test")];
                
                m_dbAccess.propDB =
                    System.Configuration.ConfigurationManager.AppSettings[string.Format("{0}ERRDataBase", m_bIsLive ? "Live" : "Test")];
                
                // wdk 20090403 modified below code by removing two seperate checks for the string.IsNullOrEmpty()
                // which allows returning and not trying to initialize the database.
                if (string.IsNullOrEmpty(m_dbAccess.propDataSrc) || string.IsNullOrEmpty(m_dbAccess.propDB))
//                if (m_dbAccess.propDataSrc == null || m_dbAccess.propDB == null)
                {
                    // no config file found or no entries in the config file
                    //skip the data base initilization
                    m_strErrMsg = "No config file found for Error Database table: data_ErrLog";
                    m_dbAccess.propValid = false;
                    ErrorHandler(ErrLevel.eINFO);
                    return;
                }
                
                //if (m_dbAccess.propDataSrc.Length == 0 || m_dbAccess.propDB.Length == 0)
                //{
                //    // no entries in config file.
                //    //skip the data base initilization
                //    m_dbAccess.propValid = false;
                //}
                //else
                //{
                //// create the dbaccess
                m_dbAccess = new DBAccess(m_dbAccess.propDataSrc, m_dbAccess.propDB, "data_ErrLog");
                // wdk 20110616 removed database problems exist that prevent this
                //UpdateDataBase(string.Format("DataBase started by {0}", propAppName), 128);
                //}
            }
            catch (Exception db)
            {
                m_strErrMsg = string.Format("ERR::InitDataBase() ERROR: {0}, STACKTRACE: {1}", db.Message.ToString(), db.StackTrace.ToString());
                ErrorHandler(ErrLevel.eEMAIL);
            }
        }
        /// <summary>
        /// Initialize the log file. Made public 20100415 by Rick and David for use with
        /// the services so that they can restart the logs at midnight when there is no work.
        /// 
        /// </summary>
        public void InitLogFile()
        {
            try
            {
                //// read config file for logfile properties
                string strTextFile =
                    System.Configuration.ConfigurationManager.AppSettings[string.Format("{0}ERRLogTextFile", m_bIsLive ? "Live" : "Test")];
                if (strTextFile == null)
                {
                    // no config file found
                    strTextFile = "%APP_DIR%";
                }
                string strSaveFile =
                    System.Configuration.ConfigurationManager.AppSettings[string.Format("{0}ERRLogSaveFile", m_bIsLive ? "Live" : "Test")];
                if (strSaveFile == null)
                {
                    // no config file found
                    strSaveFile = "%APP_DIR%";
                }
                
                if (strTextFile == "%APP_DIR%" )
                {
                    strTextFile = string.Format(@"C:\TEMP\{0}{1}.log",propAppName,m_strServerToMonitor  == null? "": string.Format("_{0}",m_strServerToMonitor));

                }
                if (strSaveFile == "%APP_DIR%")
                {
                    strSaveFile = strTextFile;
                }
                m_Logfile = new LogFile(strTextFile, strSaveFile);
                propLogFileName = strTextFile;
            }
            catch (Exception fe)
            {
                // couldn't write to the log file so try to send an email???
                m_strErrMsg = string.Format("InitLogFile error. Message {0}\r\n StackTrace {1}", fe.Message.ToString(), fe.StackTrace.ToString());
                ErrorHandler(ErrLevel.eERR_DATABASE);
                return;
            }
            m_Logfile.WriteLogFile(string.Format("LogFile started for application -- {0} ", propAppName));
         }
        
        /// <summary>
        /// Writes to data_ErrLog. If the write fails try to log the error in the log file.
        /// if that also fails or the error level is severe enough attempt to send an email.
        /// </summary>
        /// <returns></returns>
        public bool UpdateDataBase(string strError, int nErrorLevel)
        {                 
            bool bRetVal = false;
            if (!m_dbAccess.propValid)
            {
                return bRetVal;
            }
            string strSQL;
            strError = SqlClean(strError); // wdk 20090316 received 150 error messages that this failed over the weekend first non usable part is after the first {'} when inserting the inserted values.  Paste the below block into the sql management studio query to see problem.
            /*
                INSERT INTO data_ErrLog 
                (App_Name, Error_Msg, Error_Level, Stack_Trace) 
                VALUES 
                (
	                'QPMREG 1.0.3267.27054',
	                'INSERT INTO registration(account, name, dob, sex, mri, AttPhy, FinClass, ServDateTime,icd9_text, RefPatID, RefDoc, RefTissue, RefEncounterNum, StatusIn,ProcessIn, StatusOut,ProcessOut, wreq_rowguid, wpat_rowguid, ssn)
		                VALUES ('', 'WILBITT,VELMA ', '4/29/1931 12:00:00 AM', 'F', 'S000801453', 'OCNH', 'A','3/17/2009 12:00 AM', '', '15B', 'MALIK,INDU BALA ','', '', 'READY','03/16/09 08:16:32', 'NEED_ACC', NULL, '3d9cc4a7-2c12-de11-8978-001d092cd451', '600910d5-91fe-dd11-a19e-001d092cd451', '386300326')',100,'   at RFClassLibrary.ERR.UpdateDataBase(String strError, Int32 nErrorLevel)
                   at RFClassLibrary.ERR.DataBaseHandler(Int32 nErrNumber)
                   at RFClassLibrary.ERR.ErrorHandler(Int32 nErrNumber)
                   at RFClassLibrary.ERR.ErrorHandler(ErrLevel el)
                   at MCL.R_registration.AddRecord()
                   at QPMREG.QPMREG.RegPat2()
                   at QPMREG.QPMREG.timer1_Elapsed(Object sender, ElapsedEventArgs e)
                   at System.Timers.Timer.MyTimerCallback(Object state)
                   at System.Threading.ExecutionContext.Run(ExecutionContext executionContext, ContextCallback callback, Object state)
                   at System.Threading._TimerCallback.PerformTimerCallback(Object state)
                */
            string strStackTrace = SqlClean((new System.Diagnostics.StackTrace(true)).ToString());
            // wdk 20091109 added formatting to strStackTrace
            if (!string.IsNullOrEmpty(strStackTrace))
            {
                strStackTrace = strStackTrace.Substring(0, strStackTrace.Length > 7999 ? 7999 : strStackTrace.Length);
            }
            strSQL = string.Format("INSERT INTO data_ErrLog (App_Name, Error_Msg, Error_Level, Stack_Trace) VALUES ('{0}','{1}',{2},'{3}')",
                                        propAppName,
                                         strError,
                                          nErrorLevel,
                                            strStackTrace);

            int iRecCount = m_dbAccess.SQLExec(strSQL, out m_strErrMsg);
            if (iRecCount > 0)
            {
                bRetVal = true;
            }
            else
            {
                string strFrom = string.Format("{0}@RFC.ERR", propAppName);
                m_Email.Send(strFrom, "David.Kelly@WTH.org", "DATA BASE UPDATE ERROR", 
                    string.Format("ERROR MESSAGE\r\n{0}\r\n\nSQL\r\n\n{1}",//\r\n\nCONN STRING\r\n{2}", 
                        m_strErrMsg ,strSQL)); // 07/14/2008 wdk added the error message to the email
            }
            return (bRetVal);           
        }


        /// <summary>
        /// Overload so the error handler can be called with a string.
        /// This sets m_strErrMsg and calls the error handler with the
        /// basic level for writing to the logfile. Escallation happens as
        /// necessary in the ErrorHandler function.
        /// 01/03/2006 David and Rick
        /// </summary>
        /// <param name="strMsg"></param>
        public void ErrorHandler(string strMsg)
        {
            m_strErrMsg = strMsg;
            ErrorHandler(ErrLevel.eINFO);
        }

        /// <summary>
        /// OverLoad so this can be called with a ErrLevel.
        /// This would be the prefered way to call this function.
        /// The other ErrorHandler parm is an int.
        /// Note: FATAL will display and shut down the program
        /// but will NOT log or email.. if you want those you must call 
        /// them too! You amy want to add a Fatal() function to your applicaion
        /// that makes all the calls you want.
        /// </summary>
        /// <param name="el">ERR.ErrLevel.?</param>
        public void ErrorHandler(ErrLevel el)
        {
            ErrorHandler((int)el);
        }
        /// <summary>
        /// This is the main way this class should be used.
        /// This function can escalate the error reporting if the level the caller
        /// request can not be accomplished.
        ///
        /// Note: ErrorHandler(ErrLevel el) Overloaded for easier calling
        /// </summary>
        /// <param name="nErrNumber"></param>
        
        public void ErrorHandler(int nErrNumber)
        {
            if (nErrNumber == (int)ErrLevel.eDISPLAY)
            {
                DisplayHandler();
                return;
            }
            if (nErrNumber <= (int)ErrLevel.eINFO)
            {
                // Info
                if (m_Logfile != null) 
                {
                    if (!LogFileHanler())
                    {
                        // log file write failed
                        if (nErrNumber < (int)ErrLevel.eERR_DATABASE)
                        {
                            nErrNumber = (int)ErrLevel.eERR_DATABASE;
                        }
                    }
                }
                else
                {
                    // log not created
                    if (nErrNumber < (int)ErrLevel.eERR_DATABASE)
                    {
                        nErrNumber = (int)ErrLevel.eERR_DATABASE;
                    }
                }
                if (nErrNumber < (int)ErrLevel.eERR_DATABASE)
                {
                    return;
                }
            }

            if (nErrNumber <= (int)ErrLevel.eERR_DATABASE)
            {
                // Err
                if (m_dbAccess.propValid == true)
                {
                    // wdk 20110716 removed because we don't want to add more writes to the data base if it is failing to connect
                    //if (!DataBaseHandler(nErrNumber))
                    {
                        // database update failed.
                        if (nErrNumber < (int)ErrLevel.eEMAIL)
                        {
                            nErrNumber = (int)ErrLevel.eEMAIL;
                        }
                    }
                }
                else
                {
                    // data base access not available 
                    if (nErrNumber < (int)ErrLevel.eEMAIL)
                    {
                        nErrNumber = (int)ErrLevel.eEMAIL;
                    }
                }
                if (nErrNumber < (int)ErrLevel.eEMAIL)
                {
                    return;
                }
            }

            if (nErrNumber <= (int)ErrLevel.eEMAIL)
            {
                // EMail
                

                if (!EmailHandler(nErrNumber))
                {
                   nErrNumber = (int)ErrLevel.eFATAL;
                }
                if (nErrNumber < (int)ErrLevel.eFATAL)
                {
                    return;
                }
            }

            if (nErrNumber == (int)ErrLevel.eFATAL)
            {
                // Fatal Error CSMO
                
                try
                {
                    Console.WriteLine(string.Format("DISPLAY: {0}\r\n{1}\r\n\n{2}", propAppExecPath, m_strErrMsg, Environment.StackTrace.ToString()));
                    System.Windows.Forms.MessageBox.Show(string.Format("DISPLAY: {0}\r\n{1}\r\n\n{2}", propAppExecPath, m_strErrMsg, Environment.StackTrace.ToString()));
                }
                catch (Exception)
                {
                    //service will throw an exception and we don't care 
                }
                Environment.Exit(nErrNumber);
            }
        }

        /// <summary>
        /// Send email.
        /// </summary>
        /// <param name="nErrNumber"></param>
        /// <returns></returns>
        public bool  EmailHandler(int nErrNumber)
        {
            string strEmailTo = System.Configuration.ConfigurationManager.AppSettings[string.Format("{0}EMailTo", m_bIsLive ? "Live" : "Test")];
            if (strEmailTo != null)
            {
                if (strEmailTo.Length == 0)
                {
                    //default if not specified in apps config file
                    strEmailTo = "David.Kelly@WTH.org";
                }
            }
            else
            {
                //NO apps config file FOUND
                strEmailTo = "David.Kelly@WTH.org";
            }

            return m_Email.Send(string.Format("{0}@ErrorHandler.nErrNumber", propAppName), strEmailTo, string.Format("Application Error {0}", nErrNumber), m_strErrMsg);
        }

        /// <summary>
        /// Display the m_strErrMsg from the base class RFCObject.
        /// Works for both windows and console application.
        /// Ok to call from service application - but will not do anything.
        /// </summary>
        public void DisplayHandler()
        {
            try
            {
                Console.WriteLine(string.Format("DISPLAY: {0}\r\n{1}", propAppExecPath, m_strErrMsg));
                System.Windows.Forms.MessageBox.Show(string.Format("DISPLAY: {0}\r\n{1}", propAppExecPath, m_strErrMsg));
            }
            catch (Exception)
            {
            }
            return;
        }

        /// <summary>
        /// Write an entry to the log file.
        /// </summary>
        /// <returns></returns>
        public bool LogFileHanler()
        {
            return (m_Logfile.WriteLogFile(string.Format("LogFileHandler() {0}",m_strErrMsg)));
        }

        
        //public bool DataBaseHandler(int nErrNumber)
        //{

        //    // wdk 20110616 removed database problems exist that prevent this
        //    //return(UpdateDataBase(m_strErrMsg, nErrNumber));
        //}

        /// <summary>
        /// Returns an XML delimited string from the Errors DataSet
        /// </summary>
        /// <returns></returns>
        public string DumpDataSetErrorsAsXML()
        {
            return m_dsErrors.GetXml();
        }

        /// <summary>
        /// Returns a delimited string for the column indicated. If no delimitor is passed then the pipe {|} will be used.
        /// 
        /// Column names in the ERR.m_dsErrors are:
        /// <list type="string">
        /// "ERR_TYPE"      -- this should be LMRP, MUTEX, SANCPHY, etc
        /// "ERR_MSG"       -- this is the actual error reported to ERR class
        /// "ERR_MOD_APP"   -- this should be the App with the version
        /// "ERR_MOD_TIME"  -- when this occured Date and time
        /// "ERR_MOD_HOST"  -- Machine used
        /// "ERR_MOD_USER"  -- Application user
        /// </list>
        /// </summary>
        /// <param name="strCol">Column name from the comment list</param>
        /// <param name="strDelimiter">Any valid string you want to use as a delimiter for the returned string. Default is "|"</param>
        /// <returns></returns>
        public string DumpDataSetErrorsByColumn(string strCol, string strDelimiter)
        {
            if (string.IsNullOrEmpty(strDelimiter))
            {
                strDelimiter = "|";
            }
            string strRetVal = string.Empty;// "ERROR";
            
            //if (nCol >= m_dsErrors.Tables[0].Columns.Count)
            //{
            //    strRetVal = string.Format("ERROR: Table only has {0} columns numbered 0 to {1} and you asked for colunn {2}",  m_dsErrors.Tables[0].Columns.Count, (m_dsErrors.Tables[0].Columns.Count-1),nCol);
            //}
            //else
            //{
                foreach (DataRow dr in m_dsErrors.Tables[0].Rows)
                {
                    strRetVal += string.Format("{0}{1}", dr[strCol], strDelimiter);
                }
            //}
            return strRetVal;
        }

        /// <summary>
        /// wdk 20090612 not coded at this time only returns "ERROR".
        /// </summary>
        /// <param name="nCol"></param>
        /// <returns></returns>
        public string DumpDataSetErrorsByColumnAsXML(int nCol)
        {
            return "ERROR";

            //DataSet ds = new DataSet();
            //ds.Tables.Add("TEMP");
            //ds.Tables[0].Columns.Add(m_dsErrors.Tables[0].Columns[nCol]);
            
            //return ds.GetXml();
        }



        
    } //don't go below this line
}
