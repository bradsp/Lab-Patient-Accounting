using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using Utilities;
using System.Collections;

namespace MCL
{
    /// <summary>
    /// The primary purpose is to manage the Acc table in Billing.
    /// Document each additional function as it is added.
    /// 08/08/2007 wdk/rgc
    /// </summary>
    [Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
    public class CAcc : RFCObject
    {
        /// <summary>
        /// wdk 20120214 added to reduce sqlserver load
        /// </summary>
        public static R_acc m_srAcc;
        /// <summary>
        /// wdk 20120214 added to reduce sqlserver load
        /// </summary>
        public static R_chrg m_srChrg;
        /// <summary>
        /// wdk 20120214 added to reduce sqlserver load
        /// </summary>
        public static R_chk m_srChk;
        /// <summary>
        /// wdk 20120214 added to reduce sqlserver load
        /// </summary>
        public R_pat m_rPat;
        /// <summary>
        /// wdk 20120214 added to reduce sqlserver load
        /// </summary>
        public R_acc m_rAccDup;
        /// <summary>
        /// Gets the last name from the Pat_name field if the record set is valid and the name is not empty and
        /// contains a comma
        /// </summary>
        public string propLastName
        {
            get 
            { 
                string strRetVal = string.Empty;
                if (m_Racc != null &&
                    !string.IsNullOrEmpty(m_Racc.m_strPatName) &&
                        m_Racc.m_strPatName.Contains(","))
                {
                    strRetVal = m_Racc.m_strPatName.Split(new char[] { ',' })[0].ToString();
                }
                return strRetVal;
            }
        }
        /// <summary>
        /// Gets the patients first name 
        /// rgc/wdk 20110913 modified to account for spaces in the patients last name area.
        /// </summary>
        public string propFirstName
        {
            get
            {
                m_strMiddleName = string.Empty;
               string strRetVal = string.Empty;
                if (m_Racc != null &&
                    !string.IsNullOrEmpty(m_Racc.m_strPatName) &&
                        m_Racc.m_strPatName.Contains(","))
                {
                    strRetVal = 
                        m_Racc.m_strPatName.Split(new string[] { ","}, 
                        StringSplitOptions.RemoveEmptyEntries)[1].ToString().Trim();
                    if (strRetVal.Contains(" "))
                    {
                        string[] strName = strRetVal.Split(new string[] { " " },
                            StringSplitOptions.None);//.RemoveEmptyEntries);
                        strRetVal = strName[0].Trim();
                            //strRetVal.Split(new string[] { " " },
                            //StringSplitOptions.RemoveEmptyEntries)[0].ToString().Trim();
                        m_strMiddleName = strName[1].Trim();
                        //strRetVal.Split(new string[] { " " },
                        //    StringSplitOptions.RemoveEmptyEntries)[1].ToString().Trim();
                    }
                }
                return strRetVal;
            }
        }
        private string m_strMiddleName = "";
        /// <summary>
        /// Get's the middle name when the prop first name has been set.
        /// </summary>
        public string propMiddleName
        {
            get { return m_strMiddleName; }
        }

        /// <summary>
        /// Recordset for R_acc
        /// </summary>
        public R_acc m_Racc;
        /// <summary>
        /// Recordset for R_notes
        /// </summary>
        public R_notes m_Rnotes;

    
        private string m_strServer;
        /// <summary>
        /// Server for the recordset.
        /// </summary>
        public string propServer
        {
            get { return m_strServer; }
        }
        private string m_strDatabase;
        /// <summary>
        /// Database for the recordset
        /// </summary>
        public string propDatabase
        {
            get { return m_strDatabase; }
        }
        /// <summary>
        /// CACC Recordset based on RCRecordset
        /// </summary>
        public CAcc(string strServer, string strDatabase, ref ERR errLog)
            : base(ref errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDatabase;
            m_Racc = new R_acc(m_strServer, m_strDatabase, ref errLog);
            m_Rnotes = new R_notes(m_strServer, m_strDatabase, ref errLog);
            
            m_ERR.m_Logfile.WriteLogFile(m_ERR.m_bValid.ToString());
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_ERR);
            m_rAccDup = new R_acc(m_strServer, m_strDatabase, ref m_ERR);
            m_srAcc = new R_acc(m_strServer, m_strDatabase, ref m_ERR);
            m_srChrg = new R_chrg(m_strServer, m_strDatabase, ref m_ERR);
            m_srChk = new R_chk(m_strServer, m_strDatabase, ref m_ERR);
        }

        #region obsolete constructor
        //[Obsolete("Call the constructor.\r\n\tpublic CAcc(string strServer, string strDatabase, ref ERR errLog)")]
        //public CAcc(string strServer, string strDatabase)
        //{
        //    m_strServer = strServer;
        //    m_strDatabase = strDatabase;
        //    m_Racc = new R_acc(m_strServer, m_strDatabase);
        //}
        #endregion obsolete constructor

        /// <summary>
        /// Returns true if the account exists and is not flagged deleted, 
        /// otherwise it returns false.
        /// 
        /// 01/08/08 rgc/wdk
        /// </summary>
        /// <param name="strAccount"></param>
        /// <returns></returns>
        public bool AccountIsValid(string strAccount)
        {
            bool bRetVal = false;
            
            
            //  m_Racc.DoesAccountExist(strAccount) return
            //     0 = the account exists and is NOT deleted. 
            //     1 = the account exists and is deleted. 
            //    -1 = the account does not exist in the table.
            switch (m_Racc.DoesAccountExist(strAccount))
            {
                case 0:
                    {
                        bRetVal = true;
                        break;
                    }
                default: // Account does not exsist or is deleted
                    {
                        bRetVal = false;
                        break;
                    }
            }
            
            return bRetVal;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strTransactionDate"></param>
        /// <returns></returns>
        public bool GetTransactionDate(string strAccount, out string strTransactionDate)
        {
            bool bRetVal = false;
            strTransactionDate = "ERR";
            if (string.IsNullOrEmpty(strAccount))
            {
                m_strErrMsg = "GetTransactionDate() strAccount's number was empty.";
                return bRetVal;
            }
            int nRec = m_Racc.GetActiveRecords(string.Format("account = '{0}'", strAccount));
            if (!string.IsNullOrEmpty(m_Racc.propTransDate))
            {
                strTransactionDate = m_Racc.propTransDate;
                bRetVal = true;
            }

            return bRetVal;
        }

        /// <summary>
        /// wdk 20090107 added to get the name from the account 
        /// returns true if able and the out varialbe is over written with the name.
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strName"></param>
        /// <returns></returns>
        public bool GetName(string strAccount, out string strName)
        {
            bool bRetVal = false;
            strName = "ERR";
            if (strAccount.Length == 0)
            {
                m_strErrMsg = "GetName() strAccount's number was empty.";
                return bRetVal;
            }
            int nRec = m_Racc.GetActiveRecords(string.Format("account = '{0}'",strAccount));
            if (!string.IsNullOrEmpty(m_Racc.m_strPatName))
            {
                strName = m_Racc.m_strPatName;
                bRetVal = true;
            }

            return bRetVal;
        }

        /// <summary>
        /// rgc/wdk 20090813 added to get the status from the account 
        /// returns true if able and the out varialbe is over written with the status.
        /// if the out strStatus is empty then the status was blank in the account table.
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strStatus"></param>
        /// <returns></returns>
        public bool GetStatus(string strAccount, out string strStatus)
        {
            bool bRetVal = false;
            strStatus = "";
            if (string.IsNullOrEmpty(strAccount))
            {
                m_strErrMsg = "GetStatus() strAccount's number was empty.";
                return bRetVal;
            }
            int nRec = m_Racc.GetActiveRecords(string.Format("account = '{0}'", strAccount));
            if (!string.IsNullOrEmpty(m_Racc.m_strStatus))
            {
                strStatus = m_Racc.m_strStatus;
                bRetVal = true;
            }

            return bRetVal;
        }

        /// <summary>
        /// Allows the setting of the account status.
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strStatus"></param>
        /// <returns></returns>
        public bool SetStatus(string strAccount, string strStatus)
        {
            bool bRetVal = false;
            
            if (string.IsNullOrEmpty(strAccount))
            {
                m_strErrMsg = "SetStatus() strAccount's number was empty.";
                return bRetVal;
            }
            int nRec = m_Racc.GetActiveRecords(string.Format("account = '{0}'", strAccount));
            m_Racc.m_strStatus = strStatus;
            nRec = m_Racc.Update();
            bRetVal = nRec > 0 ? true : false;
            return bRetVal;
        }

        /// <summary>
        /// Returns the balance for the given account. 
        /// 08/08/2007 wdk/rgc
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strBal"></param>
        /// <returns></returns>
        public bool GetBalance(string strAccount, out string strBal)
        {
            bool bRetVal = false;
            strBal = "ERR";
            if (string.IsNullOrEmpty(strAccount))
            {
                m_strErrMsg = "GetBalance() Account number was empty.";
                return bRetVal;
            }
            
            DBAccess db = new DBAccess(m_strServer, m_strDatabase, "");
            string strChrgNetAmt = 
                db.GetField("vw_net_charge_david", 
                "total_chg", string.Format("account = '{0}'", strAccount), out m_strErrMsg);
            //string strChk = db.GetField("vw_net_pay_david", "total_chk", string.Format("account = '{0}'", strAccount), out m_strErrMsg);
            string strChrg = db.GetField("vw_chrg_bal", "total", string.Format("account = '{0}'", strAccount), out m_strErrMsg);
           
            string strChk = db.GetField("vw_chk_bal", "total", string.Format("account = '{0}'", strAccount), out m_strErrMsg);
            if (string.IsNullOrEmpty(strChrg))
            {
                m_strErrMsg = string.Format("Unable to retrieve charge for account {0}", strAccount);
                return bRetVal;
            }
            if (string.IsNullOrEmpty(strChk)) // 06/10/2008 rgc/wdk unable to retrieve a check from the GetField call in dbAccess.
            {
                strChk = "0.00";
            }
            double dchk = -1;
            try
            {
                strBal = string.Format("{0}", (double.Parse(strChrg) - (double.TryParse(strChk, out dchk) ? dchk : double.Parse("0.00"))));
                bRetVal = true;
            }
            catch (Exception excpt)
            {
                m_strErrMsg = string.Format("CALLING METHOD: {0}\r\n\nSOURCE: {1}\r\n\nEXCPT INSTANCE: {2}\r\n\nSTACK: {3}", OS.getCallingMedthodName(),excpt.Source, excpt.InnerException, excpt.StackTrace);
            }

            return bRetVal;
        }

        /// <summary>
        /// Adds the comment to the note table 
        /// </summary>
        /// <param name="strNote">Note to add to the table</param>
        /// <returns>-1 if the string is empty or the number of records that are in the notes recordset.</returns>
        public int AddNote(string strNote)
        {
            int nRetVal = -1;
            if (string.IsNullOrEmpty(strNote))
            {
                return nRetVal;
            }
            if (!string.IsNullOrEmpty(m_Racc.m_strAccount))
            {
                m_Rnotes.propAccount = m_Racc.m_strAccount;
                m_Rnotes.propComment = strNote;
                nRetVal = m_Rnotes.AddRecord(string.Format("account = '{0}'", m_Racc.m_strAccount));
               
            }
            return nRetVal;
        }

        /// <summary>
        /// LoadAccount initializes all of the recordsets for this account.
        /// Returns true if acc recordset was able to get a record.
        /// </summary>
        /// <param name="strAcc"></param>
        /// <returns></returns>
        public bool LoadAccount(string strAcc)
        {
            bool bRetVal = false;
            int nRec = m_Racc.GetActiveRecords(string.Format("account = '{0}'", strAcc));
            if (nRec == 1)
            {
                bRetVal = true;
            }
            // rgc/wdk 20100609 keep the notes in sync with this account.
            m_Rnotes.GetRecords(string.Format("account = '{0}' order by mod_date desc", m_Racc.m_strAccount));

            return bRetVal;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="strPostDate"></param>
        /// <param name="strBal"></param>
        /// <returns></returns>
        public bool GetBalanceByPostDate(string strAccount, string strPostDate, out string strBal)
        {
            bool bRetVal = false;
            strBal = "ERR";
            if (string.IsNullOrEmpty(strAccount))
            {
                m_strErrMsg = "GetBalance() Account number was empty.";
                return bRetVal;
            }

            DBAccess db = new DBAccess(m_strServer, m_strDatabase, "");
            //string strChrg = db.GetField("vw_net_charge_david", "total_chg", string.Format("account = '{0}'", strAccount), out m_strErrMsg);
            //string strChk = db.GetField("vw_net_pay_david", "total_chk", string.Format("account = '{0}'", strAccount), out m_strErrMsg);
            string strChrg = db.GetField(string.Format("usp_GetAccBalanceAsOfDate @Account = '{0}', @mod_date = '{1}'",strAccount,strPostDate),
                                "Balance",
                                    string.Format("account = '{0}' and mod_date <= '{1}'", 
                                        strAccount,strPostDate), out m_strErrMsg);
            string strChk = db.GetField("vw_chk_bal", "total", string.Format("account = '{0}' and mod_date <", strAccount), out m_strErrMsg);
            if (string.IsNullOrEmpty(strChrg))
            {
                m_strErrMsg = string.Format("Unable to retrieve charge for account {0}", strAccount);
                return bRetVal;
            }
            if (string.IsNullOrEmpty(strChk)) // 06/10/2008 rgc/wdk unable to retrieve a check from the GetField call in dbAccess.
            {
                strChk = "0.00";
            }
            double dchk = -1;
            try
            {
                strBal = string.Format("{0}", (double.Parse(strChrg) - (double.TryParse(strChk, out dchk) ? dchk : double.Parse("0.00"))));
                bRetVal = true;
            }
            catch (Exception excpt)
            {
                m_strErrMsg = string.Format("CALLING METHOD: {0}\r\n\nSOURCE: {1}\r\n\nEXCPT INSTANCE: {2}\r\n\nSTACK: {3}", OS.getCallingMedthodName(), excpt.Source, excpt.InnerException, excpt.StackTrace);
            }

            return bRetVal;
        }

        /// <summary>
        /// returns true if there is a possible duplicate account and fills the string with the first value to be checked
        /// </summary>
        /// <param name="alDupAcc">An array list of accounts to be loaded</param>
        /// <returns>true if there is more than one record</returns>
        public bool HasDuplicateAccount(ref ArrayList alDupAcc)
        {
           if (string.IsNullOrEmpty(propLastName))
            {
                m_ERR.m_Logfile.WriteLogFile(string.Format("Name is empty {0}", m_Racc.m_strPatName));
                return false;
            }
            string strFilter =
                string.Format("status not in ('closed','paid_out') and pat_name like '{0},{1}%' and trans_date = '{2}' and fin_code = '{3}' and (ssn = '{4}' or ssn is null or ssn = '')",
                    propLastName, propFirstName.Substring(0, 1), m_Racc.propTransDate, m_Racc.m_strFinCode, m_Racc.m_strSsn);
            int nRec = m_rAccDup.GetRecords(strFilter);
            if (nRec > 1)
            {
                while (m_rAccDup.propErrMsg != "EOF")
                {
                    string strBal;
                    // rgc/wdk 20100609 use this Recordset copy of GetBalance instead of creating a new one.
                    // has helped the speed of loading the application and reduced the number of constructors being done.
                    //CAcc tempAcc = new CAcc(m_strServer, m_strDatabase, ref m_ERR);
                    //tempAcc.GetBalance(dupAcc.m_strAccount, out strBal);
                    GetBalance(m_rAccDup.m_strAccount, out strBal);
                    //R_pat m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_ERR);
                    m_rPat.ClearMemberVariables();
                    m_rPat.GetActiveRecords(string.Format("account = '{0}'", m_rAccDup.m_strAccount));
                    string strRetVal = string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}", 
                            m_rAccDup.m_strAccount, 
                                m_rAccDup.m_strPatName,
                                    m_rAccDup.propTransDate, 
                                        m_rAccDup.m_strCliMnem,
                                            m_rAccDup.m_strFinCode, 
                                                m_rPat.propDob_yyyy,
                                                m_rAccDup.m_strSsn,
                                                m_rAccDup.m_strStatus,
                                                    strBal);
                    if (alDupAcc.Contains(strRetVal))
                    {
                        m_rAccDup.GetNext();
                        continue;
                    }
                    alDupAcc.Add(strRetVal);
                    m_rAccDup.GetNext();
                }
            }
            
            return (nRec > 1);
        }

        /// <summary>
        /// the args are 
        /// </summary>
        /// <param name="args">/ENVIRONMENT (LIVE/TEST), /SERVER, /DATABASE, /DATE_THRU, /POST_DATE</param>
        static public int[] PostAccount(string[] args)
        {
            int[] nRetVal = new int[3];

            if (args.GetUpperBound(0) < 3)
            {
                return nRetVal;
            }
            //ERR e = new ERR(args);
            //R_acc lAcc = new R_acc(args[1].Replace("/", ""), args[2].Replace("/", ""), ref e);
            //R_chrg lChrg = new R_chrg(args[1].Replace("/", ""), args[2].Replace("/", ""), ref e);
            //R_chk lChk = new R_chk(args[1].Replace("/", ""), args[2].Replace("/", ""), ref e);
            string strPostDate = args[3].Remove(0, 1);
            string strThruDate = args[3].Remove(0, 1);
            int nRecAcc =
                m_srAcc.GetActiveRecords(string.Format("post_date is null and trans_date <= '{0} 23:59'", strThruDate));
                //lAcc.GetActiveRecords(string.Format("post_date is null and trans_date between '{0} 00:00' and '{1} 23:59'", args[3].Remove(0, 1), args[4].Remove(0, 1)));
            int nRecChrg = 0;
            int nRecChk = 0;
            nRetVal[0] = nRecAcc;
            for (int nAcc = 0; nAcc < nRecAcc; nAcc++)
            {
                nRecChrg = m_srChrg.GetNonCreditedRecords(string.Format("post_date is null and account = '{0}'", m_srAcc.m_strAccount));
                nRetVal[1] = nRecChrg;
                for (int nChrg = 0; nChrg < nRecChrg; nChrg++)
                {
                    m_srChrg.propPostDate = strPostDate;
                    m_srChrg.propFinCode = m_srAcc.m_strFinCode;
                    m_srChrg.Update();
                    m_srChrg.GetNext();
                }
                
                nRecChk = m_srChk.GetActiveRecords(string.Format("post_date is null and account = '{0}'", m_srAcc.m_strAccount));
                nRetVal[2] = nRecChk;
                for (int nChk = 0; nChk < nRecChk; nChk++)
                {
                    m_srChk.propPostDate = strPostDate;
                    m_srChk.Update();
                    m_srChk.GetNext();
                }
                m_srAcc.propPostDate = strPostDate;
                m_srAcc.Update();
                m_srAcc.GetNext();
            }




            return nRetVal;
            

        }

        private void CheckLMRP(int nLMRP, string strTestMnem, string strAMAYear, string[] strArrICD9s)
        {
            ////string[] strArrICD9s = new string[] { "600.00", "294.11" };
            //if (nLMRP > 0) // cpt4 is in the table
            //{
            //    if (int.Parse(m_rsWlmrp.propChkForBad) != 0)
            //    {
            //        if (m_rsWlmrp.CheckForBad(strArrICD9s, strAMAYear) < int.Parse(m_rsWlmrp.propChkForBad))
            //        // wdk 20110113 changed this check because if any record is returned then this is ok to order
            //        //if (m_rsWlmrp.CheckForBad(strArrICD9s, strAMAYear) < 1)
            //        {
            //            m_ERR.AddErrorToDataSet("LMRP", strTestMnem, string.Format("TEST {0}, CPT4 {1} ", strTestMnem, m_rsWlmrp.propCpt4));
            //        }
            //    }
            //    else
            //    {
            //        if (m_rsWlmrp.CheckForGood(strArrICD9s, strAMAYear) != int.Parse(m_rsWlmrp.propChkForBad))
            //        {
            //            m_ERR.AddErrorToDataSet("LMRP", strTestMnem, string.Format("TEST {0}, CPT4 {1} ", strTestMnem, m_rsWlmrp.propCpt4));
            //        }
            //    }
            //}
        }


    } // don't go below this line
}
