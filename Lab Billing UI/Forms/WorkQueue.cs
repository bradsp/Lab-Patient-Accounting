using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// programmer added
using System.Data.SqlClient;
using System.Collections;
// programmer added
using RFClassLibrary;
using MCL;
using System.Drawing.Printing;
using System.Reflection;
using LabBilling.Logging;
using LabBilling.Core.BusinessLogic.Validators;
using System.Data.Common;

namespace LabBilling.Forms
{
    public partial class WorkqueueForm : Form
    {
        //ArrayList m_alMethodNameINSC = null;
        //ArrayList m_alMethodNameMethods = null;
        bool m_bExcludeHardCodedCode = true;
        ArrayList m_alMethods = new ArrayList();
        DataTable m_dtViewerEdits = null;
        /// <summary>
        /// Calculates the time between two dates
        /// </summary>
        public string propDurationToString
        {
            get
            {
                Log.Instance.Debug("Entering");
                double dDay = (int)dSec / (3600 * 24);
                double dHour = (int)dSec / 3600 == 0 ? ((int)dSec % (3600 * 24)) : ((int)dSec % (3600 * 24)) % (int)dSec / 3600;

                double dMin = (int)(dSec % 3600) / 60; ;
                double dSecs = (int)((dSec % 3600) % 60);
                return string.Format("{0}{1}Minutes {2} and Seconds {3}",
                    double.Parse(dDay.ToString("F0")) > 0 ?
                    string.Format("Day{1} {0} ",
                        dDay.ToString("F0"),
                        int.Parse(dDay.ToString("F0")) > 1 ? "s" : ""
                    ) : "",

                    double.Parse(dHour.ToString("F0")) > 0 ? string.Format("Hours {0} ",
                    dHour.ToString("F0")) : "",
                    dMin.ToString("F0"), dSecs.ToString("F2"));

            }
            set
            {
                Log.Instance.Debug("Entering");
                if (!double.TryParse(value, out dSec))
                {
                    dSec = 0.00;
                }
            }
        }
        private double dSec = 0.00;
        DataTable m_dtMultiUnitCpt4 = null;
        DataTable m_dtGlobalBilling = null;
        SortedDictionary<string, int> m_dicErrorTotals;
        string m_strRequeryColFilter = "";
        string m_strRequery;
        DateTime m_dtFrom;
        DateTime m_dtThru;
        string m_strOutPatientBilltype = "and [billing type] in ('OUTPATIENT','REF LAB')";
        private DateTime m_dtOutpatient;
        private DateTime m_dtNHBillthru;
        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;
        /// <summary>
        /// Provides the Validation outines a thread safe member variable to use to check the account's validity.
        /// </summary>
        //DataGridViewRow m_drCur = null;
        DataRow m_drCur = null;
        DataRow m_drCurIns = null;
        DataTable m_dtDictCpt4Warnings = new DataTable();
        ArrayList m_alDiagCodes = new ArrayList();
        // ArrayList m_alZip = new ArrayList();
        Dictionary<string, ListObject> m_dicZip = new Dictionary<string, ListObject>();
        int nFilterColumn = -1;
        ArrayList m_alDiagnosis = new ArrayList();
        //  private R_lmrp m_rLmrp = null;
        private DateTime m_dtStartSSI = DateTime.MinValue;
        //  private R_mutually_excl m_rMutex = null;
        //  private R_acc m_rAcc = null;
        private R_VW_LMRP m_rVwLmrp = null;
        private R_icd9desc m_rIcd9desc = null;
        private R_dict_a_cpedit m_rAEdit = null;
        private R_dict_c_meedit m_rCEdit = null;
        private ArrayList m_alTests = null;
        private ArrayList m_alTestsModi = null;
        private R_notes m_rNotes = null;
        private string m_strProductionEnvironment = null;
        private ERR m_Err = null;
        private R_h1500 m_rH1500 = null;
        private R_ub m_rUb = null;
        ArrayList m_alFinCodeProcessing = null;
        //private BackgroundWorker worker;
        private SqlDataAdapter m_daAcc = null;
        private DataSet m_dsAccount = null;
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        ToolStripControlHost m_cboxInclude; // CheckBox
        ToolStripLabel m_lblInclude; // Label
        private SqlConnection m_sqlConn = null;
        private string m_strServer = null;
        private string m_strDatabase = null;
        private string _connectionString = null;
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }


        /// <summary>
        /// Receives three arguments from the launching application
        /// </summary>
        /// <param name="args">args[0] = production environment
        ///                    args[1] = SQL server
        ///                    args[2] = SQL database</param>
        public WorkqueueForm(string connectionString)
        {
            Log.Instance.Debug($"Entering");
            InitializeComponent();

            _connectionString = connectionString;

            DbConnectionStringBuilder dbConnectionStringBuilder = new DbConnectionStringBuilder();
            dbConnectionStringBuilder.ConnectionString = connectionString;

            m_strServer = (string)dbConnectionStringBuilder["Server"];
            m_strDatabase = (string)dbConnectionStringBuilder["Database"];

            m_strProductionEnvironment = m_strDatabase == "MCLLIVE" ? "LIVE" : "TEST";

            string[] strArgs = new string[] { string.Format("/{0}", m_strProductionEnvironment),
                m_strServer, m_strDatabase};
            m_Err = new ERR(strArgs);
            m_rNotes = new R_notes(m_strServer, m_strDatabase, ref m_Err);
            m_rAEdit = new R_dict_a_cpedit(m_strServer, m_strDatabase, ref m_Err);
            m_rCEdit = new R_dict_c_meedit(m_strServer, m_strDatabase, ref m_Err);
            m_rIcd9desc = new R_icd9desc(m_strServer, m_strDatabase, ref m_Err);
            m_rVwLmrp = new R_VW_LMRP(m_strServer, m_strDatabase, ref m_Err);
            m_rH1500 = new R_h1500(m_strServer, m_strDatabase, ref m_Err);
            m_rUb = new R_ub(m_strServer, m_strDatabase, ref m_Err);

            m_dsAccount = new DataSet("ACCOUNT");
            this.Text += string.Format(" {0}", m_strDatabase);

            //          InitializeBackgroundWorkder();
        }

        //private void InitializeBackgroundWorkder()
        //{
        //    // backgroundworker
        //    worker = new BackgroundWorker();
        //    worker.DoWork += new DoWorkEventHandler(worker_DoWork);
        //    worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(worker_RunWorkerCompleted);
        //    worker.ProgressChanged += new ProgressChangedEventHandler(worker_ProgressChanged);

        //    worker.WorkerReportsProgress = true;
        //    worker.WorkerSupportsCancellation = true;

        //}

        //void worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //    tspbCount.Value = 0;
        //  //  dgvAccount.Refresh();
        //    this.Invalidate();

        //}

        //void worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        //{
        //    tspbCount.PerformStep();
        //}

