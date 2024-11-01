using System;
//Programmer added usings
using Utilities;

namespace MCL;

/// <summary>
/// CHRG Recordset based on RCRecordset
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_chrg : RCRecordset
{
    private string m_strWhere; // wdk 20110114 added for multi record support when updating records.
    private string m_strFinCode;
    /// <summary>
    /// wdk 20110114 added
    /// </summary>
    public string propFinCode
    {
        get { return m_strFinCode; }
        set
        {
            m_strFinCode = value.Substring(0, value.Length > 10 ? 10 : value.Length);
        }
    }

    private string m_strPostDate;
    /// <summary>
    /// wdk 20110114 added
    /// </summary>
    public string propPostDate
    {
        get { return m_strPostDate; }
        set
        {
            m_strPostDate = value;
        }
    }
    private int m_nDetailRecCount;
    /// <summary>
    /// 
    /// </summary>
    public int propDetailRecCount
    {
        get { return m_nDetailRecCount; }
    }
    private string m_strRetail;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propRetail
    {
        get { return m_strRetail; }
        set
        {
            m_strRetail = string.Format("{0:2}", value);
        }
    }

    private string m_strInp_price;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propInp_price
    {
        get { return m_strInp_price; }
        set
        {
            m_strInp_price = string.Format("{0:2}", value);
        }
    }

    private string m_strNet_amt;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propNet_amt
    {
        get { return m_strNet_amt; }
        set
        {
            m_strNet_amt = string.Format("{0:2}", value);
        }
    }

    private string m_strService_date;
    ///<summary>
    /// No Comment Available
    /// Field Type - datetime
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propService_date
    {
        get { return m_strService_date; }
        set
        {
            m_strService_date = value.ToString();
        }
    }

    private string m_strHist_date;
    ///<summary>
    /// No Comment Available
    /// Field Type - datetime
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propHist_date
    {
        get { return m_strHist_date; }
        set
        {
            m_strHist_date = value.ToString();
        }
    }

    private string m_strCredited;
    ///<summary>
    /// No Comment Available
    /// Field Type - bit
    /// length - 1
    /// IsNullable - 0
    ///</summary>
    public string propCredited
    {
        get { return m_strCredited; }
        set
        {
            m_strCredited = value.Substring(0, value.ToString().Length > 1 ? 1 : value.Length);
        }
    }

    private string m_strChrg_num;
    ///<summary>
    /// No Comment Available
    /// Field Type - numeric
    /// length - 9
    /// IsNullable - 0
    /// 
    /// Get the charge number only
    ///</summary>
    public string propChrg_num
    {
        get { return m_strChrg_num; }
        //set {
        //        m_strChrg_num = value.ToString();
        //    }
    }

    private string m_strQty;
    ///<summary>
    /// No Comment Available
    /// Field Type - numeric
    /// length - 5
    /// IsNullable - 1
    ///</summary>
    public string propQty
    {
        get { return m_strQty; }
        set
        {
            m_strQty = value.ToString();
        }
    }

    private string m_strAccount;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 1
    ///</summary>
    public string propAccount
    {
        get { return m_strAccount; }
        set
        {
            m_strAccount = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strStatus;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 1
    ///</summary>
    public string propStatus
    {
        get { return m_strStatus; }
        set
        {
            m_strStatus = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strCdm;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 7
    /// IsNullable - 1
    ///</summary>
    public string propCdm
    {
        get { return m_strCdm; }
        set
        {
            m_strCdm = value.Substring(0, value.ToString().Length > 7 ? 7 : value.Length);
        }
    }

    private string m_strComment;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 50
    /// IsNullable - 1
    ///</summary>
    public string propComment
    {
        get { return m_strComment; }
        set
        {
            m_strComment = value.Substring(0, value.ToString().Length > 50 ? 50 : value.Length);
        }
    }

    private string m_strInvoice;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 1
    ///</summary>
    public string propInvoice
    {
        get { return m_strInvoice; }
        set
        {
            m_strInvoice = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strFin_type;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 1
    /// IsNullable - 1
    ///</summary>
    public string propFin_type
    {
        get { return m_strFin_type; }
        set
        {
            m_strFin_type = value.Substring(0, value.ToString().Length > 1 ? 1 : value.Length);
        }
    }

    private string m_strMt_req_no;
    ///<summary>
    /// rgc/wdk 20090930 added for tracking charges that should be credited from PostChrg
    /// Field Type - varchar
    /// length - 50
    /// IsNullable - 1
    ///</summary>
    public string propMt_req_no
    {
        get { return m_strMt_req_no; }
        set
        {
            m_strMt_req_no = value.Substring(0, value.ToString().Length > 50 ? 50 : value.Length);
        }
    }

    /// <summary>
    /// added recordset handler for the amount table
    /// </summary>
    public R_amt m_rsAmt;
    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// CHRG Recordset based on RCRecordset
    /// </summary>
    public R_chrg(string strServer, string strDataBase, ref ERR errLog)
          : base(strServer, strDataBase, "chrg", ref errLog)
    {
        m_strServer = strServer;
        m_strDataBase = strDataBase;
        m_rsAmt = new R_amt(m_strServer, m_strDataBase, ref errLog);
        // Error log set via  RCRecordset m_ERR base class call
    }

    ///<summary>
    /// Gets all records for the strWhere
    ///</summary>
    ///<param name = "strWhere">filter for the recordset</param>
    ///<returns>the number of records for the filter</returns>
    public int GetRecords(string strWhere)
    {
        strWhere += " and (not status in ('cbill','cap','n/a'))";
        Querry(strWhere);

        m_strWhere = strWhere;
        m_strErrMsg = string.Format("{0} Records(s) read", m_CurrentRecordCount);
        if (m_CurrentRecordCount > 0)
        {
            LoadMemberVariablesFromDataSet();

        }
        return (m_CurrentRecordCount);
    }

    /// <summary>
    /// Adds credited = 0 to the filter 
    /// </summary>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    public int GetNonCreditedRecords(string strWhere)
    {
        strWhere = string.Format("credited = 0 and {0}", strWhere);
        Querry(strWhere);
        m_strWhere = strWhere;
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
        m_CurrentRecordIndex = 0;
        return (LoadMemberVariablesFromDataSet());
    }

    ///<summary>
    /// Sets the records index to the last record in the recordset
    /// Sets the Recordsets m_strErrMsg to "EOF"
    ///</summary>
    ///<returns>the recordsets ability to LoadMemberVariablesFromDataSet()</returns>
    public bool MoveLast()
    {
        m_CurrentRecordIndex = m_CurrentRecordCount - 1;
        return (LoadMemberVariablesFromDataSet());
    }

    ///<summary>
    ///Clears the member variables for the current instance
    ///</summary>
    public void ClearMemberVariables()
    {
        // Standard Fields
        m_strRowguid = Guid.NewGuid().ToString();
        m_strModDate = Time.SNows();
        m_strModUser = OS.GetUserName();
        m_strModPrg = OS.GetAppName();
        m_strModHost = OS.GetMachineName();

        // Tables fields
        m_strRetail = "";
        m_strInp_price = "";
        m_strNet_amt = "";
        m_strService_date = "";
        m_strHist_date = "";
        m_strCredited = "";
        m_strChrg_num = "";
        m_strQty = "";
        m_strAccount = "";
        m_strStatus = "";
        m_strCdm = "";
        m_strComment = "";
        m_strInvoice = "";
        m_strFin_type = "";
        m_strMt_req_no = "";
        m_rsAmt.ClearMemberVariables();
        // wdk 20110114 added
        m_strPostDate = "";
        m_strFinCode = "";
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
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("d");
            }
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== End of "standard fields" =============

            m_strRetail = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["retail"].ToString();
            m_strInp_price = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["inp_price"].ToString();
            m_strNet_amt = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["net_amt"].ToString();
            m_strCredited = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["credited"].ToString();
            m_strChrg_num = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["chrg_num"].ToString();
            m_strQty = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["qty"].ToString();
            m_strAccount = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"].ToString();
            m_strStatus = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["status"].ToString();
            m_strCdm = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cdm"].ToString();
            m_strComment = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["comment"].ToString();
            m_strInvoice = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["invoice"].ToString();
            m_strFin_type = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["fin_type"].ToString();
            m_strMt_req_no = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mt_req_no"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["service_date"].Equals(System.DBNull.Value))
            {
                m_strService_date = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["service_date"]).ToString("G");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["hist_date"].Equals(System.DBNull.Value))
            {
                m_strHist_date = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["hist_date"]).ToString("G");
            }
            m_strFinCode = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["fin_code"].ToString();

            // wdk 20110114 added
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["post_date"].Equals(System.DBNull.Value))
            {
                m_strPostDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["post_date"]).ToString("g");
            }

            m_nDetailRecCount = m_rsAmt.GetActiveRecords(string.Format("chrg_num = '{0}'", propChrg_num));
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
        "retail, " +
        "inp_price, " +
        "net_amt, " +
        "service_date, " +
        "hist_date, " +
        "credited, " +
        "qty, " +
        "account, " +
        "status, " +
        "cdm, " +
        "comment, " +
        "invoice, " +
        "fin_type, " +
        "mt_req_no, post_date, fin_code, mod_prg, rowguid)VALUES(" +
        "{1}, " + // retail
        "{2}, " + // inp_price
        "{3}, " + //net amt
        "{4}, " + // service_date
        "'{5}', " + // hist_date
        "'{6}', " + //credited
        "'{7}', " + // qty
        "'{8}', " + // account
        "'{9}', " + //status
        "'{10}', " + //cdm
        "'{11}', " + //comment
        "{12}, " + //invoice
        "'{13}', " + // fin_type
        "'{14}',{15}, '{16}','{17}','{18}')",
        propTable,
        string.IsNullOrEmpty(m_strRetail) ? "NULL" : m_strRetail,
    string.IsNullOrEmpty(m_strInp_price) ? "NULL" : m_strInp_price,
    string.IsNullOrEmpty(m_strNet_amt) ? "NULL" : m_strNet_amt,
    string.IsNullOrEmpty(m_strService_date) ? "NULL" : string.Format("'{0}'", m_strService_date), //m_strService_date,
        string.IsNullOrEmpty(m_strHist_date) ? DateTime.Now.ToString("G") : m_strHist_date,
        m_strCredited,
        m_strQty,
        m_strAccount,
        m_strStatus,
        m_strCdm,
        m_strComment,
        string.IsNullOrEmpty(m_strInvoice) ? "NULL" : string.Format("'{0}'", m_strInvoice),
        m_strFin_type,
        m_strMt_req_no,
    string.IsNullOrEmpty(m_strPostDate) ? "NULL" : string.Format("'{0}'", m_strPostDate),
    m_strFinCode, m_strModPrg,
    m_strRowguid);

        iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            //Add new Record to dataset
            //strWhere = string.Format("chrg_num = '{0}'",
            //    m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["chrg_num"].ToString());
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
        "retail= '{1}', " +
        "inp_price= '{2}', " +
        "net_amt= '{3}', " +
        "service_date= {4}, " +
        "hist_date= {5}, " +
        "credited= '{6}', " +
        "qty= '{7}', " +
        "account= '{8}', " +
        "status= '{9}', " +
        "cdm= '{10}', " +
        "comment= '{11}', " +
        "invoice= {12}, " +
        "fin_type= '{13}', " +
        "mt_req_no= '{14}', post_date = {15}, fin_code = '{16}'" +
        "where {17}",
        propTable,
        m_strRetail,
        m_strInp_price,
        m_strNet_amt,
        string.IsNullOrEmpty(m_strService_date) ? "NULL" : string.Format("'{0}'", m_strService_date),
    string.IsNullOrEmpty(m_strHist_date) ? "NULL" : string.Format("'{0}'", m_strHist_date),
        m_strCredited,
        m_strQty,
        m_strAccount,
        m_strStatus,
        m_strCdm,
        m_strComment,
        string.IsNullOrEmpty(m_strInvoice) ? "NULL" : string.Format("'{0}'", m_strInvoice),
        m_strFin_type,
        m_strMt_req_no,
    string.IsNullOrEmpty(m_strPostDate) ? "NULL" : string.Format("'{0}'", m_strPostDate),
    m_strFinCode, strWhere);

        //		return SQLExec(strSQL, out m_strErrMsg);
        int iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            //Add new Record to dataset
            int nRetVal = Querry(m_strWhere);
            LoadMemberVariablesFromDataSet();
            return nRetVal;
        }
        return iRetVal;

    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public decimal GetNetAmt()
    {
        return decimal.Parse(propNet_amt) * decimal.Parse(propQty);
    }

    /// <summary>
    /// rgc/wdk 20110601 added for posting of charges.
    /// </summary>
    /// <param name="strPostDate">the close of month date</param>
    /// <returns>number of records updated</returns>
    public int UpdatePostDate(string strPostDate)
    {
        string strWhere;
        string strSQL;

        //Set the where clause for the KEY for this table from the DataSet values

        strWhere = string.Format("rowguid = '{0}'",
        m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());
        strSQL = string.Format("UPDATE {0} SET " +
        " post_date = '{1}'" +
        "where {2}",
        propTable, strPostDate, strWhere);

        //		return SQLExec(strSQL, out m_strErrMsg);
        int iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            //Add new Record to dataset
            int nRetVal = Querry(m_strWhere);
            LoadMemberVariablesFromDataSet();
            return nRetVal;
        }
        return iRetVal;

    }

} // R_Amt records set below here

