using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using LabBilling.Core.DataAccess;
using LabBilling.Core.Models;
using NHapi;
using NHapi.Base;
using NHapi.Base.Parser;
using NHapi.Model.V23.Message;

namespace LabBilling.Forms
{
    public partial class InterfaceMonitor : Form
    {
        public InterfaceMonitor()
        {
            InitializeComponent();
            msgs = new MessagesInboundRepository(Helper.ConnVal);
        }

        private readonly MessagesInboundRepository msgs;

        private void InterfaceMonitor_Load(object sender, EventArgs e)
        {
            MessageTypeSelect.SelectedItem = "ADT";
            
            MessagesGrid.DataSource = msgs.GetByMessageType(MessageTypeSelect.Text, DateTime.Now.AddDays(-30), DateTime.Now);

        }

        private void MessagesGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            //place the content of the message in the viewer

            string message = MessagesGrid.SelectedRows[0].Cells[nameof(MessagesInbound.HL7Message)].Value.ToString();

            message = message.Replace("EVN|", "\nEVN|");
            message = message.Replace("PID|", "\nPID|");
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

            hl7Message.Text = message;

        }

        private void MessageTypeSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            MessagesGrid.DataSource = msgs.GetByMessageType(MessageTypeSelect.Text, DateTime.Now.AddDays(-30), DateTime.Now);
        }

        private void ReprocessMessage_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Function not implemented.");
        }

        private void MessagesGrid_SelectionChanged(object sender, EventArgs e)
        {
            string message = MessagesGrid.SelectedRows[0].Cells[nameof(MessagesInbound.HL7Message)].Value.ToString();

            message = message.Replace("EVN|", "\nEVN|");
            message = message.Replace("PID|", "\nPID|");
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

            hl7Message.Text = message;

        }
    }
}
