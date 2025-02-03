using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Library;
using LabBilling.Logging;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using Utilities;
using WinFormsLibrary;

namespace LabBilling.Forms;
public partial class ProcessRemittanceForm : Form
{
    private readonly Remittance835Service remittanceService = new(Program.AppEnvironment);
    private readonly BindingSource remittanceBindingSource = new();
    private readonly ProgressBar progressBar;
    private readonly Label progressLabel;
    private DataTable _remittancesDt;
    private string lastSortedColumn;
    private ListSortDirection lastSortDirection;
    private Dictionary<string, int> columnWidths = new();

    public event EventHandler<int> RemittanceFileSelected;

    public ProcessRemittanceForm()
    {
        InitializeComponent();
        remittancesDataGridView.VirtualMode = true;
        remittancesDataGridView.CellValueNeeded += RemittancesDataGridView_CellValueNeeded;
        remittancesDataGridView.CellValuePushed += RemittancesDataGridView_CellValuePushed;
        remittancesDataGridView.ColumnHeaderMouseClick += RemittancesDataGridView_ColumnHeaderMouseClick;
        remittancesDataGridView.DataBindingComplete += RemittancesDataGridView_DataBindingComplete;
        remittancesDataGridView.RowPrePaint += RemittancesDataGridView_RowPrePaint;
        remittanceBindingSource.ListChanged += RemittanceBindingSource_ListChanged;
        remittancesDataGridView.CellMouseDoubleClick += remittancesDataGridView_CellMouseDoubleClick;
        remittancesDataGridView.MouseDown += RemittancesDataGridView_MouseDown;
        remittancesDataGridView.ColumnWidthChanged += RemittancesDataGridView_ColumnWidthChanged;
        remittancesDataGridView.DoubleBuffered(true); // Enable double buffering
        DefineColumns();

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

    private void RemittancesDataGridView_ColumnWidthChanged(object sender, DataGridViewColumnEventArgs e)
    {
        columnWidths[e.Column.Name] = e.Column.Width;
    }

    private void RemittancesDataGridView_MouseDown(object sender, MouseEventArgs e)
    {
        if (e.Button == MouseButtons.Right)
        {
            var hitTestInfo = remittancesDataGridView.HitTest(e.X, e.Y);
            if (hitTestInfo.RowIndex >= 0)
            {
                remittancesDataGridView.ClearSelection();
                remittancesDataGridView.Rows[hitTestInfo.RowIndex].Selected = true;
            }
        }
    }

    private void RemittancesDataGridView_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
    {
        if (e.RowIndex >= 0 && e.RowIndex < _remittancesDt.Rows.Count)
        {
            var row = remittancesDataGridView.Rows[e.RowIndex];
            var postingDateCell = row.Cells[nameof(RemittanceFile.PostedDate)];
            if (postingDateCell.Value != null && postingDateCell.Value != DBNull.Value)
            {
                row.DefaultCellStyle.Font = new Font(remittancesDataGridView.Font, FontStyle.Italic);
                row.DefaultCellStyle.ForeColor = Color.Gray;
            }
            else
            {
                row.DefaultCellStyle.Font = new Font(remittancesDataGridView.Font, FontStyle.Regular);
                row.DefaultCellStyle.ForeColor = Color.Black;
            }

            // Apply alternating row shading
            if (row.Index % 2 == 0)
            {
                row.DefaultCellStyle.BackColor = Color.LightCyan;
            }
            else
            {
                row.DefaultCellStyle.BackColor = Color.White;
            }
        }
    }

    private void RemittancesDataGridView_CellValuePushed(object sender, DataGridViewCellValueEventArgs e)
    {
        if (e.RowIndex >= 0 && e.RowIndex < _remittancesDt.Rows.Count)
        {
            var columnName = remittancesDataGridView.Columns[e.ColumnIndex].DataPropertyName;
            if (_remittancesDt.Columns.Contains(columnName))
            {
                _remittancesDt.Rows[e.RowIndex][columnName] = e.Value;
            }
        }
    }

    private void RemittancesDataGridView_CellValueNeeded(object sender, DataGridViewCellValueEventArgs e)
    {
        if (e.RowIndex >= 0 && e.RowIndex < _remittancesDt.Rows.Count)
        {
            var columnName = remittancesDataGridView.Columns[e.ColumnIndex].DataPropertyName;
            if (_remittancesDt.Columns.Contains(columnName))
            {
                e.Value = _remittancesDt.Rows[e.RowIndex][columnName];
            }
        }
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

        progressLabel.Text = "Import completed.";
    }

    private async void ProcessRemittanceForm_Load(object sender, EventArgs e)
    {
        reimportUnpostedRemittancesToolStripButton.Visible = Program.AppEnvironment.UserAccount.IsAdministrator;

        await importRemittances();

        _remittancesDt = remittanceService.GetAllRemittances().ToDataTable();

        _remittancesDt.PrimaryKey = new[] { _remittancesDt.Columns[nameof(RemittanceFile.RemittanceId)] };

        remittanceBindingSource.DataSource = _remittancesDt;
        remittancesDataGridView.RowCount = _remittancesDt.Rows.Count;
        remittancesDataGridView.AllowUserToResizeColumns = true;

        //restore column widths
        foreach (DataGridViewColumn column in remittancesDataGridView.Columns)
        {
            if (columnWidths.ContainsKey(column.Name))
            {
                column.Width = columnWidths[column.Name];
            }
        }


        //set default sort to ProcessedDate Descending
        DataView dataView = _remittancesDt.DefaultView;
        dataView.Sort = $"{nameof(RemittanceFile.ProcessedDate)} DESC";
        _remittancesDt = dataView.ToTable();
        _remittancesDt.PrimaryKey = new[] { _remittancesDt.Columns[nameof(RemittanceFile.RemittanceId)] };
        lastSortedColumn = nameof(RemittanceFile.ProcessedDate);
        lastSortDirection = ListSortDirection.Descending;
        remittancesDataGridView.Columns[nameof(RemittanceFile.ProcessedDate)].HeaderCell.SortGlyphDirection = SortOrder.Descending;

    }

    private void remittancesDataGridView_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        //Get the selected remittance file and invoke the event
        if (e.RowIndex >= 0)
        {
            var remittanceId = (int)remittancesDataGridView.Rows[e.RowIndex].Cells[nameof(RemittanceFile.RemittanceId)].Value;
            RemittanceFileSelected?.Invoke(this, remittanceId);
        }
    }

