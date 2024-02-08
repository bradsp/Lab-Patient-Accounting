using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Logging;
using LabBilling.Core.Models;
using LabBilling.Library;
using Utilities;
using System.Data;
using System.Threading.Tasks;
using LabBilling.Core.BusinessLogic;
using LabBilling.Legacy;
using WinFormsLibrary;
using LabBilling.LookupForms;


namespace LabBilling.Forms
{
    public partial class AccountForm : BaseForm
    {
        private BindingList<PatDiag> dxBindingList;
        private DataTable dxPointers = new();
        private BindingSource dxPointerBindingSource = new();
        private const string setHoldMenuText = "Set Claim Hold";
        private const string clearHoldMenuText = "Clear Claim Hold";

        private List<Phy> providers = null;
        private Account currentAccount = null;
        private bool InEditMode = false;
        private List<string> changedControls = new();
        private readonly Dictionary<Control, string> controlColumnMap = new();
        private readonly InsCompanyLookupForm lookupForm = new();

        private readonly AccountRepository accountRepository = new(Program.AppEnvironment);
        private readonly PatRepository patRepository = new(Program.AppEnvironment);
        private readonly DictDxRepository dictDxRepository = new(Program.AppEnvironment);
        private readonly UserProfileRepository userProfileDB = new(Program.AppEnvironment);
        private readonly ChkRepository chkRepository = new(Program.AppEnvironment);
        private readonly AccountNoteRepository accountNoteRepository = new(Program.AppEnvironment);
        private readonly PatientStatementAccountRepository patientStatementAccountRepository = new(Program.AppEnvironment);
        private bool billingTabLoading = false;
        private const int _timerInterval = 650;
        private const string notesAlertText = "** SEE NOTES **";
        private bool closing = false;
        private ChargeMaintenanceUC chargeMaintenance = new();
        private InsMaintenanceUC insPrimaryMaintenanceUC = new(InsCoverage.Primary);
        private InsMaintenanceUC insSecondaryMaintenanceUC = new(InsCoverage.Secondary);
        private InsMaintenanceUC insTertiaryMaintenanceUC = new(InsCoverage.Tertiary);

        //private bool skipSelectionChanged = false;
        private Timer _timer;

        public event EventHandler<string> AccountOpenedEvent;

        private string _selectedAccount;
        public string SelectedAccount
        {
            get { return _selectedAccount; }
        }

        private ListBox providerSearchListBox = new();

        /// <summary>
        /// Construct form with an account to open and optionally send the MDI parent form.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="parentForm"></param>
        public AccountForm(string account) : this()
        {
            Log.Instance.Trace("Entering");

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
            Log.Instance.Trace("Entering");

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
                closing = true;
                this.Visible = false;
                this.Close();
                return;
            }

            providerSearchListBox.Visible = false;
            tabDemographics.Controls.Add(providerSearchListBox);

            tabCharges.Controls.Add(chargeMaintenance);
            chargeMaintenance.Dock = DockStyle.Fill;
            chargeMaintenance.ChargesUpdated += DataChanged_EventHandler;
            chargeMaintenance.OnError += UserControl_OnError;

            tabInsPrimary.Controls.Add(insPrimaryMaintenanceUC);
            insPrimaryMaintenanceUC.Dock = DockStyle.Fill;
            insPrimaryMaintenanceUC.InsuranceChanged += DataChanged_EventHandler;
            insPrimaryMaintenanceUC.OnError += UserControl_OnError;

            tabInsSecondary.Controls.Add(insSecondaryMaintenanceUC);
            insSecondaryMaintenanceUC.Dock = DockStyle.Fill;
            insSecondaryMaintenanceUC.InsuranceChanged += DataChanged_EventHandler;
            insSecondaryMaintenanceUC.OnError += UserControl_OnError;

            tabInsTertiary.Controls.Add(insTertiaryMaintenanceUC);
            insTertiaryMaintenanceUC.Dock = DockStyle.Fill;
            insTertiaryMaintenanceUC.InsuranceChanged += DataChanged_EventHandler;
            insTertiaryMaintenanceUC.OnError += UserControl_OnError;

            #region Process permissions and enable controls

            SetFormPermissions();

            #endregion

            #region load controlColumnMap
            controlColumnMap.Add(SocSecNoTextBox, nameof(Account.SocSecNo));
            controlColumnMap.Add(DateOfBirthTextBox, nameof(Account.BirthDate));
            controlColumnMap.Add(SexComboBox, nameof(Account.Sex));

            controlColumnMap.Add(ZipcodeTextBox, nameof(Pat.ZipCode));
            controlColumnMap.Add(MaritalStatusComboBox, nameof(Pat.MaritalStatus));
            controlColumnMap.Add(EmailAddressTextBox, nameof(Pat.EmailAddress));
            controlColumnMap.Add(SuffixTextBox, nameof(Pat.PatNameSuffix));
            controlColumnMap.Add(LastNameTextBox, nameof(Pat.PatNameSuffix));
            controlColumnMap.Add(MiddleNameTextBox, nameof(Pat.PatMiddleName));
            controlColumnMap.Add(FirstNameTextBox, nameof(Pat.PatFirstName));
            controlColumnMap.Add(StateComboBox, nameof(Pat.State));
            controlColumnMap.Add(PhoneTextBox, nameof(Pat.PrimaryPhone));
            controlColumnMap.Add(CityTextBox, nameof(Pat.City));
            controlColumnMap.Add(Address2TextBox, nameof(Pat.Address2));
            controlColumnMap.Add(Address1TextBox, nameof(Pat.Address1));
            controlColumnMap.Add(GuarZipTextBox, nameof(Pat.GuarantorZipCode));
            controlColumnMap.Add(GuarSuffixTextBox, nameof(Pat.GuarantorNameSuffix));
            controlColumnMap.Add(GuarMiddleNameTextBox, nameof(Pat.GuarantorMiddleName));
            controlColumnMap.Add(GuarFirstNameTextBox, nameof(Pat.GuarantorFirstName));
            controlColumnMap.Add(GuarStateComboBox, nameof(Pat.GuarantorState));
            controlColumnMap.Add(GuarantorRelationComboBox, nameof(Pat.GuarRelationToPatient));
            controlColumnMap.Add(GuarantorPhoneTextBox, nameof(Pat.GuarantorPrimaryPhone));
            controlColumnMap.Add(GuarCityTextBox, nameof(Pat.GuarantorCity));
            controlColumnMap.Add(GuarantorAddressTextBox, nameof(Pat.GuarantorAddress));
            controlColumnMap.Add(GuarantorLastNameTextBox, nameof(Pat.GuarantorLastName));
            controlColumnMap.Add(orderingPhyTextBox, nameof(Pat.ProviderId));

