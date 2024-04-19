namespace LabBilling.Legacy
{
    partial class Posting835
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Posting835));
            ssMain = new StatusStrip();
            tspbRecords = new ToolStripProgressBar();
            tsslProgress = new ToolStripStatusLabel();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tsslProcessed = new ToolStripStatusLabel();
            toolStripStatusLabel2 = new ToolStripStatusLabel();
            tsslDenieds = new ToolStripStatusLabel();
            toolStripStatusLabel3 = new ToolStripStatusLabel();
            tsslNotProcessed = new ToolStripStatusLabel();
            toolStripStatusLabel4 = new ToolStripStatusLabel();
            tsslEOB = new ToolStripStatusLabel();
            tc835 = new TabControl();
            tpProcessed = new TabPage();
            dgvProcessed = new DataGridView();
            tpDenieds = new TabPage();
            dgvDenieds = new DataGridView();
            tpNotProcessed = new TabPage();
            dgvNotProcessed = new DataGridView();
            tpEOB = new TabPage();
            dgvEOB = new DataGridView();
            tlpTotals = new TableLayoutPanel();
            tbProviderID = new TextBox();
            tbCheckDate = new TextBox();
            tbFileDate = new TextBox();
            tbFileName = new TextBox();
            tbCheckNo = new TextBox();
            tbBatchNo = new TextBox();
            tbBillCycle = new TextBox();
            tbFileNumber = new TextBox();
            tbDatabase = new TextBox();
            tbChargeAmt = new TextBox();
            tbEOBChargeAmt = new TextBox();
            tbPaidAmt = new TextBox();
            tbEOBPaidAmt = new TextBox();
            tbContractualAmt = new TextBox();
            tbEOBContractualAmt = new TextBox();
            textBox1 = new TextBox();
            tbEOBOtherAmt = new TextBox();
            tbDeniedAmt = new TextBox();
            tbEOBDeniedAmt = new TextBox();
            rtbCheckSource = new RichTextBox();
            tbCheckAmt = new TextBox();
            lbChecks = new ListBox();
            msMain = new MenuStrip();
            tsddbImport = new ToolStripDropDownButton();
            cmsMedicare835Files = new ContextMenuStrip(components);
            tsmiImport = new ToolStripMenuItem();
            purgeInvalidFilesToolStripMenuItem = new ToolStripMenuItem();
            findAccountInFilesToolStripMenuItem = new ToolStripMenuItem();
            tsmiFileOpen = new ToolStripMenuItem();
            tsmiPostCheckRecords = new ToolStripMenuItem();
            tspPrintView = new ToolStripButton();
            tsmiPrint = new ToolStripMenuItem();
            tsmiEOBMenu = new ToolStripMenuItem();
            postEOBsToolStripMenuItem = new ToolStripMenuItem();
            printEOBsToolStripMenuItem = new ToolStripMenuItem();
            tsmiDisplayRecords = new ToolStripMenuItem();
            tsmiFirst20 = new ToolStripMenuItem();
            tsmiNext100 = new ToolStripMenuItem();
            tsmiPrevious100 = new ToolStripMenuItem();
            tsmiLast100 = new ToolStripMenuItem();
            tsmiFindAccount = new ToolStripMenuItem();
            tstbFindAccount = new ToolStripTextBox();
            tsmiFindSubscriber = new ToolStripMenuItem();
            tstbFindSubscriber = new ToolStripTextBox();
            openFileDialog = new OpenFileDialog();
            chkBindingSource = new BindingSource(components);
            scMain = new SplitContainer();
            niMain = new NotifyIcon(components);
            tstbFileSearch = new ToolStripTextBox();
            tsmiExtension = new ToolStripTextBox();
            ssMain.SuspendLayout();
            tc835.SuspendLayout();
            tpProcessed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvProcessed).BeginInit();
            tpDenieds.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvDenieds).BeginInit();
            tpNotProcessed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvNotProcessed).BeginInit();
            tpEOB.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvEOB).BeginInit();
            tlpTotals.SuspendLayout();
            msMain.SuspendLayout();
            cmsMedicare835Files.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)chkBindingSource).BeginInit();
            ((System.ComponentModel.ISupportInitialize)scMain).BeginInit();
            scMain.Panel1.SuspendLayout();
            scMain.Panel2.SuspendLayout();
            scMain.SuspendLayout();
            SuspendLayout();
            // 
            // ssMain
            // 
            ssMain.ImageScalingSize = new Size(20, 20);
            ssMain.Items.AddRange(new ToolStripItem[] { tspbRecords, tsslProgress, toolStripStatusLabel1, tsslProcessed, toolStripStatusLabel2, tsslDenieds, toolStripStatusLabel3, tsslNotProcessed, toolStripStatusLabel4, tsslEOB });
            ssMain.Location = new Point(0, 560);
            ssMain.Name = "ssMain";
            ssMain.Padding = new Padding(1, 0, 16, 0);
            ssMain.Size = new Size(1223, 24);
            ssMain.TabIndex = 1;
            ssMain.Text = "Working...";
            // 
            // tspbRecords
            // 
            tspbRecords.BackColor = Color.LightGray;
            tspbRecords.ForeColor = SystemColors.GrayText;
            tspbRecords.Name = "tspbRecords";
            tspbRecords.Size = new Size(583, 18);
            tspbRecords.Step = 1;
            tspbRecords.Value = 1;
            // 
            // tsslProgress
            // 
            tsslProgress.Name = "tsslProgress";
            tsslProgress.Size = new Size(0, 19);
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(22, 19);
            toolStripStatusLabel1.Text = "  |  ";
            // 
            // tsslProcessed
            // 
            tsslProcessed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsslProcessed.Name = "tsslProcessed";
            tsslProcessed.Size = new Size(72, 19);
            tsslProcessed.Text = "Processed: 0";
            // 
            // toolStripStatusLabel2
            // 
            toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            toolStripStatusLabel2.Size = new Size(16, 19);
            toolStripStatusLabel2.Text = " | ";
            // 
            // tsslDenieds
            // 
            tsslDenieds.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsslDenieds.Name = "tsslDenieds";
            tsslDenieds.Size = new Size(61, 19);
            tsslDenieds.Text = "Denieds: 0";
            // 
            // toolStripStatusLabel3
            // 
            toolStripStatusLabel3.DisplayStyle = ToolStripItemDisplayStyle.Text;
            toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            toolStripStatusLabel3.Size = new Size(16, 19);
            toolStripStatusLabel3.Text = " | ";
            // 
            // tsslNotProcessed
            // 
            tsslNotProcessed.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsslNotProcessed.Name = "tsslNotProcessed";
            tsslNotProcessed.Size = new Size(95, 19);
            tsslNotProcessed.Text = "Not Processed: 0";
            // 
            // toolStripStatusLabel4
            // 
            toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            toolStripStatusLabel4.Size = new Size(16, 19);
            toolStripStatusLabel4.Text = " | ";
            // 
            // tsslEOB
            // 
            tsslEOB.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsslEOB.Name = "tsslEOB";
            tsslEOB.Size = new Size(46, 19);
            tsslEOB.Text = "EOBs: 0";
            // 
            // tc835
            // 
            tc835.Controls.Add(tpProcessed);
            tc835.Controls.Add(tpDenieds);
            tc835.Controls.Add(tpNotProcessed);
            tc835.Controls.Add(tpEOB);
            tc835.Dock = DockStyle.Fill;
            tc835.Location = new Point(0, 0);
            tc835.Margin = new Padding(4, 3, 4, 3);
            tc835.Multiline = true;
            tc835.Name = "tc835";
            tc835.SelectedIndex = 0;
            tc835.Size = new Size(1223, 379);
            tc835.TabIndex = 3;
            tc835.SelectedIndexChanged += tcControl835_SelectedIndexChanged;
            // 
            // tpProcessed
            // 
            tpProcessed.Controls.Add(dgvProcessed);
            tpProcessed.Location = new Point(4, 24);
            tpProcessed.Margin = new Padding(4, 3, 4, 3);
            tpProcessed.Name = "tpProcessed";
            tpProcessed.Padding = new Padding(4, 3, 4, 3);
            tpProcessed.Size = new Size(1215, 351);
            tpProcessed.TabIndex = 0;
            tpProcessed.Text = "Processed";
            tpProcessed.UseVisualStyleBackColor = true;
            // 
            // dgvProcessed
            // 
            dgvProcessed.AllowUserToAddRows = false;
            dgvProcessed.AllowUserToDeleteRows = false;
            dgvProcessed.AllowUserToOrderColumns = true;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvProcessed.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvProcessed.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvProcessed.DefaultCellStyle = dataGridViewCellStyle2;
            dgvProcessed.Dock = DockStyle.Fill;
            dgvProcessed.Location = new Point(4, 3);
            dgvProcessed.Margin = new Padding(4, 3, 4, 3);
            dgvProcessed.Name = "dgvProcessed";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dgvProcessed.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvProcessed.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvProcessed.Size = new Size(1207, 345);
            dgvProcessed.TabIndex = 0;
            dgvProcessed.Tag = "decimal?[] dTotals";
            // 
            // tpDenieds
            // 
            tpDenieds.Controls.Add(dgvDenieds);
            tpDenieds.Location = new Point(4, 24);
            tpDenieds.Margin = new Padding(4, 3, 4, 3);
            tpDenieds.Name = "tpDenieds";
            tpDenieds.Padding = new Padding(4, 3, 4, 3);
            tpDenieds.Size = new Size(1215, 351);
            tpDenieds.TabIndex = 1;
            tpDenieds.Text = "Denieds";
            tpDenieds.UseVisualStyleBackColor = true;
            // 
            // dgvDenieds
            // 
            dgvDenieds.AllowUserToAddRows = false;
            dgvDenieds.AllowUserToDeleteRows = false;
            dgvDenieds.AllowUserToOrderColumns = true;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvDenieds.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvDenieds.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvDenieds.DefaultCellStyle = dataGridViewCellStyle5;
            dgvDenieds.Dock = DockStyle.Fill;
            dgvDenieds.Location = new Point(4, 3);
            dgvDenieds.Margin = new Padding(4, 3, 4, 3);
            dgvDenieds.Name = "dgvDenieds";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvDenieds.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvDenieds.Size = new Size(1207, 345);
            dgvDenieds.TabIndex = 0;
            dgvDenieds.Tag = "decimal?[] dTotals";
            // 
            // tpNotProcessed
            // 
            tpNotProcessed.Controls.Add(dgvNotProcessed);
            tpNotProcessed.Location = new Point(4, 24);
            tpNotProcessed.Margin = new Padding(4, 3, 4, 3);
            tpNotProcessed.Name = "tpNotProcessed";
            tpNotProcessed.Size = new Size(1215, 351);
            tpNotProcessed.TabIndex = 2;
            tpNotProcessed.Text = "Not Processed";
            tpNotProcessed.UseVisualStyleBackColor = true;
            // 
            // dgvNotProcessed
            // 
            dgvNotProcessed.AllowUserToAddRows = false;
            dgvNotProcessed.AllowUserToDeleteRows = false;
            dgvNotProcessed.AllowUserToOrderColumns = true;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            dgvNotProcessed.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            dgvNotProcessed.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            dgvNotProcessed.DefaultCellStyle = dataGridViewCellStyle8;
            dgvNotProcessed.Dock = DockStyle.Fill;
            dgvNotProcessed.Location = new Point(0, 0);
            dgvNotProcessed.Margin = new Padding(4, 3, 4, 3);
            dgvNotProcessed.Name = "dgvNotProcessed";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            dgvNotProcessed.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            dgvNotProcessed.Size = new Size(1215, 351);
            dgvNotProcessed.TabIndex = 0;
            dgvNotProcessed.Tag = "decimal?[] dTotals";
            // 
            // tpEOB
            // 
            tpEOB.Controls.Add(dgvEOB);
            tpEOB.Location = new Point(4, 24);
            tpEOB.Margin = new Padding(4, 3, 4, 3);
            tpEOB.Name = "tpEOB";
            tpEOB.Size = new Size(1215, 351);
            tpEOB.TabIndex = 3;
            tpEOB.Text = "EOBs";
            tpEOB.UseVisualStyleBackColor = true;
            // 
            // dgvEOB
            // 
            dgvEOB.AllowUserToAddRows = false;
            dgvEOB.AllowUserToDeleteRows = false;
            dgvEOB.AllowUserToOrderColumns = true;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Control;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            dgvEOB.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            dgvEOB.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle11.ForeColor = Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            dgvEOB.DefaultCellStyle = dataGridViewCellStyle11;
            dgvEOB.Dock = DockStyle.Fill;
            dgvEOB.Location = new Point(0, 0);
            dgvEOB.Margin = new Padding(4, 3, 4, 3);
            dgvEOB.Name = "dgvEOB";
            dgvEOB.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Control;
            dataGridViewCellStyle12.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            dgvEOB.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            dgvEOB.Size = new Size(1215, 351);
            dgvEOB.TabIndex = 0;
            dgvEOB.Tag = "decimal?[] dTotals";
            // 
            // tlpTotals
            // 
            tlpTotals.ColumnCount = 8;
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 99F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 99F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 99F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 99F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 117F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 146F));
            tlpTotals.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 292F));
            tlpTotals.Controls.Add(tbProviderID, 5, 1);
            tlpTotals.Controls.Add(tbCheckDate, 5, 2);
            tlpTotals.Controls.Add(tbFileDate, 5, 0);
            tlpTotals.Controls.Add(tbFileName, 6, 0);
            tlpTotals.Controls.Add(tbCheckNo, 6, 2);
            tlpTotals.Controls.Add(tbBatchNo, 5, 3);
            tlpTotals.Controls.Add(tbBillCycle, 6, 3);
            tlpTotals.Controls.Add(tbFileNumber, 4, 0);
            tlpTotals.Controls.Add(tbDatabase, 0, 0);
            tlpTotals.Controls.Add(tbChargeAmt, 0, 1);
            tlpTotals.Controls.Add(tbEOBChargeAmt, 0, 2);
            tlpTotals.Controls.Add(tbPaidAmt, 1, 1);
            tlpTotals.Controls.Add(tbEOBPaidAmt, 1, 2);
            tlpTotals.Controls.Add(tbContractualAmt, 2, 1);
            tlpTotals.Controls.Add(tbEOBContractualAmt, 2, 2);
            tlpTotals.Controls.Add(textBox1, 3, 1);
            tlpTotals.Controls.Add(tbEOBOtherAmt, 3, 2);
            tlpTotals.Controls.Add(tbDeniedAmt, 4, 1);
            tlpTotals.Controls.Add(tbEOBDeniedAmt, 4, 2);
            tlpTotals.Controls.Add(rtbCheckSource, 0, 3);
            tlpTotals.Controls.Add(tbCheckAmt, 6, 1);
            tlpTotals.Controls.Add(lbChecks, 7, 1);
            tlpTotals.Dock = DockStyle.Fill;
            tlpTotals.Location = new Point(0, 0);
            tlpTotals.Margin = new Padding(4, 3, 4, 3);
            tlpTotals.Name = "tlpTotals";
            tlpTotals.RowCount = 5;
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Percent, 33.33333F));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tlpTotals.RowStyles.Add(new RowStyle(SizeType.Absolute, 23F));
            tlpTotals.Size = new Size(1223, 150);
            tlpTotals.TabIndex = 2;
            // 
            // tbProviderID
            // 
            tbProviderID.BackColor = Color.WhiteSmoke;
            tbProviderID.Dock = DockStyle.Fill;
            tbProviderID.Location = new Point(517, 38);
            tbProviderID.Margin = new Padding(4, 3, 4, 3);
            tbProviderID.Name = "tbProviderID";
            tbProviderID.Size = new Size(138, 23);
            tbProviderID.TabIndex = 24;
            tbProviderID.Text = "Provider ID:";
            // 
            // tbCheckDate
            // 
            tbCheckDate.BackColor = Color.WhiteSmoke;
            tbCheckDate.Dock = DockStyle.Fill;
            tbCheckDate.Location = new Point(517, 68);
            tbCheckDate.Margin = new Padding(4, 3, 4, 3);
            tbCheckDate.Name = "tbCheckDate";
            tbCheckDate.Size = new Size(138, 23);
            tbCheckDate.TabIndex = 4;
            tbCheckDate.Text = "Check Date: ";
            // 
            // tbFileDate
            // 
            tbFileDate.BackColor = Color.WhiteSmoke;
            tbFileDate.Dock = DockStyle.Fill;
            tbFileDate.Location = new Point(517, 3);
            tbFileDate.Margin = new Padding(4, 3, 4, 3);
            tbFileDate.Name = "tbFileDate";
            tbFileDate.Size = new Size(138, 23);
            tbFileDate.TabIndex = 30;
            tbFileDate.Text = "File Date:";
            // 
            // tbFileName
            // 
            tbFileName.BackColor = Color.WhiteSmoke;
            tlpTotals.SetColumnSpan(tbFileName, 2);
            tbFileName.Dock = DockStyle.Fill;
            tbFileName.Location = new Point(663, 3);
            tbFileName.Margin = new Padding(4, 3, 4, 3);
            tbFileName.Name = "tbFileName";
            tbFileName.Size = new Size(556, 23);
            tbFileName.TabIndex = 3;
            tbFileName.Text = "File Name:";
            // 
            // tbCheckNo
            // 
            tbCheckNo.BackColor = Color.WhiteSmoke;
            tbCheckNo.Dock = DockStyle.Fill;
            tbCheckNo.Location = new Point(663, 68);
            tbCheckNo.Margin = new Padding(4, 3, 4, 3);
            tbCheckNo.Name = "tbCheckNo";
            tbCheckNo.Size = new Size(138, 23);
            tbCheckNo.TabIndex = 1;
            tbCheckNo.Text = "Ck No: ";
            // 
            // tbBatchNo
            // 
            tbBatchNo.BackColor = Color.WhiteSmoke;
            tbBatchNo.Dock = DockStyle.Fill;
            tbBatchNo.Location = new Point(517, 98);
            tbBatchNo.Margin = new Padding(4, 3, 4, 3);
            tbBatchNo.Name = "tbBatchNo";
            tbBatchNo.Size = new Size(138, 23);
            tbBatchNo.TabIndex = 26;
            tbBatchNo.Text = "Batch No:";
            // 
            // tbBillCycle
            // 
            tbBillCycle.BackColor = Color.WhiteSmoke;
            tbBillCycle.Dock = DockStyle.Fill;
            tbBillCycle.Location = new Point(663, 98);
            tbBillCycle.Margin = new Padding(4, 3, 4, 3);
            tbBillCycle.Name = "tbBillCycle";
            tbBillCycle.Size = new Size(138, 23);
            tbBillCycle.TabIndex = 28;
            tbBillCycle.Text = "Bill Cycle";
            // 
            // tbFileNumber
            // 
            tbFileNumber.BackColor = SystemColors.Control;
            tbFileNumber.Dock = DockStyle.Fill;
            tbFileNumber.Location = new Point(400, 3);
            tbFileNumber.Margin = new Padding(4, 3, 4, 3);
            tbFileNumber.Name = "tbFileNumber";
            tbFileNumber.Size = new Size(109, 23);
            tbFileNumber.TabIndex = 29;
            tbFileNumber.Text = "File Number";
            // 
            // tbDatabase
            // 
            tbDatabase.BackColor = Color.FromArgb(255, 255, 192);
            tbDatabase.CharacterCasing = CharacterCasing.Upper;
            tlpTotals.SetColumnSpan(tbDatabase, 3);
            tbDatabase.Dock = DockStyle.Fill;
            tbDatabase.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            tbDatabase.ForeColor = Color.FromArgb(0, 192, 0);
            tbDatabase.Location = new Point(4, 3);
            tbDatabase.Margin = new Padding(4, 3, 4, 3);
            tbDatabase.Name = "tbDatabase";
            tbDatabase.ReadOnly = true;
            tbDatabase.Size = new Size(289, 20);
            tbDatabase.TabIndex = 31;
            // 
            // tbChargeAmt
            // 
            tbChargeAmt.Dock = DockStyle.Fill;
            tbChargeAmt.Location = new Point(4, 38);
            tbChargeAmt.Margin = new Padding(4, 3, 4, 3);
            tbChargeAmt.Name = "tbChargeAmt";
            tbChargeAmt.ReadOnly = true;
            tbChargeAmt.Size = new Size(91, 23);
            tbChargeAmt.TabIndex = 8;
            tbChargeAmt.Text = "Charge Amt";
            // 
            // tbEOBChargeAmt
            // 
            tbEOBChargeAmt.Dock = DockStyle.Fill;
            tbEOBChargeAmt.Location = new Point(4, 68);
            tbEOBChargeAmt.Margin = new Padding(4, 3, 4, 3);
            tbEOBChargeAmt.Name = "tbEOBChargeAmt";
            tbEOBChargeAmt.ReadOnly = true;
            tbEOBChargeAmt.Size = new Size(91, 23);
            tbEOBChargeAmt.TabIndex = 12;
            tbEOBChargeAmt.TextAlign = HorizontalAlignment.Right;
            // 
            // tbPaidAmt
            // 
            tbPaidAmt.Dock = DockStyle.Fill;
            tbPaidAmt.Location = new Point(103, 38);
            tbPaidAmt.Margin = new Padding(4, 3, 4, 3);
            tbPaidAmt.Name = "tbPaidAmt";
            tbPaidAmt.ReadOnly = true;
            tbPaidAmt.Size = new Size(91, 23);
            tbPaidAmt.TabIndex = 9;
            tbPaidAmt.Text = "Paid Amt";
            // 
            // tbEOBPaidAmt
            // 
            tbEOBPaidAmt.Dock = DockStyle.Fill;
            tbEOBPaidAmt.Location = new Point(103, 68);
            tbEOBPaidAmt.Margin = new Padding(4, 3, 4, 3);
            tbEOBPaidAmt.Name = "tbEOBPaidAmt";
            tbEOBPaidAmt.ReadOnly = true;
            tbEOBPaidAmt.Size = new Size(91, 23);
            tbEOBPaidAmt.TabIndex = 15;
            tbEOBPaidAmt.TextAlign = HorizontalAlignment.Right;
            // 
            // tbContractualAmt
            // 
            tbContractualAmt.Dock = DockStyle.Fill;
            tbContractualAmt.Location = new Point(202, 38);
            tbContractualAmt.Margin = new Padding(4, 3, 4, 3);
            tbContractualAmt.Name = "tbContractualAmt";
            tbContractualAmt.ReadOnly = true;
            tbContractualAmt.Size = new Size(91, 23);
            tbContractualAmt.TabIndex = 10;
            tbContractualAmt.Text = "Contractual Amt";
            // 
            // tbEOBContractualAmt
            // 
            tbEOBContractualAmt.Dock = DockStyle.Fill;
            tbEOBContractualAmt.Location = new Point(202, 68);
            tbEOBContractualAmt.Margin = new Padding(4, 3, 4, 3);
            tbEOBContractualAmt.Name = "tbEOBContractualAmt";
            tbEOBContractualAmt.ReadOnly = true;
            tbEOBContractualAmt.Size = new Size(91, 23);
            tbEOBContractualAmt.TabIndex = 18;
            tbEOBContractualAmt.TextAlign = HorizontalAlignment.Right;
            // 
            // textBox1
            // 
            textBox1.BackColor = SystemColors.Control;
            textBox1.Dock = DockStyle.Fill;
            textBox1.Location = new Point(301, 38);
            textBox1.Margin = new Padding(4, 3, 4, 3);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(91, 23);
            textBox1.TabIndex = 32;
            textBox1.Text = "Other Amt";
            // 
            // tbEOBOtherAmt
            // 
            tbEOBOtherAmt.BackColor = SystemColors.Control;
            tbEOBOtherAmt.Dock = DockStyle.Fill;
            tbEOBOtherAmt.Location = new Point(301, 68);
            tbEOBOtherAmt.Margin = new Padding(4, 3, 4, 3);
            tbEOBOtherAmt.Name = "tbEOBOtherAmt";
            tbEOBOtherAmt.Size = new Size(91, 23);
            tbEOBOtherAmt.TabIndex = 33;
            tbEOBOtherAmt.TextAlign = HorizontalAlignment.Right;
            // 
            // tbDeniedAmt
            // 
            tbDeniedAmt.Dock = DockStyle.Fill;
            tbDeniedAmt.Location = new Point(400, 38);
            tbDeniedAmt.Margin = new Padding(4, 3, 4, 3);
            tbDeniedAmt.Name = "tbDeniedAmt";
            tbDeniedAmt.ReadOnly = true;
            tbDeniedAmt.Size = new Size(109, 23);
            tbDeniedAmt.TabIndex = 11;
            tbDeniedAmt.Text = "Denied Amt";
            // 
            // tbEOBDeniedAmt
            // 
            tbEOBDeniedAmt.Dock = DockStyle.Fill;
            tbEOBDeniedAmt.Location = new Point(400, 68);
            tbEOBDeniedAmt.Margin = new Padding(4, 3, 4, 3);
            tbEOBDeniedAmt.Name = "tbEOBDeniedAmt";
            tbEOBDeniedAmt.ReadOnly = true;
            tbEOBDeniedAmt.Size = new Size(109, 23);
            tbEOBDeniedAmt.TabIndex = 21;
            tbEOBDeniedAmt.TextAlign = HorizontalAlignment.Right;
            // 
            // rtbCheckSource
            // 
            tlpTotals.SetColumnSpan(rtbCheckSource, 5);
            rtbCheckSource.DetectUrls = false;
            rtbCheckSource.ForeColor = Color.Red;
            rtbCheckSource.Location = new Point(4, 98);
            rtbCheckSource.Margin = new Padding(4, 3, 4, 3);
            rtbCheckSource.Multiline = false;
            rtbCheckSource.Name = "rtbCheckSource";
            rtbCheckSource.ReadOnly = true;
            rtbCheckSource.Size = new Size(380, 20);
            rtbCheckSource.TabIndex = 25;
            rtbCheckSource.Text = "";
            // 
            // tbCheckAmt
            // 
            tbCheckAmt.BackColor = Color.WhiteSmoke;
            tbCheckAmt.Dock = DockStyle.Fill;
            tbCheckAmt.Location = new Point(663, 38);
            tbCheckAmt.Margin = new Padding(4, 3, 4, 3);
            tbCheckAmt.Name = "tbCheckAmt";
            tbCheckAmt.Size = new Size(138, 23);
            tbCheckAmt.TabIndex = 0;
            tbCheckAmt.Text = "Ck Amt: ";
            // 
            // lbChecks
            // 
            lbChecks.Dock = DockStyle.Fill;
            lbChecks.Font = new Font("Microsoft Sans Serif", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            lbChecks.FormattingEnabled = true;
            lbChecks.ItemHeight = 12;
            lbChecks.Location = new Point(809, 38);
            lbChecks.Margin = new Padding(4, 3, 4, 3);
            lbChecks.Name = "lbChecks";
            tlpTotals.SetRowSpan(lbChecks, 4);
            lbChecks.Size = new Size(410, 109);
            lbChecks.TabIndex = 34;
            // 
            // msMain
            // 
            msMain.ImageScalingSize = new Size(20, 20);
            msMain.Items.AddRange(new ToolStripItem[] { tsddbImport, tsmiFileOpen, tsmiPostCheckRecords, tspPrintView, tsmiPrint, tsmiEOBMenu, tsmiDisplayRecords });
            msMain.Location = new Point(0, 0);
            msMain.Name = "msMain";
            msMain.Padding = new Padding(7, 2, 0, 2);
            msMain.Size = new Size(1223, 26);
            msMain.TabIndex = 5;
            msMain.Text = "menuStrip";
            // 
            // tsddbImport
            // 
            tsddbImport.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tsddbImport.DropDown = cmsMedicare835Files;
            tsddbImport.Enabled = false;
            tsddbImport.Image = (Image)resources.GetObject("tsddbImport.Image");
            tsddbImport.ImageTransparentColor = Color.Magenta;
            tsddbImport.Name = "tsddbImport";
            tsddbImport.Size = new Size(134, 19);
            tsddbImport.Text = "Import Medicare Files";
            tsddbImport.Visible = false;
            // 
            // cmsMedicare835Files
            // 
            cmsMedicare835Files.ImageScalingSize = new Size(20, 20);
            cmsMedicare835Files.Items.AddRange(new ToolStripItem[] { tsmiImport, purgeInvalidFilesToolStripMenuItem, findAccountInFilesToolStripMenuItem });
            cmsMedicare835Files.Name = "cmsMedicare835Files";
            cmsMedicare835Files.OwnerItem = tsddbImport;
            cmsMedicare835Files.Size = new Size(185, 70);
            // 
            // tsmiImport
            // 
            tsmiImport.Enabled = false;
            tsmiImport.Name = "tsmiImport";
            tsmiImport.Size = new Size(184, 22);
            tsmiImport.Text = "Import Files";
            tsmiImport.Visible = false;
            tsmiImport.Click += ImportMedicareFilesFromWTH;
            // 
            // purgeInvalidFilesToolStripMenuItem
            // 
            purgeInvalidFilesToolStripMenuItem.Enabled = false;
            purgeInvalidFilesToolStripMenuItem.Name = "purgeInvalidFilesToolStripMenuItem";
            purgeInvalidFilesToolStripMenuItem.Size = new Size(184, 22);
            purgeInvalidFilesToolStripMenuItem.Text = "Purge Invalid Files";
            purgeInvalidFilesToolStripMenuItem.Click += purgeInvalidFilesToolStripMenuItem_Click;
            // 
            // findAccountInFilesToolStripMenuItem
            // 
            findAccountInFilesToolStripMenuItem.Enabled = false;
            findAccountInFilesToolStripMenuItem.Name = "findAccountInFilesToolStripMenuItem";
            findAccountInFilesToolStripMenuItem.Size = new Size(184, 22);
            findAccountInFilesToolStripMenuItem.Text = "Find Account in Files";
            findAccountInFilesToolStripMenuItem.Visible = false;
            findAccountInFilesToolStripMenuItem.Click += FindAccountInFilesToolStripMenuItem_Click;
            // 
            // tsmiFileOpen
            // 
            tsmiFileOpen.Name = "tsmiFileOpen";
            tsmiFileOpen.Size = new Size(69, 22);
            tsmiFileOpen.Text = "Open File";
            tsmiFileOpen.Click += TsmiFileOpen_Click;
            // 
            // tsmiPostCheckRecords
            // 
            tsmiPostCheckRecords.Name = "tsmiPostCheckRecords";
            tsmiPostCheckRecords.Size = new Size(123, 22);
            tsmiPostCheckRecords.Text = "Post Check Records";
            tsmiPostCheckRecords.Click += tsmiPostCheckRecords_Click;
            // 
            // tspPrintView
            // 
            tspPrintView.DisplayStyle = ToolStripItemDisplayStyle.Text;
            tspPrintView.Image = (Image)resources.GetObject("tspPrintView.Image");
            tspPrintView.ImageTransparentColor = Color.Magenta;
            tspPrintView.Name = "tspPrintView";
            tspPrintView.Size = new Size(64, 19);
            tspPrintView.Text = "Print View";
            tspPrintView.ToolTipText = "Prints this View as a bitmap";
            tspPrintView.Click += tsbPrintView_Click;
            // 
            // tsmiPrint
            // 
            tsmiPrint.Name = "tsmiPrint";
            tsmiPrint.Size = new Size(116, 22);
            tsmiPrint.Text = "Print Selected Grid";
            tsmiPrint.Click += tsmiPrint_Click;
            // 
            // tsmiEOBMenu
            // 
            tsmiEOBMenu.DropDownItems.AddRange(new ToolStripItem[] { postEOBsToolStripMenuItem, printEOBsToolStripMenuItem });
            tsmiEOBMenu.Name = "tsmiEOBMenu";
            tsmiEOBMenu.Size = new Size(49, 22);
            tsmiEOBMenu.Text = "EOB's";
            // 
            // postEOBsToolStripMenuItem
            // 
            postEOBsToolStripMenuItem.CheckOnClick = true;
            postEOBsToolStripMenuItem.Name = "postEOBsToolStripMenuItem";
            postEOBsToolStripMenuItem.Size = new Size(132, 22);
            postEOBsToolStripMenuItem.Text = "Post EOB's";
            postEOBsToolStripMenuItem.Click += tsmiEOB_Click;
            // 
            // printEOBsToolStripMenuItem
            // 
            printEOBsToolStripMenuItem.CheckOnClick = true;
            printEOBsToolStripMenuItem.Name = "printEOBsToolStripMenuItem";
            printEOBsToolStripMenuItem.Size = new Size(132, 22);
            printEOBsToolStripMenuItem.Text = "Print EOB's";
            printEOBsToolStripMenuItem.Click += tsbPrintEOB_Click;
            // 
            // tsmiDisplayRecords
            // 
            tsmiDisplayRecords.DropDownItems.AddRange(new ToolStripItem[] { tsmiFirst20, tsmiNext100, tsmiPrevious100, tsmiLast100, tsmiFindAccount, tsmiFindSubscriber });
            tsmiDisplayRecords.Name = "tsmiDisplayRecords";
            tsmiDisplayRecords.Size = new Size(117, 22);
            tsmiDisplayRecords.Text = "Load Selected Grid";
            // 
            // tsmiFirst20
            // 
            tsmiFirst20.Name = "tsmiFirst20";
            tsmiFirst20.Size = new Size(180, 22);
            tsmiFirst20.Text = "First 20";
            tsmiFirst20.Click += tsmiFirst20_Click;
            // 
            // tsmiNext100
            // 
            tsmiNext100.Name = "tsmiNext100";
            tsmiNext100.Size = new Size(180, 22);
            tsmiNext100.Text = "Next 20";
            tsmiNext100.Click += tsmiNext20_Click;
            // 
            // tsmiPrevious100
            // 
            tsmiPrevious100.Name = "tsmiPrevious100";
            tsmiPrevious100.Size = new Size(180, 22);
            tsmiPrevious100.Text = "Previous 20";
            tsmiPrevious100.Click += tsmiPrevious20_Click;
            // 
            // tsmiLast100
            // 
            tsmiLast100.Name = "tsmiLast100";
            tsmiLast100.Size = new Size(180, 22);
            tsmiLast100.Text = "Last 20";
            tsmiLast100.Click += tsmiLast20_Click;
            // 
            // tsmiFindAccount
            // 
            tsmiFindAccount.DropDownItems.AddRange(new ToolStripItem[] { tstbFindAccount });
            tsmiFindAccount.Name = "tsmiFindAccount";
            tsmiFindAccount.Size = new Size(180, 22);
            tsmiFindAccount.Text = "Find Account";
            tsmiFindAccount.Click += FindAccountInFilesToolStripMenuItem_Click;
            // 
            // tstbFindAccount
            // 
            tstbFindAccount.AcceptsReturn = true;
            tstbFindAccount.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tstbFindAccount.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tstbFindAccount.CharacterCasing = CharacterCasing.Upper;
            tstbFindAccount.Name = "tstbFindAccount";
            tstbFindAccount.Size = new Size(100, 23);
            // 
            // tsmiFindSubscriber
            // 
            tsmiFindSubscriber.DropDownItems.AddRange(new ToolStripItem[] { tstbFindSubscriber });
            tsmiFindSubscriber.Enabled = false;
            tsmiFindSubscriber.Name = "tsmiFindSubscriber";
            tsmiFindSubscriber.Size = new Size(180, 22);
            tsmiFindSubscriber.Text = "Find Subscriber";
            // 
            // tstbFindSubscriber
            // 
            tstbFindSubscriber.AutoCompleteMode = AutoCompleteMode.SuggestAppend;
            tstbFindSubscriber.AutoCompleteSource = AutoCompleteSource.CustomSource;
            tstbFindSubscriber.Name = "tstbFindSubscriber";
            tstbFindSubscriber.Size = new Size(100, 23);
            // 
            // chkBindingSource
            // 
            chkBindingSource.DataMember = "chk";
            // 
            // scMain
            // 
            scMain.Dock = DockStyle.Fill;
            scMain.FixedPanel = FixedPanel.Panel1;
            scMain.IsSplitterFixed = true;
            scMain.Location = new Point(0, 26);
            scMain.Margin = new Padding(4, 3, 4, 3);
            scMain.Name = "scMain";
            scMain.Orientation = Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            scMain.Panel1.Controls.Add(tlpTotals);
            // 
            // scMain.Panel2
            // 
            scMain.Panel2.Controls.Add(tc835);
            scMain.Size = new Size(1223, 534);
            scMain.SplitterDistance = 150;
            scMain.SplitterWidth = 5;
            scMain.TabIndex = 6;
            // 
            // niMain
            // 
            niMain.BalloonTipIcon = ToolTipIcon.Info;
            niMain.BalloonTipTitle = "Messages from Posting835Remittance";
            niMain.Text = "Posting835Remittance";
            niMain.Visible = true;
            // 
            // tstbFileSearch
            // 
            tstbFileSearch.Name = "tstbFileSearch";
            tstbFileSearch.Size = new Size(100, 21);
            // 
            // tsmiExtension
            // 
            tsmiExtension.Name = "tsmiExtension";
            tsmiExtension.Size = new Size(160, 21);
            tsmiExtension.Text = "*.*";
            tsmiExtension.ToolTipText = "Add the extension of the file type to search for.";
            // 
            // Posting835
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1223, 584);
            Controls.Add(scMain);
            Controls.Add(msMain);
            Controls.Add(ssMain);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "Posting835";
            Text = "Posting835Remittance";
            WindowState = FormWindowState.Maximized;
            Load += Posting835_Load;
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            tc835.ResumeLayout(false);
            tpProcessed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvProcessed).EndInit();
            tpDenieds.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvDenieds).EndInit();
            tpNotProcessed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvNotProcessed).EndInit();
            tpEOB.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)dgvEOB).EndInit();
            tlpTotals.ResumeLayout(false);
            tlpTotals.PerformLayout();
            msMain.ResumeLayout(false);
            msMain.PerformLayout();
            cmsMedicare835Files.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)chkBindingSource).EndInit();
            scMain.Panel1.ResumeLayout(false);
            scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)scMain).EndInit();
            scMain.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.MenuStrip msMain;
        private System.Windows.Forms.ToolStripMenuItem tsmiFileOpen;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrint;
        private System.Windows.Forms.ToolStripMenuItem tsmiPostCheckRecords;
        private System.Windows.Forms.TabControl tc835;
        private System.Windows.Forms.TabPage tpProcessed;
        private System.Windows.Forms.DataGridView dgvProcessed;
        private System.Windows.Forms.TabPage tpDenieds;
        private System.Windows.Forms.DataGridView dgvDenieds;
        private System.Windows.Forms.TabPage tpNotProcessed;
        private System.Windows.Forms.DataGridView dgvNotProcessed;
        private System.Windows.Forms.TabPage tpEOB;
        private System.Windows.Forms.DataGridView dgvEOB;
        private System.Windows.Forms.TableLayoutPanel tlpTotals;
        private System.Windows.Forms.TextBox tbCheckAmt;
        private System.Windows.Forms.TextBox tbCheckDate;
        private System.Windows.Forms.TextBox tbCheckNo;
        private System.Windows.Forms.TextBox tbChargeAmt;
        private System.Windows.Forms.TextBox tbPaidAmt;
        private System.Windows.Forms.TextBox tbContractualAmt;
        private System.Windows.Forms.TextBox tbDeniedAmt;
        private System.Windows.Forms.TextBox tbEOBChargeAmt;
        private System.Windows.Forms.TextBox tbEOBPaidAmt;
        private System.Windows.Forms.TextBox tbEOBContractualAmt;
        private System.Windows.Forms.TextBox tbEOBDeniedAmt;
        private System.Windows.Forms.TextBox tbFileName;
        private System.Windows.Forms.TextBox tbProviderID;
        private System.Windows.Forms.RichTextBox rtbCheckSource;
        private System.Windows.Forms.TextBox tbBatchNo;
        private System.Windows.Forms.TextBox tbBillCycle;
        private System.Windows.Forms.TextBox tbFileNumber;
        private System.Windows.Forms.ToolStripDropDownButton tsddbImport;
        private System.Windows.Forms.ToolStripButton tspPrintView;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.BindingSource chkBindingSource;
        private System.Windows.Forms.ContextMenuStrip cmsMedicare835Files;
        private System.Windows.Forms.ToolStripMenuItem tsmiImport;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.TextBox tbFileDate;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.NotifyIcon niMain;
        private System.Windows.Forms.TextBox tbDatabase;
        private System.Windows.Forms.ToolStripStatusLabel tsslProgress;
        private System.Windows.Forms.ToolStripMenuItem tsmiDisplayRecords;
        private System.Windows.Forms.ToolStripMenuItem tsmiFindAccount;
        private System.Windows.Forms.ToolStripTextBox tstbFindAccount;
        private System.Windows.Forms.ToolStripStatusLabel tsslProcessed;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsslDenieds;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tsslNotProcessed;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsslEOB;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox tbEOBOtherAmt;
        private System.Windows.Forms.ToolStripMenuItem tsmiFindSubscriber;
        private System.Windows.Forms.ToolStripTextBox tstbFindSubscriber;
        private System.Windows.Forms.ToolStripMenuItem tsmiFirst20;
        private System.Windows.Forms.ToolStripMenuItem tsmiNext100;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrevious100;
        private System.Windows.Forms.ToolStripMenuItem tsmiLast100;
        private System.Windows.Forms.ToolStripMenuItem tsmiEOBMenu;
        private System.Windows.Forms.ToolStripTextBox tstbFileSearch;
        private System.Windows.Forms.ToolStripTextBox tsmiExtension;
        private System.Windows.Forms.ToolStripMenuItem postEOBsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printEOBsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purgeInvalidFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAccountInFilesToolStripMenuItem;
        private System.Windows.Forms.ListBox lbChecks;
    }
}

