namespace LabBilling.Legacy
{
    partial class frmSSI
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSSI));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tssbBillingType = new System.Windows.Forms.ToolStripSplitButton();
            this.iToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.medicareCIGNAToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.allOthersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChampus = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiAllOther1500 = new System.Windows.Forms.ToolStripMenuItem();
            this.outPatientBillingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnStart = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrintGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrintView = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnClear = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFileLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsbpCreate = new System.Windows.Forms.ToolStripProgressBar();
            this.tsslRecCount = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslBatch = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.dsRecords = new System.Data.DataSet();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpAcc = new System.Windows.Forms.TabPage();
            this.tpDoc = new System.Windows.Forms.TabPage();
            this.rtbDoc = new System.Windows.Forms.RichTextBox();
            this.cmsPrint = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiPrintText = new System.Windows.Forms.ToolStripMenuItem();
            this.pAGEToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printDocument1 = new System.Drawing.Printing.PrintDocument();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRecords)).BeginInit();
            this.tcMain.SuspendLayout();
            this.tpAcc.SuspendLayout();
            this.tpDoc.SuspendLayout();
            this.cmsPrint.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbBillingType,
            this.toolStripSeparator1,
            this.btnStart,
            this.toolStripSeparator2,
            this.tsbPrintGrid,
            this.toolStripSeparator3,
            this.tsbPrintView,
            this.toolStripSeparator4,
            this.btnClear,
            this.toolStripSeparator5,
            this.tsbFileLoad,
            this.toolStripSeparator6});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(900, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tssbBillingType
            // 
            this.tssbBillingType.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbBillingType.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iToolStripMenuItem,
            this.pToolStripMenuItem,
            this.outPatientBillingToolStripMenuItem});
            this.tssbBillingType.Image = ((System.Drawing.Image)(resources.GetObject("tssbBillingType.Image")));
            this.tssbBillingType.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbBillingType.Name = "tssbBillingType";
            this.tssbBillingType.Size = new System.Drawing.Size(83, 22);
            this.tssbBillingType.Text = "Billing Type";
            // 
            // iToolStripMenuItem
            // 
            this.iToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.medicareCIGNAToolStripMenuItem,
            this.allOthersToolStripMenuItem});
            this.iToolStripMenuItem.Name = "iToolStripMenuItem";
            this.iToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.iToolStripMenuItem.Text = "837I - UB";
            this.iToolStripMenuItem.Click += new System.EventHandler(this.LoadGrid_UB_Click);
            // 
            // medicareCIGNAToolStripMenuItem
            // 
            this.medicareCIGNAToolStripMenuItem.CheckOnClick = true;
            this.medicareCIGNAToolStripMenuItem.Name = "medicareCIGNAToolStripMenuItem";
            this.medicareCIGNAToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.medicareCIGNAToolStripMenuItem.Text = "Medicare/CIGNA";
            this.medicareCIGNAToolStripMenuItem.Click += new System.EventHandler(this.medicareCIGNAToolStripMenuItem_Click);
            // 
            // allOthersToolStripMenuItem
            // 
            this.allOthersToolStripMenuItem.CheckOnClick = true;
            this.allOthersToolStripMenuItem.Name = "allOthersToolStripMenuItem";
            this.allOthersToolStripMenuItem.Size = new System.Drawing.Size(164, 22);
            this.allOthersToolStripMenuItem.Text = "All Others";
            this.allOthersToolStripMenuItem.Click += new System.EventHandler(this.allOthersToolStripMenuItem_Click);
            // 
            // pToolStripMenuItem
            // 
            this.pToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChampus,
            this.tsmiAllOther1500});
            this.pToolStripMenuItem.Name = "pToolStripMenuItem";
            this.pToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.pToolStripMenuItem.Text = "837P - 1500";
            // 
            // tsmiChampus
            // 
            this.tsmiChampus.CheckOnClick = true;
            this.tsmiChampus.Enabled = false;
            this.tsmiChampus.Name = "tsmiChampus";
            this.tsmiChampus.Size = new System.Drawing.Size(131, 22);
            this.tsmiChampus.Text = "CHAMPUS";
            this.tsmiChampus.Visible = false;
            this.tsmiChampus.Click += new System.EventHandler(this.tsmiChampus_Click);
            // 
            // tsmiAllOther1500
            // 
            this.tsmiAllOther1500.CheckOnClick = true;
            this.tsmiAllOther1500.Name = "tsmiAllOther1500";
            this.tsmiAllOther1500.Size = new System.Drawing.Size(131, 22);
            this.tsmiAllOther1500.Text = "ALL 1500\'s";
            this.tsmiAllOther1500.Click += new System.EventHandler(this.tsmiAllOther1500_Click);
            // 
            // outPatientBillingToolStripMenuItem
            // 
            this.outPatientBillingToolStripMenuItem.CheckOnClick = true;
            this.outPatientBillingToolStripMenuItem.Name = "outPatientBillingToolStripMenuItem";
            this.outPatientBillingToolStripMenuItem.Size = new System.Drawing.Size(167, 22);
            this.outPatientBillingToolStripMenuItem.Text = "OutPatient Billing";
            this.outPatientBillingToolStripMenuItem.Click += new System.EventHandler(this.outPatientBillingToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // btnStart
            // 
            this.btnStart.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnStart.Enabled = false;
            this.btnStart.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.Image")));
            this.btnStart.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new System.Drawing.Size(66, 22);
            this.btnStart.Text = "Create File";
            this.btnStart.Click += new System.EventHandler(this.btnCreateFile_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbPrintView
            // 
            this.tsbPrintView.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPrintView.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrintView.Image")));
            this.tsbPrintView.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrintView.Name = "tsbPrintView";
            this.tsbPrintView.Size = new System.Drawing.Size(64, 22);
            this.tsbPrintView.Text = "Print View";
            this.tsbPrintView.Click += new System.EventHandler(this.tsbPrintView_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // btnClear
            // 
            this.btnClear.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnClear.Image = ((System.Drawing.Image)(resources.GetObject("btnClear.Image")));
            this.btnClear.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(71, 22);
            this.btnClear.Text = "Clear Batch";
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbFileLoad
            // 
            this.tsbFileLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbFileLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsbFileLoad.Image")));
            this.tsbFileLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFileLoad.Name = "tsbFileLoad";
            this.tsbFileLoad.Size = new System.Drawing.Size(69, 22);
            this.tsbFileLoad.Text = "Review File";
            this.tsbFileLoad.Click += new System.EventHandler(this.tsbFileLoad_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // ssMain
            // 
            this.ssMain.Dock = System.Windows.Forms.DockStyle.Top;
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbpCreate,
            this.tsslRecCount,
            this.toolStripStatusLabel1,
            this.tsslBatch,
            this.toolStripStatusLabel2});
            this.ssMain.Location = new System.Drawing.Point(0, 25);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(900, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // tsbpCreate
            // 
            this.tsbpCreate.Name = "tsbpCreate";
            this.tsbpCreate.Size = new System.Drawing.Size(200, 16);
            this.tsbpCreate.Step = 1;
            this.tsbpCreate.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // tsslRecCount
            // 
            this.tsslRecCount.Name = "tsslRecCount";
            this.tsslRecCount.Size = new System.Drawing.Size(48, 17);
            this.tsslRecCount.Text = "Ready...";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(10, 17);
            this.toolStripStatusLabel1.Text = "|";
            // 
            // tsslBatch
            // 
            this.tsslBatch.Name = "tsslBatch";
            this.tsslBatch.Size = new System.Drawing.Size(0, 17);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(199, 17);
            this.toolStripStatusLabel2.Text = "DELIVER THIS PRINT OUT TO CAROL.";
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.BackgroundColor = System.Drawing.Color.White;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.Location = new System.Drawing.Point(3, 3);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            this.dgvRecords.Size = new System.Drawing.Size(886, 360);
            this.dgvRecords.TabIndex = 2;
            this.dgvRecords.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecords_RowHeaderMouseDoubleClick);
            this.dgvRecords.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvRecords_UserDeletedRow);
            // 
            // dsRecords
            // 
            this.dsRecords.DataSetName = "NewDataSet";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpAcc);
            this.tcMain.Controls.Add(this.tpDoc);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 47);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(900, 392);
            this.tcMain.TabIndex = 3;
            // 
            // tpAcc
            // 
            this.tpAcc.Controls.Add(this.dgvRecords);
            this.tpAcc.Location = new System.Drawing.Point(4, 22);
            this.tpAcc.Name = "tpAcc";
            this.tpAcc.Padding = new System.Windows.Forms.Padding(3);
            this.tpAcc.Size = new System.Drawing.Size(892, 366);
            this.tpAcc.TabIndex = 0;
            this.tpAcc.Text = "Accounts";
            this.tpAcc.UseVisualStyleBackColor = true;
            // 
            // tpDoc
            // 
            this.tpDoc.Controls.Add(this.rtbDoc);
            this.tpDoc.Location = new System.Drawing.Point(4, 22);
            this.tpDoc.Name = "tpDoc";
            this.tpDoc.Padding = new System.Windows.Forms.Padding(3);
            this.tpDoc.Size = new System.Drawing.Size(892, 366);
            this.tpDoc.TabIndex = 1;
            this.tpDoc.Text = "Document";
            this.tpDoc.UseVisualStyleBackColor = true;
            // 
            // rtbDoc
            // 
            this.rtbDoc.ContextMenuStrip = this.cmsPrint;
            this.rtbDoc.DetectUrls = false;
            this.rtbDoc.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbDoc.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbDoc.Location = new System.Drawing.Point(3, 3);
            this.rtbDoc.Name = "rtbDoc";
            this.rtbDoc.Size = new System.Drawing.Size(886, 360);
            this.rtbDoc.TabIndex = 0;
            this.rtbDoc.Text = "";
            this.rtbDoc.MouseClick += new System.Windows.Forms.MouseEventHandler(this.rtbDoc_MouseClick);
            this.rtbDoc.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.rtbDoc_MouseDoubleClick);
            this.rtbDoc.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.rtbDoc_PreviewKeyDown);
            // 
            // cmsPrint
            // 
            this.cmsPrint.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPrintText,
            this.pAGEToolStripMenuItem});
            this.cmsPrint.Name = "cmsPrint";
            this.cmsPrint.Size = new System.Drawing.Size(107, 48);
            this.cmsPrint.Text = "PRINT TEXT";
            // 
            // tsmiPrintText
            // 
            this.tsmiPrintText.Name = "tsmiPrintText";
            this.tsmiPrintText.Size = new System.Drawing.Size(106, 22);
            this.tsmiPrintText.Text = "PRINT";
            this.tsmiPrintText.Click += new System.EventHandler(this.tsmiPrintText_Click);
            // 
            // pAGEToolStripMenuItem
            // 
            this.pAGEToolStripMenuItem.Name = "pAGEToolStripMenuItem";
            this.pAGEToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.pAGEToolStripMenuItem.Text = "PAGE";
            this.pAGEToolStripMenuItem.Click += new System.EventHandler(this.pAGEToolStripMenuItem_Click);
            // 
            // frmSSI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(900, 439);
            this.Controls.Add(this.tcMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Name = "frmSSI";
            this.Text = "SSI Submission";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Load_frmSSI);
            this.Shown += new System.EventHandler(this.frmSSI_Shown);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dsRecords)).EndInit();
            this.tcMain.ResumeLayout(false);
            this.tpAcc.ResumeLayout(false);
            this.tpDoc.ResumeLayout(false);
            this.cmsPrint.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Data.DataSet dsRecords;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpAcc;
        private System.Windows.Forms.TabPage tpDoc;
        private System.Windows.Forms.RichTextBox rtbDoc;
        private System.Windows.Forms.ToolStripButton btnStart;
        private System.Windows.Forms.ToolStripStatusLabel tsslRecCount;
        private System.Drawing.Printing.PrintDocument printDocument1;
        private System.Windows.Forms.ContextMenuStrip cmsPrint;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintText;
        private System.Windows.Forms.ToolStripSplitButton tssbBillingType;
        private System.Windows.Forms.ToolStripMenuItem iToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem pToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripProgressBar tsbpCreate;
        private System.Windows.Forms.ToolStripMenuItem pAGEToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbPrintGrid;
        private System.Windows.Forms.ToolStripMenuItem medicareCIGNAToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem allOthersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem outPatientBillingToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel tsslBatch;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbPrintView;
        private System.Windows.Forms.ToolStripMenuItem tsmiChampus;
        private System.Windows.Forms.ToolStripMenuItem tsmiAllOther1500;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnClear;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripButton tsbFileLoad;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
    }
}

