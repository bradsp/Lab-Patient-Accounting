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
//--- added
//using System.Data.SqlClient; // SQL 7.0

namespace RFClassLibrary
{
    /// <summary>
    /// This is the representation of the Conflict table in the Sql server database. It allows access
    /// to the total number of tables and rows within thoses tables for replication events that fail because 
    /// of conflicts
    /// 
    /// 10/19/2007 wdk
    /// </summary>
    public class R_Conflict : RCRecordset
    {
        //===== string variables for fields in record  =====
        /// <summary>
        ///  This is the Table name that contains a conflict
        /// </summary>
        public string m_strTname;

        /// <summary>
        ///  This is the date the Stored procedure was run that created the conflict table.
        /// </summary>
        public string m_strTdate;

        /// <summary>
        /// this is the count of the rows in the Table that contains a conflict.
        /// </summary>
        public string m_strTqty;
 
        //===== end of string variables for fields in record  =====

        /// <summary>
        /// Consructor - base class construtor does the work!
        /// passes table name to base class DBAccess
        /// - we can get the table name from the base class with propTable.ToString()
        /// 04/05/2007 Rick Crone
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDataBase"></param>
        /// <param name="errLog">Reference to the applications ERR Class</param>
        public R_Conflict(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "Conflict", ref errLog)
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
            // 10/19/2007 wdk not applicable to this table

            //--- table fields
            m_strTname = "";
            m_strTqty = "";
            m_strTdate = "";

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
                // 10/19/2007 wdk not applicable to this table
                // ==== end of 'standard' fields =====================

                m_strTname = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["Tname"].ToString();
                m_strTqty = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["Tqty"].ToString();
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["Tdate"].Equals(System.DBNull.Value))
                {
                    m_strTdate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["Tdate"]).ToString("d");
                }
                bRetVal = true;
            }
            return (bRetVal);
        }

    }
}
