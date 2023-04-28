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
using RFClassLibrary;
using System.Data;
using System.Text;
using System.Reflection;
using System.Threading.Tasks;
using MetroFramework;
using LabBilling.Core.BusinessLogic;
using System.Text.RegularExpressions;
using System.Configuration;
using System.Drawing.Printing;
using LabBilling.Legacy;

namespace LabBilling.Forms
{
    public partial class AccountForm : Form
    {
        private BindingList<PatDiag> dxBindingList;
        private DataTable chargesTable = new DataTable();
        private DataTable dxPointers = new DataTable();
        private BindingSource dxPointerBindingSource = new BindingSource();

        private List<InsCompany> insCompanies = null;
        private List<Phy> providers = null;
        private Account currentAccount = null;
        private BindingSource insGridSource = null;
        private bool InEditMode = false;
        private List<string> changedControls = new List<string>();
        private Dictionary<Control, string> controlColumnMap = new Dictionary<Control, string>();
        private InsCompanyLookupForm lookupForm = new InsCompanyLookupForm();

        private readonly InsRepository insRepository = new InsRepository(Program.AppEnvironment);
        private readonly AccountRepository accountRepository = new AccountRepository(Program.AppEnvironment);
        private readonly PatRepository patRepository = new PatRepository(Program.AppEnvironment);
        private readonly DictDxRepository dictDxRepository = new DictDxRepository(Program.AppEnvironment);
        private readonly InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Program.AppEnvironment);
        private readonly ChrgRepository chrgRepository = new ChrgRepository(Program.AppEnvironment);
        private readonly ChrgDetailRepository chrgDetailRepository = new ChrgDetailRepository(Program.AppEnvironment);
        private readonly UserProfileRepository userProfileDB = new UserProfileRepository(Program.AppEnvironment);
        private readonly FinRepository finRepository = new FinRepository(Program.AppEnvironment);
        private readonly ChkRepository chkRepository = new ChkRepository(Program.AppEnvironment);
        private readonly AccountNoteRepository accountNoteRepository = new AccountNoteRepository(Program.AppEnvironment);
        private readonly BillingActivityRepository billingActivityRepository = new BillingActivityRepository(Program.AppEnvironment);
        private readonly SystemParametersRepository systemParametersRepository = new SystemParametersRepository(Program.AppEnvironment);
        private readonly PhyRepository phyRepository = new PhyRepository(Program.AppEnvironment);
        private bool billingTabLoading = false;
        private const int _timerInterval = 650;
        private const string notesAlertText = "** SEE NOTES **";
        //private bool skipSelectionChanged = false;
        private System.Windows.Forms.Timer _timer;

        public event EventHandler<string> AccountOpenedEvent;

        private string _selectedAccount;
        public string SelectedAccount
        {
            get { return _selectedAccount; }
        }

        private ListBox providerSearchListBox = new ListBox();

        /// <summary>
        /// Construct form with an account to open and optionally send the MDI parent form.
        /// </summary>
        /// <param name="account"></param>
        /// <param name="parentForm"></param>
        public AccountForm(string account, Form parentForm = null) : this()
        {
            Log.Instance.Trace("Entering");

            if (account != null)
            {
                _selectedAccount = account;
            }

            if (parentForm != null)
                this.MdiParent = parentForm;
        }

        private AccountForm()
        {
            Log.Instance.Trace("Entering");
            InitializeComponent();
            _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(insurancePlanTextBox_KeyUpDone);
        }

        #region MainForm

        private void AccountForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");


            providerSearchListBox.Visible = false;
            tabDemographics.Controls.Add(providerSearchListBox);

            #region Process permissions and enable controls

            SetFormPermissions();

            #endregion

            #region load controlColumnMap

            controlColumnMap.Add(ZipcodeTextBox, nameof(Pat.ZipCode));
            controlColumnMap.Add(MaritalStatusComboBox, nameof(Pat.MaritalStatus));
            controlColumnMap.Add(EmailAddressTextBox, nameof(Pat.EmailAddress));
            controlColumnMap.Add(SuffixTextBox, nameof(Pat.PatNameSuffix));
            controlColumnMap.Add(LastNameTextBox, nameof(Pat.PatNameSuffix));
            controlColumnMap.Add(MiddleNameTextBox, nameof(Pat.PatMiddleName));
            controlColumnMap.Add(FirstNameTextBox, nameof(Pat.PatFirstName));
            controlColumnMap.Add(StateComboBox, nameof(Pat.State));
            controlColumnMap.Add(SocSecNoTextBox, nameof(Account.SocSecNo));
            controlColumnMap.Add(DateOfBirthTextBox, nameof(Account.BirthDate));
            controlColumnMap.Add(PhoneTextBox, nameof(Pat.PrimaryPhone));
            controlColumnMap.Add(CityTextBox, nameof(Pat.City));
            controlColumnMap.Add(Address2TextBox, nameof(Pat.Address2));
            controlColumnMap.Add(SexComboBox, nameof(Account.Sex));
            controlColumnMap.Add(Address1TextBox, nameof(Pat.Address1));
            controlColumnMap.Add(GuarZipTextBox, nameof(Pat.GuarantorZipCode));
            controlColumnMap.Add(PlanFinCodeComboBox, nameof(Ins.FinCode));
            controlColumnMap.Add(CertSSNTextBox, nameof(Ins.CertSSN));
            controlColumnMap.Add(HolderLastNameTextBox, nameof(Ins.HolderLastName));
            controlColumnMap.Add(GroupNameTextBox, nameof(Ins.GroupName));
            controlColumnMap.Add(HolderZipTextBox, nameof(Ins.HolderZip));
            controlColumnMap.Add(GroupNumberTextBox, nameof(Ins.GroupNumber));
            controlColumnMap.Add(PlanAddress2TextBox, nameof(Ins.PlanStreetAddress2));
            controlColumnMap.Add(InsRelationComboBox, nameof(Ins.Relation));
            controlColumnMap.Add(PolicyNumberTextBox, nameof(Ins.PolicyNumber));
            controlColumnMap.Add(HolderDOBTextBox, nameof(Ins.HolderBirthDate));
            controlColumnMap.Add(HolderStateComboBox, nameof(Ins.HolderState));
            controlColumnMap.Add(HolderFirstNameTextBox, nameof(Ins.HolderFirstName));
            controlColumnMap.Add(InsOrderComboBox, nameof(Ins.Coverage));
            controlColumnMap.Add(HolderSexComboBox, nameof(Ins.HolderSex));
            controlColumnMap.Add(HolderMiddleNameTextBox, nameof(Ins.HolderMiddleName));
            controlColumnMap.Add(PlanNameTextBox, nameof(Ins.PlanName));
            controlColumnMap.Add(insurancePlanTextBox, nameof(Ins.InsCode));
            controlColumnMap.Add(HolderAddressTextBox, nameof(Ins.HolderStreetAddress));
            controlColumnMap.Add(PlanCityStTextBox, nameof(Ins.PlanCityState));
            controlColumnMap.Add(PlanAddressTextBox, nameof(Ins.PlanStreetAddress1));
            controlColumnMap.Add(HolderCityTextBox, nameof(Ins.HolderCity));
            controlColumnMap.Add(GuarSuffixTextBox, nameof(Pat.GuarantorNameSuffix));
            controlColumnMap.Add(GuarMiddleNameTextBox, nameof(Pat.GuarantorMiddleName));
            controlColumnMap.Add(GuarFirstNameTextBox, nameof(Pat.GuarantorFirstName));
            controlColumnMap.Add(GuarStateComboBox, nameof(Pat.GuarantorState));
            controlColumnMap.Add(GuarantorRelationComboBox, nameof(Pat.GuarRelationToPatient));
            controlColumnMap.Add(GuarantorPhoneTextBox, nameof(Pat.GuarantorPrimaryPhone));
            controlColumnMap.Add(GuarCityTextBox, nameof(Pat.GuarantorCity));
            controlColumnMap.Add(GuarantorAddressTextBox, nameof(Pat.GuarantorAddress));
            controlColumnMap.Add(GuarantorLastNameTextBox, nameof(Pat.GuarantorLastName));
            controlColumnMap.Add(providerLookup1, nameof(Pat.ProviderId));
            #endregion

