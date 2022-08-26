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

namespace LabBilling.Forms
{
    public partial class AccountForm : Form
    {
        private BindingList<PatDiag> dxBindingList;

        private List<InsCompany> insCompanies = null;
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

        private string _selectedAccount;
        public string SelectedAccount
        {
            get { return _selectedAccount; }
        } 

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
            if(systemParametersRepository.GetByKey("allow_chk_entry") == "1")
            {
                if(Program.LoggedInUser.CanAddPayments)
                {
                    Helper.SetControlsAccess(tabPayments.Controls, false);
                }
            }

            Helper.SetControlsAccess(demoTabLayoutPanel.Controls, false);
            Helper.SetControlsAccess(tabInsurance.Controls, false);
            Helper.SetControlsAccess(insTabLayoutPanel.Controls, false);
            Helper.SetControlsAccess(tabDiagnosis.Controls, false);
            Helper.SetControlsAccess(tabNotes.Controls, false);
            Helper.SetControlsAccess(tabCharges.Controls, false);
            Helper.SetControlsAccess(tabPayments.Controls, false);
            lGuarCopyPatient.Enabled = false;
            lInsCopyPatient.Enabled = false;
            changeClientToolStripMenuItem.Enabled = false;
            changeDateOfServiceToolStripMenuItem.Enabled = false;
            changeFinancialClassToolStripMenuItem.Enabled = false;
            clearHoldStatusToolStripMenuItem.Enabled = false;
            if(Convert.ToBoolean(systemParametersRepository.GetByKey("allow_edit")))
            {
                if(Program.LoggedInUser.Access == "ENTER/EDIT")
                {
                    Helper.SetControlsAccess(demoTabLayoutPanel.Controls, true);
                    Helper.SetControlsAccess(tabInsurance.Controls, true);
                    Helper.SetControlsAccess(insTabLayoutPanel.Controls, true);
                    Helper.SetControlsAccess(tabDiagnosis.Controls, true);
                    Helper.SetControlsAccess(tabNotes.Controls, true);
                    Helper.SetControlsAccess(tabCharges.Controls, true);
                    Helper.SetControlsAccess(tabPayments.Controls, true);
                    lGuarCopyPatient.Enabled = true;
                    lInsCopyPatient.Enabled = true;
                    changeClientToolStripMenuItem.Enabled = true;
                    changeDateOfServiceToolStripMenuItem.Enabled = true;
                    changeFinancialClassToolStripMenuItem.Enabled = true;
                    clearHoldStatusToolStripMenuItem.Enabled = true;
                }
            }

            #endregion

            #region load controlColumnMap

            controlColumnMap.Add(tbZipcode, nameof(Pat.ZipCode));
            controlColumnMap.Add(cbMaritalStatus, nameof(Pat.MaritalStatus));
            controlColumnMap.Add(tbEmailAddress, nameof(Pat.EmailAddress));
            controlColumnMap.Add(tbSuffix, nameof(Pat.PatNameSuffix));
            controlColumnMap.Add(tbLastName, nameof(Pat.PatNameSuffix));
            controlColumnMap.Add(tbMiddleName, nameof(Pat.PatMiddleName));
            controlColumnMap.Add(tbFirstName, nameof(Pat.PatFirstName));
            controlColumnMap.Add(cbState, nameof(Pat.State));
            controlColumnMap.Add(tbSSN, nameof(Account.SocSecNo));
            controlColumnMap.Add(tbDateOfBirth, nameof(Pat.BirthDate));
            controlColumnMap.Add(tbPhone, nameof(Pat.PrimaryPhone));
            controlColumnMap.Add(tbCity, nameof(Pat.City));
            controlColumnMap.Add(tbAddress2, nameof(Pat.Address2));
            controlColumnMap.Add(cbSex, nameof(Pat.Sex));
            controlColumnMap.Add(tbAddress1, nameof(Pat.Address1));
            controlColumnMap.Add(tbGuarZip, nameof(Pat.GuarantorZipCode));
            controlColumnMap.Add(cbPlanFinCode, nameof(Ins.FinCode));
            controlColumnMap.Add(tbCertSSN, nameof(Ins.CertSSN));
            controlColumnMap.Add(tbHolderLastName, nameof(Ins.HolderLastName));
            controlColumnMap.Add(tbGroupName, nameof(Ins.GroupName));
            controlColumnMap.Add(cbInsCode, nameof(Ins.InsCode));
            controlColumnMap.Add(tbHolderZip, nameof(Ins.HolderZip));
            controlColumnMap.Add(tbGroupNumber, nameof(Ins.GroupNumber));
            controlColumnMap.Add(tbPlanAddress2, nameof(Ins.PlanAddress2));
            controlColumnMap.Add(cbInsRelation, nameof(Ins.Relation));
            controlColumnMap.Add(tbPolicyNumber, nameof(Ins.PolicyNumber));
            controlColumnMap.Add(tbHolderDOB, nameof(Ins.HolderBirthDate));
            controlColumnMap.Add(cbHolderState, nameof(Ins.HolderState));
            controlColumnMap.Add(tbHolderFirstName, nameof(Ins.HolderFirstName));
            controlColumnMap.Add(cbInsOrder, nameof(Ins.Coverage));
            controlColumnMap.Add(cbHolderSex, nameof(Ins.HolderSex));
            controlColumnMap.Add(tbHolderMiddleName, nameof(Ins.HolderMiddleName));
            controlColumnMap.Add(tbPlanName, nameof(Ins.PlanName));
            controlColumnMap.Add(tbHolderAddress, nameof(Ins.HolderAddress));
            controlColumnMap.Add(tbPlanCitySt, nameof(Ins.PlanCityState));
            controlColumnMap.Add(tbPlanAddress, nameof(Ins.PlanAddress1));
            controlColumnMap.Add(tbHolderCity, nameof(Ins.HolderCity));
            controlColumnMap.Add(tbGuarSuffix, nameof(Pat.GuarantorNameSuffix));
            controlColumnMap.Add(tbGuarMiddleName, nameof(Pat.GuarantorMiddleName));
            controlColumnMap.Add(tbGuarFirstName, nameof(Pat.GuarantorFirstName));
            controlColumnMap.Add(cbGuarState, nameof(Pat.GuarantorState));
            controlColumnMap.Add(cbGuarantorRelation, nameof(Pat.GuarRelationToPatient));
            controlColumnMap.Add(tbGuarantorPhone, nameof(Pat.GuarantorPrimaryPhone));
            controlColumnMap.Add(tbGuarCity, nameof(Pat.GuarantorCity));
            controlColumnMap.Add(tbGuarantorAddress, nameof(Pat.GuarantorAddress));
            controlColumnMap.Add(tbGuarantorLastName, nameof(Pat.GuarantorLastName));
            #endregion

