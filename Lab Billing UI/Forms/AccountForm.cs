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

namespace LabBilling.Forms
{
    public partial class AccountForm : Form
    {
        private BindingList<PatDiag> dxBindingList;
        private DataTable chargesTable = new DataTable();

        private List<InsCompany> insCompanies = null;
        private List<Phy> providers = null;
        private Account currentAccount = null;
        private BindingSource insGridSource = null;
        private bool InEditMode = false;
        private List<string> changedControls = new List<string>();
        private Dictionary<Control, string> controlColumnMap = new Dictionary<Control, string>();

        private readonly InsRepository insDB = new InsRepository(Helper.ConnVal);
        private readonly AccountRepository accDB = new AccountRepository(Helper.ConnVal);
        private readonly PatRepository patDB = new PatRepository(Helper.ConnVal);
        private readonly DictDxRepository dxDB = new DictDxRepository(Helper.ConnVal);
        private readonly InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        private readonly ChrgRepository chrgdb = new ChrgRepository(Helper.ConnVal);
        private readonly UserProfileRepository userProfileDB = new UserProfileRepository(Helper.ConnVal);
        private readonly FinRepository finDB = new FinRepository(Helper.ConnVal);
        private readonly ChkRepository chkdb = new ChkRepository(Helper.ConnVal);
        private readonly AccountNoteRepository notesdb = new AccountNoteRepository(Helper.ConnVal);
        private readonly BillingActivityRepository dbBillingActivity = new BillingActivityRepository(Helper.ConnVal);
        private readonly DictDxRepository dictDxDb = new DictDxRepository(Helper.ConnVal);
        private readonly SystemParametersRepository systemParametersRepository = new SystemParametersRepository(Helper.ConnVal);
        private readonly PhyRepository phyRepository = new PhyRepository(Helper.ConnVal);

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
                _selectedAccount = account;

            if (parentForm != null)
                this.MdiParent = parentForm;
        }

        public AccountForm()
        {
            Log.Instance.Trace("Entering");
            InitializeComponent();
        }

        #region MainForm

        private void AccountForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");


            providerSearchListBox.Visible = false;
            tabDemographics.Controls.Add(providerSearchListBox);

            #region Process permissions and enable controls

            Helper.SetControlsAccess(tabCharges.Controls, false);
            if (systemParametersRepository.GetByKey("allow_chrg_entry") == "1")
            {
                if (Program.LoggedInUser.CanSubmitCharges)
                {
                    Helper.SetControlsAccess(tabCharges.Controls, true);
                }
            }

            Helper.SetControlsAccess(tabPayments.Controls, false);
            if (systemParametersRepository.GetByKey("allow_chk_entry") == "1")
            {
                if (Program.LoggedInUser.CanAddPayments)
                {
                    Helper.SetControlsAccess(tabPayments.Controls, false);
                }
            }

            //Helper.SetControlsAccess(DemographicsTabLayoutPanel.Controls, false);
            Helper.SetControlsAccess(tabDemographics.Controls, false);
            Helper.SetControlsAccess(tabInsurance.Controls, false);
            Helper.SetControlsAccess(insTabLayoutPanel.Controls, false);
            Helper.SetControlsAccess(tabDiagnosis.Controls, false);
            Helper.SetControlsAccess(tabGuarantor.Controls, false);
            Helper.SetControlsAccess(tabNotes.Controls, false);
            Helper.SetControlsAccess(tabCharges.Controls, false);
            Helper.SetControlsAccess(tabPayments.Controls, false);
            GuarCopyPatientLink.Enabled = false;
            InsCopyPatientLink.Enabled = false;
            changeClientToolStripMenuItem.Enabled = false;
            changeDateOfServiceToolStripMenuItem.Enabled = false;
            changeFinancialClassToolStripMenuItem.Enabled = false;
            clearHoldStatusToolStripMenuItem.Enabled = false;
            if (Convert.ToBoolean(systemParametersRepository.GetByKey("allow_edit")))
            {
                if (Program.LoggedInUser.Access == "ENTER/EDIT")
                {
                    Helper.SetControlsAccess(tabDemographics.Controls, true);
                    Helper.SetControlsAccess(tabInsurance.Controls, true);
                    Helper.SetControlsAccess(insTabLayoutPanel.Controls, true);
                    Helper.SetControlsAccess(tabDiagnosis.Controls, true);
                    Helper.SetControlsAccess(tabGuarantor.Controls, true);
                    Helper.SetControlsAccess(tabNotes.Controls, true);
                    Helper.SetControlsAccess(tabCharges.Controls, true);
                    Helper.SetControlsAccess(tabPayments.Controls, true);
                    GuarCopyPatientLink.Enabled = true;
                    InsCopyPatientLink.Enabled = true;
                    changeClientToolStripMenuItem.Enabled = true;
                    changeDateOfServiceToolStripMenuItem.Enabled = true;
                    changeFinancialClassToolStripMenuItem.Enabled = true;
                    clearHoldStatusToolStripMenuItem.Enabled = true;
                }
            }

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
            controlColumnMap.Add(DateOfBirthTextBox, nameof(Pat.BirthDate));
            controlColumnMap.Add(PhoneTextBox, nameof(Pat.PrimaryPhone));
            controlColumnMap.Add(CityTextBox, nameof(Pat.City));
            controlColumnMap.Add(Address2TextBox, nameof(Pat.Address2));
            controlColumnMap.Add(SexComboBox, nameof(Pat.Sex));
            controlColumnMap.Add(Address1TextBox, nameof(Pat.Address1));
            controlColumnMap.Add(GuarZipTextBox, nameof(Pat.GuarantorZipCode));
            controlColumnMap.Add(PlanFinCodeComboBox, nameof(Ins.FinCode));
            controlColumnMap.Add(CertSSNTextBox, nameof(Ins.CertSSN));
            controlColumnMap.Add(HolderLastNameTextBox, nameof(Ins.HolderLastName));
            controlColumnMap.Add(GroupNameTextBox, nameof(Ins.GroupName));
            controlColumnMap.Add(InsCodeComboBox, nameof(Ins.InsCode));
            controlColumnMap.Add(HolderZipTextBox, nameof(Ins.HolderZip));
            controlColumnMap.Add(GroupNumberTextBox, nameof(Ins.GroupNumber));
            controlColumnMap.Add(PlanAddress2TextBox, nameof(Ins.PlanAddress2));
            controlColumnMap.Add(InsRelationComboBox, nameof(Ins.Relation));
            controlColumnMap.Add(PolicyNumberTextBox, nameof(Ins.PolicyNumber));
            controlColumnMap.Add(HolderDOBTextBox, nameof(Ins.HolderBirthDate));
            controlColumnMap.Add(HolderStateComboBox, nameof(Ins.HolderState));
            controlColumnMap.Add(HolderFirstNameTextBox, nameof(Ins.HolderFirstName));
            controlColumnMap.Add(InsOrderComboBox, nameof(Ins.Coverage));
            controlColumnMap.Add(HolderSexComboBox, nameof(Ins.HolderSex));
            controlColumnMap.Add(HolderMiddleNameTextBox, nameof(Ins.HolderMiddleName));
            controlColumnMap.Add(PlanNameTextBox, nameof(Ins.PlanName));
            controlColumnMap.Add(HolderAddressTextBox, nameof(Ins.HolderAddress));
            controlColumnMap.Add(PlanCityStTextBox, nameof(Ins.PlanCityState));
            controlColumnMap.Add(PlanAddressTextBox, nameof(Ins.PlanAddress1));
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

