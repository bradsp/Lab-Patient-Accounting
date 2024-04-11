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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            tsMain = new System.Windows.Forms.ToolStrip();
            worklistsToolStripDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            readyForCollectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            paymentPlanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            sentToCollectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            importCollectionsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            tsbWriteOff = new System.Windows.Forms.ToolStripButton();
            tsbPrintGrid = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            tsbSmallBalWriteOff = new System.Windows.Forms.ToolStripButton();
            runStatementsToolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ssMain = new System.Windows.Forms.StatusStrip();
            tspbRecords = new System.Windows.Forms.ToolStripProgressBar();
            ssRecords = new System.Windows.Forms.ToolStripStatusLabel();
            dgvAccounts = new System.Windows.Forms.DataGridView();
            tsMain.SuspendLayout();
            ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dgvAccounts).BeginInit();
            SuspendLayout();
            // 
            // tsMain
            // 
            tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { worklistsToolStripDropDown, toolStripSeparator5, tsbWriteOff, tsbPrintGrid, toolStripSeparator6, tsbSmallBalWriteOff, runStatementsToolStripButton1 });
            tsMain.Location = new System.Drawing.Point(0, 0);
            tsMain.Name = "tsMain";
            tsMain.Size = new System.Drawing.Size(1178, 25);
            tsMain.TabIndex = 0;
            tsMain.Text = "toolStrip1";
            // 
            // worklistsToolStripDropDown
            // 
            worklistsToolStripDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            worklistsToolStripDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { readyForCollectionsToolStripMenuItem, paymentPlanToolStripMenuItem, sentToCollectionsToolStripMenuItem, importCollectionsFileToolStripMenuItem });
            worklistsToolStripDropDown.Image = (System.Drawing.Image)resources.GetObject("worklistsToolStripDropDown.Image");
            worklistsToolStripDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            worklistsToolStripDropDown.Name = "worklistsToolStripDropDown";
            worklistsToolStripDropDown.Size = new System.Drawing.Size(68, 22);
            worklistsToolStripDropDown.Text = "Worklists";
            // 
            // readyForCollectionsToolStripMenuItem
            // 
            readyForCollectionsToolStripMenuItem.Name = "readyForCollectionsToolStripMenuItem";
            readyForCollectionsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            readyForCollectionsToolStripMenuItem.Text = "Ready for Collections";
            readyForCollectionsToolStripMenuItem.Click += tsbLoad_Click;
            // 
            // paymentPlanToolStripMenuItem
            // 
            paymentPlanToolStripMenuItem.Name = "paymentPlanToolStripMenuItem";
            paymentPlanToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            paymentPlanToolStripMenuItem.Text = "Payment Plan";
            paymentPlanToolStripMenuItem.Click += tsbLoadMailerP_Click;
            // 
            // sentToCollectionsToolStripMenuItem
            // 
            sentToCollectionsToolStripMenuItem.Name = "sentToCollectionsToolStripMenuItem";
            sentToCollectionsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            sentToCollectionsToolStripMenuItem.Text = "Sent to Collections";
            sentToCollectionsToolStripMenuItem.Click += tsbLoad_Click;
            // 
            // importCollectionsFileToolStripMenuItem
            // 
            importCollectionsFileToolStripMenuItem.Name = "importCollectionsFileToolStripMenuItem";
            importCollectionsFileToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            importCollectionsFileToolStripMenuItem.Text = "Import Collections File";
            importCollectionsFileToolStripMenuItem.Click += tsbReadMCLFile_Click;
            // 
            // toolStripSeparator5
            // 
            toolStripSeparator5.Name = "toolStripSeparator5";
            toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbWriteOff
            // 
            tsbWriteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbWriteOff.Image = (System.Drawing.Image)resources.GetObject("tsbWriteOff.Image");
            tsbWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbWriteOff.Name = "tsbWriteOff";
            tsbWriteOff.Size = new System.Drawing.Size(59, 22);
            tsbWriteOff.Text = "Write Off";
            tsbWriteOff.Visible = false;
            tsbWriteOff.Click += tsbWriteOff_Click;
            // 
            // tsbPrintGrid
            // 
            tsbPrintGrid.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbPrintGrid.Image = (System.Drawing.Image)resources.GetObject("tsbPrintGrid.Image");
            tsbPrintGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbPrintGrid.Name = "tsbPrintGrid";
            tsbPrintGrid.Size = new System.Drawing.Size(61, 22);
            tsbPrintGrid.Text = "Print Grid";
            tsbPrintGrid.Click += tsbPrintGrid_Click;
            // 
            // toolStripSeparator6
            // 
            toolStripSeparator6.Name = "toolStripSeparator6";
            toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSmallBalWriteOff
            // 
            tsbSmallBalWriteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsbSmallBalWriteOff.Enabled = false;
            tsbSmallBalWriteOff.Image = (System.Drawing.Image)resources.GetObject("tsbSmallBalWriteOff.Image");
            tsbSmallBalWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            tsbSmallBalWriteOff.Name = "tsbSmallBalWriteOff";
            tsbSmallBalWriteOff.Size = new System.Drawing.Size(135, 22);
            tsbSmallBalWriteOff.Text = "Small Balance Write Off";
            tsbSmallBalWriteOff.ToolTipText = "Small Balance Write Off $10.00 and less";
            tsbSmallBalWriteOff.Visible = false;
            tsbSmallBalWriteOff.Click += TsbSmallBalWriteOff_Click;
            // 
            // runStatementsToolStripButton1
            // 
            runStatementsToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            runStatementsToolStripButton1.Image = (System.Drawing.Image)resources.GetObject("runStatementsToolStripButton1.Image");
            runStatementsToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            runStatementsToolStripButton1.Name = "runStatementsToolStripButton1";
            runStatementsToolStripButton1.Size = new System.Drawing.Size(158, 22);
            runStatementsToolStripButton1.Text = "Run Statements/Collections";
            runStatementsToolStripButton1.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            runStatementsToolStripButton1.Click += patientStatementsWizardToolStripMenuItem_Click;
            // 
            // ssMain
            // 
            ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tspbRecords, ssRecords });
            ssMain.Location = new System.Drawing.Point(0, 728);
            ssMain.Name = "ssMain";
            ssMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            ssMain.Size = new System.Drawing.Size(1178, 24);
            ssMain.TabIndex = 1;
            ssMain.Text = "statusStrip1";
            // 
            // tspbRecords
            // 
            tspbRecords.Name = "tspbRecords";
            tspbRecords.Size = new System.Drawing.Size(233, 18);
            tspbRecords.Step = 1;
            // 
            // ssRecords
            // 
            ssRecords.Name = "ssRecords";
            ssRecords.Size = new System.Drawing.Size(52, 19);
            ssRecords.Text = "Records:";
            // 
            // dgvAccounts
            // 
            dgvAccounts.AllowUserToAddRows = false;
            dgvAccounts.AllowUserToDeleteRows = false;
            dgvAccounts.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dgvAccounts.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvAccounts.DefaultCellStyle = dataGridViewCellStyle2;
            dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            dgvAccounts.Location = new System.Drawing.Point(0, 25);
            dgvAccounts.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dgvAccounts.MultiSelect = false;
            dgvAccounts.Name = "dgvAccounts";
            dgvAccounts.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvAccounts.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dgvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvAccounts.Size = new System.Drawing.Size(1178, 703);
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
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1178, 752);
            Controls.Add(dgvAccounts);
            Controls.Add(ssMain);
            Controls.Add(tsMain);
            ForeColor = System.Drawing.Color.Black;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
        private System.Windows.Forms.ToolStripButton tsbWriteOff;
        private System.Windows.Forms.ToolStripStatusLabel ssRecords;
        private System.Windows.Forms.ToolStripButton tsbPrintGrid;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbSmallBalWriteOff;
        private System.Windows.Forms.ToolStripDropDownButton worklistsToolStripDropDown;
        private System.Windows.Forms.ToolStripMenuItem readyForCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paymentPlanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sentToCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCollectionsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton runStatementsToolStripButton1;
    }
}

