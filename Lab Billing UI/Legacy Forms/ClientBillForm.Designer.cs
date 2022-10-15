namespace LabBilling.Legacy
{
    /// <summary>
    /// Class for the viewer.
    /// </summary>
    partial class ClientBillForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ClientBillForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbLoad = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.tscbClient = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCbill = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddGreenSheets = new System.Windows.Forms.ToolStripDropDownButton();
            this.clientsWithYFinCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nonClientFinCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patientsWmultipleFinCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clientsWithSFinCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.industryBillingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wCCDMNotToBillAsYToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbPrintGrid = new System.Windows.Forms.ToolStripButton();
            this.InvoiceTestButton = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslAccounts = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvRecords = new System.Windows.Forms.DataGridView();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tvAccounts = new System.Windows.Forms.TreeView();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoad,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.tscbClient,
            this.toolStripSeparator2,
            this.tsbCbill,
            this.toolStripSeparator3,
            this.tsddGreenSheets,
            this.toolStripSeparator4,
            this.tsbPrintGrid,
            this.InvoiceTestButton});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(659, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbLoad
            // 
            this.tsbLoad.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoad.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoad.Image")));
            this.tsbLoad.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoad.Name = "tsbLoad";
            this.tsbLoad.Size = new System.Drawing.Size(37, 22);
            this.tsbLoad.Text = "Load";
            this.tsbLoad.Click += new System.EventHandler(this.tsbLoad_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(62, 22);
            this.toolStripLabel1.Text = "Client List:";
            // 
            // tscbClient
            // 
            this.tscbClient.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbClient.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbClient.Name = "tscbClient";
            this.tscbClient.Size = new System.Drawing.Size(100, 25);
            this.tscbClient.Sorted = true;
            this.tscbClient.SelectedIndexChanged += new System.EventHandler(this.tscbClient_SelectedIndexChanged);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbCbill
            // 
            this.tsbCbill.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbCbill.Image = ((System.Drawing.Image)(resources.GetObject("tsbCbill.Image")));
            this.tsbCbill.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCbill.Name = "tsbCbill";
            this.tsbCbill.Size = new System.Drawing.Size(78, 22);
            this.tsbCbill.Text = "Create CBILL";
            this.tsbCbill.Click += new System.EventHandler(this.tsbCbill_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tsddGreenSheets
            // 
            this.tsddGreenSheets.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddGreenSheets.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clientsWithYFinCodesToolStripMenuItem,
            this.nonClientFinCodesToolStripMenuItem,
            this.patientsWmultipleFinCodesToolStripMenuItem,
            this.clientsWithSFinCodesToolStripMenuItem,
            this.industryBillingToolStripMenuItem,
            this.wCCDMNotToBillAsYToolStripMenuItem});
            this.tsddGreenSheets.Image = ((System.Drawing.Image)(resources.GetObject("tsddGreenSheets.Image")));
            this.tsddGreenSheets.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddGreenSheets.Name = "tsddGreenSheets";
            this.tsddGreenSheets.Size = new System.Drawing.Size(143, 22);
            this.tsddGreenSheets.Text = "Green Sheet Processing";
            // 
            // clientsWithYFinCodesToolStripMenuItem
            // 
            this.clientsWithYFinCodesToolStripMenuItem.Name = "clientsWithYFinCodesToolStripMenuItem";
            this.clientsWithYFinCodesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.clientsWithYFinCodesToolStripMenuItem.Text = "Clients with \"Y\" FinCodes";
            this.clientsWithYFinCodesToolStripMenuItem.ToolTipText = "\'BRMC\',\'DMG\',\'CFMC\',\'CTSC\',\'DMGH\',\'EJC\',\'GOO\',\'MCJ\',\'MSC\',\'ONHE\',\'WTN\',\'BLS\',\'DDC" +
    "\',\'BAFC\',\'LXMC\',\'FCH\',\'JFMC\',\'TSOSC\'";
            this.clientsWithYFinCodesToolStripMenuItem.Click += new System.EventHandler(this.clientsWithYFinCodesToolStripMenuItem_Click);
            // 
            // nonClientFinCodesToolStripMenuItem
            // 
            this.nonClientFinCodesToolStripMenuItem.Name = "nonClientFinCodesToolStripMenuItem";
            this.nonClientFinCodesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.nonClientFinCodesToolStripMenuItem.Text = "Non Client FinCodes";
            this.nonClientFinCodesToolStripMenuItem.ToolTipText = "\'BCBE\',\'MAC\',\'PTAI\',\'WTTC\',\'JAH\',\'TTP\'";
            this.nonClientFinCodesToolStripMenuItem.Click += new System.EventHandler(this.nonClientFinCodesToolStripMenuItem_Click);
            // 
            // patientsWmultipleFinCodesToolStripMenuItem
            // 
            this.patientsWmultipleFinCodesToolStripMenuItem.Name = "patientsWmultipleFinCodesToolStripMenuItem";
            this.patientsWmultipleFinCodesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.patientsWmultipleFinCodesToolStripMenuItem.Text = "Patients w/multiple FinCodes";
            this.patientsWmultipleFinCodesToolStripMenuItem.Click += new System.EventHandler(this.patientsWmultipleFinCodesToolStripMenuItem_Click);
            // 
            // clientsWithSFinCodesToolStripMenuItem
            // 
            this.clientsWithSFinCodesToolStripMenuItem.Name = "clientsWithSFinCodesToolStripMenuItem";
            this.clientsWithSFinCodesToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.clientsWithSFinCodesToolStripMenuItem.Text = "Clients with \"S\" FinCodes";
            this.clientsWithSFinCodesToolStripMenuItem.Click += new System.EventHandler(this.clientsWithSFinCodesToolStripMenuItem_Click);
            // 
            // industryBillingToolStripMenuItem
            // 
            this.industryBillingToolStripMenuItem.Name = "industryBillingToolStripMenuItem";
            this.industryBillingToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.industryBillingToolStripMenuItem.Text = "Industry Billing";
            this.industryBillingToolStripMenuItem.Click += new System.EventHandler(this.industryBillingToolStripMenuItem_Click);
            // 
            // wCCDMNotToBillAsYToolStripMenuItem
            // 
            this.wCCDMNotToBillAsYToolStripMenuItem.Name = "wCCDMNotToBillAsYToolStripMenuItem";
            this.wCCDMNotToBillAsYToolStripMenuItem.Size = new System.Drawing.Size(229, 22);
            this.wCCDMNotToBillAsYToolStripMenuItem.Text = "WC CDM Exclusions";
            this.wCCDMNotToBillAsYToolStripMenuItem.Click += new System.EventHandler(this.wCCDMNotToBillAsYToolStripMenuItem_Click);
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
            // InvoiceTestButton
            // 
            this.InvoiceTestButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.InvoiceTestButton.Image = ((System.Drawing.Image)(resources.GetObject("InvoiceTestButton.Image")));
            this.InvoiceTestButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.InvoiceTestButton.Name = "InvoiceTestButton";
            this.InvoiceTestButton.Size = new System.Drawing.Size(72, 22);
            this.InvoiceTestButton.Text = "Invoice Test";
            this.InvoiceTestButton.Click += new System.EventHandler(this.InvoiceTestButton_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslAccounts});
            this.ssMain.Location = new System.Drawing.Point(0, 390);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(659, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // tsslAccounts
            // 
            this.tsslAccounts.Name = "tsslAccounts";
            this.tsslAccounts.Size = new System.Drawing.Size(0, 17);
            // 
            // dgvRecords
            // 
            this.dgvRecords.AllowUserToAddRows = false;
            this.dgvRecords.AllowUserToOrderColumns = true;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.dgvRecords.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvRecords.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvRecords.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvRecords.Location = new System.Drawing.Point(0, 0);
            this.dgvRecords.Name = "dgvRecords";
            this.dgvRecords.ReadOnly = true;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvRecords.RowsDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvRecords.Size = new System.Drawing.Size(437, 365);
            this.dgvRecords.TabIndex = 4;
            this.dgvRecords.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecords_ColumnHeaderMouseClick);
            this.dgvRecords.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvRecords_RowHeaderMouseDoubleClick);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tvAccounts);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.dgvRecords);
            this.scMain.Size = new System.Drawing.Size(659, 365);
            this.scMain.SplitterDistance = 218;
            this.scMain.TabIndex = 5;
            // 
            // tvAccounts
            // 
            this.tvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvAccounts.Location = new System.Drawing.Point(0, 0);
            this.tvAccounts.Name = "tvAccounts";
            this.tvAccounts.Size = new System.Drawing.Size(218, 365);
            this.tvAccounts.TabIndex = 0;
            this.tvAccounts.NodeMouseClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.tvAccounts_NodeMouseClick);
            // 
            // frmViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(659, 412);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Name = "frmViewer";
            this.ShowInTaskbar = false;
            this.Text = "Viewer Client";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmViewer_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRecords)).EndInit();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox tscbClient;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripButton tsbLoad;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripStatusLabel tsslAccounts;
        private System.Windows.Forms.DataGridView dgvRecords;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TreeView tvAccounts;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbCbill;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripDropDownButton tsddGreenSheets;
        private System.Windows.Forms.ToolStripMenuItem clientsWithYFinCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nonClientFinCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patientsWmultipleFinCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbPrintGrid;
        private System.Windows.Forms.ToolStripMenuItem clientsWithSFinCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem industryBillingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wCCDMNotToBillAsYToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton InvoiceTestButton;
    }
}

