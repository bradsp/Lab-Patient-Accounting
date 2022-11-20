using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
// Programmer added
using System.Drawing.Printing;
using System.IO;
using System.Data.SqlClient;
using RFClassLibrary;
using MCL;
using System.Collections;  // billing
// Move these two lines to the header space
using System.Reflection;
using System.Diagnostics;
using LabBilling.Core.Models;
using LabBilling.Core.BusinessLogic;
using LabBilling.Forms;

namespace LabBilling.Legacy
{
    public partial class ClientBillForm : Form
    {
        Dictionary<string, string> m_dicWCExclusions = null;
        string m_strRequery = "";
        string m_strRequeryColFilter = "";
        DataTable m_dtClient = null;
        SqlConnection sqlCon = null;
        Dictionary<string, string> m_dicClients = null;
        //ToolStripComboBox m_cbClient;
        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        ERR m_Err = null;
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        ToolStripControlHost m_cboxInclude; // CheckBox
        DateTime m_dtFrom;
        DateTime m_dtThru;
        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;


        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;

            //  create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dpFrom.Text = DateTime.Now.AddDays(-20).ToString("d");
            m_dtFrom = DateTime.Now.AddDays(-20);
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
            m_dpThru.Text = DateTime.Now.ToString();//because of nursing homes ability to register and order in advance this is set to 5 days in advance.
            m_dtThru = DateTime.Now;
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
            ((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpThru.Control).Name = "THRU";
            m_dpThru.Control.Width = 95;
            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("Thru Date: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
            //   tsMain.BackColor = Color.Lavender;

            // check box
            ToolStripLabel tslInclude = new ToolStripLabel("Include in Filter");
            m_cboxInclude = new ToolStripControlHost(new CheckBox());
            tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
            tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

            tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
            tsMain.Refresh();

            m_Err.m_Logfile.WriteLogFile("Leaving CreateDateTimes()");
        }

        void frmAcc_CloseUp(object sender, EventArgs e)
        {
            //  Requery();
        }

        void frmAcc_ValueChanged(object sender, EventArgs e)
        {
            if (((DateTimePicker)sender).Name == "FROM")
            {
                m_dtFrom = ((DateTimePicker)sender).Value;
            }
            else
            {
                m_dtThru = ((DateTimePicker)sender).Value;
            }

        }

        /// <summary>
        /// Client Bill Viewer Form
        /// </summary>
        /// <param name="args">String array containing:
        /// args[0] = server name
        /// args[1] = database
        /// </param>
        public ClientBillForm(string[] args)
        {
            InitializeComponent();
            if (args.GetUpperBound(0) < 1)
            {
                MessageBox.Show("Not enough arguments to start this application");
                Environment.Exit(13);
            }
            m_strServer = args[0];
            m_strDatabase = args[1];
            m_strProductionEnvironment = m_strDatabase.Contains("LIVE") ? "LIVE" : "TEST";
            string[] strArgs = new string[3];
            strArgs[0] = string.Format("/{0}", m_strProductionEnvironment);
            strArgs[1] = args[0];
            strArgs[2] = args[1];
            m_Err = new ERR(strArgs);

            //this.Text += string.Format(" - Production Environment {0}", m_strProductionEnvironment);

            sqlCon = new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};" +
                 "Integrated Security = 'SSPI'", m_strServer, m_strDatabase));
            m_Err.m_Logfile.WriteLogFile(string.Format("Connection String [{0}]", sqlCon.ConnectionString));

        }

        private void LoadClientCombo()
        {

            m_dicClients = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadClientCombo = new SqlCommand("select cli_mnem, cli_nme from client" +
                    " where deleted = 0", conn);
                sda.SelectCommand = cmdLoadClientCombo;
                m_Err.m_Logfile.WriteLogFile(string.Format("LoadClientCombo select [{0}]", cmdLoadClientCombo.CommandText));
                DataTable dtClient = new DataTable();
                try
                {
                    sda.Fill(dtClient);
                    foreach (DataRow dr in dtClient.Rows)
                    {
                        Application.DoEvents();
                        m_dicClients.Add(dr["cli_mnem"].ToString(), dr["cli_nme"].ToString());
                        tscbClient.Items.Add(dr["cli_mnem"]);

                    }
                    tscbClient.ComboBox.SelectedIndex = -1;
                }
                catch (Exception Exc)
                {
                    MessageBox.Show(
                        string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name,
                        Exc.Message), this.Name);


                    m_Err.m_Logfile.WriteLogFile(string.Format("Exception [{0}]", Exc.Message));
                }
            }
            m_Err.m_Logfile.WriteLogFile("Leaving LoadClientCombo()");
        }

        private void frmViewer_Load(object sender, EventArgs e)
        {

            m_Err.m_Logfile.WriteLogFile("Before CreateDateTimes()");
            CreateDateTimes();

            m_Err.m_Logfile.WriteLogFile("Before LoadClientCombo()");
            LoadClientCombo();

            m_Err.m_Logfile.WriteLogFile("Before CreateDataGridPrinter()");
            CreateDataGridPrinter();

            m_Err.m_Logfile.WriteLogFile("Before CreateWCExclDictionary()");
            CreateWCExclDictionary();

        }

        private void CreateWCExclDictionary()
        {
            m_dicWCExclusions = new Dictionary<string, string>();
            m_dicWCExclusions.Add("6322134", "FETAL FIBRONECTIN");
            m_dicWCExclusions.Add("5565222", "DIL RUSSELL VIPER VENOM");
            m_dicWCExclusions.Add("5527276", "PROTEIN S AG, TOTAL & FREE");
            m_dicWCExclusions.Add("5565248", "PROTEIN S ACTIVITY");
            m_dicWCExclusions.Add("5529345", "GENZYME FIRST SCREEN");
            m_dicWCExclusions.Add("5529617", "IMMUNO QN OTHER");
            m_dicWCExclusions.Add("5527821", "BILE ACIDS, TOTAL");
            m_dicWCExclusions.Add("5528570", "ANTIPHOSPHOLIPID EVALUATION");
            m_dicWCExclusions.Add("5528384", "VITAMIN K");
            m_dicWCExclusions.Add("5565170", "FACTOR V ASSAY");
            m_dicWCExclusions.Add("5687030", "FACTOR V LEIDEN");
            m_dicWCExclusions.Add("5527961", "METHYLENETET. REDUCTASE");
            m_dicWCExclusions.Add("5529120", "FACTOR II");
            m_dicWCExclusions.Add("5687032", "PROTHROMBIN NUCLEOTIDE -20210-G MUT");
            m_dicWCExclusions.Add("5949016", "THIN LAYER PAP");
            m_dicWCExclusions.Add("5527418", "VENIPUNCTURE");
            m_dicWCExclusions.Add("5525259", "METHYLENETET.REDUCTASE");
            m_dicWCExclusions.Add("5565120", "PROTHROMBIN TIME");
            m_dicWCExclusions.Add("5325020", "FETAL LUNG MATURITY");
            m_dicWCExclusions.Add("5529165", "INHIBIN A/B TUMOR MARKER");
            m_dicWCExclusions.Add("5687042", "HCV RNA QUANTITATION VIRAL");
            m_dicWCExclusions.Add("5527628", "COXSACKIE A  ANTIBODY PANEL");
            m_dicWCExclusions.Add("5525300", "SYPHILIS IGG AB");
            m_dicWCExclusions.Add("6322114", "OVA AND PARASITE SCREEN");
            m_dicWCExclusions.Add("6321004", "STOOL CULTURE");
            m_dicWCExclusions.Add("5687064", "E. COLI SHIGA TOXIN 1 AND 2");
            m_dicWCExclusions.Add("5525970", "COXSACKIE VIRUS B");
            m_dicWCExclusions.Add("5529605", "PREGNENOLONE");
            m_dicWCExclusions.Add("5528195", "NICOTINE");
            m_dicWCExclusions.Add("5529341", "INSULIN, FREE AND TOTAL");
            m_dicWCExclusions.Add("5642260", "CORTISOL A.M.");
            m_dicWCExclusions.Add("5642326", "CORTISOL P.M.");
            m_dicWCExclusions.Add("5642264", "CORTISOL POST DOSE");
            m_dicWCExclusions.Add("5642262", "CORTISOL PRE DOSE");
            m_dicWCExclusions.Add("5687026", "HIV1-VIRAL LOAD");
            m_dicWCExclusions.Add("5527058", "WESTERN BLOT");

            for (Int32 iStart = 5929106; iStart <= 5929116; iStart++)
            {
                m_dicWCExclusions.Add(iStart.ToString(), "SURG PATH CHARGES");
            }

            for (Int32 iStart = 5939106; iStart <= 5939117; iStart++)
            {
                m_dicWCExclusions.Add(iStart.ToString(), "SURG PATH CHARGES");
            }

            m_Err.m_Logfile.WriteLogFile("Leaving CreateWCExclDictionary()");
        }

        private void CreateDataGridPrinter()
        {
            m_ViewerPrintDocument = new PrintDocument();
            m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
            m_Err.m_Logfile.WriteLogFile("Leaving CreateDataGridPrinter()");
        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            scMain.Panel1Collapsed = false;
            if (tscbClient.ComboBox.SelectedIndex == -1)
            {
                return;
            }
            string strClient = tscbClient.SelectedItem.ToString();

            m_dicClients = new Dictionary<string, string>();
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();

                SqlCommand cmdLoadInvoice = null;

                cmdLoadInvoice =
                new SqlCommand(
                    string.Format("select * from cbill_hist where cl_mnem = '{0}' {1}",
                    strClient,
                    ((CheckBox)m_cboxInclude.Control).Checked ?
                        string.Format("and thru_date between '{0}' and '{1}'",
                            m_dtFrom.ToShortDateString(), m_dtThru.ToShortDateString()) : "")
                    , conn);
                sda.SelectCommand = cmdLoadInvoice;
                m_strRequery = cmdLoadInvoice.CommandText;
                DataTable dtInvoice = new DataTable();
                try
                {
                    sda.Fill(dtInvoice);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                    string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ex.Message),
                        this.Name);
                }
                tvAccounts.Nodes.Clear();

                TreeNode tnClient = tvAccounts.Nodes.Add(string.Format("{0}", strClient));
                tnClient.Name = "CLIENT";
                TreeNode tnInvoice = tnClient.Nodes.Add("INVOICES", "INVOICES");
                tnInvoice.Name = "INVOICE";
                foreach (DataRow drInv in dtInvoice.Rows)
                {
                    Application.DoEvents();
                    TreeNode nodeInvoiceInfo = tnInvoice.Nodes.Add(drInv["invoice"].ToString());
                    nodeInvoiceInfo.Name = "INFO";
                    nodeInvoiceInfo.Nodes.Add("THRU DATE", string.Format("Thru Date: {0}",
                        DateTime.Parse(drInv["thru_date"].ToString()).ToString("d")));
                    nodeInvoiceInfo.Nodes.Add("BAL DUE", string.Format("Balance Due : {0:C}",
                        double.Parse(double.Parse(drInv["balance_due"].ToString()).ToString("F2"))));


                }
                tnClient.Nodes.Add("CURRENT", "CURRENT");



                tscbClient.ComboBox.SelectedIndex = -1;


            }

        }

        private void tscbClient_SelectedIndexChanged(object sender, EventArgs e)
        {

            ssMain.Focus();

        }

        private void tvAccounts_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            e.Node.ExpandAll();
            if (e.Node.PrevNode != null)
            {
                e.Node.PrevNode.Collapse();
            }

            if (e.Node.Name == "INFO")
            {
                // this is the text of the invoice number
                string strInvoice = e.Node.Text;
                using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    SqlCommand cmdLoadRecords =
                        new SqlCommand(string.Format(
                            "with cte as ( " +
                            "select cl_mnem, chrg.account, acc.pat_name, acc.fin_code " +
                            ", cdm, qty as [QTY] " +
                            ", qty*net_amt as [charges] " +
                            ", comment " +
                            ", convert(datetime,convert(varchar(10),chrg.service_date,101)) as [DOS] " +
                            " ,convert(datetime,convert(varchar(10),chrg.post_date,101)) as [pdate] " +
                            " ,invoice " +
                            " from chrg " +
                            " inner join acc on acc.account = chrg.account " +
                            " ) select invoice, account,pat_name,fin_code,cdm,qty,charges,comment,[DOS], [pdate] from cte " +
                            " where invoice = '{0}' " +//and credited = 0 " +
                            " order by cl_mnem, pat_name , [DOS]",
                            strInvoice)
                            , conn);
                    sda.SelectCommand = cmdLoadRecords;
                    m_strRequery = cmdLoadRecords.CommandText.Replace(" order by cl_mnem, pat_name , [DOS]", "");
                    m_dtClient = new DataTable();
                    try
                    {
                        sda.Fill(m_dtClient);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(
                        string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ex.Message),
                            this.Name);
                    }


                    dgvRecords.DataSource = m_dtClient;
                    dgvRecords.TopLeftHeaderCell.Value = strInvoice;
                }
            }
            if (e.Node.Name == "CURRENT")
            {
                string strClient = e.Node.Parent.Text;

                using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    SqlCommand cmdLoadRecords =
                        new SqlCommand(string.Format(
                            "with cte as ( " +
                            "select cl_mnem, chrg.account, acc.pat_name, acc.fin_code " +
                            ", cdm, qty as [QTY] " +
                            ", qty*net_amt as [charges] " +
                            ", comment " +
                            ", convert(datetime,convert(varchar(10),chrg.service_date,101)) as [DOS] " +
                            " from chrg " +
                            " inner join acc on acc.account = chrg.account " +
                            " where chrg.post_date is null" +//and credited = 0 " +
                            " ) select account,pat_name,fin_code,cdm,qty,charges,comment,[DOS] from cte " +
                            "  where cl_mnem = '{0}' " +
                            " order by cl_mnem, pat_name , [DOS] "
                            ,
                            strClient)
                            , conn);
                    sda.SelectCommand = cmdLoadRecords;
                    m_strRequery = cmdLoadRecords.CommandText.Replace(" order by cl_mnem, pat_name , [DOS] ", "");
                    DataTable dtClient = new DataTable();
                    sda.Fill(dtClient);

                    dgvRecords.DataSource = dtClient;
                    dgvRecords.TopLeftHeaderCell.Value = "CURRENT";
                }
            }
            tsslAccounts.Text = string.Format("Records {0}", dgvRecords.Rows.Count);
        }

        private void dgvRecords_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                string strAccount = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                AccountForm frm = new AccountForm(strAccount)
                {
                    MdiParent = this.ParentForm
                };
                frm.Show();

                //LaunchAcc la = new LaunchAcc(m_strDatabase);
                //string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                //la.LaunchAccount(strAcc);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    string.Format("Exception occured trying to open the account. \r\n {0}", ex.Message));
            }
        }

        private void tsbCbill_Click(object sender, EventArgs e)
        {
            if (tscbClient.ComboBox.SelectedIndex == -1)
            {
                return;
            }
            string strClient = tscbClient.SelectedItem.ToString();
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                        "SELECT DISTINCT account, cl_mnem, trans_date, pat_name, fin_code " +
                        "FROM         dbo.acc " +
                        "inner join client on client.cli_mnem = acc.cl_mnem " +
                        "WHERE    type <> '0' and  (account IN " +
                        "(SELECT     account " +
                        "FROM          dbo.chrg " +
                        "WHERE      (status NOT IN ('CBILL', 'N/A')) AND (invoice IS NULL OR " +
                        "invoice = ''))) AND (fin_code IN ('APC', 'X', 'Y', 'W', 'Z')) " +
                        "order by cl_mnem, trans_date, pat_name ")
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "CBILL";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);
        }

        private void clientsWithYFinCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            dgvRecords.DataSource = null;
            ClearGrid();
            scMain.Panel1Collapsed = true;

            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                       "select cl_mnem, account, pat_name " +
                       "from acc " +
                       "where cl_mnem in ('BRMC','DMG','CFMC','CTSC','DMGH','EJC','GOO','MCJ','MSC','ONHE','WTN', " +
                       "'BLS','DDC','BAFC','LXMC','FCH','JFMC','TSOSC') " +
                       "and fin_code = 'Y' " +
                       "and  trans_date between '{0} 00:00' and '{1} 23:59:59' ",
                       m_dtFrom.ToShortDateString(),
                       m_dtThru.ToShortDateString())
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                m_strRequery = cmdLoadRecords.CommandText;
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);

        }

        private void ClearGrid()
        {
            dgvRecords.DataSource = null;
            try
            {
                dgvRecords.Rows.Clear();
                dgvRecords.Columns.Clear();
            }
            catch (ArgumentException ae)
            {

                MessageBox.Show(
                string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ae.Message),
                    this.Name);

            }
        }

        private void nonClientFinCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            ClearGrid();
            scMain.Panel1Collapsed = true;
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                       "select cl_mnem, account, pat_name, fin_code " +
                       "from acc " +
                       "where cl_mnem in ('BCBE','MAC','PTAI','WTTC','JAH','TTP') " +
                       "and not( fin_code in ('CLIENT', 'Y')) " +
                       "and  trans_date between '{0} 00:00' and '{1} 23:59:59' ",
                       m_dtFrom.ToShortDateString(),
                       m_dtThru.ToShortDateString())
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                m_strRequery = cmdLoadRecords.CommandText;
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);

        }

        private void patientsWmultipleFinCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {

            dgvRecords.DataSource = null;
            dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            ClearGrid();
            // dgvRecords.AlternatingRowsDefaultCellStyle.BackColor = Color.LightBlue;
            scMain.Panel1Collapsed = true;
            m_dtClient = new DataTable();
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadSql = new SqlCommand(
                    "select sql_code from monthly_reports where report_title = 'Multiple Fincode Patients'", conn);
                sda.SelectCommand = cmdLoadSql;
                sda.Fill(m_dtClient);
                string strSelect = m_dtClient.Rows[0]["sql_code"].ToString();
                m_dtClient = new DataTable();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                       strSelect,
                       m_dtFrom.ToShortDateString(),
                       m_dtThru.ToShortDateString())
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                m_strRequery = cmdLoadRecords.CommandText.Replace("order by cl_mnem, pat_name", "");
                sda.Fill(m_dtClient);

                foreach (DataColumn dc in m_dtClient.Columns)
                {
                    Application.DoEvents();
                    dgvRecords.Columns.Add(dc.ColumnName, dc.ColumnName);

                }
                foreach (DataRow dr in m_dtClient.Rows)
                {
                    Application.DoEvents();
                    dgvRecords.Rows.Add(dr.ItemArray);
                }


            }

            DataGridViewRow drOldRow = null;
            Patient patOld = null;
            List<string> strErrList = new List<string>();
            try
            {
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    Application.DoEvents();
                    if (dr.IsNewRow)
                    {
                        continue;
                    }
                    Patient patNew = new Patient(dr.Cells["pat_name"].Value.ToString());
                    //dr.DefaultCellStyle.BackColor = Color.White;
                    if (drOldRow == null)
                    {
                        drOldRow = dr;
                        patOld = new Patient(dr.Cells["pat_name"].Value.ToString());
                        continue;
                    }
                    if (patNew.propLastName == patOld.propLastName &&
                        patNew.propFirstName == patOld.propFirstName &&
                        (patNew.propMiddleName.StartsWith(patOld.propMiddleName) || patOld.propMiddleName.StartsWith(patNew.propMiddleName))
                        )
                    {
                        if (drOldRow.Cells["fin_code"].Value.ToString() !=
                            dr.Cells["fin_code"].Value.ToString())
                        {
                            dr.ErrorText = "Nonmatching fin_codes.";
                            drOldRow.ErrorText = "Nonmatching fin_codes.";

                            if (!strErrList.Contains(dr.Cells["pat_name"].Value.ToString())
                                && !strErrList.Contains(drOldRow.Cells["pat_name"].Value.ToString()))
                            {
                                strErrList.Add(dr.Cells["pat_name"].Value.ToString());
                            }
                        }

                    }
                    drOldRow = dr;
                    patOld = patNew;
                }
            }
            catch (IndexOutOfRangeException ioore)
            {
                MessageBox.Show(
                string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ioore.Message),
                        this.Name);

            }

            foreach (DataGridViewRow drRow in dgvRecords.Rows)
            {
                Application.DoEvents();
                if (drRow.IsNewRow)
                {
                    continue;
                }
                if (strErrList.Contains(drRow.Cells["pat_name"].Value.ToString()))
                {
                    continue;
                }
                if (string.IsNullOrEmpty(drRow.ErrorText))
                {
                    drRow.Visible = false;
                }
            }


            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);
            dgvRecords.Refresh();
            #region notes
            //string strPat_lname = null;
            //string strPat_suffix = null;
            //string strPat_fname = null;
            //string strPat_mname = null;
            //DateTime dtTrans_date = DateTime.MaxValue;
            //string strPat_sex = null;
            //string strFinCode = null;
            //string strClient = null;
            //string[] strPatName = new string[10]
            //    {"","","","","","","","","",""}; // [0] lname, [1] suffix, [2] fname [3] middle name/init

            //string str1 = "brown jr,james";
            //string str2 = "brown,james m";
            //string strLong;
            //string strShort;
            //if (str1.Length > str2.Length)
            //{
            //    strLong = str1;
            //    strShort = str2;
            //}
            //else
            //{
            //    strLong = str2;
            //    strShort = str1;
            //}
            //double dVal = 0.00d;
            //for (int i = 0; i < strShort.Length; i++)
            //{
            //    if (Char.Equals(strShort[i], strLong[i]))
            //    {
            //        dVal = ((double)i / strShort.Length);
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}
            #endregion notes



        }

        /// <summary>
        /// used to sort the account table only!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRecords_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (dgvRecords.DataSource == null)
            {
                return;
            }
            string strColText = dgvRecords.Columns[e.ColumnIndex].HeaderText;
            //   nFilterColumn = e.ColumnIndex;
            System.Windows.Forms.SortOrder SO = dgvRecords.SortOrder;
            BindingSource bbs = new BindingSource(m_dtClient, "ACCOUNT");
            //new BindingSource(m_dsAccount.Tables["ACC"], "ACCOUNT");

            BindingSource bs = new BindingSource(dgvRecords.DataSource,
                dgvRecords.Columns[e.ColumnIndex].Name);
            bs.DataMember = dgvRecords.DataMember;
            string strFilter = bs.Filter;

            if (e.Button == MouseButtons.Right)
            {
                if (dgvRecords.Columns[e.ColumnIndex].Name.ToUpper() == "BILLING TYPE")
                {
                    MessageBox.Show("Cannot filter on this column.");
                    return;
                }
                string strColName = dgvRecords.Columns[e.ColumnIndex].Name;
                FormResponse f = new FormResponse();
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    Application.DoEvents();
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
                bs = new BindingSource(dgvRecords.DataSource,
                    strColText);
                bs.DataMember = dgvRecords.DataMember;
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
                //    bs.RemoveFilter();

                //}
                //else
                //{

                //    bs.Filter = string.Format("[{0}] in ({1})' ", strColText, strResponse);
                //}
                //bs.Sort = string.Format("{0} {1},pat_name ASC", dgvRecords.Columns[e.ColumnIndex].Name,
                //    dgvRecords.SortOrder == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC");
                //dgvRecords.DataSource = bs;
            }
            else
            {
                string strSort = string.Format("{0} {1}, pat_name ASC", dgvRecords.Columns[e.ColumnIndex].Name
                    , dgvRecords.SortOrder == System.Windows.Forms.SortOrder.Ascending ? "ASC" : "DESC"
                    );
                bs.Sort = strSort;
                dgvRecords.DataSource = bs;
            }

        }

        private void Requery()
        {
            // m_strRequery;
            ClearGrid();
            scMain.Panel1Collapsed = true;
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format("{0} {1}", m_strRequery, m_strRequeryColFilter)
                       , conn);
                sda.SelectCommand = cmdLoadRecords;
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);
        }

        private void tsbPrintGrid_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(string.Format("Grid has {0} records! Continue?", dgvRecords.Rows.Count),
                "PRINT GRID", MessageBoxButtons.YesNo) != DialogResult.Yes)
            {
                return;
            }
            m_ViewerPrintDocument.Print();

        }

        /// <summary>
        /// Prints the applications current view 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbPrintView_Click(object sender, EventArgs e)
        {
            Bitmap[] bmps = RFClassLibrary.dkPrint.Capture(dkPrint.CaptureType.Form);
            try
            {
                bmps[0].Save(string.Format(@"C:\Temp\{0}.bmp", Application.ProductName));
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            try
            {
                RFClassLibrary.dkPrint.propStreamToPrint =
                  new StreamReader(string.Format(@"C:\Temp\{0}.bmp", Application.ProductName));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
                RFClassLibrary.dkPrint.propStreamToPrint.Close();
            }
            PrintDocument printDoc = new PrintDocument();

            printDoc.DefaultPageSettings.Landscape = true;
            printDoc.PrintPage += new PrintPageEventHandler
                (RFClassLibrary.dkPrint.PrintGraphic_PrintPage);

            printDoc.Print();

            printDoc.PrintPage -= new PrintPageEventHandler
                (RFClassLibrary.dkPrint.PrintGraphic_PrintPage);

            RFClassLibrary.dkPrint.propStreamToPrint.Close();

        }

        private void clientsWithSFinCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearGrid();

            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                       "select cl_mnem, account, pat_name " +
                       "from acc " +
                       "where cl_mnem in ('BRMC','DMG','CFMC','CTSC','DMGH','EJC','GOO','MCJ','MSC','ONHE','WTN', " +
                       "'BLS','DDC','BAFC','LXMC','FCH','JFMC','TSOSC') " +
                       "and fin_code = 'S' " +
                       "and  trans_date between '{0} 00:00' and '{1} 23:59:59' ",
                       m_dtFrom.ToShortDateString(),
                       m_dtThru.ToShortDateString())
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                m_strRequery = cmdLoadRecords.CommandText;
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);

        }

        private void industryBillingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ClearGrid();
            scMain.Panel1Collapsed = true;
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                       "select cl_mnem, account, pat_name , fin_code, convert(datetime,trans_date,101) as [trans_date] " +
                       "from acc " +
                       "inner join client on client.cli_mnem = acc.cl_mnem " +
                       "where status = 'new' and type = '6' and not (fin_code in ('Y','CLIENT')) " +
                       "and  trans_date between '{0} 00:00' and '{1} 23:59:59' " +
                       " order by cl_mnem, pat_name, trans_date"
                       , m_dtFrom.ToShortDateString()
                       , m_dtThru.ToShortDateString())
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                m_strRequery = cmdLoadRecords.CommandText.Replace(" order by cl_mnem, pat_name, trans_date", "");
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);
        }

        private void wCCDMNotToBillAsYToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StringBuilder sbExcl = new StringBuilder();
            foreach (string strKey in m_dicWCExclusions.Keys)
            {
                sbExcl.AppendFormat("'{0}',", strKey);
            }
            string strCDMs = string.Format("({0})", sbExcl.ToString().TrimEnd(new char[] { ',' }));
            ClearGrid();
            scMain.Panel1Collapsed = true;
            using (SqlConnection conn = new SqlConnection(sqlCon.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                SqlCommand cmdLoadRecords =
                    new SqlCommand(string.Format(
                       "select account, pat_name , fin_code, convert(datetime,trans_date,101) as [trans_date] " +
                       "from acc " +
                       "where cl_mnem = 'WC' and status = 'new' and (fin_code in ('Y','CLIENT')) " +
                       "and  trans_date between '{0} 00:00' and '{1} 23:59:59' " +
                       "and account in (select account from chrg where cdm in {2}) " +
                       " order by cl_mnem, pat_name, trans_date",
                       m_dtFrom.ToShortDateString(),
                       m_dtThru.ToShortDateString(),
                       strCDMs)
                        , conn);
                sda.SelectCommand = cmdLoadRecords;
                m_strRequery = cmdLoadRecords.CommandText.Replace(" order by cl_mnem, pat_name, trans_date", "");
                DataTable dtClient = new DataTable();
                sda.Fill(dtClient);

                dgvRecords.DataSource = dtClient;
                dgvRecords.TopLeftHeaderCell.Value = "GREEN SHEET";
            }
            tsslAccounts.Text = string.Format("{0} Records.", dgvRecords.Rows.Count);

        }

        private void InvoiceTestButton_Click(object sender, EventArgs e)
        {
            //setup test data
            InvoiceModel invModel = new InvoiceModel
            {
                StatementType = InvoiceModel.StatementTypeEnum.Invoice,
                ClientName = "WTTC",
                Address1 = "597 WEST FOREST COVE",
                City = "JACKSON",
                State = "TN",
                ZipCode = "38301",
                InvoiceNo = "75338",
                InvoiceDate = DateTime.Parse("02/02/2021"),
                InvoiceTotal = 21.00,
                BillingCompanyAddress = "PO Box 3099",
                BillingCompanyName = "Medical Center Laboratory",
                BillingCompanyCity = "Jackson",
                BillingCompanyState = "TN",
                BillingCompanyZipCode = "38303",
                BillingCompanyPhone = "731.541.7300 / 866.396.8537",
                ImageFilePath = @"\\wthmclbill\shared\billing\test\mcl-logo-horiz-300x60.jpg",
                InvoiceDetails = new List<InvoiceDetailModel>
                {
                    new InvoiceDetailModel
                    {
                        Account="L17138937",
                        PatientName = "MOUSE,MARION",
                        ServiceDate = DateTime.Parse("12/12/2020"),
                        AccountTotal = 13.54,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5362524",
                                CPT = "80202",
                                Description = "VANCOMYCIN TROUGH",
                                Qty = 1,
                                Amount = 13.54
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="L17158137",
                        PatientName = "DOE,JUNELEE",
                        ServiceDate = DateTime.Parse("01/09/2021"),
                        AccountTotal = 3.17 + 5.17,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "6127072",
                                CPT = "81001",
                                Description = "URINALYSIS W/MIC, C&S IF INDICATED",
                                Qty = 1,
                                Amount = 3.17
                            },
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5545154",
                                CPT = "88143",
                                Description = "CBC w/Auto Diff",
                                Qty = 1,
                                Amount = 5.17
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="L17158136",
                        PatientName = "DOE,MARCUS",
                        ServiceDate = DateTime.Parse("01/09/2021"),
                        AccountTotal = 13.54,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5362524",
                                CPT = "80202",
                                Description = "VANCOMYCIN TROUGH",
                                Qty = 1,
                                Amount = 13.54
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="L17163700",
                        PatientName = "DEER,MARGO",
                        ServiceDate = DateTime.Parse("01/17/2021"),
                        AccountTotal = 4.29,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5565120",
                                CPT = "85610",
                                Description = "PROTHROMBIN TIME",
                                Qty = 1,
                                Amount = 4.29
                            }
                        }
                    },
                    new InvoiceDetailModel
                    {
                        Account="WTTC",
                        PatientName = "WEST TENNESSEE TRANSITIONAL CARE",
                        ServiceDate = DateTime.Parse("12/29/2020"),
                        AccountTotal = -13.54,
                        InvoiceDetailLines = new List<InvoiceDetailLinesModel>
                        {
                            new InvoiceDetailLinesModel
                            {
                                CDM = "5362524",
                                CPT = "80202",
                                Description = "VANCOMYCIN TROUGH",
                                Qty = -1,
                                Amount = -13.54
                            }
                        }
                    },
                }
            };

            string filename = @"c:\temp\demo.pdf";
            //InvoicePrint.CreatePDF(invModel, filename);
            InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            invoicePrint.CreateInvoicePdf(invModel, filename);



            System.Diagnostics.Process.Start(filename);

        }
    } // other classes below this line that are part of this viewer

    class Patient
    {
        private string m_strName;
        private string _NameLast = "";
        public string propLastName
        {
            get { return _NameLast; }
            private set
            {
                _NameLast = value;
            }
        }
        private string _NameSuffix = "";
        public string propSuffix
        {
            get { return _NameSuffix; }
            private set
            {
                _NameSuffix = value;
            }
        }
        private string _NameFirst = "";
        public string propFirstName
        {
            get { return _NameFirst; }
            set
            {
                _NameFirst = value;
            }
        }
        private string _NameMiddle = "";
        public string propMiddleName
        {
            get { return _NameMiddle; }
            set
            {
                _NameMiddle = value;
            }
        }
        private string _NameTitle = "";
        public string propTitle
        {
            get { return _NameTitle; }
            set
            {
                _NameTitle = value;
            }
        }

        ArrayList m_alSuffix;
        ArrayList m_alTitle;
        public Patient(string strName)
        {
            m_alSuffix = new ArrayList();
            m_alSuffix.AddRange(new string[] {
                "JR","JR.","(JR)","(JR.)","SR","SR.","(SR)","(SR.)","II","III","IV","MD","M.D.","DDS","LLP"});

            m_alTitle = new ArrayList();
            m_alTitle.AddRange(new string[] {
                "MR","MR.","MRS","MRS.","MS","MS.","DR","DR."});

            m_strName = strName;
            ParseName();
        }

        private void ParseName()
        {
            // strName = "del ray me (J)()R., james wayne ";

            string[] strNameArr = m_strName.Split(new char[] { ',' });

            propLastName = strNameArr[0].Replace("(", "").Replace(")", "").Replace(".", "");

            string FMname = strNameArr[1].Trim();
            //if (strNameArr[1].Contains(' '))
            //{
            //    FMname = strNameArr[1].TrimEnd(new char[] { ' ' });
            //}
            //   string suffix = "";

            if (propLastName.Contains(" "))
            {
                string[] strLname = propLastName.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i <= strLname.GetUpperBound(0); i++)
                {
                    if (m_alSuffix.Contains(strLname[i].ToUpper()))
                    {
                        propSuffix = strLname[i];
                        break;
                    }
                    string.Format("{0} ", strLname[i]);
                }

            }
            //   string fname = "";
            //    string mname = "";
            if (FMname.Contains(" "))
            {
                propFirstName = FMname.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[0];
                propMiddleName = FMname.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries)[1];
            }
            else
            {
                propFirstName = FMname;
            }


        }
    }
}
