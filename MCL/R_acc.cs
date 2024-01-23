/*
 * R_acc 08/09/2007 Rick Crone and David
 * 
 * Class dervied from RCRecordset!
 * 
 * This is the acc recordset
 * 
 * The caller should use the m_str field variables to read or update
 * the values in the table.
 * 
 * calling example:
 *          // INSTANIATE THE RECORD SET 
            R_acc r_acc = new R_acc("MCL02","MCLTEST");
            if (!r_acc.m_bValid)
            {
                r_acc.DispErrMsg();
                return;
            }

            // READING A RECORD with 'special' function
            if (r_acc.GetRecord(strAcc) == -1) // 0 = no record found -1 = error
            {
                r_acc.DispErrMsg();
                return;
            }

            // ADDING A NEW RECORD
            // clear first to avoid values from reading some other record
            // and to set some of the 'standard' field values appropriate 
            // for a NEW record
            r_acc.ClearMemberVariables();
            // set 'field' values
            r_acc.m_strIcd9_desc = "SICKLY";
            r_acc.m_strAMA_year = "2007";
            r_acc.m_strIcd9_num = "999.9";
            //r_acc.m_strModPrg = OS.GetAppName();
            // call the AddRecord() function
            if (r_acc.AddRecord() == -1)
            {
                r_acc.DispErrMsg();
                return;
            }
            
            //UPDATING AN EXISTING RECORD
            // FIRST GET THE RECORD
            //- in this case the record I just wrote so this is NOT nessasary as it is the current record
            if (r_acc.GetRecord("999.9", "2007") < 1)
            {
                r_acc.DispErrMsg();
                return;
            }

            // change any field values
            r_acc.m_strIcd9_desc = "Very sickly";
            if (r_acc.Update() == - 1)
            {
                r_acc.DispErrMsg();
                return;
            }

 */
using System;
using System.Collections.Generic;
using System.Text;
//--- added
using Utilities;
using System.Data.SqlClient; // SQL 7.0
using System.Data; // DataRow

