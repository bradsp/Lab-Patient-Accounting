using System;
using System.ComponentModel;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Logging;
using LabBilling.Core.Services;

namespace LabBilling.Forms;

public partial class PaymentAdjustmentEntryForm : Form
{
    private readonly Account _account;

    public Chk chk = new();

    private readonly DictionaryService _dictionaryService;
    private readonly ToolTip _toolTip = new();

    public PaymentAdjustmentEntryForm(ref Account account) 
    {
        InitializeComponent();

        _account = account;
        _dictionaryService = new(Program.AppEnvironment);
    }

    private void PaymentAdjustmentEntryForm_Load(object sender, EventArgs e)
    {
        //get write off codes
        writeOffCodeComboBox.DataSource = _dictionaryService.GetWriteOffCodes();
        writeOffCodeComboBox.DisplayMember = nameof(WriteOffCode.Description);
        writeOffCodeComboBox.ValueMember = nameof(WriteOffCode.Code);
        writeOffCodeComboBox.SelectedIndex = -1;

        writeOffCodeTextBox.Text = string.Empty;

        //load ins combobox with account's insurances
        insuranceComboBox.DataSource = _account.Insurances;
        insuranceComboBox.ValueMember = nameof(Ins.InsCode);
        insuranceComboBox.DisplayMember = nameof(Ins.PlanName);
        insuranceComboBox.SelectedIndex = -1;

        paymentAmtTextBox.Enabled = Program.LoggedInUser.CanAddPayments;
        refundCheckBox.Enabled = Program.LoggedInUser.CanAddPayments;

        // enforce positive currency amount for payment
        paymentAmtTextBox.Validating += paymentAmtTextBox_Validating;
        // enforce positive currency amount for contractual and write-off amounts
        contractualAmtTextBox.Validating += contractualAmtTextBox_Validating;
        writeOffAmtTextBox.Validating += writeOffAmtTextBox_Validating;
        // todo: investigate restriction from legacy system:
        // CODE 1500 SHOULD NOT BE USED TO WRITE OFF FOR BAD DEBT. BAD DEBT CANNOT BE HANDLED BY THE ACCOUNT PROGRAM.
    }

    private void postButton_Click(object sender, EventArgs e)
    {
        if (!this.ValidateChildren())
        {
            return;
        }

        try
        {
            chk.CheckNo = checkNoTextBox.Text;
            chk.DateReceived = dateReceivedTextBox.DateValue;
            chk.ChkDate = checkDateTextBox.DateValue;
            chk.Comment = commentTextBox.Text;
            chk.ContractualAmount = Convert.ToDouble(contractualAmtTextBox.DollarValue);
            chk.WriteOffAmount = Convert.ToDouble(writeOffAmtTextBox.DollarValue);
            chk.WriteOffDate = writeOffDateTextBox.DateValue;
            if(chk.WriteOffDate != null)
            {
                if (writeOffCodeComboBox.SelectedValue != null)
                    chk.WriteOffCode = writeOffCodeComboBox.SelectedValue.ToString();

                if(string.IsNullOrEmpty(chk.WriteOffCode))
                {
                    //must have write off code
                    errorProvider1.SetError(writeOffCodeComboBox, "Must select a write off reason");
                    return;
                }
            }

            chk.InsCode = insuranceComboBox.SelectedValue != null ? insuranceComboBox.SelectedValue.ToString() : String.Empty;
            chk.Source = fromTextBox.Text;

            chk.PaidAmount = Convert.ToDouble(paymentAmtTextBox.DollarValue);
            chk.IsRefund = refundCheckBox.Checked;
            if (refundCheckBox.Checked)
            {
                if (chk.PaidAmount > 0)
                    chk.PaidAmount *= -1;
            }

            DialogResult = DialogResult.OK;
            return;
        }
        catch(Exception ex)
        {
            Log.Instance.Error(ex, "Error parsing payment information.");
            MessageBox.Show("Error parsing payment information. No payment information written.");
            DialogResult = DialogResult.Abort;
            return;
        }
    }

    private void dateReceivedTextBox_Validated(object sender, EventArgs e)
    {
        if(dateReceivedTextBox.DateValue == DateTime.MinValue)
        {
            errorProvider1.SetError(dateReceivedTextBox, "Enter a valid date.");
        }
        else
        {
            errorProvider1.SetError(dateReceivedTextBox, string.Empty);
        }
    }

    private void checkDateTextBox_Validated(object sender, EventArgs e)
    {
        if (checkDateTextBox.DateValue == DateTime.MinValue && !string.IsNullOrEmpty(checkDateTextBox.Text))
        {
            errorProvider1.SetError(checkDateTextBox, "Enter a valid date.");
        }
        else
        {
            errorProvider1.SetError(checkDateTextBox, string.Empty);
        }
    }

    private void writeOffDateTextBox_Validated(object sender, EventArgs e)
    {
        if (writeOffDateTextBox.DateValue == DateTime.MinValue && !string.IsNullOrEmpty(writeOffDateTextBox.Text))
        {
            errorProvider1.SetError(writeOffDateTextBox, "Enter a valid date.");
        }
        else
        {
            errorProvider1.SetError(writeOffDateTextBox, string.Empty);
        }
    }

