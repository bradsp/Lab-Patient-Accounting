using LabBilling.Core.Models;
using LabBilling.Core.Services;

namespace LabBilling.Forms;
public partial class AccountLocksForm : Form
{
    private readonly AccountService _accountService;
    public AccountLocksForm()
    {
        _accountService = new(Program.AppEnvironment, Program.UnitOfWork);
        InitializeComponent();
    }

    private void AccountLocksForm_Load(object sender, EventArgs e)
    {
        accountLockDataGrid.DataSource = _accountService.GetAccountLocks();
        accountLockDataGrid.Columns[nameof(AccountLock.id)].Visible = false;
        accountLockDataGrid.Columns[nameof(AccountLock.rowguid)].Visible = false;
        accountLockDataGrid.Columns[nameof(AccountLock.UpdatedHost)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        accountLockDataGrid.AutoResizeColumns();
    }

    private void accountLockDataGrid_SelectionChanged(object sender, EventArgs e)
    {
        if (accountLockDataGrid.SelectedRows.Count > 0)
        {
            clearLockToolStripButton.Enabled = true;
        }
        else
        {
            clearLockToolStripButton.Enabled = false;
        }
    }

    private void clearLockToolStripButton_Click(object sender, EventArgs e)
    {
        if (accountLockDataGrid.SelectedRows.Count > 0)
        {
            var id = accountLockDataGrid.SelectedRows[0].Cells[nameof(AccountLock.id)].Value.ToString();

            _accountService.ClearAccountLock(Convert.ToInt32(id));

            accountLockDataGrid.DataSource = _accountService.GetAccountLocks();
        }
    }

    private void refreshToolStripButton_Click(object sender, EventArgs e)
    {
        accountLockDataGrid.DataSource = _accountService.GetAccountLocks();
    }
}