            #endregion

            #region Setup ordering provider combo box

            providers = DataCache.Instance.GetProviders();
            _timer = new Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += _timer_Tick;
            //providerLookup1.Datasource = providers;

            #endregion

            #region populate combo boxes

            StateComboBox.DataSource = new BindingSource(Dictionaries.stateSource, null);
            StateComboBox.DisplayMember = "Value";
            StateComboBox.ValueMember = "Key";

            SexComboBox.DataSource = new BindingSource(Dictionaries.sexSource, null);
            SexComboBox.DisplayMember = "Value";
            SexComboBox.ValueMember = "Key";

            MaritalStatusComboBox.DataSource = new BindingSource(Dictionaries.maritalSource, null);
            MaritalStatusComboBox.DisplayMember = "Value";
            MaritalStatusComboBox.ValueMember = "Key";

            GuarStateComboBox.DataSource = new BindingSource(Dictionaries.stateSource, null);
            GuarStateComboBox.DisplayMember = "Value";
            GuarStateComboBox.ValueMember = "Key";

            GuarantorRelationComboBox.DataSource = new BindingSource(Dictionaries.relationSource, null);
            GuarantorRelationComboBox.DisplayMember = "Value";
            GuarantorRelationComboBox.ValueMember = "Key";

            #endregion

            if (SelectedAccount != null || SelectedAccount != "")
            {
                Log.Instance.Debug($"Loading account data for {SelectedAccount}");
                userProfileDB.InsertRecentAccount(SelectedAccount, Program.LoggedInUser.UserName);
                AccountOpenedEvent?.Invoke(this, SelectedAccount);

                AddOnChangeHandlerToInputControls(tabDemographics);
            }
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

        private async void DataChanged_EventHandler(object sender, EventArgs e) => await LoadAccountData();

        private void SetFormPermissions()
        {
            chargeMaintenance.AllowChargeEntry = Program.LoggedInUser.CanSubmitCharges;

            Helper.SetControlsAccess(tabPayments.Controls, false);
            if (Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry)
            {
                Helper.SetControlsAccess(tabPayments.Controls, Program.LoggedInUser.CanAddAdjustments);
            }

            insPrimaryMaintenanceUC.AllowEditing = false;
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
                if (Program.LoggedInUser.Access == "ENTER/EDIT")
                {
                    insPrimaryMaintenanceUC.AllowEditing = true;
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
            Log.Instance.Trace($"Entering");
            e.Cancel = false;

        }

        private async void RefreshButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            await LoadAccountData();
        }

        /// <summary>
        /// Loads account object from database and refreshes the form controls.
        /// </summary>
        private async Task LoadAccountData()
        {
            Log.Instance.Trace($"Entering");

            if (closing)
                return;

            this.SuspendLayout();
            currentAccount = await accountRepository.GetByAccountAsync(SelectedAccount);

            this.Text = $"{currentAccount.AccountNo} - {currentAccount.PatFullName}";

            dxBindingList = new BindingList<PatDiag>(currentAccount.Pat.Diagnoses);

            if (currentAccount.Status == AccountStatus.Hold)
                clearHoldStatusToolStripMenuItem.Text = clearHoldMenuText;
            else
                clearHoldStatusToolStripMenuItem.Text = setHoldMenuText;

            chargeMaintenance.CurrentAccount = currentAccount;
            insPrimaryMaintenanceUC.CurrentAccount = currentAccount;
            insPrimaryMaintenanceUC.CurrentIns = currentAccount.InsurancePrimary;
            insSecondaryMaintenanceUC.CurrentAccount = currentAccount;
            insSecondaryMaintenanceUC.CurrentIns = currentAccount.InsuranceSecondary;
            insTertiaryMaintenanceUC.CurrentAccount = currentAccount;
            insTertiaryMaintenanceUC.CurrentIns = currentAccount.InsuranceTertiary;

            RefreshAccountData();

            if (currentAccount.FinCode == "CLIENT")
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

            this.ResumeLayout();
        }

        /// <summary>
        /// Updates form controls from Account object
        /// </summary>
        private void RefreshAccountData()
        {
            LoadSummaryTab();
            LoadDemographics();
            chargeMaintenance.LoadCharges();
            insPrimaryMaintenanceUC.LoadInsuranceData();
            insSecondaryMaintenanceUC.LoadInsuranceData();
            insTertiaryMaintenanceUC.LoadInsuranceData();
            LoadPayments();
            LoadDx();
            LoadNotes();
            LoadBillingActivity();
        }

