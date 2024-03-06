namespace LabBilling.Library
{
    partial class InvoiceWaitForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            invoicesProcessedProgress = new System.Windows.Forms.ProgressBar();
            accountsProcessedProgress = new System.Windows.Forms.ProgressBar();
            invoicesProcessedLabel = new System.Windows.Forms.Label();
            accountsProcessedLabel = new System.Windows.Forms.Label();
            SuspendLayout();
            // 
            // invoicesProcessedProgress
            // 
            invoicesProcessedProgress.Location = new System.Drawing.Point(30, 65);
            invoicesProcessedProgress.Name = "invoicesProcessedProgress";
            invoicesProcessedProgress.Size = new System.Drawing.Size(332, 23);
            invoicesProcessedProgress.TabIndex = 0;
            // 
            // accountsProcessedProgress
            // 
            accountsProcessedProgress.Location = new System.Drawing.Point(30, 124);
            accountsProcessedProgress.Name = "accountsProcessedProgress";
            accountsProcessedProgress.Size = new System.Drawing.Size(332, 23);
            accountsProcessedProgress.TabIndex = 0;
            // 
            // invoicesProcessedLabel
            // 
            invoicesProcessedLabel.Location = new System.Drawing.Point(30, 29);
            invoicesProcessedLabel.Name = "invoicesProcessedLabel";
            invoicesProcessedLabel.Size = new System.Drawing.Size(332, 33);
            invoicesProcessedLabel.TabIndex = 1;
            invoicesProcessedLabel.Text = "Invoices Processed";
            // 
            // accountsProcessedLabel
            // 
            accountsProcessedLabel.AutoSize = true;
            accountsProcessedLabel.Location = new System.Drawing.Point(30, 106);
            accountsProcessedLabel.Name = "accountsProcessedLabel";
            accountsProcessedLabel.Size = new System.Drawing.Size(113, 15);
            accountsProcessedLabel.TabIndex = 1;
            accountsProcessedLabel.Text = "Accounts Processed";
            // 
            // InvoiceWaitForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(397, 193);
            ControlBox = false;
            Controls.Add(accountsProcessedLabel);
            Controls.Add(invoicesProcessedLabel);
            Controls.Add(accountsProcessedProgress);
            Controls.Add(invoicesProcessedProgress);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InvoiceWaitForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "InvoiceWaitForm";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ProgressBar invoicesProcessedProgress;
        private System.Windows.Forms.ProgressBar accountsProcessedProgress;
        private System.Windows.Forms.Label invoicesProcessedLabel;
        private System.Windows.Forms.Label accountsProcessedLabel;
    }
}