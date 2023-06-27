using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using LabBilling.Library;
using System.Data;
//using System.CodeDom;
using LabBilling.Core;

namespace LabBilling.Forms
{

    public partial class WorkListForm : Form
    {
        private string _connectionString;
        private AccountRepository accountRepository;
        private AccountSearchRepository accountSearchRepository;
        private SystemParametersRepository systemParametersRepository;
        private bool tasksRunning = false;
        private bool requestAbort = false;
        private BindingSource accountBindingSource = new BindingSource();
        private DataTable accountTable = null;
        private int worklistPanelWidth = 0;
        private System.Windows.Forms.Timer _timer;
        private const int _timerDelay = 650;
        private string selectedQueue = null;
        private TreeNode currentNode = null;

        private void WorkListForm_Load(object sender, EventArgs e)
        {

            //Cursor.Current = Cursors.WaitCursor;
            accountRepository = new AccountRepository(Program.AppEnvironment);
            accountSearchRepository = new AccountSearchRepository(Program.AppEnvironment);
            systemParametersRepository = new SystemParametersRepository(Program.AppEnvironment);

            accountTable = new List<AccountSearch>().ToDataTable();
            accountTable.PrimaryKey = new DataColumn[] { accountTable.Columns[nameof(AccountSearch.Account)] };

            accountBindingSource.DataSource = accountTable;
            accountGrid.DataSource = accountBindingSource;

            // load the treeview with worklists
            TreeNode[] worklistsTreeNode = new TreeNode[]
            {
                new TreeNode(Worklists.MedicareCigna),
                new TreeNode(Worklists.BlueCross),
                new TreeNode(Worklists.Champus),
                new TreeNode(Worklists.TenncareBCBS),
                new TreeNode(Worklists.CommercialInst),
                new TreeNode(Worklists.CommercialProf),
                new TreeNode(Worklists.UHCCommunityPlan),
                new TreeNode(Worklists.PathwaysTNCare),
                new TreeNode(Worklists.Amerigroup),
                new TreeNode(Worklists.SelfPay),
                new TreeNode(Worklists.ManualHold),
                new TreeNode(Worklists.InitialHold),
                new TreeNode(Worklists.ErrorFinCode),
                new TreeNode(Worklists.ClientBill),
                new TreeNode(Worklists.SubmittedInstitutional),
                new TreeNode(Worklists.SubmittedProfessional),
                new TreeNode(Worklists.SubmittedOtherClaim),
                new TreeNode(Worklists.ReceivingStatements)
            };

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
                        if(acct != null)
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

                    if(acct != null)
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

        private void workqueues_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if(currentNode != null)
            {
                currentNode.BackColor = Color.White;
                currentNode.ForeColor = Color.Black;
            }
            selectedQueue = workqueues.SelectedNode.Text;
            currentNode = workqueues.SelectedNode;

            workqueues.SelectedNode.BackColor = Color.Green;
            workqueues.SelectedNode.ForeColor = Color.White;
            LoadWorkList();
        }

