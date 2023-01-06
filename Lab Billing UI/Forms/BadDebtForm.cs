using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Data.SqlClient;
// programmer added
using RFClassLibrary;
using MCL;
using System.Drawing.Printing;
using LabBilling.Logging;
using LabBilling.Core.Models;
using Microsoft.Identity.Client;

namespace LabBilling.Forms
{
    public partial class BadDebtForm : Form
    {
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }
        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;

        R_notes m_rNotes = null;
        R_ins m_rIns = null;
        R_pat m_rPat = null;
        R_chk m_rChk = null;
        CAcc m_CAcc = null;
        ERR m_Err = null; 
        string m_strServer = null;
        string m_strDatabase = null;
        string m_strProductionEnvironment = null;
        DataTable m_dtAccounts;
        SqlDataAdapter m_sdaBadDebt; 
        //ToolStripControlHost m_dpFrom;
        //ToolStripControlHost m_dpThru;
        //ToolStripControlHost m_cboxInclude; // CheckBox
        
        //private void CreateDateTimes()
        //{
        //    Log.Instance.Trace($"Entering");
        //    int nSert = tsMain.Items.Count;
        //    // create the datetime controls for the From and Thru dates
        //    m_dpFrom = new ToolStripControlHost(new DateTimePicker());
        //    m_dpFrom.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 0, 0, 0).ToString();
        //    ((DateTimePicker)m_dpFrom.Control).Format = DateTimePickerFormat.Short;
        //    m_dpFrom.Control.Width = 95;
        //    m_dpFrom.Control.Refresh();
        //    m_dpFrom.Invalidate();
        //    tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        //    ToolStripLabel tslFrom = new ToolStripLabel("From: ");
        //    tsMain.Items.Insert(tsMain.Items.Count, tslFrom);
        //    tsMain.Items.Insert(tsMain.Items.Count, m_dpFrom);

        //    m_dpThru = new ToolStripControlHost(new DateTimePicker());
        //    m_dpThru.Text = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day, 23, 59, 59).ToString();
        //    ((DateTimePicker)m_dpThru.Control).Format = DateTimePickerFormat.Short;
        //    m_dpThru.Control.Width = 95;
        //    m_dpThru.Control.Refresh();
        //    m_dpThru.Invalidate();

        //    ToolStripLabel tslThru = new ToolStripLabel("Thru: ");
        //    tsMain.Items.Insert(tsMain.Items.Count, tslThru);
        //    tsMain.Items.Insert(tsMain.Items.Count, m_dpThru);
        //    //   tsMain.BackColor = Color.Lavender;

        //    // wdk 20100322 added check box
        //    ToolStripLabel tslInclude = new ToolStripLabel("COLLECTIONS");
        //    m_cboxInclude = new ToolStripControlHost(new CheckBox());
        //    m_cboxInclude.Click += new EventHandler(m_cboxInclude_Click);
        //    tsMain.Items.Insert(tsMain.Items.Count, tslInclude);
        //    tsMain.Items.Insert(tsMain.Items.Count, m_cboxInclude);

        // //   tsMain.Items.Insert(tsMain.Items.Count, new ToolStripSeparator());
        //    tsMain.Refresh();
        //}

