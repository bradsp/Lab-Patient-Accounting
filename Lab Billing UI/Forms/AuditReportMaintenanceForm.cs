using BasicSQLFormatter;
using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using NPOI.OpenXmlFormats.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Forms;
public partial class AuditReportMaintenanceForm : Form
{
    public AuditReportMaintenanceForm()
    {
        InitializeComponent();
    }

    private DictionaryService _dictionaryService;
    private BindingSource _bindingSource;
    private DataTable _auditReports = new();

    private void AuditReportMaintenanceForm_Load(object sender, EventArgs e)
    {
        _dictionaryService = new(Program.AppEnvironment);
        //load grid
        try
        {
            _bindingSource = [];
            _auditReports = _dictionaryService.GetAuditReports().ToDataTable();

            if (_auditReports.Rows.Count > 0)
            {
                _bindingSource.DataSource = _auditReports;

                reportListDataGrid.DataSource = _bindingSource;
            }
            else
            {
                MessageBox.Show("No records found.");
                Close();
            }
            reportListDataGrid.SetColumnsVisibility(false);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.ReportName), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.ReportTitle), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.Comments), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.Button), true);
            reportListDataGrid.SetColumnVisibility(nameof(AuditReport.IsChildButton), true);
            reportListDataGrid.Columns[nameof(AuditReport.ReportName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            reportListDataGrid.AutoResizeColumns();
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex);            
        }

    }

    private void reportListDataGrid_SelectionChanged(object sender, EventArgs e)
    {
        if(reportListDataGrid.SelectedRows.Count <= 0)
            return;

        DataGridViewRow row = reportListDataGrid.SelectedRows[0];

        reportCodeTextbox.Text = new SQLFormatter(row.Cells[nameof(AuditReport.ReportCode)].Value.ToString()).Format();
        
        reportNameTextbox.Text = row.Cells[nameof(AuditReport.ReportName)].Value.ToString();
        reportTitleTextbox.Text = row.Cells[nameof(AuditReport.ReportTitle)].Value.ToString();
        buttonTextBox.Text = row.Cells[nameof(AuditReport.Button)].Value.ToString();
        commentsTextbox.Text = row.Cells[nameof(AuditReport.Comments)].Value.ToString();
        isChildButtonCheckBox.Checked = Convert.ToBoolean(row.Cells[nameof(AuditReport.IsChildButton)].Value);
    }

    private void saveButton_Click(object sender, EventArgs e)
    {

    }

    private void cancelButton_Click(object sender, EventArgs e)
    {

    }
}
