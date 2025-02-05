using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Drawing.Printing;
// programmer added
using Utilities;
using WinFormsLibrary;

namespace LabBilling.Forms;

public partial class PatientCollectionsForm : Form
{
    private static string AppName
    { get { return $"{Application.ProductName} {Application.ProductVersion}"; }  }

    private DataTable _dtAccounts;
    private SqlDataAdapter _sdaBadDebt;
    private Account _account;

    private readonly AccountService _accountService;
    private readonly PatientBillingService _patientBillingService;
    private readonly DictionaryService _dictionaryService;

    public event EventHandler<string> AccountLaunched;
    private void CboxInclude_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        smallBalanceWriteoffToolStripButton.Enabled = !smallBalanceWriteoffToolStripButton.Enabled;
    }

    public PatientCollectionsForm()
    {
        Log.Instance.Trace($"Entering");
        InitializeComponent();

        _accountService = new(Program.AppEnvironment);
        _patientBillingService = new(Program.AppEnvironment);
        _dictionaryService = new(Program.AppEnvironment);
    }

    private void frmBadDebt_Load(object sender, EventArgs e)
    {
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
    private void SmallBalanceWriteOffToolStripButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        int nUpdated = 0;
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
            _account = _accountService.GetAccount(strAccount);

            if (_account.Balance <= 0.0f)
            {
                continue;
            }
            // wdk 20111122 In weekly meeting BC said to make the chk record entries.
            // wdk 20111121 No longer really writing off the balances keeping track in 
            // aging as BAD_DEBT and COLLECTIONS.
            // write chk record for balance due as write off with bad debt flagged	

            Chk chk = new()
            {
                AccountNo = strAccount,
                PaidAmount = 0.00,
                IsCollectionPmt = true,
                Comment = "BAD DEBT WRITE OFF",
                ContractualAmount = 0.00,
                Source = "BAD_DEBT",
                Status = "WRITE_OFF",
                WriteOffAmount = _account.Balance,
                WriteOffDate = DateTime.Today,
                InsCode = _account.PrimaryInsuranceCode,
                FinCode = _account.FinCode,
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
        ToolStripMenuItem item = (ToolStripMenuItem)sender;

        if (item.Name == nameof(readyForCollectionsToolStripMenuItem))
        {
            smallBalanceWriteoffToolStripButton.Enabled = false;
            writeOffToolStripButton.Enabled = false;
        }
        else //if(nameof(sender) == "sentToCollectionsToolStripMenuItem")
        {
            smallBalanceWriteoffToolStripButton.Enabled = true;
            writeOffToolStripButton.Enabled = true;
        }

        Log.Instance.Trace($"Entering");

        _dtAccounts = new DataTable("BAD_DEBT");
        _sdaBadDebt = new SqlDataAdapter();
        using (SqlConnection conn = new(Program.AppEnvironment.ConnectionString))
        {
            string strSelectBadDebt;
            //get last collections sent date
            SqlCommand cmd = new("select max(cast(date_sent as date)) from bad_debt", conn);
            conn.Open();
            var result = cmd.ExecuteScalar();
            DateTime dtSent = (DateTime)result;

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

            SqlCommand cmdSelect = new(strSelectBadDebt, conn);
            _sdaBadDebt.SelectCommand = cmdSelect;
            _sdaBadDebt.Fill(_dtAccounts);
        }
        dgvAccounts.DataSource = _dtAccounts;

        if (!false)
        {
            if (dgvAccounts.Columns["dataGridViewDeleteButton"] == null)
            {
                var deleteButton = new DataGridViewButtonColumn
                {
                    Name = "dataGridViewDeleteButton",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true
                };
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

        _dtAccounts = new DataTable("BAD_DEBT");
        _sdaBadDebt = new SqlDataAdapter();
        using (SqlConnection conn = new(Program.AppEnvironment.ConnectionString))
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

            SqlCommand cmdSelect = new(strSelectBadDebt, conn);
            _sdaBadDebt.SelectCommand = cmdSelect;
            _sdaBadDebt.SelectCommand.CommandTimeout = _sdaBadDebt.SelectCommand.CommandTimeout * 2;
            try
            {
                _sdaBadDebt.Fill(_dtAccounts);
            }
            catch (SqlException)
            {
                _sdaBadDebt.SelectCommand.CommandTimeout = _sdaBadDebt.SelectCommand.CommandTimeout * 2;
                try
                {
                    _sdaBadDebt.Fill(_dtAccounts);
                }
                catch (SqlException se2)
                {
                    MessageBox.Show(se2.Message, AppName);
                    return;
                }
            }

        }
        dgvAccounts.DataSource = _dtAccounts;

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
        //_viewerPrintDocument.Print();

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
            string strAccount = dr.Cells["account"].Value.ToString().Trim();
            _account = _accountService.GetAccount(strAccount);

            if (_account.Balance <= 10.0f)
            {
                continue;
            }

            int nRec = _account.Payments.Where(x => x.IsCollectionPmt).Count();

            List<Chk> chks = _account.Payments.Where(x => x.IsCollectionPmt).ToList();

            if (chks.Count == 0)
            {
                Chk chk = new()
                {
                    AccountNo = strAccount,
                    IsCollectionPmt = true,
                    Comment = "BAD DEBT WRITE OFF",
                    Source = "BAD_DEBT",
                    Status = "WRITE_OFF",
                    WriteOffAmount = _account.Balance,
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
                    cmdDelete.Parameters.Add("@rowguid", SqlDbType.UniqueIdentifier).Value = _dtAccounts.Columns["rowguid"];
                    cmdDelete.Parameters["@rowguid"].SourceColumn = "rowguid";

                    _sdaBadDebt.DeleteCommand = cmdDelete;
                    string accountNo = dgvAccounts["account", e.RowIndex].Value.ToString();
                    DataRow dr = _dtAccounts.AsEnumerable().SingleOrDefault(b => b.Field<string>("account") == accountNo);

                    dr.Delete();

                    _sdaBadDebt.Update(_dtAccounts);

                    //remove bd_list_date from pat record
                    _accountService.ClearCollectionsListDate(accountNo);
                }
                catch (Exception ex)
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