            DataTable inscDataTable = new DataTable(typeof(InsCompany).Name);
            inscDataTable.Columns.Add("Code");
            inscDataTable.Columns.Add("Name");
            var values = new object[2];
            // add no selection row
            values[0] = "";
            values[1] = "<select plan>";
            inscDataTable.Rows.Add(values);
            foreach (InsCompany insc in insCompanies)
            {
                values[0] = insc.InsuranceCode;
                values[1] = insc.PlanName;
                inscDataTable.Rows.Add(values);
            }

            InsCodeComboBox.DataSource = inscDataTable;
            InsCodeComboBox.DisplayMember = "Name";
            InsCodeComboBox.ValueMember = "Code";

            #endregion

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
            PlanFinCodeComboBox.DisplayMember = nameof(Fin.res_party);
            PlanFinCodeComboBox.ValueMember = nameof(Fin.fin_code);
            PlanFinCodeComboBox.SelectedIndex = -1;

            #endregion

            if (SelectedAccount != null || SelectedAccount != "")
            {
                Log.Instance.Debug($"Loading account data for {SelectedAccount}");
                userProfileDB.InsertRecentAccount(SelectedAccount, Program.LoggedInUser.UserName);
                LoadAccountData();

                AddOnChangeHandlerToInputControls(tabDemographics);
                AddOnChangeHandlerToInputControls(tabGuarantor);
                AddOnChangeHandlerToInputControls(tabInsurance);
            }

            UpdateDxPointersButton.Enabled = false; // start disabled until a box has been checked.
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

        #endregion

