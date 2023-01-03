namespace LabBilling.Legacy
{
    partial class PrintEOBForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrintEOBForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbPrint = new System.Windows.Forms.ToolStripButton();
            this.tsddbtnFile = new System.Windows.Forms.ToolStripDropDownButton();
            this.tstbFile = new System.Windows.Forms.ToolStripTextBox();
            this.tscbAccounts = new System.Windows.Forms.ToolStripComboBox();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.dgvSelection = new System.Windows.Forms.DataGridView();
            this.Claim_Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eft_date = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eft_number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.eft_file = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tsbViewEOB = new System.Windows.Forms.ToolStripButton();
            this.tsMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelection)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbPrint,
            this.tsddbtnFile,
            this.tsbViewEOB});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(719, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "Main";
            // 
            // tsbPrint
            // 
            this.tsbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsbPrint.Image")));
            this.tsbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbPrint.Name = "tsbPrint";
            this.tsbPrint.Size = new System.Drawing.Size(56, 22);
            this.tsbPrint.Text = "Print EOB";
            this.tsbPrint.Click += new System.EventHandler(this.tsbPrint_Click);
            // 
            // tsddbtnFile
            // 
            this.tsddbtnFile.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstbFile,
            this.tscbAccounts});
            this.tsddbtnFile.Enabled = false;
            this.tsddbtnFile.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnFile.Image")));
            this.tsddbtnFile.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnFile.Name = "tsddbtnFile";
            this.tsddbtnFile.Size = new System.Drawing.Size(36, 22);
            this.tsddbtnFile.Text = "File";
            this.tsddbtnFile.ToolTipText = "Gets the EOB\'s for a file";
            this.tsddbtnFile.Visible = false;
            // 
            // tstbFile
            // 
            this.tstbFile.AcceptsReturn = true;
            this.tstbFile.Name = "tstbFile";
            this.tstbFile.Size = new System.Drawing.Size(100, 21);
            // 
            // tscbAccounts
            // 
            this.tscbAccounts.Name = "tscbAccounts";
            this.tscbAccounts.Size = new System.Drawing.Size(121, 21);
            // 
            // ssMain
            // 
            this.ssMain.Location = new System.Drawing.Point(0, 328);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(719, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "Main";
            // 
            // dgvSelection
            // 
            this.dgvSelection.AllowUserToAddRows = false;
            this.dgvSelection.AllowUserToDeleteRows = false;
            this.dgvSelection.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvSelection.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvSelection.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Claim_Status,
            this.eft_date,
            this.eft_number,
            this.eft_file});
            this.dgvSelection.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvSelection.Location = new System.Drawing.Point(0, 25);
            this.dgvSelection.Name = "dgvSelection";
            dataGridViewCellStyle1.Format = "N0";
            dataGridViewCellStyle1.NullValue = null;
            this.dgvSelection.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvSelection.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvSelection.Size = new System.Drawing.Size(719, 303);
            this.dgvSelection.TabIndex = 2;
            this.dgvSelection.RowHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvSelection_RowHeaderMouseClick);
            // 
            // Claim_Status
            // 
            this.Claim_Status.HeaderText = "Claim Status";
            this.Claim_Status.Name = "Claim_Status";
            this.Claim_Status.Width = 90;
            // 
            // eft_date
            // 
            this.eft_date.HeaderText = "File Date";
            this.eft_date.Name = "eft_date";
            this.eft_date.Width = 74;
            // 
            // eft_number
            // 
            this.eft_number.HeaderText = "File Number";
            this.eft_number.Name = "eft_number";
            this.eft_number.Width = 88;
            // 
            // eft_file
            // 
            this.eft_file.HeaderText = "File Name";
            this.eft_file.Name = "eft_file";
            this.eft_file.Width = 79;
            // 
            // tsbViewEOB
            // 
            this.tsbViewEOB.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbViewEOB.DoubleClickEnabled = true;
            this.tsbViewEOB.Image = ((System.Drawing.Image)(resources.GetObject("tsbViewEOB.Image")));
            this.tsbViewEOB.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbViewEOB.Name = "tsbViewEOB";
            this.tsbViewEOB.Size = new System.Drawing.Size(56, 22);
            this.tsbViewEOB.Text = "View EOB";
            this.tsbViewEOB.Click += new System.EventHandler(this.tsbViewEOB_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(719, 350);
            this.Controls.Add(this.dgvSelection);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Print EOBs";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvSelection)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripButton tsbPrint;
        private System.Windows.Forms.DataGridView dgvSelection;
        private System.Windows.Forms.DataGridViewTextBoxColumn Claim_Status;
        private System.Windows.Forms.DataGridViewTextBoxColumn eft_date;
        private System.Windows.Forms.DataGridViewTextBoxColumn eft_number;
        private System.Windows.Forms.DataGridViewTextBoxColumn eft_file;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnFile;
        private System.Windows.Forms.ToolStripComboBox tscbAccounts;
        private System.Windows.Forms.ToolStripTextBox tstbFile;
        private System.Windows.Forms.ToolStripButton tsbViewEOB;
    }
}

