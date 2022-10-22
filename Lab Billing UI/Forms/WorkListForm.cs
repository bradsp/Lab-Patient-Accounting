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
using Microsoft.Identity.Client;


namespace LabBilling.Forms
{
    public partial class WorkListForm : Form
    {

        public WorkListForm(string connValue)
        {
            InitializeComponent();
            _connectionString = connValue;
        }
        private string _connectionString;
        private AccountRepository accountRepository;
        private AccountSearchRepository accountSearchRepository;
        private SystemParametersRepository systemParametersRepository;
        private List<AccountSearch> accounts;
        private bool tasksRunning = false;
        private bool requestAbort = false;
        private BindingSource accountBindingSource = new BindingSource();
        private int worklistPanelWidth = 0;

        private void WorkListForm_Load(object sender, EventArgs e)
        {
            //Cursor.Current = Cursors.WaitCursor;
            accountRepository = new AccountRepository(_connectionString);
            accountSearchRepository = new AccountSearchRepository(_connectionString);
            systemParametersRepository = new SystemParametersRepository(_connectionString);
            accountGrid.DataSource = accountBindingSource;

            // load the treeview with worklists
            TreeNode[] worklistsTreeNode = new TreeNode[]
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
                new TreeNode("Manual Hold"),
                new TreeNode("Initial Hold"),
                new TreeNode("Client Bill"),
                new TreeNode("Submitted Institutional"),
                new TreeNode("Submitted Professional")
            };

            TreeNode rootNode = new TreeNode("Worklists", worklistsTreeNode);
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
            toolStripProgressBar1.Minimum = 0;
            toolStripProgressBar1.Maximum = cnt;
            toolStripProgressBar1.Value = 0;
            Cursor.Current = Cursors.WaitCursor;

            tasksRunning = true;
            foreach (var acc in accounts)
            {
                if (requestAbort)
                {
                    toolStripStatusLabel1.Text = "Aborting...";
                    tasksRunning = false;
                    break;
                }
                toolStripStatusLabel1.Text = $"Validating {toolStripProgressBar1.Value} of {accounts.Count}.";
                await RunValidationAsync(acc.Account);
                accountGrid.Refresh();
                toolStripProgressBar1.Increment(1);
            }
            tasksRunning = false;
            toolStripStatusLabel1.Text = "Validation complete.";

            Cursor.Current = Cursors.Default;
            ValidateButton.Enabled = true;
            PostButton.Enabled = true;
            workqueues.Enabled = true;
            CancelValidationButton.Visible = false;
            CancelValidationButton.Enabled = false;
        }

        //private void RunValidation(string accountNo)
        //{
        //    if (!string.IsNullOrEmpty(accountNo))
        //    {
        //        int rowIndex = -1;
        //        bool tempAllowUserToAddRows = accountGrid.AllowUserToAddRows;
        //        accountGrid.AllowUserToAddRows = false; // Turn off or .Value below will throw null exception
        //        DataGridViewRow row = accountGrid.Rows
        //            .Cast<DataGridViewRow>()
        //            .Where(r => r.Cells["Account"].Value.ToString().Equals(accountNo))
        //            .First();
        //        rowIndex = row.Index;
        //        accountGrid.AllowUserToAddRows = tempAllowUserToAddRows;

        //        Account account;
        //        account = accountRepository.GetByAccount(accountNo);
        //        if (!accountRepository.Validate(ref account))
        //        {
        //            //account has validation errors - update grid
        //            accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Value = account.AccountValidationStatus.validation_text;
        //            accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
        //            accountGrid[nameof(AccountSearch.Status), rowIndex].Value = "ERROR";
        //            accountRepository.UpdateStatus(accountNo, "NEW");
        //        }
        //        else
        //        {
        //            accountGrid[nameof(AccountSearch.ValidationStatus), rowIndex].Value = "No validation errors";
        //            accountGrid[nameof(AccountSearch.Status), rowIndex].Value = account.Fin.ClaimType;
        //            accountGrid[nameof(AccountSearch.LastValidationDate), rowIndex].Value = DateTime.Now.ToString();
        //            accountRepository.UpdateStatus(account.AccountNo, account.Fin.ClaimType);
        //        }

