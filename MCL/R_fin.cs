/*
 * R_Fin 06/20/2007 David Kelly
 * 
 * New class dervied from RCRecordset!
 * 
 * This is the [fin] table of the MCLLive database. 
 * 
 * Use the m_str field variables to read or update the values in the table.
 * 
 * calling example:
 *          // INSTANIATE THE RECORD SET 
            R_fin r_fim = new R_fin("MCL02","MCLTEST");
            if (!r_fin.m_bValid)
            {
                r_fin.DispErrMsg();
                return;
            }

            // READING A RECORD with 'special' function
 *          string strFinCode = "A";
            if (r_fin.GetRecord(strFinCode) == -1) // 0 = no record found -1 = error
            {
                r_fin.DispErrMsg();
                return;
            }

            // ADDING A NEW RECORD
            // clear first to avoid values from reading some other record
            // and to set some of the 'standard' field values appropriate 
            // for a NEW record
            r_fin.ClearMemberVariables();
            // set 'field' values
            r_fin.m_strDeleted = "F";
            r_fin.m_strFinCode = "A";
            r_fin.m_strResParty = "MEDICARE";
            r_fin.m_strFormType = "UB"; // converted from UB92 to just UB in this record set the table's field contains UB92
            r_fin.m_strChrgSource = "CDM";
            r_fin.m_strType = "M";
            r_fin.m_strH1500 = "P";
            r_fin.m_strUB = "T"; // converted from UB92 to just UB in this record set the table's field name is  UB92
    
            // call the AddRecord() function
            if (r_fin.AddRecord() == -1)
            {
                r_fin.DispErrMsg();
                return;
            }
            
            //UPDATING AN EXISTING RECORD
            // FIRST GET THE RECORD
            //- in this case the record I just wrote so this is NOT nessasary as it is the current record
            if (r_fin.GetRecord(strFincode, bDeleted) < 1)
            {
                r_fin.DispErrMsg();
                return;
            }

            // wdk not allowing change of field values
            // but if I did do it like this.
            r_fin.deleted = "T";
            if (r_fin.Update() == - 1)
            {
                r_fin.DispErrMsg();
                return;
            }
 * 
 * The script for the view is below
 * 
 *  Used in MCL's R_vw_prg_report_by_plan_name
	to allow viewing of Insurance Records by financial code sorted on
	plan name.

***** Object:  View [dbo].[vw_prg_report_by_plan_name]    Script Date: 07/20/2007 11:55:06 *****
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE VIEW [dbo].[vw_prg_report_by_plan_name]
AS
SELECT     TOP 100 PERCENT dbo.ins.plan_nme, dbo.acc.account, dbo.acc.pat_name, dbo.acc.trans_date, dbo.ins.ins_a_b_c, dbo.ins.fin_code, 
                      dbo.pat.batch_date
FROM         dbo.acc INNER JOIN
                      dbo.ins ON dbo.acc.account = dbo.ins.account LEFT OUTER JOIN
                      dbo.pat ON dbo.acc.account = dbo.pat.account
WHERE     (NOT (dbo.acc.status IN ('PAID_OUT', 'CLOSED')))
GROUP BY dbo.acc.fin_code, dbo.ins.plan_nme, dbo.acc.account, dbo.acc.pat_name, dbo.acc.fin_code, dbo.acc.trans_date, dbo.ins.ins_a_b_c, dbo.ins.fin_code, 
                      dbo.pat.batch_date
HAVING      (dbo.acc.trans_date > CONVERT(DATETIME, '2007-01-01 00:00:00', 102))
ORDER BY dbo.ins.fin_code, dbo.ins.plan_nme, dbo.acc.pat_name, dbo.ins.ins_a_b_c


 */
using System;
//--- added
using Utilities;
//using System.Data.SqlClient; // SQL 7.0

namespace MCL;

