using System;
//Programmer added usings
using Utilities;

namespace MCL;

/// <summary>
/// INS Recordset based on RCRecordset
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_ins : RCRecordset
{
    private string m_strHolderAddr;
    /// <summary>
    /// wdk 20110926 added for SSI reconfiguration
    /// </summary>
    public string propHolderAddr
    {
        get { return m_strHolderAddr; }
        set
        {
            m_strHolderAddr = value.Substring(0, value.ToString().Length > 40 ? 40 : value.Length);
        }
    }
    private string m_strHolderCity;
    /// <summary>
    /// wdk 20110926 Sets the holders city in the propHolderCityStZip
    /// </summary>
    public string propHolderCity
    {
        set
        {
            m_strHolderCity = value.Substring(0, value.ToString().Length > 27 ? 27 : value.Length);
        }
    }
    private string m_strHolderState;
    /// <summary>
    /// wdk 20110926 Sets the holders city in the propHolderCityStZip
    /// </summary>
    public string propHolderState
    {
        set
        {
            m_strHolderState = value.Substring(0, value.Length > 2 ? 2 : value.Length);
        }
    }

    private string m_strHolderZip;
    /// <summary>
    /// wdk 20110926 Sets the holders city in the propHolderCityStZip
    /// </summary>
    public string propHolderZip
    {
        set
        {
            value = value.Replace("-", "");
            m_strHolderZip = value.Substring(0, value.Length > 9 ? 9 : value.Length);
        }
    }

    private string m_strHolderCityStZip;
    /// <summary>
    /// wdk 20110926
    /// Must set the propHolderCity, propHolderState and propHolderZip to use this variable
    /// the database call loads the three variables from this string.
    /// </summary>
    public string propHolderCityStZip
    {
        set
        {
            m_strHolderCityStZip = value.Trim();
            if (string.IsNullOrEmpty(m_strHolderCityStZip))
            {
                m_strHolderCity = "";
                m_strHolderState = "";
                m_strHolderZip = "";
                return;
            }
            if (m_strHolderCityStZip.Contains(", "))
            {
                propHolderCity = value.Split(new string[] { ", " }, StringSplitOptions.None)[0].Trim();
                string strCityZip = value.Split(new string[] { ", " }, StringSplitOptions.None)[1].Trim();
                if (strCityZip.Contains(" "))
                {
                    m_strHolderState = strCityZip.Split(new string[] { " " }, StringSplitOptions.None)[0].Trim();
                    m_strHolderZip = strCityZip.Split(new string[] { " " }, StringSplitOptions.None)[1].Trim().Replace("-", "");
                }
            }
        }
        get
        {
            string strRetVal = string.Format("{0}", m_strHolderCity.Trim());

            if (string.IsNullOrEmpty(strRetVal))
            {
                return strRetVal;
            }
            if ((m_strHolderState.Length + m_strHolderZip.Length + m_strHolderCity.Length) > 40)
            {
                m_strHolderCity = m_strHolderCity.Substring(0, (39 - (m_strHolderState.Length + m_strHolderZip.Length)));
            }
            strRetVal = string.Format("{0},{1} {2}", m_strHolderCity, m_strHolderState, m_strHolderZip);
            return strRetVal;
        }
    }


    private string m_strRelation;
    /// <summary>
    /// rgc/wdk 20110920 added for SSI reconfiguration.
    /// This value must be converted to 01,02,03,09 before being placed here.
    /// 
    /// </summary>
    public string propRelation
    {
        get { return m_strRelation; }
        set
        {
            m_strRelation = value.Substring(0, value.ToString().Length > 2 ? 2 : value.Length);
        }
    }
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

    private string m_strIns_a_b_c;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 1
    /// IsNullable - 0
    ///</summary>
    public string propIns_a_b_c
    {
        get { return m_strIns_a_b_c; }
        set
        {
            m_strIns_a_b_c = value.Substring(0, value.ToString().Length > 1 ? 1 : value.Length);
        }
    }

    private string m_strHolder_nme;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 40
    /// IsNullable - 1
    ///</summary>
    public string propHolder_nme
    {
        get { return m_strHolder_nme; }
        set
        {
            m_strHolder_nme = value.Substring(0, value.ToString().Length > 40 ? 40 : value.Length);
        }
    }

    private string m_strPlan_nme;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 25
    /// IsNullable - 1
    ///</summary>
    public string propPlan_nme
    {
        get { return m_strPlan_nme; }
        set
        {
            m_strPlan_nme = value.Substring(0, value.ToString().Length > 25 ? 25 : value.Length);
        }
    }

    private string m_strPlan_addr1;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 40
    /// IsNullable - 1
    ///</summary>
    public string propPlan_addr1
    {
        get { return m_strPlan_addr1; }
        set
        {
            m_strPlan_addr1 = value.Substring(0, value.ToString().Length > 40 ? 40 : value.Length);
        }
    }

    private string m_strPlan_addr2;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 40
    /// IsNullable - 1
    ///</summary>
    public string propPlan_addr2
    {
        get { return m_strPlan_addr2; }
        set
        {
            m_strPlan_addr2 = value.Substring(0, value.ToString().Length > 40 ? 40 : value.Length);
        }
    }

    private string m_strEmployer;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 25
    /// IsNullable - 1
    ///</summary>
    public string propEmployer
    {
        get { return m_strEmployer; }
        set
        {
            m_strEmployer = value.Substring(0, value.ToString().Length > 25 ? 25 : value.Length);
        }
    }

    private string m_strE_city_st;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 35
    /// IsNullable - 1
    ///</summary>
    public string propE_city_st
    {
        get { return m_strE_city_st; }
        set
        {
            m_strE_city_st = value.Substring(0, value.ToString().Length > 35 ? 35 : value.Length);
        }
    }

    private string m_strFin_code;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 1
    /// IsNullable - 1
    ///</summary>
    public string propFin_code
    {
        get { return m_strFin_code; }
        set
        {
            m_strFin_code = value.Substring(0, value.ToString().Length > 1 ? 1 : value.Length);
        }
    }

    private string m_strIns_code;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 10
    /// IsNullable - 1
    ///</summary>
    public string propIns_code
    {
        get { return m_strIns_code; }
        set
        {
            m_strIns_code = value.Substring(0, value.ToString().Length > 10 ? 10 : value.Length);
        }
    }

    private string m_strP_city_st;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 40
    /// IsNullable - 1
    ///</summary>
    public string propP_city_st
    {
        get { return m_strP_city_st; }
        set
        {
            m_strP_city_st = value.Substring(0, value.ToString().Length > 40 ? 40 : value.Length);
        }
    }

    private string m_strPolicy_num;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 20
    /// IsNullable - 1
    ///</summary>
    public string propPolicy_num
    {
        get { return m_strPolicy_num; }
        set
        {
            m_strPolicy_num = value.Substring(0, value.ToString().Length > 20 ? 20 : value.Length);
        }
    }

    private string m_strCert_ssn;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 1
    ///</summary>
    public string propCert_ssn
    {
        get { return m_strCert_ssn; }
        set
        {
            m_strCert_ssn = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strGrp_nme;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 1
    ///</summary>
    public string propGrp_nme
    {
        get { return m_strGrp_nme; }
        set
        {
            m_strGrp_nme = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strGrp_num;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 15
    /// IsNullable - 1
    ///</summary>
    public string propGrp_num
    {
        get { return m_strGrp_num; }
        set
        {
            m_strGrp_num = value.Substring(0, value.ToString().Length > 15 ? 15 : value.Length);
        }
    }

    private string m_strHolder_dob;
    /// <summary>
    /// wdk 20110801 added for ViewerDemoTransfer
    /// No Comment Available
    /// Field Type = datetime
    /// length - 0
    /// IsNullable - 0
    /// </summary>
    public string propHolder_dob
    {
        get { return m_strHolder_dob; }
        set
        {
            DateTime dt = new DateTime();
            if (DateTime.TryParse(value, out dt))
            {
                m_strHolder_dob = value;
            }
            else
            {
                m_strHolder_dob = null;
            }
        }
    }
    private string m_strHolder_sex;
    ///<summary>
    /// No Comment Available
    /// Field Type - varchar
    /// length - 1
    /// IsNullable - 1
    ///</summary>
    public string propHolder_sex
    {
        get { return m_strHolder_sex; }
        set
        {
            m_strHolder_sex = value.Substring(0, value.ToString().Length > 1 ? 1 : value.Length);
        }
    }

    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// INS Recordset based on RCRecordset
    /// </summary>
    public R_ins(string strServer, string strDataBase, ref ERR errLog)
          : base(strServer, strDataBase, "ins", ref errLog)
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
        m_strModHost = OS.GetMachineName();
        m_strModUser = OS.GetUserName();
        m_strModPrg = OS.GetAppName();

        // Tables fields
        m_strAccount = "";
        m_strIns_a_b_c = "";
        m_strHolder_nme = "";
        m_strPlan_nme = "";
        m_strPlan_addr1 = "";
        m_strPlan_addr2 = "";
        m_strEmployer = "";
        m_strE_city_st = "";
        m_strFin_code = "";
        m_strIns_code = "";
        m_strP_city_st = "";
        m_strPolicy_num = "";
        m_strCert_ssn = "";
        m_strGrp_nme = "";
        m_strGrp_num = "";
        m_strHolder_sex = "";
        m_strRelation = "";
        // wdk 20110926 added
        m_strHolderCityStZip = "";
        m_strHolderAddr = "";
        m_strHolderCity = "";
        m_strHolderState = "";
        m_strHolderZip = "";
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
            m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            // ==== End of "standard fields" =============

            m_strAccount = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"].ToString();
            m_strIns_a_b_c = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ins_a_b_c"].ToString();
            m_strHolder_nme = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["holder_nme"].ToString();
            m_strPlan_nme = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["plan_nme"].ToString();
            m_strPlan_addr1 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["plan_addr1"].ToString();
            m_strPlan_addr2 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["plan_addr2"].ToString();
            m_strEmployer = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["employer"].ToString();
            m_strE_city_st = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["e_city_st"].ToString();
            m_strFin_code = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["fin_code"].ToString();
            m_strIns_code = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ins_code"].ToString();
            m_strP_city_st = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["p_city_st"].ToString();
            m_strPolicy_num = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["policy_num"].ToString();
            m_strCert_ssn = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["cert_ssn"].ToString();
            m_strGrp_nme = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["grp_nme"].ToString();
            m_strGrp_num = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["grp_num"].ToString();
            m_strHolder_sex = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["holder_sex"].ToString();
            m_strRelation = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["relation"].ToString();
            // wdk 20110926 added
            m_strHolderAddr = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["holder_addr"].ToString();
            propHolderCityStZip = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["holder_city_st_zip"].ToString();
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
        "rowguid,   deleted,    account,    ins_a_b_c,  holder_nme, " +
        "plan_nme,  plan_addr1, plan_addr2, employer,   e_city_st, " +
        "fin_code,  ins_code,   p_city_st,  policy_num, cert_ssn, " +
        "grp_nme,   grp_num,    holder_sex, holder_dob, relation, holder_addr, holder_city_st_zip, mod_prg) " +
    "VALUES(" +
        "'{1}',     '{2}',      '{3}',      '{4}',      '{5}', " +
        "'{6}',     '{7}',      '{8}',      '{9}',      '{10}', " +
        "'{11}',    '{12}',     '{13}',     '{14}',     '{15}', " +
        "'{16}',    '{17}',     '{18}' ,    {19},     '{20}', '{21}', '{22}', '{23}')",
    propTable,
        m_strRowguid, m_strDeleted == "F" ? 0 : 1, m_strAccount, m_strIns_a_b_c, m_strHolder_nme,
        m_strPlan_nme, m_strPlan_addr1, m_strPlan_addr2, m_strEmployer, m_strE_city_st,
        m_strFin_code, m_strIns_code, m_strP_city_st, m_strPolicy_num, m_strCert_ssn,
        m_strGrp_nme, m_strGrp_num, m_strHolder_sex,
    string.IsNullOrEmpty(m_strHolder_dob) ? "NULL" : string.Format("'{0}'", m_strHolder_dob),
    m_strRelation, m_strHolderAddr, m_strHolderCityStZip, m_strModPrg);

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
        //"rowguid= '{1}', "+ wdk 20110801 don't update the rowguid it is the key field
        "deleted= '{1}', " +
        "account= '{2}', " +
        "ins_a_b_c= '{3}', " +
        "holder_nme= '{4}', " +
        "plan_nme= '{5}', " +
        "plan_addr1= '{6}', " +
        "plan_addr2= '{7}', " +
        "employer= '{8}', " +
        "e_city_st= '{9}', " +
        "fin_code= '{10}', " +
        "ins_code= '{11}', " +
        "p_city_st= '{12}', " +
        "policy_num= '{13}', " +
        "cert_ssn= '{14}', " +
        "grp_nme= '{15}', " +
        "grp_num= '{16}', " +
        "holder_sex= '{17}', holder_dob = {18} , relation = '{19}', " +
    "holder_addr = '{20}', holder_city_st_zip = '{21}' where {22}",
        propTable,
    //m_strRowguid, 
    m_strDeleted == "F" ? 0 : 1,
                        m_strAccount, m_strIns_a_b_c, m_strHolder_nme, m_strPlan_nme,
        m_strPlan_addr1, m_strPlan_addr2, m_strEmployer, m_strE_city_st, m_strFin_code,
        m_strIns_code, m_strP_city_st, m_strPolicy_num, m_strCert_ssn, m_strGrp_nme,
    m_strGrp_num, m_strHolder_sex, string.IsNullOrEmpty(m_strHolder_dob) ? "NULL" : string.Format("'{0}'", m_strHolder_dob),
    m_strRelation, m_strHolderAddr, m_strHolderCityStZip,
    strWhere);

        return SQLExec(strSQL, out m_strErrMsg);
    }


} // do not go below this line

