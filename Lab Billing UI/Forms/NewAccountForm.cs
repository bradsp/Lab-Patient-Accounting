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
    public partial class NewAccountForm : BaseForm
    {

        public string CreatedAccount;
        private readonly NumberRepository numberRepository = new NumberRepository(Program.AppEnvironment);
        private readonly AccountRepository accountRepository = new AccountRepository(Program.AppEnvironment);
        private readonly FinRepository finRepository = new FinRepository(Program.AppEnvironment);
        private Timer _timer;
        private int _timerInterval = 650;

        public NewAccountForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
            _timer = new Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(clientTextBox_KeyUpDone);
        }

        private void AddAccount_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if(!IsValid())
            {
                return;
            }

            //if account number is blank - assign a new account number
            if (accountNoTextBox.Text == "")
            {
                //get new account number
                accountNoTextBox.Text = string.Format("D{0}", numberRepository.GetNumber("account"));
            }
            //MessageBox.Show("Account Number " + AccountNoTextBox.Text + " assigned.");
            if (accountNoTextBox.Text == "D-1")
            {
                //error assigning account number
                MessageBox.Show("Error assigning account number. Process aborted.");
                this.DialogResult = DialogResult.Abort;
            }
            else
            {
                CreatedAccount = accountNoTextBox.Text;

                //create the account

                Account acc = new Account();
                acc.AccountNo = accountNoTextBox.Text;
                acc.PatLastName = lastNameTextBox.Text;
                acc.PatFirstName = firstNameTextBox.Text;
                acc.PatMiddleName = middleNameTextBox.Text;
                acc.BirthDate = Convert.ToDateTime(dateOfBirthTextBox.Text);
                acc.TransactionDate = Convert.ToDateTime(serviceDateTextBox.Text);
                acc.Pat.AccountNo = accountNoTextBox.Text;
                acc.Sex = patientSexComboBox.SelectedValue.ToString();
                acc.FinCode = financialClassComboBox.SelectedValue.ToString();
                acc.ClientMnem = clientTextBox.Text;
                acc.Status = "NEW";

                accountRepository.AddAccount(acc);

                this.DialogResult = DialogResult.OK;
            }
            return;
        }

        private void NewAccountForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            patientSexComboBox.DataSource = Dictionaries.sexSource.ToList();
            patientSexComboBox.ValueMember = "Key";
            patientSexComboBox.DisplayMember = "Value";

            #region Setup Financial Code Combobox
            List<Fin> fins = DataCache.Instance.GetFins(); // finRepository.GetAll().ToList();

            financialClassComboBox.DisplayMember = nameof(Fin.Description);
            financialClassComboBox.ValueMember = nameof(Fin.FinCode);
            financialClassComboBox.DataSource = fins;
            financialClassComboBox.SelectedIndex = -1;

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
            if (string.IsNullOrWhiteSpace(lastNameTextBox.Text))
            {
                e.Cancel = true;
                lastNameTextBox.Focus();
                errorProvider1.SetError(lastNameTextBox, "Name should not be left blank!");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(lastNameTextBox, "");
            }
        }

        private void FirstName_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (string.IsNullOrWhiteSpace(firstNameTextBox.Text))
            {
                errorProvider1.SetError(firstNameTextBox, "Name should not be left blank!");
            }
            else
            {
                errorProvider1.SetError(firstNameTextBox, "");
            }
        }

        private void DateOfBirth_Validating(object sender, CancelEventArgs e)
        {
            //Log.Instance.Trace($"Entering");
            if (dateOfBirthTextBox.DateValue == DateTime.MinValue || string.IsNullOrEmpty(dateOfBirthTextBox.Text))
            {
                errorProvider1.SetError(dateOfBirthTextBox, "Enter a valid date of birth.");
            }
            else
            {
                errorProvider1.SetError(dateOfBirthTextBox, "");
            }
        }

        private void PatientSex_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (patientSexComboBox.SelectedIndex == -1 || patientSexComboBox.SelectedIndex == 0)
            {
                errorProvider1.SetError(patientSexComboBox, "Select a valid sex.");
            }
            else
            {
                errorProvider1.SetError(patientSexComboBox, "");
            }

        }

        private void ServiceDate_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (serviceDateTextBox.DateValue == DateTime.MinValue || string.IsNullOrEmpty(serviceDateTextBox.Text))
            {
                errorProvider1.SetError(serviceDateTextBox, "Enter a valid date of service.");
            }
            else
            {
                errorProvider1.SetError(serviceDateTextBox, "");
            }
        }

        private void FinancialClass_Validating(object sender, CancelEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            if (financialClassComboBox.SelectedValue == null)
            {
                errorProvider1.SetError(financialClassComboBox, "Select a valid financial class.");
            }
            else
            {
                errorProvider1.SetError(financialClassComboBox, "");
            }

        }

        private void clientTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void clientTextBox_KeyUpDone(object sender, EventArgs e)
        {
            _timer.Stop();

            if (!string.IsNullOrEmpty(clientTextBox.Text))
            {

                ClientLookupForm clientLookupForm = new ClientLookupForm();
                ClientRepository clientRepository = new ClientRepository(Program.AppEnvironment);
                clientLookupForm.Datasource = DataCache.Instance.GetClients();

                clientLookupForm.InitialSearchText = clientTextBox.Text;

                if (clientLookupForm.ShowDialog() == DialogResult.OK)
                {
                    clientTextBox.Text = clientLookupForm.SelectedValue;
                }
            }
        }

        private void clientTextBox_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(clientTextBox.Text))
            {
                errorProvider1.SetError(clientTextBox, "Select a valid client");
            }
            else
            {
                errorProvider1.SetError(clientTextBox, "");
            }
        }

        private bool IsValid()
        {
            this.ValidateChildren();

            if (financialClassComboBox.SelectedValue == null)
                errorProvider1.SetError(financialClassComboBox, "Select a financial class.");
            else
                errorProvider1.SetError(financialClassComboBox, "");

            bool success = true;

            if (errorProvider1.GetError(clientTextBox).Length > 0 ||
            errorProvider1.GetError(financialClassComboBox).Length > 0 ||
            errorProvider1.GetError(serviceDateTextBox).Length > 0 ||
            errorProvider1.GetError(patientSexComboBox).Length > 0 ||
            errorProvider1.GetError(dateOfBirthTextBox).Length > 0 ||
            errorProvider1.GetError(lastNameTextBox).Length > 0 ||
            errorProvider1.GetError(firstNameTextBox).Length > 0)
            {
                success = false;
            }


            return success;
        }
    }
}
