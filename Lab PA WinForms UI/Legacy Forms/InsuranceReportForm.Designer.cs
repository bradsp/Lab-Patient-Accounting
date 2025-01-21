namespace LabBilling.ReportByInsuranceCompany
{
    partial class InsuranceReportForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InsuranceReportForm));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.cbFinCode = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel4 = new System.Windows.Forms.ToolStripLabel();
            this.tscbInsABC = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnCreateReport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tspbLoading = new System.Windows.Forms.ToolStripProgressBar();
            this.tsslRecords = new System.Windows.Forms.ToolStripStatusLabel();
            this.dgvReportInsurance = new System.Windows.Forms.DataGridView();
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportInsurance)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.cbFinCode,
            this.toolStripSeparator1,
            this.toolStripLabel4,
            this.tscbInsABC,
            this.toolStripSeparator2,
            this.toolStripLabel2,
            this.toolStripLabel3,
            this.toolStripSeparator4,
            this.tsbtnCreateReport,
            this.toolStripSeparator3,
            this.btnPrint,
            this.toolStripSeparator5});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(912, 25);
            this.tsMain.TabIndex = 0;
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(54, 22);
            this.toolStripLabel1.Text = "Fin Code";
            // 
            // cbFinCode
            // 
            this.cbFinCode.AutoToolTip = true;
            this.cbFinCode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFinCode.Name = "cbFinCode";
            this.cbFinCode.Size = new System.Drawing.Size(75, 25);
            this.cbFinCode.SelectedIndexChanged += new System.EventHandler(this.cbFinCode_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel4
            // 
            this.toolStripLabel4.Name = "toolStripLabel4";
            this.toolStripLabel4.Size = new System.Drawing.Size(53, 22);
            this.toolStripLabel4.Text = "Ins Code";
            // 
            // tscbInsABC
            // 
            this.tscbInsABC.Items.AddRange(new object[] {
            "A",
            "B",
            "C"});
            this.tscbInsABC.Name = "tscbInsABC";
            this.tscbInsABC.Size = new System.Drawing.Size(75, 25);
            this.tscbInsABC.ToolTipText = "Leave blank to select all insuranc records.";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(62, 22);
            this.toolStripLabel2.Text = "From Date";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(58, 22);
            this.toolStripLabel3.Text = "Thru Date";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnCreateReport
            // 
            this.tsbtnCreateReport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnCreateReport.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnCreateReport.Image")));
            this.tsbtnCreateReport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnCreateReport.Name = "tsbtnCreateReport";
            this.tsbtnCreateReport.Size = new System.Drawing.Size(83, 22);
            this.tsbtnCreateReport.Text = "Create Report";
            this.tsbtnCreateReport.Click += new System.EventHandler(this.tsbtnCreateReport_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // btnPrint
            // 
            this.btnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnPrint.Image = ((System.Drawing.Image)(resources.GetObject("btnPrint.Image")));
            this.btnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnPrint.Name = "btnPrint";
            this.btnPrint.Size = new System.Drawing.Size(57, 22);
            this.btnPrint.Text = "Print List";
            this.btnPrint.Click += new System.EventHandler(this.btnPrint_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // ssMain
            // 
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tspbLoading,
            this.tsslRecords});
            this.ssMain.Location = new System.Drawing.Point(0, 244);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(912, 22);
            this.ssMain.TabIndex = 2;
            this.ssMain.Text = "statusStrip1";
            // 
            // tspbLoading
            // 
            this.tspbLoading.Name = "tspbLoading";
            this.tspbLoading.Size = new System.Drawing.Size(100, 16);
            this.tspbLoading.Step = 1;
            // 
            // tsslRecords
            // 
            this.tsslRecords.Name = "tsslRecords";
            this.tsslRecords.Size = new System.Drawing.Size(0, 17);
            // 
            // dgvReportInsurance
            // 
            this.dgvReportInsurance.AllowUserToAddRows = false;
            this.dgvReportInsurance.AllowUserToDeleteRows = false;
            this.dgvReportInsurance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvReportInsurance.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvReportInsurance.Location = new System.Drawing.Point(0, 25);
            this.dgvReportInsurance.Name = "dgvReportInsurance";
            this.dgvReportInsurance.ReadOnly = true;
            this.dgvReportInsurance.Size = new System.Drawing.Size(912, 219);
            this.dgvReportInsurance.TabIndex = 3;
            this.dgvReportInsurance.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvAccounts_ColumnHeaderMouseClick);
            this.dgvReportInsurance.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgReportInsurance_RowHeaderMouseDoubleClick);
            // 
            // frmReport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(912, 266);
            this.Controls.Add(this.dgvReportInsurance);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.KeyPreview = true;
            this.Name = "frmReport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Insurance Report";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmReport_KeyUp);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvReportInsurance)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripComboBox cbFinCode;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton tsbtnCreateReport;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnPrint;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox tscbInsABC;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripLabel toolStripLabel4;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.DataGridView dgvReportInsurance;
        private System.Windows.Forms.ToolStripProgressBar tspbLoading;
        private System.Windows.Forms.ToolStripStatusLabel tsslRecords;
    }
}

