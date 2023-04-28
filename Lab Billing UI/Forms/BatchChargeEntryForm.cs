using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Logging;

namespace LabBilling.Forms
{
    public partial class BatchChargeEntryForm : Form
    {
        AccountRepository accountRepository;
        CdmRepository cdmRepository;
        private DataTable batchChargesTable = new DataTable();
        private BindingSource batchChargeSource = new BindingSource();
        private bool skipHandler = false;

        private class BatchChargeRecord
        {
            public string Account { get; set; }
            public string PatientName { get; set; }
            public string FinCode { get; set; }
            public DateTime DateOfService { get; set; }
            public string Cdm { get; set; }
            public int Qty { get; set; }
            public string Description { get; set; }

        }

        private List<BatchChargeRecord> chargeRecords = new List<BatchChargeRecord>();

        public BatchChargeEntryForm()
        {

            accountRepository = new AccountRepository(Program.AppEnvironment);
            cdmRepository = new CdmRepository(Program.AppEnvironment);

            InitializeComponent();

            batchChargesTable = chargeRecords.ToDataTable();
            batchChargeSource.DataSource = batchChargesTable;

            chargesDataGridView.DataSource = batchChargeSource;

            chargesDataGridView.Columns[nameof(BatchChargeRecord.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            chargesDataGridView.AutoResizeColumns();
        }

        private void chargesDataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (chargesDataGridView.Columns[e.ColumnIndex].Name == nameof(BatchChargeRecord.Account))
            {
                PersonSearchForm frm = new PersonSearchForm();
                frm.ShowDialog();
                if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
                {
                    string strAccount = frm.SelectedAccount.ToUpper();
                    //account = accountRepository.GetByAccount(strAccount, true);
                    chargesDataGridView[e.ColumnIndex, e.RowIndex].Value = strAccount;
                    chargesDataGridView.EndEdit();
                }
            }

            if (chargesDataGridView.Columns[e.ColumnIndex].Name == nameof(BatchChargeRecord.Cdm))
            {
                Cdm cdm = new Cdm();
                //look up cdm number and get amount

                //pop a cdm search dialog
                CdmLookupForm cdmLookupForm = new CdmLookupForm();
                cdmLookupForm.Datasource = DataCache.Instance.GetCdms();

                cdmLookupForm.InitialSearchText = chargesDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
                if (cdmLookupForm.ShowDialog() == DialogResult.OK)
                {
                    chargesDataGridView[e.ColumnIndex, e.RowIndex].Value = cdmLookupForm.SelectedValue;
                    chargesDataGridView.EndEdit();
                }
            }
        }

        private void chargesDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (chargesDataGridView.Columns[e.ColumnIndex].Name == nameof(BatchChargeRecord.Account))
            {
                if (skipHandler)
                    return;
                Account account = null;

                account = accountRepository.GetByAccount(chargesDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString().ToUpper(), true);
                skipHandler = true;
                if (account != null)
                {
                    chargesDataGridView.EndEdit();
                    chargesDataGridView[nameof(BatchChargeRecord.Account), e.RowIndex].Value = account.AccountNo;
                    chargesDataGridView[nameof(BatchChargeRecord.PatientName), e.RowIndex].Value = account.PatFullName;
                    chargesDataGridView[nameof(BatchChargeRecord.FinCode), e.RowIndex].Value = account.FinCode;
                    chargesDataGridView[nameof(BatchChargeRecord.DateOfService), e.RowIndex].Value = account.TransactionDate;
                    chargesDataGridView.CurrentCell = chargesDataGridView[nameof(BatchChargeRecord.Cdm), e.RowIndex];
                }
                else
                {
                    PersonSearchForm frm = new PersonSearchForm();
                    frm.ShowDialog();
                    if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
                    {
                        string strAccount = frm.SelectedAccount.ToUpper();
                        account = accountRepository.GetByAccount(strAccount, true);
                        if (account != null)
                        {
                            chargesDataGridView.EndEdit();
                            chargesDataGridView[nameof(BatchChargeRecord.Account), e.RowIndex].Value = account.AccountNo;
                            chargesDataGridView[nameof(BatchChargeRecord.PatientName), e.RowIndex].Value = account.PatFullName;
                            chargesDataGridView[nameof(BatchChargeRecord.FinCode), e.RowIndex].Value = account.FinCode;
                            chargesDataGridView[nameof(BatchChargeRecord.DateOfService), e.RowIndex].Value = account.TransactionDate;
                            chargesDataGridView.CurrentCell = chargesDataGridView[nameof(BatchChargeRecord.Cdm), e.RowIndex];
                            //chargesDataGridView.EndEdit();
                        }
                    }
                    else
                    {
                        Log.Instance.Error($"Person search returned an empty selected account.");
                        MessageBox.Show("A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
                    }
                }
                skipHandler = false;
            }

            if (chargesDataGridView.Columns[e.ColumnIndex].Name == nameof(BatchChargeRecord.Cdm))
            {
                if (skipHandler)
                    return;

                skipHandler = true;
                Cdm cdm = new Cdm();
                //look up cdm number and get amount
                cdm = cdmRepository.GetCdm(chargesDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString());
                if (cdm != null)
                {
                    chargesDataGridView[nameof(BatchChargeRecord.Description), e.RowIndex].Value = cdm.Description;
                    chargesDataGridView[nameof(BatchChargeRecord.Qty), e.RowIndex].Value = 1;
                }
                else
                {
                    //pop a cdm search dialog
                    CdmLookupForm cdmLookupForm = new CdmLookupForm();
                    cdmLookupForm.Datasource = DataCache.Instance.GetCdms();

                    cdmLookupForm.InitialSearchText = chargesDataGridView[e.ColumnIndex, e.RowIndex].Value.ToString();
                    if (cdmLookupForm.ShowDialog() == DialogResult.OK)
                    {
                        chargesDataGridView[e.ColumnIndex, e.RowIndex].Value = cdmLookupForm.SelectedValue;
                        cdm = cdmRepository.GetCdm(cdmLookupForm.SelectedValue);
                        if (cdm != null)
                        {
                            chargesDataGridView[nameof(BatchChargeRecord.Description), e.RowIndex].Value = cdm.Description;
                            chargesDataGridView[nameof(BatchChargeRecord.Qty), e.RowIndex].Value = 1;

                            chargesDataGridView.Focus();
                            chargesDataGridView.CurrentCell = chargesDataGridView[nameof(BatchChargeRecord.Qty), 0];
                            chargesDataGridView.BeginEdit(true);
                        }
                        else
                        {
                            chargesDataGridView[e.ColumnIndex, e.RowIndex].Value = string.Empty;

                            chargesDataGridView.Focus();
                            chargesDataGridView.CurrentCell = chargesDataGridView[nameof(BatchChargeRecord.Cdm), 0];
                            chargesDataGridView.BeginEdit(true);
                        }
                    }
                }
                skipHandler = false;
            }
        }

        private void chargesDataGridView_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void clearGridButton_Click(object sender, EventArgs e)
        {

        }

        private void chargesDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (chargesDataGridView.CurrentCell.ColumnIndex == chargesDataGridView.Columns[nameof(BatchChargeRecord.Account)].Index)
            {
                if (e.KeyCode == Keys.F2)
                {
                    
                }
            }
        }

        private void chargesDataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == chargesDataGridView.Columns[nameof(BatchChargeRecord.Account)].Index ||
                e.ColumnIndex == chargesDataGridView.Columns[nameof(BatchChargeRecord.Cdm)].Index)
            {
                if (e.Value != null)
                {
                    e.Value = e.Value.ToString().ToUpper();
                    e.FormattingApplied = true;
                }
            }
        }
    }
}
