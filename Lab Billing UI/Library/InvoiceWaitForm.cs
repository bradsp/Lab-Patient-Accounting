using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Library;

public partial class InvoiceWaitForm : Form
{
    public event EventHandler<EventArgs> CancelRequested;

    public InvoiceWaitForm()
    {
        InitializeComponent();
    }
    delegate void SetAccountProgressCallback(int progress, string status);
    delegate void SetInvoiceProgressCallback(int progress, string status);

    public void UpdateAccountProgress(int progress, string status)
    {
        if (accountsProcessedLabel.InvokeRequired)
        {
            SetAccountProgressCallback progressCallback = new SetAccountProgressCallback(SetAccountProgress);
            this.Invoke(progressCallback, [progress, status]);
        }
        else
        {
            accountsProcessedLabel.Text = status;
            accountsProcessedProgress.Value = progress;
        }
    }

    public void UpdateInvoiceProgress(int progress, string status)
    {
        if (invoicesProcessedLabel.InvokeRequired)
        {
            SetInvoiceProgressCallback progressCallback = new SetInvoiceProgressCallback(SetInvoiceProgress);
            this.Invoke(progressCallback, [progress, status]);
        }
        else
        {
            invoicesProcessedLabel.Text = status;
            invoicesProcessedProgress.Value = progress;
        }
    }

    public void SetAccountProgress(int progress, string status)
    {
        accountsProcessedLabel.Text = status;
        accountsProcessedProgress.Value = progress;
    }
    public void SetInvoiceProgress(int progress, string status)
    {
        invoicesProcessedLabel.Text = status;
        invoicesProcessedProgress.Value = progress;
    }

    private void cancelButton_Click(object sender, EventArgs e)
    {
        if(MessageBox.Show("Cancelling will remove all invoices created during this run.\nAre you sure you want to cancel creating invoices?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
        {
            // send cancel to process
            CancelRequested?.Invoke(this, EventArgs.Empty);
        }
    }
}

