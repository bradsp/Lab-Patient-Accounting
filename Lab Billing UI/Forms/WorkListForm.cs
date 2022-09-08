using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Logging;
using LabBilling.Core.Models;
using LabBilling.Library;
using RFClassLibrary;


namespace LabBilling.Forms
{
    public partial class WorkListForm : Form
    {
 
        public WorkListForm(string connValue)
        {
            InitializeComponent();
            _connectionString = connValue;
        }
        string _connectionString;
        AccountRepository accountRepository;
        AccountSearchRepository accountSearchRepository;
        SystemParametersRepository systemParametersRepository;
        List<AccountSearch> accounts;
        bool tasksRunning = false;
        bool requestAbort = false;

        private void WorkListForm_Load(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            accountRepository = new AccountRepository(_connectionString);
            accountSearchRepository = new AccountSearchRepository(_connectionString);
            systemParametersRepository = new SystemParametersRepository(_connectionString);

            // load the treeview with worklists
            TreeNode[] worklists = new TreeNode[]
            {
                new TreeNode("Medicare/Cigna"),
                new TreeNode("BlueCross"),
                new TreeNode("Champus"),
                new TreeNode("Tenncare BC/BS"),
                new TreeNode("Commercial UB"),
                new TreeNode("Commercial 1500"),
                new TreeNode("UHC Community Plan"),
                new TreeNode("Pathways TNCare"),
                new TreeNode("Amerigroup"),
                new TreeNode("Manual Hold")
            };

            TreeNode rootNode = new TreeNode("Worklists", worklists);
            workqueues.Nodes.Add(rootNode);
            workqueues.ExpandAll();

            CancelValidationButton.Visible = false;
            CancelValidationButton.Enabled = false;

        }

        private async void ValidateButton_Click(object sender, EventArgs e)
        {
            requestAbort = false;
            ValidateButton.Enabled = false;
            PostButton.Enabled = false;
            workqueues.Enabled = false;
            CancelValidationButton.Enabled = true;
            CancelValidationButton.Visible = true;
            
            int cnt = accounts.Count;
            progressBar.Minimum = 0;
            progressBar.Maximum = cnt;
            progressBar.Value = 0;
            Cursor.Current = Cursors.WaitCursor;
            //var accountList = (List<AccountSearch>)accountGrid.DataSource;

            tasksRunning = true;
            foreach (var acc in accounts)
            {
                if (requestAbort)
                {
                    statusLabel2.Text = "Aborting...";
                    tasksRunning = false;
                    //this.Close();
                    break;
                }
                statusLabel2.Text = $"Validating {progressBar.Value} of {accounts.Count}.";
                await RunValidationAsync(acc.Account);
                progressBar.Increment(1);
            }
            tasksRunning = false;
            statusLabel2.Text = "Validation complete.";

            Cursor.Current = Cursors.Default;
            ValidateButton.Enabled = true;
            PostButton.Enabled = true;
            workqueues.Enabled = true;
            CancelValidationButton.Visible = false;
            CancelValidationButton.Enabled = false;
        }

        private void RunValidation(string accountNo)
        {
            if (!string.IsNullOrEmpty(accountNo))
            {
                int rowIndex =  -1;
                bool tempAllowUserToAddRows = accountGrid.AllowUserToAddRows;
                accountGrid.AllowUserToAddRows = false; // Turn off or .Value below will throw null exception
                DataGridViewRow row = accountGrid.Rows
                    .Cast<DataGridViewRow>()
                    .Where(r => r.Cells["Account"].Value.ToString().Equals(accountNo))
                    .First();
                rowIndex = row.Index;
                accountGrid.AllowUserToAddRows = tempAllowUserToAddRows;

                Account account;
                account =  accountRepository.GetByAccount(accountNo);
                if (!accountRepository.Validate(ref account))
                {
                    //account has validation errors - update grid
                    accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Value = account.AccountValidationStatus.validation_text;
                    accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
                    accountGrid[nameof(AccountSearch.Status), rowIndex].Value = "ERROR";
                    accountRepository.UpdateStatus(accountNo, "NEW");
                }
                else
                {
                    accountGrid[nameof(AccountSearch.Status), rowIndex].Value = account.Fin.form_type;
                    accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
                    accountRepository.UpdateStatus(account.AccountNo, account.Fin.form_type);
                }

            }
        }

        private async Task RunValidationAsync(string accountNo)
        {
            if (!string.IsNullOrEmpty(accountNo))
            {

                int rowIndex = await Task<int>.Run(() =>
                {
                    rowIndex = -1;
                    accountGrid.AllowUserToAddRows = false; // Turn off or .Value below will throw null exception
                    DataGridViewRow row = accountGrid.Rows
                        .Cast<DataGridViewRow>()
                        .Where(r => r.Cells["Account"].Value.ToString().Equals(accountNo))
                        .First();
                    rowIndex = row.Index;
                    if(row.Index > (accountGrid.FirstDisplayedScrollingRowIndex + accountGrid.DisplayedRowCount(false)-2))
                        accountGrid.FirstDisplayedScrollingRowIndex = row.Index-5;
                    return rowIndex;
                });
                try
                {
                    var (isValid, validationText, formType) = await ValidateAccountAsync(accountNo);

                    if (!isValid)
                    {
                        //account has validation errors - update grid
                        accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Value = validationText;
                        accountGrid[nameof(AccountSearch.Status), rowIndex].Value = "ERROR";
                        accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
                        accountGrid[nameof(AccountSearch.Status), rowIndex].Style.BackColor = Color.Red;
                        accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Style.BackColor = Color.LightPink;
                        accountGrid[nameof(AccountSearch.Status), rowIndex].Style.ForeColor = Color.White;
                        accountRepository.UpdateStatus(accountNo, "NEW");
                    }
                    else
                    {
                        accountGrid[nameof(AccountSearch.Status), rowIndex].Value = formType;
                        accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
                        if (formType != "UNDEFINED")
                            accountRepository.UpdateStatus(accountNo, formType);
                        accountGrid[nameof(AccountSearch.Status), rowIndex].Style.BackColor = Color.LightGreen;
                    }
                }
                catch(Exception ex)
                {
                    Log.Instance.Error($"Error validating account {accountNo} - {ex.Message}");
                    accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Value = "Error validating account. Notify support.";
                    accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
                    accountGrid[nameof(AccountSearch.Status), rowIndex].Value = "ERROR";
                    accountGrid[nameof(AccountSearch.Status), rowIndex].Style.BackColor = Color.Red;
                    accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Style.BackColor = Color.LightPink;
                    accountGrid[nameof(AccountSearch.Status), rowIndex].Style.ForeColor = Color.White;
                    accountRepository.UpdateStatus(accountNo, "NEW");
                }
            }
        }

