
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
            this.components = new System.ComponentModel.Container();
            this.InvoicesDGV = new System.Windows.Forms.DataGridView();
            this.GenerateInvoicesBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.PreviousMonth = new System.Windows.Forms.RadioButton();
            this.CurrentMonth = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.TotalUnbilledCharges = new System.Windows.Forms.TextBox();
            this.UnbilledAccountsDGV = new System.Windows.Forms.DataGridView();
            this.SelectionProfile = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.InvoiceHistoryTabControl = new System.Windows.Forms.TabControl();
            this.GenerateInvoicesTabPage = new System.Windows.Forms.TabPage();
            this.refreshUnbilledInvoices = new System.Windows.Forms.PictureBox();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.InvoiceHistoryTabPage = new System.Windows.Forms.TabPage();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ThroughDate = new System.Windows.Forms.MaskedTextBox();
            this.FromDate = new System.Windows.Forms.MaskedTextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.ClientFilter = new System.Windows.Forms.ComboBox();
            this.generateStatementButton = new System.Windows.Forms.Button();
            this.ViewInvoice = new System.Windows.Forms.Button();
            this.PrintInvoiceButton = new System.Windows.Forms.Button();
            this.InvoiceHistoryDGV = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.undoInvoiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.printContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToPDFToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.invoiceLabel = new System.Windows.Forms.Label();
            this.invoiceTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.InvoicesDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnbilledAccountsDGV)).BeginInit();
            this.InvoiceHistoryTabControl.SuspendLayout();
            this.GenerateInvoicesTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refreshUnbilledInvoices)).BeginInit();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.InvoiceHistoryTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceHistoryDGV)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.printContextMenu.SuspendLayout();
            this.SuspendLayout();
            // 
            // InvoicesDGV
            // 
            this.InvoicesDGV.AllowUserToAddRows = false;
            this.InvoicesDGV.AllowUserToDeleteRows = false;
            this.InvoicesDGV.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.InvoicesDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.InvoicesDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InvoicesDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InvoicesDGV.Location = new System.Drawing.Point(0, 0);
            this.InvoicesDGV.Name = "InvoicesDGV";
            this.InvoicesDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.InvoicesDGV.Size = new System.Drawing.Size(976, 281);
            this.InvoicesDGV.TabIndex = 0;
            this.InvoicesDGV.SelectionChanged += new System.EventHandler(this.InvoicesDGV_SelectionChanged);
            // 
            // GenerateInvoicesBtn
            // 
            this.GenerateInvoicesBtn.Location = new System.Drawing.Point(652, 6);
            this.GenerateInvoicesBtn.Name = "GenerateInvoicesBtn";
            this.GenerateInvoicesBtn.Size = new System.Drawing.Size(115, 23);
            this.GenerateInvoicesBtn.TabIndex = 1;
            this.GenerateInvoicesBtn.Text = "Generate Invoices";
            this.GenerateInvoicesBtn.UseVisualStyleBackColor = true;
            this.GenerateInvoicesBtn.Click += new System.EventHandler(this.GenerateInvoicesBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(112, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Show unbilled through";
            // 
            // PreviousMonth
            // 
            this.PreviousMonth.AutoSize = true;
            this.PreviousMonth.Checked = true;
            this.PreviousMonth.Location = new System.Drawing.Point(128, 8);
            this.PreviousMonth.Name = "PreviousMonth";
            this.PreviousMonth.Size = new System.Drawing.Size(99, 17);
            this.PreviousMonth.TabIndex = 7;
            this.PreviousMonth.TabStop = true;
            this.PreviousMonth.Text = "Previous Month";
            this.PreviousMonth.UseVisualStyleBackColor = true;
            this.PreviousMonth.CheckedChanged += new System.EventHandler(this.ThruDate_CheckedChanged);
            // 
            // CurrentMonth
            // 
            this.CurrentMonth.AutoSize = true;
            this.CurrentMonth.Location = new System.Drawing.Point(233, 8);
            this.CurrentMonth.Name = "CurrentMonth";
            this.CurrentMonth.Size = new System.Drawing.Size(92, 17);
            this.CurrentMonth.TabIndex = 8;
            this.CurrentMonth.Text = "Current Month";
            this.CurrentMonth.UseVisualStyleBackColor = true;
            this.CurrentMonth.CheckedChanged += new System.EventHandler(this.ThruDate_CheckedChanged);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(773, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Total Unbilled Charges";
            // 
            // TotalUnbilledCharges
            // 
            this.TotalUnbilledCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.TotalUnbilledCharges.Location = new System.Drawing.Point(893, 7);
            this.TotalUnbilledCharges.Name = "TotalUnbilledCharges";
            this.TotalUnbilledCharges.Size = new System.Drawing.Size(96, 20);
            this.TotalUnbilledCharges.TabIndex = 10;
            // 
            // UnbilledAccountsDGV
            // 
            this.UnbilledAccountsDGV.AllowUserToAddRows = false;
            this.UnbilledAccountsDGV.AllowUserToDeleteRows = false;
            this.UnbilledAccountsDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.UnbilledAccountsDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UnbilledAccountsDGV.Dock = System.Windows.Forms.DockStyle.Fill;
            this.UnbilledAccountsDGV.Location = new System.Drawing.Point(0, 0);
            this.UnbilledAccountsDGV.Name = "UnbilledAccountsDGV";
            this.UnbilledAccountsDGV.ReadOnly = true;
            this.UnbilledAccountsDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UnbilledAccountsDGV.Size = new System.Drawing.Size(976, 162);
            this.UnbilledAccountsDGV.TabIndex = 11;
            this.UnbilledAccountsDGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.UnbilledAccountsDGV_CellMouseDoubleClick);
            this.UnbilledAccountsDGV.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.UnbilledAccountsDGV_RowHeaderMouseDoubleClick);
            // 
            // SelectionProfile
            // 
            this.SelectionProfile.FormattingEnabled = true;
            this.SelectionProfile.Items.AddRange(new object[] {
            "None",
            "All Except Nursing Homes",
            "Nursing Homes"});
            this.SelectionProfile.Location = new System.Drawing.Point(472, 7);
            this.SelectionProfile.Name = "SelectionProfile";
            this.SelectionProfile.Size = new System.Drawing.Size(174, 21);
            this.SelectionProfile.TabIndex = 12;
            this.SelectionProfile.SelectedIndexChanged += new System.EventHandler(this.SelectionProfile_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(383, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Selection Profile";
            // 
            // InvoiceHistoryTabControl
            // 
            this.InvoiceHistoryTabControl.Controls.Add(this.GenerateInvoicesTabPage);
            this.InvoiceHistoryTabControl.Controls.Add(this.InvoiceHistoryTabPage);
            this.InvoiceHistoryTabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.InvoiceHistoryTabControl.Location = new System.Drawing.Point(0, 0);
            this.InvoiceHistoryTabControl.Name = "InvoiceHistoryTabControl";
            this.InvoiceHistoryTabControl.SelectedIndex = 0;
            this.InvoiceHistoryTabControl.Size = new System.Drawing.Size(1009, 555);
            this.InvoiceHistoryTabControl.TabIndex = 15;
            // 
            // GenerateInvoicesTabPage
            // 
            this.GenerateInvoicesTabPage.Controls.Add(this.refreshUnbilledInvoices);
            this.GenerateInvoicesTabPage.Controls.Add(this.statusStrip1);
            this.GenerateInvoicesTabPage.Controls.Add(this.splitContainer1);
            this.GenerateInvoicesTabPage.Controls.Add(this.label3);
            this.GenerateInvoicesTabPage.Controls.Add(this.SelectionProfile);
            this.GenerateInvoicesTabPage.Controls.Add(this.CurrentMonth);
            this.GenerateInvoicesTabPage.Controls.Add(this.PreviousMonth);
            this.GenerateInvoicesTabPage.Controls.Add(this.label1);
            this.GenerateInvoicesTabPage.Controls.Add(this.GenerateInvoicesBtn);
            this.GenerateInvoicesTabPage.Controls.Add(this.TotalUnbilledCharges);
            this.GenerateInvoicesTabPage.Controls.Add(this.label2);
            this.GenerateInvoicesTabPage.Location = new System.Drawing.Point(4, 22);
            this.GenerateInvoicesTabPage.Name = "GenerateInvoicesTabPage";
            this.GenerateInvoicesTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.GenerateInvoicesTabPage.Size = new System.Drawing.Size(1001, 529);
            this.GenerateInvoicesTabPage.TabIndex = 0;
            this.GenerateInvoicesTabPage.Text = "Generate Invoices";
            this.GenerateInvoicesTabPage.UseVisualStyleBackColor = true;
            // 
            // refreshUnbilledInvoices
            // 
            this.refreshUnbilledInvoices.Image = global::LabBilling.Properties.Resources.refresh_icon;
            this.refreshUnbilledInvoices.Location = new System.Drawing.Point(331, 7);
            this.refreshUnbilledInvoices.Name = "refreshUnbilledInvoices";
            this.refreshUnbilledInvoices.Size = new System.Drawing.Size(21, 20);
            this.refreshUnbilledInvoices.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.refreshUnbilledInvoices.TabIndex = 18;
            this.refreshUnbilledInvoices.TabStop = false;
            this.refreshUnbilledInvoices.Click += new System.EventHandler(this.refreshUnbilledInvoices_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(3, 504);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(995, 22);
            this.statusStrip1.TabIndex = 17;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(13, 44);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.InvoicesDGV);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.UnbilledAccountsDGV);
            this.splitContainer1.Size = new System.Drawing.Size(976, 447);
            this.splitContainer1.SplitterDistance = 281;
            this.splitContainer1.TabIndex = 16;
            // 
            // InvoiceHistoryTabPage
            // 
            this.InvoiceHistoryTabPage.Controls.Add(this.invoiceTextBox);
            this.InvoiceHistoryTabPage.Controls.Add(this.invoiceLabel);
            this.InvoiceHistoryTabPage.Controls.Add(this.label6);
            this.InvoiceHistoryTabPage.Controls.Add(this.label5);
            this.InvoiceHistoryTabPage.Controls.Add(this.ThroughDate);
            this.InvoiceHistoryTabPage.Controls.Add(this.FromDate);
            this.InvoiceHistoryTabPage.Controls.Add(this.label4);
            this.InvoiceHistoryTabPage.Controls.Add(this.ClientFilter);
            this.InvoiceHistoryTabPage.Controls.Add(this.generateStatementButton);
            this.InvoiceHistoryTabPage.Controls.Add(this.ViewInvoice);
            this.InvoiceHistoryTabPage.Controls.Add(this.PrintInvoiceButton);
            this.InvoiceHistoryTabPage.Controls.Add(this.InvoiceHistoryDGV);
            this.InvoiceHistoryTabPage.Location = new System.Drawing.Point(4, 22);
            this.InvoiceHistoryTabPage.Name = "InvoiceHistoryTabPage";
            this.InvoiceHistoryTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.InvoiceHistoryTabPage.Size = new System.Drawing.Size(1001, 529);
            this.InvoiceHistoryTabPage.TabIndex = 1;
            this.InvoiceHistoryTabPage.Text = "Invoice History";
            this.InvoiceHistoryTabPage.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(751, 20);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "through";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(542, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "From Create Date";
            // 
            // ThroughDate
            // 
            this.ThroughDate.Location = new System.Drawing.Point(800, 17);
            this.ThroughDate.Mask = "00/00/0000";
            this.ThroughDate.Name = "ThroughDate";
            this.ThroughDate.Size = new System.Drawing.Size(100, 20);
            this.ThroughDate.TabIndex = 5;
            this.ThroughDate.ValidatingType = typeof(System.DateTime);
            this.ThroughDate.TextChanged += new System.EventHandler(this.ThroughDate_TextChanged);
            // 
            // FromDate
            // 
            this.FromDate.Location = new System.Drawing.Point(638, 17);
            this.FromDate.Mask = "00/00/0000";
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(100, 20);
            this.FromDate.TabIndex = 5;
            this.FromDate.ValidatingType = typeof(System.DateTime);
            this.FromDate.TextChanged += new System.EventHandler(this.FromDate_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(9, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Filter by Client";
            // 
            // ClientFilter
            // 
            this.ClientFilter.FormattingEnabled = true;
            this.ClientFilter.Location = new System.Drawing.Point(87, 17);
            this.ClientFilter.Name = "ClientFilter";
            this.ClientFilter.Size = new System.Drawing.Size(252, 21);
            this.ClientFilter.TabIndex = 3;
            this.ClientFilter.SelectedIndexChanged += new System.EventHandler(this.ClientFilter_SelectedIndexChanged);
            // 
            // generateStatementButton
            // 
            this.generateStatementButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateStatementButton.Location = new System.Drawing.Point(906, 86);
            this.generateStatementButton.Name = "generateStatementButton";
            this.generateStatementButton.Size = new System.Drawing.Size(87, 44);
            this.generateStatementButton.TabIndex = 2;
            this.generateStatementButton.Text = "Generate Statement";
            this.generateStatementButton.UseVisualStyleBackColor = true;
            this.generateStatementButton.Click += new System.EventHandler(this.generateStatementButton_Click);
            // 
            // ViewInvoice
            // 
            this.ViewInvoice.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ViewInvoice.Location = new System.Drawing.Point(906, 47);
            this.ViewInvoice.Name = "ViewInvoice";
            this.ViewInvoice.Size = new System.Drawing.Size(87, 33);
            this.ViewInvoice.TabIndex = 2;
            this.ViewInvoice.Text = "View Invoice";
            this.ViewInvoice.UseVisualStyleBackColor = true;
            this.ViewInvoice.Click += new System.EventHandler(this.ViewInvoice_Click);
            // 
            // PrintInvoiceButton
            // 
            this.PrintInvoiceButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PrintInvoiceButton.Location = new System.Drawing.Point(906, 136);
            this.PrintInvoiceButton.Name = "PrintInvoiceButton";
            this.PrintInvoiceButton.Size = new System.Drawing.Size(87, 42);
            this.PrintInvoiceButton.TabIndex = 1;
            this.PrintInvoiceButton.Text = "Print Selected Invoices";
            this.PrintInvoiceButton.UseVisualStyleBackColor = true;
            this.PrintInvoiceButton.Click += new System.EventHandler(this.PrintInvoice_Click_1);
            // 
            // InvoiceHistoryDGV
            // 
            this.InvoiceHistoryDGV.AllowUserToAddRows = false;
            this.InvoiceHistoryDGV.AllowUserToDeleteRows = false;
            this.InvoiceHistoryDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.InvoiceHistoryDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.InvoiceHistoryDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.InvoiceHistoryDGV.ContextMenuStrip = this.contextMenuStrip1;
            this.InvoiceHistoryDGV.Location = new System.Drawing.Point(8, 47);
            this.InvoiceHistoryDGV.Name = "InvoiceHistoryDGV";
            this.InvoiceHistoryDGV.ReadOnly = true;
            this.InvoiceHistoryDGV.Size = new System.Drawing.Size(892, 400);
            this.InvoiceHistoryDGV.TabIndex = 0;
            this.InvoiceHistoryDGV.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.InvoiceHistoryDGV_CellMouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoInvoiceToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(145, 26);
            // 
            // undoInvoiceToolStripMenuItem
            // 
            this.undoInvoiceToolStripMenuItem.Name = "undoInvoiceToolStripMenuItem";
            this.undoInvoiceToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.undoInvoiceToolStripMenuItem.Text = "Undo Invoice";
            this.undoInvoiceToolStripMenuItem.Click += new System.EventHandler(this.undoInvoiceToolStripMenuItem_Click);
            // 
            // printContextMenu
            // 
            this.printContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.printToolStripMenuItem,
            this.saveToPDFToolStripMenuItem});
            this.printContextMenu.Name = "printContextMenu";
            this.printContextMenu.Size = new System.Drawing.Size(137, 48);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.printToolStripMenuItem.Text = "Print";
            this.printToolStripMenuItem.Click += new System.EventHandler(this.PrintInvoice_Click);
            // 
            // saveToPDFToolStripMenuItem
            // 
            this.saveToPDFToolStripMenuItem.Name = "saveToPDFToolStripMenuItem";
            this.saveToPDFToolStripMenuItem.Size = new System.Drawing.Size(136, 22);
            this.saveToPDFToolStripMenuItem.Text = "Save to PDF";
            this.saveToPDFToolStripMenuItem.Click += new System.EventHandler(this.PrintInvoice_Click);
            // 
            // invoiceLabel
            // 
            this.invoiceLabel.AutoSize = true;
            this.invoiceLabel.Location = new System.Drawing.Point(345, 19);
            this.invoiceLabel.Name = "invoiceLabel";
            this.invoiceLabel.Size = new System.Drawing.Size(42, 13);
            this.invoiceLabel.TabIndex = 8;
            this.invoiceLabel.Text = "Invoice";
            // 
            // invoiceTextBox
            // 
            this.invoiceTextBox.Location = new System.Drawing.Point(393, 17);
            this.invoiceTextBox.Name = "invoiceTextBox";
            this.invoiceTextBox.Size = new System.Drawing.Size(133, 20);
            this.invoiceTextBox.TabIndex = 9;
            this.invoiceTextBox.TextChanged += new System.EventHandler(this.invoiceTextBox_TextChanged);
            // 
            // ClientInvoiceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1009, 555);
            this.Controls.Add(this.InvoiceHistoryTabControl);
            this.Name = "ClientInvoiceForm";
            this.Text = "Client Invoicing";
            this.Load += new System.EventHandler(this.ClientInvoiceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.InvoicesDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.UnbilledAccountsDGV)).EndInit();
            this.InvoiceHistoryTabControl.ResumeLayout(false);
            this.GenerateInvoicesTabPage.ResumeLayout(false);
            this.GenerateInvoicesTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.refreshUnbilledInvoices)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.InvoiceHistoryTabPage.ResumeLayout(false);
            this.InvoiceHistoryTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.InvoiceHistoryDGV)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.printContextMenu.ResumeLayout(false);
            this.ResumeLayout(false);

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
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox refreshUnbilledInvoices;
        private System.Windows.Forms.ContextMenuStrip printContextMenu;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToPDFToolStripMenuItem;
        private System.Windows.Forms.TextBox invoiceTextBox;
        private System.Windows.Forms.Label invoiceLabel;
    }
}