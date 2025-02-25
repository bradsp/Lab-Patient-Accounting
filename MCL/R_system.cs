/* R_system Created 07/02/2008 
 * PURPOSE 
 *  * Retrieve values from the dbo.system table in MCLLIVE or MCLTEST. 
 *  * Developed for use in the ISA segment of the Claim837 Class
 *  
 *  * Notes
 *      1. This class was created using the SQL Stored Procedure [dbo].[uspCSharpe_Make_Class] (in MCLTEST on MCLBILL)
 *      2. Read Only class
 * 
 * 
*/
using System;
using System.Data;  // for use with DataRow
//Programmer added usings
using Utilities;

namespace MCL;

/// <summary>
/// SYSTEM Recordset based on RCRecordset
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_system : RCRecordset
{
    private string m_strKeyName;
    ///<summary>
    /// Returns the key name from the table.
    ///</summary>
    public string propKeyName
    {
        get { return m_strKeyName; }
        //set {
        //        m_strKeyName = value.Substring(0,value.ToString().Length >25?25:value.Length);
        //    }
    }

    private string m_strValue;
    ///<summary>
    /// Returns the value for the key in the table
    ///</summary>
    public string propValue
    {
        get { return m_strValue; }
        //set {
        //        m_strValue = value.Substring(0,value.ToString().Length >80?80:value.Length);
        //    }
    }

    private string m_strComment;
    ///<summary>
    /// Added 09/19/2008 to track changes to these values as WHY?
    ///</summary>
    public string propComment
    {
        get { return m_strComment; }
        set
        {
            m_strComment = value.Substring(0, value.ToString().Length > 1024 ? 1024 : value.Length);
        }
    }

    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// SYSTEM Recordset based on RCRecordset
    /// </summary>
    public R_system(string strServer, string strDataBase, ref ERR errLog)
          : base(strServer, strDataBase, "system", ref errLog)
    {
        m_strServer = strServer;
        m_strDataBase = strDataBase;
        // Error log set via  RCRecordset m_ERR base class call
    }

    ///<summary>
    /// Gets all records for the strWhere
    ///</summary>
    ///<param name = "strWhere">filter for the recordset</param>
    ///<returns>the number of records for the filter</returns>
    public int GetRecords(string strWhere)
    {
        Querry(strWhere);
        if (m_CurrentRecordCount > 0)
        {
            LoadMemberVariablesFromDataSet();
            m_strErrMsg = string.Format("{0} Records(s) read", m_CurrentRecordCount);
        }
        return (m_CurrentRecordCount);
    }

    ///<summary>
    /// Gets next record from the recordset
    /// Sets the Recordsets m_strErrMsg to "EOF" when there are no more records in the recordset
    ///</summary>
    ///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>
    public bool GetNext()
    {
        bool bRetVal = false;
        if (m_CurrentRecordIndex < (m_CurrentRecordCount - 1))
        {
            m_CurrentRecordIndex++;
            return (LoadMemberVariablesFromDataSet());
        }
        m_strErrMsg = "EOF";
        return (bRetVal);
    }

    ///<summary>
    /// Gets previous record from the recordset
    /// Sets the Recordsets m_strErrMsg to "BOF" when there are no more records in the recordset
    ///</summary>
    ///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>
    public bool GetPrev()
    {
        bool bRetVal = false;
        if (m_CurrentRecordIndex > 1)
        {
            m_CurrentRecordIndex--;
            return (LoadMemberVariablesFromDataSet());
        }
        m_strErrMsg = "BOF";
        return (bRetVal);
    }
    ///<summary>
    /// Sets the records index to the first record in the recordset
    /// Sets the Recordsets m_strErrMsg to "BOF"
    ///</summary>
    ///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns> 
    public bool MoveFirst()
    {
        m_CurrentRecordIndex = 1;
        return (LoadMemberVariablesFromDataSet());
    }
    ///<summary>
    /// Sets the records index to the last record in the recordset
    /// Sets the Recordsets m_strErrMsg to "EOF"
    ///</summary>
    ///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>
    public bool MoveLast()
    {
        m_CurrentRecordIndex = m_CurrentRecordCount;
        return (LoadMemberVariablesFromDataSet());
    }

    /// <summary>
    /// Clears the member variables before reloading.
    /// </summary>
    public void ClearMemberVariables()
    {
        // Standard Fields
        //m_strDeleted = "F";
        //m_strRowguid = Guid.NewGuid().ToString();
        m_strModDate = Time.SNows();
        m_strModHost = OS.GetMachineName();
        m_strModPrg = OS.GetAppName();
        m_strModUser = OS.GetUserName();

        // Tables fields
        m_strValue = "";
        m_strKeyName = "";
        m_strComment = "";

    }

    /// <summary>    
    /// Loads the member variables for use add the additional fields    
    /// Clears the variables before loading.    
    /// </summary>    
    /// <returns>Number of records update or -1 for Error. m_strErrMsg has details of error</returns>    
    public bool LoadMemberVariablesFromDataSet()
    {
        ClearMemberVariables();
        bool bRetVal = false;
        if (m_CurrentRecordIndex > -1) // do not attempt to load if there are no records
        {
            // ==== "standard" fields =====================
            //   m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();
            //    m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
            {
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
            }
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== End of "standard fields" =============
            m_strValue = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["value"].ToString();
            m_strKeyName = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["key_name"].ToString();
            m_strComment = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["comment"].ToString();

            // returns true we were successful 
            bRetVal = true;
        }
        return (bRetVal);
    }
    /// <summary>
    /// Specialized function to return the value for the key passed in
    /// </summary>
    /// <param name="strKeyName"></param>
    /// <returns></returns>
    public string GetValue(string strKeyName)
    {
        GetRecords(string.Format("key_name = '{0}'", strKeyName));
        return m_strValue;
    }

    /// <summary>     
    /// Returns the number of records in the DataSet for the strWhere passed.    
    /// if you expect only one record (the currently added record) you can check    
    ///     the return value to see if only one record exists.  
    ///     i.e. only one insurance record for ins_a_b_c of "A" should exist for any patient. More records means an erorr has happened.    
    ///
    /// This allows the RecordSet instance to scroll through the records as necessary.    
    ///
    /// This is the new methodology for AddRecord   
    /// This function will load the DataSet    
    /// The first record will be the last added record when the strWhere contains and order by mod_date   
    /// </summary>
    [Obsolete("This class is read only as of 20090626 wdk")]
    public int AddRecord()
    {
        throw new NotImplementedException("07/02/2008 Read only class");
#pragma warning disable 0162
        int iRetVal = -1;
        string strSQL;

        throw new NotImplementedException();
        strSQL = string.Format("INSERT INTO {0}" +
        "(FIELD LIST HERE)" +
        "VALUES (VALUE LIST HERE)",
        propTable,
        "MEMBER VARIABLES HERE");
        iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            //Add new Record to dataset
            DataRow L_DataRow = m_DataSet.Tables[propTable].NewRow();
            m_DataSet.Tables[propTable].Rows.Add(L_DataRow);
            // Index is zero based while record count is 1 based  ie. the correct recourd count
            m_CurrentRecordIndex = m_CurrentRecordCount++;
            // if unarchiving a record flagged to deleted have to covert back to the live system as deleted
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "F" ? 0 : 1;
            //    m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["FIELD LIST"] = m_strVARIABLE_NAME;
            // use the above line to set the properties of the data set for the new row.
        }
        return iRetVal;
#pragma warning restore 0162
    }


    /// <summary>
    /// Updates the recordset with the application supplied values. Cannot update the primary key!
    /// </summary>
    /// <returns>Count of the records updated</returns>
    [Obsolete("This class is read only as of 20090626 wdk")]
    public int Update()
    {
        throw new NotImplementedException("07/02/2008 Read only class");
#pragma warning disable 0162
        string strWhere;
        string strSQL;

        //Set the where clause for the KEY for this table from the DataSet values

        strWhere = string.Format("rowguid = '{0}'",
                   m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());
        throw new NotImplementedException();
        strSQL = string.Format("UPDATE {0} SET " +
        "(FIELD LIST HERE)" +
        "VALUES (VALUE LIST HERE)",
        propTable,
        "MEMBER VARIABLES HERE");

        return SQLExec(strSQL, out m_strErrMsg);
#pragma warning restore 0162
    }

} // do not go below this line

