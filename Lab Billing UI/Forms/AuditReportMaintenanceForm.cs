using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using PoorMansTSqlFormatterRedux;
using PoorMansTSqlFormatterRedux.Formatters;
using PoorMansTSqlFormatterRedux.Interfaces;
using System;
using System.Data;
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
            _auditReports.PrimaryKey = new DataColumn[] { _auditReports.Columns[nameof(AuditReport.Id)] };
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
        if (reportListDataGrid.SelectedRows.Count <= 0)
            return;

        DataGridViewRow row = reportListDataGrid.SelectedRows[0];
        var rawSql = row.Cells[nameof(AuditReport.ReportCode)].Value.ToString();

        var formatter = new TSqlStandardFormatter();
        var formatMgr = new SqlFormattingManager(formatter);

        var formattedSql = formatMgr.Format(rawSql);
        reportCodeTextbox.Text = formattedSql;

        reportNameTextbox.Text = row.Cells[nameof(AuditReport.ReportName)].Value.ToString();
        reportTitleTextbox.Text = row.Cells[nameof(AuditReport.ReportTitle)].Value.ToString();
        buttonTextBox.Text = row.Cells[nameof(AuditReport.Button)].Value.ToString();
        commentsTextbox.Text = row.Cells[nameof(AuditReport.Comments)].Value.ToString();
        isChildButtonCheckBox.Checked = Convert.ToBoolean(row.Cells[nameof(AuditReport.IsChildButton)].Value);
        idLabel.Text = row.Cells[nameof(AuditReport.Id)].Value.ToString();
    }

    private void saveButton_Click(object sender, EventArgs e)
    {
        AuditReport auditReport = new()
        {
            ReportName = reportNameTextbox.Text,
            ReportTitle = reportTitleTextbox.Text,
            ReportCode = reportCodeTextbox.Text,
            Comments = commentsTextbox.Text,
            Button = buttonTextBox.Text,
            IsChildButton = isChildButtonCheckBox.Checked,
            Id = Convert.ToInt32(idLabel.Text)
        };

        _dictionaryService.SaveAuditReport(auditReport);
        bool add = false;
        DataRow row = _auditReports.Rows.Find(Convert.ToInt32(auditReport.Id));
        if(row == null)
        {
            //add a row
            row = _auditReports.NewRow();
            row[nameof(AuditReport.Id)] = auditReport.Id;
            add = true;
        }
        row[nameof(AuditReport.ReportName)] = auditReport.ReportName;
        row[nameof(AuditReport.ReportTitle)] = auditReport.ReportTitle;
        row[nameof(AuditReport.ReportCode)] = auditReport.ReportCode;
        row[nameof(AuditReport.Comments)] = auditReport.Comments;
        row[nameof(AuditReport.Button)] = auditReport.Button;
        row[nameof(AuditReport.IsChildButton)] = auditReport.IsChildButton;
        if (add)
            _auditReports.Rows.Add(row);
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {

    }

    private void deleteToolStripButton_Click(object sender, EventArgs e)
    {
        if(MessageBox.Show($"Delete {reportTitleTextbox.Text}?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            if(_dictionaryService.DeleteAuditReport(Convert.ToInt32(idLabel.Text)))
            {
                DataRow row = _auditReports.Rows.Find(Convert.ToInt32(idLabel.Text));
                row.Delete();
            }
        }
    }
}
