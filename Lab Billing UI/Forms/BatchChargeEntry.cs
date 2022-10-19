using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;

namespace LabBilling.Forms
{
    public partial class BatchChargeEntry : Form
    {
        private readonly ClientRepository clientRepository = new ClientRepository(Helper.ConnVal);
        private readonly CdmRepository cdmRepository = new CdmRepository(Helper.ConnVal);
        private readonly AccountRepository accountRepository = new AccountRepository(Helper.ConnVal);
        private List<BatchCharge> charges;
        private BindingSource chrgBindingSource;
        private Account currentAccount;

        public BatchChargeEntry()
        {
            InitializeComponent();
        }

        private void dgvBatchEntry_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void BatchChargeEntry_Load(object sender, EventArgs e)
        {
            charges = new List<BatchCharge>();
            chrgBindingSource = new BindingSource();
            chrgBindingSource.DataSource = charges;
            dgvBatchEntry.DataSource = chrgBindingSource;

        }

        private void dgvBatchEntry_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvBatchEntry_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvBatchEntry.Columns[e.ColumnIndex].Name == "CDM")
            {

                Cdm cdm = new Cdm();

                //look up cdm number and get amount
                cdm = cdmRepository.GetCdm(dgvBatchEntry[e.ColumnIndex, e.RowIndex].Value.ToString());

                MessageBox.Show(string.Format("CDM Description: {0}",cdm.Description));

                //if cdm is not valid, show an error


            }
        }

        private void SaveCharges_Click(object sender, EventArgs e)
        {
            //loop through rows to write charges

        }

        private void CDM_Leave(object sender, EventArgs e)
        {
            //look up cdm number and get amount
            Cdm cdm = cdmRepository.GetCdm(cdmTextBox.Text);

            if (cdm == null)
            {
                MessageBox.Show(string.Format("CDM {0} not found", cdmTextBox.Text));
                cdmTextBox.BackColor = Color.Red;
            }
            else
            {
                MessageBox.Show(string.Format("CDM Description: {0}", cdm.Description));
                cdmTextBox.BackColor = Color.White;
            }

            //if cdm is not valid, show an error

        }

        private void AddChargeToGrid_Click(object sender, EventArgs e)
        {

        }

        private void patientSearchButton_Click(object sender, EventArgs e)
        {
            PersonSearchForm personSearchForm = new PersonSearchForm();
            if(personSearchForm.ShowDialog() == DialogResult.OK)
            {
                var selectedAccount = personSearchForm.SelectedAccount;

                currentAccount = accountRepository.GetByAccount(selectedAccount);

                AccountNoTextBox.Text = currentAccount.AccountNo;
                PatientNameTextBox.Text = currentAccount.PatFullName;
                PatientSSNTextBox.Text = currentAccount.SocSecNo;
                PatientDOBTextBox.Text = currentAccount.Pat.BirthDate.ToString();
                clientTextBox.Text = currentAccount.ClientName;


            }

        }
    }
}