/// <summary>
/// AMT Recordset based on RCRecordset
/// Should only be called by the R_chrg Recordset because this table contains
/// no quantity or credited field. 
/// </summary>
public class R_amt : RCRecordset
{
    // wdk 20140418 added
    private string m_strPointerSet;
    /// <summary>
    /// Pointer to the recordset
    /// </summary>
    public string propPointerSet
    {
        get { return m_strPointerSet; }
        set { m_strPointerSet = value; }
    }
    // wdk 20130808 added bill_method to amt table
    private string m_strBillMethod;
    /// <summary>
    /// wdk 20130808 added bill_method to amt table
    /// Should be the clients mnemonic to put on a "CBILL" or "INS"
    /// </summary>
    public string propBillMethod
    {
        get { return m_strBillMethod; }
        set { m_strBillMethod = value; }
    }

    private string m_strAmount;
    ///<summary>
    /// No Comment Available
    /// Field Type - money
    /// length - 8
    /// IsNullable - 1
    ///</summary>
    public string propAmount
    {
        get { return m_strAmount; }
        set
        {
            m_strAmount = string.Format("{0:2}", value);
        }
    }

    private string m_strChrg_num;
    ///<summary>
    /// No Comment Available
    /// Field Type - numeric
    /// length - 9
    /// IsNullable - 0
    ///</summary>
    public string propChrg_num
    {
        get { return m_strChrg_num; }
        set
        {
            m_strChrg_num = value.ToString();
        }
    }

