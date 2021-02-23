using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.IO;

namespace LabBilling.Forms
{
    public partial class PatBillForm : Form
    {
        private BindingSource masterBindingSource = new BindingSource();
        private BindingSource detailBindingSourceAcc = new BindingSource();
        private BindingSource detailBingingSourceEnctr = new BindingSource();
        private BindingSource detailBindingSourceActv = new BindingSource();
        public string m_strServer = null;
        public string m_strDatabase = null;
        DataSet m_dsPatBill = new DataSet();
        public PatBillForm()
        {
            InitializeComponent();
        }

        private void PatBill_Load(object sender, EventArgs e)
        {
            Application.DoEvents();
            InitializeDataGrid();
            scMain.SplitterDistance = this.Height / 2;
            scStmtAcc.SplitterDistance = (scMain.SplitterDistance / 2) ;
            scEnctrActv.SplitterDistance = (scMain.SplitterDistance / 2) ;
            
        }

        private void InitializeDataGrid()
        {
            //dgvStatement.DataSource = masterBindingSource;
            //dgvAccount.DataSource = detailBindingSourceAcc;
            //dgvEncounter.DataSource = detailBingingSourceEnctr;
            //dgvActivity.DataSource = detailBindingSourceActv;
            dgvStatement.DataSource = null;
            dgvAccount.DataSource = null ;
            dgvEncounter.DataSource = null;
            dgvActivity.DataSource = null;
            
            //dgvAccount = detailBindingSourceAcc;
            //dgvEncounter = detailBingingSourceEnctr;
            //dgvActivity = detailBindingSourceActv;
            using (SqlConnection conn = new SqlConnection(
            string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                    m_strServer, m_strDatabase)))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
               // DataSet ds = new DataSet();
                DataTable dtStmt = new DataTable("STMT");
                DataTable dtBMSG = new DataTable("BMSG");
                DataTable dtAcct = new DataTable("ACCT");
                DataTable dtEnctr = new DataTable("ENCTR");
                DataTable dtActv = new DataTable("ACTV");
                DataTable dtAcctMsg = new DataTable("ACCT_MSG");
                m_dsPatBill.Tables.Add(dtStmt);
                m_dsPatBill.Tables.Add(dtBMSG);
                m_dsPatBill.Tables.Add(dtAcct);
                m_dsPatBill.Tables.Add(dtAcctMsg);
                m_dsPatBill.Tables.Add(dtEnctr);
                m_dsPatBill.Tables.Add(dtActv);
                SqlCommand cmdClear = new SqlCommand();
                DateTime dtBillFor = DateTime.Today.AddDays(DateTime.Today.Day*-1);
                string strBatchId = string.Format("{0}{1}", dtBillFor.Year.ToString("D4"), dtBillFor.Month.ToString("D2"));
                SqlCommand cmdHDR = new SqlCommand();

                SqlCommand cmdSelectStmt = new SqlCommand(
                    string.Format(
                    "select * from dbo.patbill_stmt " +
                    "WHERE dbo.patbill_stmt.batch_id = '{0}' "+
                    " and nullif(statement_submitted_dt_tm,'') is null "+
                    "order by statement_number, record_cnt", strBatchId)
                    , conn);


                
                SqlCommand cmdSelectAcc = new SqlCommand(
                    string.Format("select account_id as [account], * from dbo.patbill_acc  "+
                    "where batch_id = '{0}' "+
                    " and nullif(date_sent,'') is null " +
                    "order by statement_number, record_cnt_acct ", strBatchId)
                    , conn);

                
                SqlCommand cmdSelectEnctr = new SqlCommand(
                    string.Format(
                    "select * from dbo.patbill_enctr " +
                    "WHERE batch_id = '{0}' and statement_number in ( " +
                    "select statement_number from dbo.patbill_acc where batch_id = '{0}' "+
                    "and nullif(date_sent,'') is null "+
                    " ) "+
                    "order by statement_number, record_cnt",strBatchId)
                    ,conn);

                
               
                SqlCommand cmdSelectEnctrActv = new SqlCommand(
                    string.Format(
                    "select * from dbo.patbill_enctr_actv  "+
                    "WHERE batch_id = '{0}' and statement_number in ( " +
                    "select statement_number from dbo.patbill_acc where batch_id = '{0}' " +
                    "and nullif(date_sent,'') is null "+
                    " ) " +
                    "order by statement_number,  parent_activity_id, record_cnt", strBatchId)
                    ,conn);

                
                SqlCommand cmdSelectNotices = new SqlCommand(
                    string.Format("select * from pat_statements_cerner "+
                    "where batch_id = '{0}' "+
                    "order by statement_number,  activity_id, record_cnt ",strBatchId)
                    , conn);

                

