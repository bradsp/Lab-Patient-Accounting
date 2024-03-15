using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Forms;

public partial class ClaimsManagementForm : Form
{
    private CancellationTokenSource cancellationToken;

    private List<BillingBatch> _billingBatches = new();
    private List<BillingActivity> _billingActivities = new();
    private readonly AccountService _accountService;
    private readonly ClaimGeneratorService _claimGeneratorService;
    private BindingSource _billingBatchBindingSource;
    private BindingSource _billingActivitiesBindingSource;
    private DataTable _billingBatchTable;
    private DataTable _billingActivitiesTable;
    public event EventHandler<string> AccountLaunched;

    public ClaimsManagementForm() 
    {
        InitializeComponent();

        _accountService = new(Program.AppEnvironment);
        _claimGeneratorService = new(Program.AppEnvironment);
    }

    private void ClaimsManagementForm_Load(object sender, EventArgs e)
    {
        cancelButton.Enabled = false;
        claimProgressStatusLabel.Text = "";

        _billingBatchBindingSource = [];

        LoadData();

        claimBatchDataGrid.Columns[nameof(BillingBatch.UpdatedApp)].Visible = false;
        claimBatchDataGrid.Columns[nameof(BillingBatch.UpdatedDate)].Visible = false;
        claimBatchDataGrid.Columns[nameof(BillingBatch.UpdatedHost)].Visible = false;
        claimBatchDataGrid.Columns[nameof(BillingBatch.UpdatedUser)].Visible = false;
        claimBatchDataGrid.Columns[nameof(BillingBatch.rowguid)].Visible = false;
        claimBatchDataGrid.Columns[nameof(BillingBatch.X12Text)].Visible = false;
        claimBatchDataGrid.Columns[nameof(BillingBatch.TotalBilled)].DefaultCellStyle.Format = "N2";
        claimBatchDataGrid.Columns[nameof(BillingBatch.TotalBilled)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        claimBatchDataGrid.AutoResizeColumns();

    }

    private void LoadData()
    {
        _billingBatches = _claimGeneratorService.GetBillingBatches();
        _billingBatchTable = _billingBatches.ToDataTable();
        _billingBatchTable.PrimaryKey = new DataColumn[] { _billingBatchTable.Columns[nameof(BillingBatch.Batch)] };
        _billingBatchTable.DefaultView.Sort = $"{nameof(BillingBatch.Batch)} desc";

        _billingBatchBindingSource.DataSource = _billingBatchTable;

        _billingActivitiesBindingSource = new BindingSource();

        claimBatchDataGrid.DataSource = _billingBatchBindingSource;

    }

    private enum BillingType
    {
        Institutional,
        Professional
    }

    private async Task RunBillingBatch(BillingType billingType)
    {
        cancellationToken = new CancellationTokenSource();

        claimProgress = new ProgressBar
        {
            Style = ProgressBarStyle.Continuous,
            Minimum = 0
        };

        claimProgressStatusLabel.Text = "Processing...";

        Progress<ProgressReportModel> progress = new();
        progress.ProgressChanged += ReportProgress;
        cancelButton.Enabled = true;
        try
        {
            int claimsProcessed = 0;
            if (billingType == BillingType.Institutional)
            {
                claimsProcessed = await Task.Run(() =>
                {
                    return _claimGeneratorService.CompileBillingBatch(ClaimType.Institutional, progress, cancellationToken.Token);
                });
            }
            else if (billingType == BillingType.Professional)
            {
                claimsProcessed = await Task.Run(() =>
                {
                    return _claimGeneratorService.CompileBillingBatch(ClaimType.Professional, progress, cancellationToken.Token);
                });
            }

            if (claimsProcessed < 0)
            {
                MessageBox.Show(this, "Error processing claims. No file generated.", "Process Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show(this, $"File generated. {claimsProcessed} claims generated.");
            }
        }
        catch (TaskCanceledException tce)
        {
            Log.Instance.Error(tce, $"{Enum.GetName(typeof(BillingType), billingType)} Claim Batch cancelled by user", tce);
            MessageBox.Show(this, "Claim batch cancelled by user. No file was generated and batch has been rolled back.");
            claimProgressStatusLabel.Text = "Job aborted.";
            claimProgress.Value = claimProgress.Maximum;
            claimProgress.SetState(2);
            cancelButton.Enabled = false;
        }
        cancelButton.Enabled = false;
        LoadData();
    }

    private void ReportProgress(object sender, ProgressReportModel e)
    {
        claimProgress.Maximum = e.TotalRecords;
        claimProgress.Value = e.RecordsProcessed;
        claimProgressStatusLabel.Text = $"Processing {e.RecordsProcessed} of {e.TotalRecords}";
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("This will cancel the claim batch and rollback any changes. Are you sure?", "Cancel Batch?",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            cancellationToken.Cancel();
    }

    private void claimBatchDataGrid_SelectionChanged(object sender, EventArgs e)
    {
        if (claimBatchDataGrid.SelectedRows.Count > 0)
        {
            var batch = claimBatchDataGrid.SelectedRows[0].Cells[nameof(BillingBatch.Batch)].Value.ToString();

            _billingActivitiesTable = _claimGeneratorService.GetBillingBatchActivity(batch).ToDataTable();
            _billingActivitiesBindingSource.DataSource = _billingActivitiesTable;

            claimBatchDetailDataGrid.DataSource = _billingActivitiesBindingSource;

            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.IsPrinted)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.Text)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.IsDeleted)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.UpdatedDate)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.UpdatedHost)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.UpdatedApp)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.UpdatedUser)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.rowguid)].Visible = false;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.ClaimAmount)].DefaultCellStyle.Format = "N2";
            claimBatchDetailDataGrid.Columns[nameof(BillingActivity.ClaimAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            claimBatchDetailDataGrid.AutoResizeColumns();

            _billingActivitiesTable.DefaultView.Sort = $"{nameof(BillingActivity.PatientName)}";
        }

    }

    private void claimBatchDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        //show the x12 text in a dialog box.


    }

    private void clearBatchToolStripMenuItem_Click(object sender, EventArgs e)
    {
        //get batch number
        var selectedBatch = claimBatchDataGrid.SelectedRows[0].Cells[nameof(BillingBatch.Batch)].Value;

        double batchNo = Convert.ToDouble(selectedBatch);
        Cursor.Current = Cursors.WaitCursor;
        if (_claimGeneratorService.ClearBatch(batchNo))
        {
            _billingBatchTable.Rows.Find(batchNo).Delete();
        }
        Cursor.Current = Cursors.Default;
    }

    private void regenerateClaimFileToolStripMenuItem_Click(object sender, EventArgs e)
    {
        var selectedBatch = claimBatchDataGrid.SelectedRows[0].Cells[nameof(BillingBatch.Batch)].Value;
        double batchNo = Convert.ToDouble(selectedBatch);

        ClaimGeneratorService claims = new ClaimGeneratorService(Program.AppEnvironment);

        Cursor.Current = Cursors.WaitCursor;

        claims.RegenerateBatch(batchNo);

        Cursor.Current = Cursors.Default;

    }

    private void generateClaimsButton_Click(object sender, EventArgs e)
    {
        BillingType billingType;

        if (institutionalRadioButton.Checked)
        {
            billingType = BillingType.Institutional;
        }
        else if (professionalRadioButton.Checked)
        {
            billingType = BillingType.Professional;
        }
        else
        {
            return;
        }

        _ = RunBillingBatch(billingType);

    }

    private void claimBatchDetailDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
    {
        var selectedAccount = claimBatchDetailDataGrid.SelectedRows[0].Cells[nameof(BillingActivity.AccountNo)].Value.ToString();

        if (!string.IsNullOrEmpty(selectedAccount))
        {
            AccountLaunched?.Invoke(this, selectedAccount);
        }
    }
}
