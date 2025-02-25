using System;
using System.Data;  // for use with DataRow
//Programmer added usings
using Utilities;

namespace MCL;

/// <summary>
/// 
/// </summary>
[Obsolete("This class is obsolete. New code should be written using LabBilling.Core libraries.")]
public class R_pat : RCRecordset
{

    private string m_strHneEpiNumber;
    /// <summary>
    /// Meditech identifier is EPI, HIS identifier is HNE. Same number.
    /// </summary>
    public string propHneEpiNumber
    {
        get { return m_strHneEpiNumber; }
        set { m_strHneEpiNumber = value; }
    }
    //private string m_strMod_user;
    //private string m_strMod_prg;
    //private string m_strMod_host;
    //private string m_strMod_date;

    private string m_strPcCode;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propPc_code
    {
        get { return m_strPcCode; }
        set
        {
            m_strPcCode = value;
        }
    }
    private string m_strMinAmt;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propMin_amt
    {
        get { return m_strMinAmt; }
        set
        {
            m_strMinAmt = value;
        }
    }
    private string m_strDobYyyy;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propDob_yyyy
    {
        get { return m_strDobYyyy; }
        set
        {
            m_strDobYyyy = value;
        }
    }
    private string m_strFirstDm;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propFirst_dm
    {
        get { return m_strFirstDm; }
        set
        {
            m_strFirstDm = value;
        }
    }
    private string m_strLastDm;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propLast_dm
    {
        get { return m_strLastDm; }
        set
        {
            m_strLastDm = value;
        }
    }
    private string m_strDbillDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propDbill_date
    {
        get { return m_strDbillDate; }
        set
        {
            m_strDbillDate = value;
        }
    }
    private string m_strUbDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propUb_date
    {
        get { return m_strUbDate; }
        set
        {
            m_strUbDate = value;
        }
    }
    private string m_strH1500Date;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propH1500_date
    {
        get { return m_strH1500Date; }
        set
        {
            m_strH1500Date = value;
        }
    }
    private string m_strEUbDemandDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propE_ub_demand_date
    {
        get { return m_strEUbDemandDate; }
        set
        {
            m_strEUbDemandDate = value;
        }
    }
    private string m_strClaimsnet1500BatchDate;

    /// <summary>
    ///  This is the date we placed in the h1500 table for billing thru Claimsnet 07/04/2008 wdk/rgc 11/17/2008 wdk changed the name by adding 1500
    /// </summary>
    public string propClaimsnet_1500_batch_date
    {
        get { return m_strClaimsnet1500BatchDate; }
        set
        {
            m_strClaimsnet1500BatchDate = value;
        }
    }
    private string m_strClaimsnetUbBatchDate;

    /// <summary>
    /// 11/17/2008 wdk this is the date and time we placed the ub in the ub table for claimsnet billing.
    /// </summary>
    public string propClaimsnet_ub_batch_date
    {
        get { return m_strClaimsnetUbBatchDate; }
        set
        {
            m_strClaimsnetUbBatchDate = value;
        }
    }
    private string m_strColLtrDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propColltr_date
    {
        get { return m_strColLtrDate; }
        set
        {
            m_strColLtrDate = value;
        }
    }
    private string m_strBadDebtDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propBaddebt_date
    {
        get { return m_strBadDebtDate; }
        set
        {
            m_strBadDebtDate = value;
        }
    }

    private string m_strBatchDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propBatch_date
    {
        get { return m_strBatchDate; }
        set
        {
            m_strBatchDate = value;
        }
    }
    private string m_strBdListDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propBd_list_date
    {
        get { return m_strBdListDate; }
        set
        {
            m_strBdListDate = value;
        }
    }
    private string m_strEbillBatchDate;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propEbill_batch_date
    {
        get { return m_strEbillBatchDate; }
        set
        {
            m_strEbillBatchDate = value;
        }
    }
    private string m_strEbillBatch1500;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propEbill_batch_1500
    {
        get { return m_strEbillBatch1500; }
        set
        {
            m_strEbillBatch1500 = value;
        }
    }
    private string m_strEUbDemand;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propE_ub_demand
    {
        get { return m_strEUbDemand; }
        set
        {
            m_strEUbDemand = value;
        }
    }
    private string m_strAccount;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propAccount
    {
        get { return m_strAccount; }
        set
        {
            m_strAccount = value;
        }
    }
    private string m_strSsn;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propSsn
    {
        get { return m_strSsn; }
        set
        {
            m_strSsn = value;
        }
    }
    private string m_strPatAddr1;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propPat_addr1
    {
        get { return m_strPatAddr1; }
        set
        {
            m_strPatAddr1 = value;
        }
    }
    private string m_strPatAddr2;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propPat_addr2
    {
        get { return m_strPatAddr2; }
        set
        {
            m_strPatAddr2 = value;
        }
    }
    private string m_strCityStZip;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propCity_st_zip
    {
        get { return m_strCityStZip; }
        set
        {
            if (string.IsNullOrEmpty(value.ToString()))
            {
                return;
            }
            m_strCityStZip = value.Trim();
            if (m_strCityStZip != ",")
            {
                m_strCityStZip = value;
            }
            else // wdk 20110722 added so that "," won't be the value in the recordset
            {
                m_strCityStZip = "";
            }
        }
    }
    private string m_strSex;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propSex
    {
        get { return m_strSex; }
        set
        {
            m_strSex = value;
        }
    }
    private string m_strIcd9_8;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_8
    {
        get { return m_strIcd9_8; }
        set
        {
            m_strIcd9_8 = value;
        }
    }

