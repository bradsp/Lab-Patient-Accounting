/*
 * 09/14/2006 Rick Crone
 * This RCRecordSet is intended for use as a base class when creating 
 * classes that need to access a table in a SQL database.
 * 
 * RCRecordSet is dervied from DBAccess which is dervied from RFCObject.
 * 
 * The classes derived from RCRecordSet will need to implement a contructor that will pass on
 * it's parameters like:
 *  public MyRecordSet(string strServer, string strDataBase) : base(strServer, strDataBase, "table_name")
 * 
 * Application should check m_bValid after construction.
 * Methods in this class should check m_bValid.
 * 
 */
using System;
using System.Collections.Generic;
using System.Text;
// added
using Microsoft.Data.SqlClient; // SQL 7.0
using System.Data; // DataSet
using System.Diagnostics; // stack trace


namespace RFClassLibrary
{
    
    /// <summary>
    ///This RCRecordSet is intended for use as a base class when creating 
    /// classes that need to access a table in a SQL database.
    /// 
    /// RCRecordSet is dervied from DBAccess which is dervied from RFCObject.
    /// 
    /// The classes derived from RCRecordSet will need to implement a contructor that will pass on
    ///  it's parameters like:
    ///  public MyRecordSet(string strServer, string strDataBase) : base(strServer, strDataBase, "table_name")
    /// 
    /// Application should check m_bValid after construction.
    /// Methods in this class should check m_bValid.
    /// 09/14/2006 Rick Crone
    /// 6t
    /// </summary>
    public class RCRecordset:DBAccess
    {
        // ==== 'standard' fields =====================
        // Most of our tables have these fields so they are here to
        // avoid having to add them to each class that is dervied from this class
        // this will give us consitant naming for these fields
        
        /// <!-- 'standard' field unique identifier-->
        public string m_strRowguid;

        /// <!-- active record flag -->
        public string m_strDeleted; // T / F

        // standard audit fields - tiggers update these
        /// <!--standard audit field for the date /time record was modified
        /// - SQL tiggers updates this -->
        public string m_strModDate;
        /// <!--standard audit field for the user who modified the record
        /// - SQL tiggers update this -->
        public string m_strModUser;
        /// <!--standard audit field for the program that modified the record
        /// - tiggers update these -->
        public string m_strModPrg;
        /// <!--standard audit field for the machine that modified the record
        /// - tiggers update these -->
        public string m_strModHost;
        //============ end of 'standard' fields =======
        ///// <summary>
        ///// Error Handling Class
        ///// </summary>
        //public ERR m_ERR; // 06/11/2008 wdk/rgc move err log reference for base class handling.
        /// <!-- the table name -->
        public string m_strTableName;
        /// <!-- zero based index to the current record-->
        public int m_CurrentRecordIndex; // 09/28/2007 wdk removed static
        /// <!-- number of records in the dataset -->
        public int m_CurrentRecordCount;  // 09/28/2007 wdk removed static
  //      /// <!--true = constructor successful-->
  //      public bool m_bValid; //10/03/2007 wdk/rgc removed to use base class RFCObject.m_bValid
        /// <!-- filter used to select records -->
        public string m_strFilter;
        /// <!--connection to server and database-->
        private SqlConnection m_DBConnection;
        /// <summary>
        /// This returns the SqlConnection which can be used for transactions.
        /// </summary>
        public SqlConnection propDBConnection
        {
            get { return m_DBConnection; }
        }
	
