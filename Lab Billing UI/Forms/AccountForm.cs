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
using RFClassLibrary;
using System.Data;
using System.Text;

namespace LabBilling.Forms
{
    public partial class AccountForm : Form
    {

        //private List<AccountSummary> accounts; // = new List<AccountSummary>();
        private AccountSummary currentAccountSummary; // = new AccountSummary();
        private Pat currentPat;
        private List<Ins> currentIns;
        private List<Chrg> charges; // = new List<Chrg>();
        private List<Chk> payments; // = new List<Chk>();
        private List<Notes> notes; // = new List<Notes>();
        private List<BillingActivity> billingActivities; // = new List<BillingActivity>();
        private BindingList<PatDiag> dxBindingList;

        //private List<Ins> currentIns = null;
        private List<InsCompany> insCompanies = null;
        private Account currentAccount = null;
        //private Pat currentPat = null;
        private BindingSource insGridSource = null;
        private bool InEditMode = false;

        private readonly InsRepository insDB = new InsRepository(Helper.ConnVal);
        private readonly AccountRepository accDB = new AccountRepository(Helper.ConnVal);
        private readonly PatRepository patDB = new PatRepository(Helper.ConnVal);
        private readonly DictDxRepository dxDB = new DictDxRepository(Helper.ConnVal);
        private readonly InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        private readonly ChrgRepository chrgdb = new ChrgRepository(Helper.ConnVal);
        private readonly UserProfileRepository userProfileDB = new UserProfileRepository(Helper.ConnVal);
        private readonly FinRepository finDB = new FinRepository(Helper.ConnVal);
        private readonly ChkRepository chkdb = new ChkRepository(Helper.ConnVal);
        private readonly NotesRepository notesdb = new NotesRepository(Helper.ConnVal);
        private readonly BillingActivityRepository dbBillingActivity = new BillingActivityRepository(Helper.ConnVal);
        private readonly DictDxRepository dictDxDb = new DictDxRepository(Helper.ConnVal);
        private readonly SystemParametersRepository systemParametersRepository = new SystemParametersRepository(Helper.ConnVal);

        private string _selectedAccount;
        public string SelectedAccount
        {
            get { return _selectedAccount; }
        } 