        private async Task<(bool isValid, string validationText, string formType)> ValidateAccountAsync(string accountNo)
        {
            Account account;
            account = await Task<Account>.Run(() =>
            {
                return accountRepository.GetByAccount(accountNo);
            });
            try
            {
                if (!accountRepository.Validate(ref account))
                {
                    return (false, account.AccountValidationStatus.validation_text, 
                        account.BillForm ?? "UNDEFINED");
                }
                else
                {
                    return (true, string.Empty, account.BillForm ?? "UNDEFINED");
                }
            }
            catch(Exception ex)
            {
                return (false, $"Exception in validation - {ex.Message}", String.Empty);
            }

        }

        private void PostButton_Click(object sender, EventArgs e)
        {

        }

        private void accountGrid_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Cursor.Current = Cursors.WaitCursor;
            int selectedRows = accountGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
                AccountForm frm = new AccountForm(selectedAccount)
                {
                    MdiParent = this.ParentForm
                };
                frm.Show();
                return;
            }
            else
            {
                MessageBox.Show("No account selected.");
                return;
            }

        }

        private void WorkListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (tasksRunning)
            {
                if(MessageBox.Show("Validation process is running. Do you want to abort?", "Abort Validation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    //code to abort process
                    requestAbort = true;
                    e.Cancel = true;
                    return;
                }
                else
                {
                    e.Cancel = true;
                    return;
                }
            }
        }

        private async void workqueues_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            workqueues.Enabled = false;
            ValidateButton.Enabled = false;

            DateTime.TryParse(systemParametersRepository.GetByKey("ssi_bill_thru_date"), out DateTime thruDate);
            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters = 
            {
                (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString()),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT")
            };

            var selectedQueue = workqueues.SelectedNode.Text;
            switch (selectedQueue)
            {
                case "Medicare/Cigna":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "A")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "BlueCross":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "B")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Champus":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "C")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Tenncare BC/BS":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "D")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Commercial UB":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "H")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Commercial 1500":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "L")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "UHC Community Plan":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "M")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Pathways TNCare":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "P")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Amerigroup":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "Q")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    break;
                case "Manual Hold":
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "HOLD")).ToArray();
                    break;
                default:
                    break;
            }

            accountGrid.DataSource = null;
            accountGrid.Columns.Clear();
            accountGrid.Rows.Clear();
            accountGrid.Refresh();

            statusLabel1.Text = "Loading Accounts ... ";
            progressBar.ProgressBarStyle = ProgressBarStyle.Marquee;
            accounts = await Task.Run(() =>
            {
                return accountSearchRepository.GetBySearch(parameters).ToList();
            });

            if (accounts == null || accounts.Count == 0)
            {
                //MessageBox.Show("No records returned.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);

                accounts.Add(new AccountSearch()
                {
                    Name = "No records found."
                });
            }
            
            accountGrid.DataSource = accounts;
            accountGrid.ForeColor = Color.Black;
            accountGrid.Columns[nameof(AccountSearch.mod_date)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_host)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_prg)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_user)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.rowguid)].Visible = false;

            //accountGrid.Columns.Add("ValidationErrors", "Validation Errors");

            accountGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            accountGrid.Refresh();
            Cursor.Current = Cursors.Default;

            statusLabel1.Text = accountGrid.Rows.Count.ToString() + $" records.";
            progressBar.ProgressBarStyle = ProgressBarStyle.Continuous;

            workqueues.Enabled = true;
            ValidateButton.Enabled = true;
        }

        private void CancelValidationButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to abort validation process?", "Abort Validation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
            {
                //code to abort process
                requestAbort = true;
                return;
            }
        }

        private void holdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();

            accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value = "HOLD";
            accountRepository.UpdateStatus(selectedAccount, "HOLD");
        }

        private void accountGrid_MouseDown(object sender, MouseEventArgs e)
        {
            // If the user pressed something else than mouse right click, return
            if (e.Button != System.Windows.Forms.MouseButtons.Right) { return; }

            DataGridView dgv = (DataGridView)sender;

            // Use HitTest to resolve the row under the cursor
            int rowIndex = dgv.HitTest(e.X, e.Y).RowIndex;

            // If there was no DataGridViewRow under the cursor, return
            if (rowIndex == -1) { return; }

            // Clear all other selections before making a new selection
            dgv.ClearSelection();

            // Select the found DataGridViewRow
            dgv.Rows[rowIndex].Selected = true;
        }
    }
}
