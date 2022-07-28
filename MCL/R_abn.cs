using System;
using System.Collections.Generic;
using System.Text;
// programmer added
using RFClassLibrary; // for RCRecordset
using System.Data; // DataRow

namespace MCL
{
    /// <summary>
    /// Representation of the abn class
    /// </summary>
    [Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
    public class R_abn : RCRecordset
    {
        /// <summary>
        /// Archived copy of the database
        /// </summary>
        private RCRecordset m_rcArchive;

        private string m_strDatabase;
        /// <summary>
        /// Database of record
        /// </summary>
	    public string propDatabase
	    {
	      get { return m_strDatabase;}
	    }
	
        private string m_strServer;
        /// <summary>
        /// Server of record
        /// </summary>
        public string propServer
        {
          get { return m_strServer;}
        }
	
        /// <summary>
        ///  Patients account number
        /// </summary>
        public string m_strAccount;

        /// <summary>
        /// 
        /// </summary>
        public string m_strCdm;

        /// <summary>
        /// 
        /// </summary>
        public string m_strLmrp;

        // part of base class
        //mod_date, mod_user, mod_prg 

        // additional variables added by programmer

        /// <summary>
        /// 
        /// </summary>
        public string m_strWhere;
        /// <summary>
        /// ABN constructor
        /// </summary>
        /// <param name="strServer"></param>
        /// <param name="strDataBase"></param>
        /// <param name="errLog"></param>
        public R_abn(string strServer, string strDataBase, ref ERR errLog)
            : base(strServer, strDataBase, "abn", ref errLog)
        {
            m_strServer = strServer;
            m_strDatabase = strDataBase;
            string strArchive = strDataBase.IndexOf("LIVE") > -1 ? "MCLARCLIVE" : "MCLARCTEST";
            m_rcArchive = new RCRecordset(strServer, strArchive, "abn", ref errLog);
           
        }

        /// <summary>
        /// Clears all the member variables.
        /// 
        /// 09/21/2007 wdk
        /// </summary>
        public void ClearMemberVariables()
        {
            m_strAccount = "";
            m_strCdm = "";
            m_strLmrp = "";
        }

        /// <summary>
        /// Move data from dataset into this classes 'field' member variables.
        /// Note: 'generic' member functions expect this function to be named:
        ///             LoadMemberVariablesFromDataSet()
        /// 
        /// 09/21/2007 wdk
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
// 09/21/2007 wdk not in table             //   m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();
// 09/21/2007 wdk not in table             //   m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
                if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                {
                    m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("G");
                }
                //m_strModDate = Time.DateTimeToHL7TimeString(m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]);
                m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
// 09/21/2007 wdk not in table                m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                // ==== end of 'standard' fields =====================

                m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"].ToString();
                m_strCdm = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cdm"].ToString();
                m_strLmrp = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["lmrp"].ToString();
                
                bRetVal = true;
            }

            return (bRetVal);
        }


