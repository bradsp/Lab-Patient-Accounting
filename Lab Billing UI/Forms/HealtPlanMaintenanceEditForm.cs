using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MetroFramework.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBilling.Forms
{
    public partial class HealthPlanMaintenanceEditForm : MetroForm
    {
        InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        InsCompany insCompany = new InsCompany();
        FinRepository finRepository = new FinRepository(Helper.ConnVal);
        private string selectedInsCode = null;


        public HealthPlanMaintenanceEditForm()
        {
            InitializeComponent();
        }

        public HealthPlanMaintenanceEditForm(string insCode) : this()
        {
            if (insCode == null)
                throw new ArgumentNullException();

            selectedInsCode = insCode;

        }

        private void HealtPlanMaintenaceEditForm_Load(object sender, EventArgs e)
        {
            //load combo boxes
            claimTypeComboBox.DropDownStyle = ComboBoxStyle.DropDown;

            finCodeComboBox.DisplayMember = nameof(Fin.fin_code);
            finCodeComboBox.ValueMember = nameof(Fin.fin_code);
            finCodeComboBox.DataSource = finRepository.GetAll();
            finCodeComboBox.SelectedIndex = -1;

            if (selectedInsCode == null)
            {
                //open in add mode

            }
            else
            {
                insCompany = insCompanyRepository.GetByCode(selectedInsCode);

                if (insCompany == null)
                {
                    MessageBox.Show($"Insurance Code {selectedInsCode} not found.");
                    return;
                }

                LoadData();

                //set permissions

                if (!Program.LoggedInUser.CanEditDictionary)
                {
                    Helper.SetControlsAccess(this.Controls, false);
                    cancelButton.Enabled = true;
                }

                insCodeTextBox.ReadOnly = true;
            }


        }

        private void LoadData()
        {
            insCodeTextBox.Text = insCompany.InsuranceCode;
            planNameTextBox.Text = insCompany.PlanName;
            address1TextBox.Text = insCompany.Address1;
            address2TextBox.Text = insCompany.Address2;
            planCityTextBox.Text = insCompany.City;
            planStateTextBox.Text = insCompany.State;
            planZipCodeTextBox.Text = insCompany.Zip;


            isMedicareHmoCheckBox.Checked = insCompany.IsMedicareHmo;
            allowOutpatientBillingCheckBox.Checked = insCompany.AllowOutpatientBilling;
            billAsJmcghCheckBox.Checked = insCompany.BillAsJmcgh;
            finCodeComboBox.SelectedValue = insCompany.FinancialCode ?? String.Empty;
            
            //finClassComboBox.SelectedValue = insCompany.FinancialClass;

            claimTypeComboBox.SelectedItem = insCompany.ClaimFilingIndicatorCode;
            string insType = string.Empty;

            if (insCompany.BillForm == "UB")
                insType = "Institutional";
            else if (insCompany.BillForm == "1500")
                insType = "Professional";
            else
                insType = string.Empty;
            
            insuranceTypeComboBox.SelectedItem = insType;
            providerNoQualifierTextBox.Text = insCompany.ProviderNoQualifer;
            providerNoTextBox.Text = insCompany.ProviderNo;
            payerNoTextBox.Text = insCompany.PayerNo;
            payorCodeTextBox.Text = insCompany.PayorCode;
            nThrivePayerNoTextBox.Text = insCompany.NThrivePayerNo;

            commentsTextBox.Text = insCompany.Comment;
            IsActiveCheckBox.Checked = !insCompany.IsDeleted;
            citystzipLabel.Text = insCompany.CityStateZip;

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            
            insCompany.PlanName = planNameTextBox.Text;
            insCompany.Address1 = address1TextBox.Text;
            insCompany.Address2 = address2TextBox.Text;
            insCompany.City = planCityTextBox.Text;
            insCompany.State = planStateTextBox.Text;
            insCompany.Zip = planZipCodeTextBox.Text;

            insCompany.IsMedicareHmo = isMedicareHmoCheckBox.Checked;
            insCompany.AllowOutpatientBilling = allowOutpatientBillingCheckBox.Checked;
            insCompany.BillAsJmcgh = billAsJmcghCheckBox.Checked;
            if(finCodeComboBox.SelectedValue != null)
                insCompany.FinancialCode = finCodeComboBox.SelectedValue.ToString();
            if(claimTypeComboBox.SelectedItem != null)
                insCompany.ClaimFilingIndicatorCode = claimTypeComboBox.SelectedItem.ToString();
            if (insuranceTypeComboBox.SelectedItem != null)
            {
                if (insuranceTypeComboBox.SelectedItem.ToString() == "Institutional")
                    insCompany.BillForm = "UB";
                else if (insuranceTypeComboBox.SelectedItem.ToString() == "Professional")
                    insCompany.BillForm = "1500";
                else
                    insCompany.BillForm = string.Empty;
            }
            insCompany.ProviderNoQualifer = providerNoQualifierTextBox.Text;
            insCompany.ProviderNo = providerNoTextBox.Text;
            insCompany.PayerNo = payerNoTextBox.Text;
            insCompany.PayorCode = payorCodeTextBox.Text;
            insCompany.NThrivePayerNo = nThrivePayerNoTextBox.Text;
            insCompany.Comment = commentsTextBox.Text;
            insCompany.IsDeleted = !IsActiveCheckBox.Checked;

            try
            {
                if (selectedInsCode == null)
                {
                    insCompany.InsuranceCode = insCodeTextBox.Text;
                    insCompanyRepository.Add(insCompany);
                }
                else
                {
                    insCompanyRepository.Update(insCompany);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("There was an error saving this record. Record not saved.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Logging.Log.Instance.Error(ex, "Error saving Insurance Company record.");
                this.DialogResult = DialogResult.Abort;
                return;
            }

            this.DialogResult = DialogResult.OK;
            return;
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            return;
        }
    }
}