namespace MCL
{
    /// <summary>
    /// RCRecordset constructor
    /// </summary>
    [Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
    public class R_acc : RCRecordset
    {
       
        // rgc/wdk 20110119 added Other vendor patient and order id to record set
        private string m_strOvOrderId;
        /// <summary>
        /// Other vendors order ID (EHS, LIFEPOINT, etc)
        /// </summary>
        public string propOvOrderId
        {
            get { return m_strOvOrderId; }
            set { m_strOvOrderId = value; }
        }
        private string m_strOvPatId;
        /// <summary>
        /// Other vendors patient id (EHS, LIFEPOINT, etc)
        /// </summary>
        public string propOvPatId
        {
            get { return m_strOvPatId; }
            set { m_strOvPatId = value; }
        }
        private string m_strPostDate;
        /// <summary>
        /// wdk 20110114 added
        /// </summary>
        public string propPostDate
        {
            get { return m_strPostDate; }
            set
            {
                m_strPostDate = value;
            }
        }
        private string m_strCL_INVOICE;
        /// <summary>
        /// rgc/wdk 20110105 added for handling multiple account numbers from Lifepoint registrations. Get from the number wheel and add CL prefix
        /// </summary>
        public string propCL_Invoice
        {
            get { return m_strCL_INVOICE;}
            set
            {
                m_strCL_INVOICE = value;
            }
        }
        string m_strServer;
        string m_strArcDatabase;
        string m_strDatabase;
        private bool m_bArchive;
        /// <summary>
        /// True if the database passed in to the classes constructor contains "ARC"
        /// False if it does not
        /// </summary>
        public bool propIsArchiveDB
        {
            get { return m_bArchive; }
        }
  
        //===== string variables for fields in record  =====
        ///// <summary>
        ///// 
        ///// </summary>
        //public string m_strRowguid;

        /// <summary>
        /// Returns the current records row guid.
        /// 08/11/2007 wdk
        /// </summary>
        public string propRowguid
        {
            get { return m_strRowguid; }
        }
	
        ///// <summary>
        ///// 
        ///// </summary>
        //public string m_strDeleted;

        /// <summary>
        /// Added for use in Error Messaging
        /// 08/11/2007 wdk 
        /// </summary>
        public string m_strWhere;

        /// <summary>
        /// 
        /// </summary>
        public string m_strAccount;

        /// <summary>
        /// 
        /// </summary>
        public string m_strPatName;

        /// <summary>
        /// 
        /// </summary>
        public string m_strCliMnem;

        /// <summary>
        /// 
        /// </summary>
        public string m_strFinCode;

        /// <summary>
        /// 
        /// </summary>
        private string m_strTransDate;
        /// <summary>
        /// Property for the transaction date
        /// </summary>
        public string propTransDate
        {
            get { return m_strTransDate; }
            set
            {
                try
                {
                    DateTime.Parse(value.ToString());
                }
                catch (Exception)
                {
                    m_strTransDate = "";
                    propErrMsg = "Could not convert TransDate value to a date.";
                    return;
                }
                m_strTransDate = value;
            }
        }


        /// <summary>
        /// 
        /// </summary>
        private string m_strCbillDate;
        /// <summary>
        /// 
        /// </summary>
        public string propCbillDate
        {
            get { return m_strCbillDate; }
            set
            {
                if (value.ToString().Length != 0)
                {
                    m_strErrMsg = string.Format("INVALID CBILL DATE. IN FUNCTION {0}", OS.getCallingMedthodName());
                    DateTime.Parse(value.ToString());
                    m_strErrMsg = "";
                }
                m_strCbillDate = value;
            }
        }

        
        /// <summary>
        /// 
        /// </summary>
        public string m_strStatus;
        
        /// <summary>
        /// 
        /// </summary>
        public string m_strSsn;
        
        /// <summary>
        /// 
        /// </summary>
        public string m_strNumComments;
        
        /// <summary>
        /// 
        /// </summary>
        public string m_strMeditechAccount;
        
        /// <summary>
        /// 
        /// </summary>
        public string m_strOriginalFincode;

        ///// <summary>
        ///// 
        ///// </summary>
        //public string m_strModDate;
        
        ///// <summary>
        ///// 
        ///// </summary>
        //public string m_strModUser;
        
        ///// <summary>
        ///// 
        ///// </summary>
        //public string m_strModPrg;
            
            /// <summary>
        /// 
        /// </summary>
        public string m_strOereqno;
        
        /// <summary>
        /// 
        /// </summary>
        public string m_strMri;
            
        ///// <summary>
        ///// 
        ///// </summary>
        //public string m_strModHost;

        //===== end of string variables for fields in record  =====

        /// <summary>
        /// Consructor - base class construtor does the work!
        /// passes table name to base class DBAccess
        /// - we can get the table name from the base class with propTable.ToString()
        /// 04/05/2007 Rick Crone
        /// </summary>
        /// <param name="strServer">Server</param>
        /// <param name="strDataBase">Database</param>
        /// <param name="errLog">Reference to the error log</param>
        public R_acc(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "acc", ref errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDataBase;
            if (strDataBase.ToUpper().IndexOf("ARC") > -1)
            {
                m_bArchive = true;
            }
            else
            {
                m_bArchive = false;
                m_strArcDatabase = strDataBase.ToUpper().Insert(3, "ARC"); ;
            }
            string[] strArgs = { m_strServer, m_strDatabase, m_strArcDatabase, string.Format("Archiving = {0}", m_bArchive.ToString()) };
            
            m_ERR.m_Logfile.WriteLogFile(string.Format("R_acc Created: \r\nServer: {0}{1}DataBase: {2}{3}ArchiveDatabase: {4}{5}{6}", strArgs[0], Environment.NewLine, strArgs[1], Environment.NewLine, strArgs[2], Environment.NewLine, strArgs[3]));

        }

        #region obsolete constructor
        // <summary>
        // Consructor - base class construtor does the work!
        // passes table name to base class DBAccess
        // - we can get the table name from the base class with propTable.ToString()
        // 04/05/2007 Rick Crone
        // </summary>
        // <param name="strServer"></param>
        // <param name="strDataBase"></param>
        //[Obsolete ("Use the constructor. \r\n\t public R_acc(string strServer, string strDataBase, ref ERR errLog)")]
        //public R_acc(string strServer, string strDataBase)
        //    : base(strServer, strDataBase, "acc")
        //{
        //    m_strServer = strServer;
        //    m_strDatabase = strDataBase;
        //    if (strDataBase.ToUpper().IndexOf("ARC") > -1)
        //    {
        //        m_bArchive = true;
        //    }
        //    else
        //    {
        //        m_bArchive = false;
        //        m_strArcDatabase = strDataBase.ToUpper().Insert(3, "ARC"); ;
        //    }
        //    string[] strArgs = { m_strServer, m_strDatabase, m_strArcDatabase, string.Format("Archiving = {0}", m_bArchive.ToString()) };
        //    m_ErrLog = new ERR(strArgs);
        //    //  RFCObject rfc = new RFCObject();
        //    m_ErrLog.m_Logfile.WriteLogFile(string.Format("Server: {0}{1}DataBase: {2}{3}ArchiveDatabase: {4}{5}{6}", strArgs[0], Environment.NewLine, strArgs[1], Environment.NewLine, strArgs[2], Environment.NewLine, strArgs[3]));

        //}
        #endregion obsolete constructor
      
        
        /// <summary>
        /// Determines if the passed in account exits in the table account table.
        /// Returns:
        ///     0 = the account exists and is NOT deleted. 
        ///     1 = the account exists and is deleted. 
        ///    -1 = the account does not exist in the table.
        /// </summary>
        /// <param name="strAccount"></param>
        /// <returns></returns>
        public int DoesAccountExist(string strAccount)
        {
            int iRetVal = -1;
            int nCount = GetRecords(string.Format("account = '{0}'", strAccount));
            // found a record either deleted or not.
            if (nCount == 1)
            {
                iRetVal = m_strDeleted.ToUpper() == "F" ? 0 : 1;
            }
            return iRetVal;
        }

        /// <summary>
        /// Generic GetRecords  - pass in where clause (without the WHERE key word).
        /// Can get active or deleted records!
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecords(string strWhere)
        {
            //strWhere = string.Format("msg_status in ('NEW','REQUED') AND msg_type = 'ORDERS'  AND deleted = 0");
            m_strWhere = strWhere; //08/11/2007 wdk 
            Querry(strWhere);
            if (m_CurrentRecordCount > 0)
            {
                LoadMemberVariablesFromDataSet();
                m_strErrMsg = string.Format("{0} Record(s) read", m_CurrentRecordCount);
            }

            return (m_CurrentRecordCount);

        }

        /// <summary>
        /// Generic GetActiveRecords  - pass in where clause (without the WHERE).
        /// Gets only ACTIVE records - ie NOT flagged deleted.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetActiveRecords(string strWhere)
        {
            //throw new System.NotImplementedException();
            strWhere = string.Format("deleted = 0 AND {0}",
                                        strWhere);

            return (GetRecords(strWhere));
        }

 
        /// <summary>
        /// Loads the 'field' member variables for the next record.
        /// Sets m_ErrMsg to "EOF" if no more records exist.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public bool GetNext()
        {
            bool bRetVal = false; // eof or error on LoadMemberVariablesFromDataSet()
           // m_strErrMsg = string.Empty;
            if (m_CurrentRecordIndex < (m_CurrentRecordCount - 1))
            {
                m_CurrentRecordIndex++;
                return (LoadMemberVariablesFromDataSet());
            }
            m_strErrMsg = "EOF";
            return (bRetVal);

        }

        /// <summary>
        /// Loads the 'field' member variables for the previous record.
        /// Sets m_ErrMsg to "BOF" if no more records exist.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public bool GetPrev()
        {
            bool bRetVal = false; // eof or error on LoadMemberVariablesFromDataSet()
            if (m_CurrentRecordIndex > 1)
            {
                m_CurrentRecordIndex--;
                return (LoadMemberVariablesFromDataSet());
            }
            m_strErrMsg = "BOF";
            return (bRetVal);

        }

        /// <summary>
        /// Loads the 'field' member variables for the first record.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns>false = error</returns>
        public bool MoveFirst()
        {
            m_CurrentRecordIndex = 1;
            return (LoadMemberVariablesFromDataSet());

        }

        /// <summary>
        /// Loads the 'field' member variables for the last record.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns>false = error</returns>
        public bool MoveLast()
        {
            m_CurrentRecordIndex = m_CurrentRecordCount;
            return (LoadMemberVariablesFromDataSet());

        }

        /// <summary>
        /// Sets the deleted flag on the current record
        /// 04/12/2007 Rick Crone
        /// Modified to ensure there is a current record.
        /// 08/11/2007 wdk
        /// </summary>
        /// <returns></returns>
        public int FlagCurrentRecordDeleted()
        {
            // 08/11/2007 wdk
            if (m_CurrentRecordIndex < 0)
            {
                m_strErrMsg = string.Format("There is no current record.\r\n\n Where clause: {0}", m_strWhere);
                return -1;
            }
            // end of 08/11/2007 
            m_strDeleted = "T";
            return (Update());
        }

        /// <summary>
        /// This function can never succeed. the GetRecords all call GetActiveRecords which sets the deleted flag =0
        /// Meaning there can never be an active record that is deleted to be restored.
        /// Added new function called GetDeletedRecords(string) that must be called before this call.
        /// Clears the deleted flag on the current record
        /// 04/12/2007 Rick Crone
        /// Modified to ensure there is a current record.
        /// 08/11/2007 wdk
        /// </summary>
        /// <returns></returns>
        public int FlagCurrentRecordNOTDeleted()
        {
            // 08/11/2007 wdk
            if (m_CurrentRecordIndex < 0)
            {
                m_strErrMsg = string.Format("There is no current record.\r\n\n Where clause: {0}", m_strWhere);
                return -1;
            }
            m_strDeleted = "F";
            return (Update());
        }

        //======== generic stuff above this line ===========

        // below this commment======= general stuff
        //         the fuction signatures need to be the same 
        //         the 'fields' are for the table this class accesses
        //=======================================================================

        /// <summary>
        /// Clears all the 'field' member variables.
        /// Call this before setting the values for a new record.
        /// 04/12/2007 Rick Crone
        /// </summary>
        public void ClearMemberVariables()
        {

            // standard fields
            m_strDeleted = "F";
            m_strRowguid = "";// Guid.NewGuid().ToString();
            m_strModDate = Time.SNows();
            m_strModHost = OS.GetMachineName();
            m_strModPrg = OS.GetAppName();
            m_strModUser = OS.GetUserName();

            //--- table fields
             m_strAccount = "";
             m_strPatName = "";
             m_strCliMnem = "";
             m_strFinCode = "";
             m_strTransDate = "";
             m_strCbillDate = "";
             m_strStatus = "";
             m_strSsn = "";
             m_strNumComments = "";
             m_strMeditechAccount = "";
             m_strOriginalFincode = "";
             m_strOereqno = "";
             m_strMri = "";
            // wdk 20110114 added
             m_strPostDate = ""; 
            // rgc/wdk 20110119 added
             m_strOvPatId = "";
             m_strOvOrderId = "";

        }

        /// <summary>
        /// Move data from dataset into this classes 'field' member variables.
        /// Note: 'generic' member functions expect this function to be named:
        ///             LoadMemberVariablesFromDataSet()
        /// 04/05/2007 Rick Crone
        /// </summary>
        /// <returns></returns>
        public bool LoadMemberVariablesFromDataSet()
        {
            ClearMemberVariables();
            //throw new System.NotImplementedException();
            bool bRetVal = false;
            if (m_CurrentRecordIndex > -1) //don't attempt to load if there are no records
            {
                // ==== 'standard' fields =====================
                m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();
                m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                {
                    m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("G");
                }
                //m_strModDate = Time.DateTimeToHL7TimeString(m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]);
                m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
                m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                // ==== end of 'standard' fields =====================

             m_strPatName = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_name"].ToString();
             m_strCliMnem    = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cl_mnem"].ToString();
             m_strFinCode    = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"].ToString();
             m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"].ToString();

             if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["trans_date"].Equals(System.DBNull.Value))
             {
                 m_strTransDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["trans_date"]).ToString("d");
             }

             if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cbill_date"].Equals(System.DBNull.Value))
             {
                 m_strCbillDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cbill_date"]).ToString("d");
             }
             m_strStatus = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["status"].ToString();
             m_strSsn = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ssn"].ToString();
             m_strNumComments = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["num_comments"].ToString();
             m_strMeditechAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["meditech_account"].ToString();
             m_strOriginalFincode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["original_fincode"].ToString();
             m_strOereqno = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["oereqno"].ToString();
             m_strMri = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mri"].ToString();
             if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_date"].Equals(System.DBNull.Value))
             {
                 m_strPostDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_date"]).ToString("d");
             }

             // rgc/wdk 20110119 added
             m_strOvOrderId = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ov_order_id"].ToString();
             m_strOvPatId = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ov_pat_id"].ToString();
             

                bRetVal = true;

            }

            return (bRetVal);
        }


