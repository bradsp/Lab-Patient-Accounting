﻿using LabBilling.Core;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Library;
using LabBilling.Logging;
using System.Data;
using WinFormsLibrary;

namespace LabBilling.Forms;

public partial class WorkListForm : Form
{
    private readonly AccountService _accountService = new(Program.AppEnvironment);
    private readonly WorklistService _worklist = new(Program.AppEnvironment);
    private readonly bool _tasksRunning = false;
    private bool _requestAbort = false;
    private readonly BindingSource _accountBindingSource = new();
    private DataTable _accountTable = null;
    private int _worklistPanelWidth = 0;
    private readonly System.Windows.Forms.Timer _timer;
    private const int _timerDelay = 650;
    private string _selectedQueue = null;
    private TreeNode _currentNode = null;


    public event EventHandler<string> AccountLaunched;

    private void WorkListForm_Load(object sender, EventArgs e)
    {
        accountGrid.DoubleBuffered(true);

        _accountTable = new List<AccountSearch>().ToDataTable();
        _accountTable.PrimaryKey = new DataColumn[] { _accountTable.Columns[nameof(AccountSearch.Account)] };

        _accountBindingSource.DataSource = _accountTable;
        accountGrid.DataSource = _accountBindingSource;

        var worklists = Worklists.ToList();

        TreeNode[] worklistsTreeNode = new TreeNode[worklists.Count];
        int i = 0;

        //load worklist selector
        worklists.ForEach(w => worklistsTreeNode[i++] = new TreeNode(w));

        TreeNode rootNode = new("Worklists", worklistsTreeNode);
        workqueues.Nodes.Add(rootNode);
        workqueues.ExpandAll();

        workqueues.Enabled = true;
    }

