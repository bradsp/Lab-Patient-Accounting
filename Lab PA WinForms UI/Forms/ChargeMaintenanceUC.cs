﻿using LabBilling.Core;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using LabBilling.ViewModel;
using System.ComponentModel;
using System.Data;
using WinFormsLibrary;

namespace LabBilling.Forms;

public partial class ChargeMaintenanceUC : UserControl
{
    public ChargeMaintenanceUC()
    {
        if (this == null)
            return;
        if (!this.DesignMode)
        {
            InitializeComponent();
            _grouper = new(ChargesDataGrid);
            _accountService = new(Program.AppEnvironment);
        }
    }

    private DataTable _chargesTable;
    private bool _allowChargeEntry;
    private Subro.Controls.DataGridViewGrouper _grouper;
    private readonly AccountService _accountService;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public Account CurrentAccount { get; set; }
    public event EventHandler ChargesUpdated;
    public event EventHandler<AppErrorEventArgs> OnError;

    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public bool AllowChargeEntry
    {
        get
        {
            return _allowChargeEntry;
        }

        set
        {
            _allowChargeEntry = value;
            changeCreditFlagToolStripMenuItem.Enabled = Program.LoggedInUser.IsAdministrator;
            AddChargeButton.Enabled = _allowChargeEntry;
            chargesContextMenu.Enabled = _allowChargeEntry;
        }
    }


    private void ChargeMaintenanceUC_Load(object sender, EventArgs e)
    {
        if (this == null)
            return;
        if (this.DesignMode)
            return;

        LoadCharges();
    }

    private void LoadViewModel()
    {
        //convert model to viewmodel

        List<Charge> charges = new();
        foreach (var chrg in CurrentAccount.Charges)
        {
            foreach (var detail in chrg.ChrgDetails)
            {
                var charge = new Charge
                {
                    ChrgId = chrg.ChrgId,
                    IsCredited = chrg.IsCredited,
                    AccountNo = chrg.AccountNo,
                    Status = chrg.Status,
                    ServiceDate = chrg.ServiceDate,
                    HistoryDate = chrg.HistoryDate,
                    CDMCode = chrg.CDMCode,
                    Quantity = chrg.Quantity,
                    NetAmount = chrg.NetAmount,
                    Comment = chrg.Comment,
                    Invoice = chrg.Invoice,
                    FinancialType = chrg.FinancialType,
                    LISReqNo = chrg.LISReqNo,
                    PostingDate = chrg.PostingDate,
                    ClientMnem = chrg.ClientMnem,
                    FinCode = chrg.FinCode,
                    PerformingSite = chrg.PerformingSite,
                    BillMethod = chrg.BillMethod,
                    OrderingSite = chrg.OrderingSite,
                    Facility = chrg.Facility,
                    ReferenceReq = chrg.ReferenceReq,
                    RetailAmount = chrg.RetailAmount,
                    HospAmount = chrg.HospAmount,
                    UpdatedHost = chrg.UpdatedHost,
                    UpdatedDate = chrg.UpdatedDate,
                    UpdatedApp = chrg.UpdatedApp,
                    rowguid = chrg.rowguid,
                    CdmDescription = chrg.CdmDescription,
                    RevenueCode = detail.RevenueCode,
                    Cpt4 = detail.Cpt4,
                    Modifier2 = detail.Modifier2,
                    Modifier = detail.Modifier,
                    Type = detail.Type,
                    Amount = detail.Amount,
                    OrderCode = detail.OrderCode,
                    PointerSet = detail.PointerSet,
                    RevenueCodeDetail = detail.RevenueCodeDetail,
                    CptDescription = detail.CptDescription,
                    uri = detail.Id
                };
                charges.Add(charge);
            }
        }

        _chargesTable = charges.ToDataTable();
    }

