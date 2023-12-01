using LabBilling.Core.DataAccess;
using LabBilling.Logging;
using LabBilling.Core.Models;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using RFClassLibrary;
using System.Data;
using System.Drawing;


namespace LabBilling.Forms
{
    public partial class BatchRemittance : Form
    {
        public BatchRemittance()
        {
            InitializeComponent();
        }

        private readonly ChkBatchRepository chkBatchRepository = new ChkBatchRepository(Program.AppEnvironment);
        private readonly ChkBatchDetailRepository chkBatchDetailRepository = new ChkBatchDetailRepository(Program.AppEnvironment);
        private readonly ChkRepository chkRepository = new ChkRepository(Program.AppEnvironment);
        private readonly AccountRepository accountRepository = new AccountRepository(Program.AppEnvironment);

        private ChkBatch chkBatch;
        private DataTable chkDetailsDataTable;
        private BindingSource chkDetailsBindingSource;

        private bool isGridLoaded = false;

        private void BatchRemittance_Load(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            List<ChkBatchDetail> chkBatchDetails = new List<ChkBatchDetail>();
            SaveBatchButton.Enabled = false;
            SubmitPaymentsButton.Enabled = false;
            LoadOpenBatches();
            dgvPayments.Enabled = false;
        }

        private int SaveBatch(int batchNo = 0)
        {
            Log.Instance.Trace("Entering");
            //save batch
            chkBatch.BatchDate = DateTime.Today;
            chkBatch.User = Program.LoggedInUser.UserName;

            if(batchNo > 0)
            {
                chkBatch.BatchNo = batchNo;
                if (chkBatchRepository.Update(chkBatch))
                    return batchNo;
                else
                    return -1;
            }
            else
            {
                int batch = (int)chkBatchRepository.Add(chkBatch);
                if (batch > 0)
                {
                    chkBatch.BatchNo = batch;
                    chkBatch = chkBatchRepository.GetById(batch);
                    LoadDetailGrid(chkBatch.ChkBatchDetails);

                    return batch;
                }
                else
                {
                    return -1;
                }
            }
        }

        private void SaveBatchButton_Click(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //saves an open batch for later use
            int retBatch = -1;

            if (OpenBatch.SelectedIndex > 0)
            {
                retBatch = SaveBatch(Convert.ToInt32(OpenBatch.SelectedValue));
            }
            else
            {
                // add new batch
                retBatch = SaveBatch();
            }

            if(retBatch > 0)
            {
                MessageBox.Show("Batch saved.");
                Clear();
                //reload OpenBatch list
                LoadOpenBatches();                
            }
            else
            {
                MessageBox.Show("Error saving batch.");
            }
        }