        //    }
        //}

        private async Task RunValidationAsync(string accountNo)
        {
            if (!string.IsNullOrEmpty(accountNo))
            {
                var acct = accounts.FirstOrDefault(x => x.Account == accountNo);
                try
                {
                    var (isValid, validationText, formType) = await ValidateAccountAsync(accountNo);

                    if (!isValid)
                    {
                        if (acct != null)
                        {
                            acct.ValidationStatus = validationText;
                            acct.LastValidationDate = DateTime.Now;
                            acct.Status = "ERROR";
                        }

                        accountRepository.UpdateStatus(accountNo, "NEW");
                    }
                    else
                    {
                        if (formType != "UNDEFINED")
                            accountRepository.UpdateStatus(accountNo, formType);

                        if(acct != null)
                        {
                            acct.Status = formType;
                            acct.LastValidationDate = DateTime.Now;
                            acct.ValidationStatus = "No validation errors";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"Error validating account {accountNo} - {ex.Message}");

                    if(acct != null)
                    {
                        acct.ValidationStatus = "Error validating account. Notify support.";
                        acct.LastValidationDate = DateTime.Now;
                        acct.Status = "ERROR";
                    }

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
            catch (Exception ex)
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
                if (MessageBox.Show("Validation process is running. Do you want to abort?", "Abort Validation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
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

            Cursor.Current = Cursors.WaitCursor;

            DateTime.TryParse(systemParametersRepository.GetByKey("ssi_bill_thru_date"), out DateTime thruDate);
            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters =
            {
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT")
            };

            var selectedQueue = workqueues.SelectedNode.Text;
            switch (selectedQueue)
            {
                case "Medicare/Cigna":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "A")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "BlueCross":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "B")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Champus":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "C")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Tenncare BC/BS":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "D")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Commercial UB":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "H")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Commercial 1500":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "L")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "UHC Community Plan":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "M")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Pathways TNCare":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "P")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Amerigroup":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "Q")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    break;
                case "Manual Hold":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "HOLD")).ToArray();
                    break;
                case "Initial Hold":
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.GreaterThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "W")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "X")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "Y")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "Z")).ToArray();
                    break;
                case "Client Bill":
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.OneOf, "'W','X','Y','Z'")).ToArray();
                    break;
                case "Submitted Institutional":
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "SSIUB")).ToArray();
                    break;
                case "Submitted Professional":
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "SSI1500")).ToArray();
                    break;
                default:
                    break;
            }
            
            accountGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            accountGrid.RowHeadersVisible = false;

            toolStripStatusLabel1.Text = "Loading Accounts ... ";
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            accounts = (List<AccountSearch>)await Task.Run(() =>
            {
                return accountSearchRepository.GetBySearch(parameters);
            });

            if (accounts == null || accounts.Count == 0)
            {
                accounts.Add(new AccountSearch()
                {
                    Name = "No records found."
                });
            }

            accountBindingSource.DataSource = accounts;

            accountGrid.ForeColor = Color.Black;
            accountGrid.Columns[nameof(AccountSearch.mod_date)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_host)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_prg)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_user)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.rowguid)].Visible = false;

            accountGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            accountGrid.Refresh();
            accountGrid.RowHeadersVisible = true;

            Cursor.Current = Cursors.Default;

            toolStripStatusLabel1.Text = accountGrid.Rows.Count.ToString() + $" records.";


            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;

            Cursor.Current = Cursors.Default;

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

        private void accountGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (accountGrid.Columns[e.ColumnIndex].Name == nameof(AccountSearch.Status))
            {
                if (accountGrid[e.ColumnIndex, e.RowIndex].Value == null)
                    return;

                if (accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == "ERROR")
                {
                    accountGrid[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Red;
                    accountGrid[e.ColumnIndex, e.RowIndex].Style.ForeColor = Color.White;
                    accountGrid[nameof(AccountSearch.ValidationStatus), e.RowIndex].Style.BackColor = Color.LightPink;
                }
                else if (accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == "UB" ||
                    accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == "1500")
                {
                    accountGrid[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGreen;
                    accountGrid[nameof(AccountSearch.ValidationStatus), e.RowIndex].Style.BackColor = Color.LightGreen;
                }
            }
        }

        private void accountGrid_MouseDoubleClick(object sender, MouseEventArgs e)
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

        private void changeFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accounts.FirstOrDefault(x => x.Account == selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);

            string newFinCode = InputDialogs.SelectFinancialCode(accts.FinCode);
            if (!string.IsNullOrEmpty(newFinCode))
            {

                //MessageBox.Show(this, $"New financial class is {newFinCode}");
                try
                {
                    accountRepository.ChangeFinancialClass(ref account, newFinCode);
                    accts.FinCode = account.FinCode;
                    accountGrid.Refresh();
                }
                catch (ArgumentException anex)
                {
                    Log.Instance.Error(anex, $"Financial code {anex.ParamName} is not valid.");
                    MessageBox.Show(this, $"{anex.ParamName} is not a valid financial code. Financial code was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex, $"Error changing financial class.");
                    MessageBox.Show(this, $"Error changing financial class. Financial code was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            Log.Instance.Trace($"Exiting");
        }

        private void changeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accounts.FirstOrDefault(x => x.Account == selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);

            ClientLookupForm clientLookupForm = new ClientLookupForm();
            ClientRepository clientRepository = new ClientRepository(Helper.ConnVal);
            clientLookupForm.Datasource = DataCache.Instance.GetClients();

            if (clientLookupForm.ShowDialog() == DialogResult.OK)
            {
                string newClient = clientLookupForm.SelectedValue;

                try
                {
                    if (accountRepository.ChangeClient(ref account, newClient))
                    {
                        accts.ClientMnem = newClient;
                        accountGrid.Refresh();
                    }
                    else
                    {
                        MessageBox.Show("Error during update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating client. Client not updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Log.Instance.Error(ex);
                    return;
                }
            }

            Log.Instance.Trace("Exiting");
        }

        private void changeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accounts.FirstOrDefault(x => x.Account == selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);


            var result = InputDialogs.SelectDateOfService((DateTime)account.TransactionDate);

            try
            {
                if (result.newDate != DateTime.MinValue)
                {
                    accountRepository.ChangeDateOfService(ref account, result.newDate, result.reason);
                    accts.ServiceDate = account.TransactionDate;
                }
                else
                {
                    MessageBox.Show("Date selected is not valid. Date has not been changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ArgumentNullException anex)
            {
                Log.Instance.Error(anex, $"Change date of service parameter {anex.ParamName} must contain a value.");
                MessageBox.Show(this, $"{anex.ParamName} must contain a value. Date of service was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, $"Error changing date of service.");
                MessageBox.Show(this, $"Error changing date of service. Date of service was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Log.Instance.Trace($"Exiting");
        }

        private void panelExpandCollapseButton_Click(object sender, EventArgs e)
        {
            if (panelExpandCollapseButton.Text == ">")
            {
                //expand
                panel1.Width = worklistPanelWidth;
                panelExpandCollapseButton.Text = "<";
                accountGrid.Left = panel1.Right + 10;
                accountGrid.Width -= worklistPanelWidth - 20;
                ValidateButton.Left = panel1.Right + 10;
                CancelValidationButton.Left = ValidateButton.Right + 10;
            }
            else
            {
                //collapse
                worklistPanelWidth = panel1.Width;
                panel1.Width = panelExpandCollapseButton.Width;
                panelExpandCollapseButton.Text = ">";
                accountGrid.Left = panel1.Right + 10;
                accountGrid.Width += worklistPanelWidth - 20;  
                ValidateButton.Left = panel1.Right + 10;
                CancelValidationButton.Left = ValidateButton.Right + 10;
            }
        }
    }
}
