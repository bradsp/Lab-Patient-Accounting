namespace LabBilling.Legacy
{
    /// <summary>
    /// Class to handle Billing Medicaid for Quest.
    /// </summary>
    partial class frmQuest
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmQuest));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbtnLoad = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiQuestRef = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiMCLRef = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBillMCL = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.tsmiExclusions = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBillExclusions = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.tstbRebill = new System.Windows.Forms.ToolStripTextBox();
            this.tsmiCheckErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddPrint = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiPrintView = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiPrintGrid = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFix360Info = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiLoad360FixInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCreate360FixInfo = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbBillingType = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tspbRecords = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslRecords = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslAmount = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslBillType = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.cmsdgvRecords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiClearError = new System.Windows.Forms.ToolStripMenuItem();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.tsMain.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            this.cmsdgvRecords.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSeparator1,
            this.tsddbtnLoad,
            this.toolStripSeparator5,
            this.tsddPrint,
            this.toolStripSeparator2,
            this.tsbFix360Info,
            this.toolStripSeparator7,
            this.tscbBillingType,
            this.toolStripSeparator3});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(777, 27);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "tsMain";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // tsddbtnLoad
            // 
            this.tsddbtnLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnLoad.DoubleClickEnabled = true;
            this.tsddbtnLoad.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiQuestRef,
            this.toolStripSeparator9,
            this.tsmiMCLRef,
            this.toolStripSeparator11,
            this.tsmiExclusions,
            this.toolStripSeparator10,
            this.tstbRebill,
            this.tsmiCheckErrors});
            this.tsddbtnLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnLoad.Image")));
            this.tsddbtnLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnLoad.Name = "tsddbtnLoad";
            this.tsddbtnLoad.Size = new System.Drawing.Size(83, 24);
            this.tsddbtnLoad.Text = "LOAD GRID";
            this.tsddbtnLoad.ButtonClick += new System.EventHandler(this.tsddbtnLoad_Click);
            // 
            // tsmiQuestRef
            // 
            this.tsmiQuestRef.CheckOnClick = true;
            this.tsmiQuestRef.Name = "tsmiQuestRef";
            this.tsmiQuestRef.Size = new System.Drawing.Size(181, 22);
            this.tsmiQuestRef.Tag = "QUEST REFERENCE LAB TESTS ONLY";
            this.tsmiQuestRef.Text = "Quest Reference";
            this.tsmiQuestRef.Click += new System.EventHandler(this.tsmiReference_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(178, 6);
            // 
            // tsmiMCLRef
            // 
            this.tsmiMCLRef.CheckOnClick = true;
            this.tsmiMCLRef.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBillMCL});
            this.tsmiMCLRef.Enabled = false;
            this.tsmiMCLRef.Name = "tsmiMCLRef";
            this.tsmiMCLRef.Size = new System.Drawing.Size(181, 22);
            this.tsmiMCLRef.Tag = "MCL REFERENCE BILLED TO QUEST";
            this.tsmiMCLRef.Text = "GAP LIST";
            this.tsmiMCLRef.Click += new System.EventHandler(this.tsmiMCLRef_Click);
            // 
            // tsmiBillMCL
            // 
            this.tsmiBillMCL.CheckOnClick = true;
            this.tsmiBillMCL.Enabled = false;
            this.tsmiBillMCL.Name = "tsmiBillMCL";
            this.tsmiBillMCL.Size = new System.Drawing.Size(151, 22);
            this.tsmiBillMCL.Text = "BILL TO QUEST";
            this.tsmiBillMCL.Click += new System.EventHandler(this.TsbBillMCLRef_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(178, 6);
            // 
            // tsmiExclusions
            // 
            this.tsmiExclusions.CheckOnClick = true;
            this.tsmiExclusions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiBillExclusions});
            this.tsmiExclusions.Enabled = false;
            this.tsmiExclusions.Name = "tsmiExclusions";
            this.tsmiExclusions.Size = new System.Drawing.Size(181, 22);
            this.tsmiExclusions.Tag = "MCL EXCEPTION LIST BILLING";
            this.tsmiExclusions.Text = "EXCLUSIONS LIST";
            this.tsmiExclusions.Click += new System.EventHandler(this.tsmiExclusion);
            // 
            // tsmiBillExclusions
            // 
            this.tsmiBillExclusions.CheckOnClick = true;
            this.tsmiBillExclusions.Enabled = false;
            this.tsmiBillExclusions.Name = "tsmiBillExclusions";
            this.tsmiBillExclusions.Size = new System.Drawing.Size(180, 22);
            this.tsmiBillExclusions.Text = "BILL TO INSURANCE";
            this.tsmiBillExclusions.Click += new System.EventHandler(this.tsbBillExclusions_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(178, 6);
            // 
            // tstbRebill
            // 
            this.tstbRebill.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbRebill.Name = "tstbRebill";
            this.tstbRebill.Size = new System.Drawing.Size(121, 23);
            this.tstbRebill.Text = "REBILL";
            this.tstbRebill.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tstbRebill_KeyUp);
            this.tstbRebill.DoubleClick += new System.EventHandler(this.tstbRebill_DoubleClick);
            // 
            // tsmiCheckErrors
            // 
            this.tsmiCheckErrors.Name = "tsmiCheckErrors";
            this.tsmiCheckErrors.Size = new System.Drawing.Size(181, 22);
            this.tsmiCheckErrors.Tag = "MCL EXCEPTION LIST BILLING for Errors";
            this.tsmiCheckErrors.Text = "CHECK ERRORS";
            this.tsmiCheckErrors.Click += new System.EventHandler(this.tsmiCheckErrors_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 27);
            this.toolStripSeparator5.Visible = false;
            // 
            // tsddPrint
            // 
            this.tsddPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPrintView,
            this.tsmiPrintGrid});
            this.tsddPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsddPrint.Image")));
            this.tsddPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddPrint.Name = "tsddPrint";
            this.tsddPrint.Size = new System.Drawing.Size(52, 24);
            this.tsddPrint.Text = "PRINT";
            // 
            // tsmiPrintView
            // 
            this.tsmiPrintView.CheckOnClick = true;
            this.tsmiPrintView.Name = "tsmiPrintView";
            this.tsmiPrintView.Size = new System.Drawing.Size(101, 22);
            this.tsmiPrintView.Text = "VIEW";
            this.tsmiPrintView.Click += new System.EventHandler(this.tsbPrintView_Click);
            // 
            // tsmiPrintGrid
            // 
            this.tsmiPrintGrid.Name = "tsmiPrintGrid";
            this.tsmiPrintGrid.Size = new System.Drawing.Size(101, 22);
            this.tsmiPrintGrid.Text = "GRID";
            this.tsmiPrintGrid.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            this.toolStripSeparator2.Visible = false;
            // 
            // tsbFix360Info
            // 
            this.tsbFix360Info.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiLoad360FixInfo,
            this.tsmiCreate360FixInfo});
            this.tsbFix360Info.Image = ((System.Drawing.Image)(resources.GetObject("tsbFix360Info.Image")));
            this.tsbFix360Info.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFix360Info.Name = "tsbFix360Info";
            this.tsbFix360Info.Size = new System.Drawing.Size(103, 24);
            this.tsbFix360Info.Tag = "MCL REFERENCE BILLED TO QUEST";
            this.tsbFix360Info.Text = "Fix 360 Info";
            // 
            // tsmiLoad360FixInfo
            // 
            this.tsmiLoad360FixInfo.CheckOnClick = true;
            this.tsmiLoad360FixInfo.Name = "tsmiLoad360FixInfo";
            this.tsmiLoad360FixInfo.Size = new System.Drawing.Size(164, 22);
            this.tsmiLoad360FixInfo.Text = "Load 360 Info";
            this.tsmiLoad360FixInfo.Click += new System.EventHandler(this.Load360_fix_info_Click);
            // 
            // tsmiCreate360FixInfo
            // 
            this.tsmiCreate360FixInfo.CheckOnClick = true;
            this.tsmiCreate360FixInfo.Name = "tsmiCreate360FixInfo";
            this.tsmiCreate360FixInfo.Size = new System.Drawing.Size(164, 22);
            this.tsmiCreate360FixInfo.Text = "Create 360 HTML";
            this.tsmiCreate360FixInfo.Click += new System.EventHandler(this.tsmiCreate360FixInfo_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 27);
            // 
            // tscbBillingType
            // 
            this.tscbBillingType.Name = "tscbBillingType";
            this.tscbBillingType.Size = new System.Drawing.Size(121, 27);
            this.tscbBillingType.Text = "Billing Type";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbRecords,
            this.toolStripStatusLabel1,
            this.tsslRecords,
            this.toolStripStatusLabel2,
            this.tsslAmount,
            this.tsslBillType});
            this.statusStrip1.Location = new System.Drawing.Point(0, 290);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(777, 25);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tspbRecords
            // 
            this.tspbRecords.Maximum = 1000;
            this.tspbRecords.Name = "tspbRecords";
            this.tspbRecords.Size = new System.Drawing.Size(200, 19);
            this.tspbRecords.Step = 1;
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 20);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // tsslRecords
            // 
            this.tsslRecords.Image = ((System.Drawing.Image)(resources.GetObject("tsslRecords.Image")));
            this.tsslRecords.Name = "tsslRecords";
            this.tsslRecords.Size = new System.Drawing.Size(20, 20);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(10, 20);
            this.toolStripStatusLabel2.Text = "|";
            // 
            // tsslAmount
            // 
            this.tsslAmount.Image = ((System.Drawing.Image)(resources.GetObject("tsslAmount.Image")));
            this.tsslAmount.Name = "tsslAmount";
            this.tsslAmount.Size = new System.Drawing.Size(20, 20);
            // 
            // tsslBillType
            // 
            this.tsslBillType.Name = "tsslBillType";
            this.tsslBillType.Size = new System.Drawing.Size(500, 20);
            this.tsslBillType.Spring = true;
            this.tsslBillType.Text = "BILLING TYPE: ";
            this.tsslBillType.ToolTipText = "Billing Cycle";
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToOrderColumns = true;
            this.dgvRecords.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecords.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvRecords.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.Location = new System.Drawing.Point(0, 27);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvRecords.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRecords.Size = new System.Drawing.Size(777, 263);
            this.dgvRecords.TabIndex = 2;
            this.dgvRecords.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecords_ColumnHeaderMouseClick);
            this.dgvRecords.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecords_RowHeaderMouseClick);
            this.dgvRecords.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecords_RowHeaderMouseDoubleClick);
            this.dgvRecords.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvRecords_RowsRemoved);
            // 
            // cmsdgvRecords
            // 
            this.cmsdgvRecords.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsdgvRecords.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiClearError});
            this.cmsdgvRecords.Name = "cmsdgvRecords";
            this.cmsdgvRecords.Size = new System.Drawing.Size(180, 26);
            this.cmsdgvRecords.Text = "DATA GRIDVIEW MENU";
            // 
            // tsmiClearError
            // 
            this.tsmiClearError.Name = "tsmiClearError";
            this.tsmiClearError.Size = new System.Drawing.Size(179, 22);
            this.tsmiClearError.Text = "CLEAR FOR BILLING";
            this.tsmiClearError.Click += new System.EventHandler(this.tsmiClearError_Click);
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // frmQuest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(777, 315);
            this.Controls.Add(this.dgvRecords);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmQuest";
            this.Text = "ViewerQuest";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmQuest_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.cmsdgvRecords.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslRecords;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsslAmount;
        private System.Windows.Forms.ToolStripStatusLabel tsslBillType;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripSplitButton tsbFix360Info;
        private System.Windows.Forms.ToolStripMenuItem tsmiLoad360FixInfo;
        private System.Windows.Forms.ToolStripMenuItem tsmiCreate360FixInfo;
        private System.Windows.Forms.ToolStripDropDownButton tsddPrint;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintView;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintGrid;
        private System.Windows.Forms.ToolStripComboBox tscbBillingType;
        private System.Windows.Forms.ContextMenuStrip cmsdgvRecords;
        private System.Windows.Forms.ToolStripMenuItem tsmiClearError;
        private System.Windows.Forms.ToolStripSplitButton tsddbtnLoad;
        private System.Windows.Forms.ToolStripMenuItem tsmiQuestRef;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem tsmiMCLRef;
        private System.Windows.Forms.ToolStripMenuItem tsmiBillMCL;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem tsmiExclusions;
        private System.Windows.Forms.ToolStripMenuItem tsmiBillExclusions;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripTextBox tstbRebill;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem tsmiCheckErrors;
    }
}