        private void LoadOpenBatches()
        {
            Log.Instance.Trace($"Entering");

            #region Setup OpenBatch Combobox
            List<ChkBatch> chkBatches = chkBatchRepository.GetOpenBatches();

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
            try
            {
                if (OpenBatch.SelectedIndex > 0)
                {
                    batch = Convert.ToInt32(OpenBatch.SelectedValue.ToString());
                    SaveBatch(batch);
                }
                else
                {
                    batch = SaveBatch();
                }

                if (batch <= 0)
                {
                    Log.Instance.Error("Error saving batch.");
                    MessageBox.Show("Error saving batch.");
                    return;
                }

                List<Chk> chks = new List<Chk>();

                var chkBatch = chkBatchRepository.GetById(batch);

                chkRepository.BeginTransaction();

                foreach (var detail in chkBatch.ChkBatchDetails)
                {
                    Chk chk = new Chk();

                    chk.AccountNo = detail.AccountNo;
                    chk.Batch = detail.Batch;
                    chk.PaidAmount = detail.AmtPaid;
                    chk.ChkDate = detail.CheckDate;
                    chk.DateReceived = detail.DateReceived;
                    chk.CheckNo = detail.CheckNo;
                    chk.Comment = detail.Comment;
                    chk.ContractualAmount = detail.Contractual;
                    chk.WriteOffAmount = detail.WriteOffAmount;
                    chk.WriteOffCode = detail.WriteOffCode;
                    chk.WriteOffDate = detail.WriteOffDate;
                    chk.Source = detail.Source;

                    chks.Add(chk);
                }

                chkRepository.AddBatch(chks);

                chkBatchRepository.UpdatePostedDate(batch, DateTime.Now);
                chkRepository.CompleteTransaction();

                LoadOpenBatches();
                //clear entry screen for next batch
                MessageBox.Show($"Batch {batch} posted.", "Batch Posted");
                Clear();
            }
            catch(ApplicationException apex)
            {
                Log.Instance.Error($"Error posting payment batch", apex);
                MessageBox.Show("Error occurred. Batch not posted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkRepository.AbortTransaction();
            }
            catch (Exception ex)
            {
                Log.Instance.Error($"Error posting payment batch", ex);
                MessageBox.Show("Error occurred. Batch not posted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                chkRepository.AbortTransaction();
            }
        }

        private void Clear()
        {
            Log.Instance.Trace($"Entering");
            chkDetailsDataTable?.Clear();
            chkBatch = new ChkBatch();
            AmountTotal.Text = "0.00";
            ContractualTotal.Text = "0.00";
            WriteoffTotal.Text = "0.00";
            EntryMode.Enabled = true;
            isGridLoaded = false;
            SaveBatchButton.Enabled = false;
            SubmitPaymentsButton.Enabled = false;
            dgvPayments.Enabled = false;
        }

        private void EntryMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");
            // changes which entry columns are active for quicker data entry

            switch (EntryMode.Text)
            {
                case "Standard":   // amt paid, contractual, & write off enabled.
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = true;
                    break;
                case "Patient":  //amt paid, chk no, chk date enabled
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = false;
                    break;
                case "Commercial":
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = true;
                    break;
                case "Amount Paid": // amt paid enabled
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = false;
                    break;
                case "Contractual": //contractual enabled
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = false;
                    break;
                case "Write Off":
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = false;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = false;
                    break;
                default:
                    dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].Visible = true;
                    dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].Visible = true;
                    break;
            }

        }

        bool skipDgvPaymentsCellValueChanged = false;

