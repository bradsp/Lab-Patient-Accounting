namespace LabBilling.Forms
{
    partial class frmBadDebt
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmBadDebt));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsddbtnBadDebt = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiSelectAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.GeneratePatientBillsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateCollectionsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoadMailerP = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbWriteOff = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReadMCLFile = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrintGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSmallBalWriteOff = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tspbRecords = new System.Windows.Forms.ToolStripProgressBar();
            this.ssRecords = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbtnBadDebt,
            this.toolStripSeparator5,
            this.tsbLoad,
            this.toolStripSeparator1,
            this.tsbLoadMailerP,
            this.toolStripSeparator2,
            this.tsbWriteOff,
            this.toolStripSeparator3,
            this.tsbReadMCLFile,
            this.toolStripSeparator4,
            this.tsbPrintGrid,
            this.toolStripSeparator6,
            this.tsbSmallBalWriteOff});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(1010, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsddbtnBadDebt
            // 
            this.tsddbtnBadDebt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnBadDebt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiSelectAccounts,
            this.GeneratePatientBillsToolStripMenuItem,
            this.GenerateCollectionsFileToolStripMenuItem});
            this.tsddbtnBadDebt.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnBadDebt.Image")));
            this.tsddbtnBadDebt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnBadDebt.Name = "tsddbtnBadDebt";
            this.tsddbtnBadDebt.Size = new System.Drawing.Size(78, 22);
            this.tsddbtnBadDebt.Text = "Operations";
            // 
            // tsmiSelectAccounts
            // 
            this.tsmiSelectAccounts.CheckOnClick = true;
            this.tsmiSelectAccounts.Name = "tsmiSelectAccounts";
            this.tsmiSelectAccounts.Size = new System.Drawing.Size(204, 22);
            this.tsmiSelectAccounts.Text = "SELECT PAT BILL ACC\'s";
            this.tsmiSelectAccounts.ToolTipText = "Use \"Bad Debt Batch\" on the MCL billing menu";
            this.tsmiSelectAccounts.Visible = false;
            this.tsmiSelectAccounts.Click += new System.EventHandler(this.TsmiSelectAccounts_Click);
            // 
            // GeneratePatientBillsToolStripMenuItem
            // 
            this.GeneratePatientBillsToolStripMenuItem.CheckOnClick = true;
            this.GeneratePatientBillsToolStripMenuItem.Name = "GeneratePatientBillsToolStripMenuItem";
            this.GeneratePatientBillsToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.GeneratePatientBillsToolStripMenuItem.Text = "Generate Patient Bills";
            this.GeneratePatientBillsToolStripMenuItem.Click += new System.EventHandler(this.GeneratePatientBillsToolStripMenuItem_Click);
            // 
            // GenerateCollectionsFileToolStripMenuItem
            // 
            this.GenerateCollectionsFileToolStripMenuItem.Name = "GenerateCollectionsFileToolStripMenuItem";
            this.GenerateCollectionsFileToolStripMenuItem.Size = new System.Drawing.Size(204, 22);
            this.GenerateCollectionsFileToolStripMenuItem.Text = "Generate Collections File";
            this.GenerateCollectionsFileToolStripMenuItem.Click += new System.EventHandler(this.GenerateCollectionsFileToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbLoad
            // 
            this.tsbLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoad.Image")));
            this.tsbLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoad.Name = "tsbLoad";
            this.tsbLoad.Size = new System.Drawing.Size(144, 22);
            this.tsbLoad.Text = "Load Grid From Bad Debt";
            this.tsbLoad.Click += new System.EventHandler(this.tsbLoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbLoadMailerP
            // 
            this.tsbLoadMailerP.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoadMailerP.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadMailerP.Image")));
            this.tsbLoadMailerP.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadMailerP.Name = "tsbLoadMailerP";
            this.tsbLoadMailerP.Size = new System.Drawing.Size(144, 22);
            this.tsbLoadMailerP.Text = "Load Grid with Mailer \"P\"";
            this.tsbLoadMailerP.Click += new System.EventHandler(this.tsbLoadMailerP_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbWriteOff
            // 
            this.tsbWriteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbWriteOff.Image = ((System.Drawing.Image)(resources.GetObject("tsbWriteOff.Image")));
            this.tsbWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWriteOff.Name = "tsbWriteOff";
            this.tsbWriteOff.Size = new System.Drawing.Size(59, 22);
            this.tsbWriteOff.Text = "Write Off";
            this.tsbWriteOff.Click += new System.EventHandler(this.tsbWriteOff_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbReadMCLFile
            // 
            this.tsbReadMCLFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReadMCLFile.Image = ((System.Drawing.Image)(resources.GetObject("tsbReadMCLFile.Image")));
            this.tsbReadMCLFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReadMCLFile.Name = "tsbReadMCLFile";
            this.tsbReadMCLFile.Size = new System.Drawing.Size(64, 22);
            this.tsbReadMCLFile.Text = "READ FILE";
            this.tsbReadMCLFile.Click += new System.EventHandler(this.tsbReadMCLFile_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPrintGrid
            // 
            this.tsbPrintGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPrintGrid.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintGrid.Image")));
            this.tsbPrintGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintGrid.Name = "tsbPrintGrid";
            this.tsbPrintGrid.Size = new System.Drawing.Size(61, 22);
            this.tsbPrintGrid.Text = "Print Grid";
            this.tsbPrintGrid.Click += new System.EventHandler(this.tsbPrintGrid_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSmallBalWriteOff
            // 
            this.tsbSmallBalWriteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSmallBalWriteOff.Enabled = false;
            this.tsbSmallBalWriteOff.Image = ((System.Drawing.Image)(resources.GetObject("tsbSmallBalWriteOff.Image")));
            this.tsbSmallBalWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSmallBalWriteOff.Name = "tsbSmallBalWriteOff";
            this.tsbSmallBalWriteOff.Size = new System.Drawing.Size(135, 22);
            this.tsbSmallBalWriteOff.Text = "Small Balance Write Off";
            this.tsbSmallBalWriteOff.ToolTipText = "Small Balance Write Off $10.00 and less";
            this.tsbSmallBalWriteOff.Click += new System.EventHandler(this.TsbSmallBalWriteOff_Click);
            // 
            // ssMain
            // 
            this.ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbRecords,
            this.ssRecords});
            this.ssMain.Location = new System.Drawing.Point(0, 630);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(1010, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // tspbRecords
            // 
            this.tspbRecords.Name = "tspbRecords";
            this.tspbRecords.Size = new System.Drawing.Size(200, 16);
            this.tspbRecords.Step = 1;
            // 
            // ssRecords
            // 
            this.ssRecords.Name = "ssRecords";
            this.ssRecords.Size = new System.Drawing.Size(52, 17);
            this.ssRecords.Text = "Records:";
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.AllowUserToAddRows = false;
            this.dgvAccounts.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvAccounts.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccounts.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccounts.Location = new System.Drawing.Point(0, 25);
            this.dgvAccounts.Name = "dgvAccounts";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccounts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvAccounts.Size = new System.Drawing.Size(1010, 605);
            this.dgvAccounts.TabIndex = 2;
            this.dgvAccounts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccounts_CellClick);
            this.dgvAccounts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAccounts_CellFormatting);
            this.dgvAccounts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_ColumnHeaderMouseClick);
            this.dgvAccounts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAccounts_DataError);
            this.dgvAccounts.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_RowHeaderMouseDoubleClick);
            this.dgvAccounts.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvAccounts_RowsAdded);
            this.dgvAccounts.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvAccounts_RowsRemoved);
            this.dgvAccounts.SelectionChanged += new System.EventHandler(this.DgvAccounts_SelectionChanged);
            // 
            // frmBadDebt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 652);
            this.Controls.Add(this.dgvAccounts);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmBadDebt";
            this.Text = "Bad Debt";
            this.Load += new System.EventHandler(this.frmBadDebt_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.ToolStripButton tsbWriteOff;
        private System.Windows.Forms.ToolStripButton tsbLoad;
        private System.Windows.Forms.ToolStripStatusLabel ssRecords;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbLoadMailerP;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbReadMCLFile;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbPrintGrid;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnBadDebt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAccounts;
        private System.Windows.Forms.ToolStripMenuItem GeneratePatientBillsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbSmallBalWriteOff;
        private System.Windows.Forms.ToolStripMenuItem GenerateCollectionsFileToolStripMenuItem;
    }
}

