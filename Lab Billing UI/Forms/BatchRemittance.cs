using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace LabBilling.Forms
{
    public partial class BatchRemittance : Form
    {
        public BatchRemittance()
        {
            InitializeComponent();
        }

        private readonly ChkBatchRepository chkBatchRepository = new ChkBatchRepository(Helper.ConnVal);
        private readonly ChkRepository chkdb = new ChkRepository(Helper.ConnVal);
        private readonly AccountRepository accdb = new AccountRepository(Helper.ConnVal);

        private void OpenBatch_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace("$Entering");
            if (OpenBatch.SelectedIndex <= 0)
            {
                Clear();
                return;
            }
            //opens a previously open batch for updating and completion
            //read batch data from database
            ChkBatch chkBatch = new ChkBatch();
            chkBatch = chkBatchRepository.GetById(Convert.ToInt32(OpenBatch.SelectedItem.Col1));

            Clear();

            //load data into data grid view
            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.LoadXml(chkBatch.BatchData);

            XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("/Batch/Payment");
            foreach(XmlNode node in nodes)
            {
                //we have to get the index of a last row id:
                var index = dgvPayments.Rows.Add();

                //and count +1 to get a new row id:
                dgvPayments["Account", index].Value = node.Attributes["Account"].Value;
                dgvPayments["CheckNo", index].Value = node.Attributes["CheckNo"].Value;
                dgvPayments["PaymentSource", index].Value = node.Attributes["PaymentSource"].Value;
                dgvPayments["Contractual", index].Value = node.Attributes["Contractual"].Value ?? "0";
                dgvPayments["AmountPaid", index].Value = node.Attributes["AmountPaid"].Value ?? "0";
                dgvPayments["WriteOff", index].Value = node.Attributes["WriteOff"].Value ?? "0";
                dgvPayments["WriteOffCode", index].Value = node.Attributes["WriteOffCode"].Value;
                dgvPayments["Comment", index].Value = node.Attributes["Comment"].Value;
                dgvPayments["CheckDate", index].Value = node.Attributes["CheckDate"].Value;
                dgvPayments["DateReceived", index].Value = node.Attributes["DateReceived"].Value;
            }
            var idx = dgvPayments.Rows.Count;
            dgvPayments.CurrentCell = dgvPayments.Rows[idx-1].Cells["Account"];

        }

        private void SaveBatch_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("$Entering");
            //saves an open batch for later use


            //batch record
            XmlDocument xmlDoc = new XmlDocument();
            XmlNode rootNode = xmlDoc.CreateElement("Batch");
            XmlAttribute batchDate = xmlDoc.CreateAttribute("BatchDate");
            batchDate.Value = DateTime.Today.ToShortDateString();
            XmlAttribute batchUser = xmlDoc.CreateAttribute("User");
            batchUser.Value = Program.LoggedInUser.UserName;
            rootNode.Attributes.Append(batchDate);
            rootNode.Attributes.Append(batchUser);
            xmlDoc.AppendChild(rootNode);

            foreach (DataGridViewRow row in dgvPayments.Rows)
            {
                if (row.IsNewRow)
                    continue;

                //payment record
                XmlNode pmtNode = xmlDoc.CreateElement("Payment");
                XmlAttribute account = xmlDoc.CreateAttribute("Account");
                account.Value = row.Cells["Account"].Value?.ToString();
                pmtNode.Attributes.Append(account);

                XmlAttribute checkNo = xmlDoc.CreateAttribute("CheckNo");
                checkNo.Value = row.Cells["CheckNo"].Value?.ToString();
                pmtNode.Attributes.Append(checkNo);

                XmlAttribute checkDate = xmlDoc.CreateAttribute("CheckDate");
                checkDate.Value = row.Cells["CheckDate"].Value?.ToString();
                pmtNode.Attributes.Append(checkDate);

                XmlAttribute dateReceived = xmlDoc.CreateAttribute("DateReceived");
                dateReceived.Value = row.Cells["dateReceived"].Value?.ToString();
                pmtNode.Attributes.Append(dateReceived);

                XmlAttribute source = xmlDoc.CreateAttribute("PaymentSource");
                source.Value = row.Cells["PaymentSource"].Value?.ToString();
                pmtNode.Attributes.Append(source);

                XmlAttribute amountPaid = xmlDoc.CreateAttribute("AmountPaid");
                amountPaid.Value = row.Cells["AmountPaid"].Value?.ToString();
                pmtNode.Attributes.Append(amountPaid);

                XmlAttribute contractual = xmlDoc.CreateAttribute("Contractual");
                contractual.Value = row.Cells["Contractual"].Value?.ToString();
                pmtNode.Attributes.Append(contractual);

                XmlAttribute writeOff = xmlDoc.CreateAttribute("WriteOff");
                writeOff.Value = row.Cells["WriteOff"].Value?.ToString();
                pmtNode.Attributes.Append(writeOff);

                XmlAttribute writeOffCode = xmlDoc.CreateAttribute("WriteOffCode");
                writeOffCode.Value = row.Cells["WriteOffCode"].Value?.ToString();
                pmtNode.Attributes.Append(writeOffCode);

                XmlAttribute comment = xmlDoc.CreateAttribute("Comment");
                comment.Value = row.Cells["Comment"].Value?.ToString();
                pmtNode.Attributes.Append(comment);

                rootNode.AppendChild(pmtNode);
            }

            //save batch
            ChkBatch chkBatch = new ChkBatch
            {
                BatchDate = DateTime.Today,
                User = Program.LoggedInUser.UserName,
                BatchData = xmlDoc.OuterXml
            };

            if (OpenBatch.SelectedIndex > 0)
            {
                //updated existing
                chkBatch.BatchNo = Convert.ToInt32(OpenBatch.SelectedItem.Col1);
                if (chkBatchRepository.Update(chkBatch))
                {
                    MessageBox.Show("Batch saved");
                    //ToDo: Clear entry screen for next batch
                    Clear();

                }
                else
                {
                    MessageBox.Show("Error saving batch");
                }
            }
            else
            {
                // add new batch
                if ((int)chkBatchRepository.Add(chkBatch) > 0)
                {
                    MessageBox.Show("Batch saved");
                    //ToDo: Clear entry screen for next batch
                    Clear();
                }
                else
                {
                    MessageBox.Show("Error saving batch");
                }
            }

            //reload OpenBatch list
            LoadOpenBatches();
        }

        private void LoadOpenBatches()
        {
            Log.Instance.Trace("$Entering");
            #region Setup OpenBatch Combobox
            List<ChkBatch> chkBatches = new List<ChkBatch>();
            chkBatches = chkBatchRepository.GetAll().ToList();

            MTGCComboBoxItem[] chkBatchItems = new MTGCComboBoxItem[chkBatches.Count + 1];
            int i = 0;
            chkBatchItems[i++] = new MTGCComboBoxItem("<new>", "", "");
            foreach (ChkBatch batch in chkBatches)
            {
                chkBatchItems[i] = new MTGCComboBoxItem(batch.BatchNo.ToString(), batch.BatchDate.ToShortDateString(), batch.User);
                i++;
            }

            OpenBatch.ColumnNum = 3;
            OpenBatch.GridLineHorizontal = true;
            OpenBatch.GridLineVertical = true;
            OpenBatch.ColumnWidth = "50;75;75";
            //cbInsCode.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown;
            OpenBatch.SelectedIndex = -1;
            OpenBatch.Items.Clear();
            OpenBatch.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            OpenBatch.Items.AddRange(chkBatchItems);
            #endregion
            OpenBatch.SelectedIndex = 0;

        }

        private void SubmitPayments_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace("$Entering");

            List<Chk> chks = new List<Chk>();

            foreach(DataGridViewRow row in dgvPayments.Rows)
            {
                if (row.IsNewRow)
                    continue;

                Chk chk = new Chk();

                double temp = 0.00;
                chk.account = row.Cells["Account"].Value.ToString();
                if (OpenBatch.SelectedIndex > 0)
                    chk.batch = Convert.ToInt16(OpenBatch.SelectedValue.ToString());
                else
                    chk.batch = -1;
                chk.amt_paid = Double.TryParse(row.Cells["AmountPaid"].Value?.ToString(), out temp) ? temp : 0.00;
                chk.chk_date = Convert.ToDateTime(row.Cells["CheckDate"].Value.ToString());
                chk.date_rec = Convert.ToDateTime(row.Cells["DateReceived"].Value.ToString());
                chk.chk_no = row.Cells["CheckNo"].Value.ToString();
                chk.comment = row.Cells["Comment"].Value.ToString();
                chk.contractual = Double.TryParse(row.Cells["Contractual"].Value?.ToString(), out temp) ? temp : 0.00;
                chk.write_off_code = row.Cells["WriteOffCode"].Value?.ToString();
                chk.write_off = Double.TryParse(row.Cells["WriteOff"].Value?.ToString(), out temp) ? temp : 0.00;
                chk.source = row.Cells["PaymentSource"].Value.ToString();

                chks.Add(chk);
            }

            if(!chkdb.AddBatch(chks))
            {
                MessageBox.Show("Batch failed to post.");
            }
            else
            {
                //if this was a saved batch, delete the batch record
                if (OpenBatch.SelectedIndex > 0)
                {
                    ChkBatch chkBatch = chkBatchRepository.GetById(Convert.ToInt32(((MTGCComboBoxItem)OpenBatch.SelectedItem).Col1));
                    chkBatchRepository.Delete(chkBatch);
                    LoadOpenBatches();
                }
                //clear entry screen for next batch
                Clear();
            }
        }

        private void Clear()
        {
            Log.Instance.Trace("$Entering");
            dgvPayments.Rows.Clear();
            AmountTotal.Text = "0.00";
            ContractualTotal.Text = "0.00";
            WriteoffTotal.Text = "0.00";
            //OpenBatch.SelectedIndex = -1;
            EntryMode.Enabled = true;
        }

        private void BatchRemittance_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace("$Entering");
            dgvPayments.AutoResizeColumns();

            // reference the combobox column
            DataGridViewComboBoxColumn cboWriteoffColumn = (DataGridViewComboBoxColumn)dgvPayments.Columns["WriteOffCode"];
            cboWriteoffColumn.DataSource = Dictionaries.WriteOffCodes.ToList();
            cboWriteoffColumn.DisplayMember = "Value";  // the Name property in Choice class
            cboWriteoffColumn.ValueMember = "Key";  // ditto for the Value property
            cboWriteoffColumn.DropDownWidth = 500;


            dgvPayments.Columns["AmountPaid"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["AmountPaid"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns["Contractual"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["Contractual"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns["WriteOff"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["WriteOff"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvPayments.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvPayments.Columns["PatientName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPayments.Columns["Comment"].Width = 500;

            dgvPayments.Columns["PatientName"].ReadOnly = true;
            dgvPayments.Columns["Balance"].ReadOnly = true;

            EntryMode.SelectedIndex = 0;

            LoadOpenBatches();
        }

        private void EntryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace("$Entering");
            // changes which entry columns are active for quicker data entry

            switch (EntryMode.Text)
            {
                case "Standard":   // amt paid, contractual, & write off enabled.
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = true;
                    break;
                case "Patient":  //amt paid, chk no, chk date enabled
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = false;
                    dgvPayments.Columns["WriteOffCode"].Visible = false;
                    dgvPayments.Columns["Contractual"].Visible = false;
                    break;
                case "Commercial":
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = true;
                    break;
                case "Amount Paid": // amt paid enabled
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = false;
                    dgvPayments.Columns["WriteOffCode"].Visible = false;
                    dgvPayments.Columns["Contractual"].Visible = false;
                    break;
                case "Contractual": //contractual enabled
                    dgvPayments.Columns["Contractual"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = false;
                    dgvPayments.Columns["WriteOffCode"].Visible = false;
                    dgvPayments.Columns["AmountPaid"].Visible = false;
                    break;
                case "Write Off":
                    dgvPayments.Columns["AmountPaid"].Visible = false;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = false;
                    break;
                default:
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = true;
                    break;
            }

        }

        private void dgvPayments_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace("$Entering");
            var senderGrid = (DataGridView)sender;

            if (senderGrid.Columns[e.ColumnIndex] is DataGridViewButtonColumn &&
                e.RowIndex >= 0)
            {
                //TODO - Button Clicked - Launch account search box - return selected account#
                Log.Instance.Debug($"Entering");
                PersonSearchForm frm = new PersonSearchForm();
                frm.ShowDialog();
                if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
                {
                    dgvPayments["Account",e.RowIndex].Value = frm.SelectedAccount;
                    
                }
                else
                {
                    Log.Instance.Error($"Person search returned an empty selected account.");
                    MessageBox.Show("A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
                }
            }
        }

        private void dgvPayments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace("$Entering");
            if (dgvPayments.Columns[e.ColumnIndex].Name == "Account")
            {
                // get account information to populate patient name and balance info
                AccountSummary accountSummary = new AccountSummary();

                accountSummary = accdb.GetAccountSummary(dgvPayments["Account", e.RowIndex].Value.ToString());

                dgvPayments["PatientName", e.RowIndex].Value = accountSummary.pat_name;
                dgvPayments["Balance", e.RowIndex].Value = accountSummary.Balance;
                dgvPayments.CurrentCell = dgvPayments.Rows[e.RowIndex].Cells["CheckNo"];
            }

        }

        private void dgvPayments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Log.Instance.Trace("$Entering");
            //disable the ability to change the entry mode once a row has been added.
            //This will protect against invalid data being written to the database if columns containing data are hidden.
            if (dgvPayments.Rows.Count > 0)
                EntryMode.Enabled = false;

        }

        private void TotalPayments()
        {
            Log.Instance.Trace("$Entering");
            double a = 0, c = 0, w = 0;

            for (int i = 0; i < dgvPayments.Rows.Count; i++)
            {
                if (!dgvPayments.Rows[i].IsNewRow)
                {
                    double temp = 0;
                    a += Double.TryParse(dgvPayments["AmountPaid", i].Value?.ToString(), out temp) ? temp : 0.00;
                    c += Double.TryParse(dgvPayments["Contractual", i].Value?.ToString(), out temp) ? temp : 0.00;
                    w += Double.TryParse(dgvPayments["WriteOff", i].Value?.ToString(), out temp) ? temp : 0.00;
                }
            }

            AmountTotal.Text = a.ToString();
            ContractualTotal.Text = c.ToString();
            WriteoffTotal.Text = w.ToString();

        }

        private void dgvPayments_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace("$Entering");
            TotalPayments();
        }
    }
}
