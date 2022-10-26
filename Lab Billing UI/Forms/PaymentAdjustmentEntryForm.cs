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

namespace LabBilling.Forms
{
    public partial class PaymentAdjustmentEntryForm : Form
    {
        private Account _account;

        public Chk chk = new Chk();

        public PaymentAdjustmentEntryForm(ref Account account)
        {
            InitializeComponent();

            _account = account;
        }

        private void PaymentAdjustmentEntryForm_Load(object sender, EventArgs e)
        {
            //get write off codes
            WriteOffCodeRepository writeOffCodeRepository = new WriteOffCodeRepository(Helper.ConnVal);
            writeOffCodeComboBox.DataSource = writeOffCodeRepository.GetAll();
            writeOffCodeComboBox.DisplayMember = nameof(WriteOffCode.Description);
            writeOffCodeComboBox.ValueMember = nameof(WriteOffCode.Code);
            writeOffCodeComboBox.SelectedIndex = -1;

            //load ins combobox with account's insurances
            insuranceComboBox.DataSource = _account.Insurances;
            insuranceComboBox.ValueMember = nameof(Ins.InsCode);
            insuranceComboBox.DisplayMember = nameof(Ins.PlanName);
            insuranceComboBox.SelectedIndex = -1;

            // todo: investigate restriction from legacy system:
            // CODE 1500 SHOULD NOT BE USED TO WRITE OFF FOR BAD DEBT. BAD DEBT CANNOT BE HANDLED BY THE ACCOUNT PROGRAM.
            

        }

        private void postButton_Click(object sender, EventArgs e)
        {
            try
            {
                chk.CheckNo = checkNoTextBox.Text;
                if (!string.IsNullOrEmpty(dateReceivedTextBox.Text) && dateReceivedTextBox.MaskFull)
                    chk.DateReceived = DateTime.Parse(dateReceivedTextBox.Text);
                if (!string.IsNullOrEmpty(checkDateTextBox.Text) && checkDateTextBox.MaskFull)
                    chk.ChkDate = DateTime.Parse(checkDateTextBox.Text);
                chk.Comment = commentTextBox.Text;
                chk.ContractualAmount = Convert.ToDouble(contractualAmtTextBox.DollarValue);
                chk.WriteOffAmount = Convert.ToDouble(writeOffAmtTextBox.DollarValue);
                if(writeOffCodeComboBox.SelectedValue != null)
                    chk.WriteOffCode = writeOffCodeComboBox.SelectedValue.ToString();
                if (!string.IsNullOrEmpty(writeOffDateTextBox.Text) && writeOffDateTextBox.MaskFull)
                    chk.WriteOffDate = DateTime.Parse(writeOffDateTextBox.Text);
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

    }
}
