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
        private CdmRepository cdmRepository;
        public string SelectedClient { get; set; }
        private BindingSource clientDiscountBindingSource = new BindingSource();
        private DataTable clientDiscountDataTable;
        private Cdm currentCdm = null;

        public ClientMaintenanceEditForm()
        {
            InitializeComponent();
            clientRepository = new ClientRepository(Program.AppEnvironment);
            gLCodeRepository = new GLCodeRepository(Program.AppEnvironment);
            cdmRepository = new CdmRepository(Program.AppEnvironment);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            client.IsDeleted = !activeCheckBox.Checked;
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
            client.MroStreetAddress2 = tbMROAddress2.Text;
            client.MroCity = tbMROCity.Text;
            client.MroState = cbMROState.SelectedValue != null ? cbMROState.SelectedValue.ToString() : String.Empty;
            client.MroZipCode = tbMROZipcode.Text;

            client.FeeSchedule = cbFeeSched.SelectedValue != null ? cbFeeSched.SelectedValue.ToString() : String.Empty;
            client.ElectronicBillingType = cbEmrType.SelectedValue != null ? cbEmrType.SelectedValue.ToString() : String.Empty;
            client.GlCode = cbCostCenter.SelectedValue != null ? cbCostCenter.SelectedValue.ToString() : String.Empty;
            client.BillMethod = billMethodComboBox.SelectedItem != null ? billMethodComboBox.SelectedItem.ToString() : String.Empty;

            client.Type = cbClientType.SelectedValue != null ? Convert.ToInt16(cbClientType.SelectedValue.ToString()) : 0;
            client.FacilityNo = tbClientCode.Text;
            client.ShowDiscountedAmtOnBill = chkBillAtDiscount.Checked;
            client.IncludeOnChargeCodeReport = chkCCReport.Checked;
            //chkCOCForms.Checked = false;
            client.PrintInvoiceInDateOrder = chkDateOrder.Checked;
            client.DoNotBill = chkDoNotBill.Checked;
            client.PrintCptOnInvoice = chkPrintCPTonBill.Checked;
            client.DefaultDiscount = (double)numDefaultDiscount.Value;

            if(clientDiscountDataTable != null)
                client.Discounts = Helper.ConvertToList<ClientDiscount>(clientDiscountDataTable);

            DialogResult = DialogResult.OK;
            return;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void LoadClient()
        {
            activeCheckBox.Checked = !client.IsDeleted;
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
            tbMROAddress2.Text = client.MroStreetAddress2;
            tbMROCity.Text = client.MroCity;
            tbMROName.Text = client.MroName;
            tbMROZipcode.Text = client.MroZipCode;
            cbMROState.SelectedValue = client.MroState ?? String.Empty;
            cbState.SelectedValue = client.State ?? String.Empty;
            cbFeeSched.SelectedValue = client.FeeSchedule ?? String.Empty;
            cbEmrType.SelectedValue = client.ElectronicBillingType ?? String.Empty;
            billMethodComboBox.SelectedItem = client.BillMethod ?? String.Empty;
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
            numDefaultDiscount.Value = (decimal)client.DefaultDiscount;


            clientDiscountDataTable = client.Discounts.ToDataTable();
            clientDiscountBindingSource.DataSource = clientDiscountDataTable;
            clientDiscountDataGrid.DataSource = clientDiscountBindingSource;

            clientDiscountDataTable.DefaultView.Sort = $"{nameof(ClientDiscount.Cdm)}";

            clientDiscountDataGrid.SetColumnsVisibility(false);

            clientDiscountDataGrid.Columns[nameof(ClientDiscount.Cdm)].Visible = true;
            clientDiscountDataGrid.Columns[nameof(ClientDiscount.CdmDescription)].Visible = true;
            clientDiscountDataGrid.Columns[nameof(ClientDiscount.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            clientDiscountDataGrid.Columns[nameof(ClientDiscount.PercentDiscount)].Visible = true;
            clientDiscountDataGrid.Columns[nameof(ClientDiscount.Price)].Visible = true;
            clientDiscountDataGrid.Columns[nameof(ClientDiscount.Price)].DefaultCellStyle.Format = "N2";
            clientDiscountDataGrid.Columns[nameof(ClientDiscount.PercentDiscount)].DefaultCellStyle.Format = "N2";

            interfaceMappingDataGrid.DataSource = client.Mappings;

            interfaceMappingDataGrid.SetColumnsVisibility(false);

            interfaceMappingDataGrid.Columns[nameof(Mapping.InterfaceName)].Visible = true;
            interfaceMappingDataGrid.Columns[nameof(Mapping.InterfaceAlias)].Visible = true;

            interfaceMappingDataGrid.Columns[nameof(Mapping.InterfaceAlias)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            interfaceMappingDataGrid.AutoResizeColumns();


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

        private void clientDiscountDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {




        }

        private void clientDiscountDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            switch (clientDiscountDataGrid.Columns[e.ColumnIndex].Name)
            {
                case nameof(ClientDiscount.Cdm):
                    //look up cdm number and get amount
                    currentCdm = cdmRepository.GetCdm(clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString());
                    if (currentCdm != null)
                    {
                        clientDiscountDataGrid[nameof(ClientDiscount.CdmDescription), e.RowIndex].Value = currentCdm.Description;
                    }
                    else
                    {
                        //pop a cdm search dialog
                        CdmLookupForm cdmLookupForm = new CdmLookupForm();
                        cdmLookupForm.Datasource = DataCache.Instance.GetCdms();

                        cdmLookupForm.InitialSearchText = clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString();
                        if (cdmLookupForm.ShowDialog() == DialogResult.OK)
                        {
                            clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value = cdmLookupForm.SelectedValue;
                            currentCdm = cdmRepository.GetCdm(cdmLookupForm.SelectedValue);
                            if (currentCdm != null)
                            {
                                clientDiscountDataGrid[nameof(ClientDiscount.CdmDescription), e.RowIndex].Value = currentCdm.Description;

                                clientDiscountDataGrid.Focus();
                                clientDiscountDataGrid.CurrentCell = clientDiscountDataGrid[nameof(ClientDiscount.PercentDiscount), 0];
                                clientDiscountDataGrid.BeginEdit(true);
                            }
                            else
                            {
                                clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value = string.Empty;

                                clientDiscountDataGrid.Focus();
                                clientDiscountDataGrid.CurrentCell = clientDiscountDataGrid[nameof(ClientDiscount.Cdm), 0];
                                clientDiscountDataGrid.BeginEdit(true);
                            }
                        }
                    }

                    break;
                case nameof(ClientDiscount.PercentDiscount):
                    double clientPrice = 0.00;
                    switch (client.FeeSchedule)
                    {
                        case "1":
                            clientPrice = currentCdm.CdmFeeSchedule1.Sum(p => p.CClassPrice);
                            break;
                        case "2":
                            clientPrice = currentCdm.CdmFeeSchedule2.Sum(p => p.CClassPrice);
                            break;
                        case "3":
                            clientPrice = currentCdm.CdmFeeSchedule3.Sum(p => p.CClassPrice);
                            break;
                        case "4":
                            clientPrice = currentCdm.CdmFeeSchedule4.Sum(p => p.CClassPrice);
                            break;
                        case "5":
                            clientPrice = currentCdm.CdmFeeSchedule5.Sum(p => p.CClassPrice);
                            break;
                        default:
                            break;
                    }
                    double percentDiscount = Convert.ToDouble(clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString());
                    percentDiscount = percentDiscount / 100;
                    double discountPrice = clientPrice - (clientPrice * percentDiscount);
                    clientDiscountDataGrid[nameof(ClientDiscount.Price), e.RowIndex].Value = discountPrice.ToString();
                    break;
                case nameof(ClientDiscount.Price):
                    clientPrice = 0.00;
                    switch (client.FeeSchedule)
                    {
                        case "1":
                            clientPrice = currentCdm.CdmFeeSchedule1.Sum(p => p.CClassPrice);
                            break;
                        case "2":
                            clientPrice = currentCdm.CdmFeeSchedule2.Sum(p => p.CClassPrice);
                            break;
                        case "3":
                            clientPrice = currentCdm.CdmFeeSchedule3.Sum(p => p.CClassPrice);
                            break;
                        case "4":
                            clientPrice = currentCdm.CdmFeeSchedule4.Sum(p => p.CClassPrice);
                            break;
                        case "5":
                            clientPrice = currentCdm.CdmFeeSchedule5.Sum(p => p.CClassPrice);
                            break;
                        default:
                            break;
                    }
                    discountPrice = Convert.ToDouble(clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString());
                    percentDiscount = ((clientPrice - discountPrice) / clientPrice) * 100;
                    clientDiscountDataGrid[nameof(ClientDiscount.PercentDiscount), e.RowIndex].Value = percentDiscount.ToString();
                    break;
                default:
                    break;
            }
        }

        private void clientDiscountDataGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == clientDiscountDataGrid.NewRowIndex)
                return;

            string cdm = clientDiscountDataGrid[nameof(ClientDiscount.Cdm), e.RowIndex].Value.ToString();

            if(!string.IsNullOrEmpty(cdm))
            {
                currentCdm = cdmRepository.GetCdm(cdm);
            }
        }

        private void clientDiscountDataGrid_RowLeave(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