    private string m_strIcd9_9;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_9
    {
        get { return m_strIcd9_9; }
        set
        {
            m_strIcd9_9 = value;
        }
    }
    private string m_strMailer;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propMailer
    {
        get { return m_strMailer; }
        set
        {
            m_strMailer = value;
        }
    }
    private string m_strPhyId;
    /// <summary>
    /// 
    /// </summary>
    public string propPhy_id
    {
        get { return m_strPhyId; }
        set
        {
            m_strPhyId = value;
        }
    }
    private string m_strGuarPhone;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propGuar_phone
    {
        get { return m_strGuarPhone; }
        set
        {
            m_strGuarPhone = value;
        }
    }
    private string m_strIcd9_2;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_2
    {
        get { return m_strIcd9_2; }
        set
        {
            m_strIcd9_2 = value;
        }
    }
    private string m_strIcd9_3;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_3
    {
        get { return m_strIcd9_3; }
        set
        {
            m_strIcd9_3 = value;
        }
    }
    private string m_strIcd9_4;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_4
    {
        get { return m_strIcd9_4; }
        set
        {
            m_strIcd9_4 = value;
        }
    }
    private string m_strIcd9_5;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_5
    {
        get { return m_strIcd9_5; }
        set
        {
            m_strIcd9_5 = value;
        }
    }
    private string m_strIcd9_6;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_6
    {
        get { return m_strIcd9_6; }
        set
        {
            m_strIcd9_6 = value;
        }
    }
    private string m_strIcd9_7;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_7
    {
        get { return m_strIcd9_7; }
        set
        {
            m_strIcd9_7 = value;
        }
    }
    private string m_strRelation;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propRelation
    {
        get { return m_strRelation; }
        set
        {
            // rgc/wdk 20110915 modifed sets from value to m_strRelation.
            // wdk 20110722 added conversion to our codes from data passed via Meditech Demographics file
            m_strRelation = null;
            if (value == "SELF" || value == "SE" || value == "01")
            {
                m_strRelation = "01";
            }
            if (value.StartsWith("SP") || value == "02")
            {
                m_strRelation = "02";
            }
            if (value == "CHILD" || value.StartsWith("ST") || value == "03")
            {
                m_strRelation = "03";
            }
            if (value.StartsWith("OT") || value == "09")
            {
                m_strRelation = "09";
            }
            // wdk 20110912 removed because the R1500 program chokes if the m_strRelation gets to be 2 alpha characters
            // wdk 20110805 added check for EMPLOYEE as it is in our download for workers comp charges.
            //if (value == "OTHER" || value == "UNKNOWN" || value == "EMPLOYEE")
            //{
            //    value = "09";
            //}

            //m_strRelation = value.Substring(0,value.Length>2?2:value.Length); // wdk 20110722 added substring to prevent overflow of the field size of 2
        }
    }
    private string m_strGuarantor;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propGuarantor
    {
        get { return m_strGuarantor; }
        set
        {
            m_strGuarantor = value;
        }
    }
    private string m_strGuarAddr;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propGuar_addr
    {
        get { return m_strGuarAddr; }
        set
        {
            m_strGuarAddr = value;
        }
    }
    private string m_strGuarCitySt;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propG_city_st
    {
        get { return m_strGuarCitySt; }
        set
        {
            m_strGuarCitySt = value;
        }
    }
    private string m_strPat_marital;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propPat_marital
    {
        get { return m_strPat_marital; }
        set
        {
            m_strPat_marital = value;
        }
    }
    private string m_strIcd9_1;
    /* No Comment Available*/
    /// <summary>
    /// 
    /// </summary>
    public string propIcd9_1
    {
        get { return m_strIcd9_1; }
        set
        {
            m_strIcd9_1 = value;
        }
    }

