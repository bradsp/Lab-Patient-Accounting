﻿namespace LabBilling.Forms
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.worklistsToolStripDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.readyForCollectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.paymentPlanToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sentToCollectionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importCollectionsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbtnBadDebt = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmiSelectAccounts = new System.Windows.Forms.ToolStripMenuItem();
            this.GeneratePatientBillsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.GenerateCollectionsFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.patientStatementsWizardToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsbWriteOff = new System.Windows.Forms.ToolStripButton();
            this.tsbPrintGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbSmallBalWriteOff = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tspbRecords = new System.Windows.Forms.ToolStripProgressBar();
            this.ssRecords = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvAccounts = new System.Windows.Forms.DataGridView();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.worklistsToolStripDropDown,
            this.toolStripSeparator5,
            this.tsddbtnBadDebt,
            this.tsbWriteOff,
            this.tsbPrintGrid,
            this.toolStripSeparator6,
            this.tsbSmallBalWriteOff});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(1010, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // worklistsToolStripDropDown
            // 
            this.worklistsToolStripDropDown.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.worklistsToolStripDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.readyForCollectionsToolStripMenuItem,
            this.paymentPlanToolStripMenuItem,
            this.sentToCollectionsToolStripMenuItem,
            this.importCollectionsFileToolStripMenuItem});
            this.worklistsToolStripDropDown.Image = ((System.Drawing.Image)(resources.GetObject("worklistsToolStripDropDown.Image")));
            this.worklistsToolStripDropDown.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.worklistsToolStripDropDown.Name = "worklistsToolStripDropDown";
            this.worklistsToolStripDropDown.Size = new System.Drawing.Size(68, 22);
            this.worklistsToolStripDropDown.Text = "Worklists";
            // 
            // readyForCollectionsToolStripMenuItem
            // 
            this.readyForCollectionsToolStripMenuItem.Name = "readyForCollectionsToolStripMenuItem";
            this.readyForCollectionsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.readyForCollectionsToolStripMenuItem.Text = "Ready for Collections";
            this.readyForCollectionsToolStripMenuItem.Click += new System.EventHandler(this.tsbLoad_Click);
            // 
            // paymentPlanToolStripMenuItem
            // 
            this.paymentPlanToolStripMenuItem.Name = "paymentPlanToolStripMenuItem";
            this.paymentPlanToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.paymentPlanToolStripMenuItem.Text = "Payment Plan";
            this.paymentPlanToolStripMenuItem.Click += new System.EventHandler(this.tsbLoadMailerP_Click);
            // 
            // sentToCollectionsToolStripMenuItem
            // 
            this.sentToCollectionsToolStripMenuItem.Name = "sentToCollectionsToolStripMenuItem";
            this.sentToCollectionsToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.sentToCollectionsToolStripMenuItem.Text = "Sent to Collections";
            this.sentToCollectionsToolStripMenuItem.Click += new System.EventHandler(this.tsbLoad_Click);
            // 
            // importCollectionsFileToolStripMenuItem
            // 
            this.importCollectionsFileToolStripMenuItem.Name = "importCollectionsFileToolStripMenuItem";
            this.importCollectionsFileToolStripMenuItem.Size = new System.Drawing.Size(193, 22);
            this.importCollectionsFileToolStripMenuItem.Text = "Import Collections File";
            this.importCollectionsFileToolStripMenuItem.Click += new System.EventHandler(this.tsbReadMCLFile_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tsddbtnBadDebt
            // 
            this.tsddbtnBadDebt.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnBadDebt.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.patientStatementsWizardToolStripMenuItem,
            this.tsmiSelectAccounts,
            this.GeneratePatientBillsToolStripMenuItem,
            this.GenerateCollectionsFileToolStripMenuItem});
            this.tsddbtnBadDebt.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnBadDebt.Image")));
            this.tsddbtnBadDebt.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnBadDebt.Name = "tsddbtnBadDebt";
            this.tsddbtnBadDebt.Size = new System.Drawing.Size(78, 22);
            this.tsddbtnBadDebt.Text = "Operations";
            // 
            // tsmiSelectAccounts
            // 
            this.tsmiSelectAccounts.CheckOnClick = true;
            this.tsmiSelectAccounts.Name = "tsmiSelectAccounts";
            this.tsmiSelectAccounts.Size = new System.Drawing.Size(197, 22);
            this.tsmiSelectAccounts.Text = "SELECT PAT BILL ACC\'s";
            this.tsmiSelectAccounts.ToolTipText = "Use \"Bad Debt Batch\" on the MCL billing menu";
            this.tsmiSelectAccounts.Visible = false;
            this.tsmiSelectAccounts.Click += new System.EventHandler(this.TsmiSelectAccounts_Click);
            // 
            // GeneratePatientBillsToolStripMenuItem
            // 
            this.GeneratePatientBillsToolStripMenuItem.CheckOnClick = true;
            this.GeneratePatientBillsToolStripMenuItem.Name = "GeneratePatientBillsToolStripMenuItem";
            this.GeneratePatientBillsToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.GeneratePatientBillsToolStripMenuItem.Text = "Generate Patient Bills";
            this.GeneratePatientBillsToolStripMenuItem.Visible = false;
            this.GeneratePatientBillsToolStripMenuItem.Click += new System.EventHandler(this.GeneratePatientBillsToolStripMenuItem_Click);
            // 
            // GenerateCollectionsFileToolStripMenuItem
            // 
            this.GenerateCollectionsFileToolStripMenuItem.Name = "GenerateCollectionsFileToolStripMenuItem";
            this.GenerateCollectionsFileToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.GenerateCollectionsFileToolStripMenuItem.Text = "Send to Collections";
            this.GenerateCollectionsFileToolStripMenuItem.Visible = false;
            this.GenerateCollectionsFileToolStripMenuItem.Click += new System.EventHandler(this.GenerateCollectionsFileToolStripMenuItem_Click);
            // 
            // patientStatementsWizardToolStripMenuItem
            // 
            this.patientStatementsWizardToolStripMenuItem.Name = "patientStatementsWizardToolStripMenuItem";
            this.patientStatementsWizardToolStripMenuItem.Size = new System.Drawing.Size(197, 22);
            this.patientStatementsWizardToolStripMenuItem.Text = "Run Patient Statements";
            this.patientStatementsWizardToolStripMenuItem.Click += new System.EventHandler(this.patientStatementsWizardToolStripMenuItem_Click);
            // 
            // tsbWriteOff
            // 
            this.tsbWriteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbWriteOff.Image = ((System.Drawing.Image)(resources.GetObject("tsbWriteOff.Image")));
            this.tsbWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbWriteOff.Name = "tsbWriteOff";
            this.tsbWriteOff.Size = new System.Drawing.Size(59, 22);
            this.tsbWriteOff.Text = "Write Off";
            this.tsbWriteOff.Visible = false;
            this.tsbWriteOff.Click += new System.EventHandler(this.tsbWriteOff_Click);
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
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbSmallBalWriteOff
            // 
            this.tsbSmallBalWriteOff.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbSmallBalWriteOff.Enabled = false;
            this.tsbSmallBalWriteOff.Image = ((System.Drawing.Image)(resources.GetObject("tsbSmallBalWriteOff.Image")));
            this.tsbSmallBalWriteOff.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbSmallBalWriteOff.Name = "tsbSmallBalWriteOff";
            this.tsbSmallBalWriteOff.Size = new System.Drawing.Size(135, 22);
            this.tsbSmallBalWriteOff.Text = "Small Balance Write Off";
            this.tsbSmallBalWriteOff.ToolTipText = "Small Balance Write Off $10.00 and less";
            this.tsbSmallBalWriteOff.Visible = false;
            this.tsbSmallBalWriteOff.Click += new System.EventHandler(this.TsbSmallBalWriteOff_Click);
            // 
            // ssMain
            // 
            this.ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbRecords,
            this.ssRecords});
            this.ssMain.Location = new System.Drawing.Point(0, 630);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(1010, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // tspbRecords
            // 
            this.tspbRecords.Name = "tspbRecords";
            this.tspbRecords.Size = new System.Drawing.Size(200, 16);
            this.tspbRecords.Step = 1;
            // 
            // ssRecords
            // 
            this.ssRecords.Name = "ssRecords";
            this.ssRecords.Size = new System.Drawing.Size(52, 17);
            this.ssRecords.Text = "Records:";
            // 
            // dgvAccounts
            // 
            this.dgvAccounts.AllowUserToAddRows = false;
            this.dgvAccounts.AllowUserToDeleteRows = false;
            this.dgvAccounts.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvAccounts.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccounts.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle5.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvAccounts.DefaultCellStyle = dataGridViewCellStyle5;
            this.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvAccounts.Location = new System.Drawing.Point(0, 25);
            this.dgvAccounts.MultiSelect = false;
            this.dgvAccounts.Name = "dgvAccounts";
            this.dgvAccounts.ReadOnly = true;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvAccounts.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvAccounts.Size = new System.Drawing.Size(1010, 605);
            this.dgvAccounts.TabIndex = 2;
            this.dgvAccounts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccounts_CellClick);
            this.dgvAccounts.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvAccounts_CellContentClick);
            this.dgvAccounts.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvAccounts_CellFormatting);
            this.dgvAccounts.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_CellMouseDoubleClick);
            this.dgvAccounts.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_ColumnHeaderMouseClick);
            this.dgvAccounts.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvAccounts_DataError);
            this.dgvAccounts.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_RowHeaderMouseDoubleClick);
            this.dgvAccounts.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvAccounts_RowsAdded);
            this.dgvAccounts.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvAccounts_RowsRemoved);
            this.dgvAccounts.SelectionChanged += new System.EventHandler(this.DgvAccounts_SelectionChanged);
            // 
            // PatientCollectionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1010, 652);
            this.Controls.Add(this.dgvAccounts);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PatientCollectionsForm";
            this.Text = "Patient Collections";
            this.Load += new System.EventHandler(this.frmBadDebt_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvAccounts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.DataGridView dgvAccounts;
        private System.Windows.Forms.ToolStripButton tsbWriteOff;
        private System.Windows.Forms.ToolStripStatusLabel ssRecords;
        private System.Windows.Forms.ToolStripButton tsbPrintGrid;
        private System.Windows.Forms.ToolStripProgressBar tspbRecords;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnBadDebt;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem tsmiSelectAccounts;
        private System.Windows.Forms.ToolStripMenuItem GeneratePatientBillsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripButton tsbSmallBalWriteOff;
        private System.Windows.Forms.ToolStripMenuItem GenerateCollectionsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton worklistsToolStripDropDown;
        private System.Windows.Forms.ToolStripMenuItem readyForCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem paymentPlanToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sentToCollectionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importCollectionsFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem patientStatementsWizardToolStripMenuItem;
    }
}