        // this class is dervied from DBAccess so we don't need an instance here
        //private static DBAccess m_DBAccess;// my DBAccess class - initializes the connection for this class in the construtor
        /// <!--used to hold records - initialised in the constructor-->
        public DataSet m_DataSet;
        /// <!-- adapter is used to Fill(dataset) //read and can be used to update 
        /// can't be initalized before SELECT string is built -->
        private SqlDataAdapter m_Adapter;
        

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDataBase"></param>
        /// <param name="strTable"></param>
        /// <param name="errLog">Reference to the Applications ERR class</param>
        public RCRecordset(string strServer, string strDataBase, string strTable, ref ERR errLog)
            : base(strServer, strDataBase, strTable)
        {
            m_bValid = true;
            //throw new System.NotImplementedException();
            //m_DBAccess = new DBAccess(strServer, strDataBase, strTable);
            m_strTableName = strTable;
            
            if (!InitalizeDBConnection(out m_DBConnection))
            {
               // m_strErrMsg = propErrMsg;
                m_bValid = false;
            }
            else
            {
                int iRetVal;
                m_DataSet = new DataSet();
                // 08/14/2007 rgc/wdk added WHERE 1= 2 to the select below to retrieve NO records.
                m_Adapter = new SqlDataAdapter(string.Format("SELECT * from {0} WHERE 1 = 2", propTable), m_DBConnection);
                m_DataSet.Clear();
                iRetVal = m_Adapter.Fill(m_DataSet, propTable);
               
            }
            
            m_CurrentRecordIndex = -1; //zero based index to dataset -1 = no records
            m_CurrentRecordCount = 0;  //0 = no records selected
            m_ERR = errLog; // 06/11/2008 wdk/rgc set the ref errlog to this classes error log.
            try
            {
                m_ERR.propErrMsg = string.Format("[{0}] Recordset Instance Created", propTable);
                m_ERR.ErrorHandler((int)ERR.ErrLevel.eINFO);
            }
            catch (Exception)
            {
                // handle it. don't crash just continue.
            }
        }

  
        /// <summary>
        /// CRecordset BASE CLASS FUNTION -  DOES NOT LOAD record set member variables
        /// should this be private???? rgc
        /// This is your C++ Open or Requerry... we don't open and close.
        /// Fills the dataset using RCRecordset's SqlDataAdaper based
        /// on the where clause passed in.
        /// </summary>
        /// <param name="strWhere"> (don't include 'WHERE' in the parameter)</param>
        /// <returns>REAL number of records selected</returns>
        public int Querry(string strWhere)
        {
            // raise exception if m_bValid = false
            if (!m_bValid)
            {
                m_ERR.m_Logfile.WriteLogFile("RFClassLibrary's RCRecordset is not valid");
                StackTrace here = new StackTrace(true);// true to have trace include file name and line numbers
                throw new SystemException(string.Format("RCRecordset {0} NOT valid - possible construtor failure - check parameter to constructor.\n {1}", propTable, here.ToString()));
               
            }

            //throw new System.NotImplementedException();
            int iRetVal = -1; // 08/11/2007 wdk added assignment of -1 because of moving assignment inside of try catch block.
            // rgc/wdk 20090326 add order by mod_date desc to select statment below to harness the power of m_DataSet's ability to hold multiple records.
            string strSelect = string.Format("SELECT * from {0} WHERE {1} ",
                                                    propTable,
                                                     strWhere);

       //     m_ERR.m_Logfile.WriteLogFile("RCRecordset's Query() strWhere");
       //     m_ERR.m_Logfile.WriteLogFile(strWhere);
            m_ERR.m_Logfile.WriteLogFile("RCRecordset's Query() strSelect");
            m_ERR.m_Logfile.WriteLogFile(strSelect);
            
            m_Adapter = new SqlDataAdapter(strSelect, m_DBConnection); 
            m_DataSet.Clear(); 
            
            // 08/11/2007 wdk added try and catch aroung the m_Adapter.Fill(m_DataSet, propTable)
            // if strWhere cannot be evaluated like rowguid = '' prevents the crash.
            try
            {
                iRetVal = m_Adapter.Fill(m_DataSet, propTable);
            }
            catch (SqlException se)
            {
               // m_CurrentRecordCount = -1;
                m_ERR.m_Logfile.WriteLogFile("SQL EXCEPTION");
                m_ERR.m_Logfile.WriteLogFile(strSelect);
                m_strErrMsg = string.Format("ERROR in [{0}]: {1}\r\n\n Select String: {2}", propTable, se.Message, strSelect);
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
            }
            catch (InvalidOperationException ioe) // rgc/wdk 20090514 added to catch invalid connection errors
            {
               // m_CurrentRecordCount = -1;
                m_ERR.m_Logfile.WriteLogFile("INVALID OPERATION EXCEPTION");
                m_ERR.m_Logfile.WriteLogFile(strSelect);
                m_strErrMsg = string.Format("ERROR in [{0}]: {1}\r\n\n Select String: {2}", propTable, ioe.Message, strSelect);
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
            }
            catch (Exception ex)
            {
                m_ERR.m_Logfile.WriteLogFile("PLAIN EXCEPTION");
                m_ERR.m_Logfile.WriteLogFile(strSelect);
                m_ERR.m_Logfile.WriteLogFile(ex.GetType().ToString());
                m_ERR.m_Logfile.WriteLogFile(ex.Message);
            }
          //  m_ERR.m_Logfile.WriteLogFile("No Exception in Query()");
            //===========
            m_CurrentRecordCount = iRetVal;
            if (m_CurrentRecordCount > 0)
            {
                m_CurrentRecordIndex = 0;
                //LoadMemberVariablesFromDataSet();
            }
            else
            {
                m_CurrentRecordIndex = -1;
                m_CurrentRecordCount = 0;
            }
            // rgc/wdk 20100525 added to retry the query if the sql server is not responding in a normal fashion.
            // the Monitor will keep this from being an endless loop.
            if (iRetVal == -1 && m_strErrMsg.Contains("Timeout expired."))
            {
                m_ERR.m_Logfile.WriteLogFile("May need to reconnect to the sql server here.");
                Time.Wait(30000);
                iRetVal = Querry(strWhere);
            }

            //===========
            m_ERR.m_Logfile.WriteLogFile(string.Format("RetVal = {0}", iRetVal));
            return iRetVal;
        }

        //int Update()
        //{
        //    //WPatAdapter.Update(WPatDataSet,"wpat");
        //    m_Adapter.Update(m_DataSet, propTable.ToString());
        //    I don't this we can do this here
        //    I don't think our dataset has the UPDATE INSERT and DELETE statement
        //    at this time (04/12/2007) rgc
        //}
        /// <summary>
        /// Returns the number of records that is in the dataset for this where clause.
        /// But the dataset is updated but the variables are not. Update() and AddRecord(strWhere) both update the dataset
        /// and load the member variables. 
        /// 
        /// </summary>
        /// <param name="strWhere">filter for the querry</param>
        /// <returns>Count of the records for this where clause</returns>
        public int GetRecordCount(string strWhere)
        {
            m_ERR.m_Logfile.WriteLogFile("entering GetRecordCount");
            int nRetVal = Querry(strWhere);
            m_ERR.m_Logfile.WriteLogFile(string.Format("Querry returned {0}", nRetVal));
            return (nRetVal);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public int GetRecCount(string filter)
        {
            m_ERR.m_Logfile.WriteLogFile("entering RecCount");
            int nRetVal = RecCount(propTable, filter, out m_strErrMsg);
            m_ERR.m_Logfile.WriteLogFile(string.Format("Query returned {0}", nRetVal));
            return (nRetVal);
        }
    }
}