    private void markRemittancePostedToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Get the selected remittance and update the posted date with today's date
        if (remittancesDataGridView.SelectedRows.Count > 0)
        {
            // Get the remittance ID from the selected row
            var remittanceId = (int)remittancesDataGridView.SelectedRows[0].Cells[nameof(RemittanceFile.RemittanceId)].Value;

            // Find the corresponding row in the DataTable
            var remittanceRow = _remittancesDt.Rows.Find(remittanceId);

            var selectedRemittance = remittanceRow.ConvertDataRowToObject<RemittanceFile>();

            // Check if the remittance is already posted
            if (selectedRemittance.PostedDate.HasValue)
            {
                MessageBox.Show("This remittance is already marked as posted.", "Already Posted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirm with the user if they want to mark the remittance as posted
            var result = MessageBox.Show("Do you want to mark this remittance as posted?", "Confirm Posting", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                selectedRemittance.PostedDate = DateTime.Today;
                selectedRemittance.PostingUser = Environment.UserName;
                selectedRemittance.PostingHost = Environment.MachineName;
                remittanceService.UpdateRemittance(selectedRemittance);

                // Update the DataTable to reflect the changes
                var row = _remittancesDt.Rows.Find(selectedRemittance.RemittanceId);
                if (row != null)
                {
                    row[nameof(RemittanceFile.PostedDate)] = selectedRemittance.PostedDate;
                    row[nameof(RemittanceFile.PostingUser)] = selectedRemittance.PostingUser;
                    row[nameof(RemittanceFile.PostingHost)] = selectedRemittance.PostingHost;
                }

                remittanceBindingSource.ResetBindings(false);
            }
        }
    }

    private void RefreshRemittancesData(int? selectedRemittanceId = null)
    {
        // Clear and reload the DataTable
        _remittancesDt.Clear();
        _remittancesDt = remittanceService.GetAllRemittances(true).ToDataTable();

        // Set the primary key
        _remittancesDt.PrimaryKey = new[] { _remittancesDt.Columns[nameof(RemittanceFile.RemittanceId)] };

        // Reapply the previous sort if available
        if (!string.IsNullOrEmpty(lastSortedColumn))
        {
            string sortDirection = lastSortDirection == ListSortDirection.Ascending ? "ASC" : "DESC";
            _remittancesDt.DefaultView.Sort = $"{lastSortedColumn} {sortDirection}";
            _remittancesDt = _remittancesDt.DefaultView.ToTable();
            _remittancesDt.PrimaryKey = new[] { _remittancesDt.Columns[nameof(RemittanceFile.RemittanceId)] };

            // Update the sort glyph
            remittancesDataGridView.Columns[lastSortedColumn].HeaderCell.SortGlyphDirection =
                lastSortDirection == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;
        }

        // Update the DataSource
        remittanceBindingSource.DataSource = _remittancesDt;
        remittancesDataGridView.RowCount = _remittancesDt.Rows.Count;

        // Reposition to the selected row if provided
        if (selectedRemittanceId.HasValue)
        {
            int rowIndex = -1;
            DataRow selectedRow = _remittancesDt.Rows.Find(selectedRemittanceId.Value);
            if (selectedRow != null)
            {
                rowIndex = _remittancesDt.Rows.IndexOf(selectedRow);
            }

            if (rowIndex >= 0)
            {
                remittancesDataGridView.CurrentCell = remittancesDataGridView.Rows[rowIndex].Cells[0];
            }
        }

        // Refresh the DataGridView
        remittancesDataGridView.Refresh();
    }

    private void reimportRemittanceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Get the selected remittance and reimport the remittance file
        // This will delete the existing claims and claim details and reload them from the remittance file

        if (remittancesDataGridView.SelectedRows.Count > 0)
        {
            //add message to user explaining that the remittance will be reimported and get verification
            var result = MessageBox.Show("Reimporting the remittance will delete all existing claims and claim details and reload them from the remittance file. Do you want to continue?", "Reimport Remittance", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes)
            {
                return;
            }
            // Save the RemittanceId of the selected row
            var selectedRemittanceId = (int)remittancesDataGridView.SelectedRows[0].Cells[nameof(RemittanceFile.RemittanceId)].Value;

            var remittanceId = (int)remittancesDataGridView.SelectedRows[0].Cells[nameof(RemittanceFile.RemittanceId)].Value;
            if (remittanceService.ReimportRemittance(remittanceId) == null)
            {
                MessageBox.Show("Failed to reimport remittance. Notify the administrator.", "Reimport Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                RefreshRemittancesData(selectedRemittanceId);
            }
        }
    }

    private void viewSourceDataToolStripMenuItem_Click(object sender, EventArgs e)
    {
        // Get the selected remittance and display the source data
        if (remittancesDataGridView.SelectedRows.Count > 0)
        {
            // Get the remittance ID from the selected row
            var remittanceId = (int)remittancesDataGridView.SelectedRows[0].Cells[nameof(RemittanceFile.RemittanceId)].Value;

            // Find the corresponding row in the DataTable
            var remittanceRow = _remittancesDt.Rows.Find(remittanceId);
            if (remittanceRow != null)
            {
                // Get the file name from the DataTable row
                var fileName = remittanceRow[nameof(RemittanceFile.FileName)].ToString();

                // Open the text file from the remittance file. Create a dynamic form with a textbox to display the data
                var remitImportDirectory = Program.AppEnvironment.ApplicationParameters.RemitImportDirectory;
                var remittanceFile = Path.Combine(remitImportDirectory, "archive", fileName);

                // Create a new form to display the source data
                var sourceDataForm = new Form
                {
                    Text = "Remittance Source Data",
                    Width = 800,
                    Height = 600
                };

                var sourceDataTextBox = new TextBox
                {
                    Multiline = true,
                    Dock = DockStyle.Fill,
                    ScrollBars = ScrollBars.Both
                };

                sourceDataForm.Controls.Add(sourceDataTextBox);

                // Read the file and display the data
                var text = File.ReadAllText(remittanceFile);
                // Add line breaks for each segment ending in '~'
                text = text.Replace("~", "~\r\n");
                sourceDataTextBox.Text = text;
                sourceDataForm.ShowDialog();
            }
        }
    }

    private DataGridViewTextBoxColumn CreateTextBoxColumn(string dataPropertyName, string headerText, int width, DataGridViewCellStyle cellStyle = null)
    {
        return new DataGridViewTextBoxColumn
        {
            DataPropertyName = dataPropertyName,
            HeaderText = headerText,
            Name = dataPropertyName,
            Width = width,
            DefaultCellStyle = cellStyle
        };
    }

    private DataGridViewDateColumn CreateDateColumn(string dataPropertyName, string headerText, int width)
    {
        return new DataGridViewDateColumn
        {
            DataPropertyName = dataPropertyName,
            HeaderText = headerText,
            Name = dataPropertyName,
            Width = width
        };
    }

    private void DefineColumns()
    {
        remittancesDataGridView.Columns.Clear();

        // Create cell style for numeric columns
        var numericCellStyle = new DataGridViewCellStyle
        {
            Format = "N2",
            Alignment = DataGridViewContentAlignment.MiddleRight
        };

        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.FileName), "File Name", 400));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.Payer), "Payer", 400));
        remittancesDataGridView.Columns.Add(CreateDateColumn(nameof(RemittanceFile.ProcessedDate), "Processed Date", 100));
        remittancesDataGridView.Columns.Add(CreateDateColumn(nameof(RemittanceFile.PostedDate), "Posted Date", 100));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.TotalChargeAmount), "Total Charge Amount", 100, numericCellStyle));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.TotalPaymentAmount), "Total Payment Amount", 100, numericCellStyle));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.TotalPatientResponsibilityAmount), "Total Patient Responsibility Amount", 100, numericCellStyle));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.TotalPaidAmount), "Total Paid Amount", 100, numericCellStyle));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.TotalAllowedAmount), "Total Allowed Amount", 100, numericCellStyle));
        remittancesDataGridView.Columns.Add(CreateTextBoxColumn(nameof(RemittanceFile.ClaimCount), "Claim Count", 100));
        remittancesDataGridView.Columns.Add(CreateDateColumn(nameof(RemittanceFile.UpdatedDate), "Updated Date", 100));

        remittancesDataGridView.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = nameof(RemittanceFile.RemittanceId),
            HeaderText = "Remittance Id",
            Name = nameof(RemittanceFile.RemittanceId),
            Visible = false
        });
    }

    private void RemittancesDataGridView_DataBindingComplete(object sender, DataGridViewBindingCompleteEventArgs e)
    {
        this.Cursor = Cursors.Default;
    }

    private void RemittancesDataGridView_ColumnHeaderMouseClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        string columnName = remittancesDataGridView.Columns[e.ColumnIndex].DataPropertyName;
        ListSortDirection direction;

        // Toggle sort direction if the same column is clicked
        if (lastSortedColumn == columnName)
        {
            direction = lastSortDirection == ListSortDirection.Ascending
                ? ListSortDirection.Descending
                : ListSortDirection.Ascending;
        }
        else
        {
            direction = ListSortDirection.Ascending;
            remittancesDataGridView.Columns[lastSortedColumn].HeaderCell.SortGlyphDirection = SortOrder.None;
        }
        this.Cursor = Cursors.WaitCursor;

        // Apply sorting to the DataTable
        DataView dataView = _remittancesDt.DefaultView;
        dataView.Sort = $"{columnName} {(direction == ListSortDirection.Ascending ? "ASC" : "DESC")}";
        _remittancesDt = dataView.ToTable();
        _remittancesDt.PrimaryKey = new[] { _remittancesDt.Columns[nameof(RemittanceFile.RemittanceId)] };
        //have the appropriate grid column reflect the sort direction
        remittancesDataGridView.Columns[columnName].HeaderCell.SortGlyphDirection = direction == ListSortDirection.Ascending ? SortOrder.Ascending : SortOrder.Descending;

        // Update the last sorted column and direction
        lastSortedColumn = columnName;
        lastSortDirection = direction;

        // Refresh the DataGridView
        remittancesDataGridView.RowCount = _remittancesDt.Rows.Count;
        remittancesDataGridView.Refresh();

        // Reset the cursor
        this.Cursor = Cursors.Default;
    }

    private void RemittancesDataGridView_Sorted(object sender, EventArgs e)
    {
        // Sorting has ended
        this.Cursor = Cursors.Default;
    }

    private void RemittanceBindingSource_ListChanged(object sender, ListChangedEventArgs e)
    {
        if (e.ListChangedType == ListChangedType.Reset)
        {
            // Sorting has begun
            this.Cursor = Cursors.Default;
        }
    }

    private async void reimportUnpostedRemittancesToolStripButton_Click(object sender, EventArgs e)
    {
        var progress = new Progress<ProgressReportModel>();
        progress.ProgressChanged += (s, report) =>
        {
            progressBar.Maximum = report.TotalRecords;
            progressBar.Value = report.RecordsProcessed;
            progressLabel.Text = report.StatusMessage;
        };

        // Disable UI elements during the operation
        this.Enabled = false;
        progressBar.Visible = true;
        progressLabel.Visible = true;

        var results = await remittanceService.ReimportUnpostedRemittancesAsync(progress);

        // Re-enable UI elements after completion
        this.Enabled = true;
        progressBar.Visible = false;
        progressLabel.Visible = false;

        // Refresh the data grid
        RefreshRemittancesData();

        // Show summary of the operation
        int successCount = results.Count(r => r.Success);
        int failureCount = results.Count(r => !r.Success);
        MessageBox.Show($"Reimport completed.\n\nSuccessful: {successCount}\nFailed: {failureCount}", "Reimport All Unposted Remittances", MessageBoxButtons.OK, MessageBoxIcon.Information);

    }
}