    private string m_strUri;
    ///<summary>
    /// No Comment Available
    /// Field Type - numeric
    /// length - 9
    /// IsNullable - 0
    /// Cannot set the URI in the table it is read only.
    ///</summary>
    public string propUri
    {
        get { return m_strUri; }
        //set {
        //        m_strUri = value.Substring(0,value.ToString().Length >9?9:value.Length);
        //    }
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
            if (value != null)
            {
                m_strCpt4 = value.Substring(0, value.ToString().Length > 5 ? 5 : value.Length);
            }
            else
            {
                m_strCpt4 = "N/A";
            }
        }
    }



    private string m_strType;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 6
    /// IsNullable - 1
    ///</summary>
    public string propType
    {
        get { return m_strType; }
        set
        {
            m_strType = value.Substring(0, value.ToString().Length > 6 ? 6 : value.Length);
        }
    }

    private string m_strModi;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 5
    /// IsNullable - 1
    ///</summary>
    public string propModi
    {
        get { return m_strModi; }
        set
        {
            m_strModi = value.Substring(0, value.ToString().Length > 5 ? 5 : value.Length);
        }
    }

    private string m_strRevcode;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 5
    /// IsNullable - 1
    ///</summary>
    public string propRevcode
    {
        get { return m_strRevcode; }
        set
        {
            m_strRevcode = value.Substring(0, value.ToString().Length > 5 ? 5 : value.Length);
        }
    }

    private string m_strModi2;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 5
    /// IsNullable - 1
    ///</summary>
    public string propModi2
    {
        get { return m_strModi2; }
        set
        {
            m_strModi2 = value.Substring(0, value.ToString().Length > 5 ? 5 : value.Length);
        }
    }

    private string m_strDiagnosis_code_ptr;
    ///<summary>
    /// 20090119 wdk shows relation between cpt4 and diagnosis
    /// Field Type - varchar
    /// length - 7
    /// IsNullable - 1
    ///</summary>
    public string propDiagnosis_code_ptr
    {
        get { return m_strDiagnosis_code_ptr; }
        set
        {
            m_strDiagnosis_code_ptr = value.Substring(0, value.ToString().Length > 7 ? 7 : value.Length);
        }
    }


    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// AMT Recordset based on RCRecordset
    /// </summary>
    //[DataSysDescription("THIS CLASS SHOULD ONLY BE CALLED FROM R_CHRG. No qty or credited flag available here.")]
    public R_amt(string strServer, string strDataBase, ref ERR errLog)
        : base(strServer, strDataBase, "amt", ref errLog)
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
        m_strModDate = Time.SNows();
        m_strDeleted = "F";
        m_strModUser = OS.GetUserName();
        m_strModPrg = OS.GetAppName();

        // Tables fields
        m_strAmount = "";
        m_strChrg_num = "";
        m_strUri = "";
        m_strCpt4 = "";
        m_strType = "";
        m_strModi = "";
        m_strRevcode = "";
        m_strModi2 = "";
        m_strDiagnosis_code_ptr = "";
        m_strBillMethod = ""; // wdk 20130809 added
        m_strPointerSet = "F";


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
            m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            // ==== End of "standard fields" =============

            m_strAmount = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["amount"].ToString();
            m_strChrg_num = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["chrg_num"].ToString();
            m_strUri = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["uri"].ToString();
            m_strCpt4 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cpt4"].ToString();
            m_strType = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["type"].ToString();
            m_strModi = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["modi"].ToString();
            m_strRevcode = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["revcode"].ToString();
            m_strModi2 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["modi2"].ToString();
            m_strDiagnosis_code_ptr = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["diagnosis_code_ptr"].ToString();
            m_strDiagnosis_code_ptr = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["bill_method"].ToString();
            m_strPointerSet = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["pointer_set"] ? "T" : "F";


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
        "amount, " +
        "deleted, " +
        "chrg_num, " +
        "cpt4, " +
        "type, " +
        "modi, " +
        "revcode, " +
        "modi2, " +
        "diagnosis_code_ptr, mod_prg, bill_method,pointer_set )VALUES(" +
        "'{1}', " +
        "'{2}', " +
        "'{3}', " +
        "'{4}', " +
        "{5}, " +
        "{6}, " +
        "{7}, " +
        "{8}, " +
        "{9},'{10}','{11}','{12}')",
        propTable, //0
        m_strAmount, //1
        m_strDeleted == "F" ? 0 : 1, //2
        m_strChrg_num, //3
        string.IsNullOrEmpty(m_strCpt4) ? "NA" : m_strCpt4, //4
        string.IsNullOrEmpty(m_strType) ? "NULL" : string.Format("'{0}'", m_strType), //5
        string.IsNullOrEmpty(m_strModi) ? "NULL" : string.Format("'{0}'", m_strModi),
        string.IsNullOrEmpty(m_strRevcode) ? "NULL" : string.Format("'{0}'", m_strRevcode),
        string.IsNullOrEmpty(m_strModi2) ? "NULL" : string.Format("'{0}'", m_strModi2),
        string.IsNullOrEmpty(m_strDiagnosis_code_ptr) ? "NULL" : string.Format("'{0}'", m_strDiagnosis_code_ptr),
    m_strModPrg, m_strBillMethod
    , m_strPointerSet == "F" ? 0 : 1);

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
        "amount= '{1}', " +
        "deleted= '{2}', " +
        "chrg_num= '{3}', " +
        "cpt4= '{4}', " +
        "type= '{5}', " +
        "modi= '{6}', " +
        "revcode= '{7}', " +
        "modi2= '{8}', " +
        "diagnosis_code_ptr= '{9}', " +
    "bill_method = '{10}', " +
    "pointer_set = '{12}' " +
        "where {11}",
        propTable,
        m_strAmount,
        m_strDeleted == "F" ? 0 : 1,
        m_strChrg_num,
        m_strCpt4,
        m_strType,
        m_strModi,
        m_strRevcode,
        m_strModi2,
        m_strDiagnosis_code_ptr,
    m_strBillMethod, strWhere
    , m_strPointerSet == "F" ? 0 : 1);

        //return SQLExec(strSQL, out m_strErrMsg);
        int iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {
            //Add new Record to dataset
            int nRetVal = Querry(strWhere);
            LoadMemberVariablesFromDataSet();
            return nRetVal;
        }
        return iRetVal;
    }





} // do not go below this line

