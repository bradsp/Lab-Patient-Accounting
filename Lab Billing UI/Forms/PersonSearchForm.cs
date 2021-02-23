using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using LabBilling.DataAccess;
using LabBilling.Logging;
using LabBilling.Models;

namespace LabBilling.Forms
{
    public partial class PersonSearchForm : Form
    {
        List<AccountSearch> searchResults = new List<AccountSearch>();

        //public string NameSearch { get; set; }
        //public string MRNSearch { get; set; }
        //public string AccNoSearch { get; set; }
        //public string DOBSearch { get; set; }
        public string SelectedAccount { get; set; }
        private readonly AccountRepository accdb = new AccountRepository(Helper.ConnVal);

        public PersonSearchForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
        }

        private void SearchButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //make sure there are some values before running search
            string dobText = "";

            if (dobSearchText.MaskFull)
                dobText = dobSearchText.Text;

            searchResults = accdb.GetBySearch(txtLastName.Text, txtFirstName.Text, mrnSearchText.Text, ssnSearchText.Text, dobText, 
                cbSexSearch.SelectedIndex < 0 ? "" : cbSexSearch.SelectedValue.ToString(), accountSearchText.Text).ToList();

            var searchBindingList = new BindingList<AccountSearch>(searchResults);
            var source = new BindingSource(searchBindingList, null);

            PersonAccountResults.DataSource = source;
            PersonAccountResults.Columns[nameof(AccountSearch.mod_date)].Visible = false;
            PersonAccountResults.Columns[nameof(AccountSearch.mod_host)].Visible = false;
            PersonAccountResults.Columns[nameof(AccountSearch.mod_prg)].Visible = false;
            PersonAccountResults.Columns[nameof(AccountSearch.mod_user)].Visible = false;
            PersonAccountResults.Columns[nameof(AccountSearch.rowguid)].Visible = false;

            PersonAccountResults.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            PersonAccountResults.Columns[nameof(AccountSearch.Name)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        }

        private void PersonSearchForm_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //txtLastName.Text = NameSearch;
            //mrnSearchText.Text = MRNSearch;
            //accountSearchText.Text = AccNoSearch;
            //dobSearchText.Text = DOBSearch;
            cbSexSearch.DataSource = new BindingSource(Dictionaries.sexSource, null);
            cbSexSearch.ValueMember = "Key";
            cbSexSearch.DisplayMember = "Value";

            cbSexSearch.SelectedIndex = -1;
        }

        private void PersonAccountResults_DoubleClick(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            SelectButton_Click(sender, e);
        }

        private void SelectButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int selectedRows = PersonAccountResults.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                SelectedAccount = PersonAccountResults.SelectedRows[0].Cells[nameof(AccountSearch.Account)].Value.ToString();
                this.DialogResult = DialogResult.OK;
                return;
            }
            else
            {
                MessageBox.Show("No account selected.");
                this.DialogResult = DialogResult.Cancel;
                return;
            }
        }

        private void AddAccount_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            NewAccountForm naf = new NewAccountForm();

            if(naf.ShowDialog() == DialogResult.OK)
            {
                SelectedAccount = naf.CreatedAccount;
                this.DialogResult = DialogResult.OK;
                return;
            }
        }
    }
}
