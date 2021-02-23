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

          
namespace LabBilling.Legacy
{
    public partial class frmQuest : Form
    {
        bool m_bIncludeCode = false; // exclude the button clicks from executing while testing
        private string propAppUser
        { get { return Environment.UserName; } }
       // List<string> m_lAcc = new List<string>();
     //   DataTable m_dtDictQuestReferenceLabTests = null;
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }
        Dictionary<string, string> m_dicPatient = null;
        HtmlDocument m_htmDoc = null;
        WebBrowser m_wbPrint = null;
        DataSet m_ds360 = null;
        private string m_strBillType = null;
        SqlConnection m_sqlConnection = null;
        int m_nFilterColumn = -1;
        R_ins m_rIns = null;
        R_pat m_rPat = null;
        DataSet m_dsQUE = null;
        R_chrg m_rChrgCurrent = null;
        R_chrg m_rChrgNew = null;
        R_chrg m_rChrgQUE = null;
        R_acc m_rAccOrig = null;
        R_acc m_rAccQUE = null;
        R_amt m_rAmtOrig = null; // wdk 20121031 added for amt records
        R_amt m_rAmtQUE = null; // wdk 20121031 added for amt records
        R_cli_dis m_rCliDisQUE = null;
        R_client m_rClient = null;
        R_cpt4 m_rCPT4 = null;

        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        ERR m_Err = null;
        ToolStripControlHost m_dpFrom;
        ToolStripControlHost m_dpThru;
        //ToolStripControlHost m_cboxInclude; // CheckBox
        //ToolStripControlHost m_cboxRightScreen; // CheckBox
        DateTime m_dtFrom;
        DateTime m_dtThru;
        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;
        
        /// <summary>
        /// Instanciate class
        /// </summary>
        /// <param name="args">args[0] = Server, args[1] = Database</param>
        public frmQuest(string[] args)
        {
            InitializeComponent();
            if (args.GetUpperBound(0) < 1)
            {
                MessageBox.Show("Not enough arguments to start this application",propAppName);
                Environment.Exit(13);
            }
            m_strServer = args[0].Remove(0, 1);
            m_strDatabase = args[1].Remove(0, 1);
            m_strProductionEnvironment = m_strDatabase.Contains("LIVE") ? "LIVE" : "TEST";
            string[] strArgs = new string[3];
            strArgs[0] = string.Format("/{0}", m_strProductionEnvironment);
            strArgs[1] = args[0];
            strArgs[2] = args[1];
            m_Err = new ERR(strArgs);

            m_rChrgCurrent = new R_chrg(m_strServer, m_strDatabase, ref m_Err);
            m_rChrgNew = new R_chrg(m_strServer, m_strDatabase, ref m_Err);
            m_rChrgQUE = new R_chrg(m_strServer, m_strDatabase, ref m_Err);
            m_rCliDisQUE = new R_cli_dis(m_strServer, m_strDatabase, ref m_Err);
            m_rAccOrig = new R_acc(m_strServer, m_strDatabase, ref m_Err);
            m_rAccQUE = new R_acc(m_strServer, m_strDatabase, ref m_Err);
            m_rIns = new R_ins(m_strServer, m_strDatabase, ref m_Err);
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);
            m_rAmtOrig = new R_amt(m_strServer, m_strDatabase, ref m_Err);
            m_rAmtQUE = new R_amt(m_strServer, m_strDatabase, ref m_Err);
            m_rClient = new R_client(m_strServer, m_strDatabase, ref m_Err);
            m_rCPT4 = new R_cpt4(m_strServer, m_strDatabase, ref m_Err);

            m_sqlConnection =
                new SqlConnection(string.Format("Data Source={0}; Initial Catalog = {1};"
                    + "Integrated Security = 'SSPI'; Connection TimeOut = 120", m_strServer, m_strDatabase));

            //this.Text += string.Format(" - Production Environment {0}", m_strProductionEnvironment);
            tsslBillType.Dock = DockStyle.Right;

