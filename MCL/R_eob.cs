/*
 * R_eob 04/07/2008 wdk
 * 
 * Representation of the data_EOB table 
 * calling example:
 *          // INSTANIATE THE RECORD SET 
            R_eob r_eob = new R_eob("MCLBILL","MCLTEST");
            if (!r_eob.m_bValid)
            {
                r_eob.DispErrMsg();
                return;
            }

            // READING A RECORD with 'special' function
            if (r_eob.GetRecord("CXXXXXXX") == -1) // 0 = no record found -1 = error
            {
                r_eob.DispErrMsg();
                return;
            }

            // ADDING A NEW RECORD
            // clear first to avoid values from reading some other record
            // and to set some of the 'standard' field values appropriate 
            // for a NEW record
            r_eob.ClearMemberVariables();
            // set 'field' values
            r_eob.m_strRowguid = new Guid();
            r_eob.m_strAccount  = "CXXXXXXX";
            r_eob.m_strEobDetailRowguid  = new Guid();
            r_eob.m_strEftFile = strFileName;
            r_eob.m_strEftNumber = strEFTNo;
            r_eob.m_strEftDate = strEFTDate;
            ...
            // call the AddRecord() function
            if (r_eob.AddRecord() == -1)
            {
                r_eob.DispErrMsg();
                return;
            }
            
            //UPDATING AN EXISTING RECORD
            // FIRST GET THE RECORD
            //- in this case the record I just wrote so this is NOT nessasary as it is the current record
            if (r_eob.GetRecord("CXXXXXXX") < 1)
            {
                r_eob.DispErrMsg();
                return;
            }

            // change any field values
            r_eob.m_EftDate = strNewDate;
            if (r_eob.Update() == - 1)
            {
                r_eob.DispErrMsg();
                return;
            }

 */
using System;
using System.Collections.Generic;
using System.Text;
//--- added
using Utilities;
//using System.Data.SqlClient; // SQL 7.0
using System.Data; // DataRow
using System.Drawing.Printing;
using System.Drawing; // for print document
using System.Transactions;
using System.Data.SqlClient; // for comittable transaction added 05/19/2008 wdk

namespace MCL
{
    /// <summary>
    /// EOB instance based on RCRecordset
    /// </summary>
    public class R_eob : RCRecordset
    {
        //===== string variables for fields in record  =====


        /// <summary>
        /// the Meditech account for this EOB
        /// </summary>
        public string m_strAccount;
        /// <summary>
        /// Subscriber Id
        /// </summary>
        public string m_strSubscriberID;
        /// <summary>
        /// Subscriber Name
        /// </summary>
        public string m_strSubscriberName;
        /// <summary>
        /// Date of service
        /// </summary>
        public string m_strDateOfService;
        /// <summary>
        /// ICN number
        /// </summary>
        public string m_strICN; // this number is part of the eob but we don't use
        /// <summary>
        /// Patients status
        /// </summary>
        public string m_strPatStat; // this number is part of the ebo but we don't use it and it's '0'


        /// <summary>
        /// 
        /// </summary>
        public string m_strClaimStatus;
        /// <summary>
        /// 
        /// </summary>
        public string m_strClaimType; // 141 for medicare
        /// <summary>
        /// 
        /// </summary>
        public string m_strChargesReported;
        /// <summary>
        /// 
        /// </summary>
        public string m_strChargesNoncovered;
        /// <summary>
        /// 
        /// </summary>
        public string m_strChargesDenied;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatLibCoinsurance;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPatLibNoncovered;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataReimbRate; // percentage
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataMSPPrimPay;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataHcpcsAmt;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataContAdjAmt;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataPatRefund;
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataPerDiemRate; // percentage
        /// <summary>
        /// 
        /// </summary>
        public string m_strPayDataNetReimbAmt;
        /// <summary>
        /// 
        /// </summary>
        public string m_strClaimForwardedTo;
        /// <summary>
        /// 
        /// </summary>
        public string m_strClaimForwardedId;

        /// <summary>
        /// 
        /// </summary>
        public string m_strEftFile;

