namespace LabBilling.Forms
{
    partial class ReportingPortalForm
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
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            kryptonWebBrowser1 = new Krypton.Toolkit.KryptonWebBrowser();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 495);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(933, 24);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(117, 18);
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 19);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // kryptonWebBrowser1
            // 
            kryptonWebBrowser1.Dock = DockStyle.Fill;
            kryptonWebBrowser1.Location = new Point(0, 0);
            kryptonWebBrowser1.Name = "kryptonWebBrowser1";
            kryptonWebBrowser1.Size = new Size(933, 495);
            kryptonWebBrowser1.TabIndex = 3;
            // 
            // ReportingPortalForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(kryptonWebBrowser1);
            Controls.Add(statusStrip1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ReportingPortalForm";
            Text = "ReportingPortalForm";
            FormClosed += ReportingPortalForm_FormClosed;
            Load += ReportingPortalForm_Load;
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private Krypton.Toolkit.KryptonWebBrowser kryptonWebBrowser1;
    }
}