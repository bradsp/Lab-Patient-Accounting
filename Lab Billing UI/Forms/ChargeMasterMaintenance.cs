using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using LabBilling.Logging;

namespace LabBilling.Forms
{
    public partial class ChargeMasterMaintenance : Form
    {
        private CdmRepository cdmRepository = new CdmRepository(Program.AppEnvironment);
        private List<Cdm> cdms = new List<Cdm>();
        private DataTable cdmdt = new DataTable();
        private BindingSource bs = new BindingSource();
        private const int _timerInterval = 650;
        System.Windows.Forms.Timer _timer;

        public ChargeMasterMaintenance()
        {
            InitializeComponent();
            _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
            _timer.Tick += new EventHandler(filterTextBox_KeyUpDone);
        }

        private void addCdmButton_Click(object sender, EventArgs e)
        {
            ChargeMasterEditForm editForm = new ChargeMasterEditForm();

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                ReloadGrid();
            }
        }

        private void includeInactiveCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void ReloadGrid()
        {
            cdms = cdmRepository.GetAll(includeInactiveCheckBox.Checked).OrderBy(c => c.Description).ToList();
            cdmdt = Helper.ConvertToDataTable(cdms);
            bs.DataSource = null;
            bs.DataSource = cdmdt;
            cdmGrid.DataSource = null;
            cdmGrid.DataSource = bs;

            RefreshGrid();
        }

        private void ChargeMasterMaintenance_Load(object sender, EventArgs e)
        {
            ReloadGrid();
        }

        private void RefreshGrid()
        {
            int cdmColCnt = cdmGrid.Columns.Count;
            for (int i = 0; i < cdmColCnt; i++)
            {
                cdmGrid.Columns[i].Visible = false;
            }
            //cdmGrid.Columns.OfType<DataGridColumn>().ToList().ForEach(col => col.Visible = false);
            cdmGrid.Columns[nameof(Cdm.IsDeleted)].DisplayIndex = 0;
            cdmGrid.Columns[nameof(Cdm.ChargeId)].DisplayIndex = 1;
            cdmGrid.Columns[nameof(Cdm.Description)].DisplayIndex = 2;
            cdmGrid.Columns[nameof(Cdm.IsOrderable)].DisplayIndex = 3;
            cdmGrid.Columns[nameof(Cdm.Mnem)].DisplayIndex = 4;
            cdmGrid.Columns[nameof(Cdm.MClassType)].DisplayIndex = 5;
            cdmGrid.Columns[nameof(Cdm.CClassType)].DisplayIndex = 6;
            cdmGrid.Columns[nameof(Cdm.ZClassType)].DisplayIndex = 7;

            cdmGrid.Columns[nameof(Cdm.ChargeId)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.Description)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.IsOrderable)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.MClassType)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.CClassType)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.ZClassType)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.IsDeleted)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.Mnem)].Visible = true;
            cdmGrid.Columns[nameof(Cdm.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            cdmGrid.AutoResizeColumns();

            if(!string.IsNullOrWhiteSpace(filterTextBox.Text))
            {
                cdmdt.DefaultView.RowFilter = $"({nameof(Cdm.Description)} like '{filterTextBox.Text.ToUpper()}*') or ({nameof(Cdm.ChargeId)} like '{filterTextBox.Text.ToUpper()}*')";
            }
        }

        private void cdmGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            if (Convert.ToBoolean(cdmGrid.Rows[e.RowIndex].Cells[nameof(Cdm.IsDeleted)].Value) == true)
            {
                cdmGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Gray;
            }
            else
            {
                cdmGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            }
        }

        private void filterTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            _timer.Stop();
            _timer.Start();
        }

        private void filterTextBox_KeyUpDone(object sender, EventArgs e)
        {
            _timer.Stop();

            cdmdt.DefaultView.RowFilter = $"({nameof(Cdm.Description)} like '{filterTextBox.Text.ToUpper()}*') or ({nameof(Cdm.ChargeId)} like '{filterTextBox.Text.ToUpper()}*')";

        }

        private void cdmGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            string cdm = cdmGrid[nameof(Cdm.ChargeId), e.RowIndex].Value.ToString();

            ChargeMasterEditForm editForm = new ChargeMasterEditForm();
            editForm.SelectedCdm = cdm;

            if (editForm.ShowDialog() == DialogResult.OK)
            {
                ReloadGrid();
            }
        }
    }
}
