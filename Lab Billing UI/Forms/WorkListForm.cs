using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using LabBilling.Library;
using System.Data;
using LabBilling.Core;
using LabBilling.Core.BusinessLogic;

namespace LabBilling.Forms
{
    public partial class WorkListForm : Form
    {
        private string _connectionString;
        private AccountRepository accountRepository;
        private bool tasksRunning = false;
        private bool requestAbort = false;
        private BindingSource accountBindingSource = new BindingSource();
        private DataTable accountTable = null;
        private int worklistPanelWidth = 0;
        private Timer _timer;
        private const int _timerDelay = 650;
        private string selectedQueue = null;
        private TreeNode currentNode = null;
        private Worklist worklist = null;

        public event EventHandler<string> AccountLaunched;

        private void WorkListForm_Load(object sender, EventArgs e)
        {
            accountGrid.DoubleBuffered(true);

            worklist = new Worklist(Program.AppEnvironment);
            accountRepository = new AccountRepository(Program.AppEnvironment);

            accountTable = new List<AccountSearch>().ToDataTable();
            accountTable.PrimaryKey = new DataColumn[] { accountTable.Columns[nameof(AccountSearch.Account)] };

            accountBindingSource.DataSource = accountTable;
            accountGrid.DataSource = accountBindingSource;

            var worklists = Worklists.ToList();

            TreeNode[] worklistsTreeNode = new TreeNode[worklists.Count];
            int i = 0;
            foreach (string wlist in worklists)
            {
                worklistsTreeNode[i++] = new TreeNode(wlist);
            }

            TreeNode rootNode = new TreeNode("Worklists", worklistsTreeNode);
            workqueues.Nodes.Add(rootNode);
            workqueues.ExpandAll();

            workqueues.Enabled = true;
        }

        public WorkListForm(string connValue)
        {
            InitializeComponent();
            _connectionString = connValue;
            _timer = new Timer() { Enabled = false, Interval = _timerDelay };
            _timer.Tick += new EventHandler(filterTextBox_KeyUpDone);
        }

