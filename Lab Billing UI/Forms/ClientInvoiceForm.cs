using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.BusinessLogic;
using LabBilling.Core.Models;
using LabBilling.Library;
using System.Diagnostics;
using LabBilling.Logging;
using System.Threading;
using SaveFileDialog = System.Windows.Forms.SaveFileDialog;

namespace LabBilling.Forms
{
    public partial class ClientInvoiceForm : BaseForm
    {
        public ClientInvoiceForm()
        {
            InitializeComponent();

            InvoiceHistoryTabPage.Enter += new System.EventHandler(InvoiceHistoryPage_Enter);
        }

        private DateTime _thruDate;
        private ClientInvoices clientInvoices;
        private InvoiceHistoryRepository historyRepository;
        private ClientRepository clientRepository;
        private List<Client> clientList;
        public event EventHandler<string> AccountLaunched;
        private async void ClientInvoiceForm_Load(object sender, EventArgs e)
        {
            progressBar1.Visible = false;

            toolStripStatusLabel1.Text = string.Empty;

            //are there any old print files that need to be cleaned up?
            CleanTempFiles();

            clientInvoices = new ClientInvoices(Program.AppEnvironment);
            historyRepository = new InvoiceHistoryRepository(Program.AppEnvironment);
            clientRepository = new ClientRepository(Program.AppEnvironment);

            clientInvoices.InvoiceGenerated += ClientInvoices_InvoiceGenerated;
            clientInvoices.InvoiceRunCompleted += ClientInvoices_InvoiceRunCompleted;

            clientList = clientRepository.GetAll().ToList();
            clientList.Sort((p, q) => p.Name.CompareTo(q.Name));

            clientList.Insert(0, new Client
            {
                ClientMnem = null,
                Name = "--All Clients--"
            });

            if (PreviousMonth.Checked)
                _thruDate = DateTime.Today.AddDays(-DateTime.Now.Day);
            else
                _thruDate = DateTime.Today;

            await RefreshUnbilledGridAsync();
            //RefreshUnbilledGrid();

            InvoicesDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InvoicesDGV.MultiSelect = false;
            SelectionProfile.SelectedIndex = 0;

            ClientFilter.DataSource = clientList;
            ClientFilter.DisplayMember = nameof(Client.Name);
            ClientFilter.ValueMember = nameof(Client.ClientMnem);

            InvoiceHistoryTabControl.Enabled = true;
            GenerateInvoicesTabPage.Enabled = true;

        }

        private void ClientInvoices_InvoiceRunCompleted(object sender, ClientInvoiceGeneratedEventArgs e)
        {
            
        }

        private void ClientInvoices_InvoiceGenerated(object sender, ClientInvoiceGeneratedEventArgs e)
        {
            
        }

