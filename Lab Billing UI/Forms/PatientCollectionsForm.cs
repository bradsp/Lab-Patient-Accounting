using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using MCL;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Printing;
using System.Linq;
using System.Windows.Forms;
// programmer added
using Utilities;

namespace LabBilling.Forms
{
    public partial class PatientCollectionsForm : Form
    {
        private string propAppName
        { get { return string.Format("{0} {1}", Application.ProductName, Application.ProductVersion); } }

        private PrintDocument m_ViewerPrintDocument;
        private ReportGenerator m_rgReport;

        private R_notes _rNotes = null;
        private R_ins _rIns = null;
        private R_pat _rPat = null;
        private R_chk _rChk = null;
        private CAcc _cAcc = null;
        private ERR _err = null;
        private string _strServer = null;
        private string _strDatabase = null;
        private string _strProductionEnvironment = null;
        private DataTable m_dtAccounts;
        private SqlDataAdapter _sdaBadDebt;
        private Account _account;

        private AccountService _accountService;
        private PatientBillingService _patientBillingService;
        private DictionaryService _dictionaryService;

        public event EventHandler<string> AccountLaunched;

        void m_cboxInclude_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tsbSmallBalWriteOff.Enabled = !tsbSmallBalWriteOff.Enabled;
        }

        public PatientCollectionsForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();

            _strServer = Program.AppEnvironment.ServerName;
            _strDatabase = Program.AppEnvironment.DatabaseName;
            _strProductionEnvironment = _strDatabase;

            string[] strArgs = new string[] { _strProductionEnvironment, _strServer, _strDatabase };
            _err = new ERR(strArgs);
            _rChk = new R_chk(_strServer, _strDatabase, ref _err);
            _rPat = new R_pat(_strServer, _strDatabase, ref _err);
            _cAcc = new CAcc(_strServer, _strDatabase, ref _err);
            _rNotes = new R_notes(_strServer, _strDatabase, ref _err);
            _rIns = new R_ins(_strServer, _strDatabase, ref _err);