            #region Setup Insurance Company Combobox

            insCompanies = insCompanyRepository.GetAll(true).ToList();

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
                values[0] = insc.code;
                values[1] = insc.name;
                inscDataTable.Rows.Add(values);
            }

            cbInsCode.DataSource = inscDataTable;
            cbInsCode.DisplayMember = "Name";
            cbInsCode.ValueMember = "Code";

            #endregion

            #region populate combo boxes

            cbState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbState.DisplayMember = "Value";
            cbState.ValueMember = "Key";

            cbSex.DataSource = new BindingSource(Dictionaries.sexSource, null);
            cbSex.DisplayMember = "Value";
            cbSex.ValueMember = "Key";

            cbMaritalStatus.DataSource = new BindingSource(Dictionaries.maritalSource, null);
            cbMaritalStatus.DisplayMember = "Value";
            cbMaritalStatus.ValueMember = "Key";

            cbGuarState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbGuarState.DisplayMember = "Value";
            cbGuarState.ValueMember = "Key";

            cbHolderState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbHolderState.DisplayMember = "Value";
            cbHolderState.ValueMember = "Key";

            cbGuarantorRelation.DataSource = new BindingSource(Dictionaries.relationSource, null);
            cbGuarantorRelation.DisplayMember = "Value";
            cbGuarantorRelation.ValueMember = "Key";

            cbInsRelation.DataSource = new BindingSource(Dictionaries.relationSource, null);
            cbInsRelation.DisplayMember = "Value";
            cbInsRelation.ValueMember = "Key";

            cbInsOrder.DataSource = new BindingSource(Dictionaries.payorOrderSource, null);
            cbInsOrder.DisplayMember = "Value";
            cbInsOrder.ValueMember = "Key";

            cbHolderSex.DataSource = new BindingSource(Dictionaries.sexSource, null);
            cbHolderSex.DisplayMember = "Value";
            cbHolderSex.ValueMember = "Key";

            cbPlanFinCode.DataSource = finDB.GetAll();
            cbPlanFinCode.DisplayMember = nameof(Fin.res_party);
            cbPlanFinCode.ValueMember = nameof(Fin.fin_code);
            cbPlanFinCode.SelectedIndex = -1;

            #endregion

            if (SelectedAccount != null || SelectedAccount != "")
            {
                Log.Instance.Debug($"Loading account data for {SelectedAccount}");
                userProfileDB.InsertRecentAccount(SelectedAccount, Program.LoggedInUser.UserName);
                LoadAccountData();

                AddOnChangeHandlerToInputControls(demoTabLayoutPanel);
                AddOnChangeHandlerToInputControls(tabInsurance);
            }

