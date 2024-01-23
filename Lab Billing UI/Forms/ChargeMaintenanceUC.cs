using LabBilling.Core;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace LabBilling.Forms
{
    public partial class ChargeMaintenanceUC : UserControl
    {
        public ChargeMaintenanceUC()
        {
            if (this == null)
                return;
            if (!this.DesignMode)
                InitializeComponent();
        }

        private DataTable chargesTable;
        private AccountRepository accountRepository;
        private ChrgRepository chrgRepository;
        private ChrgDetailRepository chrgDetailRepository;

        public Account CurrentAccount { get; set; }
        public event EventHandler ChargesUpdated;
        public event EventHandler<AppErrorEventArgs> OnError;
        private bool _allowChargeEntry;

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

            accountRepository = new AccountRepository(Program.AppEnvironment);
            chrgRepository = new ChrgRepository(Program.AppEnvironment);
            chrgDetailRepository = new ChrgDetailRepository(Program.AppEnvironment);

            //build context menu
            foreach (var item in Dictionaries.cptModifiers)
            {
                ToolStripMenuItem tsItem = new(item.Key)
                {
                    Tag = item.Value
                };
                tsItem.Click += new EventHandler(AddModifier_Click);

                addModifierToolStripMenuItem1.DropDownItems.Add(tsItem);
            }

            removeModifierToolStripMenuItem1.Click += new EventHandler(RemoveModifier_Click);

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
                        Modifer2 = detail.Modifer2,
                        Modifier = detail.Modifier,
                        Type = detail.Type,
                        Amount = detail.Amount,
                        OrderCode = detail.OrderCode,
                        PointerSet = detail.PointerSet,
                        RevenueCodeDetail = detail.RevenueCodeDetail,
                        DiagnosisPointer = detail.DiagnosisPointer,
                        DiagCodePointer = detail.DiagCodePointer,
                        CptDescription = detail.CptDescription,
                        uri = detail.uri
                    };
                    charges.Add(charge);
                }
            }

            chargesTable = charges.ToDataTable();
        }

        public void LoadCharges()
        {

            if (CurrentAccount == null)
                return;

            LoadViewModel();

            TotalChargesTextBox.Text = CurrentAccount.TotalCharges.ToString("c");

            ChargesDataGrid.DataSource = chargesTable;
            //ChargesDataGrid.DataMember = chargesTable.TableName;
            if (CurrentAccount.FinCode == "CLIENT")
            {
                chargesTable.DefaultView.Sort = $"{nameof(Chrg.ChrgId)} desc";
            }
            if (!ShowCreditedChrgCheckBox.Checked)
            {
                chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.IsCredited)} = false";
            }

            foreach (DataGridViewColumn col in ChargesDataGrid.Columns)
            {
                col.Visible = false;
            }

            ChargesDataGrid.Columns[nameof(Charge.IsCredited)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.CDMCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.CdmDescription)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Quantity)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.ServiceDate)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Status)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Comment)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.ChrgId)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Invoice)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.FinCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.ClientMnem)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.RevenueCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Cpt4)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Modifier)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Modifer2)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Amount)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.Type)].Visible = true;
            ChargesDataGrid.Columns[nameof(Charge.CptDescription)].Visible = true;

            ChargesDataGrid.Columns[nameof(Charge.Amount)].DefaultCellStyle.Format = "N2";
            ChargesDataGrid.Columns[nameof(Charge.Amount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ChargesDataGrid.Columns[nameof(Charge.Quantity)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            ChargesDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            ChargesDataGrid.Columns[nameof(Charge.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ChargesDataGrid.Columns[nameof(Charge.CptDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ChargesDataGrid.Columns[nameof(Charge.Comment)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            ChargesDataGrid.BackgroundColor = Color.AntiqueWhite;

            //set grid column order
            int z = 1;
            ChargesDataGrid.Columns[nameof(Charge.ChrgId)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.IsCredited)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.CDMCode)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.CdmDescription)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Status)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.ServiceDate)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Invoice)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.ClientMnem)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.FinCode)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.CptDescription)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Quantity)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.RevenueCode)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Cpt4)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Modifier)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Modifer2)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Type)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Amount)].DisplayIndex = z++;
            ChargesDataGrid.Columns[nameof(Charge.Comment)].DisplayIndex = z++;

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

        private void RemoveModifier_Click(object sender, EventArgs e)
        {

            // get selected charge detail uri
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.uri)].Value.ToString());

                chrgDetailRepository.RemoveModifier(uri);
                ChargesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AddModifier_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            // get selected charge detail uri
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(Charge.uri)].Value.ToString());

                chrgDetailRepository.AddModifier(uri, item.Text);
                ChargesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void DgvCharges_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {

                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var chrg = chrgRepository.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

                DisplayPOCOForm<Chrg> frm = new(chrg)
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
        private void ToolStripCreditCharge_Click(object sender, EventArgs e)
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
                    chrgRepository.CreditCharge(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()), prompt.Text);
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
                ChargesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void FilterCharges()
        {

            chargesTable.DefaultView.RowFilter = string.Empty;

            if (show3rdPartyRadioButton.Checked)
                chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.FinancialType)} = 'M'";

            if (showClientRadioButton.Checked)
                chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.FinancialType)} = 'C'";

            if (showAllChargeRadioButton.Checked)
                chargesTable.DefaultView.RowFilter = String.Empty;

            if (!ShowCreditedChrgCheckBox.Checked)
            {
                if (chargesTable.DefaultView.RowFilter == string.Empty)
                    chargesTable.DefaultView.RowFilter = $"{nameof(Chrg.IsCredited)} = false";
                else
                    chargesTable.DefaultView.RowFilter += $"and {nameof(Chrg.IsCredited)} = false";
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

                    chrgRepository.SetCredited(chrgId, !currentFlag);
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
                        accountRepository.MoveCharge(CurrentAccount.AccountNo, destAccount, chrgId);
                    }
                    ChargesUpdated?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        private void filterRadioButton_CheckedChanged(object sender, EventArgs e) => FilterCharges();
    }

}