        private void CleanTempFiles()
        {
            string[] dirs = Directory.GetFiles(@"c:\temp\", "invoiceTemp*.pdf");
            foreach(string dir in dirs)
            {
                File.Delete(dir);
            }
        }

        private void RefreshUnbilledGrid()
        {
            Cursor.Current = Cursors.WaitCursor;

            InvoicesDGV.DataSource = clientRepository.GetUnbilledClients(_thruDate);

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

            double sum = 0;
            foreach (DataGridViewRow row in InvoicesDGV.Rows)
            {
                sum += Convert.ToDouble(row.Cells[nameof(UnbilledClient.UnbilledAmount)].Value);
            }

            TotalUnbilledCharges.Text = sum.ToString("C");
            TotalUnbilledCharges.TextAlign = HorizontalAlignment.Right;

            Cursor.Current = Cursors.Default;
        }

        private async Task RefreshUnbilledGridAsync()
        {
            InvoiceHistoryTabControl.Enabled = false;
            GenerateInvoicesTabPage.Enabled = false;

            toolStripProgressBar1.Style = ProgressBarStyle.Marquee;

            InvoicesDGV.DataSource = await Task.Run(() => clientRepository.GetUnbilledClients(_thruDate));

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

            double sum = 0;
            foreach (DataGridViewRow row in InvoicesDGV.Rows)
            {
                sum += Convert.ToDouble(row.Cells[nameof(UnbilledClient.UnbilledAmount)].Value);
            }

            TotalUnbilledCharges.Text = sum.ToString("C");
            TotalUnbilledCharges.TextAlign = HorizontalAlignment.Right;

            toolStripProgressBar1.Style = ProgressBarStyle.Continuous;


            InvoiceHistoryTabControl.Enabled = true;
            GenerateInvoicesTabPage.Enabled = true;
        }

        private void RefreshUnbilledAccountsGrid(string clientMnem)
        {
            UnbilledAccountsDGV.DataSource = clientRepository.GetUnbilledAccounts(clientMnem, _thruDate);

            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.UnbilledAmount)].DefaultCellStyle.Format = "c2";
            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.UnbilledAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.Account)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.ClientMnem)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.FinancialClass)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            UnbilledAccountsDGV.Columns[nameof(UnbilledAccounts.TransactionDate)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
        }

        private void ClientInvoice_ProgressUpdate(object sender, int e)
        {
            toolStripProgressBar1.Value = e;
        }

        private void GenerateInvoicesBtn_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("Entering");
            List<UnbilledClient> unbilledClients = new List<UnbilledClient>();

            Cursor.Current = Cursors.WaitCursor;

            foreach (DataGridViewRow row in InvoicesDGV.Rows)
            {
                if ((bool)row.Cells["SelectForInvoice"].Value == true)
                {
                    //add client info to list
                    unbilledClients.Add(new UnbilledClient
                    {
                        ClientMnem = row.Cells[nameof(UnbilledClient.ClientMnem)].Value.ToString(),
                        ClientName = row.Cells[nameof(UnbilledClient.ClientName)].Value.ToString(),
                        ClientType = row.Cells[nameof(UnbilledClient.ClientType)].Value.ToString(),
                        UnbilledAmount = Convert.ToDouble(row.Cells[nameof(UnbilledClient.UnbilledAmount)].Value.ToString())
                    });
                }
            }
            Cursor.Current = Cursors.Default;

            toolStripProgressBar1.Value = 0;
            GenerateInvoicesBtn.Enabled = false;

            var progress = new Progress<int>(percent =>
            {
                toolStripProgressBar1.Value = percent;
            });

            try
            {
                using (WaitForm frm = new WaitForm(() => { clientInvoices.Compile(_thruDate, unbilledClients, progress); }))
                {
                    frm.ShowDialog(this);
                }
                //await Task.Run(() => clientInvoices.Compile(_thruDate, unbilledClients, progress));
                MessageBox.Show("Generating Invoices Completed");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Invoice processing failed with error: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Log.Instance.Error(ex);
            }
            toolStripProgressBar1.Value = 100;

            GenerateInvoicesBtn.Enabled = true;
        }

        private void ThruDate_CheckedChanged(object sender, EventArgs e)
        {
            if (PreviousMonth.Checked)
                _thruDate = DateTime.Today.AddDays(-DateTime.Now.Day);

            if (CurrentMonth.Checked)
                _thruDate = DateTime.Today;

            RefreshUnbilledGrid();
        }

        private void InvoicesDGV_SelectionChanged(object sender, EventArgs e)
        {
            if (InvoicesDGV.SelectedRows.Count > 0)
            {
                if(InvoicesDGV.SelectedRows[0].Cells[nameof(UnbilledClient.ClientMnem)].Value != null)
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

            InvoiceHistoryDGV.DataSource = historyRepository.GetWithSort(ClientFilter.SelectedValue?.ToString(), fd, td, invoiceTextBox.Text);

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

        }

        private async Task RefreshInvoiceHistoryGridAsync()
        {
            DateTime.TryParse(FromDate.Text, out DateTime fd);
            DateTime.TryParse(ThroughDate.Text, out DateTime td);

            string client = ClientFilter.SelectedValue?.ToString();

            InvoiceHistoryDGV.DataSource = await Task.Run(() => historyRepository.GetWithSort(client, fd, td, invoiceTextBox.Text));

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
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.InvoiceData)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedUser)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedDate)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedApp)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.UpdatedHost)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.rowguid)].Visible = false;

        }

        private void ClientFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(FromDate.MaskCompleted && ThroughDate.MaskCompleted)
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

                ClientInvoices clientInvoices = new ClientInvoices(Program.AppEnvironment);

                string filename = clientInvoices.PrintInvoice(invoiceNo);

                System.Diagnostics.Process.Start(filename);
            }
        }

        private void CompileInvoicesToPdf(string filename, bool duplex = false)
        {
            List<string> files = new List<string>();

            int records = InvoiceHistoryDGV.SelectedRows.Count;
            progressBar1.Visible = true;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = records;
            progressBar1.Value = 0;

            foreach (DataGridViewRow row in InvoiceHistoryDGV.SelectedRows)
            {
                var client = row.Cells[nameof(InvoiceHistory.ClientMnem)].Value.ToString();
                var invoice = row.Cells[nameof(InvoiceHistory.InvoiceNo)].Value.ToString();

                string invFilename = clientInvoices.GenerateStatement(client, DateTime.Today.AddDays(-120));
                files.Add(invFilename);

                string stmtFilename = clientInvoices.PrintInvoice(invoice);
                if(!string.IsNullOrWhiteSpace(stmtFilename))
                    files.Add(stmtFilename);

                progressBar1.Increment(1);
            }

            //merge all the files into one pdf
            InvoicePrintPdfSharp invoicePrint = new InvoicePrintPdfSharp();

            invoicePrint.MergeFiles(files, filename, duplex);

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
            string senderName = null;
            if(sender is ToolStripMenuItem)
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
                PrintDialog printDialog = new PrintDialog();

                Cursor.Current = Cursors.WaitCursor;

                string outfile = $"c:\\temp\\invoiceTemp-{Guid.NewGuid()}.pdf";

                CompileInvoicesToPdf(outfile, duplexPrinting);

                Process.Start(outfile);

            }
            else if (senderName == saveToPDFToolStripMenuItem.Name || senderName == saveAllToPDFToolStripMenuItem.Name)
            {
                string path = Program.AppEnvironment.ApplicationParameters.InvoiceFileLocation;
                if(!Directory.Exists(path))
                {
                    path = "";
                }

                SaveFileDialog saveFileDialog = new SaveFileDialog();

                saveFileDialog.Title = "Save File";
                saveFileDialog.DefaultExt = "pdf";
                saveFileDialog.Filter = "pdf files (*.pdf)|*.pdf|All files (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.InitialDirectory = path;
                saveFileDialog.FileName = $"InvoicesPrint-{DateTime.Now:MM-dd-yyyy}.pdf";

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filename = saveFileDialog.FileName;

                    CompileInvoicesToPdf(filename);
                    System.Diagnostics.Process.Start(filename);
                }
            }

            Cursor.Current = Cursors.Default;
            progressBar1.Visible = false;
        }

        private bool PrintPDF(string file, string printer)
        {
            try
            {
                var process = new Process();
                process.StartInfo = new ProcessStartInfo();
                process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                process.StartInfo.FileName = file;
                process.StartInfo.Verb = "print";
                process.StartInfo.CreateNoWindow = true;
                process.StartInfo.UseShellExecute = true;

                process.Start();

                process.WaitForExit();

                Thread.Sleep(8000);

                if(!process.CloseMainWindow())
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

            DateTime.TryParse(FromDate.Text, out DateTime fd);
            DateTime.TryParse(ThroughDate.Text, out DateTime td);

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

            DateTime.TryParse(FromDate.Text, out DateTime fd);
            DateTime.TryParse(ThroughDate.Text, out DateTime td);
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
            if (InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.ClientMnem)].Value == null)
            {
                MessageBox.Show("Invoice image not stored in history record.");
                return;
            }
            else
            {
                var statementBeginDate = InputDialogs.SelectStatementBeginDate(DateTime.Today.AddDays(-120));
                string client = InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.ClientMnem)].Value.ToString();
                clientInvoices.GenerateStatement(client, (DateTime)statementBeginDate);
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
            var pos = System.Windows.Forms.Cursor.Position;

            int top = pos.X - printContextMenu.Width;
            int left = pos.Y;

            printContextMenu.Show(Cursor.Position);

        }

        private void invoiceTextBox_TextChanged(object sender, EventArgs e)
        {
            RefreshInvoiceHistoryGrid();
        }


    }
}