        private async void LoadWorkList()
        {
            workqueues.Enabled = false;

            Cursor.Current = Cursors.WaitCursor;

            DateTime thruDate = Program.AppEnvironment.ApplicationParameters.SSIBillThruDate;
            (string propertyName, AccountSearchRepository.operation oper, string searchText)[] parameters =
            {
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT")
            };

            if (selectedQueue == null)
            {
                workqueues.Enabled = true;
                return;
            }

            switch (selectedQueue)
            {
                case Worklists.MedicareCigna:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "A")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.BlueCross:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "B")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.Champus:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "C")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.TenncareBCBS:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "D")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.CommercialInst:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "H")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.CommercialProf:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "L")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.UHCCommunityPlan:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "M")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.PathwaysTNCare:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "P")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.Amerigroup:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "Q")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.SelfPay:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "E")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "HOLD")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLAIM")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "STMT")).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.ThirdPartyBalance), AccountSearchRepository.operation.NotEqual, "0.00")).ToArray();
                    break;
                case Worklists.ManualHold:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "HOLD")).ToArray();
                    break;
                case Worklists.InitialHold:
                    parameters = parameters.Append((nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.GreaterThanOrEqual, thruDate.ToString())).ToArray();
                    parameters = parameters.Append((nameof(AccountSearch.FinType), AccountSearchRepository.operation.NotEqual, "C")).ToArray();
                    break;
                case Worklists.ErrorFinCode:
                    parameters = parameters.Append((nameof(AccountSearch.FinCode), AccountSearchRepository.operation.Equal, "K")).ToArray();
                    break;
                case Worklists.ClientBill:
                    parameters = parameters.Append((nameof(AccountSearch.FinType), AccountSearchRepository.operation.Equal, "C")).ToArray();
                    break;
                case Worklists.SubmittedInstitutional:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "SSIUB")).ToArray();
                    break;
                case Worklists.SubmittedProfessional:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "SSI1500")).ToArray();
                    break;
                case Worklists.SubmittedOtherClaim:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "CLAIM")).ToArray();
                    break;
                case Worklists.ReceivingStatements:
                    parameters = parameters.Append((nameof(AccountSearch.Status), AccountSearchRepository.operation.Equal, "STMT")).ToArray();
                    break;
                default:
                    break;
            }

            accountGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            accountGrid.RowHeadersVisible = false;

            toolStripStatusLabel1.Text = "Loading Accounts ... ";
            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;
            var accounts = (List<AccountSearch>)await Task.Run(() =>
            {
                return accountSearchRepository.GetBySearch(parameters);
            });

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
            accountGrid.Columns[nameof(AccountSearch.TotalCharges)].DefaultCellStyle.Format = "N2";
            accountGrid.Columns[nameof(AccountSearch.TotalPayments)].DefaultCellStyle.Format = "N2";

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

        //private void CancelValidationButton_Click(object sender, EventArgs e)
        //{
        //    if (MessageBox.Show("Are you sure you want to abort validation process?", "Abort Validation?", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
        //            == DialogResult.Yes)
        //    {
        //        //code to abort process
        //        requestAbort = true;
        //        return;
        //    }
        //}

        private void holdToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);

            if (accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value.ToString() != "HOLD")
            {
                //accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Status)].Value = "HOLD";
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
                else if (accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == "UB" ||
                    accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == "1500" ||
                    accountGrid[e.ColumnIndex, e.RowIndex].Value.ToString() == "RTB")
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
                // TODO: make sure there is not another tab already open for this account
                var formsList = Application.OpenForms.OfType<AccountForm>();
                bool formFound = false;
                foreach(var form in formsList)
                {
                    if(form.SelectedAccount == selectedAccount)
                    {
                        //form is already open, activate this one
                        form.Focus();
                        formFound = true;
                        break;
                    }
                }

                if (!formFound)
                {
                    AccountForm frm = new AccountForm(selectedAccount, this.ParentForm);
                    frm.Show();
                }
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
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);

            string newFinCode = InputDialogs.SelectFinancialCode(accts[nameof(AccountSearch.FinCode)].ToString());
            if (!string.IsNullOrEmpty(newFinCode))
            {
                try
                {
                    accountRepository.ChangeFinancialClass(ref account, newFinCode);
                    //accts[nameof(AccountSearch.FinCode)] = account.FinCode;
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

        private void changeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);

            ClientLookupForm clientLookupForm = new ClientLookupForm();
            ClientRepository clientRepository = new ClientRepository(Program.AppEnvironment);
            clientLookupForm.Datasource = DataCache.Instance.GetClients();

            if (clientLookupForm.ShowDialog() == DialogResult.OK)
            {
                string newClient = clientLookupForm.SelectedValue;

                try
                {
                    if (accountRepository.ChangeClient(ref account, newClient))
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

        private void changeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
            var accts = accountTable.Rows.Find(selectedAccount);
            var account = accountRepository.GetByAccount(selectedAccount);


            var result = InputDialogs.SelectDateOfService((DateTime)account.TransactionDate);

            try
            {
                if (result.newDate != DateTime.MinValue)
                {
                    accountRepository.ChangeDateOfService(ref account, result.newDate, result.reason);
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

                if(!showReadyToBillCheckbox.Checked)
                {
                    accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Status)} not in ('RTB','1500','UB')";
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

                if(!showReadyToBillCheckbox.Checked)
                {
                    if(accountTable.DefaultView.RowFilter != string.Empty)
                    {
                        accountTable.DefaultView.RowFilter += $" and {nameof(AccountSearch.Status)} not in ('RTB','1500','UB')";
                    }
                    else
                    {
                        accountTable.DefaultView.RowFilter = $"{nameof(AccountSearch.Status)} not in ('RTB','1500','UB')";
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

        private void accountGridContextMenu_VisibleChanged(object sender, EventArgs e)
        {
        }

        private void showAccountsWithPmtCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void changeToYFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {

            try
            {
                var selectedAccount = accountGrid.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
                var accts = accountTable.Rows.Find(selectedAccount);
                var account = accountRepository.GetByAccount(selectedAccount);

                if (account.FinCode != "Y")
                {
                    accountRepository.ChangeFinancialClass(ref account, "Y");
                    //accts[nameof(AccountSearch.FinCode)] = account.FinCode;
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

            if(readyToBillToolStripMenuItem.Checked)
            {
                if (!account.ReadyToBill)
                {
                    accountRepository.Validate(ref account);
                    if (account.AccountValidationStatus.validation_text == "No validation errors.")
                    {
                        accountRepository.UpdateStatus(selectedAccount, "RTB");
                        accountRepository.AddNote(selectedAccount, "Marked ready to bill.");
                        accts[nameof(AccountSearch.Status)] = "RTB";
                        _ = Task.Run(() => RunValidationAsync(selectedAccount));
                    }                    
                    accountGrid.Refresh();
                }
            }
            else
            {
                if(account.ReadyToBill)
                {
                    accountRepository.UpdateStatus(selectedAccount, "NEW");
                    accountRepository.AddNote(selectedAccount, "Ready to bill removed.");
                    accts[nameof(AccountSearch.Status)] = "NEW";
                    accountGrid.Refresh();
                }
            }
        }

        private void WorkListForm_Activated(object sender, EventArgs e)
        {
            LoadWorkList();
        }

        private void showReadyToBillCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            UpdateFilter();
        }

        private void WorkListForm_Enter(object sender, EventArgs e)
        {

        }
    }
}
