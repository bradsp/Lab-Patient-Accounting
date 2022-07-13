using System;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using MetroFramework.Forms;

namespace LabBilling.Forms
{
    public partial class BadDebtEditForm : MetroForm
    {

        public string SelectedRecord { get; set; }
        private BadDebt badDebt = new BadDebt();
        private readonly BadDebtRepository badDebtRepository = new BadDebtRepository(Helper.ConnVal);

        public BadDebtEditForm(string selectedGuid)
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();

            SelectedRecord = selectedGuid;
        }
        
        private void BadDebtEditForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (SelectedRecord is null || SelectedRecord == "")
            {
                MessageBox.Show("No record is selected.");
                return;
            }
            else
            {
                badDebt = badDebtRepository.GetRecord(SelectedRecord);
                LoadData();
            }

        }

        private void LoadData()
        {
            Log.Instance.Trace($"Entering");
            LastName.Text = badDebt.debtor_last_name;
            FirstName.Text = badDebt.debtor_first_name;
            Address1.Text = badDebt.st_addr_1;
            Address2.Text = badDebt.st_addr_2;
            City.Text = badDebt.city;
            State.Text = badDebt.State;
            ZipCode.Text = badDebt.Zip;
            Spouse.Text = badDebt.spouse;
            Phone.Text = badDebt.phone;
            SocSecNo.Text = badDebt.soc_security;
            License.Text = badDebt.license_number;
            Employment.Text = badDebt.employment;
            Remarks.Text = badDebt.remarks;
            Remarks2.Text = badDebt.remarks2;
            AccountNo.Text = badDebt.account_no;
            PatientName.Text = badDebt.patient_name;
            Misc.Text = badDebt.misc;
            ServiceDate.Text = badDebt.service_date?.ToString("MM/dd/yyyy");
            PaymentDate.Text = badDebt.payment_date?.ToString("MM/dd/yyyy");
            Balance.Text = badDebt.balance.ToString();
            DateEntered.Text = badDebt.date_entered?.ToString("MM/dd/yyyy");
            DateSent.Text = badDebt.date_sent?.ToString("MM/dd/yyyy");
        }

        private void ReadData()
        {
            Log.Instance.Trace($"Entering");
            badDebt.debtor_last_name = LastName.Text;
            badDebt.debtor_first_name = FirstName.Text;
            badDebt.st_addr_1 = Address1.Text;
            badDebt.st_addr_2 = Address2.Text;
            badDebt.city = City.Text;
            badDebt.State = State.Text;
            badDebt.Zip = ZipCode.Text;
            badDebt.spouse = Spouse.Text;
            badDebt.phone = Phone.Text;
            badDebt.soc_security = SocSecNo.Text;
            badDebt.license_number = License.Text;
            badDebt.employment = Employment.Text;
            badDebt.remarks = Remarks.Text;
            badDebt.remarks2 = Remarks2.Text;
            badDebt.account_no = AccountNo.Text;
            badDebt.patient_name = PatientName.Text;
            badDebt.misc = Misc.Text;
            badDebt.service_date = Convert.ToDateTime(ServiceDate.Text);
            badDebt.payment_date = Convert.ToDateTime(PaymentDate.Text);
            badDebt.balance = Convert.ToDouble(Balance.Text);
        }

        private void Save_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            ReadData();

            if(badDebtRepository.Update(badDebt))
            {
                this.DialogResult = DialogResult.OK;
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
            }

            return;
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            this.DialogResult = DialogResult.Cancel;
            return;
        }
    }
}
