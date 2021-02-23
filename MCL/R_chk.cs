using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using RFClassLibrary; // for RCRecordset
using System.Data; // DataRow
using System.Net.Mime;
using System.Data.Common;
using System.Data.SqlClient; // Application.ExecutablePath


namespace MCL
{
    /// <summary>
    /// The Mod_date in this table is used for payment reports. You cannot change the mod_date once it has a value. 
    /// Use the mod_date_audit for auditing purposes.
    /// </summary>
    public class R_chk : RCRecordset
    {
        private string m_strWhere; // wdk 20110114 added to maintain loop when updating records

        private decimal m_dTotalPaid;
        /// <summary>
        /// returns the total paid (amt_paid + contractual + write off) for the current record
        /// if the calculation fails it will return decimal.MinValue
        /// </summary>
        public decimal propTotalPaid
        {
            get { return m_dTotalPaid; }
            set
            {
                try
                {
                    m_dTotalPaid = decimal.Parse(m_strAmtPaid) + decimal.Parse(m_strContractual) + decimal.Parse(m_strWriteOff);
                }
                catch
                {
                    m_dTotalPaid = decimal.MinValue;
                }
            }
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
        /// <summary>
        /// wdk 20100712 added for tracking percentages of payments to charges
        /// </summary>
        public string m_strInsCode;
        /// <summary>
        ///  wdk 20100621 added for tracking percentage of payments to charges
        /// </summary>
        public string m_strFinCode;
        //05/06/08 used as part of the electronic check posting filter
        /// <summary>
        /// 05/07/2008 rgc/wdk this "date" is really a string in the from of YYMMDD from the ISA09 field of the file being parsed.
        /// </summary>
        public string m_strEftDate;
        /// <summary>
        /// 
        /// </summary>
        public string m_strEftNumber;
        // end of 05/06/08 used as part of the electronic check posting filter

        private bool m_bArchive;
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, string> m_dicFields;
        /// <summary>
        /// True if the database passed in to the classes constructor contains "ARC"
        /// False if it does not
        /// </summary>
        public bool propIsArchiveDB
        {
            get { return m_bArchive; }
        }

    //    ERR m_ErrLog;
        //===== string variables for fields in record  =====
        #region base class members
        string m_strServer;
        string m_strArcDatabase;
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

        // 04/03/2008 wdk/rgc added new fields to the table for 835 remittance posting. 
        /// <summary>
        /// Code from the SVC tag in the file. 
        /// 04/03/2008 wdk/rgc added new fields to the table for 835 remittance posting. 
        /// </summary>
        public string m_strCpt4Code; 
        /// <summary>
        /// File name the posting is from as of 04/03/2008 Only TLC and Medicare is doing this.
        /// 04/03/2008 wdk/rgc added new fields to the table for 835 remittance posting. 
        /// </summary>
        public string m_strPostFile; 
        /// <summary>
        /// Not used but should be the rowguid based on the HCPC code in the payment from the charge table
        /// 04/03/2008 wdk/rgc added new fields to the table for 835 remittance posting. 
        /// </summary>
        public string m_strChrgRowguid; 
        /// <summary>
        /// This is loaded from the dict_write_off_codes table.
        /// 04/03/2008 wdk/rgc added new fields to the table for 835 remittance posting. 
        /// </summary>
        public string m_strWriteOffCode; 
        // end of 04/03/2008 wdk/rgc added new fields to the table for 835 remittance posting. 
   
        /// <summary>
        /// This should not be set in this class as it is an identity field in the table.
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
        /// 
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

        //// 12/31/2007 wdk added new fields
        //public string m_strCpt4Code;
        //public string m_strPostFile;
        //public string m_strChrgRowguid;

        //===== end of string variables for fields in record  =====

        #region obsolete constructor
        ///// <summary>
        ///// OBSOLETE 05/29/2008 wdk/rgc use the constructor 
        ///// public R_chk(string strServer, string strDataBase, ref ERR errLog)
        ///// Consructor - base class construtor does the work!
        ///// passes table name to base class DBAccess
        ///// - we can get the table name from the base class with propTable.ToString()
        ///// 04/05/2007 Rick Crone
        ///// To use the record set to 
        /////     archive set the database to the MCLLIVE database 
        /////     <code>
        /////     <example>
        /////     R_chk racc = new R_chk("MCLBILL", "MCLLIVE");
        /////     if (racc.ArchiveRecord() < 1)
        /////     {
        /////         ... code here...
        /////     }
        /////     </example>
        /////     </code>
        /////    and to 
        /////     unarchive data set the MCLARCLIVE archive database
        /////     <code>
        /////     <example>
        /////     R_chk racc = new R_chk("MCLBILL", "MCLARCLIVE");
        /////     if (racc.UnArchiveRecord() < 1)
        /////     {
        /////         ... code here...
        /////     }
        /////     </example>
        /////     </code>
        ///// 
        ///// <remarks>Because the chk table has a unique identity field called pay_no, Unarchiving adds a new record
        ///// to the table with a NEW pay_no. Searching by pay_no after an unarchive will produce incorrect results
        ///// if looking for the old pay_no. 10/05/2007 wdk </remarks>
        ///// 
        ///// Assumptions for the use of this class.
        ///// 1. If strDataBase DOES NOT include the substring "ARC"
        /////     A. You can archive USING ArchiveRecord() which checks m_bArchive.
        ///// 
        ///// 2. If strDataBase DOES include the substring "ARC" 
        /////     A. You can unarchive USING UnArchiveRecord() which checks m_bArchive.
        /////     B. You can write the record(s) to disk USING WriteArchiveRecordToDisk() which checks m_bArchive.
        ///// </summary>
        ///// <param name="strServer"></param>
        ///// <param name="strDataBase"></param>
        //[Obsolete("Call the constructor. \r\n\tpublic R_chk(string strServer, string strDataBase, ref ERR errLog)")]
        //public R_chk(string strServer, string strDataBase)
        //    : base(strServer, strDataBase, "chk")
        //{
        //    m_strServer = strServer;
        //    m_strDatabase = strDataBase;
        //    // set our archive flag
        //    m_bArchive = false; // can only archive

        //    if (strDataBase.ToUpper().IndexOf("ARC") > -1)
        //    {
        //        m_bArchive = true;  // can only unarchive
        //    }
        //    else
        //    {
        //        m_strArcDatabase = strDataBase.Insert(3, "ARC");
        //    }
            
        //    string[] strArgs = { m_strServer, m_strDatabase , m_strArcDatabase, string.Format("Archiving = {0}",m_bArchive.ToString())};
        //    m_ErrLog = new ERR(strArgs);
        //    RFCObject rfc = new RFCObject();
        //    m_ErrLog.m_Logfile.WriteLogFile(string.Format("Server: {0}{1}DataBase: {2}{3}ArchiveDatabase: {4}{5}{6}", strArgs[0], Environment.NewLine, strArgs[1], Environment.NewLine, strArgs[2], Environment.NewLine, strArgs[3]));

        //    CreateFieldDictionary(); //12/31/2007 wdk
        //}
        #endregion obsolete constructor

        /// <summary>
        /// Consructor - base class construtor does the work!
        /// passes table name to base class DBAccess
        /// - we can get the table name from the base class with propTable.ToString()
        /// 05/29/2008 wdk/rgc
        ///    Added new constructor to handle passing of the error log instead of creating
        ///    a new one when the class is constructed. Depreciate the old constructor. 
        ///    (when we find out how, currently has an obsolete attribute)
        /// 04/05/2007 Rick Crone
        /// To use the record set to 
        ///     archive set the database to the MCLLIVE database 
        ///     <code>
        ///     <example>
        ///     R_chk racc = new R_chk("MCLBILL", "MCLLIVE");
        ///     <![CDATA[if (racc.ArchiveRecord() < 1)]]>
        ///     {
        ///         ... code here...
        ///     }
        ///     </example>
        ///     </code>
        ///    and to 
        ///     unarchive data set the MCLARCLIVE archive database
        ///     <code>
        ///     <example>
        ///     R_chk racc = new R_chk("MCLBILL", "MCLARCLIVE");
        ///     <![CDATA[if (racc.UnArchiveRecord() < 1)]]>
        ///     {
        ///         ... code here...
        ///     }
        ///     </example>
        ///     </code>
        /// 
        /// <remarks>Because the chk table has a unique identity field called pay_no, Unarchiving adds a new record
        /// to the table with a NEW pay_no. Searching by pay_no after an unarchive will produce incorrect results
        /// if looking for the old pay_no. 10/05/2007 wdk </remarks>
        /// 
        /// Assumptions for the use of this class.
        /// 1. If strDataBase DOES NOT include the substring "ARC"
        ///     A. You can archive USING ArchiveRecord() which checks m_bArchive.
        /// 
        /// 2. If strDataBase DOES include the substring "ARC" 
        ///     A. You can unarchive USING UnArchiveRecord() which checks m_bArchive.
        ///     B. You can write the record(s) to disk USING WriteArchiveRecordToDisk() which checks m_bArchive.
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDataBase"></param>
        /// <param name="errLog"></param>
        public R_chk(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "chk", ref errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDataBase;
            // set our archive flag
            m_bArchive = false; // can only archive

            if (strDataBase.ToUpper().IndexOf("ARC") > -1)
            {
                m_bArchive = true;  // can only unarchive
            }
            else
            {
                m_strArcDatabase = strDataBase.Insert(3, "ARC");
            }

            string[] strArgs = { m_strServer, m_strDatabase, m_strArcDatabase, string.Format("Archiving = {0}", m_bArchive.ToString()) };
           // m_ErrLog = errLog;
            //RFCObject rfc = new RFCObject();
            m_ERR.m_Logfile.WriteLogFile(string.Format("Server: {0}{1}DataBase: {2}{3}ArchiveDatabase: {4}{5}{6}", strArgs[0], Environment.NewLine, strArgs[1], Environment.NewLine, strArgs[2], Environment.NewLine, strArgs[3]));

            CreateFieldDictionary(); //12/31/2007 wdk
        }       

        /// <summary>
        /// Generic GetRecords  - pass in where clause (without the WHERE key word).
        /// Can get active or deleted records!
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecords(string strWhere)
        {
            strWhere += " and (not status in ('cbill'))";
            Querry(strWhere);
            m_strWhere = strWhere;
            if (m_CurrentRecordCount > 0)
            {
                LoadMemberVariablesFromDataSet();
                m_strErrMsg = string.Format("{0} Record(s) read", m_CurrentRecordCount);
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
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

        /// <summary>
        /// Sets the deleted flag on the current record
        /// 04/12/2007 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int FlagCurrentRecordDeleted()
        {
            m_strDeleted = "T";
            return (Update());
        }

        /// <summary>
        /// Clears the deleted flag on the current record
        /// 04/12/2007 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int FlagCurrentRecordNOTDeleted()
        {
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
            m_strRowguid = Guid.NewGuid().ToString();
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
            m_strBatch = "0"; // 11/09/2007 wdk because the chk table has this as a numeric data type with no default vale
            m_strComment = "";
            m_strBadDebt = "F";
            m_strModDateAudit = DateTime.Now.ToString("G");
            // 04/03/2008 wdk new fields added
            m_strCpt4Code = "";
            m_strPostFile = "";
            m_strChrgRowguid = "";
            m_strWriteOffCode = "";  
            // 05/06/08
            m_strEftDate = "";
            m_strEftNumber = "";

            // wdk 20100621 added
            m_strFinCode = "";
            // wdk 20100712 added
            m_strInsCode = ""; 
            // wdk 20110114 added
            m_strPostDate = "";
            // wdk 20111214 added
            m_dTotalPaid = decimal.MinValue;
        }

        /// <summary>
        /// Creates a public dictionary of fileds with their recordset names for ease of querying.
        /// Partially complete.
        /// </summary>
        public void CreateFieldDictionary()
        {
            m_dicFields =  new Dictionary<string,string>();
            m_dicFields.Add("m_strChkDate", "chk_date");
            m_dicFields.Add("m_strAccount", "account");
            m_dicFields.Add("m_strDateRec", "date_rec");
            m_dicFields.Add("m_strChkNo", "chk_no");
            m_dicFields.Add("m_strInvoice", "invoice");
            m_dicFields.Add("m_strBatch", "batch");
            m_dicFields.Add("m_strCpt4Code", "cpt4Code");
            m_dicFields.Add("m_strPostFile", "post_file");
            m_dicFields.Add("m_strEftDate", "eft_date");
            m_dicFields.Add("m_strEftNumber", "eft_number");
            m_dicFields.Add("m_strFinCode", "fin_code");
            m_dicFields.Add("m_strInsCode", "ins_code");
            //m_dicFields.Add("m_dTotalPaid", "total");
            //m_dicFields.Add("m_str ", "");
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
                m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
                m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                //// ==== end of 'standard' fields =====================
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
              
                //04/03/2008 wdk new fields added
                m_strCpt4Code = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cpt4Code"].ToString();
                m_strPostFile = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_file"].ToString();
                m_strChrgRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chrg_rowguid"].ToString();
                m_strWriteOffCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["write_off_code"].ToString();
                // 05/06/08
                m_strEftDate =   m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_date"].ToString();
                m_strEftNumber = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_number"].ToString();

                // wdk 20100621 added
                m_strFinCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"].ToString();
                // wdk 20100712 added
                m_strInsCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ins_code"].ToString();

                // wdk 20110114 added
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_date"].Equals(System.DBNull.Value))
                {
                    m_strPostDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_date"]).ToString("d"); // no hours minutes etc
                } 
                // rgc/wdk 20111214 added
                try
                {
                    m_dTotalPaid = decimal.Parse(m_strAmtPaid) + decimal.Parse(m_strContractual) + decimal.Parse(m_strWriteOff);
                }
                catch
                {
                    m_dTotalPaid = decimal.MinValue;
                }
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
        /// 
        /// 12/31/2007 wdk three new fields added. Archive needs to be updated for this add to work there
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
            int iRetVal = -1;
            string strSQL;

            // pay_no is an identity field so it can not be inserted into.
            strSQL = string.Format(@"INSERT INTO {0}
                    (rowguid, deleted, account, chk_date, date_rec, 
                    chk_no, amt_paid, write_off, contractual, status, 
                    source, w_off_date, invoice, batch, comment, 
                    bad_debt, mod_date, mod_user, mod_prg, mod_host, 
                    mod_date_audit, cpt4Code, post_file, chrg_rowguid, write_off_code,  
                    eft_date, eft_number, fin_code, ins_code, post_date
                    ) VALUES 
                    ('{1}','{2}','{3}',{4},{5},
                     '{6}','{7}','{8}','{9}','{10}',
                     '{11}','{12}','{13}','{14}','{15}',
                     '{16}','{17}','{18}','{19}','{20}',
                      '{21}','{22}','{23}',{24}, '{25}',
                      '{26}', '{27}', '{28}', '{29}', {30})",
                 propTable, 
                    m_strRowguid, 0, m_strAccount,
                    string.IsNullOrEmpty(m_strChkDate) ? "NULL" : string.Format("'{0}'", m_strChkDate),
                    string.IsNullOrEmpty(m_strDateRec) ? "NULL" : string.Format("'{0}'", m_strDateRec), 
                    m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual, m_strStatus, 
                    m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch.Equals("") ? "0" : m_strBatch, m_strComment, 
                    m_strBadDebt == "F" ? 0:1, m_strModDate, m_strModUser, m_strModPrg, m_strModHost, 
                    m_strModDateAudit, m_strCpt4Code, m_strPostFile,
                    string.IsNullOrEmpty(m_strChrgRowguid) ? "NULL" : string.Format("'{0}'", m_strChrgRowguid), m_strWriteOffCode,
                    m_strEftDate, m_strEftNumber, m_strFinCode, m_strInsCode, 
                    string.IsNullOrEmpty(m_strPostDate)? "NULL":string.Format("'{0}'",m_strPostDate));
                                                 
            iRetVal = SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                iRetVal = Querry(string.Format("rowguid = '{0}'", m_strRowguid));
                LoadMemberVariablesFromDataSet();
                // add new record to dataset
                //DataRow L_DataRow = m_DataSet.Tables[propTable.ToString()].NewRow();
                //m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);

                //// index is zero based while record count is 1 based ie the correct record count
                //m_CurrentRecordIndex = m_CurrentRecordCount++;

                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"] = m_strRowguid;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "F" ? 0 : 1;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"] = m_strModDate;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"] = m_strModUser;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"] = m_strModPrg;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"] = m_strModHost;
                //// ==== end of 'standard' fields =====================
                //// pay_no is an identity field so it can not be inserted into.
                //// m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pay_no"] = m_strPayNo;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_date"] = m_strChkDate;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_rec"] = m_strDateRec;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chk_no"] = m_strChkNo;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["amt_paid"] = m_strAmtPaid;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["write_off"] = m_strWriteOff;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["contractual"] = m_strContractual;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["status"] = m_strStatus;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["source"] = m_strSource;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["w_off_date"] = m_strWriteOffDate;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["invoice"] = m_strInvoice;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch"] = m_strBatch.Equals("") ? "0": m_strBatch;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["comment"] = m_strComment;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bad_debt"] = m_strBadDebt == "F" ? 0 : 1;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date_audit"] = m_strModDateAudit;
                //// 04/03/2008 wdk three new fields added
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cpt4Code"] = m_strCpt4Code;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_file"] = m_strPostFile;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chrg_rowguid"] = m_strChrgRowguid;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["write_off_code"] = m_strWriteOffCode;
                //// 05/06/08
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_date"] = m_strEftDate;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["eft_number"] = m_strEftNumber;
                //// wdk 20100621 added
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"] = m_strFinCode;
                //// wdk 2010712 added
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ins_code"] = m_strInsCode;
                //m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_date"] = m_strPostDate;

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
        /// 
        /// 12/31/2007 wdk
        /// modified for three new fields. As we don't update archive this needs no further changes.
        /// </summary>
        /// <returns>number of records 
        /// OR 
        /// -1 = ERROR - see m_strErrMsg for details</returns>
        public int Update()
        {
            if (m_bArchive)
            {
                m_strErrMsg = "Cannot update the Archive Database. Must be restored to Live then updated.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return -1;
            }
            string strSQL;
            string strWhere;
            // wdk 20110114 converted to use new methodology for auditing mod history
            //m_strModDate = Time.SNows();
            //m_strModHost = OS.GetMachineName();
            //m_strModPrg = OS.GetAppName();
            //m_strModUser = OS.GetUserName();
    
            /*
             * Set the where clause for the KEY for this table from the DataSet values
             */
            strWhere = string.Format("rowguid = '{0}'",
                                        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString());

            //pay_no is an identity column so it cannot be updated.
            strSQL = string.Format(@" UPDATE {0} SET deleted = '{1}',  account = '{2}', chk_date = '{3}',
                date_rec = '{4}', chk_no = '{5}', amt_paid = '{6}', write_off = '{7}', contractual = '{8}',
                status = '{9}', source = '{10}', w_off_date = '{11}', invoice = '{12}', batch = '{13}', 
                comment = '{14}', bad_debt = '{15}',
                 mod_date_audit = '{16}', cpt4Code = '{17}', post_file = '{18}',
                chrg_rowguid = {19}, write_off_code = '{20}', eft_date = {21}, eft_number = '{22}',
                fin_code = '{23}', ins_code = '{24}', post_date = {25}
                WHERE {26}",
            propTable.ToString(), m_strDeleted == "T" ? 1 : 0, m_strAccount, m_strChkDate,
                m_strDateRec, m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual,
                m_strStatus, m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch.Equals("") ? "0" : m_strBatch,
                m_strComment, m_strBadDebt == "F" ? 0 : 1,
                //m_strModDate, m_strModUser, m_strModPrg, m_strModHost, 
                DateTime.Now.ToString("G"), m_strCpt4Code, m_strPostFile, 
                string.IsNullOrEmpty(m_strChrgRowguid)?"NULL":string.Format("'{0}'",m_strChrgRowguid), 
                m_strWriteOffCode , 
                string.IsNullOrEmpty(m_strEftDate)?"NULL":string.Format("'{0}'",m_strEftDate), 
                m_strEftNumber, m_strFinCode, m_strInsCode,
                string.IsNullOrEmpty(m_strPostDate)? "NULL": string.Format("'{0}'",m_strPostDate),
                strWhere);

            int nRetval =  SQLExec(strSQL, out m_strErrMsg);
            if (nRetval > 0)
            {
                nRetval = Querry(m_strWhere);
                if (nRetval >= 0)
                {
                    LoadMemberVariablesFromDataSet();
                }
            }
            return nRetval;

            
        }

        //==== 'special' functions just for this class below this line ======
        /// <summary>
        /// Gets ACTIVE record.
        /// 04/05/07 Rick Crone
        /// </summary>
        /// <returns></returns>
        public int GetRecord(string strRowguid)
        {
            string strWhere;    // table
            strWhere = string.Format("rowguid  = '{0}'",
                                         strRowguid);
            m_strWhere = strWhere;
            return (GetActiveRecords(strWhere));

        }

        /// <summary>
        /// Gets ACTIVE chk records for the account passed in
        /// 
        /// 09/26/2007 David
        /// </summary>
        /// <returns></returns>
        public int GetRecordsByAccount(string strAccount)
        {
            string strWhere;
            strWhere = string.Format("account = '{0}'",
                                          strAccount);
            return (GetActiveRecords(strWhere));

        }

        /// <summary>
        /// Load this class's member variables and call this function to
        /// add a new record to the archive database's table .
        /// 
        /// Then archive the audit_chk records for this record using recordset R_chkAudit.
        /// 
        /// 09/26/2007 wdk
        /// MODIFICATIONS NEEDED
        /// 04/02/2007 once the archive tables have been updated for the six new fields this needs to be changed.
        /// </summary>
        /// <returns>number of records updated 
        /// Or 
        /// -1 = Error m_strErrMsg has detailis of error</returns>
        public int ArchiveRecord()
        {
            throw new System.NotImplementedException("04/02/2007 once the archive tables have been updated for the four new fields this needs to be changed.");
#pragma warning disable 0162
            int iRetVal = -1;
            string strSQL;

            if (m_bArchive)
            {
                m_strErrMsg = "Cannot archive from the archive database.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return iRetVal;
            }

            // create a record set for the chk table in the archive database/server
            R_chk rchkArchive = new R_chk(m_strServer, m_strArcDatabase, ref m_ERR);
 
            //format the insert command for the values in the current record
            // pay_no is an identity field so it can not be inserted into.
            strSQL = string.Format(@"INSERT INTO {0}
                                                    (rowguid, deleted, pay_no,account, chk_date,
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
                                        m_strRowguid, m_strDeleted == "F" ? 0:1, m_strPayNo, m_strAccount, m_strChkDate,
                                                    m_strDateRec, m_strChkNo, m_strAmtPaid, m_strWriteOff, m_strContractual,
                                                    m_strStatus, m_strSource, m_strWriteOffDate, m_strInvoice, m_strBatch,
                                                    m_strComment, m_strBadDebt == "F" ? 0 : 1, m_strModDate, m_strModUser, m_strModPrg,
                                                    m_strModHost, m_strModDateAudit);

           
            // attempt archive the record. If there is an error -1 will be returned and m_strErrMsg will have text
            iRetVal = rchkArchive.SQLExec(strSQL, out m_strErrMsg);
            
            // we added a record to archive 
            // move the audit_chk records to archive
            if (iRetVal > 0)
            {
                // must deleted the record from the table in order to create the delete in the audit table
                string strSQLDelete = string.Format("DELETE FROM {0} WHERE (account = '{1}') AND (pay_no = {2})",
                                               propTable, m_strAccount, m_strPayNo);
                iRetVal = SQLExec(strSQLDelete, out m_strErrMsg);
                
                if (iRetVal == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strSQLDelete);
                    m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                    return iRetVal;
                }
                // if delete is successful then attempt to archive the audit records
                R_chkAudit rchkAudit = new R_chkAudit(m_strServer, m_strDatabase, ref m_ERR); 
                string strAuditWhere = string.Format("chk_rowguid = '{0}'", m_strRowguid);
                int nAuditRecs = rchkAudit.GetRecords(strAuditWhere); //how many audit records to archive.
                if (iRetVal == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strAuditWhere);
                    m_ERR.m_Logfile.WriteLogFile(rchkAudit.propErrMsg);
                    return iRetVal;
                }
                // no error so insert the audit_chk records into the archive database
                for (int j = 0; j < nAuditRecs; j++)
                {
                    if (rchkAudit.ArchiveRecord() < 1)
                    {
                        m_ERR.m_Logfile.WriteLogFile(propErrMsg);
                    }
                    rchkAudit.GetNext();

                } // end of for
                //Do not delete the current record from the record set. the next query will do it for you.
            }
            else
            {
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
            }
            return iRetVal;
#pragma warning restore 0162
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
        /// 
        /// MODIFICATIONS NEEDED
        /// 12/31/2007 once the archive tables have been updated for the six new fields this needs to be changed.
        /// </summary>
        /// <returns>number of records updated 
        /// Or 
        /// -1 = Error m_strErrMsg has detailis of error</returns>
        public int UnArchiveRecord()
        {
            throw new System.NotImplementedException("04/02/2007 once the archive tables have been updated for the four new fields this needs to be changed.");
#pragma warning disable 0162
            if (!m_bArchive)
            {
                m_strErrMsg = "Cannot unarchive a live record.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return -1;
            }

            int iRetVal = -1;
            string strSQL;

            R_chk rchk = new R_chk(m_strServer, m_strDatabase.Remove(3, 3), ref m_ERR);
            
            // pay_no is an identity field so it can not be inserted into.
            strSQL = string.Format(@"INSERT INTO {0}
                                                    (rowguid, deleted, 
                                                    account,  chk_date,
                                                    date_rec, chk_no, 
                                                    amt_paid,  write_off, 
                                                    contractual, status, 
                                                    source,  w_off_date, 
                                                    invoice,  batch, 
                                                    comment,  bad_debt, 
                                                    mod_date,  mod_user, 
                                                    mod_prg,  mod_host, 
                                                    mod_date_audit
                                                    ) VALUES ('{1}','{2}','{3}','{4}','{5}',
                                                               '{6}','{7}','{8}','{9}','{10}',
                                                                '{11}','{12}','{13}','{14}','{15}',
                                                               '{16}','{17}','{18}','{19}','{20}',
                                                                '{21}')",
                                        propTable.ToString(),
                                                    m_strRowguid,  m_strDeleted == "F" ? 0 : 1, 
                                                    m_strAccount,  m_strChkDate,
                                                    m_strDateRec, m_strChkNo, 
                                                    m_strAmtPaid,  m_strWriteOff,
                                                    m_strContractual, m_strStatus, 
                                                    m_strSource,  m_strWriteOffDate, 
                                                    m_strInvoice,  m_strBatch,
                                                    m_strComment,  m_strBadDebt == "F" ? 0 : 1, 
                                                    m_strModDate,  m_strModUser, 
                                                    m_strModPrg, m_strModHost, 
                                                    m_strModDateAudit);

            iRetVal = rchk.SQLExec(strSQL, out m_strErrMsg); //live/test database's check table
            // we added a chk record to live/test
            // move the audit_chk records to live/test
            if (iRetVal > 0)
            {
                // must deleted the record from the table in order to create the delete in the audit table
                // put trigger on archive chk for the delete and it will be the audit record for the undelete of chk.
                string strSQLDelete = string.Format("DELETE FROM {0} WHERE (account = '{1}') AND (pay_no = {2})",
                                               propTable, m_strAccount, m_strPayNo);
                iRetVal = SQLExec(strSQLDelete, out m_strErrMsg); // mlcarctest.chk
                if (iRetVal == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strSQLDelete);
                    m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                    return iRetVal;
                }
                R_chkAudit rchkArchiveAudit = new R_chkAudit(m_strServer, m_strDatabase, ref m_ERR);  // mclarctest.audit_chk
                string strAuditWhere = string.Format("chk_rowguid = '{0}'", m_strRowguid);
                int nAuditRecs = rchkArchiveAudit.GetRecords(strAuditWhere); //how many audit records to unarchive.
                if (iRetVal == -1)
                {
                    m_ERR.m_Logfile.WriteLogFile(strAuditWhere);
                    m_ERR.m_Logfile.WriteLogFile(rchkArchiveAudit.propErrMsg);
                    return iRetVal;
                }
                for (int j = 0; j < nAuditRecs; j++)
                {
                    rchkArchiveAudit.UnArchiveRecord();
                    rchkArchiveAudit.GetNext();
                } // end of for
            }
            else
            {
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
            }

            return iRetVal;
#pragma warning restore 0162
        }