    private void writeOffAmtTextBox_Validated(object sender, EventArgs e)
    {
        if (!string.IsNullOrEmpty(writeOffAmtTextBox.Text))
            writeOffDateTextBox.Text = dateReceivedTextBox.Text;
    }

    private void paymentAmtTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (IsCancelling())
        {
            // allow cancel to bypass validation
            return;
        }
        // allow empty when payments are disabled; otherwise require a positive currency amount
        if (!paymentAmtTextBox.Enabled)
        {
            errorProvider1.SetError(paymentAmtTextBox, string.Empty);
            _toolTip.SetToolTip(paymentAmtTextBox, string.Empty);
            return;
        }

        var text = paymentAmtTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(text))
        {
            errorProvider1.SetError(paymentAmtTextBox, "Enter a payment amount.");
            _toolTip.SetToolTip(paymentAmtTextBox, "Please enter a positive amount");
            e.Cancel = true;
            return;
        }

        try
        {
            var amount = Convert.ToDouble(paymentAmtTextBox.DollarValue);
            if (amount <= 0)
            {
                errorProvider1.SetError(paymentAmtTextBox, "Amount must be a positive currency value.");
                _toolTip.SetToolTip(paymentAmtTextBox, "Please enter a positive amount");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(paymentAmtTextBox, string.Empty);
                _toolTip.SetToolTip(paymentAmtTextBox, string.Empty);
            }
        }
        catch
        {
            errorProvider1.SetError(paymentAmtTextBox, "Enter a valid currency amount.");
            _toolTip.SetToolTip(paymentAmtTextBox, "Please enter a positive amount");
            e.Cancel = true;
        }
    }

    private void contractualAmtTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (IsCancelling())
        {
            return;
        }
        var text = contractualAmtTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(text))
        {
            errorProvider1.SetError(contractualAmtTextBox, "Enter a contractual amount.");
            _toolTip.SetToolTip(contractualAmtTextBox, "Please enter a positive amount");
            e.Cancel = true;
            return;
        }

        try
        {
            var amount = Convert.ToDouble(contractualAmtTextBox.DollarValue);
            if (amount <= 0)
            {
                errorProvider1.SetError(contractualAmtTextBox, "Amount must be a positive currency value.");
                _toolTip.SetToolTip(contractualAmtTextBox, "Please enter a positive amount");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(contractualAmtTextBox, string.Empty);
                _toolTip.SetToolTip(contractualAmtTextBox, string.Empty);
            }
        }
        catch
        {
            errorProvider1.SetError(contractualAmtTextBox, "Enter a valid currency amount.");
            _toolTip.SetToolTip(contractualAmtTextBox, "Please enter a positive amount");
            e.Cancel = true;
        }
    }

    private void writeOffAmtTextBox_Validating(object sender, CancelEventArgs e)
    {
        if (IsCancelling())
        {
            return;
        }
        var text = writeOffAmtTextBox.Text?.Trim();
        if (string.IsNullOrEmpty(text))
        {
            errorProvider1.SetError(writeOffAmtTextBox, "Enter a write-off amount.");
            _toolTip.SetToolTip(writeOffAmtTextBox, "Please enter a positive amount");
            e.Cancel = true;
            return;
        }

        try
        {
            var amount = Convert.ToDouble(writeOffAmtTextBox.DollarValue);
            if (amount <= 0)
            {
                errorProvider1.SetError(writeOffAmtTextBox, "Amount must be a positive currency value.");
                _toolTip.SetToolTip(writeOffAmtTextBox, "Please enter a positive amount");
                e.Cancel = true;
            }
            else
            {
                errorProvider1.SetError(writeOffAmtTextBox, string.Empty);
                _toolTip.SetToolTip(writeOffAmtTextBox, string.Empty);
            }
        }
        catch
        {
            errorProvider1.SetError(writeOffAmtTextBox, "Enter a valid currency amount.");
            _toolTip.SetToolTip(writeOffAmtTextBox, "Please enter a positive amount");
            e.Cancel = true;
        }
    }

    private bool IsCancelling()
    {
        if (ActiveControl is Button btn)
        {
            if (btn.DialogResult == DialogResult.Cancel || !btn.CausesValidation)
            {
                return true;
            }
        }
        return false;
    }

    bool writeOffCodeDoNotChange = false;
    private void writeOffCodeTextBox_TextChanged(object sender, EventArgs e)
    {
        if (!writeOffCodeDoNotChange)
        {
            writeOffCodeDoNotChange = true;
            writeOffCodeComboBox.SelectedValue = writeOffCodeTextBox.Text;
            writeOffCodeDoNotChange = false;
        }
    }

    private void writeOffCodeComboBox_SelectedValueChanged(object sender, EventArgs e)
    {
        if(!writeOffCodeDoNotChange)
        {
            if (writeOffCodeComboBox.SelectedValue != null)
            {
                writeOffCodeDoNotChange = true;
                writeOffCodeTextBox.Text = writeOffCodeComboBox.SelectedValue.ToString();
                writeOffCodeDoNotChange = false;
            }
        }

    }
}
