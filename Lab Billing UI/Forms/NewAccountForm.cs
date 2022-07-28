using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Data;
using MetroFramework.Forms;

namespace LabBilling.Forms
{
    public partial class NewAccountForm : MetroForm
    {
        public NewAccountForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
        }

        public string CreatedAccount;
        private readonly NumberRepository numberRepository = new NumberRepository(Helper.ConnVal);
        private readonly AccountRepository accDB = new AccountRepository(Helper.ConnVal);
        private readonly FinRepository finRepository = new FinRepository(Helper.ConnVal);

        private void AddAccount_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (!this.ValidateChildren())
            {
                return;
            }

            //if account number is blank - assign a new account number
            if (AccountNo.Text == "")
            {
                //get new account number
                AccountNo.Text = string.Format("D{0}", numberRepository.GetNumber("account"));
            }
            MessageBox.Show("Account Number " + AccountNo.Text + " assigned.");
            if (AccountNo.Text == "D-1")
            {
                //error assigning account number
                MessageBox.Show("Error assigning account number. Process aborted.");
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                CreatedAccount = AccountNo.Text;

                //create the account

                Account acc = new Account();
                acc.AccountNo = AccountNo.Text;
                acc.PatFullName = string.Format("{0},{1} {2}", LastName.Text, FirstName.Text, MiddleName.Text);
                acc.Pat.PatFullName = string.Format("{0},{1} {2}", LastName.Text, FirstName.Text, MiddleName.Text);
                acc.Pat.BirthDate = Convert.ToDateTime(DateOfBirth.Text);
                acc.TransactionDate = Convert.ToDateTime(ServiceDate.Text);
                acc.Pat.AccountNo = AccountNo.Text;
                acc.Pat.Sex = PatientSex.SelectedValue.ToString();
                acc.FinCode = FinancialClass.SelectedValue.ToString(); ;

                accDB.AddAccount(acc);

                this.DialogResult = DialogResult.OK;
            }
            return;
        }

        private void NewAccountForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            PatientSex.DataSource = Dictionaries.sexSource.ToList();
            PatientSex.ValueMember = "Key";
            PatientSex.DisplayMember = "Value";

            #region Setup Financial Code Combobox
            List<Fin> fins = finRepository.GetAll().ToList();

            DataTable finDataTable = new DataTable(typeof(Fin).Name);

            finDataTable.Columns.Add("fin_code");
            finDataTable.Columns.Add("res_party");
            var values = new object[2];

            foreach (Fin fin in fins)
            {
                values[0] = fin.fin_code;
                values[1] = fin.res_party;
            }

            FinancialClass.Items.Clear();
            FinancialClass.DisplayMember = "res_party";
            FinancialClass.ValueMember = "fin_code";
            FinancialClass.DataSource = finDataTable;
            FinancialClass.SelectedIndex = -1;

            #endregion

        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            this.DialogResult = DialogResult.Cancel;
            return;
        }

        private void LastName_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (string.IsNullOrWhiteSpace(LastName.Text))
            {
                e.Cancel = true;
                LastName.Focus();
                errorProvider1.SetError(LastName, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(LastName, "");
            }
        }

        private void FirstName_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (string.IsNullOrWhiteSpace(FirstName.Text))
            {
                errorProvider1.SetError(FirstName, "Name should not be left blank!");
            }
            else
            {
                errorProvider1.SetError(FirstName, "");
            }
        }

        private void DateOfBirth_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (!DateOfBirth.MaskCompleted)
            {
                errorProvider1.SetError(DateOfBirth, "Enter a valid date of birth.");
            }
            else
            {
                errorProvider1.SetError(DateOfBirth, "");
            }
        }

        private void PatientSex_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (PatientSex.SelectedIndex == -1 || PatientSex.SelectedIndex == 0)
            {
                errorProvider1.SetError(PatientSex, "Select a valid sex.");
            }
            else
            {
                errorProvider1.SetError(PatientSex, "");
            }

        }

        private void ServiceDate_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (!ServiceDate.MaskCompleted)
            {
                errorProvider1.SetError(ServiceDate, "Enter a valid date of service.");
            }
            else
            {
                errorProvider1.SetError(ServiceDate, "");
            }

        }

        private void FinancialClass_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (FinancialClass.SelectedIndex == -1)
            {
                errorProvider1.SetError(FinancialClass, "Select a valid financial class.");
            }
            else
            {
                errorProvider1.SetError(FinancialClass, "");
            }

        }

    }
}
