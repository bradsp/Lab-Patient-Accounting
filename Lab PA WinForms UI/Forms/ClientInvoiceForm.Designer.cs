
namespace LabBilling.Forms
{
    partial class ClientInvoiceForm
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
            InvoicesDGV = new System.Windows.Forms.DataGridView();
            GenerateInvoicesBtn = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            PreviousMonth = new System.Windows.Forms.RadioButton();
            CurrentMonth = new System.Windows.Forms.RadioButton();
            label2 = new System.Windows.Forms.Label();
            TotalUnbilledCharges = new System.Windows.Forms.TextBox();
            UnbilledAccountsDGV = new System.Windows.Forms.DataGridView();
            SelectionProfile = new System.Windows.Forms.ComboBox();
            label3 = new System.Windows.Forms.Label();
            InvoiceHistoryTabControl = new System.Windows.Forms.TabControl();
            GenerateInvoicesTabPage = new System.Windows.Forms.TabPage();
            refreshUnbilledInvoices = new System.Windows.Forms.PictureBox();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            InvoiceHistoryTabPage = new System.Windows.Forms.TabPage();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            invoiceTextBox = new System.Windows.Forms.TextBox();
            invoiceLabel = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ThroughDate = new System.Windows.Forms.MaskedTextBox();
            FromDate = new System.Windows.Forms.MaskedTextBox();
            label4 = new System.Windows.Forms.Label();
            ClientFilter = new System.Windows.Forms.ComboBox();
            generateStatementButton = new System.Windows.Forms.Button();
            ViewInvoice = new System.Windows.Forms.Button();
            PrintInvoiceButton = new System.Windows.Forms.Button();
            InvoiceHistoryDGV = new System.Windows.Forms.DataGridView();
            contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(components);
            undoInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            printAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            saveAllToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)InvoicesDGV).BeginInit();
            ((System.ComponentModel.ISupportInitialize)UnbilledAccountsDGV).BeginInit();
            InvoiceHistoryTabControl.SuspendLayout();
            GenerateInvoicesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)refreshUnbilledInvoices).BeginInit();
            statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            InvoiceHistoryTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)InvoiceHistoryDGV).BeginInit();
            contextMenuStrip1.SuspendLayout();
            printContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // InvoicesDGV
            // 
            InvoicesDGV.AllowUserToAddRows = false;
            InvoicesDGV.AllowUserToDeleteRows = false;
            InvoicesDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            InvoicesDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            InvoicesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            InvoicesDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            InvoicesDGV.Location = new System.Drawing.Point(0, 0);
            InvoicesDGV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InvoicesDGV.Name = "InvoicesDGV";
            InvoicesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            InvoicesDGV.Size = new System.Drawing.Size(1139, 324);
            InvoicesDGV.TabIndex = 0;
            InvoicesDGV.SelectionChanged += InvoicesDGV_SelectionChanged;
            // 
            // GenerateInvoicesBtn
            // 
            GenerateInvoicesBtn.Location = new System.Drawing.Point(761, 7);
            GenerateInvoicesBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GenerateInvoicesBtn.Name = "GenerateInvoicesBtn";
            GenerateInvoicesBtn.Size = new System.Drawing.Size(134, 27);
            GenerateInvoicesBtn.TabIndex = 1;
            GenerateInvoicesBtn.Text = "Generate Invoices";
            GenerateInvoicesBtn.UseVisualStyleBackColor = true;
            GenerateInvoicesBtn.Click += GenerateInvoicesBtn_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(12, 12);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(128, 15);
            label1.TabIndex = 6;
            label1.Text = "Show unbilled through";
            // 
            // PreviousMonth
            // 
            PreviousMonth.AutoSize = true;
            PreviousMonth.Checked = true;
            PreviousMonth.Location = new System.Drawing.Point(149, 9);
            PreviousMonth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PreviousMonth.Name = "PreviousMonth";
            PreviousMonth.Size = new System.Drawing.Size(109, 19);
            PreviousMonth.TabIndex = 7;
            PreviousMonth.TabStop = true;
            PreviousMonth.Text = "Previous Month";
            PreviousMonth.UseVisualStyleBackColor = true;
            PreviousMonth.CheckedChanged += ThruDate_CheckedChanged;
            // 
            // CurrentMonth
            // 
            CurrentMonth.AutoSize = true;
            CurrentMonth.Location = new System.Drawing.Point(272, 9);
            CurrentMonth.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CurrentMonth.Name = "CurrentMonth";
            CurrentMonth.Size = new System.Drawing.Size(104, 19);
            CurrentMonth.TabIndex = 8;
            CurrentMonth.Text = "Current Month";
            CurrentMonth.UseVisualStyleBackColor = true;
            CurrentMonth.CheckedChanged += ThruDate_CheckedChanged;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(902, 7);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(125, 15);
            label2.TabIndex = 9;
            label2.Text = "Total Unbilled Charges";
            // 
            // TotalUnbilledCharges
            // 
            TotalUnbilledCharges.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TotalUnbilledCharges.Location = new System.Drawing.Point(1042, 8);
            TotalUnbilledCharges.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TotalUnbilledCharges.Name = "TotalUnbilledCharges";
            TotalUnbilledCharges.Size = new System.Drawing.Size(111, 23);
            TotalUnbilledCharges.TabIndex = 10;
            // 
            // UnbilledAccountsDGV
            // 
            UnbilledAccountsDGV.AllowUserToAddRows = false;
            UnbilledAccountsDGV.AllowUserToDeleteRows = false;
            UnbilledAccountsDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            UnbilledAccountsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            UnbilledAccountsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            UnbilledAccountsDGV.Location = new System.Drawing.Point(0, 0);
            UnbilledAccountsDGV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            UnbilledAccountsDGV.Name = "UnbilledAccountsDGV";
            UnbilledAccountsDGV.ReadOnly = true;
            UnbilledAccountsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            UnbilledAccountsDGV.Size = new System.Drawing.Size(1139, 187);
            UnbilledAccountsDGV.TabIndex = 11;
            UnbilledAccountsDGV.CellMouseDoubleClick += UnbilledAccountsDGV_CellMouseDoubleClick;
            UnbilledAccountsDGV.RowHeaderMouseDoubleClick += UnbilledAccountsDGV_RowHeaderMouseDoubleClick;
            // 
            // SelectionProfile
            // 
            SelectionProfile.FormattingEnabled = true;
            SelectionProfile.Items.AddRange(new object[] { "None", "All Except Nursing Homes", "Nursing Homes" });
            SelectionProfile.Location = new System.Drawing.Point(551, 8);
            SelectionProfile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelectionProfile.Name = "SelectionProfile";
            SelectionProfile.Size = new System.Drawing.Size(202, 23);
            SelectionProfile.TabIndex = 12;
            SelectionProfile.SelectedIndexChanged += SelectionProfile_SelectedIndexChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(447, 12);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(92, 15);
            label3.TabIndex = 13;
            label3.Text = "Selection Profile";
            // 
            // InvoiceHistoryTabControl
            // 
            InvoiceHistoryTabControl.Controls.Add(GenerateInvoicesTabPage);
            InvoiceHistoryTabControl.Controls.Add(InvoiceHistoryTabPage);
            InvoiceHistoryTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            InvoiceHistoryTabControl.Location = new System.Drawing.Point(0, 0);
            InvoiceHistoryTabControl.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InvoiceHistoryTabControl.Name = "InvoiceHistoryTabControl";
            InvoiceHistoryTabControl.SelectedIndex = 0;
            InvoiceHistoryTabControl.Size = new System.Drawing.Size(1177, 640);
            InvoiceHistoryTabControl.TabIndex = 15;
            // 
            // GenerateInvoicesTabPage
            // 
            GenerateInvoicesTabPage.Controls.Add(refreshUnbilledInvoices);
            GenerateInvoicesTabPage.Controls.Add(statusStrip1);
            GenerateInvoicesTabPage.Controls.Add(splitContainer1);
            GenerateInvoicesTabPage.Controls.Add(label3);
            GenerateInvoicesTabPage.Controls.Add(SelectionProfile);
            GenerateInvoicesTabPage.Controls.Add(CurrentMonth);
            GenerateInvoicesTabPage.Controls.Add(PreviousMonth);
            GenerateInvoicesTabPage.Controls.Add(label1);
            GenerateInvoicesTabPage.Controls.Add(GenerateInvoicesBtn);
            GenerateInvoicesTabPage.Controls.Add(TotalUnbilledCharges);
            GenerateInvoicesTabPage.Controls.Add(label2);
            GenerateInvoicesTabPage.Location = new System.Drawing.Point(4, 24);
            GenerateInvoicesTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GenerateInvoicesTabPage.Name = "GenerateInvoicesTabPage";
            GenerateInvoicesTabPage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GenerateInvoicesTabPage.Size = new System.Drawing.Size(1169, 612);
            GenerateInvoicesTabPage.TabIndex = 0;
            GenerateInvoicesTabPage.Text = "Generate Invoices";
            GenerateInvoicesTabPage.UseVisualStyleBackColor = true;
            // 
            // refreshUnbilledInvoices
            // 
            refreshUnbilledInvoices.Image = Properties.Resources.refresh_icon;
            refreshUnbilledInvoices.Location = new System.Drawing.Point(386, 8);
            refreshUnbilledInvoices.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            refreshUnbilledInvoices.Name = "refreshUnbilledInvoices";
            refreshUnbilledInvoices.Size = new System.Drawing.Size(24, 23);
            refreshUnbilledInvoices.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            refreshUnbilledInvoices.TabIndex = 18;
            refreshUnbilledInvoices.TabStop = false;
            refreshUnbilledInvoices.Click += refreshUnbilledInvoices_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(4, 587);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(1161, 22);
            statusStrip1.TabIndex = 17;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(16, 17);
            toolStripStatusLabel1.Text = "...";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(15, 51);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(InvoicesDGV);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(UnbilledAccountsDGV);
            splitContainer1.Size = new System.Drawing.Size(1139, 516);
            splitContainer1.SplitterDistance = 324;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 16;
            // 
            // InvoiceHistoryTabPage
            // 
            InvoiceHistoryTabPage.Controls.Add(progressBar1);
            InvoiceHistoryTabPage.Controls.Add(invoiceTextBox);
            InvoiceHistoryTabPage.Controls.Add(invoiceLabel);
            InvoiceHistoryTabPage.Controls.Add(label6);
            InvoiceHistoryTabPage.Controls.Add(label5);
            InvoiceHistoryTabPage.Controls.Add(ThroughDate);
            InvoiceHistoryTabPage.Controls.Add(FromDate);
            InvoiceHistoryTabPage.Controls.Add(label4);
            InvoiceHistoryTabPage.Controls.Add(ClientFilter);
            InvoiceHistoryTabPage.Controls.Add(generateStatementButton);
            InvoiceHistoryTabPage.Controls.Add(ViewInvoice);
            InvoiceHistoryTabPage.Controls.Add(PrintInvoiceButton);
            InvoiceHistoryTabPage.Controls.Add(InvoiceHistoryDGV);
            InvoiceHistoryTabPage.Location = new System.Drawing.Point(4, 24);
            InvoiceHistoryTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InvoiceHistoryTabPage.Name = "InvoiceHistoryTabPage";
            InvoiceHistoryTabPage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InvoiceHistoryTabPage.Size = new System.Drawing.Size(1169, 612);
            InvoiceHistoryTabPage.TabIndex = 1;
            InvoiceHistoryTabPage.Text = "Invoice History";
            InvoiceHistoryTabPage.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            progressBar1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            progressBar1.Location = new System.Drawing.Point(9, 535);
            progressBar1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(1041, 27);
            progressBar1.TabIndex = 10;
            // 
            // invoiceTextBox
            // 
            invoiceTextBox.Location = new System.Drawing.Point(458, 20);
            invoiceTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            invoiceTextBox.Name = "invoiceTextBox";
            invoiceTextBox.Size = new System.Drawing.Size(154, 23);
            invoiceTextBox.TabIndex = 9;
            invoiceTextBox.TextChanged += invoiceTextBox_TextChanged;
            // 
            // invoiceLabel
            // 
            invoiceLabel.AutoSize = true;
            invoiceLabel.Location = new System.Drawing.Point(402, 22);
            invoiceLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            invoiceLabel.Name = "invoiceLabel";
            invoiceLabel.Size = new System.Drawing.Size(45, 15);
            invoiceLabel.TabIndex = 8;
            invoiceLabel.Text = "Invoice";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(876, 23);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(50, 15);
            label6.TabIndex = 7;
            label6.Text = "through";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(632, 22);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(99, 15);
            label5.TabIndex = 6;
            label5.Text = "From Create Date";
            // 
            // ThroughDate
            // 
            ThroughDate.Location = new System.Drawing.Point(933, 20);
            ThroughDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ThroughDate.Mask = "00/00/0000";
            ThroughDate.Name = "ThroughDate";
            ThroughDate.Size = new System.Drawing.Size(116, 23);
            ThroughDate.TabIndex = 5;
            ThroughDate.ValidatingType = typeof(System.DateTime);
            ThroughDate.TextChanged += ThroughDate_TextChanged;
            // 
            // FromDate
            // 
            FromDate.Location = new System.Drawing.Point(744, 20);
            FromDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FromDate.Mask = "00/00/0000";
            FromDate.Name = "FromDate";
            FromDate.Size = new System.Drawing.Size(116, 23);
            FromDate.TabIndex = 5;
            FromDate.ValidatingType = typeof(System.DateTime);
            FromDate.TextChanged += FromDate_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(10, 23);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(83, 15);
            label4.TabIndex = 4;
            label4.Text = "Filter by Client";
            // 
            // ClientFilter
            // 
            ClientFilter.FormattingEnabled = true;
            ClientFilter.Location = new System.Drawing.Point(102, 20);
            ClientFilter.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ClientFilter.Name = "ClientFilter";
            ClientFilter.Size = new System.Drawing.Size(293, 23);
            ClientFilter.TabIndex = 3;
            ClientFilter.SelectedIndexChanged += ClientFilter_SelectedIndexChanged;
            // 
            // generateStatementButton
            // 
            generateStatementButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            generateStatementButton.Location = new System.Drawing.Point(1057, 99);
            generateStatementButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            generateStatementButton.Name = "generateStatementButton";
            generateStatementButton.Size = new System.Drawing.Size(102, 51);
            generateStatementButton.TabIndex = 2;
            generateStatementButton.Text = "Generate Statement";
            generateStatementButton.UseVisualStyleBackColor = true;
            generateStatementButton.Click += generateStatementButton_Click;
            // 
            // ViewInvoice
            // 
            ViewInvoice.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ViewInvoice.Location = new System.Drawing.Point(1057, 54);
            ViewInvoice.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ViewInvoice.Name = "ViewInvoice";
            ViewInvoice.Size = new System.Drawing.Size(102, 38);
            ViewInvoice.TabIndex = 2;
            ViewInvoice.Text = "View Invoice";
            ViewInvoice.UseVisualStyleBackColor = true;
            ViewInvoice.Click += ViewInvoice_Click;
            // 
            // PrintInvoiceButton
            // 
            PrintInvoiceButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            PrintInvoiceButton.Location = new System.Drawing.Point(1057, 157);
            PrintInvoiceButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PrintInvoiceButton.Name = "PrintInvoiceButton";
            PrintInvoiceButton.Size = new System.Drawing.Size(102, 48);
            PrintInvoiceButton.TabIndex = 1;
            PrintInvoiceButton.Text = "Print Invoices";
            PrintInvoiceButton.UseVisualStyleBackColor = true;
            PrintInvoiceButton.Click += PrintInvoice_Click_1;
            // 
            // InvoiceHistoryDGV
            // 
            InvoiceHistoryDGV.AllowUserToAddRows = false;
            InvoiceHistoryDGV.AllowUserToDeleteRows = false;
            InvoiceHistoryDGV.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            InvoiceHistoryDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            InvoiceHistoryDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            InvoiceHistoryDGV.ContextMenuStrip = contextMenuStrip1;
            InvoiceHistoryDGV.Location = new System.Drawing.Point(9, 54);
            InvoiceHistoryDGV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            InvoiceHistoryDGV.Name = "InvoiceHistoryDGV";
            InvoiceHistoryDGV.ReadOnly = true;
            InvoiceHistoryDGV.Size = new System.Drawing.Size(1041, 462);
            InvoiceHistoryDGV.TabIndex = 0;
            InvoiceHistoryDGV.CellMouseDoubleClick += InvoiceHistoryDGV_CellMouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { undoInvoiceToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new System.Drawing.Size(145, 26);
            // 
            // undoInvoiceToolStripMenuItem
            // 
            undoInvoiceToolStripMenuItem.Name = "undoInvoiceToolStripMenuItem";
            undoInvoiceToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            undoInvoiceToolStripMenuItem.Text = "Undo Invoice";
            undoInvoiceToolStripMenuItem.Click += undoInvoiceToolStripMenuItem_Click;
            // 
            // printContextMenu
            // 
            printContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { printToolStripMenuItem, printAllToolStripMenuItem, saveToPDFToolStripMenuItem, saveAllToPDFToolStripMenuItem });
            printContextMenu.Name = "printContextMenu";
            printContextMenu.Size = new System.Drawing.Size(184, 92);
            // 
            // printToolStripMenuItem
            // 
            printToolStripMenuItem.Name = "printToolStripMenuItem";
            printToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            printToolStripMenuItem.Text = "Print Selected";
            printToolStripMenuItem.Click += PrintInvoice_Click;
            // 
            // printAllToolStripMenuItem
            // 
            printAllToolStripMenuItem.Name = "printAllToolStripMenuItem";
            printAllToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            printAllToolStripMenuItem.Text = "Print All";
            printAllToolStripMenuItem.Click += printAllToolStripMenuItem_Click;
            // 
            // saveToPDFToolStripMenuItem
            // 
            saveToPDFToolStripMenuItem.Name = "saveToPDFToolStripMenuItem";
            saveToPDFToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            saveToPDFToolStripMenuItem.Text = "Save Selected to PDF";
            saveToPDFToolStripMenuItem.Click += PrintInvoice_Click;
            // 
            // saveAllToPDFToolStripMenuItem
            // 
            saveAllToPDFToolStripMenuItem.Name = "saveAllToPDFToolStripMenuItem";
            saveAllToPDFToolStripMenuItem.Size = new System.Drawing.Size(183, 22);
            saveAllToPDFToolStripMenuItem.Text = "Save All to PDF";
            saveAllToPDFToolStripMenuItem.Click += saveAllToPDFToolStripMenuItem_Click;
            // 
            // ClientInvoiceForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1177, 640);
            Controls.Add(InvoiceHistoryTabControl);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "ClientInvoiceForm";
            Text = "Client Invoicing";
            Load += ClientInvoiceForm_Load;
            ((System.ComponentModel.ISupportInitialize)InvoicesDGV).EndInit();
            ((System.ComponentModel.ISupportInitialize)UnbilledAccountsDGV).EndInit();
            InvoiceHistoryTabControl.ResumeLayout(false);
            GenerateInvoicesTabPage.ResumeLayout(false);
            GenerateInvoicesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)refreshUnbilledInvoices).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            InvoiceHistoryTabPage.ResumeLayout(false);
            InvoiceHistoryTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)InvoiceHistoryDGV).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            printContextMenu.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView InvoicesDGV;
        private System.Windows.Forms.Button GenerateInvoicesBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton PreviousMonth;
        private System.Windows.Forms.RadioButton CurrentMonth;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TotalUnbilledCharges;
        private System.Windows.Forms.DataGridView UnbilledAccountsDGV;
        private System.Windows.Forms.ComboBox SelectionProfile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TabControl InvoiceHistoryTabControl;
        private System.Windows.Forms.TabPage GenerateInvoicesTabPage;
        private System.Windows.Forms.TabPage InvoiceHistoryTabPage;
        private System.Windows.Forms.DataGridView InvoiceHistoryDGV;
        private System.Windows.Forms.Button ViewInvoice;
        private System.Windows.Forms.Button PrintInvoiceButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox ClientFilter;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.MaskedTextBox ThroughDate;
        private System.Windows.Forms.MaskedTextBox FromDate;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem undoInvoiceToolStripMenuItem;
        private System.Windows.Forms.Button generateStatementButton;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox refreshUnbilledInvoices;
        private System.Windows.Forms.ContextMenuStrip printContextMenu;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToPDFToolStripMenuItem;
        private System.Windows.Forms.TextBox invoiceTextBox;
        private System.Windows.Forms.Label invoiceLabel;
        private System.Windows.Forms.ToolStripMenuItem saveAllToPDFToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem printAllToolStripMenuItem;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}