    public void LoadCharges()
    {
        _grouper.RemoveGrouping();
        ChargesDataGrid.DataSource = null;
        ChargesDataGrid.Rows.Clear();
        ChargesDataGrid.Columns.Clear();

        if (CurrentAccount == null)
            return;

        if (CurrentAccount.Charges == null)
            return;

        LoadViewModel();

        TotalChargesTextBox.Text = CurrentAccount.TotalCharges.ToString("c");

        //add modifier combobox columns
        DataGridViewComboBoxColumn modifier1 = new();
        DataGridViewComboBoxColumn modifier2 = new();

        modifier1.Name = nameof(Charge.Modifier);
        modifier1.HeaderText = nameof(Charge.Modifier);
        modifier1.DataPropertyName = nameof(Charge.Modifier);
        modifier1.FlatStyle = FlatStyle.Flat;

        modifier2.Name = nameof(Charge.Modifier2);
        modifier2.HeaderText = nameof(Charge.Modifier2);
        modifier2.DataPropertyName = nameof(Charge.Modifier2);
        modifier2.FlatStyle = FlatStyle.Flat;

        modifier1.DataSource = new BindingSource(Dictionaries.CptModifiers, null);
        modifier2.DataSource = new BindingSource(Dictionaries.CptModifiers, null);

        modifier1.ValueMember = "Key";
        modifier1.DisplayMember = "Key";

        modifier2.ValueMember = "Key";
        modifier2.DisplayMember = "Key";

        ChargesDataGrid.Columns.Add(modifier1);
        ChargesDataGrid.Columns.Add(modifier2);

        ChargesDataGrid.DataSource = _chargesTable;

        try
        {
            if (ChargesDataGrid.Columns.Contains(nameof(Charge.ChrgId)))
            {
                _grouper.SetGroupOn(ChargesDataGrid.Columns[nameof(Charge.ChrgId)]);
            }
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);
        }

