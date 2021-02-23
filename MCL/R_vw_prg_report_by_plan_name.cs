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
using System.Data; // DataRow

namespace MCL
{
    /// <summary>
    /// RCRecordset for vw_prg_report_by_plan_name
    /// </summary>
    public class R_vw_prg_report_by_plan_name : RCRecordset
    {
        // wdk 20120305 added the ins_code
        private string m_strInsCode;
        /// <summary>
        /// Insurance code from the insurance company table.
        /// </summary>
        public string propInsCode
        {
            get { return m_strInsCode; }
            set { m_strInsCode = value.Substring(0, value.Length > 10 ? 10 : value.Length); }
        }
        // wdk 20110829 added  for Policy Number
        private string m_strPolicyNumber;
        /// <summary>
        /// Policy number for this insurance.
        /// </summary>
        public string propPolicyNumber 
        {
            get { return m_strPolicyNumber; }
            set { m_strPolicyNumber = value.Substring(0, value.Length > 20 ? 20 : value.Length); }
        }
        //===== string variables for fields in record  =====
        // wdk 20090108 added to report
        /// <summary>
        /// 
        /// </summary>
        public string m_strEUbDemandDate;
        /// <summary>
        /// 
        /// </summary>
        public string m_strCnetUbBatchDate;
        /// <summary>
        /// 
        /// </summary>
        public string m_strCnet1500BatchDate;
        // wdk 20090108 end of add
        // 09/20/2007 wdk added to report
        /// <summary>
        /// 
        /// </summary>
        public string m_strDbillDate;
        /// <summary>
        /// 
        /// </summary>
        public string m_strUBDate;
        /// <summary>
        /// 
        /// </summary>
        public string m_strH1500Date;
        /// <summary>
        /// 
        /// </summary>
        public string m_strEbillBatchDate;
        /// <summary>
        /// 
        /// </summary>
        public string m_strEbillBatch1500;
        // end of 09/20/2007 wdk add
        /// <summary>
        /// 
        /// </summary>
        public string m_strPlanName;

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
        public string m_strTransDate;

        /// <summary>
        /// 
        /// </summary>
        public string m_strInsABC;

        /// <summary>
        /// 
        /// </summary>
        public string m_strFinCode;

        /// <summary>
        /// 
        /// </summary>
        public string m_strBatchDate;

        /// <summary>
        /// 
        /// </summary>
        public string m_strDataMailer;


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
        public R_vw_prg_report_by_plan_name(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "vw_prg_report_by_plan_name", ref errLog)
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
            if ( m_CurrentRecordIndex < (m_CurrentRecordCount-1))
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
        //// <returns></returns>
        //public int FlagCurrentRecordDeleted()
        //{
        //    m_strDeleted = "T";
        //    return (Update());
        //}

        // <summary>
        // Clears the deleted flag on the current record
        // 04/12/2007 Rick Crone
        // </summary>
        ///// <returns></returns>
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
            m_strFinCode = "";
            m_strInsABC = "";
            m_strPatName = "";
            m_strPlanName = "";
            m_strTransDate = "";
            m_strBatchDate = "";
            m_strDataMailer = "";
            //09/20/2007 wdk added
            m_strDbillDate = "";
            m_strUBDate = "";
            m_strH1500Date = "";
            m_strEbillBatchDate = "";
            m_strEbillBatch1500 = "";
            // wdk 20090108 wdk added
            m_strEUbDemandDate = "";
            m_strCnetUbBatchDate = "";
            m_strCnet1500BatchDate = "";
             // wdk 20110829 added
            m_strPolicyNumber = "";
            // wdk 20120305 added
            m_strInsCode = "";

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
                //m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();
                //if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                //{
                //    m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
                //}
                //
                //m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
                //m_strModDate = Time.DateTimeToHL7TimeString(m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]);
                //m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                //m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
                //m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                // ==== end of 'standard' fields =====================
                m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"].ToString(); 
                m_strFinCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"].ToString();
                m_strInsABC = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ins_a_b_c"].ToString(); 
                m_strPatName = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pat_name"].ToString(); 
                m_strPlanName = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["plan_nme"].ToString();

