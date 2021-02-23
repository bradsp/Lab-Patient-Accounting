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
    /// RCRecordset for vw_prg_client_payments
    /// </summary>
    public class R_vw_prg_client_payments : RCRecordset
    {
        //===== string variables for fields in record  =====
        /// <summary>
        /// 
        /// </summary>
        public string  m_strCliNme;
        /// <summary>
        /// 
        /// </summary>
        public string  m_strAmtPaid;
        /// <summary>
        /// 
        /// </summary>
        public string  m_strDateRec;
        /// <summary>
        /// 
        /// </summary>
        public string m_strType;
        
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
        public R_vw_prg_client_payments(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "vw_prg_client_payments", ref errLog)
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
            //strWhere = string.Format("msg_status in ('NEW','REQUED') AND msg_type = 'ORDERS'  AND deleted = 0");

            Querry(strWhere);
            if (m_CurrentRecordCount > 0)
            {
                LoadMemberVariablesFromDataSet();
                m_strErrMsg = string.Format("{0} Record(s) read", m_CurrentRecordCount);

            }
            return (m_CurrentRecordCount);

        }

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
           // m_strDeleted = "F";
           // m_strRowguid = Guid.NewGuid().ToString();
            m_strModDate = Time.SNows();
            //m_strModHost = OS.GetMachineName();
           // m_strModPrg = OS.GetAppName();

            //--- table fields
            m_strCliNme = "";
            m_strAmtPaid = "";
            m_strDateRec = "";
            m_strType = "";


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
               // m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                {
                    m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
                }              
                //m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                //m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
                //m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                // ==== end of 'standard' fields =====================
                
                
                
                
                m_strCliNme = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cli_nme"].ToString();
                m_strAmtPaid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["PAID"].ToString();
                m_strType = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["type"].ToString();
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_rec"].Equals(System.DBNull.Value))
                {
                    m_strDateRec = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["date_rec"]).ToString("d");
                }
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
        // Or 
        // -1 = Error m_strErrMsg has detailis of error</returns>
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

        //        // if unarchiving a record flagged to deleted have to convert back to the live system as deleted.
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "F" ? 0 : 1;  // new record so flag it NOT deleted
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_num"] = m_strIcd9_num;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_desc"] = m_strIcd9_desc;
        //        m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["AMA_year"] = m_strAMA_year;

        //    }
        //    return iRetVal;
        //}

        // <summary>
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


    }
}
