﻿using LabBilling.Core.DataAccess;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Library;
using LabBilling.Core;
using NPOI.OpenXmlFormats.Vml;
using LabBilling.Core.Services;

namespace LabBilling.Forms
{
    public partial class InsMaintenanceUC : UserControl
    {
        private AccountService accountService;
        private DictionaryService dictionaryService;
        private List<InsCompany> insCompanies;
        private InsCompanyLookupForm insCoLookupForm;
        private List<string> changedControls;
        private bool _allowEditing;
        private Timer _timer;
        private const int timerInterval = 650;

        public Account CurrentAccount { get; set; }
        public Ins CurrentIns { get; set; }
        public event EventHandler<EventArgs> InsuranceChanged;
        public event EventHandler<AppErrorEventArgs> OnError;
        public InsCoverage Coverage { get; set; }

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

            accountService = new(Program.AppEnvironment);
            dictionaryService = new(Program.AppEnvironment);

            InitializeComponent();
            Coverage = coverage;
            changedControls = new();
            insCoLookupForm = new InsCompanyLookupForm();
            _timer = new Timer() { Enabled = false, Interval = timerInterval };
            _timer.Tick += insurancePlanTextBox_KeyUpDone;
            #region Setup Insurance Company Combobox
            insCompanies = DataCache.Instance.GetInsCompanies();
            #endregion

            InsRelationComboBox.DataSource = new BindingSource(Dictionaries.relationSource, null);
            InsRelationComboBox.DisplayMember = "Value";
            InsRelationComboBox.ValueMember = "Key";

            HolderSexComboBox.DataSource = new BindingSource(Dictionaries.sexSource, null);
            HolderSexComboBox.DisplayMember = "Value";
            HolderSexComboBox.ValueMember = "Key";

            PlanFinCodeComboBox.DataSource = DataCache.Instance.GetFins();
            PlanFinCodeComboBox.DisplayMember = nameof(Fin.Description);
            PlanFinCodeComboBox.ValueMember = nameof(Fin.FinCode);
            PlanFinCodeComboBox.SelectedIndex = -1;

            HolderStateComboBox.DataSource = new BindingSource(Dictionaries.stateSource, null);
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

            if (CurrentIns == null)
                return;

            if (CurrentIns.Coverage != Coverage)
                throw new ApplicationException($"Insurance coverage does not match form coverage. Insurance {CurrentIns.Coverage}, Form {Coverage}");

            HolderLastNameTextBox.Text = CurrentIns.HolderLastName;
            HolderFirstNameTextBox.Text = CurrentIns.HolderFirstName;
            HolderMiddleNameTextBox.Text = CurrentIns.HolderMiddleName;
            HolderAddressTextBox.Text = CurrentIns.HolderStreetAddress ?? "";

            HolderCityTextBox.Text = CurrentIns.HolderCity;
            HolderStateComboBox.SelectedValue = CurrentIns.HolderState;
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
                changedControls.Remove(ctrl.Name);
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
            CurrentIns.HolderState = HolderStateComboBox.SelectedValue.ToString();
            CurrentIns.HolderZip = HolderZipTextBox.Text;
            CurrentIns.HolderCityStZip = string.Format("{0}, {1} {2}",
                HolderCityTextBox.Text,
                HolderStateComboBox.SelectedValue == null ? "" : HolderStateComboBox.SelectedValue.ToString(),
                HolderZipTextBox.Text);

            if (CurrentIns.HolderCityStZip.Trim() == ",")
            {
                CurrentIns.HolderCityStZip = String.Empty;
            }

            if (HolderDOBTextBox.MaskCompleted)
                CurrentIns.HolderBirthDate = DateTime.Parse(HolderDOBTextBox.Text);

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
                accountService.SaveInsurance(CurrentIns);

                InsuranceChanged?.Invoke(this, EventArgs.Empty);

            }
            catch(Exception ex)
            {
                OnError?.Invoke(this, new AppErrorEventArgs() { ErrorLevel = AppErrorEventArgs.ErrorLevelType.Error, ErrorMessage = ex.Message });
                InsTabMessageTextBox.Text = "Error occured during save. Contact your administrator.";
            }
            //clear entry fields
            ClearInsEntryFields();
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

            var record = dictionaryService.GetInsCompany(code);

            if (record != null)
            {
                //this is a valid code

                PlanFinCodeComboBox.SelectedValue = record.FinancialCode ?? String.Empty;

                if (record.IsGenericPayor)
                {
                    PlanNameTextBox.Enabled = true;
                    PlanAddress2TextBox.Enabled = true;
                    PlanAddress2TextBox.Enabled = true;
                    PlanCityStTextBox.Enabled = true;
                    PlanFinCodeComboBox.Enabled = true;
                }
                else
                {
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
                insCoLookupForm.Datasource = insCompanies;
                insCoLookupForm.InitialSearchText = insurancePlanTextBox.Text;
                if (insCoLookupForm.ShowDialog() == DialogResult.OK)
                {
                    string insCode = insurancePlanTextBox.Text = insCoLookupForm.SelectedValue;
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
                    if (accountService.DeleteInsurance(CurrentIns))
                    {
                        InsuranceChanged?.Invoke(this, EventArgs.Empty);
                    }
                }
                catch(Exception ex)
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
                changedControls.Remove(ctrl.Name);
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
            HolderStateComboBox.SelectedValue = CurrentAccount.Pat.State;
            HolderZipTextBox.Text = CurrentAccount.Pat.ZipCode;
            HolderDOBTextBox.Text = CurrentAccount.BirthDate?.ToString("MM/dd/yyyy");
            HolderSexComboBox.SelectedValue = CurrentAccount.Sex;
            InsRelationComboBox.SelectedValue = "01";
        }

    }


}