                m_strTransDate = "";
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["trans_date"].Equals(null))
                {
                    m_strTransDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["trans_date"]).ToString("d");
                }

                m_strBatchDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch_date"].ToString();
                if (m_strBatchDate.Length > 0)
                {             
                    m_strBatchDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch_date"]).ToString("d");
                }
                m_strDataMailer = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mailer"].ToString();

                // dbill_date, ub_date, h1500_date, ebill_batch_date, ebill_batch_1500
            
                m_strDbillDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["dbill_date"].ToString();
                if (m_strDbillDate.Length > 0)
                {
                    m_strDbillDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["dbill_date"]).ToString("d");
                }
                m_strUBDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ub_date"].ToString();
                if (m_strUBDate.Length > 0)
                {
                    m_strUBDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ub_date"]).ToString("d");
                }
                m_strH1500Date = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["h1500_date"].ToString();
                if (m_strH1500Date.Length > 0)
                {
                    m_strH1500Date = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["h1500_date"]).ToString("d");
                }

                
                m_strEbillBatchDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ebill_batch_date"].ToString();
                if (m_strEbillBatchDate.Length > 0)
                {
                    m_strEbillBatchDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ebill_batch_date"]).ToString("d");
                }


                m_strEbillBatch1500= m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ebill_batch_1500"].ToString();
                if (m_strEbillBatch1500.Length > 0)
                {
                    m_strEbillBatch1500 = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ebill_batch_1500"]).ToString("d");
                }
                // wdk 20090108 added
                m_strCnet1500BatchDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claimsnet_1500_batch_date"].ToString();
                if (m_strCnet1500BatchDate.Length > 0)
                {
                    m_strCnet1500BatchDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claimsnet_1500_batch_date"]).ToString("d");
                }
                
                m_strCnetUbBatchDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claimsnet_ub_batch_date"].ToString();
                if (m_strCnetUbBatchDate.Length > 0)
                {
                    m_strCnetUbBatchDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claimsnet_ub_batch_date"]).ToString("d");
                }
        
                m_strEUbDemandDate = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["e_ub_demand_date"].ToString();
                if (m_strEUbDemandDate.Length > 0)
                {
                    m_strEUbDemandDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["e_ub_demand_date"]).ToString("d");
                }
        
                // wdk 20110829 added
                m_strPolicyNumber = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["policy_num"].ToString();

                // wdk 20120305 added
                m_strInsCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ins_code"].ToString(); 

                bRetVal = true;
            }

            return (bRetVal);
        }

        // <summary>
        // Add a NEW record to the table 
        // Load this class's member variables and call this function to
        // add a new record.
        // 
        // This record is added to the end of the dataset and is the current
        // record after this call.
        // 04/05/2007 Rick Crone
        // </summary>
        // <returns>number of records updated 
        ///// Or 
        ///// -1 = Error m_strErrMsg has detailis of error</returns>
        //public int AddRecord()
        //{
        //    //throw new System.NotImplementedException();
        //    int iRetVal = -1;
        //    string strSQL;

        //    //INSERT INTO MyTable (PriKey, Description)
        //    //   VALUES (123, 'A description of part 123.')
        //    //                                table fields   
        //    strSQL = string.Format("INSERT INTO {0}(deleted, icd9_num, icd9_desc, AMA_year, mod_prg) VALUES ('{1}','{2}','{3}','{4}','{5}')",
        //                                propTable.ToString(), //0
        //                                 0,//1
        //                                  m_strIcd9_num,//2
        //                                   m_strIcd9_desc,//3
        //                                    m_strAMA_year,
        //                                     OS.GetAppName());//4                             



        //    iRetVal = SQLExec(strSQL, out m_strErrMsg);
        //    if (iRetVal > 0)
        //    {
        //        // add new record to dataset
        //        DataRow L_DataRow = m_DataSet.Tables[propTable.ToString()].NewRow();
        //        m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);

        //        // index is zero based while record count is 1 based ie the correct record count
        //        m_CurrentRecordIndex = m_CurrentRecordCount++;

        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = 0; // new record so flag it NOT deleted
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_num"] = m_strIcd9_num;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_desc"] = m_strIcd9_desc;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["AMA_year"] = m_strAMA_year;

        //    }
        //    return iRetVal;
        //}

        ///// <summary>
        ///// Updates the 'current' record.
        ///// Rick Crone 04/12/2006
        ///// </summary>
        ///// <returns>number of records 
        ///// OR 
        ///// -1 = ERROR - see m_strErrMsg for details</returns>
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
        //     * Set the where class for the KEY for this table from the DataSet values
        //     */
        //    strWhere = string.Format("icd9_num = '{0}' and AMA_year = '{1}'",
        //                                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_num"].ToString(),
        //                                 m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["AMA_year"].ToString());

        //    strSQL = string.Format("UPDATE {0} SET deleted = '{1}', icd9_num = '{2}', icd9_desc = '{3}', AMA_year = '{4}', mod_prg = '{5}' WHERE {6}",
        //                               propTable.ToString(), //0
        //                                m_strDeleted == "T" ? 1 : 0,//1
        //                                 m_strIcd9_num,//2
        //                                  m_strIcd9_desc,//3
        //                                   m_strAMA_year, //4
        //                                    OS.GetAppName(),
        //                                    strWhere);


        //    return SQLExec(strSQL, out m_strErrMsg);

        //}



        //==== 'special' functions just for this class below this line ======
        // <summary>
        // Gets ACTIVE ICD9 record for the icd9 and ama year passed in.
        // 04/05/07 Rick Crone
        // </summary>
        ///// <returns></returns>
        //public int GetRecord(string strIcd9Num, string strAMAYear)
        //{
        //    //throw new System.NotImplementedException();
        //    string strWhere;                      // table
        //    strWhere = string.Format("icd9_num = '{0}' AND AMA_year = '{1}'",
        //                                 strIcd9Num,
        //                                  strAMAYear);

        //    return (GetActiveRecords(strWhere));

        //}


    }
}