        private void LoadSummaryTab()
        {
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
            sd.Add(new SummaryData("EMR Account", currentAccount.MeditechAccount, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Status", currentAccount.Status, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("MRN", currentAccount.MRN, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("SSN", currentAccount.SocSecNo.FormatSSN(), SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Client", currentAccount.ClientName, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Ordering Provider", currentAccount.Pat.Physician?.FullName ?? currentAccount.Pat.ProviderId, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("DOB/Sex", currentAccount.DOBSex, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Address", currentAccount.Pat.AddressLine, SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Phone", currentAccount.Pat.PrimaryPhone.FormatPhone(), SummaryData.GroupType.Demographics, row++, col));
            sd.Add(new SummaryData("Email", currentAccount.Pat.EmailAddress, SummaryData.GroupType.Demographics, row++, col));


            sd.Add(new SummaryData("Diagnoses", "", SummaryData.GroupType.Diagnoses, row++, col, true));
            sd.Add(new SummaryData(currentAccount.Pat.Dx1, currentAccount.Pat.Dx1Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx2, currentAccount.Pat.Dx2Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx3, currentAccount.Pat.Dx3Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx4, currentAccount.Pat.Dx4Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx5, currentAccount.Pat.Dx5Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx6, currentAccount.Pat.Dx6Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx7, currentAccount.Pat.Dx7Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx8, currentAccount.Pat.Dx8Desc, SummaryData.GroupType.Diagnoses, row++, col));
            sd.Add(new SummaryData(currentAccount.Pat.Dx9, currentAccount.Pat.Dx9Desc, SummaryData.GroupType.Diagnoses, row++, col));

            //column 2
            col = 2;
            row = 1;
            sd.Add(new SummaryData("Financial", "", SummaryData.GroupType.Financial, row++, col, true));
            sd.Add(new SummaryData("Financial Class", currentAccount.FinCode, SummaryData.GroupType.Financial, row++, col));
            sd.Add(new SummaryData("Date of Service", currentAccount.TransactionDate.ToShortDateString(), SummaryData.GroupType.Financial, row++, col));
            sd.Add(new SummaryData("Total Charges", currentAccount.TotalCharges.ToString("c"), SummaryData.GroupType.Financial, row++, col));
            sd.Add(new SummaryData("Total Payments", (currentAccount.TotalPayments + currentAccount.TotalContractual + currentAccount.TotalWriteOff).ToString("c"),
                SummaryData.GroupType.Financial, row++, col));
            sd.Add(new SummaryData("3rd Party/Patient Balance", currentAccount.ClaimBalance.ToString("c"), SummaryData.GroupType.Financial, row++, col));
            foreach (var (client, balance) in currentAccount.ClientBalance)
            {
                sd.Add(new SummaryData($"Client Balance {client}", balance.ToString("c"), SummaryData.GroupType.Financial, row++, col));
            }
            sd.Add(new SummaryData("Account Balance", currentAccount.Balance.ToString("c"), SummaryData.GroupType.Financial, row++, col));

            foreach (Ins ins in currentAccount.Insurances)
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

        private void LoadDemographics()
        {
            Log.Instance.Trace("Entering");
            DemoStatusMessagesTextBox.Text = String.Empty;

            BannerNameTextBox.Text = currentAccount.PatFullName;
            BannerDobTextBox.Text = currentAccount.BirthDate.GetValueOrDefault().ToShortDateString();
            BannerSexTextBox.Text = currentAccount.Sex;
            BannerAccountTextBox.Text = SelectedAccount;
            BannerMRNTextBox.Text = currentAccount.MRN;
            BannerClientTextBox.Text = currentAccount.ClientName;
            BannerFinClassTextBox.Text = currentAccount.FinCode;
            BannerBillStatusTextBox.Text = currentAccount.Status;
            BannerProviderTextBox.Text = currentAccount.Pat.Physician?.FullName ?? "";
            bannerDateOfServiceTextBox.Text = currentAccount.TransactionDate.ToShortDateString();
            if (currentAccount.AccountAlert != null)
            {
                bannerAlertLabel.Text = currentAccount.AccountAlert.Alert ? notesAlertText : "";
                bannerAlertLabel.Visible = !string.IsNullOrEmpty(bannerAlertLabel.Text);
            }
            else
            {
                bannerAlertLabel.Visible = false;
                bannerAlertLabel.Text = "";
            }

            if (currentAccount.ReadyToBill)
            {
                bannerAlertLabel.Visible = true;
                bannerAlertLabel.Text += "  Account is flagged ready to bill, or has been billed. Any changes can affect the claim.";
                bannerAlertLabel.BackColor = Color.Red;
                bannerAlertLabel.ForeColor = Color.White;
            }

            BannerNameTextBox.Text = currentAccount.PatFullName;
            BannerAccountTextBox.Text = _selectedAccount;
            BannerDobTextBox.Text = currentAccount.BirthDate == null ? null : currentAccount.BirthDate.Value.ToShortDateString();
            BannerSexTextBox.Text = currentAccount.Sex;
            BannerMRNTextBox.Text = currentAccount.MRN;

            TotalChargesLabel.Text = currentAccount.TotalCharges.ToString("c");
            TotalPmtAdjLabel.Text = (currentAccount.TotalContractual + currentAccount.TotalPayments + currentAccount.TotalWriteOff).ToString("c");
            BalanceLabel.Text = currentAccount.Balance.ToString("c");
            ThirdPartyBalLabel.Text = currentAccount.ClaimBalance.ToString("c");
            ClientBalLabel.Text = currentAccount.ClientBalance.Sum(x => x.balance).ToString("c");

            //PatientFullNameLabel.Text = currentAccount.PatFullName;
            LastNameTextBox.Text = currentAccount.PatLastName;
            LastNameTextBox.BackColor = Color.White;
            FirstNameTextBox.Text = currentAccount.PatFirstName;
            FirstNameTextBox.BackColor = Color.White;
            MiddleNameTextBox.Text = currentAccount.PatMiddleName;
            MiddleNameTextBox.BackColor = Color.White;
            SuffixTextBox.Text = currentAccount.PatNameSuffix;
            SuffixTextBox.BackColor = Color.White;
            Address1TextBox.Text = currentAccount.Pat.Address1;
            Address1TextBox.BackColor = Color.White;
            Address2TextBox.Text = currentAccount.Pat.Address2;
            Address2TextBox.BackColor = Color.White;
            CityTextBox.Text = currentAccount.Pat.City;
            CityTextBox.BackColor = Color.White;
            StateComboBox.SelectedValue = currentAccount.Pat.State ?? "";
            StateComboBox.BackColor = Color.White;
            ZipcodeTextBox.Text = currentAccount.Pat.ZipCode;
            ZipcodeTextBox.BackColor = Color.White;
            PhoneTextBox.Text = currentAccount.Pat.PrimaryPhone;
            PhoneTextBox.BackColor = Color.White;
            SocSecNoTextBox.Text = currentAccount.SocSecNo;
            SocSecNoTextBox.BackColor = Color.White;
            EmailAddressTextBox.Text = currentAccount.Pat.EmailAddress;
            EmailAddressTextBox.BackColor = Color.White;
            DateOfBirthTextBox.Text = currentAccount.BirthDate == null ? string.Empty : currentAccount.BirthDate.Value.ToString("MM/dd/yyyy");
            DateOfBirthTextBox.BackColor = Color.White;
            SexComboBox.SelectedValue = currentAccount.Sex ?? string.Empty;
            SexComboBox.BackColor = Color.White;
            MaritalStatusComboBox.SelectedValue = !string.IsNullOrEmpty(currentAccount.Pat.MaritalStatus) ? currentAccount.Pat.MaritalStatus : "U";
            MaritalStatusComboBox.BackColor = Color.White;

            //providerLookup1.SelectedValue = currentAccount.Pat.ProviderId;
            //providerLookup1.DisplayValue = currentAccount.Pat.Physician.FullName;
            //providerLookup1.BackColor = Color.White;

            orderingPhyTextBox.Text = currentAccount.Pat.Physician?.ToString();
            orderingPhyTextBox.Tag = currentAccount.Pat.ProviderId;

            GuarantorLastNameTextBox.Text = currentAccount.Pat.GuarantorLastName;
            GuarFirstNameTextBox.Text = currentAccount.Pat.GuarantorFirstName;
            GuarMiddleNameTextBox.Text = currentAccount.Pat.GuarantorMiddleName;
            GuarSuffixTextBox.Text = currentAccount.Pat.GuarantorNameSuffix;
            GuarantorAddressTextBox.Text = currentAccount.Pat.GuarantorAddress;
            GuarCityTextBox.Text = currentAccount.Pat.GuarantorCity;
            GuarStateComboBox.SelectedValue = currentAccount.Pat.GuarantorState ?? "";
            GuarZipTextBox.Text = currentAccount.Pat.GuarantorZipCode;
            GuarantorPhoneTextBox.Text = currentAccount.Pat.GuarantorPrimaryPhone;
            GuarantorRelationComboBox.SelectedValue = currentAccount.Pat.GuarRelationToPatient ?? "";

            ResetControls(tabDemographics.Controls.OfType<Control>().ToArray());
            ResetControls(demographicsLayoutPanel.Controls.OfType<Control>().ToArray());
        }

        private async void SaveDemographics_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            currentAccount.PatFullName = $"{LastNameTextBox.Text} {SuffixTextBox.Text},{FirstNameTextBox.Text} {MiddleNameTextBox.Text}";
            currentAccount.PatLastName = LastNameTextBox.Text;
            currentAccount.PatFirstName = FirstNameTextBox.Text;
            currentAccount.PatMiddleName = MiddleNameTextBox.Text;
            currentAccount.PatNameSuffix = SuffixTextBox.Text;
            currentAccount.SocSecNo = SocSecNoTextBox.Text;
            currentAccount.BirthDate = DateTimeExtension.ValidateDateOrNull(DateOfBirthTextBox.Text);
            currentAccount.Sex = SexComboBox.SelectedValue.ToString();

            accountRepository.Update(currentAccount);

            currentAccount.Pat.Address1 = Address1TextBox.Text;
            currentAccount.Pat.Address2 = Address2TextBox.Text;
            currentAccount.Pat.EmailAddress = EmailAddressTextBox.Text;
            currentAccount.Pat.MaritalStatus = MaritalStatusComboBox.SelectedValue.ToString();
            currentAccount.Pat.PrimaryPhone = PhoneTextBox.Text;
            currentAccount.Pat.City = CityTextBox.Text;
            currentAccount.Pat.State = StateComboBox.SelectedValue.ToString();
            currentAccount.Pat.ZipCode = ZipcodeTextBox.Text;
            currentAccount.Pat.CityStateZip = string.Format("{0}, {1} {2}", CityTextBox.Text, StateComboBox.SelectedValue.ToString(), ZipcodeTextBox.Text);
            currentAccount.Pat.PatFullName = string.Format("{0},{1} {2}", LastNameTextBox.Text, FirstNameTextBox.Text, MiddleNameTextBox.Text);
            currentAccount.Pat.ProviderId = orderingPhyTextBox.Tag.ToString(); //providerLookup1.SelectedValue;

            currentAccount.Pat.GuarantorFullName = $"{GuarantorLastNameTextBox.Text} {GuarSuffixTextBox.Text},{GuarFirstNameTextBox.Text} {GuarMiddleNameTextBox.Text}";
            currentAccount.Pat.GuarantorLastName = GuarantorLastNameTextBox.Text;
            currentAccount.Pat.GuarantorFirstName = GuarFirstNameTextBox.Text;
            currentAccount.Pat.GuarantorMiddleName = GuarMiddleNameTextBox.Text;
            currentAccount.Pat.GuarantorNameSuffix = GuarSuffixTextBox.Text;
            currentAccount.Pat.GuarantorAddress = GuarantorAddressTextBox.Text;
            currentAccount.Pat.GuarantorCity = GuarCityTextBox.Text;
            currentAccount.Pat.GuarantorPrimaryPhone = GuarantorPhoneTextBox.Text;
            currentAccount.Pat.GuarantorState = GuarStateComboBox.SelectedValue.ToString();
            currentAccount.Pat.GuarantorZipCode = GuarZipTextBox.Text;
            currentAccount.Pat.GuarantorCityState = $"{GuarCityTextBox.Text}, {GuarStateComboBox.SelectedValue} {GuarZipTextBox.Text}";
            currentAccount.Pat.GuarRelationToPatient = GuarantorRelationComboBox.SelectedValue.ToString();

            patRepository.SaveAll(currentAccount.Pat);

            var controls = tabDemographics.Controls; //tabDemographics.Controls;

            foreach (Control control in controls)
            {
                //set background back to white to indicate change has been saved to database
                control.BackColor = Color.White;
            }

            await this.LoadAccountData();

        }

        #endregion

        #region PaymentsTab

        private void LoadPayments()
        {
            Log.Instance.Trace("Entering");

            TotalPaymentTextBox.Text = currentAccount.TotalPayments.ToString("c");
            TotalContractualTextBox.Text = currentAccount.TotalContractual.ToString("c");
            TotalWriteOffTextBox.Text = currentAccount.TotalWriteOff.ToString("c");
            TotalPmtAllTextBox.Text = (currentAccount.TotalPayments + currentAccount.TotalContractual + currentAccount.TotalWriteOff).ToString("c");

            if ((currentAccount.Fin.FinClass == "C" && currentAccount.FinCode != "CLIENT") ||
                currentAccount.Status == AccountStatus.Closed)
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

            currentAccount.Payments = currentAccount.Payments.OrderByDescending(x => x.PaymentNo).ToList();
            PaymentsDataGrid.DataSource = currentAccount.Payments.ToList();

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

            PaymentsDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            PaymentsDataGrid.BackgroundColor = Color.AntiqueWhite;
        }

        private void PaymentsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = PaymentsDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {

                DataGridViewRow row = PaymentsDataGrid.SelectedRows[0];
                var chk = chkRepository.GetById(Convert.ToInt32(row.Cells[nameof(Chk.PaymentNo)].Value.ToString()));

                DisplayPOCOForm<Chk> frm = new(chk)
                {
                    Title = "Payment Details"
                };
                frm.Show();
            }
        }

        private async void AddPaymentButton_Click(object sender, EventArgs e)
        {
            PaymentAdjustmentEntryForm form = new(ref currentAccount);

            if (currentAccount.SentToCollections)
            {
                if (MessageBox.Show($"Account {currentAccount.AccountNo} has been sent to collections. Follow process to notify collection agency of payment.\n Continue with add payment?",
                    "Account Sent to Collections", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) != DialogResult.Yes)
                    return;
            }

            if (form.ShowDialog() == DialogResult.OK)
            {
                //post record to account
                var chk = form.chk;

                if (currentAccount.Status == "PAID_OUT")
                {
                    accountRepository.UpdateStatus(currentAccount.AccountNo, "NEW");
                    currentAccount.Status = "NEW";
                }

                chk.AccountNo = currentAccount.AccountNo;
                chk.FinCode = currentAccount.FinCode;
                try
                {
                    chkRepository.Add(chk);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex);
                    MessageBox.Show($"Error adding payment. See log for details.");
                }
                await LoadAccountData();
            }
        }

