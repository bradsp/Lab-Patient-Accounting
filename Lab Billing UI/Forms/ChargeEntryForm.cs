using LabBilling.Core;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;


namespace LabBilling.Forms;

public partial class ChargeEntryForm : Form
{
    private readonly Account _currentAccount = new();
    private readonly System.Windows.Forms.Timer _timer;
    private const int _timerInterval = 650;
    private readonly AccountService accountService;
    private readonly DictionaryService dictionaryService;


    public string SelectedCdm { get; set; }
    public decimal Quantity { get; set; }
    public double Amount { get; set; }
    public string Comment { get; set; }
    public string ReferenceNumber { get; set; }

    public ChargeEntryForm(Account currentAccount)
    {
        Log.Instance.Trace($"Entering");
        _currentAccount = currentAccount;
        InitializeComponent();
        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
        _timer.Tick += new EventHandler(cdmTextBox_KeyUpDone);
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

        SelectedCdm = cdmTextBox.Text;
        Quantity = quantityNumericUpDown.Value;
        ReferenceNumber = ReferenceNumberTextBox.Text;
        Comment = commentTextBox.Text;
        Amount = Convert.ToDouble(string.IsNullOrEmpty(amountTextBox.Text) ? "0.00" : amountTextBox.Text);

        if (string.IsNullOrEmpty(SelectedCdm))
        {
            MessageBox.Show("Please select a charge item.", "Incomplete request", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

    private void SearchByCheckChanged(object sender, EventArgs e)
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
        var cdmLookup = new CdmLookupForm
        {
            InitialSearchText = cdmTextBox.Text,
            Datasource = dictionaryService.GetAllCdms(false)
        };

        if (cdmLookup.ShowDialog() == DialogResult.OK)
        {
            //if cdm is a variable type, ask for amount
            cdmTextBox.Text = cdmLookup.SelectedValue;
            Cdm cdm = dictionaryService.GetCdm(cdmLookup.SelectedValue);
            if (cdm.Variable)
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
