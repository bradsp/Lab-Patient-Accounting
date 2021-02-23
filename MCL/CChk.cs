using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using RFClassLibrary;

namespace MCL
{
    /// <summary>
    /// The primary purpose of this class is to manage the Chk table in the billing system.
    /// 
    /// 01/22/2008 wdk
    /// </summary>
    public class CChk : RFCObject
    {
        private MCL.R_acc m_Racc; // validity checking for an existing account
        private MCL.R_chk m_Rchk; 
        private string m_strServer;
        /// <summary>
        /// wdk 20090724 Gets the name of the database server.
        /// </summary>
        public string propServer
        {
            get { return m_strServer; }
        }
        private string m_strDatabase;
        /// <summary>
        /// 
        /// </summary>
        public string propDatabase
        {
            get { return m_strDatabase; }
        }
	    /// <summary>
	    /// CHK RECORDSET CONSTRUCTOR.
	    /// </summary>
	    /// <param name="strServer"></param>
	    /// <param name="strDatabase"></param>
	    /// <param name="errLog"></param>
        public CChk(string strServer, string strDatabase, ref ERR errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDatabase;
            m_Rchk = new R_chk(m_strServer, m_strDatabase, ref errLog); 
            m_Racc = new R_acc(m_strServer, m_strDatabase, ref errLog);            
        }

        /// <summary>
        /// If the check has been previously posted this will return true. Putting the amounts into the out paramaters.
        /// If there is an error it also returns true. Putting "ERR" in each of the three out paramaters.
        /// If there is no error or previously posted check it returns false.
        /// m_strErrMsg will contain text with the error message.
        /// </summary>
        /// <param name="strCheck"></param>
        /// <param name="strSource"></param>
        /// <param name="strAmtPaid">Amout Paid</param>
        /// <param name="strAmtCont">Contracutal</param>
        /// <param name="strAmtWO">Write Off</param>
        /// <returns></returns>
        public bool GetCheckAmounts(
            string strCheck, string strSource, out string strAmtPaid, out string strAmtCont, out string strAmtWO)
        {
            bool bRetVal = false;
            strAmtPaid = "ERR";
            strAmtCont = "ERR";
            strAmtWO = "ERR";
            m_strErrMsg = "";
            if (strCheck.Length == 0 || strSource.Length == 0)
            {
                m_strErrMsg = "CChk.GetBalance() Check number or Check Source was empty.";                
                return bRetVal;
            }
            try
            {
                DBAccess db = new DBAccess(m_strServer, m_strDatabase, "");
                string strFilter = string.Format("(chk_no = '{0}') AND (source = '{1}')", strCheck, strSource);

                strAmtPaid = db.GetField("chk", "SUM(amt_paid)", strFilter, out m_strErrMsg);
                strAmtCont = db.GetField("chk", "SUM(CONTRACTUAL)", strFilter, out m_strErrMsg);
                strAmtWO = db.GetField("chk", "SUM(write_off)", strFilter, out m_strErrMsg);
                decimal dResult;
                if (decimal.TryParse(strAmtPaid, out dResult))
                {
                    bRetVal = true;
                }
                if (decimal.TryParse(strAmtCont, out dResult))
                {
                    bRetVal = true;
                }
                if (decimal.TryParse(strAmtWO, out dResult))
                {
                    bRetVal = true;
                }
            }
            catch (Exception ex)
            {
                m_strErrMsg = ex.Message;
                return bRetVal;
            }

            if (strAmtPaid.Length == 0)
            {
                strAmtPaid = "0";
            }
            if (strAmtWO.Length == 0)
            {
                strAmtWO = "0";
            }
            if (strAmtCont.Length == 0)
            {
                strAmtCont = "0";
            }
            
            return bRetVal;
        }


    } // don't type below this line
}