    //Private variables set in constructor to initialize the base class
    private string m_strServer;
    private string m_strDataBase;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strServer"></param>
    /// <param name="strDataBase"></param>
    /// <param name="errLog"></param>
    public R_pat(string strServer, string strDataBase, ref ERR errLog)
          : base(strServer, strDataBase, "pat", ref errLog)
    {
        m_strServer = strServer;
        m_strDataBase = strDataBase;
        // Error log set via  RCRecordset m_ERR base class call
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strWhere"></param>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <param name="strWhere"></param>
    /// <returns></returns>
    public int GetActiveRecords(string strWhere)
    {
        strWhere = string.Format("deleted = 0 AND {0}", strWhere);
        return (GetRecords(strWhere));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool MoveFirst()
    {
        m_CurrentRecordIndex = 1;
        return (LoadMemberVariablesFromDataSet());
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public bool MoveLast()
    {
        m_CurrentRecordIndex = m_CurrentRecordCount;
        return (LoadMemberVariablesFromDataSet());
    }
    // FlagCurrentRecordDeleted
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int FlagCurrentRecordDeleted()
    {
        m_strDeleted = "T";
        return (Update());
    }

    // FlagCurrentRecordNOTDeleted
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int FlagCurrentRecordNOTDeleted()
    {
        m_strDeleted = "F";
        return (Update());
    }

    /// <summary>
    /// 
    /// </summary>
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
        m_strPcCode = "";
        m_strMinAmt = "";
        m_strDobYyyy = "";
        m_strFirstDm = "";
        m_strLastDm = "";
        m_strDbillDate = "";
        m_strUbDate = "";
        m_strH1500Date = "";
        m_strEUbDemandDate = "";
        m_strClaimsnet1500BatchDate = "";
        m_strClaimsnetUbBatchDate = "";
        m_strColLtrDate = "";
        m_strBadDebtDate = "";
        m_strBatchDate = "";
        m_strBdListDate = "";
        m_strEbillBatchDate = "";
        m_strEbillBatch1500 = "";
        m_strEUbDemand = "";
        m_strAccount = "";
        m_strSsn = "";
        m_strPatAddr1 = "";
        m_strPatAddr2 = "";
        m_strCityStZip = "";
        m_strSex = "";
        m_strIcd9_8 = "";
        m_strIcd9_9 = "";
        m_strMailer = "";
        m_strPhyId = "";
        m_strGuarPhone = "";
        m_strIcd9_2 = "";
        m_strIcd9_3 = "";
        m_strIcd9_4 = "";
        m_strIcd9_5 = "";
        m_strIcd9_6 = "";
        m_strIcd9_7 = "";
        m_strRelation = "";
        m_strGuarantor = "";
        m_strGuarAddr = "";
        m_strGuarCitySt = "";
        m_strPat_marital = "";
        m_strIcd9_1 = "";
        // rgc/wdk 20110104 added
        m_strHneEpiNumber = "";


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
                m_strModDate = ((DateTime)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_date"]).ToString("g");
            }
            m_strDeleted = (bool)m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["deleted"] ? "T" : "F";
            m_strModUser = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_user"].ToString();
            m_strModPrg = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_prg"].ToString();
            m_strModHost = m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["mod_host"].ToString();
            // ==== End of "standard fields" =============

            m_strPcCode = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pc_code"].ToString();
            m_strMinAmt = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["min_amt"].ToString();
            m_strEUbDemand = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["e_ub_demand"].ToString();
            m_strAccount = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"].ToString();
            m_strSsn = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ssn"].ToString();
            m_strPatAddr1 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pat_addr1"].ToString();
            m_strPatAddr2 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pat_addr2"].ToString();
            m_strCityStZip = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["city_st_zip"].ToString();
            m_strSex = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["sex"].ToString();
            m_strMailer = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mailer"].ToString();
            m_strPhyId = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["phy_id"].ToString();
            m_strGuarPhone = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["guar_phone"].ToString();
            m_strIcd9_1 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_1"].ToString();
            m_strIcd9_2 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_2"].ToString();
            m_strIcd9_3 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_3"].ToString();
            m_strIcd9_4 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_4"].ToString();
            m_strIcd9_5 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_5"].ToString();
            m_strIcd9_6 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_6"].ToString();
            m_strIcd9_7 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_7"].ToString();
            m_strIcd9_8 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_8"].ToString();
            m_strIcd9_9 = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_9"].ToString();
            m_strRelation = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["relation"].ToString();
            m_strGuarantor = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["guarantor"].ToString();
            m_strGuarAddr = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["guar_addr"].ToString();
            m_strGuarCitySt = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["g_city_st"].ToString();
            m_strPat_marital = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pat_marital"].ToString();
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["dob_yyyy"].Equals(System.DBNull.Value))
            {
                m_strDobYyyy = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["dob_yyyy"]).ToString("d");
            }

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["first_dm"].Equals(System.DBNull.Value))
            {
                m_strFirstDm = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["first_dm"]).ToString("d");
            }

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["last_dm"].Equals(System.DBNull.Value))
            {
                m_strLastDm = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["last_dm"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["dbill_date"].Equals(System.DBNull.Value))
            {
                m_strDbillDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["dbill_date"]).ToString("d");
            }

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ub_date"].Equals(System.DBNull.Value))
            {
                m_strUbDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ub_date"]).ToString("d");
            }

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["h1500_date"].Equals(System.DBNull.Value))
            {
                m_strH1500Date = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["h1500_date"]).ToString("d");
            }

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["e_ub_demand_date"].Equals(System.DBNull.Value))
            {
                m_strEUbDemandDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["e_ub_demand_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claimsnet_1500_batch_date"].Equals(System.DBNull.Value))
            {
                m_strClaimsnet1500BatchDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["claimsnet_1500_batch_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["claimsnet_ub_batch_date"].Equals(System.DBNull.Value))
            {
                m_strClaimsnetUbBatchDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["claimsnet_ub_batch_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["colltr_date"].Equals(System.DBNull.Value))
            {
                m_strColLtrDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["colltr_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["baddebt_date"].Equals(System.DBNull.Value))
            {
                m_strBadDebtDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["baddebt_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["batch_date"].Equals(System.DBNull.Value))
            {
                m_strBatchDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["batch_date"]).ToString("d");
            }

            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["bd_list_date"].Equals(System.DBNull.Value))
            {
                m_strBdListDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["bd_list_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ebill_batch_date"].Equals(System.DBNull.Value))
            {
                m_strEbillBatchDate = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ebill_batch_date"]).ToString("d");
            }
            if (!m_DataSet.Tables[propTable.ToString()].Rows[m_CurrentRecordIndex]["ebill_batch_1500"].Equals(System.DBNull.Value))
            {
                m_strEbillBatch1500 = ((DateTime)m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ebill_batch_1500"]).ToString("d");
            }
            // rgc/wdk 20110104 added
            m_strHneEpiNumber = m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["hne_epi_number"].ToString();


            // Return true! we were successful
            bRetVal = true;

        }
        return (bRetVal);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int AddRecord(string strWhere)
    {
        int iRetVal = -1;
        string strSQL;

        strSQL = string.Format("INSERT INTO {0} (" +
        // "rowguid, "+ the sql table will insert this identity field
        "pc_code, min_amt, dob_yyyy, first_dm, last_dm, " +
        "dbill_date, ub_date, h1500_date, e_ub_demand_date, claimsnet_1500_batch_date, " +
        "claimsnet_ub_batch_date, colltr_date, baddebt_date, batch_date, bd_list_date, " +
        "ebill_batch_date, ebill_batch_1500, e_ub_demand, deleted, account, " +
        "ssn, pat_addr1, pat_addr2, city_st_zip, sex, " +
        "mailer, phy_id, guar_phone, icd9_1, icd9_2, " +
        "icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, " +
        "icd9_8, icd9_9, relation, guarantor, guar_addr, " +
        "g_city_st, pat_marital, hne_epi_number, mod_prg)VALUES(" +
        "'{1}','{2}', {3},  {4}, {5}, " +
        "{6}, {7}, {8}, {9}, {10}, " +
        "{11}, {12}, {13}, {14}, {15}, " +
        "{16}, {17}, '{18}', '{19}', '{20}', " +
        "'{21}', '{22}', '{23}', '{24}', '{25}', " +
        "'{26}', '{27}', '{28}', {29}, {30}, " +
        "{31}, {32}, {33}, {34}, {35}, {36}, " +
        "{37}, '{38}', '{39}', '{40}', " +
        "'{41}', '{42}', '{43}', '{44}')",
       propTable, //0 
        m_strPcCode, m_strMinAmt, // 1,2
           string.IsNullOrEmpty(m_strDobYyyy) ? "null" : string.Format("'{0}'", m_strDobYyyy), // 3
           string.IsNullOrEmpty(m_strFirstDm) ? "null" : string.Format("'{0}'", m_strFirstDm), //4
           string.IsNullOrEmpty(m_strLastDm) ? "null" : string.Format("'{0}'", m_strLastDm), //5
        string.IsNullOrEmpty(m_strDbillDate) ? "null" : string.Format("'{0}'", m_strDbillDate), //6
           string.IsNullOrEmpty(m_strUbDate) ? "NULL" : string.Format("'{0}'", m_strUbDate),
           string.IsNullOrEmpty(m_strH1500Date) ? "NULL" : string.Format("'{0}'", m_strH1500Date),
           string.IsNullOrEmpty(m_strEUbDemandDate) ? "NULL" : string.Format("'{0}'", m_strEUbDemandDate),
           string.IsNullOrEmpty(m_strClaimsnet1500BatchDate) ? "NULL" : string.Format("'{0}'", m_strClaimsnet1500BatchDate),
        string.IsNullOrEmpty(m_strClaimsnetUbBatchDate) ? "NULL" : string.Format("'{0}'", m_strClaimsnetUbBatchDate), // 11
           string.IsNullOrEmpty(m_strColLtrDate) ? "NULL" : string.Format("'{0}'", m_strColLtrDate),
           string.IsNullOrEmpty(m_strBadDebtDate) ? "NULL" : string.Format("'{0}'", m_strBadDebtDate),
           string.IsNullOrEmpty(m_strBatchDate) ? "NULL" : string.Format("'{0}'", m_strBatchDate),
           string.IsNullOrEmpty(m_strBdListDate) ? "NULL" : string.Format("'{0}'", m_strBdListDate),
        string.IsNullOrEmpty(m_strEbillBatchDate) ? "NULL" : string.Format("'{0}'", m_strEbillBatchDate), //16
           string.IsNullOrEmpty(m_strEbillBatch1500) ? "NULL" : string.Format("'{0}'", m_strEbillBatch1500),
           m_strDeleted == "T" ? 1 : 0, m_strEUbDemand, m_strAccount, //20
        m_strSsn, m_strPatAddr1, m_strPatAddr2, m_strCityStZip, m_strSex, //25
        m_strMailer, m_strPhyId, m_strGuarPhone,
        string.IsNullOrEmpty(m_strIcd9_1) ? "NULL" : string.Format("'{0}'", m_strIcd9_1),
        string.IsNullOrEmpty(m_strIcd9_2) ? "NULL" : string.Format("'{0}'", m_strIcd9_2), //30
        string.IsNullOrEmpty(m_strIcd9_3) ? "NULL" : string.Format("'{0}'", m_strIcd9_3), //m_strIcd9_3,
        string.IsNullOrEmpty(m_strIcd9_4) ? "NULL" : string.Format("'{0}'", m_strIcd9_4), //m_strIcd9_4,
        string.IsNullOrEmpty(m_strIcd9_5) ? "NULL" : string.Format("'{0}'", m_strIcd9_5), //m_strIcd9_5, 
        string.IsNullOrEmpty(m_strIcd9_6) ? "NULL" : string.Format("'{0}'", m_strIcd9_6),  //m_strIcd9_6,
        string.IsNullOrEmpty(m_strIcd9_7) ? "NULL" : string.Format("'{0}'", m_strIcd9_7), //m_strIcd9_7, // 35
        string.IsNullOrEmpty(m_strIcd9_8) ? "NULL" : string.Format("'{0}'", m_strIcd9_8), //m_strIcd9_8,
        string.IsNullOrEmpty(m_strIcd9_9) ? "NULL" : string.Format("'{0}'", m_strIcd9_9),  //m_strIcd9_9, 
        m_strRelation, m_strGuarantor, m_strGuarAddr,  // 40
        m_strGuarCitySt, m_strPat_marital, m_strHneEpiNumber, m_strModPrg);

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
    /// 
    /// </summary>
    /// <returns></returns>
    public int AddRecord()
    {
        int iRetVal = -1;
        string strSQL;

        strSQL = string.Format("INSERT INTO {0} (" +
        // "rowguid, "+ the sql table will insert this identity field
        "pc_code, min_amt, dob_yyyy, first_dm, last_dm, " +
        "dbill_date, ub_date, h1500_date, e_ub_demand_date, claimsnet_1500_batch_date, " +
        "claimsnet_ub_batch_date, colltr_date, baddebt_date, batch_date, bd_list_date, " +
        "ebill_batch_date, ebill_batch_1500, e_ub_demand, deleted, account, " +
        "ssn, pat_addr1, pat_addr2, city_st_zip, sex, " +
        "mailer, phy_id, guar_phone, icd9_1, icd9_2, " +
        "icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, " +
        "icd9_8, icd9_9, relation, guarantor, guar_addr, " +
        "g_city_st, pat_marital, hne_epi_number, mod_prg)VALUES(" +
        "'{1}','{2}', {3},  {4}, {5}, " +
        "{6}, {7}, {8}, {9}, {10}, " +
        "{11}, {12}, {13}, {14}, {15}, " +
        "{16}, {17}, '{18}', '{19}', '{20}', " +
        "'{21}', '{22}', '{23}', '{24}', '{25}', " +
        "'{26}', '{27}', '{28}', {29}, {30}, " +
        "{31}, {32}, {33}, {34}, {35}, {36}, " +
        "{37}, '{38}', '{39}', '{40}', " +
        "'{41}', '{42}', '{43}', '{44}')",
           propTable, //0 
        m_strPcCode, m_strMinAmt, // 1,2
           string.IsNullOrEmpty(m_strDobYyyy) ? "null" : string.Format("'{0}'", m_strDobYyyy), // 3
           string.IsNullOrEmpty(m_strFirstDm) ? "null" : string.Format("'{0}'", m_strFirstDm), //4
           string.IsNullOrEmpty(m_strLastDm) ? "null" : string.Format("'{0}'", m_strLastDm), //5
        string.IsNullOrEmpty(m_strDbillDate) ? "null" : string.Format("'{0}'", m_strDbillDate), //6
           string.IsNullOrEmpty(m_strUbDate) ? "NULL" : string.Format("'{0}'", m_strUbDate),
           string.IsNullOrEmpty(m_strH1500Date) ? "NULL" : string.Format("'{0}'", m_strH1500Date),
           string.IsNullOrEmpty(m_strEUbDemandDate) ? "NULL" : string.Format("'{0}'", m_strEUbDemandDate),
           string.IsNullOrEmpty(m_strClaimsnet1500BatchDate) ? "NULL" : string.Format("'{0}'", m_strClaimsnet1500BatchDate),
        string.IsNullOrEmpty(m_strClaimsnetUbBatchDate) ? "NULL" : string.Format("'{0}'", m_strClaimsnetUbBatchDate), // 11
           string.IsNullOrEmpty(m_strColLtrDate) ? "NULL" : string.Format("'{0}'", m_strColLtrDate),
           string.IsNullOrEmpty(m_strBadDebtDate) ? "NULL" : string.Format("'{0}'", m_strBadDebtDate),
           string.IsNullOrEmpty(m_strBatchDate) ? "NULL" : string.Format("'{0}'", m_strBatchDate),
           string.IsNullOrEmpty(m_strBdListDate) ? "NULL" : string.Format("'{0}'", m_strBdListDate),
        string.IsNullOrEmpty(m_strEbillBatchDate) ? "NULL" : string.Format("'{0}'", m_strEbillBatchDate), //16
           string.IsNullOrEmpty(m_strEbillBatch1500) ? "NULL" : string.Format("'{0}'", m_strEbillBatch1500),
           m_strDeleted == "T" ? 1 : 0, m_strEUbDemand, m_strAccount, //20
        m_strSsn, m_strPatAddr1, m_strPatAddr2, m_strCityStZip, m_strSex, //25
        m_strMailer, m_strPhyId, m_strGuarPhone,
        string.IsNullOrEmpty(m_strIcd9_1) ? "NULL" : string.Format("'{0}'", m_strIcd9_1),
        string.IsNullOrEmpty(m_strIcd9_2) ? "NULL" : string.Format("'{0}'", m_strIcd9_2), //30
        string.IsNullOrEmpty(m_strIcd9_3) ? "NULL" : string.Format("'{0}'", m_strIcd9_3), //m_strIcd9_3,
        string.IsNullOrEmpty(m_strIcd9_4) ? "NULL" : string.Format("'{0}'", m_strIcd9_4), //m_strIcd9_4,
        string.IsNullOrEmpty(m_strIcd9_5) ? "NULL" : string.Format("'{0}'", m_strIcd9_5), //m_strIcd9_5, 
        string.IsNullOrEmpty(m_strIcd9_6) ? "NULL" : string.Format("'{0}'", m_strIcd9_6),  //m_strIcd9_6,
        string.IsNullOrEmpty(m_strIcd9_7) ? "NULL" : string.Format("'{0}'", m_strIcd9_7), //m_strIcd9_7, // 35
        string.IsNullOrEmpty(m_strIcd9_8) ? "NULL" : string.Format("'{0}'", m_strIcd9_8), //m_strIcd9_8,
        string.IsNullOrEmpty(m_strIcd9_9) ? "NULL" : string.Format("'{0}'", m_strIcd9_9),  //m_strIcd9_9, 
        m_strRelation, m_strGuarantor, m_strGuarAddr,  // 40
        m_strGuarCitySt, m_strPat_marital, m_strHneEpiNumber, m_strModPrg);

        iRetVal = SQLExec(strSQL, out m_strErrMsg);
        if (iRetVal > 0)
        {

            //Add new Record to dataset
            DataRow L_DataRow = m_DataSet.Tables[propTable].NewRow();
            m_DataSet.Tables[propTable].Rows.Add(L_DataRow);
            // Index is zero based while record count is 1 based  ie. the correct recourd count
            m_CurrentRecordIndex = m_CurrentRecordCount++;
            // if unarchiving a record flagged to deleted have to covert back to the live system as deleted
            //Don't forget to change m_strDeleted == "F" ? 0 : 1;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"] = m_strRowguid;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pc_code"] = string.IsNullOrEmpty(m_strPcCode) ? "0" : m_strPcCode;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["min_amt"] = string.IsNullOrEmpty(m_strMinAmt) ? "0.00" : m_strMinAmt;
            if (!string.IsNullOrEmpty(m_strDobYyyy))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["dob_yyyy"] = m_strDobYyyy;
            }
            if (!string.IsNullOrEmpty(m_strFirstDm))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["first_dm"] = m_strFirstDm;
            }
            if (!string.IsNullOrEmpty(m_strLastDm))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["last_dm"] = m_strLastDm;
            }
            if (!string.IsNullOrEmpty(m_strDbillDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["dbill_date"] = m_strDbillDate;
            }
            if (!string.IsNullOrEmpty(m_strUbDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ub_date"] = m_strUbDate;
            }
            if (!string.IsNullOrEmpty(m_strH1500Date))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["h1500_date"] = m_strH1500Date;
            }
            if (!string.IsNullOrEmpty(m_strEUbDemandDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["e_ub_demand_date"] = m_strEUbDemandDate;
            }
            if (!string.IsNullOrEmpty(m_strClaimsnet1500BatchDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["claimsnet_1500_batch_date"] = m_strClaimsnet1500BatchDate;
            }
            if (!string.IsNullOrEmpty(m_strClaimsnetUbBatchDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["claimsnet_ub_batch_date"] = m_strClaimsnetUbBatchDate;
            }
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mod_date"] = m_strModDate;
            if (!string.IsNullOrEmpty(m_strColLtrDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["colltr_date"] = m_strColLtrDate;
            }
            if (!string.IsNullOrEmpty(m_strBadDebtDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["baddebt_date"] = m_strBadDebtDate;
            }
            if (!string.IsNullOrEmpty(m_strBatchDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["batch_date"] = m_strBatchDate;
            }
            if (!string.IsNullOrEmpty(m_strBdListDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["bd_list_date"] = m_strBdListDate;
            }
            if (!string.IsNullOrEmpty(m_strEbillBatchDate))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ebill_batch_date"] = m_strEbillBatchDate;
            }
            if (!string.IsNullOrEmpty(m_strEbillBatch1500))
            {
                m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ebill_batch_1500"] = m_strEbillBatch1500;
            }
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["deleted"] = m_strDeleted == "T" ? 1 : 0;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["e_ub_demand"] = m_strEUbDemand == "T" ? 1 : 0;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["account"] = m_strAccount;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["ssn"] = m_strSsn;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pat_addr1"] = m_strPatAddr1;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pat_addr2"] = m_strPatAddr2;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["city_st_zip"] = m_strCityStZip;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["sex"] = m_strSex;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mod_user"] = m_strModUser;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mod_prg"] = m_strModPrg;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_8"] = m_strIcd9_8;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_9"] = m_strIcd9_9;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mailer"] = m_strMailer;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["phy_id"] = m_strPhyId;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["guar_phone"] = m_strGuarPhone;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["mod_host"] = m_strModHost;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_2"] = m_strIcd9_2;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_3"] = m_strIcd9_3;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_4"] = m_strIcd9_4;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_5"] = m_strIcd9_5;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_6"] = m_strIcd9_6;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_7"] = m_strIcd9_7;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["relation"] = m_strRelation;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["guarantor"] = m_strGuarantor;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["guar_addr"] = m_strGuarAddr;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["g_city_st"] = m_strGuarCitySt;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["pat_marital"] = m_strPat_marital;
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["icd9_1"] = m_strIcd9_1;
            // wdk 20110308
            m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["hne_epi_number"] = m_strHneEpiNumber;
        }
        return iRetVal;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int Update()
    {
        string strWhere;
        string strSQL;

        //Set the where clause for the KEY for this table from the DataSet values

        strWhere = string.Format("rowguid = '{0}'",
                   m_DataSet.Tables[propTable].Rows[m_CurrentRecordIndex]["rowguid"].ToString());
        strSQL = string.Format("UPDATE {0} SET " +
        "rowguid= '{1}', " +
        "pc_code= '{2}', " +
        "min_amt= '{3}', " +
        "dob_yyyy= {4}, " +
        "first_dm= {5}, " +
        "last_dm= {6}, " +
        "dbill_date= {7}, " +
        "ub_date= {8}, " +
        "h1500_date= {9}, " +
        "e_ub_demand_date= {10}, " +
        "claimsnet_1500_batch_date= {11}, " +
        "claimsnet_ub_batch_date= {12}, " +
        //"mod_date= '{13}', "+
        "colltr_date= {13}, " +
        "baddebt_date= {14}, " +
        "batch_date= {15}, " +
        "bd_list_date= {16}, " +
        "ebill_batch_date= {17}, " +
        "ebill_batch_1500= {18}, " +
        "deleted= '{19}', " +
        "e_ub_demand= '{20}', " +
        "account= '{21}', " +
        "ssn= '{22}', " +
        "pat_addr1= '{23}', " +
        "pat_addr2= '{24}', " +
        "city_st_zip= '{25}', " +
        "sex= '{26}', " +
        //"mod_user= '{27}', "+
        //"mod_prg= '{28}', "+
        "icd9_8= {27}, " +
        "icd9_9= {28}, " +
        "mailer= '{29}', " +
        "phy_id= '{30}', " +
        "guar_phone= '{31}', " +
        //"mod_host= '{34}', "+
        "icd9_2= {32}, " +
        "icd9_3= {33}, " +
        "icd9_4= {34}, " +
        "icd9_5= {35}, " +
        "icd9_6= {36}, " +
        "icd9_7= {37}, " +
        "relation= '{38}', " +
        "guarantor= '{39}', " +
        "guar_addr= '{40}', " +
        "g_city_st= '{41}', " +
        "pat_marital= '{42}', " +
        "icd9_1= {43}, hne_epi_number = '{44}' " +
        "WHERE {45}",
               propTable,
        m_strRowguid,
        m_strPcCode,
        m_strMinAmt,
        string.IsNullOrEmpty(m_strDobYyyy) ? "NULL" : string.Format("'{0}'", m_strDobYyyy), //m_strDobYyyy,
        string.IsNullOrEmpty(m_strFirstDm) ? "NULL" : string.Format("'{0}'", m_strFirstDm),//m_strFirstDm,
        string.IsNullOrEmpty(m_strLastDm) ? "NULL" : string.Format("'{0}'", m_strLastDm),//m_strLastDm,
        string.IsNullOrEmpty(m_strDbillDate) ? "NULL" : string.Format("'{0}'", m_strDbillDate),//m_strDbillDate,
        string.IsNullOrEmpty(m_strUbDate) ? "NULL" : string.Format("'{0}'", m_strUbDate),//m_strUbDate,
        string.IsNullOrEmpty(m_strH1500Date) ? "NULL" : string.Format("'{0}'", m_strH1500Date),//m_strH1500Date,
        string.IsNullOrEmpty(m_strEUbDemandDate) ? "NULL" : string.Format("'{0}'", m_strEUbDemandDate),//m_strEUbDemandDate,
        string.IsNullOrEmpty(m_strClaimsnet1500BatchDate) ? "NULL" : string.Format("'{0}'", m_strClaimsnet1500BatchDate),//m_strClaimsnet1500BatchDate,
        string.IsNullOrEmpty(m_strClaimsnetUbBatchDate) ? "NULL" : string.Format("'{0}'", m_strClaimsnetUbBatchDate),//m_strClaimsnetUbBatchDate,
                                                                                                                     //string.IsNullOrEmpty(m_strModDate) ? "NULL" : string.Format("'{0}'", m_strModDate),//m_strModDate,
        string.IsNullOrEmpty(m_strColLtrDate) ? "NULL" : string.Format("'{0}'", m_strColLtrDate),//m_strColLtrDate,
        string.IsNullOrEmpty(m_strBadDebtDate) ? "NULL" : string.Format("'{0}'", m_strBadDebtDate),//m_strBadDebtDate,
        string.IsNullOrEmpty(m_strBatchDate) ? "NULL" : string.Format("'{0}'", m_strBatchDate),//m_strBatchDate,
        string.IsNullOrEmpty(m_strBdListDate) ? "NULL" : string.Format("'{0}'", m_strBdListDate),//m_strBdListDate,
        string.IsNullOrEmpty(m_strEbillBatchDate) ? "NULL" : string.Format("'{0}'", m_strEbillBatchDate),//m_strEbillBatchDate,
        string.IsNullOrEmpty(m_strEbillBatch1500) ? "NULL" : string.Format("'{0}'", m_strEbillBatch1500),//m_strEbillBatch1500,
        m_strDeleted == "F" ? 0 : 1,
        m_strEUbDemand,
        m_strAccount,
        m_strSsn,
        m_strPatAddr1,
        m_strPatAddr2,
        m_strCityStZip,
        m_strSex,
        // m_strModUser,
        // m_strModPrg,
        string.IsNullOrEmpty(m_strIcd9_8) ? "NULL" : string.Format("'{0}'", m_strIcd9_8), //m_strIcd9_8,
        string.IsNullOrEmpty(m_strIcd9_9) ? "NULL" : string.Format("'{0}'", m_strIcd9_9), //m_strIcd9_9,
        m_strMailer,
        m_strPhyId,
        m_strGuarPhone,
        //  m_strModHost,
        string.IsNullOrEmpty(m_strIcd9_2) ? "NULL" : string.Format("'{0}'", m_strIcd9_2), //m_strIcd9_2,
        string.IsNullOrEmpty(m_strIcd9_3) ? "NULL" : string.Format("'{0}'", m_strIcd9_3), //m_strIcd9_3,
        string.IsNullOrEmpty(m_strIcd9_4) ? "NULL" : string.Format("'{0}'", m_strIcd9_4), //m_strIcd9_4,
        string.IsNullOrEmpty(m_strIcd9_5) ? "NULL" : string.Format("'{0}'", m_strIcd9_5), //m_strIcd9_5,
        string.IsNullOrEmpty(m_strIcd9_6) ? "NULL" : string.Format("'{0}'", m_strIcd9_6), //m_strIcd9_6,
        string.IsNullOrEmpty(m_strIcd9_7) ? "NULL" : string.Format("'{0}'", m_strIcd9_7), //m_strIcd9_7,
        m_strRelation,
        m_strGuarantor,
        m_strGuarAddr,
        m_strGuarCitySt,
        m_strPat_marital,
        string.IsNullOrEmpty(m_strIcd9_1) ? "NULL" : string.Format("'{0}'", m_strIcd9_1), //m_strIcd9_1,
        m_strHneEpiNumber, strWhere);

        return SQLExec(strSQL, out m_strErrMsg);
    }

} // do not go below this line


