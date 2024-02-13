using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core;
using System;
using System.Windows.Forms;
using LabBilling.Logging;
using LabBilling.Core.Services;


namespace LabBilling.Forms
{
    public partial class ChargeEntryForm : BaseForm
    {
        private Account _currentAccount = new();
        private Timer _timer;
        private const int _timerInterval = 650;
        private AccountService accountService;
        private DictionaryService dictionaryService;

        public ChargeEntryForm(Account currentAccount)
        {
            Log.Instance.Trace($"Entering");
            _currentAccount = currentAccount;
            InitializeComponent();
            _timer = new Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(cdmTextBox_KeyUpDone);
            accountService = new(Program.AppEnvironment);
            dictionaryService = new(Program.AppEnvironment);
        }

        private void ChargeEntryForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tbBannerAccount.Text = _currentAccount.AccountNo;
            tbBannerName.Text = _currentAccount.PatFullName;
            tbBannerMRN.Text = _currentAccount.MRN;
            tbDateOfService.Text = _currentAccount.TransactionDate.ToShortDateString();
        }

        private void btnSubmit_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            try
            {
                string cdm = "";

                cdm = cdmTextBox.Text;

                if(string.IsNullOrEmpty(cdm))
                {
                    MessageBox.Show("Please select a charge item.", "Incomplete request", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                accountService.AddCharge(_currentAccount.AccountNo,
                    cdm,
                    Convert.ToInt32(nQty.Value),
                    _currentAccount.TransactionDate,
                    tbComment.Text,
                    ReferenceNumber.Text,
                    Convert.ToDouble(!string.IsNullOrWhiteSpace(amountTextBox.Text) ? amountTextBox.Text : "0.00"));
            }
            catch(CdmNotFoundException)
            {
                // this should not happen
                MessageBox.Show("CDM number is not valid. Charge entry failed.");
            }
            catch(AccountNotFoundException)
            {
                MessageBox.Show("Account number is not valid.");
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message + "\nCharge not written.","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Cancel action in ChargeEntryForm");
            DialogResult = DialogResult.Cancel;
            return;
        }

        private void SearchByCheckChanged (object sender, EventArgs e)
        {

        }

        private void cdmTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void cdmTextBox_KeyUpDone(object sender, EventArgs e)
        {
            _timer.Stop();
            var cdmLookup = new CdmLookupForm();
            cdmLookup.InitialSearchText = cdmTextBox.Text;
            cdmLookup.Datasource = dictionaryService.GetAllCdms(false);

            if(cdmLookup.ShowDialog() == DialogResult.OK)
            {
                //if cdm is a variable type, ask for amount
                cdmTextBox.Text = cdmLookup.SelectedValue;
                Cdm cdm = dictionaryService.GetCdm(cdmLookup.SelectedValue);
                if(cdm.Variable)
                {
                    var result = GetAmount();
                    if (result.ReturnCode == DialogResult.OK)
                    {
                        amountTextBox.Text = result.Text;
                    }
                    else
                    {
                        cdmTextBox.Text = string.Empty;
                    }
                }
            }
        }

        private static InputBoxResult GetAmount()
        {
            var result = InputBox.Show("Enter amount:");
            if (result.ReturnCode == DialogResult.OK)
            {
                if (decimal.TryParse(result.Text, out decimal amountValue))
                {
                    return result;
                }
                else
                {
                    return GetAmount();
                }
            }
            return result;
        }
    }
}
