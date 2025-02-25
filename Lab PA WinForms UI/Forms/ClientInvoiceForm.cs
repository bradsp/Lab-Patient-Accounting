﻿using LabBilling.Core.Models;
using LabBilling.Core.Services;
using LabBilling.Library;
using LabBilling.Logging;
using System.Diagnostics;
using System.IO;
using Utilities;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace LabBilling.Forms;

public partial class ClientInvoiceForm : Form
{
    public ClientInvoiceForm()
    {
        InitializeComponent();

        InvoiceHistoryTabPage.Enter += new System.EventHandler(InvoiceHistoryPage_Enter);
    }

    private DateTime _thruDate;
    private ClientInvoicesService _clientInvoicesService;
    //private DictionaryService _dictionaryService;
    private List<Client> _clientList;
    private List<UnbilledClient> _unbilledClients;
    private InvoiceWaitForm _invoiceWaitForm;
    public event EventHandler<string> AccountLaunched;
    private CancellationTokenSource _cts = new();
    private async void ClientInvoiceForm_Load(object sender, EventArgs e)
    {
        progressBar1.Visible = false;

        toolStripStatusLabel1.Text = string.Empty;

        //are there any old print files that need to be cleaned up?
        CleanTempFiles();
        Cursor.Current = Cursors.WaitCursor;

        _clientInvoicesService = new ClientInvoicesService(Program.AppEnvironment);

        _clientInvoicesService.InvoiceGenerated += ClientInvoices_InvoiceGenerated;
        _clientInvoicesService.InvoiceRunCompleted += ClientInvoices_InvoiceRunCompleted;

        _clientList = DataCache.Instance.GetClients();
        _clientList.Sort((p, q) => p.Name.CompareTo(q.Name));

        _clientList.Insert(0, new Client
        {
            ClientMnem = null,
            Name = "--All Clients--"
        });

        if (PreviousMonth.Checked)
            _thruDate = DateTime.Today.AddDays(-DateTime.Now.Day);
        else
            _thruDate = DateTime.Today;

        InvoicesDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        InvoicesDGV.MultiSelect = false;
        SelectionProfile.SelectedIndex = 0;

        ClientFilter.DataSource = _clientList;
        ClientFilter.DisplayMember = nameof(Client.Name);
        ClientFilter.ValueMember = nameof(Client.ClientMnem);

        InvoiceHistoryTabControl.Enabled = true;
        GenerateInvoicesTabPage.Enabled = true;
        Cursor.Current = Cursors.Default;

        await RefreshUnbilledGridAsync();
    }

    private void ClientInvoices_InvoiceRunCompleted(object sender, ClientInvoiceGeneratedEventArgs e)
    {

    }

    private void ClientInvoices_InvoiceGenerated(object sender, ClientInvoiceGeneratedEventArgs e)
    {

    }

    private static void CleanTempFiles()
    {
        //string[] dirs = Directory.GetFiles(@"c:\temp\", "invoiceTemp*.pdf");        
        //foreach (string dir in dirs) { File.Delete(dir); }

        string[] tempFiles = Directory.GetFiles(Program.AppEnvironment.TempFilePath);
        foreach(string dir in tempFiles)  { File.Delete(dir); }
    }

