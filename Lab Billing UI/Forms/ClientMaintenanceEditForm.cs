using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.DataAccess;
using LabBilling.Logging;

namespace LabBilling.Forms
{
    public partial class ClientMaintenanceEditForm : Form
    {
        public Client client;
        private ClientRepository clientRepository;
        private GLCodeRepository gLCodeRepository;
        public string SelectedClient { get; set; }

        public ClientMaintenanceEditForm()
        {
            InitializeComponent();
            clientRepository = new ClientRepository(Helper.ConnVal);
            gLCodeRepository = new GLCodeRepository(Helper.ConnVal);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            client.ClientMnem = tbClientMnem.Text;
            client.Name = tbClientName.Text;
            client.StreetAddress1 = tbAddress1.Text;
            client.StreetAddress2 = tbAddress2.Text;
            client.City = tbCity.Text;
            client.State = cbState != null ? cbState.SelectedValue.ToString() : String.Empty;
            client.ZipCode = tbZipcode.Text;
            client.County = cbCounty.SelectedValue != null ? cbCounty.SelectedValue.ToString() : String.Empty;
            client.ContactEmail = tbEmail.Text;
            client.Phone = tbPhone.Text;
            client.Fax = tbFax.Text;
            client.Contact = tbContact.Text;
            client.Comment = tbComment.Text;

            client.MroName = tbMROName.Text;
            client.MroStreetAddress1 = tbMROAddress.Text;
            client.MroCity = tbMROCity.Text;
            client.MroState = cbMROState.SelectedValue != null ? cbMROState.SelectedValue.ToString() : String.Empty;
            client.MroZipCode = tbMROZipcode.Text;

            client.FeeSchedule = cbFeeSched.SelectedValue != null ? cbFeeSched.SelectedValue.ToString() : String.Empty;
            client.ElectronicBillingType = cbEmrType.SelectedValue != null ? cbEmrType.SelectedValue.ToString() : String.Empty;
            client.GlCode = cbCostCenter.SelectedValue != null ? cbCostCenter.SelectedValue.ToString() : String.Empty;

            client.Type = cbClientType.SelectedValue != null ? Convert.ToInt16(cbClientType.SelectedValue.ToString()) : 0;
            client.FacilityNo = tbClientCode.Text;
            client.ShowDiscountedAmtOnBill = chkBillAtDiscount.Checked;
            client.IncludeOnChargeCodeReport = chkCCReport.Checked;
            //chkCOCForms.Checked = false;
            client.PrintInvoiceInDateOrder = chkDateOrder.Checked;
            client.DoNotBill = chkDoNotBill.Checked;
            client.PrintCptOnInvoice = chkPrintCPTonBill.Checked;

            DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void LoadClient()
        {
            tbClientMnem.Text = client.ClientMnem;
            tbClientName.Text = client.Name;
            tbAddress1.Text = client.StreetAddress1;
            tbAddress2.Text = client.StreetAddress2;
            tbCity.Text = client.City;
            tbContact.Text = client.Contact;
            tbComment.Text = client.Comment;
            tbEmail.Text = client.ContactEmail;
            tbPhone.Text = client.Phone;
            tbFax.Text = client.Fax;
            tbZipcode.Text = client.ZipCode;
            tbMROAddress.Text = client.MroStreetAddress1;
            tbMROCity.Text = client.MroCity;
            tbMROName.Text = client.MroName;
            tbMROZipcode.Text = client.MroZipCode;
            cbMROState.SelectedValue = client.MroState ?? String.Empty;
            cbState.SelectedValue = client.State ?? String.Empty;
            cbFeeSched.SelectedValue = client.FeeSchedule ?? String.Empty;
            cbEmrType.SelectedValue = client.ElectronicBillingType ?? String.Empty;
            cbCounty.SelectedValue = client.County ?? String.Empty;
            cbCostCenter.SelectedValue = client.GlCode ?? String.Empty;
            cbClientType.SelectedValue = client.Type.ToString();
            tbClientCode.Text = client.FacilityNo;
            chkBillAtDiscount.Checked = client.ShowDiscountedAmtOnBill;
            chkCCReport.Checked = client.IncludeOnChargeCodeReport;
            chkCOCForms.Checked = false;
            chkDateOrder.Checked = client.PrintInvoiceInDateOrder;
            chkDoNotBill.Checked = client.DoNotBill;
            chkPrintCPTonBill.Checked = client.PrintCptOnInvoice;

            tbClientMnem.ReadOnly = true;
        }

        private void ClientMaintenanceEditForm_Load(object sender, EventArgs e)
        {
            cbState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbState.DisplayMember = "Value";
            cbState.ValueMember = "Key";
            cbState.SelectedIndex = -1;

            cbClientType.DataSource = new BindingSource(Dictionaries.clientType, null);
            cbClientType.DisplayMember = "Value";
            cbClientType.ValueMember = "Key";
            cbClientType.SelectedIndex = -1;

            cbCounty.DataSource = new BindingSource(Dictionaries.counties, null);
            cbCounty.DisplayMember = "Value";
            cbCounty.ValueMember = "Key";
            cbCounty.SelectedIndex = -1;

            cbFeeSched.DataSource = new BindingSource(Dictionaries.feeSchedule, null);
            cbFeeSched.DisplayMember = "Value";
            cbFeeSched.ValueMember = "Key";
            cbFeeSched.SelectedIndex = -1;

            cbMROState.DataSource = new BindingSource(Dictionaries.stateSource, null);
            cbMROState.DisplayMember = "Value";
            cbMROState.ValueMember = "Key";
            cbMROState.SelectedIndex = -1;

            cbEmrType.DataSource = new BindingSource(Dictionaries.emrType, null);
            cbEmrType.DisplayMember = "Value";
            cbEmrType.ValueMember = "Key";
            cbEmrType.SelectedIndex = -1;

            cbCostCenter.DataSource = new BindingSource(gLCodeRepository.GetAll(), null);
            cbCostCenter.DisplayMember = "level_1";
            cbCostCenter.ValueMember = "level_1";
            cbCostCenter.SelectedIndex = -1;

            if(!string.IsNullOrEmpty(SelectedClient))
            {
                client = clientRepository.GetClient(SelectedClient);
                LoadClient();
            }
        }

        private void tbClientMnem_Leave(object sender, EventArgs e)
        {
            if (!String.IsNullOrEmpty(tbClientMnem.Text) && !tbClientMnem.ReadOnly)
            {
                var record = clientRepository.GetClient(tbClientMnem.Text);

                if (record != null)
                {
                    string message = $"Client with mnmen {tbClientMnem.Text} already exists. Edit this record instead?\n\nChoose Yes to load this client, or no to enter a new mnem.";
                    if (MessageBox.Show(message, "Client Exists",
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        client = record;
                        LoadClient();
                    }
                    else
                    {
                        tbClientMnem.Focus();
                    }
                }
                else
                {
                    client = new Client();
                }
            }
        }
    }
}
