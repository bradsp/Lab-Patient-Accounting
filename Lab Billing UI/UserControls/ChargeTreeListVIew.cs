using BrightIdeasSoftware;
using LabBilling.Core.Models;
using MetroFramework.Controls;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.UserControls
{
    public partial class ChargeTreeListView : UserControl
    {
        public ChargeTreeListView()
        {
            InitializeComponent();
        }

        private List<Chrg> _chrgs = new List<Chrg>();
        private List<ChrgDetail> _chrgDetails = new List<ChrgDetail>();
        private BindingSource chargeBindingSource = new BindingSource();
        private BindingSource chargeDetailBindingsource = new BindingSource();
        private DataTable chargeTable = new DataTable();
        private DataTable chargeDetailTable = new DataTable();
        private double selectedChargeNo = 0;

        public List<Chrg> Charges
        {
            get
            {
                return _chrgs;
            }
            set
            {
                _chrgs = value;
                LoadChargeTreeView();
            }
        }
        public List<ChrgDetail> ChargeDetails
        {
            get
            {
                return _chrgDetails;
            }
            set
            {
                _chrgDetails = value;
                LoadChargeTreeView();
            }
        }

        private void LoadChargeTreeView()
        {
            chargeTable = _chrgs.ToDataTable();
            chargeDetailTable = _chrgDetails.ToDataTable();
            chargeBindingSource.DataSource = chargeTable;
            chargeDetailBindingsource.DataSource = chargeDetailTable;

            chargeGrid.DataSource = chargeBindingSource;
            chargeDetailGrid.DataSource = chargeDetailBindingsource;

            foreach (DataGridViewColumn col in chargeGrid.Columns)
                col.Visible = false;

            chargeGrid.Columns[nameof(Chrg.CDMCode)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.CdmDescription)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.ChrgId)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.Comment)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.DiagnosisCodePointer)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.IsCredited)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.LISReqNo)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.OrderMnem)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.PostingDate)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.ServiceDate)].Visible = true;
            chargeGrid.Columns[nameof(Chrg.Status)].Visible = true;


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
            chargeDetailGrid.Columns[nameof(ChrgDetail.DiscountAmount)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.IsCredited)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Modifier)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Modifer2)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.PostedDate)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Quantity)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.RevenueCode)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.RevenueCodeDetail)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.Type)].Visible = true;
            chargeDetailGrid.Columns[nameof(ChrgDetail.ServiceDate)].Visible = true;
        }

        private void ChargeTreeListView_Load(object sender, EventArgs e)
        {
            chargeGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            chargeGrid.AllowUserToAddRows = false;
            chargeGrid.AllowUserToDeleteRows = false;
            chargeGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            chargeGrid.MultiSelect = false;

            chargeDetailGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            chargeDetailGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            chargeDetailGrid.AllowUserToAddRows = false;
            chargeDetailGrid.AllowUserToDeleteRows = false;
            chargeDetailGrid.MultiSelect = false;

            LoadChargeTreeView();
        }

        private void chargeGrid_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            selectedChargeNo = Convert.ToDouble(chargeGrid.SelectedRows[0].Cells[nameof(Chrg.ChrgId)].Value.ToString());
            chargeDetailGrid.Refresh();
        }

        private void chargeDetailGrid_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if(selectedChargeNo > 0 && e.RowIndex >= 0)
            {
                if (chargeDetailGrid.Rows[e.RowIndex].Cells[nameof(ChrgDetail.ChrgNo)].Value.ToString() == selectedChargeNo.ToString())
                {
                    e.CellStyle.BackColor = Color.LightBlue;               
                }
                else
                {
                    e.CellStyle.BackColor = Color.White;
                }
                chargeStatus2.Text += $"({e.RowIndex},{e.ColumnIndex}: {chargeDetailGrid[e.ColumnIndex, e.RowIndex].Style.BackColor}\n";
            }
        }
    }
}
