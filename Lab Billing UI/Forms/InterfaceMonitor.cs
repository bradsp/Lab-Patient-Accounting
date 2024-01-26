using System;
using System.Data;
using System.Windows.Forms;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using Utilities;
using LabBilling.Core.BusinessLogic;
using WinFormsLibrary;

namespace LabBilling.Forms
{
    public partial class InterfaceMonitor : BaseForm
    {
        public InterfaceMonitor()
        {
            InitializeComponent();
            msgs = new MessagesInboundRepository(Program.AppEnvironment);
        }

        private readonly MessagesInboundRepository msgs;
        private BindingSource bindingSource = new BindingSource();
        private DataTable messagesTable;

        private void InterfaceMonitor_Load(object sender, EventArgs e)
        {
            FromDate.Value = DateTimeHelper.Yesterday();
            Cursor.Current = Cursors.WaitCursor;

            messagesTable = msgs.GetByDateRange(FromDate.Value, ThruDate.Value).ToDataTable();
            messagesTable.PrimaryKey = new DataColumn[] { messagesTable.Columns[nameof(MessageInbound.SystemMsgId)] };
            bindingSource.DataSource = messagesTable;

            MessagesGrid.VirtualMode = true;
            MessagesGrid.DataSource = bindingSource;

            messagesTable.DefaultView.Sort = $"{nameof(MessageInbound.MessageDate)} DESC";

            MessagesGrid.SetColumnsVisibility(false);

            int i = 0;

            MessagesGrid.Columns[nameof(MessageInbound.SystemMsgId)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.SourceAccount)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.SourceMsgId)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.MessageType)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.MessageDate)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.ProcessFlag)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.ProcessStatusMsg)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.Errors)].SetVisibilityOrder(true, i++);
            MessagesGrid.Columns[nameof(MessageInbound.UpdatedDate)].SetVisibilityOrder(true, i++);

            //MessagesGrid.Columns[nameof(MessageInbound.ProcessStatusMsg)].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            MessagesGrid.AutoResizeColumns();

            processFlagFilterCombo.Items.Add("All");
            foreach(var item in Enum.GetValues(typeof(HL7Processor.Status)))
            {
                processFlagFilterCombo.Items.Add(item);
            }

            processFlagFilterCombo.SelectedItem = "All";
            MessageTypeFilterComboBox.SelectedItem = "All";

            ApplyFilter();

            Cursor.Current = Cursors.Default;
        }

        private void MessagesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //place the content of the message in the viewer

            string message = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.HL7Message)].Value.ToString();

            message = FormatMessage(message);

            hl7MessageTextBox.Text = message;

            errorsTextBox.Text = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.Errors)].Value.ToString();

        }

        private void MessageTypeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void ReprocessMessage_Click(object sender, EventArgs e)
        {
            if(MessagesGrid.SelectedRows.Count > 0)
            {
                //string msgType = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.MessageType)].Value.ToString();
                int msgID = Convert.ToInt32(MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.SystemMsgId)].Value);
                string msgType = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.MessageType)].Value.ToString();
                string processFlag = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.ProcessFlag)].Value.ToString();

                
                bool okToProcess = false;

                if (msgType.StartsWith("DFT") && processFlag == "P")
                {
                    if(MessageBox.Show("Reprocessing could result in duplicate charges. Continue anyway?", "Reprocess Charge Message", 
                        MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        okToProcess = true;
                    }
                }
                else
                {
                    okToProcess = true;
                }

                if (okToProcess)
                {
                    HL7Processor hL7Processor = new HL7Processor(Program.AppEnvironment);
                    hL7Processor.ProcessMessage(msgID);

                    var row = messagesTable.Rows.Find(msgID);
                    row[nameof(MessageInbound.ProcessFlag)] = "Reprocessed";
                    row[nameof(MessageInbound.ProcessStatusMsg)] = "Update Date Range to refresh status.";
                    MessagesGrid.Refresh();
                }
            }
            else
            {
                MessageBox.Show("No message selected to process.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private string FormatMessage(string message)
        {
            message = message.Replace("EVN|", "\nEVN|");
            message = message.Replace("PID|", "\nPID|");
            message = message.Replace("PD1|", "\nPD1|");
            message = message.Replace("NK1|", "\nNK1|");
            message = message.Replace("PV1|", "\nPV1|");
            message = message.Replace("PV2|", "\nPV2|");
            message = message.Replace("IN1|", "\nIN1|");
            message = message.Replace("IN2|", "\nIN2|");
            message = message.Replace("GT1|", "\nGT1|");
            message = message.Replace("OBR|", "\nOBR|");
            message = message.Replace("OBX|", "\nOBX|");
            message = message.Replace("NTE|", "\nNTE|");
            message = message.Replace("DG1|", "\nDG1|");
            message = message.Replace("FT1|", "\nFT1|");
            message = message.Replace("PR1|", "\nPR1|");
            message = message.Replace("MFI|", "\nMFI|");
            message = message.Replace("MFE|", "\nMFE|");
            message = message.Replace("STF|", "\nSTF|");
            message = message.Replace("PRA|", "\nPRA|");

            return message;
        }

        private void MessagesGrid_SelectionChanged(object sender, EventArgs e)
        {

            if (MessagesGrid.SelectedRows.Count > 0)
            {
                string message = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.HL7Message)].Value.ToString();

                message = FormatMessage(message);

                hl7MessageTextBox.Text = message;
                errorsTextBox.Text = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.Errors)].Value.ToString();
            }
        }

        private void ApplyFilter()
        {
            messagesTable.DefaultView.RowFilter = "";

            if(!string.IsNullOrEmpty(accountFilterTextBox.Text))
            {
                messagesTable.DefaultView.RowFilter = $"{nameof(MessageInbound.SourceAccount)} = '{accountFilterTextBox.Text}'";
            }

            if (MessageTypeFilterComboBox.SelectedItem != null)
            {
                if (MessageTypeFilterComboBox.SelectedItem.ToString() != "All")
                {
                    string newFilter = messagesTable.DefaultView.RowFilter;

                    if (!string.IsNullOrEmpty(messagesTable.DefaultView.RowFilter))
                    {
                        newFilter += " and ";
                    }

                    newFilter += $"{nameof(MessageInbound.MessageType)} like '{MessageTypeFilterComboBox.SelectedItem}%'";

                    messagesTable.DefaultView.RowFilter = newFilter;
                }
            }

            if (processFlagFilterCombo.SelectedItem != null)
            {
                if (processFlagFilterCombo.SelectedItem.ToString() != "All")
                {
                    string newFilter = messagesTable.DefaultView.RowFilter;

                    if (!string.IsNullOrEmpty(messagesTable.DefaultView.RowFilter))
                    {
                        newFilter += " and ";
                    }

                    newFilter += $"{nameof(MessageInbound.ProcessFlag)} = '{HL7Processor.StatusToString(processFlagFilterCombo.SelectedItem.ToString())}'";

                    messagesTable.DefaultView.RowFilter = newFilter;
                }
            }

            if(showMessagesWithErrorsCheckBox.Checked)
            {
                string newFilter = messagesTable.DefaultView.RowFilter;

                if(!string.IsNullOrEmpty(messagesTable.DefaultView.RowFilter))
                {
                    newFilter += " and ";
                }
                //"NOT(ISNULL(ColumnName,'')='')"
                newFilter += $"NOT(ISNULL({nameof(MessageInbound.Errors)},'') = '')";

                messagesTable.DefaultView.RowFilter = newFilter;
            }
        }

        private void FilterButton_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

            messagesTable = msgs.GetByDateRange(FromDate.Value, ThruDate.Value).ToDataTable();
            messagesTable.PrimaryKey = new DataColumn[] { messagesTable.Columns[nameof(MessageInbound.SystemMsgId)] };

            bindingSource.DataSource = messagesTable;
            MessagesGrid.Refresh();

            messagesTable.DefaultView.Sort = $"{nameof(MessageInbound.MessageDate)} DESC";

            ApplyFilter();

            Cursor.Current = Cursors.Default;
        }

        private void accountFilterTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                ApplyFilter();
            }
        }

        private void processFlagFilterCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }

        private void markDoNotProcessToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessagesGrid.SelectedRows.Count > 0)
            {
                //string msgType = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.MessageType)].Value.ToString();
                int msgID = Convert.ToInt32(MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.SystemMsgId)].Value);
                string msgType = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.MessageType)].Value.ToString();
                string processFlag = MessagesGrid.SelectedRows[0].Cells[nameof(MessageInbound.ProcessFlag)].Value.ToString();

                HL7Processor hl7 = new HL7Processor(Program.AppEnvironment);

                hl7.SetMessageDoNotProcess(msgID, $"Set to do not process by {Program.LoggedInUser.FullName}");

                var row = messagesTable.Rows.Find(msgID);
                row[nameof(MessageInbound.ProcessFlag)] = "DNP";
                row[nameof(MessageInbound.ProcessStatusMsg)] = $"Set to do not process by {Program.LoggedInUser.FullName}";
                MessagesGrid.Refresh();
            }
            else
            {
                MessageBox.Show("No message selected.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }
        }

        private void showMessagesWithErrorsCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            ApplyFilter();
        }
    }
}