        private void Load_frmAcc(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            Application.DoEvents();
            tsslNote.Text = "Creating DataSets";
            Log.Instance.Debug("Timer Start Creating DataSets");
            CreateDataSet();
            Log.Instance.Debug("Timer Stop Creating DataSets");
            Application.DoEvents();
            tsslNote.Text = "Creating ArrayLists";
            Log.Instance.Debug("Timer Start Creating Array Lists");
            CreateArrayLists();
            Log.Instance.Debug("Timer Stop Creating Array Lists");
            Application.DoEvents();

            m_sqlConn = new SqlConnection(_connectionString);

            SqlDataAdapter sda = new SqlDataAdapter();
            Application.DoEvents();
            Log.Instance.Debug("Timer Start Creating DateTimes");
            CreateDateTimes();
            Log.Instance.Debug("Timer Stop Creating DateTimes");

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                Application.DoEvents();
                tsslNote.Text = "Loading Table Edits";
                Log.Instance.Debug("Timer start Loading Table Edits");
                // get the operating date range for queries.

                SqlCommand cdmDictionary =
                    new SqlCommand("select  fin_code, ins_code, bill_form,valid, strSql, " +
                        "effective_date, coalesce(expire_date,getdate()), error " +
                        "from dict_ViewerAccSql " +
                        "where effective_date <= getdate()", connection);
                sda.SelectCommand = cdmDictionary;
                tsslNote.Text = "Loading Select Command";
                m_dtViewerEdits = new DataTable();
                sda.Fill(m_dtViewerEdits);
                Log.Instance.Debug("Timer stop Loading Table Edits");
            }

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {

                SqlCommand cmdEndDate = new SqlCommand("select value from dbo.system where key_name = 'ssi_bill_thru_date'", connection);
                sda.SelectCommand = cmdEndDate;
                DataTable dtEnd = new DataTable();
                sda.Fill(dtEnd);
                DateTime dtThruDate = DateTime.Parse(dtEnd.Rows[0]["value"].ToString());
                if (Control.IsKeyLocked(Keys.Scroll) && Environment.UserName.ToUpper() == "WKELLY")
                {
                    dtThruDate = DateTime.Today;
                }
                ((DateTimePicker)m_dpFrom.Control).MaxDate = dtThruDate;
                ((DateTimePicker)m_dpThru.Control).MaxDate = dtThruDate;
            }

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                // when value is true all methods contain the check will be excluded.
                // when the restricted users are parsed into the Method's array and the methods name is 
                //      in the array it will be eliminated.
                SqlCommand cmdIncludeMethods = new SqlCommand("select value,restricted_users as [methods] from dbo.system where key_name = 'DIAGNOSIS'", connection);
                sda.SelectCommand = cmdIncludeMethods;
                DataTable dtExclude = new DataTable();
                sda.Fill(dtExclude);
                m_bExcludeHardCodedCode = bool.Parse(dtExclude.Rows[0]["value"].ToString());
                m_alMethods = new ArrayList(dtExclude.Rows[0]["methods"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

                //SqlCommand cmdIncludeMethods = new SqlCommand("select value,restricted_users as [methods] from dbo.system where key_name = 'DIAGNOSIS'", connection);
                //sda.SelectCommand = cmdIncludeMethods;
                //DataTable dtInclude = new DataTable();
                //string[] strNoFail = new string[99]();
                //sda.Fill(dtInclude);
                ////m_bExcludeHardCodedCode = bool.Parse(dtInclude.Rows[0]["value"].ToString());
                //m_alMethodNameMethods 
                //    =  new ArrayList(dtInclude.Rows[0]["methods"].ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries));

            }

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                SqlCommand cdmNHBillThruDate =
                    new SqlCommand("select value from dbo.system where key_name = 'nh_bill_thru'", connection);
                sda.SelectCommand = cdmNHBillThruDate;
                DataTable dtStart = new DataTable();
                sda.Fill(dtStart);
                m_dtNHBillthru = DateTime.Parse(dtStart.Rows[0]["value"].ToString());
            }
            //   ts.Add(DateTime.Now - dteStart); 
            //   tsslNote.Text = string.Format("Global Billing Date Loaded in {0} from a total of {1}", ts.Seconds
            //       , ts.Seconds);

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                SqlCommand cmdGlobalBilling =
                    new SqlCommand("SELECT distinct  cpt4 " +
                        "FROM dict_global_billing_cdms dict " +
                        "inner join cpt4 on cpt4.cdm = dict.cdm", connection);
                sda.SelectCommand = cmdGlobalBilling;
                m_dtGlobalBilling = new DataTable();
                sda.Fill(m_dtGlobalBilling);
            }
            //    ts.Add(DateTime.Now - dteStart);
            //propDurationToString = "1000000";
            //propDurationToString = "90061";
            //propDurationToString = "180122";
            //  propDurationToString = ts.Seconds.ToString();
            // propDurationToString = "90061";
            //  tsslNote.Text = string.Format("Global Billing Table Loaded in {0} from a total of {1}"
            //      , propDurationToString,ts.Seconds);
            //  alTimes.Add(tsslNote.Text);

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {

                SqlCommand cdmOutPatStartDate = new SqlCommand("select value from dbo.system where key_name = 'outpatient_bill_start'", connection);
                sda.SelectCommand = cdmOutPatStartDate;
                DataTable dtStart = new DataTable();
                sda.Fill(dtStart);
                m_dtOutpatient = DateTime.Parse(dtStart.Rows[0]["value"].ToString());

                SqlCommand cmdMultiUnitCpt4 = new SqlCommand("select cpt4 from dict_multiple_unit_cpt4s", connection);
                sda.SelectCommand = cmdMultiUnitCpt4;
                m_dtMultiUnitCpt4 = new DataTable();
                sda.Fill(m_dtMultiUnitCpt4);
                m_dtMultiUnitCpt4.PrimaryKey = new DataColumn[] { m_dtMultiUnitCpt4.Columns["cpt4"] };

                if (m_dtOutpatient <= DateTime.Today)
                {
                    m_lblInclude.Visible = true;
                    ((CheckBox)m_cboxInclude.Control).Visible = true;
                }
                // ((CheckBox)m_cboxInclude.Control).Visible = true;

                m_daAcc = new SqlDataAdapter();
                // Fin code
                SqlCommand cmdSelectFinCodes =
                    new SqlCommand("SELECT fin_code from fin where deleted = 0", connection);
                DataTable dtFin = new DataTable();
                m_daAcc.SelectCommand = cmdSelectFinCodes;
                m_daAcc.Fill(dtFin);

                foreach (DataRow dr in dtFin.Rows)
                {
                    tscbFinCodes.Items.Add(dr["fin_code"]);

                }
                // Diag Codes
                // wdk 20121001 updated for charges in old ama_year
                int nAmaYear = DateTime.Today.Year;
                int nAmaYearGovernment = DateTime.Today.Year;
                if (DateTime.Today >= new DateTime(DateTime.Today.Year, 10, 1))
                {
                    nAmaYearGovernment++;
                }
                else
                {
                    nAmaYearGovernment--;
                }
                Log.Instance.Debug("Timer start Loading DX Codeset");
                SqlCommand cmdSelectDiagCodes =
                    new SqlCommand(
                        string.Format("select icd9_num from icd9desc where ama_year in ('{0}','{1}')", nAmaYear, nAmaYearGovernment),
                        connection);
                DataTable m_dtDiagCodes = new DataTable();
                m_daAcc.SelectCommand = cmdSelectDiagCodes;
                m_daAcc.Fill(m_dtDiagCodes);
                foreach (DataRow dr in m_dtDiagCodes.Rows)
                {
                    m_alDiagCodes.Add(dr["icd9_num"]);
                }
                Log.Instance.Debug("Timer stop Loading DX Codeset");
                // Zip codes
                SqlCommand cmdSelctZipCodes =
                    new SqlCommand("select zip, st, city from zip", connection);
                DataTable dtZip = new DataTable();
                m_daAcc.SelectCommand = cmdSelctZipCodes;
                m_daAcc.Fill(dtZip);
                foreach (DataRow dr in dtZip.Rows)
                {
                    //   m_alZip.Add(dr["zip"]);
                    m_dicZip.Add(dr["zip"].ToString(), new ListObject(dr["st"].ToString(), dr["city"].ToString()));
                }
                // Dict_Cpt4_Warnings
                SqlCommand cmdSelectCpt4Warning =
                    new SqlCommand("Select deleted, cpt4, note, is_ssi_edit from dict_cpt4_warnings order by cpt4",
                        connection);
                m_daAcc.SelectCommand = cmdSelectCpt4Warning;
                m_daAcc.Fill(m_dtDictCpt4Warnings);

                DateTime tdate = ((DateTimePicker)m_dpThru.Control).Value;

                SqlCommand cmdUpdateAcc = new SqlCommand("UPDATE acc set " +
                    " status = @status WHERE account = @account"
                    , m_sqlConn);
                cmdUpdateAcc.Parameters.Add("@status", SqlDbType.VarChar, 10, "status");
                cmdUpdateAcc.Parameters.Add("@account", SqlDbType.VarChar, 15, "account");
                m_daAcc.UpdateCommand = cmdUpdateAcc;

                int nRows = 0;
                SqlCommand cmdSelectAcc;
                // don't forget to change in post to ssi
                if (DateTime.Today <= new DateTime(2012, 06, 14))
                {
                    cmdSelectAcc = new SqlCommand(
                   "SELECT acc.status, " +
                   " acc.account, acc.pat_name, ins.ins_code, acc.cl_mnem, " +
                   " acc.fin_code " +
                   " ,convert(datetime,convert(varchar(10),acc.trans_date,101)) as [trans_date] " +
                   " ,case when outpatient_billing = 1 and trans_date > '04/01/2012' " +
                   " then 'OUTPATIENT' " +
                   " else 'REF LAB' " +
                   " end as [billing type] " +
                   //" , insc.bill_form "+
                   " , case when outpatient_billing = 1 and trans_date > '04/01/2012' " +
                        " then 'UBOP' " +
                        " else insc.bill_form " +
                        " end as [bill_form]" +
                   " FROM acc " +
                   " inner join pat on pat.account = acc.account " +
                   " left outer join ins on ins.account = acc.account and ins.ins_a_b_c = 'A' " +
                   " left outer join client on client.cli_mnem = acc.cl_mnem " +
                   " left outer join insc on insc.code = ins.ins_code" +
                   " WHERE  acc.account in ('C3815110')"
                   , m_sqlConn);
                    // must have two above to get one to work.

                    m_daAcc.SelectCommand = cmdSelectAcc;

                    nRows = m_daAcc.Fill(m_dsAccount.Tables["ACC"]);
                    dgvAccount.DataSource = m_dsAccount.Tables["ACC"];
                }
                else
                {
                    if (DateTime.Now >= new DateTime(2013, 08, 01, 16, 30, 00))
                    {
                        Query();
                    }
                    else
                    {
                        return;
                    }
                }

                //   _sortedColumns = new List<SortColDefn>(dgvAccount.Columns);
                Application.DoEvents();
                tsslNote.Text = "Loading Min Date to Datetime pickers";

                ((DateTimePicker)m_dpFrom.Control).MinDate = m_dtStartSSI;
                ((DateTimePicker)m_dpThru.Control).MinDate = m_dtStartSSI;

                Application.DoEvents();
                tsslNote.Text = "Loading End Date";

                tsslAccounts.Text = string.Format("ACCOUNTS: {0}", m_dsAccount.Tables["ACC"].Rows.Count);

                SqlCommand cmdSelectPat =
                     new SqlCommand("SELECT  pat.account, acc.pat_name,  pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation " +
                           ", guarantor, guar_addr, g_city_st, pat_marital " +
                           ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
                           ", guar_phone, acc.trans_date,  " +
                           " phy_id, phy.last_name as [phy_last_name], phy.first_name as [phy_first_name], phy.mid_init as [phy_mid_init]" +
                           " FROM     pat " +
                           " inner join acc on acc.account = pat.account and post_date is null and (fin_code < 'w') and status not in ('closed','paid_out') " +
                           " inner join phy on phy.tnh_num = pat.phy_id and phy.deleted = 0 " +
                           " where 1=2",
                            connection);
                m_daAcc.SelectCommand = cmdSelectPat;
                nRows = m_daAcc.Fill(m_dsAccount.Tables["PAT"]);
                dgvPat.DataSource = m_dsAccount.Tables["PAT"];

                SqlCommand cmdSelectIns =
                   new SqlCommand("SELECT distinct ins.account, ins_a_b_c, ins.fin_code, holder_nme, holder_dob, plan_nme, policy_num, cert_ssn, grp_nme, " +
                       " grp_num, holder_sex, ins_code, ins.relation" +
                       " , isnull(guar_addr,pat_addr1) as [Addr], isnull(g_city_st,city_st_zip) as [City_st_zip] " +
                       " , insc.provider_no as [provider_no], insc.bill_form, acc.trans_date " +
                       " FROM  ins " +
                       " inner join acc on acc.account = ins.account and post_date is null and (acc.fin_code < 'w') and status not in ('closed','paid_out') " +
                       " INNER JOIN PAT on pat.account = ins.account" +
                       " LEFT OUTER JOIN INSC on insc.name = ins.plan_nme " +
                       " where 1=2 --deleted = 0 and provider_no is not null ",
                       connection);

                m_daAcc.SelectCommand = cmdSelectIns;
                nRows = m_daAcc.Fill(m_dsAccount.Tables["INS"]);
                dgvInsurance.DataSource = m_dsAccount.Tables["INS"];

                SqlCommand cmdSelectBilling =
                    new SqlCommand("select  chrg.account, sum(qty) as [qty] " +// over (partition by account,cpt4) as [qty] "+
                                                                               //" , cdm, cpt4, type, sum(amount) over (partition by account, cpt4) as [amount], modi, "+
                                                                               //" , cpt4, type, sum(amount) over (partition by account, cpt4) as [amount], modi, "  + wdk 20120810 modified
                        " , cpt4, type, sum(amount) as [amount], modi, " +
                        " revcode, modi2, diagnosis_code_ptr" +
                        " from chrg " +
                        " inner join amt on amt.chrg_num = chrg.chrg_num " +
                        " where 1=2" +
                        " group by chrg.account, " +//qty, cdm, 
                        " cpt4, type,  modi, revcode, modi2, diagnosis_code_ptr"
                        , connection);
                m_daAcc.SelectCommand = cmdSelectBilling;
                nRows = m_daAcc.Fill(m_dsAccount.Tables["BILLING"]);
                dgvBilling.DataSource = m_dsAccount.Tables["BILLING"];


                SqlCommand cmdSelectChrgErr =
                  new SqlCommand("select  account, pat_name, cl_mnem, fin_code, cdm, cpt4, amount, " +
                       " trans_date, service_date, qty, type, error, uri, deleted, mt_reqno, location " +
                      " from chrg_err " +
                      " where 1=2" +
                      " order by account"
                      , connection);
                m_daAcc.SelectCommand = cmdSelectChrgErr;
                nRows = m_daAcc.Fill(m_dsAccount.Tables["CHRG_ERR"]);
                dgvChrgErr.DataSource = m_dsAccount.Tables["CHRG_ERR"];

                ////////
                m_dsAccount.Tables["ACC"].PrimaryKey =
             new DataColumn[] { m_dsAccount.Tables["ACC"].Columns["ACCOUNT"] };

            }
            // grid printing
            m_ViewerPrintDocument = new PrintDocument();
            m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            m_rgReport = new ReportGenerator(dgvAccount, m_ViewerPrintDocument, "ViewerAcc", m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
            m_cboxInclude.Visible = false;
            dgvAccount.Columns["billing type"].Visible = false;
            if (m_dtOutpatient <= DateTime.Today)
            {
                m_cboxInclude.Visible = true;
                dgvAccount.Columns["billing type"].Visible = true;
            }

            m_dicErrorTotals = new SortedDictionary<string, int>();
        }

        private void CreateArrayLists()
        {
            m_alFinCodeProcessing = new ArrayList();
            // m_alFinCodeProcessing.Add("E");
            m_alFinCodeProcessing.Add("CLIENT");
            m_alFinCodeProcessing.Add("W");
            m_alFinCodeProcessing.Add("X");
            m_alFinCodeProcessing.Add("Y");
            m_alFinCodeProcessing.Add("Z");

            m_alTests = new ArrayList();
            m_alTestsModi = new ArrayList();

        }
        /// <summary>
        /// Hopefully a threadsafe way to validate the grid
        /// </summary>
        private string ValidateData(DataRow drAccount)
        {
            Log.Instance.Debug($"Entering");
            StringBuilder sbRetVal = new StringBuilder();
            m_alTests = new ArrayList();
            m_alTestsModi = new ArrayList();

            string strAcc = drAccount["account"].ToString();

            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                SqlDataAdapter daAcc = new SqlDataAdapter();
                m_dsAccount.Tables["PAT"].Rows.Clear();
                SqlCommand cmdSelectPat = new SqlCommand(
                    string.Format("SELECT top(1) pat.account, acc.pat_name,  pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation " +
                           ", guarantor, guar_addr, g_city_st, pat_marital " +
                           ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
                           ", guar_phone, acc.trans_date, " +
                           "phy_id, phy.last_name as [phy_last_name], phy.first_name as [phy_first_name], phy.mid_init as [phy_mid_init]" +
                           " FROM     pat " +
                           " inner join acc on acc.account = pat.account and post_date is null and (fin_code < 'w') and status not in ('closed','paid_out') " +
                           " left outer join phy on phy.tnh_num = pat.phy_id and phy.deleted = 0 " +
                           " where pat.account = '{0}'", strAcc),
                            connection);
                daAcc.SelectCommand = cmdSelectPat;
                int nRows = daAcc.Fill(m_dsAccount.Tables["PAT"]);
                dgvPat.DataSource = m_dsAccount.Tables["PAT"];

                //if (m_alDiagnosis.Count == 0)
                //{
                //    LoadDiagnosisArray(sbRetVal);
                //}


                m_dsAccount.Tables["INS"].Rows.Clear();
                SqlCommand cmdSelectIns = new SqlCommand(
                    string.Format("SELECT distinct ins.account, ins_a_b_c, ins.fin_code, holder_nme, holder_dob, plan_nme, policy_num, cert_ssn, grp_nme, " +
                       " grp_num, holder_sex, ins_code, ins.relation" +
                       " , isnull(guar_addr,pat_addr1) as [Addr], isnull(g_city_st,city_st_zip) as [City_st_zip] " +
                       " , insc.provider_no as [provider_no], insc.bill_form, acc.trans_date " +
                       " FROM  ins " +
                       " inner join acc on acc.account = ins.account and post_date is null and (acc.fin_code < 'w') and status not in ('closed','paid_out') " +
                       " INNER JOIN PAT on pat.account = ins.account" +
                       " LEFT OUTER JOIN INSC on insc.name = ins.plan_nme and insc.deleted = 0 " +
                       " where  ins.deleted = 0 and ins.account = '{0}'  --and provider_no is not null ", strAcc), // provider_no is not null and
                       connection);
                daAcc.SelectCommand = cmdSelectIns;
                nRows = daAcc.Fill(m_dsAccount.Tables["INS"]);
                dgvInsurance.DataSource = m_dsAccount.Tables["INS"];
                try
                {
                    m_drCurIns =
                        ((DataRow)(m_dsAccount.Tables["INS"].Rows[0]));
                }
                catch (IndexOutOfRangeException)
                {
                }

                m_dsAccount.Tables["BILLING"].Rows.Clear();

                SqlCommand cmdSelectBilling = new SqlCommand(
                    string.Format("exec usp_ViewerAcc_Billing_Table @acc = '{0}'", strAcc)
                     , connection);

                daAcc.SelectCommand = cmdSelectBilling;
                nRows = daAcc.Fill(m_dsAccount.Tables["BILLING"]);
                dgvBilling.DataSource = m_dsAccount.Tables["BILLING"];

                // load the array list with the tests for CCI edits don't use for anything else as the list gets items removed as the validation is performed.
                try
                {
                    LoadTestArray();
                }
                catch (IndexOutOfRangeException)
                {
                    // continue on may be no billing --  other code will catch the lack of billing
                }

                m_dsAccount.Tables["CHRG_ERR"].Rows.Clear();
                SqlCommand cmdSelectChrgErr =
                  new SqlCommand(
                      string.Format("select  account, pat_name, cl_mnem, fin_code, cdm, cpt4, amount, " +
                       " trans_date, service_date, qty, type, error, uri, deleted, mt_reqno, location " +
                      " from chrg_err " +
                      " where account = '{0}'", strAcc) +
                      " order by account"
                      , connection);
                daAcc.SelectCommand = cmdSelectChrgErr;
                nRows = daAcc.Fill(m_dsAccount.Tables["CHRG_ERR"]);
                dgvChrgErr.DataSource = m_dsAccount.Tables["CHRG_ERR"];


                ValidateData(drAccount["fin_code"].ToString(), ref sbRetVal);
                m_alTests.Clear();
                m_alTestsModi.Clear();
            }

            ////////////
            return sbRetVal.ToString();
        }
        //  DataRow m_drValidate = null;
        /// <summary>
        /// Hopefully a threadsafe way to validate the grid
        /// </summary>
        //private string ValidateData(DataGridViewRow drAccount)
        //{
        //    StringBuilder sbRetVal = new StringBuilder();
        //    m_alTests = new ArrayList();
        //    m_alTestsModi = new ArrayList();

        //    string strAcc = drAccount.Cells["account"].FormattedValue.ToString();
        //    if (strAcc == "C3467330")
        //    {
        //        //int x;
        //        //x= 9;
        //    }

        //    using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
        //    {
        //        SqlDataAdapter daAcc = new SqlDataAdapter();
        //        m_dsAccount.Tables["PAT"].Rows.Clear();
        //        SqlCommand cmdSelectPat = new SqlCommand(
        //            string.Format("SELECT top(1) pat.account, acc.pat_name,  pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation " +
        //                   ", guarantor, guar_addr, g_city_st, pat_marital " +
        //                   ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
        //                   ", guar_phone, acc.trans_date, " +
        //                   "phy_id, phy.last_name as [phy_last_name], phy.first_name as [phy_first_name], phy.mid_init as [phy_mid_init]" +
        //                   " FROM     pat " +
        //                   " inner join acc on acc.account = pat.account and post_date is null and (fin_code < 'w') and status not in ('closed','paid_out') " +
        //                   " left outer join phy on phy.tnh_num = pat.phy_id and phy.deleted = 0 " +
        //                   " where pat.account = '{0}'", strAcc),
        //                    connection);
        //        daAcc.SelectCommand = cmdSelectPat;
        //        int nRows = daAcc.Fill(m_dsAccount.Tables["PAT"]);
        //        dgvPat.DataSource = m_dsAccount.Tables["PAT"];

        //        try
        //        {
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_1"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_2"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_3"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_4"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
        //            m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
        //        }
        //        catch (IndexOutOfRangeException)
        //        {
        //            // keep on trucking.
        //        }
        //        if (m_alDiagnosis.Count == 0)
        //        {
        //            sbRetVal.Append("No diagnosis for this patient.\r\n");
        //        }
        //        else
        //        {
        //            for (int i = 8; i >= 0; i--)
        //            {
        //                if (string.IsNullOrEmpty(m_alDiagnosis[i].ToString()))
        //                {
        //                    m_alDiagnosis.RemoveAt(i);
        //                }
        //            }
        //        }


        //        m_dsAccount.Tables["INS"].Rows.Clear();
        //        SqlCommand cmdSelectIns = new SqlCommand(
        //            string.Format("SELECT distinct ins.account, ins_a_b_c, ins.fin_code, holder_nme, holder_dob, plan_nme, policy_num, cert_ssn, grp_nme, " +
        //               " grp_num, holder_sex, ins_code, ins.relation" +
        //               " , isnull(guar_addr,pat_addr1) as [Addr], isnull(g_city_st,city_st_zip) as [City_st_zip] " +
        //               " , insc.provider_no as [provider_no], insc.bill_form, acc.trans_date " +
        //               " FROM  ins " +
        //               " inner join acc on acc.account = ins.account and post_date is null and (acc.fin_code < 'w') and status not in ('closed','paid_out') " +
        //               " INNER JOIN PAT on pat.account = ins.account" +
        //               " LEFT OUTER JOIN INSC on insc.name = ins.plan_nme and insc.deleted = 0 " +
        //               " where  ins.deleted = 0 and ins.account = '{0}'  --and provider_no is not null ", strAcc), // provider_no is not null and
        //               connection);
        //        daAcc.SelectCommand = cmdSelectIns;
        //        nRows = daAcc.Fill(m_dsAccount.Tables["INS"]);
        //        dgvInsurance.DataSource = m_dsAccount.Tables["INS"];


        //        m_dsAccount.Tables["BILLING"].Rows.Clear();
        //        SqlCommand cmdSelectBilling = new SqlCommand(
        //            string.Format("select  chrg.account, sum(qty) as [qty], "+//chrg.cdm, 
        //            "cpt4, type, sum(amount)  as [amount]  "+
        //            ", case when modi <> '' "+
        //            " then modi "+
        //            " else "+
        //            " case when lmrp is null "+
        //            " then null else "+
        //            " case when lmrp = 0 "+
        //            " then 'GA' else 'GZ' end "+
        //            " end end as modi "+
        //            ", revcode, modi2, diagnosis_code_ptr "+
        //            " from chrg  "+
        //            " inner join amt on amt.chrg_num = chrg.chrg_num  "+
        //            " left outer join abn on abn.account = chrg.account and chrg.cdm = abn.cdm "+
        //            " where chrg.account = '{0}' "+
        //            " and credited = 0 and chrg.cdm <> 'CBILL'  "+
        //            " group by chrg.account, qty, "+//chrg.cdm	, 
        //            " cpt4, type, modi, revcode, modi2, diagnosis_code_ptr, lmrp " + // wdk 20120810 removed amount, 
        //            " order by chrg.account, cpt4, modi, modi2", strAcc)
        //                , connection);


        //        daAcc.SelectCommand = cmdSelectBilling;
        //        nRows = daAcc.Fill(m_dsAccount.Tables["BILLING"]);
        //        dgvBilling.DataSource = m_dsAccount.Tables["BILLING"];

        //        // load the array list with the tests for CCI edits don't use for anything else as the list gets items removed as the validation is performed.
        //        try
        //        {
        //            LoadTestArray();
        //        }
        //        catch (IndexOutOfRangeException)
        //        {
        //         // continue on may be no billing --  other code will catch the lack of billing
        //        }

        //        m_dsAccount.Tables["CHRG_ERR"].Rows.Clear();
        //        SqlCommand cmdSelectChrgErr =
        //          new SqlCommand(
        //              string.Format("select  account, pat_name, cl_mnem, fin_code, cdm, cpt4, amount, " +
        //               " trans_date, service_date, qty, type, error, uri, deleted, mt_reqno, location " +
        //              " from chrg_err " +
        //              " where account = '{0}'", strAcc) +
        //              " order by account"
        //              , connection);
        //        daAcc.SelectCommand = cmdSelectChrgErr;
        //        nRows = daAcc.Fill(m_dsAccount.Tables["CHRG_ERR"]);
        //        dgvChrgErr.DataSource = m_dsAccount.Tables["CHRG_ERR"];


        //        ValidateData(drAccount.Cells["fin_code"].FormattedValue.ToString(), ref sbRetVal);
        //        m_alTests.Clear();
        //        m_alTestsModi.Clear();

        //    }

        //    ////////////
        //    return sbRetVal.ToString();
        //}

        private void LoadTestArray()
        {
            Log.Instance.Debug($"Entering");
            m_alTests.Clear();
            m_alTestsModi.Clear();
            foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
            {
                if (!m_alTestsModi.Contains(new object[] { dr["cpt4"], dr["modi"], dr["modi2"] }))
                {
                    if (!m_alTests.Contains(dr["cpt4"]))
                    {
                        m_alTests.Add(dr["cpt4"]);
                        //m_alTestsModi.Add(new object[] { dr["cpt4"], dr["modi"], dr["modi2"] });
                    }
                    m_alTestsModi.Add(new object[] { dr["cpt4"], dr["modi"], dr["modi2"] });
                }
            }
        }

        private void ValidateFinCode(string strFinCode, ref StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            strFinCode = strFinCode.ToUpper().Trim();
            if (strFinCode == "Y")
            {
                CheckForVeniPunctureOnly(sbRetVal);
                return;
            }

            if (m_dsAccount.Tables["INS"].Rows.Count > 0)
            {
                string strInsFinCode = m_dsAccount.Tables["INS"].Rows[0]["FIN_CODE"].ToString().ToUpper().Trim();
                if (strFinCode != strInsFinCode)
                {
                    sbRetVal.Append("DO NOT BILL -- Insurance FinCode and Account FinCode do not match.\r\n");
                }
            }
            else
            {
                sbRetVal.Append("DO NOT BILL -- NO insurance record.\r\n");
                return; // Can't get the account from the non existing record.
            }
            string strAccount = m_dsAccount.Tables["INS"].Rows[0]["ACCOUNT"].ToString().ToUpper().Trim();
            DataRow[] drAccount = m_dsAccount.Tables["ACC"].Select(string.Format("account = '{0}'", strAccount));
            string strBillFrom = null;
            if (drAccount.GetUpperBound(0) > -1)
            {
                strBillFrom = drAccount[0]["BILL_FORM"].ToString().ToUpper().Trim();
            }

            string strInsCode = m_dsAccount.Tables["INS"].Rows[0]["ins_code"].ToString().ToUpper().Trim();
            foreach (DataRow drCheck in m_dtViewerEdits.Rows)
            {
                Application.DoEvents();
                string strEditFinCode = drCheck["fin_code"].ToString();
                string strEditInsCode = drCheck["ins_code"].ToString().ToUpper().Trim();
                string strEditBillForm = drCheck["bill_form"].ToString().ToUpper().Trim();
                string strSql = string.Format(drCheck["strSql"].ToString(),
                    strAccount
                    , m_dtFrom.ToShortDateString()
                    , m_dtThru.ToShortDateString());

                using (SqlConnection conn = new SqlConnection(m_sqlConn.ConnectionString))
                {
                    //todo 
                    if (string.IsNullOrEmpty(strEditFinCode) ||
                        (strFinCode == strEditFinCode &&
                         (strEditInsCode == strInsCode || string.IsNullOrEmpty(strEditInsCode))))
                    {
                        if (!string.IsNullOrEmpty(strEditBillForm)
                                && (strEditBillForm != strBillFrom)
                           )
                        {
                            continue;
                        }
                        SqlCommand cmdSel = new SqlCommand(strSql, conn);
                        SqlDataAdapter sda = new SqlDataAdapter(cmdSel);
                        DataTable dtSel = new DataTable();
                        int nSel = 0;
                        try
                        {
                            nSel = sda.Fill(dtSel);
                        }
                        catch (SqlException sqe)
                        {
                            sbRetVal.AppendLine(sqe.Errors.ToString());
                            continue;
                        }
                        ArrayList al = new ArrayList(sbRetVal.ToString().Split(new string[] { "/r/n" }, StringSplitOptions.RemoveEmptyEntries));
                        switch (drCheck["valid"].ToString().ToUpper())
                        {
                            case "FALSE": // false if the data table has a record.
                                {
                                    if (nSel == 1)
                                    {
                                        string strText = string.Format("{0}", drCheck["error"].ToString());
                                        if (!al.Contains(strText))
                                        {
                                            string strErr = string.Format("{0}", drCheck["error"].ToString());
                                            if (!sbRetVal.ToString().Contains(strErr))
                                            {
                                                sbRetVal.AppendLine(strErr);
                                            }
                                        }
                                    }
                                    break;
                                }
                            case "TRUE": //true if the data table should have a record
                                {
                                    if (nSel == 0)
                                    {
                                        //sbRetVal.AppendFormat("{0}\r\n", drCheck["error"].ToString());
                                        string strText = string.Format("{0}\r\n", drCheck["error"].ToString());
                                        if (!al.Contains(strText))
                                        {
                                            sbRetVal.AppendFormat("{0}\r\n", drCheck["error"].ToString());
                                        }
                                    }
                                    break;
                                }
                        }

                    }
                }

            }

            ///////////////////////////

            ///////////////////////////
            CheckForGender(sbRetVal);
            CheckInsNamesForNonAlphaChars(sbRetVal);
            CheckNamesForNonAlphaChars(sbRetVal);
            CheckPhysicianForNonAlphaChars(sbRetVal);
            CheckForCCIEdits(sbRetVal);
            CheckForDiagCodesV22_THRU_V23_89_OR_614_THRU_677(sbRetVal);
            if (strFinCode.ToUpper() != "D")
            {
                CheckForVeniPunctureOnly(sbRetVal);
            }
            CheckForMultipleVeniPunctures(sbRetVal);
            CheckForTransDateServiceDateMismatches(sbRetVal);
            CheckForDiagCodeV72_42(sbRetVal, strFinCode); // wdk 20111006 added wdk 20120521 modified to pass fin code also
            CheckForDiagCodeV70_3(sbRetVal); // wdk 20111006 added
            CheckForMutex(sbRetVal); // wdk 20111226 added
            ValidateZipCode(sbRetVal); // rgc/wdk 20120207 added
            CheckForAccidentDiagnosisCodes(sbRetVal); // rgc/wdk 20120221 added
            CheckForSSIErrorTests(sbRetVal);
            CheckForInvalidDiagCodes(sbRetVal); // wdk 20140107 removed rgc/wdk 20120222 added 
            CheckForPatInsRelationCompatability(sbRetVal); // rgc/wdk 20120222
            CheckAddressForInvalidChar(sbRetVal); // rgc/wdk 20120313 moved from medicare fincode as 5010 does not allow.
            CheckForGlobalBillingCDM(sbRetVal);
            CheckAgeForV20_2Diag(sbRetVal); // wdk 20130320 added
            Checkforv76_12Diag(sbRetVal); //wdk 20130320 added

            CheckForPSAScreeningCode(sbRetVal); // wdk 20120807
            CheckForMultipleCPT4(sbRetVal); // wdk 20120807

            if (strFinCode == "A")
            {
                CheckForInfertilityOrDentalCodes(sbRetVal);
                CheckForPatientRelationAsSelf(sbRetVal);
                CheckForGroupNumberMustBeBlank(sbRetVal);
                CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                CheckForCpt80101(sbRetVal);
                is_cpt4_icd9_ok(sbRetVal, strFinCode);
                CheckForV70Diag(sbRetVal);
                //CheckForV70_3Diag(sbRetVal);
                CheckForV70_9Diag(sbRetVal);
                // CheckMedicarePolicyNumber(sbRetVal); ; wdk 20180413 removed per Carol over new constraints for MC policy numbers.
                CheckForAccidentRelatedDiagnosis(sbRetVal);
                CheckForInvalidPSATesting(sbRetVal);
                CheckForBundledGHP(sbRetVal);

                CheckForBundledOBP(sbRetVal); // wdk 20120807 added

            }
            if (strFinCode == "B")
            {
                CheckForInfertilityOrDentalCodes(sbRetVal);
                CheckForDiagCodeV72_31(sbRetVal);
                CheckForObesityCodes(sbRetVal);
                CheckForV70Diag(sbRetVal);
                //CheckForV70_3Diag(sbRetVal);
                CheckForV70_9Diag(sbRetVal);
                CheckForBodyMassIndex(sbRetVal);
                CheckBlueCrossPolicyNumber(sbRetVal);
                CheckBlueCrossGroupNumber(sbRetVal);
                CheckFor80299(sbRetVal);
                // wdk 20120828 added check 
                string strPolicyNum = null;
                try
                {
                    strPolicyNum =
                        m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
                }
                catch (IndexOutOfRangeException)
                {
                    return;
                }
                if (strPolicyNum.StartsWith("ZXD"))
                {
                    is_cpt4_icd9_ok(sbRetVal, strFinCode);
                }

            }
            if (strFinCode == "C")
            {
                CheckForInfertilityOrDentalCodes(sbRetVal);
                CheckForGroupNumberMustBeBlank(sbRetVal);
                CheckForDiagCodeStartsWithV(sbRetVal);
                // wdk 20120521 if Outpatient don't check this as the form will be ub not 1500
                //if (!((CheckBox)m_cboxInclude.Control).Checked) 
                //{
                //    CheckForDiagCodePointers(sbRetVal);
                //}
                CheckForPolicyNumHasNoPrefix(sbRetVal);
                CheckForObesityCodes(sbRetVal);
                CheckForV70Diag(sbRetVal);
                CheckForV70_9Diag(sbRetVal);
                CheckForBodyMassIndex(sbRetVal);
                CheckFor80299(sbRetVal);
            }
            if (strFinCode == "D")
            {
                CheckForInfertilityOrDentalCodes(sbRetVal);
                CheckForPatientRelationAsSelf(sbRetVal);
                CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                CheckForObesityCodes(sbRetVal);
                CheckForV70_9Diag(sbRetVal);
                CheckFor80299(sbRetVal);
                CheckForPSRAS(sbRetVal);



            }
            if (strFinCode == "H")
            {
                strInsCode = null;
                try
                {
                    strInsCode = m_dsAccount.Tables["INS"].Rows[0]["INS_CODE"].ToString().ToUpper().Trim();
                }
                catch (IndexOutOfRangeException)
                {
                    return;
                }
                if (strInsCode == "CIGNA")
                {
                    CheckForObesityCodes(sbRetVal);
                    //  CheckForAbnormalWeigthGain(sbRetVal); // csp/wdk 20120816 removed
                    CheckForBehavioralCodes(sbRetVal);
                    // CheckForV70Diag(sbRetVal); // csp/wdk 20120816 removed
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    CheckForValidPolicyNumber(sbRetVal);
                    CheckFor80299(sbRetVal);
                    CheckForContraceptiveCode(sbRetVal); // csp/wdk 20120816 added 
                }
                if (strInsCode == "HESP")
                {
                    is_cpt4_icd9_ok(sbRetVal, strFinCode); // rgc/wdk 20120412 added per carols request
                    CheckFor80299(sbRetVal);
                    // csp/wdk 20120816 added 
                    CheckForInfertilityOrDentalCodes(sbRetVal);
                    CheckForPatientRelationAsSelf(sbRetVal);
                    //CheckForGroupNumberMustBeBlank(sbRetVal); // wdk 20131017 removed
                    CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                    CheckForCpt80101(sbRetVal);
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForAccidentRelatedDiagnosis(sbRetVal);
                    CheckForInvalidPSATesting(sbRetVal);
                    CheckForBundledGHP(sbRetVal);

                    CheckForBundledOBP(sbRetVal); // wdk 20120807 added
                    CheckForPolicyNumLength(sbRetVal, new int[] { 9, 11 }); // csp/wdk 20120816 added 
                }
            }
            if (strFinCode == "M")
            {
                CheckForInfertilityOrDentalCodes(sbRetVal);
                CheckForOBPanel(sbRetVal);
                CheckForGHPPanel(sbRetVal);
                CheckForPatientRelationAsSelf(sbRetVal);
                CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                CheckForObesityCodes(sbRetVal);
                CheckForV70_9Diag(sbRetVal);
                is_cpt4_icd9_ok(sbRetVal, strFinCode);
                CheckFor80299(sbRetVal);
                CheckForBodyMassIndex(sbRetVal); // csp/wdk 20120816 added 
            }

            if (strFinCode == "L")
            {

                strInsCode = null;
                try
                {
                    strInsCode = m_dsAccount.Tables["INS"].Rows[0]["INS_CODE"].ToString().ToUpper();
                }
                catch (IndexOutOfRangeException)
                {
                    return;
                }
                if (strInsCode == "HP")
                {
                    CheckForInfertilityOrDentalCodes(sbRetVal);
                    CheckForDiagCodeV72_31(sbRetVal);
                    CheckForInsGroupNumber(sbRetVal);
                    // wdk 20120521 if Outpatient don't check this as the form will be ub not 1500
                    //if (!((CheckBox)m_cboxInclude.Control).Checked)
                    //{
                    //    CheckForDiagCodePointers(sbRetVal);
                    //}
                    CheckForObesityCodes(sbRetVal);
                    CheckForAbnormalWeightGain(sbRetVal);
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    CheckFor80299(sbRetVal);

                }
                if (strInsCode == "AETNA")
                {
                    CheckForInfertilityOrDentalCodes(sbRetVal);
                    CheckForOBPanel(sbRetVal);
                    CheckForGHPPanel(sbRetVal);
                    CheckForDiagCodeV72_31(sbRetVal);
                    // wdk 20140130 restored as Aetna replaces HP for West Tn Health Care
                    // rgc/wdk 20120412 Carol asked to have it removed
                    // wdk 20120521 if Outpatient don't check this as the form will be ub not 1500
                    //if (!((CheckBox)m_cboxInclude.Control).Checked)
                    //{
                    //    CheckForDiagCodePointers(sbRetVal);
                    //}
                    CheckForObesityCodes(sbRetVal);
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    CheckFor80299(sbRetVal);
                }
                if (strInsCode == "SEHZ")
                {
                    CheckForPatientRelationAsSelf(sbRetVal);
                    //CheckForGroupNumberMustBeBlank(sbRetVal); // wdk 20131017 removed
                    CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                    // wdk 20120521 if Outpatient don't check this as the form will be ub not 1500
                    //if (!((CheckBox)m_cboxInclude.Control).Checked)
                    //{
                    //    CheckForDiagCodePointers(sbRetVal);
                    //}
                    CheckForPolicyNumHasNoPrefix(sbRetVal);
                    is_cpt4_icd9_ok(sbRetVal, strFinCode);
                    CheckForCpt80101(sbRetVal); // wdk 20110725 added by request 
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    CheckPolicyNumberValidity(sbRetVal);

                }
                if (strInsCode == "WIN")
                {
                    CheckForPatientRelationAsSelf(sbRetVal);
                    //CheckForGroupNumberMustBeBlank(sbRetVal); // wdk 20131017 removed
                    CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                    CheckForPolicyStartsWithWX(sbRetVal);
                    is_cpt4_icd9_ok(sbRetVal, strFinCode);
                    CheckForCpt80101(sbRetVal); // wdk 20110725 added by request 
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    // rgc/wdk 20120313 SSI for Windsor is looking for this.
                    // wdk 20120521 if Outpatient don't check this as the form will be ub not 1500
                    //if (!((CheckBox)m_cboxInclude.Control).Checked)
                    //{
                    //    CheckForDiagCodePointers(sbRetVal);
                    //} 

                }
                if (strInsCode == "SECP")
                {
                    CheckForPatientRelationAsSelf(sbRetVal);
                    CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                    is_cpt4_icd9_ok(sbRetVal, strFinCode);
                    CheckForCpt80101(sbRetVal); // wdk 20110725 added by request 
                    CheckForV70Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    CheckForDiagCodePointers(sbRetVal);


                }
                if (strInsCode == "UHC")
                {
                    CheckForPolicyNumHasNoPrefix(sbRetVal);
                    CheckForObesityCodes(sbRetVal);
                    CheckForBehavioralCodes(sbRetVal);
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForBodyMassIndex(sbRetVal);
                    CheckPolicyNumberValidity(sbRetVal);
                    CheckFor80299(sbRetVal);
                }
                if (strInsCode == "HUM")
                {
                    // wdk 20120521 if Outpatient don't check this as the form will be ub not 1500
                    //if (!((CheckBox)m_cboxInclude.Control).Checked) 
                    //{
                    //    CheckForDiagCodePointers(sbRetVal);
                    //}
                    // csp/wdk 20120816 added 
                    CheckForInfertilityOrDentalCodes(sbRetVal);
                    CheckForPatientRelationAsSelf(sbRetVal);
                    //CheckForGroupNumberMustBeBlank(sbRetVal); // wdk 20131017 removed
                    CheckForPatientNameSameAsPolicyHolderName(strAccount, sbRetVal);
                    CheckForCpt80101(sbRetVal);
                    is_cpt4_icd9_ok(sbRetVal, strFinCode);
                    CheckForV70Diag(sbRetVal);
                    //CheckForV70_3Diag(sbRetVal);
                    CheckForV70_9Diag(sbRetVal);
                    CheckForAccidentRelatedDiagnosis(sbRetVal);
                    CheckForInvalidPSATesting(sbRetVal);
                    CheckForBundledGHP(sbRetVal);

                    CheckForBundledOBP(sbRetVal); // wdk 20120807 added
                    CheckForPolicyNumLength(sbRetVal, new int[] { 7, 9 }); // csp/wdk 20120816 added 
                }

            }
            if (strFinCode == "E")
            {

            }
            if (strFinCode == "CLIENT")
            {
                //    sbRetVal.Append("CLIENT accounts won't load????");
            }
            if (strFinCode == "W")
            {
            }
            if (strFinCode == "X")
            {
            }
            if (strFinCode == "Y")
            {
            }
            if (strFinCode == "Z")
            {
            }

        }

        /// <summary>
        /// 20130320 added per Carol
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void Checkforv76_12Diag(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            if (m_alDiagnosis.Contains("V76.12"))
            {
                sbRetVal.AppendFormat("V76.12 Diagnosis error -- mammogram code.");
            }
        }

        /// <summary>
        /// 20130320 added per Carol
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckAgeForV20_2Diag(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }

            DateTime dtPatDob = DateTime.MaxValue;
            DateTime dtTransDate = DateTime.MaxValue;
            if (!DateTime.TryParse(
                m_dsAccount.Tables["PAT"].Rows[0]["dob_yyyy"].ToString(), out dtPatDob))
            {
                sbRetVal.AppendFormat("Patient DOB [{0}] is not a valid date.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["dob_yyyy"].ToString());
            }
            try
            {
                if (!DateTime.TryParse(
                        m_dsAccount.Tables["ACC"].Rows[0]["trans_date"].ToString(), out dtTransDate))
                {
                    sbRetVal.AppendFormat("Account trans_date [{0}] is not a valid date.\r\n",
                        m_dsAccount.Tables["ACC"].Rows[0]["trans_date"].ToString());
                }
            }
            catch (DeletedRowInaccessibleException)
            {
                return;
            }

            if (m_alDiagnosis.Contains("V20.2"))
            {
                string[] strAge = Time.GetAge(dtPatDob, dtTransDate).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int Years = int.Parse(strAge[0].Split(new char[] { ' ' })[0].ToString());

                if (Years >= 18)
                {
                    sbRetVal.AppendFormat("V20.2 Diagnosis error. Individual's age on Date of service was [{0}]",
                        Years);
                }

            }
        }

        private void CheckForGlobalBillingCDM(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            if (m_drCur["fin_code"].ToString().ToUpper() == "D")
            {
                return;
            }

            m_dtGlobalBilling.PrimaryKey = new DataColumn[] {
                m_dtGlobalBilling.Columns["cpt4"] };

            foreach (DataRow drCpt4 in m_dsAccount.Tables["BILLING"].Rows)
            {
                DataRow drFound = m_dtGlobalBilling.Rows.Find(drCpt4["CPT4"]);
                if (drFound != null)
                {
                    sbRetVal.AppendFormat("Charges contain Global Billing Cpt4 {0}", drCpt4["cpt4"]);
                }
            }

        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForPSRAS(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (dr["cpt4"].ToString().Contains("G0103"))
                    {
                        sbRetVal.Append("Charges contain cpt4 G0103.\r\n");
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        private void CheckBlueCrossGroupNumber(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            string strGrpNum = "";
            try
            {
                strGrpNum = m_dsAccount.Tables["INS"].Rows[0]["grp_num"].ToString();

            }
            catch (IndexOutOfRangeException)
            {
                return;
            }


            for (int i = 0; i < strGrpNum.Length; i++)
            {
                if (!char.IsLetterOrDigit(strGrpNum[i]))
                {
                    if (!char.IsWhiteSpace(strGrpNum[i]))
                    {
                        sbRetVal.AppendFormat("{0}[{1}]{2} is not a letter, digit or space in policy number.\r\n",
                            strGrpNum.Substring(0, i), strGrpNum[i], strGrpNum.Substring(i + 1, strGrpNum.Length - i - 1));
                    }
                }
            }
        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForContraceptiveCode(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            try
            {
                if (m_alDiagnosis.Count == 0)
                {
                    if (LoadDiagnosisArray(sbRetVal) == 0)
                    {
                        return;
                    }
                }
                if (m_alDiagnosis.Contains("V25.09"))
                {
                    sbRetVal.Append("Account contains Diagnosis code V25.09.\r\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbRetVal"></param>
        /// <param name="nLen"></param>
        private void CheckForPolicyNumLength(StringBuilder sbRetVal, int[] nLen)
        {
            Log.Instance.Debug($"Entering");
            try
            {
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString()))
                {
                    sbRetVal.Append("Primary Insurance has no policy number.\r\n");
                }
                string strPolicy = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
                int nMin = nLen[0];
                int nMax = nLen[1];
                if (strPolicy.Length < nMin || strPolicy.Length > nMax)
                {
                    sbRetVal.Append(
                        string.Format("Primary Insurance policy number is not within the range {0} - {1}.\r\n",
                        nMin, nMax));
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        /// <summary>
        /// wdk 20130130 Converted to use dictionary table from sql so additions can be made
        /// without having to rebuild the software.
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForMultipleCPT4(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            //ArrayList alUnits = new ArrayList();
            //alUnits.AddRange(new string[] { 
            //  "80101","83883","83890","83891","83892","83894","83896","83898","83897", 
            //  "83901","83903","83908","83909","83914","86003","86146","86160","86235","86255",
            //  "86256","86905","87046","87077","87184","87186","87188","87254","87449","87798","88271","88347"});
            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (int.Parse(dr["qty"].ToString()) == 1)
                    {
                        continue;
                    }
                    if (!string.IsNullOrEmpty(dr["modi"].ToString()) ||
                        !string.IsNullOrEmpty(dr["modi2"].ToString()))
                    {
                        continue;
                    }

                    if (m_dtMultiUnitCpt4.Rows.Find(dr["cpt4"].ToString()) == null)
                    //  if (!alUnits.Contains(dr["cpt4"].ToString()))
                    {
                        string strErr = string.Format("Charges contain CPT {0} with a qty of [{1}].",
                            dr["cpt4"].ToString(), dr["qty"].ToString());
                        if (!sbRetVal.ToString().Contains(strErr))
                        {
                            sbRetVal.AppendLine(strErr);
                            //.AppendFormat(strErr);
                        }
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        private void CheckForPSAScreeningCode(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            try
            {
                if (m_alDiagnosis.Count == 0)
                {
                    if (LoadDiagnosisArray(sbRetVal) == 0)
                    {
                        return;
                    }
                }

                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (dr["cpt4"].ToString().Contains("G0101") ||
                        dr["cpt4"].ToString().Contains("G0102") ||
                        dr["cpt4"].ToString().Contains("G0103"))
                    {
                        if (!m_alDiagnosis.Contains("V76.44"))
                        {
                            sbRetVal.AppendFormat("Charges contain a PSA screening. For CPT {0} without Diagnosis V76.44\r\n", dr["cpt4"].ToString());
                        }
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        private void CheckBlueCrossPolicyNumber(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // format upto 17 Alphanumerics in length
            string strPolicyNumber = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
            if (string.IsNullOrEmpty(strPolicyNumber))
            {
                sbRetVal.Append("BlueCross Policy number is null or blank.\r\n");
                return;
            }
            if (strPolicyNumber.Length > 17)
            {
                sbRetVal.Append("BlueCross Policy number must be no more than 17 characters long.");
            }
            // csp/wdk 20120816 added 
            for (int i = 0; i < 1; i++)
            {
                if (!char.IsLetter(strPolicyNumber[i]))
                {
                    sbRetVal.AppendFormat("{0}[{1}]{2} is not a letter in policy number.\r\n",
                        strPolicyNumber.Substring(0, i), strPolicyNumber[i], strPolicyNumber.Substring(i + 1, strPolicyNumber.Length - i - 1));
                }
            }
            for (int i = 0; i < strPolicyNumber.Length; i++)
            {
                if (!char.IsLetterOrDigit(strPolicyNumber[i]))
                {
                    sbRetVal.AppendFormat("{0}[{1}]{2} is not a letter or digit in policy number.\r\n",
                        strPolicyNumber.Substring(0, i), strPolicyNumber[i], strPolicyNumber.Substring(i + 1, strPolicyNumber.Length - i - 1));
                }
            }


        }

        /// <summary>
        /// this is for SSI payor ID 95378-NOCD which encompasses Secure Plus Complete and UHC Community Plan
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckPolicyNumberValidity(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            string strPolicyNumber = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
            switch (strPolicyNumber.Length)
            {
                case 8:
                    {
                        break;
                    }
                case 9:
                    {
                        // 2 alphas 7 numerics
                        // 1 alphas 8 numerics
                        // 9 numerics
                        int nAlpha = 0;
                        foreach (char c in strPolicyNumber)
                        {
                            if (char.IsLetter(c))
                            {
                                nAlpha++;
                            }
                        }
                        if (nAlpha > 2)
                        {
                            sbRetVal.AppendFormat("Policy number [{0}] is invalid. It contains [{1}] alpha characters.", strPolicyNumber, nAlpha);
                            return;
                        }
                        break;
                    }
                case 11:
                    {
                        break;
                    }
                default:
                    {
                        sbRetVal.AppendFormat("Policy number [{0}] is invalid with a length of [{1}].\r\n",
                            strPolicyNumber, strPolicyNumber.Length);
                        break;
                    }
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForBundledOBP(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");

            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (dr["cpt4"].ToString().Contains("80055"))
                    {
                        sbRetVal.Append("Charges contain a Bundled OB panel cpt4 80055.\r\n");
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

        }

        private void CheckForBundledGHP(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (dr["cpt4"].ToString().Contains("80050"))
                    {
                        sbRetVal.Append("Charges contain a Bundled GHP panel cpt4 80050.\r\n");
                    }

                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

        }

        private void CheckForPatInsRelationCompatability(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            string strPatRel = m_dsAccount.Tables["PAT"].Rows[0]["relation"].ToString().Trim();
            string strInsRel = m_dsAccount.Tables["INS"].Rows[0]["relation"].ToString().Trim();
            if (strPatRel != strInsRel)
            {
                sbRetVal.AppendFormat("Pat relations listed as [{0}] not compatable with Ins relation of [{1}].\r\n",
                    strPatRel, strInsRel);
            }
        }

        private void CheckForInvalidDiagCodes(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            foreach (string str in m_alDiagnosis)
            {
                if (!m_alDiagCodes.Contains(str))
                {
                    sbRetVal.AppendFormat("Diagnosis [{0}] does not exist in ICD table.\r\n", str);
                }
            }
        }

        private void CheckForSSIErrorTests(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            LoadTestArray();

            foreach (DataRow dr in m_dtDictCpt4Warnings.Rows)
            {
                if (m_alTests.Contains(dr["cpt4"]) && dr["deleted"].ToString() == "False")
                {
                    sbRetVal.AppendFormat("Tests include [{0}] with error [{1}].\r\n", dr["cpt4"], dr["note"]);
                }
            }

            if (m_alTests.Contains("86705") && m_alTests.Contains("87340") && m_alTests.Contains("86803"))
            {
                sbRetVal.Append("Tests include [86705][86803]and[87340] an unbundled (Acute Hepatitis Panel).\r\n");
            }

            //if (m_alTests.Contains("83519") && m_alTests.Contains("83520"))
            //{
            //    sbRetVal.Append("WARNING: CCI CONFLICT WITH HCPCS CODES 83520 AND 83519: Mutually Exclusive Edits.\r\n");
            //    /* rgc/wdk 20120223 added
            //     * WARNING: CCI CONFLICT WITH HCPCS CODES 83520 AND 83519: Mutually Exclusive Edits. 
            //     * OCE-39/OCE-40 WARNING 
            //     * CPT CODES COPYRIGHT 1999 AMERICAN MEDICAL ASSOCIATION. ALL RIGHTS RESERVED.
            //     * ***CCI WARNING EDIT*** [VERSION 02/20/2012]
            //     */
            //}


        }

        private void ValidateZipCode(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            string[] strCSZ = new string[4];
            string strAddrPat = m_dsAccount.Tables["PAT"].Rows[0]["city_st_zip"].ToString();
            strAddrPat =
                string.IsNullOrEmpty(strAddrPat) ? m_dsAccount.Tables["PAT"].Rows[0]["g_city_st"].ToString() : strAddrPat;
            try
            {

                if (strAddrPat.Contains(','))
                {
                    try
                    {
                        strCSZ[0] = strAddrPat.Substring(0, strAddrPat.IndexOf(", "));
                        strAddrPat = strAddrPat.Replace(strCSZ[0], "");
                        strCSZ[1] = strAddrPat.Split(new string[] { ",", " ", "-" }, StringSplitOptions.RemoveEmptyEntries)[0];
                        strCSZ[2] = strAddrPat.Split(new string[] { ",", " ", "-" }, StringSplitOptions.RemoveEmptyEntries)[1];
                        strCSZ[3] =
                            string.IsNullOrEmpty(strAddrPat.Split(new string[] { ",", " ", "-" }, StringSplitOptions.RemoveEmptyEntries)[2]) == true
                            ? ""
                            : strAddrPat.Split(new string[] { ",", " ", "-" }, StringSplitOptions.RemoveEmptyEntries)[2];
                    }
                    catch (IndexOutOfRangeException)
                    {
                        //strCSZ[3] is null
                    }

                }
                else
                {
                    sbRetVal.AppendFormat("Patients Address [{0}] does not contain a comma. Can't parse.\r\n", strAddrPat);

                }

                ListObject lo = null;

                if (!m_dicZip.TryGetValue(strCSZ[2], out lo))
                {
                    //if (!m_alZip.Contains(strCSZ[2]))
                    {
                        sbRetVal.AppendFormat("Zip code [{0}] not in zip directory.\r\n", strCSZ[2]);
                    }
                }
                else
                {
                    if (strCSZ[1].ToString().ToUpper() != lo.DisplayName.ToUpper())
                    {
                        sbRetVal.AppendFormat("Zip directory has state as [{0}], while address has state as [{1}]\r\n",
                            lo.DisplayName, strCSZ[1]);
                    }
                    if (strCSZ[0].ToString().ToUpper() != lo.Description.ToUpper())
                    {
                        sbRetVal.AppendFormat("Zip directory has city as [{0}], while address has city as [{1}]\r\n",
                            lo.Description, strCSZ[0]);
                    }

                }



                if (!string.IsNullOrEmpty(strCSZ[3]))
                {
                    if (strCSZ[3].Length != 4)
                    {
                        sbRetVal.AppendFormat("Zip code's + 4 [{0}] is invalid.\r\n", strCSZ[3]);
                    }
                }
            }
            catch (Exception ex)
            {
                string strType = ex.GetType().ToString();
                sbRetVal.AppendFormat("Address [{0}] not valid .\r\n", strAddrPat);
                //m_dsAccount.Tables["PAT"].Rows[0]["city_st_zip"].ToString());
            }
        }

        /// <summary>
        /// MODIFIED
        /// rgc/wdk 20120202 added check for PSA testing for under 50 year olds
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForInvalidPSATesting(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            DataRow[] drRow =
                    m_dsAccount.Tables["ACC"].Select(string.Format("account = '{0}'",
                        m_dsAccount.Tables["PAT"].Rows[0]["ACCOUNT"].ToString()));


            DateTime dtPatDob = DateTime.MaxValue;
            DateTime dtTransDate = DateTime.MaxValue;
            if (!DateTime.TryParse(
                m_dsAccount.Tables["PAT"].Rows[0]["dob_yyyy"].ToString(), out dtPatDob))
            {
                sbRetVal.AppendFormat("Patient DOB [{0}] is not a valid date.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["dob_yyyy"].ToString());
            }

            if (m_alTests.Contains("G0103"))
            {
                if (!DateTime.TryParse(
                    drRow[0]["trans_date"].ToString(), out dtTransDate))
                {
                    sbRetVal.AppendFormat("Account trans_date [{0}] is not a valid date.\r\n",
                        drRow[0]["trans_date"]);
                }

                string[] strAge = Time.GetAge(dtPatDob, dtTransDate).Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                int Years = int.Parse(strAge[0].Split(new char[] { ' ' })[0].ToString());

                if (Years < 50)
                {
                    drRow[0].SetColumnError(1, "Check for Previous PSA in last 12 months.");
                }

            }
        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForAccidentRelatedDiagnosis(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }

            ArrayList al = new ArrayList();
            double dStart = 800.00;
            while (double.Parse(dStart.ToString("F2")) <= 994.80)
            {
                al.Add(string.Format(string.Format("{0:f2}", dStart), dStart.ToString("F2")));
                dStart += .01;
            }
            dStart = 996.00;
            while (double.Parse(dStart.ToString("F2")) <= 996.03)
            {
                al.Add(string.Format(string.Format("{0:f2}", dStart), dStart.ToString("F2")));
                dStart += .01;
            }
            al.Add("996.09");

            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }

            //if (m_alDiagnosis.Count == 0)
            //{
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_1"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_2"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_3"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_4"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"]);
            //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"]);
            //}
            //for (int i = 8; i >= 0; i--)
            //{
            //    if (string.IsNullOrEmpty(m_alDiagnosis[i].ToString()))
            //    {
            //        m_alDiagnosis.RemoveAt(i);
            //    }
            //}
            //}
            string strDiag = null;
            foreach (string str in m_alDiagnosis)
            {
                strDiag = string.Format("{0}", str.PadRight(6, '0'));
                if (al.Contains(strDiag))
                {
                    sbRetVal.Append(
                        string.Format("Diagnosis Code [{0}] on Patient Record needs Accident Occurance Codes per SSI edit.\r\n",
                        str));
                }
            }
        }

        private void CheckMedicarePolicyNumber(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            string strPolicyNumber = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
            if (string.IsNullOrEmpty(strPolicyNumber))
            {
                sbRetVal.Append("Medicare Policy number is null or blank.\r\n");
                return;
            }
            return;

            /*
            // format 1 -- 9 numerics followed by 1 alpha OR 1 alpha and 1 alpha numeric
            if (char.IsNumber(strPolicyNumber[0]))
            {
                if (strPolicyNumber.Length < 10)
                {
                    sbRetVal.AppendFormat("Policy number [{0}] has an invald length of [{1}].\r\n", strPolicyNumber, strPolicyNumber.Length);
                    return;
                }
                for (int i = 1; i <= 8; i++)
                {
                    if (!char.IsNumber(strPolicyNumber[i]))
                    {
                        sbRetVal.AppendFormat("Policy number has a value at position [{0}] that is not a number.\r\n", i+1);
                    }
                }
                for (int i = 8; i < strPolicyNumber.Length; i++)
                {
                    if (i > 11)
                    {
                        sbRetVal.AppendFormat("Policy number is too long for this format.\r\n");
                        break;
                    }
                    if (i == 9)
                    {
                        if (!char.IsLetter(strPolicyNumber[i]))
                        {
                            sbRetVal.AppendFormat("Policy number value at postition [{0}] is not an letter.\r\n", i+1);
                        }
                    }
                    if (i == 10)
                    {
                        if (!char.IsLetterOrDigit(strPolicyNumber[i]))
                        {
                            sbRetVal.AppendFormat("Policy number value at position [{0}] is not an letter or numeric.\r\n",i+1);
                        }
                    }
                        
                }
            }
            // format 2 -- 1 to 3 alphas followed by 6 or 9 numerics
            if (char.IsLetter(strPolicyNumber[0]))
            {
                
                if (strPolicyNumber.Length < 3)
                {
                    sbRetVal.AppendFormat("Policy number [{0}] has an invald length of [{1}].\r\n", strPolicyNumber, strPolicyNumber.Length);
                    return;
                }
                int nLastLetter = 1; 
                if (char.IsLetter(strPolicyNumber[1]))
                {
                    nLastLetter++;
                }
                if (char.IsLetter(strPolicyNumber[2]) && nLastLetter == 2)
                {
                    nLastLetter++;
                }
                ArrayList alInvalid = new ArrayList();
                for (int i = nLastLetter; i < strPolicyNumber.Length; i++)
                {
                    if (char.IsLetter(strPolicyNumber[i]))
                    {
                        alInvalid.Add(i);
                    }
                }
                string strErr = null;
                if (alInvalid.Count > 0)
                {
                    foreach (int str in alInvalid)
                    {
                        strErr += string.Format("Policy Number at position [{0}] is not a number.\r\n", str+1);
                    }
                }
                if (!string.IsNullOrEmpty(strErr))
                {
                    sbRetVal.Append(strErr);
                }
                int nRemainingLen = strPolicyNumber.Length - nLastLetter;
                if (nRemainingLen != 6)
                {
                    if (nRemainingLen != 9)
                    {
                        sbRetVal.AppendFormat("Policy number [{0}] has an invald length of [{1}].\r\n", strPolicyNumber, strPolicyNumber.Length);
                        
                    }
                }

            }

            for (int i = 0; i < strPolicyNumber.Length; i++)
            {
                if (char.IsLetter(strPolicyNumber[i]) && i == 2)
                {
                    sbRetVal.AppendFormat("{0}[{1}]{2} is not a number in policy number.\r\n",
                        strPolicyNumber.Substring(0, i), strPolicyNumber[i], strPolicyNumber.Substring(i + 1, strPolicyNumber.Length - i - 1));
                }
                if (!char.IsLetterOrDigit(strPolicyNumber[i]))
                {
                    sbRetVal.AppendFormat("{0}[{1}]{2} is not a letter or digit in policy number.\r\n",
                        strPolicyNumber.Substring(0, i), strPolicyNumber[i], strPolicyNumber.Substring(i + 1, strPolicyNumber.Length - i - 1));
                }
            }
            */
            //  bool bContainsChar = false;

            //for (int i = 0; i < strPolicyNumber.Length; i++)
            //{
            //    if (char.IsLetter(strPolicyNumber[i]))
            //    {
            //        bContainsChar = true;
            //        break;


            //    }
            //}
            //if (!bContainsChar)
            //{
            //    sbRetVal.AppendFormat("Policy number [{0}] does not contain a character.\r\n",
            //            strPolicyNumber);
            //}
            //// rgc/wdk 20120203 added additional check for format per carrier Level Edit
            //    // policy number must be in one of the following formats:
            //    // 9 numerics followed by 1 alph or 1 alpha and 1 alpha numeric.
            //    // 1 to 3 alphas follwed by 6 or 9 numerics.
            //int nParse = 0;
            //try
            //{
            //    if (int.TryParse(strPolicyNumber.Substring(0, 8), out nParse))
            //    {
            //        int nLen = strPolicyNumber.Length;

            //        string strAlphNum = strPolicyNumber.Substring(9, nLen - 9);
            //        if (strAlphNum.Length > 2)
            //        {
            //            sbRetVal.AppendFormat("Policy number [{0}] is too long.\r\n", strPolicyNumber);
            //        }
            //    }
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    sbRetVal.AppendFormat("Policy number [{0}] is too short.\r\n", strPolicyNumber);
            //}
            //catch (ArgumentOutOfRangeException)
            //{
            //    sbRetVal.AppendFormat("Policy number [{0}] is too short.\r\n", strPolicyNumber);
            //}

            //if (char.IsLetter(strPolicyNumber[0]))
            //{
            //    int nValidLen = strPolicyNumber.Length;
            //    if (nValidLen < 7)
            //    {
            //        sbRetVal.AppendFormat("Policy number {0} is too short.\r\n", strPolicyNumber);
            //    }
            //    if (nValidLen > 12)
            //    {
            //        sbRetVal.AppendFormat("Policy number {0} is too Long.\r\n", strPolicyNumber);
            //    }

            //    nValidLen = 1;
            //    if (char.IsLetter(strPolicyNumber[1]))
            //    {
            //        nValidLen++;
            //    }
            //    if (char.IsLetter(strPolicyNumber[2]))
            //    {
            //        nValidLen++;
            //    }
            //    int nRemainLen = strPolicyNumber.Length - nValidLen;

            //    if (!int.TryParse(strPolicyNumber.Substring(nValidLen, strPolicyNumber.Length - nValidLen), out nParse))
            //    {
            //        sbRetVal.AppendFormat("Policy number [{0}] contains a letter after the 3rd character.\r\n", strPolicyNumber);
            //    }

            //}

        }

        private void CheckPhysicianForNonAlphaChars(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set m_bExcludeHardCodedCode in system table where key_name = INCLUDE
            // the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }

            string strPhyName =
                string.Format("{0}-{1}-{2}",
                    m_dsAccount.Tables["PAT"].Rows[0]["phy_last_name"].ToString(),
                        m_dsAccount.Tables["PAT"].Rows[0]["phy_first_name"].ToString(),
                            m_dsAccount.Tables["PAT"].Rows[0]["phy_mid_init"].ToString());


            for (int i = 0; i < strPhyName.Length; i++)// char c in strPhyName)
            {
                if (char.IsPunctuation(strPhyName[i]))
                {
                    if (strPhyName[i].Equals('-')) // SSI generic edit physician names can only contain Letters, hyphens, apostrophes, and spaces
                    {
                        continue;
                    }
                    if (strPhyName[i].Equals('\'')) // SSI generic edit physician names can only contain Letters, hyphens, apostrophes, and spaces
                    {
                        continue;
                    }
                    sbRetVal.AppendFormat("{0}[{1}]{2} is Punctuation in physician name.\r\n",
                        strPhyName.Substring(0, i), strPhyName[i], strPhyName.Substring(i + 1, strPhyName.Length - i - 1));


                }
                if (char.IsSymbol(strPhyName[i]))
                {
                    sbRetVal.AppendFormat("{0}[{1}]{2} is Symbol in physician name.\r\n",
                        strPhyName.Substring(0, i), strPhyName[i], strPhyName.Substring(i + 1, strPhyName.Length - i - 1));
                }
                if (char.IsSeparator(strPhyName[i]))
                {
                    if (strPhyName[i].Equals(' ')) // SSI generic edit physician names can only contain Letters, hyphens, apostrophes, and spaces
                    {
                        continue;
                    }
                    sbRetVal.AppendFormat("{0}[{1}]{2} is Separator in physician name.\r\n",
                        strPhyName.Substring(0, i), strPhyName[i], strPhyName.Substring(i + 1, strPhyName.Length - i - 1));
                }
                if (char.IsNumber(strPhyName[i]))
                {
                    sbRetVal.AppendFormat("{0}[{1}]{2} is Number in physician name.\r\n",
                        strPhyName.Substring(0, i), strPhyName[i], strPhyName.Substring(i + 1, strPhyName.Length - i - 1));
                }

            }
        }

        private void CheckAddressForInvalidChar(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            // rgc/wdk 20120329 added '`' to char[] as not being valid in an address.
            string strAddress = m_dsAccount.Tables["PAT"].Rows[0]["pat_addr1"].ToString();

            if (strAddress.IndexOfAny(new char[] { '`', '!', '@', '#', '$', '~', '%', '^', '*', '|', '\\', '/', '<', '>' }) > -1)
            {
                sbRetVal.Append("Patients address has an invalid character.\r\n");
            }
            strAddress = m_dsAccount.Tables["PAT"].Rows[0]["guar_addr"].ToString();
            if (strAddress.IndexOfAny(new char[] { '`', '!', '@', '#', '$', '~', '%', '^', '*', '|', '\\', '/', '<', '>' }) > -1)
            {
                sbRetVal.Append("Guarantor address has an invalid character.\r\n");
            }

        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForValidPolicyNumber(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            string strPolicyNumber = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
            try
            {
                if (strPolicyNumber.Contains(" "))
                {
                    sbRetVal.Append("Primary Insurance policy number contains a space.\r\n");

                }
                strPolicyNumber = strPolicyNumber.Replace(" ", "");
                if (strPolicyNumber.Length < 7 || strPolicyNumber.Length > 12)
                {
                    sbRetVal.Append(
                        string.Format("Primary Insurance policy number must be between 7 and 12 characters long. " +
                        " Policy number [{0}] is {1} character{2} long.", strPolicyNumber, strPolicyNumber.Length, strPolicyNumber.Length > 1 ? "s" : ""));
                }
            }
            catch (Exception)
            {
                sbRetVal.Append("Could not check for valid policy number.");
            }
        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForBodyMassIndex(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }


            ArrayList m_alBMI = new ArrayList();
            for (float f = 85.00f; f <= 85.55f;)
            {
                m_alBMI.Add(string.Format("V{0:F2}", f));
                f += .01f;
            }
            foreach (string str in m_alDiagnosis)
            {
                if (m_alBMI.Contains(str.PadRight(6, '0')))
                {
                    sbRetVal.AppendFormat("Diagnosis's include BMI code {0}'.\r\n", str);
                }
            }

        }

        private void CheckForTransDateServiceDateMismatches(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            // throw new NotImplementedException();
        }

        /// <summary>
        /// Use the m_alTestsModi to do this check. CCI edits use the m_alTests and consumes the array for
        /// recursive testing.
        /// </summary>
        /// <param name="sbRetVal">StringBuilder for errors</param>
        private void CheckForMutex(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            // m_rMutex = new R_mutually_excl(m_strServer, m_strDatabase, ref m_Err);
            // m_rMutex.ClearMemberVariables();

            // ArrayList alCpt4s = new ArrayList();
            // try
            // {
            //     foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
            //     {
            //         if (!alCpt4s.Contains(dr["cpt4"]))
            //         {
            //             alCpt4s.Add(dr["cpt4"]);
            //         }
            //     }
            // }
            // catch (IndexOutOfRangeException)
            // {
            //     sbRetVal.Append("No CPT4's to bill with.");
            //     return;

            // }
            // alCpt4s.Sort();

            //// ArrayList alCopy =  new ArrayList(m_alTestsModi.ToArray());
            // string strWhere = null;

            //// while (alCopy.Count >= 2)
            // while (alCpt4s.Count >= 2)
            // //while (((ArrayList)m_alTestsModi[0]).Count >= 2)
            // {              
            //     foreach (object[] oCpt4 in alCopy)
            //     {
            //        // string strCpt4 = oCpt4.GetValue(0).ToString();

            //         //if (strCpt4 == alCpt4s[0].ToString())
            //         //if (strCpt4 == ((ArrayList)m_alTestsModi[0])[0].ToString())
            //         //{
            //         //    continue;
            //         //}
            //         strWhere = string.Format("cpt4_1 = '{0}' and cpt4_2 = '{1}' " +
            //             " OR cpt4_1 = '{1}' and cpt4_2 = '{0}'", 
            //                 //alCpt4s[0], 
            //                 ((ArrayList)m_alTestsModi[0])[0]
            //                 ,strCpt4);
            //         int nRec = m_rMutex.GetRecords(strWhere);
            //         if (nRec > 10)
            //         {
            //             if (m_dsAccount.Tables["BILLING"].PrimaryKey.GetUpperBound(0) == -1)
            //             {
            //                 m_dsAccount.Tables["BILLING"].PrimaryKey =
            //                     new DataColumn[] 
            //                     { 
            //                         m_dsAccount.Tables["BILLING"].Columns["ACCOUNT"],
            //                         m_dsAccount.Tables["BILLING"].Columns["CPT4"] 
            //                     };
            //             }
            //             DataRow drs = m_dsAccount.Tables["BILLING"].Rows.Find(
            //                 string.Format("account = '{0}' and cpt4 = '{1}'", strCpt4));

            //             if (drs != null && !string.IsNullOrEmpty(drs["modi"].ToString()))
            //             {
            //                 // we have a modifier so probably not a mutually exculsive error
            //             }
            //             else
            //             {
            //                 sbRetVal.AppendFormat("cpt4 {0} and cpt4 {1} are mutually exculsive", 
            //                     //alCpt4s[0].ToString(), 
            //                     ((ArrayList)m_alTestsModi[0])[0],
            //                         strCpt4);

            //             }

            //         }
            //     }
            //     //alCpt4s.RemoveAt(0);
            //     m_alTestsModi.RemoveAt(0);
            //  }

            //// don't need icd9 to check for mutually exclusive cpt4's.
            ////ArrayList alIcd9s = new ArrayList();
            ////try
            ////{
            ////    alIcd9s.AddRange(new object[] {
            ////     dgvPat.Rows[0].Cells["icd9_1"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_2"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_3"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_4"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_5"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_6"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_7"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_8"].Value.ToString().Trim(),
            ////     dgvPat.Rows[0].Cells["icd9_9"].Value.ToString().Trim()
            ////        });

            ////    for (int i = 8; i >= 0; i--)
            ////    {
            ////        if (string.IsNullOrEmpty(alIcd9s[i].ToString()))
            ////        {
            ////            alIcd9s.RemoveAt(i);
            ////        }
            ////    }
            ////}
            ////catch (ArgumentOutOfRangeException)
            ////{
            ////    // no patient record
            ////    sbRetVal.Append("No Patient record");
            ////    return;
            ////}
            ////catch (IndexOutOfRangeException)
            ////{
            ////    // no icd9's.
            ////    sbRetVal.Append("No ICD's");
            ////    return;
            ////}

        }

        /// <summary>
        /// Old way is_cpt4_icd9_ok(CString strAcc, File **ptr, CString payor, CString fin_code)
        ///     note: payor and fin_code unused in the function and not passed by the app. File pointer used to display the message not necessary here
        /// </summary>
        /// <param name="sbRetVal"></param>
        /// <returns>false if the cpt4 is not valid with the ICD. If we don't verify this as BAD for sure
        /// return that is is ok</returns>
        /// <param name="strFinCode">Necessary for check</param>
        private bool is_cpt4_icd9_ok(StringBuilder sbRetVal, string strFinCode)
        {
            Log.Instance.Debug($"Entering");
            string strIcdFormat = "";
            bool bRetVal = false;
            m_rVwLmrp.ClearMemberVariables();
            m_rIcd9desc.ClearMemberVariables();

            // 1. Can't check the LMRP table without a trans_date (ACC) or service_date (Chrg) which should be the same.
            DateTime Service_date = DateTime.MinValue;
            try
            {
                if (!DateTime.TryParse(m_drCur["trans_date"].ToString(), out Service_date))
                {
                    // this is really bad trans_date is not valid!!!!! What to do!?!
                    return true;
                }
            }
            catch (ArgumentException ae)
            {
                m_Err.m_Logfile.WriteLogFile(ae.Message);
                Log.Instance.Error(ae.Message);
                return true;
            }

            catch (IndexOutOfRangeException ioe)
            {
                m_Err.m_Logfile.WriteLogFile(ioe.Message);
                Log.Instance.Error(ioe.Message);
                return true;
            }


            // 2. get the ama year
            int nAmaYear = Service_date.Year;
            DateTime nNewAmaYear = DateTime.Parse(
                string.Format("10/01/{0}", nAmaYear));// October 1 is the start of the new AMAYEAR
            if (Service_date >= nNewAmaYear)
            {
                nAmaYear++;
            }

            //if (m_rVwLmrp.GetRecords(string.Format("ama_year = '{0}'", nAmaYear)) < 1)
            if (m_rVwLmrp.GetRecCount(string.Format("ama_year = '{0}'", nAmaYear)) < 1)
            {
                //sbRetVal.AppendFormat("DO NOT BILL. No rules exist for ama_year {0}.\r\n", nAmaYear);
                sbRetVal.AppendFormat("{1}No rules exist for ama_year {0}.\r\n", nAmaYear, strFinCode == "A" ? "DO NOT BILL. " : ""); // wdk 20130905
                return false;
            }

            ArrayList alCpt4s = new ArrayList();
            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    // wdk 20120210 handle modifiers
                    if (!string.IsNullOrEmpty(dr["modi"].ToString().Trim()))
                    {
                        continue;
                    }
                    if (!alCpt4s.Contains(dr["cpt4"]))
                    {
                        string strWhere = string.Format("cpt4 = '{0}' and ama_year = '{1}' " +
                            " and (rb_date <= '{2} 00:00' and expire_date > '{2}')"
                            , dr["cpt4"], nAmaYear, Service_date.ToShortDateString());
                        int nRec = m_rVwLmrp.GetRecords(strWhere);
                        if (nRec > 1)
                        {
                            alCpt4s.Add(dr["cpt4"]);
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                // continue on may be no billing other code will catch the lack of billing
            }
            if (alCpt4s.Count == 0)
            {
                // no lmrp erorrs possible for this account
                return false;
            }
            if (m_alDiagnosis.Count == 0)
            {
                LoadDiagnosisArray(sbRetVal);
            }

            if (m_alDiagnosis.Count == 0)
            {
                foreach (string strCpt4 in alCpt4s)
                {
                    string strErr = string.Format("{2}LMRP Violation for: CPT4 [{0}]. ICD's [{1}]."
                             , strCpt4, "NONE LISTED", strFinCode == "A" ? "DO NOT BILL. " : ""); // wdk 20140414 // violation

                    if (!sbRetVal.ToString().Contains(strErr))
                    {
                        sbRetVal.AppendLine(strErr);
                    }
                }
                return false;
            }

            //foreach (string strIcd9 in alIcd9s)
            foreach (string strIcd9 in m_alDiagnosis)
            {
                string strFilter = string.Format("icd9_num = '{0}' and ama_year = '{1}'",
                            strIcd9, nAmaYear);
                int nRec = m_rIcd9desc.GetActiveRecords(strFilter);
                if (nRec == 0)
                {
                    string strErr = string.Format("ICD for [{0}] is not on file.", strFilter);
                    if (!sbRetVal.ToString().Contains(strErr))
                    {
                        sbRetVal.AppendLine(strErr);
                    }
                }
            }
            // All variable validation is done and should be available for testing.
            foreach (string strCpt4 in alCpt4s)
            {
                // Validate the ICD's if no ICD's still check to see if the CPT4 is in the table if so must be LMRP error
                if (m_alDiagnosis.Count == 0)
                {
                    //sbRetVal.AppendFormat("Can not check LMRP without ICD's.", nAmaYear);
                    string strLmrpFilter =
                        string.Format(@"ama_year = '{0}' AND cpt4 = '{1}' AND (rb_date < = '{2} 00:00' and expire_date > '{2}') ",
                                        nAmaYear, strCpt4, Service_date.ToString("d"));
                    int nRec = 0;
                    try
                    {
                        nRec = m_rVwLmrp.GetRecords(strLmrpFilter);
                    }
                    catch (SqlException se)
                    {
                        string strLmrpError = string.Format("Error in CheckForLMRP.\r\n"); // violation
                        m_Err.m_Logfile.WriteLogFile(se.Message);
                        Log.Instance.Error(se.Message);
                        break;
                    }

                    if (nRec < 1)
                    {
                        // there is no LMRP rule for this CPT4
                        continue; // not an error
                    }
                    return false;
                }

                int bad_icd9 = 1;
                //foreach (string icd9 in alIcd9s)
                foreach (string icd9 in m_alDiagnosis)
                {
                    if (bad_icd9 == 1)
                    {
                        bad_icd9 = is_lmrp_violation(strCpt4, icd9, nAmaYear, Service_date);
                    }
                    else
                    {
                        break;
                    }
                }
                if (bad_icd9 == 1)
                {
                    // lmrp violation                 
                    // check for abn
                    // otherwise report error
                    string strErr = string.Format("{2}LMRP Violation for: CPT4 [{0}]. ICDs[{1}]."
                        , strCpt4, strIcdFormat, strFinCode == "A" ? "DO NOT BILL. " : ""); // wdk 20130905 // violation
                    if (!sbRetVal.ToString().Contains(strErr))
                    {
                        sbRetVal.AppendFormat(strErr);
                    }

                }
            }
            return bRetVal;
        }

        private int is_lmrp_violation(string strCpt4, string icd9, int nAmaYear, DateTime Service_date)
        {
            Log.Instance.Debug($"Entering");
            int nRetVal = 1;
            // set the filter to determine if this CPT4 has a rule in the table.
            string strLmrpFilter =
                string.Format(@"ama_year = '{0}' AND cpt4 = '{1}' AND (rb_date < = '{2} 00:00' and expire_date > '{2}') ",
                                nAmaYear, strCpt4, Service_date.ToString("d"));
            int nRec = 0;
            try
            {
                nRec = m_rVwLmrp.GetRecords(strLmrpFilter);
            }
            catch (SqlException se)
            {
                m_Err.m_Logfile.WriteLogFile(se.Message);
                Log.Instance.Error(se.Message);
                return 0;
            }

            if (nRec < 1)
            {
                // there is no LMRP rule for this CPT4
                return 0;
            }
            // There is a rule set the type Good or Bad
            int save_chk_for_bad = int.Parse(m_rVwLmrp.propChk_for_bad);

            string strLmrpRefilter =
            string.Format("(cpt4 = '{0}') AND (beg_icd9 <= '{1}' AND end_icd9 >= '{1}') AND ama_year = '{2}' AND (rb_date <= '{3} 00:00' AND expire_date > '{4}')",
                                        strCpt4,
                                         icd9,
                                           nAmaYear,
                                             Service_date.ToString("d"),
                                              Service_date.ToString("d"));

            int nRecVwLmrp = m_rVwLmrp.GetRecords(strLmrpRefilter);

            if (nRecVwLmrp == 0)
            {
                if (save_chk_for_bad == 0)
                {
                    return 0; // ok
                }
                else
                {
                    return 1; // violation
                }
            }

            if (nRecVwLmrp == 1)
            {
                if (save_chk_for_bad == 0)
                {
                    return 1; // violation
                }
                else
                {
                    return 0; // ok
                }
            }
            return nRetVal;
        }

        private void CheckForCCIEdits(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }

            if (m_alTests.Count == 0)
            {
                LoadTestArray();
            }

            if (m_alTests.Count < 2)
            {
                return;
            }

            string strFirstCpt4 = null;
            string strSecondCpt4 = null;
            IEnumerator ie = m_alTests.GetEnumerator();
            ie.Reset();
            ie.MoveNext();
            strFirstCpt4 = ((string)ie.Current);
            string strRemove = strFirstCpt4;
            while (ie.MoveNext())
            {
                strSecondCpt4 = ((string)ie.Current);
                string strWhere =
                    string.Format("((ME_1 = '{0}') and (ME_2 = '{1}')) or ((ME_1 = '{1}') and (ME_2 = '{0}'))",
                    strFirstCpt4, strSecondCpt4);
                int raERec = m_rAEdit.GetRecords(strWhere);
                if (raERec > 0)
                {

                    DataRow[] drModi =
                        m_dsAccount.Tables["BILLING"].Select(string.Format("cpt4 = '{0}'", m_rAEdit.propMe_2));
                    //        m_dsAccount.Tables["BILLING"].Select(string.Format("cpt4 = '{0}'", strSecondCpt4));
                    bool bHasNoModi = string.IsNullOrEmpty(drModi[0]["MODI"].ToString());

                    if (m_rAEdit.propCci_indicator == "0")
                    {
                        string strErr = string.Format("A_CPEDIT CPT4s {0} and {1} are mutually exclusive. And, have a CCI indicator of {2}.",
                            m_rAEdit.propMe_1, //strFirstCpt4, 
                            m_rAEdit.propMe_2 //strSecondCpt4
                            , m_rAEdit.propCci_indicator);
                        if (!sbRetVal.ToString().Contains(strErr))
                        {
                            sbRetVal.AppendLine(strErr);
                        }
                    }
                    if (m_rAEdit.propCci_indicator == "1" && bHasNoModi)
                    {
                        string strErr = string.Format("A_CPEDIT CPT4s {0} and {1} are invalid together. And, have a CCI indicator of {2}.",
                          m_rAEdit.propMe_1, //strFirstCpt4, 
                            m_rAEdit.propMe_2 //strSecondCpt4
                            , m_rAEdit.propCci_indicator);
                        if (!sbRetVal.ToString().Contains(strErr))
                        {
                            sbRetVal.AppendLine(strErr);
                        }
                    }
                }

                int rcErec = m_rCEdit.GetRecords(strWhere);
                if (rcErec > 0)
                {
                    // wdk 20120827 changed below to use the second ME in the table instead of the second cpt4 that is tested.
                    DataRow[] drModi =
                       m_dsAccount.Tables["BILLING"].Select(string.Format("cpt4 = '{0}'", m_rCEdit.propMe_2));
                    bool bHasNoModi = string.IsNullOrEmpty(drModi[0]["MODI"].ToString());

                    if (m_rCEdit.propCci_indicator == "0")
                    {
                        string strErr = string.Format("C_MEEDIT CPT4s {0} and {1} are mutually exclusive. And, have a CCI indicator of {2}.",
                            m_rCEdit.propMe_1, //strFirstCpt4, 
                            m_rCEdit.propMe_2 //strSecondCpt4
                            , m_rCEdit.propCci_indicator);
                        if (!sbRetVal.ToString().Contains(strErr))
                        {
                            sbRetVal.AppendLine(strErr);
                        }
                    }
                    if (m_rCEdit.propCci_indicator == "1" && bHasNoModi)
                    {
                        string strErr = string.Format("C_MEEDIT CPT4s {0} and {1} are invalid together. And, have a CCI indicator of {2}.",
                            m_rCEdit.propMe_1, //strFirstCpt4, 
                            m_rCEdit.propMe_2 //strSecondCpt4
                            , m_rCEdit.propCci_indicator);
                        if (!sbRetVal.ToString().Contains(strErr))
                        {
                            sbRetVal.AppendFormat(strErr);
                        }
                    }
                }
            }
            m_alTests.Remove(strRemove);

            CheckForCCIEdits(sbRetVal); // recursive call to get the 2nd cpt4 etc against the remaining ones.
        }

        private void CheckForAccidentDiagnosisCodes(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }

            foreach (string strDiag in m_alDiagnosis)
            {
                if (strDiag.StartsWith("8") || strDiag.StartsWith("9") || strDiag.StartsWith("E"))
                {
                    sbRetVal.AppendFormat("Contains diagnosis code [{0}] that starts with [{1}].\r\n", strDiag, strDiag.Substring(0, 1));
                }
            }

        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForBehavioralCodes(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            foreach (string strDiag in m_alDiagnosis)
            {
                float lParseMin = 290.0f;
                float lParseMax = 319.99f;
                float lParseVal = float.MaxValue;
                if (!float.TryParse(strDiag, out lParseVal)) // has a non numeric value so it can't be what we are screening for here
                {
                    continue;
                }
                if (lParseVal >= lParseMin &&
                        lParseVal <= lParseMax)
                {
                    sbRetVal.Append(
                    string.Format("Diagnosis's include behavioral diagnosis '{0}'.\r\n", strDiag));
                }
            }

        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForAbnormalWeightGain(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }

            if (m_alDiagnosis.Contains("783.1"))
            {
                sbRetVal.Append("Diagnosis's include obesity code '783.1'.\r\n");
            }
        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForObesityCodes(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }

            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            if (m_alDiagnosis.Contains("278.00"))
            {
                sbRetVal.Append("Diagnosis's include obesity code '278.00'.\r\n");
            }
            if (m_alDiagnosis.Contains("278.01"))
            {
                sbRetVal.Append("Diagnosis's include obesity code '278.01'.\r\n");
            }
            if (m_alDiagnosis.Contains("278.02"))
            {
                sbRetVal.Append("Diagnosis's include obesity code '278.02'.\r\n");
            }

        }

        private void CheckForPolicyNumHasNoPrefix(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString()))
                {
                    sbRetVal.Append("Primary Insurance has no policy number.\r\n");
                }
                string strPolicy = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
                long lOut = long.MaxValue;
                if (!long.TryParse(strPolicy, out lOut))
                {
                    sbRetVal.Append("Primary Insurance policy number contains non numeric characters.\r\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

        }

        private void CheckForDiagCodePointers(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            //else
            //{
            //    m_alDiagnosis.TrimToSize();
            //    for (int i = (m_alDiagnosis.Count - 1); i >= 0; i--)
            //    {
            //        if (string.IsNullOrEmpty(m_alDiagnosis[i].ToString()))
            //        {
            //            m_alDiagnosis.RemoveAt(i);
            //        }
            //    }
            //}

            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    string strPointer = dr["diagnosis_code_ptr"].ToString();
                    if (string.IsNullOrEmpty(strPointer))
                    {
                        sbRetVal.AppendFormat("CPT4 {0} has a blank diagnosis code pointer.\r\n", dr["cpt4"].ToString());
                        continue;
                    }
                    string[] strPointerSisters = strPointer.Split(new char[] { ':' }, StringSplitOptions.RemoveEmptyEntries);
                    for (int i = 0; i <= strPointerSisters.GetUpperBound(0); i++)
                    {
                        int nOut = int.MaxValue;
                        if (int.TryParse(strPointerSisters[i].ToString(), out nOut))
                        {
                            if (nOut > m_alDiagnosis.Count)
                            {
                                sbRetVal.AppendFormat("CPT4 {0} has an invalid diagnosis code pointer. The value is higher than the number of diagnosis's.\r\n", dr["cpt4"].ToString());
                            }
                        }
                        else
                        {
                            sbRetVal.AppendFormat("CPT4 {0} has an invalid diagnosis code pointer. The value is not convertable to a number.\r\n", dr["cpt4"].ToString());
                        }

                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
            }
        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForCpt80101(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (dr["cpt4"].ToString().Contains("80101"))
                    {
                        sbRetVal.Append("Charges contain cpt4 80101.\r\n");
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }

        }

        private void CheckForPolicyStartsWithWX(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            try
            {
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString()))
                {
                    sbRetVal.Append("Primary Insurance has no policy number.\r\n");
                }
                string strPolicy = m_dsAccount.Tables["INS"].Rows[0]["policy_num"].ToString();
                if (!strPolicy.StartsWith("WX"))
                {
                    sbRetVal.Append("Primary Insurance policy number does not start with 'WX'.\r\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }
        /// <summary>
        /// wdk 20111006 added by Carol see email
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForDiagCodeV70_3(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            if (m_alDiagnosis.Contains("V70.3"))
            {
                sbRetVal.Append("Diagnosis's include 'V70.3'.\r\n");
            }

        }

        /// <summary>
        /// wdk 20111006 added by Carol see email
        /// </summary>
        /// <param name="sbRetVal"></param>
        /// <param name="strFinCode"></param>
        private void CheckForDiagCodeV72_42(StringBuilder sbRetVal, string strFinCode)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            // exclude specific fincodes.
            // wdk 20120521 per Carol do not need to validate TNBC for this item
            if (strFinCode == "D")
            {
                return;
            }
            if (m_alDiagnosis.Contains("V72.42"))
            {
                sbRetVal.Append("Diagnosis's include 'V72.42'.\r\n");
            }

        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForDiagCodeV72_31(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            //ArrayList alDiag = new ArrayList();
            // try
            // {
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_1"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_2"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_3"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_4"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
            //     alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
            // }
            // catch (IndexOutOfRangeException)
            // {
            //     return;
            // }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            if (m_alDiagnosis.Contains("V72.31"))
            {
                sbRetVal.Append("Diagnosis's include 'V72.31'.\r\n");
            }

        }

        private void CheckForDiagCodesV22_THRU_V23_89_OR_614_THRU_677(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            string strSex = m_dsAccount.Tables["PAT"].Rows[0]["sex"].ToString();
            if (strSex != "F")
            {
                ArrayList al = new ArrayList();
                double dStart = 22;
                while (double.Parse(dStart.ToString("F2")) <= 23.89)
                {
                    al.Add(string.Format(string.Format("V{0:f2}", dStart), dStart.ToString("F2")));
                    dStart += .01;
                }
                dStart = 614;
                while (double.Parse(dStart.ToString("F2")) <= 677)
                {
                    al.Add(string.Format(string.Format("{0:f2}", dStart), dStart.ToString("F2")));
                    dStart += .01;
                }
                string strDiag = "";
                foreach (string str in m_alDiagnosis)
                {
                    strDiag = str;
                    // rgc/wdk 20120328 added to make the strings length 6 so the contains matches xxx.9 etc.
                    strDiag += str.Length <= 4 ? "0" : "";
                    strDiag += str.Length <= 5 ? "0" : "";
                    if (al.Contains(strDiag))
                    {
                        sbRetVal.Append(
                            string.Format("Diagnosis Code [{0}] on Patient Record that has sex listed as [{1}].\r\n",
                            strDiag, strSex == "M" ? "MALE" : "UNKNOWN/OTHER"));
                        return;
                    }
                }
            }

        }

        /// <summary>
        /// Takes the patients ICD9 and loads them into the array. If this icd9 is already in the array we indicate 
        /// that there are duplicates on the account, but we don't load the duplicates into the array. 
        /// </summary>
        /// <returns></returns>
        private int LoadDiagnosisArray(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            try
            {
                m_alDiagnosis.AddRange(new object[]
                {
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_1"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_2"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_3"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_4"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString(),
                    m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString()
                }
                );

                ArrayList alCodes = new ArrayList();
                for (int i = m_alDiagnosis.Count - 1; i >= 0; i--)
                {
                    if (string.IsNullOrEmpty(m_alDiagnosis[i].ToString()))
                    {
                        m_alDiagnosis.RemoveAt(i);
                        continue;
                    }
                    if (!alCodes.Contains(m_alDiagnosis[i]))
                    {
                        alCodes.Add(m_alDiagnosis[i]);
                    }
                    else
                    {
                        sbRetVal.AppendFormat("Diagnosis [{0}] was listed more than once.\r\n", m_alDiagnosis[i]);
                        m_alDiagnosis.RemoveAt(i);
                    }
                }
                m_alDiagnosis.TrimToSize();

                /* wdk 20130806 if the bill form is a UB don't load the retval error;*/
                DataRow[] dr = m_dsAccount.Tables["ACC"].Select(
                    string.Format("account = '{0}'", m_dsAccount.Tables["PAT"].Rows[0]["account"]));
                string strBillType = dr[0]["bill_form"].ToString();
                /* wdk 20130404 requested because the 1500 only takes 4 and this is 
                         * allowing the last 5 to validate but not be used.*/
                //if (!m_alDiagnosis.Contains(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString()))
                //{
                //    if (!string.IsNullOrEmpty(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString())
                //        && strBillType == "1500")
                //    {
                //        //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
                //        sbRetVal.AppendFormat("Diagnosis 5 - [{0}] is available but not listed.\r\n",
                //            m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
                //    }
                //}
                //else
                //{
                //    sbRetVal.AppendFormat("Diagnosis [{0}] is listed more than once.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
                //}
                //if (!m_alDiagnosis.Contains(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString()))
                //{
                //    if (!string.IsNullOrEmpty(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString())
                //        && strBillType == "1500")
                //    {
                //        //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
                //        sbRetVal.AppendFormat("Diagnosis 6 - [{0}] is available but not listed.\r\n",
                //            m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
                //    }
                //}
                //else
                //{
                //    sbRetVal.AppendFormat("Diagnosis [{0}] is listed more than once.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
                //}
                //if (!m_alDiagnosis.Contains(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString()))
                //{
                //    if (!string.IsNullOrEmpty(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString())
                //        && strBillType == "1500")
                //    {
                //        //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
                //        sbRetVal.AppendFormat("Diagnosis 7 - [{0}] is available but not listed.\r\n",
                //            m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
                //    }
                //}
                //else
                //{
                //    sbRetVal.AppendFormat("Diagnosis [{0}] is listed more than once.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
                //}
                //if (!m_alDiagnosis.Contains(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString()))
                //{
                //    if (!string.IsNullOrEmpty(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString())
                //        && strBillType == "1500")
                //    {
                //        //m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
                //        sbRetVal.AppendFormat("Diagnosis 8 - [{0}] is available but not listed.\r\n",
                //            m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
                //    }
                //}
                //else
                //{
                //    sbRetVal.AppendFormat("Diagnosis [{0}] is listed more than once.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
                //}
                //if (!m_alDiagnosis.Contains(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString()))
                //{
                //    if (!string.IsNullOrEmpty(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString())
                //        && strBillType == "1500")
                //    {
                //       // m_alDiagnosis.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
                //        sbRetVal.AppendFormat("Diagnosis 9 - [{0}] is available but not listed.\r\n",
                //            m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
                //    }
                //}
                //else
                //{
                //    sbRetVal.AppendFormat("Diagnosis [{0}] is listed more than once.\r\n", m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
                //}

            }
            catch (IndexOutOfRangeException)
            {
                // 
            }


            // wdk 20131112 code added to table

            //if (m_alDiagnosis.Count == 0)
            //{
            //    string strNote = "No diagnosis for this patient.\r\n";
            //    if (!sbRetVal.ToString().Contains(strNote))
            //    {
            //        sbRetVal.Append("No diagnosis for this patient.\r\n");
            //    }
            //}
            //else
            if (m_alDiagnosis.Count > 0)
            {
                for (int i = (m_alDiagnosis.Count - 1); i >= 0; i--)
                {
                    if (string.IsNullOrEmpty(m_alDiagnosis[i].ToString()))
                    {
                        m_alDiagnosis.RemoveAt(i);
                    }
                }
            }


            return m_alDiagnosis.Count;
        }

        private void CheckForDiagCodeStartsWithV(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            //ArrayList alDiag = new ArrayList();
            //try
            //{
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_1"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_2"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_3"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_4"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    return;
            //}
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            foreach (string str in m_alDiagnosis)
            {
                if (str.StartsWith("V"))
                {
                    sbRetVal.Append("Diagnosis starts with 'V'.\r\n");
                    return;
                }
            }

        }

        /// <summary>
        /// </summary>
        /// <param name="strAccount"></param>
        /// <param name="sbRetVal"></param>
        private void CheckForPatientNameSameAsPolicyHolderName(string strAccount, StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            string strAccName = null;
            DataRow[] dr = null;
            try
            {
                dr = m_dsAccount.Tables["ACC"].Select(
                string.Format("ACCOUNT = '{0}'", strAccount));

            }
            catch (Exception)
            {
                return;
            }
            try
            {
                string strName = dr[0]["pat_name"].ToString().Trim().ToUpper();
                strAccName = m_dsAccount.Tables["INS"].Rows[0]["holder_nme"].ToString().ToUpper().Trim();
                if (strName != strAccName)
                {
                    if (strAccName.Contains(" "))
                    {
                        strAccName = strAccName.Substring(0, strAccName.LastIndexOf(" "));
                    }
                    if (strName.Contains(" "))
                    {
                        strName = strName.Substring(0, strName.LastIndexOf(" "));
                    }
                    if (strName != strAccName)
                    {
                        sbRetVal.Append("Primary insurance holder name and patient name are not the same.\r\n");
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        private void CheckForInsHolderInfoNotBlank(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            try
            {
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["holder_sex"].ToString()))
                {
                    sbRetVal.Append("SSI Edit Primary Insurance must have a holder sex.\r\n");
                }
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["holder_dob"].ToString()))
                {
                    sbRetVal.Append("SSI Edit Primary Insurance must have a holder date of birth.\r\n");
                }
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["relation"].ToString()))
                {
                    sbRetVal.Append("SSI Edit Primary Insurance must have a relation code.\r\n");
                }
                else // rgc/wdk 20120216 added check to ensure that the pat and ins relation are the same.
                {
                    if (m_dsAccount.Tables["PAT"].Rows[0]["relation"].ToString() !=
                        m_dsAccount.Tables["INS"].Rows[0]["relation"].ToString())
                    {
                        sbRetVal.Append(string.Format("Pat relation = [{0}] and Ins relation = [{1}].",
                            m_dsAccount.Tables["PAT"].Rows[0]["relation"].ToString(),
                            m_dsAccount.Tables["INS"].Rows[0]["relation"].ToString()));
                    }
                }


            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        /// <summary>
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForInsGroupNumber(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            try
            {
                if (string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["grp_num"].ToString()))
                {
                    sbRetVal.Append("Primary Insurance has no group number.\r\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        private void CheckForGroupNumberMustBeBlank(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            try
            {
                if (!string.IsNullOrEmpty(m_dsAccount.Tables["INS"].Rows[0]["grp_num"].ToString()))
                {
                    sbRetVal.Append("Primary Insurance has group number but should be blank.\r\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForPatientRelationAsSelf(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");

            try
            {
                if (m_dsAccount.Tables["PAT"].Rows[0]["relation"].ToString() != "01")
                {
                    sbRetVal.Append("Patient relation is not 01.\r\n");
                }
            }
            catch (IndexOutOfRangeException)
            {
                // previously reported no pat record so skip town.
            }
        }

        /// <summary>
        /// GH panel consists of 
        ///     CBC -- 85025,
        ///     CMP -- 80053,
        ///     TSH -- 84443
        /// </summary>
        /// <param name="sbRetVal">Error list for this account</param>
        private void CheckForGHPPanel(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            ArrayList alTests = new ArrayList();
            foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
            {
                if (!alTests.Contains(dr["cpt4"]))
                {
                    alTests.Add(dr["cpt4"]);
                }
            }
            if (alTests.Contains("85025") &&
                alTests.Contains("80053") &&
                alTests.Contains("84443"))
            {
                sbRetVal.Append("Account contains an un bundled GHP panel.\r\n");
            }
        }

        /// <summary>
        /// OB panel consists of 
        ///     CBC -- 85025, 
        ///     HBSAG -- 87340,
        ///     RUB -- 86762,
        ///     RPR -- 86592, 
        ///     ABS -- 86850, 
        ///     ABO -- 86900 and 
        ///     RH -- 86901 
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForOBPanel(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            ArrayList alTests = new ArrayList();
            try
            {
                foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
                {
                    if (!alTests.Contains(dr["cpt4"]))
                    {
                        alTests.Add(dr["cpt4"]);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                return;
            }
            if (alTests.Contains("85025") &&
                alTests.Contains("87340") &&
                alTests.Contains("86762") &&
                alTests.Contains("86592") &&
                alTests.Contains("86850") &&
                alTests.Contains("86900") &&
                alTests.Contains("86901"))
            {
                sbRetVal.Append("Account contains an unbundled OB panel.\r\n");
            }


        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForInfertilityOrDentalCodes(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name)
                )
            {
                return;
            }

            //ArrayList alDiag = new ArrayList();
            //try
            //{
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_1"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_2"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_3"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_4"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_5"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_6"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_7"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_8"].ToString());
            //    alDiag.Add(m_dsAccount.Tables["PAT"].Rows[0]["icd9_9"].ToString());
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    return;
            //}
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            if (m_alDiagnosis.Contains("521.0") || m_alDiagnosis.Contains("521.00"))
            {
                sbRetVal.Append("Account contains Diagnosis code 521.0.\r\n");
            }
            if (m_alDiagnosis.Contains("524.62"))
            {
                sbRetVal.Append("Account contains Diagnosis code 524.62.\r\n");
            }
            if (m_alDiagnosis.Contains("606.9") || m_alDiagnosis.Contains("606.90"))
            {
                sbRetVal.Append("Account contains Diagnosis code 606.9.\r\n");
            }
            if (m_alDiagnosis.Contains("628.0") || m_alDiagnosis.Contains("628.00"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.0.\r\n");
            }
            if (m_alDiagnosis.Contains("628.1") || m_alDiagnosis.Contains("628.10"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.1.\r\n");
            }
            if (m_alDiagnosis.Contains("628.2") || m_alDiagnosis.Contains("628.2"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.2.\r\n");
            }
            if (m_alDiagnosis.Contains("628.3") || m_alDiagnosis.Contains("628.30"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.3.\r\n");
            }
            if (m_alDiagnosis.Contains("628.4") || m_alDiagnosis.Contains("628.40"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.4.\r\n");
            }
            if (m_alDiagnosis.Contains("628.5") || m_alDiagnosis.Contains("628.50"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.5.\r\n");
            }
            if (m_alDiagnosis.Contains("628.6") || m_alDiagnosis.Contains("628.60"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.6.\r\n");
            }
            if (m_alDiagnosis.Contains("628.7") || m_alDiagnosis.Contains("628.70"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.7.\r\n");
            }
            if (m_alDiagnosis.Contains("628.8") || m_alDiagnosis.Contains("628.80"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.8.\r\n");
            }
            if (m_alDiagnosis.Contains("628.9") || m_alDiagnosis.Contains("628.90"))
            {
                sbRetVal.Append("Account contains Diagnosis code 628.9.\r\n");
            }
            if (m_alDiagnosis.Contains("704.0") || m_alDiagnosis.Contains("704.00"))
            {
                sbRetVal.Append("Account contains Diagnosis code 704.00.\r\n");
            }
        }

        private void CheckInsNamesForNonAlphaChars(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set m_bExcludeHardCodedCode in system table where key_name = INCLUDE
            // the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            //bool bHasNonAlphaChar = false;
            int nApostrophesInName = 0;
            // int nSpaceInName = 0;
            if (m_drCurIns == null)
            {
                return;
            }

            string strAcc = m_drCurIns["account"].ToString();
            string strPatName = m_drCurIns["holder_nme"].ToString();
            if (string.IsNullOrEmpty(strPatName))
            {
                sbRetVal.Append("Insurance Holder name is blank\r\n");
            }
            if (!strPatName.Contains(','))
            {
                sbRetVal.Append("Insurance Holder name not valid (does not cotain an comma.)\r\n");
                return;
            }

            // split into hopefully 2 parts only!
            string[] strName = strPatName.Split(new char[] { ',' });

            // if more than 2 elements in the array. The name is not valid because it contains multiple commas
            if (strName.GetUpperBound(0) != 1)
            {
                sbRetVal.AppendFormat("Insurance Holder name [{0}] has [{1}] commas in the name.\r\n", strPatName, (strName.GetUpperBound(0) + 1));
                return;
            }
            // create two hopefull no more than 2 element arrays for Last name and name suffix
            string[] strLSName = strName[0].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
            // and first and middle names
            string[] strFMName = strName[1].Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            // create the 4 parts of the name we think are possible with our input schema.
            string strNameLast = null;
            string strNameSuffix = null;
            string strNameFirst = null;
            string strNameMiddle = null;
            // use the try catch to keep the application running when the name does not contain a suffix 
            // the name suffix will contain a last name part when the name is entered with a space we will handle
            // that next
            try
            {
                strNameLast = strLSName[0].Trim();
                strNameSuffix = strLSName[1].Trim();
            }
            catch (IndexOutOfRangeException)
            {
                // no problem 
            }
            // if last name contained a space-anated name like "SMITH JONES" handle it here
            if (!string.IsNullOrEmpty(strNameSuffix) &&
                ((strNameSuffix.ToUpper() == "JR" ||
                strNameSuffix.ToUpper() == "SR" ||
                strNameSuffix.ToUpper() == "II" ||
                strNameSuffix.ToUpper() == "III" ||
                strNameSuffix.ToUpper() == "IV")))
            //&& nSpaceInLastName == 1)
            {
                // the strNameSuffix is ok
            }
            else
            {
                strNameLast += strNameSuffix;
                strNameSuffix = null;
            }

            // now split the first and if available middle names, using the try to eliminate crash
            try
            {
                strNameFirst = strFMName[0].Trim();
                strNameMiddle = strFMName[1].Trim();
            }
            catch (IndexOutOfRangeException)
            {
                // no problem 
            }

            try
            {
                foreach (char c in strNameLast)
                {
                    if (!char.IsLetter(c))
                    {
                        if (c.ToString() == "'") // for o'connor etc
                        {
                            nApostrophesInName++;
                            if (nApostrophesInName == 2)
                            {
                                sbRetVal.Append("Insurance Holders last name contains multiple apostrophes.\r\n");
                            }
                            continue;
                        }
                        if (c.ToString() == "-")
                        {
                            continue; // hyphens are ok in the last name 
                        }

                        sbRetVal.AppendFormat("Insurance Holders last name contains [{0}] which is invalid in a name.", c.ToString());
                    }
                }
            }
            catch (NullReferenceException)
            {
                sbRetVal.Append("Insurance Holders last name is blank.");
            }

            nApostrophesInName = 0;
            //nSpaceInName = 0;
            try
            {
                foreach (char c in strNameFirst)
                {
                    if (!char.IsLetter(c))
                    {
                        if (c.ToString() == "'") // for o'connor etc
                        {
                            nApostrophesInName++;
                            if (nApostrophesInName == 2)
                            {
                                sbRetVal.Append("Insurance Holders first name contains multiple apostrophes.\r\n");
                            }
                            continue;
                        }

                        sbRetVal.AppendFormat("Insurance Holders first name contains [{0}] which is invalid in a name.", c.ToString());
                    }
                }
            }
            catch (NullReferenceException)
            {
                sbRetVal.Append("Insurance Holders first name is blank.");
            }

            nApostrophesInName = 0;
            // nSpaceInName = 0;
            try
            {
                foreach (char c in strNameMiddle)
                {
                    if (!char.IsLetter(c))
                    {
                        if (c.ToString() == "'") // for o'connor etc
                        {
                            nApostrophesInName++;
                            if (nApostrophesInName == 2)
                            {
                                sbRetVal.Append("Insurance Holders middle name contains multiple apostrophes.\r\n");
                            }
                            continue;
                        }

                        sbRetVal.AppendFormat("Insurance Holders middle name contains [{0}] which is invalid in a name.", c.ToString());
                    }
                }
            }
            catch (NullReferenceException)
            {
                // nothing in the middle name is acceptable
            }

            nApostrophesInName = 0;
            //nSpaceInName = 0;
            try
            {
                foreach (char c in strNameSuffix)
                {
                    if (!char.IsLetter(c))
                    {
                        if (c.ToString() == "'") // for o'connor etc
                        {
                            nApostrophesInName++;
                            if (nApostrophesInName == 2)
                            {
                                sbRetVal.Append("Insurance Holders name suffix contains multiple apostrophes.\r\n");
                            }
                            continue;
                        }
                        sbRetVal.AppendFormat("Insurance Holders name suffix contains [{0}] which is invalid in a name.", c.ToString());
                    }
                }
            }
            catch (NullReferenceException)
            {
                // nothing in the suffix is ok
            }
        }

        private void CheckNamesForNonAlphaChars(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set m_bExcludeHardCodedCode in system table where key_name = INCLUDE
            // the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            bool bHasNonAlphaChar = false;
            // DataGridViewRow dr = null;
            string strAcc = null;
            strAcc = m_drCur["account"].ToString();

            string strPatName = null;
            strPatName = m_drCur["pat_name"].ToString();
            if (!strPatName.Contains(','))
            {
                sbRetVal.Append("Patient name not valid.\r\n");
                return;
            }


            // ((DataRowView)dr.DataBoundItem).Row["pat_name"];
            string[] strName = strPatName.Split(new char[] { ',' });
            StringBuilder strErr = new StringBuilder();
            string strNameSuffix = "";
            int nSpaceInLastName = 0;
            int nApostrophesInName = 0;
            foreach (char c in strName[0])
            {
                if (!char.IsLetter(c))
                {
                    if (c.ToString() == "'") // for 0'connor etc
                    {
                        continue;
                        // nApostrophesInName++;
                    }
                    if (char.IsWhiteSpace(c))
                    {
                        nSpaceInLastName++;
                        strNameSuffix = "";
                    }
                    //strErr.AppendFormat("{0} has a non alpha character in the last name.\r\n", strName[0]);
                    bHasNonAlphaChar = true;
                    continue;
                }
                if (nSpaceInLastName > 0)
                {
                    strNameSuffix += c.ToString();
                }
            }
            if (nApostrophesInName > 1)
            {
                sbRetVal.AppendFormat("Name contains [{0}] aprostophes", nApostrophesInName);
            }
            foreach (char c in strName[1])
            {
                if (char.IsPunctuation(c))
                {
                    if (c == '\'')
                    {
                        continue;
                    }
                    if (c == '-')
                    {
                        continue;
                    }
                    sbRetVal.AppendFormat("Patients first name [{0}] contains an invalid character.\r\n", strName[1].ToString());
                    return;
                }
            }
            if ((strNameSuffix.ToUpper() == "JR" ||
                    strNameSuffix.ToUpper() == "SR" ||
                    strNameSuffix.ToUpper() == "II" ||
                    strNameSuffix.ToUpper() == "III" ||
                    strNameSuffix.ToUpper() == "IV")
                && nSpaceInLastName == 1)
            {
                bHasNonAlphaChar = false;
            }
            else
            {
                if (bHasNonAlphaChar)
                {
                    strErr.AppendFormat("{0} has a non alpha character in the last name.\r\n", strName[0]);
                }
            }

            if (strName[1].Contains(" "))
            {
                string[] strSplitFirstName = strName[1].Split(new string[] { " " }, StringSplitOptions.None);
                if (strSplitFirstName.GetUpperBound(0) > 1)
                {
                    strErr.Append("First or Middle name contains too many spaces.\r\n");
                    bHasNonAlphaChar = true;
                }
            }
            if (bHasNonAlphaChar)
            {
                sbRetVal.Append(strErr.ToString());
            }

        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForV70Diag(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            if (m_alDiagnosis.Contains("V70.0"))
            {
                sbRetVal.Append("Account contains Diagnosis code V70.0.\r\n");
            }
        }

        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckForV70_9Diag(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }
            if (m_alDiagnosis.Count == 0)
            {
                if (LoadDiagnosisArray(sbRetVal) == 0)
                {
                    return;
                }
            }
            if (m_alDiagnosis.Contains("V70.9"))
            {
                sbRetVal.Append("Account contains Diagnosis code V70.9.\r\n");
            }
        }

        private void CheckForGender(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set m_bExcludeHardCodedCode in system table where key_name = INCLUDE
            // the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            foreach (DataRow dr in m_dsAccount.Tables["PAT"].Rows)
            {
                if (dr["SEX"].ToString() == "F" || dr["SEX"].ToString() == "M")
                {
                    return;
                }
            }
            // rgc/wdk 20120412 moved from foreach loop.
            sbRetVal.Append("Patient's Gender is not listed as Male or Female.\r\n");

        }

        private void CheckForMultipleVeniPunctures(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            DataRow[] dr = m_dsAccount.Tables["BILLING"].Select("CPT4 = '36415'");
            if (dr.GetUpperBound(0) == -1)
            {
                return;
            }
            if (int.Parse(dr[0]["qty"].ToString()) > 1)
            {
                sbRetVal.Append("DO NOT BILL. Account contains multiple Venipunctures.\r\n");
            }
        }

        private void CheckForVeniPunctureOnly(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            if (m_dsAccount.Tables["BILLING"].Rows.Count == 1)
            {
                if (m_dsAccount.Tables["BILLING"].Rows[0]["CPT4"].ToString().Contains("36415"))
                {
                    sbRetVal.Append("DO NOT BILL. Account contains only a Venipuncture for billing.\r\n");
                }
            }
        }
        /// <summary>
        /// MODIFIED
        /// </summary>
        /// <param name="sbRetVal"></param>
        private void CheckFor80299(StringBuilder sbRetVal)
        {
            Log.Instance.Debug($"Entering");
            Log.Instance.Debug($"Entering");
            // wdk 20151109 set the restricted_users are pipe delimited mehods to exclude if set to true.
            if (m_bExcludeHardCodedCode || m_alMethods.Contains(MethodBase.GetCurrentMethod().Name))
            {
                return;
            }

            foreach (DataRow dr in m_dsAccount.Tables["BILLING"].Rows)
            {
                if (dr["cpt4"].ToString() == "80299")
                {
                    sbRetVal.Append("Account contains CPT4 80299.\r\n");
                }
            }
        }

        private void CreateDataSet()
        {
            Log.Instance.Debug($"Entering");
            m_dsAccount.Tables.Add("ACC");
            m_dsAccount.Tables.Add("PAT");
            m_dsAccount.Tables.Add("INS");
            m_dsAccount.Tables.Add("BILLING");
            m_dsAccount.Tables.Add("CHRG_ERR");

            dgvAccount.TopLeftHeaderCell.Value = "ACCOUNTS";
            dgvAccount.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            dgvPat.TopLeftHeaderCell.Value = "PATIENT";
            dgvPat.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvInsurance.TopLeftHeaderCell.Value = "INSURANCE";
            dgvInsurance.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvBilling.TopLeftHeaderCell.Value = "BILLING";
            dgvBilling.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            dgvChrgErr.TopLeftHeaderCell.Value = "CHRG ERROR";
            dgvChrgErr.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void CreateDateTimes()
        {
            Log.Instance.Debug($"Entering");
            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                Application.DoEvents();
                tsslNote.Text = "Loading Bill Thru Date";
                // get the operating date range for queries.

                SqlCommand cdmStartDate =
                        new SqlCommand("select value from dbo.system where key_name = 'ssi_bill_thru_date'", connection);
                sda.SelectCommand = cdmStartDate;
                Application.DoEvents();
                tsslNote.Text = "Loading Select Command";
                DataTable dtStart = new DataTable();
                Application.DoEvents();
                sda.Fill(dtStart);

                m_dtThru = DateTime.Parse(dtStart.Rows[0]["value"].ToString());
            }

            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            ((DateTimePicker)m_dpFrom.Control).Text = DateTime.Parse("12/21/2002").ToString("d");
            m_dtFrom = ((DateTimePicker)m_dpFrom.Control).Value;
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpFrom.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpFrom.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpFrom.Control).Name = "FROM";
            m_dpFrom.Control.Width = 95;
            m_dpFrom.Control.Refresh();
            m_dpFrom.Invalidate();
            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            ToolStripLabel tslFrom = new ToolStripLabel("From: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

            m_dpThru = new ToolStripControlHost(new DateTimePicker());
            ((DateTimePicker)m_dpThru.Control).Text = m_dtThru.ToShortDateString();
            //DateTime.Now.AddDays(-10).ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
            //m_dtThru = m_dtStartSSI;
            //((DateTimePicker)m_dpThru.Control).Value;
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpThru.Control).Name = "THRU";
            m_dpThru.Control.Width = 95;
            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);

            // tscb include
            m_lblInclude = new ToolStripLabel();
            m_lblInclude.Text = "Select Outpatient Billing";
            m_lblInclude.Visible = false;

            m_cboxInclude = new ToolStripControlHost(new CheckBox());
            ((CheckBox)m_cboxInclude.Control).Click += new EventHandler(frmAcc_Click_Outpatient_Billing);
            tsMain.Items.Insert(tsMain.Items.Count, m_lblInclude);
            tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();
        }

        void frmAcc_CloseUp(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            Requery();
        }

        void frmAcc_ValueChanged(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (((DateTimePicker)sender).Name == "FROM")
            {
                m_dtFrom = ((DateTimePicker)sender).Value;
            }
            else
            {
                m_dtThru = ((DateTimePicker)sender).Value;
            }

        }

        void frmAcc_Click_Outpatient_Billing(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            Requery();
            tsslAccounts.Text = string.Format("ACCOUNTS: {0}", dgvAccount.Rows.Count);
            this.Refresh();
            //return;
            tsslNote.Text = "";

        }

        private void dgvAccount_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //instead of LaunchAccount - we can call the account form directly
            Log.Instance.Debug($"Entering");


            string strErrors = ((DataGridView)sender).Rows[e.RowIndex].ErrorText;
            string strAccount = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString().Trim();
            string strPatName = ((DataGridView)sender).Rows[e.RowIndex].Cells["pat_name"].Value.ToString().Trim();
            try
            {
                AccountForm frm = new AccountForm(strAccount)
                {
                    MdiParent = this.ParentForm
                };
                frm.Show();


                if (!string.IsNullOrEmpty(strErrors))
                {
                    ErrorsForm f = new ErrorsForm();
                    f.Text = strAccount;

                    f.rtbErrors.Text = string.Format("Errors for account {0}.\r\nPatient [{1}]\r\n\n", strAccount, strPatName);
                    f.rtbErrors.Text += strErrors;
                    this.AddOwnedForm(f);
                    f.Show();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Exception occured trying to open the account. \r\n {0}", ex.Message));
            }

        }

        private void dgvAccount_SelectionChanged(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (dgvAccount.CurrentRow == null)
            {
                return;
            }
            if (!dgvAccount.Focused)
            {
                return;
            }

            string strAcc = ((DataGridView)sender)["account", dgvAccount.CurrentRow.Index].Value.ToString();
            DataRow[] drArray = m_dsAccount.Tables["ACC"].Select(
                string.Format("account = '{0}'", strAcc));

            //////
            m_dsAccount.Tables["PAT"].Rows.Clear();
            dgvPat.Update();
            m_alDiagnosis.Clear();

            m_dsAccount.Tables["INS"].Rows.Clear();
            dgvInsurance.Update();

            m_dsAccount.Tables["BILLING"].Rows.Clear();
            dgvBilling.Update();

            m_dsAccount.Tables["CHRG_ERR"].Rows.Clear();
            dgvChrgErr.Update();

        }

        /// <summary>
        /// if the account datagridview has been checked add it to the ssi billing file.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPost_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            dgvAccount.EndEdit();
            if (string.IsNullOrEmpty(m_strRequery))
            {
                if (tscbFinCodes.SelectedIndex == -1)
                {
                    m_strRequery = string.Format("trans_date >= '{0}' and trans_date <= '{1}' "
                        , m_dtFrom, m_dtThru);
                    // don't forget to change in Load_frmAcc
                    if (DateTime.Today <= new DateTime(2012, 04, 02))
                    {
                        m_strRequery = string.Format("1 = 1");
                    }
                }
                else
                {
                    m_strRequery = string.Format("trans_date >= '{0}' and trans_date <= '{1}' " +
                        " and [fin_code] = '{2}'", m_dtFrom, m_dtThru, tscbFinCodes.SelectedItem.ToString());
                }
            }

            m_dsAccount.AcceptChanges();
            string strPostQuery = string.Format("status = 'UB' and {0}", m_strRequery);
            DataRow[] drarrUB =
                m_dsAccount.Tables["ACC"].Select(strPostQuery);//, "pat_name", DataViewRowState.ModifiedCurrent);
            foreach (DataRow dr in drarrUB)
            {
                dr.SetModified();
            }

            strPostQuery = string.Format("status = 'UBOP' and {0}", m_strRequery);
            DataRow[] drarrUBOP =
                m_dsAccount.Tables["ACC"].Select(strPostQuery); //, "pat_name", DataViewRowState.ModifiedCurrent);
            foreach (DataRow dr in drarrUBOP)
            {
                dr.SetModified();
            }
            strPostQuery = string.Format("status = '1500' and {0}", m_strRequery);
            DataRow[] drarr1500 =
                m_dsAccount.Tables["ACC"].Select(strPostQuery);//, "pat_name", DataViewRowState.ModifiedCurrent);

            foreach (DataRow dr in drarr1500)
            {
                dr.SetModified();
            }
            // wdk 20121018 added for Quest\Bluecare handling
            strPostQuery = string.Format("status = 'QUEST' and {0}", m_strRequery);
            DataRow[] drarrQuest =
                m_dsAccount.Tables["ACC"].Select(strPostQuery);//, "pat_name", DataViewRowState.ModifiedCurrent);

            foreach (DataRow dr in drarrQuest)
            {
                dr.SetModified();
            }
            // end of 20121018
            int nRecText = drarr1500.Count();// drarr1500.GetUpperBound(0) + drarr1500.GetUpperBound(0) == -1 ? 0 : 1;
            nRecText += drarrUB.Count();// drarrUB.GetUpperBound(0) + drarrUB.GetUpperBound(0) == -1 ? 0 : 1;
            nRecText += drarrUBOP.Count();// drarrUBOP.GetUpperBound(0) + drarrUBOP.GetUpperBound(0) == -1 ? 0 : 1;
            nRecText += drarrQuest.Count(); // wdk 20121018 added

            tsslAccounts.Text = string.Format("Records posted {0}", nRecText);

            int nRec = 0;


            if (drarr1500.Count() > 0)
            {
                try
                {
                    nRec = m_daAcc.Update(drarr1500);
                }
                catch (SqlException se)
                {
                    if (se.Message.Contains("timeout"))
                    {
                        try
                        {
                            m_daAcc.Update(drarr1500);
                        }
                        catch (SqlException se1)
                        {
                            m_Err.m_Logfile.WriteLogFile(se1.Message);
                            Log.Instance.Error(se1.Message);
                        }
                    }
                }
            }

            if (drarrUB.Count() > 0)
            {
                try
                {
                    nRec += m_daAcc.Update(drarrUB);
                }
                catch (SqlException se)
                {
                    if (se.Message.Contains("timeout"))
                    {
                        try
                        {
                            m_daAcc.Update(drarrUB);
                        }
                        catch (SqlException se1)
                        {
                            m_Err.m_Logfile.WriteLogFile(se1.Message);
                            Log.Instance.Debug(se1.Message);
                        }
                    }
                }

            }

            if (drarrUBOP.Count() > 0)
            {
                try
                {
                    nRec += m_daAcc.Update(drarrUBOP);
                }
                catch (SqlException se)
                {
                    if (se.Message.Contains("timeout"))
                    {
                        try
                        {
                            m_daAcc.Update(drarrUBOP);
                        }
                        catch (SqlException se1)
                        {
                            m_Err.m_Logfile.WriteLogFile(se1.Message);
                            Log.Instance.Error(se1.Message);
                        }
                    }
                }
            }
            // wdk 20121018 added
            if (drarrQuest.Count() > 0)
            {
                try
                {
                    nRec += m_daAcc.Update(drarrQuest);
                }
                catch (SqlException se)
                {
                    if (se.Message.Contains("timeout"))
                    {
                        try
                        {
                            m_daAcc.Update(drarrQuest);
                        }
                        catch (SqlException se1)
                        {
                            m_Err.m_Logfile.WriteLogFile(se1.Message);
                            Log.Instance.Error(se1.Message);
                        }
                    }
                }
                // end of 20121018


            }

            if (MessageBox.Show(string.Format("[{0}] Record{1} posted. Select YES to RELOAD the Grid.", nRec, nRec > 1 ? "s" : ""), "Post complete", MessageBoxButtons.YesNo)
                == DialogResult.Yes)
            {
                Query();// tscbFinCodes_SelectedIndexChanged(null, null);
            }
        }

        private void Query()
        {
            Log.Instance.Debug($"Entering");
            m_dsAccount.Tables["ACC"].Clear();
            using (SqlConnection connection = new SqlConnection(m_sqlConn.ConnectionString))
            {
                if (DateTime.Now <= new DateTime(2012, 12, 3, 16, 30, 00))
                {
                    m_dtThru = new DateTime(2012, 11, 1);
                }
                if (Control.IsKeyLocked(Keys.Scroll) && Environment.UserName.ToUpper() == "WKELLY")
                {
                    m_dtThru = DateTime.Today;
                }

                SqlCommand cmdSelectAcc = new SqlCommand(
                    string.Format("exec usp_prg_ViewerAcc_Select @startDate = '{0}', @thruDate = '{1}', @outpatientDate = '{2}'",
                    m_dtFrom, m_dtThru, m_dtOutpatient), m_sqlConn);
                m_daAcc.SelectCommand = cmdSelectAcc;

                int nRows = m_daAcc.Fill(m_dsAccount.Tables["ACC"]);
                if (!m_dsAccount.Tables["ACC"].Columns.Contains("ERRORS"))
                {
                    m_dsAccount.Tables["ACC"].Columns.Add("ERRORS");
                }

                dgvAccount.DataSource = m_dsAccount.Tables["ACC"];
            }

            //foreach(DataGridViewRow dr in dgvAccount.Rows)//m_dsAccount.Tables["ACC"].Rows)
            //{
            //    Application.DoEvents();
            //    if (DateTime.Parse(dr.Cells["trans_date"].Value.ToString()) >= new DateTime(2012, 10, 1) &&
            //        dr.Cells["fin_code"].Value.ToString() == "D")
            //    {

            //        ((DataRowView)dr.DataBoundItem).Row["bill_form"] = "QUEST";
            //    }
            //}

            tsslAccounts.Text = string.Format("ACCOUNTS: {0}", m_dsAccount.Tables["ACC"].Rows.Count);
        }

        private void dgvAccount_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Log.Instance.Debug($"Entering");
            tsslAccounts.Text = string.Format("ACCOUNTS: {0}", dgvAccount.Rows.Count);
        }

        private bool ValidateData(string strFinCode, ref StringBuilder sb)
        {
            Log.Instance.Debug($"Entering");
            bool bRetVal = true;
            m_alDiagnosis.Clear();
            m_alTests.Clear();
            m_alTestsModi.Clear();

            if (!m_alFinCodeProcessing.Contains(strFinCode))
            {
                if (dgvPat.Rows.Count != 1)
                {
                    sb.AppendFormat("{0}\r\n", dgvPat.Rows.Count == 0 ? "DO NOT BILL -- No Patient Record" : "DO NOT BILL -- Too many patient records.");
                    bRetVal = false;
                }
                else // validate pat data
                {
                    // pat_addr1
                    if (string.IsNullOrEmpty(dgvPat.Rows[0].Cells["pat_addr1"].Value.ToString().Trim()))
                    {
                        sb.AppendFormat("Patient's pat_addr1 is blank\r\n");
                        bRetVal = false;
                    }
                    if (dgvPat.Rows[0].Cells["pat_addr1"].Value.ToString().Trim() == ",")
                    {
                        sb.AppendFormat("Patient's pat_addr1 is equal ',' and not valid");
                        bRetVal = false;
                    }
                    // pat city_st_zip
                    if (string.IsNullOrEmpty(dgvPat.Rows[0].Cells["city_st_zip"].Value.ToString().Trim()))
                    {
                        sb.AppendFormat("Patient's city_st_zip is blank\r\n");
                        bRetVal = false;
                    }
                    else
                    {
                        if (dgvPat.Rows[0].Cells["city_st_zip"].Value.ToString().Trim() == ",")
                        {
                            sb.AppendFormat("Patient's city_st_zip is equal ',' and not valid\r\n");
                            bRetVal = false;
                        }
                        else
                        {
                            string strCSZ = dgvPat.Rows[0].Cells["city_st_zip"].Value.ToString().Trim();
                            if (strCSZ.Contains(','))
                            {
                                string strCity = strCSZ.Split(new string[] { ", " }, StringSplitOptions.None)[0];
                                string[] strST = strCSZ.Split(new string[] { "," }, StringSplitOptions.None)[1].ToString().Trim().Split(new string[] { " " }, StringSplitOptions.None);
                                string[] strNoFail = new string[3] { "", "", "" };
                                strST.CopyTo(strNoFail, 0);
                                if (string.IsNullOrEmpty(strNoFail[0]) || strNoFail[0].Length != 2)
                                {
                                    sb.AppendFormat("Patient's city_st_zip does not contain a valid state.\r\n");
                                    bRetVal = false;
                                }
                                if (string.IsNullOrEmpty(strNoFail[1]) || (strNoFail[1].Length < 5 || strNoFail[1].Length > 10))
                                {
                                    sb.AppendFormat("Patient's city_st_zip does not contain a valid zip code\r\n");
                                    bRetVal = false;
                                }
                            }
                            else
                            {
                                sb.AppendFormat("Patient's city_st_zip does not contain a ',' so cannot be valid.\r\n");
                                bRetVal = false;
                            }
                        }
                    }
                    // pat dob
                    DateTime dtPatDob = DateTime.MaxValue;
                    if (!DateTime.TryParse(dgvPat.Rows[0].Cells["dob_yyyy"].Value.ToString().Trim(), out dtPatDob))
                    {
                        sb.AppendFormat("[{0}] is not a vaild date of birth\r\n", dgvPat.Rows[0].Cells["dob_yyyy"].Value.ToString().Trim());
                        bRetVal = false;
                    }
                    // pat sex
                    string strPatSex = dgvPat.Rows[0].Cells["sex"].Value.ToString().Trim().ToUpper();
                    if (strPatSex != "M" && strPatSex != "F" && strPatSex != "U")
                    {
                        sb.AppendFormat("[{0}] is not valid for the patients gender.\r\n", strPatSex);
                        bRetVal = false;
                    }

                    // ICD's 
                    LoadDiagnosisArray(sb);


                    string strRelation = dgvPat.Rows[0].Cells["relation"].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(strRelation))
                    {
                        sb.AppendFormat("No relation is assigned to this patient.\r\n");
                        bRetVal = false;
                    }
                    string strGuarantorName = dgvPat.Rows[0].Cells["guarantor"].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(strGuarantorName))
                    {
                        sb.AppendFormat("Guarantor Name is blank for this patient.\r\n");
                        bRetVal = false;
                    }
                    string strGuarantorAddr = dgvPat.Rows[0].Cells["guar_addr"].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(strGuarantorAddr))
                    {
                        sb.AppendFormat("Guarantor Address is blank for this patient.\r\n");
                        bRetVal = false;
                    }
                    string strGuarantorCityStZip = dgvPat.Rows[0].Cells["g_city_st"].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(strGuarantorCityStZip))
                    {
                        sb.AppendFormat("Guarantor's City, State and Zip fields are blank for this patient.\r\n");
                        bRetVal = false;
                    }
                    else
                    {
                        if (strGuarantorCityStZip == ",")
                        {
                            sb.AppendFormat("Guarantor's city_st_zip is equal ',' and not valid\r\n");
                            bRetVal = false;
                        }
                        else
                        {
                            string strCSZ = strGuarantorCityStZip;
                            if (strCSZ.Contains(','))
                            {
                                string strCity = strCSZ.Split(new string[] { ", " }, StringSplitOptions.None)[0];
                                string[] strST = strCSZ.Split(new string[] { "," }, StringSplitOptions.None)[1].ToString().Trim().Split(new string[] { " " }, StringSplitOptions.None);
                                string[] strNoFail = new string[5] { "", "", "", "", "" }; // WDK 20140106 ADDED 2 more places zip code failed on "99999-  91"
                                strST.CopyTo(strNoFail, 0);
                                if (string.IsNullOrEmpty(strNoFail[0]) || strNoFail[0].Length != 2)
                                {
                                    sb.AppendFormat("Guarantors's city_st_zip does not contain a valid state.\r\n");
                                    bRetVal = false;
                                }
                                if (string.IsNullOrEmpty(strNoFail[1]) || (strNoFail[1].Length < 5 || strNoFail[1].Length > 10)
                                    || !string.IsNullOrEmpty(strNoFail[2]))
                                {
                                    sb.AppendFormat("Guarantor's city_st_zip does not contain a valid zip code\r\n");
                                    bRetVal = false;
                                }

                            }
                            else
                            {
                                sb.AppendFormat("Guarantor's city_st_zip does not contain a ',' so cannot be valid.\r\n");
                                bRetVal = false;
                            }
                        }
                    }

                    string strPhyId = dgvPat.Rows[0].Cells["phy_id"].Value.ToString().Trim();
                    string strPhyLName = dgvPat.Rows[0].Cells["phy_last_name"].Value.ToString().Trim();
                    string strPhyFName = dgvPat.Rows[0].Cells["phy_first_name"].Value.ToString().Trim();
                    if (string.IsNullOrEmpty(strPhyId))
                    {
                        sb.AppendFormat("Physicians NPI is blank for this patient.\r\n");
                        bRetVal = false;
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(strPhyLName))
                        {
                            sb.AppendFormat("Physicians last name is blank.\r\n");
                            bRetVal = false;
                        }

                        if (string.IsNullOrEmpty(strPhyFName))
                        {
                            sb.AppendFormat("Physicians first name is blank.\r\n");
                            bRetVal = false;
                        }
                    }

                }  // end of pat data verification

                // E fincodes don't need an insurance record but the other tables must be correct
                if (strFinCode == "E")
                {
                    // special handling
                }
                else
                {
                    int nInsCount = dgvInsurance.Rows.Count;
                    if (nInsCount < 1)
                    {
                        sb.AppendFormat("{0}\r\n", "No Insurance Record");
                        bRetVal = false;
                    }
                    else
                    {
                        for (int i = 0; i < nInsCount; i++)
                        {
                            string strHolder = dgvInsurance.Rows[i].Cells["holder_nme"].Value.ToString().Trim();
                            if (string.IsNullOrEmpty(strHolder))
                            {
                                sb.AppendFormat("Insurance {0} holder name is missing.\r\n",
                                    dgvInsurance.Rows[i].Cells["ins_a_b_c"].Value.ToString().Trim());
                                bRetVal = false;
                            }
                            string strPlanName = dgvInsurance.Rows[i].Cells["plan_nme"].Value.ToString().Trim();
                            if (string.IsNullOrEmpty(strPlanName))
                            {
                                sb.AppendFormat("Insurance {0} plan name is missing.\r\n",
                                    dgvInsurance.Rows[i].Cells["ins_a_b_c"].Value.ToString().Trim());
                                bRetVal = false;
                            }
                            string strPolicyNum = dgvInsurance.Rows[i].Cells["policy_num"].Value.ToString().Trim();
                            if (string.IsNullOrEmpty(strPolicyNum))
                            {
                                string strGrpNum = dgvInsurance.Rows[i].Cells["grp_num"].Value.ToString().Trim();
                                if (string.IsNullOrEmpty(strGrpNum))
                                {
                                    sb.AppendFormat("Insurance {0} policy and group number is missing.\r\n",
                                        dgvInsurance.Rows[i].Cells["ins_a_b_c"].Value.ToString().Trim());
                                    bRetVal = false;
                                }
                            }

                        } // end of each insurance record

                    } // end of insurance verification
                } // end of fincode E processing 
                int nBillingRows = dgvBilling.Rows.Count;
                if (nBillingRows < 1)
                {
                    sb.AppendFormat("DO NOT BILL -- No charge records for which to bill.\r\n");
                    bRetVal = false;
                }
                else
                {
                    for (int i = 0; i < nBillingRows; i++)
                    {
                        int nQty = int.Parse(dgvBilling["qty", i].Value.ToString().Trim());
                        if (nQty <= 0)
                        {
                            sb.AppendFormat("Charge record has an invalid quantity for billing.\r\n");
                            bRetVal = false;
                        }
                        string strCpt4 = dgvBilling["cpt4", i].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(strCpt4))
                        {
                            sb.AppendFormat("Charge record has an invalid CPT4 for billing.\r\n");
                            bRetVal = false;
                        }
                        string strType = dgvBilling["type", i].Value.ToString().Trim();
                        if (string.IsNullOrEmpty(strType))
                        {
                            sb.AppendFormat("Charge record has an invalid type for billing.\r\n");
                            bRetVal = false;
                        }
                        else
                        {
                            if (strType != "NORM" && strType != "N/A" && strType != "TC")
                            {
                                sb.AppendFormat("Charge record type {0} is invalid for billing.\r\n",
                                    strType);
                                bRetVal = false;
                            }
                        }
                        float f = float.Parse(dgvBilling["amount", i].Value.ToString());
                        if (f <= 0.00)
                        {
                            sb.AppendFormat("Charge record amount {0} is invalid for billing.\r\n",
                                f.ToString());
                            bRetVal = false;
                        }

                    } // end of each billing record

                } // end of billing checks

                if (dgvChrgErr.Rows.Count > 0)
                {
                    sb.AppendFormat("{0}\r\n", "Resolve charge errors before billing.");
                    bRetVal = false;
                }
                // TOD0: cannot bill account trans_date chrg service_Date mismatches.

            }
            ValidateFinCode(strFinCode, ref sb);

            return bRetVal;
        }

        //private void tsmiSearchInfo_Click(object sender, EventArgs e)
        //{
        //    //if (nFilterColumn == -1)
        //    //{
        //    //    MessageBox.Show("You must first select a column before searching!");
        //    //    return;
        //    //}
        //    //string strColName = dgvAccount.Columns[nFilterColumn].Name;
        //    //string strText =            tstbSearchInfo.Text;
        //    //BindingSource bs = new BindingSource(dgvAccount.DataSource, "status");
        //    //bs.DataMember = dgvAccount.DataMember;
        //    //bs.Filter = string.Format("{0} like '{1}%'", strColName,strText); // ((CheckBox)m_cboxInclude.Control).Checked ? "COLL" : "NEW"
        //    //dgvAccount.DataSource = bs;

        //    //tsslAccounts.Text = string.Format(" {0} Account{1}", dgvAccount.Rows.Count, dgvAccount.Rows.Count > 1 ? "s" : "");

        //    //tsmiSearchInfo.Checked = false;
        //    //cmsTreeView.Close();

        //}

        /// <summary>
        /// used to sort the account table only!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvAccount_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Debug($"Entering");
            string strColText = dgvAccount.Columns[e.ColumnIndex].HeaderText;
            if (strColText.Contains("ERROR"))
            {
                return;
            }
            nFilterColumn = e.ColumnIndex;
            System.Windows.Forms.SortOrder SO = dgvAccount.SortOrder;
            BindingSource bbs = new BindingSource(m_dsAccount.Tables["ACC"], "ACCOUNT");

            BindingSource bs = new BindingSource(dgvAccount.DataSource,
                dgvAccount.Columns[e.ColumnIndex].Name);
            bs.DataMember = dgvAccount.DataMember;
            string strFilter = bs.Filter;

            if (e.Button == MouseButtons.Right)
            {
                if (dgvAccount.Columns[e.ColumnIndex].Name.ToUpper() == "BILLING TYPE")
                {
                    MessageBox.Show("Cannot filter on this column.");
                    return;
                }
                string strColName = dgvAccount.Columns[e.ColumnIndex].Name;
                FormResponse f = new FormResponse();
                foreach (DataGridViewRow dr in dgvAccount.Rows)
                {
                    string strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (strColText == "pat_name")
                    {
                        strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString().Split(new char[] { ',' })[0][0].ToString();
                    }

                    if (!f.clbFilter.Items.Contains(strText))
                    {
                        f.clbFilter.Items.Add(strText);
                    }
                }
                string strResponse = null;
                string strFilterHelper = null;
                if (f.ShowDialog() == DialogResult.Yes)
                {
                    foreach (string str in f.clbFilter.CheckedItems)
                    {
                        strResponse += string.Format("'{0}',", str);
                        strFilterHelper += string.Format(" pat_name like '{0}%' or ", str);
                    }
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        //   int nli = strResponse.LastIndexOf(',');
                        //   int nlen = strResponse.Length;
                        strResponse = strResponse.Remove(strResponse.LastIndexOf(','));
                        strFilterHelper = strFilterHelper.Remove(strFilterHelper.LastIndexOf("or"));
                    }

                }

                //BindingSource 
                bs = new BindingSource(dgvAccount.DataSource,
                    strColText);
                bs.DataMember = dgvAccount.DataMember;
                if (string.IsNullOrEmpty(strResponse))
                {
                    m_strRequeryColFilter = "";
                    Requery();
                }
                else
                {
                    m_strRequeryColFilter = string.Format(" and [{0}] in ({1})", strColText, strResponse);
                    if (strColName == "pat_name")
                    {

                        m_strRequeryColFilter = string.Format(" and ({0}) ", strFilterHelper);
                    }

                    Requery();
                }
                //return;

                //if (string.IsNullOrEmpty(strResponse))
                //{
                //    // bs.RemoveFilter();
                //    bs.Filter = string.Format("FIN_CODE = '{0}' and [billing type] = '{1}'"
                //        , tscbFinCodes.SelectedItem.ToString(), ((CheckBox)m_cboxInclude.Control).Checked ? "OUTPATIENT" : "REF LAB");

                //}
                //else
                //{

                //    bs.Filter = string.Format("[{0}] in ({1}) AND FIN_CODE = '{2}' and [billing type] = '{3}' ",
                //        strColText, strResponse, tscbFinCodes.SelectedItem.ToString(), ((CheckBox)m_cboxInclude.Control).Checked ? "OUTPATIENT" : "REF LAB");
                //}
                //bs.Sort = string.Format("{0} {1},pat_name ASC", dgvAccount.Columns[e.ColumnIndex].Name,
                //    dgvAccount.SortOrder == System.Windows.Forms.SortOrder.Ascending? "ASC" : "DESC");
                //dgvAccount.DataSource = bs;
            }
            else
            {
                string strSort = string.Format("{0} {1}, pat_name ASC", dgvAccount.Columns[e.ColumnIndex].Name
                    , dgvAccount.SortOrder == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC"
                    );
                bs.Sort = strSort;
                dgvAccount.DataSource = bs;
            }

        }

        private void dgvChrgErr_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //chrg_err.exe /%thisdb
        }

        private void tscbFinCodes_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            Requery();
            //DateTime tdate = ((DateTimePicker)m_dpThru.Control).Value;
            tsslAccounts.Text = string.Format("ACCOUNTS: {0}", dgvAccount.Rows.Count);
            Control c = this.GetNextControl(tscbFinCodes.Control, true);
            c.Focus();
        }

        private void Requery()
        {
            Log.Instance.Debug($"Entering");
            this.Cursor = new Cursor(Cursor.Current.Handle);
            Cursor.Position = new Point(Cursor.Position.X + 50, Cursor.Position.Y + 50);
            Cursor.Current = Cursors.WaitCursor;
            if (m_dtOutpatient <= DateTime.Today)
            {
                m_strOutPatientBilltype = string.Format(" and [billing type] = {0}",
                    ((CheckBox)m_cboxInclude.Control).Checked ? "'OUTPATIENT'" : "'REF LAB'");
            }
            else
            {
                m_strOutPatientBilltype = "";
            }
            string strFinCode = "";
            if (tscbFinCodes.SelectedIndex != -1)
            {
                strFinCode = string.Format(" and fin_code = '{0}'", tscbFinCodes.SelectedItem.ToString());
            }
            if (Control.IsKeyLocked(Keys.Scroll) && Environment.UserName.ToUpper() == "WKELLY")
            {
                m_dtThru = DateTime.Today;
            }
            m_strRequery = string.Format("trans_date >= '{0:d}' and trans_date <= '{1:d}' {2} {3} {4}",
                m_dtFrom, m_dtThru, m_strOutPatientBilltype, strFinCode, m_strRequeryColFilter).Trim();
            if (DateTime.Today <= new DateTime(2012, 03, 2))
            {
                m_strRequery = "account in ('C3460988','C3450961','c3450450')";
            }
            tsslFilter.Text = m_strRequery;
            m_dsAccount.Tables["PAT"].Rows.Clear();
            m_dsAccount.Tables["INS"].Rows.Clear();
            m_dsAccount.Tables["BILLING"].Rows.Clear();
            m_dsAccount.Tables["CHRG_ERR"].Rows.Clear();

            BindingSource bs = new BindingSource();
            bs.DataSource = m_dsAccount.Tables["ACC"];

            bs.Filter = m_strRequery;

            if (dgvAccount.SortedColumn != null)
            {
                bs.Sort = string.Format("{0} {1},pat_name ASC", dgvAccount.SortedColumn.Name,
                dgvAccount.SortOrder == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC");
            }
            else
            {
                bs.Sort = string.Format("fin_code ASC, pat_name ASC");
            }
            dgvAccount.DataSource = bs;
            tsslAccounts.Text = string.Format("ACCOUNTS: {0}", dgvAccount.Rows.Count);
            //foreach (DataGridViewRow dr in dgvAccount.Rows)//m_dsAccount.Tables["ACC"].Rows)
            //{
            //    if (DateTime.Parse(dr.Cells["trans_date"].Value.ToString()) >= new DateTime(2012, 10, 1) &&
            //        dr.Cells["fin_code"].Value.ToString() == "D")
            //    {

            //        ((DataRowView)dr.DataBoundItem).Row["bill_form"] = "QUEST";
            //    }
            //}
        }

        private delegate string DelegateValidateData(DataRow drAcc);


        private void Validate_Accounts_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (dgvAccount.Rows.Count <= 0)
            {
                tsslNote.Text = "Nothing in grid.";
                Application.DoEvents();
                return;
            }
            tsslNote.Text = "Clearing Error Totals";
            Application.DoEvents();

            m_dicErrorTotals.Clear();
            //foreach (DataGridViewRow dr in dgvAccount.Rows)
            //{
            //    Application.DoEvents();
            //    dr.ErrorText = "";
            //    dr.DefaultCellStyle = dgvAccount.DefaultCellStyle;
            //}

            tsbValidate.Enabled = false;

            tscbFinCodes.Enabled = false;
            tspbCount.Value = 0;
            tspbCount.Step = 1;
            tspbCount.Maximum = dgvAccount.Rows.Count;

            DataGridViewCellStyle styleInfoLMRP = new DataGridViewCellStyle();
            styleInfoLMRP.BackColor = Color.PaleVioletRed;
            DataGridViewCellStyle styleInfoErr = new DataGridViewCellStyle();
            styleInfoErr.BackColor = Color.PaleGoldenrod;
            int nLmrp = 0;
            int nErr = 0;
            int nValid = 0;

            #region DataRow[]
            DataRow[] drArray = m_dsAccount.Tables["ACC"].Select(m_strRequery, "pat_name");

            foreach (DataGridViewRow dr in dgvAccount.Rows)
            {

                Application.DoEvents();
                // this is causing an issue - it causes unpredictability if the user closes the window. The process continues.
                // this process should be in a separate thread

                m_Err.m_Logfile.WriteLogFile(
                    string.Format("[Account {0} start.]\r\n", dr.Cells["account"].Value));
                Log.Instance.Error(string.Format("[Account {0} start.]\r\n", dr.Cells["account"].Value));
                m_alDiagnosis.Clear();
                m_alTests.Clear();
                m_alTestsModi.Clear();

                // m_drCur = (dr.DataBoundItem as DataRowView).Row; either this one or the next one work.
                m_drCur = ((DataRow)((DataRowView)dr.DataBoundItem).Row);

                DelegateValidateData dvd = new DelegateValidateData(ValidateData);
                IAsyncResult tag = null;
                string strResult = null;
                try
                {
                    tag = dvd.BeginInvoke(m_drCur, null, null);
                    strResult = dvd.EndInvoke(tag);
                }
                catch (InvalidOperationException ioe)
                {
                    strResult = ioe.Message;
                }

                m_drCur.RowError = strResult;

                dr.ErrorText = strResult;
                dr.Cells["ERRORS"].Value = strResult.Replace(Environment.NewLine, "\t");
                foreach (string strErr in strResult.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries))
                {
                    int nVal = 1;
                    // wdk 20121126 added client to code to allow tracking by clients.
                    //string newErr = strErr.Insert(0, string.Format("{0} | ", dr.Cells["cl_mnem"].Value.ToString()));
                    // wdk 20160129 add to track errors by type
                    string newErr = "";
                    if (strErr.Contains("LMRP"))
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "LMRP"));
                    }
                    else
                    if (strErr.ToUpper().Contains("FINCODE")
                        || strErr.ToUpper().Contains("FIN_CODE")
                        )
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "FINCODE"));
                    }
                    else
                    if (strErr.Contains("DO NOT BILL. This insurance requires diagnosis pointers (dictionary edit)")
                        || strErr.Contains("DO NOT BILL. Policy Number is not all Numeric. (dictionary edit)")
                        || strErr.Contains("DO NOT BILL. Insurance does not accept behavioral codes. (dictionary edit)")
                        || strErr.ToUpper().Contains("INSURANCE POLICY NUMBER")
                        || strErr.Contains("Insurance A policy and group number is missing.")
                        || strErr.Contains("BlueCross Policy number is null or blank.")
                        || strErr.Contains("Primary Insurance policy number is not within the range 9 - 11.")
                        || strErr.Contains("Primary Insurance policy number must be between 7 and 12 characters long.")
                        || strErr.Contains("DO NOT BILL. Medicare does not allow these tests. '83880','G0479','G0480'(dictionary edit)")
                        || strErr.Contains("is not a letter, digit or space in policy number.")
                        || strErr.Contains("Insurance does not accept obesity codes. (dictionary edit)")
                        )
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "INS"));
                    }
                    else
                    if (strErr.Contains("Zip directory has city as [")
                        || strErr.Contains("Pat relations listed as [01] not compatable with Ins relation of [02].")
                        || strErr.Contains("Zip code")
                        || strErr.Contains("Physicians NPI is blank for this patient.")
                        || strErr.Contains("DO NOT BILL. No physician on record. (dictionary edit)")
                        || strErr.Contains("has a non alpha character in the last name.")
                        )
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "DEMO"));
                    }
                    else
                    if (strErr.Contains("DO NOT BILL. Has no ICD codes. (dictionary edit)")
                        || strErr.Contains("Diagnosis contains routine codes. (dictionary edit)")
                        || strErr.Contains("Diagnosis contains dental codes. (dictionary edit)")
                        )
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "DIAG"));
                    }
                    else
                    if (strErr.Contains("Charges contain CPT 83516 with a qty of [")
                        || strErr.Contains("Charges contain CPT 83519 with a qty of [")
                        || strErr.Contains("Charges contain CPT 82607 with a qty of [")
                        || strErr.Contains("Charges contain CPT 36415 with a qty of [")
                        || strErr.Contains("Charges contain CPT 86611 with a qty of [")
                        || strErr.Contains("Charges contain CPT 86665 with a qty of [")
                        || strErr.Contains("[Acute Hepatitis Panel]")
                        || strErr.Contains("Account has multiple Venipunctures.")
                        )
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "CHRG"));
                    }
                    else
                    if (strErr.Contains("are mutually exclusive. And, have a CCI indicator of 0.")
                        || strErr.Contains("are invalid together. And, have a CCI indicator of 1."))
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "MUTEX"));
                    }
                    else
                    {
                        newErr = strErr.Insert(0, string.Format("{0} | ", "UNK"));
                    }


                    if (m_dicErrorTotals.TryGetValue(newErr, out nVal))
                    {
                        m_dicErrorTotals.Remove(newErr);
                        m_dicErrorTotals.Add(newErr, ++nVal);
                    }
                    else
                    {
                        m_dicErrorTotals.Add(newErr, 1);
                    }

                }
                if (dr.ErrorText.Contains("LMRP"))
                {

                    dr.DefaultCellStyle = styleInfoLMRP;
                    nLmrp++;
                }
                else if (!string.IsNullOrEmpty(dr.ErrorText))
                {
                    dr.DefaultCellStyle = styleInfoErr;
                    nErr++;
                }
                else
                {
                    nValid++;
                }

                dr.ErrorText = strResult;
                if (string.IsNullOrEmpty(dr.ErrorText))
                {
                    dr.Cells["status"].Value = dr.Cells["bill_form"].Value;


                }
                dgvAccount.EndEdit();
                tspbCount.PerformStep();

                if (!dr.Displayed)
                {
                    dgvAccount.FirstDisplayedCell = dr.Cells[0];
                }

                dgvAccount.Refresh();
                this.Refresh();
                m_Err.m_Logfile.WriteLogFile(
                    string.Format("[Account {0} end.]\r\n", dr.Cells["account"].Value));
                Log.Instance.Error(string.Format("[Account {0} end.]\r\n", dr.Cells["account"].Value));
            }
            #endregion DataRow[]


            tscbFinCodes.Enabled = true;
            dgvAccount.Invalidate();
            tsbPost.Enabled = true;

            toolStripButton1_Click(toolStripButton1, EventArgs.Empty);

            tsslAccounts.Text = string.Format("ACCOUNTS: {0}. ACC W/LMRP {1}. ACC W/Other Err {2}. Valid Acc {3}.",
                dgvAccount.Rows.Count, nLmrp, nErr, nValid);
            MessageBox.Show("Validation Complete", "ViewerAcc");
            tsbValidate.Enabled = true;
            tspbCount.Value = 0;
        }

        private void dgvAccount_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (e.Button == MouseButtons.Right)
            {
                if (dgvAccount.Rows[e.RowIndex].HeaderCell.ErrorText.Contains("DO NOT BILL") &&
                    dgvAccount.Rows[e.RowIndex].HeaderCell.ErrorText.Contains("LMRP") &&
                    dgvAccount.Rows[e.RowIndex].Cells["fin_code"].Value.ToString() == "A")
                {
                    MessageBox.Show("Can not over ride the status. Validation of LMRP failed.", "DO NOT BILL");
                    return;
                }
                if (dgvAccount.Rows[e.RowIndex].HeaderCell.ErrorText.Contains("DO NOT BILL") &&
                    dgvAccount.Rows[e.RowIndex].Cells["fin_code"].Value.ToString() != "A")
                {
                    if (!dgvAccount.Rows[e.RowIndex].HeaderCell.ErrorText.Contains("LMRP"))
                    {
                        MessageBox.Show("Can not over ride the status.", "DO NOT BILL");
                        return;
                    }

                }
                if (string.IsNullOrEmpty(dgvAccount.Rows[e.RowIndex].HeaderCell.ErrorText) &&
                     dgvAccount.Rows[e.RowIndex].Cells["status"].Value.ToString().ToUpper() == "NEW")
                {
                    MessageBox.Show("Can not override the status of a non validated account.", "CHANGE STAUS");
                    return;
                }
                if (dgvAccount.Rows[e.RowIndex].Cells["status"].Value.ToString().ToUpper() != "NEW")
                {
                    if (MessageBox.Show(
                        string.Format("Account has a status of {0}. Do you want to change it back to 'NEW'?",
                        dgvAccount.Rows[e.RowIndex].Cells["status"].Value.ToString()), "CHANGE STATUS", MessageBoxButtons.OKCancel)
                        == DialogResult.OK)
                    {
                        //dgvAccount.Rows[e.RowIndex].Cells["status"].Value = "NEW";
                        ((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row["status"] = "NEW";

                    }
                    return;
                }

                if (MessageBox.Show(
                    string.Format("Account {0} has the following errors:\r\n{1}\r\n\r\nDo you certify this for billing?",
                     dgvAccount.Rows[e.RowIndex].Cells["account"].Value.ToString(), dgvAccount.Rows[e.RowIndex].ErrorText),
                    "ERRORS ON ACCOUNT", MessageBoxButtons.OKCancel) == DialogResult.OK)
                {
                    string strAccount = dgvAccount.Rows[e.RowIndex].Cells["account"].Value.ToString();

                    ((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row["status"] =
                        dgvAccount.Rows[e.RowIndex].Cells["bill_form"].Value.ToString();
                    //m_dsAccount.Tables["INS"].Rows[0]["bill_form"].ToString();
                    // dgvAccount.Rows[e.RowIndex].Cells["status"].Value = m_dsAccount.Tables["INS"].Rows[0]["bill_form"].ToString();
                    string strFilter = string.Format("account = '{0}'", strAccount);
                    int nRec = m_rNotes.GetRecords(strFilter);
                    m_rNotes.propAccount = strAccount;
                    m_rNotes.propComment =
                        string.Format("Billing authorized by {0} with [{1}] errors.", Environment.UserName,
                        dgvAccount.Rows[e.RowIndex].ErrorText);
                    nRec = m_rNotes.AddRecord(strFilter);
                    if (nRec == -1)
                    {
                        m_Err.m_Logfile.WriteLogFile(m_rNotes.propErrMsg);
                        Log.Instance.Error(m_rNotes.propErrMsg);
                    }
                    ((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row.RowError = "";

                }

            }
        }

        private void duplicateAccountsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            Form f = new Form();
            f.Text = string.Format("Duplicate Accounts - Production Environment {0}", m_strProductionEnvironment);
            f.WindowState = FormWindowState.Maximized;

            DataGridView dgv = new DataGridView();
            dgv.Dock = DockStyle.Fill;
            /////////
            using (SqlConnection conn = new SqlConnection(m_sqlConn.ConnectionString))
            {
                //     select
            }

            /////////
            f.Controls.Add(dgv);

            f.Show();
        }

        private void dgvAccount_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Log.Instance.Debug($"Entering");
            tsslAccounts.Text = string.Format("ACCOUNTS: {0}", dgvAccount.Rows.Count);
        }

        private void tsbPrintGrid_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (MessageBox.Show(string.Format("Grid has {0} records! Continue?", dgvAccount.Rows.Count),
                "PRINT GRID", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            // wdk 20121127 added Hide message.
            //if (MessageBox.Show("Hide the 'ERROR' column?", "PRINT GRID", MessageBoxButtons.YesNo)
            //    == DialogResult.Yes)
            //{
            dgvAccount.Columns["ERRORS"].Visible = false;
            // }
            m_ViewerPrintDocument.Print();
            dgvAccount.Columns["ERRORS"].Visible = true;

        }

        private void frmAcc_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (e.KeyData == Keys.Escape)
            {
                int x = 9;
                if (x == 9)
                {
                    x = 8;
                }
            }
        }

        private void frmAcc_FormClosing(object sender, FormClosingEventArgs e)
        {
            m_Err.m_Logfile.WriteLogFile(e.CloseReason.ToString());
            Log.Instance.Info(e.CloseReason.ToString());
        }

        private void tsbViewErrList_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");

            if (m_dicErrorTotals.Count() <= 0)
            {
                MessageBox.Show("There are no current errors to display. Run Validate Accounts", "EMPTY ERROR LIST");
                return;
            }
            Form f = new Form();
            f.Text = "ERROR LIST";
            WebBrowser wb = new WebBrowser();

            StringBuilder sb = new StringBuilder();
            sb.Append("<HTML>");
            sb.Append("<BODY>");
            sb.Append("<TABLE BORDER>");
            sb.Append("<TR><TH>ERROR</TH><TH>QTY</TH></TR>");

            foreach (KeyValuePair<string, int> kvp in m_dicErrorTotals.OrderByDescending(key => key.Value))
            {
                if (string.IsNullOrEmpty(kvp.Key))
                {
                    continue;
                }
                sb.Append("<TR>");
                sb.AppendFormat("<TD>{0}</TD>", kvp.Key.ToString().Split(new string[] { "|" }, StringSplitOptions.RemoveEmptyEntries)[0]);
                sb.AppendFormat("<TD>{0}</TD>", kvp.Key);
                sb.AppendFormat("<TD>{0}</TD>", kvp.Value.ToString());
                sb.Append("</TR>");
            }
            sb.AppendFormat("<CAPTION ALIGN=BOTTOM>Errors for : {0}</CAPTION>", m_strRequery);
            sb.Append("</TABLE>");
            sb.Append("</BODY>");
            sb.Append("</HTML>");
            wb.Dock = DockStyle.Fill;
            wb.DocumentText = sb.ToString();
            f.Controls.Add(wb);
            f.Show();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Log.Instance.Debug($"Entering");
            foreach (DataGridViewRow dr in dgvAccount.Rows)
            {

                Application.DoEvents();
                if (string.IsNullOrEmpty(dr.Cells["errors"].Value.ToString()))
                {
                    continue;
                }
                using (SqlConnection conn = new SqlConnection(m_sqlConn.ConnectionString))
                {
                    SqlCommand sqlInsert = new SqlCommand(
                        string.Format("IF (NOT EXISTS(SELECT account FROM dbo.ACC_LMRP " +
                        "WHERE dbo.ACC_LMRP.account = '{0}')) " +
                        "BEGIN " +
                        "INSERT INTO dbo.ACC_LMRP (account ,dos ,	fin_code ," +
                        "	cl_mnem ,	erorr	)" +
                        " values " +
                        " ('{0}','{1}','{2}','{3}','{4}' ) " +
                        "END"
                        , dr.Cells["account"].Value.ToString()
                        , dr.Cells["trans_date"].Value.ToString()
                        , dr.Cells["fin_code"].Value.ToString()
                        , dr.Cells["cl_mnem"].Value.ToString()
                        , dr.Cells["errors"].Value.ToString().Replace("'", "''").Replace("DO NOT BILL. ", "").Replace(" ICDs[].", "")
                        )
                        , conn);

                    try
                    {
                        conn.Open();
                        int nRetVal = sqlInsert.ExecuteNonQuery();
                    }
                    catch (Exception ec)
                    {
                        string str = ec.Message;
                    }
                    finally
                    {
                        conn.Close();
                    }

                }
            }

        }

        private void dgvAccount_KeyUp(object sender, KeyEventArgs e)
        {
            Log.Instance.Debug($"Entering");
            if (e.KeyData == Keys.Escape)
            {
                Environment.Exit(0);
            }
        }

        //private void dgvAccount_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        //{
        //    if (e.ColumnIndex == 0)
        //    {
        //        // dr is a datagridviewrow
        //        string strText = ((DataRow)((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row)["status"].ToString();
        //             //((DataRow)((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row).AcceptChanges();
        //             //((DataRow)((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row).SetModified();// = DataRowState.Modified;
        //        strText = ((DataRow)((DataRowView)dgvAccount.Rows[e.RowIndex].DataBoundItem).Row)["status"].ToString().ToUpper();


        //    }
        //    else
        //    {

        //    }
        //}

        #region multicolumn sort

        //#region comment
        //        public int Compare(object x, object y)
        //        {
        //            DataGridViewRow lhs = x as DataGridViewRow;
        //            DataGridViewRow rhs = y as DataGridViewRow;

        //            return Compare(lhs.Cells, rhs.Cells);
        //        }

        //        public int Compare(DataGridViewCellCollection lhs, DataGridViewCellCollection rhs)
        //        {
        //            foreach (SortColDefn colDefn in _sortedColumns)
        //            {
        //                int retval = Comparer.Default.Compare(
        //                lhs[colDefn.colNum].Value,
        //                rhs[colDefn.colNum].Value);

        //                if (retval != 0)
        //                    return (colDefn.ascending ? retval : -retval);
        //            }

        //            // These two rows are indistinguishable.
        //            return 0;
        //        }
        //        List<SortColDefn> _sortedColumns;
        //        private struct SortColDefn
        //        {
        //            internal Int16 colNum;
        //            internal bool ascending;

        //            internal SortColDefn(int columnNum, System.Windows.Forms.SortOrder sortOrder)
        //            {
        //                colNum = Convert.ToInt16(columnNum);
        //                ascending = (sortOrder != System.Windows.Forms.SortOrder.Descending);
        //            }
        //        }
        //        int sortPriority = _sortedColumns.FindIndex(
        //        delegate(SortColDefn cd) { return cd.colNum == columnIndex; });
        //        // project download in c:\download\datagridviewsort
        //#endregion comment

        #endregion multicolumnsort

        private void tsMain_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            Log.Instance.Debug($"Entering");
        }
    } // don't go below this line this is were other classes live in this solution
}
