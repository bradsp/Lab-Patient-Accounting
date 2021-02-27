using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class NewAccountForm : Form
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
                acc.account = AccountNo.Text;
                acc.pat_name = string.Format("{0},{1} {2}", LastName.Text, FirstName.Text, MiddleName.Text);
                acc.Pat.pat_full_name = string.Format("{0},{1} {2}", LastName.Text, FirstName.Text, MiddleName.Text);
                acc.Pat.dob_yyyy = Convert.ToDateTime(DateOfBirth.Text);
                acc.trans_date = Convert.ToDateTime(ServiceDate.Text);
                acc.Pat.account = AccountNo.Text;
                acc.Pat.sex = PatientSex.SelectedValue.ToString();
                acc.fin_code = FinancialClass.SelectedValue;

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

            MTGCComboBoxItem[] finItems = new MTGCComboBoxItem[fins.Count];
            int i = 0;

            foreach (Fin fin in fins)
            {
                finItems[i] = new MTGCComboBoxItem(fin.fin_code, fin.res_party);
                i++;
            }

            FinancialClass.ColumnNum = 2;
            FinancialClass.GridLineHorizontal = true;
            FinancialClass.GridLineVertical = true;
            FinancialClass.ColumnWidth = "75;200";
            //cbInsCode.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown;
            FinancialClass.SelectedIndex = -1;
            FinancialClass.Items.Clear();
            FinancialClass.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            FinancialClass.Items.AddRange(finItems);
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
