/*
 * R_Eample 04/05/2007 Rick Crone
 * 
 * Use this example to start a new class dervied from RCRecordset!
 * 
 * This example icd9desc table of the MCL database. 
 * 
 * The caller should use the m_str field variables to read or update
 * the values in the table.
 * 
 * calling example:
 *          // INSTANIATE THE RECORD SET 
            R_icd9desc r_icd9desc = new R_icd9desc("MCL02","MCLTEST");
            if (!r_icd9desc.m_bValid)
            {
                r_icd9desc.DispErrMsg();
                return;
            }

            // READING A RECORD with 'special' function
            if (r_icd9desc.GetRecord("001.0", "2007") == -1) // 0 = no record found -1 = error
            {
                r_icd9desc.DispErrMsg();
                return;
            }

            // ADDING A NEW RECORD
            // clear first to avoid values from reading some other record
            // and to set some of the 'standard' field values appropriate 
            // for a NEW record
            r_icd9desc.ClearMemberVariables();
            // set 'field' values
            r_icd9desc.m_strIcd9_desc = "SICKLY";
            r_icd9desc.m_strAMA_year = "2007";
            r_icd9desc.m_strIcd9_num = "999.9";
            //r_icd9desc.m_strModPrg = OS.GetAppName();
            // call the AddRecord() function
            if (r_icd9desc.AddRecord() == -1)
            {
                r_icd9desc.DispErrMsg();
                return;
            }
            
            //UPDATING AN EXISTING RECORD
            // FIRST GET THE RECORD
            //- in this case the record I just wrote so this is NOT nessasary as it is the current record
            if (r_icd9desc.GetRecord("999.9", "2007") < 1)
            {
                r_icd9desc.DispErrMsg();
                return;
            }

            // change any field values
            r_icd9desc.m_strIcd9_desc = "Very sickly";
            if (r_icd9desc.Update() == - 1)
            {
                r_icd9desc.DispErrMsg();
                return;
            }

 */
using System;
using System.Collections.Generic;
using System.Text;
//--- added
using RFClassLibrary;
//using System.Data.SqlClient; // SQL 7.0
using System.Data;
using System.Transactions;
using System.Data.SqlClient; // DataRow



namespace MCL
{
    /// <summary>
    /// m_strRowguid this is the link to the data_EOB table. It has to be passed in when creating the recordset.
    /// </summary>
    public class R_eob_detail : RCRecordset
    {
        // comittable transaction
        private CommittableTransaction eobDetailComittableTransaction;
        /// <summary>
        /// 
        /// </summary>
        public CommittableTransaction propCommittableTransDetail
        {
            get { return eobDetailComittableTransaction; }
            set { eobDetailComittableTransaction = value; }
        }

