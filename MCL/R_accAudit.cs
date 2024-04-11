using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using Utilities; // for RCRecordset
using System.Data; // DataRow

namespace MCL
{
    /// <summary>
    /// 
    /// </summary>
    [Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
    public class R_accAudit : RCRecordset
    {
        private bool m_bArchive;
        /// <summary>
        /// True if the database passed in to the classes constructor contains "ARC"
        /// False if it does not
        /// </summary>
        public bool propIsArchiveDB
        {
            get { return m_bArchive; }
        }
	

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
        /// 
        /// </summary>
        public string propTransDate
        {
            get { return m_strTransDate; }
            set
            {
                DateTime.Parse(value.ToString());
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
                    m_strErrMsg = string.Format("INVALID CBILL DATE. IN FUNCTION {0}", OS.GetCallingMedthodName());
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

  
        /// <summary>
        /// 
        /// </summary>
        public string m_strOereqno;

        /// <summary>
        /// 
        /// </summary>
        public string m_strMri;

        /// <summary>
        /// 
        /// </summary>
        public string m_strUid;

        private string m_strServer;
        private string m_strDatabase;
        //===== end of string variables for fields in record  =====

                /// <summary>
        /// Consructor - base class construtor does the work!
        /// passes table name to base class DBAccess
        /// - we can get the table name from the base class with propTable.ToString()
        /// 10/08/2007 David Kelly
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDataBase"></param>
        /// <param name="errLog"></param>
        public R_accAudit(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "audit_acc", ref errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDataBase;
            m_bArchive = false;
            if (strDataBase.ToUpper().IndexOf("ARC") > -1)
            {
                m_bArchive = true;
            }
            //else
            //{
            //    m_strArcDatabase = strDataBase.Insert(3, "ARC");
            //}
        }

        /// <summary>
        /// Determines if the passed in account exits in the table account table.
        /// Returns:
        ///     0 = the account exists and is NOT deleted
        ///     1 = the account exists and is deleted
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
            Querry(strWhere);
            if (m_CurrentRecordCount > 0)
            {
                LoadMemberVariablesFromDataSet();
                m_strErrMsg = string.Format("{0} Record(s) read", m_CurrentRecordCount);
            }
            return (m_CurrentRecordCount);
        }

        #region NOTE: Should get all audit records reguardless of DELETED flag
        ///// <summary>
        ///// Generic GetActiveRecords  - pass in where clause (without the WHERE).
        ///// Gets only ACTIVE records - ie NOT flagged deleted.
        ///// 04/05/07 Rick Crone
        ///// </summary>
        ///// <returns></returns>
        //public int GetActiveRecords(string strWhere)
        //{
        //    //throw new System.NotImplementedException();
        //    strWhere = string.Format("deleted = 0 AND {0}",
        //                                strWhere);

        //    return (GetRecords(strWhere));
        //}

        #endregion NOTE:Should get all audit records reguardless of DELETED flag

        /// <summary>
        /// Loads the 'field' member variables for the next record.
        /// Sets m_ErrMsg to "EOF" if no more records exist.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public bool GetNext()
        {
            bool bRetVal = false; // eof or error on LoadMemberVariablesFromDataSet()
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

        #region NOTE should not update audit records.
        // <summary>
        // Sets the deleted flag on the current record
        // 04/12/2007 Rick Crone
        // Modified to ensure there is a current record.
        // 08/11/2007 wdk
        // </summary>
        // <returns></returns>
        //public int FlagCurrentRecordDeleted()
        //{
        //    // 08/11/2007 wdk
        //    if (m_CurrentRecordIndex < 0)
        //    {
        //        m_strErrMsg = string.Format("There is no current record.\r\n\n Where clause: {0}", m_strWhere);
        //        return -1;
        //    }
        //    // end of 08/11/2007 
        //    m_strDeleted = "T";
        //    return (Update());
        //}

        // <summary>
        // This function can never succeed. the GetRecords all call GetActiveRecords which sets the deleted flag =0
        // Meaning there can never be an active record that is deleted to be restored.
        // Added new function called GetDeletedRecords(string) that must be called before this call.
        // Clears the deleted flag on the current record
        // 04/12/2007 Rick Crone
        // Modified to ensure there is a current record.
        // 08/11/2007 wdk
        // </summary>
        // <returns></returns>
        //public int FlagCurrentRecordNOTDeleted()
        //{
        //    // 08/11/2007 wdk
        //    if (m_CurrentRecordIndex < 0)
        //    {
        //        m_strErrMsg = string.Format("There is no current record.\r\n\n Where clause: {0}", m_strWhere);
        //        return -1;
        //    }
        //    m_strDeleted = "F";
        //    return (Update());
        //}
        #endregion NOTE

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

            //--- table fields
            m_strAccount = "";
            m_strPatName = "";
            m_strCliMnem = "";
            m_strFinCode = "";
            m_strTransDate = "";
            m_strCbillDate = "";
            m_strStatus = "";
            m_strSsn = "";
            m_strNumComments = "0"; // 11/12/2007 wdk numeric data type with no default.
            m_strMeditechAccount = "";
            m_strOriginalFincode = "";
            m_strOereqno = "";
            m_strMri = "";
            m_strUid = "";

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
            bool bRetVal = false;
            if (m_CurrentRecordIndex > -1) //don't attempt to load if there are no records
            {
                // ==== 'standard' fields =====================
                m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["acc_rowguid"].ToString();
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
                m_strCliMnem = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cl_mnem"].ToString();
                m_strFinCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"].ToString();
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
                m_strUid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["uid"].ToString();
                bRetVal = true;
            }

            return (bRetVal);
        }


        #region AddRecord() is accomplished via triggers for this table
        // <summary>
        // Add a NEW record to the table 
        // 
        // Caller should call the ClearMemberVariables() function.
        // 
        // Then load this class's member variables and call this function to
        // add a new record.
        // 
        // This function will create the rowguid for the inserted record. Caller should not attempt to set!
        // 
        // This record is added to the end of the dataset and is the current
        // record after this call.
        // 04/05/2007 Rick Crone
        // </summary>
        // <returns>number of records updated 
        // Or 
        // -1 = Error m_strErrMsg has detailis of error</returns>
        //public int AddRecord()
        //{
        //    //throw new System.NotImplementedException();
        //    if (m_strRowguid.Length > 0)
        //    {
        //        m_strErrMsg = "Caller attempted to add record before clearing variables, or attempted to set rowguid.";
        //        return -1;
        //    }
        //    int iRetVal = -1;
        //    string strSQL;

        //    //INSERT INTO MyTable (PriKey, Description)
        //    //   VALUES (123, 'A description of part 123.')
        //    //                                table fields   

        //    //m_strRowguid = Guid.NewGuid().ToString();
        //    strSQL = string.Format("INSERT INTO {0}(acc_rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, mod_prg, oereqno, mri ) " +
        //                                    "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}',   '{6}',     '{7}',      '{8}',     '{9}',  '{10}', '{11}',     '{12}',           '{13}',          '{14}',  '{15}',  '{16}')",
        //                                            propTable.ToString(), //0
        //                                            m_strRowguid,
        //                                            0,//1
        //                                            m_strAccount,
        //                                            m_strPatName,
        //                                            m_strCliMnem,
        //                                            m_strFinCode,
        //                                            m_strTransDate,
        //                                            m_strCbillDate,
        //                                            m_strStatus,
        //                                            m_strSsn,
        //                                            m_strNumComments,
        //                                            m_strMeditechAccount,
        //                                            m_strOriginalFincode,
        //                                            OS.GetAppName(),
        //                                            m_strOereqno,
        //                                            m_strMri);//15


        //    iRetVal = SQLExec(strSQL, out m_strErrMsg);
        //    if (iRetVal > 0)
        //    {
        //        // add new record to dataset
        //        DataRow L_DataRow = m_DataSet.Tables[propTable].NewRow();
        //        m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);

        //        // index is zero based while record count is 1 based ie the correct record count
        //        m_CurrentRecordIndex = m_CurrentRecordCount++;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"] = m_strRowguid;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = 0; // new record so flag it NOT deleted
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_name"] = m_strPatName;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cl_mnem"] = m_strCliMnem;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"] = m_strFinCode;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["trans_date"] = m_strTransDate;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cbill_date"] = m_strCbillDate;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["status"] = m_strStatus;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ssn"] = m_strSsn;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["num_comments"] = m_strNumComments;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["meditech_account"] = m_strMeditechAccount;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["original_fincode"] = m_strOriginalFincode;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"] = OS.GetAppName();
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["oereqno"] = m_strOereqno;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mri"] = m_strMri;
        //    }
        //    return iRetVal;
        //}
        #endregion AddRecord() is accomplished via triggers for this table

