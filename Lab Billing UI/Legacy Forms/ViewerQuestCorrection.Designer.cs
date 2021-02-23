namespace LabBilling.Legacy
{
    partial class frmCorrection
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCorrection));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tssbFilter = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiDuplicates = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBillCodeErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiBundling = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiClientDiscount = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImportBillingErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCancelNotify = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiTransDateErrors = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCbillFix = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi360NotEntered = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiCbillInvoice = new System.Windows.Forms.ToolStripMenuItem();
            this.tscbInvoices = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbAcc = new System.Windows.Forms.ToolStripComboBox();
            this.tscbName = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbCorrection = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbReqNo = new System.Windows.Forms.ToolStripComboBox();
            this.tscbDQBAccount = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbReload = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbDirections = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbFixHtml = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslRecords = new System.Windows.Forms.ToolStripStatusLabel();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tvMain = new System.Windows.Forms.TreeView();
            this.scSecond = new System.Windows.Forms.SplitContainer();
            this.dgv360 = new System.Windows.Forms.DataGridView();
            this.dgvBilling = new System.Windows.Forms.DataGridView();
            this.dgvChrg = new System.Windows.Forms.DataGridView();
            this.cmsBilling = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiReqNo = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiUID = new System.Windows.Forms.ToolStripMenuItem();
            this.tiReload = new System.Windows.Forms.Timer(this.components);
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.scSecond.Panel1.SuspendLayout();
            this.scSecond.Panel2.SuspendLayout();
            this.scSecond.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgv360)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBilling)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChrg)).BeginInit();
            this.cmsBilling.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbFilter,
            this.toolStripSeparator5,
            this.tscbAcc,
            this.tscbName,
            this.toolStripSeparator1,
            this.tsbCorrection,
            this.toolStripSeparator6,
            this.tscbReqNo,
            this.tscbDQBAccount,
            this.toolStripSeparator3,
            this.tsbReload,
            this.toolStripSeparator4,
            this.tsbDirections,
            this.toolStripSeparator2,
            this.tsbFixHtml});
            this.tsMain.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(913, 46);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tssbFilter
            // 
            this.tssbFilter.AutoSize = false;
            this.tssbFilter.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbFilter.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiDuplicates,
            this.tsmiBillCodeErrors,
            this.tsmiBundling,
            this.tsmiClientDiscount,
            this.tsmiImportBillingErrors,
            this.tsmiCancelNotify,
            this.tsmiTransDateErrors,
            this.tsmiCbillFix,
            this.tsmi360NotEntered,
            this.tsmiCbillInvoice});
            this.tssbFilter.Image = ((System.Drawing.Image)(resources.GetObject("tssbFilter.Image")));
            this.tssbFilter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbFilter.Name = "tssbFilter";
            this.tssbFilter.Size = new System.Drawing.Size(75, 17);
            this.tssbFilter.Text = "FILTER";
            // 
            // tsmiDuplicates
            // 
            this.tsmiDuplicates.CheckOnClick = true;
            this.tsmiDuplicates.Enabled = false;
            this.tsmiDuplicates.Name = "tsmiDuplicates";
            this.tsmiDuplicates.Size = new System.Drawing.Size(217, 22);
            this.tsmiDuplicates.Tag = "1";
            this.tsmiDuplicates.Text = "DUPLICATES";
            this.tsmiDuplicates.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiDuplicates.Click += new System.EventHandler(this.TsbDups_Click);
            // 
            // tsmiBillCodeErrors
            // 
            this.tsmiBillCodeErrors.CheckOnClick = true;
            this.tsmiBillCodeErrors.Name = "tsmiBillCodeErrors";
            this.tsmiBillCodeErrors.Size = new System.Drawing.Size(217, 22);
            this.tsmiBillCodeErrors.Tag = "2";
            this.tsmiBillCodeErrors.Text = "BILL CODE ERRORS";
            this.tsmiBillCodeErrors.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiBillCodeErrors.Click += new System.EventHandler(this.tsmiBillCodeErrors_Click);
            // 
            // tsmiBundling
            // 
            this.tsmiBundling.CheckOnClick = true;
            this.tsmiBundling.Name = "tsmiBundling";
            this.tsmiBundling.Size = new System.Drawing.Size(217, 22);
            this.tsmiBundling.Tag = "4";
            this.tsmiBundling.Text = "BUNDLING NEEDED";
            this.tsmiBundling.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiBundling.Click += new System.EventHandler(this.bUNDLINGNEEDEDToolStripMenuItem_Click);
            // 
            // tsmiClientDiscount
            // 
            this.tsmiClientDiscount.CheckOnClick = true;
            this.tsmiClientDiscount.Name = "tsmiClientDiscount";
            this.tsmiClientDiscount.Size = new System.Drawing.Size(217, 22);
            this.tsmiClientDiscount.Tag = "5";
            this.tsmiClientDiscount.Text = "CLIENT DISCOUNT NEEDED";
            this.tsmiClientDiscount.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiClientDiscount.Click += new System.EventHandler(this.tsmiClientDiscount_Click);
            // 
            // tsmiImportBillingErrors
            // 
            this.tsmiImportBillingErrors.CheckOnClick = true;
            this.tsmiImportBillingErrors.Enabled = false;
            this.tsmiImportBillingErrors.Name = "tsmiImportBillingErrors";
            this.tsmiImportBillingErrors.Size = new System.Drawing.Size(217, 22);
            this.tsmiImportBillingErrors.Tag = "6";
            this.tsmiImportBillingErrors.Text = "IMPORT BILLING ERRORS";
            this.tsmiImportBillingErrors.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiImportBillingErrors.Click += new System.EventHandler(this.tsmiImportBillingErrors_Click);
            // 
            // tsmiCancelNotify
            // 
            this.tsmiCancelNotify.CheckOnClick = true;
            this.tsmiCancelNotify.Enabled = false;
            this.tsmiCancelNotify.Name = "tsmiCancelNotify";
            this.tsmiCancelNotify.Size = new System.Drawing.Size(217, 22);
            this.tsmiCancelNotify.Tag = "7";
            this.tsmiCancelNotify.Text = "CANCEL NOTIFICATIONS";
            this.tsmiCancelNotify.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiCancelNotify.Click += new System.EventHandler(this.tsmiCancelNotify_Click);
            // 
            // tsmiTransDateErrors
            // 
            this.tsmiTransDateErrors.CheckOnClick = true;
            this.tsmiTransDateErrors.Enabled = false;
            this.tsmiTransDateErrors.Name = "tsmiTransDateErrors";
            this.tsmiTransDateErrors.Size = new System.Drawing.Size(217, 22);
            this.tsmiTransDateErrors.Tag = "8";
            this.tsmiTransDateErrors.Text = "TRANS DATE ERRORS";
            this.tsmiTransDateErrors.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiTransDateErrors.Click += new System.EventHandler(this.tRANSDATEERRORSToolStripMenuItem_Click);
            // 
            // tsmiCbillFix
            // 
            this.tsmiCbillFix.CheckOnClick = true;
            this.tsmiCbillFix.Name = "tsmiCbillFix";
            this.tsmiCbillFix.Size = new System.Drawing.Size(217, 22);
            this.tsmiCbillFix.Tag = "9";
            this.tsmiCbillFix.Text = "CBILL FIXES REQUIRED";
            this.tsmiCbillFix.Visible = false;
            this.tsmiCbillFix.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiCbillFix.Click += new System.EventHandler(this.cBILLFIXESREQUIREDToolStripMenuItem_Click);
            // 
            // tsmi360NotEntered
            // 
            this.tsmi360NotEntered.CheckOnClick = true;
            this.tsmi360NotEntered.Name = "tsmi360NotEntered";
            this.tsmi360NotEntered.Size = new System.Drawing.Size(217, 22);
            this.tsmi360NotEntered.Tag = "3";
            this.tsmi360NotEntered.Text = "360 PENDING";
            this.tsmi360NotEntered.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmi360NotEntered.Click += new System.EventHandler(this.tsmi360NotEntered_Click);
            // 
            // tsmiCbillInvoice
            // 
            this.tsmiCbillInvoice.CheckOnClick = true;
            this.tsmiCbillInvoice.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbInvoices});
            this.tsmiCbillInvoice.Name = "tsmiCbillInvoice";
            this.tsmiCbillInvoice.Size = new System.Drawing.Size(217, 22);
            this.tsmiCbillInvoice.Tag = "10";
            this.tsmiCbillInvoice.Text = "CBILL BY INVOICE";
            this.tsmiCbillInvoice.MouseHover += new System.EventHandler(this.tssbFilter_MouseHover);
            this.tsmiCbillInvoice.Click += new System.EventHandler(this.tsmiCbillInvoice_Click);
            // 
            // tscbInvoices
            // 
            this.tscbInvoices.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbInvoices.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbInvoices.DropDownWidth = 121;
            this.tscbInvoices.Name = "tscbInvoices";
            this.tscbInvoices.Size = new System.Drawing.Size(121, 21);
            this.tscbInvoices.Text = "Select Invoice Number";
            this.tscbInvoices.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tsmiInvoice_KeyUp);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 23);
            // 
            // tscbAcc
            // 
            this.tscbAcc.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbAcc.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbAcc.Name = "tscbAcc";
            this.tscbAcc.Size = new System.Drawing.Size(121, 21);
            this.tscbAcc.Sorted = true;
            this.tscbAcc.SelectedIndexChanged += new System.EventHandler(this.tscbAcc_SelectedIndexChanged);
            this.tscbAcc.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tscbAcc_KeyUp);
            // 
            // tscbName
            // 
            this.tscbName.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbName.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbName.Name = "tscbName";
            this.tscbName.Size = new System.Drawing.Size(200, 21);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 23);
            // 
            // tsbCorrection
            // 
            this.tsbCorrection.AutoSize = false;
            this.tsbCorrection.Image = ((System.Drawing.Image)(resources.GetObject("tsbCorrection.Image")));
            this.tsbCorrection.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbCorrection.Name = "tsbCorrection";
            this.tsbCorrection.Size = new System.Drawing.Size(115, 20);
            this.tsbCorrection.Text = "SEND STATUS EMAIL ";
            this.tsbCorrection.Click += new System.EventHandler(this.tsbCorrection_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 23);
            // 
            // tscbReqNo
            // 
            this.tscbReqNo.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tscbReqNo.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbReqNo.Enabled = false;
            this.tscbReqNo.Name = "tscbReqNo";
            this.tscbReqNo.Size = new System.Drawing.Size(125, 21);
            this.tscbReqNo.Text = "Requisition No.";
            this.tscbReqNo.ToolTipText = "From data_quest_billing";
            this.tscbReqNo.SelectedIndexChanged += new System.EventHandler(this.tscbReqNo_SelectedIndexChanged);
            this.tscbReqNo.DropDownClosed += new System.EventHandler(this.tscbReqNo_DropDownClosed);
            // 
            // tscbDQBAccount
            // 
            this.tscbDQBAccount.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tscbDQBAccount.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbDQBAccount.Enabled = false;
            this.tscbDQBAccount.Name = "tscbDQBAccount";
            this.tscbDQBAccount.Size = new System.Drawing.Size(125, 21);
            this.tscbDQBAccount.Text = "Requisition Account";
            this.tscbDQBAccount.ToolTipText = "From data_quest_billing";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 23);
            // 
            // tsbReload
            // 
            this.tsbReload.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbReload.Image = ((System.Drawing.Image)(resources.GetObject("tsbReload.Image")));
            this.tsbReload.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbReload.Name = "tsbReload";
            this.tsbReload.Size = new System.Drawing.Size(58, 17);
            this.tsbReload.Text = "LOAD ALL";
            this.tsbReload.ToolTipText = "LOAD ALL";
            this.tsbReload.Click += new System.EventHandler(this.tsbReload_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 23);
            // 
            // tsbDirections
            // 
            this.tsbDirections.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbDirections.Image = ((System.Drawing.Image)(resources.GetObject("tsbDirections.Image")));
            this.tsbDirections.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbDirections.Name = "tsbDirections";
            this.tsbDirections.Size = new System.Drawing.Size(115, 17);
            this.tsbDirections.Text = "VIEWER DIRECTIONS";
            this.tsbDirections.Click += new System.EventHandler(this.tsbDirections_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 23);
            // 
            // tsbFixHtml
            // 
            this.tsbFixHtml.Image = ((System.Drawing.Image)(resources.GetObject("tsbFixHtml.Image")));
            this.tsbFixHtml.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbFixHtml.Name = "tsbFixHtml";
            this.tsbFixHtml.Size = new System.Drawing.Size(85, 20);
            this.tsbFixHtml.Text = "Fix 360 Info";
            this.tsbFixHtml.Click += new System.EventHandler(this.tsbFixHtml_Click);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslRecords});
            this.ssMain.Location = new System.Drawing.Point(0, 447);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(913, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // tsslRecords
            // 
            this.tsslRecords.Name = "tsslRecords";
            this.tsslRecords.Size = new System.Drawing.Size(0, 17);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 46);
            this.scMain.Name = "scMain";
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.tvMain);
            this.scMain.Panel1Collapsed = true;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.scSecond);
            this.scMain.Size = new System.Drawing.Size(913, 401);
            this.scMain.SplitterDistance = 208;
            this.scMain.TabIndex = 2;
            // 
            // tvMain
            // 
            this.tvMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvMain.Location = new System.Drawing.Point(0, 0);
            this.tvMain.Name = "tvMain";
            this.tvMain.Size = new System.Drawing.Size(208, 100);
            this.tvMain.TabIndex = 0;
            this.tvMain.Visible = false;
            // 
            // scSecond
            // 
            this.scSecond.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scSecond.Location = new System.Drawing.Point(0, 0);
            this.scSecond.Name = "scSecond";
            this.scSecond.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scSecond.Panel1
            // 
            this.scSecond.Panel1.Controls.Add(this.dgv360);
            // 
            // scSecond.Panel2
            // 
            this.scSecond.Panel2.Controls.Add(this.dgvBilling);
            this.scSecond.Panel2.Controls.Add(this.dgvChrg);
            this.scSecond.Size = new System.Drawing.Size(913, 401);
            this.scSecond.SplitterDistance = 75;
            this.scSecond.TabIndex = 0;
            // 
            // dgv360
            // 
            this.dgv360.AllowUserToAddRows = false;
            this.dgv360.AllowUserToDeleteRows = false;
            this.dgv360.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv360.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgv360.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgv360.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgv360.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgv360.Location = new System.Drawing.Point(0, 0);
            this.dgv360.MultiSelect = false;
            this.dgv360.Name = "dgv360";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgv360.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgv360.Size = new System.Drawing.Size(913, 75);
            this.dgv360.TabIndex = 0;
            this.dgv360.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv360_ColumnHeaderMouseDoubleClick);
            this.dgv360.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv360_RowHeaderMouseDoubleClick);
            this.dgv360.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgv360_CellMouseDoubleClick);
            // 
            // dgvBilling
            // 
            this.dgvBilling.AllowUserToAddRows = false;
            this.dgvBilling.AllowUserToDeleteRows = false;
            this.dgvBilling.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBilling.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvBilling.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvBilling.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvBilling.Location = new System.Drawing.Point(98, 121);
            this.dgvBilling.MultiSelect = false;
            this.dgvBilling.Name = "dgvBilling";
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvBilling.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvBilling.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvBilling.Size = new System.Drawing.Size(788, 28);
            this.dgvBilling.TabIndex = 0;
            this.dgvBilling.Visible = false;
            this.dgvBilling.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccount_RowHeaderMouseDoubleClick);
            this.dgvBilling.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvBilling_RowHeaderMouseClick);
            // 
            // dgvChrg
            // 
            this.dgvChrg.AllowUserToAddRows = false;
            this.dgvChrg.AllowUserToDeleteRows = false;
            this.dgvChrg.AllowUserToOrderColumns = true;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChrg.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.dgvChrg.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvChrg.DefaultCellStyle = dataGridViewCellStyle8;
            this.dgvChrg.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvChrg.Location = new System.Drawing.Point(0, 0);
            this.dgvChrg.Name = "dgvChrg";
            this.dgvChrg.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvChrg.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvChrg.Size = new System.Drawing.Size(913, 322);
            this.dgvChrg.TabIndex = 0;
            this.dgvChrg.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccount_RowHeaderMouseDoubleClick);
            this.dgvChrg.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvChrg_RowHeaderMouseClick);
            // 
            // cmsBilling
            // 
            this.cmsBilling.Enabled = false;
            this.cmsBilling.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiReqNo,
            this.tsmiUID});
            this.cmsBilling.Name = "cmsBilling";
            this.cmsBilling.Size = new System.Drawing.Size(153, 48);
            this.cmsBilling.Text = "DELETE";
            // 
            // tsmiReqNo
            // 
            this.tsmiReqNo.Enabled = false;
            this.tsmiReqNo.Name = "tsmiReqNo";
            this.tsmiReqNo.Size = new System.Drawing.Size(152, 22);
            this.tsmiReqNo.Text = "By Requisition";
            this.tsmiReqNo.Click += new System.EventHandler(this.tsmiReqNo_Click);
            // 
            // tsmiUID
            // 
            this.tsmiUID.Enabled = false;
            this.tsmiUID.Name = "tsmiUID";
            this.tsmiUID.Size = new System.Drawing.Size(152, 22);
            this.tsmiUID.Text = "By UID";
            this.tsmiUID.Click += new System.EventHandler(this.tsmiUID_Click);
            // 
            // tiReload
            // 
            this.tiReload.Enabled = true;
            this.tiReload.Interval = 100000;
            // 
            // frmCorrection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(913, 469);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmCorrection";
            this.Text = "ViewerQuestCorrection";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCorrection_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.scSecond.Panel1.ResumeLayout(false);
            this.scSecond.Panel2.ResumeLayout(false);
            this.scSecond.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgv360)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBilling)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvChrg)).EndInit();
            this.cmsBilling.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripComboBox tscbAcc;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripComboBox tscbName;
        private System.Windows.Forms.ToolStripStatusLabel tsslRecords;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TreeView tvMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.SplitContainer scSecond;
        private System.Windows.Forms.DataGridView dgv360;
        private System.Windows.Forms.DataGridView dgvBilling;
        private System.Windows.Forms.ToolStripButton tsbCorrection;
        private System.Windows.Forms.ToolStripComboBox tscbReqNo;
        private System.Windows.Forms.ToolStripComboBox tscbDQBAccount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton tsbReload;
        private System.Windows.Forms.DataGridView dgvChrg;
        private System.Windows.Forms.Timer tiReload;
        private System.Windows.Forms.ToolStripSplitButton tssbFilter;
        private System.Windows.Forms.ToolStripMenuItem tsmiDuplicates;
        private System.Windows.Forms.ToolStripMenuItem tsmiBillCodeErrors;
        private System.Windows.Forms.ToolStripMenuItem tsmi360NotEntered;
        private System.Windows.Forms.ToolStripMenuItem tsmiBundling;
        private System.Windows.Forms.ToolStripMenuItem tsmiClientDiscount;
        private System.Windows.Forms.ToolStripMenuItem tsmiImportBillingErrors;
        private System.Windows.Forms.ToolStripMenuItem tsmiCancelNotify;
        private System.Windows.Forms.ToolStripMenuItem tsmiTransDateErrors;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton tsbDirections;
        private System.Windows.Forms.ToolStripMenuItem tsmiCbillFix;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem tsmiCbillInvoice;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tsbFixHtml;
        private System.Windows.Forms.ContextMenuStrip cmsBilling;
        private System.Windows.Forms.ToolStripMenuItem tsmiReqNo;
        private System.Windows.Forms.ToolStripMenuItem tsmiUID;
        private System.Windows.Forms.ToolStripComboBox tscbInvoices;
    }
}

