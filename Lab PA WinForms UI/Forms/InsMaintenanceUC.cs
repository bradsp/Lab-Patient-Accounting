using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Library;
using Newtonsoft.Json;
using System.ComponentModel;

namespace LabBilling.Forms;

public partial class InsMaintenanceUC : UserControl
{
    private readonly AccountService _accountService;
    private readonly DictionaryService _dictionaryService;
    private readonly List<InsCompany> _insCompanies;
    private readonly InsCompanyLookupForm _insCoLookupForm;
    private readonly List<string> _changedControls;
    private bool _allowEditing;
    private readonly System.Windows.Forms.Timer _timer;
    private const int _timerInterval = 650;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Account CurrentAccount { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Ins CurrentIns { get; set; }
    public event EventHandler<InsuranceUpdatedEventArgs> InsuranceChanged;
    public event EventHandler<AppErrorEventArgs> OnError;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public InsCoverage Coverage { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AllowEditing
    {
        get
        {
            return _allowEditing;
        }

        set
        {
            _allowEditing = value;
            Helper.SetControlsAccess(insTabLayoutPanel.Controls, _allowEditing);
            SetInsDataEntryAccess(_allowEditing);
            SaveInsuranceButton.Enabled = _allowEditing;
        }
    }

    public InsMaintenanceUC(InsCoverage coverage)
    {
        if (this.DesignMode)
            return;

        _accountService = new(Program.AppEnvironment, Program.UnitOfWork);
        _dictionaryService = new(Program.AppEnvironment, Program.UnitOfWork);

        InitializeComponent();
        Coverage = coverage;
        _changedControls = new();
        _insCoLookupForm = new InsCompanyLookupForm();
        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
        _timer.Tick += insurancePlanTextBox_KeyUpDone;
        #region Setup Insurance Company Combobox
        _insCompanies = DataCache.Instance.GetInsCompanies();
        #endregion

        InsRelationComboBox.DataSource = new BindingSource(Dictionaries.RelationSource, null);
        InsRelationComboBox.DisplayMember = "Value";
        InsRelationComboBox.ValueMember = "Key";

        HolderSexComboBox.DataSource = new BindingSource(Dictionaries.SexSource, null);
        HolderSexComboBox.DisplayMember = "Value";
        HolderSexComboBox.ValueMember = "Key";

        PlanFinCodeComboBox.DataSource = DataCache.Instance.GetFins();
        PlanFinCodeComboBox.DisplayMember = nameof(Fin.Description);
        PlanFinCodeComboBox.ValueMember = nameof(Fin.FinCode);
        PlanFinCodeComboBox.SelectedIndex = -1;

        HolderStateComboBox.DataSource = new BindingSource(Dictionaries.StateSource, null);
        HolderStateComboBox.DisplayMember = "Value";
        HolderStateComboBox.ValueMember = "Key";

        HolderStateComboBox.SelectedIndex = 0;
        HolderSexComboBox.SelectedIndex = 0;
        PlanFinCodeComboBox.BackColor = Color.Linen;

        //populate edit boxes
        HolderStateComboBox.SelectedItem = null;
        HolderStateComboBox.SelectedText = "--Select--";
        HolderSexComboBox.SelectedItem = null;
        HolderSexComboBox.SelectedText = "--Select--";

        CurrentIns = new Ins();

    }

    private void InsMaintenanceUC_Load(object sender, EventArgs e)
    {
        if (this == null)
            return;
        if (this.DesignMode)
            return;
    }

    public void LoadInsuranceData()
    {
        if (this == null)
            return;

        if (CurrentAccount == null)
            return;

        CurrentIns = CurrentAccount.Insurances.Find(i => i.Coverage == Coverage);

        if (CurrentIns == null)
            return;

        HolderLastNameTextBox.Text = CurrentIns.HolderLastName;
        HolderFirstNameTextBox.Text = CurrentIns.HolderFirstName;
        HolderMiddleNameTextBox.Text = CurrentIns.HolderMiddleName;
        HolderAddressTextBox.Text = CurrentIns.HolderStreetAddress ?? "";

        HolderCityTextBox.Text = CurrentIns.HolderCity;
        HolderStateComboBox.SelectedValue = CurrentIns.HolderState ?? "";
        HolderZipTextBox.Text = CurrentIns.HolderZip;

        HolderSexComboBox.SelectedValue = CurrentIns.HolderSex ?? "";
        HolderDOBTextBox.Text = CurrentIns.HolderBirthDate?.ToString("MM/dd/yyyy");
        InsRelationComboBox.SelectedValue = CurrentIns.Relation ?? "";

        insurancePlanTextBox.Text = CurrentIns.InsCode ?? "";

        PlanNameTextBox.Text = CurrentIns.PlanName;
        PlanAddressTextBox.Text = CurrentIns.PlanStreetAddress1;
        PlanAddress2TextBox.Text = CurrentIns.PlanStreetAddress2;
        PlanCityStTextBox.Text = CurrentIns.PlanCityState;


        if (CurrentIns.InsCompany.IsGenericPayor)
        {
            PlanNameTextBox.Enabled = true;
            PlanNameTextBox.ReadOnly = false;
            PlanAddressTextBox.Enabled = true;
            PlanAddress2TextBox.Enabled = true;
            PlanCityStTextBox.Enabled = true;
        }

        PolicyNumberTextBox.Text = CurrentIns.PolicyNumber;
        GroupNumberTextBox.Text = CurrentIns.GroupNumber;
        GroupNameTextBox.Text = CurrentIns.GroupName;
        CertSSNTextBox.Text = CurrentIns.CertSSN;
        PlanFinCodeComboBox.SelectedValue = CurrentIns.FinCode ?? "";

        //reset changed flag & colors
        foreach (Control ctrl in insTabLayoutPanel.Controls)
        {
            if (ctrl is TextBox || ctrl is ComboBox || ctrl is FlatCombo || ctrl is MaskedTextBox)
            {
                ctrl.BackColor = Color.White;
            }
            _changedControls.Remove(ctrl.Name);
        }

        //enable data entry fields
        SaveInsuranceButton.Enabled = true;
        SetInsDataEntryAccess(true);

    }

    private void SaveInsuranceButton_Click(object sender, EventArgs e)
    {
        // saves the insurance info back to the grid            
        OnError?.Invoke(this, new AppErrorEventArgs() { ErrorLevel = AppErrorEventArgs.ErrorLevelType.Trace, ErrorMessage = "Entering" });

        CurrentIns ??= new Ins();
        CurrentIns.Account = CurrentAccount.AccountNo;
        CurrentIns.CertSSN = CertSSNTextBox.Text;
        CurrentIns.GroupName = GroupNameTextBox.Text;
        CurrentIns.GroupNumber = GroupNumberTextBox.Text;
        CurrentIns.HolderStreetAddress = HolderAddressTextBox.Text;
        CurrentIns.HolderCity = HolderCityTextBox.Text;
        CurrentIns.HolderState = HolderStateComboBox.SelectedValue?.ToString();
        CurrentIns.HolderZip = HolderZipTextBox.Text;
        CurrentIns.HolderCityStZip = string.Format("{0}, {1} {2}",
            HolderCityTextBox.Text,
            HolderStateComboBox.SelectedValue?.ToString(),
            HolderZipTextBox.Text);

        if (CurrentIns.HolderCityStZip.Trim() == ",")
        {
            CurrentIns.HolderCityStZip = String.Empty;
        }

        if (HolderDOBTextBox.MaskCompleted)
            CurrentIns.HolderBirthDate = DateTime.Parse(HolderDOBTextBox.Text);
        else
            CurrentIns.HolderBirthDate = null;

        CurrentIns.HolderFirstName = HolderFirstNameTextBox.Text;
        CurrentIns.HolderLastName = HolderLastNameTextBox.Text;
        CurrentIns.HolderMiddleName = HolderMiddleNameTextBox.Text;
        CurrentIns.HolderFullName = $"{HolderLastNameTextBox.Text},{HolderFirstNameTextBox.Text} {HolderMiddleNameTextBox.Text}";
        CurrentIns.HolderSex = HolderSexComboBox.SelectedValue.ToString();
        CurrentIns.PolicyNumber = PolicyNumberTextBox.Text;
        CurrentIns.PlanStreetAddress1 = PlanAddressTextBox.Text;
        CurrentIns.PlanStreetAddress2 = PlanAddress2TextBox.Text;
        CurrentIns.PlanName = PlanNameTextBox.Text;
        CurrentIns.PlanCityState = PlanCityStTextBox.Text;
        CurrentIns.Relation = InsRelationComboBox.SelectedValue.ToString();
        CurrentIns.Coverage = Coverage;
        if (PlanFinCodeComboBox.SelectedValue != null)
            CurrentIns.FinCode = PlanFinCodeComboBox.SelectedValue.ToString();

        CurrentIns.InsCode = insurancePlanTextBox.Text;
        try
        {
            //call method to update the record in the database
            CurrentIns = _accountService.SaveInsurance(CurrentIns);
            CurrentIns.InsCompany = _dictionaryService.GetInsCompany(CurrentIns.InsCode);
            int index = CurrentAccount.Insurances.FindIndex(i => i.Coverage == Coverage);
            if (index != -1)
                CurrentAccount.Insurances[index] = CurrentIns;
            else
                CurrentAccount.Insurances.Add(CurrentIns);
            InsuranceChanged?.Invoke(this, new InsuranceUpdatedEventArgs() { UpdatedIns = CurrentIns });

        }
        catch (NullReferenceException nre)
        {
            string result = JsonConvert.SerializeObject(nre.Data, Formatting.Indented);

            OnError?.Invoke(this, new AppErrorEventArgs() { ErrorLevel = AppErrorEventArgs.ErrorLevelType.Error, ErrorMessage = nre.Message + "|" + result });
            InsTabMessageTextBox.Text = "Error occured during save. Contact your administrator."; ;
        }
        catch (Exception ex)
        {
            OnError?.Invoke(this, new AppErrorEventArgs() { ErrorLevel = AppErrorEventArgs.ErrorLevelType.Error, ErrorMessage = ex.Message });
            InsTabMessageTextBox.Text = "Error occured during save. Contact your administrator.";
        }
    }

    private void ClearInsEntryFields()
    {
        PlanAddressTextBox.Text = String.Empty;
        PlanAddress2TextBox.Text = String.Empty;
        PlanCityStTextBox.Text = String.Empty;
        PlanNameTextBox.Text = String.Empty;
        HolderAddressTextBox.Text = String.Empty;
        HolderCityTextBox.Text = String.Empty;
        HolderDOBTextBox.Text = String.Empty;
        HolderFirstNameTextBox.Text = String.Empty;
        HolderLastNameTextBox.Text = String.Empty;
        HolderMiddleNameTextBox.Text = String.Empty;
        HolderZipTextBox.Text = String.Empty;
        PolicyNumberTextBox.Text = String.Empty;
        GroupNameTextBox.Text = String.Empty;
        GroupNumberTextBox.Text = String.Empty;
        CertSSNTextBox.Text = String.Empty;
        insurancePlanTextBox.Text = String.Empty;
        HolderSexComboBox.SelectedIndex = 0;
        HolderStateComboBox.SelectedIndex = 0;
        PlanFinCodeComboBox.SelectedIndex = -1;

        //disable fields
        ResetControls(insTabLayoutPanel.Controls.OfType<Control>().ToArray());
    }

    private void SetInsDataEntryAccess(bool enable)
    {
        PlanAddressTextBox.Enabled = enable;
        PlanAddress2TextBox.Enabled = enable;
        PlanCityStTextBox.Enabled = enable;
        PlanNameTextBox.Enabled = enable;
        HolderAddressTextBox.Enabled = enable;
        HolderCityTextBox.Enabled = enable;
        HolderDOBTextBox.Enabled = enable;
        HolderFirstNameTextBox.Enabled = enable;
        HolderLastNameTextBox.Enabled = enable;
        HolderMiddleNameTextBox.Enabled = enable;
        HolderZipTextBox.Enabled = enable;
        PolicyNumberTextBox.Enabled = enable;
        GroupNameTextBox.Enabled = enable;
        GroupNumberTextBox.Enabled = enable;
        CertSSNTextBox.Enabled = enable;
        HolderStateComboBox.Enabled = enable;
        HolderSexComboBox.Enabled = enable;
        PlanFinCodeComboBox.Enabled = enable;
        InsRelationComboBox.Enabled = enable;
        insurancePlanTextBox.Enabled = enable;
    }

    private void LookupInsCode(string code)
    {
        //lookup code to see if it is valid, then populate other plan fields from data

        if (code == "")
            return;

        var record = _dictionaryService.GetInsCompany(code);

        if (record != null)
        {
            //this is a valid code

            PlanFinCodeComboBox.SelectedValue = record.FinancialCode ?? String.Empty;

            if (record.IsGenericPayor)
            {
                PlanNameTextBox.Enabled = true;
                PlanNameTextBox.ReadOnly = false;
                PlanAddress2TextBox.Enabled = true;
                PlanAddress2TextBox.Enabled = true;
                PlanCityStTextBox.Enabled = true;
                PlanFinCodeComboBox.Enabled = true;
            }
            else
            {
                PlanNameTextBox.ReadOnly = true;
                PlanNameTextBox.Text = record.PlanName;
                PlanAddressTextBox.Text = record.Address1;
                PlanAddress2TextBox.Text = record.Address2;
                PlanCityStTextBox.Text = record.CityStateZip;
            }
        }
    }

    private void AddInsuranceButton_Click(object sender, EventArgs e)
    {
        //clear data entry fields.
        ClearInsEntryFields();
        SetInsDataEntryAccess(true);
        SaveInsuranceButton.Enabled = true;
    }

    private void insurancePlanTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        _timer.Stop();
        _timer.Start();
    }

    private void insurancePlanTextBox_KeyUpDone(object sender, EventArgs e)
    {
        _timer.Stop();
        if (insurancePlanTextBox.Text.Length > 2)
        {
            _insCoLookupForm.Datasource = _insCompanies;
            _insCoLookupForm.InitialSearchText = insurancePlanTextBox.Text;
            if (_insCoLookupForm.ShowDialog() == DialogResult.OK)
            {
                string insCode = insurancePlanTextBox.Text = _insCoLookupForm.SelectedValue;
                LookupInsCode(insCode);
            }
        }
    }

    private void deleteInsButton_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show(this, string.Format("Delete {0} insurance {1} for this patient?",
            CurrentIns.Coverage,
            CurrentIns.PlanName),
            "Delete Insurance", MessageBoxButtons.YesNo) == DialogResult.Yes)
        {
            try
            {
                if (_accountService.DeleteInsurance(CurrentIns))
                {
                    int index = CurrentAccount.Insurances.FindIndex(i => i.Coverage == Coverage);
                    if (index != -1)
                    {
                        CurrentAccount.Insurances.RemoveAt(index);
                    }
                    InsuranceChanged?.Invoke(this, new InsuranceUpdatedEventArgs());
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke(this, new AppErrorEventArgs() { ErrorLevel = AppErrorEventArgs.ErrorLevelType.Error, ErrorMessage = ex.Message });
                InsTabMessageTextBox.Text = "Failed to delete insurance. Contact your administrator.";
            }
        }
        ClearInsEntryFields();
    }

