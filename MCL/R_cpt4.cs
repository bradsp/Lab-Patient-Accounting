using System;
using System.Data;  // for use with DataRow
//Programmer added usings
using Utilities;

namespace MCL;

/// <summary>
/// CPT4 Recordset based on RCRecordset
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_cpt4 : RCRecordset
{

    private string m_strLink;
    ///<summary>
    /// No Comment Available
    /// Field Type - int
    /// length - 4
    /// IsNullable - 0
    ///</summary>
    public string propLink
    {
        get { return m_strLink; }
        set
        {
            m_strLink = value.Substring(0, value.ToString().Length > 4 ? 4 : value.Length);
        }
    }

    private string m_strMprice;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propMprice
    {
        get { return m_strMprice; }
        set
        {
            m_strMprice = value.Substring(0, value.ToString().Length > 8 ? 8 : value.Length);
        }
    }

    private string m_strCprice;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propCprice
    {
        get { return m_strCprice; }
        set
        {
            m_strCprice = value.Substring(0, value.ToString().Length > 8 ? 8 : value.Length);
        }
    }

    private string m_strZprice;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propZprice
    {
        get { return m_strZprice; }
        set
        {
            m_strZprice = value.Substring(0, value.ToString().Length > 8 ? 8 : value.Length);
        }
    }

    private string m_strCost;
    ///<summary>
    /// wdk 20100709 Added for capturing the Test Cost
    /// Field Type - decimal
    /// length - 9
    /// IsNullable - 1
    ///</summary>
    public string propCost
    {
        get { return m_strCost; }
        set
        {
            m_strCost = value.Substring(0, value.ToString().Length > 9 ? 9 : value.Length);
        }
    }

    private string m_strCdm;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 7
    /// IsNullable - 0
    ///</summary>
    public string propCdm
    {
        get { return m_strCdm; }
        set
        {
            m_strCdm = value.Substring(0, value.ToString().Length > 7 ? 7 : value.Length);
        }
    }

    private string m_strCpt4;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 5
    /// IsNullable - 1
    ///</summary>
    public string propCpt4
    {
        get { return m_strCpt4; }
        set
        {
            m_strCpt4 = value.Substring(0, value.ToString().Length > 5 ? 5 : value.Length);
        }
    }

    private string m_strDescript;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 50
    /// IsNullable - 1
    ///</summary>
    public string propDescript
    {
        get { return m_strDescript; }
        set
        {
            m_strDescript = value.Substring(0, value.ToString().Length > 50 ? 50 : value.Length);
        }
    }

    private string m_strRev_code;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 4
    /// IsNullable - 1
    ///</summary>
    public string propRev_code
    {
        get { return m_strRev_code; }
        set
        {
            m_strRev_code = value.Substring(0, value.ToString().Length > 4 ? 4 : value.Length);
        }
    }

    private string m_strType;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 4
    /// IsNullable - 1
    ///</summary>
    public string propType
    {
        get { return m_strType; }
        set
        {
            m_strType = value.Substring(0, value.ToString().Length > 4 ? 4 : value.Length);
        }
    }

    private string m_strModi;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 2
    /// IsNullable - 1
    ///</summary>
    public string propModi
    {
        get { return m_strModi; }
        set
        {
            m_strModi = value.Substring(0, value.ToString().Length > 2 ? 2 : value.Length);
        }
    }

    private string m_strBillcode;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 7
    /// IsNullable - 1
    ///</summary>
    public string propBillcode
    {
        get { return m_strBillcode; }
        set
        {
            m_strBillcode = value.Substring(0, value.ToString().Length > 7 ? 7 : value.Length);
        }
    }

    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// CPT4 Recordset based on RCRecordset
    /// </summary>
    public R_cpt4(string strServer, string strDataBase, ref ERR errLog)
          : base(strServer, strDataBase, "cpt4", ref errLog)
    {
        m_strServer = strServer;
        m_strDataBase = strDataBase;
        // Error log set via  RCRecordset m_ERR base class call
    }

    /// <summary>
    /// Overloaded to pass the cpt4 table name to the constructor
    /// </summary>
    /// <param name="strServer"></param>
    /// <param name="strDataBase"></param>
    /// <param name="errLog"></param>
    /// <param name="strTable"></param>
    public R_cpt4(string strServer, string strDataBase, ref ERR errLog, string strTable)
        : base(strServer, strDataBase, strTable, ref errLog)
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
    /// Gets all active records for the strWhere
    ///</summary>
    ///<param name = "strWhere">filter for the recordset</param>
    ///<returns>the number of records for the filter</returns>
    public int GetActiveRecords(string strWhere)
    {
        strWhere = string.Format("deleted = 0 AND {0}", strWhere);
        return (GetRecords(strWhere));
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
    /// Flags the current record as deleted
    ///</summary>
    ///<returns>Returns the value from Update()</returns>
    public int FlagCurrentRecordDeleted()
    {
        m_strDeleted = "T";
        return (Update());
    }

    ///<summary>
    /// Flags the current record not deleted
    ///</summary>
    ///<returns>Returns the value from Update()</returns>
    public int FlagCurrentRecordNOTDeleted()
    {
        m_strDeleted = "F";
        return (Update());
    }


    ///<summary>
    ///Clears the member variables for the current instance
    ///</summary>
    public void ClearMemberVariables()
    {
        // Standard Fields
        m_strRowguid = Guid.NewGuid().ToString();
        m_strModDate = Time.SNows();
        m_strDeleted = "F";
        m_strModUser = OS.GetUserName();
        m_strModPrg = OS.GetAppName();
        m_strModHost = OS.GetMachineName();

        // Tables fields
        m_strLink = "";
        m_strMprice = "";
        m_strCprice = "";
        m_strZprice = "";
        m_strCost = "";
        m_strCdm = "";
        m_strCpt4 = "";
        m_strDescript = "";
        m_strRev_code = "";
        m_strType = "";
        m_strModi = "";
        m_strBillcode = "";
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

            m_strRowguid = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["rowguid"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"].Equals(System.DBNull.Value))
            {
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("G");
            }
            m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== End of "standard fields" =============

            m_strLink = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["link"].ToString();
            m_strMprice = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mprice"].ToString();
            m_strCprice = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cprice"].ToString();
            m_strZprice = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["zprice"].ToString();
            m_strCost = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cost"].ToString();
            m_strCdm = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cdm"].ToString();
            m_strCpt4 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cpt4"].ToString();
            m_strDescript = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["descript"].ToString();
            m_strRev_code = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rev_code"].ToString();
            m_strType = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["type"].ToString();
            m_strModi = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["modi"].ToString();
            m_strBillcode = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["billcode"].ToString();

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
        "link, mprice, cprice, zprice, " +
        "deleted, cost, cdm, cpt4, descript, " +
        "rev_code, type, modi, billcode )VALUES(" +
        "'{1}', '{2}', '{3}', '{4}', '{5}', " +
        "'{6}', '{7}', '{8}', '{9}', '{10}', " +
        "'{11}', '{12}', '{13}')",
        propTable,
    m_strLink, m_strMprice, m_strCprice, m_strZprice,
    m_strDeleted, m_strCost, m_strCdm, m_strCpt4, m_strDescript,
    m_strRev_code, m_strType, m_strModi, m_strBillcode);

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
    /// Updates the recordset with the application supplied values. Cannot update the primary key!
    /// </summary>
    /// <returns>Count of the records updated</returns>
    public int Update()
    {
        string strWhere;
        string strSQL;

        //Set the where clause for the KEY for this table from the DataSet values

        strWhere = string.Format("rowguid = '{0}'",
        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());
        strSQL = string.Format("UPDATE {0} SET " +
        "rowguid= '{1}', " +
        "link= '{2}', " +
        "mprice= '{3}', " +
        "cprice= '{4}', " +
        "zprice= '{5}', " +
        "deleted= '{6}', " +
        "cost= '{7}', " +
        "cdm= '{8}', " +
        "cpt4= '{9}', " +
        "descript= '{10}', " +
        "rev_code= '{11}', " +
        "type= '{12}', " +
        "modi= '{13}', " +
        "billcode= '{14}' " +
        "where {15}",
        propTable,
        m_strRowguid,
        m_strLink,
        m_strMprice,
        m_strCprice,
        m_strZprice,
        m_strDeleted == "F" ? 0 : 1,
        m_strCost,
        m_strCdm,
        m_strCpt4,
        m_strDescript,
        m_strRev_code,
        m_strType,
        m_strModi,
        m_strBillcode, strWhere);

        return SQLExec(strSQL, out m_strErrMsg);
    }
    //==== 'special' functions just for this class below this line ======
    /// <summary>
    /// Gets ACTIVE cpt4 records
    /// 04/12/07 Rick Crone
    /// </summary>
    /// <returns>number of records</returns>
    public int GetRecordsByCpt4(string strCpt4)
    {
        //throw new System.NotImplementedException();
        string strWhere;                      // table
        strWhere = string.Format("cpt4 = '{0}'",
                                     strCpt4);

        return (GetActiveRecords(strWhere));

    }

    /// <summary>
    /// Gets ACTIVE cpt4 records
    /// 04/12/07 Rick Crone
    /// </summary>
    /// <returns>number of records</returns>
    public int GetRecordsByCdm(string strCdm)
    {
        //throw new System.NotImplementedException();
        string strWhere;                      // table
        strWhere = string.Format("cdm = '{0}' order by cpt4",
                                     strCdm);
        GetActiveRecords(strWhere);
        return (GetRecordCount(strWhere));

    }

    /// <summary>
    /// Gets ACTIVE cpt4 records with/without the PC charges based on the boolean GetPcChrg
    /// 20121218 David
    /// </summary>
    /// <returns>number of records</returns>    
    public int GetRecordsByCdm(string strCdm, bool bIncludePcChrg)
    {
        //throw new System.NotImplementedException();
        string strWhere;                      // table
        strWhere = string.Format("cdm = '{0}' {1} order by cpt4",
            strCdm, bIncludePcChrg ? "" : "and type <> 'PC'");
        GetActiveRecords(strWhere);
        return (GetRecordCount(strWhere));

    }

    /// <summary>
    /// Resets the current record when done.
    /// Returns array elements
    ///         0 = Mprice
    ///         1 = Cprice
    ///         2 = Zprice
    /// </summary>
    /// <returns></returns>
    public double[] GetCdmPrice(ref DataSet dsAmt)
    {
        int nCurrent = m_CurrentRecordIndex;
        double[] dRetVal = new double[3];
        //MoveFirst();
        while (propErrMsg != "EOF")
        {
            dRetVal[0] += double.Parse(propMprice);
            dRetVal[1] += double.Parse(propCprice);
            dRetVal[2] += double.Parse(propZprice);
            dsAmt.Tables[0].Rows.Add(
                    null,
                        propCpt4,
                            propType,
                                dRetVal[0].ToString(),
                                    propModi,
                                        propRev_code,
                                            null,
                                                null);


            GetNext();

        }
        m_CurrentRecordIndex = nCurrent;
        LoadMemberVariablesFromDataSet();
        return dRetVal;

    }




} // do not go below this line

