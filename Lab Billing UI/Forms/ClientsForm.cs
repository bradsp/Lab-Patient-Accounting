using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class ClientsForm : Form
    {
        public ClientsForm()
        {
            Log.Instance.Trace($"Entering");
            InitializeComponent();
        }

        private readonly ClientRepository db = new ClientRepository(Helper.ConnVal);
        private readonly GLCodeRepository dbGL = new GLCodeRepository(Helper.ConnVal);

        private List<Client> _clientList = null;

        private void Clients_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            _clientList = db.GetAll().ToList();
            //_clientList.Sort();
            dgvClients.RowCount = _clientList.Count;

            Log.Instance.Debug("Timer Start Fetching Clients");
            dgvClients.Columns[0].Name = "cli_mnem";
            dgvClients.Columns[0].HeaderText = "Mnemonic";
            dgvClients.Columns.Add("cli_nme", "Name");
            dgvClients.Columns.Add(new DataGridViewCheckBoxColumn { HeaderText = "Deleted", Name = "deleted" });
            dgvClients.RowHeadersVisible = false;
            dgvClients.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvClients.Columns["cli_nme"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            Log.Instance.Debug("Timer stop Fetching Clients");

            cbState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbState.DisplayMember = "Value";
            cbState.ValueMember = "Key";
            cbState.SelectedIndex = 0;

            cbClientType.DataSource = new BindingSource(Dictionaries.clientType, null);
            cbClientType.DisplayMember = "Value";
            cbClientType.ValueMember = "Key";
            cbClientType.SelectedIndex = 0;

            cbCounty.DataSource = new BindingSource(Dictionaries.counties, null);
            cbCounty.DisplayMember = "Value";
            cbCounty.ValueMember = "Key";
            cbCounty.SelectedIndex = 0;

            cbFeeSched.DataSource = new BindingSource(Dictionaries.feeSchedule, null);
            cbFeeSched.DisplayMember = "Value";
            cbFeeSched.ValueMember = "Key";
            cbFeeSched.SelectedIndex = 0;

            cbMROState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbMROState.DisplayMember = "Value";
            cbMROState.ValueMember = "Key";
            cbMROState.SelectedIndex = 0;

            cbEmrType.DataSource = new BindingSource(Dictionaries.emrType, null);
            cbEmrType.DisplayMember = "Value";
            cbEmrType.ValueMember = "Key";
            cbEmrType.SelectedIndex = 0;

            cbCostCenter.DataSource = new BindingSource(dbGL.GetAll(), null);
            cbCostCenter.DisplayMember = "level_1";
            cbCostCenter.ValueMember = "level_1";
            cbCostCenter.SelectedIndex = -1;

            Log.Instance.Trace($"Exiting");
        }

        private void dgvClients_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {

            Log.Instance.Trace($"Entering");
            tbClientMnem.Text = dgvClients.SelectedRows[0].Cells["cli_mnem"].Value?.ToString();
            tbClientName.Text = dgvClients.SelectedRows[0].Cells["cli_nme"].Value?.ToString();
            int cliIndex = _clientList.FindIndex(c => c.ClientMnem == tbClientMnem.Text);
            tbAddress1.Text = _clientList[cliIndex].StreetAddress1;
            tbAddress2.Text = _clientList[cliIndex].StreetAddress2;
            tbCity.Text = _clientList[cliIndex].City;
            tbContact.Text = _clientList[cliIndex].Contact;
            tbComment.Text = _clientList[cliIndex].Comment;
            tbEmail.Text = _clientList[cliIndex].ContactEmail;
            tbPhone.Text = _clientList[cliIndex].Phone;
            tbFax.Text = _clientList[cliIndex].Fax;
            tbZipcode.Text = _clientList[cliIndex].ZipCode;
            tbMROAddress.Text = _clientList[cliIndex].MroStreetAddress1;
            tbMROCity.Text = _clientList[cliIndex].MroCity;
            tbMROName.Text = _clientList[cliIndex].MroName;
            tbMROZipcode.Text = _clientList[cliIndex].MroZipCode;
            cbMROState.SelectedValue = _clientList[cliIndex].MroState ?? "";
            cbState.SelectedValue = _clientList[cliIndex].State ?? "";
            cbFeeSched.SelectedValue = _clientList[cliIndex].FeeSchedule ?? "";
            cbEmrType.SelectedValue = _clientList[cliIndex].ElectronicBillingType ?? "";
            cbCounty.SelectedValue = _clientList[cliIndex].County ?? "";
            cbCostCenter.SelectedValue = _clientList[cliIndex].GlCode ?? "";
            cbClientType.SelectedValue = _clientList[cliIndex].Type.ToString() ?? "";
            tbClientCode.Text = _clientList[cliIndex].FacilityNo;
            chkBillAtDiscount.Checked = _clientList[cliIndex].ShowDiscountedAmtOnBill;
            chkCCReport.Checked = _clientList[cliIndex].IncludeOnChargeCodeReport;
            chkCOCForms.Checked = false;
            chkDateOrder.Checked = _clientList[cliIndex].PrintInvoiceInDateOrder;
            chkDoNotBill.Checked = _clientList[cliIndex].DoNotBill;
            chkPrintCPTonBill.Checked = _clientList[cliIndex].PrintCptOnInvoice;

        }

        private void dgvClients_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Client client = _clientList[e.RowIndex];

            switch(e.ColumnIndex)
            {
                case 0:
                    e.Value = client.ClientMnem;
                    break;
                case 1:
                    e.Value = client.Name;
                    break;
                case 2:
                    e.Value = client.IsDeleted;
                    break;
                default:
                    break;
            }

            return;
        }
    }
}
