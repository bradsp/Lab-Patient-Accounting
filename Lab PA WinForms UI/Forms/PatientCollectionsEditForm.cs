using System;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Logging;
using LabBilling.Core.Services;
using System.ComponentModel;

namespace LabBilling.Forms;

public partial class PatientCollectionsEditForm : Form
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SelectedRecord { get; set; }
    private BadDebt badDebt = new BadDebt();
    private AccountService accountService;
    private PatientBillingService patientBillingService;

    public PatientCollectionsEditForm(string selectedGuid) 
    {
        Log.Instance.Trace($"Entering");
        InitializeComponent();

        SelectedRecord = selectedGuid;

        accountService = new(Program.AppEnvironment);
        patientBillingService = new(Program.AppEnvironment);
    }
    
    private void BadDebtEditForm_Load(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        if (SelectedRecord is null || SelectedRecord == "")
        {
            MessageBox.Show("No record is selected.");
            return;
        }
        else
        {
            badDebt = patientBillingService.GetCollectionRecord(SelectedRecord);
            LoadData();
        }

    }

    private void LoadData()
    {
        Log.Instance.Trace($"Entering");
        LastName.Text = badDebt.DebtorLastName;
        FirstName.Text = badDebt.DebtorFirstName;
        Address1.Text = badDebt.StreetAddress1;
        Address2.Text = badDebt.StreetAddress2;
        City.Text = badDebt.City;
        State.Text = badDebt.State;
        stateZipLabel.Text = badDebt.StateZip;
        ZipCode.Text = badDebt.Zip;
        Spouse.Text = badDebt.Spouse;
        Phone.Text = badDebt.Phone;
        SocSecNo.Text = badDebt.SocialSecurityNo;
        License.Text = badDebt.LicenseNumber;
        Employment.Text = badDebt.Employment;
        Remarks.Text = badDebt.Remarks;
        Remarks2.Text = badDebt.Remarks2;
        AccountNo.Text = badDebt.AccountNo;
        PatientName.Text = badDebt.PatientName;
        Misc.Text = badDebt.Misc;
        ServiceDate.Text = badDebt.ServiceDate?.ToString("MM/dd/yyyy");
        PaymentDate.Text = badDebt.PaymentDate?.ToString("MM/dd/yyyy");
        Balance.Text = badDebt.Balance.ToString();
        DateEntered.Text = badDebt.DateEntered?.ToString("MM/dd/yyyy");
        DateSent.Text = badDebt.DateSent?.ToString("MM/dd/yyyy");
    }

    private void ReadData()
    {
        Log.Instance.Trace($"Entering");
        badDebt.DebtorLastName = LastName.Text;
        badDebt.DebtorFirstName = FirstName.Text;
        badDebt.StreetAddress1 = Address1.Text;
        badDebt.StreetAddress2 = Address2.Text;
        badDebt.City = City.Text;
        badDebt.State = State.Text;
        badDebt.Zip = ZipCode.Text;
        badDebt.Spouse = Spouse.Text;
        badDebt.Phone = Phone.Text;
        badDebt.SocialSecurityNo = SocSecNo.Text;
        badDebt.LicenseNumber = License.Text;
        badDebt.Employment = Employment.Text;
        badDebt.Remarks = Remarks.Text;
        badDebt.Remarks2 = Remarks2.Text;
        badDebt.AccountNo = AccountNo.Text;
        badDebt.PatientName = PatientName.Text;
        badDebt.Misc = Misc.Text;

        if(!DateTime.TryParse(ServiceDate.Text, out DateTime serviceDate))
        {
            badDebt.ServiceDate = null;
        }
        else
        {
            badDebt.ServiceDate = serviceDate;
        }
        if(!DateTime.TryParse(PaymentDate.Text, out DateTime paymentDate))
        {
            badDebt.PaymentDate = null;
        }
        else
        {
            badDebt.PaymentDate = paymentDate;
        }
        badDebt.Balance = Convert.ToDouble(Balance.Text);
    }

    private void Save_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        ReadData();
        try
        {
            patientBillingService.SaveCollectionRecord(badDebt);
        }
        catch(Exception ex)
        {
            DialogResult = DialogResult.Abort;
            Log.Instance.Error(ex.ToString(), ex);
        }

        return;
    }

    private void Cancel_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering");
        this.DialogResult = DialogResult.Cancel;
        return;
    }
}