        public AccountForm(string account = null)
        {
            Log.Instance.Trace("Entering");
            InitializeComponent();

            if (account != null)
                _selectedAccount = account;
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

            Helper.SetControlsAccess(tabDemographics.Controls, false);
            Helper.SetControlsAccess(tabInsurance.Controls, false);
            Helper.SetControlsAccess(tableLayoutPanel1.Controls, false);
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
            if(systemParametersRepository.GetByKey("allow_edit") == "1")
            {
                if(Program.LoggedInUser.Access == "ENTER/EDIT")
                {
                    Helper.SetControlsAccess(tabDemographics.Controls, true);
                    Helper.SetControlsAccess(tabInsurance.Controls, true);
                    Helper.SetControlsAccess(tableLayoutPanel1.Controls, true);
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

            if (SelectedAccount != null || SelectedAccount != "")
            {
                Log.Instance.Debug($"Loading account data for {SelectedAccount}");
                userProfileDB.InsertRecentAccount(SelectedAccount, Program.LoggedInUser.UserName);
                LoadAccountData();

                AddOnChangeHandlerToInputControls(tabDemographics);
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

        #region SummaryTab

        private void LoadAccountData()
        {
            Log.Instance.Trace("Entering");

            currentAccountSummary = accDB.GetAccountSummary(SelectedAccount);
            currentPat = patDB.GetByAccount(SelectedAccount);
            currentIns = insDB.GetByAccount(SelectedAccount).ToList();

            this.Text = $"{currentAccountSummary.pat_name}";

            tbBannerName.Text = currentAccountSummary.pat_name;
            tbBannerDob.Text = currentPat.dob_yyyy.GetValueOrDefault().ToShortDateString();
            tbBannerSex.Text = currentPat.sex;
            tbBannerAccount.Text = SelectedAccount;
            tbBannerMRN.Text = currentAccountSummary.mri;
            tbBannerClient.Text = currentAccountSummary.ClientName;
            tbBannerFinClass.Text = currentAccountSummary.fin_code;

            tbTotalCharges.Text = currentAccountSummary.TotalCharges.ToString("c");

            // note: when adding new items to the summary data list, be sure to enter the items in the order they
            // should appear. In other words, keep the group types together and the individual items in the order
            // they should be displayed.
            #region PopulateSummaryTab
            List<SummaryData> sd = new List<SummaryData>
            {
                new SummaryData("Demographics","",SummaryData.GroupType.Demographics,1,1,true),
                new SummaryData("Account", SelectedAccount, SummaryData.GroupType.Demographics,2,1),
                new SummaryData("EMR Account", currentAccountSummary.meditech_account, SummaryData.GroupType.Demographics,3,1),
                new SummaryData("Status", currentAccountSummary.status,SummaryData.GroupType.Demographics,4,1),
                new SummaryData("MRN", currentAccountSummary.mri, SummaryData.GroupType.Demographics,5,1),
                new SummaryData("Client", currentAccountSummary.ClientName, SummaryData.GroupType.Demographics,7,1),
                new SummaryData("Address", currentPat.AddressLine, SummaryData.GroupType.Demographics,9,1),
                new SummaryData("Phone", currentPat.pat_phone.FormatPhone(), SummaryData.GroupType.Demographics,10,1),
                new SummaryData("Email", currentPat.pat_email, SummaryData.GroupType.Demographics,11,1),

                new SummaryData("Financial","",SummaryData.GroupType.Financial,1,2,true),
                new SummaryData("Financial Class", currentAccountSummary.fin_code, SummaryData.GroupType.Financial,2,2),
                new SummaryData("Date of Service", currentAccountSummary.trans_date?.ToShortDateString(), SummaryData.GroupType.Financial,3,2),
                new SummaryData("Total Charges", currentAccountSummary.TotalCharges.ToString("c"),SummaryData.GroupType.Financial,4,2),
                new SummaryData("Total Payments", (currentAccountSummary.TotalPayments+currentAccountSummary.TotalContractual+currentAccountSummary.TotalWriteOff).ToString("c"), 
                    SummaryData.GroupType.Financial,5,2),
                new SummaryData("Balance", currentAccountSummary.Balance.ToString("c"), SummaryData.GroupType.Financial,6,2)
            };
            //this data is not relevant if this is a CLIENT account
            if(currentAccountSummary.fin_code != "CLIENT")
            {
                sd.Add(new SummaryData("SSN", currentAccountSummary.ssn.FormatSSN(), SummaryData.GroupType.Demographics, 6, 1));
                sd.Add(new SummaryData("DOB/Sex", currentPat.DOBSex, SummaryData.GroupType.Demographics, 8, 1));
                sd.Add(new SummaryData("Diagnoses", "", SummaryData.GroupType.Diagnoses, 12, 1, true));
                sd.Add(new SummaryData(currentPat.icd9_1, currentPat.Dx1Desc, SummaryData.GroupType.Diagnoses, 13, 1));
                sd.Add(new SummaryData(currentPat.icd9_2, currentPat.Dx2Desc, SummaryData.GroupType.Diagnoses, 14, 1));
                sd.Add(new SummaryData(currentPat.icd9_3, currentPat.Dx3Desc, SummaryData.GroupType.Diagnoses, 15, 1));
                sd.Add(new SummaryData(currentPat.icd9_4, currentPat.Dx4Desc, SummaryData.GroupType.Diagnoses, 16, 1));
                sd.Add(new SummaryData(currentPat.icd9_5, currentPat.Dx5Desc, SummaryData.GroupType.Diagnoses, 17, 1));
                sd.Add(new SummaryData(currentPat.icd9_6, currentPat.Dx6Desc, SummaryData.GroupType.Diagnoses, 18, 1));
                sd.Add(new SummaryData(currentPat.icd9_7, currentPat.Dx7Desc, SummaryData.GroupType.Diagnoses, 19, 1));
                sd.Add(new SummaryData(currentPat.icd9_8, currentPat.Dx8Desc, SummaryData.GroupType.Diagnoses, 20, 1));
                sd.Add(new SummaryData(currentPat.icd9_9, currentPat.Dx9Desc, SummaryData.GroupType.Diagnoses, 21, 1));

                foreach (Ins ins in currentIns)
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
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent,50));

            foreach (SummaryData sdi in sd)
            {
                summaryTable.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                summaryTable.Controls.Add(sdi.DisplayLabel, sdi.ColPos == 1 ? 0 : 2, sdi.RowPos-1);
                if(sdi.IsHeader)
                {
                    summaryTable.SetColumnSpan(summaryTable.GetControlFromPosition(sdi.ColPos == 1 ? 0 : 2, sdi.RowPos - 1), 2);
                }
                else
                {
                    summaryTable.Controls.Add(sdi.ValueLabel, sdi.ColPos == 1 ? 1 : 3, sdi.RowPos - 1);
                }
                
            }
            #endregion

            dxBindingList = new BindingList<PatDiag>(currentPat.Diagnoses);
            ckShowCreditedChrg.Checked = false;
            LoadCharges();
            LoadPayments();
            LoadDx();
            LoadNotes();
            LoadDemographics();
            LoadBillingActivity();
        }

        private void LoadDemographics()
        {
            Log.Instance.Trace("Entering");
            DemoStatusMessages.Text = String.Empty;
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
            cbPlanFinCode.DisplayMember = "res_party";
            cbPlanFinCode.ValueMember = "fin_code";
            cbPlanFinCode.SelectedIndex = -1;

            #endregion

            currentAccount = new Account();
            currentAccount = accDB.GetByAccount(_selectedAccount);
            currentPat = new Pat();
            currentPat = patDB.GetByAccount(_selectedAccount);

            #region Setup Insurance Company Combobox
            insCompanies = insCompanyRepository.GetAll(true).ToList();

            MTGCComboBoxItem[] insItems = new MTGCComboBoxItem[insCompanies.Count];
            int i = 0;

            foreach (InsCompany ins in insCompanies)
            {
                insItems[i] = new MTGCComboBoxItem(ins.name, ins.code, ins.addr1 ?? "", ins.citystzip ?? "");
                i++;
            }

            cbInsCode.ColumnNum = 4;
            cbInsCode.GridLineHorizontal = true;
            cbInsCode.GridLineVertical = true;
            cbInsCode.ColumnWidth = "200;75;75;75";
            //cbInsCode.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown;
            cbInsCode.SelectedIndex = -1;
            cbInsCode.Items.Clear();
            cbInsCode.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            cbInsCode.Items.AddRange(insItems);
            #endregion

            //this.Text = $" {currentAccount.pat_name} - Demographics";

            tbBannerName.Text = currentAccount.pat_name;
            tbBannerAccount.Text = _selectedAccount;
            tbBannerDob.Text = currentPat.dob_yyyy.Value.ToShortDateString();
            tbBannerSex.Text = currentPat.sex;
            tbBannerMRN.Text = currentAccount.mri;

            if (!Str.ParseName(currentAccount.pat_name, out string strLname, out string strFname, out string strMidName, out string strSuffix))
            {
                Log.Instance.Debug($"Entering");
                Log.Instance.Warn("Error parsing patient name. Parsed data will not be shown.");
                DemoStatusMessages.AppendText("Error parsing patient name. Parsed data will not be shown.");
                DemoStatusMessages.AppendText(Environment.NewLine);
                DemoStatusMessages.BackColor = Color.Yellow;
            }

            if (!Str.ParseCityStZip(currentPat.city_st_zip, out string strCity, out string strState, out string strZip))
            {
                Log.Instance.Debug($"Entering");
                Log.Instance.Warn("Error parsing City St Zip. Data will not be shown.");
                DemoStatusMessages.AppendText("Error parsing City St Zip. Data will not be shown.");
                DemoStatusMessages.AppendText(Environment.NewLine);
                DemoStatusMessages.BackColor = Color.Yellow;
            }

            tbLastName.Text = strLname;
            tbLastName.BackColor = Color.White;
            tbFirstName.Text = strFname;
            tbFirstName.BackColor = Color.White;
            tbMiddleName.Text = strMidName;
            tbMiddleName.BackColor = Color.White;
            tbSuffix.Text = strSuffix;
            tbSuffix.BackColor = Color.White;
            tbAddress1.Text = currentPat.pat_addr1;
            tbAddress1.BackColor = Color.White;
            tbAddress2.Text = currentPat.pat_addr2;
            tbAddress2.BackColor = Color.White;
            tbCity.Text = currentPat.pat_city != null ? currentPat.pat_city : strCity;
            tbCity.BackColor = Color.White;
            cbState.SelectedValue = currentPat.pat_state != null ? currentPat.pat_state : strState;
            cbState.BackColor = Color.White;
            tbZipcode.Text = currentPat.pat_zip != null ? currentPat.pat_zip : strZip;
            tbZipcode.BackColor = Color.White;
            tbPhone.Text = currentPat.pat_phone;
            tbPhone.BackColor = Color.White;
            tbSSN.Text = currentAccount.ssn;
            tbSSN.BackColor = Color.White;
            tbEmailAddress.Text = currentPat.pat_email;
            tbEmailAddress.BackColor = Color.White;
            tbDateOfBirth.Text = currentPat.dob_yyyy.Value.ToString("MM/dd/yyyy");
            tbDateOfBirth.BackColor = Color.White;
            cbSex.SelectedValue = currentPat.sex;
            cbSex.BackColor = Color.White;
            cbMaritalStatus.SelectedValue = currentPat.pat_marital != null ? currentPat.pat_marital : "";
            cbMaritalStatus.BackColor = Color.White;

            if (!Str.ParseName(currentPat.guarantor, out strLname, out strFname, out strMidName, out strSuffix))
            {
                Log.Instance.Info($"Guarantor name could not be parsed. {currentPat.guarantor}");
                Log.Instance.Warn("Error parsing guarantor name. Name may be blank. Parsed data will not be shown.");
                DemoStatusMessages.AppendText("Error parsing guarantor name. Name may be blank. Parsed data will not be shown.");
                DemoStatusMessages.AppendText(Environment.NewLine);
                DemoStatusMessages.BackColor = Color.Yellow;
            }

            tbGuarantorLastName.Text = strLname;
            tbGuarFirstName.Text = strFname;
            tbGuarMiddleName.Text = strMidName;
            tbGuarSuffix.Text = strSuffix;
            tbGuarantorAddress.Text = currentPat.guar_addr;
            tbGuarCity.Text = currentPat.guar_city;
            cbGuarState.SelectedValue = currentPat.guar_state != null ? currentPat.guar_state : "";
            tbGuarZip.Text = currentPat.guar_zip;
            tbGuarantorPhone.Text = currentPat.guar_phone;
            cbGuarantorRelation.SelectedValue = currentPat.relation != null ? currentPat.relation : "";

            
            currentIns = new List<Ins>();
            currentIns = insDB.GetByAccount(_selectedAccount).ToList();

            if (currentIns.Count > 0)
            {
                DataGridViewButtonColumn deleteCol = new DataGridViewButtonColumn
                {
                    Name = "delete",
                    HeaderText = "Delete",
                    Text = "Delete",
                    UseColumnTextForButtonValue = true,
                    Width = 50
                };
                this.dgvInsurance.Columns.Add(deleteCol);

                var insBindingList = new BindingList<Ins>(currentIns);
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

                dgvInsurance.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
                //dgvInsurance.Columns[nameof(Ins.HolderName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

                cbInsOrder.SelectedIndex = 0;
                cbHolderState.SelectedIndex = 0;
                cbHolderSex.SelectedIndex = 0;

                dgvInsurance.ClearSelection();
            }

        }


        #endregion

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

            currentAccount.pat_name = string.Format("{0} {1},{2} {3}", tbLastName.Text, tbSuffix.Text, tbFirstName.Text, tbMiddleName.Text);
            currentAccount.ssn = tbSSN.Text;

            accDB.Update(currentAccount);

            currentPat.city_st_zip = string.Format("{0}, {1} {2}", tbCity.Text, cbState.SelectedValue.ToString(), tbZipcode.Text);
            currentPat.pat_city = tbCity.Text;
            currentPat.pat_addr1 = tbAddress1.Text;
            currentPat.pat_addr2 = tbAddress2.Text;
            currentPat.pat_email = tbEmailAddress.Text;
            currentPat.pat_marital = cbMaritalStatus.SelectedValue.ToString();
            currentPat.pat_phone = tbPhone.Text;
            currentPat.pat_state = cbState.SelectedValue.ToString();
            currentPat.pat_zip = tbZipcode.Text;
            currentPat.pat_full_name = string.Format("{0},{1} {2}", tbLastName.Text, tbFirstName.Text, tbMiddleName.Text);
            currentPat.dob_yyyy = DateTime.Parse(tbDateOfBirth.Text);
            currentPat.guarantor = string.Format("{0} {1},{2} {3}",
                tbGuarantorLastName.Text, tbGuarSuffix.Text, tbGuarFirstName.Text, tbGuarMiddleName.Text);
            currentPat.guar_addr = tbGuarantorAddress.Text;
            currentPat.guar_city = tbGuarCity.Text;
            currentPat.guar_phone = tbGuarantorPhone.Text;
            currentPat.guar_state = cbGuarState.SelectedValue.ToString();
            currentPat.guar_zip = tbGuarZip.Text;
            currentPat.g_city_st = string.Format("{0}, {1} {2}", tbGuarCity.Text, cbGuarState.SelectedValue.ToString(), tbGuarZip.Text);
            currentPat.sex = cbSex.SelectedValue.ToString();
            currentPat.relation = cbGuarantorRelation.SelectedValue.ToString();
            currentPat.ssn = tbSSN.Text;

            patDB.SaveAll(currentPat);

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
                MessageBox.Show("Insurance Order is not a valid selection.");
                Log.Instance.Debug("Insurance Order is not a valid selection.");
                return;
            }

            if (currentIns.Count - 1 < selectedIns)
            {
                //This is a new record
                currentIns.Insert(selectedIns, new Ins());
                currentIns[selectedIns].Account = _selectedAccount;
            }

            currentIns[selectedIns].CertSSN = tbCertSSN.Text;
            currentIns[selectedIns].GroupName = tbGroupName.Text;
            currentIns[selectedIns].GroupNumber = tbGroupNumber.Text;
            currentIns[selectedIns].HolderAddress = tbHolderAddress.Text;
            currentIns[selectedIns].HolderCityStZip = string.Format("{0}, {1} {2}",
                tbHolderCity.Text,
                cbHolderState.SelectedValue.ToString(),
                tbHolderZip.Text);
            currentIns[selectedIns].HolderBirthDate = DateTime.Parse(tbHolderDOB.Text);
            currentIns[selectedIns].HolderFirstName = tbHolderFirstName.Text;
            currentIns[selectedIns].HolderLastName = tbHolderLastName.Text;
            currentIns[selectedIns].HolderMiddleName = tbHolderMiddleName.Text;
            currentIns[selectedIns].HolderName = string.Format("{0},{1} {2}",
                tbHolderLastName.Text, tbHolderFirstName.Text, tbHolderMiddleName.Text);
            currentIns[selectedIns].HolderSex = cbHolderSex.SelectedValue.ToString();
            currentIns[selectedIns].PolicyNumber = tbPolicyNumber.Text;
            currentIns[selectedIns].PlanAddress1 = tbPlanAddress.Text;
            currentIns[selectedIns].PlanAddress2 = tbPlanAddress2.Text;
            currentIns[selectedIns].PlanName = tbPlanName.Text;
            currentIns[selectedIns].PlanCityState = tbPlanCitySt.Text;
            currentIns[selectedIns].Relation = cbInsRelation.SelectedValue.ToString();
            currentIns[selectedIns].Coverage = cbInsOrder.SelectedValue.ToString();
            currentIns[selectedIns].FinCode = cbPlanFinCode.SelectedValue.ToString();

            currentIns[selectedIns].InsCode = cbInsCode.SelectedItem.Col2;

            //call method to update the record in the database
            if (currentIns[selectedIns].rowguid == Guid.Empty)
            {
                insDB.Add(currentIns[selectedIns]);
            }
            else
            {
                if (!InEditMode)
                {
                    if (MessageBox.Show(string.Format("You are adding a new {0} insurance {1}. This will replace the existing {2} insurance. OK to update?",
                        cbInsOrder.SelectedValue, tbPlanName.Text, cbInsOrder.SelectedValue), "Existing Insurance", MessageBoxButtons.YesNo) == DialogResult.Yes)
                    {
                        insDB.Update(currentIns[selectedIns]);
                    }
                    else
                    {
                        return;
                    }
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
                    MessageBox.Show("Insurance Order is not a valid selection.");
                    Log.Instance.Debug("Insurance Order is not a valid selection.");
                    return;
                }

                if (MessageBox.Show(string.Format("Delete {0} insurance {1} for this patient?", currentIns[selectedIns].Coverage, currentIns[selectedIns].PlanName),
                    "Delete Insurance", MessageBoxButtons.YesNo)
                    == DialogResult.Yes)
                {
                    if (insDB.Delete(currentIns[selectedIns]))
                        currentIns.RemoveAt(selectedIns);
                }
                insGridSource.ResetBindings(false);
                ClearInsEntryFields();

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
                Log.Instance.Info($"Insurance holder name could not be parsed. {dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderName)].Value.ToString()}");
                MessageBox.Show("Error while parsing name into its parts. Name will not be shown in fields.");
            }

            tbHolderLastName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderLastName)].Value?.ToString() ?? lname;
            tbHolderFirstName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderFirstName)].Value?.ToString() ?? fname;
            tbHolderMiddleName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderMiddleName)].Value?.ToString() ?? mname;
            tbHolderAddress.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderAddress)].Value != null ? dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderAddress)].Value.ToString() : "";

            if (dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value != null)
            {
                if (!Str.ParseCityStZip(dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value.ToString(),
                    out string city, out string state, out string zip))
                {
                    Log.Instance.Info($"Insurance holder city, st, zip could not be parsed. {dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderCityStZip)].Value.ToString()}");
                    MessageBox.Show("Error parsing City, st zip into its parts. Will not be shown in fields.");
                }
                tbHolderCity.Text = city;
                cbHolderState.SelectedValue = state;
                tbHolderZip.Text = zip;
            }

            cbHolderSex.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderSex)].Value?.ToString();
            tbHolderDOB.Text = DateTime.Parse(dgvInsurance.SelectedRows[0].Cells[nameof(Ins.HolderBirthDate)].Value?.ToString()).ToString(@"MM/dd/yyyy");
            cbInsRelation.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.Relation)].Value?.ToString();

            // set ins Code combo box
            cbInsCode.ItemSelect(2, dgvInsurance.SelectedRows[0].Cells[nameof(Ins.InsCode)].Value?.ToString(), false, true);

            tbPlanName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanName)].Value?.ToString();
            tbPlanAddress.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanAddress1)].Value?.ToString();
            tbPlanAddress2.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanAddress2)].Value?.ToString();
            tbPlanCitySt.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PlanCityState)].Value?.ToString();
            tbPolicyNumber.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.PolicyNumber)].Value?.ToString();
            tbGroupNumber.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.GroupNumber)].Value?.ToString();
            tbGroupName.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.GroupName)].Value?.ToString();
            tbCertSSN.Text = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.CertSSN)].Value?.ToString();
            cbPlanFinCode.SelectedValue = dgvInsurance.SelectedRows[0].Cells[nameof(Ins.FinCode)].Value?.ToString();

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

        private void CbInsCode_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (cbInsCode.SelectedIndex >= 0)
            {
                string insCode = ((MTGCComboBoxItem)cbInsCode.SelectedItem).Col2;
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
            dgvCharges.DataSource = chrgdb.GetByAccount(SelectedAccount, ckShowCreditedChrg.Checked);

            foreach (DataGridViewColumn col in dgvCharges.Columns)
            {
                col.Visible = false;
            }

            dgvCharges.Columns[nameof(Chrg.credited)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.cdm)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.cdm_desc)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.qty)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.net_amt)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.service_date)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.status)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.comment)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.chrg_num)].Visible = true;
            dgvCharges.Columns[nameof(Chrg.invoice)].Visible = true;

            dgvCharges.Columns[nameof(Chrg.net_amt)].DefaultCellStyle.Format = "N2";
            dgvCharges.Columns[nameof(Chrg.net_amt)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCharges.Columns[nameof(Chrg.calc_amt)].DefaultCellStyle.Format = "N2";
            dgvCharges.Columns[nameof(Chrg.calc_amt)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvCharges.Columns[nameof(Chrg.qty)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvCharges.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvCharges.Columns[nameof(Chrg.cdm_desc)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
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
                var chrg = chrgdb.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.chrg_num)].Value.ToString()));

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
                var chrg = chrgdb.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.chrg_num)].Value.ToString()));

                dgvChrgDetail.DataSource = chrg.ChrgDetails;

                foreach (DataGridViewColumn col in dgvChrgDetail.Columns)
                {
                    col.Visible = false;
                }

                dgvChrgDetail.Columns[nameof(ChrgDetails.cpt4)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.bill_type)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.diagnosis_code_ptr)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.modi)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.modi2)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.revcode)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.type)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.bill_method)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.order_code)].Visible = true;
                dgvChrgDetail.Columns[nameof(ChrgDetails.amount)].Visible = true;

                dgvChrgDetail.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                dgvChrgDetail.Columns[nameof(ChrgDetails.cpt4)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                dgvChrgDetail.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                dgvChrgDetail.Columns[nameof(ChrgDetails.amount)].DefaultCellStyle.Format = "N2";

            }
        }

        private void DgvCharges_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (e.ColumnIndex == dgvCharges.Columns[nameof(Chrg.credited)].Index && e.Value.ToString() == "True")
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
                    row.Cells[nameof(Chrg.chrg_num)].Value.ToString()),
                    "Credit Charge", "");

                if (prompt.ReturnCode == DialogResult.OK)
                {
                    chrgdb.CreditCharge(Convert.ToInt32(row.Cells[nameof(Chrg.chrg_num)].Value.ToString()), prompt.Text);
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
            ChargeEntryForm frm = new ChargeEntryForm(currentAccountSummary);

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
                if(MessageBox.Show("Changes were made to diagnosis pointers. Save changes?", "Save Changes?", 
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

                string dxPtr = row.Cells["diagnosis_code_ptr"].Value.ToString();

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
                    foreach (PatDiag diag in currentPat.Diagnoses)
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

                    DiagnosisPointerDGV.DataSource = dt;
                    DiagnosisPointerDGV.Columns["DxCode"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill; 
                    DiagnosisPointerDGV.AutoResizeColumns();
                }
                catch(Exception ex)
                {
                    Log.Instance.Error(ex.Message);
                    MessageBox.Show("Error loading Diagnosis Code Pointer. Exception has been logged. Report to Administrator.");
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

            ChrgDetails amt = new ChrgDetails();

            amt.uri = Convert.ToInt32(dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.uri)].Value);
            amt.amount = Convert.ToDouble(dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.amount)].Value?? 0.0);
            amt.bill_method = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.bill_method)].Value?.ToString();
            amt.bill_type = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.bill_type)].Value?.ToString();
            amt.chrg_num = Convert.ToDouble(dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.chrg_num)].Value ?? 0.0);
            amt.cpt4 = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.cpt4)].Value?.ToString();
            amt.diagnosis_code_ptr = dxPtr;
            amt.modi = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.modi)].Value?.ToString();
            amt.modi2 = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.modi2)].Value?.ToString();
            amt.mt_req_no = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.mt_req_no)].Value?.ToString();
            amt.order_code = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.order_code)].Value?.ToString();
            amt.pointer_set = true;
            amt.revcode = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.revcode)].Value?.ToString();
            amt.type = dgvChrgDetail.SelectedRows[0].Cells[nameof(amt.type)].Value?.ToString();

            AmtRepository amtRepository = new AmtRepository(Helper.ConnVal);
            try
            {
                amtRepository.Update(amt);
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex.Message);
                MessageBox.Show("Error updating charge detail record. Please try again. If error continues, report error to Administrator.");
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

            tbTotalPayment.Text = currentAccountSummary.TotalPayments.ToString("c");
            tbTotalContractual.Text = currentAccountSummary.TotalContractual.ToString("c");
            tbTotalWriteOff.Text = currentAccountSummary.TotalWriteOff.ToString("c");
            tbTotalPmtAll.Text = (currentAccountSummary.TotalPayments + currentAccountSummary.TotalContractual + currentAccountSummary.TotalWriteOff).ToString("c");

            //payments = chkdb.GetByAccount(SelectedAccount);
            //Log.Instance.Debug($"GetPayments returned {payments.Count} rows.");
            dgvPayments.DataSource = chkdb.GetByAccount(SelectedAccount);

            foreach (DataGridViewColumn col in dgvPayments.Columns)
            {
                col.Visible = false;
            }

            dgvPayments.Columns[nameof(Chk.amt_paid)].Visible = true;
            dgvPayments.Columns[nameof(Chk.chk_date)].Visible = true;
            dgvPayments.Columns[nameof(Chk.chk_no)].Visible = true;
            dgvPayments.Columns[nameof(Chk.comment)].Visible = true;
            dgvPayments.Columns[nameof(Chk.contractual)].Visible = true;
            dgvPayments.Columns[nameof(Chk.date_rec)].Visible = true;
            dgvPayments.Columns[nameof(Chk.invoice)].Visible = true;
            dgvPayments.Columns[nameof(Chk.source)].Visible = true;
            dgvPayments.Columns[nameof(Chk.status)].Visible = true;
            dgvPayments.Columns[nameof(Chk.write_off)].Visible = true;
            dgvPayments.Columns[nameof(Chk.write_off_code)].Visible = true;
            dgvPayments.Columns[nameof(Chk.w_off_date)].Visible = true;
            
            dgvPayments.Columns[nameof(Chk.amt_paid)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(Chk.amt_paid)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(Chk.contractual)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(Chk.contractual)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(Chk.write_off)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(Chk.write_off)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

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
                var chk = chkdb.GetById(Convert.ToInt32(row.Cells[nameof(Chk.pay_no)].Value.ToString()));

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
            var dictRecords = dictDxDb.Search(txtSearchDx.Text, currentAccount.trans_date.GetValueOrDefault(DateTime.Now));

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
                    MessageBox.Show("Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                    return;
                }

                int maxNo = 0;
                if (dxBindingList.Count > 0)
                    maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                if (maxNo >= 9)
                {
                    MessageBox.Show("Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
                        MessageBox.Show("Diagnosis already entered for this account", "Record Exists", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
                        txtDxQuickAdd.Text = "";
                        return;
                    }

                    var record = dictDxDb.GetByCode(txtDxQuickAdd.Text, currentAccount.trans_date.GetValueOrDefault(DateTime.Now));
                    if (record != null)
                    {
                        //this is a valid entry
                        int maxNo = 0;
                        if (dxBindingList.Count > 0)
                            maxNo = dxBindingList.Max<PatDiag>(n => n.No);

                        if (maxNo >= 9)
                        {
                            MessageBox.Show("Maximum number of diagnosis reached.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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

            currentPat.Diagnoses = dxBindingList.ToList<PatDiag>();

            if (patDB.SaveDiagnoses(currentPat) == true)
                MessageBox.Show("Diagnoses updated successfully.");
            else
                MessageBox.Show("Diagnosis update failed.");
        }

        private void dgvDiagnosis_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        #endregion

        #region NotesTab

        private void LoadNotes()
        {
            Log.Instance.Trace("Entering");
            notes = notesdb.GetByAccount(currentAccountSummary.account).ToList();
            tbNotesDisplay.Text = "";
            tbNotesDisplay.BackColor = Color.AntiqueWhite;
            foreach(Notes note in notes)
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
            InputBoxResult prompt = InputBox.Show("Enter note:", "New Note");
            Notes note = new Notes();

            if (prompt.ReturnCode == DialogResult.OK)
            {
                note.account = currentAccountSummary.account;
                note.comment = prompt.Text;
                notesdb.Add(note);
                //reload notes to pick up changes
                LoadNotes();
            }

        }

        #endregion

        #region BillingActivityTab

        private void LoadBillingActivity()
        {
            Log.Instance.Trace("Entering");

            //billingActivities = dbBillingActivity.GetByAccount(currentAccountSummary.account);
            dgvBillActivity.DataSource = dbBillingActivity.GetByAccount(currentAccountSummary.account);
           
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
                MessageBox.Show("A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
            }
        }

        private void ChangeDateOfServiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
        }

        private void ChangeFinancialClassToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
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
                if (subctrl is TextBox)
                    ((TextBox)subctrl).TextChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is CheckBox)
                    ((CheckBox)subctrl).CheckedChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is RadioButton)
                    ((RadioButton)subctrl).CheckedChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is ListBox)
                    ((ListBox)subctrl).SelectedIndexChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is ComboBox)
                    ((ComboBox)subctrl).SelectedIndexChanged +=
                        new EventHandler(InputControls_OnChange);
                else if (subctrl is MaskedTextBox)
                    ((MaskedTextBox)subctrl).TextChanged +=
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

        }

        private void ClearHoldStatusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (currentAccount.status != "HOLD")
            {
                MessageBox.Show("Account is not in HOLD status. This will only set account status from HOLD to NEW. It will not change any billing information.");
                return;
            }

            //Reason must be entered for changing status

            InputBoxResult prompt = InputBox.Show("Enter reason for setting status back to New:", "New Note");
            Notes note = new Notes();

            if (prompt.ReturnCode == DialogResult.OK)
            {
                note.account = currentAccountSummary.account;
                note.comment = prompt.Text;
                notesdb.Add(note);
                //reload notes to pick up changes
                LoadNotes();
            }

            //AccountRepository accDB = new AccountRepository();
            currentAccount.status = "NEW";
            accDB.Update(currentAccount);

        }

    }
}
