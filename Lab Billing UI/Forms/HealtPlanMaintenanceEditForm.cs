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
    public partial class HealtPlanMaintenanceEditForm : MetroForm
    {
        InsCompanyRepository insCompanyRepository = new InsCompanyRepository(Helper.ConnVal);
        InsCompany insCompany = new InsCompany();
        private string selectedInsCode;

        public HealtPlanMaintenanceEditForm(string insCode)
        {
            if (insCode == null)
                throw new ArgumentNullException();

            selectedInsCode = insCode;

            InitializeComponent();
        }

        private void HealtPlanMaintenaceEditForm_Load(object sender, EventArgs e)
        {
            insCompany = insCompanyRepository.GetByCode(selectedInsCode);

            if (insCompany == null)
            {
                MessageBox.Show($"Insurance Code {selectedInsCode} not found.");
                return;
            }

            LoadData();

            //set permissions

            if(!Program.LoggedInUser.CanEditDictionary)
            {
                Helper.SetControlsAccess(this.Controls, false);
                cancelButton.Enabled = true;
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
            finCodeComboBox.SelectedValue = insCompany.FinancialCode;
            finClassComboBox.SelectedValue = insCompany.FinancialClass;
            claimTypeComboBox.SelectedValue = insCompany.ClaimFilingIndicatorCode;
            insuranceTypeComboBox.SelectedValue = insCompany.BillForm;
            providerNoQualifierTextBox.Text = insCompany.ProviderNoQualifer;
            providerNoTextBox.Text = insCompany.ProviderNo;
            payerNoTextBox.Text = insCompany.PayerNo;
            payorCodeTextBox.Text = insCompany.PayorCode;
            nThrivePayerNoTextBox.Text = insCompany.NThrivePayerNo;

            commentsTextBox.Text = insCompany.Comment;

        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Not implemented. No data saved.");
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