        void m_cboxInclude_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tsbSmallBalWriteOff.Enabled = !tsbSmallBalWriteOff.Enabled;
        }
          
        public BadDebtForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
            

            m_strServer = Program.Server; 
            m_strDatabase = Program.Database;
            m_strProductionEnvironment = m_strDatabase; //.Contains("LIVE")? "LIVE":"TEST";

            string[] strArgs = new string[] { m_strProductionEnvironment, m_strServer, m_strDatabase };
            m_Err = new ERR(strArgs);
            m_rChk = new R_chk(m_strServer, m_strDatabase, ref m_Err);
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);
            m_CAcc = new CAcc(m_strServer, m_strDatabase, ref m_Err);
            m_rNotes = new R_notes(m_strServer, m_strDatabase, ref m_Err);
            m_rIns = new R_ins(m_strServer, m_strDatabase, ref m_Err);

            //this.Text += string.Format(" {0}", m_strProductionEnvironment);
        }

        private void frmBadDebt_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //CreateDateTimes();
            m_ViewerPrintDocument = new PrintDocument();
            m_ViewerPrintDocument.DefaultPageSettings.Landscape = false;
            m_rgReport = new ReportGenerator(dgvAccounts, m_ViewerPrintDocument, "BAD DEBT", m_strDatabase);
            m_ViewerPrintDocument.PrintPage += new PrintPageEventHandler(m_rgReport.MyPrintDocument_PrintPage);

        }

        /// <summary>
        /// On 20111101 Ed,BC,Jan and David met in Ed's office the new policy is.
        /// 1. If no payment on account in 120 (4 Month)
        ///     a. send to collections
        ///     b. write off
        ///     
        /// Changes the method now of putting into collections after 3 months with no payment 
        /// and writting off next month.
        /// 
        /// Accounts can no longer be returned from Bad debt. the check needs to be added to the
        /// account Bad_debt or the name de jour'
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tsbWriteOff_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int nUpdated = 0;
            string strBal = null;
            tspbRecords.Minimum = 0;
            tspbRecords.Maximum = dgvAccounts.Rows.Count;

            #region commented out
            //using (SqlConnection conn = new SqlConnection(
            //string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
            //        m_strServer, m_strDatabase)))
            //{
            //    SqlTransaction transaction;
            //    try
            //    {
            //        if (!conn.Open)
            //        {
            //            conn.Open();
            //        }
            //        transaction = conn.BeginTransaction(IsolationLevel.ReadUncommitted);
            //    }
            //    catch (InvalidOperationException ioe)
            //    {
            //        MessageBox.Show(                                                 
            //        string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, ioe.Message)  , propAppName);

            //    }
            //    catch (SqlException se)
            //    {
            //        MessageBox.Show(
            //        string.Format("{0}.\r\n{1}.", MethodBase.GetCurrentMethod().Name, se.Message), propAppName);
            //    }
            //    finally
            //    {
            //        conn.Close();
            //    }

            //}
            //return;

            #endregion

            foreach (DataGridViewRow dr in dgvAccounts.Rows)
            {
                tspbRecords.PerformStep();
                Application.DoEvents();

                if (!string.IsNullOrEmpty(dr.Cells["baddebt_date"].Value.ToString().Trim()))
                {
                    continue;
                }
                
                string strAccount = dr.Cells["account"].Value.ToString().Trim();
                if (string.IsNullOrEmpty(strAccount))
                {
                    continue;
                }
                

                m_CAcc.GetBalance(strAccount, out strBal);
                if (strBal.Contains("ERR"))
                {
                    continue;
                }
                double dBal = double.Parse(strBal);
                if (dBal <= 0.0f)
                {
                    continue;
                }
                // wdk 20111122 In weekly meeting BC said to make the chk record entries.
                // wdk 20111121 No longer really writing off the balances keeping track in 
                // aging as BAD_DEBT and COLLECTIONS.
                // write chk record for balance due as write off with bad debt flagged	
                m_rChk.GetActiveRecords("account = '~'");
                m_rChk.ClearMemberVariables();
                m_rChk.m_strAccount = strAccount;
                m_rChk.m_strAmtPaid = "";
                m_rChk.m_strBadDebt = "TRUE";
                m_rChk.m_strChkDate = DBNull.Value.ToString();
                m_rChk.m_strDateRec = DBNull.Value.ToString();
                m_rChk.m_strChkNo = "";
                m_rChk.m_strComment = "BAD DEBT WRITE OFF";
                m_rChk.m_strContractual = "";
                m_rChk.m_strInvoice = "";
                m_rChk.m_strSource = "BAD_DEBT";
                m_rChk.m_strStatus = "WRITE_OFF";
                m_rChk.m_strWriteOff = dBal.ToString("F2");
                m_rChk.m_strWriteOffDate = DateTime.Today.ToString();
                m_rChk.m_strInsCode =
                    m_rIns.GetActiveRecords(string.Format("Account = '{0}' and ins_a_b_c = 'A'", strAccount)) == 1 ?
                    m_rIns.propIns_code.Trim().ToUpper() : "";
                m_CAcc.LoadAccount(strAccount);
                m_rChk.m_strFinCode = m_CAcc.m_Racc.m_strFinCode;
                    ;
                m_rChk.m_strModPrg = Application.ProductName + Application.ProductVersion;
                int nRec = m_rChk.AddRecord();

                    // update pat
                string strUpdateErr = null;
                    m_rPat.GetActiveRecords("account = '~'");
                    m_rPat.ClearMemberVariables();
                    m_rPat.GetActiveRecords(string.Format("account = '{0}'", strAccount));
                    if (m_rPat.UpdateField("baddebt_date",
                        DateTime.Today.ToString(),
                            string.Format("account = '{0}'", strAccount),
                                out strUpdateErr) > 0)
                    {
                        nUpdated += 1;
                    }
                    else
                    {
                        m_Err.m_Logfile.WriteLogFile(strUpdateErr);
                    }
                       
                        
                // update notes for this account
                m_rNotes.GetRecords(string.Format("account = '{0}'", strAccount));
                m_rNotes.propComment = string.Format("Bad debt set by [{0}]", 
                    System.Environment.UserName);
                m_rNotes.AddRecord(string.Format("account = '{0}'", strAccount));
                  
            }
            MessageBox.Show(string.Format("{0} Pat Records Updated",nUpdated), "POSTING FINISHED");
  
        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            bool sentCollections = false;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item.Name == "readyForCollectionsToolStripMenuItem")
            {
                sentCollections = false;
            }
            else //if(nameof(sender) == "sentToCollectionsToolStripMenuItem")
            {
                sentCollections = true;
            }

            Log.Instance.Trace($"Entering");
            //DateTime dtFrom = ((DateTimePicker)m_dpFrom.Control).Value;
            //DateTime dtThru = ((DateTimePicker)m_dpThru.Control).Value;
            //dtFrom = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 0, 0, 0);
            //dtThru = new DateTime(dtThru.Year, dtThru.Month, dtThru.Day, 23, 59, 59);

            m_dtAccounts = new DataTable("BAD_DEBT");
            m_sdaBadDebt = new SqlDataAdapter();
            using (SqlConnection conn = new SqlConnection(
            string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                    m_strServer, m_strDatabase)))
            {
                string strSelectBadDebt;
                //get last collections sent date
                SqlCommand cmd = new SqlCommand("select max(cast(date_sent as date)) from bad_debt", conn);
                conn.Open();
                var result = cmd.ExecuteScalar();
                DateTime dtSent = (DateTime)result;

                //if (!((CheckBox)m_cboxInclude.Control).Checked)
                if(sentCollections)
                {
                    strSelectBadDebt =
                        string.Format("select datepart(month, date_sent) as [Month], " +
                        "datepart(year, date_sent) as [Year], account_no as [account], " +
                        "debtor_last_name +', '+debtor_first_name as [Debtor Name], " +
                        "convert(datetime,convert(varchar(10),service_date,101)) as [service_date] " +
                        " ,pat.baddebt_date, balance, 0 as [SMALL BAL WRITEOFF], " +
                        "bad_debt.rowguid " +
                        "from bad_debt " +
                        "inner join pat on pat.account = bad_debt.account_no " +
                        "where date_sent between '{0}' and '{1}' " + //and pat.baddebt_date is null "+
                        "order by service_date ",
                        dtSent, dtSent.Date.AddHours(23).AddMinutes(59).AddSeconds(59));
                }
                else
                {
                    strSelectBadDebt =
                    string.Format("select datepart(month, date_sent) as [Month], " +
                    "datepart(year, date_sent) as [Year], account_no as [account], " +
                    "debtor_last_name +', '+debtor_first_name as [Debtor Name], " +
                    "convert(datetime,convert(varchar(10),service_date,101)) as [service_date] " +
                    " ,pat.baddebt_date, balance, 0 as [SMALL BAL WRITEOFF], " +
                    "bad_debt.rowguid " +
                    "from bad_debt " +
                    "inner join pat on pat.account = bad_debt.account_no " +
                    "where date_sent is null " +
                    "order by service_date ");

                    //dtFrom, dtThru);

                }
                // special case processing 
                if (DateTime.Now < new DateTime(2015, 12, 18, 15, 15, 0))
                {
                    strSelectBadDebt = "select datepart(month, date_sent) as [Month] , datepart(year, date_sent) as [Year] , " + 
                        "account_no as [account] , dbo.pat.pat_full_name , debtor_last_name +', '+debtor_first_name as [Debtor Name] ," +
                        "convert(datetime,convert(varchar(10),service_date,101)) as [service_date], pat.baddebt_date  " + 
                        "from pat inner join dbo.bad_debt on bad_debt.account_no = pat.account  " + 
                        "AND bad_debt.date_entered BETWEEN  '2015-10-26 00:00:00.570' AND '2015-10-27 23:59:52.570' " + 
                        "WHERE dbo.pat.bd_list_date = '2015-12-07 00:00:00.000' AND dbo.pat.baddebt_date IS NULL order by dbo.pat.pat_full_name";
                }
                //    string.Format("select " +
                //    " datepart(month, date_sent) as [Month], datepart(year, date_sent) as [Year], " +
                //    " aging_history.account,convert(varchar(10), datestamp,101) as [ah date], " +
                //    " (aging_history.balance) as [ah bal] " +
                //    " 	,convert(varchar(10), date_sent,101) as [collections date] " +
                //    " 	,(bad_debt.balance) as [bd bal] " +
                //    "	, case " +
                //    " 	when convert(varchar(10),coalesce(max(chk.date_rec),max(chk.mod_date)),101) is not null " +
                //    " 	then convert(varchar(10),coalesce(max(chk.date_rec),max(chk.mod_date)),101) " +
                //    " 	else " +
                //    "   convert(varchar(10), date_sent,101) " +
                //    " 	end as [last chk date] " +
                //    " from aging_history " +
                //    " left outer join bad_debt on account_no = account " +
                //    " left outer join chk on chk.account = aging_history.account " +
                //    //" left outer join mcb_report on mcb_report.account = aging_history.account " +
                //    //" where datestamp between '{0}' and '{1}' and  " +
                //    " where date_sent between '{0}' and '{1}' " +
                //    " group by datestamp, convert(datetime,date_sent), aging_history.account,aging_history.balance " +
                //    " ,bad_debt.balance "+ //,mcb_report.date_last_payment, mcb_report.balance " +
                //    " order by coalesce(max(chk.date_rec), max(chk.mod_date), convert(varchar(10), date_sent,101))",
                //    m_dpFrom, m_dpThru);

                SqlCommand cmdSelect = new SqlCommand(strSelectBadDebt, conn);
                m_sdaBadDebt.SelectCommand = cmdSelect;
                m_sdaBadDebt.Fill(m_dtAccounts);
            }
            dgvAccounts.DataSource = m_dtAccounts;

            // remove the accounts that have had a check in the last 120 days
            //int i = 0;
            //foreach (DataRow dr in m_dtAccounts.Rows)
            //{
            //    i++;
            //    if (DateTime.Parse(dr["last chk date"].ToString()) > DateTime.Today.AddDays(-120))
            //    {
            //        dr.Delete();

            //    }
            //}
            //m_dtAccounts.AcceptChanges();
            if (!sentCollections)
            {
                if (dgvAccounts.Columns["dataGridViewDeleteButton"] == null)
                {
                    var deleteButton = new DataGridViewButtonColumn();
                    deleteButton.Name = "dataGridViewDeleteButton";
                    deleteButton.HeaderText = "Delete";
                    deleteButton.Text = "Delete";
                    deleteButton.UseColumnTextForButtonValue = true;
                    dgvAccounts.Columns.Add(deleteButton);
                }
            }

            dgvAccounts.AutoResizeColumns();
            dgvAccounts.Columns["rowguid"].Visible = false;

            ssRecords.Text = string.Format("Records {0}", dgvAccounts.Rows.Count);
            Application.DoEvents();

        }

        private void dgvAccounts_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            //we need to know if the data is from the bad debt table or from Mailer P
            Log.Instance.Trace($"Entering");
            try
            { 
                if(dgvAccounts.Columns.Contains("rowguid"))
                {
                    string selectedGuid = ((DataGridView)sender).Rows[e.RowIndex].Cells["rowguid"].Value.ToString();
                    BadDebtEditForm bdFrm = new BadDebtEditForm(selectedGuid);

                    if (bdFrm.ShowDialog() == DialogResult.OK)
                    {
                        //refresh the list
                        tsbLoad_Click(sender, e);
                    }
                }
                else
                {
                    string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();

                    AccountForm accFrm = new AccountForm(strAcc)
                    {
                        MdiParent = this.ParentForm,
                        WindowState = FormWindowState.Normal,
                        AutoScroll = true
                    };
                    accFrm.Show();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Exception occured trying to open the account.");
            }
        }

        private void dgvAccounts_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ssRecords.Text = string.Format("Records: {0}", dgvAccounts.Rows.Count);
        }

        private void dgvAccounts_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            e.Cancel = true;
        }

        private void tsbLoadMailerP_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            dgvAccounts.Columns.Clear();

            DateTime dtFrom = DateTime.Today; // ((DateTimePicker)m_dpFrom.Control).Value;
            DateTime dtThru = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59); // ((DateTimePicker)m_dpThru.Control).Value;

            m_dtAccounts = new DataTable("BAD_DEBT");
            m_sdaBadDebt = new SqlDataAdapter();
            using (SqlConnection conn = new SqlConnection(
            string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                    m_strServer, m_strDatabase)))
            {

                string strSelectBadDebt =
                    string.Format("with cte as " +
                    "( select pat.account from pat " +
                    " inner join acc on acc.account = pat.account " +
                    " where mailer = 'p' and not acc.status in ('closed','paid_out') ) " +
                    " , ctepay as ( " +
                    " select account, convert(varchar(10),max(mod_date),101) as [last chk date] " +
                    " from chk " +
                    " group by account ) " +
                    " , cteBal as ( " +
                    " select acc.account, cb.total, sum(amt_paid+contractual+write_off)  as [paid] " +
                    " , cb.total-sum(amt_paid+contractual+write_off) as [BALANCE] " +
                    " from vw_chrg_bal cb " +
                    " inner join acc on acc.account = cb.account " +
                    " left outer join chk on chk.account = cb.account " +
                    " where not acc.status in ('closed','Paid_out') " +
                    " group by acc.account, cb.total ) " +
                    " select cte.account, ctePay.[last chk date] " +
                    " , cteBal.[BALANCE] " +
                    " ,datediff(day, ctePay.[last chk date], getdate()) as [Days since payment] " +
                    " from cte " +
                    " left outer join ctePay on ctePay.account = cte.account " +
                    " left outer join cteBal on cteBal.account = cte.account " +
                    " where datediff(day, ctePay.[last chk date], getdate())  > 110 " +
                    " order by datediff(day, ctePay.[last chk date], getdate())"); 
                     //dtFrom, dtThru);

                SqlCommand cmdSelect = new SqlCommand(strSelectBadDebt, conn);
                m_sdaBadDebt.SelectCommand = cmdSelect;
                m_sdaBadDebt.SelectCommand.CommandTimeout = m_sdaBadDebt.SelectCommand.CommandTimeout * 2;
                try
                {
                    m_sdaBadDebt.Fill(m_dtAccounts);
                }
                catch (SqlException )
                {
                    m_sdaBadDebt.SelectCommand.CommandTimeout = m_sdaBadDebt.SelectCommand.CommandTimeout * 2;
                    try
                    {
                        m_sdaBadDebt.Fill(m_dtAccounts);
                    }
                    catch (SqlException se2)
                    {
                        MessageBox.Show(se2.Message, propAppName);
                        return;
                    }
                }

            }
            dgvAccounts.DataSource = m_dtAccounts;

          
            ssRecords.Text = string.Format("Records {0}", dgvAccounts.Rows.Count);

        }

        private void dgvAccounts_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ssRecords.Text = string.Format("Records {0}", dgvAccounts.Rows.Count);
            

            if (e.Button == MouseButtons.Right)
            {
                if (MessageBox.Show("Do you want to hide this column?", "HIDE COLUMN", MessageBoxButtons.YesNo) != DialogResult.Yes)
                {
                    return;
                }
                dgvAccounts.Columns[e.ColumnIndex].Visible = false;
            }
        }

        private void tsbReadMCLFile_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            /*
             * outstring.Format("%-20.20s%-15.15s%-25.25s%-25.25s%-18.18s%-15.15s%-15.15s%-12.12s%-10.10s%-20.20s%-35.35s%-35.35s%-25.25s%-20.20s%-35.35s%-29.29s%-6.6s%-6.6s%-10.10s\n",
				rbad_debt.m_debtor_last_name,       20  
				rbad_debt.m_debtor_first_name,      15  35
				rbad_debt.m_st_addr_1,              25  60
				rbad_debt.m_st_addr_2,              25  85
				rbad_debt.m_city,                   18  103
				rbad_debt.m_state_zip,              15  118
				rbad_debt.m_spouse,                 15  133
				rbad_debt.m_phone,                  12  145
				rbad_debt.m_soc_security,           10  155
				rbad_debt.m_license_number,         20  175
				rbad_debt.m_employment,             35  210
				rbad_debt.m_remarks,                35  245
				rbad_debt.m_account_no,             25  270
				rbad_debt.m_patient_name,           20  290
				rbad_debt.m_remarks2,               35  325
				rbad_debt.m_misc,                   29  354
				service_date,                        6  360
				"",//payment_date,                   6  366
				money(rbad_debt.m_balance));        10  376
			*/
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = @"\\labops1\c:\temp\";
            ofd.Filter = "Text Files|*.txt";
          
            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string[]  alAccounts = 
                RFClassLibrary.RFCObject.GetFileContents(ofd.FileName).Split(new string[] {Environment.NewLine},StringSplitOptions.RemoveEmptyEntries).ToArray();
            
            MessageBox.Show(string.Format("{0} accounts in file",alAccounts.GetUpperBound(0)));
            dgvAccounts.Columns.Add("ACCOUNT", "ACCOUNT");
            dgvAccounts.Columns.Add("PAT NAME", "PAT NAME");
            dgvAccounts.Columns.Add("GUAR NAME", "GUAR NAME");
            foreach(string str in alAccounts)
            {
                string strGuarName = string.Format("{0}, {1}",str.Substring(0,19).Trim(), str.Substring(20,15).Trim());
                string strAccount = str.Substring(240, 25).Trim();
                string strPatName = str.Substring(270, 20).Trim();

                dgvAccounts.Rows.Add(new object[] { strAccount, strPatName, strGuarName });
            }
          
        }

        private void tsbPrintGrid_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int nRows = dgvAccounts.Rows.Count;
            if (nRows == 0)
            {
                MessageBox.Show("No records ready to print.");
                return;
            }
            m_ViewerPrintDocument.Print();

        }

        private void dgvAccounts_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ((DataGridView)sender).Rows[e.RowIndex].HeaderCell.Value = string.Format("{0}", (e.RowIndex + 1));
        }

        private void dgvAccounts_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            // Numbers the rows
            ((DataGridView)sender).Rows[e.RowIndex].HeaderCell.Value = string.Format("{0}", (e.RowIndex + 1));
        }

        private void GeneratePatientBillsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Application.DoEvents();
            PatBillForm pb = new PatBillForm();
            pb.m_strServer = m_strServer;
            pb.m_strDatabase = m_strDatabase;
            pb.ShowDialog();
            return;
           
        }

        private void TsmiSelectAccounts_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            return;
           // int nRows = 0;
           // Exception myException = new Exception("Message Initialized and Upload failed."); 
               
           //using (SqlConnection conn = new SqlConnection(
           //string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'; "+
           // "Connection Timeout = 600",
           //        m_strServer, m_strDatabase)))
           // {
           //    conn.Open();
           //    SqlCommand command = conn.CreateCommand();
           //    SqlTransaction transaction =conn.BeginTransaction();
           //    command.Connection = conn;
           //    command.Transaction = transaction;

           //    try
           //    {
           //        DateTime batchDate = new DateTime(
           //            DateTime.Today.AddMonths(-1).Year
           //            , DateTime.Today.AddMonths(-1).Month
           //            , DateTime.Today.AddDays((DateTime.Today.Day * -1)).Day
           //            , 23, 59, 59);
           //        command.CommandText = string.Format("exec dbo.usp_prg_pat_bill_acct "+
           //            "@batchDate = '{0}', @endDate = '{1}',  " +
           //            "@batch = '{2}{3}'"
           //            , DateTime.Today.ToString("d")
           //            , batchDate
           //            , batchDate.ToString("yyyy"), batchDate.ToString("MM"));

           //        nRows = command.ExecuteNonQuery();

           //        transaction.Commit();
           //    }
           //    catch (SqlException sqe)
           //    {
           //        myException = sqe;
           //        //MessageBox.Show(sqe.Message, sqe.GetType().ToString());
           //        transaction.Rollback();
           //    }
           //    catch (Exception ex)
           //    {
           //        myException =  ex;
           //        MessageBox.Show(ex.Message, ex.GetType().ToString());
           //        transaction.Rollback();
           //    }
           //     finally
           //     {
                    
           //         if (conn.State == ConnectionState.Open)
           //         {
           //             conn.Close();
           //         }
           //     }
           // }

           // if (!string.IsNullOrEmpty(myException.GetType().ToString()))
           // {
           //   MessageBox.Show(myException.Message, myException.GetType().ToString());
           // }
           // else
           // {
           //     MessageBox.Show(string.Format("{0) Accounts loaded.",nRows),"FINISHED UPLOAD");
           // }
           

        }

        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //int x;
            //x = 9;
        }

        private void TsbSmallBalWriteOff_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (DateTime.Now > new DateTime(2016, 06, 14, 16, 0, 0))
            {
                MessageBox.Show("Function not available at this time.", "PROCESSING INVALID");
                return;
            }

            foreach (DataGridViewRow dr in dgvAccounts.Rows)
            {
                int nUpdated = 0;
                Application.DoEvents();
                if (dr.IsNewRow)
                {
                    return;
                }
                double dBal = double.Parse(dr.Cells["balance"].Value.ToString());
                if (dBal <= 10.0f)
                {
                    continue;
                }
                string strAccount = dr.Cells["account"].Value.ToString();
                m_rChk.ClearMemberVariables();
                int nRec = m_rChk.GetActiveRecords(
                    string.Format("account = '{0}' and bad_debt <> 0", strAccount));
                if (nRec == 0)
                {
                    m_rChk.GetActiveRecords("account = '~'");
                    m_rChk.ClearMemberVariables();
                    m_rChk.m_strAccount = strAccount;
                    m_rChk.m_strAmtPaid = "";
                    m_rChk.m_strBadDebt = "TRUE";
                    m_rChk.m_strChkDate = DBNull.Value.ToString();
                    m_rChk.m_strDateRec = DBNull.Value.ToString();
                    m_rChk.m_strChkNo = "";
                    m_rChk.m_strComment = "BAD DEBT WRITE OFF";
                    m_rChk.m_strContractual = "";
                    m_rChk.m_strInvoice = "";
                    m_rChk.m_strSource = "BAD_DEBT";
                    m_rChk.m_strStatus = "WRITE_OFF";
                    m_rChk.m_strWriteOff = dBal.ToString("F2");
                    m_rChk.m_strWriteOffDate = DateTime.Today.ToString();
                    m_rChk.m_strInsCode =
                        m_rIns.GetActiveRecords(string.Format("Account = '{0}' and ins_a_b_c = 'A'", strAccount)) == 1 ?
                        m_rIns.propIns_code.Trim().ToUpper() : "";
                    m_CAcc.LoadAccount(strAccount);
                    m_rChk.m_strFinCode = m_CAcc.m_Racc.m_strFinCode;

                    m_rChk.m_strModPrg = propAppName;
                    int nChk = m_rChk.AddRecord();

                    // update pat
                    string strUpdateErr = null;
                    m_rPat.GetActiveRecords("account = '~'");
                    m_rPat.ClearMemberVariables();
                    m_rPat.GetActiveRecords(string.Format("account = '{0}'", strAccount));
                    if (m_rPat.UpdateField("baddebt_date",
                        DateTime.Today.ToString(),
                            string.Format("account = '{0}'", strAccount),
                                out strUpdateErr) > 0)
                    {
                        nUpdated += 1;
                    }
                    else
                    {
                        m_Err.m_Logfile.WriteLogFile(strUpdateErr);
                    }


                    // update notes for this account
                    m_rNotes.GetRecords(string.Format("account = '{0}'", strAccount));
                    m_rNotes.propComment = string.Format("Bad debt set by [{0}]",
                        System.Environment.UserName);
                    m_rNotes.AddRecord(string.Format("account = '{0}'", strAccount));
                    //m_rChk.AddRecord(
                }
                /*
                 * m_rChk = new R_chk(m_strServer, m_strDatabase, ref m_Err);
                    m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);
                    m_CAcc = new CAcc(m_strServer, m_strDatabase, ref m_Err);
                    m_rNotes = new R_notes(m_strServer, m_strDatabase, ref m_Err);
                    m_rIns = new R_ins(m_strServer, m_strDatabase, ref m_Err);

                 * */
            }
            MessageBox.Show("Processing small balance write_off is complete.", "SMALL BALANCE");
        }

        private void GenerateCollectionsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            string filename = string.Format(@"c:\temp\MCL{0}{1}{2}.txt", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                using (SqlConnection conn = new SqlConnection(
               string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                       m_strServer, m_strDatabase)))
                {
                    conn.Open();
                    //SqlDataAdapter sda = new SqlDataAdapter(conn);

                    SqlCommand cmdSelect = new SqlCommand("select * from bad_debt where date_sent IS NULL", conn);

                    SqlDataReader dr = cmdSelect.ExecuteReader();

                    if(dr.HasRows)
                    {
                        while(dr.Read())
                        {
                            FixedFileLine ffl = new RFClassLibrary.FixedFileLine(19);
                            ffl.SetField(1, 20, dr["debtor_last_name"] as string);
                            ffl.SetField(2, 15, dr["debtor_first_name"] as string);
                            ffl.SetField(3, 25, dr["st_addr_1"] as string);                            
                            ffl.SetField(4, 25, dr["st_addr_2"] as string);
                            ffl.SetField(5, 18, dr["city"] as string);
                            ffl.SetField(6, 15, dr["state_zip"] as string);
                            ffl.SetField(7, 15, dr["spouse"] as string);
                            ffl.SetField(8, 12, dr["phone"] as string);
                            ffl.SetField(9, 10, dr["soc_security"] as string);
                            ffl.SetField(10, 20, dr["license_number"] as string);
                            ffl.SetField(11, 35, dr["employment"] as string);
                            ffl.SetField(12, 35, dr["remarks"] as string);
                            ffl.SetField(13, 25, dr["account_no"] as string);
                            ffl.SetField(14, 20, dr["patient_name"] as string);
                            ffl.SetField(15, 35, dr["remarks2"] as string);
                            ffl.SetField(16, 29, dr["misc"] as string);
                            DateTime svcDate = (DateTime)dr["service_date"];
                            ffl.SetField(17, 6, svcDate.ToString("MMddyy")); //mmddyy
                            ffl.SetField(18, 6, ""); //was payment date- removed to prevent file read errors by collection agency
                            decimal balance = (decimal)dr["balance"];
                            ffl.SetField(19, 10, balance.ToString()); //balance

                            file.WriteLine(ffl.OutputLine());


                            using (SqlConnection conn2 = new SqlConnection(
                                    string.Format("Data Source={0}; Initial Catalog = {1}; Integrated Security = 'SSPI'",
                                    m_strServer, m_strDatabase)))
                            {
                                //update the date sent field in the database record
                                conn2.Open();
                                string updateCmd = $"update bad_debt set date_sent = @date_sent where rowguid = @rowguid";
                                SqlCommand cmdUpdate = new SqlCommand(updateCmd, conn2);
                                cmdUpdate.Parameters.AddWithValue("@date_sent", DateTime.Today);
                                cmdUpdate.Parameters.AddWithValue("@rowguid", (Guid)dr["rowguid"]);

                                Int32 affected = cmdUpdate.ExecuteNonQuery();
                                if (affected <= 0)
                                {
                                    //error updating row

                                }
                                conn2.Close();
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No data to send.");
                    }
                    dr.Close();
                    conn.Close();
                }
            }
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            return;


            //if (e.RowIndex == dgvAccounts.NewRowIndex || e.RowIndex < 0)
            //    return;

            //if (e.ColumnIndex < 0)
            //    return;

            ////check if click is on specific column
            //if(e.ColumnIndex == dgvAccounts.Columns["dataGridViewDeleteButton"].Index)
            //{
            //    //delete the row from the database and grid
            //    using (SqlConnection conn = new SqlConnection(Helper.ConnVal))
            //    {
            //        if(MessageBox.Show("Are you sure?", "Delete Row",MessageBoxButtons.YesNo) == DialogResult.Yes)
            //        {
            //            SqlCommand cmdDelete = new SqlCommand("delete from bad_debt where rowguid = @rowguid", conn);
            //            cmdDelete.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = m_dtAccounts.Columns["rowguid"];
            //            cmdDelete.Parameters["@rowguid"].SourceColumn = "rowguid";

            //            m_sdaBadDebt.DeleteCommand = cmdDelete;

            //            DataRow dr = m_dtAccounts.Rows[e.RowIndex];
            //            dr.Delete();

            //            m_sdaBadDebt.Update(m_dtAccounts);
            //        }
            //    }
            //}    
        }

        private void dgvAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //button clicked
                //delete the row from the database and grid
                using (SqlConnection conn = new SqlConnection(Helper.ConnVal))
                {
                    if (MessageBox.Show("Are you sure?", "Delete Row", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SqlCommand cmdDelete = new SqlCommand("delete from bad_debt where rowguid = @rowguid", conn);
                        cmdDelete.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = m_dtAccounts.Columns["rowguid"];
                        cmdDelete.Parameters["@rowguid"].SourceColumn = "rowguid";

                        m_sdaBadDebt.DeleteCommand = cmdDelete;

                        DataRow dr = m_dtAccounts.Rows[e.RowIndex];
                        dr.Delete();

                        m_sdaBadDebt.Update(m_dtAccounts);
                    }
                }

            }
        }

        private void dgvAccounts_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            dgvAccounts_RowHeaderMouseDoubleClick(sender, e);
        }
    }
}