            UpdateDxPointers.Enabled = false; // start disabled until a box has been checked.
        }

        private void AccountForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
        }

        private void BtnRefresh_Click(object sender, EventArgs e)
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
            ckShowCreditedChrg.Checked = false;
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
                new SummaryData("Address", currentAccount.Pat.AddressLine, SummaryData.GroupType.Demographics,9,1),
                new SummaryData("Phone", currentAccount.Pat.PrimaryPhone.FormatPhone(), SummaryData.GroupType.Demographics,10,1),
                new SummaryData("Email", currentAccount.Pat.EmailAddress, SummaryData.GroupType.Demographics,11,1),

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
                sd.Add(new SummaryData("DOB/Sex", currentAccount.Pat.DOBSex, SummaryData.GroupType.Demographics, 8, 1));
                sd.Add(new SummaryData("Diagnoses", "", SummaryData.GroupType.Diagnoses, 12, 1, true));
                sd.Add(new SummaryData(currentAccount.Pat.Dx1, currentAccount.Pat.Dx1Desc, SummaryData.GroupType.Diagnoses, 13, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx2, currentAccount.Pat.Dx2Desc, SummaryData.GroupType.Diagnoses, 14, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx3, currentAccount.Pat.Dx3Desc, SummaryData.GroupType.Diagnoses, 15, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx4, currentAccount.Pat.Dx4Desc, SummaryData.GroupType.Diagnoses, 16, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx5, currentAccount.Pat.Dx5Desc, SummaryData.GroupType.Diagnoses, 17, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx6, currentAccount.Pat.Dx6Desc, SummaryData.GroupType.Diagnoses, 18, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx7, currentAccount.Pat.Dx7Desc, SummaryData.GroupType.Diagnoses, 19, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx8, currentAccount.Pat.Dx8Desc, SummaryData.GroupType.Diagnoses, 20, 1));
                sd.Add(new SummaryData(currentAccount.Pat.Dx9, currentAccount.Pat.Dx9Desc, SummaryData.GroupType.Diagnoses, 21, 1));

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
            DemoStatusMessages.Text = String.Empty;

            tbBannerName.Text = currentAccount.PatFullName;
            tbBannerDob.Text = currentAccount.Pat.BirthDate.GetValueOrDefault().ToShortDateString();
            tbBannerSex.Text = currentAccount.Pat.Sex;
            tbBannerAccount.Text = SelectedAccount;
            tbBannerMRN.Text = currentAccount.MRN;
            tbBannerClient.Text = currentAccount.ClientName;
            tbBannerFinClass.Text = currentAccount.FinCode;

            tbTotalCharges.Text = currentAccount.TotalCharges.ToString("c");

            //this.Text = $" {currentAccount.pat_name} - Demographics";

            tbBannerName.Text = currentAccount.PatFullName;
            tbBannerAccount.Text = _selectedAccount;
            tbBannerDob.Text = currentAccount.Pat.BirthDate.Value.ToShortDateString();
            tbBannerSex.Text = currentAccount.Pat.Sex;
            tbBannerMRN.Text = currentAccount.MRN;

            lblTotalCharges.Text = currentAccount.TotalCharges.ToString("c");
            lblTotalPmtAdj.Text = (currentAccount.TotalContractual + currentAccount.TotalPayments + currentAccount.TotalWriteOff).ToString("c");
            lblBalance.Text = currentAccount.Balance.ToString("c");
            
            if (!Str.ParseName(currentAccount.PatFullName, out string strLname, out string strFname, out string strMidName, out string strSuffix))
            {
                Log.Instance.Debug($"Entering");
                Log.Instance.Warn("Error parsing patient name. Parsed data will not be shown.");
                DemoStatusMessages.AppendText("Error parsing patient name. Parsed data will not be shown.");
                DemoStatusMessages.AppendText(Environment.NewLine);
                DemoStatusMessages.BackColor = Color.Yellow;
            }

            if (!Str.ParseCityStZip(currentAccount.Pat.CityStateZip, out string strCity, out string strState, out string strZip))
            {
                Log.Instance.Debug($"Entering");
                Log.Instance.Warn("Error parsing City St Zip. Data will not be shown.");
                DemoStatusMessages.AppendText("Error parsing City St Zip. Data will not be shown.");
                DemoStatusMessages.AppendText(Environment.NewLine);
                DemoStatusMessages.BackColor = Color.Yellow;
            }
            lblPatientFullName.Text = currentAccount.PatFullName;
            tbLastName.Text = strLname;
            tbLastName.BackColor = Color.White;
            tbFirstName.Text = strFname;
            tbFirstName.BackColor = Color.White;
            tbMiddleName.Text = strMidName;
            tbMiddleName.BackColor = Color.White;
            tbSuffix.Text = strSuffix;
            tbSuffix.BackColor = Color.White;
            tbAddress1.Text = currentAccount.Pat.Address1;
            tbAddress1.BackColor = Color.White;
            tbAddress2.Text = currentAccount.Pat.Address2;
            tbAddress2.BackColor = Color.White;
            tbCity.Text = currentAccount.Pat.City != null ? currentAccount.Pat.City : strCity;
            tbCity.BackColor = Color.White;
            cbState.SelectedValue = currentAccount.Pat.State != null ? currentAccount.Pat.State : strState;
            cbState.BackColor = Color.White;
            tbZipcode.Text = currentAccount.Pat.ZipCode != null ? currentAccount.Pat.ZipCode : strZip;
            tbZipcode.BackColor = Color.White;
            tbPhone.Text = currentAccount.Pat.PrimaryPhone;
            tbPhone.BackColor = Color.White;
            tbSSN.Text = currentAccount.SocSecNo;
            tbSSN.BackColor = Color.White;
            tbEmailAddress.Text = currentAccount.Pat.EmailAddress;
            tbEmailAddress.BackColor = Color.White;
            tbDateOfBirth.Text = currentAccount.Pat.BirthDate.Value.ToString("MM/dd/yyyy");
            tbDateOfBirth.BackColor = Color.White;
            cbSex.SelectedValue = currentAccount.Pat.Sex;
            cbSex.BackColor = Color.White;
            cbMaritalStatus.SelectedValue = currentAccount.Pat.MaritalStatus != null ? currentAccount.Pat.MaritalStatus : "";
            cbMaritalStatus.BackColor = Color.White;

            if (!Str.ParseName(currentAccount.Pat.GuarantorFullName, out strLname, out strFname, out strMidName, out strSuffix))
            {
                Log.Instance.Info($"Guarantor name could not be parsed. {currentAccount.Pat.GuarantorFullName}");
                Log.Instance.Warn("Error parsing guarantor name. Name may be blank. Parsed data will not be shown.");
                DemoStatusMessages.AppendText("Error parsing guarantor name. Name may be blank. Parsed data will not be shown.");
                DemoStatusMessages.AppendText(Environment.NewLine);
                DemoStatusMessages.BackColor = Color.Yellow;
            }

            tbGuarantorLastName.Text = strLname;
            tbGuarFirstName.Text = strFname;
            tbGuarMiddleName.Text = strMidName;
            tbGuarSuffix.Text = strSuffix;
            tbGuarantorAddress.Text = currentAccount.Pat.GuarantorAddress;
            tbGuarCity.Text = currentAccount.Pat.GuarantorCity;
            cbGuarState.SelectedValue = currentAccount.Pat.GuarantorState != null ? currentAccount.Pat.GuarantorState : "";
            tbGuarZip.Text = currentAccount.Pat.GuarantorZipCode;
            tbGuarantorPhone.Text = currentAccount.Pat.GuarantorPrimaryPhone;
            cbGuarantorRelation.SelectedValue = currentAccount.Pat.GuarRelationToPatient != null ? currentAccount.Pat.GuarRelationToPatient : "";

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
                this.dgvInsurance.Columns.Add(deleteCol);

                var insBindingList = new BindingList<Ins>(currentAccount.Insurances);
                insGridSource = new BindingSource(insBindingList, null);

                dgvInsurance.DataSource = insGridSource;
                dgvInsurance.Columns[nameof(Ins.mod_prg)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.mod_user)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.mod_date)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.mod_host)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.Employer)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.EmployerCityState)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.HolderLastName)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.HolderFirstName)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.HolderMiddleName)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.PlanEffectiveDate)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.PlanExpirationDate)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.rowguid)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.Account)].Visible = false;
                dgvInsurance.Columns[nameof(Ins.InsCompany)].Visible = false;

                dgvInsurance.Columns[nameof(Ins.PlanName)].DisplayIndex = 1;
                dgvInsurance.Columns[nameof(Ins.PlanAddress1)].DisplayIndex = 2;
                dgvInsurance.Columns[nameof(Ins.PolicyNumber)].DisplayIndex = 3;

                dgvInsurance.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                //dgvInsurance.Columns[nameof(Ins.HolderName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                cbInsOrder.SelectedIndex = 0;
                cbHolderState.SelectedIndex = 0;
                cbHolderSex.SelectedIndex = 0;

                dgvInsurance.ClearSelection();

                ResetControls(demoTabLayoutPanel.Controls);
            }

        }

        #region DemographicTab

        private void LGuarCopyPatient_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tbGuarantorLastName.Text = tbLastName.Text;
            tbGuarFirstName.Text = tbFirstName.Text;
            tbGuarMiddleName.Text = tbMiddleName.Text;
            tbGuarSuffix.Text = tbSuffix.Text;
            tbGuarantorAddress.Text = tbAddress1.Text;
            tbGuarCity.Text = tbCity.Text;
            cbGuarState.SelectedValue = cbState.SelectedValue;
            tbGuarZip.Text = tbZipcode.Text;
            tbGuarantorPhone.Text = tbPhone.Text;
            cbGuarantorRelation.SelectedValue = "01";
        }

        private void LInsCopyPatient_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Log.Instance.Debug($"Copying patient data to insurance holder.");
            //copy patient data for insurance holder info
            tbHolderLastName.Text = tbLastName.Text;
            tbHolderFirstName.Text = tbFirstName.Text;
            tbHolderMiddleName.Text = tbMiddleName.Text;
            tbHolderAddress.Text = tbAddress1.Text;
            tbHolderCity.Text = tbCity.Text;
            cbHolderState.SelectedValue = cbState.SelectedValue;
            tbHolderZip.Text = tbZipcode.Text;
            tbHolderDOB.Text = tbDateOfBirth.Text;
            cbHolderSex.SelectedValue = cbSex.SelectedValue;
            cbInsRelation.SelectedValue = "01";
        }

        private void SaveDemographics_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            currentAccount.PatFullName = string.Format("{0} {1},{2} {3}", tbLastName.Text, tbSuffix.Text, tbFirstName.Text, tbMiddleName.Text);
            currentAccount.SocSecNo = tbSSN.Text;

            accDB.Update(currentAccount);

            currentAccount.Pat.CityStateZip = string.Format("{0}, {1} {2}", tbCity.Text, cbState.SelectedValue.ToString(), tbZipcode.Text);
            currentAccount.Pat.City = tbCity.Text;
            currentAccount.Pat.Address1 = tbAddress1.Text;
            currentAccount.Pat.Address2 = tbAddress2.Text;
            currentAccount.Pat.EmailAddress = tbEmailAddress.Text;
            currentAccount.Pat.MaritalStatus = cbMaritalStatus.SelectedValue.ToString();
            currentAccount.Pat.PrimaryPhone = tbPhone.Text;
            currentAccount.Pat.State = cbState.SelectedValue.ToString();
            currentAccount.Pat.ZipCode = tbZipcode.Text;
            currentAccount.Pat.PatFullName = string.Format("{0},{1} {2}", tbLastName.Text, tbFirstName.Text, tbMiddleName.Text);
            currentAccount.Pat.BirthDate = DateTime.Parse(tbDateOfBirth.Text);
            currentAccount.Pat.GuarantorFullName = string.Format("{0} {1},{2} {3}",
                tbGuarantorLastName.Text, tbGuarSuffix.Text, tbGuarFirstName.Text, tbGuarMiddleName.Text);
            currentAccount.Pat.GuarantorAddress = tbGuarantorAddress.Text;
            currentAccount.Pat.GuarantorCity = tbGuarCity.Text;
            currentAccount.Pat.GuarantorPrimaryPhone = tbGuarantorPhone.Text;
            currentAccount.Pat.GuarantorState = cbGuarState.SelectedValue.ToString();
            currentAccount.Pat.GuarantorZipCode = tbGuarZip.Text;
            currentAccount.Pat.GuarantorCityState = string.Format("{0}, {1} {2}", tbGuarCity.Text, cbGuarState.SelectedValue.ToString(), tbGuarZip.Text);
            currentAccount.Pat.Sex = cbSex.SelectedValue.ToString();
            currentAccount.Pat.GuarRelationToPatient = cbGuarantorRelation.SelectedValue.ToString();
            //currentAccount.Pat.SocSecNo = tbSSN.Text;

            patDB.SaveAll(currentAccount.Pat);

            var controls = tabDemographics.Controls;

            foreach (Control control in controls)
            {
                //set background back to white to indicate change has been saved to database
                control.BackColor = Color.White;
            }

        }

        #endregion

        #region InsuranceTab

        private void BSaveInsurance_Click_1(object sender, EventArgs e)
        {
            // saves the insurance info back to the grid
            Log.Instance.Trace($"Entering");

            int selectedIns = -1;

            switch (cbInsOrder.SelectedValue)
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
                MetroMessageBox.Show(this,"Insurance Order is not a valid selection.");
                Log.Instance.Debug("Insurance Order is not a valid selection.");
                return;
            }

            if (currentAccount.Insurances.Count() - 1 < selectedIns)
            {
                //This is a new record
                currentAccount.Insurances.Insert(selectedIns, new Ins());
                currentAccount.Insurances[selectedIns].Account = _selectedAccount;
            }

            currentAccount.Insurances[selectedIns].CertSSN = tbCertSSN.Text;
            currentAccount.Insurances[selectedIns].GroupName = tbGroupName.Text;
            currentAccount.Insurances[selectedIns].GroupNumber = tbGroupNumber.Text;
            currentAccount.Insurances[selectedIns].HolderAddress = tbHolderAddress.Text;
            currentAccount.Insurances[selectedIns].HolderCityStZip = string.Format("{0}, {1} {2}",
                tbHolderCity.Text,
                cbHolderState.SelectedValue == null ? "" : cbHolderState.SelectedValue.ToString(),
                tbHolderZip.Text);
            if (currentAccount.Insurances[selectedIns].HolderCityStZip.Trim() == ",")
            {
                currentAccount.Insurances[selectedIns].HolderCityStZip = String.Empty;
            }
            if(tbHolderDOB.MaskCompleted)
                currentAccount.Insurances[selectedIns].HolderBirthDate = DateTime.Parse(tbHolderDOB.Text);

            currentAccount.Insurances[selectedIns].HolderFirstName = tbHolderFirstName.Text;
            currentAccount.Insurances[selectedIns].HolderLastName = tbHolderLastName.Text;
            currentAccount.Insurances[selectedIns].HolderMiddleName = tbHolderMiddleName.Text;
            currentAccount.Insurances[selectedIns].HolderName = string.Format("{0},{1} {2}",
                tbHolderLastName.Text, tbHolderFirstName.Text, tbHolderMiddleName.Text);
            currentAccount.Insurances[selectedIns].HolderSex = cbHolderSex.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].PolicyNumber = tbPolicyNumber.Text;
            currentAccount.Insurances[selectedIns].PlanAddress1 = tbPlanAddress.Text;
            currentAccount.Insurances[selectedIns].PlanAddress2 = tbPlanAddress2.Text;
            currentAccount.Insurances[selectedIns].PlanName = tbPlanName.Text;
            currentAccount.Insurances[selectedIns].PlanCityState = tbPlanCitySt.Text;
            currentAccount.Insurances[selectedIns].Relation = cbInsRelation.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].Coverage = cbInsOrder.SelectedValue.ToString();
            currentAccount.Insurances[selectedIns].FinCode = cbPlanFinCode.SelectedValue.ToString();

            currentAccount.Insurances[selectedIns].InsCode = cbInsCode.SelectedValue.ToString();

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
                        cbInsOrder.SelectedValue, tbPlanName.Text, cbInsOrder.SelectedValue), "Existing Insurance", MessageBoxButtons.YesNo) == DialogResult.Yes)
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

                    foreach(Control control in insTabLayoutPanel.Controls)
                    {
                        if(changedControls.Contains(control.Name))
                        {
                            //get field name from map
                            string field = controlColumnMap[control].ToString();
                            if (!string.IsNullOrEmpty(field))
                                updatedColumns.Add(field);
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
            tbPlanAddress.Text = "";
            tbPlanAddress2.Text = "";
            tbPlanCitySt.Text = "";
            tbPlanName.Text = "";
            tbHolderAddress.Text = "";
            tbHolderCity.Text = "";
            tbHolderDOB.Text = "";
            tbHolderFirstName.Text = "";
            tbHolderLastName.Text = "";
            tbHolderMiddleName.Text = "";
            tbHolderZip.Text = "";
            tbPolicyNumber.Text = "";
            tbGroupName.Text = "";
            tbGroupNumber.Text = "";
            tbCertSSN.Text = "";
            cbHolderSex.SelectedIndex = 0;
            cbHolderState.SelectedIndex = 0;
            cbInsCode.SelectedIndex = -1;
            cbInsOrder.SelectedIndex = 0;
            cbPlanFinCode.SelectedIndex = -1;
        }

        private void DgvInsurance_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            tbInsTabMessage.Text = String.Empty;
            if (e.ColumnIndex == 0)
            {
                #region delete button was clicked
                int selectedIns = -1;

                switch (dgvInsurance.SelectedRows[0].Cells[nameof(Ins.Coverage)].Value.ToString())
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

                if (MetroMessageBox.Show(this, string.Format("Delete {0} insurance {1} for this patient?", currentAccount.Insurances[selectedIns].Coverage, currentAccount.Insurances[selectedIns].PlanName),
                    "Delete Insurance", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    if (insDB.Delete(currentAccount.Insurances[selectedIns]))
                        currentAccount.Insurances.RemoveAt(selectedIns);
                }
                insGridSource.ResetBindings(false);
                ClearInsEntryFields();

                ResetControls(insTabLayoutPanel.Controls);
                                
                return;
                #endregion
            }

            //populate edit boxes
            InEditMode = true;
            cbHolderState.SelectedItem = null;
            cbHolderState.SelectedText = "--Select--";
            cbHolderSex.SelectedItem = null;
            cbHolderSex.SelectedText = "--Select--";
            cbInsOrder.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.Coverage)].Value.ToString();

            if (!Str.ParseName(dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderName)].Value.ToString(),
                out string lname, out string fname, out string mname, out string suffix))
            {
                //error parsing name
                Log.Instance.Info($"Insurance holder name could not be parsed. {dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderName)].Value}");
                tbInsTabMessage.Text += "Error while parsing name into its parts. Name will not be shown in fields.";
                tbInsTabMessage.Text += Environment.NewLine;
                tbInsTabMessage.BackColor = Color.Yellow;
            }

            tbHolderLastName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderLastName)].Value?.ToString() ?? lname;
            tbHolderFirstName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderFirstName)].Value?.ToString() ?? fname;
            tbHolderMiddleName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderMiddleName)].Value?.ToString() ?? mname;
            tbHolderAddress.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderAddress)].Value != null ? dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderAddress)].Value.ToString() : "";

            if (dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value != null 
                && dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value.ToString() != "")
            {
                if (!Str.ParseCityStZip(dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value.ToString(),
                    out string city, out string state, out string zip))
                {
                    Log.Instance.Info($"Insurance holder city, st, zip could not be parsed. {dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value}");
                    MetroMessageBox.Show(this, "Error parsing City, st zip into its parts. Will not be shown in fields.");
                }
                tbHolderCity.Text = city;
                cbHolderState.SelectedValue = state;
                tbHolderZip.Text = zip;
            }

            cbHolderSex.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderSex)].Value?.ToString() ?? "";
            if (!string.IsNullOrEmpty(dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()))
                tbHolderDOB.Text = DateTime.Parse(dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()).ToString(@"MM/dd/yyyy");
            else
                tbHolderDOB.Text = "";
            cbInsRelation.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.Relation)].Value?.ToString() ?? "";

            // set ins Code combo box
            cbInsCode.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.InsCode)].Value?.ToString() ?? "";

            tbPlanName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanName)].Value?.ToString();
            tbPlanAddress.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanAddress1)].Value?.ToString();
            tbPlanAddress2.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanAddress2)].Value?.ToString();
            tbPlanCitySt.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanCityState)].Value?.ToString();
            tbPolicyNumber.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PolicyNumber)].Value?.ToString();
            tbGroupNumber.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.GroupNumber)].Value?.ToString();
            tbGroupName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.GroupName)].Value?.ToString();
            tbCertSSN.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.CertSSN)].Value?.ToString();
            cbPlanFinCode.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.FinCode)].Value?.ToString() ?? "";

            //reset changed flag & colors
            foreach(Control ctrl in insTabLayoutPanel.Controls)
            {
                if (ctrl is TextBox || ctrl is ComboBox || ctrl is FlatCombo || ctrl is MaskedTextBox)
                {
                    ctrl.BackColor = Color.White;
                }
                changedControls.Remove(ctrl.Name);
            }

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
                tbPlanName.Text = record.name;
                tbPlanAddress.Text = record.addr1;
                tbPlanAddress2.Text = record.addr2;
                tbPlanCitySt.Text = record.citystzip;
                cbPlanFinCode.SelectedValue = record.fin_code;
            }
        }

        private void AddInsurance_Click(object sender, EventArgs e)
        {
            //clear the insurance table selection and data entry fields.
            dgvInsurance.ClearSelection();
            ClearInsEntryFields();
        }

        #endregion

        #region ChargeTab
        private void LoadCharges()
        {
            Log.Instance.Trace("Entering");
            //dgvCharges.DataSource = chrgdb.GetByAccount(SelectedAccount, ckShowCreditedChrg.Checked);
            dgvCharges.DataSource = currentAccount.Charges;

            foreach (DataGridViewColumn col in dgvCharges.Columns)
            {
                col.Visible = false;
            }

            dgvCharges.Columns[nameof(Chrg.IsCredited)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.CDMCode)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.CdmDescription)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.Quantity)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.NetAmount)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.ServiceDate)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.Status)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.Comment)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.ChrgId)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.Invoice)].Visible = true;

            dgvCharges.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Format = "N2";
            dgvCharges.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCharges.Columns[nameof(Chrg.CalculatedAmount)].DefaultCellStyle.Format = "N2";
            dgvCharges.Columns[nameof(Chrg.CalculatedAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCharges.Columns[nameof(Chrg.Quantity)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvCharges.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvCharges.Columns[nameof(Chrg.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvCharges.BackgroundColor = Color.AntiqueWhite;
            dgvChrgDetail.BackgroundColor = Color.AntiqueWhite;

        }

        private void DgvCharges_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = dgvCharges.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {

                DataGridViewRow row = dgvCharges.SelectedRows[0];
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
        private void DgvCharges_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = dgvCharges.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = dgvCharges.SelectedRows[0];
                var chrg = chrgdb.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

                try
                {
                    dgvChrgDetail.DataSource = chrg.ChrgDetails;
                }
                catch (Exception ex)
                {
                    MetroMessageBox.Show(this, string.Format("Exception {0}", ex.Message));
                }
                foreach (DataGridViewColumn col in dgvChrgDetail.Columns)
                {
                    col.Visible = false;
                }

                dgvChrgDetail.Columns[nameof(ChrgDetail.Cpt4)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.BillType)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.DiagCodePointer)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.Modifier)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.Modifer2)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.RevenueCode)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.Type)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.BillMethod)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.OrderCode)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetail.Amount)].Visible = true;

                dgvChrgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvChrgDetail.Columns[nameof(ChrgDetail.Cpt4)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvChrgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                dgvChrgDetail.Columns[nameof(ChrgDetail.Amount)].DefaultCellStyle.Format = "N2";

            }
        }

        private void DgvCharges_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.ColumnIndex == dgvCharges.Columns[nameof(Chrg.IsCredited)].Index && e.Value.ToString() == "True")
            {
                dgvCharges.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Red;
                dgvCharges.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
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
            int selectedRows = dgvCharges.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = dgvCharges.SelectedRows[0];

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

        private void CkShowCreditedChrg_CheckedChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            LoadCharges();
        }

        private void BtnAddCharge_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ChargeEntryForm frm = new ChargeEntryForm(currentAccount);

            if (frm.ShowDialog() == DialogResult.OK)
            {
                LoadAccountData();
            }
        }

        private void dgvChrgDetail_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
        }

        private void dgvChrgDetail_SelectionChanged(object sender, EventArgs e)
        {
            if(UpdateDxPointers.Enabled == true)
            {
                if(MetroMessageBox.Show(this, "Changes were made to diagnosis pointers. Save changes?", "Save Changes?", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) 
                    == DialogResult.Yes)
                {
                    UpdateDxPointers_Click(sender, e);                    
                }
                UpdateDxPointers.Enabled = false;
            }

            Log.Instance.Trace($"Entering");

            //load dx pointers
            int selectedRows = dgvChrgDetail.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = dgvChrgDetail.SelectedRows[0];

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
                    DiagnosisPointerDGV.DataSource = dt;
                    DiagnosisPointerDGV.Columns["DxCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
                    DiagnosisPointerDGV.AutoResizeColumns();
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex.Message);
                    MetroMessageBox.Show(this, "Error loading Diagnosis Code Pointer. Exception has been logged. Report to Administrator.");
                }
            }
        }

        private void UpdateDxPointers_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();

            for (int c = 2; c < DiagnosisPointerDGV.ColumnCount; c++)
            {
                for (int r = 0; r < DiagnosisPointerDGV.RowCount; r++)
                {
                    if((bool)DiagnosisPointerDGV.Rows[r].Cells[c].Value == true)
                    {
                        sb.Append(DiagnosisPointerDGV.Rows[r].Cells[0].Value.ToString() + ":");
                    }
                }
            }

            string dxPtr = sb.ToString();


            //update amt record

            ChrgDetail amt = new ChrgDetail();

            amt.uri = Convert.ToInt32(dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.uri)].Value);
            amt.Amount = Convert.ToDouble(dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.Amount)].Value?? 0.0);
            amt.BillMethod = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.BillMethod)].Value?.ToString();
            amt.BillType = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.BillType)].Value?.ToString();
            amt.ChrgNo = Convert.ToInt32(dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.ChrgNo)].Value ?? 0.0);
            amt.Cpt4 = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.Cpt4)].Value?.ToString();
            amt.DiagCodePointer = dxPtr;
            amt.Modifier = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.Modifier)].Value?.ToString();
            amt.Modifer2 = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.Modifer2)].Value?.ToString();
            amt.LISReqNo = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.LISReqNo)].Value?.ToString();
            amt.OrderCode = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.OrderCode)].Value?.ToString();
            amt.PointerSet = true;
            amt.RevenueCode = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.RevenueCode)].Value?.ToString();
            amt.Type = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.Type)].Value?.ToString();

            ChrgDetailRepository amtRepository = new ChrgDetailRepository(Helper.ConnVal);
            try
            {
                amtRepository.Update(amt);
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex.Message);
                MetroMessageBox.Show(this, "Error updating charge detail record. Please try again. If error continues, report error to Administrator.");
                return;
            }

            UpdateDxPointers.Enabled = false;
            LoadCharges();
            dgvChrgDetail.DataSource = null;
        }

        private void DiagnosisPointerDGV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            DiagnosisPointerDGV.EndEdit();

            if(e.ColumnIndex > 1 )
            {
                UpdateDxPointers.Enabled = true;

                DataGridViewCheckBoxCell cbCell = DiagnosisPointerDGV.Rows[e.RowIndex].Cells[e.ColumnIndex] as DataGridViewCheckBoxCell;
                bool checkedValue = (bool)cbCell.Value;

                if (checkedValue == true)
                {
                    //loop through the column entries and ensure only one is checked
                    for (int i = 0; i < DiagnosisPointerDGV.RowCount; i++)
                    {
                        if(i != e.RowIndex)
                        {
                            DiagnosisPointerDGV.Rows[i].Cells[e.ColumnIndex].Value = false;
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

            tbTotalPayment.Text = currentAccount.TotalPayments.ToString("c");
            tbTotalContractual.Text = currentAccount.TotalContractual.ToString("c");
            tbTotalWriteOff.Text = currentAccount.TotalWriteOff.ToString("c");
            tbTotalPmtAll.Text = (currentAccount.TotalPayments + currentAccount.TotalContractual + currentAccount.TotalWriteOff).ToString("c");

            //payments = chkdb.GetByAccount(SelectedAccount);
            //Log.Instance.Debug($"GetPayments returned {payments.Count} rows.");
            dgvPayments.DataSource = currentAccount.Payments.ToList();

            foreach (DataGridViewColumn col in dgvPayments.Columns)
            {
                col.Visible = false;
            }

            dgvPayments.Columns[nameof(Chk.PaidAmount)].Visible = true;
            dgvPayments.Columns[nameof(Chk.ChkDate)].Visible = true;
            dgvPayments.Columns[nameof(Chk.CheckNo)].Visible = true;
            dgvPayments.Columns[nameof(Chk.Comment)].Visible = true;
            dgvPayments.Columns[nameof(Chk.ContractualAmount)].Visible = true;
            dgvPayments.Columns[nameof(Chk.DateReceived)].Visible = true;
            dgvPayments.Columns[nameof(Chk.Invoice)].Visible = true;
            dgvPayments.Columns[nameof(Chk.Source)].Visible = true;
            dgvPayments.Columns[nameof(Chk.Status)].Visible = true;
            dgvPayments.Columns[nameof(Chk.WriteOffAmount)].Visible = true;
            dgvPayments.Columns[nameof(Chk.WriteOffCode)].Visible = true;
            dgvPayments.Columns[nameof(Chk.WriteOffDate)].Visible = true;
            
            dgvPayments.Columns[nameof(Chk.PaidAmount)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(Chk.PaidAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(Chk.ContractualAmount)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(Chk.ContractualAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(Chk.WriteOffAmount)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(Chk.WriteOffAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvPayments.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvPayments.BackgroundColor = Color.AntiqueWhite;
        }

        private void DgvPayments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = dgvPayments.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {

                DataGridViewRow row = dgvPayments.SelectedRows[0];
                var chk = chkdb.GetById(Convert.ToInt32(row.Cells[nameof(Chk.PaymentNo)].Value.ToString()));

                DisplayPOCOForm<Chk> frm = new DisplayPOCOForm<Chk>(chk)
                {
                    Title = "Payment Details"
                };
                frm.Show();
            }
        }

        private void AddPayment_Click(object sender, EventArgs e)
        {

        }

        #endregion

        #region DiagnosisTab

        private void LoadDx()
        {
            Log.Instance.Trace("Entering");
            dgvDiagnosis.DataSource = new BindingSource(dxBindingList, null);
        }
        private void BtnDxSearch_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            var dictRecords = dictDxDb.Search(txtSearchDx.Text, currentAccount.TransactionDate.GetValueOrDefault(DateTime.Now));

            dgvDxSearch.DataSource = dictRecords;
            dgvDxSearch.Columns[nameof(DictDx.mod_date)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.mod_user)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.mod_prg)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.mod_host)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.deleted)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.AMA_year)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.id)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.version)].Visible = false;
            dgvDxSearch.Columns[nameof(DictDx.rowguid)].Visible = false;
        }

        private void DgvDxSearch_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            dgvDxSearch.Columns[nameof(DictDx.icd9_desc)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvDxSearch.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvDxSearch.Columns[1].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DgvDiagnosis_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            dgvDiagnosis.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvDiagnosis.Columns[2].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void DgvDxSearch_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = dgvDxSearch.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                //add selected diagnosis to account dx grid

                string selectedCode = dgvDxSearch.SelectedRows[0].Cells[nameof(DictDx.icd9_num)].Value.ToString();
                string selectedDesc = dgvDxSearch.SelectedRows[0].Cells[nameof(DictDx.icd9_desc)].Value.ToString();

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

        private void TxtSearchDx_KeyPress(object sender, KeyPressEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.KeyChar == (char)13)
            {
                Log.Instance.Debug("Enter key pressed");
                BtnDxSearch_Click(sender, e);
            }
        }

        private void TxtDxQuickAdd_KeyPress(object sender, KeyPressEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.KeyChar == (char)13)
            {
                Log.Instance.Debug("Enter key pressed");
                if (txtDxQuickAdd.Text != "")
                {
                    //check to see if the text entered is a valid DX code - if so, add the code and description to the selected grid

                    if (dxBindingList.First<PatDiag>(n => n.Code == txtDxQuickAdd.Text) != null)
                    {
                        //this code already exists in the list
                        MetroMessageBox.Show(this, "Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        txtDxQuickAdd.Text = "";
                        return;
                    }

                    var record = dictDxDb.GetByCode(txtDxQuickAdd.Text, currentAccount.TransactionDate.GetValueOrDefault(DateTime.Now));
                    if (record != null)
                    {
                        //this is a valid entry
                        int maxNo = 0;
                        if (dxBindingList.Count > 0)
                            maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                        if (maxNo >= 9)
                        {
                            MetroMessageBox.Show(this, "Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            txtDxQuickAdd.Text = "";
                            return;
                        }
                        dxBindingList.Add(new PatDiag { No = maxNo + 1, Code = record.icd9_num, Description = record.icd9_desc });
                        txtDxQuickAdd.Text = "";
                    }
                    else
                    {
                        //not valid - clear box and do nothing
                        txtDxQuickAdd.Text = "";
                        //System.Media.SystemSounds.Beep.Play();
                    }
                }
            }
        }

        private void BtnDxDelete_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = dgvDiagnosis.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                //add selected diagnosis to account dx grid

                string selectedCode = dgvDiagnosis.SelectedRows[0].Cells[nameof(PatDiag.Code)].Value.ToString();
                string selectedNo = dgvDiagnosis.SelectedRows[0].Cells[nameof(PatDiag.No)].Value.ToString();

                var record = dxBindingList.IndexOf(dxBindingList.First<PatDiag>(n => n.Code == selectedCode));

                dxBindingList.RemoveAt(record);

                //loop through and renumber
                for (int i = 0; i < dxBindingList.Count; i++)
                {
                    dxBindingList[i].No = i + 1;
                }

            }
        }

        private void DgvDiagnosis_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            e.Cancel = true;
            BtnDxDelete_Click(sender, e);
        }

        private void BtnSaveDx_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            currentAccount.Pat.Diagnoses = dxBindingList.ToList<PatDiag>();

            if (patDB.SaveDiagnoses(currentAccount.Pat) == true)
                MetroMessageBox.Show(this, "Diagnoses updated successfully.");
            else
                MetroMessageBox.Show(this, "Diagnosis update failed.");
        }

        private void dgvDiagnosis_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        #endregion

        #region NotesTab

        private void LoadNotes()
        {
            Log.Instance.Trace("Entering");
            tbNotesDisplay.Text = "";
            tbNotesDisplay.BackColor = Color.AntiqueWhite;
            foreach(AccountNote note in currentAccount.Notes)
            {
                tbNotesDisplay.DeselectAll();
                tbNotesDisplay.SelectionFont = new Font(tbNotesDisplay.SelectionFont, FontStyle.Bold);
                tbNotesDisplay.AppendText(note.mod_date + " - " + note.mod_user);
                tbNotesDisplay.SelectionFont = new Font(tbNotesDisplay.SelectionFont, FontStyle.Regular);
                tbNotesDisplay.AppendText(Environment.NewLine + note.comment + Environment.NewLine + Environment.NewLine);
            }
        }

        private void BtnNoteAdd_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            InputBoxResult prompt = InputBox.Show("Enter note:", "New Note", true);
            AccountNote note = new AccountNote();

            if (prompt.ReturnCode == DialogResult.OK)
            {
                note.account = currentAccount.AccountNo;
                note.comment = prompt.Text;
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

            //billingActivities = dbBillingActivity.GetByAccount(currentAccountSummary.account);
            dgvBillActivity.DataSource = currentAccount.BillingActivities.ToList();

            tbValidationResults.Text = currentAccount.AccountValidationStatus.validation_text;
            lblLastValidated.Text = currentAccount.AccountValidationStatus.mod_date.ToString("G");
           
        }

        #endregion


        private void PersonSearchToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            PersonSearchForm frm = new PersonSearchForm();
            frm.ShowDialog();
            if(frm.SelectedAccount != "" && frm.SelectedAccount != null)
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
                Location = new Point(lbl2.Width + 10,10),
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
                Location = new Point(lbl1.Width + 10,50),
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
                if(string.IsNullOrEmpty(tbReason.Text))
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
            if(frm.ShowDialog() == DialogResult.OK)
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
                catch(Exception ex)
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

            if(!changedControls.Contains(ctrl.Name))
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
                note.account = currentAccount.AccountNo;
                note.comment = prompt.Text;
                notesdb.Add(note);
                //reload notes to pick up changes
                LoadNotes();
            }

            //AccountRepository accDB = new AccountRepository();
            currentAccount.Status = "NEW";
            accDB.Update(currentAccount);

        }

        private void ResetControls(TableLayoutControlCollection controls)
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

        private void cbInsCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (cbInsCode.SelectedIndex >= 0)
            {
                string insCode = cbInsCode.SelectedValue.ToString();
                LookupInsCode(insCode);

                if (insCode == "MISC")
                {
                    tbPlanName.ReadOnly = false;
                    tbPlanAddress.ReadOnly = false;
                    tbPlanAddress2.ReadOnly = false;
                    tbPlanCitySt.ReadOnly = false;
                    cbPlanFinCode.Enabled = true;
                }
                else
                {
                    tbPlanName.ReadOnly = true;
                    tbPlanAddress.ReadOnly = true;
                    tbPlanAddress2.ReadOnly = true;
                    tbPlanCitySt.ReadOnly = true;
                    cbPlanFinCode.Enabled = false;
                }
            }

        }

        private async void btnValidateAccount_Click(object sender, EventArgs e)
        {
            if (!await Task.Run(() => accDB.Validate(ref currentAccount)))
            {
                //has validation errors - do not bill
                tbValidationResults.Text = currentAccount.AccountValidationStatus.validation_text;
                lblLastValidated.Text = currentAccount.AccountValidationStatus.mod_date.ToString("G");
            }
            else
            {
                //ok to bill
                tbValidationResults.Text = "No validation errors.";
                lblLastValidated.Text = currentAccount.AccountValidationStatus.mod_date.ToString("G");
            }

        }
    }
}
