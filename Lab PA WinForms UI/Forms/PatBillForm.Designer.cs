namespace LabBilling.Forms
{
    partial class PatBillForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatBillForm));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbCreateFile = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.scStmtAcc = new System.Windows.Forms.SplitContainer();
            this.dgvStatement = new System.Windows.Forms.DataGridView();
            this.dgvAccount = new System.Windows.Forms.DataGridView();
            this.scEnctrActv = new System.Windows.Forms.SplitContainer();
            this.dgvEncounter = new System.Windows.Forms.DataGridView();
            this.dgvActivity = new System.Windows.Forms.DataGridView();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scStmtAcc)).BeginInit();
            this.scStmtAcc.Panel1.SuspendLayout();
            this.scStmtAcc.Panel2.SuspendLayout();
            this.scStmtAcc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccount)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scEnctrActv)).BeginInit();
            this.scEnctrActv.Panel1.SuspendLayout();
            this.scEnctrActv.Panel2.SuspendLayout();
            this.scEnctrActv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEncounter)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivity)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbCreateFile});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(798, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbCreateFile
            // 
            this.tsbCreateFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCreateFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbCreateFile.Image")));
            this.tsbCreateFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCreateFile.Name = "tsbCreateFile";
            this.tsbCreateFile.Size = new System.Drawing.Size(75, 22);
            this.tsbCreateFile.Text = "CREATE FILE";
            this.tsbCreateFile.Click += new System.EventHandler(this.tsbCreateFile_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslCount});
            this.ssMain.Location = new System.Drawing.Point(0, 374);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(798, 22);
            this.ssMain.TabIndex = 8;
            this.ssMain.Text = "statusStrip1";
            // 
            // tsslCount
            // 
            this.tsslCount.Name = "tsslCount";
            this.tsslCount.Size = new System.Drawing.Size(55, 17);
            this.tsslCount.Text = "Records: ";
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.scMain.IsSplitterFixed = true;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.scMain.Panel1.Controls.Add(this.scStmtAcc);
            this.scMain.Panel1MinSize = 100;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.scMain.Panel2.Controls.Add(this.scEnctrActv);
            this.scMain.Size = new System.Drawing.Size(798, 349);
            this.scMain.SplitterDistance = 100;
            this.scMain.TabIndex = 9;
            // 
            // scStmtAcc
            // 
            this.scStmtAcc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scStmtAcc.IsSplitterFixed = true;
            this.scStmtAcc.Location = new System.Drawing.Point(0, 0);
            this.scStmtAcc.Name = "scStmtAcc";
            this.scStmtAcc.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scStmtAcc.Panel1
            // 
            this.scStmtAcc.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.scStmtAcc.Panel1.Controls.Add(this.dgvStatement);
            // 
            // scStmtAcc.Panel2
            // 
            this.scStmtAcc.Panel2.BackColor = System.Drawing.Color.Cyan;
            this.scStmtAcc.Panel2.Controls.Add(this.dgvAccount);
            this.scStmtAcc.Size = new System.Drawing.Size(798, 100);
            this.scStmtAcc.SplitterDistance = 71;
            this.scStmtAcc.TabIndex = 0;
            // 
            // dgvStatement
            // 
            this.dgvStatement.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.dgvStatement.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvStatement.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvStatement.Location = new System.Drawing.Point(0, 0);
            this.dgvStatement.Name = "dgvStatement";
            this.dgvStatement.Size = new System.Drawing.Size(798, 71);
            this.dgvStatement.TabIndex = 0;
            this.dgvStatement.Enter += new System.EventHandler(this.dgvEnter);
            // 
            // dgvAccount
            // 
            this.dgvAccount.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvAccount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccount.Location = new System.Drawing.Point(0, 0);
            this.dgvAccount.Name = "dgvAccount";
            this.dgvAccount.Size = new System.Drawing.Size(798, 25);
            this.dgvAccount.TabIndex = 0;
            this.dgvAccount.Enter += new System.EventHandler(this.dgvEnter);
            // 
            // scEnctrActv
            // 
            this.scEnctrActv.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scEnctrActv.Location = new System.Drawing.Point(0, 0);
            this.scEnctrActv.Name = "scEnctrActv";
            this.scEnctrActv.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scEnctrActv.Panel1
            // 
            this.scEnctrActv.Panel1.Controls.Add(this.dgvEncounter);
            // 
            // scEnctrActv.Panel2
            // 
            this.scEnctrActv.Panel2.Controls.Add(this.dgvActivity);
            this.scEnctrActv.Size = new System.Drawing.Size(798, 245);
            this.scEnctrActv.SplitterDistance = 186;
            this.scEnctrActv.TabIndex = 0;
            // 
            // dgvEncounter
            // 
            this.dgvEncounter.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEncounter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvEncounter.Location = new System.Drawing.Point(0, 0);
            this.dgvEncounter.Name = "dgvEncounter";
            this.dgvEncounter.Size = new System.Drawing.Size(798, 186);
            this.dgvEncounter.TabIndex = 0;
            this.dgvEncounter.Enter += new System.EventHandler(this.dgvEnter);
            // 
            // dgvActivity
            // 
            this.dgvActivity.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvActivity.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvActivity.Location = new System.Drawing.Point(0, 0);
            this.dgvActivity.Name = "dgvActivity";
            this.dgvActivity.Size = new System.Drawing.Size(798, 55);
            this.dgvActivity.TabIndex = 0;
            this.dgvActivity.Enter += new System.EventHandler(this.dgvEnter);
            // 
            // PatBillForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 396);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Name = "PatBillForm";
            this.Text = "PatBill";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.PatBill_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.scStmtAcc.Panel1.ResumeLayout(false);
            this.scStmtAcc.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scStmtAcc)).EndInit();
            this.scStmtAcc.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvStatement)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccount)).EndInit();
            this.scEnctrActv.Panel1.ResumeLayout(false);
            this.scEnctrActv.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scEnctrActv)).EndInit();
            this.scEnctrActv.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEncounter)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvActivity)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.DataGridView dgvStatement;
        private System.Windows.Forms.SplitContainer scStmtAcc;
        private System.Windows.Forms.DataGridView dgvAccount;
        private System.Windows.Forms.SplitContainer scEnctrActv;
        private System.Windows.Forms.DataGridView dgvEncounter;
        private System.Windows.Forms.DataGridView dgvActivity;
        private System.Windows.Forms.ToolStripButton tsbCreateFile;
        private System.Windows.Forms.ToolStripStatusLabel tsslCount;
    }
}