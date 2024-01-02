using System;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;
// programmer added
using RFClassLibrary;
using MCL;
using System.Drawing.Printing;
using LabBilling.Logging;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using System.Collections.Generic;

namespace LabBilling.Forms
{
    public partial class PatientCollectionsForm : Form
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
        PatRepository patRepository;
        ChkRepository chkRepository;
        AccountRepository accountRepository;
        AccountNoteRepository accountNoteRepository;
        InsRepository insRepository;
        Account acc;


        void m_cboxInclude_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tsbSmallBalWriteOff.Enabled = !tsbSmallBalWriteOff.Enabled;
        }

        public PatientCollectionsForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();

            m_strServer = Program.AppEnvironment.ServerName;
            m_strDatabase = Program.AppEnvironment.DatabaseName;
            m_strProductionEnvironment = m_strDatabase; //.Contains("LIVE")? "LIVE":"TEST";

            string[] strArgs = new string[] { m_strProductionEnvironment, m_strServer, m_strDatabase };
            m_Err = new ERR(strArgs);
            m_rChk = new R_chk(m_strServer, m_strDatabase, ref m_Err);
            m_rPat = new R_pat(m_strServer, m_strDatabase, ref m_Err);
            m_CAcc = new CAcc(m_strServer, m_strDatabase, ref m_Err);
            m_rNotes = new R_notes(m_strServer, m_strDatabase, ref m_Err);
            m_rIns = new R_ins(m_strServer, m_strDatabase, ref m_Err);

            //this.Text += string.Format(" {0}", m_strProductionEnvironment);
            accountRepository = new AccountRepository(Program.AppEnvironment);
            patRepository = new PatRepository(Program.AppEnvironment);
            accountNoteRepository = new AccountNoteRepository(Program.AppEnvironment);
            chkRepository = new ChkRepository(Program.AppEnvironment);
            insRepository = new InsRepository(Program.AppEnvironment);

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
                acc = accountRepository.GetByAccount(strAccount);

                Chk chk = new Chk();
                chk.AccountNo = strAccount;
                chk.PaidAmount = 0.00;
                chk.IsCollectionPmt = true;
                chk.Comment = "BAD DEBT WRITE OFF";
                chk.ContractualAmount = 0.00;
                chk.Source = "BAD_DEBT";
                chk.Status = "WRITE_OFF";
                chk.WriteOffAmount = dBal;
                chk.WriteOffDate = DateTime.Today;
                chk.InsCode =
                    m_rIns.GetActiveRecords(string.Format("Account = '{0}' and ins_a_b_c = 'A'", strAccount)) == 1 ?
                    m_rIns.propIns_code.Trim().ToUpper() : "";
                
                chk.FinCode = m_CAcc.m_Racc.m_strFinCode;
                chkRepository.Add(chk);

                // update pat
                acc.Pat.BadDebtListDate = DateTime.Today;

                if(patRepository.Update(acc.Pat, new[] { nameof(Pat.BadDebtListDate) }))
                {
                    nUpdated++;
                }

                // update notes for this account
                accountRepository.AddNote(strAccount, $"Bad debt set by [{Program.AppEnvironment.UserName}]");
            }
            MessageBox.Show(string.Format("{0} Pat Records Updated", nUpdated), "POSTING FINISHED");

        }

        private void tsbLoad_Click(object sender, EventArgs e)
        {
            bool sentCollections = false;

            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            if (item.Name == "readyForCollectionsToolStripMenuItem")
            {
                sentCollections = false;
                tsbSmallBalWriteOff.Enabled = false;
                tsbWriteOff.Enabled = false;
            }
            else //if(nameof(sender) == "sentToCollectionsToolStripMenuItem")
            {
                sentCollections = true;
                tsbSmallBalWriteOff.Enabled = true;
                tsbWriteOff.Enabled = true;
            }

            Log.Instance.Trace($"Entering");
            //DateTime dtFrom = ((DateTimePicker)m_dpFrom.Control).Value;
            //DateTime dtThru = ((DateTimePicker)m_dpThru.Control).Value;
            //dtFrom = new DateTime(dtFrom.Year, dtFrom.Month, dtFrom.Day, 0, 0, 0);
            //dtThru = new DateTime(dtThru.Year, dtThru.Month, dtThru.Day, 23, 59, 59);

            m_dtAccounts = new DataTable("BAD_DEBT");
            m_sdaBadDebt = new SqlDataAdapter();
            using (SqlConnection conn = new(Program.AppEnvironment.ConnectionString))
            {
                string strSelectBadDebt;
                //get last collections sent date
                SqlCommand cmd = new("select max(cast(date_sent as date)) from bad_debt", conn);
                conn.Open();
                var result = cmd.ExecuteScalar();
                DateTime dtSent = (DateTime)result;

                if (sentCollections)
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

                SqlCommand cmdSelect = new SqlCommand(strSelectBadDebt, conn);
                m_sdaBadDebt.SelectCommand = cmdSelect;
                m_sdaBadDebt.Fill(m_dtAccounts);
            }
            dgvAccounts.DataSource = m_dtAccounts;

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
                if (dgvAccounts.Columns.Contains("rowguid"))
                {
                    string selectedGuid = ((DataGridView)sender).Rows[e.RowIndex].Cells["rowguid"].Value.ToString();
                    PatientCollectionsEditForm bdFrm = new PatientCollectionsEditForm(selectedGuid);

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

            DateTime dtFrom = DateTime.Today; 
            DateTime dtThru = DateTime.Today.AddHours(23).AddMinutes(59).AddSeconds(59);

            m_dtAccounts = new DataTable("BAD_DEBT");
            m_sdaBadDebt = new SqlDataAdapter();
            using (SqlConnection conn = new SqlConnection(Program.AppEnvironment.ConnectionString))
            {

                string strSelectBadDebt =
                    string.Format("with cte as " +
                    "( select pat.account from pat " +
                    " inner join acc on acc.account = pat.account " +
                    " where mailer = 'P' and not acc.status in ('CLOSED','PAID_OUT') ) " +
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
                catch (SqlException)
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

            OpenFileDialog ofd = new();
            ofd.InitialDirectory = @"c:\temp\";
            ofd.Filter = "Text Files|*.txt";

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string[] alAccounts =
                RFClassLibrary.RFCObject.GetFileContents(ofd.FileName).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray();

            MessageBox.Show(string.Format("{0} accounts in file", alAccounts.GetUpperBound(0)));
            dgvAccounts.Columns.Add("ACCOUNT", "ACCOUNT");
            dgvAccounts.Columns.Add("PAT NAME", "PAT NAME");
            dgvAccounts.Columns.Add("GUAR NAME", "GUAR NAME");
            foreach (string str in alAccounts)
            {
                string strGuarName = string.Format("{0}, {1}", str.Substring(0, 19).Trim(), str.Substring(20, 15).Trim());
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
                int nRec = m_rChk.GetActiveRecords(
                    string.Format("account = '{0}' and bad_debt <> 0", strAccount));

                acc = accountRepository.GetByAccount(strAccount);
                List<Chk> chks = chkRepository.GetByAccount(strAccount).Where(x => x.IsCollectionPmt != false).ToList();

                if (chks.Count == 0)
                {
                    Chk chk = new Chk();
                    chk.AccountNo = strAccount;
                    chk.IsCollectionPmt = true;
                    chk.Comment = "BAD DEBT WRITE OFF";
                    chk.Source = "BAD_DEBT";
                    chk.Status = "WRITE_OFF";
                    chk.WriteOffAmount = dBal;
                    chk.WriteOffDate = DateTime.Today;
                    chk.InsCode = acc.PrimaryInsuranceCode;
                    chk.FinCode = acc.FinCode;
                    chkRepository.Add(chk);

                    // update pat
                    acc.Pat.BadDebtListDate = DateTime.Today;
                    if(patRepository.Update(acc.Pat, new[] { nameof(Pat.BadDebtListDate) }))
                    {
                        nUpdated++;
                    }

                    accountRepository.AddNote(strAccount, $"Bad debt set by [{Program.AppEnvironment.UserName}]");
                }
            }
            MessageBox.Show("Processing small balance write_off is complete.", "SMALL BALANCE");
        }

        private void GenerateCollectionsFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            string filename = string.Format(@"c:\temp\MCL{0}{1}{2}.txt", DateTime.Today.Year, DateTime.Today.Month, DateTime.Today.Day);
            using (System.IO.StreamWriter file = new System.IO.StreamWriter(filename))
            {
                using (SqlConnection conn = new SqlConnection(Program.AppEnvironment.ConnectionString))
                {
                    conn.Open();
                    SqlCommand cmdSelect = new SqlCommand("select * from bad_debt where date_sent IS NULL", conn);
                    SqlDataReader dr = cmdSelect.ExecuteReader();

                    if (dr.HasRows)
                    {
                        while (dr.Read())
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


                            using (SqlConnection conn2 = new SqlConnection(Program.AppEnvironment.ConnectionString))
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
                using (SqlConnection conn = new SqlConnection(Program.AppEnvironment.ConnectionString))
                {
                    if (MessageBox.Show("Are you sure?", "Delete Row", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        SqlCommand cmdDelete = new("delete from bad_debt where rowguid = @rowguid", conn);
                        cmdDelete.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = m_dtAccounts.Columns["rowguid"];
                        cmdDelete.Parameters["@rowguid"].SourceColumn = "rowguid";

                        m_sdaBadDebt.DeleteCommand = cmdDelete;

                        DataRow dr = m_dtAccounts.AsEnumerable().SingleOrDefault(b => b.Field<string>("account") == dgvAccounts["account", e.RowIndex].Value.ToString());
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

        private void patientStatementsWizardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PatientCollectionsRunWizard frm = new PatientCollectionsRunWizard();

            frm.ShowDialog();

        }
    }
}