        #endregion

        #region DiagnosisTab

        bool dxLoadingMode = false;

        private void LoadDx()
        {
            Log.Instance.Trace("Entering");
            DiagnosisDataGrid.DataSource = new BindingSource(dxBindingList, null);
            dxLoadingMode = true;
            dxPointerBindingSource.DataSource = null;
            dxPointerGrid2.DataSource = null;

            int cnt = dxBindingList.Count;
            string[] ptrStrings = new string[cnt + 1];

            ptrStrings[0] = "";

            for (int z = 1; z < cnt + 1; z++)
            {
                ptrStrings[z] = z.ToString();
            }

            dxPointers.Rows.Clear();
            dxPointers.Columns.Clear();

            dxPointers.Columns.Add(new DataColumn()
            {
                DataType = System.Type.GetType("System.String"),
                Caption = "CDM",
                ColumnName = "CDM",
            });

            dxPointers.Columns.Add(new DataColumn()
            {
                DataType = System.Type.GetType("System.String"),
                Caption = "CPT4",
                ColumnName = "CPT4",
            });

            dxPointers.Columns.Add(new DataColumn()
            {
                DataType = System.Type.GetType("System.String"),
                Caption = "Description",
                ColumnName = "Description",
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

            for (int i = 1; i < cnt + 1; i++)
            {
                dxPointers.Columns.Add(new DataColumn()
                {
                    DataType = System.Type.GetType("System.String"),
                    Caption = "Pointer",
                    ColumnName = ptrStrings[i]
                });

                dxPointerGrid2.Columns.Add(new DataGridViewComboBoxColumn()
                {
                    Name = ptrStrings[i],
                    DataPropertyName = ptrStrings[i],
                    DataSource = dxBindingList,
                    ValueMember = nameof(PatDiag.Code),
                    DisplayMember = nameof(PatDiag.Code),
                    HeaderText = ptrStrings[i],
                    FlatStyle = FlatStyle.Flat,
                    MinimumWidth = 100
                });
            }

            dxPointerBindingSource.DataSource = dxPointers;

            dxPointerGrid2.DataSource = dxPointerBindingSource;

            //load charges and pointers to grid
            foreach (var chrg in currentAccount.Charges)
            {
                if (!chrg.IsCredited && chrg.Status != "N/A")
                {
                    foreach (var chrgDetail in chrg.ChrgDetails)
                    {
                        DataRow row = dxPointers.NewRow();
                        row["CDM"] = chrg.CDMCode;
                        row["CPT4"] = chrgDetail.Cpt4;
                        row["Description"] = chrg.CdmDescription;

                        if (chrgDetail.DiagnosisPointer != null)
                        {
                            if (chrgDetail.DiagnosisPointer.DiagnosisPointer != null)
                            {
                                string[] ptrs = chrgDetail.DiagnosisPointer.DiagnosisPointer.Split(':');
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

                                    row[ptrStrings[pi + 1]] = dxBindingList.Where(x => x.No == iPtr).First().Code;
                                }
                            }
                        }

                        dxPointers.Rows.Add(row);
                    }
                }
            }
            dxPointerGrid2.AutoResizeColumns();
            dxLoadingMode = false;
        }

        private void dxPointerGrid2_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {

        }

        private void dxPointerGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //You can check for e.ColumnIndex to limit this to your specific column
            if (e.ColumnIndex > 2)
            {
                if (this.dxPointerGrid2.EditingControl is DataGridViewComboBoxEditingControl editingControl)
                    editingControl.DroppedDown = true;
            }
        }

