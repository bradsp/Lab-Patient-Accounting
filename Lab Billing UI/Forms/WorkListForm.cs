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
        FinRepository finRepository;
        AccountRepository accountRepository;
        AccountSearchRepository accountSearchRepository;
        SystemParametersRepository systemParametersRepository;
        List<AccountSearch> accounts;

        private void WorkListForm_Load(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            finRepository = new FinRepository(_connectionString);
            accountRepository = new AccountRepository(_connectionString);
            accountSearchRepository = new AccountSearchRepository(_connectionString);
            systemParametersRepository = new SystemParametersRepository(_connectionString);

            //load filter combobox
            ClaimFilter.DataSource = finRepository.GetAll();
            ClaimFilter.DisplayMember = nameof(Fin.res_party);
            ClaimFilter.ValueMember = nameof(Fin.fin_code);
            ClaimFilter.SelectedIndex = -1;

            //thru date is ssi_bill_thru_date
            DateTime.TryParse(systemParametersRepository.GetByKey("ssi_bill_thru_date"), out DateTime thruDate);
            
            accounts = accountSearchRepository.GetBySearch(new[] { (nameof(AccountSearch.ServiceDate), AccountSearchRepository.operation.LessThanOrEqual, thruDate.ToString()),
                                                                    (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "PAID_OUT"),
                                                                    (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "CLOSED"),
                                                                    (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSI1500"),
                                                                    (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "SSIUB"),
                                                                    (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "UB"),
                                                                    (nameof(AccountSearch.Status), AccountSearchRepository.operation.NotEqual, "1500"),
                                                                    (nameof(AccountSearch.FinCode), AccountSearchRepository.operation.NotEqual, "CLIENT")}).ToList();

            if (accounts == null || accounts.Count == 0)
            {
                MessageBox.Show("No records returned.", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            //accountData = Helper.ConvertToDataTable(accounts);
            //accountData.Columns.Add("Validation Errors");

            accountGrid.DataSource = accounts;
            accountGrid.ForeColor = Color.Black;
            accountGrid.Columns[nameof(AccountSearch.mod_date)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_host)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_prg)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.mod_user)].Visible = false;
            accountGrid.Columns[nameof(AccountSearch.rowguid)].Visible = false;

            accountGrid.Columns.Add("ValidationErrors", "Validation Errors");

            accountGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            accountGrid.Columns["ValidationErrors"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Cursor.Current = Cursors.Default;
        }

        private void ValidateButton_Click(object sender, EventArgs e)
        {
            int cnt = accountGrid.Rows.Count;
            progressBar.Minimum = 0;
            progressBar.Maximum = cnt;
            Cursor.Current = Cursors.WaitCursor;
            foreach (DataGridViewRow row in accountGrid.Rows)
            {
                Application.DoEvents();
                string accountNo = row.Cells[nameof(AccountSearch.Account)].Value.ToString();
                if(!string.IsNullOrEmpty(accountNo))
                {
                    var account = accountRepository.GetByAccount(accountNo);
                    if(!accountRepository.Validate(ref account))
                    {
                        //account has validation errors - update grid
                        accountGrid["ValidationErrors", row.Index].Value = account.AccountValidationStatus.validation_text;
                        accountGrid[nameof(AccountSearch.Status), row.Index].Value = "ERROR";
                    }
                    else
                    {
                        accountGrid[nameof(AccountSearch.Status), row.Index].Value = "OK";
                    }
                }
                progressBar.Increment(1);
            }
            Cursor.Current = Cursors.Default;
        }

        private void PostButton_Click(object sender, EventArgs e)
        {

        }

        private void OutpatientBilling_CheckedChanged(object sender, EventArgs e)
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

        private void ClaimFilter_SelectionChangeCommitted(object sender, EventArgs e)
        {
            var selectedValue = ClaimFilter.SelectedValue;
            if (selectedValue != null)
            {
                accountGrid.DataSource = accounts.Where(a => a.FinCode == selectedValue.ToString()).ToList();
            }
        }
    }
}
