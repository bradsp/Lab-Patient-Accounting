using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Logging;
using Newtonsoft.Json;
using WinFormsLibrary;

namespace LabBilling.Forms;
public partial class PostRemittanceForm : Form
{
    private readonly Remittance835Service remittanceService = new(Program.AppEnvironment, Program.UnitOfWork);

    private readonly ProgressBar progressBar;
    private readonly Label progressLabel;
    private WebBrowser remittanceInfoWebBrowser;
    private RichTextBox remittanceInfoRichTextBox;
    public event EventHandler<string> AccountLaunched;

    private int _remittanceId;
    private RemittanceFile _selectedRemittance;
    private const string _postRemittanceText = "Post Remittance";
    private const string _unpostRemittanceText = "Unpost Remittance";

    public PostRemittanceForm(int remittanceId)
    {
        _remittanceId = remittanceId;
        InitializeComponent();
        remittanceInfoWebBrowser = new WebBrowser
        {
            Dock = DockStyle.Fill
        };
        remittanceInfoRichTextBox = new RichTextBox
        {
            Dock = DockStyle.Fill
        };

        splitContainer1.Panel1.Controls.Add(remittanceInfoWebBrowser);

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

    private void PostRemittanceForm_Load(object sender, EventArgs e)
    {
        LoadRemittance();
    }

    private void LoadRemittance()
    {
        _selectedRemittance = remittanceService.GetRemittance(_remittanceId);
        if (_selectedRemittance == null)
        {
            Log.Instance.Error($"Remittance not found. RemittanceId: {_remittanceId}");
            MessageBox.Show("Remittance not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return;
        }

        // Populate remittanceInfo RichTextBox with a human-readable version of RemittanceFile data
        var remittanceData = JsonConvert.DeserializeObject<RemittanceData>(_selectedRemittance.RemittanceData);

        try
        {
            remittanceInfoWebBrowser.DocumentText = remittanceService.ConvertRemittanceHeaderToHtml(remittanceData);
            //remittanceInfoRichTextBox.Rtf = remittanceService.ConvertRemittanceHeaderToRtf(remittanceData);
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error loading remittance data.");
            MessageBox.Show("Error loading remittance data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        // Load the claims into claimDataGridView
        claimDataGridView.DataSource = _selectedRemittance.Claims;
        claimDataGridView.SetColumnsVisibility(false);
        // Set the visibility order of the columns
        int z = 0;
        claimDataGridView.Columns[nameof(RemittanceClaim.AccountNo)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.PatientName)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.ProcessStatus)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.ClaimStatusCode)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.ClaimChargeAmount)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.ClaimPaymentAmount)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.ClaimFilingIndicatorCode)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.PaidAmount)].SetVisibilityOrder(true, z++);
        claimDataGridView.Columns[nameof(RemittanceClaim.AllowedAmount)].SetVisibilityOrder(true, z++);

        // Make the patient name column fill the remaining space
        claimDataGridView.Columns[nameof(RemittanceClaim.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        claimDataGridView.AutoResizeColumns();

        if (_selectedRemittance.PostedDate != null)
        {
            postRemittanceToolButton.Text = _unpostRemittanceText;
        }
        else
        {
            postRemittanceToolButton.Text = _postRemittanceText;
        }
    }

    private async void postRemittanceToolButton_Click(object sender, EventArgs e)
    {
        try
        {
            progressBar.Visible = true;
            progressLabel.Visible = true;

            var progress = new Progress<ProgressReportModel>(report =>
            {
                progressBar.Value = report.PercentageComplete;
                progressLabel.Text = report.StatusMessage;
            });

            if(postRemittanceToolButton.Text == _postRemittanceText)
            {
                var result = await remittanceService.HandleRemittanceAsync(_selectedRemittance.RemittanceId, true, progress);
                if(!result.Success)
                {
                    MessageBox.Show($"Error posting remittance.\n\n{result.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Remittance posted successfully.\n\n", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else
            {
                var result = await remittanceService.HandleRemittanceAsync(_selectedRemittance.RemittanceId, false, progress);
                if (!result.Success)
                {
                    MessageBox.Show($"Error unposting remittance.\n\n{result.ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show($"Remittance unposted successfully.\n\n", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }

            progressBar.Visible = false;
            progressLabel.Visible = false;

            LoadRemittance();
        }
        catch (Exception ex)
        {
            Log.Instance.Error(ex, "Error posting remittance.");
            MessageBox.Show("Error posting remittance.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void claimDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        //invoke AccountLaunched for the account number in the selected row
        if (e.RowIndex >= 0)
        {
            var accountNo = claimDataGridView.Rows[e.RowIndex].Cells[nameof(RemittanceClaim.AccountNo)].Value.ToString();
            AccountLaunched?.Invoke(this, accountNo);
        }

    }

    private void printRemittanceToolStripButton_Click(object sender, EventArgs e)
    {
        var remittanceData = JsonConvert.DeserializeObject<RemittanceData>(_selectedRemittance.RemittanceData);
        if (remittanceData != null)
        {
            var htmlContent = remittanceService.ConvertRemittanceDataToHtml(remittanceData);

            var htmlForm = new Form
            {
                Text = "Remittance Data HTML",
                Width = 800,
                Height = 600
            };

            var printButton = new Button
            {
                Text = "Print",
                Dock = DockStyle.Top
            };

            printButton.Click += (s, args) =>
            {
                var webBrowserForPrint = new WebBrowser
                {
                    DocumentText = htmlContent
                };
                webBrowserForPrint.DocumentCompleted += (s2, e2) =>
                {
                    PrintDialog printDialog = new PrintDialog();
                    if (printDialog.ShowDialog() == DialogResult.OK)
                    {
                        webBrowserForPrint.Print();
                    }
                };
            };

            var webBrowser = new WebBrowser
            {
                Dock = DockStyle.Fill,
                DocumentText = htmlContent
            };

            htmlForm.Controls.Add(printButton);
            htmlForm.Controls.Add(webBrowser);
            htmlForm.ShowDialog();
        }
        else
        {
            MessageBox.Show("Invalid remittance data.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }
}
