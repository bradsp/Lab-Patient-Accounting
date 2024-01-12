using LabBilling.Core;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using System;
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
        public event EventHandler<ChargeMaintErrorEventArgs> OnError;
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
                chargeDetailsContextMenu.Enabled = _allowChargeEntry;
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

                addModifierToolStripMenuItem.DropDownItems.Add(tsItem);
            }

            removeModifierToolStripMenuItem.Click += new EventHandler(RemoveModifier_Click);

            LoadCharges();
        }

        public void LoadCharges()
        {

            if (CurrentAccount == null)
                return;

            var chargesList = CurrentAccount.Charges;

            TotalChargesTextBox.Text = CurrentAccount.TotalCharges.ToString("c");

            chargesTable = Helper.ConvertToDataTable(chargesList);

            ChargesDataGrid.DataSource = chargesTable;
            ChargesDataGrid.DataMember = chargesTable.TableName;
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

            ChargesDataGrid.Columns[nameof(Chrg.IsCredited)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.CDMCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.CdmDescription)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Quantity)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.ServiceDate)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Status)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Comment)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.ChrgId)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.Invoice)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.FinCode)].Visible = true;
            ChargesDataGrid.Columns[nameof(Chrg.ClientMnem)].Visible = true;

            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Format = "N2";
            ChargesDataGrid.Columns[nameof(Chrg.NetAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            ChargesDataGrid.Columns[nameof(Chrg.Quantity)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            ChargesDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.DisplayedCells);
            ChargesDataGrid.Columns[nameof(Chrg.CdmDescription)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            ChargesDataGrid.BackgroundColor = Color.AntiqueWhite;
            ChrgDetailDataGrid.BackgroundColor = Color.AntiqueWhite;

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
            ChrgDetailDataGrid.ClearSelection();

        }

        private void RemoveModifier_Click(object sender, EventArgs e)
        {

            // get selected charge detail uri
            int selectedRows = ChrgDetailDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChrgDetailDataGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.uri)].Value.ToString());

                chrgDetailRepository.RemoveModifier(uri);
                ChargesUpdated?.Invoke(this, EventArgs.Empty);
            }
        }

        private void AddModifier_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            // get selected charge detail uri
            int selectedRows = ChrgDetailDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChrgDetailDataGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.uri)].Value.ToString());

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
        /// Single Click on charge table will display charge details for the clicked row in the
        /// charge details grid.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ChargesDataGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            int selectedRows = ChargesDataGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = ChargesDataGrid.SelectedRows[0];
                var chrg = chrgRepository.GetById(Convert.ToInt32(row.Cells[nameof(Chrg.ChrgId)].Value.ToString()));

                try
                {
                    ChrgDetailDataGrid.DataSource = chrg.ChrgDetails;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(this, string.Format("Exception {0}", ex.Message));
                    OnError?.Invoke(this, new ChargeMaintErrorEventArgs()
                    {
                        ErrorMessage = ex.Message,
                        ErrorLevel = ChargeMaintErrorEventArgs.ErrorLevelType.Error
                    });
                }
                foreach (DataGridViewColumn col in ChrgDetailDataGrid.Columns)
                {
                    col.Visible = false;
                }

                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Cpt4)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Modifier)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Modifer2)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.RevenueCode)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Type)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.OrderCode)].Visible = true;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Amount)].Visible = true;

                ChrgDetailDataGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Cpt4)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                ChrgDetailDataGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

                ChrgDetailDataGrid.Columns[nameof(ChrgDetail.Amount)].DefaultCellStyle.Format = "N2";

            }
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
                    OnError?.Invoke(this, new ChargeMaintErrorEventArgs()
                    {
                        ErrorLevel = ChargeMaintErrorEventArgs.ErrorLevelType.Debug,
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
                        OnError?.Invoke(this, new ChargeMaintErrorEventArgs() 
                        { 
                            ErrorLevel = ChargeMaintErrorEventArgs.ErrorLevelType.Debug,
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

    public class ChargeMaintErrorEventArgs : EventArgs
    {
        public string ErrorMessage { get; set; }
        public ErrorLevelType ErrorLevel { get; set; }

        public enum ErrorLevelType
        {
            Trace,
            Debug,
            Info,
            Warning,
            Error,
            Fatal
        }
    }

}
