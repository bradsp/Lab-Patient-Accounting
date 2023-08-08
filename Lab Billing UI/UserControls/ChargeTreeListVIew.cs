using LabBilling.Core;
using LabBilling.Core.Models;
using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace LabBilling.UserControls
{
    public partial class ChargeTreeListView : UserControl
    {
        public ChargeTreeListView()
        {
            InitializeComponent();
        }

        private Account _currentAccount = null;
        private BindingList<Chrg> boundChrgList = new BindingList<Chrg>();
        private BindingList<ChrgDetail> boundChrgDetails = new BindingList<ChrgDetail>();
        private const string patientFinClass = "M";
        private const string clientFinClass = "C";

        private double selectedChargeNo = 0;

        public Account CurrentAccount
        {
            get { return _currentAccount; }
            set
            {
                _currentAccount = value;
                if (_currentAccount != null)
                    LoadChargeTreeView();
            }
        }

        private void LoadChargeTreeView()
        {
            boundChrgList = new BindingList<Chrg>(_currentAccount.Charges);
            boundChrgDetails = new BindingList<ChrgDetail>(_currentAccount.ChargeDetails);

            chargeGrid.DataSource = boundChrgList;
            chargeDetailGrid.DataSource = boundChrgDetails;

            _currentAccount.ChargeDetails.Sort((ChrgDetail x, ChrgDetail y) => x.ChrgNo.CompareTo(y.ChrgNo));

            FilterCharges();

            foreach (DataGridViewColumn col in chargeGrid.Columns)
                col.Visible = false;

            chargeGrid.Columns[nameof(Chrg.CDMCode)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.CdmDescription)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.ChrgNo)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.Comment)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.DiagnosisCodePointer)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.IsCredited)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.LISReqNo)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.OrderMnem)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.PostingDate)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.ServiceDate)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.Status)].Visible = true;

            int n = 0;

            chargeGrid.Columns[nameof(Chrg.IsCredited)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.ChrgNo)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.CDMCode)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.CdmDescription)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.Status)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.PostingDate)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.ServiceDate)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.Comment)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.DiagnosisCodePointer)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.LISReqNo)].DisplayIndex = n++;
            chargeGrid.Columns[nameof(Chrg.OrderMnem)].DisplayIndex = n++;

            chargeGrid.AutoResizeColumns();

            foreach (DataGridViewColumn col in chargeDetailGrid.Columns)
                col.Visible = false;

            chargeDetailGrid.Columns[nameof(ChrgDetail.Invoice)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.FinancialType)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.FinCode)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Amount)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.BillingCode)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ChrgNo)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ClientMnem)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Cpt4)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.CdmDescription)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.DiscountAmount)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.IsCredited)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Modifier)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Modifer2)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.PostedDate)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Quantity)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.RevenueCode)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Type)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ServiceDate)].Visible = true;

            n = 0;

            chargeDetailGrid.Columns[nameof(ChrgDetail.IsCredited)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ChrgNo)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.FinancialType)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.FinCode)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ClientMnem)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.BillingCode)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.CdmDescription)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.PostedDate)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Type)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ServiceDate)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Quantity)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Cpt4)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Modifier)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Modifer2)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.RevenueCode)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Invoice)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Amount)].DisplayIndex = n++;
            chargeDetailGrid.Columns[nameof(ChrgDetail.DiscountAmount)].DisplayIndex = n++;

            chargeDetailGrid.AutoResizeColumns();

            chargeStatus1.Text = "";
            chargeStatus1.SelectionFont = new Font(chargeStatus1.Font.FontFamily, 10, FontStyle.Bold);
            chargeStatus1.SelectedText = "3rd Party Patient Balance\n";

            chargeStatus1.AppendText(_currentAccount.ClaimBalance.ToString("c") + "\n");

            foreach (var (client, balance) in _currentAccount.ClientBalance)
            {
                chargeStatus1.SelectionFont = new Font(chargeStatus1.Font.FontFamily, 10, FontStyle.Bold);
                chargeStatus1.SelectedText = $"Client {client} Balance\n";
                chargeStatus1.AppendText(balance.ToString("c") + "\n");
            }

            chargeGrid.ClearSelection();
            chargeDetailGrid.ClearSelection();
        }

        private void ChargeTreeListView_Load(object sender, EventArgs e)
        {
            chargeGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            chargeGrid.AllowUserToAddRows = false;
            chargeGrid.AllowUserToDeleteRows = false;
            chargeGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            chargeGrid.MultiSelect = false;
            chargeGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            chargeDetailGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            chargeDetailGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            chargeDetailGrid.AllowUserToAddRows = false;
            chargeDetailGrid.AllowUserToDeleteRows = false;
            chargeDetailGrid.MultiSelect = false;
            chargeDetailGrid.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

            //build context menu
            foreach (var item in Dictionaries.cptModifiers)
            {
                ToolStripMenuItem tsItem = new ToolStripMenuItem(item.Key);
                tsItem.Tag = item.Value;
                tsItem.Click += new EventHandler(addModifierToolStripMenuItem_Click);

                addModifierToolStripMenuItem.DropDownItems.Add(tsItem);
            }
        }

        private void chargeGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            selectedChargeNo = Convert.ToDouble(chargeGrid.SelectedRows[0].Cells[nameof(Chrg.ChrgNo)].Value.ToString());
            chargeDetailGrid.Refresh();
            chargeDetailGrid.ClearSelection();
        }

        private void chargeDetailGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
        }

        private void chargeDetailGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (selectedChargeNo > 0 && e.RowIndex >= 0)
            {
                if (chargeDetailGrid.Rows[e.RowIndex].Cells[nameof(ChrgDetail.ChrgNo)].Value.ToString() == selectedChargeNo.ToString())
                {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Bold);
                }
                else
                {
                    e.CellStyle.Font = new Font(e.CellStyle.Font, FontStyle.Regular);
                }
            }
        }

        private void chargeDetailGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRows = chargeDetailGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                try
                {
                    ChrgDetail chrg = (ChrgDetail)chargeDetailGrid.SelectedRows[0].DataBoundItem;

                    DisplayPOCOForm<ChrgDetail> frm = new DisplayPOCOForm<ChrgDetail>(chrg)
                    {
                        Title = "Charge Details"
                    };
                    frm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void chargeGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            int selectedRows = chargeGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                try
                {
                    Chrg chrg = (Chrg)chargeGrid.SelectedRows[0].DataBoundItem;
                    DisplayPOCOForm<Chrg> frm = new DisplayPOCOForm<Chrg>(chrg)
                    {
                        Title = "Order"
                    };
                    frm.Show();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            }
        }

        private void creditChargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRows = chargeDetailGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = chargeGrid.SelectedRows[0];
                var chrgNo = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgNo)].Value.ToString());

                ChargeTreeListViewArgs args = new ChargeTreeListViewArgs();
                args.ChrgNo = chrgNo;

                CreditCharge?.Invoke(this, args);
            }
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when user has clicked CreditCharge on a charge line.")]
        public event EventHandler<ChargeTreeListViewArgs> CreditCharge;

        private void moveChargeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRows = chargeGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = chargeGrid.SelectedRows[0];
                var chrgNo = Convert.ToInt32(row.Cells[nameof(Chrg.ChrgNo)].Value.ToString());
                var description = row.Cells[nameof(Chrg.CdmDescription)].Value.ToString();

                ChargeTreeListViewArgs args = new ChargeTreeListViewArgs();
                args.ChrgNo = chrgNo;
                args.ChargeDescription = description;

                MoveCharge?.Invoke(this, args);
            }
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when user has clicked MoveCharge on a Charge line.")]
        public event EventHandler<ChargeTreeListViewArgs> MoveCharge;

        private void removeModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int selectedRows = chargeDetailGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = chargeDetailGrid.SelectedRows[0];
                var chrgNo = Convert.ToInt32(row.Cells[nameof(ChrgDetail.ChrgNo)].Value.ToString());
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.ChrgDetailId)].Value.ToString());

                ChargeTreeListViewArgs args = new ChargeTreeListViewArgs();
                args.ChrgNo = chrgNo;
                args.ChrgDetailId = uri;

                RemoveModifier?.Invoke(this, args);
            }
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when user has clicked RemoveModifier on a Charge line.")]
        public event EventHandler<ChargeTreeListViewArgs> RemoveModifier;

        private void addModifierToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            int selectedRows = chargeDetailGrid.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedRows > 0)
            {
                DataGridViewRow row = chargeDetailGrid.SelectedRows[0];
                var uri = Convert.ToInt32(row.Cells[nameof(ChrgDetail.ChrgDetailId)].Value.ToString());
                var chrgNo = Convert.ToInt32(row.Cells[nameof(ChrgDetail.ChrgNo)].Value.ToString());

                ChargeTreeListViewArgs args = new ChargeTreeListViewArgs();
                args.ModifierToAdd = item.Text;
                args.ChrgNo = chrgNo;
                args.ChrgDetailId = uri;

                AddModifierClicked?.Invoke(this, args);
            }
        }

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when user has clicked AddModifier on a Charge Detail line.")]
        public event EventHandler<ChargeTreeListViewArgs> AddModifierClicked;

        [Browsable(true)]
        [Category("Action")]
        [Description("Occurs when user has clicked the Add Charge button.")]
        public event EventHandler<ChargeTreeListViewArgs> AddChargeButtonClicked;

        private void addChargeButton_Click(object sender, EventArgs e)
        {
            ChargeTreeListViewArgs args = new ChargeTreeListViewArgs();

            AddChargeButtonClicked?.Invoke(this, args);
        }

        private void showThirdPartyRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FilterCharges();
        }

        private void showClientRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FilterCharges();
        }

        private void showAllRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            FilterCharges();
        }

        private void FilterCharges()
        {

            if (showAllRadioButton.Checked)
            {
                if (!showCreditsCheckbox.Checked)
                {
                    boundChrgDetails = new BindingList<ChrgDetail>(_currentAccount.ChargeDetails.Where(x => x.IsCredited == false).ToList());
                }
                else
                {
                    boundChrgDetails = new BindingList<ChrgDetail>(_currentAccount.ChargeDetails);
                }
            }
            else
            {
                string finClass;
                if (showThirdPartyRadioButton.Checked)
                    finClass = patientFinClass;
                else
                    finClass = clientFinClass;

                if (!showCreditsCheckbox.Checked)
                {
                    boundChrgDetails = new BindingList<ChrgDetail>(_currentAccount.ChargeDetails.Where(x => x.FinancialType == finClass && x.IsCredited == false).ToList());
                }
                else
                {
                    boundChrgDetails = new BindingList<ChrgDetail>(_currentAccount.ChargeDetails.Where(x => x.FinancialType == finClass).ToList());
                }

            }

            chargeDetailGrid.DataSource = boundChrgDetails;
        }

        private void showCreditsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            FilterCharges();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChargeTreeListViewArgs : EventArgs
    {
        public int ChrgNo { get; set; }
        public int ChrgDetailId { get; set; }
        public string ModifierToAdd { get; set; }
        public string ChargeDescription { get; set; }
    }

}
