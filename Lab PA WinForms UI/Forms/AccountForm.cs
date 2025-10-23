using LabBilling.Core;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Core.UnitOfWork;
using LabBilling.Legacy;
using LabBilling.Library;
using LabBilling.Logging;
using LabBilling.LookupForms;
using System.ComponentModel;
using System.Data;
using Utilities;
using WinFormsLibrary;

namespace LabBilling.Forms;

public partial class AccountForm : Form
{
    private readonly AccountService _accountService;
    private readonly DictionaryService _dictionaryService;
    private BindingList<PatDiag> _dxBindingList;
    private readonly DataTable _dxPointers = new();
    private readonly BindingSource _dxPointerBindingSource = new();
    private const string _setHoldMenuText = "Set Claim Hold";
    private const string _clearHoldMenuText = "Clear Claim Hold";

    private List<Phy> _providers = null;
    private Account _currentAccount = null;
    private readonly List<string> _changedControls = new();
    private readonly Dictionary<Control, string> _controlColumnMap = new();

    private bool _billingTabLoading = false;
    private const int _timerInterval = 650;
    private const string _notesAlertText = "** SEE NOTES **";
    //private bool _closing = false;
    private bool _readOnly = false;
    private readonly ChargeMaintenanceUC _chargeMaintenance = new();
    private readonly InsMaintenanceUC _insPrimaryMaintenanceUC = new(InsCoverage.Primary);
    private readonly InsMaintenanceUC _insSecondaryMaintenanceUC = new(InsCoverage.Secondary);
    private readonly InsMaintenanceUC _insTertiaryMaintenanceUC = new(InsCoverage.Tertiary);
    private ContextMenuStrip _chkTabContextMenu = new();
    private bool _isClosing = false;
    private System.Windows.Forms.Timer _timer;

    public event EventHandler<string> AccountOpenedEvent;
    public event EventHandler<Account> AccountUpdatedEvent;

    private readonly string _selectedAccount;
    public string SelectedAccount
    {
        get { return _selectedAccount; }
    }

    private readonly ListBox providerSearchListBox = new();

    /// <summary>
    /// Construct form with an account to open and optionally send the MDI parent form.
    /// </summary>
    /// <param name="account"></param>
    /// <param name="parentForm"></param>
    public AccountForm(string account) : this()
    {
        Log.Instance.Trace($"Entering - {account}");
        _dictionaryService = new(Program.AppEnvironment);
        _accountService = new(Program.AppEnvironment);

        if (account != null)
            _selectedAccount = account;
    }

    private AccountForm()
    {
        Log.Instance.Trace("Entering");
        InitializeComponent();
    }

    #region MainForm

    private void AccountForm_Load(object sender, EventArgs e)
    {
        if (SelectedAccount != null)
        {
            Log.Instance.Trace($"Entering - {SelectedAccount}");
        }
        else
        {
            Log.Instance.Trace("Entering - no selected account.");
        }


        bannerPanel.BackColor = Color.Blue;

        try
        {
            bannerPanel.GetAllControls<TextBox>().ForEach(tb =>
            {
                tb.BackColor = Color.Blue;
                tb.ForeColor = Color.White;
            });

            bannerPanel.GetAllControls<Label>().ForEach(l =>
            {
                l.ForeColor = Color.White;
                l.BackColor = Color.Blue;
            });
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error setting control formatting.");
            MessageBox.Show("Unable to load Account. Contact your administrator.", "Error during load", MessageBoxButtons.OK, MessageBoxIcon.Error);
            _isClosing = true;
            this.Visible = false;
            this.Close();
            return;
        }

        providerSearchListBox.Visible = false;
        tabDemographics.Controls.Add(providerSearchListBox);

        tabCharges.Controls.Add(_chargeMaintenance);
        _chargeMaintenance.Dock = DockStyle.Fill;
        _chargeMaintenance.ChargesUpdated += DataChanged_EventHandler;
        _chargeMaintenance.OnError += UserControl_OnError;

        tabInsPrimary.Controls.Add(_insPrimaryMaintenanceUC);
        _insPrimaryMaintenanceUC.Dock = DockStyle.Fill;
        _insPrimaryMaintenanceUC.InsuranceChanged += InsChanged_EventHander;
        _insPrimaryMaintenanceUC.OnError += UserControl_OnError;

        tabInsSecondary.Controls.Add(_insSecondaryMaintenanceUC);
        _insSecondaryMaintenanceUC.Dock = DockStyle.Fill;
        _insSecondaryMaintenanceUC.InsuranceChanged += InsChanged_EventHander;
        _insSecondaryMaintenanceUC.OnError += UserControl_OnError;

        tabInsTertiary.Controls.Add(_insTertiaryMaintenanceUC);
        _insTertiaryMaintenanceUC.Dock = DockStyle.Fill;
        _insTertiaryMaintenanceUC.InsuranceChanged += InsChanged_EventHander;
        _insTertiaryMaintenanceUC.OnError += UserControl_OnError;

        #region Process permissions and enable controls

        SetFormPermissions();

        #endregion

        #region load controlColumnMap
        _controlColumnMap.Add(SocSecNoTextBox, nameof(Account.SocSecNo));
        _controlColumnMap.Add(DateOfBirthTextBox, nameof(Account.BirthDate));
        _controlColumnMap.Add(SexComboBox, nameof(Account.Sex));
        _controlColumnMap.Add(SuffixTextBox, nameof(Account.PatNameSuffix));
        _controlColumnMap.Add(LastNameTextBox, nameof(Account.PatNameSuffix));
        _controlColumnMap.Add(MiddleNameTextBox, nameof(Account.PatMiddleName));
        _controlColumnMap.Add(FirstNameTextBox, nameof(Account.PatFirstName));

        _controlColumnMap.Add(ZipcodeTextBox, nameof(Pat.ZipCode));
        _controlColumnMap.Add(MaritalStatusComboBox, nameof(Pat.MaritalStatus));
        _controlColumnMap.Add(EmailAddressTextBox, nameof(Pat.EmailAddress));

        _controlColumnMap.Add(StateComboBox, nameof(Pat.State));
        _controlColumnMap.Add(PhoneTextBox, nameof(Pat.PrimaryPhone));
        _controlColumnMap.Add(CityTextBox, nameof(Pat.City));
        _controlColumnMap.Add(Address2TextBox, nameof(Pat.Address2));
        _controlColumnMap.Add(Address1TextBox, nameof(Pat.Address1));
        _controlColumnMap.Add(GuarZipTextBox, nameof(Pat.GuarantorZipCode));
        _controlColumnMap.Add(GuarSuffixTextBox, nameof(Pat.GuarantorNameSuffix));
        _controlColumnMap.Add(GuarMiddleNameTextBox, nameof(Pat.GuarantorMiddleName));
        _controlColumnMap.Add(GuarFirstNameTextBox, nameof(Pat.GuarantorFirstName));
        _controlColumnMap.Add(GuarStateComboBox, nameof(Pat.GuarantorState));
        _controlColumnMap.Add(GuarantorRelationComboBox, nameof(Pat.GuarRelationToPatient));
        _controlColumnMap.Add(GuarantorPhoneTextBox, nameof(Pat.GuarantorPrimaryPhone));
        _controlColumnMap.Add(GuarCityTextBox, nameof(Pat.GuarantorCity));
        _controlColumnMap.Add(GuarantorAddressTextBox, nameof(Pat.GuarantorAddress));
        _controlColumnMap.Add(GuarantorLastNameTextBox, nameof(Pat.GuarantorLastName));
        _controlColumnMap.Add(orderingPhyTextBox, nameof(Pat.ProviderId));

        #endregion

        #region Setup ordering provider combo box

        _providers = DataCache.Instance.GetProviders();
        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
        _timer.Tick += _timer_Tick;

        #endregion

        #region populate combo boxes

        StateComboBox.DataSource = new BindingSource(Dictionaries.StateSource, null);
        StateComboBox.DisplayMember = "Value";
        StateComboBox.ValueMember = "Key";

        SexComboBox.DataSource = new BindingSource(Dictionaries.SexSource, null);
        SexComboBox.DisplayMember = "Value";
        SexComboBox.ValueMember = "Key";

        MaritalStatusComboBox.DataSource = new BindingSource(Dictionaries.MaritalSource, null);
        MaritalStatusComboBox.DisplayMember = "Value";
        MaritalStatusComboBox.ValueMember = "Key";

        GuarStateComboBox.DataSource = new BindingSource(Dictionaries.StateSource, null);
        GuarStateComboBox.DisplayMember = "Value";
        GuarStateComboBox.ValueMember = "Key";

        GuarantorRelationComboBox.DataSource = new BindingSource(Dictionaries.RelationSource, null);
        GuarantorRelationComboBox.DisplayMember = "Value";
        GuarantorRelationComboBox.ValueMember = "Key";

        #endregion

        if (SelectedAccount != null || SelectedAccount != "")
        {
            Log.Instance.Debug($"Loading account data for {SelectedAccount}");
            _accountService.AddRecentlyAccessedAccount(SelectedAccount, Program.LoggedInUser.UserName);
            AccountOpenedEvent?.Invoke(this, SelectedAccount);

            AddOnChangeHandlerToInputControls(tabDemographics);
        }

        notesDataGridView.DoubleBuffered(true);

        SetupPaymentTabContextMenu();
    }