            #region Setup Insurance Company Combobox

            insCompanies = DataCache.Instance.GetInsCompanies(); //insCompanyRepository.GetAll(true).ToList();
            #endregion

            lookupForm.Datasource = insCompanies;

            #region Setup ordering provider combo box

            providers = DataCache.Instance.GetProviders(); //phyRepository.GetActive().OrderBy(x => x.FullName).ToList();
            providerLookup1.Datasource = providers;

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

            HolderStateComboBox.DataSource = new BindingSource(Dictionaries.stateSource, null);
            HolderStateComboBox.DisplayMember = "Value";
            HolderStateComboBox.ValueMember = "Key";

            GuarantorRelationComboBox.DataSource = new BindingSource(Dictionaries.relationSource, null);
            GuarantorRelationComboBox.DisplayMember = "Value";
            GuarantorRelationComboBox.ValueMember = "Key";

            InsRelationComboBox.DataSource = new BindingSource(Dictionaries.relationSource, null);
            InsRelationComboBox.DisplayMember = "Value";
            InsRelationComboBox.ValueMember = "Key";

            InsOrderComboBox.DataSource = new BindingSource(Dictionaries.payorOrderSource, null);
            InsOrderComboBox.DisplayMember = "Value";
            InsOrderComboBox.ValueMember = "Key";

            HolderSexComboBox.DataSource = new BindingSource(Dictionaries.sexSource, null);
            HolderSexComboBox.DisplayMember = "Value";
            HolderSexComboBox.ValueMember = "Key";


            PlanFinCodeComboBox.DataSource = DataCache.Instance.GetFins(); // finDB.GetAll();
            PlanFinCodeComboBox.DisplayMember = nameof(Fin.Description);
            PlanFinCodeComboBox.ValueMember = nameof(Fin.FinCode);
            PlanFinCodeComboBox.SelectedIndex = -1;

            #endregion

            if (SelectedAccount != null || SelectedAccount != "")
            {
                Log.Instance.Debug($"Loading account data for {SelectedAccount}");
                userProfileDB.InsertRecentAccount(SelectedAccount, Program.LoggedInUser.UserName);
                AccountOpenedEvent?.Invoke(this, SelectedAccount);
                //LoadAccountData();

                AddOnChangeHandlerToInputControls(tabDemographics);
                AddOnChangeHandlerToInputControls(tabGuarantor);
                AddOnChangeHandlerToInputControls(tabInsurance);
            }

            //build context menu
            foreach (var item in Dictionaries.cptModifiers)
            {
                ToolStripMenuItem tsItem = new ToolStripMenuItem(item.Key);
                tsItem.Tag = item.Value;
                tsItem.Click += new EventHandler(AddModifier_Click);

                addModifierToolStripMenuItem.DropDownItems.Add(tsItem);
            }

            removeModifierToolStripMenuItem.Click += new EventHandler(RemoveModifier_Click);


