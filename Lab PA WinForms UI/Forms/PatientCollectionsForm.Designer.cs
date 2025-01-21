namespace LabBilling.Forms
{
    partial class PatientCollectionsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatientCollectionsForm));
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            tsMain = new ToolStrip();
            worklistsToolStripDropDown = new ToolStripDropDownButton();
            readyForCollectionsToolStripMenuItem = new ToolStripMenuItem();
            paymentPlanToolStripMenuItem = new ToolStripMenuItem();
            sentToCollectionsToolStripMenuItem = new ToolStripMenuItem();
            importCollectionsFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator5 = new ToolStripSeparator();
            writeOffToolStripButton = new ToolStripButton();
            printGridToolStripButton = new ToolStripButton();
            toolStripSeparator6 = new ToolStripSeparator();
            smallBalanceWriteoffToolStripButton = new ToolStripButton();
            runStatementsToolStripButton1 = new ToolStripButton();
            ssMain = new StatusStrip();
            tspbRecords = new ToolStripProgressBar();
            ssRecords = new ToolStripStatusLabel();
            dgvAccounts = new DataGridView();
            tsMain.SuspendLayout();
            ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            SuspendLayout();
            // 
            // tsMain
            // 
            tsMain.GripStyle = ToolStripGripStyle.Hidden;
            tsMain.ImageScalingSize = new Size(20, 20);
            tsMain.Items.AddRange(new ToolStripItem[] { worklistsToolStripDropDown, toolStripSeparator5, writeOffToolStripButton, printGridToolStripButton, toolStripSeparator6, smallBalanceWriteoffToolStripButton, runStatementsToolStripButton1 });
            tsMain.Location = new Point(0, 0);
            tsMain.Name = "tsMain";
            tsMain.Size = new Size(1178, 25);
            tsMain.TabIndex = 0;
            tsMain.Text = "toolStrip1";
            // 
            // worklistsToolStripDropDown
            // 
            worklistsToolStripDropDown.DisplayStyle = ToolStripItemDisplayStyle.Text;
            worklistsToolStripDropDown.DropDownItems.AddRange(new ToolStripItem[] { readyForCollectionsToolStripMenuItem, paymentPlanToolStripMenuItem, sentToCollectionsToolStripMenuItem, importCollectionsFileToolStripMenuItem });
            worklistsToolStripDropDown.Image = (Image)resources.GetObject("worklistsToolStripDropDown.Image");
            worklistsToolStripDropDown.ImageTransparentColor = Color.Magenta;
            worklistsToolStripDropDown.Name = "worklistsToolStripDropDown";
            worklistsToolStripDropDown.Size = new Size(68, 22);
            worklistsToolStripDropDown.Text = "Worklists";
            // 
            // readyForCollectionsToolStripMenuItem
            // 
            readyForCollectionsToolStripMenuItem.Name = "readyForCollectionsToolStripMenuItem";
            readyForCollectionsToolStripMenuItem.Size = new Size(193, 22);
            readyForCollectionsToolStripMenuItem.Text = "Ready for Collections";
            readyForCollectionsToolStripMenuItem.Click += tsbLoad_Click;
            // 
            // paymentPlanToolStripMenuItem
            // 
            paymentPlanToolStripMenuItem.Name = "paymentPlanToolStripMenuItem";
            paymentPlanToolStripMenuItem.Size = new Size(193, 22);
            paymentPlanToolStripMenuItem.Text = "Payment Plan";
            paymentPlanToolStripMenuItem.Click += tsbLoadMailerP_Click;
            // 
            // sentToCollectionsToolStripMenuItem
            // 
            sentToCollectionsToolStripMenuItem.Name = "sentToCollectionsToolStripMenuItem";
            sentToCollectionsToolStripMenuItem.Size = new Size(193, 22);
            sentToCollectionsToolStripMenuItem.Text = "Sent to Collections";
            sentToCollectionsToolStripMenuItem.Click += tsbLoad_Click;
            // 
            // importCollectionsFileToolStripMenuItem
            // 
            importCollectionsFileToolStripMenuItem.Name = "importCollectionsFileToolStripMenuItem";
            importCollectionsFileToolStripMenuItem.Size = new Size(193, 22);
            importCollectionsFileToolStripMenuItem.Text = "Import Collections File";
            importCollectionsFileToolStripMenuItem.Click += tsbReadMCLFile_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new Size(6, 25);
            // 
            // writeOffToolStripButton
            // 
            writeOffToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            writeOffToolStripButton.Image = (Image)resources.GetObject("writeOffToolStripButton.Image");
            writeOffToolStripButton.ImageTransparentColor = Color.Magenta;
            writeOffToolStripButton.Name = "writeOffToolStripButton";
            writeOffToolStripButton.Size = new Size(59, 22);
            writeOffToolStripButton.Text = "Write Off";
            writeOffToolStripButton.Visible = false;
            writeOffToolStripButton.Click += SmallBalanceWriteOffToolStripButton_Click;
            // 
            // printGridToolStripButton
            // 
            printGridToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            printGridToolStripButton.Image = (Image)resources.GetObject("printGridToolStripButton.Image");
            printGridToolStripButton.ImageTransparentColor = Color.Magenta;
            printGridToolStripButton.Name = "printGridToolStripButton";
            printGridToolStripButton.Size = new Size(61, 22);
            printGridToolStripButton.Text = "Print Grid";
            printGridToolStripButton.Click += tsbPrintGrid_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new Size(6, 25);
            // 
            // smallBalanceWriteoffToolStripButton
            // 
            smallBalanceWriteoffToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
            smallBalanceWriteoffToolStripButton.Enabled = false;
            smallBalanceWriteoffToolStripButton.Image = (Image)resources.GetObject("smallBalanceWriteoffToolStripButton.Image");
            smallBalanceWriteoffToolStripButton.ImageTransparentColor = Color.Magenta;
            smallBalanceWriteoffToolStripButton.Name = "smallBalanceWriteoffToolStripButton";
            smallBalanceWriteoffToolStripButton.Size = new Size(135, 22);
            smallBalanceWriteoffToolStripButton.Text = "Small Balance Write Off";
            smallBalanceWriteoffToolStripButton.ToolTipText = "Small Balance Write Off $10.00 and less";
            smallBalanceWriteoffToolStripButton.Visible = false;
            smallBalanceWriteoffToolStripButton.Click += TsbSmallBalWriteOff_Click;
            // 
            // runStatementsToolStripButton1
            // 
            runStatementsToolStripButton1.DisplayStyle = ToolStripItemDisplayStyle.Text;
            runStatementsToolStripButton1.Image = (Image)resources.GetObject("runStatementsToolStripButton1.Image");
            runStatementsToolStripButton1.ImageTransparentColor = Color.Magenta;
            runStatementsToolStripButton1.Name = "runStatementsToolStripButton1";
            runStatementsToolStripButton1.Size = new Size(158, 22);
            runStatementsToolStripButton1.Text = "Run Statements/Collections";
            runStatementsToolStripButton1.TextImageRelation = TextImageRelation.TextBeforeImage;
            runStatementsToolStripButton1.Click += patientStatementsWizardToolStripMenuItem_Click;
            // 
            // ssMain
            // 
            ssMain.ImageScalingSize = new Size(20, 20);
            ssMain.Items.AddRange(new ToolStripItem[] { tspbRecords, ssRecords });
            ssMain.Location = new Point(0, 728);
            ssMain.Name = "ssMain";
            ssMain.Padding = new Padding(1, 0, 16, 0);
            ssMain.Size = new Size(1178, 24);
            ssMain.TabIndex = 1;
            ssMain.Text = "statusStrip1";
            // 
            // tspbRecords
            // 
            tspbRecords.Name = "tspbRecords";
            tspbRecords.Size = new Size(233, 18);
            tspbRecords.Step = 1;
            // 
            // ssRecords
            // 
            ssRecords.Name = "ssRecords";
            ssRecords.Size = new Size(52, 19);
            ssRecords.Text = "Records:";
            // 
            // dgvAccounts
            // 
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.BackgroundColor = SystemColors.ControlLightLight;
            dgvAccounts.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            dgvAccounts.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            dgvAccounts.DefaultCellStyle = dataGridViewCellStyle5;
            dgvAccounts.Dock = DockStyle.Fill;
            dgvAccounts.Location = new Point(0, 25);
            dgvAccounts.Margin = new Padding(4, 3, 4, 3);
            dgvAccounts.MultiSelect = false;
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            dgvAccounts.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            dgvAccounts.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.Size = new Size(1178, 703);
            dgvAccounts.TabIndex = 2;
            dgvAccounts.CellClick += dgvAccounts_CellClick;
            dgvAccounts.CellContentClick += dgvAccounts_CellContentClick;
            dgvAccounts.CellFormatting += dgvAccounts_CellFormatting;
            dgvAccounts.CellMouseDoubleClick += dgvAccounts_CellMouseDoubleClick;
            dgvAccounts.ColumnHeaderMouseClick += dgvAccounts_ColumnHeaderMouseClick;
            dgvAccounts.DataError += dgvAccounts_DataError;
            dgvAccounts.RowHeaderMouseDoubleClick += dgvAccounts_RowHeaderMouseDoubleClick;
            dgvAccounts.RowsAdded += dgvAccounts_RowsAdded;
            dgvAccounts.RowsRemoved += dgvAccounts_RowsRemoved;
            dgvAccounts.SelectionChanged += DgvAccounts_SelectionChanged;
            // 
            // PatientCollectionsForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1178, 752);
            Controls.Add(dgvAccounts);
            Controls.Add(ssMain);
            Controls.Add(tsMain);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            Name = "PatientCollectionsForm";
            Text = "Patient Collections";
            Load += frmBadDebt_Load;
            tsMain.ResumeLayout(false);
            tsMain.PerformLayout();
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.ToolStripButton writeOffToolStripButton;
        private System.Windows.Forms.ToolStripStatusLabel ssRecords;
        private System.Windows.Forms.ToolStripButton printGridToolStripButton;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton smallBalanceWriteoffToolStripButton;
        private System.Windows.Forms.ToolStripDropDownButton worklistsToolStripDropDown;
        private System.Windows.Forms.ToolStripMenuItem readyForCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paymentPlanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sentToCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCollectionsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton runStatementsToolStripButton1;
    }
}