/// <summary>
/// RCRecordset representation of the Fin table
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_fin : RCRecordset
{
    //===== string variables for fields in record  =====

    /// <summary>
    /// Financial Class -- varchar(10) not null
    /// </summary>
    public string m_strFinCode;

    /// <summary>
    /// Responsible Party -- varchar(40)
    /// </summary>
    public string m_strResParty;

    /// <summary>
    /// Type of billing form -- varchar(30)
    /// </summary>
    public string m_strFormType;

    /// <summary>
    /// Charge Source -- varchar(20)
    /// </summary>
    public string m_strChrgSource;

    /// <summary>
    /// Type -- varchar(1)
    /// </summary>
    public string m_strType;

    /// <summary>
    /// H1500 -- varchar(1)
    /// </summary>
    public string m_strH1500;

    /// <summary>
    /// UB92 -- varchar(1) 
    /// </summary>
    public string m_strUB;


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
    public R_fin(string strServer, string strDataBase, ref ERR errLog)
        : base(strServer, strDataBase, "fin", ref errLog)
    {

    }



    /// <summary>
    /// Generic GetRecords  - pass in where clause (without the WHERE key word).
    /// Can get active or deleted records!
    /// </summary>
    /// <returns>Record Count</returns>
    /// <param name="strWhere">Where clause to retrieve the records.</param>
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
    /// Generic GetActiveRecords  - pass in where clause (without the WHERE).
    /// Gets only ACTIVE records - ie NOT flagged deleted.
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
    /// </summary>
    /// <returns>false = error</returns>
    public bool MoveFirst()
    {
        m_CurrentRecordIndex = 1;
        return (LoadMemberVariablesFromDataSet());

    }

    /// <summary>
    /// Loads the 'field' member variables for the last record.
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

    ///// <summary>
    ///// Clears the deleted flag on the current record
    ///// </summary>
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
        // m_strRowguid = Guid.NewGuid().ToString();
        m_strModDate = Time.SNows();
        m_strModHost = OS.GetMachineName();
        m_strModPrg = OS.GetAppName();
        m_strModUser = OS.GetUserName(); // 07/20/2007 wdk added

        //--- table fields
        m_strFinCode = "";
        m_strResParty = "";
        m_strFormType = "";
        m_strChrgSource = "";
        m_strType = "";
        m_strH1500 = "";
        m_strUB = "";

    }

    /// <summary>
    /// Move data from dataset into this classes 'field' member variables.
    /// Note: 'generic' member functions expect this function to be named:
    ///             LoadMemberVariablesFromDataSet()
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
            m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
            {
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
            }
            //m_strModDate = Time.DateTimeToHL7TimeString(m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]);
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString(); ;
            //m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== end of 'standard' fields =====================

            m_strFinCode = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["fin_code"].ToString();
            m_strResParty = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["res_party"].ToString();
            m_strFormType = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["form_type"].ToString();
            m_strChrgSource = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["chrgsource"].ToString();
            m_strType = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["type"].ToString();
            m_strH1500 = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["h1500"].ToString();
            m_strUB = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ub92"].ToString();
            bRetVal = true;
        }

        return (bRetVal);
    }


    #region AddRecord and Update record not used
    /*
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
        //throw new System.NotImplementedException();
        int iRetVal = -1;
        string strSQL;

        //INSERT INTO MyTable (PriKey, Description)
        //   VALUES (123, 'A description of part 123.')
        //                                table fields   
        strSQL = string.Format("INSERT INTO {0}(deleted, icd9_num, icd9_desc, AMA_year, mod_prg) VALUES ('{1}','{2}','{3}','{4}','{5}')",
                                    propTable.ToString(), //0
                                     0,//1
                                      m_strIcd9_num,//2
                                       m_strIcd9_desc,//3
                                        m_strAMA_year,
                                         OS.GetAppName());//4                             
                                     


        iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            // add new record to dataset
            DataRow L_DataRow = m_DataSet.Tables[propTable.ToString()].NewRow();
            m_DataSet.Tables[propTable.ToString()].Rows.Add(L_DataRow);
            
            // index is zero based while record count is 1 based ie the correct record count
            m_CurrentRecordIndex = m_CurrentRecordCount++;
            
            m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] = 0; // new record so flag it NOT deleted
            m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_num"] = m_strIcd9_num;
            m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_desc"] = m_strIcd9_desc;
            m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["AMA_year"] = m_strAMA_year;
            
        }
        return iRetVal;
    }

    /// <summary>
    /// Updates the 'current' record.
    /// Rick Crone 04/12/2006
    /// </summary>
    /// <returns>number of records 
    /// OR 
    /// -1 = ERROR - see m_strErrMsg for details</returns>
    public int Update()
    {
        
        string strSQL;
        string strWhere;
        
        //strSQL = string.Format("UPDATE {0} SET {1} = '{2}' WHERE {3}",
        //    m_strTable, //* a property field in this class 
        //    strField,
        //    strNewValue,
        //    strFilter);
        

        
         * Set the where class for the KEY for this table from the DataSet values
         
        strWhere = string.Format("icd9_num = '{0}' and AMA_year = '{1}'",
                                    m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["icd9_num"].ToString(),
                                     m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["AMA_year"].ToString());

        strSQL = string.Format("UPDATE {0} SET deleted = '{1}', icd9_num = '{2}', icd9_desc = '{3}', AMA_year = '{4}', mod_prg = '{5}' WHERE {6}",
                                   propTable.ToString(), //0
                                    m_strDeleted == "T" ? 1:0,//1
                                     m_strIcd9_num,//2
                                      m_strIcd9_desc,//3
                                       m_strAMA_year, //4
                                        OS.GetAppName(),
                                        strWhere);                       
       

        return SQLExec(strSQL, out m_strErrMsg);
        
    }

*/
    #endregion AddRecord and Update record not used


    //==== 'special' functions just for this class below this line ======
    /// <summary>
    /// Gets ACTIVE fin_code record
    /// </summary>
    /// <returns></returns>
    public int GetRecord(string strFinCode)
    {
        //throw new System.NotImplementedException();
        string strWhere;                      // table
        strWhere = string.Format("fin_code = '{0}' and deleted = '0'",
                                     strFinCode);

        return (GetActiveRecords(strWhere));

    }



} // don't go below this line