        // <summary>
        // Archives checks thru the date passed in allowing that it has to be 6 months old or older
        // This function cannot be used to unarchive chk's that has to be done individually.
        // </summary>
        // <param name="dtThru"></param>
        // <returns></returns>
        //public int ArchiveChks(DateTime dtThru)
        //{           
        //    int nRetVal = -1;
        //    if (m_bArchive)
        //    {
        //        m_strErrMsg = "Cannot archive archived records";
        //        return nRetVal;
        //    }

        //    if (dtThru > DateTime.Now.Subtract(new TimeSpan(180,0,0,0)))
        //    {
        //        m_strErrMsg = "Cannot archive records that are less than 6 months old";
        //        return nRetVal;
        //    }
        //    string strWhere = string.Format("date_rec < '{0}'", dtThru);
        //    nRetVal = GetRecords(strWhere);
        //    for (int i = 0; i < nRetVal; i++)
        //    {
        //        ArchiveRecord();
        //        GetNext();
        //    }
        //    m_strErrMsg = string.Format("{0} records archived", nRetVal);
        //    return nRetVal;
        //}

        /// <summary>
        /// This EVENTUALLY should write old archive records to removable media in MS Access format.
        /// 
        ///      /// 09/26/2007 wdk
        /// MODIFICATIONS NEEDED
        /// 12/31/2007 once the archive tables have been updated for the three new fields this needs to be changed.
        /// </summary>
        /// <param name="strFile">Name of the database to create along with its path.</param>
        /// <param name="dtThru">The date the archived records should be moved thru</param>
        /// <returns></returns>
        public bool WriteArchiveRecordToDisk(string strFile, DateTime dtThru)
        {
            throw new System.NotImplementedException("04/02/2007 once the archive tables have been updated for the four new fields this needs to be changed.");
#pragma warning disable 0162
            bool bRetVal = false;
            if (m_strDatabase.ToUpper().IndexOf("ARC") == -1)
            {
                m_strErrMsg = "You cannot write unarchived records to disk.";
                return bRetVal;
            }

            return bRetVal;
#pragma warning restore 0162
        }

