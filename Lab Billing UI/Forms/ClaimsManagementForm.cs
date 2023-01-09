using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core.Models;
using Opulos.Core.UI;
using LabBilling.Logging;
using LabBilling.Core.DataAccess;

namespace LabBilling.Forms
{
    public partial class ClaimsManagementForm : Form
    {
        private CancellationTokenSource cancellationToken;

        private List<BillingBatch> billingBatches = new List<BillingBatch>();
        private List<BillingActivity> billingActivities = new List<BillingActivity>();
        private BillingBatchRepository batchRepository;
        private BillingActivityRepository billingActivityRepository;
        private BindingSource billingBatchBindingSource;
        private BindingSource billingActivitiesBindingSource;
        private DataTable billingBatchTable;
        private DataTable billingActivitiesTable;

        public ClaimsManagementForm()
        {
            InitializeComponent();

            batchRepository = new BillingBatchRepository(Helper.ConnVal);
            billingActivityRepository = new BillingActivityRepository(Helper.ConnVal);
        }

        private void ClaimsManagementForm_Load(object sender, EventArgs e)
        {
            billingBatches = batchRepository.GetAll();
            billingBatchTable = billingBatches.ToDataTable();

            billingBatchBindingSource = new BindingSource();
            billingBatchBindingSource.DataSource = billingBatchTable;

            billingActivitiesBindingSource = new BindingSource();

            claimBatchDataGrid.DataSource = billingBatchBindingSource;
            claimBatchDataGrid.Columns[nameof(BillingBatch.mod_prg)].Visible = false;
            claimBatchDataGrid.Columns[nameof(BillingBatch.mod_date)].Visible = false;
            claimBatchDataGrid.Columns[nameof(BillingBatch.mod_host)].Visible = false;
            claimBatchDataGrid.Columns[nameof(BillingBatch.mod_user)].Visible = false;
            claimBatchDataGrid.Columns[nameof(BillingBatch.rowguid)].Visible = false;
            claimBatchDataGrid.Columns[nameof(BillingBatch.X12Text)].Visible = false;
            //claimBatchDataGrid.Columns[nameof(BillingBatch.X12Text)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            claimBatchDataGrid.AutoResizeColumns();

            billingBatchTable.DefaultView.Sort = $"{nameof(BillingBatch.Batch)} desc";
        }

        private enum BillingType
        {
            Institutional,
            Professional
        }

        private async Task RunBillingBatch(BillingType billingType)
        {
            cancellationToken = new CancellationTokenSource();

            //TableLayoutPanel tlpClaimBatch = new TableLayoutPanel { Dock = DockStyle.Fill };
            //tlpClaimBatch.ColumnCount = 1;
            //tlpClaimBatch.RowCount = 1;

            //Label claimProcessTitleLabel = new Label();
            //switch (billingType)
            //{
            //    case BillingType.Institutional:
            //        claimProcessTitleLabel.Text = "Institutional Claims";
            //        break;
            //    case BillingType.Professional:
            //        claimProcessTitleLabel.Text = "Professional Claims";
            //        break;
            //    default:
            //        throw new ArgumentOutOfRangeException(nameof(billingType));
            //}
            //tlpClaimBatch.Controls.Add(claimProcessTitleLabel, 0, 0);
            //claimProcessTitleLabel.Dock = DockStyle.Fill;

            claimProgress = new ProgressBar();
            claimProgress.Style = ProgressBarStyle.Continuous;
            claimProgress.Minimum = 0;
            //tlpClaimBatch.Controls.Add(claimProgress, 0, 1);
            //claimProgress.Dock = DockStyle.Fill;

            //claimProgressStatusLabel = new Label();
            //tlpClaimBatch.Controls.Add(claimProgressStatusLabel, 0, 2);
            claimProgressStatusLabel.Text = "Processing...";
            //claimProgressStatusLabel.Dock = DockStyle.Fill;

            //Button cancelButton = new Button { Text = "Cancel", Name = "cancelButton" };
            //cancelButton.Click += new EventHandler(cancelButton_Click);
            //tlpClaimBatch.Controls.Add(cancelButton, 0, 3);
            //cancelButton.Dock = DockStyle.Fill;

            //accordion.Add(tlpClaimBatch, "Claim Batch", "Claim Batch", 1, true);
            //accordion.PerformLayout();

            ClaimGenerator claims = new ClaimGenerator(Helper.ConnVal);
            Progress<ProgressReportModel> progress = new Progress<ProgressReportModel>();
            progress.ProgressChanged += ReportProgress;
            try
            {
                int claimsProcessed = 0;
                if (billingType == BillingType.Institutional)
                {
                    claimsProcessed = await Task.Run(() =>
                    {
                        return claims.CompileBillingBatch(Core.ClaimType.Institutional, progress, cancellationToken.Token);
                    });
                }
                else if (billingType == BillingType.Professional)
                {
                    claimsProcessed = await Task.Run(() =>
                    {
                        return claims.CompileBillingBatch(Core.ClaimType.Professional, progress, cancellationToken.Token);
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
                
                billingActivitiesTable = billingActivityRepository.GetBatch(batch).ToDataTable();
                billingActivitiesBindingSource.DataSource = billingActivitiesTable;

                claimBatchDetailDataGrid.DataSource = billingActivitiesBindingSource;

                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.IsPrinted)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.Text)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.IsDeleted)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.mod_date)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.mod_host)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.mod_prg)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.mod_user)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.rowguid)].Visible = false;
                claimBatchDetailDataGrid.Columns[nameof(BillingActivity.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                claimBatchDetailDataGrid.AutoResizeColumns();

                billingActivitiesTable.DefaultView.Sort = $"{nameof(BillingActivity.PatientName)}";
            }

        }

        private void claimBatchDataGrid_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //show the x12 text in a dialog box.


        }

        private void clearBatchToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void regenerateClaimFileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void generateClaimsButton_Click(object sender, EventArgs e)
        {

        }
    }
}