        private void LoadAccountData()
        {
            Log.Instance.Trace($"Entering");
            currentAccount = accDB.GetByAccount(SelectedAccount);
            this.Text = $"{currentAccount.PatFullName}";

            dxBindingList = new BindingList<PatDiag>(currentAccount.Pat.Diagnoses);
            ShowCreditedChrgCheckBox.Checked = false;
            LoadSummaryTab();
            LoadDemographics();
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
            List<SummaryData> sd = new List<SummaryData>
            {
                new SummaryData("Demographics","",SummaryData.GroupType.Demographics,1,1,true),
                new SummaryData("Account", SelectedAccount, SummaryData.GroupType.Demographics,2,1),
                new SummaryData("EMR Account", currentAccount.MeditechAccount, SummaryData.GroupType.Demographics,3,1),
                new SummaryData("Status", currentAccount.Status,SummaryData.GroupType.Demographics,4,1),
                new SummaryData("MRN", currentAccount.MRN, SummaryData.GroupType.Demographics,5,1),
                new SummaryData("Client", currentAccount.ClientName, SummaryData.GroupType.Demographics,7,1),
                new SummaryData("Ordering Provider", currentAccount.Pat.Physician.FullName, SummaryData.GroupType.Demographics, 8, 1),
                new SummaryData("Address", currentAccount.Pat.AddressLine, SummaryData.GroupType.Demographics,10,1),
                new SummaryData("Phone", currentAccount.Pat.PrimaryPhone.FormatPhone(), SummaryData.GroupType.Demographics,11,1),
                new SummaryData("Email", currentAccount.Pat.EmailAddress, SummaryData.GroupType.Demographics,12,1),

                new SummaryData("Financial","",SummaryData.GroupType.Financial,1,2,true),
                new SummaryData("Financial Class", currentAccount.FinCode, SummaryData.GroupType.Financial,2,2),
                new SummaryData("Date of Service", currentAccount.TransactionDate?.ToShortDateString(), SummaryData.GroupType.Financial,3,2),
                new SummaryData("Total Charges", currentAccount.TotalCharges.ToString("c"),SummaryData.GroupType.Financial,4,2),
                new SummaryData("Total Payments", (currentAccount.TotalPayments+currentAccount.TotalContractual+currentAccount.TotalWriteOff).ToString("c"),
                    SummaryData.GroupType.Financial,5,2),
                new SummaryData("Balance", currentAccount.Balance.ToString("c"), SummaryData.GroupType.Financial,6,2)
            };
            //this data is not relevant if this is a CLIENT account
            if (currentAccount.FinCode != "CLIENT")
            {
                sd.Add(new SummaryData("SSN", currentAccount.SocSecNo.FormatSSN(), SummaryData.GroupType.Demographics, 6, 1));
                sd.Add(new SummaryData("DOB/Sex", currentAccount.Pat.DOBSex, SummaryData.GroupType.Demographics, 9, 1));
                sd.Add(new SummaryData("Diagnoses", "", SummaryData.GroupType.Diagnoses, 13, 1, true));
                sd.Add(new SummaryData(currentAccount.Pat.Dx1, currentAccount.Pat.Dx1Desc, SummaryData.GroupType.Diagnoses, 14, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx2, currentAccount.Pat.Dx2Desc, SummaryData.GroupType.Diagnoses, 15, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx3, currentAccount.Pat.Dx3Desc, SummaryData.GroupType.Diagnoses, 16, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx4, currentAccount.Pat.Dx4Desc, SummaryData.GroupType.Diagnoses, 17, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx5, currentAccount.Pat.Dx5Desc, SummaryData.GroupType.Diagnoses, 18, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx6, currentAccount.Pat.Dx6Desc, SummaryData.GroupType.Diagnoses, 19, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx7, currentAccount.Pat.Dx7Desc, SummaryData.GroupType.Diagnoses, 20, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx8, currentAccount.Pat.Dx8Desc, SummaryData.GroupType.Diagnoses, 21, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx9, currentAccount.Pat.Dx9Desc, SummaryData.GroupType.Diagnoses, 22, 1));

                foreach (Ins ins in currentAccount.Insurances)
                {
                    if (ins.Coverage == "A")
                    {
                        sd.Add(new SummaryData("Primary Insurance", "", SummaryData.GroupType.Insurance, 7, 2, true));
                        sd.Add(new SummaryData("Holder", ins.HolderName, SummaryData.GroupType.Insurance, 8, 2));
                        sd.Add(new SummaryData("Insurance", ins.PlanName, SummaryData.GroupType.Insurance, 9, 2));
                        sd.Add(new SummaryData("Policy", ins.PolicyNumber, SummaryData.GroupType.Insurance, 10, 2));
                        sd.Add(new SummaryData("Group #", ins.GroupNumber, SummaryData.GroupType.Insurance, 11, 2));
                        sd.Add(new SummaryData("Group", ins.GroupName, SummaryData.GroupType.Insurance, 12, 2));
                    }
                    if (ins.Coverage == "B")
                    {
                        sd.Add(new SummaryData("Secondary Insurance", "", SummaryData.GroupType.Insurance, 13, 2, true));
                        sd.Add(new SummaryData("Holder", ins.HolderName, SummaryData.GroupType.Insurance, 14, 2));
                        sd.Add(new SummaryData("Insurance", ins.PlanName, SummaryData.GroupType.Insurance, 15, 2));
                        sd.Add(new SummaryData("Policy", ins.PolicyNumber, SummaryData.GroupType.Insurance, 16, 2));
                        sd.Add(new SummaryData("Group #", ins.GroupNumber, SummaryData.GroupType.Insurance, 17, 2));
                        sd.Add(new SummaryData("Group", ins.GroupName, SummaryData.GroupType.Insurance, 18, 2));
                    }
                    if (ins.Coverage == "C")
                    {
                        sd.Add(new SummaryData("Tertiary Insurance", "", SummaryData.GroupType.Insurance, 19, 2, true));
                        sd.Add(new SummaryData("Holder", ins.HolderName, SummaryData.GroupType.Insurance, 20, 2));
                        sd.Add(new SummaryData("Insurance", ins.PlanName, SummaryData.GroupType.Insurance, 21, 2));
                        sd.Add(new SummaryData("Policy", ins.PolicyNumber, SummaryData.GroupType.Insurance, 22, 2));
                        sd.Add(new SummaryData("Group #", ins.GroupNumber, SummaryData.GroupType.Insurance, 23, 2));
                        sd.Add(new SummaryData("Group", ins.GroupName, SummaryData.GroupType.Insurance, 24, 2));
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

        private void LoadDemographics()
        {
            Log.Instance.Trace("Entering");
            DemoStatusMessagesTextBox.Text = String.Empty;

            BannerNameTextBox.Text = currentAccount.PatFullName;
            BannerDobTextBox.Text = currentAccount.Pat.BirthDate.GetValueOrDefault().ToShortDateString();
            BannerSexTextBox.Text = currentAccount.Pat.Sex;
            BannerAccountTextBox.Text = SelectedAccount;
            BannerMRNTextBox.Text = currentAccount.MRN;
            BannerClientTextBox.Text = currentAccount.ClientName;
            BannerFinClassTextBox.Text = currentAccount.FinCode;
            BannerBillStatusTextBox.Text = currentAccount.Status;
            BannerProviderTextBox.Text = currentAccount.Pat.Physician.FullName;

            TotalChargesTextBox.Text = currentAccount.TotalCharges.ToString("c");

            //this.Text = $" {currentAccount.pat_name} - Demographics";

            BannerNameTextBox.Text = currentAccount.PatFullName;
            BannerAccountTextBox.Text = _selectedAccount;
            BannerDobTextBox.Text = currentAccount.Pat.BirthDate.Value.ToShortDateString();
            BannerSexTextBox.Text = currentAccount.Pat.Sex;
            BannerMRNTextBox.Text = currentAccount.MRN;

            TotalChargesLabel.Text = currentAccount.TotalCharges.ToString("c");
            TotalPmtAdjLabel.Text = (currentAccount.TotalContractual + currentAccount.TotalPayments + currentAccount.TotalWriteOff).ToString("c");
            BalanceLabel.Text = currentAccount.Balance.ToString("c");

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
            StateComboBox.SelectedValue = currentAccount.Pat.State;
            StateComboBox.BackColor = Color.White;
            ZipcodeTextBox.Text = currentAccount.Pat.ZipCode;
            ZipcodeTextBox.BackColor = Color.White;
            PhoneTextBox.Text = currentAccount.Pat.PrimaryPhone;
            PhoneTextBox.BackColor = Color.White;
            SocSecNoTextBox.Text = currentAccount.SocSecNo;
            SocSecNoTextBox.BackColor = Color.White;
            EmailAddressTextBox.Text = currentAccount.Pat.EmailAddress;
            EmailAddressTextBox.BackColor = Color.White;
            DateOfBirthTextBox.Text = currentAccount.Pat.BirthDate.Value.ToString("MM/dd/yyyy");
            DateOfBirthTextBox.BackColor = Color.White;
            SexComboBox.SelectedValue = currentAccount.Pat.Sex;
            SexComboBox.BackColor = Color.White;
            MaritalStatusComboBox.SelectedValue = currentAccount.Pat.MaritalStatus != null ? currentAccount.Pat.MaritalStatus : "";
            MaritalStatusComboBox.BackColor = Color.White;

            providerLookup1.SelectedValue = currentAccount.Pat.ProviderId;
            providerLookup1.DisplayValue = currentAccount.Pat.Physician.FullName;

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

            if (currentAccount.Insurances.Count() > 0)
            {
                InsuranceDataGrid.Rows.Clear();
                InsuranceDataGrid.Columns.Clear();
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
                InsuranceDataGrid.Columns[nameof(Ins.mod_prg)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.mod_user)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.mod_date)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.mod_host)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.Employer)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.EmployerCityState)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.HolderLastName)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.HolderFirstName)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.HolderMiddleName)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.PlanEffectiveDate)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.PlanExpirationDate)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.rowguid)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.Account)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.InsCompany)].Visible = false;
                InsuranceDataGrid.Columns[nameof(Ins.IsDeleted)].Visible = false;

                InsuranceDataGrid.Columns[nameof(Ins.PlanName)].DisplayIndex = 1;
                InsuranceDataGrid.Columns[nameof(Ins.PlanAddress1)].DisplayIndex = 2;
                InsuranceDataGrid.Columns[nameof(Ins.PolicyNumber)].DisplayIndex = 3;

                InsuranceDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                //dgvInsurance.Columns[nameof(Ins.HolderName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                InsOrderComboBox.SelectedIndex = 0;
                HolderStateComboBox.SelectedIndex = 0;
                HolderSexComboBox.SelectedIndex = 0;

                InsuranceDataGrid.ClearSelection();
                SetInsDataEntryAccess(false);
                ResetControls(tabDemographics.Controls.OfType<Control>().ToArray());
            }

        }

        #region DemographicTab

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
            currentAccount.SocSecNo = SocSecNoTextBox.Text;

            accDB.Update(currentAccount);

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
            currentAccount.Pat.BirthDate = DateTime.Parse(DateOfBirthTextBox.Text);
            currentAccount.Pat.Sex = SexComboBox.SelectedValue.ToString();
            currentAccount.Pat.ProviderId = providerLookup1.SelectedValue;

            //currentAccount.Pat.SocSecNo = tbSSN.Text;

            patDB.SaveAll(currentAccount.Pat);

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

        private void SaveInsuranceButton_Click_1(object sender, EventArgs e)
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
                MetroMessageBox.Show(this, "Insurance Order is not a valid selection.");
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
            currentAccount.Insurances[selectedIns].HolderAddress = HolderAddressTextBox.Text;
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
            currentAccount.Insurances[selectedIns].HolderName = $"{HolderLastNameTextBox.Text},{HolderFirstNameTextBox.Text} {HolderMiddleNameTextBox.Text}";
            currentAccount.Insurances[selectedIns].HolderSex = HolderSexComboBox.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].PolicyNumber = PolicyNumberTextBox.Text;
            currentAccount.Insurances[selectedIns].PlanAddress1 = PlanAddressTextBox.Text;
            currentAccount.Insurances[selectedIns].PlanAddress2 = PlanAddress2TextBox.Text;
            currentAccount.Insurances[selectedIns].PlanName = PlanNameTextBox.Text;
            currentAccount.Insurances[selectedIns].PlanCityState = PlanCityStTextBox.Text;
            currentAccount.Insurances[selectedIns].Relation = InsRelationComboBox.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].Coverage = InsOrderComboBox.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].FinCode = PlanFinCodeComboBox.SelectedValue.ToString();

            currentAccount.Insurances[selectedIns].InsCode = InsCodeComboBox.SelectedValue.ToString();

            //call method to update the record in the database
            if (currentAccount.Insurances[selectedIns].rowguid == Guid.Empty)
            {
                insDB.Add(currentAccount.Insurances[selectedIns]);
            }
            else
            {
                if (!InEditMode)
                {
                    if (MetroMessageBox.Show(this, string.Format("You are adding a new {0} insurance {1}. This will replace the existing {2} insurance. OK to update?",
                        InsOrderComboBox.SelectedValue, PlanNameTextBox.Text, InsOrderComboBox.SelectedValue), "Existing Insurance", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        insDB.Update(currentAccount.Insurances[selectedIns]);
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

                    insDB.Update(currentAccount.Insurances[selectedIns], updatedColumns);
                }

            }

            insGridSource.ResetBindings(false);

            //clear entry fields
            ClearInsEntryFields();

        }

        private void ClearInsEntryFields()
        {
            Log.Instance.Trace($"Entering");
            InEditMode = false;
            PlanAddressTextBox.Text = "";
            PlanAddress2TextBox.Text = "";
            PlanCityStTextBox.Text = "";
            PlanNameTextBox.Text = "";
            HolderAddressTextBox.Text = "";
            HolderCityTextBox.Text = "";
            HolderDOBTextBox.Text = "";
            HolderFirstNameTextBox.Text = "";
            HolderLastNameTextBox.Text = "";
            HolderMiddleNameTextBox.Text = "";
            HolderZipTextBox.Text = "";
            PolicyNumberTextBox.Text = "";
            GroupNameTextBox.Text = "";
            GroupNumberTextBox.Text = "";
            CertSSNTextBox.Text = "";
            HolderSexComboBox.SelectedIndex = 0;
            HolderStateComboBox.SelectedIndex = 0;
            InsCodeComboBox.SelectedIndex = 0;
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
            InsCodeComboBox.Enabled = enable;
            InsOrderComboBox.Enabled = enable;
            PlanFinCodeComboBox.Enabled = enable;
            InsRelationComboBox.Enabled = enable;
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
                    MetroMessageBox.Show(this, "Insurance Order is not a valid selection.");
                    Log.Instance.Debug("Insurance Order is not a valid selection.");
                    return;
                }

                if (MetroMessageBox.Show(this, string.Format("Delete {0} insurance {1} for this patient?",
                    currentAccount.Insurances[selectedIns].Coverage,
                    currentAccount.Insurances[selectedIns].PlanName),
                    "Delete Insurance", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    if (insDB.Delete(currentAccount.Insurances[selectedIns]))
                        currentAccount.Insurances.RemoveAt(selectedIns);
                }
                insGridSource.ResetBindings(false);
                ClearInsEntryFields();

                ResetControls(insTabLayoutPanel.Controls.OfType<Control>().ToArray());

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
            HolderAddressTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderAddress)].Value != null ? InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderAddress)].Value.ToString() : "";

            HolderCityTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderCity)].Value.ToString();
            HolderStateComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderState)].Value.ToString();
            HolderZipTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderZip)].Value.ToString();

            HolderSexComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderSex)].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()))
                HolderDOBTextBox.Text = DateTime.Parse(InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()).ToString(@"MM/dd/yyyy");
            else
                HolderDOBTextBox.Text = "";
            InsRelationComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.Relation)].Value?.ToString() ?? "";

            // set ins Code combo box
            InsCodeComboBox.SelectedValue = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.InsCode)].Value?.ToString() ?? "";

            PlanNameTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanName)].Value?.ToString();
            PlanAddressTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanAddress1)].Value?.ToString();
            PlanAddress2TextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanAddress2)].Value?.ToString();
            PlanCityStTextBox.Text = InsuranceDataGrid.SelectedRows[0].Cells[nameof(Ins.PlanCityState)].Value?.ToString();
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
            SetInsDataEntryAccess(true);
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
                PlanFinCodeComboBox.SelectedValue = record.FinancialCode;
            }
        }

        private void AddInsuranceButton_Click(object sender, EventArgs e)
        {
            //clear the insurance table selection and data entry fields.
            InsuranceDataGrid.ClearSelection();
            ClearInsEntryFields();
        }

        private void InsCodeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (InsCodeComboBox.SelectedIndex >= 0)
            {
                string insCode = InsCodeComboBox.SelectedValue.ToString();

                LookupInsCode(insCode);

                if (insCode == "MISC")
                {
                    PlanNameTextBox.ReadOnly = false;
                    PlanAddressTextBox.ReadOnly = false;
                    PlanAddress2TextBox.ReadOnly = false;
                    PlanCityStTextBox.ReadOnly = false;
                    PlanFinCodeComboBox.Enabled = true;
                }
                else
                {
                    PlanNameTextBox.ReadOnly = true;
                    PlanAddressTextBox.ReadOnly = true;
                    PlanAddress2TextBox.ReadOnly = true;
                    PlanCityStTextBox.ReadOnly = true;
                    PlanFinCodeComboBox.Enabled = false;
                }
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
            if (!ShowCreditedChrgCheckBox.Checked)
            {
                chargesTable.DefaultView.RowFilter = "IsCredited = false";
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

            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Format = "N2";
            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ChargesDataGrid.Columns[nameof(Chrg.CalculatedAmount)].DefaultCellStyle.Format = "N2";
            ChargesDataGrid.Columns[nameof(Chrg.CalculatedAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ChargesDataGrid.Columns[nameof(Chrg.Quantity)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            ChargesDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            ChargesDataGrid.Columns[nameof(Chrg.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ChargesDataGrid.BackgroundColor = Color.AntiqueWhite;
            ChrgDetailDataGrid.BackgroundColor = Color.AntiqueWhite;

        }

        private void DgvCharges_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {

                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var chrg = chrgdb.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

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
                var chrg = chrgdb.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

                try
                {
                    ChrgDetailDataGrid.DataSource = chrg.ChrgDetails;
                }
                catch (Exception ex)
                {
                    MetroMessageBox.Show(this, string.Format("Exception {0}", ex.Message));
                }
                foreach (DataGridViewColumn col in ChrgDetailDataGrid.Columns)
                {
                    col.Visible = false;
                }

                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Cpt4)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.BillType)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.DiagCodePointer)].Visible = true;
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
                    chrgdb.CreditCharge(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()), prompt.Text);
                    //reload charge grids to pick up changes
                    LoadCharges();
                }
            }
        }

        private void ShowCreditedChrgCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
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

        private void ChrgDetailDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
        }

        private void ChrgDetailDataGrid_SelectionChanged(object sender, EventArgs e)
        {
            if (UpdateDxPointersButton.Enabled == true)
            {
                if (MetroMessageBox.Show(this, "Changes were made to diagnosis pointers. Save changes?", "Save Changes?",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    == DialogResult.Yes)
                {
                    UpdateDxPointersButton_Click(sender, e);
                }
                UpdateDxPointersButton.Enabled = false;
            }

            Log.Instance.Trace($"Entering");

            //load dx pointers
            int selectedRows = ChrgDetailDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChrgDetailDataGrid.SelectedRows[0];

                string dxPtr = row.Cells[nameof(ChrgDetail.DiagCodePointer)].Value.ToString();

                string[] ptrs = dxPtr.Split(':');

                DataTable dt = new DataTable("DxPointer");
                dt.Columns.Add("DxNo", typeof(int));
                dt.Columns.Add("DxCode", typeof(string));
                dt.Columns.Add("Ptr1", typeof(bool));
                dt.Columns.Add("Ptr2", typeof(bool));
                dt.Columns.Add("Ptr3", typeof(bool));
                dt.Columns.Add("Ptr4", typeof(bool));

                try
                {
                    int iDx = 1;
                    if (currentAccount.Pat.Diagnoses.Count > 0)
                    {
                        foreach (PatDiag diag in currentAccount.Pat.Diagnoses)
                        {
                            dt.Rows.Add(new object[] { iDx++, diag.Code, false, false, false, false });
                        }

                        int i = 1;
                        foreach (string ptr in ptrs)
                        {
                            if (ptr == null || ptr == "")
                                continue;
                            int iPtr = Convert.ToInt32(ptr);
                            switch (i)
                            {
                                case 1:
                                    dt.Rows[iPtr - 1]["Ptr1"] = true;
                                    break;
                                case 2:
                                    dt.Rows[iPtr - 1]["Ptr2"] = true;
                                    break;
                                case 3:
                                    dt.Rows[iPtr - 1]["Ptr3"] = true;
                                    break;
                                case 4:
                                    dt.Rows[iPtr - 1]["Ptr4"] = true;
                                    break;
                                default:
                                    break;
                            }
                            i++;
                        }
                    }
                    DiagnosisPointerDataGrid.DataSource = dt;
                    DiagnosisPointerDataGrid.Columns["DxCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    DiagnosisPointerDataGrid.AutoResizeColumns();
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex.Message);
                    MetroMessageBox.Show(this, "Error loading Diagnosis Code Pointer. Exception has been logged. Report to Administrator.");
                }
            }
        }

        private void UpdateDxPointersButton_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            for (int c = 2; c < DiagnosisPointerDataGrid.ColumnCount; c++)
            {
                for (int r = 0; r < DiagnosisPointerDataGrid.RowCount; r++)
                {
                    if ((bool)DiagnosisPointerDataGrid.Rows[r].Cells[c].Value == true)
                    {
                        sb.Append(DiagnosisPointerDataGrid.Rows[r].Cells[0].Value.ToString() + ":");
                    }
                }
            }

            string dxPtr = sb.ToString();


            //update amt record

            ChrgDetail amt = new ChrgDetail();

            amt.uri = Convert.ToInt32(ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.uri)].Value);
            amt.Amount = Convert.ToDouble(ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.Amount)].Value ?? 0.0);
            amt.BillMethod = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.BillMethod)].Value?.ToString();
            amt.BillType = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.BillType)].Value?.ToString();
            amt.ChrgNo = Convert.ToInt32(ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.ChrgNo)].Value ?? 0.0);
            amt.Cpt4 = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.Cpt4)].Value?.ToString();
            amt.DiagCodePointer = dxPtr;
            amt.Modifier = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.Modifier)].Value?.ToString();
            amt.Modifer2 = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.Modifer2)].Value?.ToString();
            amt.LISReqNo = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.LISReqNo)].Value?.ToString();
            amt.OrderCode = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.OrderCode)].Value?.ToString();
            amt.PointerSet = true;
            amt.RevenueCode = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.RevenueCode)].Value?.ToString();
            amt.Type = ChrgDetailDataGrid.SelectedRows[0].Cells[nameof(amt.Type)].Value?.ToString();

            ChrgDetailRepository amtRepository = new ChrgDetailRepository(Helper.ConnVal);
            try
            {
                amtRepository.Update(amt);
            }
            catch (Exception ex)
            {
                Log.Instance.Error(ex.Message);
                MetroMessageBox.Show(this, "Error updating charge detail record. Please try again. If error continues, report error to Administrator.");
                return;
            }

            UpdateDxPointersButton.Enabled = false;
            LoadCharges();
            ChrgDetailDataGrid.DataSource = null;
        }

        private void DiagnosisPointerDataGrid_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DiagnosisPointerDataGrid.EndEdit();

            if (e.ColumnIndex > 1)
            {
                UpdateDxPointersButton.Enabled = true;

                DataGridViewCheckBoxCell cbCell = DiagnosisPointerDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                bool checkedValue = (bool)cbCell.Value;

                if (checkedValue == true)
                {
                    //loop through the column entries and ensure only one is checked
                    for (int i = 0; i < DiagnosisPointerDataGrid.RowCount; i++)
                    {
                        if (i != e.RowIndex)
                        {
                            DiagnosisPointerDataGrid.Rows[i].Cells[e.ColumnIndex].Value = false;
                        }
                    }
                }
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

            //payments = chkdb.GetByAccount(SelectedAccount);
            //Log.Instance.Debug($"GetPayments returned {payments.Count} rows.");
            PaymentsDataGrid.DataSource = currentAccount.Payments.ToList();

            foreach (DataGridViewColumn col in PaymentsDataGrid.Columns)
            {
                col.Visible = false;
            }

            PaymentsDataGrid.Columns[nameof(Chk.PaidAmount)].Visible = true;
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
                var chk = chkdb.GetById(Convert.ToInt32(row.Cells[nameof(Chk.PaymentNo)].Value.ToString()));

                DisplayPOCOForm<Chk> frm = new DisplayPOCOForm<Chk>(chk)
                {
                    Title = "Payment Details"
                };
                frm.Show();
            }
        }

        private void AddPaymentButton_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region DiagnosisTab

        private void LoadDx()
        {
            Log.Instance.Trace("Entering");
            DiagnosisDataGrid.DataSource = new BindingSource(dxBindingList, null);
        }
        private void DxSearchButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var dictRecords = dictDxDb.Search(txtSearchDx.Text, currentAccount.TransactionDate.GetValueOrDefault(DateTime.Now));

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
                    MetroMessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                int maxNo = 0;
                if (dxBindingList.Count > 0)
                    maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                if (maxNo >= 9)
                {
                    MetroMessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                else
                {
                    dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = selectedCode, Description = selectedDesc });
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
                        MetroMessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        DxQuickAddTextBox.Text = "";
                        return;
                    }

                    var record = dictDxDb.GetByCode(DxQuickAddTextBox.Text, currentAccount.TransactionDate.GetValueOrDefault(DateTime.Now));
                    if (record != null)
                    {
                        //this is a valid entry
                        int maxNo = 0;
                        if (dxBindingList.Count > 0)
                            maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                        if (maxNo >= 9)
                        {
                            MetroMessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            DxQuickAddTextBox.Text = "";
                            return;
                        }
                        dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = record.DxCode, Description = record.Description });
                        DxQuickAddTextBox.Text = "";
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

                //loop through and renumber
                for (int i = 0; i < dxBindingList.Count; i++)
                {
                    dxBindingList[i].No = i + 1;
                }

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

            currentAccount.Pat.Diagnoses = dxBindingList.ToList<PatDiag>();

            if (patDB.SaveDiagnoses(currentAccount.Pat) == true)
                MetroMessageBox.Show(this, "Diagnoses updated successfully.");
            else
                MetroMessageBox.Show(this, "Diagnosis update failed.");
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
                notesdb.Add(note);
                //reload notes to pick up changes
                currentAccount.Notes = notesdb.GetByAccount(currentAccount.AccountNo);
                LoadNotes();
            }

        }

        #endregion

        #region BillingActivityTab

        private void LoadBillingActivity()
        {
            Log.Instance.Trace("Entering");


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
                MetroMessageBox.Show(this, "A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
            }
        }
        private void ChangeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            DateTime newDate = DateTime.MinValue;

            Form frm = new Form()
            {
                Text = "Change Date of Service",
                DialogResult = DialogResult.OK,
                Width = 400
            };
            Label lbl2 = new Label()
            {
                Text = "Choose the new date of service",
                Location = new Point(10, 10),
                Width = 130
            };
            DateTimePicker dateTimePicker = new DateTimePicker()
            {
                Name = "newDateFrm",
                Location = new Point(lbl2.Width + 10, 10),
                Format = DateTimePickerFormat.Short,
                Width = 100,
                Value = (DateTime)currentAccount.TransactionDate
            };
            Label lbl1 = new Label()
            {
                Text = "Enter Reason for date change",
                Location = new Point(10, 50),
                Width = 130
            };
            TextBox tbReason = new TextBox()
            {
                Location = new Point(lbl1.Width + 10, 50),
                Width = 200,
                Multiline = true
            };
            Button btnOK = new Button()
            {
                Text = "OK",
                Location = new System.Drawing.Point(10, 80)
            };
            Button btnCanel = new Button()
            {
                Text = "Cancel",
                Location = new Point(btnOK.Width + 20, 80)
            };


            btnOK.Click += (o, s) =>
            {
                if (string.IsNullOrEmpty(tbReason.Text))
                {
                    MetroMessageBox.Show(this, "Must enter a reason.");
                    return;
                }
                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };
            btnCanel.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.Cancel;
                frm.Close();
            };

            frm.Controls.Add(dateTimePicker);
            frm.Controls.Add(tbReason);
            frm.Controls.Add(lbl2);
            frm.Controls.Add(lbl1);
            frm.Controls.Add(btnOK);
            frm.Controls.Add(btnCanel);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MetroMessageBox.Show(this, $"New date is {dateTimePicker}. Reason is {tbReason.Text}");
                try
                {
                    accDB.ChangeDateOfService(ref currentAccount, dateTimePicker.Value, tbReason.Text);
                }
                catch (ArgumentNullException anex)
                {
                    Log.Instance.Error(anex, $"Change date of service parameter {anex.ParamName} must contain a value.");
                    MetroMessageBox.Show(this, $"{anex.ParamName} must contain a value. Date of service was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex, $"Error changing date of service.");
                    MetroMessageBox.Show(this, $"Error changing date of service. Date of service was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            Log.Instance.Trace($"Exiting");

        }

        private void ChangeFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            //create form to capture new financial code with dropdownlist of codes.
            string newFinCode = string.Empty;

            Form frm = new Form()
            {
                Text = "Change Financial Code",
                DialogResult = DialogResult.OK,
                Width = 400
            };
            Label lbl1 = new Label()
            {
                Text = "Choose new financial class",
                Location = new Point(10, 10),
                Width = 130
            };
            ComboBox cbNewFinCode = new ComboBox()
            {
                Location = new Point(lbl1.Width + 10, 10),
                Width = 200,
            };

            cbNewFinCode.DataSource = finDB.GetAll();
            cbNewFinCode.DisplayMember = nameof(Fin.res_party); // "res_party";
            cbNewFinCode.ValueMember = nameof(Fin.fin_code); // "fin_code";
            cbNewFinCode.SelectedIndex = -1;

            Button btnOK = new Button()
            {
                Text = "OK",
                Location = new System.Drawing.Point(10, 80)
            };
            Button btnCanel = new Button()
            {
                Text = "Cancel",
                Location = new Point(btnOK.Width + 20, 80)
            };

            btnOK.Click += (o, s) =>
            {

                frm.DialogResult = DialogResult.OK;
                frm.Close();
            };
            btnCanel.Click += (o, s) =>
            {
                frm.DialogResult = DialogResult.Cancel;
                frm.Close();
            };

            frm.Controls.Add(cbNewFinCode);
            frm.Controls.Add(lbl1);
            frm.Controls.Add(btnOK);
            frm.Controls.Add(btnCanel);
            if (frm.ShowDialog() == DialogResult.OK)
            {
                MetroMessageBox.Show(this, $"New financial class is {cbNewFinCode.SelectedValue}");
                try
                {
                    accDB.ChangeFinancialClass(ref currentAccount, cbNewFinCode.SelectedValue.ToString());
                }
                catch (ArgumentException anex)
                {
                    Log.Instance.Error(anex, $"Financial code {anex.ParamName} is not valid.");
                    MetroMessageBox.Show(this, $"{anex.ParamName} is not a valid financial code. Financial code was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error(ex, $"Error changing financial class.");
                    MetroMessageBox.Show(this, $"Error changing financial class. Financial code was not changed.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void ChangeClientToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            MessageBox.Show("Function not implemented.");
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
                MetroMessageBox.Show(this, "Account is not in HOLD status. This will only set account status from HOLD to NEW. It will not change any billing information.");
                return;
            }

            //Reason must be entered for changing status

            InputBoxResult prompt = InputBox.Show("Enter reason for setting status back to New:", "New Note");
            AccountNote note = new AccountNote();

            if (prompt.ReturnCode == DialogResult.OK)
            {
                note.Account = currentAccount.AccountNo;
                note.Comment = prompt.Text;
                notesdb.Add(note);
                //reload notes to pick up changes
                LoadNotes();
            }

            //AccountRepository accDB = new AccountRepository();
            currentAccount.Status = "NEW";
            accDB.Update(currentAccount);

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
                if (!await Task.Run(() => accDB.Validate(ref currentAccount)))
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
            ClaimGenerator claimGenerator = new ClaimGenerator(Helper.ConnVal);

            claimGenerator.CompileClaim(currentAccount.AccountNo);
        }

        private void BillActivityDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
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

            ClaimData claim = Newtonsoft.Json.JsonConvert.DeserializeObject<ClaimData>(claimJson);

            Billing837 billing837 = new Billing837(Helper.ConnVal);

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

            PrintClaimForm printClaim = new PrintClaimForm(Helper.ConnVal);

            //string x12Text = billing837.GenerateSingleClaim(claim, fileLocation);
            //printClaim.Print(x12Text, false, true);

            printClaim.PrintForm(claim);

        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Name == tabDemographics.Name)
            {
                //load demographics

            }
        }

        private void GuarantorSaveButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            currentAccount.Pat.GuarantorFullName = string.Format("{0} {1},{2} {3}",
                GuarantorLastNameTextBox.Text, GuarSuffixTextBox.Text, GuarFirstNameTextBox.Text, GuarMiddleNameTextBox.Text);
            currentAccount.Pat.GuarantorLastName = GuarantorLastNameTextBox.Text;
            currentAccount.Pat.GuarantorFirstName = GuarFirstNameTextBox.Text;
            currentAccount.Pat.GuarantorMiddleName = GuarMiddleNameTextBox.Text;
            currentAccount.Pat.GuarantorNameSuffix = GuarSuffixTextBox.Text;
            currentAccount.Pat.GuarantorAddress = GuarantorAddressTextBox.Text;
            currentAccount.Pat.GuarantorCity = GuarCityTextBox.Text;
            currentAccount.Pat.GuarantorPrimaryPhone = GuarantorPhoneTextBox.Text;
            currentAccount.Pat.GuarantorState = GuarStateComboBox.SelectedValue.ToString();
            currentAccount.Pat.GuarantorZipCode = GuarZipTextBox.Text;
            currentAccount.Pat.GuarantorCityState = string.Format("{0}, {1} {2}", GuarCityTextBox.Text, GuarStateComboBox.SelectedValue.ToString(), GuarZipTextBox.Text);
            currentAccount.Pat.GuarRelationToPatient = GuarantorRelationComboBox.SelectedValue.ToString();

            patDB.SaveAll(currentAccount.Pat);

            var controls = tabGuarantor.Controls;

            foreach (Control control in controls)
            {
                //set background back to white to indicate change has been saved to database
                control.BackColor = Color.White;
            }
            LoadAccountData();
        }

        private void providerLookup1_SelectedValueChanged(object source, EventArgs args)
        {
            string phy = providerLookup1.SelectedValue;
        }
    }
}
