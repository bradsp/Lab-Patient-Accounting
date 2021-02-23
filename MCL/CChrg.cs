using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// programmer added
using RFClassLibrary;
using System.Data;
namespace MCL
{
   /// <summary>
   /// Chg class
   /// </summary>
    public class CChrg : RFCObject
    {
        /// <summary>
        /// 
        /// </summary>
        public DataSet m_dsChrg;
        private string m_strServer;
        private string m_strDatabase;
        new ERR m_ERR;
        R_chrg _chrg;
        R_amt _amt;

        private string m_strAccount;   
        /// <summary>
        /// Sets / Gets the account number for the charge
        /// </summary>
        public string propAccount 
        {
            get { return m_strAccount; }
            set 
            { 
                m_strAccount = value;
                Load();
            }
        }

       
        /// <summary>
        /// Creates an instance of this class.
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDatabase"></param>
        /// <param name="m_Err"></param>
        public CChrg(string strServer, string strDatabase, ref ERR m_Err)
        {
            m_ERR = m_Err;
            m_strServer = strServer;
            m_strDatabase = strDatabase;
            CreateDataSet();
        }

        private void CreateDataSet()
        {
            m_dsChrg.Tables.Add("CHRG");
            m_dsChrg.Tables["CHRG"].Columns.Add("rowguid", typeof (Guid));
            m_dsChrg.Tables["CHRG"].Columns.Add("credited",typeof (bool));
            m_dsChrg.Tables["CHRG"].Columns.Add("chrg_num",typeof (decimal));
            m_dsChrg.Tables["CHRG"].Columns.Add("account",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("status",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("service_date",typeof (DateTime));
            m_dsChrg.Tables["CHRG"].Columns.Add("hist_date",typeof (DateTime));
            m_dsChrg.Tables["CHRG"].Columns.Add("cdm",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("qty",typeof (int));
            m_dsChrg.Tables["CHRG"].Columns.Add("retail",typeof (decimal));
            m_dsChrg.Tables["CHRG"].Columns.Add("inp_price",typeof (decimal));
            m_dsChrg.Tables["CHRG"].Columns.Add("comment",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("invoice",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("net_amt",typeof (decimal));
            m_dsChrg.Tables["CHRG"].Columns.Add("fin_type",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("mt_req_no",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("post_date",typeof (DateTime));
            m_dsChrg.Tables["CHRG"].Columns.Add("mod_date",typeof (DateTime));
            m_dsChrg.Tables["CHRG"].Columns.Add("mod_user",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("mod_prg",typeof (string));
            m_dsChrg.Tables["CHRG"].Columns.Add("mod_host",typeof (string));
            
            m_dsChrg.Tables.Add("AMT");
            m_dsChrg.Tables["AMT"].Columns.Add("chrg_num",typeof (decimal));
            m_dsChrg.Tables["AMT"].Columns.Add("cpt4",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("type",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("amount",typeof (decimal));
            m_dsChrg.Tables["AMT"].Columns.Add("deleted",typeof (bool));
            m_dsChrg.Tables["AMT"].Columns.Add("uri",typeof (int));
            m_dsChrg.Tables["AMT"].Columns.Add("modi",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("modi2",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("revcode",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("diagnosis_code_ptr",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("mod_date",typeof (DateTime));
            m_dsChrg.Tables["AMT"].Columns.Add("mod_user",typeof (string));
            m_dsChrg.Tables["AMT"].Columns.Add("mod_prg",typeof (string));

            DataColumn[] pKey = new DataColumn[] { m_dsChrg.Tables["CHRG"].Columns["chrg_num"] };
            m_dsChrg.Tables["CHRG"].PrimaryKey = pKey;
            m_dsChrg.Tables["AMT"].PrimaryKey = pKey;
            
        }

        private void Load()
        {
            _chrg = new R_chrg(m_strServer, m_strDatabase, ref m_ERR);
            _chrg.GetRecords(string.Format("account = '{0}'",m_strAccount));
            _amt = new R_amt(m_strServer, m_strDatabase, ref m_ERR);
        }



    } // don't go below this line
}