        if (CurrentAccount.FinCode == Program.AppEnvironment.ApplicationParameters.ClientAccountFinCode)
        {
            _chargesTable.DefaultView.Sort = $"{nameof(Chrg.ChrgId)} desc";
            _grouper.GroupSortOrder = SortOrder.Descending;
        }
        if (!ShowCreditedChrgCheckBox.Checked)
        {
            _chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.IsCredited)} = false";
        }

        foreach (DataGridViewColumn col in ChargesDataGrid.Columns)
        {
            col.Visible = false;
            col.ReadOnly = true;
        }
        int z = 1;
        ChargesDataGrid.Columns[nameof(Charge.ChrgId)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.IsCredited)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.CDMCode)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.CdmDescription)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Status)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.ServiceDate)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Invoice)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.ClientMnem)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.FinCode)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.CptDescription)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Quantity)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.RevenueCode)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Cpt4)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Modifier)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Type)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Amount)].SetVisibilityOrder(true, z++);
        ChargesDataGrid.Columns[nameof(Charge.Comment)].SetVisibilityOrder(true, z++);

        ChargesDataGrid.Columns[nameof(Charge.Modifier)].ReadOnly = false;
        ChargesDataGrid.Columns[nameof(Charge.Modifier2)].ReadOnly = false;

        ChargesDataGrid.Columns[nameof(Charge.Amount)].DefaultCellStyle.Format = "N2";
        ChargesDataGrid.Columns[nameof(Charge.Amount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        ChargesDataGrid.Columns[nameof(Charge.Quantity)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

        ChargesDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
        ChargesDataGrid.Columns[nameof(Charge.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        ChargesDataGrid.Columns[nameof(Charge.CptDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        ChargesDataGrid.Columns[nameof(Charge.Comment)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        ChargesDataGrid.Columns[nameof(Charge.ChrgId)].SortMode = DataGridViewColumnSortMode.NotSortable;
        ChargesDataGrid.BackgroundColor = Color.AntiqueWhite;

        chargeBalRichTextbox.Text = "";
        chargeBalRichTextbox.SelectionFont = new Font(chargeBalRichTextbox.Font.FontFamily, 10, FontStyle.Bold);
        chargeBalRichTextbox.SelectedText = "3rd Party Patient Balance\n";

        chargeBalRichTextbox.AppendText(CurrentAccount.ClaimBalance.ToString("c") + "\n");

        foreach (var (client, balance) in CurrentAccount.ClientBalance)
        {
            chargeBalRichTextbox.SelectionFont = new Font(chargeBalRichTextbox.Font.FontFamily, 10, FontStyle.Bold);
            chargeBalRichTextbox.SelectedText = $"Client {client} Balance\n";
            chargeBalRichTextbox.AppendText(balance.ToString("c") + "\n");
        }

        ChargesDataGrid.ClearSelection();
    }

    private void AddModifier(int uri, int chrg_num, string modifier)
    {
        try
        {
            _accountService.AddChargeModifier(uri, modifier);
        }
        catch (ApplicationException apex)
        {
            string message = apex.Message;
            if (apex.InnerException != null) { message += Environment.NewLine + apex.InnerException.Message; }
            MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        var idx = CurrentAccount.Charges.FindIndex(x => x.ChrgId == chrg_num);
        var dIdx = CurrentAccount.Charges[idx].ChrgDetails.FindIndex(x => x.Id == uri);
        CurrentAccount.Charges[idx].ChrgDetails[dIdx].Modifier = modifier;

        ChargesUpdated?.Invoke(this, EventArgs.Empty);
    }

    private void DgvCharges_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {

            DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
            var chrgNo = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString());

            DisplayPOCOForm<Chrg> frm = new(CurrentAccount.Charges.Where(c => c.ChrgId == chrgNo).Single())
            {
                Title = "Charge Details"
            };
            frm.Show();
        }
    }

    /// <summary>
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void ChargesDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        return;
    }

    private void ChargesDataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
    {
        if (_grouper.IsGroupRow(e.RowIndex))
            return;

        if (ChargesDataGrid[nameof(Chrg.IsCredited), e.RowIndex].Value.ToString() == "True")
        {
            e.CellStyle.BackColor = Color.WhiteSmoke;
            e.CellStyle.ForeColor = Color.Red;
            return;
        }

        if (ChargesDataGrid[nameof(Chrg.FinancialType), e.RowIndex].Value.ToString() == "C")
        {
            e.CellStyle.BackColor = Color.LightGreen;
        }
        if (ChargesDataGrid[nameof(Chrg.FinancialType), e.RowIndex].Value.ToString() == "M")
        {
            e.CellStyle.BackColor = Color.LightBlue;
        }

        return;

    }

    /// <summary>
    /// Function will credit the charge selected in the charge grid.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void creditChargeToolStrip_Click(object sender, EventArgs e)
    {
        int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {
            DataGridViewRow row = ChargesDataGrid.SelectedRows[0];

            InputBoxResult prompt = InputBox.Show(string.Format("Credit Charge Number {0}?\nEnter credit reason.",
                row.Cells[nameof(Chrg.ChrgId)].Value.ToString()),
                "Credit Charge", "");

            if (prompt.ReturnCode == DialogResult.OK)
            {
                int chrgNo = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString());
                var updatedChrg = _accountService.CreditCharge(chrgNo, prompt.Text);
                //add charge to list
                CurrentAccount.Charges.Add(updatedChrg);
                int idx = CurrentAccount.Charges.FindIndex(c => c.ChrgId == chrgNo);
                var creditedChrg = _accountService.GetCharge(chrgNo);
                CurrentAccount.Charges[idx] = creditedChrg;
                ChargesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void ShowCreditedChrgCheckBox_CheckedChanged(object sender, EventArgs e) => FilterCharges();

    private void AddChargeButton_Click(object sender, EventArgs e)
    {
        ChargeEntryForm frm = new(CurrentAccount);

        if (frm.ShowDialog() == DialogResult.OK)
        {
            CurrentAccount = _accountService.AddCharge(new AddChargeParameters()
            {
                Account = CurrentAccount,
                Cdm = frm.SelectedCdm,
                Quantity = (int)frm.Quantity,
                ServiceDate = CurrentAccount.TransactionDate,
                Comment = frm.Comment,
                RefNumber = frm.ReferenceNumber,
                MiscAmount = frm.Amount
            });
            ChargesUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    private void FilterCharges()
    {

        _chargesTable.DefaultView.RowFilter = string.Empty;

        if (show3rdPartyRadioButton.Checked)
            _chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.FinancialType)} = 'M'";

        if (showClientRadioButton.Checked)
            _chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.FinancialType)} = 'C'";

        if (showAllChargeRadioButton.Checked)
            _chargesTable.DefaultView.RowFilter = String.Empty;

        if (!ShowCreditedChrgCheckBox.Checked)
        {
            if (_chargesTable.DefaultView.RowFilter == string.Empty)
                _chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.IsCredited)} = false";
            else
                _chargesTable.DefaultView.RowFilter += $"and {nameof(Chrg.IsCredited)} = false";
        }
    }

    private void changeCreditFlagToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {
            DataGridViewRow row = ChargesDataGrid.SelectedRows[0];

            int chrgId = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value);
            bool currentFlag = Convert.ToBoolean(row.Cells[nameof(Chrg.IsCredited)].Value);

            if (MessageBox.Show($"Change credited flag on {chrgId}?",
                "Confirm Change", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                OnError?.Invoke(this, new AppErrorEventArgs()
                {
                    ErrorLevel = AppErrorEventArgs.ErrorLevelType.Debug,
                    ErrorMessage = $"Changing credited flag on {chrgId} from {currentFlag} to {!currentFlag}"
                });

                var chrg = _accountService.SetChargeCreditFlag(chrgId, !currentFlag);
                int idx = CurrentAccount.Charges.IndexOf(chrg);
                if (idx > -1)
                    CurrentAccount.Charges[idx] = chrg;
            }
            ChargesUpdated?.Invoke(this, EventArgs.Empty);
        }
    }

    private void moveChargeToolStripMenuItem_Click(object sender, EventArgs e)
    {
        int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
        if (selectedRows > 0)
        {
            DataGridViewRow row = ChargesDataGrid.SelectedRows[0];

            PersonSearchForm personSearch = new();

            if (personSearch.ShowDialog() == DialogResult.OK)
            {
                string destAccount = personSearch.SelectedAccount;
                int chrgId = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value);

                if (MessageBox.Show($"Move charge {chrgId} ({row.Cells[nameof(Chrg.CdmDescription)].Value}) to account {destAccount}?",
                    "Confirm Move", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    OnError?.Invoke(this, new AppErrorEventArgs()
                    {
                        ErrorLevel = AppErrorEventArgs.ErrorLevelType.Debug,
                        ErrorMessage = $"Moving charge {chrgId} from {CurrentAccount.AccountNo} to {destAccount}"
                    });
                    var credited = _accountService.MoveCharge(CurrentAccount.AccountNo, destAccount, chrgId);
                    CurrentAccount.Charges.Add(credited);
                    var idx = CurrentAccount.Charges.FindIndex(c => c.ChrgId == chrgId);
                    if (idx > -1)
                        CurrentAccount.Charges[idx].IsCredited = true;
                }
                ChargesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }
    }

    private void filterRadioButton_CheckedChanged(object sender, EventArgs e) => FilterCharges();

    private void ChargesDataGrid_ColumnSortModeChanged(object sender, DataGridViewColumnEventArgs e)
    {

    }

    private void ChargesDataGrid_CellEndEdit(object sender, DataGridViewCellEventArgs e)
    {
    }

    private void ChargesDataGrid_CellValueChanged(object sender, DataGridViewCellEventArgs e)
    {
        if (ChargesDataGrid.Columns[e.ColumnIndex].Name == nameof(Charge.Modifier))
        {
            DataGridViewComboBoxCell cb = (DataGridViewComboBoxCell)ChargesDataGrid.CurrentCell;
            var uri = Convert.ToInt32(ChargesDataGrid.Rows[e.RowIndex].Cells[nameof(Charge.uri)].Value.ToString());
            int chrgId = Convert.ToInt32(ChargesDataGrid.Rows[e.RowIndex].Cells[nameof(Chrg.ChrgId)].Value);

            var mod = ChargesDataGrid.Rows[e.RowIndex].Cells[nameof(Charge.Modifier)].Value.ToString();
            if (mod == "<none>")
                mod = string.Empty;
            AddModifier(uri, chrgId, mod);
        }

    }

    private void ChargesDataGrid_CurrentCellDirtyStateChanged(object sender, EventArgs e)
    {
        if (ChargesDataGrid.IsCurrentCellDirty)
        {
            ChargesDataGrid.CommitEdit(DataGridViewDataErrorContexts.Commit);
            ChargesDataGrid.EndEdit();
        }
    }

    private void ChargesDataGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (ChargesDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex] is DataGridViewComboBoxCell)
        {
            ChargesDataGrid.CurrentCell = ChargesDataGrid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            ChargesDataGrid.BeginEdit(true);
            ((ComboBox)ChargesDataGrid.EditingControl).DroppedDown = true;
        }
    }
}
