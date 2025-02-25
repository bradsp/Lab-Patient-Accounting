using System;
//Programmer added usings
using Utilities;

namespace MCL;

/// <summary>
/// ACC_MERGES Recordset based on RCRecordset
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_acc_merges : RCRecordset
{

    private string m_strAccount;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 0
    ///</summary>
    public string propAccount
    {
        get { return m_strAccount; }
        set
        {
            m_strAccount = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strDup_acc;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 0
    ///</summary>
    public string propDup_acc
    {
        get { return m_strDup_acc; }
        set
        {
            m_strDup_acc = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// ACC_MERGES Recordset based on RCRecordset
    /// </summary>
    public R_acc_merges(string strServer, string strDataBase, ref ERR errLog)
          : base(strServer, strDataBase, "acc_merges", ref errLog)
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
        m_strErrMsg = string.Format("{0} Records(s) read", m_CurrentRecordCount);
        if (m_CurrentRecordCount > 0)
        {
            LoadMemberVariablesFromDataSet();
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

    ///<summary>
    ///Clears the member variables for the current instance
    ///</summary>
    public void ClearMemberVariables()
    {
        // Standard Fields
        m_strModDate = Time.SNows();
        m_strModPrg = OS.GetAppName();
        m_strModUser = OS.GetUserName();
        m_strModHost = OS.GetMachineName();

        // Tables fields
        m_strAccount = "";
        m_strDup_acc = "";
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

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
            {
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
            }
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== End of "standard fields" =============

            m_strAccount = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"].ToString();
            m_strDup_acc = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["dup_acc"].ToString();

            // Return true! we were successful
            bRetVal = true;

        }
        return (bRetVal);
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
    /// <param name = "strWhere">Filter for the records required</param>
    /// </summary>
    public int AddRecord(string strWhere)
    {
        int iRetVal = -1;
        string strSQL;

        strSQL = string.Format("INSERT INTO {0} (" +
        "account, " +
        "dup_acc, mod_prg" +
        ")VALUES(" +
        "'{1}', " +
        "'{2}', '{3}')",
        propTable,
        m_strAccount,
        m_strDup_acc, m_strModPrg);

        iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            //Add new Record to dataset
            int nRetVal = Querry(strWhere);
            LoadMemberVariablesFromDataSet();
            return nRetVal;
        }
        return iRetVal;
    }

    /// <summary>
    /// This recordset cannot be updated it is add only.
    /// </summary>
    /// <returns>Count of the records updated</returns>
    public int Update()
    {
        return -1; // we only 
                   //string strWhere;
                   //string strSQL;

        ////Set the where clause for the KEY for this table from the DataSet values

        //strWhere = string.Format("rowguid = '{0}'",
        //m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());
        //strSQL = string.Format("UPDATE {0} SET " +
        //"mod_date= '{1}', "+
        //"account= '{2}', "+
        //"dup_acc= '{3}', "+
        //"mod_prg= '{4}', "+
        //"mod_user= '{5}', "+
        //"mod_host= '{6}', "+
        //"where {7}",
        //propTable,
        //m_straccount,
        //m_strDup_acc
        //);

        //return SQLExec(strSQL, out m_strErrMsg);
    }

} // do not go below this line

