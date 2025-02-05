using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using LabBilling.Core.Models;
using LabBilling.Core.Services;

namespace LabBilling.Forms;

public partial class CdmLookupForm : Form
{
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public int CharacterLookupCountMin { get; set; } = 3;
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public List<Cdm> Datasource { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string InitialSearchText { get; set; }
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
    public string SelectedValue { get; set; }

    private const int _timerInterval = 650;
    private bool skipSelectionChanged = false;
    private System.Windows.Forms.Timer _timer;

    private readonly DictionaryService dictionaryService = new(Program.AppEnvironment, Program.UnitOfWork);

    public CdmLookupForm() 
    {
        InitializeComponent();

        _timer = new System.Windows.Forms.Timer() { Enabled = false, Interval = _timerInterval };
        _timer.Tick += new EventHandler(searchTextBox_OnKeyUpDone);
    }

    private void LookupForm_Load(object sender, EventArgs e)
    {
        searchTextBox.Focus();
    }

    private void searchTextBox_KeyUp(object sender, KeyEventArgs e)
    {
        _timer.Stop();
        _timer.Start();
    }

    private void searchTextBox_OnKeyUpDone(object sender, EventArgs args)
    {
        _timer.Stop();

        if (Datasource != null)
        {
            //load box with selectable entries once there are the minumum number of letters in the box
            if (searchTextBox.Text.Length >= CharacterLookupCountMin)
            {
                skipSelectionChanged = true;

                resultsDataGrid.DataSource = null;
                resultsDataGrid.Rows.Clear();
                resultsDataGrid.Refresh();

                IEnumerable<Cdm> cdmQuery = null;

                if (cdmDescSearchRadioButton.Checked)
                {
                    cdmQuery =
                        from cdm in Datasource
                        where cdm.Description.Contains(searchTextBox.Text.ToUpper()) || cdm.ChargeId.Equals(searchTextBox.Text.ToUpper())
                        select cdm;
                }

                if(cptSearchRadioButton.Checked)
                {
                    cdmQuery = dictionaryService.GetCdmByCpt(searchTextBox.Text.ToUpper());
                }

                if (cdmQuery.Any())
                {
                    resultsDataGrid.DataSource = cdmQuery.ToList();
                    resultsDataGrid.Columns.OfType<DataGridViewColumn>().ToList().ForEach(col => col.Visible = false);
                    resultsDataGrid.Columns[nameof(Cdm.Description)].Visible = true;
                    resultsDataGrid.Columns[nameof(Cdm.ChargeId)].Visible = true;
                    resultsDataGrid.Columns[nameof(Cdm.Description)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                    resultsDataGrid.AutoResizeColumns();
                    resultsDataGrid.ClearSelection();
                }
                skipSelectionChanged = false;
            }
        }
    }

    private void resultsDataGrid_SelectionChanged(object sender, EventArgs e)
    {
        if (!skipSelectionChanged)
        {
            if (resultsDataGrid.SelectedRows.Count > 0)
            {
                DataGridViewRow row = resultsDataGrid.SelectedRows[0];
                if (row != null)
                {
                    SelectedValue = row.Cells[nameof(Cdm.ChargeId)].Value.ToString();
                }
            }
        }
    }

    private void LookupForm_Shown(object sender, EventArgs e)
    {
        if(!string.IsNullOrEmpty(InitialSearchText))
        {
            searchTextBox.Text = InitialSearchText;
            searchTextBox_OnKeyUpDone(sender, e);
            searchTextBox.Focus();
        }
    }

    private void resultsDataGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
    {
        selectButton_Click(sender, e);
    }

    private void selectButton_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.OK;
    }

    private void cdmDescSearchRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if(cdmDescSearchRadioButton.Checked)
            searchTextBox_OnKeyUpDone(sender, e);
    }

    private void cptSearchRadioButton_CheckedChanged(object sender, EventArgs e)
    {
        if(cptSearchRadioButton.Checked)
            searchTextBox_OnKeyUpDone(sender, e);
    }
}