    private void SetupInvoicesDGV()
    {
        InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].DefaultCellStyle.Format = "c2";
        InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoicesDGV.Columns[nameof(UnbilledClient.PriorBalance)].DefaultCellStyle.Format = "c2";
        InvoicesDGV.Columns[nameof(UnbilledClient.PriorBalance)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoicesDGV.Columns[nameof(UnbilledClient.ClientMnem)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        InvoicesDGV.Columns[nameof(UnbilledClient.SelectForInvoice)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        InvoicesDGV.Columns[nameof(UnbilledClient.ClientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        InvoicesDGV.Columns[nameof(UnbilledClient.PriorBalance)].ReadOnly = true;
        InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].ReadOnly = true;
        InvoicesDGV.Columns[nameof(UnbilledClient.ClientMnem)].ReadOnly = true;
        InvoicesDGV.Columns[nameof(UnbilledClient.ClientType)].ReadOnly = true;
        InvoicesDGV.Columns[nameof(UnbilledClient.ClientType)].ReadOnly = true;
    }

    private async Task RefreshUnbilledGridAsync()
    {
        InvoiceHistoryTabControl.Enabled = false;
        GenerateInvoicesTabPage.Enabled = false;

        using WaitForm waitForm = new();

        var progress = new Progress<int>(percent =>
        {
            waitForm.UpdateProgress(percent, "Loading accounts ... ");
        });
        waitForm.ProgressBarStyle = ProgressBarStyle.Continuous;
        waitForm.StartPosition = FormStartPosition.CenterScreen;

        waitForm.Show(this);

        _unbilledClients = await _clientInvoicesService.GetUnbilledClientsAsync(_thruDate, progress);

        waitForm.Close();

        InvoicesDGV.DataSource = _unbilledClients;

        SetupInvoicesDGV();

        double sum = _unbilledClients.Sum(x => x.UnbilledAmount);

        TotalUnbilledCharges.Text = sum.ToString("C");
        TotalUnbilledCharges.TextAlign = HorizontalAlignment.Right;

        InvoiceHistoryTabControl.Enabled = true;
        GenerateInvoicesTabPage.Enabled = true;
    }

    private void RefreshUnbilledAccountsGrid(string clientMnem)
    {
        UnbilledAccountsDGV.DataSource = _unbilledClients.Where(x => x.ClientMnem == clientMnem).First().UnbilledAccounts;

        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.FinancialClass)].Visible = false;
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.UnbilledAmount)].DefaultCellStyle.Format = "c2";
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.UnbilledAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.Account)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.ClientMnem)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.FinancialClass)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.TransactionDate)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
    }

    private async void GenerateInvoicesBtn_Click(object sender, EventArgs e)
    {
        Log.Instance.Trace("Entering");
        _cts.Dispose();
        _cts = new CancellationTokenSource();
        List<UnbilledClient> clientsToBill = new();

        Cursor.Current = Cursors.WaitCursor;

        foreach (DataGridViewRow row in InvoicesDGV.Rows)
        {
            if ((bool)row.Cells["SelectForInvoice"].Value == true)
            {
                clientsToBill.Add(_unbilledClients.First(x => x.ClientMnem == row.Cells[nameof(UnbilledClient.ClientMnem)].Value.ToString()));
            }
        }
        Cursor.Current = Cursors.Default;

        GenerateInvoicesBtn.Enabled = false;
        _invoiceWaitForm = new();

        try
        {
            _clientInvoicesService.ReportProgress += ClientInvoicesService_ReportProgress;
            _invoiceWaitForm.CancelRequested += (sender, e) => { _cts.Cancel(); };
            _invoiceWaitForm.Show(this);
            await _clientInvoicesService.CompileAsync(_thruDate, clientsToBill, _cts.Token);
            _invoiceWaitForm.Close();
            if (_cts.IsCancellationRequested)
                MessageBox.Show("Invoice processing cancelled.");
            else
                MessageBox.Show("Invoice processing complete.");
        }
        catch (Exception ex)
        {
            _invoiceWaitForm.Close();
            if (_cts.IsCancellationRequested)
            {
                MessageBox.Show("Invoice processing cancelled.");
            }
            else
                MessageBox.Show("Invoice processing failed with error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Log.Instance.Error(ex);
        }
        GenerateInvoicesBtn.Enabled = true;
    }

    private void ClientInvoicesService_ReportProgress(object sender, ClientInvoiceProgressEventArgs e)
    {
        if (e.ReportingClient)
        {
            _invoiceWaitForm.UpdateInvoiceProgress(HelperExtensions.ComputePercentage(e.ClientsProcessed, e.ClientsTotal), $"Processing invoice for {e.Client} - {e.ClientsProcessed} of {e.ClientsTotal} processed.");
        }

        if (e.ReportingAccount)
        {
            _invoiceWaitForm.UpdateAccountProgress(HelperExtensions.ComputePercentage(e.AccountsProcessed, e.AccountsTotal), $"Processed {e.AccountsProcessed} of {e.AccountsTotal} accounts.");
        }
    }

    private async void ThruDate_CheckedChanged(object sender, EventArgs e)
    {
        if (PreviousMonth.Checked)
            _thruDate = DateTime.Today.AddDays(-DateTime.Now.Day);

        if (CurrentMonth.Checked)
            _thruDate = DateTime.Today;

        await RefreshUnbilledGridAsync();
    }

    private void InvoicesDGV_SelectionChanged(object sender, EventArgs e)
    {
        if (InvoicesDGV.SelectedRows.Count > 0)
        {
            if (InvoicesDGV.SelectedRows[0].Cells[nameof(UnbilledClient.ClientMnem)].Value != null)
                RefreshUnbilledAccountsGrid(InvoicesDGV.SelectedRows[0].Cells[nameof(UnbilledClient.ClientMnem)].Value.ToString());
        }
    }

    private void UnbilledAccountsDGV_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        if (UnbilledAccountsDGV.SelectedRows.Count > 0)
        {
            AccountLaunched?.Invoke(this, UnbilledAccountsDGV.SelectedRows[0].Cells[nameof(UnbilledAccounts.Account)].Value.ToString());
        }
    }

    private void SelectionProfile_SelectedIndexChanged(object sender, EventArgs e)
    {
        string profile = SelectionProfile.SelectedItem.ToString();
        int counter;
        Cursor.Current = Cursors.WaitCursor;
        for (counter = 0; counter < (InvoicesDGV.Rows.Count); counter++)
        {
            switch (profile)
            {
                case "None":
                    InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.SelectForInvoice)].Value = false;
                    break;
                case "Nursing Homes":
                    if (InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.ClientType)].Value.ToString() == "Nursing Homes")
                        InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.SelectForInvoice)].Value = true;
                    else
                        InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.SelectForInvoice)].Value = false;
                    break;
                case "All Except Nursing Homes":
                    if (InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.ClientType)].Value.ToString() == "Nursing Homes")
                        InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.SelectForInvoice)].Value = false;
                    else
                        InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.SelectForInvoice)].Value = true;
                    break;
                default:
                    break;
            }
        }
        Cursor.Current = Cursors.Default;
    }

    #region InvoiceHistory Tab functions
    private async void InvoiceHistoryPage_Enter(object sender, EventArgs e)
    {
        FromDate.Text = DateTime.Today.ToString("MM/dd/yyyy");
        ThroughDate.Text = DateTime.Today.AddDays(1).ToString("MM/dd/yyyy");

        await RefreshInvoiceHistoryGridAsync();
    }

    private void RefreshInvoiceHistoryGrid()
    {
        DateTime.TryParse(FromDate.Text, out DateTime fd);
        DateTime.TryParse(ThroughDate.Text, out DateTime td);

        InvoiceHistoryDGV.DataSource = _clientInvoicesService.GetInvoiceHistory(ClientFilter.SelectedValue?.ToString(), fd, td, invoiceTextBox.Text);

        SetupInvoiceHistoryGrid();

    }

    private void SetupInvoiceHistoryGrid()
    {
        InvoiceHistoryDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.ClientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.BalanceForward)].DefaultCellStyle.Format = "c2";
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.BalanceForward)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.TotalCharges)].DefaultCellStyle.Format = "c2";
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.TotalCharges)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.Discount)].DefaultCellStyle.Format = "c2";
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.Discount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.BalanceDue)].DefaultCellStyle.Format = "c2";
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.BalanceDue)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.Payments)].DefaultCellStyle.Format = "c2";
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.Payments)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.TrueBalanceDue)].DefaultCellStyle.Format = "c2";
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.TrueBalanceDue)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.InvoiceFilename)].Visible = false;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedUser)].Visible = false;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedDate)].Visible = false;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedApp)].Visible = false;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedHost)].Visible = false;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.rowguid)].Visible = false;
        InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.InvoiceData)].Visible = false;
    }

    private async Task RefreshInvoiceHistoryGridAsync()
    {
        _ = DateTime.TryParse(FromDate.Text, out DateTime fd);
        _ = DateTime.TryParse(ThroughDate.Text, out DateTime td);

        string client = ClientFilter.SelectedValue?.ToString();

        InvoiceHistoryDGV.DataSource = await Task.Run(() => _clientInvoicesService.GetInvoiceHistory(client, fd, td, invoiceTextBox.Text));

        SetupInvoiceHistoryGrid();

    }

    private void ClientFilter_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (FromDate.MaskCompleted && ThroughDate.MaskCompleted)
            RefreshInvoiceHistoryGrid();
    }

    private void ViewInvoice_Click(object sender, EventArgs e)
    {
        if (InvoiceHistoryDGV.SelectedRows == null)
            return;

        if (InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.InvoiceData)].Value == null)
        {
            MessageBox.Show("Invoice image not stored in history record.");
            return;
        }
        else
        {
            string invoiceNo = InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.InvoiceNo)].Value.ToString();

            InvoicePrintPdfSharp invoicePrint = new(Program.AppEnvironment);

            string filename = invoicePrint.PrintInvoice(invoiceNo);
            if (!string.IsNullOrWhiteSpace(filename))
            {
                LaunchPDF(filename);
            }
            else
            {
                MessageBox.Show("No invoice to print.");
                return;
            }
        }
    }

    private void CompileInvoicesToPdf(string filename, bool duplex = false)
    {
        List<InvoiceModel> models = new();
        List<string> files = new();

        int records = InvoiceHistoryDGV.SelectedRows.Count;
        progressBar1.Visible = true;
        progressBar1.Minimum = 0;
        progressBar1.Maximum = records;
        progressBar1.Value = 0;
        InvoicePrintPdfSharp invoicePrint = new(Program.AppEnvironment);

        foreach (DataGridViewRow row in InvoiceHistoryDGV.SelectedRows)
        {
            var client = row.Cells[nameof(InvoiceHistory.ClientMnem)].Value.ToString();
            var invoice = row.Cells[nameof(InvoiceHistory.InvoiceNo)].Value.ToString();

            var model = _clientInvoicesService.GenerateStatement(client, DateTime.Today.AddDays(-120));
            models.Add(model);
            string stmtFilename = invoicePrint.CreateStatementPdf(model);
            string invFilename = invoicePrint.PrintInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(stmtFilename))
                files.Add(stmtFilename);
            if (!string.IsNullOrWhiteSpace(invFilename))
                files.Add(invFilename);

            progressBar1.Increment(1);
        }

        //merge all the files into one pdf
        InvoicePrintPdfSharp.MergeFiles(files, filename, duplex);

        return;
    }

    private void printAllToolStripMenuItem_Click(object sender, EventArgs e)
    {
        InvoiceHistoryDGV.SelectAll();
        PrintInvoice_Click(sender, e);
    }

    private void saveAllToPDFToolStripMenuItem_Click(object sender, EventArgs e)
    {
        InvoiceHistoryDGV.SelectAll();
        PrintInvoice_Click(sender, e);
    }

    private void PrintInvoice_Click(object sender, EventArgs e)
    {
        Cursor.Current = Cursors.WaitCursor;

        if (InvoiceHistoryDGV.SelectedRows == null)
        {
            MessageBox.Show("No rows selected.");
            return;
        }

        bool duplexPrinting = false;
        string senderName;
        if (sender is ToolStripMenuItem)
        {
            var menuItem = sender as ToolStripMenuItem;
            senderName = menuItem.Name;
        }
        else
        {
            return;
        }

        if (senderName == printToolStripMenuItem.Name || senderName == printAllToolStripMenuItem.Name)
        {
            //PrintDialog printDialog = new();

            Cursor.Current = Cursors.WaitCursor;

            string outfile = $"{Program.AppEnvironment.TempFilePath}invoiceTemp-{Guid.NewGuid()}.pdf";

            CompileInvoicesToPdf(outfile, duplexPrinting);
            LaunchPDF(outfile);
        }
        else if (senderName == saveToPDFToolStripMenuItem.Name || senderName == saveAllToPDFToolStripMenuItem.Name)
        {
            string path = Program.AppEnvironment.TempFilePath;
            if (!Directory.Exists(path))
            {
                path = "";
            }

            SaveFileDialog saveFileDialog = new()
            {
                Title = "Save File",
                DefaultExt = "pdf",
                Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*",
                FilterIndex = 1,
                InitialDirectory = path,
                FileName = $"InvoicesPrint-{DateTime.Now:MM-dd-yyyy}.pdf"
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                string filename = saveFileDialog.FileName;

                CompileInvoicesToPdf(filename);

                LaunchPDF(filename);
            }
        }

        Cursor.Current = Cursors.Default;
        progressBar1.Visible = false;
    }

    private static void LaunchPDF(string filename)
    {
        using Process fileopener = new();

        fileopener.StartInfo.FileName = "explorer";
        fileopener.StartInfo.Arguments = "\"" + filename + "\"";

        fileopener.Start();
    }

    private static bool PrintPDF(string file, string printer)
    {
        try
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    WindowStyle = ProcessWindowStyle.Hidden,

                    FileName = file,
                    Verb = "print",
                    CreateNoWindow = true,
                    UseShellExecute = true
                }
            };

            process.Start();

            process.WaitForExit();

            Thread.Sleep(8000);

            if (!process.CloseMainWindow())
            {
                process.Kill();
            }

            return true;
        }
        catch
        {
            return false;
        }
    }

    private void InvoiceHistoryDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        ViewInvoice_Click(sender, e);
    }

    private void FromDate_TextChanged(object sender, EventArgs e)
    {
        if (FromDate.Text == "" || FromDate.Text == null)
            return;

        if (!FromDate.MaskFull)
            return;

        _ = DateTime.TryParse(FromDate.Text, out DateTime fd);
        _ = DateTime.TryParse(ThroughDate.Text, out DateTime td);

        if (fd > td)
        {
            return;
        }
        else
        {
            RefreshInvoiceHistoryGrid();
        }
    }

    private void ThroughDate_TextChanged(object sender, EventArgs e)
    {
        if (!ThroughDate.MaskFull)
            return;

        if (ThroughDate.Text == "" || ThroughDate.Text == null)
            return;

        _ = DateTime.TryParse(FromDate.Text, out DateTime fd);
        _ = DateTime.TryParse(ThroughDate.Text, out DateTime td);
        if (fd > td)
        {
            return;
        }
        else
        {
            td = td.AddDays(1);
            RefreshInvoiceHistoryGrid();
        }
    }

    #endregion

    private void undoInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
    {
        MessageBox.Show("Function not implemented.");
    }

    private void generateStatementButton_Click(object sender, EventArgs e)
    {
        InvoicePrintPdfSharp invoicePrint = new(Program.AppEnvironment);

        if (InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.ClientMnem)].Value == null)
        {
            MessageBox.Show("Invoice image not stored in history record.");
            return;
        }
        else
        {
            var statementBeginDate = InputDialogs.SelectStatementBeginDate(DateTime.Today.AddDays(-120));
            string client = InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.ClientMnem)].Value.ToString();
            var model = _clientInvoicesService.GenerateStatement(client, (DateTime)statementBeginDate);
            string filename = invoicePrint.CreateStatementPdf(model);
            LaunchPDF(filename);
        }

    }

    private void UnbilledAccountsDGV_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
        string account = UnbilledAccountsDGV[nameof(UnbilledAccounts.Account), e.RowIndex].Value.ToString();
        AccountLaunched?.Invoke(this, account);
    }

    private async void refreshUnbilledInvoices_Click(object sender, EventArgs e)
    {
        await RefreshUnbilledGridAsync();
    }

    private void PrintInvoice_Click_1(object sender, EventArgs e)
    {
        var pos = Cursor.Position;

        int top = pos.X - printContextMenu.Width;
        int left = pos.Y;

        printContextMenu.Show(Cursor.Position);

    }

    private void invoiceTextBox_TextChanged(object sender, EventArgs e)
    {
        RefreshInvoiceHistoryGrid();
    }


}
