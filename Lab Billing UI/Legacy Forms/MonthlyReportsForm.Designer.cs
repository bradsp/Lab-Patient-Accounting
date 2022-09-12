namespace LabBilling.Legacy
{
    partial class frmReports
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmReports));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsddbtnUserDefined = new System.Windows.Forms.ToolStripSplitButton();
            this.tscbCPT4s = new System.Windows.Forms.ToolStripComboBox();
            this.tsddbtnBillingReports = new System.Windows.Forms.ToolStripDropDownButton();
            this.tsmi80299 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi59Modis = new System.Windows.Forms.ToolStripMenuItem();
            this.modifiersMultipleCpt4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.mailerErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsWithNoPatientRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsWithNoInsuranceRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiNeedModifiers = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_82575_82565 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_86157_86156 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_87081_87040 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_88360_88342 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmi_87591_withMultiples = new System.Windows.Forms.ToolStripMenuItem();
            this.testToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            this.tsmi_C3_C4 = new System.Windows.Forms.ToolStripMenuItem();
            this.tssBilling_Mcloe = new System.Windows.Forms.ToolStripSeparator();
            this.tsddbtnMCLOEReports = new System.Windows.Forms.ToolStripDropDownButton();
            this.aBNsReportedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbtnPrint = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbCDM = new System.Windows.Forms.ToolStripSplitButton();
            this.tstbCDM1 = new System.Windows.Forms.ToolStripTextBox();
            this.tstbCDM2 = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbTableReports = new System.Windows.Forms.ToolStripSplitButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tssbExcel = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiExcelDirectory = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmicbExcelDirectory = new System.Windows.Forms.ToolStripComboBox();
            this.tsmiExcelFile = new System.Windows.Forms.ToolStripMenuItem();
            this.tstbExcelFileName = new System.Windows.Forms.ToolStripTextBox();
            this.tsmiExcelWorkSheetName = new System.Windows.Forms.ToolStripMenuItem();
            this.tstbExcelWorkSheetName = new System.Windows.Forms.ToolStripTextBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tscbClients = new System.Windows.Forms.ToolStripComboBox();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tsslReportTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.tspbCount = new System.Windows.Forms.ToolStripProgressBar();
            this.tsslRecords = new System.Windows.Forms.ToolStripStatusLabel();
            this.tsslDisplayReportTitle = new System.Windows.Forms.ToolStripStatusLabel();
            this.m_dgvReport = new System.Windows.Forms.DataGridView();
            this.cmsRecords = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.niMsg = new System.Windows.Forms.NotifyIcon(this.components);
            this.tsMain.SuspendLayout();
            this.ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvReport)).BeginInit();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsddbtnUserDefined,
            this.tsddbtnBillingReports,
            this.tssBilling_Mcloe,
            this.tsddbtnMCLOEReports,
            this.toolStripSeparator2,
            this.tsbtnPrint,
            this.toolStripSeparator4,
            this.tssbCDM,
            this.toolStripSeparator1,
            this.tssbTableReports,
            this.toolStripSeparator3,
            this.tssbExcel,
            this.toolStripSeparator5,
            this.tscbClients});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(798, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "Main Toolstrip";
            // 
            // tsddbtnUserDefined
            // 
            this.tsddbtnUserDefined.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnUserDefined.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tscbCPT4s});
            this.tsddbtnUserDefined.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnUserDefined.Image")));
            this.tsddbtnUserDefined.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnUserDefined.Name = "tsddbtnUserDefined";
            this.tsddbtnUserDefined.Size = new System.Drawing.Size(128, 22);
            this.tsddbtnUserDefined.Text = "User Defined Report";
            this.tsddbtnUserDefined.ButtonClick += new System.EventHandler(this.tsbtnComboCpt4_Click);
            this.tsddbtnUserDefined.DropDownOpening += new System.EventHandler(this.tsddbtnBillingReports_DropDownOpening);
            // 
            // tscbCPT4s
            // 
            this.tscbCPT4s.Name = "tscbCPT4s";
            this.tscbCPT4s.Size = new System.Drawing.Size(179, 23);
            this.tscbCPT4s.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tscbCPT4s_KeyUp);
            // 
            // tsddbtnBillingReports
            // 
            this.tsddbtnBillingReports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnBillingReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi80299,
            this.tsmi59Modis,
            this.modifiersMultipleCpt4ToolStripMenuItem,
            this.mailerErrorsToolStripMenuItem,
            this.accountsWithNoPatientRecordToolStripMenuItem,
            this.accountsWithNoInsuranceRecordToolStripMenuItem,
            this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem,
            this.tsmiNeedModifiers});
            this.tsddbtnBillingReports.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnBillingReports.Image")));
            this.tsddbtnBillingReports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnBillingReports.Name = "tsddbtnBillingReports";
            this.tsddbtnBillingReports.Size = new System.Drawing.Size(96, 22);
            this.tsddbtnBillingReports.Tag = "WTHMCLBILL|MCLLIVE";
            this.tsddbtnBillingReports.Text = "Billing Reports";
            this.tsddbtnBillingReports.DropDownOpening += new System.EventHandler(this.tsddbtnBillingReports_DropDownOpening);
            // 
            // tsmi80299
            // 
            this.tsmi80299.Name = "tsmi80299";
            this.tsmi80299.Size = new System.Drawing.Size(437, 22);
            this.tsmi80299.Text = "J Codes and 80299";
            this.tsmi80299.Click += new System.EventHandler(this.tsmi80299_Click);
            // 
            // tsmi59Modis
            // 
            this.tsmi59Modis.Name = "tsmi59Modis";
            this.tsmi59Modis.Size = new System.Drawing.Size(437, 22);
            this.tsmi59Modis.Text = "59 Modifiers - Charge Details";
            this.tsmi59Modis.Click += new System.EventHandler(this.tsmi59Modis_Click);
            // 
            // modifiersMultipleCpt4ToolStripMenuItem
            // 
            this.modifiersMultipleCpt4ToolStripMenuItem.Name = "modifiersMultipleCpt4ToolStripMenuItem";
            this.modifiersMultipleCpt4ToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            this.modifiersMultipleCpt4ToolStripMenuItem.Text = "59 Modifiers - Multiple Cpt4";
            this.modifiersMultipleCpt4ToolStripMenuItem.Click += new System.EventHandler(this.modifiersMultipleCpt4ToolStripMenuItem_Click);
            // 
            // mailerErrorsToolStripMenuItem
            // 
            this.mailerErrorsToolStripMenuItem.Name = "mailerErrorsToolStripMenuItem";
            this.mailerErrorsToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            this.mailerErrorsToolStripMenuItem.Text = "Mailer Errors";
            this.mailerErrorsToolStripMenuItem.Click += new System.EventHandler(this.mailerErrorsToolStripMenuItem_Click);
            // 
            // accountsWithNoPatientRecordToolStripMenuItem
            // 
            this.accountsWithNoPatientRecordToolStripMenuItem.Name = "accountsWithNoPatientRecordToolStripMenuItem";
            this.accountsWithNoPatientRecordToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            this.accountsWithNoPatientRecordToolStripMenuItem.Text = "Accounts with no Patient Record";
            this.accountsWithNoPatientRecordToolStripMenuItem.Click += new System.EventHandler(this.accountsWithNoPatientRecordToolStripMenuItem_Click);
            // 
            // accountsWithNoInsuranceRecordToolStripMenuItem
            // 
            this.accountsWithNoInsuranceRecordToolStripMenuItem.Name = "accountsWithNoInsuranceRecordToolStripMenuItem";
            this.accountsWithNoInsuranceRecordToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            this.accountsWithNoInsuranceRecordToolStripMenuItem.Text = "Accounts with no Insurance Record";
            this.accountsWithNoInsuranceRecordToolStripMenuItem.Click += new System.EventHandler(this.accountsWithNoInsuranceRecordToolStripMenuItem_Click);
            // 
            // accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem
            // 
            this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Name = "accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem";
            this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Text = "Accounts where fin code does not match primary insurance fin_code";
            this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Click += new System.EventHandler(this.accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem_Click);
            // 
            // tsmiNeedModifiers
            // 
            this.tsmiNeedModifiers.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmi_82575_82565,
            this.tsmi_86157_86156,
            this.tsmi_87081_87040,
            this.tsmi_88360_88342,
            this.tsmi_87591_withMultiples,
            this.testToolStripMenuItem,
            this.tsmi_C3_C4});
            this.tsmiNeedModifiers.Name = "tsmiNeedModifiers";
            this.tsmiNeedModifiers.Size = new System.Drawing.Size(437, 22);
            this.tsmiNeedModifiers.Text = "Needs Modifiers";
            // 
            // tsmi_82575_82565
            // 
            this.tsmi_82575_82565.CheckOnClick = true;
            this.tsmi_82575_82565.Name = "tsmi_82575_82565";
            this.tsmi_82575_82565.Size = new System.Drawing.Size(265, 22);
            this.tsmi_82575_82565.Tag = "82575|82565";
            this.tsmi_82575_82565.Text = "82575 and 82565";
            this.tsmi_82575_82565.Click += new System.EventHandler(this.tsmi_Contains_Click);
            // 
            // tsmi_86157_86156
            // 
            this.tsmi_86157_86156.CheckOnClick = true;
            this.tsmi_86157_86156.Name = "tsmi_86157_86156";
            this.tsmi_86157_86156.Size = new System.Drawing.Size(265, 22);
            this.tsmi_86157_86156.Tag = "86157|86156";
            this.tsmi_86157_86156.Text = "86157 and 86156";
            this.tsmi_86157_86156.Click += new System.EventHandler(this.tsmi_Contains_Click);
            // 
            // tsmi_87081_87040
            // 
            this.tsmi_87081_87040.CheckOnClick = true;
            this.tsmi_87081_87040.Name = "tsmi_87081_87040";
            this.tsmi_87081_87040.Size = new System.Drawing.Size(265, 22);
            this.tsmi_87081_87040.Tag = "87081|87040";
            this.tsmi_87081_87040.Text = "87081 and 87040";
            this.tsmi_87081_87040.Click += new System.EventHandler(this.tsmi_Contains_Click);
            // 
            // tsmi_88360_88342
            // 
            this.tsmi_88360_88342.CheckOnClick = true;
            this.tsmi_88360_88342.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmi_88360_88342.Name = "tsmi_88360_88342";
            this.tsmi_88360_88342.Size = new System.Drawing.Size(265, 22);
            this.tsmi_88360_88342.Tag = "88360|88342";
            this.tsmi_88360_88342.Text = "88360 and 88342";
            this.tsmi_88360_88342.Click += new System.EventHandler(this.tsmi_Contains_Click);
            // 
            // tsmi_87591_withMultiples
            // 
            this.tsmi_87591_withMultiples.CheckOnClick = true;
            this.tsmi_87591_withMultiples.Name = "tsmi_87591_withMultiples";
            this.tsmi_87591_withMultiples.Size = new System.Drawing.Size(265, 22);
            this.tsmi_87591_withMultiples.Tag = "87591|83890\',\'83891\',\'83898";
            this.tsmi_87591_withMultiples.Text = "87591\'s with 83890 or 83891 or 83898";
            this.tsmi_87591_withMultiples.Click += new System.EventHandler(this.tsmi_Contains_Click);
            // 
            // testToolStripMenuItem
            // 
            this.testToolStripMenuItem.Name = "testToolStripMenuItem";
            this.testToolStripMenuItem.Size = new System.Drawing.Size(262, 6);
            // 
            // tsmi_C3_C4
            // 
            this.tsmi_C3_C4.BackColor = System.Drawing.SystemColors.Control;
            this.tsmi_C3_C4.CheckOnClick = true;
            this.tsmi_C3_C4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmi_C3_C4.Name = "tsmi_C3_C4";
            this.tsmi_C3_C4.Size = new System.Drawing.Size(265, 22);
            this.tsmi_C3_C4.Tag = "86160|86160";
            this.tsmi_C3_C4.Text = "C3 and C4";
            this.tsmi_C3_C4.Click += new System.EventHandler(this.tsmi_C3_C4_Click);
            // 
            // tssBilling_Mcloe
            // 
            this.tssBilling_Mcloe.BackColor = System.Drawing.SystemColors.Control;
            this.tssBilling_Mcloe.ForeColor = System.Drawing.Color.Red;
            this.tssBilling_Mcloe.Name = "tssBilling_Mcloe";
            this.tssBilling_Mcloe.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.tssBilling_Mcloe.Size = new System.Drawing.Size(6, 25);
            // 
            // tsddbtnMCLOEReports
            // 
            this.tsddbtnMCLOEReports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsddbtnMCLOEReports.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aBNsReportedToolStripMenuItem});
            this.tsddbtnMCLOEReports.Image = ((System.Drawing.Image)(resources.GetObject("tsddbtnMCLOEReports.Image")));
            this.tsddbtnMCLOEReports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsddbtnMCLOEReports.Name = "tsddbtnMCLOEReports";
            this.tsddbtnMCLOEReports.Size = new System.Drawing.Size(103, 22);
            this.tsddbtnMCLOEReports.Tag = "MCLOE|GOMCLLIVE";
            this.tsddbtnMCLOEReports.Text = "MCLOE Reports";
            this.tsddbtnMCLOEReports.Visible = false;
            this.tsddbtnMCLOEReports.DropDownOpening += new System.EventHandler(this.tsddbtnMCLOEReports_DropDownOpening);
            // 
            // aBNsReportedToolStripMenuItem
            // 
            this.aBNsReportedToolStripMenuItem.Name = "aBNsReportedToolStripMenuItem";
            this.aBNsReportedToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.aBNsReportedToolStripMenuItem.Text = "ABN\'s Reported";
            this.aBNsReportedToolStripMenuItem.Click += new System.EventHandler(this.aBNsReportedToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbtnPrint
            // 
            this.tsbtnPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbtnPrint.Image = ((System.Drawing.Image)(resources.GetObject("tsbtnPrint.Image")));
            this.tsbtnPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbtnPrint.Name = "tsbtnPrint";
            this.tsbtnPrint.Size = new System.Drawing.Size(36, 22);
            this.tsbtnPrint.Text = "Print";
            this.tsbtnPrint.ToolTipText = "Print\'s the report in the viewer.";
            this.tsbtnPrint.Click += new System.EventHandler(this.tsbtnPrint_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tssbCDM
            // 
            this.tssbCDM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbCDM.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstbCDM1,
            this.tstbCDM2});
            this.tssbCDM.Image = ((System.Drawing.Image)(resources.GetObject("tssbCDM.Image")));
            this.tssbCDM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbCDM.Name = "tssbCDM";
            this.tssbCDM.Size = new System.Drawing.Size(88, 22);
            this.tssbCDM.Text = "CDM Report";
            this.tssbCDM.ButtonClick += new System.EventHandler(this.tssbCDM_ButtonClick);
            this.tssbCDM.DropDownOpening += new System.EventHandler(this.tssbCDM_DropDownOpening);
            // 
            // tstbCDM1
            // 
            this.tstbCDM1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbCDM1.Name = "tstbCDM1";
            this.tstbCDM1.Size = new System.Drawing.Size(100, 23);
            this.tstbCDM1.ToolTipText = "Add first CDM to be compaired";
            this.tstbCDM1.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tstbCDM_KeyUp);
            // 
            // tstbCDM2
            // 
            this.tstbCDM2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbCDM2.Name = "tstbCDM2";
            this.tstbCDM2.Size = new System.Drawing.Size(100, 23);
            this.tstbCDM2.ToolTipText = "Add second CDM to be compaired";
            this.tstbCDM2.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tstbCDM_KeyUp);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tssbTableReports
            // 
            this.tssbTableReports.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbTableReports.Image = ((System.Drawing.Image)(resources.GetObject("tssbTableReports.Image")));
            this.tssbTableReports.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbTableReports.Name = "tssbTableReports";
            this.tssbTableReports.Size = new System.Drawing.Size(122, 22);
            this.tssbTableReports.Text = "Reports from Table";
            this.tssbTableReports.ToolTipText = "Retrieve the report code from the Sql table for each item listed.";
            this.tssbTableReports.ButtonClick += new System.EventHandler(this.tssbTableReports_ButtonClick);
            this.tssbTableReports.DropDownOpened += new System.EventHandler(this.tsddbtnBillingReports_DropDownOpening);
            this.tssbTableReports.DropDownItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.tssbTableReports_DropDownItemClicked);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tssbExcel
            // 
            this.tssbExcel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbExcel.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExcelDirectory,
            this.tsmiExcelFile,
            this.tsmiExcelWorkSheetName});
            this.tssbExcel.Image = ((System.Drawing.Image)(resources.GetObject("tssbExcel.Image")));
            this.tssbExcel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbExcel.Name = "tssbExcel";
            this.tssbExcel.Size = new System.Drawing.Size(56, 22);
            this.tssbExcel.Text = "EXCEL";
            this.tssbExcel.ButtonClick += new System.EventHandler(this.tssbExcel_ButtonClick);
            // 
            // tsmiExcelDirectory
            // 
            this.tsmiExcelDirectory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmicbExcelDirectory});
            this.tsmiExcelDirectory.Name = "tsmiExcelDirectory";
            this.tsmiExcelDirectory.Size = new System.Drawing.Size(165, 22);
            this.tsmiExcelDirectory.Text = "Directory";
            // 
            // tsmicbExcelDirectory
            // 
            this.tsmicbExcelDirectory.Name = "tsmicbExcelDirectory";
            this.tsmicbExcelDirectory.Size = new System.Drawing.Size(152, 23);
            this.tsmicbExcelDirectory.Text = "C:\\TEMP\\";
            // 
            // tsmiExcelFile
            // 
            this.tsmiExcelFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstbExcelFileName});
            this.tsmiExcelFile.Name = "tsmiExcelFile";
            this.tsmiExcelFile.Size = new System.Drawing.Size(165, 22);
            this.tsmiExcelFile.Text = "File Name";
            // 
            // tstbExcelFileName
            // 
            this.tstbExcelFileName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbExcelFileName.Name = "tstbExcelFileName";
            this.tstbExcelFileName.Size = new System.Drawing.Size(152, 23);
            this.tstbExcelFileName.Text = "EXCEL";
            // 
            // tsmiExcelWorkSheetName
            // 
            this.tsmiExcelWorkSheetName.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tstbExcelWorkSheetName});
            this.tsmiExcelWorkSheetName.Name = "tsmiExcelWorkSheetName";
            this.tsmiExcelWorkSheetName.Size = new System.Drawing.Size(165, 22);
            this.tsmiExcelWorkSheetName.Text = "Worksheet Name";
            // 
            // tstbExcelWorkSheetName
            // 
            this.tstbExcelWorkSheetName.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.tstbExcelWorkSheetName.Name = "tstbExcelWorkSheetName";
            this.tstbExcelWorkSheetName.Size = new System.Drawing.Size(152, 23);
            this.tstbExcelWorkSheetName.Text = "Sheet1";
            this.tstbExcelWorkSheetName.ToolTipText = "Enter the worksheet name";
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tscbClients
            // 
            this.tscbClients.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbClients.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbClients.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.tscbClients.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.tscbClients.Name = "tscbClients";
            this.tscbClients.Size = new System.Drawing.Size(92, 25);
            this.tscbClients.Sorted = true;
            // 
            // ssMain
            // 
            this.ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsslReportTitle,
            this.tspbCount,
            this.tsslRecords,
            this.tsslDisplayReportTitle});
            this.ssMain.Location = new System.Drawing.Point(0, 410);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(798, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "Main Status Strip";
            // 
            // tsslReportTitle
            // 
            this.tsslReportTitle.Name = "tsslReportTitle";
            this.tsslReportTitle.Size = new System.Drawing.Size(0, 17);
            // 
            // tspbCount
            // 
            this.tspbCount.Name = "tspbCount";
            this.tspbCount.Size = new System.Drawing.Size(200, 16);
            // 
            // tsslRecords
            // 
            this.tsslRecords.Name = "tsslRecords";
            this.tsslRecords.Size = new System.Drawing.Size(0, 17);
            // 
            // tsslDisplayReportTitle
            // 
            this.tsslDisplayReportTitle.Name = "tsslDisplayReportTitle";
            this.tsslDisplayReportTitle.Size = new System.Drawing.Size(10, 17);
            this.tsslDisplayReportTitle.Text = "|";
            // 
            // m_dgvReport
            // 
            this.m_dgvReport.AllowUserToAddRows = false;
            this.m_dgvReport.AllowUserToOrderColumns = true;
            this.m_dgvReport.BackgroundColor = System.Drawing.Color.White;
            this.m_dgvReport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            this.m_dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_dgvReport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.m_dgvReport.Location = new System.Drawing.Point(0, 25);
            this.m_dgvReport.Name = "m_dgvReport";
            this.m_dgvReport.RowHeadersWidth = 75;
            this.m_dgvReport.RowTemplate.Height = 24;
            this.m_dgvReport.Size = new System.Drawing.Size(798, 385);
            this.m_dgvReport.TabIndex = 2;
            this.m_dgvReport.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_dgvReport_CellMouseClick);
            this.m_dgvReport.ColumnHeaderMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_dgvReport_ColumnHeaderMouseClick);
            this.m_dgvReport.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.m_dgvReport_DataError);
            this.m_dgvReport.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.m_dgvReport_RowHeaderMouseDoubleClick);
            this.m_dgvReport.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.m_dgvReport_RowsAdded);
            // 
            // cmsRecords
            // 
            this.cmsRecords.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.cmsRecords.Name = "cmsRecords";
            this.cmsRecords.Size = new System.Drawing.Size(61, 4);
            // 
            // niMsg
            // 
            this.niMsg.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.niMsg.Icon = ((System.Drawing.Icon)(resources.GetObject("niMsg.Icon")));
            this.niMsg.Text = "Monthly Reports";
            this.niMsg.Visible = true;
            // 
            // frmReports
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(798, 432);
            this.Controls.Add(this.m_dgvReport);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmReports";
            this.Text = "Monthly Reports";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmReports_FormClosing);
            this.Load += new System.EventHandler(this.frmReports_Load);
            this.Enter += new System.EventHandler(this.frmReports_Enter);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.ssMain.ResumeLayout(false);
            this.ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.m_dgvReport)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnBillingReports;
        private System.Windows.Forms.ToolStripMenuItem tsmi80299;
        private System.Windows.Forms.ToolStripSeparator tssBilling_Mcloe;
        private System.Windows.Forms.ToolStripButton tsbtnPrint;
        private System.Windows.Forms.DataGridView m_dgvReport;
        private System.Windows.Forms.ToolStripStatusLabel tsslReportTitle;
        private System.Windows.Forms.ToolStripMenuItem tsmi59Modis;
        private System.Windows.Forms.ToolStripMenuItem modifiersMultipleCpt4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mailerErrorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWithNoPatientRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWithNoInsuranceRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripDropDownButton tsddbtnMCLOEReports;
        private System.Windows.Forms.ToolStripMenuItem aBNsReportedToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem tsmiNeedModifiers;
        private System.Windows.Forms.ToolStripMenuItem tsmi_82575_82565;
        private System.Windows.Forms.ToolStripMenuItem tsmi_86157_86156;
        private System.Windows.Forms.ToolStripMenuItem tsmi_87081_87040;
        private System.Windows.Forms.ToolStripMenuItem tsmi_88360_88342;
        private System.Windows.Forms.ToolStripMenuItem tsmi_C3_C4;
        private System.Windows.Forms.ToolStripMenuItem tsmi_87591_withMultiples;
        private System.Windows.Forms.ToolStripSeparator testToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton tsddbtnUserDefined;
        private System.Windows.Forms.ToolStripComboBox tscbCPT4s;
        private System.Windows.Forms.ToolStripSplitButton tssbCDM;
        private System.Windows.Forms.ToolStripTextBox tstbCDM1;
        private System.Windows.Forms.ToolStripTextBox tstbCDM2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSplitButton tssbTableReports;
        private System.Windows.Forms.ContextMenuStrip cmsRecords;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSplitButton tssbExcel;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelDirectory;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelWorkSheetName;
        private System.Windows.Forms.ToolStripTextBox tstbExcelWorkSheetName;
        private System.Windows.Forms.ToolStripTextBox tstbExcelFileName;
        private System.Windows.Forms.ToolStripComboBox tsmicbExcelDirectory;
        private System.Windows.Forms.ToolStripProgressBar tspbCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripComboBox tscbClients;
        private System.Windows.Forms.ToolStripStatusLabel tsslRecords;
        private System.Windows.Forms.NotifyIcon niMsg;
        private System.Windows.Forms.ToolStripStatusLabel tsslDisplayReportTitle;
    }
}