        /// <summary>
        /// Updates the 'current' record, using both the account and cdm as this is the primary key.
        /// 09/21/2007 wdk
        /// </summary>
        /// <returns>number of records 
        /// OR 
        /// -1 = ERROR - see m_strErrMsg for details</returns>
        public int Update()
        {
            string strSQL;
            /*
             * Set the where clause for the KEY for this table from the DataSet values
             */
            m_strWhere = string.Format("account = '{0}' and cdm = '{1}'",
                                        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"].ToString(),
                                        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cdm"].ToString());

            string strUser = ParseUserHost();
            strSQL = string.Format("UPDATE {0} SET account= '{1}', cdm= '{2}', lmrp= '{3}', mod_prg= '{4}', mod_date= '{5}', mod_user='{6}' WHERE {7}",
                                       propTable, //0
                                            m_strAccount, //1
                                            m_strCdm, //2
                                            m_strLmrp, //3
                                            OS.GetAppName(),//4
                                            DateTime.Now.ToString("G"), //5
                                            strUser, //6
                                            m_strWhere); //08/11/2007 wdk made into member level variable for Error Msg inclusion

           
            return SQLExec(strSQL, out m_strErrMsg);

        }

        private static string ParseUserHost()
        {
            string strUser = OS.GetUserName();
            int nUser = strUser.Length;
            if (nUser > 37)
            {
                strUser = strUser.Substring(nUser - 37);
            }

            string strHost = OS.GetMachineName();
            int nHost = strHost.Length;
            strUser += " : ";
            if (nHost > 10)
            {
                strHost = strHost.Substring((strHost.Length - 10), 10);
            }

            strUser += strHost;

            return strUser;
        }




        /// <summary>
        /// Gets acc record for the account and cdm passed in
        /// 09/21/2007 wdk
        /// </summary>
        /// <returns></returns>
        public int GetRecordByAccount(string strAccount)
        {
            //throw new System.NotImplementedException();
            string strWhere;                      // table
            strWhere = string.Format("account = '{0}'",
                                          strAccount);
            m_strWhere = strWhere; //08/11/2007 wdk
            return (GetRecords(strWhere));

        }

        /// <summary>
        /// Generic GetRecords  - pass in where clause (without the WHERE key word).
        /// Can get active or deleted records!
        /// 09/21/2007 wdk
        /// </summary>
        /// <returns></returns>
        public int GetRecords(string strWhere)
        {
            m_strWhere = strWhere; 
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
        /// 09/21/2007 wdk
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
        /// 09/21/2007 wdk
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
        /// 09/21/2007 wdk
        /// </summary>
        /// <returns>false = error</returns>
        public bool MoveFirst()
        {
            m_CurrentRecordIndex = 1;
            return (LoadMemberVariablesFromDataSet());

        }

        /// <summary>
        /// Loads the 'field' member variables for the last record.
        /// 09/21/2007 wdk
        /// </summary>
        /// <returns>false = error</returns>
        public bool MoveLast()
        {
            m_CurrentRecordIndex = m_CurrentRecordCount;
            return (LoadMemberVariablesFromDataSet());
        }

        /// <summary>
        /// Add a NEW record to the table 
        /// 
        /// Caller should call the ClearMemberVariables() function.
        /// 
        /// Then load this class's member variables and call this function to
        /// add a new record.
        /// 
        /// This function will create the rowguid for the inserted record. Caller should not attempt to set!
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

            strSQL = string.Format("INSERT INTO {0}(account, cdm, mod_prg, mod_user, lmrp) " +
                                            "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}')",
                                                    propTable, //0
                                                    m_strAccount, //1
                                                    m_strCdm, //2
                                                    OS.GetAppName(), //3
                                                    ParseUserHost(), //4
                                                    m_strLmrp); //5

            
            iRetVal = SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                // add new record to dataset
                DataRow L_DataRow = m_DataSet.Tables[propTable].NewRow();
                m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);

                // index is zero based while record count is 1 based ie the correct record count
                m_CurrentRecordIndex = m_CurrentRecordCount++;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cdm"] = m_strCdm;
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"] = OS.GetAppName();
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"] = ParseUserHost();
                m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["lmrp"] = m_strLmrp;
            }
            return iRetVal;
        }


        /// <summary>
        /// Adds the CURRENT record to the archive table,
        /// Then DELETES the current record from the corresponding MCLLIVE/MCLTEST table
        /// 
        /// 09/25/2007 David
        /// </summary>
        /// <returns>1 Success
        ///          0 Able to add to the Archive but not able to delete the original record.
        ///                 m_strErrMsg has details
        ///          -1 = Error m_strErrMsg has details of error</returns>
        public int ArchiveRecord()
        {
            int l_nCurrentRecordCount = m_CurrentRecordCount;
            int l_nCurrentRecordIndex = m_CurrentRecordIndex;

            int iRetVal = -1;
            // no current record so can't archive
            if (m_CurrentRecordIndex < 0)
            {
                m_strErrMsg = "No current record. Archive attempt failed.";
                return -1;
            }
            // if the current member variables have not been loaded then load the variables
            LoadMemberVariablesFromDataSet();
                
            // does the current record exist in the archived table.
            m_rcArchive.m_strFilter = string.Format("account = '{0}' and cdm = '{1}'",
                                                        m_strAccount, m_strCdm);
                                                    //    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"],
                                                      //  m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["cdm"]);
            int nCount = m_rcArchive.Querry(m_rcArchive.m_strFilter);
            // NOTE: m_rcArchive.querry sets the m_CurrentRecordIndex  to -1 and the m_CurrentRecordCount to 0
            //        because they are static. Removed the static label from RFClassLibrary.
            if (nCount > 0)
            {
                m_strErrMsg = "ABN exists in archive database.";
                // restore the original counters so you can continue looping through the dataset.
                m_CurrentRecordCount = l_nCurrentRecordCount;
                m_CurrentRecordIndex = l_nCurrentRecordIndex;
                // could compare the records update the archive and delete the original record here.
                return -1;
            }

            string strSQL;
            //INSERT INTO MyTable (PriKey, Description)
            //   VALUES (123, 'A description of part 123.')
            //                                table fields   
            strSQL = string.Format("INSERT INTO {0}(account, cdm, mod_prg, mod_user, lmrp) " +
                                   "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}')",
                                           propTable, //0
                                           m_strAccount, //1
                                           m_strCdm, //2
                                           m_strModPrg, //3
                                           m_strModUser, //4
                                           m_strLmrp); //5



            iRetVal = m_rcArchive.SQLExec(strSQL, out m_strErrMsg);
            if (iRetVal > 0)
            {
                DeleteRecord();
            }
            else
            {
                // restore the original counters so you can continue looping through the dataset
                m_CurrentRecordCount = l_nCurrentRecordCount;
                m_CurrentRecordIndex = l_nCurrentRecordIndex;
                m_strErrMsg = "Could not add record to archive database.";
                iRetVal = -1;
            }
            return iRetVal;
        }

        private int DeleteRecord()
        {
            int iRetVal = -1;
            int l_nCurrentRecordIndex = m_CurrentRecordIndex;
            string strSQL = string.Format("DELETE FROM {0} WHERE ACCOUNT = '{1}' and CDM = '{2}'",
                                                propTable, m_strAccount, m_strCdm);
            iRetVal = SQLExec(strSQL, out m_strErrMsg);
            // restore the original counters so you can continue looping through the dataset
            int l_nCurrentRecordCount = GetRecords(m_strWhere);
            m_CurrentRecordIndex = (l_nCurrentRecordIndex - 1); // remove the deleted record from the count              
            return iRetVal;
        }

        /// <summary>
        /// If the Account number exists in the Current database reguardless of the deleted field value, it is not an
        /// orphan. 
        /// 
        /// 09/24/2007 wdk
        /// </summary>
        /// <param name="strAccount"></param>
        /// <returns></returns>
        public bool IsOrphan(string strAccount)
        {
            bool bRetVal = false;
            R_acc ra = new R_acc(propServer, propDatabase, ref m_ERR);
            switch(ra.DoesAccountExist(strAccount))
            {
                case -1:
                    {
                        bRetVal = true;
                        break;
                    }
                // return value from m_strDeleted in the R_acc record.
                case 0: // record exists but is not deleted 
                case 1: // record exists but is deleted.
                    {
                        // in either of these cases the record does exist and the abn record is not an orphin.
                        break;
                    }                    
            }
            return bRetVal;
        }

        /// <summary>
        /// Updates the 'archive' record, using both the account and cdm as this is the primary key.
        /// Then Deletes the active record.
        /// 09/25/2007 wdk
        /// </summary>
        /// <returns>number of records 
        /// OR 
        /// -1 = ERROR - see m_strErrMsg for details</returns>
      
        public int UpdateArchiveRecord()
        {
            string strSQL;
            /*
             * Set the where clause for the KEY for this table from the DataSet values
             */
            m_strWhere = string.Format("account = '{0}' and cdm = '{1}'",
                                        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"].ToString(),
                                        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cdm"].ToString());

           
            strSQL = string.Format("UPDATE {0} SET account= '{1}', cdm= '{2}', lmrp= '{3}', mod_prg= '{4}', mod_date= '{5}', mod_user='{6}' WHERE {7}",
                                       propTable, //0
                                            m_strAccount, //1
                                            m_strCdm, //2
                                            m_strLmrp, //3
                                            m_strModPrg,//OS.GetAppName(),//4
                                            m_strModDate,//DateTime.Now.ToString("G"), //5
                                            m_strModUser,//strUser, //6
                                            m_strWhere); //08/11/2007 wdk made into member level variable for Error Msg inclusion

           
            return m_rcArchive.SQLExec(strSQL, out m_strErrMsg);

        

            
        }


    }// don't type below this line
}
