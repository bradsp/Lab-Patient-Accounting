
using System;
// programmer added
using Utilities; // for RCRecordset

namespace MCL;

/// <summary>
/// 
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_chkAudit : RCRecordset
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

    //===== string variables for fields in record  =====
    #region base class members
    string m_strServer;
    string m_strDatabase;
    ///// <summary>
    ///// 
    ///// </summary>
    //public string m_strRowguid;

    ///// <summary>
    ///// 
    ///// </summary>
    //public string m_strDeleted;

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

    ///// <summary>
    ///// 
    ///// </summary>
    //public string m_strModHost;
    #endregion base class members

    /// <summary>
    /// 
    /// </summary>
    public string m_strUid;

    /// <summary>
    /// 
    /// </summary>
    public string m_strPayNo;

    /// <summary>
    /// 
    /// </summary>
    public string m_strAccount;

    /// <summary>
    /// 
    /// </summary>
    public string m_strChkDate;

    /// <summary>
    /// 
    /// </summary>
    public string m_strDateRec;

    /// <summary>
    /// 
    /// </summary>
    public string m_strChkNo;

    /// <summary>
    /// 
    /// </summary>
    public string m_strAmtPaid;

    /// <summary>
    /// 
    /// </summary>
    public string m_strWriteOff;

    /// <summary>
    /// 
    /// </summary>
    public string m_strContractual;

    /// <summary>
    /// 
    /// </summary>
    public string m_strStatus;

    /// <summary>
    /// 
    /// </summary>
    public string m_strSource;

    /// <summary>
    /// w_off_date
    /// </summary>
    public string m_strWriteOffDate;

    /// <summary>
    /// 
    /// </summary>
    public string m_strInvoice;

    /// <summary>
    /// 
    /// </summary>
    public string m_strBatch;

    /// <summary>
    /// 
    /// </summary>
    public string m_strComment;

    /// <summary>
    /// 
    /// </summary>
    public string m_strBadDebt;


    /// <summary>
    /// 
    /// </summary>
    public string m_strModDateAudit;

    //===== end of string variables for fields in record  =====

    /// <summary>
    /// Consructor - base class construtor does the work!
    /// passes table name to base class DBAccess
    /// - we can get the table name from the base class with propTable.ToString()
    /// 04/05/2007 Rick Crone
    /// </summary>
    /// <param name="strServer"></param>
    /// <param name="strDataBase"></param>
    /// <param name="errLog"></param>
    public R_chkAudit(string strServer, string strDataBase, ref ERR errLog)
        : base(strServer, strDataBase, "audit_chk", ref errLog)
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
    /// Generic GetRecords  - pass in where clause (without the WHERE key word).
    /// Can get active or deleted records!
    /// 04/05/07 Rick Crone
    /// </summary>
    /// <returns></returns>
    public int GetRecords(string strWhere)
    {
        //strWhere = string.Format("msg_status in ('NEW','REQUED') AND msg_type = 'ORDERS'  AND deleted = 0");

        Querry(strWhere);
        if (m_CurrentRecordCount > 0)
        {
            LoadMemberVariablesFromDataSet();
            m_strErrMsg = string.Format("CHK_AUDIT:{0} Record(s) read", m_CurrentRecordCount);

        }

        return (m_CurrentRecordCount);

    }

    #region NOTE should get all audit records regardless of deleted flag
    // <summary>
    // Generic GetActiveRecords  - pass in where clause (without the WHERE).
    // Gets only ACTIVE records - ie NOT flagged deleted.
    // 04/05/07 Rick Crone
    // </summary>
    // <returns></returns>
    //public int GetActiveRecords(string strWhere)
    //{
    //    //throw new System.NotImplementedException();
    //    strWhere = string.Format("deleted = 0 AND {0}",
    //                                strWhere);

    //    return (GetRecords(strWhere));
    //}
    #endregion NOTE

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

    #region NOTE cannot update audit records
    ///// <summary>
    ///// Sets the deleted flag on the current record
    ///// 04/12/2007 Rick Crone
    ///// </summary>
    ///// <returns></returns>
    //public int FlagCurrentRecordDeleted()
    //{
    //    m_strDeleted = "T";
    //    return (Update());
    //}

    ///// <summary>
    ///// Clears the deleted flag on the current record
    ///// 04/12/2007 Rick Crone
    ///// </summary>
    ///// <returns></returns>
    //public int FlagCurrentRecordNOTDeleted()
    //{
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
        m_strRowguid = ""; // have to clear this in the audit record because it has have to come from the chk table
        m_strModDate = Time.SNows();
        m_strModHost = OS.GetMachineName();
        m_strModPrg = OS.GetAppName();
        m_strModUser = OS.GetUserName();

        //--- table fields
        m_strPayNo = "";
        m_strAccount = "";
        m_strChkDate = "";
        m_strDateRec = "";
        m_strChkNo = "";
        m_strAmtPaid = "";
        m_strWriteOff = "";
        m_strContractual = "";
        m_strStatus = "";
        m_strSource = "";
        m_strWriteOffDate = "";
        m_strInvoice = "";
        m_strBatch = "0"; // 11/09/2007 wdk numeric data type with no default.
        m_strComment = "";
        m_strBadDebt = "F";
        m_strModDateAudit = DateTime.Now.ToString("G");
        m_strUid = "0";

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
            m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_rowguid"].ToString();
            m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
            {
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("G");
            }
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== end of 'standard' fields =====================
            m_strPayNo = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_no"].ToString();
            m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_date"].Equals(System.DBNull.Value))
            {
                m_strChkDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_date"]).ToString("G");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_rec"].Equals(System.DBNull.Value))
            {
                m_strDateRec = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_rec"]).ToString("G");
            }
            m_strChkNo = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_no"].ToString();
            m_strAmtPaid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["amt_paid"].ToString();
            m_strWriteOff = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["write_off"].ToString();
            m_strContractual = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["contractual"].ToString();
            m_strStatus = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["status"].ToString();
            m_strSource = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["source"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["w_off_date"].Equals(System.DBNull.Value))
            {
                m_strWriteOffDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["w_off_date"]).ToString("G");
            }
            m_strInvoice = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["invoice"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch"].Equals(System.DBNull.Value))
            {
                m_strBatch = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch"].ToString();
            }
            m_strComment = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["comment"].ToString();
            m_strBadDebt = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bad_debt"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date_audit"].Equals(System.DBNull.Value))
            {
                m_strModDateAudit = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date_audit"]).ToString("G");
            }
            m_strUid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["uid"].ToString();
            bRetVal = true;
        }

        return (bRetVal);
    }

    #region Add record is accomplished by triggers for this table.
    //        /// <summary>
    //        /// Add a NEW record to the table 
    //        /// Load this class's member variables and call this function to
    //        /// add a new record.
    //        /// 
    //        /// This record is added to the end of the dataset and is the current
    //        /// record after this call.
    //        /// 04/05/2007 Rick Crone
    //        /// </summary>
    //        /// <returns>number of records updated 
    //        /// Or 
    //        /// -1 = Error m_strErrMsg has detailis of error</returns>
    //        public int AddRecord()
    //        {
    //            //throw new System.NotImplementedException();
    //            int iRetVal = -1;
    //            string strSQL;

    //            //INSERT INTO MyTable (PriKey, Description)
    //            //   VALUES (123, 'A description of part 123.')
    //            //                                table fields   
    //            // pay_no is an identity field so it can not be inserted into.
    //            strSQL = string.Format(@"INSERT INTO {0}
    //                                                    (chk_rowguid, deleted, account, chk_date,
    //                                                    date_rec, chk_no, amt_paid, write_off, contractual,
    //                                                    status, source, w_off_date, invoice, batch, 
    //                                                    comment, bad_debt, mod_date, mod_user, mod_prg, 
    //                                                    mod_host, mod_date_audit, uid)
    //                                                                VALUES ('{1}','{2}','{3}','{4}','{5}',
    //                                                               '{6}','{7}','{8}','{9}','{10}',
    //                                                                '{11}','{12}','{13}','{14}','{15}',
    //                                                               '{16}','{17}','{18}','{19}','{20}',
    //                                                                '{21}','{22}')",
    //                                        propTable, 
    //                                                    m_strRowguid, m_strDeleted == "F" ? 0:1, m_strAccount, m_strChkDate,
    //                                                    m_strDateRec, m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual,
    //                                                    m_strStatus, m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch,
    //                                                    m_strComment, m_strBadDebt == "F" ? 0:1, m_strModDate, m_strModUser, m_strModPrg,
    //                                                    m_strModHost, m_strModDateAudit, int.Parse(m_strUid));



    //            iRetVal = SQLExec(strSQL, out m_strErrMsg);
    //            if (iRetVal > 0)
    //            {
    //                // add new record to dataset
    //                DataRow L_DataRow = m_DataSet.Tables[propTable.ToString()].NewRow();
    //                m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);

    //                // index is zero based while record count is 1 based ie the correct record count
    //                m_CurrentRecordIndex = m_CurrentRecordCount++;

    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"] = m_strRowguid;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "F" ? 0 : 1;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"] = m_strModDate;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"] = m_strModUser;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"] = m_strModPrg;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"] =m_strModHost;
    //                // ==== end of 'standard' fields =====================

    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_no"] = m_strPayNo;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_date"] = m_strChkDate;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_rec"] = m_strDateRec;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_no"] = m_strChkNo;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["amt_paid"] = m_strAmtPaid;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["write_off"] = m_strWriteOff;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["contractual"] = m_strContractual;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["status"] = m_strStatus;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["source"] = m_strSource;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["w_off_date"] = m_strWriteOffDate;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["invoice"] = m_strInvoice;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch"] = m_strBatch;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["comment"] = m_strComment;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bad_debt"] = m_strBadDebt == "F" ? 0 : 1;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date_audit"] = m_strModDateAudit;
    //                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["uid"] = m_strUid;


    //            }
    //            return iRetVal;
    //        }
    #endregion

    #region Should not be updateing audit records
    // <summary>
    // Should not be updating audit records
    // 10/08/2007 David
    // </summary>
    // <returns>number of records 
    // OR 
    // -1 = ERROR - see m_strErrMsg for details</returns>
    //        public int Update()
    //        {
    //            int nRetVal = -1;
    //            string strSQL;
    //            string strWhere;
    //            /*
    //            strSQL = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3}",
    //                m_strTable, //* a property field in this class 
    //                strField,
    //                strNewValue,
    //                strFilter);
    //            */

    //            /*
    //             * Set the where clause for the KEY for this table from the DataSet values
    //             */
    //            strWhere = string.Format("rowguid = '{0}'",
    //                                        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString());

    //            //pay_no is an identity column so it cannot be updated.
    //            strSQL = string.Format(@" UPDATE {0} SET deleted = '{1}',  account = '{2}', chk_date = '{3}',
    //                                                    date_rec = '{4}', chk_no = '{5}', amt_paid = '{6}', write_off = '{7}', contractual = '{8}',
    //                                                    status = '{9}', source = '{10}', w_off_date = '{11}', invoice = '{12}', batch = '{13}', 
    //                                                    comment = '{14}', bad_debt = '{15}', mod_date = '{16}', mod_user = '{17}', mod_prg = '{18}', 
    //                                                    mod_host = '{19}', mod_date_audit = '{20}', uid = '{21}' WHERE {22}",
    //                            propTable.ToString(), m_strDeleted == "T" ? 1 : 0, m_strAccount, m_strChkDate,
    //                                        m_strDateRec, m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual,
    //                                        m_strStatus, m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch,
    //                                        m_strComment, m_strBadDebt == "F" ? 0 : 1, m_strModDate, m_strModUser, m_strModPrg,
    //                                        m_strModHost, DateTime.Now.ToString("G"), int.Parse(m_strUid),strWhere);

    //             nRetVal = SQLExec(strSQL, out m_strErrMsg);
    //             return nRetVal;

    //        }
    #endregion

    //==== 'special' functions just for this class below this line ======
    /// <summary>
    /// Gets record by rowguid
    /// 
    /// 09/28/2007 wdk
    /// </summary>
    /// <returns></returns>
    public int GetRecord(string strRowguid)
    {
        //throw new System.NotImplementedException();
        string strWhere;                      // table
        strWhere = string.Format("chk_rowguid  = '{0}'",
                                     strRowguid);

        return (GetRecords(strWhere));

    }

    /// <summary>
    /// Gets ALL chk records for the account passed in
    /// 
    /// 09/26/2007 David
    /// </summary>
    /// <returns></returns>
    public int GetRecordsByAccount(string strAccount)
    {
        string strWhere;
        strWhere = string.Format("account = '{0}'",
                                      strAccount);
        return (GetRecords(strWhere));

    }

    /// <summary>
    /// Add a NEW record to the archive database's table 
    /// Load this class's member variables and call this function to
    /// add a new record.
    /// 
    /// 09/26/2007 wdk
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
        R_chkAudit rchkAuditArchive = new R_chkAudit(m_strServer, strArcDatabase, ref m_ERR);

        //throw new System.NotImplementedException();
        int iRetVal = -1;
        string strSQL;

        // pay_no is an identity field so it can not be inserted into.
        strSQL = string.Format(@"INSERT INTO {0}
                                                    (chk_rowguid, deleted, pay_no,account, chk_date,
                                                    date_rec, chk_no, amt_paid, write_off, contractual,
                                                    status, source, w_off_date, invoice, batch, 
                                                    comment, bad_debt, mod_date, mod_user, mod_prg, 
                                                    mod_host, mod_date_audit
                                                    ) VALUES ('{1}','{2}','{3}','{4}','{5}',
                                                               '{6}','{7}','{8}','{9}','{10}',
                                                                '{11}','{12}','{13}','{14}','{15}',
                                                               '{16}','{17}','{18}','{19}','{20}',
                                                                '{21}', '{22}')",
                                    propTable,
                                    m_strRowguid, m_strDeleted == "F" ? 0 : 1, m_strPayNo, m_strAccount, m_strChkDate,
                                                m_strDateRec, m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual,
                                                m_strStatus, m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch.Equals("") ? "0" : m_strBatch,
                                                m_strComment, m_strBadDebt == "F" ? 0 : 1, m_strModDate, m_strModUser, m_strModPrg,
                                                m_strModHost, m_strModDateAudit);


        iRetVal = rchkAuditArchive.SQLExec(strSQL, out m_strErrMsg);
        // we added a audit_chk record to archive 
        // so delete the record from the current database
        if (iRetVal > 0)
        {
            // must deleted the record from the audit table.
            string strSQLAuditDelete = string.Format(@"DELETE FROM {0} WHERE chk_rowguid = '{1}' AND
                                                                                    pay_no = '{2}' AND
                                                                                         uid = '{3}'",
                                                        propTable, m_strRowguid,
                                                                            m_strPayNo,
                                                                                m_strUid);
            iRetVal = SQLExec(strSQLAuditDelete, out m_strErrMsg);
            if (iRetVal == -1)
            {
                return iRetVal;
            }
        }
        return iRetVal;
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

        R_chk rchk = new R_chk(m_strServer, m_strDatabase.Remove(3, 3), ref m_ERR);

        // pay_no is an identity field so it can not be inserted into.
        strSQL = string.Format(@"INSERT INTO {0}
                                                    (chk_rowguid, deleted, pay_no,account, chk_date,
                                                    date_rec, chk_no, amt_paid, write_off, contractual,
                                                    status, source, w_off_date, invoice, batch, 
                                                    comment, bad_debt, mod_date, mod_user, mod_prg, 
                                                    mod_host, mod_date_audit
                                                    ) VALUES ('{1}','{2}','{3}','{4}','{5}',
                                                               '{6}','{7}','{8}','{9}','{10}',
                                                                '{11}','{12}','{13}','{14}','{15}',
                                                               '{16}','{17}','{18}','{19}','{20}',
                                                                '{21}', '{22}')",
                                   propTable,
                                   m_strRowguid, m_strDeleted == "F" ? 0 : 1, m_strPayNo, m_strAccount, m_strChkDate,
                                               m_strDateRec, m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual,
                                               m_strStatus, m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch.Equals("") ? "0" : m_strBatch,
                                               m_strComment, m_strBadDebt == "F" ? 0 : 1, m_strModDate, m_strModUser, m_strModPrg,
                                               m_strModHost, m_strModDateAudit);

        iRetVal = rchk.SQLExec(strSQL, out m_strErrMsg); //live/test database's check table
        // we added a chk record to live/test
        // move the audit_chk records to live/test
        if (iRetVal > 0)
        {
            // must deleted the record from the table in order to create the delete in the audit table
            // put trigger on archive chk for the delete and it will be the audit record for the undelete of chk.
            string strSQLDelete = string.Format("DELETE FROM {0} WHERE (account = '{1}') AND (pay_no = {2}) AND (uid = {3})",
                                           propTable, m_strAccount, m_strPayNo, m_strUid);
            iRetVal = SQLExec(strSQLDelete, out m_strErrMsg); // mlcarctest.chk
            if (iRetVal == -1)
            {

                return iRetVal;
            }
            string strDatabase = m_strDatabase.Remove(3, 3);
            R_chkAudit rchkArchiveAudit = new R_chkAudit(m_strServer, strDatabase, ref m_ERR);
            string strAuditWhere = string.Format("chk_rowguid = '{0}'", m_strRowguid);
            int nAuditRecs = rchkArchiveAudit.GetRecords(strAuditWhere); //how many audit records to unarchive.
            if (iRetVal == -1)
            {
                m_strErrMsg = "No records to unarchive";
                return iRetVal;
            }
            for (int j = 0; j < nAuditRecs; j++)
            {
                rchkArchiveAudit.UnArchiveRecord();
                rchkArchiveAudit.GetNext();

            } // end of for
        }

        return iRetVal;
    }


} // don't go below this line
