using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using LabBilling.Library;
using LabBilling.Logging;
using LabBilling.Core.Services;

namespace LabBilling.Forms
{
    public partial class PaymentAdjustmentEntryForm : BaseForm
    {
        private Account _account;

        public Chk chk = new Chk();

        private DictionaryService dictionaryService;

        public PaymentAdjustmentEntryForm(ref Account account)
        {
            InitializeComponent();

            _account = account;
            dictionaryService = new(Program.AppEnvironment);
        }

        private void PaymentAdjustmentEntryForm_Load(object sender, EventArgs e)
        {
            //get write off codes
            writeOffCodeComboBox.DataSource = dictionaryService.GetWriteOffCodes();
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
}