        /// <summary>
        /// 
        /// </summary>
        public string m_strEftNumber;


        /// <summary>
        /// 
        /// </summary>
        public string m_strEftDate;

        /// <summary>
        /// 
        /// </summary>
        public string m_strEobPrintDate;

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
        public R_eob(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "data_EOB", ref errLog)
        {




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
            m_strRowguid = Guid.NewGuid().ToString();
            m_strModDate = Time.SNows();
            m_strModHost = OS.GetMachineName();
            m_strModPrg = OS.GetAppName();

            //--- table fields
            m_strAccount = "";
            m_strSubscriberID = "";
            m_strSubscriberName = "";
            m_strClaimStatus = "";
            m_strDateOfService = "NULL";
            m_strClaimType = ""; // 141 for medicare
            m_strChargesReported = "0.00";
            m_strChargesNoncovered = "0.00";
            m_strChargesDenied = "0.00";
            m_strPatLibCoinsurance = "0.00";
            m_strPatLibNoncovered = "0.00";
            m_strPayDataReimbRate = ".65"; // percentage default can be .45 at times
            m_strPayDataMSPPrimPay = "0.00";
            m_strPayDataHcpcsAmt = "0.00";
            m_strPayDataContAdjAmt = "0.00";
            m_strPayDataPatRefund = "0.00";
            m_strPayDataPerDiemRate = ".65"; // percentage default can be .45 at times
            m_strPayDataNetReimbAmt = "0.00";
            m_strClaimForwardedTo = "";
            m_strClaimForwardedId = "";
            m_strEftFile = "";
            m_strEftNumber = "";
            m_strEftDate = "";
            m_strEobPrintDate = "";

            m_strICN = ""; // this number is part of the eob but we don't use
            m_strPatStat = ""; // this number is part of the ebo but we don't use it and it's '0'
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
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                {
                    m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
                }
                m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
                m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                // ==== end of 'standard' fields =====================

                m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"].ToString();
                m_strSubscriberID = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["subscriberID"].ToString();
                m_strSubscriberName = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["subscriberName"].ToString();
                m_strClaimStatus = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_status"].ToString();
                m_strClaimType = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_type"].ToString();
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_of_service"].Equals(System.DBNull.Value))
                {
                    m_strDateOfService = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_of_service"]).ToString("d");
                }
                m_strChargesReported =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charges_reported"]);
                m_strChargesNoncovered =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charges_noncovered"]);
                m_strChargesDenied =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charges_denied"]);
                m_strPatLibCoinsurance =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_lib_coinsurance"]);

                m_strEftFile = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_file"].ToString();
                m_strEftNumber = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_number"].ToString();
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_date"].Equals(System.DBNull.Value))
                {
                    m_strEftDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_date"]).ToString("d");
                }
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eob_print_date"].Equals(System.DBNull.Value))
                {
                    m_strEobPrintDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eob_print_date"]).ToString("d");
                }
                m_strPatLibNoncovered =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_lib_noncovered"]);
                m_strPayDataReimbRate =
                    string.Format("0{0}", m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_reimb_rate"]);
                m_strPayDataMSPPrimPay =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_msp_prim_pay"]);
                m_strPayDataHcpcsAmt =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_hcpcs_amt"]);
                m_strPayDataContAdjAmt =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_cont_adj_amt"]);
                m_strPayDataPatRefund =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_pat_refund"]);
                m_strPayDataPerDiemRate =
                    string.Format("0{0}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_per_diem_rate"].ToString());
                m_strPayDataNetReimbAmt =
                    string.Format("{0:F2}",
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_net_reimb_amt"]);
                m_strClaimForwardedTo = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_forwarded_to"].ToString();
                m_strClaimForwardedId = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_forwarded_id"].ToString();
                m_strICN = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icn"].ToString();
                m_strPatStat = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["patstat"].ToString();

                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bill_cycle_date"].Equals(System.DBNull.Value))
                {
                    m_strBillCycleDate =
                        ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bill_cycle_date"]).ToString("d");
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
            int iRetVal = -1;
            string strSQL;

            //INSERT INTO MyTable (PriKey, Description)
            //   VALUES (123, 'A description of part 123.')
            //                                table fields   

            /* minimum fields that must be inserted
                account, date_of_service, eft_file, eft_number, eft_date, eob_detail_rowguid
            */
            strSQL = string.Format("INSERT INTO {0}" +
                "(account, subscriberID, subscriberName, date_of_service, ICN," +
                    "PatStat, claim_status, claim_type, charges_reported,  charges_noncovered," +
                        "charges_denied, pat_lib_coinsurance, pat_lib_noncovered, pay_data_pat_refund, pay_data_reimb_rate," +
                        "pay_data_msp_prim_pay," +
                            "pay_data_hcpcs_amt,  pay_data_cont_adj_amt, pay_data_per_diem_rate, pay_data_net_reimb_amt, claim_forwarded_to, " +
                                "claim_forwarded_id, eft_file,  eft_number, eft_date, " +
                                    "eob_print_date, mod_date, mod_prg, mod_user, mod_host, bill_cycle_date, check_no) " +
                                            "VALUES ('{1}','{2}','{3}','{4}','{5}', " +
                                                    " '{6}','{7}','{8}','{9}','{10}', " +
                                                        " '{11}','{12}','{13}','{14}','{15}', " +
                                                            " '{16}','{17}','{18}','{19}','{20}', " +
                                                                " '{21}','{22}','{23}','{24}','{25}', " +
                                                                    " '{26}', {27},'{28}','{29}', '{30}', '{31}', '{32}')",
                                         propTable.ToString(),
                                         m_strAccount, m_strSubscriberID, m_strSubscriberName, m_strDateOfService, m_strICN, //5
                                         m_strPatStat, m_strClaimStatus, m_strClaimType, m_strChargesReported, m_strChargesNoncovered, //10
                                         m_strChargesDenied, m_strPatLibCoinsurance, m_strPatLibNoncovered, m_strPayDataPatRefund, m_strPayDataReimbRate, //15  
                                         m_strPayDataMSPPrimPay,
                                         m_strPayDataHcpcsAmt, m_strPayDataContAdjAmt, m_strPayDataPerDiemRate, m_strPayDataNetReimbAmt, m_strClaimForwardedTo, //20
                                         m_strClaimForwardedId, m_strEftFile, m_strEftNumber, m_strEftDate, //25
                                         m_strEobPrintDate.Length == 0 ? "null" : string.Format("'{0}'", m_strEobPrintDate), m_strModDate, m_strModPrg, m_strModUser, m_strModHost, m_strBillCycleDate, m_strCheckNo);

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
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_num"] = m_strIcd9_num;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_desc"] = m_strIcd9_desc;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["AMA_year"] = m_strAMA_year;

                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["subscriberID"] = m_strSubscriberID;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["subscriberName"] = m_strSubscriberName;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_status"] = m_strClaimStatus;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_type"] = m_strClaimType;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_of_service"] = m_strDateOfService;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charges_reported"] = m_strChargesReported;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charges_noncovered"] = m_strChargesNoncovered;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["charges_denied"] = m_strChargesDenied;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_lib_coinsurance"] = m_strPatLibCoinsurance;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_file"] = m_strEftFile;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_number"] = m_strEftNumber;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_date"] = m_strEftDate;
                if (m_strEobPrintDate.Length > 7)
                {
                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eob_print_date"] = m_strEobPrintDate;
                }
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_lib_noncovered"] = m_strPatLibNoncovered;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_reimb_rate"] = m_strPayDataReimbRate;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_msp_prim_pay"] = m_strPayDataMSPPrimPay;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_hcpcs_amt"] = m_strPayDataHcpcsAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_cont_adj_amt"] = m_strPayDataContAdjAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_pat_refund"] = m_strPayDataPatRefund;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_per_diem_rate"] = m_strPayDataPerDiemRate;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_data_net_reimb_amt"] = m_strPayDataNetReimbAmt;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_forwarded_to"] = m_strClaimForwardedTo;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claim_forwarded_id"] = m_strClaimForwardedId;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icn"] = m_strICN;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["patstat"] = m_strPatStat;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bill_cycle_date"] = m_strBillCycleDate;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["check_no"] = m_strCheckNo;

            }

            return iRetVal;
        }

        // <summary>
        // 04/30/2008 rgc/wdk if reinstated (should not be) then have to fix the update fields.
        // Updates the 'current' record.
        // Rick Crone 04/12/2006
        // </summary>
        // <returns>number of records 
        // OR 
        // -1 = ERROR - see m_strErrMsg for details</returns>
        //public int Update()
        //{
        //    string strSQL;
        //    string strWhere;
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
        //    strWhere = string.Format("rowguid  = '{0}' ",
        //                                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString());

        //    strSQL = string.Format("UPDATE {0} SET " +
        //        "(deleted = '{1}', account = '{2}', subscriberID = '{3}', subscriberName = '{4}', date_of_service = '{5}', ICN = '{6}'," +
        //            "PatStat = '{7}', claim_status = '{8}', claim_type = '{9}', charges_reported = '{10}',  charges_noncovered = '{11}',"+
        //                "charges_denied = '{12}', pat_lib_coinsurance = '{13}', pat_lib_noncovered = '{14}', pay_data_reimb_rate = '{15}',"+
        //                    "pay_data_hcpcs_amt = '{16}',  pay_data_cont_adj_amt = '{17}', pay_data_per_diem_rate = '{18}', pay_data_net_reimb_amt = '{19}', "+
        //                        "claim_forwarded_to = '{20}', "+
        //                        "claim_forwarded_id = '{21}', eob_detail_rowguid = '{22}', eft_file = '{23}',  eft_number = '{24}', eft_date = '{25}', "+
        //                            "eob_print_date = '{26}', mod_date = '{27}', mod_prg = '{28}', mod_user = '{29}', mod_host = '{30}', pay_data_pat_refund = '(31)') "+
        //                                    "VALUES ('{1}','{2}','{3}','{4}','{5}' "+
        //                                            "'{6}','{7}','{8}','{9}','{10}' "+
        //                                                "'{11}','{12}','{13}','{14}','{15}' "+
        //                                                    "'{16}','{17}','{18}','{19}','{20}'"+
        //                                                        "'{21}','{22}','{23}','{24}','{25}'"+
        //                                                            "'{26}','{27}','{28}','{29}','{30}', '{31}'",
        //                                 propTable.ToString(), m_strDeleted,
        //                                 m_strAccount, m_strSubscriberID, m_strSubscriberName, m_strDateOfService, m_strICN, //5
        //                                 m_strPatStat, m_strClaimStatus, m_strClaimType, m_strChargesReported, m_strChargesNoncovered, //10
        //                                 m_strChargesDenied, m_strPatLibCoinsurance, m_strPatLibNoncovered, m_strPayDataReimbRate, //14  
        //                                 m_strPayDataHcpcsAmt, m_strPayDataContAdjAmt, m_strPayDataPerDiemRate, m_strPayDataNetReimbAmt, m_strClaimForwardedTo, //19
        //                                 m_strClaimForwardedId, m_strEobDetailRowguid, m_strEftFile, m_strEftNumber, m_strEftDate, //24
        //                                 m_strEobPrintDate, m_strModDate, m_strModPrg, m_strModUser, m_strModHost, m_strPayDataPatRefund);

        //    return SQLExec(strSQL, out m_strErrMsg);    
        //}


        //==== 'special' functions just for this class below this line ======
        // <summary>
        // Gets ACTIVE ICD9 record for the icd9 and ama year passed in.
        // 04/05/07 Rick Crone
        // </summary>
        // <returns></returns>
        //public int GetRecord(string strIcd9Num, string strAMAYear)
        //{
        //    //throw new System.NotImplementedException();
        //    string strWhere;                      // table
        //    strWhere = string.Format("icd9_num = '{0}' AND AMA_year = '{1}'",
        //                                 strIcd9Num,
        //                                  strAMAYear);

        //    return (GetActiveRecords(strWhere));

        //}




    } // don't type below this line
}