        private void dxPointerGrid2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!dxLoadingMode)
            {
                //loop through columns
                int cnt = dxBindingList.Count;

                var dxSelected = new List<string>();

                for (int i = 0; i < cnt; i++)
                {
                    var val = dxPointerGrid2[(i + 1).ToString(), e.RowIndex].Value.ToString();
                    if (!string.IsNullOrEmpty(val))
                        dxSelected.Add(val);
                }

                var dup = dxSelected.GroupBy(x => x).Where(c => c.Count() > 1)
                    .Select(x => new { dx = x.Key, cnt = x.Count() }).ToList();

                if (dup.Count() > 0)
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

                    for (int i = 3; i < cnt + 3; i++)
                    {
                        dxPointerGrid2[i, e.RowIndex].Style.BackColor = Color.White;

                        var dxValue = dxPointerGrid2[i, e.RowIndex].Value.ToString();
                        var dxRecord = dxBindingList.Where(x => x.Code == dxValue).FirstOrDefault();
                        if (dxRecord != null && !string.IsNullOrEmpty(dxValue))
                        {
                            newPointer += $"{dxRecord.No}:";
                        }
                    }
                    //update pointers
                    var cpt = dxPointerGrid2["CPT4", e.RowIndex].Value.ToString();

                    var updatedChrg = currentAccount.Charges.Where(c => c.IsCredited == false && c.ChrgDetails.Any(cd => cd.Cpt4 == cpt)).ToList();
                    updatedChrg.ForEach(c => c.ChrgDetails.ForEach((cd) =>
                    {
                        if (cd.DiagnosisPointer == null)
                        {
                            cd.DiagnosisPointer = new ChrgDiagnosisPointer();
                            cd.DiagnosisPointer.ChrgDetailUri = cd.uri;
                        }
                        cd.DiagnosisPointer.DiagnosisPointer = newPointer;
                    }));

                    ChrgRepository chrgRepository = new(Program.AppEnvironment);

                    chrgRepository.UpdateDxPointers(updatedChrg);

                }
            }
        }

        private void DxSearchButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (!string.IsNullOrEmpty(txtSearchDx.Text))
            {
                var dictRecords = dictDxRepository.Search(txtSearchDx.Text, currentAccount.TransactionDate);

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
            Log.Instance.Trace($"Entering");
            DxSearchDataGrid.Columns[nameof(DictDx.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            DxSearchDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DxSearchDataGrid.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DiagnosisDataGrid_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            DiagnosisDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            DiagnosisDataGrid.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DxSearchDataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = DxSearchDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                //add selected diagnosis to account dx grid

                string selectedCode = DxSearchDataGrid.SelectedRows[0].Cells[nameof(DictDx.DxCode)].Value.ToString();
                string selectedDesc = DxSearchDataGrid.SelectedRows[0].Cells[nameof(DictDx.Description)].Value.ToString();

                if (dxBindingList.FirstOrDefault(n => n.Code == selectedCode) != null)
                {
                    //this code already exists in the list
                    MessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                int maxNo = 0;
                if (dxBindingList.Count > 0)
                    maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                if (maxNo >= 9)
                {
                    MessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = selectedCode, Description = selectedDesc });
                    DiagnosisDataGrid.BackgroundColor = Color.Orange;

                    SaveDiagnoses();
                }
            }
        }

        private void SearchDxTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.KeyChar == (char)13)
            {
                Log.Instance.Debug("Enter key pressed");
                DxSearchButton_Click(sender, e);
            }
        }

        private void DxQuickAddTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.KeyChar == (char)13)
            {
                Log.Instance.Debug("Enter key pressed");
                if (DxQuickAddTextBox.Text != "")
                {
                    //check to see if the text entered is a valid DX code - if so, add the code and description to the selected grid

                    if (dxBindingList.FirstOrDefault<PatDiag>(n => n.Code == DxQuickAddTextBox.Text) != null)
                    {
                        //this code already exists in the list
                        MessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        DxQuickAddTextBox.Text = "";
                        return;
                    }

                    var record = dictDxRepository.GetByCode(DxQuickAddTextBox.Text, currentAccount.TransactionDate);
                    if (record != null)
                    {
                        //this is a valid entry
                        int maxNo = 0;
                        if (dxBindingList.Count > 0)
                            maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                        if (maxNo >= 9)
                        {
                            MessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            DxQuickAddTextBox.Text = "";
                            return;
                        }
                        dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = record.DxCode, Description = record.Description });
                        DiagnosisDataGrid.BackgroundColor = Color.Orange;
                        DxQuickAddTextBox.Text = "";

                        SaveDiagnoses();
                    }
                    else
                    {
                        //not valid - clear box and do nothing
                        DxQuickAddTextBox.Text = "";
                        //System.Media.SystemSounds.Beep.Play();
                    }
                }
            }
        }

        private void DxDeleteButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = DiagnosisDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                //add selected diagnosis to account dx grid

                string selectedCode = DiagnosisDataGrid.SelectedRows[0].Cells[nameof(PatDiag.Code)].Value.ToString();
                string selectedNo = DiagnosisDataGrid.SelectedRows[0].Cells[nameof(PatDiag.No)].Value.ToString();

                var record = dxBindingList.IndexOf(dxBindingList.First<PatDiag>(n => n.Code == selectedCode));

                dxBindingList.RemoveAt(record);
                DiagnosisDataGrid.BackgroundColor = Color.Orange;
                //loop through and renumber
                for (int i = 0; i < dxBindingList.Count; i++)
                {
                    dxBindingList[i].No = i + 1;
                }

                SaveDiagnoses();
            }
        }

        private void DiagnosisDataGrid_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            e.Cancel = true;
            DxDeleteButton_Click(sender, e);
        }

        private void SaveDxButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            SaveDiagnoses();
        }

        private void SaveDiagnoses()
        {
            Log.Instance.Trace($"Entering");

            currentAccount.Pat.Diagnoses = dxBindingList.ToList<PatDiag>();

            if (accountRepository.UpdateDiagnoses(currentAccount) == true)
            {
                DiagnosisDataGrid.BackgroundColor = Color.White;
                //MessageBox.Show(this, "Diagnoses updated successfully.");
                RefreshAccountData();
            }
            else
            {
                MessageBox.Show(this, "Diagnosis update failed.");
            }
        }

        private void DiagnosisDataGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        #endregion

        #region NotesTab

        private void LoadNotes()
        {
            Log.Instance.Trace("Entering");
            notesDataGridView.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.AllCells;
            notesDataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            notesDataGridView.DefaultCellStyle.WrapMode = DataGridViewTriState.True;
            notesDataGridView.DataSource = currentAccount.Notes;
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

            if (currentAccount.AccountAlert != null)
                noteAlertCheckBox.Checked = currentAccount.AccountAlert.Alert;
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            InputBoxResult prompt = InputBox.Show("Enter note:", "New Note", true);
            AccountNote note = new();

            if (prompt.ReturnCode == DialogResult.OK)
            {
                note.Account = currentAccount.AccountNo;
                note.Comment = prompt.Text;
                accountNoteRepository.Add(note);
                //reload notes to pick up changes
                currentAccount.Notes = accountNoteRepository.GetByAccount(currentAccount.AccountNo);
                LoadNotes();
            }

        }

        #endregion

        #region BillingActivityTab

        private void LoadBillingActivity()
        {
            Log.Instance.Trace("Entering");

            billingTabLoading = true;

            BillActivityDataGrid.DataSource = currentAccount.BillingActivities.ToList();
            BillActivityDataGrid.Columns[nameof(BillingActivity.rowguid)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedDate)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedHost)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedApp)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.UpdatedUser)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.Text)].Visible = false;

            BillActivityDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            ValidationResultsTextBox.Text = currentAccount.AccountValidationStatus.ValidationText;
            LastValidatedLabel.Text = currentAccount.AccountValidationStatus.UpdatedDate.ToString("G");

            statementFlagComboBox.SelectedItem = currentAccount.Pat.StatementFlag;
            firstStmtDateTextBox.Text = currentAccount.Pat.FirstStatementDate.ToString();
            lastStmtDateTextBox.Text = currentAccount.Pat.LastStatementDate.ToString();
            minPmtTextBox.Text = currentAccount.Pat.MinimumPaymentAmount.ToString();

            readyToBillCheckbox.Checked = currentAccount.ReadyToBill;

            statementHistoryDataGrid.DataSource = patientStatementAccountRepository.GetByAccount(currentAccount.AccountNo);

            statementHistoryDataGrid.SetColumnsVisibility(false);

            statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.DateSent)].Visible = true;
            statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.Mailer)].Visible = true;
            statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.MailerCount)].Visible = true;
            statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.ProcessedDate)].Visible = true;
            statementHistoryDataGrid.Columns[nameof(PatientStatementAccount.StatementNumber)].Visible = true;

            billingTabLoading = false;
        }

        #endregion


        //private async void PersonSearchToolStripMenuItem_Click(object sender, EventArgs e)
        //{
        //    Log.Instance.Trace($"Entering");
        //    PersonSearchForm frm = new();
        //    frm.ShowDialog();
        //    if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
        //    {
        //        _selectedAccount = frm.SelectedAccount;
        //        await LoadAccountData();
        //    }
        //    else
        //    {
        //        Log.Instance.Error($"Person search returned an empty selected account.");
        //        MessageBox.Show(this, "A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
        //    }
        //}

        private void ChangeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var result = InputDialogs.SelectDateOfService((DateTime)currentAccount.TransactionDate);

            try
            {
                if (result.newDate != DateTime.MinValue)
                {
                    accountRepository.ChangeDateOfService(currentAccount, result.newDate, result.reason);
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

        private async void ChangeFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            string newFinCode = InputDialogs.SelectFinancialCode(currentAccount.FinCode);
            if (!string.IsNullOrEmpty(newFinCode))
            {
                try
                {
                    accountRepository.ChangeFinancialClass(currentAccount, newFinCode);
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

                await LoadAccountData();
            }
        }

        private async void ChangeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            ClientLookupForm clientLookupForm = new();
            clientLookupForm.Datasource = DataCache.Instance.GetClients();

            if (clientLookupForm.ShowDialog() == DialogResult.OK)
            {
                string newClient = clientLookupForm.SelectedValue;

                try
                {
                    if (accountRepository.ChangeClient(currentAccount, newClient))
                    {
                        await LoadAccountData();
                    }
                    else
                    {
                        MessageBox.Show("Error during update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
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
            Log.Instance.Trace($"Entering");
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
            Log.Instance.Trace($"Entering");
            // Set background of control to orange if it has been changed

            var ctrl = sender as Control;

            ctrl.BackColor = Color.Orange;

            if (!changedControls.Contains(ctrl.Name))
                changedControls.Add(ctrl.Name);

        }

        private async void ClearHoldStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (clearHoldStatusToolStripMenuItem.Text == clearHoldMenuText)
            {
                InputBoxResult prompt = InputBox.Show("Enter reason for setting status back to New:", "New Note");
                AccountNote note = new();

                if (prompt.ReturnCode == DialogResult.OK)
                {
                    note.Account = currentAccount.AccountNo;
                    note.Comment = $"Claim hold cleared: {prompt.Text}";
                    accountNoteRepository.Add(note);
                    //reload notes to pick up changes
                    LoadNotes();
                }

                accountRepository.UpdateStatus(currentAccount.AccountNo, AccountStatus.New);
            }

            if (clearHoldStatusToolStripMenuItem.Text == setHoldMenuText)
            {
                InputBoxResult prompt = InputBox.Show("Enter reason for claim hold:", "New Note");
                AccountNote note = new();

                if (prompt.ReturnCode == DialogResult.OK)
                {
                    note.Account = currentAccount.AccountNo;
                    note.Comment = $"Claim hold set: {prompt.Text}";
                    accountNoteRepository.Add(note);
                    //reload notes to pick up changes
                    LoadNotes();
                }

                accountRepository.UpdateStatus(currentAccount.AccountNo, AccountStatus.Hold);
            }

            await LoadAccountData();

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
                changedControls.Remove(ctrl.Name);
            }
        }

        private async void ValidateAccountButton_Click(object sender, EventArgs e)
        {
            try
            {
                await LoadAccountData();
                if (!await Task.Run(() => accountRepository.Validate(currentAccount)))
                {
                    //has validation errors - do not bill
                    ValidationResultsTextBox.Text = currentAccount.AccountValidationStatus.ValidationText;
                    LastValidatedLabel.Text = currentAccount.AccountValidationStatus.UpdatedDate.ToString("G");
                }
                else
                {
                    //ok to bill
                    ValidationResultsTextBox.Text = "No validation errors.";
                    LastValidatedLabel.Text = currentAccount.AccountValidationStatus.UpdatedDate.ToString("G");
                }
                await LoadAccountData();
            }
            catch (Exception ex)
            {
                ValidationResultsTextBox.Text = $"Exception in validation - report to support. {ex.Message}";
            }
        }

        private void GenerateClaimButton_Click(object sender, EventArgs e)
        {
            ClaimGenerator claimGenerator = new(Program.AppEnvironment);

            claimGenerator.CompileClaim(currentAccount.AccountNo);
        }

        private void BillActivityDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == tabDiagnosis.Name)
            {
                DiagnosisDataGrid.BackgroundColor = Color.White;
            }
        }

        private void providerLookup1_SelectedValueChanged(object source, EventArgs args)
        {
            //string phy = providerLookup1.SelectedValue;
        }

        private void GuarCopyPatientLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
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

        private async void swapInsurancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskInsuranceSwapForm frm = new(ref currentAccount);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    accountRepository.InsuranceSwap(currentAccount.AccountNo, InsCoverage.Parse(frm.swap1), InsCoverage.Parse(frm.swap2));
                    await LoadAccountData();
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

        private async void statementFlagComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (currentAccount.Pat.StatementFlag != "N")
            {
                accountRepository.UpdateStatus(currentAccount.AccountNo, "STMT");
            }

            //validate account - if valid, change statement flag. Otherwise, show errors.
            accountRepository.AddNote(currentAccount.AccountNo, $"Statement flag changed from {currentAccount.Pat.StatementFlag} to {statementFlagComboBox.SelectedItem}");

            currentAccount.Pat.StatementFlag = statementFlagComboBox.SelectedItem.ToString();
            patRepository.Update(currentAccount.Pat, new[] { nameof(Pat.StatementFlag) });
            accountRepository.UpdateStatus(currentAccount.AccountNo, AccountStatus.Statements);

            await LoadAccountData();
        }

        private async void moveAllChargesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            PersonSearchForm personSearch = new();

            if (personSearch.ShowDialog() == DialogResult.OK)
            {
                string destAccount = personSearch.SelectedAccount;

                if (MessageBox.Show($"Move all charges to account {destAccount}?",
                    "Confirm Move", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    Log.Instance.Debug($"Moving all charges from {currentAccount.AccountNo} to {destAccount}");
                    var result = accountRepository.MoveCharges(currentAccount.AccountNo, destAccount);
                    if (!result.isSuccess)
                    {
                        MessageBox.Show(result.error, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    await LoadAccountData();
                }
            }
        }

        private void dxPointerGrid2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            Log.Instance.Error(e.Exception, e.Exception.Message);
            return;
        }

        private async void AccountForm_Activated(object sender, EventArgs e) => await LoadAccountData();

        private async void readyToBillCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!billingTabLoading)
            {
                currentAccount.ReadyToBill = readyToBillCheckbox.Checked;
                accountRepository.UpdateStatus(currentAccount.AccountNo, currentAccount.Status);
                accountRepository.AddNote(currentAccount.AccountNo, "Marked ready to bill.");
                accountRepository.Validate(currentAccount);
                await LoadAccountData();
            }
        }

        private void clearDxPointerToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                DataGridViewCell c = (sender as DataGridView)[e.ColumnIndex, e.RowIndex];
                c.DataGridView.ClearSelection();
                c.DataGridView.CurrentCell = c;
                c.Selected = true;

                dxPointerMenuStrip.Show(c.DataGridView, dxPointerGrid2.PointToClient(Cursor.Position));
            }
        }

        private void printEOBToolStripMenuItem_Click(object sender, EventArgs e)
        {

            List<string> args = new();
            args.AddRange(Helper.GetArgs());

            args.Add(currentAccount.AccountNo);

            PrintEOBForm frm = new(args.ToArray());

            frm.ShowDialog(this);
        }

        private async void clearClaimStatusButton_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Clearing the claim status may result in duplicate claim submissions. Ensure the claim has been deleted in the clearing house system.",
                "Potential Duplicate Submission", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                accountRepository.ClearClaimStatus(currentAccount);

                await LoadAccountData();
            }
        }

        private void chargeDetailsContextMenu_Opening(object sender, CancelEventArgs e) { }

        private void noteAlertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            currentAccount.AccountAlert ??= new AccountAlert();
            currentAccount.AccountAlert.AccountNo = currentAccount.AccountNo;
            currentAccount.AccountAlert.Alert = noteAlertCheckBox.Checked;
            bannerAlertLabel.Text = noteAlertCheckBox.Checked ? notesAlertText : "";

            accountRepository.SetNoteAlert(currentAccount.AccountNo, noteAlertCheckBox.Checked);
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
            if (orderingPhyTextBox.Text.Length >= 3)
            {
                ProviderLookupForm frm = new()
                {
                    Datasource = providers,
                    InitialSearchText = orderingPhyTextBox.Text
                };
                if (frm.ShowDialog() == DialogResult.OK)
                {
                    orderingPhyTextBox.Text = frm.SelectedPhy.ToString();
                    orderingPhyTextBox.Tag = frm.SelectedPhy.NpiId;
                    currentAccount.Pat.Physician = frm.SelectedPhy;
                }
            }
            return;
        }
    }
}
