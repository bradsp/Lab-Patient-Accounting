// Rick Crone 07/2008
// this class should be in GOMCL instead of here in MCL
//The purpose of this table is to allow the FORD services to know what to do and when to do it.
// this table was added to avoid updating the wreq as it is replicated - the client updates this table too so this was causing replication conflicts
//----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Text;
//Programmer added usings
using RFClassLibrary;
using System.IO;
using System.Data;

namespace MCL
{
    public  class R_services_status:RCRecordset
    {
        /// <summary>
        /// when the QPMREQ_status is set by GetAcc
        /// 08/21/2008 rgc
        /// </summary>
        private string m_strQPMREQ_time;
        public string propQPMREQ_time
        {
        get {return m_strQPMREQ_time;}
        set {
            m_strQPMREQ_time = value;
             }
        }
        /// <summary>
        /// set by GetACC so QPMReq can select the record and get the meditech account number
        /// 08/21/2008 rgc
        /// </summary>
        
        private string m_strQPMREQ_status;
        public string propQPMREQ_status
        {
            get { return m_strQPMREQ_status; }
            set
            {
                m_strQPMREQ_status = value;
            }
        }
        /// <summary>
        /// set by GetACC so QPMReq can get the meditech account number and place it in the worder record
        /// 08/21/2008 rgc
        /// </summary>
        
        private string m_strAccount;
        public string propAccount
        {
            get { return m_strAccount; }
            set
            {
                m_strAccount = value;
            }
        }

        private string m_strQPMREG_time;
        public string propQPMREG_time
        {
            get { return m_strQPMREG_time; }
            set
            {
                m_strQPMREG_time = value;
            }
        }
        private string m_strWreqRowguid;
        public string propwreq_rowguid
		{
			get { return m_strWreqRowguid; } 
			set {
					m_strWreqRowguid = value;
				}
		}
		

        private string m_strQPMREG_status;
        /// <summary>
        /// Why won't you do right?
        /// </summary>
        public string propQPMREG_status
		{
			get { return m_strQPMREG_status; } 
			set {
					m_strQPMREG_status = value;
				}
		}
		
             
        //Private variables set in constructor to initialize the base class
        private string m_strServer;
        private string m_strDataBase;
             /// <summary>
             /// Constructor
             /// </summary>
             /// <param name="strServer"></param>
             /// <param name="strDataBase"></param>
             /// <param name="errLog"></param>
        public R_services_status(string strServer, string strDataBase, ref ERR errLog)
              : base(strServer, strDataBase, "services_status", ref errLog)
        {
             m_strServer = strServer;
             m_strDataBase = strDataBase;
				m_ERR = errLog;
        }
             
        public int GetRecords(string strWhere)
        {
		     Querry(strWhere);
             if (m_CurrentRecordCount> 0)
             {    LoadMemberVariablesFromDataSet();
                  m_strErrMsg = string.Format("{0} Records(s) read", m_CurrentRecordCount);
             }
             return (m_CurrentRecordCount);
        }
         
        // NOT STANDARD - no deleted field
        //public int GetActiveRecords(string strWhere)
        //{
        //     strWhere = string.Format("deleted = 0 AND {0}", strWhere);
        //     return (GetRecords(strWhere));
        //}
             
        public bool GetNext()
        {
             bool bRetVal = false;
	            if (m_CurrentRecordIndex < (m_CurrentRecordCount-1))
             {
                  m_CurrentRecordIndex++;
                  return(LoadMemberVariablesFromDataSet());
             }
             m_strErrMsg = "EOF";
             return (bRetVal);
        }
             
        public bool GetPrev()
        {
             bool bRetVal = false;
	            if (m_CurrentRecordIndex > 1)
             {
                  m_CurrentRecordIndex--;
                  return(LoadMemberVariablesFromDataSet());
             }
             m_strErrMsg = "BOF";
             return (bRetVal);
        }
             
        public bool MoveFirst()
        {
	            m_CurrentRecordIndex = 1;
             return(LoadMemberVariablesFromDataSet());
        }
             
        public bool MoveLast()
        {
	            m_CurrentRecordIndex = m_CurrentRecordCount;
             return(LoadMemberVariablesFromDataSet());
        }
        // NOT STANDARD - no deleted field     
        //public int FlagCurrentRecordDeleted()
        //{
        //     m_strDeleted = "T";
        //     return(Update());
        //}

        // NOT STANDARD - no deleted field
        //public int FlagCurrentRecordNOTDeleted()
        //{
        //     //   m_strDeleted = "F";
        //     //return(Update());
        //    return (0);// no record updated
        //}
             
        public void ClearMemberVariables()
        {
	            // Standard Fields
            /*  NO STANDARD FIELDS - work table
            m_strDeleted = "F";
             m_strRowguid = Guid.NewGuid().ToString();
             m_strModDate = Time.SNows();
             m_strModHost = OS.GetMachineName();
             m_strModPrg = OS.GetAppName();
             */
             // Tables fields
            m_strQPMREG_status = "";
            m_strQPMREG_time = Time.SNows();
            m_strQPMREQ_status = "";
            m_strQPMREQ_time = Time.SNows();
            m_strAccount = "";
            m_strRowguid = "";

             //throw new NotImplementedException();
        }
             