        /// <summary>
        /// Add a NEW record to the archive table 
        /// Load this class's member variables and call this function to
        /// add a new record.
        /// 
        /// </summary>
        /// <returns>number of records updated 
        /// Or 
        /// -1 = Error m_strErrMsg has detailis of error</returns>
        public int ArchiveRecord()
        {
            if (m_bArchive)
            {
                m_strErrMsg = "Cannot archive from the archive database.";
                return -1;
            }
            string strArcDatabase = m_strDatabase.Insert(3, "ARC");
            R_acc raccAuditArchive = new R_acc(m_strServer, strArcDatabase, ref m_ERR);

            int iRetVal = -1;
            string strSQL;

            strSQL = string.Format("INSERT INTO {0}(acc_rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, mod_date, mod_user, mod_prg, oereqno, mri, mod_host ) " +
                                            "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}',   '{6}',     '{7}',      '{8}',     '{9}',  '{10}', '{11}',     '{12}',           '{13}',          '{14}',  '{15}',    '{16}',  '{17}',  '{18}', '{19}'   )",
                                                    propTable.ToString(), //0
                                                    m_strRowguid,
                                                    m_strDeleted == "F" ? 0 : 1,
                                                    m_strAccount,
                                                    m_strPatName,
                                                    m_strCliMnem,
                                                    m_strFinCode,
                                                    m_strTransDate,
                                                    m_strCbillDate,
                                                    m_strStatus,
                                                    m_strSsn,
                                                    m_strNumComments.Equals("")?"0":m_strNumComments,
                                                    m_strMeditechAccount,
                                                    m_strOriginalFincode,
                                                    m_strModDate,
                                                    m_strModUser,
                                                    m_strModPrg,
                                                    m_strOereqno,
                                                    m_strMri,
                                                    m_strModHost);//15


            iRetVal = raccAuditArchive.SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                string strSQLAuditDelete = string.Format(@"DELETE from {0} where acc_rowguid = '{1}'
                                                        AND uid = {2}", propTable, m_strRowguid, m_strUid);
                iRetVal = SQLExec(strSQLAuditDelete, out m_strErrMsg);
                if (iRetVal == -1)
                {
                    return iRetVal;
                }
            }
            return iRetVal;
        }

    
#region NOTE: Update() should not be called as we should not be updating these records.
        // <summary>
        // Should not be updating audit records.
        // 10/08/2007 David
        // </summary>
        // <returns>number of records 
        // OR 
        // -1 = ERROR - see m_strErrMsg for details</returns>
        //public int Update()
        //{

