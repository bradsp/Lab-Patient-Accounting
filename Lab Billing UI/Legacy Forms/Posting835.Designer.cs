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
            providerIdTextBox = new TextBox();
            checkDateTextBox = new TextBox();
            fileDateTextBox = new TextBox();
            fileNameTextBox = new TextBox();
            checkNoTextBox = new TextBox();
            batchNoTextBox = new TextBox();
            billCycleTextBox = new TextBox();
            fileNumberTextBox = new TextBox();
            databaseTextBox = new TextBox();
            chargeAmountTextBox = new TextBox();
            eobChargeAmountTextBox = new TextBox();
            paidAmountTextBox = new TextBox();
            eobPaidAmountTextBox = new TextBox();
            contractualAmountTextBox = new TextBox();
            eobContractualAmountTextBox = new TextBox();
            otherAmountTextBox = new TextBox();
            eobOtherAmountTextBox = new TextBox();
            deniedAmountTextBox = new TextBox();
            eobDeniedAmountTextBox = new TextBox();
            rtbCheckSource = new RichTextBox();
            checkAmountTextBox = new TextBox();
            checksLabel = new ListBox();
            mainToolStripMenu = new MenuStrip();
            tsddbImport = new ToolStripDropDownButton();
            cmsMedicare835Files = new ContextMenuStrip(components);
            tsmiImport = new ToolStripMenuItem();
            purgeInvalidFilesToolStripMenuItem = new ToolStripMenuItem();
            findAccountInFilesToolStripMenuItem = new ToolStripMenuItem();
            openFileToolStripItem = new ToolStripMenuItem();
            postCheckRecordsToolStripItem = new ToolStripMenuItem();
            printViewToolStripItem = new ToolStripButton();
            printGridToolStripItem = new ToolStripMenuItem();
            eobToolStripMenu = new ToolStripMenuItem();
            postEOBsToolStripMenuItem = new ToolStripMenuItem();
            printEOBsToolStripMenuItem = new ToolStripMenuItem();
            loadGridToolStripMenu = new ToolStripMenuItem();
            loadFirstToolStripItem = new ToolStripMenuItem();
            loadNextToolStripItem = new ToolStripMenuItem();
            loadPreviousToolStripItem = new ToolStripMenuItem();
            loadLastToolStripItem = new ToolStripMenuItem();
            findAccountToolStripItem = new ToolStripMenuItem();
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
            mainToolStripMenu.SuspendLayout();
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
            tlpTotals.Controls.Add(providerIdTextBox, 5, 1);
            tlpTotals.Controls.Add(checkDateTextBox, 5, 2);
            tlpTotals.Controls.Add(fileDateTextBox, 5, 0);
            tlpTotals.Controls.Add(fileNameTextBox, 6, 0);
            tlpTotals.Controls.Add(checkNoTextBox, 6, 2);
            tlpTotals.Controls.Add(batchNoTextBox, 5, 3);
            tlpTotals.Controls.Add(billCycleTextBox, 6, 3);
            tlpTotals.Controls.Add(fileNumberTextBox, 4, 0);
            tlpTotals.Controls.Add(databaseTextBox, 0, 0);
            tlpTotals.Controls.Add(chargeAmountTextBox, 0, 1);
            tlpTotals.Controls.Add(eobChargeAmountTextBox, 0, 2);
            tlpTotals.Controls.Add(paidAmountTextBox, 1, 1);
            tlpTotals.Controls.Add(eobPaidAmountTextBox, 1, 2);
            tlpTotals.Controls.Add(contractualAmountTextBox, 2, 1);
            tlpTotals.Controls.Add(eobContractualAmountTextBox, 2, 2);
            tlpTotals.Controls.Add(otherAmountTextBox, 3, 1);
            tlpTotals.Controls.Add(eobOtherAmountTextBox, 3, 2);
            tlpTotals.Controls.Add(deniedAmountTextBox, 4, 1);
            tlpTotals.Controls.Add(eobDeniedAmountTextBox, 4, 2);
            tlpTotals.Controls.Add(rtbCheckSource, 0, 3);
            tlpTotals.Controls.Add(checkAmountTextBox, 6, 1);
            tlpTotals.Controls.Add(checksLabel, 7, 1);
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
            // providerIdTextBox
            // 
            providerIdTextBox.BackColor = Color.WhiteSmoke;
            providerIdTextBox.Dock = DockStyle.Fill;
            providerIdTextBox.Location = new Point(517, 38);
            providerIdTextBox.Margin = new Padding(4, 3, 4, 3);
            providerIdTextBox.Name = "providerIdTextBox";
            providerIdTextBox.Size = new Size(138, 23);
            providerIdTextBox.TabIndex = 24;
            providerIdTextBox.Text = "Provider ID:";
            // 
            // checkDateTextBox
            // 
            checkDateTextBox.BackColor = Color.WhiteSmoke;
            checkDateTextBox.Dock = DockStyle.Fill;
            checkDateTextBox.Location = new Point(517, 68);
            checkDateTextBox.Margin = new Padding(4, 3, 4, 3);
            checkDateTextBox.Name = "checkDateTextBox";
            checkDateTextBox.Size = new Size(138, 23);
            checkDateTextBox.TabIndex = 4;
            checkDateTextBox.Text = "Check Date: ";
            // 
            // fileDateTextBox
            // 
            fileDateTextBox.BackColor = Color.WhiteSmoke;
            fileDateTextBox.Dock = DockStyle.Fill;
            fileDateTextBox.Location = new Point(517, 3);
            fileDateTextBox.Margin = new Padding(4, 3, 4, 3);
            fileDateTextBox.Name = "fileDateTextBox";
            fileDateTextBox.Size = new Size(138, 23);
            fileDateTextBox.TabIndex = 30;
            fileDateTextBox.Text = "File Date:";
            // 
            // fileNameTextBox
            // 
            fileNameTextBox.BackColor = Color.WhiteSmoke;
            tlpTotals.SetColumnSpan(fileNameTextBox, 2);
            fileNameTextBox.Dock = DockStyle.Fill;
            fileNameTextBox.Location = new Point(663, 3);
            fileNameTextBox.Margin = new Padding(4, 3, 4, 3);
            fileNameTextBox.Name = "fileNameTextBox";
            fileNameTextBox.Size = new Size(556, 23);
            fileNameTextBox.TabIndex = 3;
            fileNameTextBox.Text = "File Name:";
            // 
            // checkNoTextBox
            // 
            checkNoTextBox.BackColor = Color.WhiteSmoke;
            checkNoTextBox.Dock = DockStyle.Fill;
            checkNoTextBox.Location = new Point(663, 68);
            checkNoTextBox.Margin = new Padding(4, 3, 4, 3);
            checkNoTextBox.Name = "checkNoTextBox";
            checkNoTextBox.Size = new Size(138, 23);
            checkNoTextBox.TabIndex = 1;
            checkNoTextBox.Text = "Ck No: ";
            // 
            // batchNoTextBox
            // 
            batchNoTextBox.BackColor = Color.WhiteSmoke;
            batchNoTextBox.Dock = DockStyle.Fill;
            batchNoTextBox.Location = new Point(517, 98);
            batchNoTextBox.Margin = new Padding(4, 3, 4, 3);
            batchNoTextBox.Name = "batchNoTextBox";
            batchNoTextBox.Size = new Size(138, 23);
            batchNoTextBox.TabIndex = 26;
            batchNoTextBox.Text = "Batch No:";
            // 
            // billCycleTextBox
            // 
            billCycleTextBox.BackColor = Color.WhiteSmoke;
            billCycleTextBox.Dock = DockStyle.Fill;
            billCycleTextBox.Location = new Point(663, 98);
            billCycleTextBox.Margin = new Padding(4, 3, 4, 3);
            billCycleTextBox.Name = "billCycleTextBox";
            billCycleTextBox.Size = new Size(138, 23);
            billCycleTextBox.TabIndex = 28;
            billCycleTextBox.Text = "Bill Cycle";
            // 
            // fileNumberTextBox
            // 
            fileNumberTextBox.BackColor = SystemColors.Control;
            fileNumberTextBox.Dock = DockStyle.Fill;
            fileNumberTextBox.Location = new Point(400, 3);
            fileNumberTextBox.Margin = new Padding(4, 3, 4, 3);
            fileNumberTextBox.Name = "fileNumberTextBox";
            fileNumberTextBox.Size = new Size(109, 23);
            fileNumberTextBox.TabIndex = 29;
            fileNumberTextBox.Text = "File Number";
            // 
            // databaseTextBox
            // 
            databaseTextBox.BackColor = Color.FromArgb(255, 255, 192);
            databaseTextBox.CharacterCasing = CharacterCasing.Upper;
            tlpTotals.SetColumnSpan(databaseTextBox, 3);
            databaseTextBox.Dock = DockStyle.Fill;
            databaseTextBox.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            databaseTextBox.ForeColor = Color.FromArgb(0, 192, 0);
            databaseTextBox.Location = new Point(4, 3);
            databaseTextBox.Margin = new Padding(4, 3, 4, 3);
            databaseTextBox.Name = "databaseTextBox";
            databaseTextBox.ReadOnly = true;
            databaseTextBox.Size = new Size(289, 20);
            databaseTextBox.TabIndex = 31;
            // 
            // chargeAmountTextBox
            // 
            chargeAmountTextBox.Dock = DockStyle.Fill;
            chargeAmountTextBox.Location = new Point(4, 38);
            chargeAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            chargeAmountTextBox.Name = "chargeAmountTextBox";
            chargeAmountTextBox.ReadOnly = true;
            chargeAmountTextBox.Size = new Size(91, 23);
            chargeAmountTextBox.TabIndex = 8;
            chargeAmountTextBox.Text = "Charge Amt";
            // 
            // eobChargeAmountTextBox
            // 
            eobChargeAmountTextBox.Dock = DockStyle.Fill;
            eobChargeAmountTextBox.Location = new Point(4, 68);
            eobChargeAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            eobChargeAmountTextBox.Name = "eobChargeAmountTextBox";
            eobChargeAmountTextBox.ReadOnly = true;
            eobChargeAmountTextBox.Size = new Size(91, 23);
            eobChargeAmountTextBox.TabIndex = 12;
            eobChargeAmountTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // paidAmountTextBox
            // 
            paidAmountTextBox.Dock = DockStyle.Fill;
            paidAmountTextBox.Location = new Point(103, 38);
            paidAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            paidAmountTextBox.Name = "paidAmountTextBox";
            paidAmountTextBox.ReadOnly = true;
            paidAmountTextBox.Size = new Size(91, 23);
            paidAmountTextBox.TabIndex = 9;
            paidAmountTextBox.Text = "Paid Amt";
            // 
            // eobPaidAmountTextBox
            // 
            eobPaidAmountTextBox.Dock = DockStyle.Fill;
            eobPaidAmountTextBox.Location = new Point(103, 68);
            eobPaidAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            eobPaidAmountTextBox.Name = "eobPaidAmountTextBox";
            eobPaidAmountTextBox.ReadOnly = true;
            eobPaidAmountTextBox.Size = new Size(91, 23);
            eobPaidAmountTextBox.TabIndex = 15;
            eobPaidAmountTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // contractualAmountTextBox
            // 
            contractualAmountTextBox.Dock = DockStyle.Fill;
            contractualAmountTextBox.Location = new Point(202, 38);
            contractualAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            contractualAmountTextBox.Name = "contractualAmountTextBox";
            contractualAmountTextBox.ReadOnly = true;
            contractualAmountTextBox.Size = new Size(91, 23);
            contractualAmountTextBox.TabIndex = 10;
            contractualAmountTextBox.Text = "Contractual Amt";
            // 
            // eobContractualAmountTextBox
            // 
            eobContractualAmountTextBox.Dock = DockStyle.Fill;
            eobContractualAmountTextBox.Location = new Point(202, 68);
            eobContractualAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            eobContractualAmountTextBox.Name = "eobContractualAmountTextBox";
            eobContractualAmountTextBox.ReadOnly = true;
            eobContractualAmountTextBox.Size = new Size(91, 23);
            eobContractualAmountTextBox.TabIndex = 18;
            eobContractualAmountTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // otherAmountTextBox
            // 
            otherAmountTextBox.BackColor = SystemColors.Control;
            otherAmountTextBox.Dock = DockStyle.Fill;
            otherAmountTextBox.Location = new Point(301, 38);
            otherAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            otherAmountTextBox.Name = "otherAmountTextBox";
            otherAmountTextBox.Size = new Size(91, 23);
            otherAmountTextBox.TabIndex = 32;
            otherAmountTextBox.Text = "Other Amt";
            // 
            // eobOtherAmountTextBox
            // 
            eobOtherAmountTextBox.BackColor = SystemColors.Control;
            eobOtherAmountTextBox.Dock = DockStyle.Fill;
            eobOtherAmountTextBox.Location = new Point(301, 68);
            eobOtherAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            eobOtherAmountTextBox.Name = "eobOtherAmountTextBox";
            eobOtherAmountTextBox.Size = new Size(91, 23);
            eobOtherAmountTextBox.TabIndex = 33;
            eobOtherAmountTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // deniedAmountTextBox
            // 
            deniedAmountTextBox.Dock = DockStyle.Fill;
            deniedAmountTextBox.Location = new Point(400, 38);
            deniedAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            deniedAmountTextBox.Name = "deniedAmountTextBox";
            deniedAmountTextBox.ReadOnly = true;
            deniedAmountTextBox.Size = new Size(109, 23);
            deniedAmountTextBox.TabIndex = 11;
            deniedAmountTextBox.Text = "Denied Amt";
            // 
            // eobDeniedAmountTextBox
            // 
            eobDeniedAmountTextBox.Dock = DockStyle.Fill;
            eobDeniedAmountTextBox.Location = new Point(400, 68);
            eobDeniedAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            eobDeniedAmountTextBox.Name = "eobDeniedAmountTextBox";
            eobDeniedAmountTextBox.ReadOnly = true;
            eobDeniedAmountTextBox.Size = new Size(109, 23);
            eobDeniedAmountTextBox.TabIndex = 21;
            eobDeniedAmountTextBox.TextAlign = HorizontalAlignment.Right;
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
            // checkAmountTextBox
            // 
            checkAmountTextBox.BackColor = Color.WhiteSmoke;
            checkAmountTextBox.Dock = DockStyle.Fill;
            checkAmountTextBox.Location = new Point(663, 38);
            checkAmountTextBox.Margin = new Padding(4, 3, 4, 3);
            checkAmountTextBox.Name = "checkAmountTextBox";
            checkAmountTextBox.Size = new Size(138, 23);
            checkAmountTextBox.TabIndex = 0;
            checkAmountTextBox.Text = "Ck Amt: ";
            // 
            // checksLabel
            // 
            checksLabel.Dock = DockStyle.Fill;
            checksLabel.Font = new Font("Microsoft Sans Serif", 6.75F, FontStyle.Regular, GraphicsUnit.Point, 0);
            checksLabel.FormattingEnabled = true;
            checksLabel.ItemHeight = 12;
            checksLabel.Location = new Point(809, 38);
            checksLabel.Margin = new Padding(4, 3, 4, 3);
            checksLabel.Name = "checksLabel";
            tlpTotals.SetRowSpan(checksLabel, 4);
            checksLabel.Size = new Size(410, 109);
            checksLabel.TabIndex = 34;
            // 
            // mainToolStripMenu
            // 
            mainToolStripMenu.ImageScalingSize = new Size(20, 20);
            mainToolStripMenu.Items.AddRange(new ToolStripItem[] { tsddbImport, openFileToolStripItem, postCheckRecordsToolStripItem, printViewToolStripItem, printGridToolStripItem, eobToolStripMenu, loadGridToolStripMenu });
            mainToolStripMenu.Location = new Point(0, 0);
            mainToolStripMenu.Name = "mainToolStripMenu";
            mainToolStripMenu.Padding = new Padding(7, 2, 0, 2);
            mainToolStripMenu.Size = new Size(1223, 26);
            mainToolStripMenu.TabIndex = 5;
            mainToolStripMenu.Text = "menuStrip";
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
            // openFileToolStripItem
            // 
            openFileToolStripItem.Name = "openFileToolStripItem";
            openFileToolStripItem.Size = new Size(69, 22);
            openFileToolStripItem.Text = "Open File";
            openFileToolStripItem.Click += fileOpenToolStripItem_Click;
            // 
            // postCheckRecordsToolStripItem
            // 
            postCheckRecordsToolStripItem.Name = "postCheckRecordsToolStripItem";
            postCheckRecordsToolStripItem.Size = new Size(123, 22);
            postCheckRecordsToolStripItem.Text = "Post Check Records";
            postCheckRecordsToolStripItem.Click += postCheckRecordsToolStripItem_Click;
            // 
            // printViewToolStripItem
            // 
            printViewToolStripItem.DisplayStyle = ToolStripItemDisplayStyle.Text;
            printViewToolStripItem.Image = (Image)resources.GetObject("printViewToolStripItem.Image");
            printViewToolStripItem.ImageTransparentColor = Color.Magenta;
            printViewToolStripItem.Name = "printViewToolStripItem";
            printViewToolStripItem.Size = new Size(64, 19);
            printViewToolStripItem.Text = "Print View";
            printViewToolStripItem.ToolTipText = "Prints this View as a bitmap";
            printViewToolStripItem.Click += printViewToolStripItem_Click;
            // 
            // printGridToolStripItem
            // 
            printGridToolStripItem.Name = "printGridToolStripItem";
            printGridToolStripItem.Size = new Size(116, 22);
            printGridToolStripItem.Text = "Print Selected Grid";
            printGridToolStripItem.Click += printToolStripMenuItem_Click;
            // 
            // eobToolStripMenu
            // 
            eobToolStripMenu.DropDownItems.AddRange(new ToolStripItem[] { postEOBsToolStripMenuItem, printEOBsToolStripMenuItem });
            eobToolStripMenu.Name = "eobToolStripMenu";
            eobToolStripMenu.Size = new Size(49, 22);
            eobToolStripMenu.Text = "EOB's";
            // 
            // postEOBsToolStripMenuItem
            // 
            postEOBsToolStripMenuItem.CheckOnClick = true;
            postEOBsToolStripMenuItem.Name = "postEOBsToolStripMenuItem";
            postEOBsToolStripMenuItem.Size = new Size(132, 22);
            postEOBsToolStripMenuItem.Text = "Post EOB's";
            postEOBsToolStripMenuItem.Click += eobToolStripMenuItem_Click;
            // 
            // printEOBsToolStripMenuItem
            // 
            printEOBsToolStripMenuItem.CheckOnClick = true;
            printEOBsToolStripMenuItem.Name = "printEOBsToolStripMenuItem";
            printEOBsToolStripMenuItem.Size = new Size(132, 22);
            printEOBsToolStripMenuItem.Text = "Print EOB's";
            printEOBsToolStripMenuItem.Click += printEOBToolStripItem_Click;
            // 
            // loadGridToolStripMenu
            // 
            loadGridToolStripMenu.DropDownItems.AddRange(new ToolStripItem[] { loadFirstToolStripItem, loadNextToolStripItem, loadPreviousToolStripItem, loadLastToolStripItem, findAccountToolStripItem, tsmiFindSubscriber });
            loadGridToolStripMenu.Name = "loadGridToolStripMenu";
            loadGridToolStripMenu.Size = new Size(117, 22);
            loadGridToolStripMenu.Text = "Load Selected Grid";
            // 
            // loadFirstToolStripItem
            // 
            loadFirstToolStripItem.Name = "loadFirstToolStripItem";
            loadFirstToolStripItem.Size = new Size(155, 22);
            loadFirstToolStripItem.Text = "First 20";
            loadFirstToolStripItem.Click += loadFirstToolStripItem_Click;
            // 
            // loadNextToolStripItem
            // 
            loadNextToolStripItem.Name = "loadNextToolStripItem";
            loadNextToolStripItem.Size = new Size(155, 22);
            loadNextToolStripItem.Text = "Next 20";
            loadNextToolStripItem.Click += loadNextToolStripItem_Click;
            // 
            // loadPreviousToolStripItem
            // 
            loadPreviousToolStripItem.Name = "loadPreviousToolStripItem";
            loadPreviousToolStripItem.Size = new Size(155, 22);
            loadPreviousToolStripItem.Text = "Previous 20";
            loadPreviousToolStripItem.Click += loadPreviousToolStripItem_Click;
            // 
            // loadLastToolStripItem
            // 
            loadLastToolStripItem.Name = "loadLastToolStripItem";
            loadLastToolStripItem.Size = new Size(155, 22);
            loadLastToolStripItem.Text = "Last 20";
            loadLastToolStripItem.Click += loadLastToolStripItem_Click;
            // 
            // findAccountToolStripItem
            // 
            findAccountToolStripItem.DropDownItems.AddRange(new ToolStripItem[] { tstbFindAccount });
            findAccountToolStripItem.Name = "findAccountToolStripItem";
            findAccountToolStripItem.Size = new Size(155, 22);
            findAccountToolStripItem.Text = "Find Account";
            findAccountToolStripItem.Click += FindAccountInFilesToolStripMenuItem_Click;
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
            tsmiFindSubscriber.Size = new Size(155, 22);
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
            Controls.Add(mainToolStripMenu);
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
            mainToolStripMenu.ResumeLayout(false);
            mainToolStripMenu.PerformLayout();
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
        private System.Windows.Forms.MenuStrip mainToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem openFileToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem printGridToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem postCheckRecordsToolStripItem;
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
        private System.Windows.Forms.TextBox checkAmountTextBox;
        private System.Windows.Forms.TextBox checkDateTextBox;
        private System.Windows.Forms.TextBox checkNoTextBox;
        private System.Windows.Forms.TextBox chargeAmountTextBox;
        private System.Windows.Forms.TextBox paidAmountTextBox;
        private System.Windows.Forms.TextBox contractualAmountTextBox;
        private System.Windows.Forms.TextBox deniedAmountTextBox;
        private System.Windows.Forms.TextBox eobChargeAmountTextBox;
        private System.Windows.Forms.TextBox eobPaidAmountTextBox;
        private System.Windows.Forms.TextBox eobContractualAmountTextBox;
        private System.Windows.Forms.TextBox eobDeniedAmountTextBox;
        private System.Windows.Forms.TextBox fileNameTextBox;
        private System.Windows.Forms.TextBox providerIdTextBox;
        private System.Windows.Forms.RichTextBox rtbCheckSource;
        private System.Windows.Forms.TextBox batchNoTextBox;
        private System.Windows.Forms.TextBox billCycleTextBox;
        private System.Windows.Forms.TextBox fileNumberTextBox;
        private System.Windows.Forms.ToolStripDropDownButton tsddbImport;
        private System.Windows.Forms.ToolStripButton printViewToolStripItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.BindingSource chkBindingSource;
        private System.Windows.Forms.ContextMenuStrip cmsMedicare835Files;
        private System.Windows.Forms.ToolStripMenuItem tsmiImport;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.TextBox fileDateTextBox;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.NotifyIcon niMain;
        private System.Windows.Forms.TextBox databaseTextBox;
        private System.Windows.Forms.ToolStripStatusLabel tsslProgress;
        private System.Windows.Forms.ToolStripMenuItem loadGridToolStripMenu;
        private System.Windows.Forms.ToolStripMenuItem findAccountToolStripItem;
        private System.Windows.Forms.ToolStripTextBox tstbFindAccount;
        private System.Windows.Forms.ToolStripStatusLabel tsslProcessed;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel tsslDenieds;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel tsslNotProcessed;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel tsslEOB;
        private System.Windows.Forms.TextBox otherAmountTextBox;
        private System.Windows.Forms.TextBox eobOtherAmountTextBox;
        private System.Windows.Forms.ToolStripMenuItem tsmiFindSubscriber;
        private System.Windows.Forms.ToolStripTextBox tstbFindSubscriber;
        private System.Windows.Forms.ToolStripMenuItem loadFirstToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem loadNextToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem loadPreviousToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem loadLastToolStripItem;
        private System.Windows.Forms.ToolStripMenuItem eobToolStripMenu;
        private System.Windows.Forms.ToolStripTextBox tstbFileSearch;
        private System.Windows.Forms.ToolStripTextBox tsmiExtension;
        private System.Windows.Forms.ToolStripMenuItem postEOBsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printEOBsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem purgeInvalidFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findAccountInFilesToolStripMenuItem;
        private System.Windows.Forms.ListBox checksLabel;
    }
}