        public bool LoadMemberVariablesFromDataSet()
        {
	         ClearMemberVariables();
             bool bRetVal = false;
             if (m_CurrentRecordIndex > -1) // do not attempt to load if there are no records
             {
                 /* NO STANDARD FIELDS 
                 // ==== "standard" fields =====================
                  m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();
                  m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
                  if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
                  {
                       m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
                  }
                  m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
                  m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); 
                  m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
                  // ==== End of "standard fields" =============
                  throw new NotImplementedException();
                  */
                 m_strWreqRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["wreq_rowguid"].ToString();
                 m_strQPMREG_status = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["QPMREG_status"].ToString();
                 if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["QPMREG_time"].Equals(System.DBNull.Value))
                 {
                     m_strQPMREG_time = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["QPMREG_time"]).ToString("G");
                 }

                 //08/21/2008 new fields added to the table
                 m_strQPMREQ_status = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["QPMREQ_status"].ToString();
                 if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["QPMREQ_time"].Equals(System.DBNull.Value))
                 {
                     m_strQPMREQ_time = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["QPMREQ_time"]).ToString("G");
                 }
                 m_strAccount = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["account"].ToString();

             }
             return (bRetVal);
        }
             
        public int AddRecord()
        {
	         int iRetVal = -1;
             string strSQL;
             //INSERT INTO MyTable (PriKey, Description)
             //   VALUES (123, 'A description of part 123.')
             //                                table fields   

             //strSQL = string.Format("INSERT INTO {0}(account, cdm, mod_prg, mod_user, lmrp) " +
             //                                "VALUES ('{1}',  '{2}',   '{3}',   '{4}',    '{5}')",
             //                                        propTable, //0
             //                                        m_strAccount, //1
             //                                        m_strCdm, //2
             //                                        OS.GetAppName(), //3
             //                                        ParseUserHost(), //4
             //                                        m_strLmrp); //5

            
             //throw new NotImplementedException();
             strSQL = string.Format("INSERT INTO {0} " +
             "(wreq_rowguid, QPMREG_status, QPMREG_time, QPMREQ_status, QPMREQ_time, account) "+
             "VALUES ('{1}', '{2}', '{3}', '{4}','{5}','{6}')",
             propTable,//0
             m_strWreqRowguid,//1
             m_strQPMREG_status,//2
             m_strQPMREG_time,//3
             m_strQPMREQ_status,//4
             m_strQPMREQ_time,//5
             m_strAccount);//6

             iRetVal = SQLExec(strSQL, out m_strErrMsg);
             if (iRetVal > 0)
             {
                  //Add new Record to dataset
                  DataRow L_DataRow = m_DataSet.Tables[propTable].NewRow();
                  m_DataSet.Tables[propTable].Rows.Add(L_DataRow);
                  // Index is zero based while record count is 1 based  ie. the correct recourd count
                  m_CurrentRecordIndex = m_CurrentRecordCount++;
                  // if unarchiving a record flagged to deleted have to covert back to the live system as deleted
                  //m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "F" ? 0 : 1;
                  m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["wreq_rowguid"] = m_strWreqRowguid;
                  m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["QPMREG_status"] = m_strQPMREG_status;
                  m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["QPMREG_time"] = m_strQPMREG_time;   
                  m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["QPMREQ_status"] = m_strQPMREQ_status;
                  m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["QPMREQ_time"] = m_strQPMREQ_time;
                  m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
                 // use the above line to set the properties of the data set for the new row.
             }
             return iRetVal;
        }
        
        /// <summary>
        /// Updates the recordset. Set the where clause for the KEY for this table from the DataSet values
        /// </summary>
        /// <returns></returns>
        public int Update()
        {
	         string strWhere;
             string strSQL;
             
             strWhere = string.Format("wreq_rowguid = '{0}'",
                        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["wreq_rowguid"].ToString());
             
             strSQL = string.Format("UPDATE {0} SET " +
             "wreq_rowguid = '{1}', QPMREG_status = '{2}', QPMREG_time  = '{3}',"+
             "QPMREQ_status = '{4}', QPMREQ_time = '{5}',account = '{6}' "+
             "WHERE ({7})", 
             propTable,//0
             m_strWreqRowguid,//1
             m_strQPMREG_status,//2
             m_strQPMREG_time,//3
             m_strQPMREQ_status,//4
             m_strQPMREQ_time,//5
             m_strAccount,//6
             strWhere);//7
             
            // 20100405 wdk added try catch
             int nRec = -1;
             try
             {
                 nRec = SQLExec(strSQL, out m_strErrMsg); 
             }
             catch (Exception ex)
             {
                 m_ERR.m_Logfile.WriteLogFile(string.Format("Exception caught [{0}].\r\n\t Sql String Executed [{1}].",
                     ex.Message, strSQL));
             }
             
                
             return nRec;
        }
     } // do not go below this line
}