            //UpdateDxPointersButton.Enabled = false; // start disabled until a box has been checked.
        }

        private void SetFormPermissions()
        {
            Helper.SetControlsAccess(tabCharges.Controls, false);
            Helper.SetControlsAccess(chargeLayoutPanel.Controls, false);
            //if (systemParametersRepository.GetByKey("allow_chrg_entry") == "1")
            if (Program.AppEnvironment.ApplicationParameters.AllowChargeEntry)
            {
                Helper.SetControlsAccess(tabCharges.Controls, Program.LoggedInUser.CanSubmitCharges);
                Helper.SetControlsAccess(chargeLayoutPanel.Controls, Program.LoggedInUser.CanSubmitCharges);
            }

            Helper.SetControlsAccess(tabPayments.Controls, false);
            //if (systemParametersRepository.GetByKey("allow_chk_entry") == "1")
            if(Program.AppEnvironment.ApplicationParameters.AllowPaymentAdjustmentEntry)
            {
                Helper.SetControlsAccess(tabPayments.Controls, Program.LoggedInUser.CanAddAdjustments);
            }

            //Helper.SetControlsAccess(DemographicsTabLayoutPanel.Controls, false);
            Helper.SetControlsAccess(tabDemographics.Controls, false);
            Helper.SetControlsAccess(tabInsurance.Controls, false);
            Helper.SetControlsAccess(insTabLayoutPanel.Controls, false);
            Helper.SetControlsAccess(tabDiagnosis.Controls, false);
            Helper.SetControlsAccess(tabGuarantor.Controls, false);
            Helper.SetControlsAccess(tabNotes.Controls, false);
            //Helper.SetControlsAccess(tabCharges.Controls, false);
            //Helper.SetControlsAccess(tabPayments.Controls, false);
            AddChargeButton.Visible = false;
            AddPaymentButton.Visible = false;
            SaveInsuranceButton.Visible = false;
            //SaveDxButton.Visible = false;
            SaveDemographics.Visible = false;
            GuarCopyPatientLink.Visible = false;
            InsCopyPatientLink.Visible = false;
            changeClientToolStripMenuItem.Visible = false;
            changeDateOfServiceToolStripMenuItem.Visible = false;
            changeFinancialClassToolStripMenuItem.Visible = false;
            clearHoldStatusToolStripMenuItem.Visible = false;
            ValidateAccountButton.Visible = false;
            GenerateClaimButton.Visible = false;
            //if (Convert.ToBoolean(systemParametersRepository.GetByKey("allow_edit")))
            if(Program.AppEnvironment.ApplicationParameters.AllowEditing)
            {
                if (Program.LoggedInUser.Access == "ENTER/EDIT")
                {
                    Helper.SetControlsAccess(tabDemographics.Controls, true);
                    Helper.SetControlsAccess(tabInsurance.Controls, true);
                    Helper.SetControlsAccess(insTabLayoutPanel.Controls, true);
                    Helper.SetControlsAccess(tabDiagnosis.Controls, true);
                    Helper.SetControlsAccess(tabGuarantor.Controls, true);
                    Helper.SetControlsAccess(tabNotes.Controls, true);
                    Helper.SetControlsAccess(chargeLayoutPanel.Controls, true);
                    Helper.SetControlsAccess(tabCharges.Controls, true);
                    Helper.SetControlsAccess(tabPayments.Controls, true);
                    AddChargeButton.Visible = Program.LoggedInUser.CanSubmitCharges;
                    AddPaymentButton.Visible = Program.LoggedInUser.CanAddAdjustments;
                    //UpdateDxPointersButton.Visible = true;
                    SaveInsuranceButton.Visible = true;
                    //SaveDxButton.Visible = true;
                    SaveDemographics.Visible = true;
                    GuarCopyPatientLink.Visible = true;
                    InsCopyPatientLink.Visible = true;
                    changeClientToolStripMenuItem.Visible = true;
                    changeDateOfServiceToolStripMenuItem.Visible = true;
                    changeFinancialClassToolStripMenuItem.Visible = Program.LoggedInUser.CanModifyAccountFincode;
                    clearHoldStatusToolStripMenuItem.Visible = true;
                    ValidateAccountButton.Visible = true;
                    GenerateClaimButton.Visible = Program.LoggedInUser.CanSubmitBilling;
                }
            }

        }

        private void AccountForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            LoadAccountData();
        }

        /// <summary>
        /// Loads account object from database and refreshes the form controls.
        /// </summary>
        private void LoadAccountData()
        {
            Log.Instance.Trace($"Entering");
            currentAccount = accountRepository.GetByAccount(SelectedAccount);

            generateClientStatementToolStripMenuItem.Enabled = currentAccount.FinCode == "CLIENT";

            this.Text = $"{currentAccount.AccountNo} - {currentAccount.PatFullName}";

            dxBindingList = new BindingList<PatDiag>(currentAccount.Pat.Diagnoses);
            ShowCreditedChrgCheckBox.Checked = false;

            if (currentAccount.ReadyToBill)
            {
                //MessageBox.Show("Account is flagged ready to bill, or has been billed. Any changes can affect the claim.");
            }

            RefreshAccountData();
        }

        /// <summary>
        /// Updates form controls from Account object
        /// </summary>
        private void RefreshAccountData()
        {
            LoadSummaryTab();
            LoadDemographics();
            LoadInsuranceData();
            LoadCharges();
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
            List<SummaryData> sd = new List<SummaryData>();

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
            sd.Add(new SummaryData("Ordering Provider", currentAccount.Pat.Physician.FullName, SummaryData.GroupType.Demographics, row++, col));
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
            BannerProviderTextBox.Text = currentAccount.Pat.Physician.FullName;
            bannerDateOfServiceTextBox.Text = currentAccount.TransactionDate.ToShortDateString();
            if (currentAccount.AccountAlert != null)
                bannerAlertLabel.Text = currentAccount.AccountAlert.Alert ? notesAlertText : "";
            else
                bannerAlertLabel.Text = "";

            if (currentAccount.ReadyToBill)
            {
                bannerAlertLabel.Text += "  Account is flagged ready to bill, or has been billed. Any changes can affect the claim.";
            }

            TotalChargesTextBox.Text = currentAccount.TotalCharges.ToString("c");

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

            PatientFullNameLabel.Text = currentAccount.PatFullName;
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
            SexComboBox.SelectedValue = currentAccount.Sex == null ? string.Empty : currentAccount.Sex;
            SexComboBox.BackColor = Color.White;
            MaritalStatusComboBox.SelectedValue = !string.IsNullOrEmpty(currentAccount.Pat.MaritalStatus) ? currentAccount.Pat.MaritalStatus : "U";
            MaritalStatusComboBox.BackColor = Color.White;

            providerLookup1.SelectedValue = currentAccount.Pat.ProviderId;
            providerLookup1.DisplayValue = currentAccount.Pat.Physician.FullName;
            providerLookup1.BackColor = Color.White;

            GuarantorLastNameTextBox.Text = currentAccount.Pat.GuarantorLastName;
            GuarFirstNameTextBox.Text = currentAccount.Pat.GuarantorFirstName;
            GuarMiddleNameTextBox.Text = currentAccount.Pat.GuarantorMiddleName;
            GuarSuffixTextBox.Text = currentAccount.Pat.GuarantorNameSuffix;
            GuarantorAddressTextBox.Text = currentAccount.Pat.GuarantorAddress;
            GuarCityTextBox.Text = currentAccount.Pat.GuarantorCity;
            GuarStateComboBox.SelectedValue = currentAccount.Pat.GuarantorState != null ? currentAccount.Pat.GuarantorState : "";
            GuarZipTextBox.Text = currentAccount.Pat.GuarantorZipCode;
            GuarantorPhoneTextBox.Text = currentAccount.Pat.GuarantorPrimaryPhone;
            GuarantorRelationComboBox.SelectedValue = currentAccount.Pat.GuarRelationToPatient != null ? currentAccount.Pat.GuarRelationToPatient : "";

            ResetControls(tabDemographics.Controls.OfType<Control>().ToArray());
            ResetControls(tabGuarantor.Controls.OfType<Control>().ToArray());
        }

        private void InsCopyPatientLink_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Log.Instance.Debug($"Copying patient data to insurance holder.");
            //copy patient data for insurance holder info
            HolderLastNameTextBox.Text = LastNameTextBox.Text;
            HolderFirstNameTextBox.Text = FirstNameTextBox.Text;
            HolderMiddleNameTextBox.Text = MiddleNameTextBox.Text;
            HolderAddressTextBox.Text = Address1TextBox.Text;
            HolderCityTextBox.Text = CityTextBox.Text;
            HolderStateComboBox.SelectedValue = StateComboBox.SelectedValue;
            HolderZipTextBox.Text = ZipcodeTextBox.Text;
            HolderDOBTextBox.Text = DateOfBirthTextBox.Text;
            HolderSexComboBox.SelectedValue = SexComboBox.SelectedValue;
            InsRelationComboBox.SelectedValue = "01";
        }

        private void SaveDemographics_Click(object sender, EventArgs e)
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
            currentAccount.Pat.ProviderId = providerLookup1.SelectedValue;

            patRepository.SaveAll(currentAccount.Pat);

            var controls = tabDemographics.Controls; //tabDemographics.Controls;

            foreach (Control control in controls)
            {
                //set background back to white to indicate change has been saved to database
                control.BackColor = Color.White;
            }

            this.LoadAccountData();

        }

        #endregion

        #region InsuranceTab

        private void LoadInsuranceData()
        {
            insGridSource = null;
            InsuranceDataGrid.DataSource = null;
            InsuranceDataGrid.Rows.Clear();
            InsuranceDataGrid.Columns.Clear();

            if (currentAccount.Insurances.Count() > 0)
            {

                DataGridViewButtonColumn deleteCol = new DataGridViewButtonColumn
                {
                    Name = "delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    Width = 50,
                    DisplayIndex = 0
                };
                this.InsuranceDataGrid.Columns.Add(deleteCol);

                var insBindingList = new BindingList<Ins>(currentAccount.Insurances);
                insGridSource = new BindingSource(insBindingList, null);

                InsuranceDataGrid.DataSource = insGridSource;
                InsuranceDataGrid.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);

                InsuranceDataGrid.Columns[nameof(Ins.Coverage)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.PlanName)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.PlanAddress)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.PolicyNumber)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.HolderFullName)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.HolderAddress)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.CertSSN)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.Relation)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.HolderBirthDate)].Visible = true;
                InsuranceDataGrid.Columns[nameof(Ins.HolderSex)].Visible = true;
                InsuranceDataGrid.Columns["delete"].Visible = true;

                int index = 1;
                InsuranceDataGrid.Columns[nameof(Ins.Coverage)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.PlanName)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.PlanAddress)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.PolicyNumber)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.CertSSN)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.HolderFullName)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.HolderAddress)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.Relation)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.HolderBirthDate)].DisplayIndex = index++;
                InsuranceDataGrid.Columns[nameof(Ins.HolderSex)].DisplayIndex = index++;

                InsuranceDataGrid.Columns[nameof(Ins.HolderFullName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                InsuranceDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCellsExceptHeader);
                InsuranceDataGrid.AllowUserToResizeColumns = true;

                InsOrderComboBox.SelectedIndex = 0;
                HolderStateComboBox.SelectedIndex = 0;
                HolderSexComboBox.SelectedIndex = 0;
                PlanFinCodeComboBox.BackColor = Color.Linen;

                InsuranceDataGrid.ClearSelection();
            }
            SetInsDataEntryAccess(false);
            //insAddMode = false;
            AddInsuranceButton.Enabled = true;
            SaveInsuranceButton.Enabled = false;

        }

        private void SaveInsuranceButton_Click(object sender, EventArgs e)
        {
            // saves the insurance info back to the grid
            Log.Instance.Trace($"Entering");

            int selectedIns = -1;

            switch (InsOrderComboBox.SelectedValue)
            {
                case "A":
                    selectedIns = 0;
                    break;
                case "B":
                    selectedIns = 1;
                    break;
                case "C":
                    selectedIns = 2;
                    break;
                default:
                    //Insurance Order is not a valid selection
                    break;
            }
            if (selectedIns < 0)
            {
                MessageBox.Show(this, "Insurance Order is not a valid selection.");
                Log.Instance.Debug("Insurance Order is not a valid selection.");
                return;
            }

            if (currentAccount.Insurances.Count() - 1 < selectedIns)
            {
                //This is a new record
                currentAccount.Insurances.Insert(selectedIns, new Ins());
                currentAccount.Insurances[selectedIns].Account = _selectedAccount;
            }

            currentAccount.Insurances[selectedIns].CertSSN = CertSSNTextBox.Text;
            currentAccount.Insurances[selectedIns].GroupName = GroupNameTextBox.Text;
            currentAccount.Insurances[selectedIns].GroupNumber = GroupNumberTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderStreetAddress = HolderAddressTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderCity = HolderCityTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderState = HolderStateComboBox.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].HolderZip = HolderZipTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderCityStZip = string.Format("{0}, {1} {2}",
                HolderCityTextBox.Text,
                HolderStateComboBox.SelectedValue == null ? "" : HolderStateComboBox.SelectedValue.ToString(),
                HolderZipTextBox.Text);

            if (currentAccount.Insurances[selectedIns].HolderCityStZip.Trim() == ",")
            {
                currentAccount.Insurances[selectedIns].HolderCityStZip = String.Empty;
            }

            if (HolderDOBTextBox.MaskCompleted)
                currentAccount.Insurances[selectedIns].HolderBirthDate = DateTime.Parse(HolderDOBTextBox.Text);

            currentAccount.Insurances[selectedIns].HolderFirstName = HolderFirstNameTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderLastName = HolderLastNameTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderMiddleName = HolderMiddleNameTextBox.Text;
            currentAccount.Insurances[selectedIns].HolderFullName = $"{HolderLastNameTextBox.Text},{HolderFirstNameTextBox.Text} {HolderMiddleNameTextBox.Text}";
            currentAccount.Insurances[selectedIns].HolderSex = HolderSexComboBox.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].PolicyNumber = PolicyNumberTextBox.Text;
            currentAccount.Insurances[selectedIns].PlanStreetAddress1 = PlanAddressTextBox.Text;
            currentAccount.Insurances[selectedIns].PlanStreetAddress2 = PlanAddress2TextBox.Text;
            currentAccount.Insurances[selectedIns].PlanName = PlanNameTextBox.Text;
            currentAccount.Insurances[selectedIns].PlanCityState = PlanCityStTextBox.Text;
            currentAccount.Insurances[selectedIns].Relation = InsRelationComboBox.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].Coverage = InsOrderComboBox.SelectedValue.ToString();
            if (PlanFinCodeComboBox.SelectedValue != null)
                currentAccount.Insurances[selectedIns].FinCode = PlanFinCodeComboBox.SelectedValue.ToString();

            currentAccount.Insurances[selectedIns].InsCode = insurancePlanTextBox.Text; // InsCodeComboBox.SelectedValue.ToString();

            //call method to update the record in the database
            if (currentAccount.Insurances[selectedIns].rowguid == Guid.Empty)
            {
                insRepository.Add(currentAccount.Insurances[selectedIns]);
            }
            else
            {
                if (!InEditMode)
                {
                    if (MessageBox.Show(this, string.Format("You are adding a new {0} insurance {1}. This will replace the existing {2} insurance. OK to update?",
                        InsOrderComboBox.SelectedValue, PlanNameTextBox.Text, InsOrderComboBox.SelectedValue), "Existing Insurance", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        insRepository.Update(currentAccount.Insurances[selectedIns]);
                    }
                    else
                    {
                        return;
                    }
                }
                else
                {
                    List<string> updatedColumns = new List<string>();

                    foreach (Control control in insTabLayoutPanel.Controls)
                    {
                        if (changedControls.Contains(control.Name))
                        {
                            //get field name from map
                            string field = controlColumnMap[control].ToString();
                            if (!string.IsNullOrEmpty(field))
                            {
                                updatedColumns.Add(field);
                            }
                        }
                    }

                    insRepository.Update(currentAccount.Insurances[selectedIns], updatedColumns);
                }

            }

            //insGridSource.ResetBindings(false);
            RefreshAccountData();

            //clear entry fields
            ClearInsEntryFields();
            SaveInsuranceButton.Enabled = false;
        }

        private void ClearInsEntryFields()
        {
            Log.Instance.Trace($"Entering");
            InEditMode = false;
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
            InsOrderComboBox.SelectedIndex = 0;
            PlanFinCodeComboBox.SelectedIndex = -1;

            //disable fields
            SetInsDataEntryAccess(false);
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
            InsOrderComboBox.Enabled = enable;
            PlanFinCodeComboBox.Enabled = enable;
            InsRelationComboBox.Enabled = enable;
            insurancePlanTextBox.Enabled = enable;
        }

        private void InsuranceDataGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            InsTabMessageTextBox.Text = String.Empty;
            if (e.ColumnIndex == 0)
            {
                #region delete button was clicked
                int selectedIns = -1;

                switch (InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.Coverage)].Value.ToString())
                {
                    case "A":
                        selectedIns = 0;
                        break;
                    case "B":
                        selectedIns = 1;
                        break;
                    case "C":
                        selectedIns = 2;
                        break;
                    default:
                        //Insurance Order is not a valid selection
                        break;
                }
                if (selectedIns < 0)
                {
                    MessageBox.Show(this, "Insurance Order is not a valid selection.");
                    Log.Instance.Debug("Insurance Order is not a valid selection.");
                    return;
                }

                if (MessageBox.Show(this, string.Format("Delete {0} insurance {1} for this patient?",
                    currentAccount.Insurances[selectedIns].Coverage,
                    currentAccount.Insurances[selectedIns].PlanName),
                    "Delete Insurance", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    if (insRepository.Delete(currentAccount.Insurances[selectedIns]))
                        currentAccount.Insurances.RemoveAt(selectedIns);
                }
                insGridSource.ResetBindings(false);
                ClearInsEntryFields();

                ResetControls(insTabLayoutPanel.Controls.OfType<Control>().ToArray());
                LoadAccountData();

                return;
                #endregion
            }

            //populate edit boxes
            InEditMode = true;
            HolderStateComboBox.SelectedItem = null;
            HolderStateComboBox.SelectedText = "--Select--";
            HolderSexComboBox.SelectedItem = null;
            HolderSexComboBox.SelectedText = "--Select--";
            InsOrderComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.Coverage)].Value.ToString();

            HolderLastNameTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderLastName)].Value?.ToString(); // ?? lname;
            HolderFirstNameTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderFirstName)].Value?.ToString(); // ?? fname;
            HolderMiddleNameTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderMiddleName)].Value?.ToString(); // ?? mname;
            HolderAddressTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderStreetAddress)].Value != null ? InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderStreetAddress)].Value.ToString() : "";

            HolderCityTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderCity)].Value.ToString();
            HolderStateComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderState)].Value.ToString();
            HolderZipTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderZip)].Value.ToString();

            HolderSexComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderSex)].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()))
                HolderDOBTextBox.Text = DateTime.Parse(InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()).ToString(@"MM/dd/yyyy");
            else
                HolderDOBTextBox.Text = "";
            InsRelationComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.Relation)].Value?.ToString() ?? "";

            insurancePlanTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.InsCode)].Value?.ToString() ?? "";

            PlanNameTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanName)].Value?.ToString();
            PlanAddressTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanStreetAddress1)].Value?.ToString();
            PlanAddress2TextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanStreetAddress2)].Value?.ToString();
            PlanCityStTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanCityState)].Value?.ToString();

            var insurances = currentAccount.Insurances.Where(x => x.Coverage == InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.Coverage)].Value.ToString());

            if (insurances.First().InsCompany.IsGenericPayor)
            {
                PlanNameTextBox.Enabled = true;
                PlanAddressTextBox.Enabled = true;
                PlanAddress2TextBox.Enabled = true;
                PlanCityStTextBox.Enabled = true;
            }

            PolicyNumberTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PolicyNumber)].Value?.ToString();
            GroupNumberTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.GroupNumber)].Value?.ToString();
            GroupNameTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.GroupName)].Value?.ToString();
            CertSSNTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.CertSSN)].Value?.ToString();
            PlanFinCodeComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.FinCode)].Value?.ToString() ?? "";

            //reset changed flag & colors
            foreach (Control ctrl in insTabLayoutPanel.Controls)
            {
                if (ctrl is TextBox || ctrl is ComboBox || ctrl is FlatCombo || ctrl is MaskedTextBox)
                {
                    ctrl.BackColor = Color.White;
                }
                changedControls.Remove(ctrl.Name);
            }
            //enable data entry fields
            SaveInsuranceButton.Enabled = true;
            SetInsDataEntryAccess(true);
            InsOrderComboBox.Enabled = false;
        }

        private void LookupInsCode(string code)
        {
            Log.Instance.Trace($"Entering");
            //lookup code to see if it is valid, then populate other plan fields from data

            if (code == "")
                return;

            var record = insCompanyRepository.GetByCode(code);

            if (record != null)
            {
                //this is a valid code
                PlanNameTextBox.Text = record.PlanName;
                PlanAddressTextBox.Text = record.Address1;
                PlanAddress2TextBox.Text = record.Address2;
                PlanCityStTextBox.Text = record.CityStateZip;
                PlanFinCodeComboBox.SelectedValue = record.FinancialCode ?? String.Empty;

                if (record.IsGenericPayor)
                {
                    PlanNameTextBox.Enabled = true;
                    PlanAddress2TextBox.Enabled = true;
                    PlanAddress2TextBox.Enabled = true;
                    PlanCityStTextBox.Enabled = true;
                    PlanFinCodeComboBox.Enabled = true;
                }

            }
        }

        //bool insAddMode = false;

        private void AddInsuranceButton_Click(object sender, EventArgs e)
        {
            //clear the insurance table selection and data entry fields.
            InsuranceDataGrid.ClearSelection();
            ClearInsEntryFields();
            SetInsDataEntryAccess(true);
            AddInsuranceButton.Enabled = false;
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

            lookupForm.InitialSearchText = insurancePlanTextBox.Text;
            if (lookupForm.ShowDialog() == DialogResult.OK)
            {
                string insCode = insurancePlanTextBox.Text = lookupForm.SelectedValue;
                LookupInsCode(insCode);
            }
        }

        #endregion

        #region ChargeTab
        private void LoadCharges()
        {
            Log.Instance.Trace("Entering");

            var chargesList = currentAccount.Charges;

            chargesTable = Helper.ConvertToDataTable(chargesList);


            ChargesDataGrid.DataSource = chargesTable;
            ChargesDataGrid.DataMember = chargesTable.TableName;
            if (currentAccount.FinCode == "CLIENT")
            {
                chargesTable.DefaultView.Sort = $"{nameof(Chrg.ChrgId)} desc";
            }
            if (!ShowCreditedChrgCheckBox.Checked)
            {
                chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.IsCredited)} = false";
            }

            foreach (DataGridViewColumn col in ChargesDataGrid.Columns)
            {
                col.Visible = false;
            }

            ChargesDataGrid.Columns[nameof(Chrg.IsCredited)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.CDMCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.CdmDescription)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Quantity)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.ServiceDate)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Status)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Comment)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.ChrgId)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Invoice)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.FinCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.ClientMnem)].Visible = true;

            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Format = "N2";
            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ChargesDataGrid.Columns[nameof(Chrg.Quantity)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            ChargesDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            ChargesDataGrid.Columns[nameof(Chrg.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ChargesDataGrid.BackgroundColor = Color.AntiqueWhite;
            ChrgDetailDataGrid.BackgroundColor = Color.AntiqueWhite;

            
            chargeBalRichTextbox.Text = "";
            chargeBalRichTextbox.SelectionFont = new Font(chargeBalRichTextbox.Font.FontFamily, 10, FontStyle.Bold);
            chargeBalRichTextbox.SelectedText = "3rd Party Patient Balance\n";

            chargeBalRichTextbox.AppendText(currentAccount.ClaimBalance.ToString("c") + "\n");

            foreach (var (client, balance) in currentAccount.ClientBalance)
            {
                chargeBalRichTextbox.SelectionFont = new Font(chargeBalRichTextbox.Font.FontFamily, 10, FontStyle.Bold);
                //chargeBalRichTextbox.AppendText($"Client {client} Balance\n");
                chargeBalRichTextbox.SelectedText = $"Client {client} Balance\n";
                chargeBalRichTextbox.AppendText(balance.ToString("c") + "\n");
            }

            ChargesDataGrid.ClearSelection();
            ChrgDetailDataGrid.ClearSelection();

        }

        private void RemoveModifier_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            // get selected charge detail uri
            int selectedRows = ChrgDetailDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChrgDetailDataGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.uri)].Value.ToString());

                chrgDetailRepository.RemoveModifier(uri);
                LoadAccountData();
            }
        }

        private void AddModifier_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            // get selected charge detail uri
            int selectedRows = ChrgDetailDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChrgDetailDataGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.uri)].Value.ToString());

                chrgDetailRepository.AddModifier(uri, item.Text);
                LoadAccountData();
            }
        }

        private void DgvCharges_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {

                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var chrg = chrgRepository.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

                DisplayPOCOForm<Chrg> frm = new DisplayPOCOForm<Chrg>(chrg)
                {
                    Title = "Charge Details"
                };
                frm.Show();
            }
        }

        /// <summary>
        /// Single Click on charge table will display charge details for the clicked row in the
        /// charge details grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargesDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var chrg = chrgRepository.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

                try
                {
                    ChrgDetailDataGrid.DataSource = chrg.ChrgDetails;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Exception {0}", ex.Message));
                }
                foreach (DataGridViewColumn col in ChrgDetailDataGrid.Columns)
                {
                    col.Visible = false;
                }

                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Cpt4)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.BillType)].Visible = true;
                //ChrgDetailDataGrid.Columns[nameof(ChrgDetail.DiagCodePointer)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Modifier)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Modifer2)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.RevenueCode)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Type)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.BillMethod)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.OrderCode)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Amount)].Visible = true;

                ChrgDetailDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Cpt4)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ChrgDetailDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Amount)].DefaultCellStyle.Format = "N2";

            }
        }

        private void ChargesDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.ColumnIndex == ChargesDataGrid.Columns[nameof(Chrg.IsCredited)].Index && e.Value.ToString() == "True")
            {
                ChargesDataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                ChargesDataGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                return;
            }
            if (e.ColumnIndex == ChargesDataGrid.Columns[nameof(Chrg.ChrgId)].Index)
            {
                if (ChargesDataGrid[nameof(Chrg.FinancialType), e.RowIndex].Value.ToString() == "C")
                    ChargesDataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightGreen;

                if (ChargesDataGrid[nameof(Chrg.FinancialType), e.RowIndex].Value.ToString() == "M")
                    ChargesDataGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.LightBlue;

            }

        }

        /// <summary>
        /// Function will credit the charge selected in the charge grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ToolStripCreditCharge_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];

                InputBoxResult prompt = InputBox.Show(string.Format("Credit Charge Number {0}?\nEnter credit reason.",
                    row.Cells[nameof(Chrg.ChrgId)].Value.ToString()),
                    "Credit Charge", "");

                if (prompt.ReturnCode == DialogResult.OK)
                {
                    chrgRepository.CreditCharge(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()), prompt.Text);
                    //reload charge grids to pick up changes
                    //LoadCharges();
                    LoadAccountData();
                }
            }
        }

        private void ShowCreditedChrgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            FilterCharges();
            return;

            if (ShowCreditedChrgCheckBox.Checked)
                chargesTable.DefaultView.RowFilter = String.Empty;
            else
                chargesTable.DefaultView.RowFilter = "IsCredited = false";

        }

        private void AddChargeButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ChargeEntryForm frm = new ChargeEntryForm(currentAccount);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadAccountData();
            }
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
                currentAccount.Status == "CLOSED")
            {
                AddPaymentButton.Enabled = false;
                Label addPaymentStatusLabel = new Label();
                addPaymentStatusLabel.AutoSize = true;
                addPaymentStatusLabel.MaximumSize = new Size(300, 300);
                addPaymentStatusLabel.Text = "Cannot add payment to this account.";
                addPaymentStatusLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                addPaymentStatusLabel.Location = new Point(tabPayments.Right - 310, AddPaymentButton.Bottom + 10);
                tabPayments.Controls.Add(addPaymentStatusLabel);
            }

            currentAccount.Payments = currentAccount.Payments.OrderByDescending(x => x.PaymentNo).ToList();
            PaymentsDataGrid.DataSource = currentAccount.Payments.ToList();

            foreach (DataGridViewColumn col in PaymentsDataGrid.Columns)
            {
                col.Visible = false;
            }

            PaymentsDataGrid.Columns[nameof(Chk.PaidAmount)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.Batch)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.ChkDate)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.CheckNo)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.Comment)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.ContractualAmount)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.DateReceived)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.Invoice)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.Source)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.Status)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.WriteOffAmount)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.WriteOffCode)].Visible = true;
            PaymentsDataGrid.Columns[nameof(Chk.WriteOffDate)].Visible = true;

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

                DisplayPOCOForm<Chk> frm = new DisplayPOCOForm<Chk>(chk)
                {
                    Title = "Payment Details"
                };
                frm.Show();
            }
        }

        private void AddPaymentButton_Click(object sender, EventArgs e)
        {
            PaymentAdjustmentEntryForm form = new PaymentAdjustmentEntryForm(ref currentAccount);

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
                chkRepository.Add(chk);
                LoadAccountData();
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
            ComboBox combo = e.Control as ComboBox;
            if (combo != null)
            {
                combo.SelectedIndexChanged -= dxCombo_SelectedIndexChanged;
                combo.SelectedIndexChanged += new EventHandler(dxCombo_SelectedIndexChanged);
            }
        }

        private void dxPointerGrid2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //You can check for e.ColumnIndex to limit this to your specific column
            if (e.ColumnIndex > 2)
            {
                var editingControl = this.dxPointerGrid2.EditingControl as
                    DataGridViewComboBoxEditingControl;
                if (editingControl != null)
                    editingControl.DroppedDown = true;
            }
        }

        private void dxPointerGrid2_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (!dxLoadingMode)
            {
                //loop through columns
                int cnt = dxBindingList.Count();

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

                    ChrgRepository chrgRepository = new ChrgRepository(Program.AppEnvironment);

                    chrgRepository.UpdateDxPointers(updatedChrg);

                }
            }
        }

        private void dxCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void DxSearchButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (!string.IsNullOrEmpty(txtSearchDx.Text))
            {
                var dictRecords = dictDxRepository.Search(txtSearchDx.Text, currentAccount.TransactionDate);

                DxSearchDataGrid.DataSource = dictRecords;
                DxSearchDataGrid.Columns[nameof(DictDx.mod_date)].Visible = false;
                DxSearchDataGrid.Columns[nameof(DictDx.mod_user)].Visible = false;
                DxSearchDataGrid.Columns[nameof(DictDx.mod_prg)].Visible = false;
                DxSearchDataGrid.Columns[nameof(DictDx.mod_host)].Visible = false;
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
            NotesDisplayTextBox.Text = "";
            NotesDisplayTextBox.BackColor = Color.AntiqueWhite;
            foreach (AccountNote note in currentAccount.Notes)
            {
                NotesDisplayTextBox.DeselectAll();
                NotesDisplayTextBox.SelectionFont = new Font(NotesDisplayTextBox.SelectionFont, FontStyle.Bold);
                NotesDisplayTextBox.AppendText(note.mod_date + " - " + note.mod_user);
                NotesDisplayTextBox.SelectionFont = new Font(NotesDisplayTextBox.SelectionFont, FontStyle.Regular);
                NotesDisplayTextBox.AppendText(Environment.NewLine + note.Comment + Environment.NewLine + Environment.NewLine);
            }
            if (currentAccount.AccountAlert != null)
                noteAlertCheckBox.Checked = currentAccount.AccountAlert.Alert;
        }

        private void AddNoteButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            InputBoxResult prompt = InputBox.Show("Enter note:", "New Note", true);
            AccountNote note = new AccountNote();

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
            BillActivityDataGrid.Columns[nameof(BillingActivity.mod_date)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.mod_host)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.mod_prg)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.mod_user)].Visible = false;
            BillActivityDataGrid.Columns[nameof(BillingActivity.Text)].Visible = false;

            BillActivityDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            //BillActivityDataGrid.Columns[nameof(BillingActivity.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            ValidationResultsTextBox.Text = currentAccount.AccountValidationStatus.validation_text;
            LastValidatedLabel.Text = currentAccount.AccountValidationStatus.mod_date.ToString("G");

            statementFlagComboBox.SelectedItem = currentAccount.Pat.StatementFlag;
            firstStmtDateTextBox.Text = currentAccount.Pat.FirstStatementDate.ToString();
            lastStmtDateTextBox.Text = currentAccount.Pat.LastStatementDate.ToString();
            minPmtTextBox.Text = currentAccount.Pat.MinimumPaymentAmount.ToString();

            readyToBillCheckbox.Checked = currentAccount.ReadyToBill;

            billingTabLoading = false;
        }

        #endregion


        private void PersonSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            PersonSearchForm frm = new PersonSearchForm();
            frm.ShowDialog();
            if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
            {
                _selectedAccount = frm.SelectedAccount;
                LoadAccountData();
            }
            else
            {
                Log.Instance.Error($"Person search returned an empty selected account.");
                MessageBox.Show(this, "A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
            }
        }
        private void ChangeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var result = InputDialogs.SelectDateOfService((DateTime)currentAccount.TransactionDate);

            try
            {
                if (result.newDate != DateTime.MinValue)
                {
                    accountRepository.ChangeDateOfService(ref currentAccount, result.newDate, result.reason);
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
            Log.Instance.Trace($"Entering");

            string newFinCode = InputDialogs.SelectFinancialCode(currentAccount.FinCode);
            if (!string.IsNullOrEmpty(newFinCode))
            {
                try
                {
                    accountRepository.ChangeFinancialClass(ref currentAccount, newFinCode);
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

                LoadAccountData();
            }
        }

        private void ChangeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            ClientLookupForm clientLookupForm = new ClientLookupForm();
            ClientRepository clientRepository = new ClientRepository(Program.AppEnvironment);
            clientLookupForm.Datasource = DataCache.Instance.GetClients();

            if (clientLookupForm.ShowDialog() == DialogResult.OK)
            {
                string newClient = clientLookupForm.SelectedValue;

                try
                {
                    if (accountRepository.ChangeClient(ref currentAccount, newClient))
                    {
                        LoadAccountData();
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

        private void ClearHoldStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (currentAccount.Status != "HOLD")
            {
                MessageBox.Show(this, "Account is not in HOLD status. This will only set account status from HOLD to NEW. It will not change any billing information.");
                return;
            }

            //Reason must be entered for changing status

            InputBoxResult prompt = InputBox.Show("Enter reason for setting status back to New:", "New Note");
            AccountNote note = new AccountNote();

            if (prompt.ReturnCode == DialogResult.OK)
            {
                note.Account = currentAccount.AccountNo;
                note.Comment = prompt.Text;
                accountNoteRepository.Add(note);
                //reload notes to pick up changes
                LoadNotes();
            }

            //AccountRepository accDB = new AccountRepository();
            currentAccount.Status = "NEW";
            accountRepository.Update(currentAccount);
            RefreshAccountData();

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
                LoadAccountData();
                if (!await Task.Run(() => accountRepository.Validate(ref currentAccount)))
                {
                    //has validation errors - do not bill
                    ValidationResultsTextBox.Text = currentAccount.AccountValidationStatus.validation_text;
                    LastValidatedLabel.Text = currentAccount.AccountValidationStatus.mod_date.ToString("G");
                }
                else
                {
                    //ok to bill
                    ValidationResultsTextBox.Text = "No validation errors.";
                    LastValidatedLabel.Text = currentAccount.AccountValidationStatus.mod_date.ToString("G");
                }
            }
            catch (Exception ex)
            {
                ValidationResultsTextBox.Text = $"Exception in validation - report to support. {ex.Message}";
            }
        }

        private void GenerateClaimButton_Click(object sender, EventArgs e)
        {
            ClaimGenerator claimGenerator = new ClaimGenerator(Program.AppEnvironment);

            claimGenerator.CompileClaim(currentAccount.AccountNo);
        }

        private void BillActivityDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            return;
            /*
            DataGridView dgv = (DataGridView)sender;

            // Use HitTest to resolve the row under the cursor
            int rowIndex = dgv.HitTest(e.X, e.Y).RowIndex;

            // If there was no DataGridViewRow under the cursor, return
            if (rowIndex == -1) { return; }

            // Clear all other selections before making a new selection
            dgv.ClearSelection();

            // Select the found DataGridViewRow
            dgv.Rows[rowIndex].Selected = true;

            var claimJson = dgv[nameof(BillingActivity.Text), rowIndex].Value.ToString();
            ClaimData claim;
            try
            {
                claim = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimData>(claimJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Claim data is not in json format.");
                Log.Instance.Error(ex);

                return;
            }

            Billing837 billing837 = new Billing837(Helper.ConnVal, systemParametersRepository.GetProductionEnvironment());

            string fileLocation;

            switch (claim.ClaimType)
            {
                case ClaimType.Institutional:
                    fileLocation = systemParametersRepository.GetByKey("claim_837i_file_location");
                    break;
                case ClaimType.Professional:
                    fileLocation = systemParametersRepository.GetByKey("claim_837p_file_location");
                    break;
                default:
                    throw new ArgumentOutOfRangeException("ClaimType is not defined.");
            }

            //PrintClaimForm printClaim = new PrintClaimForm(Helper.ConnVal);

            //string x12Text = billing837.GenerateSingleClaim(claim, fileLocation);
            //printClaim.Print(x12Text, false, true);

            //printClaim.PrintForm(claim);
            */
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == tabDemographics.Name)
            {
                //load demographics

            }
            if (e.TabPage.Name == summaryTab.Name)
            {
                RefreshAccountData();
            }
            if (e.TabPage.Name == tabDiagnosis.Name)
            {
                DiagnosisDataGrid.BackgroundColor = Color.White;
            }
        }

        private void providerLookup1_SelectedValueChanged(object source, EventArgs args)
        {
            string phy = providerLookup1.SelectedValue;
        }

        #region Guarantor Tab

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

        private void GuarantorSaveButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

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

            var controls = tabGuarantor.Controls;

            foreach (Control control in controls)
            {
                //set background back to white to indicate change has been saved to database
                control.BackColor = Color.White;
            }
            LoadAccountData();
        }

        #endregion


        private void swapInsurancesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AskInsuranceSwapForm frm = new AskInsuranceSwapForm(ref currentAccount);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    accountRepository.InsuranceSwap(currentAccount.AccountNo, InsCoverage.Parse(frm.swap1), InsCoverage.Parse(frm.swap2));
                    LoadAccountData();
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
            //DiagnosisDataGrid.BackgroundColor = Color.Orange;
        }

        private void DiagnosisDataGrid_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {
            //DiagnosisDataGrid.BackgroundColor = Color.Orange;
        }

        private void statementFlagComboBox_SelectionChangeCommitted(object sender, EventArgs e)
        {
            if (currentAccount.Pat.StatementFlag != "N")
            {
                accountRepository.UpdateStatus(currentAccount.AccountNo, "STMT");
            }

            //validate account - if valid, change statement flag. Otherwise, show errors.
            if (accountRepository.Validate(ref currentAccount, true))
            {
                accountRepository.AddNote(currentAccount.AccountNo, $"Statement flag changed from {currentAccount.Pat.StatementFlag} to {statementFlagComboBox.SelectedItem}");

                currentAccount.Pat.StatementFlag = statementFlagComboBox.SelectedItem.ToString();
                patRepository.Update(currentAccount.Pat, new[] { nameof(Pat.StatementFlag) });
                accountRepository.UpdateStatus(currentAccount.AccountNo, "STMT");
            }
            else
            {
                MessageBox.Show("There are validation errors. Resolve before setting statement flag.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            LoadAccountData();
        }

        private void moveChargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];

                PersonSearchForm personSearch = new PersonSearchForm();

                if (personSearch.ShowDialog() == DialogResult.OK)
                {
                    string destAccount = personSearch.SelectedAccount;
                    int chrgId = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value);

                    if (MessageBox.Show($"Move charge {chrgId} ({row.Cells[nameof(Chrg.CdmDescription)].Value}) to account {destAccount}?",
                        "Confirm Move", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        Log.Instance.Debug($"Moving charge {chrgId} from {currentAccount.AccountNo} to {destAccount}");
                        accountRepository.MoveCharge(currentAccount.AccountNo, destAccount, chrgId);
                    }
                    LoadAccountData();
                }
            }
        }

        private void moveAllChargesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            PersonSearchForm personSearch = new PersonSearchForm();

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

                    LoadAccountData();
                }
            }
        }

        private void generateClientStatementToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void dxPointerGrid2_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

            Log.Instance.Error(e.Exception, e.Exception.Message);

            return;

        }

        private void AccountForm_Activated(object sender, EventArgs e)
        {
            LoadAccountData();
        }

        private void readyToBillCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (!billingTabLoading)
            {
                currentAccount.ReadyToBill = readyToBillCheckbox.Checked;
                accountRepository.UpdateStatus(currentAccount.AccountNo, currentAccount.Status);
                accountRepository.AddNote(currentAccount.AccountNo, "Marked ready to bill.");
                accountRepository.Validate(ref currentAccount);
                LoadAccountData();
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

            List<string> args = new List<string>();
            args.AddRange(Helper.GetArgs());

            args.Add(currentAccount.AccountNo);

            PrintEOBForm frm = new PrintEOBForm(args.ToArray());

            frm.ShowDialog(this);
        }

        private void clearClaimStatusButton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Clearing the claim status may result in duplicate claim submissions. Ensure the claim has been deleted in the clearing house system.",
                "Potential Duplicate Submission", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation) == DialogResult.OK)
            {
                accountRepository.ClearClaimStatus(currentAccount);

                LoadAccountData();
            }
        }

        private void chargeDetailsContextMenu_Opening(object sender, CancelEventArgs e)
        {

        }

        private void noteAlertCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (currentAccount.AccountAlert == null)
                currentAccount.AccountAlert = new AccountAlert();
            currentAccount.AccountAlert.AccountNo = currentAccount.AccountNo;
            currentAccount.AccountAlert.Alert = noteAlertCheckBox.Checked;
            bannerAlertLabel.Text = noteAlertCheckBox.Checked ? notesAlertText : "";

            accountRepository.SetNoteAlert(currentAccount.AccountNo, noteAlertCheckBox.Checked);
        }

        private void FilterCharges()
        {

            chargesTable.DefaultView.RowFilter = string.Empty;

            if (show3rdPartyRadioButton.Checked)
                chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.FinancialType)} = 'M'";

            if (showClientRadioButton.Checked)
                chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.FinancialType)} = 'C'";

            if (showAllChargeRadioButton.Checked)
                chargesTable.DefaultView.RowFilter = String.Empty;

            if (!ShowCreditedChrgCheckBox.Checked)
            {
                if (chargesTable.DefaultView.RowFilter == string.Empty)
                    chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.IsCredited)} = false";
                else
                    chargesTable.DefaultView.RowFilter += $"and {nameof(Chrg.IsCredited)} = false";
            }
        }

        private void show3rdPartyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FilterCharges();
        }
    }
}
