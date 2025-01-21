using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using System.ComponentModel;
using System.IO;
using WinFormsLibrary;

namespace LabBilling.Forms;
public partial class ProcessRemittanceForm : Form
{
    private readonly Remittance835Service remittanceService = new(Program.AppEnvironment);
    private readonly BindingList<RemittanceFile> remittanceBindingList = new();
    private readonly BindingSource remittanceBindingSource = new();
    private readonly ProgressBar progressBar;
    private readonly Label progressLabel;

    public event EventHandler<int> RemittanceFileSelected;

    public ProcessRemittanceForm()
    {
        InitializeComponent();

        progressBar = new()
        {
            Dock = DockStyle.Bottom,
            Visible = false
        };
        Controls.Add(progressBar);

        progressLabel = new()
        {
            Dock = DockStyle.Bottom,
            TextAlign = ContentAlignment.MiddleCenter,
            Visible = false
        };

        // Add progressBar and progressLabel to statusStrip1
        statusStrip1.Items.Add(new ToolStripControlHost(progressBar));
        statusStrip1.Items.Add(new ToolStripControlHost(progressLabel));
    }

    private async Task importRemittances()
    {
        var remitImportDirectory = Program.AppEnvironment.ApplicationParameters.RemitImportDirectory;
        var archiveDirectory = Path.Combine(remitImportDirectory, "archive");

        if (!Directory.Exists(archiveDirectory))
        {
            Directory.CreateDirectory(archiveDirectory);
        }

        var remittanceFiles = Directory.GetFiles(remitImportDirectory, "*.835");
        progressBar.Maximum = remittanceFiles.Length;
        progressBar.Value = 0;
        progressLabel.Text = "Importing remittance files...";

        foreach (var remittanceFile in remittanceFiles)
        {
            try
            {
                await Task.Run(() => remittanceService.Load835(remittanceFile));
                var fileName = Path.GetFileName(remittanceFile);
                var archiveFilePath = Path.Combine(archiveDirectory, fileName);
                File.Move(remittanceFile, archiveFilePath);
                progressBar.Value++;
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"Failed to import file {remittanceFile}: {ex.Message}");
                MessageBox.Show($"Failed to import file {Path.GetFileName(remittanceFile)}. \n\nNotify the administrator. \n\nYou may continue processing imported remittances.", "Import Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        remittanceBindingList.Clear();
        remittanceBindingList.AddRange(await remittanceService.GetAllRemittancesAsync());

        progressLabel.Text = "Import completed.";
    }

    private void ProcessRemittanceForm_Load(object sender, EventArgs e)
    {
        importRemittances();

        remittanceBindingList.Clear();
        remittancesDataGridView.DataSource = null;

        remittanceBindingList.AddRange(remittanceService.GetAllRemittances());
        remittancesDataGridView.DataSource = remittanceBindingList;

        remittancesDataGridView.SetColumnsVisibility(false);
        int z = 0;
        remittancesDataGridView.Columns[nameof(RemittanceFile.FileName)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.Payer)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.ProcessedDate)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.PostedDate)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalChargeAmount)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalChargeAmount)].DefaultCellStyle.Format = "N2";
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalPaymentAmount)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalPaymentAmount)].DefaultCellStyle.Format = "N2";
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalPatientResponsibilityAmount)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalPatientResponsibilityAmount)].DefaultCellStyle.Format = "N2";
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalPaidAmount)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalPaidAmount)].DefaultCellStyle.Format = "N2";
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalAllowedAmount)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.TotalAllowedAmount)].DefaultCellStyle.Format = "N2";
        remittancesDataGridView.Columns[nameof(RemittanceFile.ClaimCount)].SetVisibilityOrder(true, z++);
        remittancesDataGridView.Columns[nameof(RemittanceFile.UpdatedDate)].SetVisibilityOrder(true, z++);

        // Set alternating row style
        remittancesDataGridView.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;

        //resize the columns to fit the data
        remittancesDataGridView.Columns[nameof(RemittanceFile.Payer)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;

        remittancesDataGridView.AutoResizeColumns();
    }

    private void remittancesDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        //Get the selected remittance file and invoke the event
        if (e.RowIndex >= 0)
        {
            var selectedRemittance = remittanceBindingList[e.RowIndex];
            RemittanceFileSelected?.Invoke(this, selectedRemittance.RemittanceId);
        }
    }

    private void markRemittancePostedToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Get the selected remittance and update the posted date with today's date
        if (remittancesDataGridView.SelectedRows.Count > 0)
        {
            var selectedRemittance = (RemittanceFile)remittancesDataGridView.SelectedRows[0].DataBoundItem;
            selectedRemittance.PostedDate = DateTime.Today;
            selectedRemittance.PostingUser = Environment.UserName;
            selectedRemittance.PostingHost = Environment.MachineName;
            remittanceService.UpdateRemittance(selectedRemittance);
            remittanceBindingList.ResetBindings();
        }

    }

    private void reimportRemittanceToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }

    private void reversePostingToolStripMenuItem_Click(object sender, EventArgs e)
    {

    }
}
