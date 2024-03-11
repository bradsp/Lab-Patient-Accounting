namespace LabBilling.Legacy
{
    partial class AuditReportsForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditReportsForm));
            tsMain = new System.Windows.Forms.ToolStrip();
            billingReportsTsmi = new System.Windows.Forms.ToolStripDropDownButton();
            jCodes80299tsmi = new System.Windows.Forms.ToolStripMenuItem();
            tsmi59Modis = new System.Windows.Forms.ToolStripMenuItem();
            modifiersMultipleCpt4ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            mailerErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            accountsWithNoPatientRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            accountsWithNoInsuranceRecordToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            needsModifiersTsmi = new System.Windows.Forms.ToolStripMenuItem();
            tsmi_82575_82565 = new System.Windows.Forms.ToolStripMenuItem();
            tsmi_86157_86156 = new System.Windows.Forms.ToolStripMenuItem();
            tsmi_87081_87040 = new System.Windows.Forms.ToolStripMenuItem();
            tsmi_88360_88342 = new System.Windows.Forms.ToolStripMenuItem();
            tsmi_87591_withMultiples = new System.Windows.Forms.ToolStripMenuItem();
            testToolStripMenuItem = new System.Windows.Forms.ToolStripSeparator();
            tsmi_C3_C4 = new System.Windows.Forms.ToolStripMenuItem();
            accountsWithSpecificCPTCodesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            accountsWithSpecificCDMsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            tableReportsToolStripItem = new System.Windows.Forms.ToolStripSplitButton();
            tssBilling_Mcloe = new System.Windows.Forms.ToolStripSeparator();
            printTsmi = new System.Windows.Forms.ToolStripButton();
            toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            excelToolStripButton = new System.Windows.Forms.ToolStripSplitButton();
            tsmiExcelDirectory = new System.Windows.Forms.ToolStripMenuItem();
            tsmicbExcelDirectory = new System.Windows.Forms.ToolStripComboBox();
            tsmiExcelFile = new System.Windows.Forms.ToolStripMenuItem();
            tstbExcelFileName = new System.Windows.Forms.ToolStripTextBox();
            tsmiExcelWorkSheetName = new System.Windows.Forms.ToolStripMenuItem();
            tstbExcelWorkSheetName = new System.Windows.Forms.ToolStripTextBox();
            toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            clientsToolStripComboBox = new System.Windows.Forms.ToolStripComboBox();
            ssMain = new System.Windows.Forms.StatusStrip();
            tsslReportTitle = new System.Windows.Forms.ToolStripStatusLabel();
            tspbCount = new System.Windows.Forms.ToolStripProgressBar();
            tsslRecords = new System.Windows.Forms.ToolStripStatusLabel();
            tsslDisplayReportTitle = new System.Windows.Forms.ToolStripStatusLabel();
            m_dgvReport = new System.Windows.Forms.DataGridView();
            cmsRecords = new System.Windows.Forms.ContextMenuStrip(components);
            niMsg = new System.Windows.Forms.NotifyIcon(components);
            reportStatusStrip = new System.Windows.Forms.StatusStrip();
            currentReportTitle = new System.Windows.Forms.ToolStripStatusLabel();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            tsMain.SuspendLayout();
            ssMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)m_dgvReport).BeginInit();
            reportStatusStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // tsMain
            // 
            tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            tsMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { billingReportsTsmi, tableReportsToolStripItem, tssBilling_Mcloe, printTsmi, toolStripSeparator4, excelToolStripButton, toolStripSeparator3, clientsToolStripComboBox });
            tsMain.Location = new System.Drawing.Point(0, 0);
            tsMain.Name = "tsMain";
            tsMain.Size = new System.Drawing.Size(931, 25);
            tsMain.TabIndex = 0;
            tsMain.Text = "Main Toolstrip";
            // 
            // billingReportsTsmi
            // 
            billingReportsTsmi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            billingReportsTsmi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { jCodes80299tsmi, tsmi59Modis, modifiersMultipleCpt4ToolStripMenuItem, mailerErrorsToolStripMenuItem, accountsWithNoPatientRecordToolStripMenuItem, accountsWithNoInsuranceRecordToolStripMenuItem, accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem, needsModifiersTsmi, accountsWithSpecificCPTCodesToolStripMenuItem, accountsWithSpecificCDMsToolStripMenuItem });
            billingReportsTsmi.Image = (System.Drawing.Image)resources.GetObject("billingReportsTsmi.Image");
            billingReportsTsmi.ImageTransparentColor = System.Drawing.Color.Magenta;
            billingReportsTsmi.Name = "billingReportsTsmi";
            billingReportsTsmi.Size = new System.Drawing.Size(96, 22);
            billingReportsTsmi.Tag = "WTHMCLBILL|MCLLIVE";
            billingReportsTsmi.Text = "Billing Reports";
            // 
            // jCodes80299tsmi
            // 
            jCodes80299tsmi.Name = "jCodes80299tsmi";
            jCodes80299tsmi.Size = new System.Drawing.Size(437, 22);
            jCodes80299tsmi.Text = "J Codes and 80299";
            jCodes80299tsmi.Click += tsmi80299_Click;
            // 
            // tsmi59Modis
            // 
            tsmi59Modis.Name = "tsmi59Modis";
            tsmi59Modis.Size = new System.Drawing.Size(437, 22);
            tsmi59Modis.Text = "59 Modifiers - Charge Details";
            tsmi59Modis.Click += tsmi59Modis_Click;
            // 
            // modifiersMultipleCpt4ToolStripMenuItem
            // 
            modifiersMultipleCpt4ToolStripMenuItem.Name = "modifiersMultipleCpt4ToolStripMenuItem";
            modifiersMultipleCpt4ToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            modifiersMultipleCpt4ToolStripMenuItem.Text = "59 Modifiers - Multiple Cpt4";
            modifiersMultipleCpt4ToolStripMenuItem.Click += modifiersMultipleCpt4ToolStripMenuItem_Click;
            // 
            // mailerErrorsToolStripMenuItem
            // 
            mailerErrorsToolStripMenuItem.Name = "mailerErrorsToolStripMenuItem";
            mailerErrorsToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            mailerErrorsToolStripMenuItem.Text = "Mailer Errors";
            mailerErrorsToolStripMenuItem.Click += mailerErrorsToolStripMenuItem_Click;
            // 
            // accountsWithNoPatientRecordToolStripMenuItem
            // 
            accountsWithNoPatientRecordToolStripMenuItem.Name = "accountsWithNoPatientRecordToolStripMenuItem";
            accountsWithNoPatientRecordToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            accountsWithNoPatientRecordToolStripMenuItem.Text = "Accounts with no Patient Record";
            accountsWithNoPatientRecordToolStripMenuItem.Click += accountsWithNoPatientRecordToolStripMenuItem_Click;
            // 
            // accountsWithNoInsuranceRecordToolStripMenuItem
            // 
            accountsWithNoInsuranceRecordToolStripMenuItem.Name = "accountsWithNoInsuranceRecordToolStripMenuItem";
            accountsWithNoInsuranceRecordToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            accountsWithNoInsuranceRecordToolStripMenuItem.Text = "Accounts with no Insurance Record";
            accountsWithNoInsuranceRecordToolStripMenuItem.Click += accountsWithNoInsuranceRecordToolStripMenuItem_Click;
            // 
            // accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem
            // 
            accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Name = "accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem";
            accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Text = "Accounts where fin code does not match primary insurance fin_code";
            accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem.Click += accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem_Click;
            // 
            // needsModifiersTsmi
            // 
            needsModifiersTsmi.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmi_82575_82565, tsmi_86157_86156, tsmi_87081_87040, tsmi_88360_88342, tsmi_87591_withMultiples, testToolStripMenuItem, tsmi_C3_C4 });
            needsModifiersTsmi.Name = "needsModifiersTsmi";
            needsModifiersTsmi.Size = new System.Drawing.Size(437, 22);
            needsModifiersTsmi.Text = "Needs Modifiers";
            // 
            // tsmi_82575_82565
            // 
            tsmi_82575_82565.Name = "tsmi_82575_82565";
            tsmi_82575_82565.Size = new System.Drawing.Size(265, 22);
            tsmi_82575_82565.Tag = "82575|82565";
            tsmi_82575_82565.Text = "82575 and 82565";
            tsmi_82575_82565.Click += tsmi_Contains_Click;
            // 
            // tsmi_86157_86156
            // 
            tsmi_86157_86156.Name = "tsmi_86157_86156";
            tsmi_86157_86156.Size = new System.Drawing.Size(265, 22);
            tsmi_86157_86156.Tag = "86157|86156";
            tsmi_86157_86156.Text = "86157 and 86156";
            tsmi_86157_86156.Click += tsmi_Contains_Click;
            // 
            // tsmi_87081_87040
            // 
            tsmi_87081_87040.Name = "tsmi_87081_87040";
            tsmi_87081_87040.Size = new System.Drawing.Size(265, 22);
            tsmi_87081_87040.Tag = "87081|87040";
            tsmi_87081_87040.Text = "87081 and 87040";
            tsmi_87081_87040.Click += tsmi_Contains_Click;
            // 
            // tsmi_88360_88342
            // 
            tsmi_88360_88342.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsmi_88360_88342.Name = "tsmi_88360_88342";
            tsmi_88360_88342.Size = new System.Drawing.Size(265, 22);
            tsmi_88360_88342.Tag = "88360|88342";
            tsmi_88360_88342.Text = "88360 and 88342";
            tsmi_88360_88342.Click += tsmi_Contains_Click;
            // 
            // tsmi_87591_withMultiples
            // 
            tsmi_87591_withMultiples.Name = "tsmi_87591_withMultiples";
            tsmi_87591_withMultiples.Size = new System.Drawing.Size(265, 22);
            tsmi_87591_withMultiples.Tag = "87591|83890','83891','83898";
            tsmi_87591_withMultiples.Text = "87591's with 83890 or 83891 or 83898";
            tsmi_87591_withMultiples.Click += tsmi_Contains_Click;
            // 
            // testToolStripMenuItem
            // 
            testToolStripMenuItem.Name = "testToolStripMenuItem";
            testToolStripMenuItem.Size = new System.Drawing.Size(262, 6);
            // 
            // tsmi_C3_C4
            // 
            tsmi_C3_C4.BackColor = System.Drawing.SystemColors.Control;
            tsmi_C3_C4.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tsmi_C3_C4.Name = "tsmi_C3_C4";
            tsmi_C3_C4.Size = new System.Drawing.Size(265, 22);
            tsmi_C3_C4.Tag = "86160|86160";
            tsmi_C3_C4.Text = "C3 and C4";
            tsmi_C3_C4.Click += tsmi_C3_C4_Click;
            // 
            // accountsWithSpecificCPTCodesToolStripMenuItem
            // 
            accountsWithSpecificCPTCodesToolStripMenuItem.Name = "accountsWithSpecificCPTCodesToolStripMenuItem";
            accountsWithSpecificCPTCodesToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            accountsWithSpecificCPTCodesToolStripMenuItem.Text = "Accounts with Specific CPT codes";
            accountsWithSpecificCPTCodesToolStripMenuItem.Click += accountsWithSpecificCpt_Click;
            // 
            // accountsWithSpecificCDMsToolStripMenuItem
            // 
            accountsWithSpecificCDMsToolStripMenuItem.Name = "accountsWithSpecificCDMsToolStripMenuItem";
            accountsWithSpecificCDMsToolStripMenuItem.Size = new System.Drawing.Size(437, 22);
            accountsWithSpecificCDMsToolStripMenuItem.Text = "Accounts with Specific CDMs";
            accountsWithSpecificCDMsToolStripMenuItem.Click += accountsWithSpecificCDMsToolStripMenuItem_Click;
            // 
            // tableReportsToolStripItem
            // 
            tableReportsToolStripItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            tableReportsToolStripItem.Image = (System.Drawing.Image)resources.GetObject("tableReportsToolStripItem.Image");
            tableReportsToolStripItem.ImageTransparentColor = System.Drawing.Color.Magenta;
            tableReportsToolStripItem.Name = "tableReportsToolStripItem";
            tableReportsToolStripItem.Size = new System.Drawing.Size(122, 22);
            tableReportsToolStripItem.Text = "Reports from Table";
            tableReportsToolStripItem.ToolTipText = "Retrieve the report code from the Sql table for each item listed.";
            tableReportsToolStripItem.ButtonClick += tssbTableReports_ButtonClick;
            tableReportsToolStripItem.DropDownItemClicked += tssbTableReports_DropDownItemClicked;
            // 
            // tssBilling_Mcloe
            // 
            tssBilling_Mcloe.BackColor = System.Drawing.SystemColors.Control;
            tssBilling_Mcloe.ForeColor = System.Drawing.Color.Red;
            tssBilling_Mcloe.Name = "tssBilling_Mcloe";
            tssBilling_Mcloe.Padding = new System.Windows.Forms.Padding(2, 0, 2, 0);
            tssBilling_Mcloe.Size = new System.Drawing.Size(6, 25);
            // 
            // printTsmi
            // 
            printTsmi.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            printTsmi.Image = (System.Drawing.Image)resources.GetObject("printTsmi.Image");
            printTsmi.ImageTransparentColor = System.Drawing.Color.Magenta;
            printTsmi.Name = "printTsmi";
            printTsmi.Size = new System.Drawing.Size(36, 22);
            printTsmi.Text = "Print";
            printTsmi.ToolTipText = "Print's the report in the viewer.";
            printTsmi.Click += tsbtnPrint_Click;
            // 
            // toolStripSeparator4
            // 
            toolStripSeparator4.Name = "toolStripSeparator4";
            toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // excelToolStripButton
            // 
            excelToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            excelToolStripButton.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmiExcelDirectory, tsmiExcelFile, tsmiExcelWorkSheetName });
            excelToolStripButton.Image = (System.Drawing.Image)resources.GetObject("excelToolStripButton.Image");
            excelToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            excelToolStripButton.Name = "excelToolStripButton";
            excelToolStripButton.Size = new System.Drawing.Size(56, 22);
            excelToolStripButton.Text = "EXCEL";
            excelToolStripButton.ButtonClick += tssbExcel_ButtonClick;
            // 
            // tsmiExcelDirectory
            // 
            tsmiExcelDirectory.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmicbExcelDirectory });
            tsmiExcelDirectory.Name = "tsmiExcelDirectory";
            tsmiExcelDirectory.Size = new System.Drawing.Size(180, 22);
            tsmiExcelDirectory.Text = "Directory";
            // 
            // tsmicbExcelDirectory
            // 
            tsmicbExcelDirectory.Name = "tsmicbExcelDirectory";
            tsmicbExcelDirectory.Size = new System.Drawing.Size(152, 23);
            tsmicbExcelDirectory.Text = "C:\\TEMP\\";
            // 
            // tsmiExcelFile
            // 
            tsmiExcelFile.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tstbExcelFileName });
            tsmiExcelFile.Name = "tsmiExcelFile";
            tsmiExcelFile.Size = new System.Drawing.Size(180, 22);
            tsmiExcelFile.Text = "File Name";
            // 
            // tstbExcelFileName
            // 
            tstbExcelFileName.Name = "tstbExcelFileName";
            tstbExcelFileName.Size = new System.Drawing.Size(152, 23);
            tstbExcelFileName.Text = "EXCEL";
            // 
            // tsmiExcelWorkSheetName
            // 
            tsmiExcelWorkSheetName.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tstbExcelWorkSheetName });
            tsmiExcelWorkSheetName.Name = "tsmiExcelWorkSheetName";
            tsmiExcelWorkSheetName.Size = new System.Drawing.Size(180, 22);
            tsmiExcelWorkSheetName.Text = "Worksheet Name";
            // 
            // tstbExcelWorkSheetName
            // 
            tstbExcelWorkSheetName.Name = "tstbExcelWorkSheetName";
            tstbExcelWorkSheetName.Size = new System.Drawing.Size(152, 23);
            tstbExcelWorkSheetName.Text = "Sheet1";
            tstbExcelWorkSheetName.ToolTipText = "Enter the worksheet name";
            // 
            // toolStripSeparator3
            // 
            toolStripSeparator3.Name = "toolStripSeparator3";
            toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // clientsToolStripComboBox
            // 
            clientsToolStripComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            clientsToolStripComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            clientsToolStripComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            clientsToolStripComboBox.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            clientsToolStripComboBox.Name = "clientsToolStripComboBox";
            clientsToolStripComboBox.Size = new System.Drawing.Size(200, 25);
            clientsToolStripComboBox.Sorted = true;
            // 
            // ssMain
            // 
            ssMain.ImageScalingSize = new System.Drawing.Size(20, 20);
            ssMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { tsslReportTitle, tspbCount, tsslRecords, tsslDisplayReportTitle });
            ssMain.Location = new System.Drawing.Point(0, 474);
            ssMain.Name = "ssMain";
            ssMain.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            ssMain.Size = new System.Drawing.Size(931, 24);
            ssMain.TabIndex = 1;
            ssMain.Text = "Main Status Strip";
            // 
            // tsslReportTitle
            // 
            tsslReportTitle.Name = "tsslReportTitle";
            tsslReportTitle.Size = new System.Drawing.Size(0, 19);
            // 
            // tspbCount
            // 
            tspbCount.Name = "tspbCount";
            tspbCount.Size = new System.Drawing.Size(233, 18);
            // 
            // tsslRecords
            // 
            tsslRecords.Name = "tsslRecords";
            tsslRecords.Size = new System.Drawing.Size(0, 19);
            // 
            // tsslDisplayReportTitle
            // 
            tsslDisplayReportTitle.Name = "tsslDisplayReportTitle";
            tsslDisplayReportTitle.Size = new System.Drawing.Size(10, 19);
            tsslDisplayReportTitle.Text = "|";
            // 
            // m_dgvReport
            // 
            m_dgvReport.AllowUserToAddRows = false;
            m_dgvReport.AllowUserToOrderColumns = true;
            m_dgvReport.BackgroundColor = System.Drawing.Color.White;
            m_dgvReport.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText;
            m_dgvReport.Dock = System.Windows.Forms.DockStyle.Fill;
            m_dgvReport.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            m_dgvReport.Location = new System.Drawing.Point(0, 0);
            m_dgvReport.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            m_dgvReport.Name = "m_dgvReport";
            m_dgvReport.RowHeadersWidth = 75;
            m_dgvReport.RowTemplate.Height = 24;
            m_dgvReport.Size = new System.Drawing.Size(787, 427);
            m_dgvReport.TabIndex = 2;
            m_dgvReport.CellMouseClick += m_dgvReport_CellMouseClick;
            m_dgvReport.ColumnHeaderMouseClick += m_dgvReport_ColumnHeaderMouseClick;
            m_dgvReport.DataError += m_dgvReport_DataError;
            m_dgvReport.RowHeaderMouseDoubleClick += m_dgvReport_RowHeaderMouseDoubleClick;
            m_dgvReport.RowsAdded += m_dgvReport_RowsAdded;
            // 
            // cmsRecords
            // 
            cmsRecords.ImageScalingSize = new System.Drawing.Size(20, 20);
            cmsRecords.Name = "cmsRecords";
            cmsRecords.Size = new System.Drawing.Size(61, 4);
            // 
            // niMsg
            // 
            niMsg.BalloonTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            niMsg.Icon = (System.Drawing.Icon)resources.GetObject("niMsg.Icon");
            niMsg.Text = "Monthly Reports";
            niMsg.Visible = true;
            // 
            // reportStatusStrip
            // 
            reportStatusStrip.Dock = System.Windows.Forms.DockStyle.Top;
            reportStatusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { currentReportTitle });
            reportStatusStrip.Location = new System.Drawing.Point(0, 25);
            reportStatusStrip.Name = "reportStatusStrip";
            reportStatusStrip.Size = new System.Drawing.Size(931, 22);
            reportStatusStrip.TabIndex = 3;
            reportStatusStrip.Text = "statusStrip1";
            // 
            // currentReportTitle
            // 
            currentReportTitle.Name = "currentReportTitle";
            currentReportTitle.Size = new System.Drawing.Size(40, 17);
            currentReportTitle.Text = "...........";
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer1.Location = new System.Drawing.Point(0, 47);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(m_dgvReport);
            splitContainer1.Size = new System.Drawing.Size(931, 427);
            splitContainer1.SplitterDistance = 140;
            splitContainer1.TabIndex = 4;
            // 
            // AuditReportsForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(931, 498);
            Controls.Add(splitContainer1);
            Controls.Add(reportStatusStrip);
            Controls.Add(ssMain);
            Controls.Add(tsMain);
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "AuditReportsForm";
            Text = "Audit Reports";
            WindowState = System.Windows.Forms.FormWindowState.Maximized;
            FormClosing += frmReports_FormClosing;
            Load += MonthlyReportsForm_Load;
            Enter += frmReports_Enter;
            tsMain.ResumeLayout(false);
            tsMain.PerformLayout();
            ssMain.ResumeLayout(false);
            ssMain.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)m_dgvReport).EndInit();
            reportStatusStrip.ResumeLayout(false);
            reportStatusStrip.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripDropDownButton billingReportsTsmi;
        private System.Windows.Forms.ToolStripMenuItem jCodes80299tsmi;
        private System.Windows.Forms.ToolStripSeparator tssBilling_Mcloe;
        private System.Windows.Forms.ToolStripButton printTsmi;
        private System.Windows.Forms.DataGridView m_dgvReport;
        private System.Windows.Forms.ToolStripStatusLabel tsslReportTitle;
        private System.Windows.Forms.ToolStripMenuItem tsmi59Modis;
        private System.Windows.Forms.ToolStripMenuItem modifiersMultipleCpt4ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem mailerErrorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWithNoPatientRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWithNoInsuranceRecordToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWhereFinCodeDoesNotMatchPrimaryInsuranceFincodeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem needsModifiersTsmi;
        private System.Windows.Forms.ToolStripMenuItem tsmi_82575_82565;
        private System.Windows.Forms.ToolStripMenuItem tsmi_86157_86156;
        private System.Windows.Forms.ToolStripMenuItem tsmi_87081_87040;
        private System.Windows.Forms.ToolStripMenuItem tsmi_88360_88342;
        private System.Windows.Forms.ToolStripMenuItem tsmi_C3_C4;
        private System.Windows.Forms.ToolStripMenuItem tsmi_87591_withMultiples;
        private System.Windows.Forms.ToolStripSeparator testToolStripMenuItem;
        private System.Windows.Forms.ToolStripSplitButton tableReportsToolStripItem;
        private System.Windows.Forms.ContextMenuStrip cmsRecords;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripSplitButton excelToolStripButton;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelDirectory;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelFile;
        private System.Windows.Forms.ToolStripMenuItem tsmiExcelWorkSheetName;
        private System.Windows.Forms.ToolStripTextBox tstbExcelWorkSheetName;
        private System.Windows.Forms.ToolStripTextBox tstbExcelFileName;
        private System.Windows.Forms.ToolStripComboBox tsmicbExcelDirectory;
        private System.Windows.Forms.ToolStripProgressBar tspbCount;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripComboBox clientsToolStripComboBox;
        private System.Windows.Forms.ToolStripStatusLabel tsslRecords;
        private System.Windows.Forms.NotifyIcon niMsg;
        private System.Windows.Forms.ToolStripStatusLabel tsslDisplayReportTitle;
        private System.Windows.Forms.StatusStrip reportStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel currentReportTitle;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ToolStripMenuItem accountsWithSpecificCPTCodesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem accountsWithSpecificCDMsToolStripMenuItem;
    }
}