        //===== string variables for fields in record  =====    
        /// <summary>
        /// Multiple EOB's exist for some accounts on the same day in the same file. To provide an 
        /// EOB for this account and claim status added this variable and m_strClaimStatus below
        /// </summary>
        public string m_strAccount;
        /// <summary>
        /// Multiple EOB's exist for some accounts on the same day in the same file. To provide an 
        /// EOB for this account and claim status added this variable and m_strAccount above
        /// </summary>
        public string m_strClaimStatus;
        /// <summary>
        /// Service Code
        /// </summary>
        public string m_strServiceCode;
        /// <summary>
        /// Rev code
        /// </summary>
        public string m_strRevCode;
        /// <summary>
        ///  number of units
        /// </summary>
        public string m_strUnits;
        /// <summary>
        /// 
        /// </summary>
        public string m_strApcNr;
        /// <summary>
        /// allowed amount
        /// </summary>
        public string m_strAllowedAmt;
        /// <summary>
        /// current status
        /// </summary>
        public string m_strStat;
        /// <summary>
        /// 
        /// </summary>
        public string m_strWght;
        /// <summary>
        /// date of service
        /// </summary>
        public string m_strDateOfService;
        /// <summary>
        /// charge amount
        /// </summary>
        public string m_strChargeAmt;
        /// <summary>
        /// amount paid
        /// </summary>
        public string m_strPaidAmt;
        /// <summary>
        /// reason type
        /// </summary>
        public string m_strReasonType;
        /// <summary>
        /// reason code
        /// </summary>
        public string m_strReasonCode;
        /// <summary>
        /// Contractual adjustment amount
        ///  06/03/2008 rgc/wdk renamed from m_strAdjAmt
        /// </summary>
        public string m_strContractualAdjAmt; 
        /// <summary>
        /// Other Adjustment amount 
        /// 06/03/2008 rgc/wdk non CO45 adjustments
        /// </summary>
        public string m_strOtherAdjAmt; 
        /// <summary>
        /// Multiple EOB's exist for some accounts on the same day in the same file. To provide an 
        /// EOB for this account and claim status added this variable and m_strClaimStatus below.
        /// Plus original files are split by HIS so additional data is required to eleminate duplicate postings
        /// </summary>
        public string m_strBillCycleDate;
        /// <summary>
        /// Multiple EOB's exist for some accounts on the same day in the same file. To provide an 
        /// EOB for this account and claim status added this variable and m_strAccount above
        /// Plus original files are split by HIS so additional data is required to eleminate duplicate postings
        /// </summary>
        public string m_strCheckNo; 

        
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
        public R_eob_detail(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "data_EOB_Detail", ref errLog)
        {
            TransactionOptions to = new TransactionOptions();
            to.IsolationLevel = System.Transactions.IsolationLevel.ReadCommitted; // 05/21/2008 rgc/wdk so other processes can read the committed transactions without locking up the table.
            eobDetailComittableTransaction = new CommittableTransaction(to); 
            if (eobDetailComittableTransaction.TransactionInformation.Status == TransactionStatus.Aborted)
            {
                eobDetailComittableTransaction.Dispose();
                eobDetailComittableTransaction = new CommittableTransaction();
            }
            propDBConnection.EnlistTransaction(eobDetailComittableTransaction);
            
        }
        /// <summary>
        /// 
        /// </summary>
        ~R_eob_detail()
        {
           // throw new System.NotImplementedException();
            //eobDetailComittableTransaction.Dispose();
            //eobDetailComittableTransaction = null;

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
            if (m_CurrentRecordIndex < (m_CurrentRecordCount-1))
            {
                m_CurrentRecordIndex++;
                return(LoadMemberVariablesFromDataSet());
            }
            m_strErrMsg = "EOF";
            return(bRetVal);

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

        // <summary>
        // Sets the deleted flag on the current record
        // 04/12/2007 Rick Crone
        // </summary>
        // <returns></returns>
        //public int FlagCurrentRecordDeleted()
        //{
        //    m_strDeleted = "T";
        //    return (Update());
        //}

        // <summary>
        // Clears the deleted flag on the current record
        // 04/12/2007 Rick Crone
        // </summary>
        // <returns></returns>
        //public int FlagCurrentRecordNOTDeleted()
        //{
        //    m_strDeleted = "F";
        //    return (Update());
        //}
        
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
            //m_strRowguid = Guid.NewGuid().ToString();
            m_strModDate = Time.SNows();
            m_strModHost = OS.GetMachineName();
            m_strModPrg = OS.GetAppName();

            //--- table fields
            m_strAccount = "";
            m_strClaimStatus = "";
            m_strServiceCode = "";
            m_strRevCode = "";
            m_strUnits = "0";
            m_strApcNr = "0";
            m_strAllowedAmt = "0.00";
            m_strStat = "";
            m_strWght = ".00";
            m_strDateOfService = "";
            m_strChargeAmt = "0.00";
            m_strPaidAmt = "0.00";
            m_strReasonType = "";
            m_strReasonCode = "";
            m_strContractualAdjAmt = "0.00";
            m_strOtherAdjAmt = "0.00";
            // additional fields to eliminate duplicates.
            m_strBillCycleDate = "";
            m_strCheckNo = "";
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
                //if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                //{
                //    m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
                //}              
                //m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                //m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
                //m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                // ==== end of 'standard' fields =====================

                m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["Account"].ToString();
                m_strServiceCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_status"].ToString();
                m_strServiceCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ServiceCode"].ToString();
                m_strRevCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rev_code"].ToString();
                m_strUnits = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["units"].ToString();
                m_strApcNr = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["apc_nr"].ToString(); 
               
                m_strAllowedAmt = 
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["allowed_amt"]); 
                m_strStat = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["stat"].ToString(); 
                m_strWght = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["wght"].ToString(); 
                
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_of_service"].Equals(System.DBNull.Value))
                {
                    m_strDateOfService = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_of_service"]).ToString("d");
                }
                m_strChargeAmt = 
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charge_amt"]); 
                m_strPaidAmt = 
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["paid_amt"]); 
                m_strReasonType = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["reason_type"].ToString(); 
                m_strReasonCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["reason_code"].ToString(); 
                m_strContractualAdjAmt = 
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["adj_amt"]);
                //06/03/2008 rgc/wdk added Other adj amt
                m_strOtherAdjAmt =
                   string.Format("{0:F2}",
                   m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["other_adj_amt"]);
                // end of 06/03/2008 

                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bill_cycle_date"].Equals(System.DBNull.Value))
                {
                    m_strBillCycleDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bill_cycle_date"]).ToString("d");
                }
                m_strCheckNo = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["check_no"].ToString(); 
                bRetVal = true;
            }
            return (bRetVal);
        }



        /// <summary>
        /// Add a NEW record to the table 
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
        public int AddRecord()
        {
            //throw new System.NotImplementedException();
            int iRetVal = -1;
            string strSQL;

            //INSERT INTO MyTable (PriKey, Description)
            //   VALUES (123, 'A description of part 123.')
            //                                table fields   
            strSQL = string.Format("INSERT INTO {0}"+
                "(rowguid, deleted, account, claim_status,"+
                "ServiceCode, rev_code, units, apc_nr, "+
                 "allowed_amt, stat, wght, date_of_service, charge_amt, "+
                 "paid_amt, reason_type, reason_code, adj_amt, other_adj_amt, bill_cycle_date, check_no) VALUES "+
                  "('{1}','{2}','{3}','{4}',"+
                  "'{5}','{6}', '{7}','{8}', '{9}', "+
                  " '{10}','{11}','{12}','{13}','{14}',"+
                  "'{15}', '{16}', '{17}', '{18}', '{19}', '{20}')",
                                        propTable.ToString(), //0
                  m_strRowguid, 0, m_strAccount, m_strClaimStatus, //1, 2, 3, 4
                  m_strServiceCode, m_strRevCode, m_strUnits, m_strApcNr, 
                  m_strAllowedAmt, m_strStat, m_strWght, m_strDateOfService, m_strChargeAmt, 
                  m_strPaidAmt, m_strReasonType, m_strReasonCode, m_strContractualAdjAmt, m_strOtherAdjAmt,
                  m_strBillCycleDate, m_strCheckNo);
                                         

            iRetVal = SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                // add new record to dataset
                DataRow L_DataRow = m_DataSet.Tables[propTable.ToString()].NewRow();
                m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);
                
                // index is zero based while record count is 1 based ie the correct record count
                m_CurrentRecordIndex = m_CurrentRecordCount++;

                // if unarchiving a record flagged to deleted have to convert back to the live system as deleted.
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "F" ? 0 : 1;  // new record so flag it NOT deleted
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ServiceCode"] = m_strServiceCode ;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rev_code"] = m_strRevCode;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["units"] = m_strUnits;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["apc_nr"] = m_strApcNr;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["allowed_amt"] = m_strAllowedAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["stat"] = m_strStat;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["wght"] = m_strWght;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_of_service"] = m_strDateOfService;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charge_amt"] = m_strChargeAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["paid_amt"] = m_strPaidAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["reason_type"] = m_strReasonType;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["reason_code"] = m_strReasonCode;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["adj_amt"] = m_strContractualAdjAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["other_adj_amt"] = m_strOtherAdjAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bill_cycle_date"] = m_strBillCycleDate;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["check_no"] = m_strCheckNo;            
            }
            return iRetVal;
        }


        /// <summary>
        /// Add a NEW record to the table 
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
        public int AddCommittableRecord()
        {
            int iRetVal = -1;
            string strSQL;

           if (eobDetailComittableTransaction.Equals(null))
            {
                m_strErrMsg = "Comittable Transaction was null";
                return -1;
            }
           
           SqlCommand eobDetailCommand = new SqlCommand();

           if (propDBConnection.State.Equals(ConnectionState.Closed))
           {
               propDBConnection.Open();        
           }
            
           if (eobDetailComittableTransaction.TransactionInformation.Status != TransactionStatus.Active)
           {
               propDBConnection.EnlistTransaction(eobDetailComittableTransaction);
           }
           eobDetailCommand.Connection = propDBConnection;
   
            //INSERT INTO MyTable (PriKey, Description)
            //   VALUES (123, 'A description of part 123.')
            //                                table fields   
            strSQL = string.Format("INSERT INTO {0}" +
                "(rowguid, deleted, account, claim_status," +
                "ServiceCode, rev_code, units, apc_nr, " +
                 "allowed_amt, stat, wght, date_of_service, charge_amt, " +
                 "paid_amt, reason_type, reason_code, adj_amt, other_adj_amt, bill_cycle_date, check_no) VALUES " +
                  "('{1}','{2}','{3}','{4}'," +
                  "'{5}','{6}', '{7}','{8}', '{9}', " +
                  " '{10}','{11}','{12}','{13}','{14}'," +
                  "'{15}', '{16}', '{17}', '{18}', '{19}', '{20}')",
                                        propTable.ToString(), //0
                  m_strRowguid, 0, m_strAccount, m_strClaimStatus, //1, 2, 3, 4
                  m_strServiceCode, m_strRevCode, m_strUnits, m_strApcNr,
                  m_strAllowedAmt, m_strStat, m_strWght, m_strDateOfService, m_strChargeAmt,
                  m_strPaidAmt, m_strReasonType, m_strReasonCode, m_strContractualAdjAmt, m_strOtherAdjAmt,
                  m_strBillCycleDate, m_strCheckNo);

            eobDetailCommand.CommandText = strSQL;
            iRetVal = eobDetailCommand.ExecuteNonQuery();
            return iRetVal;
        }

    }
}