        private async Task RunValidationAsync(string accountNo)
        {
            if (!string.IsNullOrEmpty(accountNo))
            {
                //var acct = accounts.FirstOrDefault(x => x.Account == accountNo);
                var acct = accountTable.Rows.Find(accountNo);
                try
                {
                    var (isValid, validationText, formType) = await ValidateAccountAsync(accountNo);

                    if (!isValid)
                    {
                        if (acct != null)
                        {
                            acct[nameof(AccountSearch.ValidationStatus)] = validationText;
                            acct[nameof(AccountSearch.LastValidationDate)] = DateTime.Now;
                            acct[nameof(AccountSearch.Status)] = "ERROR";
                        }
                    }
                    else
                    {
                        if (acct != null)
                        {
                            acct[nameof(AccountSearch.Status)] = formType;
                            acct[nameof(AccountSearch.LastValidationDate)] = DateTime.Now;
                            acct[nameof(AccountSearch.ValidationStatus)] = "No validation errors";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Instance.Error($"Error validating account {accountNo} - {ex.Message}");

                    if (acct != null)
                    {
                        acct[nameof(AccountSearch.ValidationStatus)] = "Error validating account. Notify support.";
                        acct[nameof(AccountSearch.LastValidationDate)] = DateTime.Now;
                        acct[nameof(AccountSearch.Status)] = "ERROR";
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

                if (!accountRepository.Validate(account))
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
            if (currentNode != null)
            {
                currentNode.BackColor = Color.White;
                currentNode.ForeColor = Color.Black;
            }
            selectedQueue = workqueues.SelectedNode.Text;
            currentNode = workqueues.SelectedNode;

            workqueues.SelectedNode.BackColor = Color.Green;
            workqueues.SelectedNode.ForeColor = Color.White;
            await LoadWorkList();
        }

        private async Task LoadWorkList()
        {
            workqueues.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            if (selectedQueue == null)
            {
                workqueues.Enabled = true;
                return;
            }

            accountGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            accountGrid.RowHeadersVisible = false;

            toolStripStatusLabel1.Text = "Loading Accounts ... ";
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

            var accounts = await worklist.GetAccountsForWorklistAsync(selectedQueue);

            accountBindingSource.DataSource = null;
            accountTable = accounts.ToDataTable();
            accountTable.PrimaryKey = new DataColumn[] { accountTable.Columns[nameof(AccountSearch.Account)] };
            accountBindingSource.DataSource = accountTable;

            accountTable.DefaultView.Sort = nameof(AccountSearch.ServiceDate);

            accountGrid.ForeColor = Color.Black;
            accountGrid.Columns[nameof(AccountSearch.FirstName)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.LastName)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.MiddleName)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_date)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_host)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_prg)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_user)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.rowguid)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.Balance)].Visible = true;
            accountGrid.Columns[nameof(AccountSearch.TotalPayments)].Visible = showAccountsWithPmtCheckbox.Checked;
            accountGrid.Columns[nameof(AccountSearch.TotalCharges)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.Balance)].DefaultCellStyle.Format = "N2";
            accountGrid.Columns[nameof(AccountSearch.Balance)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accountGrid.Columns[nameof(AccountSearch.TotalCharges)].DefaultCellStyle.Format = "N2";
            accountGrid.Columns[nameof(AccountSearch.TotalCharges)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accountGrid.Columns[nameof(AccountSearch.TotalPayments)].DefaultCellStyle.Format = "N2";
            accountGrid.Columns[nameof(AccountSearch.TotalPayments)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accountGrid.Columns[nameof(AccountSearch.ThirdPartyBalance)].DefaultCellStyle.Format = "N2";
            accountGrid.Columns[nameof(AccountSearch.ThirdPartyBalance)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            accountGrid.Columns[nameof(AccountSearch.ClientBalance)].DefaultCellStyle.Format = "N2";
            accountGrid.Columns[nameof(AccountSearch.ClientBalance)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            //column order
            int x = 0;
            accountGrid.Columns[nameof(AccountSearch.Account)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.Name)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.SSN)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.DateOfBirth)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.Sex)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.MRN)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.ServiceDate)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.Balance)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.FinCode)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.PrimaryInsCode)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.PrimaryInsPlanName)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.Status)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.ClientMnem)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.LastValidationDate)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.ThirdPartyBalance)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.ClientBalance)].DisplayIndex = x++;
            accountGrid.Columns[nameof(AccountSearch.FinType)].DisplayIndex = x++;

            accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].MinimumWidth = 200;
            accountGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            accountGrid.Refresh();
            accountGrid.RowHeadersVisible = true;

            Cursor.Current = Cursors.Default;

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;

            Cursor.Current = Cursors.Default;

            if (!showAccountsWithPmtCheckbox.Checked)
                accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.TotalPayments)} = 0";

            toolStripStatusLabel1.Text = $"{accountTable.DefaultView.Count} records";

            workqueues.Enabled = true;
            UpdateFilter();

        }

        private void holdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);

            if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString() != "HOLD")
            {
                accts[nameof(AccountSearch.Status)] = "HOLD";
                //get user comment
                var result = InputBox.Show("Enter a reason for placing account on hold", true);

                if (result.ReturnCode == DialogResult.OK)
                {
                    accountRepository.UpdateStatus(selectedAccount, "HOLD");
                    accountRepository.AddNote(selectedAccount, result.Text);
                }
            }
            else
            {
                accts[nameof(AccountSearch.Status)] = "NEW";
                accountRepository.UpdateStatus(selectedAccount, "NEW");
                accountRepository.AddNote(selectedAccount, "Account removed from hold.");
            }
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
                else if (accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == AccountStatus.Institutional ||
                    accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == AccountStatus.Professional ||
                    accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == AccountStatus.ReadyToBill)
                {
                    accountGrid[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGreen;
                    accountGrid[nameof(AccountSearch.ValidationStatus), e.RowIndex].Style.BackColor = Color.LightGreen;
                }
            }
        }

        private void accountGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            int selectedRows = accountGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
                AccountLaunched?.Invoke(this, selectedAccount);
                return;
            }
            else
            {
                MessageBox.Show("No account selected.");
                return;
            }
        }

        private async void changeFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = await accountRepository.GetByAccountAsync(selectedAccount);

            string newFinCode = InputDialogs.SelectFinancialCode(accts[nameof(AccountSearch.FinCode)].ToString());
            if (!string.IsNullOrEmpty(newFinCode))
            {
                try
                {
                    await accountRepository.ChangeFinancialClassAsync(account, newFinCode);
                    accts.Delete();
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

        private async void changeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = await accountRepository.GetByAccountAsync(selectedAccount);

            ClientLookupForm clientLookupForm = new ClientLookupForm();
            ClientRepository clientRepository = new ClientRepository(Program.AppEnvironment);
            clientLookupForm.Datasource = DataCache.Instance.GetClients();

            if (clientLookupForm.ShowDialog() == DialogResult.OK)
            {
                string newClient = clientLookupForm.SelectedValue;

                try
                {
                    if (await accountRepository.ChangeClientAsync(account, newClient))
                    {
                        accts[nameof(AccountSearch.ClientMnem)] = newClient;
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

        private async void changeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = await accountRepository.GetByAccountAsync(selectedAccount);


            var result = InputDialogs.SelectDateOfService((DateTime)account.TransactionDate);

            try
            {
                if (result.newDate != DateTime.MinValue)
                {
                    await accountRepository.ChangeDateOfServiceAsync(account, result.newDate, result.reason);
                    accts[nameof(AccountSearch.ServiceDate)] = account.TransactionDate;
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
            }
            else
            {
                //collapse
                worklistPanelWidth = panel1.Width;
                panel1.Width = panelExpandCollapseButton.Width;
                panelExpandCollapseButton.Text = ">";
                accountGrid.Left = panel1.Right + 10;
                accountGrid.Width += worklistPanelWidth - 20;
            }
        }

        private void filterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void UpdateFilter()
        {
            if (!string.IsNullOrEmpty(filterTextBox.Text))
            {
                if (nameFilterRadioBtn.Checked)
                {
                    accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Name)} like '{filterTextBox.Text}%'";
                }
                if (clientFilterRadioBtn.Checked)
                {
                    accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.ClientMnem)} = '{filterTextBox.Text}'";
                }
                if (accountFilterRadioBtn.Checked)
                {
                    accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Account)} like '{filterTextBox.Text}%'";
                }
                if (insuranceFilterRadioButton.Checked)
                {
                    accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.PrimaryInsPlanName)} like '%{filterTextBox.Text}%'";
                }

                if (!showAccountsWithPmtCheckbox.Checked)
                {
                    accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.TotalPayments)} = 0";
                }

                if (!showZeroBalanceCheckBox.Checked)
                {
                    accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Balance)} <> 0";
                }

                if (!showReadyToBillCheckbox.Checked)
                {
                    accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Status)} not in ('{AccountStatus.ReadyToBill}','{AccountStatus.Professional}','{AccountStatus.Institutional}')";
                }
            }
            else
            {
                if (!showAccountsWithPmtCheckbox.Checked)
                {
                    accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.TotalPayments)} = 0";
                }
                else
                {
                    accountTable.DefaultView.RowFilter = String.Empty;
                }

                if (!showZeroBalanceCheckBox.Checked)
                {
                    if (accountTable.DefaultView.RowFilter != string.Empty)
                    {
                        accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Balance)} <> 0";
                    }
                    else
                    {
                        accountTable.DefaultView.RowFilter = $" and {nameof(AccountSearch.Balance)} <> 0";
                    }
                }

                if (!showReadyToBillCheckbox.Checked)
                {
                    if (accountTable.DefaultView.RowFilter != string.Empty)
                    {
                        accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Status)} not in ('{AccountStatus.ReadyToBill}','{AccountStatus.Professional}','{AccountStatus.Institutional}')";
                    }
                    else
                    {
                        accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Status)} not in ('{AccountStatus.ReadyToBill}','{AccountStatus.Professional}','{AccountStatus.Institutional}')";
                    }
                }
            }

            toolStripStatusLabel1.Text = $"{accountTable.DefaultView.Count} records";
            accountGrid.Columns[nameof(AccountSearch.TotalPayments)].Visible = showAccountsWithPmtCheckbox.Checked;
        }

        private void filterTextBox_KeyUpDone(object sender, EventArgs e)
        {
            _timer.Stop();
            UpdateFilter();
        }

        private void accountGridContextMenu_VisibleChanged(object sender, EventArgs e) { }

        private void showAccountsWithPmtCheckbox_CheckedChanged(object sender, EventArgs e) => UpdateFilter();

        private void changeToYFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
                var accts = accountTable.Rows.Find(selectedAccount);
                var account = accountRepository.GetByAccount(selectedAccount);

                if (account.FinCode != "Y")
                {
                    accountRepository.ChangeFinancialClass(account, "Y");
                    accts.Delete();
                    accountGrid.Refresh();
                }
                else
                {
                    MessageBox.Show("Account is already a Y financial code.");
                }
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

        private void accountGridContextMenu_Opened(object sender, EventArgs e)
        {
            if (accountGridContextMenu.Visible)
            {
                var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();

                if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString() == "HOLD")
                    holdToolStripMenuItem.Text = "Remove from Hold";
                else
                    holdToolStripMenuItem.Text = "Manual Hold";

                if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.FinCode)].Value.ToString() != "Y")
                {
                    changeToYFinancialClassToolStripMenuItem.Visible = true;
                    changeToYFinancialClassToolStripMenuItem.Enabled = true;
                }
                else
                {
                    changeToYFinancialClassToolStripMenuItem.Visible = false;
                    changeToYFinancialClassToolStripMenuItem.Enabled = false;
                }

                var status = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString();
                if (status == "RTB" || status == "1500" || status == "UB")
                    readyToBillToolStripMenuItem.Checked = true;
                else
                    readyToBillToolStripMenuItem.Checked = false;
            }

        }

        private void readyToBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);

            if (readyToBillToolStripMenuItem.Checked)
            {
                if (!account.ReadyToBill)
                {
                    accountRepository.Validate(account);
                    if (account.AccountValidationStatus.validation_text == "No validation errors.")
                    {
                        accountRepository.UpdateStatus(selectedAccount, AccountStatus.ReadyToBill);
                        accountRepository.AddNote(selectedAccount, "Marked ready to bill.");
                        accts[nameof(AccountSearch.Status)] = AccountStatus.ReadyToBill;
                        _ = Task.Run(() => RunValidationAsync(selectedAccount));
                    }
                    accountGrid.Refresh();
                }
            }
            else
            {
                if (account.ReadyToBill)
                {
                    accountRepository.UpdateStatus(selectedAccount, AccountStatus.New);
                    accountRepository.AddNote(selectedAccount, "Ready to bill removed.");
                    accts[nameof(AccountSearch.Status)] = AccountStatus.New;
                    accountGrid.Refresh();
                }
            }
        }

        private async void WorkListForm_Activated(object sender, EventArgs e) => await LoadWorkList();

        private void showReadyToBillCheckbox_CheckedChanged(object sender, EventArgs e) => UpdateFilter();

        private void WorkListForm_Enter(object sender, EventArgs e) { }

        private void showZeroBalanceCheckBox_CheckedChanged(object sender, EventArgs e) => UpdateFilter();

    }
}