    private void UserControl_OnError(object sender, AppErrorEventArgs e)
    {
        switch (e.ErrorLevel)
        {
            case AppErrorEventArgs.ErrorLevelType.Trace:
                Log.Instance.Trace(e.ErrorMessage);
                break;
            case AppErrorEventArgs.ErrorLevelType.Debug:
                Log.Instance.Debug(e.ErrorMessage);
                break;
            case AppErrorEventArgs.ErrorLevelType.Info:
                Log.Instance.Info(e.ErrorMessage);
                break;
            case AppErrorEventArgs.ErrorLevelType.Warning:
                Log.Instance.Warn(e.ErrorMessage);
                break;
            case AppErrorEventArgs.ErrorLevelType.Error:
                Log.Instance.Error(e.ErrorMessage);
                break;
            case AppErrorEventArgs.ErrorLevelType.Fatal:
                Log.Instance.Fatal(e.ErrorMessage);
                break;
            default:
                Log.Instance.Info(e.ErrorMessage);
                break;
        }
    }

    private void DataChanged_EventHandler(object sender, EventArgs e) => RefreshAccountData();

    private void InsChanged_EventHander(object sender, InsuranceUpdatedEventArgs e)
    {
        var idx = _currentAccount.Insurances.FindIndex(i => i.Account == e.UpdatedIns.Account && i.Coverage == e.UpdatedIns.Coverage);
        if (idx >= 0)
        {
            _currentAccount.Insurances[idx] = e.UpdatedIns;
        }
        else
        {
            _currentAccount.Insurances.Add(e.UpdatedIns);
        }
        RefreshAccountData();
    }

    private void SetFormPermissions()
    {
        Log.Instance.Trace("Entering");
        _chargeMaintenance.AllowChargeEntry = !_readOnly && Program.LoggedInUser.CanSubmitCharges;

        Helper.SetControlsAccess(tabPayments.Controls, false);
        if (Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry)
        {
            Helper.SetControlsAccess(tabPayments.Controls, !_readOnly && Program.LoggedInUser.CanAddAdjustments);
        }

        _insPrimaryMaintenanceUC.AllowEditing = false;
        Helper.SetControlsAccess(tabDemographics.Controls, false);
        Helper.SetControlsAccess(tabDiagnosis.Controls, false);
        Helper.SetControlsAccess(demographicsLayoutPanel.Controls, false);
        Helper.SetControlsAccess(tabNotes.Controls, false);
        AddPaymentButton.Visible = false;
        SaveDemographics.Visible = false;
        GuarCopyPatientLink.Visible = false;
        changeClientToolStripMenuItem.Visible = false;
        changeDateOfServiceToolStripMenuItem.Visible = false;
        changeFinancialClassToolStripMenuItem.Visible = false;
        clearHoldStatusToolStripMenuItem.Visible = false;
        ValidateAccountButton.Visible = false;
        GenerateClaimButton.Visible = false;
        if (Program.AppEnvironment.ApplicationParameters.AllowEditing)
        {
            if (Program.LoggedInUser.Access == "ENTER/EDIT" && !_readOnly)
            {
                _insPrimaryMaintenanceUC.AllowEditing = true;
                Helper.SetControlsAccess(tabDemographics.Controls, true);
                Helper.SetControlsAccess(tabDiagnosis.Controls, true);
                Helper.SetControlsAccess(demographicsLayoutPanel.Controls, true);
                Helper.SetControlsAccess(tabNotes.Controls, true);
                Helper.SetControlsAccess(tabPayments.Controls, true);
                AddPaymentButton.Visible = Program.LoggedInUser.CanAddAdjustments;
                SaveDemographics.Visible = true;
                GuarCopyPatientLink.Visible = true;
                changeClientToolStripMenuItem.Visible = true;
                changeDateOfServiceToolStripMenuItem.Visible = true;
                changeFinancialClassToolStripMenuItem.Visible = Program.LoggedInUser.CanModifyAccountFincode;
                clearHoldStatusToolStripMenuItem.Visible = true;
                ValidateAccountButton.Visible = true;
            }
        }
    }