        //    string strSQL;
        //    //  string strWhere;
        //    /*
        //    strSQL = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3}",
        //        m_strTable, //* a property field in this class 
        //        strField,
        //        strNewValue,
        //        strFilter);
        //    */

        //    /*
        //     * Set the where clause for the KEY for this table from the DataSet values
        //     */
        //    string strCbillDate = "";
        //    if (m_strCbillDate.Length == 0)
        //    {
        //        strCbillDate = "null";
        //    }
        //    else
        //    {
        //        strCbillDate = string.Format("'{0}'", m_strCbillDate);
        //    }
        //    m_strWhere = string.Format("rowguid = '{0}' ",
        //                                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());

        //    strSQL = string.Format("UPDATE {0} SET deleted= '{1}', account= '{2}', pat_name= '{3}', cl_mnem= '{4}', fin_code= '{5}', trans_date= '{6}', cbill_date= {7}, status= '{8}', ssn= '{9}', num_comments= '{10}', meditech_account = '{11}', original_fincode= '{12}', mod_prg= '{13}', oereqno= '{14}', mri= '{15}' WHERE {16}",
        //                               propTable, //0
        //                                m_strDeleted == "T" ? 1 : 0,//1
        //                                            m_strAccount,
        //                                            m_strPatName,
        //                                            m_strCliMnem,
        //                                            m_strFinCode,
        //                                            m_strTransDate,
        //                                            strCbillDate, // not the member variable pay attention.
        //                                            m_strStatus,
        //                                            m_strSsn,
        //                                            m_strNumComments,
        //                                            m_strMeditechAccount,
        //                                            m_strOriginalFincode,
        //                                            OS.GetAppName(),
        //                                            m_strOereqno,
        //                                            m_strMri,
        //                                             m_strWhere); //08/11/2007 wdk made into member level variable for Error Msg inclusion

        //    return SQLExec(strSQL, out m_strErrMsg);

        //}
        #endregion NOTE:

        //==== 'special' functions just for this class below this line ======


        //==== 'special' functions just for this class below this line ======
        /// <summary>
        /// Modified 11/13/2007 wdk to get ALL records. This is the audit class so 
        /// all records should be returned every time.
        /// 
        /// Gets ACTIVE acc record for the rowguid passed in
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns>Number of records with the specified Rowguid</returns>
        public int GetRecordByRowguid(string strRowguid)
        {
            string strWhere;                      // table
            strWhere = string.Format("acc_rowguid = '{0}'",
                                          strRowguid);
            
            //return (GetActiveRecords(strWhere)); 11/13/2007 wdk
            return (GetRecords(strWhere));
        }


