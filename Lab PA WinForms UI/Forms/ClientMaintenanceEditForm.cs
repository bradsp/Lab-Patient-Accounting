using System;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.Services;

namespace LabBilling.Forms;

public partial class ClientMaintenanceEditForm : Form
{
    public Client client;
    private readonly DictionaryService _dictionaryService;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SelectedClient { get; set; }
    private readonly BindingSource _clientDiscountBindingSource = new();
    private DataTable _clientDiscountDataTable;
    private Cdm _currentCdm = null;

    public ClientMaintenanceEditForm()
    {
        InitializeComponent();
        _dictionaryService = new(Program.AppEnvironment, Program.UnitOfWork);
    }

    private void btnSave_Click(object sender, EventArgs e)
    {

        if(!ValidateChildren(ValidationConstraints.Enabled))
        {
            return;
        }


        client.IsDeleted = !activeCheckBox.Checked;
        client.ClientMnem = ClientMnemTextBox.Text;
        client.Name = ClientNameTextBox.Text;
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
        client.MroStreetAddress2 = MROAddress2TextBox.Text;
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

        if(_clientDiscountDataTable != null)
            client.Discounts = Helper.ConvertToList<ClientDiscount>(_clientDiscountDataTable);

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
        ClientMnemTextBox.Text = client.ClientMnem;
        ClientNameTextBox.Text = client.Name;
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
        MROAddress2TextBox.Text = client.MroStreetAddress2;
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


        _clientDiscountDataTable = client.Discounts.ToDataTable();
        _clientDiscountBindingSource.DataSource = _clientDiscountDataTable;
        clientDiscountDataGrid.DataSource = _clientDiscountBindingSource;

        _clientDiscountDataTable.DefaultView.Sort = $"{nameof(ClientDiscount.Cdm)}";

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


        ClientMnemTextBox.ReadOnly = true;
    }

    private void ClientMaintenanceEditForm_Load(object sender, EventArgs e)
    {
        cbState.DataSource = new BindingSource(Dictionaries.StateSource, null);
        cbState.DisplayMember = "Value";
        cbState.ValueMember = "Key";
        cbState.SelectedIndex = -1;

        cbClientType.DataSource = new BindingSource(Dictionaries.ClientType, null);
        cbClientType.DisplayMember = "Value";
        cbClientType.ValueMember = "Key";
        cbClientType.SelectedIndex = -1;

        cbCounty.DataSource = new BindingSource(Dictionaries.Counties, null);
        cbCounty.DisplayMember = "Value";
        cbCounty.ValueMember = "Key";
        cbCounty.SelectedIndex = -1;

        cbFeeSched.DataSource = new BindingSource(Dictionaries.FeeSchedule, null);
        cbFeeSched.DisplayMember = "Value";
        cbFeeSched.ValueMember = "Key";
        cbFeeSched.SelectedIndex = -1;

        cbMROState.DataSource = new BindingSource(Dictionaries.StateSource, null);
        cbMROState.DisplayMember = "Value";
        cbMROState.ValueMember = "Key";
        cbMROState.SelectedIndex = -1;

        cbEmrType.DataSource = new BindingSource(Dictionaries.EmrType, null);
        cbEmrType.DisplayMember = "Value";
        cbEmrType.ValueMember = "Key";
        cbEmrType.SelectedIndex = -1;

        cbCostCenter.DataSource = new BindingSource(_dictionaryService.GetGLCodes(), null);
        cbCostCenter.DisplayMember = "level_1";
        cbCostCenter.ValueMember = "level_1";
        cbCostCenter.SelectedIndex = -1;

        if(!string.IsNullOrEmpty(SelectedClient))
        {
            client = _dictionaryService.GetClient(SelectedClient);
            LoadClient();
        }
    }