            _accountService = new(Program.AppEnvironment);
            _patientBillingService = new(Program.AppEnvironment);
            _dictionaryService = new(Program.AppEnvironment);
        }

        private void frmBadDebt_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //CreateDateTimes();
            m_ViewerPrintDocument = new PrintDocument();
            m_ViewerPrintDocument.DefaultPageSettings.Landscape = false;
            m_rgReport = new ReportGenerator(dgvAccounts, m_ViewerPrintDocument, "BAD DEBT", _strDatabase);
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

                _cAcc.GetBalance(strAccount, out strBal);
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
                _account = _accountService.GetAccount(strAccount);

                Chk chk = new()
                {
                    AccountNo = strAccount,
                    PaidAmount = 0.00,
                    IsCollectionPmt = true,
                    Comment = "BAD DEBT WRITE OFF",
                    ContractualAmount = 0.00,
                    Source = "BAD_DEBT",
                    Status = "WRITE_OFF",
                    WriteOffAmount = dBal,
                    WriteOffDate = DateTime.Today,
                    InsCode =
                    _rIns.GetActiveRecords(string.Format("Account = '{0}' and ins_a_b_c = 'A'", strAccount)) == 1 ?
                    _rIns.propIns_code.Trim().ToUpper() : "",
                    FinCode = _cAcc.m_Racc.m_strFinCode
                };
                _accountService.AddPayment(chk);

                // update pat
                _account.Pat.BadDebtListDate = DateTime.Today;

                try
                {
                    _account.Pat = _accountService.SetCollectionsDate(_account.Pat);
                    nUpdated++;
                }
                catch (ApplicationException apex)
                {
                    Log.Instance.Error(apex.Message, apex);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex.Message, ex);
                }

                // update notes for this account
                _account.Notes = _accountService.AddNote(strAccount, $"Bad debt set by [{Program.AppEnvironment.UserName}]").ToList();
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
            _sdaBadDebt = new SqlDataAdapter();
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

                SqlCommand cmdSelect = new(strSelectBadDebt, conn);
                _sdaBadDebt.SelectCommand = cmdSelect;
                _sdaBadDebt.Fill(m_dtAccounts);
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
                    PatientCollectionsEditForm bdFrm = new(selectedGuid);

                    if (bdFrm.ShowDialog() == DialogResult.OK)
                    {
                        //refresh the list
                        tsbLoad_Click(sender, e);
                    }
                }
                else
                {
                    string strAcc = ((DataGridView)sender).Rows[e.RowIndex].Cells["account"].Value.ToString();

                    AccountLaunched?.Invoke(this, strAcc);
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
            _sdaBadDebt = new SqlDataAdapter();
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
                _sdaBadDebt.SelectCommand = cmdSelect;
                _sdaBadDebt.SelectCommand.CommandTimeout = _sdaBadDebt.SelectCommand.CommandTimeout * 2;
                try
                {
                    _sdaBadDebt.Fill(m_dtAccounts);
                }
                catch (SqlException)
                {
                    _sdaBadDebt.SelectCommand.CommandTimeout = _sdaBadDebt.SelectCommand.CommandTimeout * 2;
                    try
                    {
                        _sdaBadDebt.Fill(m_dtAccounts);
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
                    return;
                dgvAccounts.Columns[e.ColumnIndex].Visible = false;
            }
        }

        private void tsbReadMCLFile_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            OpenFileDialog ofd = new()
            {
                InitialDirectory = @"c:\temp\",
                Filter = "Text Files|*.txt"
            };

            if (ofd.ShowDialog() == DialogResult.Cancel)
            {
                return;
            }
            string[] alAccounts =
                Utilities.RFCObject.GetFileContents(ofd.FileName).Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries).ToArray();

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

        private void DgvAccounts_SelectionChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

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
                int nRec = _rChk.GetActiveRecords(
                    string.Format("account = '{0}' and bad_debt <> 0", strAccount));

                _account = _accountService.GetAccount(strAccount);
                List<Chk> chks = _account.Payments.Where(x => x.IsCollectionPmt != false).ToList();

                if (chks.Count == 0)
                {
                    Chk chk = new()
                    {
                        AccountNo = strAccount,
                        IsCollectionPmt = true,
                        Comment = "BAD DEBT WRITE OFF",
                        Source = "BAD_DEBT",
                        Status = "WRITE_OFF",
                        WriteOffAmount = dBal,
                        WriteOffDate = DateTime.Today,
                        InsCode = _account.PrimaryInsuranceCode,
                        FinCode = _account.FinCode
                    };
                    _accountService.AddPayment(chk);

                    try
                    {
                        // update pat
                        _account.Pat.BadDebtListDate = DateTime.Today;
                        _account.Pat = _accountService.SetCollectionsDate(_account.Pat);
                        nUpdated++;
                        _account.Notes = _accountService.AddNote(strAccount, $"Bad debt set by [{Program.AppEnvironment.UserName}]").ToList();
                    }
                    catch (ApplicationException apex)
                    {
                        Log.Instance.Error(apex.Message, apex);
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.Error(ex.Message, ex);
                    }
                }
            }
            MessageBox.Show("Processing small balance write_off is complete.", "SMALL BALANCE");
        }

        private void dgvAccounts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            return;
        }

        private void dgvAccounts_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //button clicked
                //delete the row from the database and grid
                using SqlConnection conn = new(Program.AppEnvironment.ConnectionString);

                if (MessageBox.Show("Are you sure?", "Delete Row", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    try
                    {
                        SqlCommand cmdDelete = new("delete from bad_debt where rowguid = @rowguid", conn);
                        cmdDelete.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = m_dtAccounts.Columns["rowguid"];
                        cmdDelete.Parameters["@rowguid"].SourceColumn = "rowguid";

                        _sdaBadDebt.DeleteCommand = cmdDelete;
                        string accountNo = dgvAccounts["account", e.RowIndex].Value.ToString();
                        DataRow dr = m_dtAccounts.AsEnumerable().SingleOrDefault(b => b.Field<string>("account") == accountNo);

                        dr.Delete();

                        _sdaBadDebt.Update(m_dtAccounts);

                        //remove bd_list_date from pat record
                        _accountService.ClearCollectionsListDate(accountNo);
                    }
                    catch(Exception ex)
                    {
                        //TODO: finish
                        Log.Instance.Error(ex);
                        MessageBox.Show("Error encountered removing account from Collections list. Try again or report to support.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