        /// <summary>
        /// Modified 11/13/2007 wdk to get ALL records. This is the audit class so 
        /// all records should be returned every time.
        /// 
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
            //return (GetActiveRecords(strWhere));  11/13/2007 wdk
            return (GetRecords(strWhere));
        }



        //==== 'special' functions just for this class below this line ======
        /// <summary>
        /// Modified 11/13/2007 wdk to get ALL records. This is the audit class so 
        /// all records should be returned every time.
        /// 
        /// Gets ACTIVE acc record for the rowguid passed in
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecordsByStatus(string[] strStatus)
        {
            string Statuses = "";
            foreach (string s in strStatus)
            {
                Statuses += "'" + s + "',";
            }
            Statuses.Insert(0, "(");
            Statuses += ")";
            string strWhere;                      // table
            strWhere = string.Format("status in '{0}'",
                                          Statuses);

           // return (GetActiveRecords(strWhere));11/13/2007 wdk
            return (GetRecords(strWhere));

        }



        /// <summary>
        /// Load this class's member variables and call this function to
        /// add a new record to the LIVE/TEST database's table. 
        /// 
        /// Then delete the record from the archive database
        /// 
        /// Then unarchive the audit_chk records for this record.
        /// Lastly, Add an audit_chk record for the unarchive with the current user info.
        /// 
        /// 10/01/2007 wdk
        /// </summary>
        /// <returns>number of records updated 
        /// Or 
        /// -1 = Error m_strErrMsg has detailis of error</returns>
        public int UnArchiveRecord()
        {
            if (!m_bArchive)
            {
                m_strErrMsg = "Cannot unarchive a live record.";
                return -1;
            }

            int iRetVal = -1;
            string strSQL;

            R_acc racc = new R_acc(m_strServer, m_strDatabase.Remove(3, 3), ref m_ERR);

            strSQL = string.Format("INSERT INTO {0}(acc_rowguid, deleted, account, pat_name, cl_mnem, fin_code, trans_date, cbill_date, status, ssn, num_comments, meditech_account, original_fincode, mod_date, mod_user, mod_prg, oereqno, mri, mod_host ) " +
                                        "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}',   '{6}',     '{7}',      '{8}',     '{9}',  '{10}', '{11}',     '{12}',           '{13}',          '{14}',  '{15}',    '{16}',  '{17}',  '{18}', '{19}'   )",
                                                propTable.ToString(), //0
                                                m_strRowguid,
                                                m_strDeleted == "F" ? 0 : 1,
                                                m_strAccount,
                                                m_strPatName,
                                                m_strCliMnem,
                                                m_strFinCode,
                                                m_strTransDate,
                                                m_strCbillDate,
                                                m_strStatus,
                                                m_strSsn,
                                                m_strNumComments.Equals("") ? "0" : m_strNumComments,
                                                m_strMeditechAccount,
                                                m_strOriginalFincode,
                                                m_strModDate,
                                                m_strModUser,
                                                m_strModPrg,
                                                m_strOereqno,
                                                m_strMri,
                                                m_strModHost);//15


            iRetVal = racc.SQLExec(strSQL, out m_strErrMsg); //live/test database's check table
            // we added a acc record to live/test
            // move the audit_acc records to live/test
            if (iRetVal > 0)
            {
                // must deleted the record from the table in order to create the delete in the audit table
                // put trigger on archive acc for the delete and it will be the audit record for the undelete of acc.
                string strSQLDelete = string.Format(@"DELETE from {0} where acc_rowguid = '{1}'
                                                        AND uid = {2}", propTable, m_strRowguid, m_strUid);
                iRetVal = SQLExec(strSQLDelete, out m_strErrMsg); // mlcarctest.chk
                if (iRetVal == -1)
                {

                    return iRetVal;
                }
                string strDatabase = m_strDatabase.Remove(3, 3);
                R_accAudit raccArchiveAudit = new R_accAudit(m_strServer, strDatabase, ref m_ERR);
                string strAuditWhere = string.Format("acc_rowguid = '{0}'", m_strRowguid);
                int nAuditRecs = raccArchiveAudit.GetRecords(strAuditWhere); //how many audit records to unarchive.
                if (iRetVal == -1)
                {
                    m_strErrMsg = "No records to unarchive";
                    return iRetVal;
                }
                for (int j = 0; j < nAuditRecs; j++)
                {
                    raccArchiveAudit.UnArchiveRecord();
                    raccArchiveAudit.GetNext();

                } // end of for
            }

            return iRetVal;
        }


    }
}