    private void tbClientMnem_Leave(object sender, EventArgs e)
    {
        if (!String.IsNullOrEmpty(ClientMnemTextBox.Text) && !ClientMnemTextBox.ReadOnly)
        {
            var record = _dictionaryService.GetClient(ClientMnemTextBox.Text);

            if (record != null)
            {
                string message = $"Client with mnmen {ClientMnemTextBox.Text} already exists. Edit this record instead?\n\nChoose Yes to load this client, or no to enter a new mnem.";
                if (MessageBox.Show(message, "Client Exists",
                    MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    client = record;
                    LoadClient();
                }
                else
                {
                    ClientMnemTextBox.Focus();
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
                _currentCdm = _dictionaryService.GetCdm(clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString());
                if (_currentCdm != null)
                {
                    clientDiscountDataGrid[nameof(ClientDiscount.CdmDescription), e.RowIndex].Value = _currentCdm.Description;
                }
                else
                {
                    //pop a cdm search dialog
                    CdmLookupForm cdmLookupForm = new()
                    {
                        Datasource = DataCache.Instance.GetCdms(),
                        InitialSearchText = clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value.ToString()
                    };

                    if (cdmLookupForm.ShowDialog() == DialogResult.OK)
                    {
                        clientDiscountDataGrid[e.ColumnIndex, e.RowIndex].Value = cdmLookupForm.SelectedValue;
                        _currentCdm = _dictionaryService.GetCdm(cdmLookupForm.SelectedValue);
                        if (_currentCdm != null)
                        {
                            clientDiscountDataGrid[nameof(ClientDiscount.CdmDescription), e.RowIndex].Value = _currentCdm.Description;

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
                        clientPrice = _currentCdm.CdmFeeSchedule1.Sum(p => p.CClassPrice);
                        break;
                    case "2":
                        clientPrice = _currentCdm.CdmFeeSchedule2.Sum(p => p.CClassPrice);
                        break;
                    case "3":
                        clientPrice = _currentCdm.CdmFeeSchedule3.Sum(p => p.CClassPrice);
                        break;
                    case "4":
                        clientPrice = _currentCdm.CdmFeeSchedule4.Sum(p => p.CClassPrice);
                        break;
                    case "5":
                        clientPrice = _currentCdm.CdmFeeSchedule5.Sum(p => p.CClassPrice);
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
                        clientPrice = _currentCdm.CdmFeeSchedule1.Sum(p => p.CClassPrice);
                        break;
                    case "2":
                        clientPrice = _currentCdm.CdmFeeSchedule2.Sum(p => p.CClassPrice);
                        break;
                    case "3":
                        clientPrice = _currentCdm.CdmFeeSchedule3.Sum(p => p.CClassPrice);
                        break;
                    case "4":
                        clientPrice = _currentCdm.CdmFeeSchedule4.Sum(p => p.CClassPrice);
                        break;
                    case "5":
                        clientPrice = _currentCdm.CdmFeeSchedule5.Sum(p => p.CClassPrice);
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
            _currentCdm = _dictionaryService.GetCdm(cdm);
        }
    }

    private void clientDiscountDataGrid_RowLeave(object sender, DataGridViewCellEventArgs e)
    {
        if (clientDiscountDataGrid.Rows[e.RowIndex].IsNewRow || 
            string.IsNullOrEmpty(clientDiscountDataGrid[nameof(ClientDiscount.Cdm), e.RowIndex].Value.ToString()))
            return;

        clientDiscountDataGrid[nameof(ClientDiscount.ClientMnem), e.RowIndex].Value = ClientMnemTextBox.Text;
    }

    private void cbFeeSched_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    private void cbFeeSched_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if(cbFeeSched.SelectedValue == null)
        {
            e.Cancel = true;
            cbFeeSched.Focus();
            errorProvider1.SetError(cbFeeSched, "Must select a value.");
        }
        else
        {
            e.Cancel = false;
            errorProvider1.SetError(cbFeeSched, "");
        }
    }

    private void ClientMnemTextBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if(string.IsNullOrWhiteSpace(ClientMnemTextBox.Text))
        {
            e.Cancel = true;
            ClientMnemTextBox.Focus();
            errorProvider1.SetError(ClientMnemTextBox, "Must have a value.");
        }
        else
        {
            e.Cancel = false;
            errorProvider1.SetError(ClientMnemTextBox, "");
        }

    }

    private void cbClientType_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (cbClientType.SelectedValue == null)
        {
            e.Cancel = true;
            cbClientType.Focus();
            errorProvider1.SetError(cbClientType, "Must select a value.");
        }
        else
        {
            e.Cancel = false;
            errorProvider1.SetError(cbClientType, "");
        }
    }

    private void cbCostCenter_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (cbCostCenter.SelectedValue == null)
        {
            e.Cancel = true;
            cbCostCenter.Focus();
            errorProvider1.SetError(cbCostCenter, "Must select a value.");
        }
        else
        {
            e.Cancel = false;
            errorProvider1.SetError(cbCostCenter, "");
        }
    }

    private void billMethodComboBox_Validating(object sender, System.ComponentModel.CancelEventArgs e)
    {
        if (billMethodComboBox.SelectedItem == null)
        {
            e.Cancel = true;
            billMethodComboBox.Focus();
            errorProvider1.SetError(billMethodComboBox, "Must select a value.");
        }
        else
        {
            e.Cancel = false;
            errorProvider1.SetError(billMethodComboBox, "");
        }
    }
}
