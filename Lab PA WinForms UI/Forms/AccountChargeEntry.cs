using System;
using System.Collections.Generic;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Logging;

namespace LabBilling.Forms;

public partial class AccountChargeEntry : Form
{
    private List<BatchCharge> charges;
    private BindingSource chrgBindingSource;
    private Account currentAccount;
    private AccountService accountService;

    public AccountChargeEntry() 
    {
        InitializeComponent();
    }

    private void dgvBatchEntry_CellLeave(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void BatchChargeEntry_Load(object sender, EventArgs e)
    {
        accountService = new(Program.AppEnvironment, Program.UnitOfWork);
        charges = new List<BatchCharge>();
        chrgBindingSource = new BindingSource
        {
            DataSource = charges
        };
        dgvBatchEntry.DataSource = chrgBindingSource;

        dgvBatchEntry.Columns[nameof(BatchCharge.CDM)].DisplayIndex = 0;
        dgvBatchEntry.Columns[nameof(BatchCharge.ChargeDescription)].DisplayIndex = 1;
        dgvBatchEntry.Columns[nameof(BatchCharge.Qty)].DisplayIndex = 2;
        dgvBatchEntry.Columns[nameof(BatchCharge.AccountNo)].DisplayIndex = 3;
        dgvBatchEntry.Columns[nameof(BatchCharge.AccountNo)].ReadOnly = true;
        dgvBatchEntry.Columns[nameof(BatchCharge.CDM)].MinimumWidth = 100;
        dgvBatchEntry.Columns[nameof(BatchCharge.ChargeDescription)].ReadOnly = true;
        dgvBatchEntry.Columns[nameof(BatchCharge.ChargeDescription)].FillWeight = 100;
        dgvBatchEntry.Columns[nameof(BatchCharge.ChargeDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        dgvBatchEntry.AutoResizeColumns();

        dgvBatchEntry.AllowUserToAddRows = false;
        
    }

    private void dgvBatchEntry_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void dgvBatchEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        using UnitOfWorkMain unitOfWork = new(Program.AppEnvironment);
        switch (dgvBatchEntry.Columns[e.ColumnIndex].Name)
        {
            case nameof(BatchCharge.CDM):
                Cdm cdm = new Cdm();
                //look up cdm number and get amount
                cdm = unitOfWork.CdmRepository.GetCdm(dgvBatchEntry[e.ColumnIndex, e.RowIndex].Value.ToString());
                if (cdm != null)
                {
                    dgvBatchEntry[nameof(BatchCharge.ChargeDescription), e.RowIndex].Value = cdm.Description;
                    dgvBatchEntry[nameof(BatchCharge.AccountNo), e.RowIndex].Value = accountNoTextBox.Text;
                    dgvBatchEntry[nameof(BatchCharge.Qty), e.RowIndex].Value = 1;
                }
                else
                {
                    //pop a cdm search dialog
                    CdmLookupForm cdmLookupForm = new CdmLookupForm();
                    cdmLookupForm.Datasource = DataCache.Instance.GetCdms();

                    cdmLookupForm.InitialSearchText = dgvBatchEntry[e.ColumnIndex, e.RowIndex].Value.ToString();
                    if (cdmLookupForm.ShowDialog() == DialogResult.OK)
                    {
                        dgvBatchEntry[e.ColumnIndex, e.RowIndex].Value = cdmLookupForm.SelectedValue;
                        cdm = unitOfWork.CdmRepository.GetCdm(cdmLookupForm.SelectedValue);
                        if (cdm != null)
                        {
                            dgvBatchEntry[nameof(BatchCharge.ChargeDescription), e.RowIndex].Value = cdm.Description;
                            dgvBatchEntry[nameof(BatchCharge.AccountNo), e.RowIndex].Value = accountNoTextBox.Text;
                            dgvBatchEntry[nameof(BatchCharge.Qty), e.RowIndex].Value = 1;

                            dgvBatchEntry.Focus();
                            dgvBatchEntry.CurrentCell = dgvBatchEntry[nameof(BatchCharge.Qty), 0];
                            dgvBatchEntry.BeginEdit(true);
                        }
                        else
                        {
                            dgvBatchEntry[e.ColumnIndex, e.RowIndex].Value = string.Empty;

                            dgvBatchEntry.Focus();
                            dgvBatchEntry.CurrentCell = dgvBatchEntry[nameof(BatchCharge.CDM), 0];
                            dgvBatchEntry.BeginEdit(true);
                        }
                    }
                }
                break;
            default:
                break;
        }
    }

    private void PostCharges_Click(object sender, EventArgs e)
    {
        //loop through rows to write charges
        AccountService accountService = new(Program.AppEnvironment, Program.UnitOfWork);
        foreach (var charge in charges)
        {
            try
            {
                accountService.AddCharge(charge.AccountNo, charge.CDM, charge.Qty, (DateTime)currentAccount.TransactionDate);
            }
            catch(Exception ex)
            {
                MessageBox.Show("Error posting charge. Process aborted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Instance.Error(ex);
                break;
            }
        }

        //clear the form
        currentAccount = null;
        patientDOBTextBox.Text = string.Empty;
        patientNameTextBox.Text = string.Empty;
        serviceDateTextBox.Text = string.Empty;
        fincodeTextBox.Text = string.Empty;
        clientTextBox.Text = string.Empty;
        accountNoTextBox.Text = string.Empty;

        charges = new List<BatchCharge>();
        dgvBatchEntry.Rows.Clear();
        dgvBatchEntry.Refresh();
    }

    private void patientSearchButton_Click(object sender, EventArgs e)
    {
        PersonSearchForm personSearchForm = new PersonSearchForm();
        if(personSearchForm.ShowDialog() == DialogResult.OK)
        {
            var selectedAccount = personSearchForm.SelectedAccount;

            currentAccount = accountService.GetAccount(selectedAccount);

            accountNoTextBox.Text = currentAccount.AccountNo;
            patientNameTextBox.Text = currentAccount.PatFullName;
            patientDOBTextBox.Text = ((DateTime)(currentAccount.BirthDate)).ToShortDateString();
            clientTextBox.Text = currentAccount.ClientName;
            fincodeTextBox.Text = currentAccount.FinCode;
            serviceDateTextBox.Text = ((DateTime)(currentAccount.TransactionDate)).ToShortDateString();

            dgvBatchEntry.AllowUserToAddRows = true;
            dgvBatchEntry.Focus();
            dgvBatchEntry.CurrentCell = dgvBatchEntry[nameof(BatchCharge.CDM), 0];
            dgvBatchEntry.BeginEdit(true);
        }

    }

    private void dgvBatchEntry_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {

    }

    private void dgvBatchEntry_RowEnter(object sender, DataGridViewCellEventArgs e)
    {

    }

    private void dgvBatchEntry_CellEnter(object sender, DataGridViewCellEventArgs e)
    {
        if (dgvBatchEntry.CurrentRow.Cells[e.ColumnIndex].ReadOnly)
        {
            SendKeys.Send("{tab}");
        }
    }
}