            m_ViewerPrintDocument = new PrintDocument();
            m_ViewerPrintDocument.DefaultPageSettings.Landscape = true;
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);
          
        }

        private void CreateDateTimes()
        {
            int nSert = tsMain.Items.Count;
            // create the datetime controls for the From and Thru dates
            m_dpFrom = new ToolStripControlHost(new DateTimePicker());
            m_dpFrom.Text = DateTime.Parse("10/01/2012").ToShortDateString();

            m_dtFrom = DateTime.Parse("10/01/2012");
            ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
            
            ((DateTimePicker)m_dpFrom.Control).MinDate = DateTime.Parse("10/01/2012");
            
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
            m_dpThru.Text = DateTime.Now.AddDays(-15).ToString();
            m_dtThru = DateTime.Now.AddDays(-1);
            ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;

            ((DateTimePicker)m_dpThru.Control).MinDate = DateTime.Parse("10/01/2012");
            
            ((DateTimePicker)m_dpThru.Control).ValueChanged += new EventHandler(frmAcc_ValueChanged);
            ((DateTimePicker)m_dpThru.Control).CloseUp += new EventHandler(frmAcc_CloseUp);
            ((DateTimePicker)m_dpThru.Control).Name = "THRU";
            m_dpThru.Control.Width = 95;
            m_dpThru.Control.Refresh();
            m_dpThru.Invalidate();

            ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
            tsMain.Items.Insert(tsMain.Items.Count, tslThru);
            tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
            //   tsMain.BackColor = Color.Lavender;

            //// check box
            //ToolStripLabel tslInclude = new ToolStripLabel("Include \"BILLED\" in Filter");
            //m_cboxInclude = new ToolStripControlHost(new CheckBox());
            //tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
            //tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

            //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());

            //// check box
            //ToolStripLabel tslRightScreen = new ToolStripLabel("RIGHT SCREEN");
            //m_cboxRightScreen = new ToolStripControlHost(new CheckBox());
            //tsMain.Items.Insert(tsMain.Items.Count, tslRightScreen);
            //tsMain.Items.Insert(tsMain.Items.Count, m_cboxRightScreen);

            //tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());

            tsMain.Refresh();
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
            tsmiQuestRef.Enabled = true;

        }

          
        private void frmQuest_Load(object sender, EventArgs e)
        {
            if (!m_bIncludeCode)
            {
                if (MessageBox.Show("Include the code to execute the button clicks", propAppName, MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    m_bIncludeCode = true;
                }
            }
            CreateDateTimes();
            LoadBillingType();

        }

        private void LoadBillingType()
        {
            DataTable dtItems = new DataTable();
            dtItems.Columns.Add("BILL TYPE");
            dtItems.Columns.Add("DESCRIPTION");
            dtItems.Rows.Add(new object[] { "QR", "Quest Reference" });
            dtItems.Rows.Add(new object[] { "Q", "Quest Gap" });
            dtItems.Rows.Add(new object[] { "C", "Exclusion" });
            tscbBillingType.ComboBox.DataSource = dtItems;
            tscbBillingType.ComboBox.DisplayMember = "BILL TYPE";
            tscbBillingType.ComboBox.ValueMember = "DESCRIPTION";
            
        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            
            m_strBillType = "Q";
            tscbBillingType.SelectedIndex = tscbBillingType.ComboBox.FindStringExact(m_strBillType);
            int nAccCount = 0;

            m_dsQUE = new DataSet();
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                              
                sda.SelectCommand =
                    new SqlCommand(
                    // exclude the accounts that are in the table that are valid and ready to go
                        string.Format("with cte as ( " +
                        "select replace(account,'C','Q') as [account] from data_quest_360 " +
                        " where bill_type = 'Q' and deleted = 0" +
                        ") " +
                        "SELECT chrg.account, chrg_num, status, service_date, cdm, qty, net_amt, fin_type " +
                        "FROM         chrg " +
                        "left outer join cte on cte.account = chrg.account " +
                        "where chrg.account in " +
                        "(select account from acc where fin_code = 'Y' " +
                        "and trans_date between '{0} 00:00' and '{1} 23:59:59' " +
                        "and cl_mnem = 'QUESTR') " +
                        "and credited = 0 and coalesce(chrg.invoice,'') = '' and cte.account is null " +
                        "order by chrg.account, chrg_num",
                        m_dtFrom.ToString("d"), m_dtThru.ToString("d")
                        )
                        , conn);
                sda.SelectCommand.CommandTimeout = conn.ConnectionTimeout * 2;
                DateTime dtWait = DateTime.Now;
                try
                {
                    int nRec = sda.Fill(m_dsQUE);
                    //  Clipboard.SetText(sda.SelectCommand.CommandText, TextDataFormat.Text);
                }
                catch (SqlException )
                {
                    try
                    {
                        sda.SelectCommand.CommandTimeout = conn.ConnectionTimeout * 2;
                        int nRec = sda.Fill(m_dsQUE);
                    }
                    catch (SqlException )
                    {

                        MessageBox.Show("Failed to retrieve data twice. Closing Application.",propAppName);
                        Environment.Exit(13);
                    }
                }



                dgvRecords.DataSource = m_dsQUE.Tables[0];
                string strOldAcct = "";

                tspbRecords.Minimum = 0;
                tspbRecords.Maximum = dgvRecords.Rows.Count;
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    try
                    {
                        Application.DoEvents();
                        tspbRecords.PerformStep();
                    }
                    catch (Exception)
                    {
                        //int x;
                        //x = 0;
                    }
                    if (strOldAcct != dr.Cells["account"].Value.ToString())
                    {
                        nAccCount++;
                        strOldAcct = dr.Cells["account"].Value.ToString();
                    }
                }
                tsslRecords.Text = string.Format("{0} Records for {1} Accounts", dgvRecords.Rows.Count, nAccCount);
            }
            //MessageBox.Show("Load Completed",propAppName);
            dgvRecords.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tsslRecords.Text = string.Format("{0} Records for {1} Accounts", dgvRecords.Rows.Count, nAccCount);
            this.Refresh();
        }

        private void dgvRecords_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                LaunchAcc la = new LaunchAcc(m_strDatabase);
                string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                la.LaunchAccount(strAcc);
                if (!strAcc.StartsWith("Q"))
                {
                    la.LaunchAccount(strAcc.Replace('C', 'Q'));

                    LaunchAcc la2 = new LaunchAcc(m_strDatabase);
                    string strAcc2 = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();
                    la.LaunchAccount(strAcc.Replace("C", "QR"));
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Exception occured trying to open the account.", propAppName);
            }
        }
        /// <summary>
        /// used to sort the account table only!
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void dgvRecords_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            m_nFilterColumn = e.ColumnIndex;
            if (e.Button == MouseButtons.Right)
            {
                //  works but has limited use.
                FormResponse f = new FormResponse();
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {
                    string strText = dr.Cells[e.ColumnIndex].FormattedValue.ToString();
                    if (!f.clbFilter.Items.Contains(strText))
                    {
                        f.clbFilter.Items.Add(strText);
                    }
                }
                string strResponse = null;
                if (f.ShowDialog() == DialogResult.Yes)
                {
                    foreach (string str in f.clbFilter.CheckedItems)
                    {
                        strResponse += string.Format("'{0}',", str);
                    }
                    if (!string.IsNullOrEmpty(strResponse))
                    {
                        int nli = strResponse.LastIndexOf(',');
                        int nlen = strResponse.Length;
                        strResponse = strResponse.Remove(strResponse.LastIndexOf(','));
                    }

                }
                string strColText = dgvRecords.Columns[m_nFilterColumn].HeaderText;
                BindingSource bs = new BindingSource(dgvRecords.DataSource,
                    strColText);
                bs.DataMember = dgvRecords.DataMember;
                if (string.IsNullOrEmpty(strResponse))
                {
                    bs.RemoveFilter();
                }
                else
                {
                    bs.Filter = string.Format("[{0}] in ({1})", strColText, strResponse);
                }
                dgvRecords.DataSource = bs;
            }
            double dAmt = 0.00;
            int nQty = 0;
            foreach(DataGridViewRow dr in dgvRecords.Rows)
            {
              //  dAmt += int.Parse(dr.Cells["qty"].Value.ToString())*double.Parse(dr.Cells["charges"].Value.ToString());
                try
                {
                    nQty += int.Parse(dr.Cells["qty"].Value.ToString());
                }
                catch (ArgumentException)
                {
                    // when sorting via EXCEPTION LIST BILLING THIS DOES NOT APPLY we set the entire account to 1500
                }

            }

            tsslAmount.Text = string.Format("Totals: QTY {0} -- CHARGES {1:C2}", nQty, dAmt);
        }

        private void tsbPrint_Click(object sender, EventArgs e)
        {
            //m_ViewerPrintDocument = new PrintDocument();
            string strName = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);

            m_ViewerPrintDocument.DocumentName = this.Text;
            m_rgReport = new ReportGenerator(dgvRecords, m_ViewerPrintDocument, strName, m_strDatabase);
            int nRows = dgvRecords.Rows.Count;
            if (nRows == 0)
            {
                MessageBox.Show("No records ready to print.", propAppName);
                return;
            }
            m_ViewerPrintDocument.Print();
        }

        private void TsbBillMCLRef_Click(object sender, EventArgs e)
        {
            UseProcedureQuestCharges();
            return;
            //if (!m_bIncludeCode)
            //{
            //    tsmiMCLRef.Enabled = Environment.UserName.ToUpper().Contains("WKELLY") ? true : false;
            //    tsmiBillExclusions.Enabled = false; // don't allow posting
            //    tsmiExclusions.Enabled = true; // allow selecting
            //    return;
            //}
            //tsmiMCLRef.Enabled = Environment.UserName.ToUpper().Contains("WKELLY")? true: false;
            //tsmiBillExclusions.Enabled = false;

            //tspbRecords.Maximum = dgvRecords.Rows.Count;
            //m_strBillType = tscbBillingType.Text;

            //string strAccCurrent = null;
            //string strAccReplace = null;
            //string strChrgNum = null;
            //string strPostedAcc = null;
            //tspbRecords.Minimum = 0;
            //tspbRecords.Maximum = dgvRecords.Rows.Count;

            //foreach (DataGridViewRow dr in dgvRecords.Rows)
            //{
            //    Application.DoEvents();
            //    tspbRecords.PerformStep();

            //    try
            //    {
            //        if (!string.IsNullOrEmpty(dr.ErrorText))
            //        {
            //            continue;
            //        }
            //        if (dr.Cells["bill type"].Value.ToString().ToUpper() == "GAP" &&
            //                string.IsNullOrEmpty(dr.Cells["quest code"].Value.ToString().ToUpper())
            //            && dr.Index != -1)
            //        {
            //            continue; // cannot bill until code is obtained.
            //        }
            //    }
            //    catch (ArgumentException ae)
            //    {
            //        MessageBox.Show(ae.Message, propAppName);
            //    }
            //    tspbRecords.PerformStep();

            //    strAccCurrent = dr.Cells["account"].Value.ToString().ToUpper();
            //    strAccReplace = strAccCurrent.Replace("C", m_strBillType).Replace("D", m_strBillType).ToUpper();
            //    tsslAmount.Text = string.Format("Working on account {0}", strAccCurrent);
            //    this.Invalidate();
            //    m_rAccOrig.ClearMemberVariables();
            //    // TODO: need to check the charge record here not the account
            //    int nRec = m_rAccOrig.GetActiveRecords(string.Format("ACCOUNT = '{0}'", strAccCurrent));
            //    // convert to 1500 or not the original account in the data grid view
            //    //string strPostedAcc = null;
            //    if (m_strBillType == "C")
            //    {
            //        string strMsg;
            //        strAccCurrent = dr.Cells["account"].Value.ToString();
            //        string strSSIType = dr.Cells["SSI type"].Value.ToString();
            //        if (strAccCurrent == strPostedAcc)
            //        {
            //            continue;
            //        }
            //        strPostedAcc = strAccCurrent;
            //        if (m_rAccOrig.UpdateField("status", strSSIType, string.Format("ACCOUNT = '{0}'", strAccCurrent), out strMsg) == -1)
            //        {
            //            m_Err.m_Logfile.WriteLogFile(strMsg);
            //        }
            //        m_Err.m_Logfile.WriteLogFile(string.Format("{0}:{1}", dr.Cells["account"].Value.ToString(), strMsg));

            //        continue;
            //    }

            //    strChrgNum = dr.Cells["chrg_num"].Value.ToString();

            //    string strChrgOrig = string.Format("account = '{0}' and chrg_num = '{1}'",
            //                strAccCurrent, strChrgNum);
            //    m_rChrgCurrent.ClearMemberVariables();
            //    int nRecChrg = m_rChrgCurrent.GetRecords(strChrgOrig);
            //    if (m_rChrgCurrent.GetRecords(strChrgOrig) == 1)
            //    {
            //        // IF THE CURRENT CHARGE HAS ALREADY BEEN CREDITED JUST GO TO THE NEXT CHARGE
            //        // IT HAS MULTIPLE CPT4'S

            //        if (m_rChrgCurrent.propCredited.ToUpper() == "TRUE")
            //        {
            //            continue;
            //        }
            //        // set the old charge as credited
            //        m_rChrgCurrent.propCredited = "1";
            //        int nRecUpdate = 0;
            //        try
            //        {
            //            nRecUpdate = m_rChrgCurrent.Update(); // update the old charge                                      
            //        }
            //        catch (Exception ex)
            //        {
            //            SendEmail(propAppName,ex.GetType().ToString(), ex.Message);
            //        }
            //        // fix needed
            //        // add the new reversal charge.
            //        string strRowguidChrgNew = Guid.NewGuid().ToString();
            //        m_rChrgNew.ClearMemberVariables();
            //        //strChrgOrig = string.Format("account = '{0}' and chrg_num = '{1}",
            //        strChrgOrig = string.Format("account = '{0}' and rowguid = '{1}", // wdk 20140423
            //                strAccCurrent, strRowguidChrgNew);
            //        m_rChrgNew.GetRecords(strChrgOrig);

            //        m_rChrgNew.propAccount = m_rChrgCurrent.propAccount;
            //        m_rChrgNew.propCredited = "1";
            //        m_rChrgNew.propStatus = "NEW";
            //        m_rChrgNew.propService_date = m_rChrgCurrent.propService_date;
            //        m_rChrgNew.propCdm = m_rChrgCurrent.propCdm;
            //        m_rChrgNew.propRetail = m_rChrgCurrent.propRetail;
            //        m_rChrgNew.propInp_price = m_rChrgCurrent.propInp_price;
            //        m_rChrgNew.propNet_amt = m_rChrgCurrent.propNet_amt;
            //        if (m_rChrgNew.propCdm == "5845090")
            //        {
            //            m_rChrgNew.propNet_amt =double.Parse(
            //                (double.Parse(m_rChrgCurrent.propNet_amt) 
            //                + double.Parse("11.57")).ToString()).ToString("F2");
            //        }
            //        m_rChrgNew.propMt_req_no = m_rChrgCurrent.propMt_req_no;
            //        m_rChrgNew.propFin_type = m_rChrgCurrent.propFin_type;
            //        m_rChrgNew.propFinCode = m_rChrgCurrent.propFinCode;
            //        m_rChrgNew.m_strRowguid = strRowguidChrgNew;
            //        m_rChrgNew.propQty = string.Format("{0}", int.Parse(m_rChrgCurrent.propQty) * -1);
            //        m_rChrgNew.propComment = string.Format("moved to [{0}]", strAccReplace);

            //        int nRecChrgNewAdded = 0;
            //        try
            //        {
            //            // at this point the net_amt, retail and inp_price will be wrong for split billing
            //            nRecChrgNewAdded = m_rChrgNew.AddRecord(
            //                 string.Format("rowguid = '{0}'", strRowguidChrgNew));
            //        }
            //        catch (Exception ex)
            //        {
            //            SendEmail(propAppName,ex.GetType().ToString(),ex.Message.Substring(0,ex.Message.Length > 1024? 1024:ex.Message.Length));
            //            m_Err.m_Logfile.WriteLogFile(strAccCurrent);
            //            m_Err.m_Logfile.WriteLogFile(strAccReplace);
            //            m_Err.m_Logfile.WriteLogFile(ex.GetType().ToString());
            //            m_Err.m_Logfile.WriteLogFile(ex.Message);
            //        }

            //        // add the new amt records for the credited charge
            //        m_rAmtOrig.ClearMemberVariables();
            //        int nRecAmt = m_rAmtOrig.GetActiveRecords(
            //                string.Format("chrg_num = '{0}'", strChrgNum));
            //        //nRecAmt = m_rAmtOrig.GetActiveRecords(
            //        //        string.Format("chrg_num = '{0}'", strChrgNum));
            //        // should not be any amount records with the new charge number
            //        m_rAmtQUE.ClearMemberVariables();
            //        m_rAmtQUE.GetActiveRecords(
            //                string.Format("chrg_num = '{0}'", m_rChrgNew.propChrg_num));
            //        for (int i = 0; i < nRecAmt; i++)
            //        {
            //            Application.DoEvents();
            //            m_rAmtQUE.propChrg_num = m_rChrgNew.propChrg_num;
            //            if (dr.Cells["cpt4"].Value.ToString() == m_rAmtOrig.propCpt4)
            //            {
            //                m_rAmtQUE.propCpt4 = m_rAmtOrig.propCpt4;
            //                m_rAmtQUE.propBillMethod = "QUESTR";
            //            }

            //            m_rAmtQUE.propType = m_rAmtOrig.propType;
            //            m_rAmtQUE.propAmount = m_rAmtOrig.propAmount;
            //            m_rAmtQUE.propModi = m_rAmtOrig.propModi;
            //            m_rAmtQUE.propRevcode = m_rAmtOrig.propRevcode;
            //            m_rAmtQUE.propModi2 = m_rAmtOrig.propModi2;
            //            m_rAmtQUE.propDiagnosis_code_ptr = m_rAmtOrig.propDiagnosis_code_ptr;
            //            m_rAmtQUE.propPointerSet = m_rAmtOrig.propPointerSet;
            //            int nAdd = m_rAmtQUE.AddRecord(
            //                string.Format("chrg_num = '{0}'", m_rChrgNew.propChrg_num));
            //            bool bMore = m_rAmtOrig.GetNext();
            //        }
            //        // wdk 20131121 handle path consultation on this cdm.
            //        try
            //        {
            //            if (strAccCurrent == "C3919959")
            //            {
            //                MessageBox.Show("Account c3919959 has a path consult charge", propAppName);
            //            }
            //            if (m_rChrgNew.propCdm == "5845090")
            //            {
            //                m_rAmtQUE.propChrg_num = m_rChrgNew.propChrg_num;
            //                m_rAmtQUE.propCpt4 = "80500";
            //                m_rAmtQUE.propBillMethod = "QUESTR";

            //                m_rAmtQUE.propType = "NORM";
            //                m_rAmtQUE.propAmount = "11.58";
            //                //m_rAmtQUE.propModi = "NULL";
            //                m_rAmtQUE.propRevcode = "300";
            //                //m_rAmtQUE.propModi2 = m_rAmtOrig.propModi2;
            //                //m_rAmtQUE.propDiagnosis_code_ptr = m_rAmtOrig.propDiagnosis_code_ptr;
            //                int nAdd = m_rAmtQUE.AddRecord(
            //                    string.Format("chrg_num = '{0}'", m_rChrgNew.propChrg_num));
            //            }
            //        }
            //        catch (Exception ex)
            //        {
            //            SendEmail(propAppName, string.Format("{0}-{1}", ex.GetType().ToString(), "5845090"), ex.Message);
            //        }


            //        //then add quest charge record
            //        if (m_strBillType.ToUpper().StartsWith("Q"))
            //        {
            //            WriteQuestCharge(strAccReplace, m_rChrgCurrent.propCdm);
            //        }

            //    }


            //    // any post processing should go here

            //    // copy the pat record
            //    m_rPat.ClearMemberVariables();
            //    int nPatRec = m_rPat.GetActiveRecords(string.Format("ACCOUNT = '{0}'", strAccReplace));
            //    if (nPatRec != 1)
            //    {
            //        InsertIntoPatTable(strAccCurrent, strAccReplace);
            //    }
            //    // copy the ins record
            //    m_rIns.ClearMemberVariables();
            //    int nInsRec = m_rIns.GetActiveRecords(string.Format("ACCOUNT = '{0}'", strAccReplace));
            //    if (nInsRec != 1)
            //    {
            //        InsertIntoInsTable(strAccCurrent, strAccReplace);
            //    }

            //}
            //Application.DoEvents();

            //if (!m_bIncludeCode)
            //{
            //    if (((ToolStripMenuItem)sender) == tsmiBillMCL)
            //    {
            //        tsbLoad_Click(null, null);

            //        tsb360_Click(null, null);
            //    }
            //}
            //tsddbtnLoad.Enabled = true;
            //MessageBox.Show("Processing completed", propAppName);
            //tsmiExclusions.Enabled = true;

        }

        private void UseProcedureQuestCharges()
        {
            if (!m_bIncludeCode)
            {
                tsmiMCLRef.Enabled = Environment.UserName.ToUpper().Contains("WKELLY") ? true : false;
                tsmiBillExclusions.Enabled = false; // don't allow posting
                tsmiExclusions.Enabled = true; // allow selecting
                return;
            }
            tsmiMCLRef.Enabled = Environment.UserName.ToUpper().Contains("WKELLY")? true: false;
            tsmiBillExclusions.Enabled = false;

            tspbRecords.Maximum = dgvRecords.Rows.Count;
            m_strBillType = tscbBillingType.Text;

            string strAccCurrent;
            string strChrgNum;
            //string strPostedAcc;
            tspbRecords.Minimum = 0;
            tspbRecords.Maximum = dgvRecords.Rows.Count;

            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                Application.DoEvents();
                tspbRecords.PerformStep();

                try
                {
                    if (!string.IsNullOrEmpty(dr.ErrorText) ||
                        dr.IsNewRow)
                    {
                        continue;
                    }
                    if (dr.Cells["bill type"].Value.ToString().ToUpper() == "GAP" &&
                            string.IsNullOrEmpty(dr.Cells["quest code"].Value.ToString().ToUpper())
                        && dr.Index != -1)
                    {
                        continue; // cannot bill until code is obtained.
                    }
                }
                catch (ArgumentException ae)
                {
                    MessageBox.Show(                                                 
                        string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ae.Message)  , propAppName);
                }
                tspbRecords.PerformStep();

                strAccCurrent = dr.Cells["account"].Value.ToString().ToUpper();
                strChrgNum = dr.Cells["chrg_num"].Value.ToString().ToUpper();
            //    strAccReplace = strAccCurrent.Replace("C", m_strBillType).Replace("D", m_strBillType).ToUpper();
                tsslAmount.Text = string.Format("Working on account {0}", strAccCurrent);
                Application.DoEvents();
                
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlTransaction trans = null;
                    try
                    {
                        conn.Open();
                        trans = conn.BeginTransaction();
                        new SqlCommand(
                            string.Format("EXEC usp_prg_Quest_Charges @chrg_num = {0}", strChrgNum)
                            , conn, trans).ExecuteNonQuery();
                        trans.Commit();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(                                                 
                        string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ex.Message)  , propAppName);
          
                        trans.Rollback();
                    }
                    finally
                    {
                        conn.Close();
                    }
                }
            }
            Application.DoEvents();

            if (m_bIncludeCode)
            {
                tsbLoad_Click(null, null);
                tsb360_Click(null, null);
            }
            tsddbtnLoad.Enabled = true;
            MessageBox.Show("Processing completed", propAppName);
            tsmiExclusions.Enabled = true;
        }

        private void SendEmail(string strApp, string strType, string strError)
        {
            using (SqlConnection conn = new SqlConnection(
            string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
            m_strServer, m_strDatabase)))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                sda.SelectCommand = new SqlCommand(
                    string.Format("exec msdb.dbo.sp_send_dbmail @recipients = N'{2}', " +
                    " @body='{0}', @subject= N'{1}', @profile_name = N'Outlook'", strError, strType, "david.kelly@wth.org"), conn);
                sda.SelectCommand.Connection.Open();
                try
                {
                    sda.SelectCommand.ExecuteNonQuery();
                }
                finally
                {
                    sda.SelectCommand.Connection.Close();
                }

            }
        }


        /// <summary>
        /// 1. Get a recordset for the original charge to handle the reversal of the original charge
        /// 2. Get a recordset for the reversed charge to to be added to the account.
        /// 3. Get a recordset for the New charge on the new account if and account does not exist.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbBillExclusions_Click(object sender, EventArgs e)
        {
            if (!m_bIncludeCode)
            {
                return;
            }
            tsmiExclusions.Enabled = false;
            tsmiBillExclusions.Enabled = false;
            
            
            
            tspbRecords.Maximum = dgvRecords.Rows.Count;
            m_strBillType = tscbBillingType.Text;
  
            string strAccCurrent = null;
            string strAccReplace = null;
            string strChrgNum = null;
            string strPostedAcc = null;
            tspbRecords.Minimum = 0;
            tspbRecords.Maximum = dgvRecords.Rows.Count;
            
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                try
                {
                    Application.DoEvents();
                    this.Invalidate();              
                }
                catch (Exception )
                {
                    //int x;
                    //x = 0;
                }
                tspbRecords.PerformStep();
                
                try
                {
                    if (!string.IsNullOrEmpty(dr.ErrorText))
                    {
                        continue;
                    }
                    if (dr.Cells["bill type"].Value.ToString().ToUpper() == "GAP" &&
                            string.IsNullOrEmpty(dr.Cells["quest code"].Value.ToString().ToUpper())
                        && dr.Index != -1)
                    {
                        // cannot bill until code is obtained.
                        continue;
                    }
                }
                catch (ArgumentException ae)
                {
                    MessageBox.Show(ae.Message, propAppName);
                }
                tspbRecords.PerformStep();
                
                strAccCurrent = dr.Cells["account"].Value.ToString().ToUpper();
                strAccReplace = strAccCurrent.Replace("C", m_strBillType).Replace("D",m_strBillType).ToUpper();
                tsslAmount.Text = string.Format("Working on account {0}", strAccCurrent);
                this.Invalidate();
                m_rAccOrig.ClearMemberVariables();
                // TODO: need to check the charge record here not the account
                int nRec = m_rAccOrig.GetActiveRecords(string.Format("ACCOUNT = '{0}'", strAccCurrent));
                // convert to 1500 or not the original account in the data grid view
                //string strPostedAcc = null;
                if (m_strBillType == "C")
                {
                    string strMsg;
                    strAccCurrent = dr.Cells["account"].Value.ToString();
                    string strSSIType = dr.Cells["SSI type"].Value.ToString();
                    if (strAccCurrent == strPostedAcc)
                    {
                        continue;
                    }
                    strPostedAcc = strAccCurrent;
                    if (m_rAccOrig.UpdateField("status", strSSIType, string.Format("ACCOUNT = '{0}'", strAccCurrent), out strMsg) == -1)
                    {
                        m_Err.m_Logfile.WriteLogFile(strMsg);
                    }
                    m_Err.m_Logfile.WriteLogFile(string.Format("{0}:{1}", dr.Cells["account"].Value.ToString(), strMsg));                   
                   
                    continue;
                }

                strChrgNum = dr.Cells["chrg_num"].Value.ToString();
           
                string strChrgOrig = string.Format("account = '{0}' and chrg_num = '{1}'",
                            strAccCurrent, strChrgNum);
                m_rChrgCurrent.ClearMemberVariables();
                int nRecChrg = m_rChrgCurrent.GetRecords(strChrgOrig);
                if (m_rChrgCurrent.GetRecords(strChrgOrig) == 1)
                {
                    // IF THE CURRENT CHARGE HAS ALREADY BEEN CREDITED JUST GO TO THE NEXT CHARGE
                    // IT HAS MULTIPLE CPT4'S

                    if (m_rChrgCurrent.propCredited.ToUpper() == "TRUE")
                    {
                        continue;
                    }
                    // set the old charge as credited
                    m_rChrgCurrent.propCredited = "1";
                    int nRecUpdate = 0;
                    try
                    {
                        nRecUpdate = m_rChrgCurrent.Update(); // update the old charge                                      
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(this,string.Format("Copy this message and send to David. \r\r\n{0}",
                            ex.Message), string.Format("{0} - {1}",propAppName,ex.GetType().ToString()),MessageBoxButtons.RetryCancel,MessageBoxIcon.Stop,MessageBoxDefaultButton.Button2);
                    }

                    // add the new reversal charge.
                    string strRowguidChrgNew = Guid.NewGuid().ToString();
                    m_rChrgNew.ClearMemberVariables();
                    strChrgOrig = string.Format("account = '{0}' and chrg_num = '{1}",
                            strAccCurrent, strRowguidChrgNew);
                    m_rChrgNew.GetRecords(strChrgOrig);

                    m_rChrgNew.propAccount = m_rChrgCurrent.propAccount;
                    m_rChrgNew.propCredited = "1";
                    m_rChrgNew.propStatus = "NEW";
                    m_rChrgNew.propService_date = m_rChrgCurrent.propService_date;
                    m_rChrgNew.propCdm = m_rChrgCurrent.propCdm;                  
                    m_rChrgNew.propRetail = m_rChrgCurrent.propRetail;
                    m_rChrgNew.propInp_price = m_rChrgCurrent.propInp_price;
                    
                    m_rChrgNew.propNet_amt = m_rChrgCurrent.propNet_amt;
                    m_rChrgNew.propMt_req_no = m_rChrgCurrent.propMt_req_no;
                    m_rChrgNew.propFin_type = m_rChrgCurrent.propFin_type;
                    m_rChrgNew.propFinCode = m_rChrgCurrent.propFinCode;
                    m_rChrgNew.m_strRowguid = strRowguidChrgNew;
                    m_rChrgNew.propQty = string.Format("{0}",int.Parse(m_rChrgCurrent.propQty)*-1);
                    m_rChrgNew.propComment = string.Format("moved to [{0}]", strAccReplace);

                    int nRecChrgNewAdded = 0;
                    try
                    {
                        // at this point the net_amt, retail and inp_price will be wrong for split billing
                        nRecChrgNewAdded = m_rChrgNew.AddRecord(
                             string.Format("rowguid = '{0}'", strRowguidChrgNew));
                    }
                    catch (Exception ex)
                    {
                        m_Err.m_Logfile.WriteLogFile(strAccCurrent);
                        m_Err.m_Logfile.WriteLogFile(strAccReplace);
                        m_Err.m_Logfile.WriteLogFile(ex.GetType().ToString());
                        m_Err.m_Logfile.WriteLogFile(ex.Message);
                    }

                    // add the new amt records for the credited charge
                    m_rAmtOrig.ClearMemberVariables();
                    int nRecAmt = m_rAmtOrig.GetActiveRecords(
                            string.Format("chrg_num = '{0}'", strChrgNum));
                    //nRecAmt = m_rAmtOrig.GetActiveRecords(
                    //        string.Format("chrg_num = '{0}'", strChrgNum));
                    // should not be any amount records with the new charge number
                    m_rAmtQUE.ClearMemberVariables();
                    m_rAmtQUE.GetActiveRecords(
                            string.Format("chrg_num = '{0}'", m_rChrgNew.propChrg_num));
                    for (int i = 0; i < nRecAmt; i++)
                    {
                        Application.DoEvents();
                        m_rAmtQUE.propChrg_num  = m_rChrgNew.propChrg_num;
                        if (dr.Cells["cpt4"].Value.ToString() == m_rAmtOrig.propCpt4)
                        {
                            m_rAmtQUE.propCpt4 = m_rAmtOrig.propCpt4;
                            m_rAmtQUE.propBillMethod = "QUESTR";
                        }
                        
                        m_rAmtQUE.propType = m_rAmtOrig.propType;
                        m_rAmtQUE.propAmount = m_rAmtOrig.propAmount;
                        m_rAmtQUE.propModi = m_rAmtOrig.propModi;
                        m_rAmtQUE.propRevcode = m_rAmtOrig.propRevcode;
                        m_rAmtQUE.propModi2 = m_rAmtOrig.propModi2;
                        m_rAmtQUE.propDiagnosis_code_ptr = m_rAmtOrig.propDiagnosis_code_ptr;
                        int nAdd = m_rAmtQUE.AddRecord(
                            string.Format("chrg_num = '{0}'", m_rChrgNew.propChrg_num));
                        bool bMore = m_rAmtOrig.GetNext();
                    }


                    //then add quest charge record
                    if (m_strBillType.ToUpper().StartsWith("Q"))
                    {
                        WriteQuestCharge(strAccReplace, m_rChrgCurrent.propCdm);
                    }
                    
                }

            
            // any post processing should go here
  
                // copy the pat record
                m_rPat.ClearMemberVariables();                
                int nPatRec = m_rPat.GetActiveRecords(string.Format("ACCOUNT = '{0}'", strAccReplace));
                if (nPatRec != 1)
                {
                    InsertIntoPatTable(strAccCurrent, strAccReplace);
                }
                // copy the ins record
                m_rIns.ClearMemberVariables();
                int nInsRec = m_rIns.GetActiveRecords(string.Format("ACCOUNT = '{0}'", strAccReplace));
                if (nInsRec != 1)
                {
                    InsertIntoInsTable(strAccCurrent, strAccReplace);
                }
               
            }
            Application.DoEvents();
            
            if (((ToolStripMenuItem) sender) == tsmiBillMCL)
            {
                tsbLoad_Click(null, null);
                
                tsb360_Click(null, null);
            }
            tsddbtnLoad.Enabled = true;
            MessageBox.Show("Processing completed", propAppName);
            tsmiExclusions.Enabled = true;
            
        }

        private void InsertIntoInsTable(string strAccCurrent, string strAccReplace)
        {  
            SqlDataAdapter sda = new SqlDataAdapter();
            DataTable dtIns = new DataTable();
            sda.SelectCommand = new SqlCommand(
                string.Format("SELECT   account, ins_a_b_c, holder_nme, holder_dob, holder_sex, holder_addr, holder_city_st_zip, plan_nme, plan_addr1, plan_addr2, "+
                "p_city_st, policy_num, cert_ssn, grp_nme, grp_num, employer, e_city_st, fin_code, ins_code, relation, mod_date, mod_user, mod_prg, mod_host "+
                "FROM  ins "+
                " where account = '{0}' and deleted = 0"
                , strAccCurrent), m_sqlConnection);
            sda.Fill(dtIns);

            sda.InsertCommand = new SqlCommand(
                string.Format("INSERT INTO INS ( account, ins_a_b_c, holder_nme, holder_dob, holder_sex, holder_addr, holder_city_st_zip, plan_nme, plan_addr1, plan_addr2, " +
                "p_city_st, policy_num, cert_ssn, grp_nme, grp_num, employer, e_city_st, fin_code, ins_code, relation, mod_date, mod_user, mod_prg, mod_host )" +
                "values ('{0}', '{1}', '{2}', {3}, '{4}', '{5}', '{6}', '{7}', '{8}', '{9}', " +
                "'{10}', '{11}', '{12}', '{13}', '{14}', '{15}', '{16}', '{17}', '{18}', '{19}', '{20}', '{21}', '{22}', '{23}' ) "
                , strAccReplace, dtIns.Rows[0]["ins_a_b_c"], dtIns.Rows[0]["holder_nme"]
                , string.IsNullOrEmpty(dtIns.Rows[0]["holder_dob"].ToString())? "NULL": string.Format("'{0}'",dtIns.Rows[0]["holder_dob"])
                , dtIns.Rows[0]["holder_sex"], dtIns.Rows[0]["holder_addr"], dtIns.Rows[0]["holder_city_st_zip"]
                , dtIns.Rows[0]["plan_nme"], dtIns.Rows[0]["plan_addr1"], dtIns.Rows[0]["plan_addr2"]
                , dtIns.Rows[0]["p_city_st"], dtIns.Rows[0]["policy_num"], dtIns.Rows[0]["cert_ssn"]
                , dtIns.Rows[0]["grp_nme"], dtIns.Rows[0]["grp_num"], dtIns.Rows[0]["employer"]
                , dtIns.Rows[0]["e_city_st"], dtIns.Rows[0]["fin_code"], dtIns.Rows[0]["ins_code"]
                , dtIns.Rows[0]["relation"], DateTime.Now.ToShortDateString()
                , Environment.UserName, propAppName, Environment.MachineName
                ), m_sqlConnection);

            int nCount = dtIns.Rows.Count;
            if (nCount > 1)
            {
                MessageBox.Show(string.Format("Account {0} has multiple insurances", strAccCurrent), propAppName);
            }
            sda.InsertCommand.Connection.Open();
            try
            {
                int nX = sda.InsertCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                m_Err.m_Logfile.WriteLogFile(se.Message);
            }
            finally
            {
                sda.InsertCommand.Connection.Close();
            }
        }

        private void InsertIntoPatTable(string strAccCurrent, string strAccReplace)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            DataTable dtPat = new DataTable();
            #region select
            sda.SelectCommand = new SqlCommand(
                string.Format("SELECT  account, ssn, pat_addr1, pat_addr2, city_st_zip, dob_yyyy, sex, relation, guarantor, guar_addr, g_city_st, pat_marital, icd9_1, " +
                "icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9, pc_code, mailer, first_dm, last_dm, min_amt, phy_id, dbill_date, ub_date, h1500_date, " +
                "ssi_batch, colltr_date, baddebt_date, batch_date, guar_phone, bd_list_date, ebill_batch_date, ebill_batch_1500, e_ub_demand, e_ub_demand_date, " +
                "claimsnet_1500_batch_date, claimsnet_ub_batch_date, mod_date, mod_user, mod_prg, mod_host, hne_epi_number " +
                "FROM         pat " +
                "where account = '{0}'", strAccCurrent),m_sqlConnection);
            sda.Fill(dtPat);

            #endregion select


            sda.InsertCommand = new SqlCommand(
                string.Format("INSERT INTO PAT ( account,       ssn,            pat_addr1,      pat_addr2,      city_st_zip, "+
                "dob_yyyy,      sex,            relation,       guarantor,      guar_addr, g_city_st,     pat_marital,    icd9_1,         icd9_2,         icd9_3, " +
                "icd9_4,        icd9_5,         icd9_6,         icd9_7,         icd9_8, icd9_9,        pc_code,        mailer,         first_dm,       last_dm, " +
                "min_amt,       phy_id,         dbill_date,     ub_date,        h1500_date, ssi_batch,     colltr_date,    baddebt_date,   batch_date,     guar_phone, " +
                "bd_list_date,  ebill_batch_date, ebill_batch_1500, e_ub_demand, e_ub_demand_date, claimsnet_1500_batch_date, claimsnet_ub_batch_date, mod_date, mod_user, mod_prg, " +
                "mod_host, hne_epi_number )" +
                "values ( '{0}', '{1}', '{2}', '{3}', '{4}', "+
                "'{5}', '{6}', '{7}', '{8}', '{9}', '{10}', '{11}', {12}, " +
                "{13}, {14}, {15}, {16}, {17}, {18}, {19}, {20}, '{21}', '{22}', {23}, {24}, '{25}', '{26}', {27}, {28}, {29}, " +
                "{30}, {31}, {32}, {33}, '{34}', {35}, {36}, {37}, '{38}', {39}, " +
                "{40}, {41}, '{42}', '{43}', '{44}', '{45}', '{46}'"+
                ")"
                ,strAccReplace
                    , dtPat.Rows[0]["ssn"]
                        , dtPat.Rows[0]["pat_addr1"].ToString().Replace("'","''")
                            , dtPat.Rows[0]["pat_addr2"].ToString().Replace("'", "''")
                                , dtPat.Rows[0]["city_st_zip"].ToString().Replace("'", "''")
                , dtPat.Rows[0]["dob_yyyy"], dtPat.Rows[0]["sex"], dtPat.Rows[0]["relation"]
                , dtPat.Rows[0]["guarantor"].ToString().Replace("'", "''"), dtPat.Rows[0]["guar_addr"].ToString().Replace("'", "''")
                , dtPat.Rows[0]["g_city_st"].ToString().Replace("'", "''"), dtPat.Rows[0]["pat_marital"]
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_1"].ToString()) ? "NULL" : string.Format("'{0}'",dtPat.Rows[0]["icd9_1"]) //{12}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_2"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_2"]) //{13}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_3"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_3"]) //{14}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_4"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_4"]) //{15}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_5"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_5"]) //{16}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_6"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_6"]) //{17}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_7"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_7"]) //{18}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_8"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_8"]) //{19}
                , string.IsNullOrEmpty(dtPat.Rows[0]["icd9_9"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["icd9_9"]) //{20}
                , dtPat.Rows[0]["pc_code"], dtPat.Rows[0]["mailer"]
                , string.IsNullOrEmpty(dtPat.Rows[0]["first_dm"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["first_dm"]) //23
                , string.IsNullOrEmpty(dtPat.Rows[0]["last_dm"].ToString()) ? "NULL" : string.Format("'{0}'", dtPat.Rows[0]["last_dm"]) // 24
                , dtPat.Rows[0]["min_amt"] // 25
            
                , dtPat.Rows[0]["phy_id"]
                , "NULL" //27
                , "NULL", "NULL"
                , "NULL"
              
                , "NULL", "NULL", "NULL"
                , dtPat.Rows[0]["guar_phone"] //34
                , "NULL"
              
                , "NULL" //dtPat.Rows[0]["ebill_batch_date"]
                , "NULL" //dtPat.Rows[0]["ebill_batch_1500"]
                , dtPat.Rows[0]["e_ub_demand"] //38
                , "NULL" //dtPat.Rows[0]["e_ub_demand_date"]
                , "NULL" //dtPat.Rows[0]["claimsnet_1500_batch_date"]
                , "NULL" //dtPat.Rows[0]["claimsnet_ub_batch_date"]
                , DateTime.Now.ToShortDateString()
              
               , Environment.UserName, propAppName, Environment.MachineName , dtPat.Rows[0]["hne_epi_number"]
            ), m_sqlConnection);

            sda.InsertCommand.Connection.Open();
            try
            {
                int nX = sda.InsertCommand.ExecuteNonQuery();
            }
            catch (SqlException se)
            {
                m_Err.m_Logfile.WriteLogFile(se.Message);
            }
            finally
            {
                sda.InsertCommand.Connection.Close();
            }
        }

        private int HasChargesRemaining(string strAccount)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            DataTable dtCharges = new DataTable();
            sda.SelectCommand = new SqlCommand(
                string.Format("select cdm, count(cdm) as [MoreCharges] " +
                "from chrg " +
                "where cdm <> 'CBILL' and credited = '0' " +
                "and account = '{0}' " +
                "group by cdm ", strAccount), m_sqlConnection);

            int nRemainingCharges = sda.Fill(dtCharges);
            return nRemainingCharges;
        }

 
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strCdm">cdm of the new charge</param>
        /// <param name="strAccount">the quest account to be used.</param>
        private void WriteQuestCharge(string strAccount, string strCdm)
        {
            //string strAccount = m_rAccQUE.m_strAccount.ToUpper().Replace("C", m_strBillType);

            if (m_rAccQUE.GetActiveRecords(string.Format("account = '{0}'", strAccount)) < 1)
            {
                // write the account first
                m_rAccQUE.ClearMemberVariables();
                m_rAccQUE.m_strAccount = strAccount;
                m_rAccQUE.m_strPatName = m_rAccOrig.m_strPatName;
                m_rAccQUE.m_strCliMnem = m_strBillType == "Q" ? "QUESTR" : "QUESTREF";
                m_rAccQUE.m_strFinCode = "Y";
                m_rAccQUE.propTransDate = m_rAccOrig.propTransDate;
                m_rAccQUE.m_strStatus = "QUEST"; // wdk 20130510 if we are adding one we want the status to be QUEST because the original account has already been checked
                //m_strBillType == "QR" ? "NEW" : 
                //m_rAccOrig.m_strStatus == "PAID_OUT" ? "QUEST" : m_rAccOrig.m_strStatus; // So Aging History will pay it out if 
                m_rAccQUE.m_strSsn = m_rAccOrig.m_strSsn;
                m_rAccQUE.m_strMeditechAccount = strAccount;
                m_rAccQUE.m_strOriginalFincode = m_rAccOrig.m_strOriginalFincode;
                m_rAccQUE.m_strMri = m_rAccOrig.m_strMri;
                //m_rAccQUE.m_strModDate = DateTime.Now.ToShortDateString();
                //m_rAccQUE.m_strModHost = Environment.MachineName;
                //m_rAccQUE.m_strModPrg = string.Format("{0} {1}", Application.ProductName, Application.ProductVersion);
                //m_rAccQUE.m_strModUser = Environment.UserName;

                int nRecAcc = m_rAccQUE.AddRecord();
                if (nRecAcc != 1)
                {
                    MessageBox.Show(string.Format("Account [{0}] not added", strAccount));
                }

                InsertIntoMergedTable(m_rAccOrig.m_strAccount, m_rAccQUE.m_strAccount);

            }
            else // wdk 20130510 update the status if the account already exists because we are adding charges this will let aging_history work correctly.
            {
                if (m_rAccQUE.m_strStatus != "QUEST")
                {
                    m_rAccQUE.m_strStatus = "QUEST";
                    m_rAccQUE.Update();
                }
            }

            double dDiscount = 0.00;
            // try to figure out a price
            m_rCliDisQUE.ClearMemberVariables();
            
            // apply the client discount
            m_rClient.GetActiveRecords(
                string.Format("cli_mnem = '{0}'", m_rAccQUE.m_strCliMnem));
            dDiscount = float.Parse(m_rClient.propPer_disc);

            int nRec = m_rCliDisQUE.GetActiveRecords(
                string.Format("cli_mnem = '{0}' and start_cdm = '{1}'", m_strBillType == "Q" ? "QUESTR":"QUESTREF", strCdm));
            if (nRec == 1)
            {
                if (string.IsNullOrEmpty(m_rCliDisQUE.propPrice))
                {
                    string strErr = string.Format("account {0} with cdm {1} has no price in client discount table. Sending Email to LIS.", m_rAccQUE.m_strAccount.ToUpper(), strCdm);
                    MessageBox.Show(strErr, propAppName);
                    m_Err.m_Email.Send(string.Format("{0}.QuestBilling@WTH.ORG", m_strProductionEnvironment), "carol.sellars@wth.org;jan.smith@wth.org;david.kelly@wth.org", "cdm missing price", strErr);
                   // Environment.Exit(13);
                    m_Err.m_Logfile.WriteLogFile(strErr);
                    return;
                }
                dDiscount = float.Parse(m_rCliDisQUE.propPercent_ds);
            }
            
            
            string strTable = "cpt4";
            if (m_rClient.propFee_schedule != "1")
            {
                strTable += string.Format("_{0}",m_rClient.propFee_schedule);
            }
            m_rCPT4 = new R_cpt4(m_strServer, m_strDatabase, ref m_Err, strTable);
            m_rCPT4.ClearMemberVariables();
            //int nRecCpt = m_rCPT4.GetRecordsByCdm(strCdm);
            int nRecCpt = m_rCPT4.GetRecordsByCdm(strCdm, false); // for QUEST we don't want to process the PC Charges
            
            double dNetTotal = 0.00;
            for (int i = 0; i < nRecCpt; i++)
            {
                dNetTotal +=
                    double.Parse(double.Parse(m_rCPT4.propCprice).ToString("F2"))
                    * ((100 - dDiscount) / 100);
                m_rCPT4.GetNext();
            }

            // write the charges
            m_rChrgQUE.ClearMemberVariables();
            m_rChrgQUE.m_strRowguid = Guid.NewGuid().ToString();
            m_rChrgQUE.propAccount = strAccount;
            m_rChrgQUE.propStatus = "NEW";
            m_rChrgQUE.propService_date = string.IsNullOrEmpty(m_rChrgCurrent.propService_date)? m_rAccQUE.propTransDate: m_rChrgCurrent.propService_date;
            m_rChrgQUE.propCdm = strCdm;
            m_rChrgQUE.propQty = m_rChrgCurrent.propQty;
            m_rChrgQUE.propRetail = m_rChrgCurrent.propRetail;
            m_rChrgQUE.propInp_price = m_rChrgCurrent.propInp_price;
            m_rChrgQUE.propComment = string.Format("moved from account [{0}]", m_rChrgCurrent.propAccount);
            m_rChrgQUE.propNet_amt = dNetTotal.ToString("F2");
            m_rChrgQUE.propFin_type = "C";
            m_rChrgQUE.propFinCode = "Y";
            string strWhere = string.Format("rowguid = '{0}'", m_rChrgQUE.m_strRowguid);
           //  this is failing on the two account in the list
            int nRecChrg = m_rChrgQUE.AddRecord(strWhere);
            if (nRecChrg == 1)
            {
                double dAmtTotal = 0.00;
                m_rCPT4.ClearMemberVariables();
                // wdk 20130425 added the false to not include the PC charge when writing the amount records.
                nRecCpt = m_rCPT4.GetRecordsByCdm(strCdm, false);
            
                for (int i = 0; i < nRecCpt; i++)
                {
                    // 20130122 we charge quest for the PC to pay to JPG
                    //if (m_rCPT4.propType == "PC")
                    //{
                    //    continue;
                    //}
                    dAmtTotal =
                        double.Parse(double.Parse(m_rCPT4.propCprice).ToString("F4"))
                        * ((100 - dDiscount) / 100);
                    
                    m_rChrgQUE.m_rsAmt.propChrg_num = m_rChrgQUE.propChrg_num;
                    m_rChrgQUE.m_rsAmt.propCpt4 = m_rCPT4.propCpt4;
                    m_rChrgQUE.m_rsAmt.propAmount = dAmtTotal.ToString("F4");
                    m_rChrgQUE.m_rsAmt.propType = m_rCPT4.propType;
                    m_rChrgQUE.m_rsAmt.propModi = m_rCPT4.propModi;
                    m_rChrgQUE.m_rsAmt.propRevcode = m_rCPT4.propRev_code;

                    int nRecAmt = m_rChrgQUE.m_rsAmt.AddRecord("1 = 2");
                    
                    if (nRecAmt != 0)
                    {
                        MessageBox.Show("Error in moving chrge to QUEST account", propAppName);
                    }
                    m_rCPT4.GetNext();
                }
                
            }
       }

        private void InsertIntoMergedTable(string strOrigAcc, string strDupAcc)
        {
            SqlDataAdapter sda = new SqlDataAdapter();
            DataTable dtMergedAcc = new DataTable();
            sda.SelectCommand = new SqlCommand(
                string.Format("insert into acc_merges (account, dup_acc, mod_prg) "+
                "values ('{0}','{1}','{2}')", strOrigAcc, strDupAcc, propAppName), m_sqlConnection);
     
            sda.SelectCommand.Connection.Open();
            int nX = sda.SelectCommand.ExecuteNonQuery();
            sda.SelectCommand.Connection.Close();
           
            
        }

        private void tsbPrintView_Click(object sender, EventArgs e)
        {
            Bitmap[] bmps = RFClassLibrary.dkPrint.Capture(dkPrint.CaptureType.Form);
            try
            {
                bmps[0].Save(@"C:\Temp\ViewerQuest.bmp");
            }
            catch (System.Runtime.InteropServices.ExternalException ex)
            {
                MessageBox.Show(ex.Message, propAppName);
                return;
            }
            try
            {
                RFClassLibrary.dkPrint.propStreamToPrint = new StreamReader(@"C:\Temp\ViewerQuest.bmp");
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message, propAppName);
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
        /// <summary>
        /// This should be only tests we don't perform here. Except for the two venipunctue that are 
        /// included in this cdm range.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiReference_Click(object sender, EventArgs e)
        {
            tsmiQuestRef.Enabled = false;
            
            dgvRecords.DataSource = null;
            m_strBillType = "QR";
            tscbBillingType.SelectedIndex = tscbBillingType.ComboBox.FindStringExact(m_strBillType);
            tsslBillType.Text = 
                string.Format("BILLING TYPE: {0}",((ToolStripMenuItem)sender).Tag.ToString());

            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable dtTests = new DataTable();

                SqlCommand cmdSelect = new SqlCommand(
                    string.Format(
                        "select chrg.account, chrg.chrg_num " +
                        ", chrg.cdm " +
                        ", sum(qty) over (partition by chrg.account, chrg.cdm) as [QTY] " +
                        ", convert(datetime,convert(varchar(10),service_date,101)) as [DOS] " +
                        ", 'QUEST' as [BILL TYPE] " +
                        "from chrg " +
                        "where chrg.account in ( " +
                        "	select colAccount from ufn_quest_processing('{0} 00:00','{1} 23:59:59') where colStatus = 'QUEST')" +
                            //"	select account from acc where status = 'QUEST' and trans_date between '{0} 00:00' and '{1} 23:59:59' "+
                            //"   and fin_code = 'D' ) " +
                        "and credited = 0 and (invoice is null or invoice = '') " +
                        //"and chrg.cdm in (select cdm from dict_outreach_supplies where deleted = 0 ) " +
                        // wdk 20140311 undo the previously replace methodology
                        // wdk 20140212 replace with line above to use new methodology
                        "and chrg.cdm in (select cdm from cdm where cdm between '5520000' and '5527417' or cdm between '5527420' and '552ZZZZ' ) " +
                        " order by chrg.account,chrg.cdm ",
                        m_dtFrom.ToString("d"), m_dtThru.ToString("d")), conn);
                sda.SelectCommand = cmdSelect;
                
                int nRec = sda.Fill(dtTests);
                if (nRec == 0)
                {
                    tsmiMCLRef.Enabled = true;
                    MessageBox.Show("QUEST REFERENCE LAB\r\nNo records to post", propAppName);
                    return;
                }
                dgvRecords.DataSource = dtTests;
                dgvRecords.Refresh();

                if (m_bIncludeCode)
                {
                    tsb360_Click(null, null);

                    BillAsQuestRef();
                }


            }

            
            MessageBox.Show("QUEST REFERENCE LAB\r\nPosting completed",propAppName);
            tsmiMCLRef.Enabled = true;

        }

        private void BillAsQuestRef()
        {
           // dgvRecords.DataSource = null;
            m_strBillType = "QR";
            tscbBillingType.SelectedIndex = tscbBillingType.ComboBox.FindStringExact(m_strBillType);
            tsslBillType.Text = "QUEST REFERENCE LAB";
            SqlTransaction transChrg;
                
            foreach (DataGridViewRow drCharges in dgvRecords.Rows)
            {
                string strAccOrig = drCharges.Cells["account"].FormattedValue.ToString();
                string strAcc = drCharges.Cells["account"].FormattedValue.ToString().Replace("C", m_strBillType);
                string strChrgNum = drCharges.Cells["chrg_num"].FormattedValue.ToString();
                
                string strChrgOrig = string.Format("account = '{0}' and chrg_num = '{1}'",
                            strAccOrig, strChrgNum);
                m_rChrgCurrent.ClearMemberVariables();
                int nRecChrg = m_rChrgCurrent.GetRecords(strChrgOrig);
                if (m_rChrgCurrent.GetRecords(strChrgOrig) != 1)
                {
                    drCharges.ErrorText = "Something really bad just happened.";
                    return;
                }

                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    DataTable dtCharge = new DataTable();
                    DataTable dtAmt = new DataTable();
                    // find the charge numbers that have not been credited or reversed.
                    SqlCommand cmdSelectCharge = new SqlCommand(
                        string.Format(
                            "select account, chrg_num " +
                            ", cdm, qty, convert(datetime,convert(varchar(10),service_date,101)) as [DOS] " +
                            ", credited, status, retail,inp_price,mt_req_no,fin_type,fin_code, rowguid "+
                            ", net_amt, comment "+
                            "from chrg " +
                            "where chrg_num = {0} ",drCharges.Cells["chrg_num"].FormattedValue.ToString()
                            )
                            , conn);
                    sda.SelectCommand = cmdSelectCharge;

                    SqlCommand cmdChargeUpdate = new SqlCommand(
                        string.Format("update chrg " +
                        "set credited = 1 " +
                        "where chrg_num = {0}", drCharges.Cells["chrg_num"].FormattedValue.ToString()
                        )
                        , conn);
                    SqlCommand cmdChrgWrite = new SqlCommand(
                        string.Format("INSERT INTO chrg " +
                        "(credited, account, status, service_date, hist_date, cdm, qty, retail, inp_price, comment,  " +
                        "invoice, net_amt, fin_type, mt_req_no, fin_code,  performing_site) " +
                        "SELECT     credited, account, status, service_date, hist_date, cdm, qty*-1, retail, inp_price,  " +
                        "'Performed and billed by Quest' " +
                        ", invoice, net_amt, fin_type, mt_req_no, fin_code,  " +
                        "performing_site " +
                        "FROM         chrg AS chrg_1  " +
                        "WHERE     (chrg_num = '{0}') ", drCharges.Cells["chrg_num"].FormattedValue.ToString()), conn);
                    SqlCommand cmdAmtWrite = new SqlCommand(
                        string.Format("; with cte "+
                        "as "+
                        "( "+
                        "select max(chrg_num) as [cnum] from chrg where account in (select account from chrg where chrg_num = '{0}') "+
                        ") "+
                        "INSERT INTO amt (chrg_num, cpt4, type, amount, deleted, modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, bill_type, bill_method, mod_prg) "+
                        "SELECT     cte.cnum,cpt4, type, amount, deleted, modi, revcode, modi2, diagnosis_code_ptr, mt_req_no, order_code, bill_type, bill_method, 'SQL Query' "+
                        "FROM         amt AS amt_1 "+
                        "cross join cte  "+
                        "WHERE     (chrg_num = '{0}')", drCharges.Cells["chrg_num"].FormattedValue.ToString(),conn));

                    conn.Open();
                    transChrg = conn.BeginTransaction(System.Data.IsolationLevel.ReadUncommitted);
                    try
                    {

                        int nRec = new SqlCommand(cmdChargeUpdate.CommandText, conn, transChrg).ExecuteNonQuery();
                        nRec = new SqlCommand(cmdChrgWrite.CommandText, conn, transChrg).ExecuteNonQuery();
                        nRec = new SqlCommand(cmdAmtWrite.CommandText, conn, transChrg).ExecuteNonQuery();
                        // wdk 20130919 credit the charge and write make it zero
                        //WriteQuestCharge(strAcc, drCharges.Cells["cdm"].FormattedValue.ToString());
                        transChrg.Commit();
                    }
                    catch (SqlException se)
                    {
                        transChrg.Rollback();
                        drCharges.ErrorText = se.Message;// "charges not moved.";
                    }
                    catch (Exception)
                    {
                        transChrg.Rollback();
                        drCharges.ErrorText = "Charges not moved.";
                    }

                    finally
                    {
                        conn.Close();
                    }

                    // add the new reversal charge.
                    //string strRowguidChrgNew = Guid.NewGuid().ToString();

                    //int nRecFill = sda.Fill(dtCharge);
                    //if (nRecFill == 0)
                    //{
                    //    tsmiMCLRef.Enabled = true;
                    //    MessageBox.Show("QUEST REFERENCE LAB\r\nNo records to post",propAppName);
                    //    return;
                    //}
                   
                }
            }
        }

        private void LoadStatusBar()
        {
            
           // double dAmt = 0.00;
            int nQty = 0;
            int nAccCount = 0;
            string strOldAcct = "";
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
           //     dAmt += int.Parse(dr.Cells["qty"].Value.ToString()) * double.Parse(dr.Cells["charges"].Value.ToString());
                try
                {
                    nQty += int.Parse(dr.Cells["qty"].Value.ToString());
                }
                catch (Exception)
                {
                    // Exception billing has no qty's
                }
                if (strOldAcct != dr.Cells["account"].Value.ToString())
                {
                    nAccCount++;
                    strOldAcct = dr.Cells["account"].Value.ToString();
                }
            }

            tsslRecords.Text = string.Format("{0} Records for {1} Accounts", dgvRecords.Rows.Count, nAccCount);
            tsslAmount.Text = string.Format("Totals: QTY {0} ", nQty);//, dAmt); // CHARGES {1:C2}
            dgvRecords.Invalidate();
            this.Invalidate();
        }

        /// <summary>
        /// theory is all the reference labs for Chantilly will already be moved to the QuestRef client 
        /// before running this step which should load all GAP list tests to be performed here and billed
        /// the QuestR account.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsmiMCLRef_Click(object sender, EventArgs e)
        {
            dgvRecords.DataSource = null;
            m_strBillType = "Q";
            tscbBillingType.SelectedIndex = tscbBillingType.ComboBox.FindStringExact(m_strBillType);
            tsslBillType.Text =
                string.Format("BILLING TYPE: {0}", ((ToolStripMenuItem)sender).Tag.ToString());

            m_dsQUE = new DataSet();
            using (SqlConnection conn =
            new SqlConnection(
                string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                m_strServer, m_strDatabase)))
            {

                SqlDataAdapter sda = new SqlDataAdapter();             
                
                sda.SelectCommand =
                new SqlCommand(SelectQuery(), conn);

                DateTime dtWait = DateTime.Now;
                try
                {
                    int nRec = sda.Fill(m_dsQUE);
                }
                catch (SqlException se)
                {
                    m_Err.m_Logfile.WriteLogFile(se.Message);
                    int n = 0;
                    while (DateTime.Now < dtWait.AddSeconds(10))
                    {
                        n += n++; // just hum a bit
                    }

                    MessageBox.Show(string.Format("{1} - {0}",se.Message, "SQL EXCEPTION DURING LOAD"),propAppName);
                }
                try
                {
                    dgvRecords.DataSource = m_dsQUE.Tables[0];
                }
                catch (IndexOutOfRangeException)
                {
                    return;
                }


                
            }
            // get the rows that have no quest code
            DataRow[] drDrop = m_dsQUE.Tables[0].Select("[Quest Code] is null");
            // put the accounts into a searchable object
            ArrayList alDrop = new ArrayList();
            foreach (DataRow dr in drDrop)
            {
                Application.DoEvents();
                if (!alDrop.Contains(dr["account"].ToString()))
                {
                    alDrop.Add(dr["account"].ToString());
                }

            }

            string strAcc = null;
            string strCdm = null;
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                Application.DoEvents();
                if (strCdm != dr.Cells["cdm"].Value.ToString())
                {
                    strCdm = dr.Cells["cdm"].Value.ToString();
                }
                else
                {
                    if (strAcc == dr.Cells["account"].Value.ToString())
                    {
                        dr.Cells["CDM"].ErrorText = "DUPLICATE CDM";
                      //  ((DataRowView)dr.DataBoundItem).Row.RowError += "DUPLICATE CDM";
                    }
                }
                if (strAcc != dr.Cells["account"].Value.ToString())
                {
                    strAcc = dr.Cells["account"].Value.ToString();
                    strCdm = dr.Cells["cdm"].Value.ToString();
                }
                

                if (alDrop.Contains(dr.Cells["account"].Value.ToString()))
                {
                    ((DataRowView)dr.DataBoundItem).Row.RowError += "DO NOT PROCESS -- NO VALID CODE\r\n";
                }
                //if (DateTime.Parse(dr.Cells["DOS"].Value.ToString()).AddDays(90) < DateTime.Today )
                //{
                //    ((DataRowView)dr.DataBoundItem).Row.RowError += "DO NOT PROCESS -- TOO OLD\r\n";
                //}

            }
            //IEnumerable<DataRow> queryNull = from account in m_dsQUE.Tables[0].AsEnumerable()
            //                                 where account.Field<string>("Quest Code") == null
            //                                 select account;
            //var accounts = queryNull.Intersect(m_dsQUE.Tables[0].AsEnumerable(), DataRowComparer.Default);
          


            dgvRecords.Refresh();
            dgvRecords.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadStatusBar();
            tsmiBillMCL.Enabled = dgvRecords.Rows.Count >= 1 ? true : false;
            MessageBox.Show("Load Completed", propAppName);
            tsmiBillMCL.Enabled = true;
            
        }

        /// <summary>
        /// 20140327 wdk only for GAP tests now use SelectQueryExclusion() for Exclusion
        /// Pass either GAP or EXCLUSION to this function to get the list desired for processing
        /// </summary>
        /// <returns></returns>
        private string SelectQuery()
        {
            /*
             /*    
                "case when outpatient_billing = 1 and trans_date >= '04/01/2012 00:00:00' "+
                    "then 'UBOP' "+
                    "else '1500' "+
                    "end as [SSI type] "+
             */
            
            string strQuery =
                string.Format("with cteAcc as ( " +
                    "SELECT colStatus AS [status],colAccount AS [account], "+
                    "colAge AS [Age], CASE WHEN colBillingtype = 'OUTPATIENT' "+
                    "THEN 'UBOP'  "+
                    "ELSE '1500' "+
                    "END AS [SSI type] "+
                    ",colFinCode "+
                    "FROM ufn_quest_processing('{0} 00:00' ,'{1} 23:59:59' ) "+
                    "WHERE colStatus = 'QUEST' ) " +
                     ", cteChrg as ( " +
                     "select cteAcc.status, chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age] " +
                     ", convert(datetime,convert(varchar(10),service_date,101)) as [DOS] " +
                     ", amt.uri " +
                     ", [SSI type]   "+
                     "from chrg " +
                     "inner join cteAcc on cteAcc.account = chrg.account " +
                     "inner join amt on amt.chrg_num = chrg.chrg_num " +
                     "where credited = 0 and (invoice is null or invoice = '') " +
                     "and not chrg.cdm in (select cdm from cdm where " +
                     "(cdm between '5520000' and '5527417' " +
                     "or cdm between '5527420' and '552ZZZZ')" +
                     ")) " +
                     "select distinct cteChrg.status, cteChrg.account, cteChrg.chrg_num, cteChrg.cdm, cteChrg.cpt4 " +
                     ", cteChrg.qty, cteChrg.dos " +
                     ", [Age] " +
                     ",case when dd.cpt is null " +
                     "then 'GAP' " +
                     " else case when (age > 11 and age_appropriate = 1) " +
                     " then 'GAP' else " +
                     "'EXCLUSION' " +
                     " end " +
                     "end as [Bill Type] " +
                     ", coalesce(dt.quest_code,dt2.quest_code) as [Quest Code] " +
                     ", coalesce(dt.quest_description,dt2.quest_description) as [Quest Description] " +
                     ", cteChrg.uri as [amt_uri]" +
                     ", [SSI type] "+
                     "from cteChrg " +
                     "left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0 " +
                     " and (cteChrg.dos >= dd.start_date  and cteChrg.dos <= isnull(dd.expire_date,getdate())) " + // wdk 20130522 changes for new Exclusion list items.
                     "left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                     "and (cteChrg.dos >= dt.start_date  " +
                      "and cteChrg.dos <= isnull(dt.expire_date,getdate())) " +
                     "left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                     "and (cteChrg.dos >= dt2.start_date  " +
                     "and cteChrg.dos <= isnull(dt2.expire_date,getdate())) " +
                     "where case when dd.cpt is null then 'GAP' " +
                     " else case when (age > 11 and age_appropriate = 1) " +
                     " then 'GAP' else 'EXCLUSION' end end = 'GAP' " +
                     "order by [DOS],cteChrg.status, account, cdm, cteChrg.cpt4 ",
                     m_dtFrom.ToString("d"), m_dtThru.ToString("d")
                     );

            return strQuery;
        }

        /// <summary>
        ///  EXCLUSION list with all GAP accounts removed. Gap must process before this is called
        ///  in order to remove the GAP cpt's from the Exclusion list
        /// </summary>
        /// <returns></returns>
        private string SelectQueryExclusion()
        {
            string strQuery =
                string.Format(";with cteAcc as "+ 
                "(  "+ 
                "	SELECT colStatus AS [status],colAccount AS [account], colAge AS [Age] "+ 
                "		, CASE WHEN colBillingtype = 'OUTPATIENT' THEN 'UBOP'  ELSE '1500' END AS [SSI type]  "+ 
                "		,colFinCode  "+ 
                "	FROM ufn_quest_processing('{0} 00:00' ,'{1} 23:59:59' )  "+ 
                "	WHERE colStatus = 'QUEST' "+ 
                ")  "+ 
                ", cteChrg as  "+ 
                "(  "+ 
                "	select cteAcc.status, chrg.account, chrg.chrg_num, qty, cdm  "+ 
                "		, cpt4, [Age] , convert(datetime,convert(varchar(10),service_date,101)) as [DOS]  "+ 
                "		, amt.uri , [SSI type] "+ 
                "	from chrg  "+ 
                "	inner join cteAcc on cteAcc.account = chrg.account  "+ 
                "	inner join amt on amt.chrg_num = chrg.chrg_num  "+ 
                "	where credited = 0 and (invoice is null or invoice = '')  "+ 
                "	and not chrg.cdm in  "+ 
                "		(select cdm from cdm where (cdm between '5520000' and '5527417' or cdm between '5527420' and '552ZZZZ')) "+ 
                ")  "+ 
                ",cteGap AS  "+ 
                "( "+ 
                "select DISTINCT  "+ 
                "	TOP(100) PERCENT  "+ 
                "	cteChrg.status, cteChrg.account, cteChrg.chrg_num, cteChrg.cdm "+ 
                "	, cteChrg.cpt4 , cteChrg.qty, cteChrg.dos , [Age]  "+ 
                "	,case when dd.cpt is null then 'GAP'  else  "+ 
                "		case when (age > 11 and age_appropriate = 1)   "+ 
                "		then 'GAP' else 'EXCLUSION'  end end as [Bill Type]  "+ 
                "	, coalesce(dt.quest_code,dt2.quest_code) as [Quest Code]  "+ 
                "	, coalesce(dt.quest_description,dt2.quest_description) as [Quest Description]  "+ 
                "	, cteChrg.uri as [amt_uri], [SSI type]  "+ 
                "from cteChrg  "+ 
                "left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0  and (cteChrg.dos >= dd.start_date  and cteChrg.dos <= isnull(dd.expire_date,getdate()))  "+ 
                "left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 and (cteChrg.dos >= dt.start_date  and cteChrg.dos <= isnull(dt.expire_date,getdate()))  "+ 
                "left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 and (cteChrg.dos >= dt2.start_date  and cteChrg.dos <= isnull(dt2.expire_date,getdate()))  "+ 
                ") "+ 
                ",cteExclusion AS  "+ 
                "( "+ 
                "	SELECT * FROM cteGap WHERE [Bill Type] = 'EXCLUSION' "+ 
                "	AND  not account IN (	SELECT account FROM cteGap WHERE [Bill Type] = 'GAP') "+ 
                ") "+ 
                "SELECT  status , "+ 
                "        account , "+ 
                "        chrg_num , "+ 
                "        cdm , "+ 
                "        cpt4 , "+ 
                "        qty , "+ 
                "        DOS , "+ 
                "        Age , "+ 
                "        [Bill Type] , "+ 
                "        [Quest Code] , "+ 
                "        [Quest Description] , "+ 
                "        amt_uri , "+ 
                "        [SSI type] FROM cteExclusion ", 
                 m_dtFrom.ToString("d"), m_dtThru.ToString("d"));  
                
            return strQuery;
        }


        private void tsb360_Click(object sender, EventArgs e)
        {
            m_ds360 = new DataSet();
            m_ds360.Tables.Add("ACC");
            m_ds360.Tables.Add("PAT");
            m_ds360.Tables.Add("INS");
            m_ds360.Tables.Add("CHRG_CLINICAL");
            m_ds360.Tables.Add("CHRG_CYTO_HISTO"); // wdk 20121218 modification
            string strAccOld = null;

            foreach (DataGridViewRow drAcc in dgvRecords.Rows)
            {
                string strAcc = drAcc.Cells["account"].Value.ToString();
                if (strAcc == strAccOld)
                {
                    continue;
                }
                strAccOld = strAcc;

                m_ds360.Tables["CHRG_CLINICAL"].Rows.Clear();
                m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Clear();
                m_ds360.Tables["INS"].Rows.Clear();
                m_ds360.Tables["PAT"].Rows.Clear();
                m_ds360.Tables["ACC"].Rows.Clear();

                // using the current row fill the tables
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    DataTable dtAcc = new DataTable();
                    #region data_quest_360
                    string strSelect360 =
                        string.Format("select account, date_of_service, bill_type " +
                        "from data_quest_360 " +
                        "where deleted = 0 " +
                        "and account = '{0}' and bill_type = '{1}' ", strAcc, m_strBillType);
                    sda.SelectCommand =
                           new SqlCommand(strSelect360, conn);
                    int nRec360 = sda.Fill(dtAcc);
                    if (nRec360 > 0)
                    {
                        if (dtAcc.Select("bill_type = 'QR'") != null)
                        {
                            return;
                        }
                    }

                    #endregion data_quest_360


                    #region ACCOUNT
                    SqlCommand cmdAccSelect = new SqlCommand(
                        string.Format("select  " +
                        "	account, " +
                        " substring(pat_name, 0, charindex(',',pat_name,0)) as [lastname], " +
                        "	substring(pat_name, " +
                        "	charindex(',',pat_name,0)+1 " +
                        " , case when charindex(' ',pat_name,charindex(',',pat_name,0)-1) = 0 " +
                        "then len(pat_name) - charindex(',',pat_name,0) " +
                        "else  " +
                        " charindex(' ',pat_name,charindex(',',pat_name,0)+1) - " +
                        " charindex(',',pat_name,0) " +
                        "	end) as [firstname] " +
                        "	,	substring(pat_name,  " +
                        "	charindex(' ',pat_name, charindex(',',pat_name,0)+1)+1 " +
                        " 	, case when charindex(' ',pat_name,charindex(',',pat_name,0)+1) = 0 " +
                        " then 0 " +
                        " else  charindex(' ',pat_name,charindex(',',pat_name,0)+1)			" +
                        " end)	as [midname]" +
                        ", cl_mnem " + //, fin_code
                        "	, convert(varchar(10),trans_date,101) as [trans_date] " +
                        " ,  stuff(stuff(ssn,4,0,'-'),7,0,'-') as [ssn] " +
                        " from acc " +
                        " where acc.account = '{0}' ", strAcc)
                        , conn);
                    sda.SelectCommand = cmdAccSelect;
                    int nAccRec = sda.Fill(m_ds360.Tables["ACC"]);

                    m_ds360.Tables["ACC"].PrimaryKey =
                        new DataColumn[] { m_ds360.Tables["ACC"].Columns["ACCOUNT"] };

                    #endregion ACCOUNT
                    #region PAT
                    SqlCommand cmdPatSelect = new SqlCommand(
                       string.Format("select " +
                       " pat.account " +
                       " ,trans_date " +
                       ", pat_addr1, pat_addr2, city_st_zip " +
                       ", convert(varchar(10),dob_yyyy,101) as [DOB], sex, relation " +
                       ", guarantor, guar_addr, g_city_st" +
                       ", guar_phone, pat_marital  " +
                       ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
                       ", phy.last_name +','+ phy.first_name +' '+ isnull(phy.mid_init,'') +' ('+phy_id +')' as [phy_id] " +
                       "  from pat " +
                       "inner join acc on acc.account = pat.account " +
                       "left outer join phy on phy.tnh_num = pat.phy_id " +
                       " where pat.account = '{0}'", strAcc)
                       , conn);

                    sda.SelectCommand = cmdPatSelect;
                    int nPatRec = sda.Fill(m_ds360.Tables["PAT"]);

                    //if (!m_ds360.Relations.Contains("AccPat"))
                    //{
                    //    DataRelation drAccPat = m_ds360.Relations.Add(
                    //        "AccPat", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                    //        m_ds360.Tables["PAT"].Columns["ACCOUNT"]);
                    //}

                    #endregion PAT
                    #region INS

                    SqlCommand cmdInsSelect = new SqlCommand(
                       string.Format("select " +
                       "	ins.account, ins_a_b_c, holder_nme, holder_dob, holder_sex, holder_addr, holder_city_st_zip, plan_nme, plan_addr1, plan_addr2, " +
                       "    p_city_st, policy_num, cert_ssn, grp_nme, grp_num, employer, e_city_st " +
                       "	, ins_code, relation " +
                       " from ins " +
                       " where account = '{0}' and deleted = 0 ", strAcc)
                       , conn);

                    sda.SelectCommand = cmdInsSelect;
                    int nInsRec = sda.Fill(m_ds360.Tables["INS"]);

                    //if (!m_ds360.Relations.Contains("AccIns"))
                    //{
                    //    DataRelation drAccPat = m_ds360.Relations.Add(
                    //        "AccIns", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                    //        m_ds360.Tables["INS"].Columns["ACCOUNT"]);
                    //}

                    #endregion INS


                    #region account charges
                    string strSelectChrgNonCytoHysto =
                        string.Format("select account, qty, chrg.cdm, " +
                        " coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
                        "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
                        "from chrg " +
                        "left outer join dict_quest_reference_lab_tests dt " +
                        "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                        "left outer join dict_quest_reference_lab_tests dt2 " +
                        "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                        "where account = '{0}' " +
                        "and ((not chrg.cdm between '5920000' and '594ZZZZ') and chrg.cdm <> 'CBILL' " +
                        "and chrg.cdm <> '5686046') " + // wdk 20130327 added to move HPV to CytoHisto for Quest
                        "order by convert(numeric(18),coalesce(dt.quest_code,dt2.quest_code)) " // wdk 20130327 added to aid tracking in care360
                        , strAcc);

                    sda.SelectCommand =
               new SqlCommand(strSelectChrgNonCytoHysto, conn);

                    DateTime dtWait = DateTime.Now;
                    try
                    {
                        int nChrgRec = sda.Fill(m_ds360.Tables["CHRG_CLINICAL"]);
                    }
                    catch (SqlException se)
                    {
                        m_Err.m_Logfile.WriteLogFile(se.Message);
                        
                        MessageBox.Show(string.Format("{1} - {0}",se.Message, "SQL EXCEPTION DURING LOAD"),propAppName);
                    }
                
                    #endregion account charges

                    #region charges cyto_hysto
                    sda.SelectCommand =
                        new SqlCommand(
                        string.Format("select account, qty, chrg.cdm, " +
                        "coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
                        "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
                        "from chrg " +
                        "left outer join dict_quest_reference_lab_tests dt " +
                        "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                        "left outer join dict_quest_reference_lab_tests dt2 " +
                        "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                        "where account = '{0}' " +
                        "and ((chrg.cdm between '5920000' and '594ZZZZ') " +
                        "or (chrg.cdm in ('5686046'))) " + //wdk 20130327 added to move hpv to cyto\histo sheet.
                        "order by convert(numeric(18),coalesce(dt.quest_code,dt2.quest_code))" // wdk 20130327 added to aid tracking in Care360 
                        , strAcc),
                    conn);

                    dtWait = DateTime.Now;
                    try
                    {
                        int nChrgCytoHystoRec = sda.Fill(m_ds360.Tables["CHRG_CYTO_HISTO"]);
                    }
                    catch (SqlException se)
                    {
                        m_Err.m_Logfile.WriteLogFile(se.Message);
                        int n = 0;
                        while (DateTime.Now < dtWait.AddSeconds(10))
                        {
                            n += n++; // just hum a bit
                        }

                        MessageBox.Show(string.Format("{1}\r\r\n{0}",se.Message, "SQL EXCEPTION DURING LOAD"),propAppName);
                    }
                    m_ds360.Tables["CHRG_CYTO_HISTO"].PrimaryKey = null;
             
                    #endregion charges cyto_hysto
                }

                if (m_ds360.Tables["CHRG_CLINICAL"].Rows.Count > 0)
                {
                    CreateHTML(false);
                }
                if (m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Count > 0)
                {
                    CreateHTML(true); // for cyto/histo 
                }
            }
         
                    

        }

        private void InsertIntoData360(string strAcc, string strDOS, string strErrorMsg)
        {
          
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlCommand cmdInsert = new SqlCommand(string.Format(
                    "INSERT INTO data_quest_360 " +
                    "(patid, html_doc, account, date_of_service, pre360_error, bill_code_error, " +
                    " bill_type, " +
                    " mod_date, mod_user, mod_prg, mod_host) " +
                    "VALUES " +
                    "    ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}')",
                    Guid.NewGuid().ToString(), // patid 0
                       strErrorMsg.Replace("'", "''"), // html_doc 1
                        strAcc, // account 2
                        strDOS, // date of service 3
                        1, // pre360_error = 1 {4}
                        0,  //(bill_code_error = 0 this should be set from Viewer360) {5}
                        m_strBillType, //bill_type 6
                        DateTime.Now, // 7
                        Environment.UserName, // 8
                        Application.ProductName + Application.ProductVersion, // 9
                        Environment.MachineName // 10
                        ), conn);

                cmdInsert.Connection.Open();
                try
                {
                    int nSert = cmdInsert.ExecuteNonQuery();
                }
                catch (SqlException se)
                {
                    m_Err.m_Logfile.WriteLogFile(se.Message);
                }
                finally
                {
                    cmdInsert.Connection.Close();
                }
            }
        }

        private void CreateHTML(bool bCytoHisto)
        {
            if (string.IsNullOrEmpty(m_strBillType))
            {
                m_strBillType = "Q";
            }
            m_dicPatient = new Dictionary<string, string>();
            string strPatID = "";
                       
            foreach (DataRow dr in m_ds360.Tables["ACC"].Rows)
            {
                if (dr["ACCOUNT"].ToString().StartsWith("QR"))
                {
                    m_strBillType = "QR";
                }
                try
                {
                    strPatID = string.Format("{0}{1}{2}{3}{4}{5}{8}|{6}|{7}",
                                            dr["LASTNAME"].ToString().Trim(), //0
                                            dr["FIRSTNAME"].ToString().Trim(), //1
                                            dr["MIDNAME"].ToString().Trim(), //2
                                            string.Format("{0,8:yyyyMMdd}", //3
                                                DateTime.Parse(m_ds360.Tables["PAT"].Rows[0]["DOB"].ToString())),
                                            m_ds360.Tables["PAT"].Rows[0]["SEX"].ToString().Trim(), //4
                                            dr["SSN"].ToString().Trim(), //5
                                            dr["ACCOUNT"], //6
                                            dr["trans_date"], //7
                                            string.Format("[CH{0}]", bCytoHisto) //8
                                            );
                }
                catch (Exception ex)
                {
                    MessageBox.Show(
                        string.Format("{1}\r\n\n{0}",string.Format("{0}\r\n\r{1}", ex.GetType().ToString(), ex.Message), dr["ACCOUNT"].ToString()),propAppName);
                    return;
                }
               
                m_wbPrint = new WebBrowser();
                m_wbPrint.AllowNavigation = false;
                m_wbPrint.Navigate(new Uri("about:blank"));

                m_htmDoc = m_wbPrint.Document;
                m_htmDoc.OpenNew(false);
                m_htmDoc.Write("<HTML>");
                m_htmDoc.Write("<BODY MARGIN-LEFT: .5IN MARGIN-RIGHT: .5IN  MARGIN-TOP: = .25 MARGIN-BOTTOM = .5>");
                StringBuilder sb = new StringBuilder();
                m_htmDoc.Title = "360 Billing";
                sb.AppendLine("<IMG SRC= \"file://C:/Program Files/Medical Center Laboratory/MCL Billing/mcllogo.bmp\">");

                sb.AppendLine("<BR>");
                //if (m_strBillType.StartsWith("Q"))
                if (dr["ACCOUNT"].ToString().StartsWith("QR") || m_strBillType == "QR")
                {
                    sb.AppendFormat("<HR><P ALIGN=CENTER><B>BILLING TYPE: {0}</B></P><HR>", "CHANTILLY");
                }
                else
                {
                    sb.AppendFormat("<HR><P ALIGN=CENTER><B>BILLING TYPE: {0}</B></P><HR>", "ATLANTA");
                }
                #region TablePatient
                // table with a table
                sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #0000FF><TD COLSPAN = 3><B>PATIENT DATA</B></TD></TR>");

                sb.AppendLine("<TR>");

    // PATIENT TABLE
                sb.AppendLine("<TD WIDTH =300>");
                sb.AppendLine("<TABLE WIDTH = 300 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>PATIENT</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 100><B>Last Name</B></td>");
                
                sb.AppendFormat("<td width = 200>{0}</td>", dr["LASTNAME"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>First Name</B></td>");
                
                sb.AppendFormat("<td>{0}</td>", dr["FIRSTNAME"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Middle Init</B></td>");
                
                sb.AppendFormat("<td>{0}</td>", dr["MIDNAME"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>BIRTH DATE</B></TD>");
                
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["DOB"]);//drPat[0]["DOB"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>GENDER</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["sex"]);//drPat[0]["sex"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>SSN</B></TD>");
                sb.AppendFormat("<td>{0}</td>", dr["ssn"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("</TABLE>");
              
                sb.AppendLine("</TD>");
                //// end of PATIENT table
     //// add table here
                sb.AppendLine("<TD WIDTH =300>");
                sb.AppendLine("<TABLE WIDTH = 300 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>ADMINISTRATIVE DETAILS</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD WIDTH = 100><B>Primary Provider</B></TD>");
                sb.AppendFormat("<td WIDTH = 200>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["phy_id"]);//drPat[0]["phy_id"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>LAB REF ID</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["account"]);//drPat[0]["account"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Health ID</B></TD>");
                /* wdk 20121204 removed and leave blank
                DataRow dr360 = 
                    m_ds360.Tables["360"].Rows.Find(new object[] {
                        strPatID.Substring(0,strPatID.IndexOf('|')), 
                        drPat[0]["account"],
                        m_strBillType });
                sb.AppendFormat("<td>{0}</td>", dr360 == null ? "":dr360["patid"]);
                 */
                sb.AppendFormat("<td>{0}</td>", "" );
                sb.AppendLine("</TR>");

                
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>DOS</B></TD>");
                sb.AppendFormat("<TD>{0}</TD>", //DateTime.Parse(drPat[0]["trans_date"].ToString()).ToShortDateString());
                    DateTime.Parse(m_ds360.Tables["PAT"].Rows[0]["trans_date"].ToString()).ToShortDateString());//
                sb.AppendLine("</TR>");
                
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>RELATION</B></TD>");
                sb.AppendFormat("<TD>{0}</TD>", ConvertRelation(m_ds360.Tables["PAT"].Rows[0]["relation"]));//drPat[0]["relation"]));

                // line fillers
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD></TD>");
                sb.AppendLine("</TR>");

                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                // end of new table
   
 //// add third table
                sb.AppendLine("<TD WIDTH =350>");
                sb.AppendLine("<TABLE WIDTH = 350 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>CONTACT INFO</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 20>");
                sb.AppendLine("<TD WIDTH = 90><B>Address Line 1</B></TD>");
                //sb.AppendLine("<TD WIDTH = 200>one</TD>");
                sb.AppendFormat("<td width = 260>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr1"]);//drPat[0]["pat_addr1"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Address Line 2</B></TD>");
                //sb.AppendLine("<TD>two</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr2"]);//drPat[0]["pat_addr2"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                string[] strCSZ = ParseCSZ(m_ds360.Tables["PAT"].Rows[0]["city_st_zip"]);//drPat[0]["city_st_zip"]);
                sb.AppendLine("<TD><B>City</B></TD>");
                //sb.AppendLine("<TD>c</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[0]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>STATE/Province/Region</B></TD>");
                //sb.AppendLine("<TD>s</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[1]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Zip/Postal Code</B></TD>");
                //sb.AppendLine("<TD>z</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[2]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Home Phone</B></TD>");
                //sb.AppendLine("<TD>h</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["guar_phone"]);//drPat[0]["guar_phone"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of third table

                sb.AppendLine("</TR>");
                #endregion tablePat

                sb.AppendLine("<BR>");
                sb.AppendLine("<hr/>");

                #region TableGuarantor
                /* Not used by Care360 Atlanta
               
                // table with a table
                
                sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #00FF00><TD COLSPAN = 3><B>GUARANTOR DATA</B></TD></TR>");

                sb.AppendLine("<TR>");

                 // Guarantor TABLE
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>GUARANTOR</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 200><B>Last Name</B></td>");
                string[] strGuar = ParseName(m_ds360.Tables["PAT"].Rows[0]["guarantor"]);//drPat[0]["guarantor"]);
                sb.AppendFormat("<td width = 275>{0}</td>", strGuar[0]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>First Name</B></td>");
                sb.AppendFormat("<td>{0}</td>", strGuar[1]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Middle Init</B></td>");
                sb.AppendFormat("<td>{0}</td>", strGuar[2]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>BIRTH DATE</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["DOB"]);//drPat[0]["DOB"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>GENDER</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["sex"]);//drPat[0]["sex"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>SSN</B></TD>");
                sb.AppendFormat("<td>{0}</td>", dr["ssn"]);
                sb.AppendLine("</TR>");

                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                //// end of Guarantor table
                
                //// add guarantor contact info table
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>CONTACT INFO</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD WIDTH = 125><B>Address Line 1</B></TD>");
                sb.AppendFormat("<td width = 350>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr1"]);//drPat[0]["pat_addr1"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Address Line 2</B></TD>");
                //sb.AppendLine("<TD>two</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["pat_addr2"]);//drPat[0]["pat_addr2"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                strCSZ = ParseCSZ(m_ds360.Tables["PAT"].Rows[0]["city_st_zip"]);//drPat[0]["city_st_zip"]);
                sb.AppendLine("<TD><B>City</B></TD>");
                //sb.AppendLine("<TD>c</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[0]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>STATE/Province/Region</B></TD>");
                //sb.AppendLine("<TD>s</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[1]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Zip/Postal Code</B></TD>");
                //sb.AppendLine("<TD>z</TD>");
                sb.AppendFormat("<td>{0}</td>", strCSZ[2]);
                sb.AppendLine("</TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Home Phone</B></TD>");
                //sb.AppendLine("<TD>h</TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["guar_phone"]);//drPat[0]["guar_phone"]);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of third table
                 */
                #endregion TableGuarantor

                #region INS
                // add a new row with the insurance tables.

                 
   #region TableIns
                sb.AppendLine("<TR>");

                sb.AppendLine("<hr/>");
                // table with a table                
                sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #FF0000><TD COLSPAN = 3><B>INSURANCE DATA</B></TD></TR>");

                sb.AppendLine("<TR>");

                // PRIMARY INSURANCE TABLE
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>PRIMARY INSURANCE</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 116><B>Insurance Name</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>", m_ds360.Tables["INS"].Rows[0]["plan_nme"]);//drIns[0]["plan_nme"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Insurance ID</B></td>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["INS"].Rows[0]["policy_num"]);//drIns[0]["policy_num"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Group Number</B></td>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["INS"].Rows[0]["grp_num"]);//drIns[0]["grp_num"]);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Insurance Number</B></TD>");
                sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["INS"].Rows[0]["policy_num"]);//drIns[0]["policy_num"]);
                sb.AppendLine("</TR>");              
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of PRIMARY INSURANCE table

             

                //// add table here
                sb.AppendLine("<TD WIDTH =475>");
                sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>INSURANCE ADDRESS</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 116><B>Address Line 1</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>", "1 CAMERON HILL CIRCLE");
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Address Line 2</B></td>");
                sb.AppendFormat("<td>{0}</td>", "SUITE 0002");
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>CITY STATE and ZIP</B></td>");
                sb.AppendFormat("<td>{0}</td>", "CHATTANOOGA, TN 37402-0002");
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B></B></TD>");
                sb.AppendFormat("<td>{0}</td>", "");
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                // end of insurance address table

                #region additional insurance
                /*
                // Secondary Insurance Table 
                string strSecInsPlanName = "";
                string strSecInsPolicyNum = "";
                string strSecInsGroupNum = "";
                string strSecInsInsNum = "";
                string strTerInsPlanName = "";
                string strTerInsPolicyNum = "";
                string strTerInsGroupNum = "";
                string strTerInsInsNum = "";
                try
                {
                    strSecInsPlanName = drIns[1]["plan_nme"].ToString();
                    strSecInsPolicyNum = drIns[1]["policy_num"].ToString();
                    strSecInsGroupNum = drIns[1]["grp_num"].ToString();
                    strSecInsInsNum = drIns[1]["policy_num"].ToString(); // change this when we know what it is

                    strTerInsPlanName = drIns[2]["plan_nme"].ToString();
                    strTerInsPolicyNum = drIns[2]["policy_num"].ToString();
                    strTerInsGroupNum = drIns[2]["grp_num"].ToString();
                    strTerInsInsNum = drIns[2]["policy_num"].ToString(); // change this when we know what it is
                }
                catch (IndexOutOfRangeException)
                {
                    // no second or third insurance so no problem
                }

                //// add table here
                sb.AppendLine("<TD WIDTH =316>");
                sb.AppendLine("<TABLE WIDTH = 300 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>SECONDARY INSURANCE</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 116><B>Insurance Name</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>",strSecInsPlanName);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Insurance ID</B></td>");
                sb.AppendFormat("<td>{0}</td>", strSecInsPolicyNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Group Number</B></td>");
                sb.AppendFormat("<td>{0}</td>", strSecInsGroupNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Insurance Number</B></TD>");
                sb.AppendFormat("<td>{0}</td>", strSecInsInsNum);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");

                sb.AppendLine("</TD>");
                // end of secondary insurance  table

                //// add tertiary insurance table
                sb.AppendLine("<TD WIDTH =320>");
                sb.AppendLine("<TABLE WIDTH = 320 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>TERTIARY INSURANCE</B></TD></TR>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td width = 120><B>Insurance Name</B></td>");
                sb.AppendFormat("<td width = 200>{0}</td>", strTerInsPlanName);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Insurance ID</B></td>");
                sb.AppendFormat("<td>{0}</td>", strTerInsPolicyNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<td><B>Group Number</B></td>");
                sb.AppendFormat("<td>{0}</td>", strTerInsGroupNum);
                sb.AppendLine("</tr>");
                sb.AppendLine("<TR HEIGHT = 25>");
                sb.AppendLine("<TD><B>Insurance Number</B></TD>");
                sb.AppendFormat("<td>{0}</td>", strTerInsInsNum);
                sb.AppendLine("</TR>");
                sb.AppendLine("</TABLE>");
                sb.AppendLine("</TD>");
                //// end of third insurance table

                #endregion additional insurance 
                sb.AppendLine("</TR>");
                #endregion INS
                #endregion tableIns
                sb.AppendLine("</TR>");
                // end of new row with the insurance tables.

                sb.AppendLine("</TR>");
                #endregion tableGuarantor

                //..................                sb.AppendLine("<BR>");
                */
                #endregion INS

#endregion Insurance

                #endregion INS

                if (!bCytoHisto)
                {
                    #region TableOrder
                    // table with a table
                    sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #0000FF><TD COLSPAN = 3><B>CLINICAL ORDER DATA</B></TD></TR>");

                    sb.AppendLine("<TR>");

                    // Diagnosis TABLE
                    sb.AppendLine("<TD WIDTH =475>");
                    sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>DIAGNOSIS</B></TD></TR>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td width = 100><B>Diagnosis 1</B></td>");
                    //sb.AppendLine("<td width = 200>ltest</td>");
                    sb.AppendFormat("<td width = 200>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_1"]);//drPat[0]["icd9_1"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 2</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_2"]);//drPat[0]["icd9_2"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 3</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_3"]);//drPat[0]["icd9_3"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 4</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_4"]);//drPat[0]["icd9_4"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 5</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_5"]);//drPat[0]["icd9_5"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 6</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_6"]);//drPat[0]["icd9_6"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 7</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_7"]);//drPat[0]["icd9_7"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 8</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_8"]);//drPat[0]["icd9_8"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 9</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_9"]);//drPat[0]["icd9_9"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("</TABLE>");

                    sb.AppendLine("</TD>");
                    //// end of Diagnosis table

                    //// add Order table here only for Atlanta 
                    if (m_strBillType == "Q")
                    {

                        sb.AppendLine("<TD WIDTH =475>");
                        sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                        sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 4><B>ORDERED TESTS</B></TD></TR>");
                        sb.AppendLine("<TD WIDTH = 100><B>QUEST CODE</B></TD>");
                        sb.AppendLine("<TD WIDTH = 100><B>QUEST DESCRIPTION</B></TD>");
                        sb.AppendLine("<TD WIDTH = 100><B>QTY</B></TD>");
                        sb.AppendLine("<TD WIDTH = 100><B>CDM</B></TD>");

                        StringBuilder strLine = new StringBuilder();
                        StringBuilder strLastLine = new StringBuilder();
                        // while the number of tests ordered
                        foreach (DataRow drOrder in m_ds360.Tables["CHRG_CLINICAL"].Rows) //drChrgClinical)
                        {
                            strLine = new StringBuilder();
                            strLine.AppendLine("<TR HEIGHT = 25>");
                            strLine.AppendFormat("<td><b>{0}</b></td>", drOrder["quest code"]);
                            strLine.AppendFormat("<td>{0}</td>", drOrder["Quest Description"]);
                            strLine.AppendFormat("<td>{0}</td>", drOrder["qty"]);
                            strLine.AppendFormat("<td>{0}</td>", drOrder["cdm"]);
                            strLine.AppendLine("</TR>");
                            if (strLine.ToString().Trim() != strLastLine.ToString().Trim())
                            {
                                sb.Append(strLine);
                                strLastLine = strLine;
                            }
                        }
                        sb.AppendLine("</TABLE>");
                        sb.AppendLine("</TD>");
                        // end of new table
                    }
                    sb.AppendLine("</TR>");
                    sb.AppendLine("</TABLE>");
                    #endregion tablePat

                    sb.AppendLine("<BR>");
                }
                if (bCytoHisto)
                {
                    #region CytoHisto

                    // table with a table
                    sb.AppendLine("<TABLE WIDTH = 950 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"3\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #0000FF><TD COLSPAN = 3><B>CYTO/HISTO ORDER DATA</B></TD></TR>");

                    sb.AppendLine("<TR>");

                    // Diagnosis TABLE
                    sb.AppendLine("<TD WIDTH =475>");
                    sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"0\" CELLSPACING=\"0\">");
                    sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 2><B>DIAGNOSIS</B></TD></TR>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td width = 100><B>Diagnosis 1</B></td>");
                    //sb.AppendLine("<td width = 200>ltest</td>");
                    sb.AppendFormat("<td width = 200>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_1"]);//drPat[0]["icd9_1"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 2</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_2"]);//drPat[0]["icd9_2"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<td><B>Diagnosis 3</B></td>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_3"]);//drPat[0]["icd9_3"]);
                    sb.AppendLine("</tr>");
                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 4</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_4"]);//drPat[0]["icd9_4"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 5</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_5"]);//drPat[0]["icd9_5"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 6</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_6"]);//drPat[0]["icd9_6"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 7</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_7"]);//drPat[0]["icd9_7"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 8</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_8"]);//drPat[0]["icd9_8"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("<TR HEIGHT = 25>");
                    sb.AppendLine("<TD><B>Diagnosis 9</B></TD>");
                    sb.AppendFormat("<td>{0}</td>", m_ds360.Tables["PAT"].Rows[0]["icd9_9"]);//drPat[0]["icd9_9"]);
                    sb.AppendLine("</TR>");

                    sb.AppendLine("</TABLE>");


                    sb.AppendLine("</TD>");
                    //// end of Diagnosis table

                    if (m_strBillType == "Q")
                    {
                        //// add Order table here
                        sb.AppendLine("<TD WIDTH =475>");
                        sb.AppendLine("<TABLE WIDTH = 475 BORDER=\"1\" CELLPADDING=\"1\" CELLSPACING=\"0\">");
                        sb.AppendLine("<TR ALIGN=CENTER BORDERCOLOR = #000000><TD COLSPAN = 4><B>ORDERED TESTS</B></TD></TR>");
                        sb.AppendLine("<TD WIDTH = 100><B>QUEST CODE</B></TD>");
                        sb.AppendLine("<TD WIDTH = 100><B>QUEST DESCRIPTION</B></TD>");
                        sb.AppendLine("<TD WIDTH = 100><B>QTY</B></TD>");
                        sb.AppendLine("<TD WIDTH = 100><B>CDM</B></TD>");


                        foreach (DataRow drOrderCH in m_ds360.Tables["CHRG_CYTO_HISTO"].Rows)//drChrgCytoHisto)
                        {
                            sb.AppendLine("<TR HEIGHT = 25>");
                            sb.AppendFormat("<td><b>{0}</b></td>", drOrderCH["quest code"]);
                            sb.AppendFormat("<td>{0}</td>", drOrderCH["Quest Description"]);
                            sb.AppendFormat("<td>{0}</td>", drOrderCH["qty"]);
                            sb.AppendFormat("<td>{0}</td>", drOrderCH["cdm"]);
                            sb.AppendLine("</TR>");
                        }
                        sb.AppendLine("</TABLE>");
                        sb.AppendLine("</TD>");
                        // end of new table
                    }

                    sb.AppendLine("</TR>");
                    sb.AppendLine("</TABLE>");


                    sb.AppendLine("<BR>");
                    #endregion CytoHisto
                }
                //..................
                // end of document
                sb.Append("</BODY>\r\n</HTML>");
                m_htmDoc.Write(sb.ToString());

                string strDocText = m_wbPrint.DocumentText;

                tsslAmount.Text = string.Format("Working on account {0}", dr["account"]);
                this.Invalidate();
                
                m_dicPatient.Add(strPatID, strDocText);
                /*
                try
                {
                    Clipboard.SetText(strDocText, TextDataFormat.Text);
                }
                catch (Exception ex)
                {
                    // why
                    MessageBox.Show(ex.Message,propAppName);
                }
                 */
              //  break;
            }
       //     throw new Exception("remove this error and reinstate the insert below");
            InsertIntoData360();
      
        }

        private void InsertIntoData360()
        {
            // put them in the table
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                foreach (KeyValuePair<string, string> kvp in m_dicPatient)
                {
                    string strId = kvp.Key.ToString().Replace("'","").Split(new char[] { '|' })[0];
                    string strAcc = kvp.Key.ToString().Split(new char[] { '|' })[1];
                    string strDOS = kvp.Key.ToString().Split(new char[] { '|' })[2];
                    SqlCommand cmdInsert = new SqlCommand(string.Format(
                        "INSERT INTO data_quest_360 " +
                        "(patid, html_doc, account, date_of_service, pre360_error, bill_code_error, " +
                        " bill_type, " +
                        " mod_date, mod_user, mod_prg, mod_host, entered, charges_entered) " +
                        "VALUES " +
                        "    ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}','{10}',{11},'{12}')",
                        strId,
                            kvp.Value.ToString().Replace("'", "''"),
                            strAcc, 
                            strDOS,
                            0, 0,
                            m_strBillType,
                        DateTime.Now, Environment.UserName, Application.ProductName + Application.ProductVersion,
                            Environment.MachineName,
                            0,m_strBillType == "QR" ? 1:0)
                            , conn);

                    cmdInsert.Connection.Open();
                    try
                    {
                        int nRecEx = cmdInsert.ExecuteNonQuery();
                    }
                    catch (SqlException se)
                    {
                        m_Err.m_Logfile.WriteLogFile(se.Message);
                    }
                    catch (Exception ex)
                    {
                        Type tT = ex.GetType();
                        m_Err.m_Logfile.WriteLogFile(ex.Message);
                    }
                    finally
                    {
                        cmdInsert.Connection.Close();
                    }


                }
            }
        }

        private string ConvertRelation(object p)
        {
            string strRetVal = "OTHER";
            int nP = 9;
            if (!int.TryParse(p.ToString(), out nP))
            {
                return strRetVal;
            }
            switch (nP)
            {
                case 1:
                    {
                        strRetVal = "SELF";
                        break;
                    }
                case 2:
                    {
                        strRetVal = "SPOUSE";
                        break;
                    }
                case 3:
                    {
                        strRetVal = "CHILD";
                        break;
                    }
                default:
                    {
                        strRetVal = "OTHER";
                        break;
                    }
            }
            return strRetVal;
        }

        private void WriteErr(DataRow dr, DataRow[] drPat, DataRow[] drIns, DataRow[] drChrg, DataRow[] drAmt, Exception ex)
        {
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                string strPatID = strPatID = string.Format("{0}{1}{2}{3}",
                                           dr["LASTNAME"],
                                           dr["FIRSTNAME"],
                                           dr["MIDNAME"],
                                           dr["SSN"]
                                           ); 
                DateTime dtDob = DateTime.MinValue;
                try
                {
                    if (DateTime.TryParse(drPat[0]["DOB"].ToString(), out dtDob))
                    {

                        strPatID = string.Format("{0}{1}{2}{3}{4}{5}",
                                               dr["LASTNAME"].ToString().Trim(),
                                               dr["FIRSTNAME"].ToString().Trim(),
                                               dr["MIDNAME"].ToString().Trim(),
                                               string.Format("{0,8:yyyyMMdd}", dtDob),
                                               drPat[0]["SEX"].ToString().Trim(),
                                               dr["SSN"].ToString().Trim()
                                               );

                    }
                }
                catch (IndexOutOfRangeException )
                {
                    // the pat record is invalid
                }

                string strErr = string.Format("Patient record is {0} and Insurance record is {1}" +
                    " Charge record is {2} and ERROR is [{3}]",
                    drPat.GetUpperBound(0) > -1 ? "valid" : "invalid", 
                    drIns.GetUpperBound(0) > -1 ? "valid" : "invalid",
                    drChrg.GetUpperBound(0) > -1 ? "valid" : "invalid",
                    ex.InnerException);
                string strAcc = dr["ACCOUNT"].ToString();
                SqlCommand cmdInsert = new SqlCommand(string.Format(
                    "INSERT INTO data_quest_360 " +
                    "(patid, html_doc, account, date_of_service, pre360_error, bill_code_error, " +
                    " mod_date, mod_user, mod_prg, mod_host) " +
                    "VALUES " +
                    "    ('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')",
                    strPatID.Replace("'","''"),
                        strErr,
                        strAcc, 
                        dr[5].ToString(),
                        1, 0,
                    DateTime.Now, Environment.UserName, Application.ProductName + Application.ProductVersion,
                        Environment.MachineName), conn);

                cmdInsert.Connection.Open();
                cmdInsert.ExecuteNonQuery();
                cmdInsert.Connection.Close();

            }
        }

        private string[] ParseName(object p)
        {
            string[] retVal = new string[3];
            if (string.IsNullOrEmpty(p.ToString()))
            {
                return retVal;
            }
            string strLname = p.ToString().ToUpper().Split(new char[] { ',' })[0].Trim();
            string strRname = p.ToString().ToUpper().Split(new char[] { ',' })[1].Trim();
            string strFname = strRname.Split(new char[] { ' ' })[0].Trim();
            string strMname = "";
            try
            {
                strMname = strRname.Split(new char[] { ' ' })[1].Trim();
            }
            catch (IndexOutOfRangeException)
            {
                // no middle name / init
            }

            retVal[0] = strLname;
            retVal[1] = strFname;
            retVal[2] = strMname;

            return retVal;
        }

        private string[] ParseCSZ(object p)
        {
            string[] retVal = new string[3];
            if (string.IsNullOrEmpty(p.ToString()))
            {
                return retVal;
            }
            string strCity  = p.ToString().ToUpper().Split(new char[] {','})[0].Trim();
            string strSTZip = p.ToString().ToUpper().Split(new char[] { ',' })[1].Trim();
            string strState = strSTZip.Split(new char[] { ' ' })[0].Trim();
            string strZip   = strSTZip.Split(new char[] { ' ' })[1].Trim();
            
            retVal[0] = strCity;
            retVal[1] = strState;
            retVal[2] = strZip;

            return retVal;
            
        }

        private void tsmiExclusion(object sender, EventArgs e)
        {
            dgvRecords.DataSource = null;
            m_strBillType = "C";
            tscbBillingType.SelectedIndex = tscbBillingType.ComboBox.FindStringExact(m_strBillType);
            tsslBillType.Text =
                string.Format("BILLING TYPE: {0}", ((ToolStripMenuItem)sender).Tag.ToString());
            
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable dtTests = new DataTable();
                
                SqlCommand cmdSelect = new SqlCommand(SelectQueryExclusion(), conn);
                sda.SelectCommand = cmdSelect;
                
                int nRec = sda.Fill(dtTests);
                dgvRecords.DataSource = dtTests;
            }
           LoadStatusBar();
           tsmiBillExclusions.Enabled = true;

        }

        private void tsbDictLoad_Click(object sender, EventArgs e)
        {
            //m_dtDictQuestReferenceLabTests = new DataTable();
            //using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            //{
            //    SqlDataAdapter sda = new SqlDataAdapter();
            //    SqlCommand cmdSelect = new SqlCommand(
            //        string.Format("SELECT mt_mnem, has_multiples, cdm, cdm_description, " +
            //        "cpt4, cpt4_description, link, quest_code, quest_description " +
            //        "FROM         dict_quest_reference_lab_tests " +
            //        "WHERE     (deleted = 0)"), conn);
            //    sda.SelectCommand = cmdSelect;
            //    sda.Fill(m_dtDictQuestReferenceLabTests);

            //    int nCount = m_dtDictQuestReferenceLabTests.Rows.Count;
            //}
        }

       

        private void tsbPurge_Click(object sender, EventArgs e)
        {
            string strAccDr = null;
            string strAccOld = null;
            string strQuestCode = null;
            bool bNullQuestCode = false;
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                strAccDr = dr.Cells["account"].Value.ToString();
                if (!bNullQuestCode)
                {
                    strQuestCode = dr.Cells["quest code"].Value.ToString();
                }
                
                if (strAccDr != strAccOld)
                {
                    strAccOld = strAccDr;
                    bNullQuestCode = string.IsNullOrEmpty(strQuestCode);

                }
                bNullQuestCode = string.IsNullOrEmpty(strQuestCode);
                if (bNullQuestCode)
                {
                    continue;
                }
            
            }

            
    }

        private void Load360_fix_info_Click(object sender, EventArgs e)
        {
            string strAcc = null;
            Form f = new Form();
            f.Text = "ENTER SELECTION";

            Button bYes = new Button();
            bYes.Text = "YES";
            bYes.Location = new Point((f.Size.Width / 2)-(bYes.Width/2), (f.Size.Height-30)/2 );
            bYes.DialogResult = DialogResult.Yes;
            bYes.TabIndex = 1;

            Button bNo = new Button();
            bNo.Location = new Point((f.Size.Width / 2)-(bNo.Width/2) , (f.Size.Height+30)/2);
            bNo.Text = "NO";
            bNo.DialogResult = DialogResult.No;
            bNo.TabIndex = 2;

            TextBox t = new TextBox();
            t.Dock = DockStyle.Fill;
            t.TabIndex = 0;
            
            f.Controls.Add(t);
            f.Controls.Add(bYes);
            f.Controls.Add(bNo);
            t.Select();

            if (f.ShowDialog() == DialogResult.Yes)
            {
                strAcc = t.Text;
            }
            if (string.IsNullOrEmpty(strAcc))
            {
                return;
            }
            
            m_strBillType = strAcc.ToUpper().StartsWith("QR") ? "QR":"Q";
            if (m_strBillType != "QR")
            {
                m_strBillType = strAcc.ToUpper().StartsWith("C") ? "QR" : "Q";
            }

            tsslBillType.Text =
                string.Format("BILLING TYPE: {0}", "QUEST");

            m_dsQUE = new DataSet();
            using (SqlConnection conn =
            new SqlConnection(
                string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                m_strServer, m_strDatabase)))
            {

                SqlDataAdapter sda = new SqlDataAdapter();
                string strQuery =
            string.Format("with cteAcc as ( " +
                     "select acc.account, datediff(year,dob_yyyy,trans_date) as [Age] " +
                     "from acc " +
                     "inner join pat on pat.account = acc.account " +
                     "where fin_code in ('D','Y') and trans_date between '{0} 00:00' and '{1} 23:59:59' "+
                     ") " +
                    // "and status = 'SSI1500') "+ // to fix lipid and leads
                    // "and status = 'QUEST') " +
                     ", cteChrg as ( " +
                     "select chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age] " +
                     ", convert(datetime,convert(varchar(10),service_date,101)) as [DOS] " +
                     "from chrg " +
                     "inner join cteAcc on cteAcc.account = chrg.account " +
                     "inner join amt on amt.chrg_num = chrg.chrg_num " +
                     "where credited = 0 "+
                     "and cdm <> 'CBILL' "+
                     ") " +
                     "select distinct cteChrg.account, cteChrg.chrg_num, cteChrg.cdm, cteChrg.cpt4 " +
                     ", cteChrg.qty, cteChrg.dos " +
                     ", [Age] " +
                     ",case when dd.cpt is null " +
                     "then 'GAP' " +
                     " else case when (age > 11 and age_appropriate = 1) " +
                     " then 'GAP' else " +
                     "'EXCLUSION' " +
                     " end " +
                     "end as [Bill Type] " +
                     ", coalesce(dt.quest_code,dt2.quest_code) as [Quest Code] " +
                     ", coalesce(dt.quest_description,dt2.quest_description) as [Quest Description] " +
                     "from cteChrg " +
                     "left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0 " +
                     "left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                     "left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                     "where account = '{2}' " +
                     "order by account, cdm, cteChrg.cpt4 ",
                     m_dtFrom.ToString("d"), m_dtThru.ToString("d"), strAcc);
                sda.SelectCommand =
                new SqlCommand(strQuery, conn);

                DateTime dtWait = DateTime.Now;
                try
                {
                    int nRec = sda.Fill(m_dsQUE);
                }
                catch (SqlException se)
                {
                    m_Err.m_Logfile.WriteLogFile(se.Message);
                    int n = 0;
                    while (DateTime.Now < dtWait.AddSeconds(10))
                    {
                        n += n++; // just hum a bit
                    }

                    MessageBox.Show(string.Format("{1}\r\n\n{0}",se.Message, "SQL EXCEPTION DURING LOAD"),propAppName);
                }

                dgvRecords.DataSource = m_dsQUE.Tables[0];
            }

            dgvRecords.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            LoadStatusBar();
            foreach (DataGridViewRow dr in dgvRecords.Rows)
            {
                if (dr.Cells["cdm"].Value.ToString().StartsWith("552"))
                {
                    if (dr.Cells["cdm"].Value.ToString().Equals("5527418") ||
                        dr.Cells["cdm"].Value.ToString().Equals("5527419"))
                    {
                        continue;
                    }
                    dr.ErrorText = "CHANTILLY Charge";
                }
                Application.DoEvents();
            }
            MessageBox.Show("Load Completed",propAppName);
            dgvRecords.Invalidate();
            this.Invalidate();
        }

        private void tsmiCreate360FixInfo_Click(object sender, EventArgs e)
        {
            // create the HTML

            m_ds360 = new DataSet();
            m_ds360.Tables.Add("ACC");
            m_ds360.Tables.Add("PAT");
            m_ds360.Tables.Add("INS");
            m_ds360.Tables.Add("CHRG_CLINICAL");
            m_ds360.Tables.Add("CHRG_CYTO_HISTO"); // wdk 20121218 modification


            string strAccOld = null;
            string sbChrgNums = "(";
            foreach (DataGridViewRow drAcc in dgvRecords.Rows)
            {


                string strAcc = drAcc.Cells["account"].Value.ToString();
                string strSelectAcc = string.Format("account = '{0}'", strAcc);
                DataRow[] drAccs = m_dsQUE.Tables[0].Select(strSelectAcc);

                foreach (DataRow drChrg in drAccs)
                {
                    sbChrgNums += string.Format("'{0}',", drChrg["chrg_num"].ToString());
                }
                sbChrgNums = sbChrgNums.Remove(sbChrgNums.LastIndexOf(','), 1);
                sbChrgNums += ")";

                if (strAcc == strAccOld)
                {
                    continue;
                }
                strAccOld = strAcc;

                // m_ds360.Tables["360"].Rows.Clear();
                //  m_ds360.Tables["AMT"].Rows.Clear();
                m_ds360.Tables["CHRG_CLINICAL"].Rows.Clear();
                m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Clear();
                m_ds360.Tables["INS"].Rows.Clear();
                m_ds360.Tables["PAT"].Rows.Clear();
                m_ds360.Tables["ACC"].Rows.Clear();

                // using the current row fill the tables
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter();
                    DataTable dtAcc = new DataTable();
                    #region ACCOUNT
                    SqlCommand cmdAccSelect = new SqlCommand(
                        string.Format("select  " +
                        "	account, " +
                        " substring(pat_name, 0, charindex(',',pat_name,0)) as [lastname], " +
                        "	substring(pat_name, " +
                        "	charindex(',',pat_name,0)+1 " +
                        " , case when charindex(' ',pat_name,charindex(',',pat_name,0)-1) = 0 " +
                        "then len(pat_name) - charindex(',',pat_name,0) " +
                        "else  " +
                        " charindex(' ',pat_name,charindex(',',pat_name,0)+1) - " +
                        " charindex(',',pat_name,0) " +
                        "	end) as [firstname] " +
                        "	,	substring(pat_name,  " +
                        "	charindex(' ',pat_name, charindex(',',pat_name,0)+1)+1 " +
                        " 	, case when charindex(' ',pat_name,charindex(',',pat_name,0)+1) = 0 " +
                        " then 0 " +
                        " else  charindex(' ',pat_name,charindex(',',pat_name,0)+1)			" +
                        " end)	as [midname]" +
                        ", cl_mnem " + //, fin_code
                        "	, convert(varchar(10),trans_date,101) as [trans_date] " +
                        " ,  stuff(stuff(ssn,4,0,'-'),7,0,'-') as [ssn] " +
                        " from acc " +
                        " where acc.account = '{0}' ", strAcc)
                        , conn);
                    sda.SelectCommand = cmdAccSelect;
                    int nAccRec = sda.Fill(m_ds360.Tables["ACC"]);

                    m_ds360.Tables["ACC"].PrimaryKey =
                        new DataColumn[] { m_ds360.Tables["ACC"].Columns["ACCOUNT"] };

                    #endregion ACCOUNT
                    #region PAT
                    SqlCommand cmdPatSelect = new SqlCommand(
                       string.Format("select " +
                       " pat.account " +
                       " ,trans_date " +
                       ", pat_addr1, pat_addr2, city_st_zip " +
                       ", convert(varchar(10),dob_yyyy,101) as [DOB], sex, relation " +
                       ", guarantor, guar_addr, g_city_st" +
                       ", guar_phone, pat_marital  " +
                       ", icd9_1, icd9_2, icd9_3, icd9_4, icd9_5, icd9_6, icd9_7, icd9_8, icd9_9 " +
                       ", phy.last_name +','+ phy.first_name +' '+ isnull(phy.mid_init,'') +' ('+phy_id +')' as [phy_id] " +
                       "  from pat " +
                       "inner join acc on acc.account = pat.account " +
                       "left outer join phy on phy.tnh_num = pat.phy_id " +
                       " where pat.account = '{0}'", strAcc)
                       , conn);

                    sda.SelectCommand = cmdPatSelect;
                    int nPatRec = sda.Fill(m_ds360.Tables["PAT"]);

                    //if (!m_ds360.Relations.Contains("AccPat"))
                    //{
                    //    DataRelation drAccPat = m_ds360.Relations.Add(
                    //        "AccPat", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                    //        m_ds360.Tables["PAT"].Columns["ACCOUNT"]);
                    //}

                    #endregion PAT
                    #region INS

                    SqlCommand cmdInsSelect = new SqlCommand(
                       string.Format("select " +
                       "	ins.account, ins_a_b_c, holder_nme, holder_dob, holder_sex, holder_addr, holder_city_st_zip, plan_nme, plan_addr1, plan_addr2, " +
                       "    p_city_st, policy_num, cert_ssn, grp_nme, grp_num, employer, e_city_st " +
                       "	, ins_code, relation " +
                       " from ins " +
                       " where account = '{0}' and deleted = 0 ", strAcc)
                       , conn);

                    sda.SelectCommand = cmdInsSelect;
                    int nInsRec = sda.Fill(m_ds360.Tables["INS"]);

                    //if (!m_ds360.Relations.Contains("AccIns"))
                    //{
                    //    DataRelation drAccPat = m_ds360.Relations.Add(
                    //        "AccIns", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                    //        m_ds360.Tables["INS"].Columns["ACCOUNT"]);
                    //}

                    #endregion INS


                    #region account charges
                    string strSelectChrg =
                        string.Format("select account, qty, chrg.cdm, " +
                        " coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
                        "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
                        "from chrg " +
                        "left outer join dict_quest_reference_lab_tests dt " +
                        "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                        "left outer join dict_quest_reference_lab_tests dt2 " +
                        "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                        "where account = '{0}' " +
                        "and (not chrg.cdm between '5920000' and '594ZZZZ') "+
                        "and chrg_num in {1} "
                        , strAcc, sbChrgNums);

                    sda.SelectCommand =

                        new SqlCommand(strSelectChrg, conn);

                    DateTime dtWait = DateTime.Now;
                    try
                    {
                        int nChrgRec = sda.Fill(m_ds360.Tables["CHRG_CLINICAL"]);
                    }
                    catch (SqlException se)
                    {
                        m_Err.m_Logfile.WriteLogFile(se.Message);
                        int n = 0;
                        while (DateTime.Now < dtWait.AddSeconds(10))
                        {
                            n += n++; // just hum a bit
                        }
                        //    if (int.Parse(((ToolStripButton)sender).Tag.ToString()) == 2)
                        //{
                        //    tsbLoad_Click(sender, e);
                        //}
                        MessageBox.Show(string.Format("{1}\r\n\n{0}",se.Message, "SQL EXCEPTION DURING LOAD"),propAppName);
                    }
                    //m_ds360.Tables["CHRG_CLINICAL"].PrimaryKey = null;
                    //if (strAcc.StartsWith("QR"))
                    //{
                    //    // there are no quest codes for reference lab testing.
                    //    m_ds360.Tables["CHRG_CLINICAL"].PrimaryKey =
                    //    new DataColumn[] { m_ds360.Tables["CHRG_CLINICAL"].Columns["ACCOUNT"],
                    //                         m_ds360.Tables["CHRG_CLINICAL"].Columns["CHRG_NUM"],
                    //                            m_ds360.Tables["CHRG_CLINICAL"].Columns["CDM"]//,
                    //                                //m_ds360.Tables["CHRG_CLINICAL"].Columns["CPT4"]
                    //                                    };
                    //}
                    //else
                    //{
                    //    m_ds360.Tables["CHRG_CLINICAL"].PrimaryKey =
                    //      new DataColumn[] { m_ds360.Tables["CHRG_CLINICAL"].Columns["ACCOUNT"],
                    //                         m_ds360.Tables["CHRG_CLINICAL"].Columns["CHRG_NUM"],
                    //                            m_ds360.Tables["CHRG_CLINICAL"].Columns["CDM"],
                    //                                //m_ds360.Tables["CHRG_CLINICAL"].Columns["CPT4"],
                    //                                    m_ds360.Tables["CHRG_CLINICAL"].Columns["QUEST CODE"]};
                    //}

                    //if (!m_ds360.Relations.Contains("AccChrg"))
                    //{
                    //    DataRelation drAccPat = m_ds360.Relations.Add(
                    //        "AccChrg", m_ds360.Tables["ACC"].Columns["ACCOUNT"],
                    //        m_ds360.Tables["CHRG_CLINICAL"].Columns["ACCOUNT"]);
                    //}

                    #endregion account charges

                    #region charges cyto_hysto
                    sda.SelectCommand =
                        new SqlCommand(
                        string.Format("select distinct account, qty, chrg.cdm, " +
                        "coalesce(dt.quest_code,dt2.quest_code) as [quest code], " +
                        "coalesce(dt.quest_description, dt2.quest_description) as [quest description] " +
                        "from chrg " +
                        "left outer join dict_quest_reference_lab_tests dt " +
                        "on dt.cdm =  chrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 " +
                        "left outer join dict_quest_reference_lab_tests dt2 " +
                        "on  dt2.cdm = chrg.cdm  and dt2.link = chrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0 " +
                        "where account = '{0}' " +
                        "and (chrg.cdm between '5920000' and '594ZZZZ') "+
                        "and chrg_num in {1} ", strAcc, sbChrgNums),
                    conn);

                    dtWait = DateTime.Now;
                    try
                    {
                        int nChrgCytoHystoRec = sda.Fill(m_ds360.Tables["CHRG_CYTO_HISTO"]);
                    }
                    catch (SqlException se)
                    {
                        m_Err.m_Logfile.WriteLogFile(se.Message);
                        int n = 0;
                        while (DateTime.Now < dtWait.AddSeconds(10))
                        {
                            n += n++; // just hum a bit
                        }

                        MessageBox.Show(string.Format("{1}\r\n\n{0}",se.Message, "SQL EXCEPTION DURING LOAD"),propAppName);
                    }
                    m_ds360.Tables["CHRG_CYTO_HISTO"].PrimaryKey = null;



                    #endregion charges cyto_hysto


                }
                if (m_ds360.Tables["CHRG_CLINICAL"].Rows.Count > 0)
                {
                CreateHTML(false);
                }
                if (m_ds360.Tables["CHRG_CYTO_HISTO"].Rows.Count > 0)
                {
                    CreateHTML(true);
                }
            }
            MessageBox.Show("HTML Created",propAppName);

        }

        private void dgvRecords_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            LoadStatusBar();

        }

        private void tstbRebill_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Enter)
            {
                return;
            }
            if (MessageBox.Show(
                string.Format("{1}\r\n\n{0}",
                "Did you select the billing type from the drop down before hitting enter?", 
                "SELECT BILL TYPE?"),propAppName, MessageBoxButtons.YesNo) ==
                    DialogResult.No)
            {
                return;
            }
            tsmiMCLRef.Enabled = true;
            tsmiBillMCL.Enabled = true;
            
            string strAcc = ((ToolStripTextBox)sender).Text;
            m_strBillType = tscbBillingType.Text;
            int nAccCount = 0;

            

            m_dsQUE = new DataSet();
            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();

                sda.SelectCommand =
                    new SqlCommand(
                    // exclude the accounts that are in the table that are valid and ready to go
                        string.Format("SELECT chrg.account, chrg_num, status, service_date, cdm, qty, net_amt, fin_type " +
                        " ,'{1}' as [BILL TYPE] "+
                        "FROM         chrg " +
                        "where chrg.account = '{0}' " +
                        "and credited = 0 " +
                        "order by chrg.account, chrg_num",
                        strAcc, m_strBillType == "QR" ? "QUESTREF" : m_strBillType == "Q" ? "QUESTR" : "NOT BILLABLE"
                        )
                        , conn);
                DateTime dtWait = DateTime.Now;
                try
                {
                    int nRec = sda.Fill(m_dsQUE);
                   
                }
                catch (SqlException se)
                {
                    
                    MessageBox.Show(string.Format("{1}\r\n\n{0}",se.Message, se.GetType().ToString()),propAppName);
                    return;
                }


                dgvRecords.DataSource = m_dsQUE.Tables[0];
                string strOldAcct = "";

                tspbRecords.Minimum = 0;
                tspbRecords.Maximum = dgvRecords.Rows.Count;
                foreach (DataGridViewRow dr in dgvRecords.Rows)
                {

                    try
                    {
                        Application.DoEvents();
                        tspbRecords.PerformStep();
                    }
                    catch (Exception)
                    {
                        //int x;
                        //x = 0;
                    }
                    if (strOldAcct != dr.Cells["account"].Value.ToString())
                    {
                        nAccCount++;
                        strOldAcct = dr.Cells["account"].Value.ToString();

                    }

                }
                tsslRecords.Text = string.Format("{0} Records for {1} Accounts", dgvRecords.Rows.Count, nAccCount);
            }
            MessageBox.Show("Load Completed",propAppName);
            dgvRecords.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvRecords.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            tsslRecords.Text = string.Format("{0} Records for {1} Accounts", dgvRecords.Rows.Count, nAccCount);
            tsmiBillMCL.Enabled = false;
            tsmiMCLRef.Enabled = propAppUser.Contains("wkelly") ? true : false;
        }

        private void tsddbtnLoad_Click(object sender, EventArgs e)
        {
            ToolStripSplitButton tssb = ((ToolStripSplitButton)sender);
            
            tsmiQuestRef.Enabled = true;
            tsmiMCLRef.Enabled = propAppUser.Contains("wkelly") ? true : false;
            tsmiBillMCL.Enabled = propAppUser.Contains("wkelly") ? true : false; ;
            tsmiExclusions.Enabled = propAppUser.Contains("wkelly") ? true : false; ;
            tsmiBillExclusions.Enabled = propAppUser.Contains("wkelly") ? true : false; ;
        }

        private void ResetLoad()
        {
            foreach (ToolStripItem tsmi in tsddbtnLoad.DropDownItems)
            {
                if (tsmi.GetType() == typeof(ToolStripSeparator))
                {
                    continue;
                }
                if (tsmi.GetType() == typeof(ToolStripTextBox))
                {
                    continue;
                }
                bool bSel = tsmi.Selected;
                tsmi.Enabled = false;
            }
            tsmiQuestRef.Enabled = true;
        }

        private void tsmiClearError_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow dr in dgvRecords.SelectedRows)
            {
                Application.DoEvents();
                ((DataRowView)dr.DataBoundItem).Row.RowError = "";
                ((DataRowView)dr.DataBoundItem).Row.AcceptChanges();
                
            }

            
        }

        private void tsmiCheckErrors_Click(object sender, EventArgs e)
        {
            dgvRecords.DataSource = null;
            m_strBillType = "C";
            tscbBillingType.SelectedIndex = tscbBillingType.ComboBox.FindStringExact(m_strBillType);
            tsslBillType.Text =
                string.Format("BILLING TYPE: {0}", ((ToolStripMenuItem)sender).Tag.ToString());

            using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
            {
                SqlDataAdapter sda = new SqlDataAdapter();
                DataTable dtTests = new DataTable();

                SqlCommand cmdSelect20131010 = new SqlCommand(
                    string.Format(";with cteAcc as ( select status, acc.account " +
                    ", datediff(year,dob_yyyy,trans_date) as [Age] from acc inner join pat on pat.account = acc.account  " +
                    "where fin_code = 'D' and trans_date between '10/1/2012 00:00' and '10/9/2013 23:59:59' and status = 'QUEST' )  " +
                    ", cteChrg as ( select cteAcc.status, chrg.account, chrg.chrg_num, qty, cdm , cpt4, [Age]  " +
                    ", convert(datetime,convert(varchar(10),service_date,101)) as [DOS] , amt.uri from chrg  " +
                    "inner join cteAcc on cteAcc.account = chrg.account  " +
                    "inner join amt on amt.chrg_num = chrg.chrg_num  " +
                    "where credited = 0 and (invoice is null or invoice = '')  " +
                    "and not chrg.cdm in (select cdm from cdm  " +
                    "where (cdm between '5520000' and '5527417' or cdm between '5527420' and '552ZZZZ')) " +
                    ")  " +
                    ", cteCharges " +
                    "as ( " +
                    "	select distinct chrg.account " +
                    ", case when coalesce(dd.cpt,'') = '' " +
                    "		then 'EXCLUSION' " +
                    "		else 'GAP'  " +
                    "		end as [Bill Type] " +
                    ", case when outpatient_billing = 1 and service_date >= '04/01/2012 00:00:00' " +
                    "	then 'UBOP' " +
                    "	else '1500' " +
                    "	end as [SSI TYPE] " +
                    ", cdm "+
                    "from chrg " +
                    "inner join amt on amt.chrg_num = chrg.chrg_num " +
                    "left outer join dict_quest_exclusions_final_draft dd on  " +
                    "	dd.cpt = amt.cpt4 and outpatient_surgery = 0   " +
                    "	and (service_date >= dd.start_date  and service_date <= isnull(dd.expire_date,getdate()))  " +
                    "inner join acc on acc.account = chrg.account and acc.fin_code = 'D' and trans_date between '{0} 00:00' and '{1} 23:59:59' and acc.status = 'QUEST'  " +
                    "inner join client on client.cli_mnem = acc.cl_mnem " +
                    "where chrg.account in (select account from acc where status = 'QUEST') " +
                    ") " +
                    "select * from cteCharges " +
                    "where  " +
                    "not cteCharges.account in (select cteChrg.account " +
                    "from cteChrg  " +
                    "left outer join dict_quest_exclusions_final_draft dd on dd.cpt = cteChrg.cpt4 and outpatient_surgery = 0  and (cteChrg.dos >= dd.start_date  and cteChrg.dos <= isnull(dd.expire_date,getdate()))  " +
                    "left outer join dict_quest_reference_lab_tests dt on dt.cdm = cteChrg.cdm and dt.has_multiples = 0 and dt.deleted = 0 and (cteChrg.dos >= dt.start_date  and cteChrg.dos <= isnull(dt.expire_date,getdate()))  " +
                    "left outer join dict_quest_reference_lab_tests dt2 on  dt2.cdm = cteChrg.cdm  and dt2.link = cteChrg.qty and dt2.has_multiples = 1 and dt2.deleted = 0  " +
                    "and (cteChrg.dos >= dt2.start_date  and cteChrg.dos <= isnull(dt2.expire_date,getdate()))  " +
                    "where  " +
                    "case when dd.cpt is null then 'GAP'   " +
                    "else case when (age > 11 and age_appropriate = 1)   " +
                    "then 'GAP' else 'EXCLUSION'  end end = 'GAP'  " +
                    ") ",
                       m_dtFrom.ToString("d"), m_dtThru.ToString("d")), conn);

              
                sda.SelectCommand = cmdSelect20131010;
                int nRec = sda.Fill(dtTests);
                dgvRecords.DataSource = dtTests;



            }
            LoadStatusBar();
            tsmiBillExclusions.Enabled = true;

        }

        

        private void tstbRebill_DoubleClick(object sender, EventArgs e)
        {
            if (tstbRebill.Text == "REBILL")
            {
                return;
            }
            TsbBillMCLRef_Click(null, null);                
        }

        private void dgvRecords_RowHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right &&
                    string.IsNullOrEmpty(dgvRecords.Rows[e.RowIndex].Cells["Quest Code"].Value.ToString()))
            {
                
                Form f = new Form();
                f.Text = "QUEST CODE NEEDED";
                DataGridView dgv = new DataGridView();
                dgv.Dock = DockStyle.Fill;
                dgv.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
                int nRec = 0;
                using (SqlConnection conn = new SqlConnection(m_sqlConnection.ConnectionString))
                {
                    SqlDataAdapter sda = new SqlDataAdapter(new SqlCommand(
                        string.Format(";with cte " +
                        "as " +
                        "( " +
                        "select mnem " +
                        ", 0 as [has_multiples], cdm.cdm, cdm.descript as [cdm_description] " +
                        ", cpt4.cpt4, cpt4.descript as [cpt4_description] " +
                        ", cpt4.link " +
                        ",'' as [quest_code] " +
                        ",'' as [quest_description] " +
                        "from cdm " +
                        "inner join cpt4 on cpt4.cdm = cdm.cdm " +
                        "where cdm.cdm = '{0}' " +
                        ") " +
                        "select * from cte ", dgvRecords.Rows[e.RowIndex].Cells["cdm"].Value.ToString()), conn));
                    DataTable dt = new DataTable();
                    nRec = sda.Fill(dt);
                    dgv.DataSource = dt;
                }
                f.Controls.Add(dgv);
                f.ShowDialog();

                }
        }

   

 

       

    



       


    } //
}
