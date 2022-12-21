using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using RFClassLibrary;
using System.Data;
using MetroFramework.Forms;
using System.Security.Principal;
using System.Drawing;
using MCL;

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

        private void SaveBatchButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
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

                XmlAttribute writeOffDate = xmlDoc.CreateAttribute("WriteOffDate");
                writeOffDate.Value = row.Cells["WriteOffdate"].Value?.ToString();
                pmtNode.Attributes.Append(writeOffDate);

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
                chkBatch.BatchNo = Convert.ToInt32(OpenBatch.SelectedValue);
                if (chkBatchRepository.Update(chkBatch))
                {
                    MessageBox.Show("Batch saved");
                    //ToDo: Clear entry screen for next batch
                    Clear();

                }
                else
                {
                    MessageBox.Show("Error saving batch", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // add new batch
                if ((int)chkBatchRepository.Add(chkBatch) > 0)
                {
                    MessageBox.Show("Batch saved", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            Log.Instance.Trace($"Entering");

            #region Setup OpenBatch Combobox
            List<ChkBatch> chkBatches = new List<ChkBatch>();
            chkBatches = chkBatchRepository.GetAll().ToList();

            DataTable chkBatchDataTable = new DataTable(typeof(ChkBatch).Name);
            chkBatchDataTable.Columns.Add("BatchNo");
            chkBatchDataTable.Columns.Add("BatchDate");
            chkBatchDataTable.Columns.Add("User");
            var values = new object[3];
            values[0] = "";
            values[1] = "";
            values[2] = "";
            chkBatchDataTable.Rows.Add(values);
            foreach (ChkBatch batch in chkBatches)
            {
                values[0] = batch.BatchNo;
                values[1] = batch.BatchDate;
                values[2] = batch.User;
                chkBatchDataTable.Rows.Add(values);
            }

            OpenBatch.DataSource = chkBatchDataTable;
            OpenBatch.DisplayMember = "BatchDate";
            OpenBatch.ValueMember = "BatchNo";

            OpenBatch.Refresh();
            #endregion
        }

        private void SubmitPayments_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            int batch = -1;

            if (OpenBatch.SelectedIndex > 0)
            {
                batch = Convert.ToInt32(OpenBatch.SelectedValue.ToString());
            }
            else
            {
                ChkBatch chkBatch = new ChkBatch
                {
                    BatchDate = DateTime.Today,
                    User = Program.LoggedInUser.UserName                    
                };
                batch = (int)chkBatchRepository.Add(chkBatch);
            }

            List<Chk> chks = new List<Chk>();
            chkdb.BeginTransaction();
            foreach (DataGridViewRow row in dgvPayments.Rows)
            {
                if (row.IsNewRow)
                    continue;

                Chk chk = new Chk();

                double temp = 0.00;
                chk.AccountNo = row.Cells["Account"].Value.ToString();
                chk.Batch = batch;
                chk.PaidAmount = Double.TryParse(row.Cells["AmountPaid"].Value?.ToString(), out temp) ? temp : 0.00;
                chk.ChkDate = DateTimeExtension.ValidateDateNullable(row.Cells["CheckDate"].Value?.ToString());
                chk.DateReceived = DateTimeExtension.ValidateDateNullable(row.Cells["DateReceived"].Value?.ToString());
                chk.CheckNo = row.Cells["CheckNo"].Value?.ToString();
                chk.Comment = row.Cells["Comment"].Value?.ToString();
                chk.ContractualAmount = Double.TryParse(row.Cells["Contractual"].Value?.ToString(), out temp) ? temp : 0.00;
                chk.WriteOffCode = row.Cells["WriteOffCode"].Value?.ToString();
                chk.WriteOffAmount = Double.TryParse(row.Cells["WriteOff"].Value?.ToString(), out temp) ? temp : 0.00;
                chk.Source = row.Cells["PaymentSource"].Value?.ToString();
                try
                {
                    chks.Add(chk);
                }
                catch (Exception ex)
                {
                    Log.Instance.Error("Error posting pmt/adj batch.", ex);
                    MessageBox.Show("Batch failed to post.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            MessageBox.Show($"Batch {batch} posted.", "Batch Posted");
            try
            {
                chkdb.AddBatch(chks);
                ChkBatch chkBatch = chkBatchRepository.GetById(batch);
                chkBatchRepository.Delete(chkBatch);
                LoadOpenBatches();
                chkdb.CompleteTransaction();
                //clear entry screen for next batch
                Clear();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"Error posting payment batch", ex);
                MessageBox.Show("Error occurred. Batch not posted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Clear()
        {
            Log.Instance.Trace($"Entering");
            dgvPayments.Rows.Clear();
            AmountTotal.Text = "0.00";
            ContractualTotal.Text = "0.00";
            WriteoffTotal.Text = "0.00";
            EntryMode.Enabled = true;
        }

        private void BatchRemittance_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
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
            dgvPayments.Columns["Balance"].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns["Balance"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvPayments.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
            dgvPayments.Columns["PatientName"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPayments.Columns["PatientName"].Width = 400;
            dgvPayments.Columns["Comment"].Width = 400;

            dgvPayments.Columns["PatientName"].ReadOnly = true;
            dgvPayments.Columns["Balance"].ReadOnly = true;

            EntryMode.SelectedIndex = 0;

            SetCellsReadonly(0, true);

            LoadOpenBatches();
        }

        private void EntryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            // changes which entry columns are active for quicker data entry

            switch (EntryMode.Text)
            {
                case "Standard":   // amt paid, contractual, & write off enabled.
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["WriteOffDate"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = true;
                    break;
                case "Patient":  //amt paid, chk no, chk date enabled
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = false;
                    dgvPayments.Columns["WriteOffCode"].Visible = false;
                    dgvPayments.Columns["WriteOffDate"].Visible = false;
                    dgvPayments.Columns["Contractual"].Visible = false;
                    break;
                case "Commercial":
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["WriteOffDate"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = true;
                    break;
                case "Amount Paid": // amt paid enabled
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = false;
                    dgvPayments.Columns["WriteOffCode"].Visible = false;
                    dgvPayments.Columns["WriteOffDate"].Visible = false;
                    dgvPayments.Columns["Contractual"].Visible = false;
                    break;
                case "Contractual": //contractual enabled
                    dgvPayments.Columns["Contractual"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = false;
                    dgvPayments.Columns["WriteOffCode"].Visible = false;
                    dgvPayments.Columns["WriteOffDate"].Visible = false;
                    dgvPayments.Columns["AmountPaid"].Visible = false;
                    break;
                case "Write Off":
                    dgvPayments.Columns["AmountPaid"].Visible = false;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["WriteOffDate"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = false;
                    break;
                default:
                    dgvPayments.Columns["AmountPaid"].Visible = true;
                    dgvPayments.Columns["WriteOff"].Visible = true;
                    dgvPayments.Columns["WriteOffCode"].Visible = true;
                    dgvPayments.Columns["WriteOffDate"].Visible = true;
                    dgvPayments.Columns["Contractual"].Visible = true;
                    break;
            }

        }

        bool skipDgvPaymentsCellValueChanged = false;

        private void dgvPayments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (dgvPayments.Columns[e.ColumnIndex].Name == "Account")
            {
                if (skipDgvPaymentsCellValueChanged)
                    return;

                // get account information to populate patient name and balance info
                Account account = null;
                string strAccount = dgvPayments["Account", e.RowIndex].Value.ToString();
                if (!string.IsNullOrEmpty(strAccount))
                {
                    strAccount = strAccount.ToUpper();
                    account = accdb.GetByAccount(strAccount, true);
                }
                else
                {
                    return;
                }
                
                if (account == null)
                {
                    skipDgvPaymentsCellValueChanged = true;

                    MessageBox.Show($"Account {strAccount} not found.", "Account not found.");
                    dgvPayments["Account", e.RowIndex].Value = string.Empty;
                    dgvPayments.CurrentCell = dgvPayments["Account", e.RowIndex];

                    skipDgvPaymentsCellValueChanged = false;
                    return;
                }

                if (account != null)
                {
                    skipDgvPaymentsCellValueChanged = true;

                    dgvPayments["Account", e.RowIndex].Value = account.AccountNo;
                    dgvPayments["PatientName", e.RowIndex].Value = account.PatFullName;
                    dgvPayments["Balance", e.RowIndex].Value = account.Balance;
                    dgvPayments.CurrentCell = dgvPayments["CheckNo", e.RowIndex];

                    //clear the readonly flag on the cells
                    SetCellsReadonly(e.RowIndex, false);

                    skipDgvPaymentsCellValueChanged = false;
                }
            }
        }

        private void SetCellsReadonly(int rowIndex, bool setReadonly)
        {
            for (int i = 3; i < dgvPayments.ColumnCount; i++)
            {
                dgvPayments[i, rowIndex].ReadOnly = setReadonly;
            }
        }


        private void dgvPayments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //disable the ability to change the entry mode once a row has been added.
            //This will protect against invalid data being written to the database if columns containing data are hidden.
            if (dgvPayments.Rows.Count > 0)
                EntryMode.Enabled = false;

            //make all the cells readonly until a valid account has been entered

            SetCellsReadonly(e.RowIndex, true);

        }

        private void TotalPayments()
        {
            Log.Instance.Trace($"Entering");
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

            AmountTotal.Text = a.ToString("0.00");
            ContractualTotal.Text = c.ToString("0.00");
            WriteoffTotal.Text = w.ToString("0.00");

        }

        private void dgvPayments_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            TotalPayments();
        }

        private void DeleteBatch_Click(object sender, EventArgs e)
        {
            if (OpenBatch.SelectedIndex > 0)
            {
                if (MessageBox.Show($"Batch {OpenBatch.SelectedValue} will be permanently deleted. This cannot be undone. Delete?", "Confirm Batch Delete",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        ChkBatch chkBatch = chkBatchRepository.GetById(Convert.ToInt32(OpenBatch.SelectedValue));
                        if (chkBatch != null)
                        {
                            chkBatchRepository.Delete(chkBatch);
                            Clear();
                        }
                        else
                        {
                            MessageBox.Show($"Batch {OpenBatch.SelectedValue} not found.", "Batch Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Instance.Error($"Error deleting payment batch {OpenBatch.SelectedValue}", ex);
                        MessageBox.Show("Error occurred. Batch not deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    LoadOpenBatches();
                }
            }
        }

        private void OpenBatch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (Int32.TryParse(OpenBatch.SelectedValue.ToString(), out Int32 number) && OpenBatch.SelectedValue != null)
            {
                //opens a previously open batch for updating and completion
                //read batch data from database
                ChkBatch chkBatch = new ChkBatch();
                chkBatch = chkBatchRepository.GetById(number);

                Clear();

                //load data into data grid view
                XmlDocument xmlDoc = new XmlDocument();
                if (chkBatch.BatchData != null)
                {
                    xmlDoc.LoadXml(chkBatch.BatchData);

                    XmlNodeList nodes = xmlDoc.DocumentElement.SelectNodes("/Batch/Payment");
                    foreach (XmlNode node in nodes)
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
                        dgvPayments["WriteOffDate", index].Value = node.Attributes["WriteOffDate"].Value;
                        dgvPayments["Comment", index].Value = node.Attributes["Comment"].Value;
                        dgvPayments["CheckDate", index].Value = node.Attributes["CheckDate"].Value;
                        dgvPayments["DateReceived", index].Value = node.Attributes["DateReceived"].Value;
                    }
                }
                var idx = dgvPayments.Rows.Count;
                dgvPayments.CurrentCell = dgvPayments.Rows[idx - 1].Cells["Account"];
            }

        }

        private void dgvPayments_CellEnter(object sender, DataGridViewCellEventArgs e)
        {

            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex].Name == "PatientName")
            {
                if (senderGrid.CurrentRow.Cells[e.ColumnIndex].Value == null)
                {
                    return;
                }
            }

            //if (senderGrid.CurrentRow.Cells[e.ColumnIndex].ReadOnly)
            //{
            //    SendKeys.Send("{tab}");
            //}

            if (senderGrid.Columns[e.ColumnIndex].Name == "DateReceived")
            {
                senderGrid[e.ColumnIndex, e.RowIndex].Value = DateTime.Now.ToShortDateString();
            }

            if (senderGrid.Columns[e.ColumnIndex].Name == "PaymentSource")
            {
                //copy value from previous column
                if (e.RowIndex > 0)
                    senderGrid[e.ColumnIndex, e.RowIndex].Value = senderGrid[e.ColumnIndex, e.RowIndex - 1].Value;
            }

        }

        private void dgvPayments_CellLeave(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dgvPayments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dgvPayments.Columns[e.ColumnIndex].Name == "Account")
            //{
            //    // get account information to populate patient name and balance info
            //    Account account = null;
            //    string strAccount = dgvPayments["Account", e.RowIndex].Value?.ToString();
            //    if (!string.IsNullOrEmpty(strAccount))
            //    {
            //        strAccount = strAccount.ToUpper();
            //        account = accdb.GetByAccount(strAccount, true);
            //    }

            //        if (account == null)
            //    {
            //        MessageBox.Show($"Account {strAccount} not found.", "Account not found.");
            //        dgvPayments["Account", e.RowIndex].Value = string.Empty;
            //        dgvPayments.CurrentCell = dgvPayments.Rows[e.RowIndex].Cells["Account"];
            //        return;
            //    }

            //    if (account != null)
            //    {
            //        dgvPayments["Account", e.RowIndex].Value = strAccount;
            //        dgvPayments["PatientName", e.RowIndex].Value = account.PatFullName;
            //        dgvPayments["Balance", e.RowIndex].Value = account.Balance;
            //        dgvPayments.CurrentCell = dgvPayments["CheckNo", e.RowIndex];
            //        }
            //}

        }

        private void dgvPayments_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvPayments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPayments.Columns[e.ColumnIndex].Name == "Account")
            {
                Account account = null;
                PersonSearchForm frm = new PersonSearchForm();
                frm.ShowDialog();
                if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
                {
                    string strAccount = frm.SelectedAccount.ToUpper();
                    account = accdb.GetByAccount(strAccount, true);
                }
                else
                {
                    Log.Instance.Error($"Person search returned an empty selected account.");
                    MessageBox.Show("A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
                }
                if (account != null)
                {
                    dgvPayments["Account", e.RowIndex].Value = account.AccountNo;
                    dgvPayments["PatientName", e.RowIndex].Value = account.PatFullName;
                    dgvPayments["Balance", e.RowIndex].Value = account.Balance;
                    dgvPayments.CurrentCell = dgvPayments["CheckNo", e.RowIndex];
                }
            }
        }

        private void dgvPayments_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvPayments["PatientName", e.RowIndex].Value == null ||
                dgvPayments["PatientName", e.RowIndex].Value.ToString() == "")
            {
                SetCellsReadonly(e.RowIndex, true);
            }
        }

        private void dgvPayments_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            if (e.ColumnIndex < 0 || e.RowIndex < 0)
                return; 

            if (dgvPayments[e.ColumnIndex, e.RowIndex].ReadOnly == true)
            {
                dgvPayments[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.Gray;
            }
            else
            {
                dgvPayments[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
            }

            //if (dgvPayments.Columns[e.ColumnIndex].Name == "Account")
            //{
            //    dgvPayments[e.ColumnIndex, e.RowIndex].Value;
            //}

        }
    }
}