        /// <summary>
        /// Archives orphaned accounts.
        /// returns -1 on failure which loads m_strErrMsg
        /// returns  0 if Account is not an orphan
        /// returns  1 if orphaned account is archived.
        ///      /// 09/26/2007 wdk
        /// MODIFICATIONS NEEDED
        /// 12/31/2007 once the archive tables have been updated for the three new fields this needs to be changed.
        /// </summary>
        /// <param name="strAccount"></param>
        /// <returns></returns>
        public int ArchiveOrphan(string strAccount)
        {
            throw new System.NotImplementedException("04/02/2007 once the archive tables have been updated for the four new fields this needs to be changed.");
#pragma warning disable 0162
            int iRetVal = -1;
            if (m_bArchive)
            {
                m_strErrMsg = "Cannot archive from the archive database.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return iRetVal;
            }
            if (strAccount.Length == 0)
            {
                m_strErrMsg = "Account passed in is blank.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return iRetVal;
            }
            R_acc acc = new R_acc(m_strServer, m_strDatabase, ref m_ERR);
            switch (acc.DoesAccountExist(strAccount))
            {
                case 0:
                    {
                        // account exists and is not deleted
                        break;
                    }
                case 1:
                    {
                        // account exists and is deleted
                        break;
                    }
                case -1: // account does not exist.
                    { 
                        int nRecs = GetRecords(string.Format("account = '{0}'", strAccount));
                        for (int i = 0; i < nRecs; i++)
                        {
                            iRetVal = ArchiveRecord();
                            GetNext();
                        }
                        break;
                    }
            }
            return iRetVal;
#pragma warning restore 0162
        }

        /// <summary>
        /// Update the post date only. Must pass the date as a string.
        /// </summary>
        /// <param name="strDate">the date to use for post date</param>
        /// <returns></returns>
        public int UpdatePostDate(string strDate)
        {
            if (m_bArchive)
            {
                m_strErrMsg = "Cannot update the Archive Database. Must be restored to Live then updated.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return -1;
            }
            if (string.IsNullOrEmpty(strDate))
            {
                m_strErrMsg = "Cannot update the post date with an empty value.";
                m_ERR.m_Logfile.WriteLogFile(m_strErrMsg);
                return -1;
            }
            string strSQL;
            string strWhere;
           
            strWhere = string.Format("rowguid = '{0}'",
                                        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString());

            strSQL = string.Format(@" UPDATE {0} SET post_date = '{1}' WHERE {2}",
                    propTable.ToString(), strDate,  strWhere);

            int nRetval = SQLExec(strSQL, out m_strErrMsg);
            if (nRetval > 0)
            {
                nRetval = Querry(m_strWhere);
                if (nRetval >= 0)
                {
                    LoadMemberVariablesFromDataSet();
                }
            }
            return nRetval;
        }



    } // do not type below this line
}