        /// <summary>
        /// Add a NEW record to the table 
        /// 
        /// Caller should call the ClearMemberVariables() function.
        /// 
        /// Then load this class's member variables and call this function to
        /// add a new record.
        /// 
        /// This function will create the rowguid for the inserted record. Caller should not attempt to set!
        /// 
        /// This record is added to the end of the dataset and is the current
        /// record after this call.
        /// 04/05/2007 Rick Crone
        /// </summary>
        /// <returns>number of records updated 
        /// Or 
        /// -1 = Error m_strErrMsg has detailis of error</returns>
        public int AddRecord()
        {
            if (m_bArchive)
            {
                m_strErrMsg = "You cannot add a record to the archive database. You must use ArchiveRecord() instead.";
                return -1;
            }
            //if (m_strRowguid.Length > 0) rgc/wdk 20110104 changed to IsNullorEmpty() check
            if (!string.IsNullOrEmpty(m_strRowguid))
            {
                m_strErrMsg = "Caller attempted to add record before clearing variables, or attempted to set rowguid.";
                return -1;
            }
            int iRetVal = -1;
            string strSQL;

            //INSERT INTO MyTable (PriKey, Description)
            //   VALUES (123, 'A description of part 123.')
            //                                table fields   

            m_strRowguid = Guid.NewGuid().ToString();
            strSQL = string.Format("INSERT INTO {0}(rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, mod_prg, oereqno, mri, post_date, ov_pat_id, ov_order_id ) "+
                                            "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}',   '{6}',     '{7}',      {8},     '{9}',  '{10}', '{11}',     '{12}',           '{13}',          '{14}',  '{15}',  '{16}', {17}, '{18}','{19}')",
                                                    propTable.ToString(), //0
                                                    m_strRowguid,
                                                    0,//1
                                                    m_strAccount,
                                                    m_strPatName,
                                                    m_strCliMnem,
                                                    m_strFinCode,
                                                    m_strTransDate,
                                                    string.IsNullOrEmpty(m_strCbillDate) ? "NULL" : string.Format("'{0}'", m_strCbillDate),
                                                    m_strStatus,
                                                    m_strSsn,
                                                    m_strNumComments,
                                                    m_strMeditechAccount,
                                                    m_strOriginalFincode,
                                                    OS.GetAppName(),
                                                    m_strOereqno,
                                                    m_strMri, 
                                                    string.IsNullOrEmpty(m_strPostDate)?"NULL":string.Format("'{0}'",m_strPostDate),
                                                    m_strOvPatId,
                                                    m_strOvOrderId);
                                                                    
              
            iRetVal = SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                iRetVal = Querry(m_strWhere);
                LoadMemberVariablesFromDataSet();
                // add new record to dataset
                //DataRow L_DataRow = m_DataSet.Tables[propTable].NewRow();
                //m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);

                //// index is zero based while record count is 1 based ie the correct record count
                //m_CurrentRecordIndex = m_CurrentRecordCount++;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"] = m_strRowguid;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = 0; // new record so flag it NOT deleted
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_name"] = m_strPatName;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cl_mnem"] = m_strCliMnem;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"] = m_strFinCode;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["trans_date"] = m_strTransDate;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cbill_date"] = m_strCbillDate;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["status"] = m_strStatus;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ssn"] = m_strSsn;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["num_comments"] = m_strNumComments;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["meditech_account"] = m_strMeditechAccount;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["original_fincode"] = m_strOriginalFincode;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"] = OS.GetAppName();
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["oereqno"] = m_strOereqno;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mri"] = m_strMri;
            }
            return iRetVal;
        }


        /// <summary>
        /// Add a NEW record to the archive table 
        /// Load this class's member variables and call this function to
        /// add a new record.
        /// 
        /// This record is added to the end of the dataset and is the current
        /// record after this call.
        /// 04/05/2007 Rick Crone
        /// </summary>
        /// <returns>number of records updated 
        /// Or 
        /// -1 = Error m_strErrMsg has detailis of error</returns>
        public int ArchiveRecord()
        {
            int iRetVal = -1;
            if (m_bArchive)
            {
                m_strErrMsg = "You cannot archive from the archive database.";
                return iRetVal;
            }
            string strSQL;
            R_acc accArc = new R_acc(m_strServer, m_strArcDatabase, ref m_ERR);
   
            strSQL = string.Format("INSERT INTO {0}(rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, mod_date, mod_user, mod_prg, oereqno, mri, mod_host ) " +
                                            "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}',   '{6}',     '{7}',      '{8}',     '{9}',  '{10}', '{11}',     '{12}',           '{13}',          '{14}',  '{15}',    '{16}',  '{17}',  '{18}', '{19}'   )",
                                                    propTable, //0
                                                    m_strRowguid,
                                                    m_strDeleted,
                                                    m_strAccount,
                                                    m_strPatName,
                                                    m_strCliMnem,
                                                    m_strFinCode,
                                                    m_strTransDate,
                                                    string.IsNullOrEmpty(m_strCbillDate) ? "NULL" : string.Format("'{0}'", m_strCbillDate),
                                                    m_strStatus,
                                                    m_strSsn,
                                                    m_strNumComments,
                                                    m_strMeditechAccount,
                                                    m_strOriginalFincode,
                                                    m_strModDate,
                                                    m_strModUser,
                                                    m_strModPrg,
                                                    m_strOereqno,
                                                    m_strMri,
                                                    m_strModHost);//15


            iRetVal = accArc.SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                string strDelete = string.Format("DELETE FROM {0} WHERE (account = '{1}')",
                                                    propTable, m_strAccount);
                iRetVal = SQLExec(strDelete, out m_strErrMsg);
                if (iRetVal == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strDelete);
                    m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                    return iRetVal;
                }
                R_accAudit rAccAudit = new R_accAudit(m_strServer, m_strDatabase, ref m_ERR);
                string strAuditWhere = string.Format("acc_rowguid = '{0}'", m_strRowguid);
                int nAuditRecs = rAccAudit.GetRecords(strAuditWhere);
                if (nAuditRecs == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strAuditWhere);
                    m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                    return iRetVal;
                }
                for (int i = 0; i < nAuditRecs; i++)
                {
                    if (rAccAudit.ArchiveRecord() < 1)
                    {
                        m_ERR.m_Logfile.WriteLogFile(propErrMsg);
                    }
                    rAccAudit.GetNext();
                }
            }
            else
            {
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
            }
            return iRetVal;
        }


        /// <summary>
        /// Updates the 'current' record.
        /// Rick Crone 04/12/2006
        /// </summary>
        /// <returns>number of records 
        /// OR 
        /// -1 = ERROR - see m_strErrMsg for details</returns>
        public int Update()
        {
            if (m_bArchive)
            {
                m_strErrMsg = "You cannot update a record to the archive database.";
                return -1;
            }
            string strSQL;
            string strWhere;
            /*
            strSQL = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3}",
                m_strTable, //* a property field in this class 
                strField,
                strNewValue,
                strFilter);
            */

            /*
             * Set the where clause for the KEY for this table from the DataSet values
             */
            string strCbillDate = "";
            if (m_strCbillDate.Length == 0)
            {
                strCbillDate = "null";
            }
            else
            {
                strCbillDate = string.Format("'{0}'", m_strCbillDate);
            }
            strWhere = string.Format("rowguid = '{0}' ",
                                        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());

            strSQL = string.Format("UPDATE {0} SET deleted= '{1}', account= '{2}', pat_name= '{3}', cl_mnem= '{4}', fin_code= '{5}', trans_date= '{6}', cbill_date= {7}, status= '{8}', ssn= '{9}', num_comments= '{10}', meditech_account = '{11}', original_fincode= '{12}', mod_prg= '{13}', oereqno= '{14}', mri= '{15}', post_date = {16}, ov_pat_id = '{17}', ov_order_id = '{18}' WHERE {19}",
                                       propTable, //0
                                        m_strDeleted == "T" ? 1 : 0,//1
                                                    m_strAccount,
                                                    m_strPatName,
                                                    m_strCliMnem,
                                                    m_strFinCode,
                                                    m_strTransDate,
                                                    string.IsNullOrEmpty(m_strCbillDate) ? "NULL" : string.Format("'{0}'", m_strCbillDate), //strCbillDate, // not the member variable pay attention.
                                                    m_strStatus,
                                                    m_strSsn,
                                                    m_strNumComments,
                                                    m_strMeditechAccount,
                                                    m_strOriginalFincode,
                                                    OS.GetAppName(),
                                                    m_strOereqno,
                                                    m_strMri, 
                                                    string.IsNullOrEmpty(m_strPostDate) ? "NULL" : string.Format("'{0}'", m_strPostDate),
                                                    m_strOvPatId,
                                                    m_strOvOrderId,
                                                     //m_strWhere); //08/11/2007 wdk made into member level variable for Error Msg inclusion
                                                     strWhere); // wdk 20110114 removed member level changes as this causes the record set to be requeired with only this record.

            int nRetVal =  SQLExec(strSQL, out m_strErrMsg);
            if (nRetVal > 0)
            {
                nRetVal = Querry(m_strWhere);
                LoadMemberVariablesFromDataSet();
            }
            return nRetVal;

        }
        //==== 'special' functions just for this class below this line ======


        //==== 'special' functions just for this class below this line ======
        /// <summary>
        /// Gets ACTIVE acc record for the rowguid passed in
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecordByRowguid(string strRowguid)
        {
            //throw new System.NotImplementedException();
            string strWhere;                      // table
            strWhere = string.Format("rowguid = '{0}'",
                                          strRowguid);
            m_strWhere = strWhere; //08/11/2007 wdk
            return (GetActiveRecords(strWhere));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strDateOfService"></param>
        /// <returns></returns>
        public string GetAccountNumberFromNameAndDateOfService(string strName, string strDateOfService)
        {
            string strRetVal = "";
            string strWhere = string.Format("pat_name = '{0}' and trans_date = '{1}'", strName, strDateOfService);
            GetActiveRecords(strWhere);
            strRetVal = m_strAccount;
            return strRetVal;
        }
        /// <summary>
        /// Gets ACTIVE acc record for the rowguid passed in
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecordByAccount(string strAccount)
        {
            //throw new System.NotImplementedException();
            string strWhere;                      // table
            strWhere = string.Format("account = '{0}'",
                                          strAccount);
            m_strWhere = strWhere; //08/11/2007 wdk
            return (GetActiveRecords(strWhere));

        }



        //==== 'special' functions just for this class below this line ======
        /// <summary>
        /// Gets ACTIVE acc record for the rowguid passed in
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecordByStatus(string strStatus)
        {
            
            //throw new System.NotImplementedException();
            string strWhere;                      // table
            strWhere = string.Format("status = '{0}'",
                                          strStatus);

            m_strWhere = strWhere; // 08/11/2007 wdk
            return (GetActiveRecords(strWhere));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int UnArchiveRecord()
        {
            int iRetVal = -1;
            if (!m_bArchive)
            {
                m_strErrMsg = "Cannot unarchive a live record.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return iRetVal;
            }
            R_acc accUnArc = new R_acc(m_strServer, m_strDatabase.Remove(3, 3), ref m_ERR);
            string strSQL = string.Format("INSERT INTO {0}(rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, mod_date, mod_user, mod_prg, oereqno, mri, mod_host ) " +
                                  "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}',   '{6}',     '{7}',      '{8}',     '{9}',  '{10}', '{11}',     '{12}',           '{13}',          '{14}',  '{15}',    '{16}',  '{17}',  '{18}', '{19}'   )",
                                          propTable, //0
                                          m_strRowguid,
                                          m_strDeleted,
                                          m_strAccount,
                                          m_strPatName,
                                          m_strCliMnem,
                                          m_strFinCode,
                                          m_strTransDate,
                                          m_strCbillDate,
                                          m_strStatus,
                                          m_strSsn,
                                          m_strNumComments,
                                          m_strMeditechAccount,
                                          m_strOriginalFincode,
                                          m_strModDate,
                                          m_strModUser,
                                          m_strModPrg,
                                          m_strOereqno,
                                          m_strMri,
                                          m_strModHost);//15

            
            iRetVal = accUnArc.SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                string strDelete = string.Format("DELETE FROM {0} WHERE (account = '{1}')",
                                                    propTable, m_strAccount);
                iRetVal = SQLExec(strDelete, out m_strErrMsg);
                if (iRetVal == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strDelete);
                    m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                    return iRetVal;
                }
                R_accAudit rAccAuditUnArc = new R_accAudit(m_strServer, m_strDatabase, ref m_ERR);
                string strAuditWhere = string.Format("acc_rowguid = '{0}'", m_strRowguid);
                int nAuditRecs = rAccAuditUnArc.GetRecords(strAuditWhere);
                if (nAuditRecs == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strAuditWhere);
                    m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                    return iRetVal;
                }
                for (int i = 0; i < nAuditRecs; i++)
                {
                    if (rAccAuditUnArc.UnArchiveRecord() < 1)
                    {
                        m_ERR.m_Logfile.WriteLogFile(propErrMsg);
                    }
                    rAccAuditUnArc.GetNext();
                }
            }
            else
            {
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
            }
            return iRetVal;
 
        }

        /// <summary>
        /// Gets all accounts deleted or not.
        /// 
        /// 11/12/2007 wdk
        /// </summary>
        /// <param name="dtThru">Trans_date in table to archive before</param>
        /// <returns>Number of records found.</returns>
        public int ArchiveAccounts(DateTime dtThru)
        {
            int iRetVal;
            string strWhere = string.Format("(status IN ('PAID_OUT', 'CLOSED')) AND trans_date < '{0}'", dtThru.ToShortDateString());
            iRetVal = GetRecords(strWhere);
            return iRetVal;
        }

      

        //public System.Data.DataRowCollection Rows()
        //{

        //    int max =  m_DataSet.Tables[0].Rows.Count;
        //    for (int i = 0; i < max; i++)
        //    {
        //        yield return m_DataSet.Tables[0].Rows;
        //    }
        //}
        //public System.Collections.Generic.IEnumerator<DataColumnCollection> Columns()
        //{
        //    int max = m_DataSet.Tables[0].Columns.Count;
        //    for (int i = 0; i < max; i++)
        //    {
        //        yield return (m_DataSet.Tables[0].Columns);
        //    }
        //}

        /// <summary>
        /// Returns the Account number and name for this account.
        /// 
        /// 11/15/2007 wdk
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "Account: " + this.m_strAccount.ToString() + Environment.NewLine+ "Name: " + this.m_strPatName;
        }


        /// <summary>
        /// Custom name format for the Account class. 
        /// 
        /// example for use
        /// str = acc.ToString("Last Name: {0} First Name: {1} Middle Init:{2} Middle Name:{3}","l","f","i","m");
        /// 11/23/2007 wdk
        /// </summary>
        /// <param name="strFormat"></param>
        /// <param name="p"></param>
        /// <returns></returns>
        public string ToString(string strFormat, params string[] p)
        {
            string strRetVal = "";
            // do some minor error checking, for format 
            int nOpens = 0;
            int nCloses = 0;
            // if there are more than 4 format specifiers then we have a problem this is a name format
            // first, last, middle, and/or initial are all that are allowed.
            if (p.Length > 4)
            {
                return ("Can only format First Name, Middle Name and/or Middle Initial, and Last Name");
            }
            foreach (string s in p)
            {
                if (s.IndexOfAny(new char[] { 'f', 'l', 'm', 'i' }) == -1)
                {
                    return (string.Format("Invalid format specifier {0}", s));
                }
            }
            // ensure that our string format contains enough closes for each of the open curly braces
            foreach (char c in strFormat)
            {
                if (c == '{')
                    nOpens++;
                if (c == '}')
                    nCloses++;
            }
            // if there is a difference 
            if (nOpens != nCloses)
            {
                return ("Incorrectly formatted string");
            }
            // ensure that the number of opens matches the number of paramaraters passed in.
            if (nOpens != p.Length)
            {
                return ("Number of paramaters does not match the number of elements.");
            }

            string[] strArrayName = new string[p.Length];

            string strName = m_strPatName;
            int nComma = strName.IndexOf(',');
            string strLName = "";
            string strFName = "";
            string strMName = ""; // if no middle name given 
            string strMInit = "";
            string[] strSplitWhole = m_strPatName.Split(new char[] { ',' });
            strLName = strSplitWhole[0];
            string[] strSplitAfterComma = strSplitWhole[1].Split(new char[] { ' ' });
            strFName = strSplitAfterComma[0];
            if (strSplitAfterComma.Length > 1)
            {
                strMName = strSplitAfterComma[1];
                if (strMName.Length > 0)
                {
                    strMInit = strMName.Substring(0, 1);
                }
            }
            //string strLName = strName.Substring(0, nComma);
            //try
            //{
            //    int nSpace = strName.IndexOf(' ', 0);
            //    strFName = strName.Substring((nComma+1), (nSpace-nComma));
            //    strMName = strName.Substring(nSpace+1, ((strName.Length-1)-nSpace));
            //    strMName.Trim();
            //    strMInit = strMName.Substring(0,1);//[0].ToString();
            //}
            //catch (ArgumentOutOfRangeException)
            //{
            //    // no middle name so get the first name
            //    strFName = strName.Substring((nComma + 1));
            //}


            for (int i = 0; i < p.Length; i++)
            {
                if (p[i] == "l")
                {
                    strArrayName.SetValue(strLName, i);
                }
                if (p[i] == "f")
                {
                    strArrayName.SetValue(strFName, i);
                }
                if (p[i] == "m")
                {
                    strArrayName.SetValue(strMName, i);
                }
                if (p[i] == "i")
                {
                    strArrayName.SetValue(strMInit, i);
                }
            }

            switch (strArrayName.Length)
            {
                case 1:
                    {
                        strRetVal = string.Format(strFormat,
                                                    strArrayName[0].ToString());
                        break;
                    }
                case 2:
                    {
                        strRetVal = string.Format(strFormat,
                                                    strArrayName[0].ToString(),
                                                        strArrayName[1].ToString());
                        break;
                    }

                case 3:
                    {
                        strRetVal = string.Format(strFormat,
                                                    strArrayName[0].ToString(),
                                                        strArrayName[1].ToString(),
                                                            strArrayName[2].ToString());
                        break;
                    }

                case 4:
                    {
                        strRetVal = string.Format(strFormat, strArrayName[0].ToString(),
                                                    strArrayName[1].ToString(),
                                                            strArrayName[2].ToString(),
                                                                strArrayName[3].ToString());

                        break;
                    }


            }
            //    strRetVal = string.Format(strFormat, strArrayName[0].ToString(), strArrayName[1].ToString(),strArrayName[2].ToString(),strArrayName[3].ToString());

            return strRetVal;
        }


    } // don't type below this line
}
