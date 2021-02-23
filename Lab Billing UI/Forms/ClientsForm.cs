using LabBilling.DataAccess;
using LabBilling.Library;
using LabBilling.Logging;
using LabBilling.Models;
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
            int cliIndex = _clientList.FindIndex(c => c.cli_mnem == tbClientMnem.Text);
            tbAddress1.Text = _clientList[cliIndex].addr_1;
            tbAddress2.Text = _clientList[cliIndex].addr_2;
            tbCity.Text = _clientList[cliIndex].city;
            tbContact.Text = _clientList[cliIndex].contact;
            tbComment.Text = _clientList[cliIndex].comment;
            tbEmail.Text = _clientList[cliIndex].email;
            tbPhone.Text = _clientList[cliIndex].phone;
            tbFax.Text = _clientList[cliIndex].fax;
            tbZipcode.Text = _clientList[cliIndex].zip;
            tbMROAddress.Text = _clientList[cliIndex].mro_addr1;
            tbMROCity.Text = _clientList[cliIndex].mro_city;
            tbMROName.Text = _clientList[cliIndex].mro_name;
            tbMROZipcode.Text = _clientList[cliIndex].mro_zip;
            cbMROState.SelectedValue = _clientList[cliIndex].mro_st ?? "";
            cbState.SelectedValue = _clientList[cliIndex].st ?? "";
            cbFeeSched.SelectedValue = _clientList[cliIndex].fee_schedule ?? "";
            cbEmrType.SelectedValue = _clientList[cliIndex].electronic_billing_type ?? "";
            cbCounty.SelectedValue = _clientList[cliIndex].county ?? "";
            cbCostCenter.SelectedValue = _clientList[cliIndex].gl_code ?? "";
            cbClientType.SelectedValue = _clientList[cliIndex].type.ToString() ?? "";
            tbClientCode.Text = _clientList[cliIndex].facilityNo;
            chkBillAtDiscount.Checked = _clientList[cliIndex].bill_at_disc;
            chkCCReport.Checked = _clientList[cliIndex].IncludeOnChargeCodeReport;
            chkCOCForms.Checked = false;
            chkDateOrder.Checked = _clientList[cliIndex].PrintInvoiceInDateOrder;
            chkDoNotBill.Checked = _clientList[cliIndex].do_not_bill;
            chkPrintCPTonBill.Checked = _clientList[cliIndex].PrintCptOnInvoice;

        }

        private void dgvClients_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            Client client = _clientList[e.RowIndex];

            switch(e.ColumnIndex)
            {
                case 0:
                    e.Value = client.cli_mnem;
                    break;
                case 1:
                    e.Value = client.cli_nme;
                    break;
                case 2:
                    e.Value = client.deleted;
                    break;
                default:
                    break;
            }

            return;
        }
    }
}