    private void ResetControls(Control[] controls)
    {
        //reset changed flag & colors
        foreach (Control ctrl in controls)
        {
            if (ctrl is TextBox || ctrl is ComboBox || ctrl is FlatCombo || ctrl is MaskedTextBox)
            {
                ctrl.BackColor = Color.White;
            }
            _changedControls.Remove(ctrl.Name);
        }
    }

    private void InsCopyPatientLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        //copy patient data for insurance holder info
        HolderLastNameTextBox.Text = CurrentAccount.PatLastName;
        HolderFirstNameTextBox.Text = CurrentAccount.PatFirstName;
        HolderMiddleNameTextBox.Text = CurrentAccount.PatMiddleName;
        HolderAddressTextBox.Text = CurrentAccount.Pat.Address1;
        HolderCityTextBox.Text = CurrentAccount.Pat.City;
        HolderStateComboBox.SelectedValue = CurrentAccount.Pat.State ?? "";
        HolderZipTextBox.Text = CurrentAccount.Pat.ZipCode;
        HolderDOBTextBox.Text = CurrentAccount.BirthDate?.ToString("MM/dd/yyyy");
        HolderSexComboBox.SelectedValue = CurrentAccount.Sex ?? "";
        InsRelationComboBox.SelectedValue = "01";
    }

}

public class InsuranceUpdatedEventArgs : EventArgs
{
    public Ins UpdatedIns { get; set; }
    public InsuranceUpdatedEventArgs()
    {

    }
}
