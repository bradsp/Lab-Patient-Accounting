using LabBilling.Core.Models;
using LabBilling.Core.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using WinFormsLibrary;

namespace LabBilling.Forms;

public partial class ChargeMasterMaintenance : Form
{
    private readonly DictionaryService _dictionaryService;
    private List<Cdm> _cdms = new();
    private DataTable _cdmdt = new();
    private readonly BindingSource _bs = new();
    private const int _timerInterval = 650;
    private readonly System.Windows.Forms.Timer _timer;

    public ChargeMasterMaintenance()
    {
        InitializeComponent();
        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
        _timer.Tick += new EventHandler(filterTextBox_KeyUpDone);
        _dictionaryService = new(Program.AppEnvironment);
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
        _cdms = _dictionaryService.GetAllCdms(includeInactiveCheckBox.Checked).OrderBy(c => c.Description).ToList();
        _cdmdt = Helper.ConvertToDataTable(_cdms);
        _bs.DataSource = null;
        _bs.DataSource = _cdmdt;
        cdmGrid.DataSource = null;
        cdmGrid.DataSource = _bs;

        RefreshGrid();
    }

    private void ChargeMasterMaintenance_Load(object sender, EventArgs e)
    {
        ReloadGrid();
    }

    private void RefreshGrid()
    {
        cdmGrid.SetColumnsVisibility(false);

        int i = 0;
        cdmGrid.Columns[nameof(Cdm.IsDeleted)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.ChargeId)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.Description)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.IsOrderable)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.Mnem)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.MClassType)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.CClassType)].SetVisibilityOrder(true, i++);
        cdmGrid.Columns[nameof(Cdm.ZClassType)].SetVisibilityOrder(true, i++);

        cdmGrid.Columns[nameof(Cdm.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

        cdmGrid.AutoResizeColumns();

        if (!string.IsNullOrWhiteSpace(filterTextBox.Text))
        {
            _cdmdt.DefaultView.RowFilter = $"({nameof(Cdm.Description)} like '{filterTextBox.Text.ToUpper()}*') or ({nameof(Cdm.ChargeId)} like '{filterTextBox.Text.ToUpper()}*')";
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

        _cdmdt.DefaultView.RowFilter = $"({nameof(Cdm.Description)} like '{filterTextBox.Text.ToUpper()}*') or ({nameof(Cdm.ChargeId)} like '{filterTextBox.Text.ToUpper()}*')";

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
