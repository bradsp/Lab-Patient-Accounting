namespace LabBilling;

partial class MainForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        MainFormMenu = new MenuStrip();
        billingToolStripMenuItem = new ToolStripMenuItem();
        accountToolStripMenuItem = new ToolStripMenuItem();
        worklistToolStripMenuItem = new ToolStripMenuItem();
        duplicateAccountsToolStripMenuItem = new ToolStripMenuItem();
        accountChargeEntryToolStripMenuItem = new ToolStripMenuItem();
        batchChargeEntryToolStripMenuItem = new ToolStripMenuItem();
        batchRemittanceToolStripMenuItem = new ToolStripMenuItem();
        claimBatchManagementToolStripMenuItem = new ToolStripMenuItem();
        clientBillsNewToolStripMenuItem = new ToolStripMenuItem();
        posting835RemitToolStripMenuItem = new ToolStripMenuItem();
        remittancePostingToolStripMenuItem = new ToolStripMenuItem();
        badDebtMaintenanceToolStripMenuItem = new ToolStripMenuItem();
        dictionariesToolStripMenuItem = new ToolStripMenuItem();
        auditReportsToolStripMenuItem = new ToolStripMenuItem();
        chargeMasterToolStripMenuItem1 = new ToolStripMenuItem();
        clientsToolStripMenuItem = new ToolStripMenuItem();
        financialClassToolStripMenuItem = new ToolStripMenuItem();
        iCDDxCodesToolStripMenuItem = new ToolStripMenuItem();
        insurancePlansToolStripMenuItem = new ToolStripMenuItem();
        physiciansToolStripMenuItem = new ToolStripMenuItem();
        pathologistsToolStripMenuItem = new ToolStripMenuItem();
        zipCodesToolStripMenuItem = new ToolStripMenuItem();
        reportsToolStripMenuItem = new ToolStripMenuItem();
        codingStatsToolStripMenuItem = new ToolStripMenuItem();
        monthlyReportsToolStripMenuItem = new ToolStripMenuItem();
        paymentsToolStripMenuItem = new ToolStripMenuItem();
        randomDrugScreenToolStripMenuItem = new ToolStripMenuItem();
        addressRequisitionsToolStripMenuItem = new ToolStripMenuItem();
        accountingReportsToolStripMenuItem = new ToolStripMenuItem();
        aBNReportToolStripMenuItem = new ToolStripMenuItem();
        reportingPortalToolStripMenuItem = new ToolStripMenuItem();
        reportByInsuranceCompanyToolStripMenuItem = new ToolStripMenuItem();
        systemAdministrationToolStripMenuItem = new ToolStripMenuItem();
        userSecurityToolStripMenuItem = new ToolStripMenuItem();
        systemParametersToolStripMenuItem = new ToolStripMenuItem();
        interfaceMappingToolStripMenuItem = new ToolStripMenuItem();
        interfaceMonitorToolStripMenuItem = new ToolStripMenuItem();
        systemLogViewerToolStripMenuItem = new ToolStripMenuItem();
        windowToolStripMenuItem = new ToolStripMenuItem();
        helpToolStripMenuItem = new ToolStripMenuItem();
        documentationToolStripMenuItem = new ToolStripMenuItem();
        latestUpdatesToolStripMenuItem = new ToolStripMenuItem();
        aboutToolStripMenuItem = new ToolStripMenuItem();
        exitToolStripMenuItem = new ToolStripMenuItem();
        statusStrip1 = new StatusStrip();
        toolStripUsernameLabel = new ToolStripStatusLabel();
        toolStripDatabaseLabel = new ToolStripStatusLabel();
        panel1 = new Panel();
        helpProvider1 = new HelpProvider();
        mdiTabControl = new CustomTabControl();
        MainFormMenu.SuspendLayout();
        statusStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // MainFormMenu
        // 
        MainFormMenu.BackColor = Color.White;
        MainFormMenu.ImageScalingSize = new Size(20, 20);
        MainFormMenu.Items.AddRange(new ToolStripItem[] { billingToolStripMenuItem, dictionariesToolStripMenuItem, reportsToolStripMenuItem, systemAdministrationToolStripMenuItem, windowToolStripMenuItem, helpToolStripMenuItem });
        resources.ApplyResources(MainFormMenu, "MainFormMenu");
        MainFormMenu.MdiWindowListItem = windowToolStripMenuItem;
        MainFormMenu.Name = "MainFormMenu";
        // 
        // billingToolStripMenuItem
        // 
        billingToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { accountToolStripMenuItem, worklistToolStripMenuItem, duplicateAccountsToolStripMenuItem, accountChargeEntryToolStripMenuItem, batchChargeEntryToolStripMenuItem, batchRemittanceToolStripMenuItem, claimBatchManagementToolStripMenuItem, clientBillsNewToolStripMenuItem, posting835RemitToolStripMenuItem, remittancePostingToolStripMenuItem, badDebtMaintenanceToolStripMenuItem });
        billingToolStripMenuItem.Name = "billingToolStripMenuItem";
        resources.ApplyResources(billingToolStripMenuItem, "billingToolStripMenuItem");
        // 
        // accountToolStripMenuItem
        // 
        accountToolStripMenuItem.Name = "accountToolStripMenuItem";
        resources.ApplyResources(accountToolStripMenuItem, "accountToolStripMenuItem");
        accountToolStripMenuItem.Click += accountToolStripMenuItem_Click;
        // 
        // worklistToolStripMenuItem
        // 
        worklistToolStripMenuItem.Name = "worklistToolStripMenuItem";
        resources.ApplyResources(worklistToolStripMenuItem, "worklistToolStripMenuItem");
        worklistToolStripMenuItem.Click += worklistToolStripMenuItem_Click;
        // 
        // duplicateAccountsToolStripMenuItem
        // 
        duplicateAccountsToolStripMenuItem.Name = "duplicateAccountsToolStripMenuItem";
        resources.ApplyResources(duplicateAccountsToolStripMenuItem, "duplicateAccountsToolStripMenuItem");
        duplicateAccountsToolStripMenuItem.Click += duplicateAccountsToolStripMenuItem_Click;
        // 
        // accountChargeEntryToolStripMenuItem
        // 
        accountChargeEntryToolStripMenuItem.Name = "accountChargeEntryToolStripMenuItem";
        resources.ApplyResources(accountChargeEntryToolStripMenuItem, "accountChargeEntryToolStripMenuItem");
        accountChargeEntryToolStripMenuItem.Click += accountChargeEntryToolStripMenuItem_Click;
        // 
        // batchChargeEntryToolStripMenuItem
        // 
        batchChargeEntryToolStripMenuItem.Name = "batchChargeEntryToolStripMenuItem";
        resources.ApplyResources(batchChargeEntryToolStripMenuItem, "batchChargeEntryToolStripMenuItem");
        batchChargeEntryToolStripMenuItem.Click += batchChargeEntryToolStripMenuItem_Click;
        // 
        // batchRemittanceToolStripMenuItem
        // 
        batchRemittanceToolStripMenuItem.Name = "batchRemittanceToolStripMenuItem";
        resources.ApplyResources(batchRemittanceToolStripMenuItem, "batchRemittanceToolStripMenuItem");
        batchRemittanceToolStripMenuItem.Click += batchRemittanceToolStripMenuItem_Click;
        // 
        // claimBatchManagementToolStripMenuItem
        // 
        claimBatchManagementToolStripMenuItem.Name = "claimBatchManagementToolStripMenuItem";
        resources.ApplyResources(claimBatchManagementToolStripMenuItem, "claimBatchManagementToolStripMenuItem");
        claimBatchManagementToolStripMenuItem.Click += claimBatchManagementToolStripMenuItem_Click;
        // 
        // clientBillsNewToolStripMenuItem
        // 
        clientBillsNewToolStripMenuItem.Name = "clientBillsNewToolStripMenuItem";
        resources.ApplyResources(clientBillsNewToolStripMenuItem, "clientBillsNewToolStripMenuItem");
        clientBillsNewToolStripMenuItem.Click += clientBillsNewToolStripMenuItem_Click;
        // 
        // posting835RemitToolStripMenuItem
        // 
        posting835RemitToolStripMenuItem.Name = "posting835RemitToolStripMenuItem";
        resources.ApplyResources(posting835RemitToolStripMenuItem, "posting835RemitToolStripMenuItem");
        posting835RemitToolStripMenuItem.Click += posting835RemitToolStripMenuItem_Click;
        // 
        // remittancePostingToolStripMenuItem
        // 
        remittancePostingToolStripMenuItem.Name = "remittancePostingToolStripMenuItem";
        resources.ApplyResources(remittancePostingToolStripMenuItem, "remittancePostingToolStripMenuItem");
        remittancePostingToolStripMenuItem.Click += remittancePostingToolStripMenuItem_Click;
        // 
        // badDebtMaintenanceToolStripMenuItem
        // 
        badDebtMaintenanceToolStripMenuItem.Name = "badDebtMaintenanceToolStripMenuItem";
        resources.ApplyResources(badDebtMaintenanceToolStripMenuItem, "badDebtMaintenanceToolStripMenuItem");
        badDebtMaintenanceToolStripMenuItem.Click += badDebtMaintenanceToolStripMenuItem_Click;
        // 
        // dictionariesToolStripMenuItem
        // 
        dictionariesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { auditReportsToolStripMenuItem, chargeMasterToolStripMenuItem1, clientsToolStripMenuItem, financialClassToolStripMenuItem, iCDDxCodesToolStripMenuItem, insurancePlansToolStripMenuItem, physiciansToolStripMenuItem, pathologistsToolStripMenuItem, zipCodesToolStripMenuItem });
        dictionariesToolStripMenuItem.Name = "dictionariesToolStripMenuItem";
        resources.ApplyResources(dictionariesToolStripMenuItem, "dictionariesToolStripMenuItem");
        // 
        // auditReportsToolStripMenuItem
        // 
        auditReportsToolStripMenuItem.Name = "auditReportsToolStripMenuItem";
        resources.ApplyResources(auditReportsToolStripMenuItem, "auditReportsToolStripMenuItem");
        auditReportsToolStripMenuItem.Click += auditReportsToolStripMenuItem_Click;
        // 
        // chargeMasterToolStripMenuItem1
        // 
        chargeMasterToolStripMenuItem1.Name = "chargeMasterToolStripMenuItem1";
        resources.ApplyResources(chargeMasterToolStripMenuItem1, "chargeMasterToolStripMenuItem1");
        chargeMasterToolStripMenuItem1.Click += chargeMasterToolStripMenuItem1_Click;
        // 
        // clientsToolStripMenuItem
        // 
        clientsToolStripMenuItem.Name = "clientsToolStripMenuItem";
        resources.ApplyResources(clientsToolStripMenuItem, "clientsToolStripMenuItem");
        clientsToolStripMenuItem.Click += clientsToolStripMenuItem_Click;
        // 
        // financialClassToolStripMenuItem
        // 
        financialClassToolStripMenuItem.Name = "financialClassToolStripMenuItem";
        resources.ApplyResources(financialClassToolStripMenuItem, "financialClassToolStripMenuItem");
        // 
        // iCDDxCodesToolStripMenuItem
        // 
        iCDDxCodesToolStripMenuItem.Name = "iCDDxCodesToolStripMenuItem";
        resources.ApplyResources(iCDDxCodesToolStripMenuItem, "iCDDxCodesToolStripMenuItem");
        // 
        // insurancePlansToolStripMenuItem
        // 
        insurancePlansToolStripMenuItem.Name = "insurancePlansToolStripMenuItem";
        resources.ApplyResources(insurancePlansToolStripMenuItem, "insurancePlansToolStripMenuItem");
        insurancePlansToolStripMenuItem.Click += insurancePlansToolStripMenuItem_Click;
        // 
        // physiciansToolStripMenuItem
        // 
        physiciansToolStripMenuItem.Name = "physiciansToolStripMenuItem";
        resources.ApplyResources(physiciansToolStripMenuItem, "physiciansToolStripMenuItem");
        physiciansToolStripMenuItem.Click += physiciansToolStripMenuItem_Click;
        // 
        // pathologistsToolStripMenuItem
        // 
        pathologistsToolStripMenuItem.Name = "pathologistsToolStripMenuItem";
        resources.ApplyResources(pathologistsToolStripMenuItem, "pathologistsToolStripMenuItem");
        pathologistsToolStripMenuItem.Click += pathologistsToolStripMenuItem_Click;
        // 
        // zipCodesToolStripMenuItem
        // 
        zipCodesToolStripMenuItem.Name = "zipCodesToolStripMenuItem";
        resources.ApplyResources(zipCodesToolStripMenuItem, "zipCodesToolStripMenuItem");
        // 
        // reportsToolStripMenuItem
        // 
        reportsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { codingStatsToolStripMenuItem, monthlyReportsToolStripMenuItem, paymentsToolStripMenuItem, randomDrugScreenToolStripMenuItem, addressRequisitionsToolStripMenuItem, accountingReportsToolStripMenuItem, aBNReportToolStripMenuItem, reportingPortalToolStripMenuItem, reportByInsuranceCompanyToolStripMenuItem });
        reportsToolStripMenuItem.Name = "reportsToolStripMenuItem";
        resources.ApplyResources(reportsToolStripMenuItem, "reportsToolStripMenuItem");
        // 
        // codingStatsToolStripMenuItem
        // 
        codingStatsToolStripMenuItem.Name = "codingStatsToolStripMenuItem";
        resources.ApplyResources(codingStatsToolStripMenuItem, "codingStatsToolStripMenuItem");
        // 
        // monthlyReportsToolStripMenuItem
        // 
        monthlyReportsToolStripMenuItem.Name = "monthlyReportsToolStripMenuItem";
        resources.ApplyResources(monthlyReportsToolStripMenuItem, "monthlyReportsToolStripMenuItem");
        monthlyReportsToolStripMenuItem.Click += monthlyReportsToolStripMenuItem_Click;
        // 
        // paymentsToolStripMenuItem
        // 
        paymentsToolStripMenuItem.Name = "paymentsToolStripMenuItem";
        resources.ApplyResources(paymentsToolStripMenuItem, "paymentsToolStripMenuItem");
        // 
        // randomDrugScreenToolStripMenuItem
        // 
        randomDrugScreenToolStripMenuItem.Name = "randomDrugScreenToolStripMenuItem";
        resources.ApplyResources(randomDrugScreenToolStripMenuItem, "randomDrugScreenToolStripMenuItem");
        // 
        // addressRequisitionsToolStripMenuItem
        // 
        addressRequisitionsToolStripMenuItem.Name = "addressRequisitionsToolStripMenuItem";
        resources.ApplyResources(addressRequisitionsToolStripMenuItem, "addressRequisitionsToolStripMenuItem");
        // 
        // accountingReportsToolStripMenuItem
        // 
        accountingReportsToolStripMenuItem.Name = "accountingReportsToolStripMenuItem";
        resources.ApplyResources(accountingReportsToolStripMenuItem, "accountingReportsToolStripMenuItem");
        // 
        // aBNReportToolStripMenuItem
        // 
        aBNReportToolStripMenuItem.Name = "aBNReportToolStripMenuItem";
        resources.ApplyResources(aBNReportToolStripMenuItem, "aBNReportToolStripMenuItem");
        // 
        // reportingPortalToolStripMenuItem
        // 
        reportingPortalToolStripMenuItem.Name = "reportingPortalToolStripMenuItem";
        resources.ApplyResources(reportingPortalToolStripMenuItem, "reportingPortalToolStripMenuItem");
        reportingPortalToolStripMenuItem.Click += reportingPortalToolStripMenuItem_Click;
        // 
        // reportByInsuranceCompanyToolStripMenuItem
        // 
        reportByInsuranceCompanyToolStripMenuItem.Name = "reportByInsuranceCompanyToolStripMenuItem";
        resources.ApplyResources(reportByInsuranceCompanyToolStripMenuItem, "reportByInsuranceCompanyToolStripMenuItem");
        reportByInsuranceCompanyToolStripMenuItem.Click += reportByInsuranceCompanyToolStripMenuItem_Click;
        // 
        // systemAdministrationToolStripMenuItem
        // 
        systemAdministrationToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { userSecurityToolStripMenuItem, systemParametersToolStripMenuItem, interfaceMappingToolStripMenuItem, interfaceMonitorToolStripMenuItem, systemLogViewerToolStripMenuItem });
        systemAdministrationToolStripMenuItem.Name = "systemAdministrationToolStripMenuItem";
        resources.ApplyResources(systemAdministrationToolStripMenuItem, "systemAdministrationToolStripMenuItem");
        // 
        // userSecurityToolStripMenuItem
        // 
        userSecurityToolStripMenuItem.Name = "userSecurityToolStripMenuItem";
        resources.ApplyResources(userSecurityToolStripMenuItem, "userSecurityToolStripMenuItem");
        userSecurityToolStripMenuItem.Click += userSecurityToolStripMenuItem_Click;
        // 
        // systemParametersToolStripMenuItem
        // 
        systemParametersToolStripMenuItem.Name = "systemParametersToolStripMenuItem";
        resources.ApplyResources(systemParametersToolStripMenuItem, "systemParametersToolStripMenuItem");
        systemParametersToolStripMenuItem.Click += systemParametersToolStripMenuItem_Click;
        // 
        // interfaceMappingToolStripMenuItem
        // 
        interfaceMappingToolStripMenuItem.Name = "interfaceMappingToolStripMenuItem";
        resources.ApplyResources(interfaceMappingToolStripMenuItem, "interfaceMappingToolStripMenuItem");
        interfaceMappingToolStripMenuItem.Click += interfaceMappingToolStripMenuItem_Click;
        // 
        // interfaceMonitorToolStripMenuItem
        // 
        interfaceMonitorToolStripMenuItem.Name = "interfaceMonitorToolStripMenuItem";
        resources.ApplyResources(interfaceMonitorToolStripMenuItem, "interfaceMonitorToolStripMenuItem");
        interfaceMonitorToolStripMenuItem.Click += interfaceMonitorToolStripMenuItem_Click;
        // 
        // systemLogViewerToolStripMenuItem
        // 
        systemLogViewerToolStripMenuItem.Name = "systemLogViewerToolStripMenuItem";
        resources.ApplyResources(systemLogViewerToolStripMenuItem, "systemLogViewerToolStripMenuItem");
        systemLogViewerToolStripMenuItem.Click += systemLogViewerToolStripMenuItem_Click;
        // 
        // windowToolStripMenuItem
        // 
        windowToolStripMenuItem.Name = "windowToolStripMenuItem";
        resources.ApplyResources(windowToolStripMenuItem, "windowToolStripMenuItem");
        // 
        // helpToolStripMenuItem
        // 
        helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { documentationToolStripMenuItem, latestUpdatesToolStripMenuItem, aboutToolStripMenuItem, exitToolStripMenuItem });
        helpToolStripMenuItem.Name = "helpToolStripMenuItem";
        resources.ApplyResources(helpToolStripMenuItem, "helpToolStripMenuItem");
        // 
        // documentationToolStripMenuItem
        // 
        documentationToolStripMenuItem.Name = "documentationToolStripMenuItem";
        resources.ApplyResources(documentationToolStripMenuItem, "documentationToolStripMenuItem");
        documentationToolStripMenuItem.Click += documentationToolStripMenuItem_Click;
        // 
        // latestUpdatesToolStripMenuItem
        // 
        latestUpdatesToolStripMenuItem.Name = "latestUpdatesToolStripMenuItem";
        resources.ApplyResources(latestUpdatesToolStripMenuItem, "latestUpdatesToolStripMenuItem");
        latestUpdatesToolStripMenuItem.Click += latestUpdatesToolStripMenuItem_Click;
        // 
        // aboutToolStripMenuItem
        // 
        aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
        resources.ApplyResources(aboutToolStripMenuItem, "aboutToolStripMenuItem");
        aboutToolStripMenuItem.Click += aboutToolStripMenuItem_Click;
        // 
        // exitToolStripMenuItem
        // 
        exitToolStripMenuItem.Name = "exitToolStripMenuItem";
        resources.ApplyResources(exitToolStripMenuItem, "exitToolStripMenuItem");
        exitToolStripMenuItem.Click += exitToolStripMenuItem_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripUsernameLabel, toolStripDatabaseLabel });
        resources.ApplyResources(statusStrip1, "statusStrip1");
        statusStrip1.Name = "statusStrip1";
        // 
        // toolStripUsernameLabel
        // 
        toolStripUsernameLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        toolStripUsernameLabel.BorderStyle = Border3DStyle.Sunken;
        toolStripUsernameLabel.Name = "toolStripUsernameLabel";
        resources.ApplyResources(toolStripUsernameLabel, "toolStripUsernameLabel");
        // 
        // toolStripDatabaseLabel
        // 
        toolStripDatabaseLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
        toolStripDatabaseLabel.BorderStyle = Border3DStyle.Sunken;
        toolStripDatabaseLabel.Name = "toolStripDatabaseLabel";
        resources.ApplyResources(toolStripDatabaseLabel, "toolStripDatabaseLabel");
        // 
        // panel1
        // 
        resources.ApplyResources(panel1, "panel1");
        panel1.Name = "panel1";
        // 
        // mdiTabControl
        // 
        // 
        // 
        // 
        mdiTabControl.DisplayStyleProvider.BorderColor = SystemColors.ControlDark;
        mdiTabControl.DisplayStyleProvider.BorderColorHot = SystemColors.ControlDark;
        mdiTabControl.DisplayStyleProvider.BorderColorSelected = Color.FromArgb(127, 157, 185);
        mdiTabControl.DisplayStyleProvider.CloserColor = Color.DarkGray;
        mdiTabControl.DisplayStyleProvider.CloserColorActive = Color.Red;
        mdiTabControl.DisplayStyleProvider.FocusTrack = false;
        mdiTabControl.DisplayStyleProvider.HotTrack = true;
        mdiTabControl.DisplayStyleProvider.ImageAlign = ContentAlignment.MiddleLeft;
        mdiTabControl.DisplayStyleProvider.Opacity = 1F;
        mdiTabControl.DisplayStyleProvider.Overlap = 0;
        mdiTabControl.DisplayStyleProvider.Padding = new Point(6, 5);
        mdiTabControl.DisplayStyleProvider.Radius = 3;
        mdiTabControl.DisplayStyleProvider.SelectedTextStyle = FontStyle.Regular;
        mdiTabControl.DisplayStyleProvider.ShowTabCloser = true;
        mdiTabControl.DisplayStyleProvider.TextColor = SystemColors.ControlText;
        mdiTabControl.DisplayStyleProvider.TextColorDisabled = SystemColors.ControlDark;
        mdiTabControl.DisplayStyleProvider.TextColorSelected = SystemColors.ControlText;
        resources.ApplyResources(mdiTabControl, "mdiTabControl");
        mdiTabControl.HotTrack = true;
        mdiTabControl.Multiline = true;
        mdiTabControl.Name = "mdiTabControl";
        mdiTabControl.SelectedIndex = 0;
        mdiTabControl.SelectedIndexChanged += mdiTabControl_SelectedIndexChanged;
        // 
        // MainForm
        // 
        resources.ApplyResources(this, "$this");
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        Controls.Add(mdiTabControl);
        Controls.Add(panel1);
        Controls.Add(statusStrip1);
        Controls.Add(MainFormMenu);
        IsMdiContainer = true;
        KeyPreview = true;
        MainMenuStrip = MainFormMenu;
        Name = "MainForm";
        WindowState = FormWindowState.Maximized;
        FormClosing += MainForm_FormClosing;
        Load += MainForm_Load;
        MdiChildActivate += MainForm_MdiChildActivate;
        KeyDown += MainForm_KeyDown;
        MainFormMenu.ResumeLayout(false);
        MainFormMenu.PerformLayout();
        statusStrip1.ResumeLayout(false);
        statusStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.MenuStrip MainFormMenu;
    private System.Windows.Forms.ToolStripMenuItem billingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem accountChargeEntryToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem batchRemittanceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem dictionariesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clientsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem financialClassToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem iCDDxCodesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem insurancePlansToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem physiciansToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem pathologistsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem zipCodesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem reportsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem codingStatsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem monthlyReportsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem paymentsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem randomDrugScreenToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem addressRequisitionsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem accountingReportsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aBNReportToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem reportingPortalToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem systemAdministrationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem userSecurityToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem systemParametersToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem badDebtMaintenanceToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem duplicateAccountsToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem reportByInsuranceCompanyToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem posting835RemitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem interfaceMappingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem clientBillsNewToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem interfaceMonitorToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem worklistToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem remittancePostingToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem chargeMasterToolStripMenuItem1;
    private System.Windows.Forms.StatusStrip statusStrip1;
    private System.Windows.Forms.ToolStripStatusLabel toolStripUsernameLabel;
    private System.Windows.Forms.ToolStripStatusLabel toolStripDatabaseLabel;
    private System.Windows.Forms.ToolStripMenuItem systemLogViewerToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem claimBatchManagementToolStripMenuItem;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.ToolStripMenuItem batchChargeEntryToolStripMenuItem;
    private System.Windows.Forms.HelpProvider helpProvider1;
    private System.Windows.Forms.ToolStripMenuItem documentationToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem latestUpdatesToolStripMenuItem;
    private System.Windows.Forms.CustomTabControl mdiTabControl;
    private System.Windows.Forms.ToolStripMenuItem windowToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem auditReportsToolStripMenuItem;
}