    private void AccountForm_FormClosing(object sender, FormClosingEventArgs e)
    {
        Log.Instance.Debug($"Exiting - {SelectedAccount}");
        _isClosing = true;

        if (_currentAccount != null && !string.IsNullOrEmpty(_currentAccount.AccountNo))
        {
            try
            {
                Log.Instance.Debug($"Clearing lock on {_currentAccount}");
                _accountService.ClearAccountLock(_currentAccount);
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex);
            }
        }
        _isClosing = true;
        e.Cancel = false;
    }

    private async void RefreshButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        await LoadAccountData();
    }

    /// <summary>
    /// Loads account object from database and refreshes the form controls.
    /// </summary>
    private async Task LoadAccountData()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");


        if (_isClosing)
            return;

        Cursor.Current = Cursors.WaitCursor;
        this.SuspendLayout();
        try
        {
            _currentAccount = await _accountService.GetAccountAsync(SelectedAccount);
        }
        catch (AccountLockException alex)
        {
            string message = $"Account is locked by {alex.LockInfo.UpdatedUser} at {alex.LockInfo.LockDateTime} on {alex.LockInfo.UpdatedHost}. Open as read only?";
            if (MessageBox.Show(message, "Account Locked", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                _currentAccount = await _accountService.GetAccountAsync(SelectedAccount, false, false);
                _readOnly = true;
                SetFormPermissions();
            }
            else
            {
                Close();
                return;
            }
        }

        if (_currentAccount.FinCode == Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            tabControl1.TabPages.Remove(tabDemographics);
            tabControl1.TabPages.Remove(tabInsPrimary);
            tabControl1.TabPages.Remove(tabInsSecondary);
            tabControl1.TabPages.Remove(tabInsTertiary);
            tabControl1.TabPages.Remove(tabDiagnosis);
            tabControl1.TabPages.Remove(tabBillingActivity);
            changeClientToolStripMenuItem.Visible = false;
            //changeFinancialClassToolStripMenuItem.Visible = false;
            swapInsurancesToolStripMenuItem.Visible = false;
            clearHoldStatusToolStripMenuItem.Visible = false;
            changeDateOfServiceToolStripMenuItem.Visible = false;
        }

        if (_currentAccount.FinCode == Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            this.Text = $"{_currentAccount.AccountNo} - {_currentAccount.PatFullName}";
        }
        else
        {
            this.Text = $"{_currentAccount.AccountNo} - {_currentAccount.PatLastName}";
        }

        _dxBindingList = new BindingList<PatDiag>(_currentAccount.Pat.Diagnoses);

        if (_currentAccount.Status == AccountStatus.Hold)
            clearHoldStatusToolStripMenuItem.Text = _clearHoldMenuText;
        else
            clearHoldStatusToolStripMenuItem.Text = _setHoldMenuText;

        _chargeMaintenance.CurrentAccount = _currentAccount;
        _insPrimaryMaintenanceUC.CurrentAccount = _currentAccount;
        _insSecondaryMaintenanceUC.CurrentAccount = _currentAccount;
        _insTertiaryMaintenanceUC.CurrentAccount = _currentAccount;

        RefreshAccountData();


        this.ResumeLayout();
        Cursor.Current = Cursors.Default;
    }

    /// <summary>
    /// Updates form controls from Account object
    /// </summary>
    private void RefreshAccountData()
    {
        if(_isClosing)
        {
            return;
        }
        LoadBanner();
        LoadSummaryTab();
        _chargeMaintenance.LoadCharges();
        LoadPayments();
        LoadNotes();

        if (_currentAccount.FinCode != Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            LoadDemographics();
            _insPrimaryMaintenanceUC.LoadInsuranceData();
            _insSecondaryMaintenanceUC.LoadInsuranceData();
            _insTertiaryMaintenanceUC.LoadInsuranceData();
            LoadDx();
            LoadBillingActivity();
        }
        AccountUpdatedEvent?.Invoke(this, _currentAccount);
    }

    private void LoadSummaryTab()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        // note: when adding new items to the summary data list, be sure to enter the items in the order they
        // should appear. In other words, keep the group types together and the individual items in the order
        // they should be displayed.
        #region PopulateSummaryTab
        List<SummaryData> sd = new();

        int col = 1;
        int row = 1;
        //column 1
        sd.Add(new SummaryData("Demographics", "", SummaryData.GroupType.Demographics, row++, col, true));
        sd.Add(new SummaryData("Account", SelectedAccount, SummaryData.GroupType.Demographics, row++, col));
        sd.Add(new SummaryData("EMR Account", _currentAccount.MeditechAccount, SummaryData.GroupType.Demographics, row++, col));
        sd.Add(new SummaryData("Status", _currentAccount.Status, SummaryData.GroupType.Demographics, row++, col));
        sd.Add(new SummaryData("EMPI/MRN", _currentAccount.EMPINumber, SummaryData.GroupType.Demographics, row++, col));
        sd.Add(new SummaryData("SSN", _currentAccount.SocSecNo.FormatSSN(), SummaryData.GroupType.Demographics, row++, col));
        sd.Add(new SummaryData("Client", _currentAccount.ClientName, SummaryData.GroupType.Demographics, row++, col));
        if (_currentAccount.FinCode != Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            sd.Add(new SummaryData("Ordering Provider", _currentAccount.Pat.Physician?.FullName ?? _currentAccount.Pat.ProviderId, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("DOB/Sex", _currentAccount.DOBSex, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Address", _currentAccount.Pat.AddressLine, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Phone", _currentAccount.Pat.PrimaryPhone.FormatPhone(), SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Email", _currentAccount.Pat.EmailAddress, SummaryData.GroupType.Demographics, row++, col));


            sd.Add(new SummaryData("Diagnoses", "", SummaryData.GroupType.Diagnoses, row++, col, true));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx1, _currentAccount.Pat.Dx1Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx2, _currentAccount.Pat.Dx2Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx3, _currentAccount.Pat.Dx3Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx4, _currentAccount.Pat.Dx4Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx5, _currentAccount.Pat.Dx5Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx6, _currentAccount.Pat.Dx6Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx7, _currentAccount.Pat.Dx7Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx8, _currentAccount.Pat.Dx8Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(_currentAccount.Pat.Dx9, _currentAccount.Pat.Dx9Desc, SummaryData.GroupType.Diagnoses, row++, col));
        }
        //column 2
        col = 2;
        row = 1;
        sd.Add(new SummaryData("Financial", "", SummaryData.GroupType.Financial, row++, col, true));
        sd.Add(new SummaryData("Financial Class", _currentAccount.FinCode, SummaryData.GroupType.Financial, row++, col));
        sd.Add(new SummaryData("Date of Service", _currentAccount.TransactionDate.ToShortDateString(), SummaryData.GroupType.Financial, row++, col));
        sd.Add(new SummaryData("Total Charges", _currentAccount.TotalCharges.ToString("c"), SummaryData.GroupType.Financial, row++, col));
        sd.Add(new SummaryData("Total Payments", (_currentAccount.TotalPayments + _currentAccount.TotalContractual + _currentAccount.TotalWriteOff).ToString("c"),
            SummaryData.GroupType.Financial, row++, col));
        sd.Add(new SummaryData("3rd Party/Patient Balance", _currentAccount.ClaimBalance.ToString("c"), SummaryData.GroupType.Financial, row++, col));
        foreach (var (client, balance) in _currentAccount.ClientBalance)
        {
            sd.Add(new SummaryData($"Client Balance {client}", balance.ToString("c"), SummaryData.GroupType.Financial, row++, col));
        }
        sd.Add(new SummaryData("Account Balance", _currentAccount.Balance.ToString("c"), SummaryData.GroupType.Financial, row++, col));

        if (_currentAccount.FinCode != Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            foreach (Ins ins in _currentAccount.Insurances)
            {
                if (ins.Coverage == "A")
                {
                    sd.Add(new SummaryData("Primary Insurance", "", SummaryData.GroupType.Insurance, row++, col, true));
                    sd.Add(new SummaryData("Holder", ins.HolderFullName, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Insurance", ins.PlanName, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Policy", ins.PolicyNumber, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Group #", ins.GroupNumber, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Group", ins.GroupName, SummaryData.GroupType.Insurance, row++, col));
                }
                if (ins.Coverage == "B")
                {
                    sd.Add(new SummaryData("Secondary Insurance", "", SummaryData.GroupType.Insurance, row++, col, true));
                    sd.Add(new SummaryData("Holder", ins.HolderFullName, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Insurance", ins.PlanName, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Policy", ins.PolicyNumber, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Group #", ins.GroupNumber, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Group", ins.GroupName, SummaryData.GroupType.Insurance, row++, col));
                }
                if (ins.Coverage == "C")
                {
                    sd.Add(new SummaryData("Tertiary Insurance", "", SummaryData.GroupType.Insurance, row++, col, true));
                    sd.Add(new SummaryData("Holder", ins.HolderFullName, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Insurance", ins.PlanName, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Policy", ins.PolicyNumber, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Group #", ins.GroupNumber, SummaryData.GroupType.Insurance, row++, col));
                    sd.Add(new SummaryData("Group", ins.GroupName, SummaryData.GroupType.Insurance, row++, col));
                }
            }
        }

        summaryTable.Controls.Clear();
        summaryTable.RowStyles.Clear();
        summaryTable.ColumnStyles.Clear();

        summaryTable.ColumnCount = 4;
        summaryTable.RowCount = sd.Max(x => x.RowPos);

        summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
        summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));

        foreach (SummaryData sdi in sd)
        {
            summaryTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            summaryTable.Controls.Add(sdi.DisplayLabel, sdi.ColPos == 1 ? 0 : 2, sdi.RowPos - 1);
            if (sdi.IsHeader)
            {
                summaryTable.SetColumnSpan(summaryTable.GetControlFromPosition(sdi.ColPos == 1 ? 0 : 2, sdi.RowPos - 1), 2);
            }
            else
            {
                summaryTable.Controls.Add(sdi.ValueLabel, sdi.ColPos == 1 ? 1 : 3, sdi.RowPos - 1);
            }

        }
        #endregion
    }

    #endregion

    #region DemographicTab

    private void LoadBanner()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        BannerNameTextBox.Text = _currentAccount.PatFullName;

        if(_currentAccount.FinCode == Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            BannerDOBSexLabel.Visible = false;
            BannerDobTextBox.Visible = false;
            BannerSexTextBox.Visible = false;
        }
        BannerDobTextBox.Text = _currentAccount.BirthDate.GetValueOrDefault().ToShortDateString();
        BannerSexTextBox.Text = _currentAccount.Sex;
        BannerAccountTextBox.Text = SelectedAccount;
        BannerMRNTextBox.Text = _currentAccount.EMPINumber;
        BannerClientTextBox.Text = _currentAccount.ClientName;
        BannerFinClassTextBox.Text = _currentAccount.FinCode;
        BannerBillStatusTextBox.Text = _currentAccount.Status;
        BannerProviderTextBox.Text = _currentAccount.Pat.Physician?.FullName ?? "";
        bannerDateOfServiceTextBox.Text = _currentAccount.TransactionDate.ToShortDateString();
        if (_currentAccount.AccountAlert != null)
        {
            bannerAlertLabel.Text = _currentAccount.AccountAlert.Alert ? _notesAlertText : "";
            bannerAlertLabel.Visible = !string.IsNullOrEmpty(bannerAlertLabel.Text);
        }
        else
        {
            bannerAlertLabel.Visible = false;
            bannerAlertLabel.Text = "";
        }

        if (_currentAccount.ReadyToBill)
        {
            bannerAlertLabel.Visible = true;
            bannerAlertLabel.Text += "  Account is flagged ready to bill, or has been billed. Any changes can affect the claim.";
            bannerAlertLabel.BackColor = Color.Red;
            bannerAlertLabel.ForeColor = Color.White;
        }

        TotalChargesLabel.Text = _currentAccount.TotalCharges.ToString("c");
        TotalPmtAdjLabel.Text = (_currentAccount.TotalContractual + _currentAccount.TotalPayments + _currentAccount.TotalWriteOff).ToString("c");
        BalanceLabel.Text = _currentAccount.Balance.ToString("c");
        ThirdPartyBalLabel.Text = _currentAccount.ClaimBalance.ToString("c");
        ClientBalLabel.Text = _currentAccount.ClientBalance.Sum(x => x.balance).ToString("c");
    }

    private void LoadDemographics()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        DemoStatusMessagesTextBox.Text = String.Empty;

        //PatientFullNameLabel.Text = currentAccount.PatFullName;
        LastNameTextBox.Text = _currentAccount.PatLastName;
        LastNameTextBox.BackColor = Color.White;
        FirstNameTextBox.Text = _currentAccount.PatFirstName;
        FirstNameTextBox.BackColor = Color.White;
        MiddleNameTextBox.Text = _currentAccount.PatMiddleName;
        MiddleNameTextBox.BackColor = Color.White;
        SuffixTextBox.Text = _currentAccount.PatNameSuffix;
        SuffixTextBox.BackColor = Color.White;
        Address1TextBox.Text = _currentAccount.Pat.Address1;
        Address1TextBox.BackColor = Color.White;
        Address2TextBox.Text = _currentAccount.Pat.Address2;
        Address2TextBox.BackColor = Color.White;
        CityTextBox.Text = _currentAccount.Pat.City;
        CityTextBox.BackColor = Color.White;
        StateComboBox.SelectedValue = _currentAccount.Pat.State ?? "";
        StateComboBox.BackColor = Color.White;
        ZipcodeTextBox.Text = _currentAccount.Pat.ZipCode;
        ZipcodeTextBox.BackColor = Color.White;
        PhoneTextBox.Text = _currentAccount.Pat.PrimaryPhone;
        PhoneTextBox.BackColor = Color.White;
        SocSecNoTextBox.Text = _currentAccount.SocSecNo;
        SocSecNoTextBox.BackColor = Color.White;
        EmailAddressTextBox.Text = _currentAccount.Pat.EmailAddress;
        EmailAddressTextBox.BackColor = Color.White;
        DateOfBirthTextBox.Text = _currentAccount.BirthDate == null ? string.Empty : _currentAccount.BirthDate.Value.ToString("MM/dd/yyyy");
        DateOfBirthTextBox.BackColor = Color.White;
        SexComboBox.SelectedValue = _currentAccount.Sex ?? string.Empty;
        SexComboBox.BackColor = Color.White;
        MaritalStatusComboBox.SelectedValue = !string.IsNullOrEmpty(_currentAccount.Pat.MaritalStatus) ? _currentAccount.Pat.MaritalStatus : "U";
        MaritalStatusComboBox.BackColor = Color.White;

        orderingPhyTextBox.Text = _currentAccount.Pat.Physician?.ToString();
        orderingPhyTextBox.Tag = _currentAccount.Pat.ProviderId;

        GuarantorLastNameTextBox.Text = _currentAccount.Pat.GuarantorLastName;
        GuarFirstNameTextBox.Text = _currentAccount.Pat.GuarantorFirstName;
        GuarMiddleNameTextBox.Text = _currentAccount.Pat.GuarantorMiddleName;
        GuarSuffixTextBox.Text = _currentAccount.Pat.GuarantorNameSuffix;
        GuarantorAddressTextBox.Text = _currentAccount.Pat.GuarantorAddress;
        GuarCityTextBox.Text = _currentAccount.Pat.GuarantorCity;
        GuarStateComboBox.SelectedValue = _currentAccount.Pat.GuarantorState ?? "";
        GuarZipTextBox.Text = _currentAccount.Pat.GuarantorZipCode;
        GuarantorPhoneTextBox.Text = _currentAccount.Pat.GuarantorPrimaryPhone;
        GuarantorRelationComboBox.SelectedValue = _currentAccount.Pat.GuarRelationToPatient ?? "";

        ResetControls(tabDemographics.Controls.OfType<Control>().ToArray());
        ResetControls(demographicsLayoutPanel.Controls.OfType<Control>().ToArray());
    }

    private void SaveDemographics_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        _currentAccount.PatFullName = $"{LastNameTextBox.Text} {SuffixTextBox.Text},{FirstNameTextBox.Text} {MiddleNameTextBox.Text}";
        _currentAccount.PatLastName = LastNameTextBox.Text;
        _currentAccount.PatFirstName = FirstNameTextBox.Text;
        _currentAccount.PatMiddleName = MiddleNameTextBox.Text;
        _currentAccount.PatNameSuffix = SuffixTextBox.Text;
        _currentAccount.SocSecNo = SocSecNoTextBox.Text;
        _currentAccount.BirthDate = DateTimeExtension.ValidateDateOrNull(DateOfBirthTextBox.Text);
        _currentAccount.Sex = SexComboBox.SelectedValue.ToString();

        _currentAccount.Pat.Address1 = Address1TextBox.Text;
        _currentAccount.Pat.Address2 = Address2TextBox.Text;
        _currentAccount.Pat.EmailAddress = EmailAddressTextBox.Text;
        _currentAccount.Pat.MaritalStatus = MaritalStatusComboBox.SelectedValue.ToString();
        _currentAccount.Pat.PrimaryPhone = PhoneTextBox.Text;
        _currentAccount.Pat.City = CityTextBox.Text;
        _currentAccount.Pat.State = StateComboBox.SelectedValue.ToString();
        _currentAccount.Pat.ZipCode = ZipcodeTextBox.Text;
        _currentAccount.Pat.CityStateZip = $"{CityTextBox.Text}, {StateComboBox.SelectedValue} {ZipcodeTextBox.Text}";
        _currentAccount.Pat.ProviderId = orderingPhyTextBox.Tag?.ToString();

        _currentAccount.Pat.GuarantorFullName = $"{GuarantorLastNameTextBox.Text} {GuarSuffixTextBox.Text},{GuarFirstNameTextBox.Text} {GuarMiddleNameTextBox.Text}";
        _currentAccount.Pat.GuarantorLastName = GuarantorLastNameTextBox.Text;
        _currentAccount.Pat.GuarantorFirstName = GuarFirstNameTextBox.Text;
        _currentAccount.Pat.GuarantorMiddleName = GuarMiddleNameTextBox.Text;
        _currentAccount.Pat.GuarantorNameSuffix = GuarSuffixTextBox.Text;
        _currentAccount.Pat.GuarantorAddress = GuarantorAddressTextBox.Text;
        _currentAccount.Pat.GuarantorCity = GuarCityTextBox.Text;
        _currentAccount.Pat.GuarantorPrimaryPhone = GuarantorPhoneTextBox.Text;
        _currentAccount.Pat.GuarantorState = GuarStateComboBox.SelectedValue.ToString();
        _currentAccount.Pat.GuarantorZipCode = GuarZipTextBox.Text;
        _currentAccount.Pat.GuarantorCityState = $"{GuarCityTextBox.Text}, {GuarStateComboBox.SelectedValue} {GuarZipTextBox.Text}";
        _currentAccount.Pat.GuarRelationToPatient = GuarantorRelationComboBox.SelectedValue.ToString();

        _currentAccount = _accountService.UpdateAccountDemographics(_currentAccount);

        var controls = tabDemographics.Controls; //tabDemographics.Controls;

        foreach (Control control in controls)
        {
            //set background back to white to indicate change has been saved to database
            control.BackColor = Color.White;
        }

        //await this.LoadAccountData();
        RefreshAccountData();

    }

    #endregion

    #region PaymentsTab

    private void LoadPayments()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        PaymentsDataGrid.ContextMenuStrip = _chkTabContextMenu;

        TotalPaymentTextBox.Text = _currentAccount.TotalPayments.ToString("c");
        TotalContractualTextBox.Text = _currentAccount.TotalContractual.ToString("c");
        TotalWriteOffTextBox.Text = _currentAccount.TotalWriteOff.ToString("c");
        TotalPmtAllTextBox.Text = (_currentAccount.TotalPayments + _currentAccount.TotalContractual + _currentAccount.TotalWriteOff).ToString("c");

        if ((_currentAccount.Fin.FinClass == "C" && _currentAccount.FinCode != "CLIENT") ||
            _currentAccount.Status == AccountStatus.Closed)
        {
            AddPaymentButton.Enabled = false;
            Label addPaymentStatusLabel = new()
            {
                AutoSize = true,
                MaximumSize = new Size(300, 300),
                Text = "Cannot add payment to this account.",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                Location = new Point(tabPayments.Right - 310, AddPaymentButton.Bottom + 10)
            };
            tabPayments.Controls.Add(addPaymentStatusLabel);
        }

        _currentAccount.Payments = _currentAccount.Payments.OrderByDescending(x => x.PaymentNo).ToList();
        PaymentsDataGrid.AutoGenerateColumns = true;
        PaymentsDataGrid.DataSource = _currentAccount.Payments.ToList();
        PaymentsDataGrid.AutoGenerateColumns = false;
        PaymentsDataGrid.SetColumnsVisibility(false);
        int z = 0;

        PaymentsDataGrid.Columns[nameof(Chk.Batch)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.DateReceived)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.ChkDate)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.CheckNo)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.Invoice)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.Source)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.Status)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.PaidAmount)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.ContractualAmount)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.WriteOffAmount)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.WriteOffCode)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.WriteOffDate)].SetVisibilityOrder(true, z++);
        PaymentsDataGrid.Columns[nameof(Chk.Comment)].SetVisibilityOrder(true, z++);

        PaymentsDataGrid.Columns[nameof(Chk.PaidAmount)].DefaultCellStyle.Format = "N2";
        PaymentsDataGrid.Columns[nameof(Chk.PaidAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        PaymentsDataGrid.Columns[nameof(Chk.ContractualAmount)].DefaultCellStyle.Format = "N2";
        PaymentsDataGrid.Columns[nameof(Chk.ContractualAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        PaymentsDataGrid.Columns[nameof(Chk.WriteOffAmount)].DefaultCellStyle.Format = "N2";
        PaymentsDataGrid.Columns[nameof(Chk.WriteOffAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        PaymentsDataGrid.Columns[nameof(Chk.Comment)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        PaymentsDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        PaymentsDataGrid.BackgroundColor = Color.AntiqueWhite;
    }

    private void PaymentsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        int selectedRows = PaymentsDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {

            DataGridViewRow row = PaymentsDataGrid.SelectedRows[0];
            var chk = _currentAccount.Payments.Where(p => p.PaymentNo == Convert.ToInt32(row.Cells[nameof(Chk.PaymentNo)].Value.ToString())).First();

            DisplayPOCOForm<Chk> frm = new(chk)
            {
                Title = "Payment Details"
            };
            frm.Show();
        }
    }

    private void AddPaymentButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        PaymentAdjustmentEntryForm form = new(ref _currentAccount);

        if (_currentAccount.SentToCollections)
        {
            if (MessageBox.Show($"Account {_currentAccount.AccountNo} has been sent to collections. Follow process to notify collection agency of payment.\n Continue with add payment?",
                "Account Sent to Collections", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                return;
        }

        if (form.ShowDialog() == DialogResult.OK)
        {
            //post record to account
            var chk = form.chk;

            if (_currentAccount.Status == AccountStatus.PaidOut)
            {
                _accountService.UpdateStatus(_currentAccount.AccountNo, AccountStatus.New);
                _currentAccount.Status = AccountStatus.New;
            }

            chk.AccountNo = _currentAccount.AccountNo;
            chk.FinCode = _currentAccount.FinCode;
            try
            {
                _currentAccount.Payments.Add(_accountService.AddPayment(chk));
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex);
                MessageBox.Show($"Error adding payment. See log for details.");
            }
            RefreshAccountData();
        }
    }

    private void SetupPaymentTabContextMenu()
    {
        _chkTabContextMenu = new ContextMenuStrip();
        var reversePaymentMenuItem = new ToolStripMenuItem("Reverse Payment/Adj");
        reversePaymentMenuItem.Click += ReversePaymentMenuItem_Click;
        _chkTabContextMenu.Items.Add(reversePaymentMenuItem);
    }

    private void ReversePaymentMenuItem_Click(object sender, EventArgs e)
    {
        if (PaymentsDataGrid.SelectedRows.Count > 0)
        {
            var result = InputBox.Show("Reason for reversal", "Reverse Payment/Adjustment", true);
            if(result.ReturnCode == DialogResult.Cancel)
            {
                return;
            }

            var selectedRow = PaymentsDataGrid.SelectedRows[0];
            var paymentNo = Convert.ToInt32(selectedRow.Cells[nameof(Chk.PaymentNo)].Value);
            var payment = _currentAccount.Payments.FirstOrDefault(p => p.PaymentNo == paymentNo);
            if (payment != null)
            {
                try
                {
                    var updatedPmt = _accountService.ReversePayment(payment, result.Text);
                    _currentAccount.Payments.Add(updatedPmt);
                    RefreshAccountData();
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex, "Error reversing payment.");
                    MessageBox.Show("Unable to reverse payment. Contact your administrator.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }

    #endregion

    #region DiagnosisTab

    bool dxLoadingMode = false;

    private void LoadDx()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        DiagnosisDataGrid.DataSource = new BindingSource(_dxBindingList, null);
        dxLoadingMode = true;
        _dxPointerBindingSource.DataSource = null;
        dxPointerGrid2.DataSource = null;

        int cnt = _dxBindingList.Count;
        string[] ptrStrings = new string[cnt + 1];

        ptrStrings[0] = "";

        for (int z = 1; z < cnt + 1; z++)
        {
            ptrStrings[z] = z.ToString();
        }

        _dxPointers.Rows.Clear();
        _dxPointers.Columns.Clear();

        _dxPointers.Columns.Add(new DataColumn()
        {
            DataType = System.Type.GetType("System.String"),
            Caption = "CDM",
            ColumnName = "CDM",
        });

        _dxPointers.Columns.Add(new DataColumn()
        {
            DataType = System.Type.GetType("System.String"),
            Caption = "CPT4",
            ColumnName = "CPT4",
        });

        _dxPointers.Columns.Add(new DataColumn()
        {
            DataType = System.Type.GetType("System.String"),
            Caption = "Description",
            ColumnName = "Description",
        });

        _dxPointers.Columns.Add(new DataColumn()
        {
            DataType = System.Type.GetType("System.String"),
            Caption = "CPT Description",
            ColumnName = "CPTDescription",
        });

        dxPointerGrid2.Columns.Clear();
        dxPointerGrid2.Rows.Clear();

        dxPointerGrid2.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "CDM",
            DataPropertyName = "CDM",
            HeaderText = "CDM",
            ReadOnly = true
        });

        dxPointerGrid2.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "CPT4",
            DataPropertyName = "CPT4",
            HeaderText = "CPT4",
            ReadOnly = true
        });

        dxPointerGrid2.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "Description",
            DataPropertyName = "Description",
            HeaderText = "Description",
            ReadOnly = true,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        dxPointerGrid2.Columns.Add(new DataGridViewTextBoxColumn()
        {
            Name = "CPTDescription",
            DataPropertyName = "CPTDescription",
            HeaderText = "CPT Description",
            ReadOnly = true,
            AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill
        });

        for (int i = 1; i < cnt + 1; i++)
        {
            _dxPointers.Columns.Add(new DataColumn()
            {
                DataType = System.Type.GetType("System.String"),
                Caption = "Pointer",
                ColumnName = ptrStrings[i]
            });

            dxPointerGrid2.Columns.Add(new DataGridViewComboBoxColumn()
            {
                Name = ptrStrings[i],
                DataPropertyName = ptrStrings[i],
                DataSource = _dxBindingList,
                ValueMember = nameof(PatDiag.Code),
                DisplayMember = nameof(PatDiag.Code),
                HeaderText = ptrStrings[i],
                FlatStyle = FlatStyle.Flat,
                MinimumWidth = 100
            });
        }

        _dxPointerBindingSource.DataSource = _dxPointers;

        dxPointerGrid2.DataSource = _dxPointerBindingSource;

        //load charges and pointers to grid
        foreach (var chrg in _currentAccount.ChrgDiagnosisPointers)
        {
            DataRow row = _dxPointers.NewRow();
            row["CDM"] = chrg.CdmCode;
            row["CPT4"] = chrg.CptCode;
            row["Description"] = chrg.CdmDescription;
            row["CPTDescription"] = chrg.CptDescription;

            string[] ptrs = chrg.DiagnosisPointer.Split(':');
            if (ptrs.Length > cnt)
            {

            }
            for (int pi = 0; pi < cnt && pi < ptrs.Length; pi++)
            {
                if (ptrs[pi] == null || ptrs[pi] == "")
                    continue;
                int iPtr = Convert.ToInt32(ptrs[pi]);
                if (iPtr > cnt)
                    continue;

                row[ptrStrings[pi + 1]] = _dxBindingList.Where(x => x.No == iPtr).First().Code;
            }

            _dxPointers.Rows.Add(row);
        }
        dxPointerGrid2.AutoResizeColumns();
        dxLoadingMode = false;
    }

    private void dxPointerGrid2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
    {

    }

    private void dxPointerGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        //You can check for e.ColumnIndex to limit this to your specific column
        if (e.ColumnIndex > 2)
        {
            if (this.dxPointerGrid2.EditingControl is DataGridViewComboBoxEditingControl editingControl)
                editingControl.DroppedDown = true;
        }
    }

    private void dxPointerGrid2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (!dxLoadingMode)
        {
            //loop through columns
            int cnt = _dxBindingList.Count;

            var dxSelected = new List<string>();

            for (int i = 0; i < cnt; i++)
            {
                var val = dxPointerGrid2[(i + 1).ToString(), e.RowIndex].Value.ToString();
                if (!string.IsNullOrEmpty(val))
                    dxSelected.Add(val);
            }

            var dup = dxSelected.GroupBy(x => x).Where(c => c.Count() > 1)
                .Select(x => new { dx = x.Key, cnt = x.Count() }).ToList();

            if (dup.Count > 0)
            {
                var dupDx = dup.FirstOrDefault();
                for (int i = 3; i < cnt + 3; i++)
                {
                    if (dxPointerGrid2[i, e.RowIndex].Value.ToString() == dupDx.dx)
                        dxPointerGrid2[i, e.RowIndex].Style.BackColor = Color.Red;
                }
            }
            else
            {
                string newPointer = "";

                for (int i = 3; i < cnt + 4; i++)
                {
                    dxPointerGrid2[i, e.RowIndex].Style.BackColor = Color.White;

                    var dxValue = dxPointerGrid2[i, e.RowIndex].Value.ToString();
                    var dxRecord = _dxBindingList.Where(x => x.Code == dxValue).FirstOrDefault();
                    if (dxRecord != null && !string.IsNullOrEmpty(dxValue))
                    {
                        newPointer += $"{dxRecord.No}:";
                    }
                }
                //update pointers
                var cpt = dxPointerGrid2["CPT4", e.RowIndex].Value.ToString();
                var cdm = dxPointerGrid2["CDM", e.RowIndex].Value.ToString();

                var ptr = _currentAccount.ChrgDiagnosisPointers.Where(d => d.CdmCode == cdm && d.CptCode == cpt).First();
                if (ptr == null)
                {
                    ptr = new ChrgDiagnosisPointer
                    {
                        CdmCode = cdm,
                        CptCode = cpt,
                        AccountNo = _currentAccount.AccountNo
                    };
                }

                ptr.DiagnosisPointer = newPointer;

                //var updatedChrg = _currentAccount.Charges.Where(c => c.IsCredited == false && c.ChrgDetails.Any(cd => cd.Cpt4 == cpt)).ToList();
                //updatedChrg.ForEach(c => c.ChrgDetails.ForEach((cd) =>
                //{
                //    if (cd.DiagnosisPointer == null)
                //    {
                //        cd.DiagnosisPointer = new ChrgDiagnosisPointer
                //        {
                //            ChrgDetailUri = cd.Id
                //        };
                //    }
                //    cd.DiagnosisPointer.DiagnosisPointer = newPointer;
                //}));

                _accountService.UpdateDiagnosisPointer(ptr);

            }
        }
    }

    private void DxSearchButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (!string.IsNullOrEmpty(txtSearchDx.Text))
        {
            var dictRecords = _dictionaryService.GetDiagnosisCodes(txtSearchDx.Text, _currentAccount.TransactionDate);

            DxSearchDataGrid.DataSource = dictRecords;
            DxSearchDataGrid.Columns[nameof(DictDx.UpdatedDate)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.UpdatedUser)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.UpdatedApp)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.UpdatedHost)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.IsDeleted)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.AmaYear)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.Id)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.Version)].Visible = false;
            DxSearchDataGrid.Columns[nameof(DictDx.rowguid)].Visible = false;
        }
    }

    private void DxSearchDataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        DxSearchDataGrid.Columns[nameof(DictDx.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        DxSearchDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        DxSearchDataGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void DiagnosisDataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        DiagnosisDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        DiagnosisDataGrid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
    }

    private void DxSearchDataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        int selectedRows = DxSearchDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {
            //add selected diagnosis to account dx grid

            string selectedCode = DxSearchDataGrid.SelectedRows[0].Cells[nameof(DictDx.DxCode)].Value.ToString();
            string selectedDesc = DxSearchDataGrid.SelectedRows[0].Cells[nameof(DictDx.Description)].Value.ToString();

            if (_dxBindingList.FirstOrDefault(n => n.Code == selectedCode) != null)
            {
                //this code already exists in the list
                MessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                return;
            }

            int maxNo = 0;
            if (_dxBindingList.Count > 0)
                maxNo = _dxBindingList.Max<PatDiag>(n => n.No);

            if (maxNo >= 9)
            {
                MessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
            else
            {
                _dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = selectedCode, Description = selectedDesc });
                DiagnosisDataGrid.BackgroundColor = Color.Orange;

                SaveDiagnoses();
            }
        }
    }

    private void SearchDxTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (e.KeyChar == (char)13)
        {
            Log.Instance.Debug("Enter key pressed");
            DxSearchButton_Click(sender, e);
        }
    }

    private void DxQuickAddTextBox_KeyPress(object sender, KeyPressEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (e.KeyChar == (char)13)
        {
            Log.Instance.Debug("Enter key pressed");
            if (DxQuickAddTextBox.Text != "")
            {
                //check to see if the text entered is a valid DX code - if so, add the code and description to the selected grid

                if (_dxBindingList.FirstOrDefault<PatDiag>(n => n.Code == DxQuickAddTextBox.Text) != null)
                {
                    //this code already exists in the list
                    MessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    DxQuickAddTextBox.Text = "";
                    return;
                }

                var record = _dictionaryService.GetDiagnosis(DxQuickAddTextBox.Text, _currentAccount.TransactionDate);

                if (record != null)
                {
                    //this is a valid entry
                    int maxNo = 0;
                    if (_dxBindingList.Count > 0)
                        maxNo = _dxBindingList.Max<PatDiag>(n => n.No);

                    if (maxNo >= 9)
                    {
                        MessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        DxQuickAddTextBox.Text = "";
                        return;
                    }
                    _dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = record.DxCode, Description = record.Description });
                    DiagnosisDataGrid.BackgroundColor = Color.Orange;
                    DxQuickAddTextBox.Text = "";

                    SaveDiagnoses();
                }
                else
                {
                    //not valid - clear box and do nothing
                    DxQuickAddTextBox.Text = "";
                }
            }
        }
    }

    private void DxDeleteButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        int selectedRows = DiagnosisDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {
            //add selected diagnosis to account dx grid

            string selectedCode = DiagnosisDataGrid.SelectedRows[0].Cells[nameof(PatDiag.Code)].Value.ToString();
            string selectedNo = DiagnosisDataGrid.SelectedRows[0].Cells[nameof(PatDiag.No)].Value.ToString();

            var record = _dxBindingList.IndexOf(_dxBindingList.First<PatDiag>(n => n.Code == selectedCode));

            _dxBindingList.RemoveAt(record);
            DiagnosisDataGrid.BackgroundColor = Color.Orange;
            //loop through and renumber
            for (int i = 0; i < _dxBindingList.Count; i++)
            {
                _dxBindingList[i].No = i + 1;
            }

            SaveDiagnoses();
        }
    }

    private void DiagnosisDataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        e.Cancel = true;
        DxDeleteButton_Click(sender, e);
    }

    private void SaveDxButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        SaveDiagnoses();
    }

    private void SaveDiagnoses()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        _currentAccount.Pat.Diagnoses = _dxBindingList.ToList<PatDiag>();

        _currentAccount = _accountService.UpdateDiagnoses(_currentAccount);
        DiagnosisDataGrid.BackgroundColor = Color.White;
        RefreshAccountData();
    }

    private void DiagnosisDataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {

    }

    #endregion

    #region NotesTab

    private void LoadNotes()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        notesDataGridView.SuspendLayout();
        notesDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
        notesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        notesDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
        notesDataGridView.DataSource = _currentAccount.Notes;
        notesDataGridView.BackgroundColor = Color.AntiqueWhite;

        notesDataGridView.SetColumnsVisibility(false);
        notesDataGridView.SetColumnVisibility(nameof(AccountNote.UpdatedDate), true);
        notesDataGridView.SetColumnVisibility(nameof(AccountNote.UpdatedUser), true);
        notesDataGridView.SetColumnVisibility(nameof(AccountNote.Comment), true);

        notesDataGridView.Columns[nameof(AccountNote.Comment)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        notesDataGridView.Columns[nameof(AccountNote.UpdatedDate)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        notesDataGridView.Columns[nameof(AccountNote.UpdatedUser)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        notesDataGridView.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
        notesDataGridView.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCells);

        int z = 0;
        notesDataGridView.Columns[nameof(AccountNote.Comment)].DisplayIndex = z++;
        notesDataGridView.Columns[nameof(Account.UpdatedDate)].DisplayIndex = z++;
        notesDataGridView.Columns[nameof(Account.UpdatedUser)].DisplayIndex = z++;

        if (_currentAccount.AccountAlert != null)
            noteAlertCheckBox.Checked = _currentAccount.AccountAlert.Alert;

        notesDataGridView.ResumeLayout();
    }

    private void AddNoteButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        InputBoxResult prompt = InputBox.Show("Enter note:", "New Note", true);

        if (prompt.ReturnCode == DialogResult.OK)
        {
            _accountService.AddNote(_currentAccount.AccountNo, prompt.Text);
            //reload notes to pick up changes
            _currentAccount.Notes = _accountService.GetNotes(_currentAccount.AccountNo);
            LoadNotes();
        }
    }

    #endregion

    #region BillingActivityTab

    private void LoadBillingActivity()
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        _billingTabLoading = true;

        BillActivityDataGrid.DataSource = _currentAccount.BillingActivities.ToList();
        BillActivityDataGrid.Columns[nameof(BillingActivity.rowguid)].Visible = false;
        BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedDate)].Visible = false;
        BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedHost)].Visible = false;
        BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedApp)].Visible = false;
        BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedUser)].Visible = false;
        BillActivityDataGrid.Columns[nameof(BillingActivity.Text)].Visible = false;

        BillActivityDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

        ValidationResultsTextBox.Text = _currentAccount.AccountValidationStatus.ValidationText;
        LastValidatedLabel.Text = _currentAccount.AccountValidationStatus.UpdatedDate.ToString("G");

        statementFlagComboBox.SelectedItem = _currentAccount.Pat.StatementFlag;
        // do not allow changes if statement flag is 'X' (sent to A/R Management Service)
        if (_currentAccount.Pat.StatementFlag == "X")
        {
            statementFlagComboBox.Enabled = false;
        }

        firstStmtDateTextBox.Text = _currentAccount.Pat.FirstStatementDate.ToString();
        lastStmtDateTextBox.Text = _currentAccount.Pat.LastStatementDate.ToString();
        minPmtTextBox.Text = _currentAccount.Pat.MinimumPaymentAmount.ToString();

        readyToBillCheckbox.Checked = _currentAccount.ReadyToBill;

        statementHistoryDataGrid.DataSource = _accountService.GetPatientStatements(_currentAccount.AccountNo);

        statementHistoryDataGrid.SetColumnsVisibility(false);

        statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.DateSent)].Visible = true;
        statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.Mailer)].Visible = true;
        statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.MailerCount)].Visible = true;
        statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.ProcessedDate)].Visible = true;
        statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.StatementNumber)].Visible = true;

        _billingTabLoading = false;
    }

    #endregion

    private void ChangeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        var result = InputDialogs.SelectDateOfService((DateTime)_currentAccount.TransactionDate);

        try
        {
            if (result.newDate != DateTime.MinValue)
            {
                _accountService.ChangeDateOfService(_currentAccount, result.newDate, result.reason);
            }
            else
            {
                MessageBox.Show("Date selected is not valid. Date has not been changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        catch (ArgumentNullException anex)
        {
            Log.Instance.Error(anex, $"Change date of service parameter {anex.ParamName} must contain a value.");
            MessageBox.Show(this, $"{anex.ParamName} must contain a value. Date of service was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, $"Error changing date of service.");
            MessageBox.Show(this, $"Error changing date of service. Date of service was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        RefreshAccountData();

        Log.Instance.Trace($"Exiting");
    }

    private void ChangeFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        string newFinCode = InputDialogs.SelectFinancialCode(_currentAccount.FinCode);
        if (!string.IsNullOrEmpty(newFinCode))
        {
            try
            {
                _currentAccount = _accountService.ChangeFinancialClass(_currentAccount, newFinCode);
            }
            catch (ArgumentException anex)
            {
                Log.Instance.Error(anex, $"Financial code {anex.ParamName} is not valid.");
                MessageBox.Show(this, $"{anex.ParamName} is not a valid financial code. Financial code was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex, $"Error changing financial class.");
                MessageBox.Show(this, $"Error changing financial class. Financial code was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            RefreshAccountData();
        }
    }

    private void ChangeClientToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        ClientLookupForm clientLookupForm = new()
        {
            Datasource = DataCache.Instance.GetClients()
        };

        if (clientLookupForm.ShowDialog() == DialogResult.OK)
        {
            string newClient = clientLookupForm.SelectedValue;

            try
            {
                if (_accountService.ChangeClient(_currentAccount, newClient))
                {
                    RefreshAccountData();
                }
                else
                {
                    MessageBox.Show("Error during update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (ApplicationException apex)
            {
                MessageBox.Show(apex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Instance.Error(apex);
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating client. Client not updated.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Instance.Error(ex);
                return;
            }
        }
    }

    private void AddOnChangeHandlerToInputControls(Control ctrl)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        foreach (Control subctrl in ctrl.Controls)
        {
            if (subctrl is TextBox box)
                box.TextChanged +=
                    new EventHandler(InputControls_OnChange);
            else if (subctrl is CheckBox checkBox)
                checkBox.CheckedChanged +=
                    new EventHandler(InputControls_OnChange);
            else if (subctrl is RadioButton button)
                button.CheckedChanged +=
                    new EventHandler(InputControls_OnChange);
            else if (subctrl is ListBox listBox)
                listBox.SelectedIndexChanged +=
                    new EventHandler(InputControls_OnChange);
            else if (subctrl is ComboBox comboBox)
                comboBox.SelectedIndexChanged +=
                    new EventHandler(InputControls_OnChange);
            else if (subctrl is MaskedTextBox maskedTextBox)
                maskedTextBox.TextChanged +=
                    new EventHandler(InputControls_OnChange);
            else
            {
                if (subctrl.Controls.Count > 0)
                    this.AddOnChangeHandlerToInputControls(subctrl);
            }
        }
    }

    private void InputControls_OnChange(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        // Set background of control to orange if it has been changed

        var ctrl = sender as Control;

        ctrl.BackColor = Color.Orange;

        if (!_changedControls.Contains(ctrl.Name))
            _changedControls.Add(ctrl.Name);

    }

    private void ClearHoldStatusToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");

        if (clearHoldStatusToolStripMenuItem.Text == _clearHoldMenuText)
        {
            InputBoxResult prompt = InputBox.Show("Enter reason for setting status back to New:", "New Note");

            if (prompt.ReturnCode == DialogResult.OK)
            {
                _accountService.AddNote(_currentAccount.AccountNo, $"Claim hold cleared: {prompt.Text}");
                //reload notes to pick up changes
                LoadNotes();
            }

            _accountService.UpdateStatus(_currentAccount.AccountNo, AccountStatus.New);
            _currentAccount.Status = AccountStatus.New;
        }

        if (clearHoldStatusToolStripMenuItem.Text == _setHoldMenuText)
        {
            InputBoxResult prompt = InputBox.Show("Enter reason for claim hold:", "New Note");

            if (prompt.ReturnCode == DialogResult.OK)
            {
                _accountService.AddNote(_currentAccount.AccountNo, $"Claim hold set: {prompt.Text}");
                //reload notes to pick up changes
                LoadNotes();
            }

            _accountService.UpdateStatus(_currentAccount.AccountNo, AccountStatus.Hold);
            _currentAccount.Status = AccountStatus.Hold;
        }

        RefreshAccountData();

    }

    private void ResetControls(Control[] controls)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
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

    private async void ValidateAccountButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        try
        {
            Cursor.Current = Cursors.WaitCursor;
            _currentAccount = await _accountService.ValidateAsync(_currentAccount);
            RefreshAccountData();
            Cursor.Current = Cursors.Default;
        }
        catch (Exception ex)
        {
            ValidationResultsTextBox.Text = $"Exception in validation - report to support. {ex.Message}";
        }
    }

    private void GenerateClaimButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        ClaimGeneratorService claimGenerator = new(Program.AppEnvironment);

        claimGenerator.CompileClaim(_currentAccount.AccountNo);
    }

    private void BillActivityDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
    {
    }

    private void tabControl1_Selected(object sender, TabControlEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (e.TabPage.Name == tabDiagnosis.Name)
        {
            DiagnosisDataGrid.BackgroundColor = Color.White;
        }
    }

    private void GuarCopyPatientLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        GuarantorLastNameTextBox.Text = LastNameTextBox.Text;
        GuarFirstNameTextBox.Text = FirstNameTextBox.Text;
        GuarMiddleNameTextBox.Text = MiddleNameTextBox.Text;
        GuarSuffixTextBox.Text = SuffixTextBox.Text;
        GuarantorAddressTextBox.Text = Address1TextBox.Text;
        GuarCityTextBox.Text = CityTextBox.Text;
        GuarStateComboBox.SelectedValue = StateComboBox.SelectedValue;
        GuarZipTextBox.Text = ZipcodeTextBox.Text;
        GuarantorPhoneTextBox.Text = PhoneTextBox.Text;
        GuarantorRelationComboBox.SelectedValue = "01";
    }

    private void swapInsurancesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        AskInsuranceSwapForm frm = new(ref _currentAccount);

        if (frm.ShowDialog() == DialogResult.OK)
        {
            try
            {
                _currentAccount = _accountService.InsuranceSwap(_currentAccount, InsCoverage.Parse(frm.swap1), InsCoverage.Parse(frm.swap2));
                //await LoadAccountData();
                RefreshAccountData();
            }
            catch (Exception ex)
            {
                Log.Instance.Fatal(ex);
                MessageBox.Show("Exception encountered during insurance swap. Report this to your system administrator.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }

    private void DiagnosisDataGrid_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
    {
    }

    private void DiagnosisDataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
    {
    }

    private void statementFlagComboBox_SelectionChangeCommitted(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        _currentAccount = _accountService.UpdateStatementFlag(_currentAccount, statementFlagComboBox.SelectedItem.ToString());
        RefreshAccountData();
    }

    private async void moveAllChargesToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        PersonSearchForm personSearch = new();

        if (personSearch.ShowDialog() == DialogResult.OK)
        {
            string destAccount = personSearch.SelectedAccount;

            if (MessageBox.Show($"Move all charges to account {destAccount}?",
                "Confirm Move", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                Cursor.Current = Cursors.WaitCursor;
                Log.Instance.Debug($"Moving all charges from {_currentAccount.AccountNo} to {destAccount}");
                var (isSuccess, error) = _accountService.MoveCharges(_currentAccount.AccountNo, destAccount);
                if (!isSuccess)
                {
                    MessageBox.Show(error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                await LoadAccountData();

                Cursor.Current = Cursors.Default;
            }
        }
    }

    private void dxPointerGrid2_DataError(object sender, DataGridViewDataErrorEventArgs e)
    {
        Log.Instance.Error(e.Exception, e.Exception.Message);
        return;
    }

    private async void AccountForm_Activated(object sender, EventArgs e)
    {
        if(_isClosing)
        {
            return;
        }

        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (this.Disposing)
            return;
        if (_isClosing)
            return;
        await LoadAccountData();
    }

    private async void readyToBillCheckbox_CheckedChanged(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (!_billingTabLoading)
        {
            Cursor.Current = Cursors.WaitCursor;
            _currentAccount.ReadyToBill = readyToBillCheckbox.Checked;
            _accountService.UpdateStatus(_currentAccount.AccountNo, _currentAccount.Status);
            _currentAccount.Notes = _accountService.AddNote(_currentAccount.AccountNo, "Marked ready to bill.").ToList();
            _currentAccount = await _accountService.ValidateAsync(_currentAccount);
            RefreshAccountData();
            Cursor.Current = Cursors.Default;
        }
    }

    private void clearDxPointerToolStripMenuItem_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        var selectedCell = dxPointerGrid2.CurrentCell;

        if (selectedCell.ColumnIndex >= 3)
        {
            DataGridViewComboBoxCell comboBoxCell = (selectedCell as DataGridViewComboBoxCell);
            comboBoxCell.Value = null;
        }
    }

    private void dxPointerGrid2_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (e.ColumnIndex != -1 && e.RowIndex != -1 && e.Button == System.Windows.Forms.MouseButtons.Right)
        {

            Log.Instance.Trace($"Entering - {SelectedAccount}");

            DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
            c.DataGridView.ClearSelection();
            c.DataGridView.CurrentCell = c;
            c.Selected = true;

            dxPointerMenuStrip.Show(c.DataGridView, dxPointerGrid2.PointToClient(Cursor.Position));
        }
    }

    private void clearClaimStatusButton_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        if (MessageBox.Show("Clearing the claim status may result in duplicate claim submissions. Ensure the claim has been deleted in the clearing house system.",
            "Potential Duplicate Submission", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
        {
            _currentAccount = _accountService.ClearClaimStatus(_currentAccount);
            RefreshAccountData();
        }
    }

    private void noteAlertCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        Log.Instance.Trace($"Entering - {SelectedAccount}");
        _currentAccount.AccountAlert ??= new AccountAlert();
        _currentAccount.AccountAlert.AccountNo = _currentAccount.AccountNo;
        _currentAccount.AccountAlert.Alert = noteAlertCheckBox.Checked;
        bannerAlertLabel.Text = noteAlertCheckBox.Checked ? _notesAlertText : "";

        _accountService.SetNoteAlert(_currentAccount.AccountNo, noteAlertCheckBox.Checked);
    }

    private void BannerAccountTextBox_Click(object sender, EventArgs e) => Clipboard.SetText(BannerAccountTextBox.Text);

    private void copyToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void orderingPhyTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        _timer.Stop();
        _timer.Start();
    }

    private void _timer_Tick(object sender, EventArgs e)
    {
        _timer.Stop();
        if (orderingPhyTextBox == null)
            return;
        if (orderingPhyTextBox.Text.Length >= 3)
        {
            ProviderLookupForm frm = new()
            {
                Datasource = _providers,
                InitialSearchText = orderingPhyTextBox.Text
            };
            if (frm.ShowDialog() == DialogResult.OK)
            {
                orderingPhyTextBox.Text = frm.SelectedPhy.ToString();
                orderingPhyTextBox.Tag = frm.SelectedPhy.NpiId;
                _currentAccount.Pat.Physician = frm.SelectedPhy;
            }
        }
        return;
    }

    private void AccountForm_FormClosed(object sender, FormClosedEventArgs e)
    {
        _isClosing = true;
    }

    private void notesDataGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if(e.RowIndex != -1)
        {
            notesDataGridView.Rows[e.RowIndex].MinimumHeight = 2;
        }        
    }

    private void notesDataGridView_RowHeightChanged(object sender, DataGridViewRowEventArgs e)
    {
        e.Row.MinimumHeight = e.Row.Height;
    }


}