    public WorkListForm()
    {
        InitializeComponent();
        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerDelay };
        _timer.Tick += new EventHandler(filterTextBox_KeyUpDone);
    }

    private void WorkListForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (_tasksRunning)
        {
            if (MessageBox.Show("Validation process is running. Do you want to abort?", "Abort Validation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                == DialogResult.Yes)
            {
                //code to abort process
                _requestAbort = true;
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
        if (_currentNode != null)
        {
            _currentNode.BackColor = Color.White;
            _currentNode.ForeColor = Color.Black;
        }
        _selectedQueue = workqueues.SelectedNode.Text;
        _currentNode = workqueues.SelectedNode;

        workqueues.SelectedNode.BackColor = Color.Green;
        workqueues.SelectedNode.ForeColor = Color.White;
        await LoadWorkList();
    }

    private async Task LoadWorkList()
    {
        workqueues.Enabled = false;

        Cursor.Current = Cursors.WaitCursor;

        if (_selectedQueue == null)
        {
            workqueues.Enabled = true;
            return;
        }

        accountGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
        accountGrid.RowHeadersVisible = false;

        toolStripStatusLabel1.Text = "Loading Accounts ... ";
        toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

        var accounts = await _worklist.GetAccountsForWorklistAsync(_selectedQueue);

        _accountBindingSource.DataSource = null;
        _accountTable = accounts.ToDataTable();
        _accountTable.PrimaryKey = new DataColumn[] { _accountTable.Columns[nameof(AccountSearch.Account)] };
        _accountBindingSource.DataSource = _accountTable;

        _accountTable.DefaultView.Sort = nameof(AccountSearch.ServiceDate);

        accountGrid.ForeColor = Color.Black;

        accountGrid.SetColumnsVisibility(false);

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
        accountGrid.Columns[nameof(AccountSearch.Account)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.Name)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.SSN)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.DateOfBirth)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.Sex)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.EMPINumber)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.ServiceDate)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.Balance)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.FinCode)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.PrimaryInsCode)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.PrimaryInsPlanName)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.Status)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.ClientMnem)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.LastValidationDate)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.ThirdPartyBalance)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.ClientBalance)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.FinType)].SetVisibilityOrder(true, x++);
        accountGrid.Columns[nameof(AccountSearch.TotalPayments)].SetVisibilityOrder(showAccountsWithPmtCheckbox.Checked, x++);

        accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].MinimumWidth = 200;
        accountGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        accountGrid.Columns[nameof(AccountSearch.ValidationStatus)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        accountGrid.Refresh();
        accountGrid.RowHeadersVisible = true;

        Cursor.Current = Cursors.Default;

        toolStripProgressBar1.Style = ProgressBarStyle.Continuous;

        Cursor.Current = Cursors.Default;

        if (!showAccountsWithPmtCheckbox.Checked)
            _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.TotalPayments)} = 0";

        toolStripStatusLabel1.Text = $"{_accountTable.DefaultView.Count} records";

        workqueues.Enabled = true;
        UpdateFilter();
    }

    private void holdToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
        var accts = _accountTable.Rows.Find(selectedAccount);

        if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString() != AccountStatus.Hold)
        {
            accts[nameof(AccountSearch.Status)] = AccountStatus.Hold;
            //get user comment
            var result = InputBox.Show("Enter a reason for placing account on hold", true);

            if (result.ReturnCode == DialogResult.OK)
            {
                _accountService.UpdateStatus(selectedAccount, AccountStatus.Hold);
                _accountService.AddNote(selectedAccount, result.Text);
            }
        }
        else
        {
            accts[nameof(AccountSearch.Status)] = AccountStatus.New;
            _accountService.UpdateStatus(selectedAccount, AccountStatus.New);
            _accountService.AddNote(selectedAccount, "Account removed from hold.");
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
        var accts = _accountTable.Rows.Find(selectedAccount);
        var account = await _accountService.GetAccountAsync(selectedAccount);

        string newFinCode = InputDialogs.SelectFinancialCode(accts[nameof(AccountSearch.FinCode)].ToString());
        if (!string.IsNullOrEmpty(newFinCode))
        {
            try
            {
                await _accountService.ChangeFinancialClassAsync(account, newFinCode);
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
        var accts = _accountTable.Rows.Find(selectedAccount);
        var account = await _accountService.GetAccountAsync(selectedAccount);

        ClientLookupForm clientLookupForm = new()
        {
            Datasource = DataCache.Instance.GetClients()
        };

        if (clientLookupForm.ShowDialog() == DialogResult.OK)
        {
            string newClient = clientLookupForm.SelectedValue;

            try
            {
                if (await _accountService.ChangeClientAsync(account, newClient))
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
        var accts = _accountTable.Rows.Find(selectedAccount);
        var account = await _accountService.GetAccountAsync(selectedAccount);


        var result = InputDialogs.SelectDateOfService((DateTime)account.TransactionDate);

        try
        {
            if (result.newDate != DateTime.MinValue)
            {
                await _accountService.ChangeDateOfServiceAsync(account, result.newDate, result.reason);
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
            panel1.Width = _worklistPanelWidth;
            panelExpandCollapseButton.Text = "<";
            accountGrid.Left = panel1.Right + 10;
            accountGrid.Width -= _worklistPanelWidth - 20;
        }
        else
        {
            //collapse
            _worklistPanelWidth = panel1.Width;
            panel1.Width = panelExpandCollapseButton.Width;
            panelExpandCollapseButton.Text = ">";
            accountGrid.Left = panel1.Right + 10;
            accountGrid.Width += _worklistPanelWidth - 20;
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
                _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Name)} like '{filterTextBox.Text}%'";
            }
            if (clientFilterRadioBtn.Checked)
            {
                _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.ClientMnem)} = '{filterTextBox.Text}'";
            }
            if (accountFilterRadioBtn.Checked)
            {
                _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Account)} like '{filterTextBox.Text}%'";
            }
            if (insuranceFilterRadioButton.Checked)
            {
                _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.PrimaryInsPlanName)} like '%{filterTextBox.Text}%'";
            }

            if (!showAccountsWithPmtCheckbox.Checked)
            {
                _accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.TotalPayments)} = 0";
            }

            if (!showZeroBalanceCheckBox.Checked)
            {
                _accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Balance)} <> 0";
            }

            if (!showReadyToBillCheckbox.Checked)
            {
                _accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Status)} not in ('{AccountStatus.ReadyToBill}','{AccountStatus.Professional}','{AccountStatus.Institutional}')";
            }
        }
        else
        {
            if (!showAccountsWithPmtCheckbox.Checked)
            {
                _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.TotalPayments)} = 0";
            }
            else
            {
                _accountTable.DefaultView.RowFilter = String.Empty;
            }

            if (!showZeroBalanceCheckBox.Checked)
            {
                if (_accountTable.DefaultView.RowFilter != string.Empty)
                {
                    _accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Balance)} <> 0";
                }
                else
                {
                    _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Balance)} <> 0";
                }
            }

            if (!showReadyToBillCheckbox.Checked)
            {
                if (_accountTable.DefaultView.RowFilter != string.Empty)
                {
                    _accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Status)} not in ('{AccountStatus.ReadyToBill}','{AccountStatus.Professional}','{AccountStatus.Institutional}')";
                }
                else
                {
                    _accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Status)} not in ('{AccountStatus.ReadyToBill}','{AccountStatus.Professional}','{AccountStatus.Institutional}')";
                }
            }
        }

        toolStripStatusLabel1.Text = $"{_accountTable.DefaultView.Count} records";
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
            var accts = _accountTable.Rows.Find(selectedAccount);
            var account = _accountService.GetAccount(selectedAccount);

            if (account.FinCode != Program.AppEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
            {
                _accountService.ChangeFinancialClass(account, Program.AppEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode);
                accts.Delete();
                accountGrid.Refresh();
            }
            else
            {
                MessageBox.Show($"Account is already a {Program.AppEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode} financial code.");
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

            if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString() == AccountStatus.Hold)
                holdToolStripMenuItem.Text = "Remove from Hold";
            else
                holdToolStripMenuItem.Text = "Manual Hold";

            if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.FinCode)].Value.ToString() != Program.AppEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode)
            {
                changeToYFinancialClassToolStripMenuItem.Text = $"Change to {Program.AppEnvironment.ApplicationParameters.BillToClientInvoiceDefaultFinCode} Financial Code";
                changeToYFinancialClassToolStripMenuItem.Visible = true;
                changeToYFinancialClassToolStripMenuItem.Enabled = true;
            }
            else
            {
                changeToYFinancialClassToolStripMenuItem.Visible = false;
                changeToYFinancialClassToolStripMenuItem.Enabled = false;
            }

            var status = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString();
            if (status == AccountStatus.ReadyToBill || status == AccountStatus.Professional || status == AccountStatus.Institutional)
                readyToBillToolStripMenuItem.Checked = true;
            else
                readyToBillToolStripMenuItem.Checked = false;
        }

    }

    private void readyToBillToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
        var accts = _accountTable.Rows.Find(selectedAccount);
        var account = _accountService.GetAccount(selectedAccount);

        if (readyToBillToolStripMenuItem.Checked)
        {
            if (!account.ReadyToBill)
            {
                account = _accountService.Validate(account);
                if (account.AccountValidationStatus.ValidationText == "No validation errors.")
                {
                    _accountService.UpdateStatus(selectedAccount, AccountStatus.ReadyToBill);
                    account.Status = AccountStatus.ReadyToBill;
                    account.Notes = _accountService.AddNote(selectedAccount, "Marked ready to bill.").ToList();
                    account = _accountService.Validate(account);
                    accts[nameof(AccountSearch.Status)] = account.Status;
                }
                accountGrid.Refresh();
            }
        }
        else
        {
            if (account.ReadyToBill)
            {
                _accountService.UpdateStatus(selectedAccount, AccountStatus.New);
                account.Status = AccountStatus.New;
                account.Notes = _accountService.AddNote(selectedAccount, "Ready to bill removed.").ToList();
                accts[nameof(AccountSearch.Status)] = AccountStatus.New;
                accountGrid.Refresh();
            }
        }
        _accountService.ClearAccountLock(account);
    }

    private void WorkListForm_Activated(object sender, EventArgs e) { }

    private void showReadyToBillCheckbox_CheckedChanged(object sender, EventArgs e) => UpdateFilter();

    private void WorkListForm_Enter(object sender, EventArgs e) { }

    private void showZeroBalanceCheckBox_CheckedChanged(object sender, EventArgs e) => UpdateFilter();

    public void UpdateAccount(Account account)
    {
        if (account == null) return;

        //find this account in the active list
        var row = _accountTable.Rows.Find(account.AccountNo);
        if (row == null) return;

        if (!WorklistService.AccountMeetsWorklistCriteria(_selectedQueue, account))
        {
            row.Delete();
            return;
        }

        row.SetField(nameof(AccountSearch.FinCode), account.FinCode);
        row.SetField(nameof(AccountSearch.Name), account.PatFullName);
        row.SetField(nameof(AccountSearch.Balance), account.Balance);
        row.SetField(nameof(AccountSearch.ClientBalance), account.ClientBalance.Sum(c => c.balance));
        row.SetField(nameof(AccountSearch.Status), account.Status);
        row.SetField(nameof(AccountSearch.ClientMnem), account.ClientMnem);
        row.SetField(nameof(AccountSearch.PrimaryInsCode), account.PrimaryInsuranceCode);
        row.SetField(nameof(AccountSearch.TotalCharges), account.TotalCharges);
        row.SetField(nameof(AccountSearch.TotalPayments), account.TotalPayments);
        row.SetField(nameof(AccountSearch.ValidationStatus), account.AccountValidationStatus.ValidationText);
        row.SetField(nameof(AccountSearch.LastValidationDate), account.AccountValidationStatus.UpdatedDate);


    }
}