                try
                {
                   

                    sda.SelectCommand = cmdSelectStmt;
                    int nRec = sda.Fill(m_dsPatBill.Tables["STMT"]);
                    dgvStatement.DataSource = m_dsPatBill.Tables["STMT"];
                    
            
                    //ds.Tables["STMT"].PrimaryKey = 
                    //    new DataColumn[] {ds.Tables["STMT"].Columns["statement_number"]};
                    //dgvStatement.DataSource = ds.Tables["STMT"];

                    sda.SelectCommand = cmdSelectAcc;
                    nRec = sda.Fill(m_dsPatBill.Tables["ACCT"]);
                    dgvAccount.DataSource = m_dsPatBill.Tables["ACCT"];

                    sda.SelectCommand = cmdSelectEnctr;
                    nRec = sda.Fill(m_dsPatBill.Tables["ENCTR"]);
                    dgvEncounter.DataSource = m_dsPatBill.Tables["ENCTR"];
                    

                    sda.SelectCommand = cmdSelectEnctrActv;
                    nRec = sda.Fill(m_dsPatBill.Tables["ACTV"]);
                    dgvActivity.DataSource = m_dsPatBill.Tables["ACTV"];

                    sda.SelectCommand = cmdSelectNotices;
                    nRec = sda.Fill(m_dsPatBill.Tables["ACCT_MSG"]);

                }
                catch (SqlException se)
                {
                    string strError = se.Message;
                }
                finally
                {
                    
                }
            }
        }

        private void tsbCreateFile_Click(object sender, EventArgs e)
        {
            // wdk 20160725 moved to new server
            //string strFileName = string.Format(@"\\mclcom1\c$\temp\PatStatement{0}.txt", DateTime.Now.ToString("yyyyMMddHHmm"));
            string strFileName = string.Format(@"\\mclftp\mclftp_d\temp\PatStatement{0}.txt", DateTime.Now.ToString("yyyyMMddHHmm"));
            StreamWriter sw = new StreamWriter(strFileName);
            sw.AutoFlush = true;
            sw.Write(string.Format("HDR~MCL~~CERNER~MCL~{0}~{1}~T~N~0~0~0\r\n"
                ,DateTime.Now.ToString("yyyyMMdd")
                ,DateTime.Now.ToString("HHmmss")));
           
            

            foreach (DataRow dr in m_dsPatBill.Tables["STMT"].Rows)
            {
                
                // each statement
                DataRow[] drAcc = m_dsPatBill.Tables["ACCT"].Select(
                    string.Format("statement_number = '{0}'", dr["statement_number"].ToString()));

                
                sw.Write(string.Format("STMT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}"+
                    "|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}" +
                    "|{36}|{37}|{38}|{39}|{40}|{41}|{42}|{43}|{44}|{45}|{46}|{47}|{48}|{49}|{50}|{51}|{52}|{53}|{54}|{55}" +
                    "|{56}|{57}|{58}|{59}|{60}|{61}|{62}|{63}|{64}|{65}|{66}|{67}|{68}|{69}|{70}|{71}|{72}\r\n",
                    dr["record_cnt"].ToString(),              //0
                    dr["statement_number"].ToString(),                    
                    dr["billing_entity_street"].ToString().ToUpper(),
                    dr["billing_entity_city"].ToString().ToUpper(),
                    dr["billing_entity_state"].ToString().ToUpper(),
                    dr["billing_entity_zip"].ToString().ToUpper(),            //5
                    dr["billing_entity_federal_tax_id"].ToString().ToUpper(),
                    dr["billing_entity_name"].ToString().ToUpper(),
                    dr["billing_entity_phone_number"].ToString().ToUpper(),
                    dr["billing_entity_fax_number"].ToString().ToUpper(),
                    dr["remit_to_street"].ToString().ToUpper(),               //10
                    dr["remit_to_street2"].ToString().ToUpper(),
                    dr["remit_to_city"].ToString().ToUpper(),
                    dr["remit_to_state"].ToString().ToUpper(),
                    dr["remit_to_zip"].ToString().ToUpper(),
                    dr["remit_to_org_name"].ToString().ToUpper(),             //15
                    dr["guarantor_street"].ToString().ToUpper(),
                    dr["guarantor_street_2"].ToString().ToUpper(),
                    dr["guarantor_city"].ToString().ToUpper(),
                    dr["guarantor_state"].ToString().ToUpper(),
                    dr["guarantor_zip"].ToString().ToUpper(),
                    dr["guarantor_name"].ToString().ToUpper(),                //21
                    dr["amount_due"].ToString().ToUpper(),
                    dr["date_due"].ToString().ToUpper(),
                    dr["balance_forward"].ToString().ToUpper(),
                    dr["aging_bucket_current"].ToString().ToUpper(),
                    dr["aging_bucket_30_day"].ToString().ToUpper(),
                    dr["aging_bucket_60_day"].ToString().ToUpper(),
                    dr["aging_bucket_90_day"].ToString().ToUpper(),
                    dr["statement_total_amount"].ToString().ToUpper(),
                    dr["insurance_billed_amount"].ToString().ToUpper(),
                    dr["balance_forward_amount"].ToString().ToUpper(),
                    dr["balance_forward_date"].ToString().ToUpper(),          //30
                    dr["primary_health_plan_name"].ToString().ToUpper(),
                    dr["prim_health_plan_policy_number"].ToString().ToUpper(),
                    dr["prim_health_plan_group_number"].ToString().ToUpper(),
                    dr["secondary_health_plan_name"].ToString().ToUpper(),
                    dr["sec_health_plan_policy_number"].ToString().ToUpper(),
                    dr["sec_health_plan_group_number"].ToString().ToUpper(),
                    dr["tertiary_health_plan_name"].ToString().ToUpper(),
                    dr["ter_health_plan_policy_number"].ToString().ToUpper(),
                    dr["ter_health_plan_group_number"].ToString().ToUpper(),
                    dr["statement_time"].ToString().ToUpper(),                //40
                    dr["page_number"].ToString().ToUpper(),
                    dr["insurance_pending"].ToString().ToUpper(),
                    dr["unpaid_balance"].ToString().ToUpper(),
                    dr["patient_balance"].ToString().ToUpper(),
                    dr["totat_paid_since_last_stmt"].ToString().ToUpper(),
                    dr["insurance_discount"].ToString().ToUpper(),
                    dr["contact text"].ToString().ToUpper(),
                    dr["transmission_dt_tm"].ToString().ToUpper(),
                    dr["guarantor_country"].ToString().ToUpper(),
                    dr["guarantor_ssn"].ToString().ToUpper(),                 //50
                    dr["guarantor_phone"].ToString().ToUpper(),
                    dr["statement_submitted_dt_tm"].ToString().ToUpper(),
                    dr["includes_est_pat_lib"].ToString().ToUpper(),
                    dr["total_charge_amount"].ToString().ToUpper(),
                    dr["non_covered_charge_amt"].ToString().ToUpper(),
                    dr["ABN_charge_amt"].ToString().ToUpper(),
                    dr["est_contract_allowance_amt_ind"].ToString().ToUpper(),
                    dr["est_contract_allowance_amt"].ToString().ToUpper(),
                    dr["encntr_deductible_rem_amt_ind"].ToString().ToUpper(),
                    dr["deductible_applied_amt"].ToString().ToUpper(),        //60
                    dr["encntr_copay_amt_ind"].ToString().ToUpper(),
                    dr["encntr_copay_amt"].ToString().ToUpper(),
                    dr["encntr_coinsurance_pct_ind"].ToString().ToUpper(),
                    dr["encntr_coinsurance_pct"].ToString().ToUpper(),
                    dr["encntr_coinsurance_amt"].ToString().ToUpper(),
                    dr["maximum_out_of_pocket_amt_ind"].ToString().ToUpper(),
                    dr["amt_over_max_out_of_pocket"].ToString().ToUpper(),
                    dr["est_patient_liab_amt"].ToString().ToUpper(),
                    dr["online_billpay_url"].ToString().ToUpper(),
                    dr["guarantor_access_code"].ToString().ToUpper()          //70

                    ));

                string strSelect = string.Format("statement_type = 'SMSG' and  statement_type_id = '{0}' ",
                    dr["statement_number"].ToString());

                if (m_dsPatBill.Tables["ACCT_MSG"].Rows.Count > 0)
                {
                    DataRow[] drStmtMsg = m_dsPatBill.Tables["ACCT_MSG"].Select(
                        string.Format("statement_type = 'SMSG' and  statement_type_id = '{0}' ",
                        dr["statement_number"].ToString()));


                    for (int iSmsg = 0; iSmsg <= drStmtMsg.GetUpperBound(0); iSmsg++)
                    {
                        sw.Write(string.Format("{0}|{1}|{2}\r\n",
                            drStmtMsg[iSmsg]["statement_type"].ToString().ToUpper(),
                            (iSmsg + 1).ToString().ToUpper(),
                            drStmtMsg[iSmsg]["statement_text"].ToString().ToUpper()));

                    }

                }
                // each account
                for (int iAcc = 0; iAcc <= drAcc.GetUpperBound(0); iAcc++)
                {
                    sw.Write(string.Format("ACCT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}|{15}"+
                        "|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}" +
                        "\r\n",
                        (iAcc + 1).ToString().ToUpper(),//drAcc[iAcc]["record_cnt_acct"].ToString(), //0
                        drAcc[iAcc]["patient_account_number"].ToString().ToUpper(),
                        drAcc[iAcc]["account_id"].ToString().ToUpper(),
                        drAcc[iAcc]["pat_name"].ToString().ToUpper(),
                        drAcc[iAcc]["account_subtotal"].ToString().ToUpper(),                 //4
                        drAcc[iAcc]["total_account_subtotal"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_amt_due"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_ins_pending"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_dates_of_service"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_unpaid_bal"].ToString().ToUpper(),                  //9
                        drAcc[iAcc]["acct_patient_bal"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_paid_since_last_stmt"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_ins_discount"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_date_due"].ToString().ToUpper(),
                        drAcc[iAcc]["acct_health_plan_name"].ToString().ToUpper(),            //14
                        drAcc[iAcc]["patient_date_of_birth"].ToString().ToUpper(),
                        drAcc[iAcc]["patient_date_of_death"].ToString().ToUpper(),
                        drAcc[iAcc]["patient_sex"].ToString().ToUpper(),
                        drAcc[iAcc]["patient_vip"].ToString().ToUpper(),
                        drAcc[iAcc]["includes_est_pat_lib"].ToString().ToUpper(),
                        drAcc[iAcc]["total_charge_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["non_covered_charge_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["ABN_charge_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["est_contract_allowance_amt_ind"].ToString().ToUpper(),
                        drAcc[iAcc]["est_contract_allowance_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["encntr_deductible_rem_amt_ind"].ToString().ToUpper(),
                        drAcc[iAcc]["deductible_applied_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["encntr_copay_amt_ind"].ToString().ToUpper(),
                        drAcc[iAcc]["encntr_copay_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["encntr_coinsurance_pct_ind"].ToString().ToUpper(),
                        drAcc[iAcc]["encntr_coinsurance_pct"].ToString().ToUpper(),
                        drAcc[iAcc]["encntr_coinsurance_amt"].ToString().ToUpper(),
                        drAcc[iAcc]["maximum_out_of_pocket_amt_ind"].ToString().ToUpper(),
                        drAcc[iAcc]["amt_over_max_out_of_pocket"].ToString().ToUpper(),
                        drAcc[iAcc]["est_patient_liab_amt"].ToString().ToUpper()

                        ));

                    // each encounter
                    DataRow[] drEnct = m_dsPatBill.Tables["ENCTR"].Select(
                    string.Format("statement_number = '{0}' and record_cnt = '{1}'"
                        , dr["statement_number"].ToString().ToUpper(), drAcc[iAcc]["record_cnt_acct"].ToString().ToUpper()));


                    for (int iEnctr = 0; iEnctr <= drEnct.GetUpperBound(0); iEnctr++)
                    {
                        sw.Write(string.Format("ENCT|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}|{12}|{13}|{14}"+
                        "|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}|{26}|{27}|{28}|{29}|{30}|{31}|{32}|{33}|{34}|{35}" +
                        "|{36}|{37}|{38}|{39}|{40}|{41}|{42}|{43}|{44}|{45}|{46}|{47}|{48}|{49}|{50}|{51}|{52}|{53}|{54}|{55}" +
                        "|{56}|{57}|{58}|{59}|{60}|{61}|{62}|{63}|{64}|{65}|{66}|{67}|{68}|{69}|{70}|{71}|{72}|{73}\r\n",
                        (iEnctr+1).ToString(),//drEnct[iEnctr]["record_cnt"].ToString(), //0
                        drEnct[iEnctr]["enctr_nbr"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_id"].ToString().ToUpper(),
                        drEnct[iEnctr]["place_of_service"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_dates_of_service"].ToString().ToUpper(),   //4
                        drEnct[iEnctr]["pft_encntr_amt_due"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_name"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_name"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_str_addr_"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_str_addr_2"].ToString().ToUpper(),    //9
                        drEnct[iEnctr]["pft_encntr_prov_org_str_addr_3"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_str_addr_4"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_city"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_state"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_zip"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_org_phone"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_prov_hrs"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_unpaid_bal"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_patient_bal"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_paid_since_last_stmt"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_ins_discount"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_ord_mgmt_act_type"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_ord_mgmt_cat_type"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_health_plan_name"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_in_pending"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_total"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_admit_dt_tm"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_discharge_dt_tm"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_medical_service"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_type"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_financial_class"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_vip"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_qualifier"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_total_charges"].ToString().ToUpper(),
                        drEnct[iEnctr]["total_patient_payments"].ToString().ToUpper(),
                        drEnct[iEnctr]["total_patient_adjustments"].ToString().ToUpper(),
                        drEnct[iEnctr]["total_insurance_payments"].ToString().ToUpper(),
                        drEnct[iEnctr]["total_insurance_adjustments"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_assigned_agency"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_pay_plan_flag"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_pay_plan_status"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_pay_plan_orig_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_pay_plan_pay_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_pay_plan_begin_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_encntr_pay_plan_delinq_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["pftectr_pri_clm_orig_trans_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pftectr_pri_clm_cur_trans_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pftectr_sec_clm_orig_trans_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pftectr_sec_clm_cur_trans_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pftectr_ter_clm_orig_trans_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pftectr_ter_clm_cur_trans_dttm"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_ectr_prim_insr_balance"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_ectr_sec_insr_balance"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_ectr_tert_insr_balance"].ToString().ToUpper(),
                        drEnct[iEnctr]["pft_ectr_self_pay_balance"].ToString().ToUpper(),
                        drEnct[iEnctr]["attending_physician_name"].ToString().ToUpper(),
                        drEnct[iEnctr]["includes_est_pat_liab"].ToString().ToUpper(),
                        drEnct[iEnctr]["total_charge_amount"].ToString().ToUpper(),
                        drEnct[iEnctr]["non_covered_charge_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["ABN_charge_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["est_contract_allowance_amt_ind"].ToString().ToUpper(),
                        drEnct[iEnctr]["est_contract_allowance_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_deductible_rem_amt_ind"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_deductible_rem_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["deductible_applied_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_copay_amt_ind"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_copay_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_coinsurance_pct_ind"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_coinsurance_pct"].ToString().ToUpper(),
                        drEnct[iEnctr]["encntr_coinsurance_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["maximum_out_of_pocket_amt_ind"].ToString().ToUpper(),
                        drEnct[iEnctr]["maximum_out_of_pocket_amt"].ToString().ToUpper(),
                        drEnct[iEnctr]["amt_over_max_out_of_pocket"].ToString().ToUpper(),
                        drEnct[iEnctr]["est_patient_liab_amt"].ToString().ToUpper()

                        ));

                        if (m_dsPatBill.Tables["ACCT_MSG"].Rows.Count > 0)
                        {
                            DataRow[] drEnctMsg = m_dsPatBill.Tables["ACCT_MSG"].Select(
                            string.Format("statement_type = 'EMSG' and  statement_type_id = '{0}' and account = '{1}'",
                            dr["statement_number"].ToString(), drEnct[iEnctr]["pft_encntr_id"].ToString().ToUpper()));

                            for (int iEmsg = 0; iEmsg <= drEnctMsg.GetUpperBound(0); iEmsg++)
                            {
                                sw.Write(string.Format("{0}|{1}|{2}|{3}\r\n",
                                    drEnctMsg[iEmsg]["statement_type"].ToString().ToUpper(),
                                    (iEmsg + 1).ToString().ToUpper(),
                                    drEnctMsg[iEmsg]["account"].ToString().ToUpper(),
                                    drEnctMsg[iEmsg]["statement_text"].ToString().ToUpper()));
                            }
                        }
                        DataRow[] drActv = m_dsPatBill.Tables["ACTV"].Select(
                                string.Format("statement_number = '{0}' and parent_activity_id = '{1}'"
                                , dr["statement_number"].ToString().ToUpper()
                                , drEnct[iEnctr]["record_cnt"].ToString().ToUpper()));
                        for (int iActv = 0; iActv <= drActv.GetUpperBound(0); iActv++)
                        {

                            sw.Write(string.Format("ACTV|{0}|{1}|{2}|{3}|{4}|{5}|{6}|{7}|{8}|{9}|{10}|{11}"+
                                "|{12}|{13}|{14}|{15}|{16}|{17}|{18}|{19}|{20}|{21}|{22}|{23}|{24}|{25}" +
                                "|{26}|{27}" +
                                "\r\n",
                            (iActv + 1).ToString().ToUpper(),//drActv[iActv]["record_cnt"].ToString(), //0
                            drActv[iActv]["enctr_nbr"].ToString().ToUpper(),
                            drActv[iActv]["activity_id"].ToString().ToUpper(),
                            drActv[iActv]["activity_type"].ToString().ToUpper(),
                            drActv[iActv]["activity_date"].ToString().ToUpper(),                  //4
                            drActv[iActv]["activity_description"].ToString().ToUpper(),
                            drActv[iActv]["activity_code"].ToString().ToUpper(),
                            drActv[iActv]["activity_amount"].ToString().ToUpper(),
                            drActv[iActv]["units"].ToString().ToUpper(),
                            drActv[iActv]["cpt_code"].ToString().ToUpper(),                       //9
                            drActv[iActv]["cpt_description"].ToString().ToUpper(),
                            drActv[iActv]["drg_code"].ToString().ToUpper(),
                            drActv[iActv]["revenue_code"].ToString().ToUpper(),
                            drActv[iActv]["revenue_code_description"].ToString().ToUpper(),
                            drActv[iActv]["hcpcs_code"].ToString().ToUpper(),
                            drActv[iActv]["hcpcs_description"].ToString().ToUpper(),
                            drActv[iActv]["order_mgmt_activity_type"].ToString().ToUpper(),
                            drActv[iActv]["activity_amount_due"].ToString().ToUpper(),
                            drActv[iActv]["activity_date_of_service"].ToString().ToUpper(),
                            drActv[iActv]["activity_patient_bal"].ToString().ToUpper(),
                            drActv[iActv]["activity_ins_discount"].ToString().ToUpper(),
                            drActv[iActv]["activity_trans_type"].ToString().ToUpper(),
                            drActv[iActv]["activity_trans_sub_type"].ToString().ToUpper(),
                            drActv[iActv]["activity_trans_amount"].ToString().ToUpper(),
                            drActv[iActv]["activity_health_plan_name"].ToString().ToUpper(),
                            drActv[iActv]["activity_ins_pending"].ToString().ToUpper(),
                            drActv[iActv]["activity_dr_cr_flag"].ToString().ToUpper(),
                            drActv[iActv]["parent_activity_id"].ToString().ToUpper()
                            
                            ));
                            



                        }

                    }
                }
                //if (dr["statement_number"].ToString() == "20150500005")
                //{
                //    break;
                //}
                
            }

            sw.Write("TRL~MCL\r\n");
            sw.Close();

            //if (System.IO.File.Exists(strFileName))
            //{
            //    System.IO.File.Delete(strFileName);
            //}
            //using (System.IO.FileStream fs = System.IO.File.Create(strFileName, 1024))
            //{
            //    byte[] info = new System.Text.UTF8Encoding(true).GetBytes("This is some stuff");
            //    fs.Write(info, 0, info.Length);

            //}
            //// Open the file and read it back.
            //using (System.IO.StreamReader sr = System.IO.File.OpenText(strFileName))
            //{
            //    string s = "";
            //    while ((s = sr.ReadLine()) != null)
            //    {
            //        MessageBox.Show(s);
            //    }
            //}
            MessageBox.Show("file created");

        }

        private void dgvEnter(object sender, EventArgs e)
        {
            tsslCount.Text = string.Format("Records: {0}", ((DataGridView)sender).Rows.Count);
            //int rowCount = ((DataGridView)sender).Rows.Count;
            //tsslCount.Text = string.Format("Records: {0}", rowCount);
        }



    }
}