        private void dgvPayments_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (dgvPayments.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.AccountNo))
            {
                if (skipDgvPaymentsCellValueChanged)
                    return;

                // get account information to populate patient name and balance info
                Account account;
                string strAccount = dgvPayments[nameof(ChkBatchDetail.AccountNo), e.RowIndex].Value.ToString();
                if (!string.IsNullOrEmpty(strAccount))
                {
                    strAccount = strAccount.ToUpper();
                    account = accountRepository.GetByAccount(strAccount);
                }
                else
                {
                    return;
                }
                
                if (account == null)
                {
                    skipDgvPaymentsCellValueChanged = true;

                    MessageBox.Show($"Account {strAccount} not found.", "Account not found.");
                    dgvPayments.EndEdit();
                    dgvPayments[nameof(ChkBatchDetail.AccountNo), e.RowIndex].Value = string.Empty;
                    dgvPayments.CurrentCell = dgvPayments[nameof(ChkBatchDetail.AccountNo), e.RowIndex];

                    skipDgvPaymentsCellValueChanged = false;
                    return;
                }
                else if (account != null)
                {
                    skipDgvPaymentsCellValueChanged = true;
                    if (account.SentToCollections)
                    {
                        dgvPayments[nameof(ChkBatchDetail.Comment), e.RowIndex].Value = account.AccountNo;
                        dgvPayments.EndEdit();
                        dgvPayments[nameof(ChkBatchDetail.AccountNo), e.RowIndex].Value = "BADDEBT";
                    }
                    else
                    {
                        dgvPayments[nameof(ChkBatchDetail.AccountNo), e.RowIndex].Value = account.AccountNo;
                    }
                    dgvPayments[nameof(ChkBatchDetail.PatientName), e.RowIndex].Value = account.PatFullName;
                    dgvPayments[nameof(ChkBatchDetail.Balance), e.RowIndex].Value = account.Balance;
                    dgvPayments.CurrentCell = dgvPayments[nameof(ChkBatchDetail.CheckNo), e.RowIndex];

                    //clear the readonly flag on the cells
                    SetCellsReadonly(e.RowIndex, false);
                    dgvPayments.RefreshEdit();
                    skipDgvPaymentsCellValueChanged = false;
                }
            }


        }

        private void SetCellsReadonly(int rowIndex, bool setReadonly)
        {
            if(!isGridLoaded)
                return;

            bool isExistingRow = false;

            if (dgvPayments[nameof(ChkBatchDetail.AccountNo), rowIndex].Value != null &&
                    dgvPayments[nameof(ChkBatchDetail.PatientName), rowIndex].Value != null)
            {
                isExistingRow = true;
            }

            for (int i = 0; i < dgvPayments.ColumnCount; i++)
            {
                //these columns are always readonly
                if (dgvPayments.Columns[i].Name == nameof(ChkBatchDetail.PatientName) ||
                    dgvPayments.Columns[i].Name == nameof(ChkBatchDetail.Balance))
                {
                    dgvPayments[i, rowIndex].ReadOnly = true;
                }
                else if(dgvPayments.Columns[i].Name == nameof(ChkBatchDetail.AccountNo))
                {
                    dgvPayments[i, rowIndex].ReadOnly = false;
                }
                else if (isExistingRow)
                {
                    dgvPayments[i, rowIndex].ReadOnly = false;
                }
                else
                {
                    dgvPayments[i, rowIndex].ReadOnly = setReadonly;
                }
            }
        }


        private void dgvPayments_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            Log.Instance.Trace($"Entering");
            //disable the ability to change the entry mode once a row has been added.
            //This will protect against invalid data being written to the database if columns containing data are hidden.
            if (dgvPayments.Rows.Count > 0 && !dgvPayments.Rows[0].IsNewRow)
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
                    a += Double.TryParse(dgvPayments[nameof(ChkBatchDetail.AmtPaid), i].Value?.ToString(), out temp) ? temp : 0.00;
                    c += Double.TryParse(dgvPayments[nameof(ChkBatchDetail.Contractual), i].Value?.ToString(), out temp) ? temp : 0.00;
                    w += Double.TryParse(dgvPayments[nameof(ChkBatchDetail.WriteOffAmount), i].Value?.ToString(), out temp) ? temp : 0.00;
                }
            }

            AmountTotal.Text = a.ToString("0.00");
            ContractualTotal.Text = c.ToString("0.00");
            WriteoffTotal.Text = w.ToString("0.00");

        }

        private void SaveDetails()
        {
            Log.Instance.Trace($"Entering");
            chkDetailsDataTable.AcceptChanges();
            foreach (DataRow row in chkDetailsDataTable.Rows)
            {
                if (string.IsNullOrEmpty(row[nameof(ChkBatchDetail.AccountNo)].ToString()))
                    continue;

                ChkBatchDetail detail = new ChkBatchDetail();

                detail.Batch = chkBatch.BatchNo;
                row[nameof(ChkBatchDetail.AccountNo)] = row[nameof(ChkBatchDetail.AccountNo)].ToString().ToUpper();
                detail.AccountNo = row[nameof(ChkBatchDetail.AccountNo)].ToString().ToUpper();
                detail.AmtPaid = Convert.ToDouble(row[nameof(ChkBatchDetail.AmtPaid)]);
                detail.Contractual = Convert.ToDouble(row[nameof(ChkBatchDetail.Contractual)]);
                detail.WriteOffAmount = Convert.ToDouble(row[nameof(ChkBatchDetail.WriteOffAmount)]);

                if (DateTime.TryParse(row[nameof(ChkBatchDetail.CheckDate)].ToString(), out DateTime temp))
                    detail.CheckDate = temp;

                detail.CheckNo = row[nameof(ChkBatchDetail.CheckNo)].ToString();
                detail.Comment = row[nameof(ChkBatchDetail.Comment)].ToString();

                if (DateTime.TryParse(row[nameof(ChkBatchDetail.DateReceived)].ToString(), out DateTime temp2))
                    detail.DateReceived = temp2;

                detail.Source = row[nameof(ChkBatchDetail.Source)].ToString();
                detail.WriteOffCode = row[nameof(ChkBatchDetail.WriteOffCode)].ToString();

                if (DateTime.TryParse(row[nameof(ChkBatchDetail.WriteOffDate)].ToString(), out DateTime temp3))
                    detail.WriteOffDate = temp3;

                if (!string.IsNullOrWhiteSpace(row[nameof(ChkBatchDetail.Id)].ToString()))
                    detail.Id = Convert.ToInt32(row[nameof(ChkBatchDetail.Id)].ToString());

                var (successFlag, newId) = chkBatchDetailRepository.Save(detail);

                if (newId > 0)
                {
                    row[nameof(ChkBatchDetail.Id)] = newId;
                }

                TotalPayments();
            }

            chkBatch.ChkBatchDetails = Helper.ConvertToList<ChkBatchDetail>(chkDetailsDataTable);

        }

        private void SaveDetailRow(DataRowView row)
        {
            Log.Instance.Trace($"Entering");

            if (string.IsNullOrEmpty(row[nameof(ChkBatchDetail.AccountNo)].ToString()))
                return;

            try
            {
                ChkBatchDetail detail = new ChkBatchDetail();

                detail.Batch = chkBatch.BatchNo;
                row[nameof(ChkBatchDetail.AccountNo)] = row[nameof(ChkBatchDetail.AccountNo)].ToString().ToUpper();
                detail.AccountNo = row[nameof(ChkBatchDetail.AccountNo)].ToString().ToUpper();
                detail.AmtPaid = Convert.ToDouble(row[nameof(ChkBatchDetail.AmtPaid)]);
                detail.Contractual = Convert.ToDouble(row[nameof(ChkBatchDetail.Contractual)]);
                detail.WriteOffAmount = Convert.ToDouble(row[nameof(ChkBatchDetail.WriteOffAmount)]);

                if (DateTime.TryParse(row[nameof(ChkBatchDetail.CheckDate)].ToString(), out DateTime temp))
                    detail.CheckDate = temp;

                detail.CheckNo = row[nameof(ChkBatchDetail.CheckNo)].ToString();
                detail.Comment = row[nameof(ChkBatchDetail.Comment)].ToString();

                if (DateTime.TryParse(row[nameof(ChkBatchDetail.DateReceived)].ToString(), out DateTime temp2))
                    detail.DateReceived = temp2;

                detail.Source = row[nameof(ChkBatchDetail.Source)].ToString();
                detail.WriteOffCode = row[nameof(ChkBatchDetail.WriteOffCode)].ToString();

                if (DateTime.TryParse(row[nameof(ChkBatchDetail.WriteOffDate)].ToString(), out DateTime temp3))
                    detail.WriteOffDate = temp3;

                if (!string.IsNullOrWhiteSpace(row[nameof(ChkBatchDetail.Id)].ToString()))
                    detail.Id = Convert.ToInt32(row[nameof(ChkBatchDetail.Id)].ToString());

                var (successFlag, newId) = chkBatchDetailRepository.Save(detail);

                if (newId > 0)
                {
                    row[nameof(ChkBatchDetail.Id)] = newId;
                }

                TotalPayments();

                chkBatch.ChkBatchDetails = Helper.ConvertToList<ChkBatchDetail>(chkDetailsDataTable);
            }
            catch(Exception ex)
            {
                Log.Instance.Error(ex);
                MessageBox.Show("Error writing detail row. Exit and try again. If the issue persists, contact your administrator.");
            }
        }

        private void dgvPayments_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (!isGridLoaded)
                return;

            //save new row to database
            if (dgvPayments.Rows[e.RowIndex].IsNewRow)
                return;

            chkDetailsBindingSource.EndEdit();
            dgvPayments.NotifyCurrentCellDirty(true);
            dgvPayments.EndEdit();
            dgvPayments.NotifyCurrentCellDirty(false);

            SaveDetailRow((DataRowView)dgvPayments.Rows[e.RowIndex].DataBoundItem);
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

        private void LoadDetailGrid(List<ChkBatchDetail> details)
        {
            dgvPayments.Enabled = true;

            chkDetailsBindingSource = new BindingSource();
            chkDetailsDataTable = details.ToDataTable();
            chkDetailsBindingSource.DataSource = chkDetailsDataTable;

            dgvPayments.DataSource = chkDetailsBindingSource;

            dgvPayments.Columns[nameof(ChkBatchDetail.PatientName)].ReadOnly = true;
            dgvPayments.Columns[nameof(ChkBatchDetail.Balance)].ReadOnly = true;

            dgvPayments.Columns[nameof(ChkBatchDetail.Batch)].Visible = false;
            dgvPayments.Columns[nameof(ChkBatchDetail.Id)].Visible = false;
            dgvPayments.Columns[nameof(ChkBatchDetail.mod_date)].Visible = false;
            dgvPayments.Columns[nameof(ChkBatchDetail.mod_host)].Visible = false;
            dgvPayments.Columns[nameof(ChkBatchDetail.mod_user)].Visible = false;
            dgvPayments.Columns[nameof(ChkBatchDetail.mod_prg)].Visible = false;
            dgvPayments.Columns[nameof(ChkBatchDetail.rowguid)].Visible = false;

            dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            dgvPayments.Columns[nameof(ChkBatchDetail.Balance)].DefaultCellStyle.Format = "N2";
            dgvPayments.Columns[nameof(ChkBatchDetail.Balance)].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;

            dgvPayments.Columns[nameof(ChkBatchDetail.PatientName)].MinimumWidth = 150;

            dgvPayments.Columns[nameof(ChkBatchDetail.Comment)].MinimumWidth = 350;

            int i = 0;
            dgvPayments.Columns[nameof(ChkBatchDetail.AccountNo)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.PatientName)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.Balance)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.CheckNo)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.CheckDate)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.DateReceived)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.Source)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.AmtPaid)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.Contractual)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffAmount)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffCode)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.WriteOffDate)].DisplayIndex = i++;
            dgvPayments.Columns[nameof(ChkBatchDetail.Comment)].DisplayIndex = i++;

            dgvPayments.Columns[nameof(ChkBatchDetail.PatientName)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dgvPayments.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);

            foreach(DataGridViewColumn column in dgvPayments.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

            isGridLoaded = true;
            EntryMode.SelectedIndex = 0;

            SaveBatchButton.Enabled = true;
            SubmitPaymentsButton.Enabled = true;
        }

        private void OpenBatch_SelectionChangeCommitted(object sender, EventArgs e)
        {
            Log.Instance.Trace($"Entering");

            if (Int32.TryParse(OpenBatch.SelectedValue.ToString(), out Int32 number) && OpenBatch.SelectedValue != null)
            {
                //opens a previously open batch for updating and completion
                //read batch data from database

                Clear();

                chkBatch = chkBatchRepository.GetById(number);
                //load data into data grid view

                LoadDetailGrid(chkBatch.ChkBatchDetails);

            }

        }

        private void dgvPayments_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            var senderGrid = (DataGridView)sender;
            if (senderGrid.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.PatientName))
            {
                if (senderGrid.CurrentRow.Cells[e.ColumnIndex].Value == null)
                {
                    return;
                }
            }

            if (senderGrid.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.DateReceived))
            {
                senderGrid[e.ColumnIndex, e.RowIndex].Value = DateTime.Now.ToShortDateString();
            }

            if (senderGrid.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.Source) ||
                senderGrid.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.CheckNo) ||
                senderGrid.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.CheckDate))
            {
                //copy value from previous column
                if (e.RowIndex > 0)
                    senderGrid[e.ColumnIndex, e.RowIndex].Value = senderGrid[e.ColumnIndex, e.RowIndex - 1].Value;
            }

            if (senderGrid.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.WriteOffDate))
            {
                if (!string.IsNullOrEmpty(senderGrid[nameof(ChkBatchDetail.WriteOffCode), e.RowIndex].Value.ToString()))
                {
                    senderGrid[e.ColumnIndex, e.RowIndex].Value = DateTime.Today;
                }
            }
        }

        private void dgvPayments_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvPayments.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.AccountNo))
            {
                Account account = null;
                PersonSearchForm frm = new PersonSearchForm();
                frm.ShowDialog();
                if (frm.SelectedAccount != "" && frm.SelectedAccount != null)
                {
                    string strAccount = frm.SelectedAccount.ToUpper();
                    account = accountRepository.GetByAccount(strAccount, true);
                }
                else
                {
                    Log.Instance.Error($"Person search returned an empty selected account.");
                    MessageBox.Show("A valid account number was not returned from search. Please try again. If problem persists, report issue to an administrator.");
                }
                if (account != null)
                {
                    dgvPayments[nameof(ChkBatchDetail.AccountNo), e.RowIndex].Value = account.AccountNo;
                }
            }
        }

        private void dgvPayments_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if(dgvPayments[nameof(ChkBatchDetail.PatientName), e.RowIndex].Value == null ||
                dgvPayments[nameof(ChkBatchDetail.PatientName), e.RowIndex].Value.ToString() == "")
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
                dgvPayments[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.LightGray;
            }
            else
            {
                dgvPayments[e.ColumnIndex, e.RowIndex].Style.BackColor = Color.White;
            }

        }

        private void NewBatchButton_Click(object sender, EventArgs e)
        {

            Clear();

            chkBatch = new ChkBatch();

            int newBatch = SaveBatch();

            //load new batch
            LoadOpenBatches();
            OpenBatch.SelectedValue = newBatch;

        }

        private void dgvPayments_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void dgvPayments_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            DateTime dateTime = DateTime.MinValue;
            if (dgvPayments.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.DateReceived))
            {

                if (dateTime.IsExpression(dgvPayments[nameof(ChkBatchDetail.DateReceived), e.RowIndex].EditedFormattedValue.ToString()))
                    dateTime = dateTime.ParseExpression(dgvPayments[nameof(ChkBatchDetail.DateReceived), e.RowIndex].EditedFormattedValue.ToString());
                else
                    dateTime = dateTime.ValidateDate(dgvPayments[nameof(ChkBatchDetail.DateReceived), e.RowIndex].EditedFormattedValue.ToString());
                
                if (dateTime != DateTime.MinValue)
                {
                    dgvPayments[nameof(ChkBatchDetail.DateReceived), e.RowIndex].Value = dateTime;
                    dgvPayments.UpdateCellValue(e.ColumnIndex, e.RowIndex);
                    e.ThrowException = false;
                    e.Cancel = false;
                    return;
                }
            }
            else if (dgvPayments.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.CheckDate))
            {
                if (dateTime.IsExpression(dgvPayments[nameof(ChkBatchDetail.CheckDate), e.RowIndex].EditedFormattedValue.ToString()))
                    dateTime = dateTime.ParseExpression(dgvPayments[nameof(ChkBatchDetail.CheckDate), e.RowIndex].EditedFormattedValue.ToString());
                else
                    dateTime = dateTime.ValidateDate(dgvPayments[nameof(ChkBatchDetail.CheckDate), e.RowIndex].EditedFormattedValue.ToString());

                if (dateTime != DateTime.MinValue)
                {
                    dgvPayments[nameof(ChkBatchDetail.CheckDate), e.RowIndex].Value = dateTime;
                    dgvPayments.UpdateCellValue(e.ColumnIndex, e.RowIndex);
                    e.ThrowException = false;
                    e.Cancel = false;
                    return;
                }
            }
            else if (dgvPayments.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.WriteOffDate))
            {
                if (dateTime.IsExpression(dgvPayments[nameof(ChkBatchDetail.WriteOffDate), e.RowIndex].EditedFormattedValue.ToString()))
                    dateTime = dateTime.ParseExpression(dgvPayments[nameof(ChkBatchDetail.WriteOffDate), e.RowIndex].EditedFormattedValue.ToString());
                else
                    dateTime = dateTime.ValidateDate(dgvPayments[nameof(ChkBatchDetail.WriteOffDate), e.RowIndex].EditedFormattedValue.ToString());

                if (dateTime != DateTime.MinValue)
                {
                    dgvPayments[nameof(ChkBatchDetail.WriteOffDate), e.RowIndex].Value = dateTime;
                    dgvPayments.UpdateCellValue(e.ColumnIndex, e.RowIndex);
                    e.ThrowException = false;
                    e.Cancel = false;
                    return;
                }
            }

            e.ThrowException = true;
            return;

        }

        private void dgvPayments_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[nameof(ChkBatchDetail.AmtPaid)].Value = "0.00";
            e.Row.Cells[nameof(ChkBatchDetail.Contractual)].Value = "0.00";
            e.Row.Cells[nameof(ChkBatchDetail.WriteOffAmount)].Value = "0.00";
        }

        private void dgvPayments_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            if (dgvPayments.Columns[e.ColumnIndex].Name == nameof(ChkBatchDetail.WriteOffCode))
            {
                if (!Dictionaries.WriteOffCodes.TryGetValue(e.FormattedValue.ToString(), out string code))
                {
                    DataGridViewCellStyle errorStyle = new DataGridViewCellStyle();
                    errorStyle.BackColor = Color.Red;
                    errorStyle.ForeColor = Color.White;
                    dgvPayments.Rows[e.RowIndex].Cells[nameof(ChkBatchDetail.WriteOffCode)].ErrorText = "Invalid Write Off code";
                    dgvPayments.InvalidateCell(dgvPayments.Rows[e.RowIndex].Cells[nameof(ChkBatchDetail.WriteOffCode)]);
                    dgvPayments.Rows[e.RowIndex].Cells[nameof(ChkBatchDetail.WriteOffCode)].Style = errorStyle;
                    e.Cancel = true;
                }
                else
                {
                    DataGridViewCellStyle normalStyle = new DataGridViewCellStyle();
                    normalStyle.BackColor = Color.White;
                    normalStyle.ForeColor = Color.Black;
                    dgvPayments.Rows[e.RowIndex].Cells[nameof(ChkBatchDetail.WriteOffCode)].Style = normalStyle;

                    dgvPayments.Rows[e.RowIndex].Cells[nameof(ChkBatchDetail.WriteOffCode)].ErrorText = "";
                    //dgvPayments.InvalidateCell(dgvPayments.Rows[e.RowIndex].Cells[nameof(ChkBatchDetail.WriteOffCode)]);
                }
            }

            return;
        }

        private void dgvPayments_RowsRemoved(object sender, DataGridViewRowsRemovedEventArgs e)
        {

        }

        private void dgvPayments_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {

        }

        private void dgvPayments_UserDeletingRow(object sender, DataGridViewRowCancelEventArgs e)
        {
            if (MessageBox.Show("Confirm delete of selected row?", "Confirm Delete",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                int id = Convert.ToInt32(e.Row.Cells[nameof(ChkBatchDetail.Id)].Value);
                e.Cancel = true;
                if (id > 0)
                {
                    if (!chkBatchDetailRepository.Delete(id))
                        MessageBox.Show("Record not deleted.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    e.Cancel = false;
                }
            }
            else
            {
                e.Cancel = true;
            }

        }

        private void dgvPayments_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == dgvPayments.Columns[nameof(ChkBatchDetail.AccountNo)].Index)
            {
                if (e.Value != null)
                {
                    e.Value = e.Value.ToString().ToUpper();
                    e.FormattingApplied = true;
                }
            }
        }

        private void dgvPayments_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            if (e.Control.GetType() == typeof(System.Windows.Forms.TextBox))
            {
                if (e.Control is System.Windows.Forms.TextBox txt)
                {
                    if(txt != null)
                        txt.CharacterCasing = CharacterCasing.Upper;
                }
            }
        }
    }
}
