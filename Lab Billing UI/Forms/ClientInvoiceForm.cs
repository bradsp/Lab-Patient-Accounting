using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;
using LabBilling.Core.DataAccess;
using LabBilling.Core;
using LabBilling.Core.Models;
using LabBilling.Library;

namespace LabBilling.Forms
{
    public partial class ClientInvoiceForm : Form
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

        private async void ClientInvoiceForm_Load(object sender, EventArgs e)
        {
            clientInvoices = new ClientInvoices(Helper.ConnVal);
            historyRepository = new InvoiceHistoryRepository(Helper.ConnVal);
            clientRepository = new ClientRepository(Helper.ConnVal);

            clientList = clientRepository.GetAll().ToList();

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

            InvoicesDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InvoicesDGV.MultiSelect = false;
            SelectionProfile.SelectedIndex = 0;

            ClientFilter.DataSource = clientList;
            ClientFilter.DisplayMember = nameof(Client.Name);
            ClientFilter.ValueMember = nameof(Client.ClientMnem);

        }

        private void RefreshUnbilledGrid()
        {
            Cursor.Current = Cursors.WaitCursor;

            InvoicesDGV.DataSource = clientRepository.GetUnbilledClients(_thruDate);

            InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].DefaultCellStyle.Format = "c2";
            InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoicesDGV.Columns[nameof(UnbilledClient.ClientMnem)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InvoicesDGV.Columns[nameof(UnbilledClient.SelectForInvoice)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InvoicesDGV.Columns[nameof(UnbilledClient.ClientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            double sum = 0;
            foreach(DataGridViewRow row in InvoicesDGV.Rows)
            {
                sum += Convert.ToDouble(row.Cells[nameof(UnbilledClient.UnbilledAmount)].Value);
            }

            TotalUnbilledCharges.Text = sum.ToString("C");
            TotalUnbilledCharges.TextAlign = HorizontalAlignment.Right;

            Cursor.Current = Cursors.Default;
        }

        private async Task RefreshUnbilledGridAsync()
        {
            progressBar2.Visible = true;
            progressBar2.Style = ProgressBarStyle.Marquee;

            InvoicesDGV.DataSource = await Task.Run(() => clientRepository.GetUnbilledClients(_thruDate));

            InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].DefaultCellStyle.Format = "c2";
            InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoicesDGV.Columns[nameof(UnbilledClient.ClientMnem)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InvoicesDGV.Columns[nameof(UnbilledClient.UnbilledAmount)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InvoicesDGV.Columns[nameof(UnbilledClient.SelectForInvoice)].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            InvoicesDGV.Columns[nameof(UnbilledClient.ClientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            double sum = 0;
            foreach (DataGridViewRow row in InvoicesDGV.Rows)
            {
                sum += Convert.ToDouble(row.Cells[nameof(UnbilledClient.UnbilledAmount)].Value);
            }

            TotalUnbilledCharges.Text = sum.ToString("C");
            TotalUnbilledCharges.TextAlign = HorizontalAlignment.Right;

            progressBar2.Style = ProgressBarStyle.Continuous;
            progressBar2.Visible = false;

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

        private async void GenerateInvoicesBtn_Click(object sender, EventArgs e)
        {
            List<UnbilledClient> unbilledClients = new List<UnbilledClient>();

            Cursor.Current = Cursors.WaitCursor;
            progressBar2.MarqueeAnimationSpeed = 30;
            progressBar2.Visible = true;
            progressBar2.Style = ProgressBarStyle.Marquee;
            progressBar1.Visible = true;

            foreach(DataGridViewRow row in InvoicesDGV.Rows)
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

            progressBar1.Value = 0;
            var progress = new Progress<int>(percent =>
            {
                progressBar1.Value = percent;
            });
            try
            {
                await Task.Run(() => clientInvoices.Compile(_thruDate, unbilledClients, progress));
            }
            catch (Exception)
            {

            }
            progressBar1.Value = 100;
            progressBar2.Style = ProgressBarStyle.Continuous;
            progressBar2.Visible = false;

        }

        private void ThruDate_CheckedChanged(object sender, EventArgs e)
        {
            if(PreviousMonth.Checked)
                _thruDate = DateTime.Today.AddDays(-DateTime.Now.Day);

            if(CurrentMonth.Checked)
                _thruDate = DateTime.Today;

            RefreshUnbilledGrid();
        }

        private void InvoicesDGV_SelectionChanged(object sender, EventArgs e)
        {
            if(InvoicesDGV.SelectedRows.Count > 0)
            {
                RefreshUnbilledAccountsGrid(InvoicesDGV.SelectedRows[0].Cells["ClientMnem"].Value.ToString());
            }
        }

        private void UnbilledAccountsDGV_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(UnbilledAccountsDGV.SelectedRows.Count > 0)
            {
                AccountForm frm = new AccountForm(UnbilledAccountsDGV.SelectedRows[0].Cells["account"].Value.ToString())
                {
                    MdiParent = this.ParentForm
                };
                frm.Show();
            }
        }

        private void SelectionProfile_SelectedIndexChanged(object sender, EventArgs e)
        {
            string profile = SelectionProfile.SelectedItem.ToString();
            int counter;
            Cursor.Current = Cursors.WaitCursor;
            for(counter = 1; counter < (InvoicesDGV.Rows.Count); counter++)
            {
                switch (profile)
                {
                    case "None":
                        InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.SelectForInvoice)].Value = false;
                        break;
                    case "Nursing Homes":
                        if(InvoicesDGV.Rows[counter].Cells[nameof(UnbilledClient.ClientType)].Value.ToString() == "Nursing Homes")
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
            FromDate.Text = DateTime.Today.AddDays(-30).ToString("MM/dd/yyyy");
            ThroughDate.Text = DateTime.Today.ToString("MM/dd/yyyy");

            DateTime.TryParse(FromDate.Text, out DateTime fd);
            DateTime.TryParse(ThroughDate.Text, out DateTime td);
            await RefreshInvoiceHistoryGridAsync(null, fd, td);
        }

        private void RefreshInvoiceHistoryGrid(string clientMnem, DateTime? fromDate = null, DateTime? throughDate = null)
        {
            InvoiceHistoryDGV.DataSource = historyRepository.GetWithSort(clientMnem, fromDate, throughDate);

            InvoiceHistoryDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.ClientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.bal_forward)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.bal_forward)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.total_chrg)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.total_chrg)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.discount)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.discount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.balance_due)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.balance_due)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.payments)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.payments)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.true_balance_due)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.true_balance_due)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.cbill_filestream)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_user)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_date)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_prg)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_host)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.rowguid)].Visible = false;

        }

        private async Task RefreshInvoiceHistoryGridAsync(string clientMnem, DateTime? fromDate = null, DateTime? throughDate = null)
        {

            InvoiceHistoryDGV.DataSource = await Task.Run( () => historyRepository.GetWithSort(clientMnem, fromDate, throughDate));

            InvoiceHistoryDGV.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.ClientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.bal_forward)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.bal_forward)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.total_chrg)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.total_chrg)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.discount)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.discount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.balance_due)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.balance_due)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.payments)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.payments)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.true_balance_due)].DefaultCellStyle.Format = "c2";
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.true_balance_due)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.cbill_filestream)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_user)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_date)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_prg)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.mod_host)].Visible = false;
            InvoiceHistoryDGV.Columns[nameof(InvoiceHistory.rowguid)].Visible = false;

        }

        private void ClientFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InvoiceHistoryTabPage.Focused)
            {
                DateTime.TryParse(FromDate.Text, out DateTime fd);
                DateTime.TryParse(ThroughDate.Text, out DateTime td);
                RefreshInvoiceHistoryGrid(ClientFilter.SelectedValue?.ToString(), fd, td);
            }
        }

        private void ViewInvoice_Click(object sender, EventArgs e)
        {
            if (InvoiceHistoryDGV.SelectedRows == null)
                return;

            if (InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.cbill_html)].Value == null)
            {
                MessageBox.Show("Invoice image not stored in history record.");
                return;
            }
            else
            {


                string xml = InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.cbill_html)].Value.ToString();

                XmlSerializer serializer = new XmlSerializer(typeof(InvoiceModel));
                StringReader rdr = new StringReader(xml);

                InvoiceModel model = (InvoiceModel)serializer.Deserialize(rdr);

                string filename = $"c:\\temp\\{model.InvoiceNo}.pdf";
                InvoicePrint.CreatePDF(model, filename);
                System.Diagnostics.Process.Start(filename);
            }
        }

        private void PrintInvoice_Click(object sender, EventArgs e)
        {

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
            
            if(fd > td)
            {
                return;
            }
            else
            {
                RefreshInvoiceHistoryGrid(ClientFilter.SelectedValue?.ToString(), fd, td);
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
                RefreshInvoiceHistoryGrid(ClientFilter.SelectedValue?.ToString(), fd, td);
            }
        }

        #endregion

        private void undoInvoiceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Function not implemented.");
        }

        private void generateStatementButton_Click(object sender, EventArgs e)
        {
            if (InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.cl_mnem)].Value == null)
            {
                MessageBox.Show("Invoice image not stored in history record.");
                return;
            }
            else
            {
                var statementBeginDate = InputDialogs.SelectStatementBeginDate(DateTime.Today.AddDays(-120));
                string client = InvoiceHistoryDGV.SelectedRows[0].Cells[nameof(InvoiceHistory.cl_mnem)].Value.ToString();
                clientInvoices.GenerateStatement(client, (DateTime)statementBeginDate);
            }

        }
    }
}
