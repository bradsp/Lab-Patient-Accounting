

namespace LabBilling.Forms
{
    partial class AccountForm
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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle11 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle12 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle13 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle14 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle15 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AccountForm));
            tabControl1 = new TabControl();
            summaryTab = new TabPage();
            summaryTable = new TableLayoutPanel();
            tabDemographics = new TabPage();
            demographicsLayoutPanel = new TableLayoutPanel();
            DemoStatusMessagesTextBox = new TextBox();
            GuarZipTextBox = new MaskedTextBox();
            GuarZipCodeLabel = new Label();
            GuarStateLabel = new Label();
            GuarStateComboBox = new Library.FlatCombo();
            GuarCityTextBox = new TextBox();
            GuarCityLabel = new Label();
            GuarantorAddressTextBox = new TextBox();
            GuarAddressLabel = new Label();
            SaveDemographics = new Button();
            DateOfBirthTextBox = new UserControls.DateTextBox();
            PatLastNameLabel = new Label();
            orderingProviderLabel = new Label();
            PatFirstNameLabel = new Label();
            SocSecNoTextBox = new MaskedTextBox();
            FirstNameTextBox = new TextBox();
            PatSSNLabel = new Label();
            PatMiddleNameLabel = new Label();
            MaritalStatusComboBox = new Library.FlatCombo();
            PatMaritalStatusLabel = new Label();
            PatDOBLabel = new Label();
            MiddleNameTextBox = new TextBox();
            PatSexLabel = new Label();
            SexComboBox = new Library.FlatCombo();
            PatPhoneLabel = new Label();
            PhoneTextBox = new TextBox();
            PatZipLabel = new Label();
            ZipcodeTextBox = new MaskedTextBox();
            PatStateLabel = new Label();
            StateComboBox = new Library.FlatCombo();
            PatCityLabel = new Label();
            CityTextBox = new TextBox();
            Address2TextBox = new TextBox();
            Address1TextBox = new TextBox();
            PatAddressLabel = new Label();
            PatSuffixLabel = new Label();
            SuffixTextBox = new TextBox();
            PatEmailLabel = new Label();
            EmailAddressTextBox = new TextBox();
            label6 = new Label();
            label7 = new Label();
            PatRelationLabel = new Label();
            GuarantorRelationComboBox = new Library.FlatCombo();
            GuarCopyPatientLink = new LinkLabel();
            GuarLastNameLabel = new Label();
            GuarantorLastNameTextBox = new TextBox();
            GuarFirstNameLabel = new Label();
            GuarFirstNameTextBox = new TextBox();
            GuarMiddleNameLabel = new Label();
            GuarMiddleNameTextBox = new TextBox();
            GuarSuffixLabel = new Label();
            GuarSuffixTextBox = new TextBox();
            GuarPhoneLabel = new Label();
            GuarantorPhoneTextBox = new TextBox();
            LastNameTextBox = new TextBox();
            orderingPhyTextBox = new TextBox();
            tabDiagnosis = new TabPage();
            dxPointerGrid2 = new DataGridView();
            DxDeleteButton = new PictureBox();
            SaveDxButton = new Button();
            QuickAddLabel = new Label();
            DxQuickAddTextBox = new TextBox();
            txtSearchDx = new TextBox();
            label4 = new Label();
            DxSearchLabel = new Label();
            DxSearchDataGrid = new DataGridView();
            SelectedDxLabel = new Label();
            DiagnosisDataGrid = new DataGridView();
            DxSearchButton = new PictureBox();
            tabInsPrimary = new TabPage();
            tabInsSecondary = new TabPage();
            tabInsTertiary = new TabPage();
            tabCharges = new TabPage();
            tabPayments = new TabPage();
            AddPaymentButton = new Button();
            PmtTotalPmtAdjLabel = new Label();
            TotalPmtAllTextBox = new TextBox();
            PmtTotalWriteOffLabel = new Label();
            TotalWriteOffTextBox = new TextBox();
            PmtTotalContractualLabel = new Label();
            TotalContractualTextBox = new TextBox();
            PmtTotalPaymentLabel = new Label();
            TotalPaymentTextBox = new TextBox();
            PaymentsDataGrid = new DataGridView();
            tabNotes = new TabPage();
            notesDataGridView = new UserControls.LabDataGridView();
            noteAlertCheckBox = new CheckBox();
            AddNoteButton = new Button();
            tabBillingActivity = new TabPage();
            label5 = new Label();
            statementHistoryDataGrid = new DataGridView();
            clearClaimStatusButton = new Button();
            readyToBillCheckbox = new CheckBox();
            statementFlagComboBox = new ComboBox();
            label3 = new Label();
            label2 = new Label();
            label1 = new Label();
            lastStmtDateTextBox = new TextBox();
            firstStmtDateTextBox = new TextBox();
            statementFlagLabel = new Label();
            GenerateClaimButton = new Button();
            LastValidatedLabel = new Label();
            BillingLastValidatedLabel = new Label();
            ValidationResultsTextBox = new TextBox();
            ValidateAccountButton = new Button();
            BillActivityDataGrid = new DataGridView();
            minPmtTextBox = new UserControls.CurrencyTextBox();
            noteTextContextMenu = new ContextMenuStrip(components);
            copyToolStripMenuItem = new ToolStripMenuItem();
            menuStrip1 = new MenuStrip();
            accountToolStripMenuItem = new ToolStripMenuItem();
            changeDateOfServiceToolStripMenuItem = new ToolStripMenuItem();
            changeFinancialClassToolStripMenuItem = new ToolStripMenuItem();
            changeClientToolStripMenuItem = new ToolStripMenuItem();
            viewAuditInfoToolStripMenuItem = new ToolStripMenuItem();
            clearHoldStatusToolStripMenuItem = new ToolStripMenuItem();
            swapInsurancesToolStripMenuItem = new ToolStripMenuItem();
            moveAllChargesToolStripMenuItem = new ToolStripMenuItem();
            BannerMRNTextBox = new TextBox();
            BannerAccountTextBox = new TextBox();
            BannerSexTextBox = new TextBox();
            BannerDobTextBox = new TextBox();
            BannerNameTextBox = new TextBox();
            BannerDOBSexLabel = new Label();
            BannerAccountMrnLabel = new Label();
            BannerClientTextBox = new TextBox();
            BannerFinClassTextBox = new TextBox();
            BannerFinClassLabel = new Label();
            BannerTotalPmtLabel = new Label();
            BannerTotalChargesLabel = new Label();
            BannerAccBalanceLabel = new Label();
            TotalPmtAdjLabel = new Label();
            BalanceLabel = new Label();
            TotalChargesLabel = new Label();
            BannerBillStatusLabel = new Label();
            BannerBillStatusTextBox = new TextBox();
            BannerProviderTextBox = new TextBox();
            RefreshButton = new PictureBox();
            bannerDateOfServiceTextBox = new TextBox();
            bannerDateOfServiceLabel = new Label();
            dxPointerMenuStrip = new ContextMenuStrip(components);
            clearDxPointerToolStripMenuItem = new ToolStripMenuItem();
            bannerAlertLabel = new Label();
            BannerThirdPartyBalLabel = new Label();
            ThirdPartyBalLabel = new Label();
            BannerClientBalLabel = new Label();
            ClientBalLabel = new Label();
            bannerPanel = new Panel();
            tabControl1.SuspendLayout();
            summaryTab.SuspendLayout();
            tabDemographics.SuspendLayout();
            demographicsLayoutPanel.SuspendLayout();
            tabDiagnosis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dxPointerGrid2).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DxDeleteButton).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DxSearchDataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DiagnosisDataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)DxSearchButton).BeginInit();
            tabPayments.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)PaymentsDataGrid).BeginInit();
            tabNotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)notesDataGridView).BeginInit();
            tabBillingActivity.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)statementHistoryDataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)BillActivityDataGrid).BeginInit();
            noteTextContextMenu.SuspendLayout();
            menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)RefreshButton).BeginInit();
            dxPointerMenuStrip.SuspendLayout();
            bannerPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(summaryTab);
            tabControl1.Controls.Add(tabDemographics);
            tabControl1.Controls.Add(tabDiagnosis);
            tabControl1.Controls.Add(tabInsPrimary);
            tabControl1.Controls.Add(tabInsSecondary);
            tabControl1.Controls.Add(tabInsTertiary);
            tabControl1.Controls.Add(tabCharges);
            tabControl1.Controls.Add(tabPayments);
            tabControl1.Controls.Add(tabNotes);
            tabControl1.Controls.Add(tabBillingActivity);
            tabControl1.HotTrack = true;
            tabControl1.Location = new Point(0, 130);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(1352, 596);
            tabControl1.TabIndex = 2;
            tabControl1.Selected += tabControl1_Selected;
            // 
            // summaryTab
            // 
            summaryTab.BackColor = Color.White;
            summaryTab.Controls.Add(summaryTable);
            summaryTab.Location = new Point(4, 24);
            summaryTab.Margin = new Padding(4, 3, 4, 3);
            summaryTab.Name = "summaryTab";
            summaryTab.Padding = new Padding(4, 3, 4, 3);
            summaryTab.Size = new Size(1344, 568);
            summaryTab.TabIndex = 0;
            summaryTab.Text = "Summary";
            // 
            // summaryTable
            // 
            summaryTable.ColumnCount = 4;
            summaryTable.ColumnStyles.Add(new ColumnStyle());
            summaryTable.ColumnStyles.Add(new ColumnStyle());
            summaryTable.ColumnStyles.Add(new ColumnStyle());
            summaryTable.ColumnStyles.Add(new ColumnStyle());
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 23F));
            summaryTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 23F));
            summaryTable.Dock = DockStyle.Fill;
            summaryTable.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            summaryTable.Location = new Point(4, 3);
            summaryTable.Margin = new Padding(4, 3, 4, 3);
            summaryTable.Name = "summaryTable";
            summaryTable.RowCount = 1;
            summaryTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            summaryTable.Size = new Size(1336, 562);
            summaryTable.TabIndex = 0;
            // 
            // tabDemographics
            // 
            tabDemographics.AutoScroll = true;
            tabDemographics.BackColor = Color.White;
            tabDemographics.Controls.Add(demographicsLayoutPanel);
            tabDemographics.Location = new Point(4, 24);
            tabDemographics.Margin = new Padding(4, 3, 4, 3);
            tabDemographics.Name = "tabDemographics";
            tabDemographics.Padding = new Padding(4, 3, 4, 3);
            tabDemographics.Size = new Size(1344, 568);
            tabDemographics.TabIndex = 9;
            tabDemographics.Text = "Demographics";
            // 
            // demographicsLayoutPanel
            // 
            demographicsLayoutPanel.ColumnCount = 4;
            demographicsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            demographicsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
            demographicsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 12F));
            demographicsLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 46F));
            demographicsLayoutPanel.Controls.Add(DemoStatusMessagesTextBox, 3, 17);
            demographicsLayoutPanel.Controls.Add(GuarZipTextBox, 3, 9);
            demographicsLayoutPanel.Controls.Add(GuarZipCodeLabel, 2, 9);
            demographicsLayoutPanel.Controls.Add(GuarStateLabel, 2, 8);
            demographicsLayoutPanel.Controls.Add(GuarStateComboBox, 3, 8);
            demographicsLayoutPanel.Controls.Add(GuarCityTextBox, 3, 7);
            demographicsLayoutPanel.Controls.Add(GuarCityLabel, 2, 7);
            demographicsLayoutPanel.Controls.Add(GuarantorAddressTextBox, 3, 6);
            demographicsLayoutPanel.Controls.Add(GuarAddressLabel, 2, 6);
            demographicsLayoutPanel.Controls.Add(SaveDemographics, 0, 17);
            demographicsLayoutPanel.Controls.Add(DateOfBirthTextBox, 1, 12);
            demographicsLayoutPanel.Controls.Add(PatLastNameLabel, 0, 1);
            demographicsLayoutPanel.Controls.Add(orderingProviderLabel, 0, 16);
            demographicsLayoutPanel.Controls.Add(PatFirstNameLabel, 0, 2);
            demographicsLayoutPanel.Controls.Add(SocSecNoTextBox, 1, 15);
            demographicsLayoutPanel.Controls.Add(FirstNameTextBox, 1, 2);
            demographicsLayoutPanel.Controls.Add(PatSSNLabel, 0, 15);
            demographicsLayoutPanel.Controls.Add(PatMiddleNameLabel, 0, 3);
            demographicsLayoutPanel.Controls.Add(MaritalStatusComboBox, 1, 14);
            demographicsLayoutPanel.Controls.Add(PatMaritalStatusLabel, 0, 14);
            demographicsLayoutPanel.Controls.Add(PatDOBLabel, 0, 12);
            demographicsLayoutPanel.Controls.Add(MiddleNameTextBox, 1, 3);
            demographicsLayoutPanel.Controls.Add(PatSexLabel, 0, 11);
            demographicsLayoutPanel.Controls.Add(SexComboBox, 1, 11);
            demographicsLayoutPanel.Controls.Add(PatPhoneLabel, 0, 10);
            demographicsLayoutPanel.Controls.Add(PhoneTextBox, 1, 10);
            demographicsLayoutPanel.Controls.Add(PatZipLabel, 0, 9);
            demographicsLayoutPanel.Controls.Add(ZipcodeTextBox, 1, 9);
            demographicsLayoutPanel.Controls.Add(PatStateLabel, 0, 8);
            demographicsLayoutPanel.Controls.Add(StateComboBox, 1, 8);
            demographicsLayoutPanel.Controls.Add(PatCityLabel, 0, 7);
            demographicsLayoutPanel.Controls.Add(CityTextBox, 1, 7);
            demographicsLayoutPanel.Controls.Add(Address2TextBox, 1, 6);
            demographicsLayoutPanel.Controls.Add(Address1TextBox, 1, 5);
            demographicsLayoutPanel.Controls.Add(PatAddressLabel, 0, 5);
            demographicsLayoutPanel.Controls.Add(PatSuffixLabel, 0, 4);
            demographicsLayoutPanel.Controls.Add(SuffixTextBox, 1, 4);
            demographicsLayoutPanel.Controls.Add(PatEmailLabel, 0, 13);
            demographicsLayoutPanel.Controls.Add(EmailAddressTextBox, 1, 13);
            demographicsLayoutPanel.Controls.Add(label6, 1, 0);
            demographicsLayoutPanel.Controls.Add(label7, 3, 0);
            demographicsLayoutPanel.Controls.Add(PatRelationLabel, 2, 1);
            demographicsLayoutPanel.Controls.Add(GuarantorRelationComboBox, 3, 1);
            demographicsLayoutPanel.Controls.Add(GuarCopyPatientLink, 2, 0);
            demographicsLayoutPanel.Controls.Add(GuarLastNameLabel, 2, 2);
            demographicsLayoutPanel.Controls.Add(GuarantorLastNameTextBox, 3, 2);
            demographicsLayoutPanel.Controls.Add(GuarFirstNameLabel, 2, 3);
            demographicsLayoutPanel.Controls.Add(GuarFirstNameTextBox, 3, 3);
            demographicsLayoutPanel.Controls.Add(GuarMiddleNameLabel, 2, 4);
            demographicsLayoutPanel.Controls.Add(GuarMiddleNameTextBox, 3, 4);
            demographicsLayoutPanel.Controls.Add(GuarSuffixLabel, 2, 5);
            demographicsLayoutPanel.Controls.Add(GuarSuffixTextBox, 3, 5);
            demographicsLayoutPanel.Controls.Add(GuarPhoneLabel, 2, 10);
            demographicsLayoutPanel.Controls.Add(GuarantorPhoneTextBox, 3, 10);
            demographicsLayoutPanel.Controls.Add(LastNameTextBox, 1, 1);
            demographicsLayoutPanel.Controls.Add(orderingPhyTextBox, 1, 16);
            demographicsLayoutPanel.Dock = DockStyle.Fill;
            demographicsLayoutPanel.Location = new Point(4, 3);
            demographicsLayoutPanel.Name = "demographicsLayoutPanel";
            demographicsLayoutPanel.RowCount = 18;
            demographicsLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.RowStyles.Add(new RowStyle());
            demographicsLayoutPanel.Size = new Size(1336, 562);
            demographicsLayoutPanel.TabIndex = 0;
            // 
            // DemoStatusMessagesTextBox
            // 
            DemoStatusMessagesTextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right;
            DemoStatusMessagesTextBox.BorderStyle = BorderStyle.None;
            DemoStatusMessagesTextBox.Location = new Point(724, 509);
            DemoStatusMessagesTextBox.Margin = new Padding(4, 3, 4, 3);
            DemoStatusMessagesTextBox.Multiline = true;
            DemoStatusMessagesTextBox.Name = "DemoStatusMessagesTextBox";
            DemoStatusMessagesTextBox.ScrollBars = ScrollBars.Vertical;
            DemoStatusMessagesTextBox.Size = new Size(608, 37);
            DemoStatusMessagesTextBox.TabIndex = 33;
            // 
            // GuarZipTextBox
            // 
            GuarZipTextBox.Location = new Point(724, 265);
            GuarZipTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarZipTextBox.Mask = "00000-9999";
            GuarZipTextBox.Name = "GuarZipTextBox";
            GuarZipTextBox.Size = new Size(125, 23);
            GuarZipTextBox.TabIndex = 51;
            // 
            // GuarZipCodeLabel
            // 
            GuarZipCodeLabel.AutoSize = true;
            GuarZipCodeLabel.Location = new Point(564, 262);
            GuarZipCodeLabel.Margin = new Padding(4, 0, 4, 0);
            GuarZipCodeLabel.Name = "GuarZipCodeLabel";
            GuarZipCodeLabel.Size = new Size(52, 15);
            GuarZipCodeLabel.TabIndex = 50;
            GuarZipCodeLabel.Text = "ZipCode";
            GuarZipCodeLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarStateLabel
            // 
            GuarStateLabel.AutoSize = true;
            GuarStateLabel.Location = new Point(564, 233);
            GuarStateLabel.Margin = new Padding(4, 0, 4, 0);
            GuarStateLabel.Name = "GuarStateLabel";
            GuarStateLabel.Size = new Size(33, 15);
            GuarStateLabel.TabIndex = 48;
            GuarStateLabel.Text = "State";
            GuarStateLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarStateComboBox
            // 
            GuarStateComboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            GuarStateComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            GuarStateComboBox.BackColor = Color.White;
            GuarStateComboBox.BorderColor = Color.Blue;
            GuarStateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            GuarStateComboBox.FlatStyle = FlatStyle.Flat;
            GuarStateComboBox.FormattingEnabled = true;
            GuarStateComboBox.Location = new Point(724, 236);
            GuarStateComboBox.Margin = new Padding(4, 3, 4, 3);
            GuarStateComboBox.Name = "GuarStateComboBox";
            GuarStateComboBox.Size = new Size(213, 23);
            GuarStateComboBox.TabIndex = 49;
            // 
            // GuarCityTextBox
            // 
            GuarCityTextBox.Location = new Point(724, 207);
            GuarCityTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarCityTextBox.Name = "GuarCityTextBox";
            GuarCityTextBox.Size = new Size(346, 23);
            GuarCityTextBox.TabIndex = 47;
            // 
            // GuarCityLabel
            // 
            GuarCityLabel.AutoSize = true;
            GuarCityLabel.Location = new Point(564, 204);
            GuarCityLabel.Margin = new Padding(4, 0, 4, 0);
            GuarCityLabel.Name = "GuarCityLabel";
            GuarCityLabel.Size = new Size(28, 15);
            GuarCityLabel.TabIndex = 46;
            GuarCityLabel.Text = "City";
            GuarCityLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarantorAddressTextBox
            // 
            GuarantorAddressTextBox.Location = new Point(724, 178);
            GuarantorAddressTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarantorAddressTextBox.Name = "GuarantorAddressTextBox";
            GuarantorAddressTextBox.Size = new Size(346, 23);
            GuarantorAddressTextBox.TabIndex = 45;
            // 
            // GuarAddressLabel
            // 
            GuarAddressLabel.AutoSize = true;
            GuarAddressLabel.Location = new Point(564, 175);
            GuarAddressLabel.Margin = new Padding(4, 0, 4, 0);
            GuarAddressLabel.Name = "GuarAddressLabel";
            GuarAddressLabel.Size = new Size(49, 15);
            GuarAddressLabel.TabIndex = 44;
            GuarAddressLabel.Text = "Address";
            GuarAddressLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // SaveDemographics
            // 
            SaveDemographics.Location = new Point(4, 497);
            SaveDemographics.Margin = new Padding(4, 3, 4, 3);
            SaveDemographics.Name = "SaveDemographics";
            SaveDemographics.Size = new Size(125, 51);
            SaveDemographics.TabIndex = 54;
            SaveDemographics.Text = "Save Changes";
            SaveDemographics.UseVisualStyleBackColor = true;
            SaveDemographics.Click += SaveDemographics_Click;
            // 
            // DateOfBirthTextBox
            // 
            DateOfBirthTextBox.DateValue = new DateTime(0L);
            DateOfBirthTextBox.Location = new Point(164, 352);
            DateOfBirthTextBox.Margin = new Padding(4, 3, 4, 3);
            DateOfBirthTextBox.Name = "DateOfBirthTextBox";
            DateOfBirthTextBox.Size = new Size(206, 23);
            DateOfBirthTextBox.TabIndex = 23;
            // 
            // PatLastNameLabel
            // 
            PatLastNameLabel.AutoSize = true;
            PatLastNameLabel.Location = new Point(4, 30);
            PatLastNameLabel.Margin = new Padding(4, 0, 4, 0);
            PatLastNameLabel.Name = "PatLastNameLabel";
            PatLastNameLabel.Size = new Size(63, 15);
            PatLastNameLabel.TabIndex = 1;
            PatLastNameLabel.Text = "Last Name";
            PatLastNameLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // orderingProviderLabel
            // 
            orderingProviderLabel.AutoSize = true;
            orderingProviderLabel.Location = new Point(4, 465);
            orderingProviderLabel.Margin = new Padding(4, 0, 4, 0);
            orderingProviderLabel.Name = "orderingProviderLabel";
            orderingProviderLabel.Size = new Size(101, 15);
            orderingProviderLabel.TabIndex = 30;
            orderingProviderLabel.Text = "Ordering Provider";
            orderingProviderLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // PatFirstNameLabel
            // 
            PatFirstNameLabel.AutoSize = true;
            PatFirstNameLabel.Location = new Point(4, 59);
            PatFirstNameLabel.Margin = new Padding(4, 0, 4, 0);
            PatFirstNameLabel.Name = "PatFirstNameLabel";
            PatFirstNameLabel.Size = new Size(64, 15);
            PatFirstNameLabel.TabIndex = 3;
            PatFirstNameLabel.Text = "First Name";
            PatFirstNameLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // SocSecNoTextBox
            // 
            SocSecNoTextBox.BorderStyle = BorderStyle.FixedSingle;
            SocSecNoTextBox.Location = new Point(164, 439);
            SocSecNoTextBox.Margin = new Padding(4, 3, 4, 3);
            SocSecNoTextBox.Mask = "000-00-0000";
            SocSecNoTextBox.Name = "SocSecNoTextBox";
            SocSecNoTextBox.Size = new Size(206, 23);
            SocSecNoTextBox.TabIndex = 29;
            // 
            // FirstNameTextBox
            // 
            FirstNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            FirstNameTextBox.CharacterCasing = CharacterCasing.Upper;
            FirstNameTextBox.Location = new Point(164, 62);
            FirstNameTextBox.Margin = new Padding(4, 3, 4, 3);
            FirstNameTextBox.Name = "FirstNameTextBox";
            FirstNameTextBox.Size = new Size(322, 23);
            FirstNameTextBox.TabIndex = 4;
            // 
            // PatSSNLabel
            // 
            PatSSNLabel.AutoSize = true;
            PatSSNLabel.Location = new Point(4, 436);
            PatSSNLabel.Margin = new Padding(4, 0, 4, 0);
            PatSSNLabel.Name = "PatSSNLabel";
            PatSSNLabel.Size = new Size(28, 15);
            PatSSNLabel.TabIndex = 28;
            PatSSNLabel.Text = "SSN";
            PatSSNLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // PatMiddleNameLabel
            // 
            PatMiddleNameLabel.AutoSize = true;
            PatMiddleNameLabel.Location = new Point(4, 88);
            PatMiddleNameLabel.Margin = new Padding(4, 0, 4, 0);
            PatMiddleNameLabel.Name = "PatMiddleNameLabel";
            PatMiddleNameLabel.Size = new Size(79, 15);
            PatMiddleNameLabel.TabIndex = 5;
            PatMiddleNameLabel.Text = "Middle Name";
            PatMiddleNameLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // MaritalStatusComboBox
            // 
            MaritalStatusComboBox.BackColor = Color.White;
            MaritalStatusComboBox.BorderColor = Color.Blue;
            MaritalStatusComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MaritalStatusComboBox.FlatStyle = FlatStyle.Flat;
            MaritalStatusComboBox.FormattingEnabled = true;
            MaritalStatusComboBox.Location = new Point(164, 410);
            MaritalStatusComboBox.Margin = new Padding(4, 3, 4, 3);
            MaritalStatusComboBox.Name = "MaritalStatusComboBox";
            MaritalStatusComboBox.Size = new Size(206, 23);
            MaritalStatusComboBox.TabIndex = 27;
            // 
            // PatMaritalStatusLabel
            // 
            PatMaritalStatusLabel.AutoSize = true;
            PatMaritalStatusLabel.Location = new Point(4, 407);
            PatMaritalStatusLabel.Margin = new Padding(4, 0, 4, 0);
            PatMaritalStatusLabel.Name = "PatMaritalStatusLabel";
            PatMaritalStatusLabel.Size = new Size(79, 15);
            PatMaritalStatusLabel.TabIndex = 26;
            PatMaritalStatusLabel.Text = "Marital Status";
            PatMaritalStatusLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // PatDOBLabel
            // 
            PatDOBLabel.AutoSize = true;
            PatDOBLabel.Location = new Point(4, 349);
            PatDOBLabel.Margin = new Padding(4, 0, 4, 0);
            PatDOBLabel.Name = "PatDOBLabel";
            PatDOBLabel.Size = new Size(73, 15);
            PatDOBLabel.TabIndex = 22;
            PatDOBLabel.Text = "Date of Birth";
            PatDOBLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // MiddleNameTextBox
            // 
            MiddleNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            MiddleNameTextBox.CharacterCasing = CharacterCasing.Upper;
            MiddleNameTextBox.Location = new Point(164, 91);
            MiddleNameTextBox.Margin = new Padding(4, 3, 4, 3);
            MiddleNameTextBox.Name = "MiddleNameTextBox";
            MiddleNameTextBox.Size = new Size(242, 23);
            MiddleNameTextBox.TabIndex = 6;
            // 
            // PatSexLabel
            // 
            PatSexLabel.AutoSize = true;
            PatSexLabel.Location = new Point(4, 320);
            PatSexLabel.Margin = new Padding(4, 0, 4, 0);
            PatSexLabel.Name = "PatSexLabel";
            PatSexLabel.Size = new Size(25, 15);
            PatSexLabel.TabIndex = 20;
            PatSexLabel.Text = "Sex";
            PatSexLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // SexComboBox
            // 
            SexComboBox.BackColor = Color.White;
            SexComboBox.BorderColor = Color.Blue;
            SexComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            SexComboBox.FlatStyle = FlatStyle.Flat;
            SexComboBox.FormattingEnabled = true;
            SexComboBox.Items.AddRange(new object[] { "Male", "Female", "Unknown" });
            SexComboBox.Location = new Point(164, 323);
            SexComboBox.Margin = new Padding(4, 3, 4, 3);
            SexComboBox.Name = "SexComboBox";
            SexComboBox.Size = new Size(206, 23);
            SexComboBox.TabIndex = 21;
            // 
            // PatPhoneLabel
            // 
            PatPhoneLabel.AutoSize = true;
            PatPhoneLabel.Location = new Point(4, 291);
            PatPhoneLabel.Margin = new Padding(4, 0, 4, 0);
            PatPhoneLabel.Name = "PatPhoneLabel";
            PatPhoneLabel.Size = new Size(41, 15);
            PatPhoneLabel.TabIndex = 18;
            PatPhoneLabel.Text = "Phone";
            PatPhoneLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // PhoneTextBox
            // 
            PhoneTextBox.BorderStyle = BorderStyle.FixedSingle;
            PhoneTextBox.Location = new Point(164, 294);
            PhoneTextBox.Margin = new Padding(4, 3, 4, 3);
            PhoneTextBox.Name = "PhoneTextBox";
            PhoneTextBox.Size = new Size(322, 23);
            PhoneTextBox.TabIndex = 19;
            // 
            // PatZipLabel
            // 
            PatZipLabel.AutoSize = true;
            PatZipLabel.Location = new Point(4, 262);
            PatZipLabel.Margin = new Padding(4, 0, 4, 0);
            PatZipLabel.Name = "PatZipLabel";
            PatZipLabel.Size = new Size(55, 15);
            PatZipLabel.TabIndex = 16;
            PatZipLabel.Text = "Zip Code";
            PatZipLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // ZipcodeTextBox
            // 
            ZipcodeTextBox.Location = new Point(164, 265);
            ZipcodeTextBox.Margin = new Padding(4, 3, 4, 3);
            ZipcodeTextBox.Mask = "00000-9999";
            ZipcodeTextBox.Name = "ZipcodeTextBox";
            ZipcodeTextBox.Size = new Size(119, 23);
            ZipcodeTextBox.TabIndex = 17;
            // 
            // PatStateLabel
            // 
            PatStateLabel.AutoSize = true;
            PatStateLabel.Location = new Point(4, 233);
            PatStateLabel.Margin = new Padding(4, 0, 4, 0);
            PatStateLabel.Name = "PatStateLabel";
            PatStateLabel.Size = new Size(33, 15);
            PatStateLabel.TabIndex = 14;
            PatStateLabel.Text = "State";
            PatStateLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // StateComboBox
            // 
            StateComboBox.AutoCompleteMode = AutoCompleteMode.Suggest;
            StateComboBox.AutoCompleteSource = AutoCompleteSource.ListItems;
            StateComboBox.BackColor = Color.White;
            StateComboBox.BorderColor = Color.Blue;
            StateComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            StateComboBox.FlatStyle = FlatStyle.Flat;
            StateComboBox.FormattingEnabled = true;
            StateComboBox.Location = new Point(164, 236);
            StateComboBox.Margin = new Padding(4, 3, 4, 3);
            StateComboBox.Name = "StateComboBox";
            StateComboBox.Size = new Size(206, 23);
            StateComboBox.TabIndex = 15;
            // 
            // PatCityLabel
            // 
            PatCityLabel.AutoSize = true;
            PatCityLabel.Location = new Point(4, 204);
            PatCityLabel.Margin = new Padding(4, 0, 4, 0);
            PatCityLabel.Name = "PatCityLabel";
            PatCityLabel.Size = new Size(28, 15);
            PatCityLabel.TabIndex = 12;
            PatCityLabel.Text = "City";
            PatCityLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // CityTextBox
            // 
            CityTextBox.BorderStyle = BorderStyle.FixedSingle;
            CityTextBox.CharacterCasing = CharacterCasing.Upper;
            CityTextBox.Location = new Point(164, 207);
            CityTextBox.Margin = new Padding(4, 3, 4, 3);
            CityTextBox.Name = "CityTextBox";
            CityTextBox.Size = new Size(322, 23);
            CityTextBox.TabIndex = 13;
            // 
            // Address2TextBox
            // 
            Address2TextBox.BorderStyle = BorderStyle.FixedSingle;
            Address2TextBox.CharacterCasing = CharacterCasing.Upper;
            Address2TextBox.Location = new Point(164, 178);
            Address2TextBox.Margin = new Padding(4, 3, 4, 3);
            Address2TextBox.Name = "Address2TextBox";
            Address2TextBox.Size = new Size(322, 23);
            Address2TextBox.TabIndex = 11;
            // 
            // Address1TextBox
            // 
            Address1TextBox.BorderStyle = BorderStyle.FixedSingle;
            Address1TextBox.CharacterCasing = CharacterCasing.Upper;
            Address1TextBox.Location = new Point(164, 149);
            Address1TextBox.Margin = new Padding(4, 3, 4, 3);
            Address1TextBox.Name = "Address1TextBox";
            Address1TextBox.Size = new Size(322, 23);
            Address1TextBox.TabIndex = 10;
            // 
            // PatAddressLabel
            // 
            PatAddressLabel.AutoSize = true;
            PatAddressLabel.Location = new Point(4, 146);
            PatAddressLabel.Margin = new Padding(4, 0, 4, 0);
            PatAddressLabel.Name = "PatAddressLabel";
            PatAddressLabel.Size = new Size(49, 15);
            PatAddressLabel.TabIndex = 9;
            PatAddressLabel.Text = "Address";
            PatAddressLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // PatSuffixLabel
            // 
            PatSuffixLabel.AutoSize = true;
            PatSuffixLabel.Location = new Point(4, 117);
            PatSuffixLabel.Margin = new Padding(4, 0, 4, 0);
            PatSuffixLabel.Name = "PatSuffixLabel";
            PatSuffixLabel.Size = new Size(37, 15);
            PatSuffixLabel.TabIndex = 7;
            PatSuffixLabel.Text = "Suffix";
            PatSuffixLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // SuffixTextBox
            // 
            SuffixTextBox.BorderStyle = BorderStyle.FixedSingle;
            SuffixTextBox.CharacterCasing = CharacterCasing.Upper;
            SuffixTextBox.Location = new Point(164, 120);
            SuffixTextBox.Margin = new Padding(4, 3, 4, 3);
            SuffixTextBox.Name = "SuffixTextBox";
            SuffixTextBox.Size = new Size(61, 23);
            SuffixTextBox.TabIndex = 8;
            // 
            // PatEmailLabel
            // 
            PatEmailLabel.AutoSize = true;
            PatEmailLabel.Location = new Point(4, 378);
            PatEmailLabel.Margin = new Padding(4, 0, 4, 0);
            PatEmailLabel.Name = "PatEmailLabel";
            PatEmailLabel.Size = new Size(81, 15);
            PatEmailLabel.TabIndex = 24;
            PatEmailLabel.Text = "Email Address";
            PatEmailLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // EmailAddressTextBox
            // 
            EmailAddressTextBox.BorderStyle = BorderStyle.FixedSingle;
            EmailAddressTextBox.Location = new Point(164, 381);
            EmailAddressTextBox.Margin = new Padding(4, 3, 4, 3);
            EmailAddressTextBox.Name = "EmailAddressTextBox";
            EmailAddressTextBox.Size = new Size(322, 23);
            EmailAddressTextBox.TabIndex = 25;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label6.Location = new Point(163, 0);
            label6.Name = "label6";
            label6.Size = new Size(188, 25);
            label6.TabIndex = 0;
            label6.Text = "Patient Information";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            label7.Location = new Point(723, 0);
            label7.Name = "label7";
            label7.Size = new Size(218, 25);
            label7.TabIndex = 33;
            label7.Text = "Guarantor Information";
            // 
            // PatRelationLabel
            // 
            PatRelationLabel.AutoSize = true;
            PatRelationLabel.Location = new Point(564, 30);
            PatRelationLabel.Margin = new Padding(4, 0, 4, 0);
            PatRelationLabel.Name = "PatRelationLabel";
            PatRelationLabel.Size = new Size(104, 15);
            PatRelationLabel.TabIndex = 34;
            PatRelationLabel.Text = "Relation to Patient";
            PatRelationLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // GuarantorRelationComboBox
            // 
            GuarantorRelationComboBox.BorderColor = Color.Blue;
            GuarantorRelationComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            GuarantorRelationComboBox.FlatStyle = FlatStyle.Flat;
            GuarantorRelationComboBox.FormattingEnabled = true;
            GuarantorRelationComboBox.Location = new Point(724, 33);
            GuarantorRelationComboBox.Margin = new Padding(4, 3, 4, 3);
            GuarantorRelationComboBox.Name = "GuarantorRelationComboBox";
            GuarantorRelationComboBox.Size = new Size(152, 23);
            GuarantorRelationComboBox.TabIndex = 35;
            // 
            // GuarCopyPatientLink
            // 
            GuarCopyPatientLink.AutoSize = true;
            GuarCopyPatientLink.Location = new Point(564, 0);
            GuarCopyPatientLink.Margin = new Padding(4, 0, 4, 0);
            GuarCopyPatientLink.Name = "GuarCopyPatientLink";
            GuarCopyPatientLink.Size = new Size(99, 15);
            GuarCopyPatientLink.TabIndex = 32;
            GuarCopyPatientLink.TabStop = true;
            GuarCopyPatientLink.Text = "Copy Patient Info";
            GuarCopyPatientLink.LinkClicked += GuarCopyPatientLink_LinkClicked;
            // 
            // GuarLastNameLabel
            // 
            GuarLastNameLabel.AutoSize = true;
            GuarLastNameLabel.Location = new Point(564, 59);
            GuarLastNameLabel.Margin = new Padding(4, 0, 4, 0);
            GuarLastNameLabel.Name = "GuarLastNameLabel";
            GuarLastNameLabel.Size = new Size(63, 15);
            GuarLastNameLabel.TabIndex = 36;
            GuarLastNameLabel.Text = "Last Name";
            GuarLastNameLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarantorLastNameTextBox
            // 
            GuarantorLastNameTextBox.Location = new Point(724, 62);
            GuarantorLastNameTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarantorLastNameTextBox.Name = "GuarantorLastNameTextBox";
            GuarantorLastNameTextBox.Size = new Size(346, 23);
            GuarantorLastNameTextBox.TabIndex = 37;
            // 
            // GuarFirstNameLabel
            // 
            GuarFirstNameLabel.AutoSize = true;
            GuarFirstNameLabel.Location = new Point(564, 88);
            GuarFirstNameLabel.Margin = new Padding(4, 0, 4, 0);
            GuarFirstNameLabel.Name = "GuarFirstNameLabel";
            GuarFirstNameLabel.Size = new Size(64, 15);
            GuarFirstNameLabel.TabIndex = 38;
            GuarFirstNameLabel.Text = "First Name";
            GuarFirstNameLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarFirstNameTextBox
            // 
            GuarFirstNameTextBox.Location = new Point(724, 91);
            GuarFirstNameTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarFirstNameTextBox.Name = "GuarFirstNameTextBox";
            GuarFirstNameTextBox.Size = new Size(346, 23);
            GuarFirstNameTextBox.TabIndex = 39;
            // 
            // GuarMiddleNameLabel
            // 
            GuarMiddleNameLabel.AutoSize = true;
            GuarMiddleNameLabel.Location = new Point(564, 117);
            GuarMiddleNameLabel.Margin = new Padding(4, 0, 4, 0);
            GuarMiddleNameLabel.Name = "GuarMiddleNameLabel";
            GuarMiddleNameLabel.Size = new Size(79, 15);
            GuarMiddleNameLabel.TabIndex = 40;
            GuarMiddleNameLabel.Text = "Middle Name";
            GuarMiddleNameLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarMiddleNameTextBox
            // 
            GuarMiddleNameTextBox.Location = new Point(724, 120);
            GuarMiddleNameTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarMiddleNameTextBox.Name = "GuarMiddleNameTextBox";
            GuarMiddleNameTextBox.Size = new Size(279, 23);
            GuarMiddleNameTextBox.TabIndex = 41;
            // 
            // GuarSuffixLabel
            // 
            GuarSuffixLabel.AutoSize = true;
            GuarSuffixLabel.Location = new Point(564, 146);
            GuarSuffixLabel.Margin = new Padding(4, 0, 4, 0);
            GuarSuffixLabel.Name = "GuarSuffixLabel";
            GuarSuffixLabel.Size = new Size(37, 15);
            GuarSuffixLabel.TabIndex = 42;
            GuarSuffixLabel.Text = "Suffix";
            GuarSuffixLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarSuffixTextBox
            // 
            GuarSuffixTextBox.Location = new Point(724, 149);
            GuarSuffixTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarSuffixTextBox.Name = "GuarSuffixTextBox";
            GuarSuffixTextBox.Size = new Size(58, 23);
            GuarSuffixTextBox.TabIndex = 43;
            // 
            // GuarPhoneLabel
            // 
            GuarPhoneLabel.AutoSize = true;
            GuarPhoneLabel.Location = new Point(564, 291);
            GuarPhoneLabel.Margin = new Padding(4, 0, 4, 0);
            GuarPhoneLabel.Name = "GuarPhoneLabel";
            GuarPhoneLabel.Size = new Size(41, 15);
            GuarPhoneLabel.TabIndex = 52;
            GuarPhoneLabel.Text = "Phone";
            GuarPhoneLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // GuarantorPhoneTextBox
            // 
            GuarantorPhoneTextBox.Location = new Point(724, 294);
            GuarantorPhoneTextBox.Margin = new Padding(4, 3, 4, 3);
            GuarantorPhoneTextBox.Name = "GuarantorPhoneTextBox";
            GuarantorPhoneTextBox.Size = new Size(346, 23);
            GuarantorPhoneTextBox.TabIndex = 53;
            // 
            // LastNameTextBox
            // 
            LastNameTextBox.BorderStyle = BorderStyle.FixedSingle;
            LastNameTextBox.CharacterCasing = CharacterCasing.Upper;
            LastNameTextBox.Location = new Point(164, 33);
            LastNameTextBox.Margin = new Padding(4, 3, 4, 3);
            LastNameTextBox.Name = "LastNameTextBox";
            LastNameTextBox.Size = new Size(322, 23);
            LastNameTextBox.TabIndex = 2;
            // 
            // orderingPhyTextBox
            // 
            orderingPhyTextBox.BorderStyle = BorderStyle.FixedSingle;
            orderingPhyTextBox.CharacterCasing = CharacterCasing.Upper;
            orderingPhyTextBox.Location = new Point(164, 468);
            orderingPhyTextBox.Margin = new Padding(4, 3, 4, 3);
            orderingPhyTextBox.Name = "orderingPhyTextBox";
            orderingPhyTextBox.Size = new Size(322, 23);
            orderingPhyTextBox.TabIndex = 31;
            orderingPhyTextBox.KeyUp += orderingPhyTextBox_KeyUp;
            // 
            // tabDiagnosis
            // 
            tabDiagnosis.AutoScroll = true;
            tabDiagnosis.BackColor = Color.White;
            tabDiagnosis.Controls.Add(dxPointerGrid2);
            tabDiagnosis.Controls.Add(DxDeleteButton);
            tabDiagnosis.Controls.Add(SaveDxButton);
            tabDiagnosis.Controls.Add(QuickAddLabel);
            tabDiagnosis.Controls.Add(DxQuickAddTextBox);
            tabDiagnosis.Controls.Add(txtSearchDx);
            tabDiagnosis.Controls.Add(label4);
            tabDiagnosis.Controls.Add(DxSearchLabel);
            tabDiagnosis.Controls.Add(DxSearchDataGrid);
            tabDiagnosis.Controls.Add(SelectedDxLabel);
            tabDiagnosis.Controls.Add(DiagnosisDataGrid);
            tabDiagnosis.Controls.Add(DxSearchButton);
            tabDiagnosis.Location = new Point(4, 24);
            tabDiagnosis.Margin = new Padding(4, 3, 4, 3);
            tabDiagnosis.Name = "tabDiagnosis";
            tabDiagnosis.Padding = new Padding(4, 3, 4, 3);
            tabDiagnosis.Size = new Size(1344, 568);
            tabDiagnosis.TabIndex = 8;
            tabDiagnosis.Text = "Diagnosis";
            // 
            // dxPointerGrid2
            // 
            dxPointerGrid2.AllowUserToAddRows = false;
            dxPointerGrid2.AllowUserToDeleteRows = false;
            dxPointerGrid2.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dxPointerGrid2.BackgroundColor = Color.White;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dxPointerGrid2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dxPointerGrid2.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dxPointerGrid2.DefaultCellStyle = dataGridViewCellStyle2;
            dxPointerGrid2.EditMode = DataGridViewEditMode.EditOnEnter;
            dxPointerGrid2.Location = new Point(22, 278);
            dxPointerGrid2.Margin = new Padding(4, 3, 4, 3);
            dxPointerGrid2.MultiSelect = false;
            dxPointerGrid2.Name = "dxPointerGrid2";
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            dxPointerGrid2.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            dxPointerGrid2.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dxPointerGrid2.Size = new Size(922, 283);
            dxPointerGrid2.TabIndex = 10;
            dxPointerGrid2.CellClick += dxPointerGrid2_CellClick;
            dxPointerGrid2.CellMouseDown += dxPointerGrid2_CellMouseDown;
            dxPointerGrid2.CellValueChanged += dxPointerGrid2_CellValueChanged;
            dxPointerGrid2.DataError += dxPointerGrid2_DataError;
            dxPointerGrid2.EditingControlShowing += dxPointerGrid2_EditingControlShowing;
            // 
            // DxDeleteButton
            // 
            DxDeleteButton.Image = Properties.Resources.hiclipart_com_id_dbhyp;
            DxDeleteButton.Location = new Point(951, 53);
            DxDeleteButton.Margin = new Padding(4, 3, 4, 3);
            DxDeleteButton.Name = "DxDeleteButton";
            DxDeleteButton.Size = new Size(23, 28);
            DxDeleteButton.SizeMode = PictureBoxSizeMode.StretchImage;
            DxDeleteButton.TabIndex = 9;
            DxDeleteButton.TabStop = false;
            DxDeleteButton.Click += DxDeleteButton_Click;
            // 
            // SaveDxButton
            // 
            SaveDxButton.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            SaveDxButton.Location = new Point(957, 7);
            SaveDxButton.Margin = new Padding(4, 3, 4, 3);
            SaveDxButton.Name = "SaveDxButton";
            SaveDxButton.Size = new Size(88, 32);
            SaveDxButton.TabIndex = 8;
            SaveDxButton.Text = "Save Diagnoses";
            SaveDxButton.UseVisualStyleBackColor = true;
            SaveDxButton.Visible = false;
            SaveDxButton.Click += SaveDxButton_Click;
            // 
            // QuickAddLabel
            // 
            QuickAddLabel.AutoSize = true;
            QuickAddLabel.Location = new Point(754, 20);
            QuickAddLabel.Margin = new Padding(4, 0, 4, 0);
            QuickAddLabel.Name = "QuickAddLabel";
            QuickAddLabel.Size = new Size(63, 15);
            QuickAddLabel.TabIndex = 7;
            QuickAddLabel.Text = "Quick Add";
            // 
            // DxQuickAddTextBox
            // 
            DxQuickAddTextBox.CharacterCasing = CharacterCasing.Upper;
            DxQuickAddTextBox.Location = new Point(827, 16);
            DxQuickAddTextBox.Margin = new Padding(4, 3, 4, 3);
            DxQuickAddTextBox.Name = "DxQuickAddTextBox";
            DxQuickAddTextBox.Size = new Size(116, 23);
            DxQuickAddTextBox.TabIndex = 6;
            DxQuickAddTextBox.KeyPress += DxQuickAddTextBox_KeyPress;
            // 
            // txtSearchDx
            // 
            txtSearchDx.CharacterCasing = CharacterCasing.Upper;
            txtSearchDx.Location = new Point(133, 21);
            txtSearchDx.Margin = new Padding(4, 3, 4, 3);
            txtSearchDx.Name = "txtSearchDx";
            txtSearchDx.Size = new Size(254, 23);
            txtSearchDx.TabIndex = 4;
            txtSearchDx.KeyPress += SearchDxTextBox_KeyPress;
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new Point(19, 260);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(104, 15);
            label4.TabIndex = 3;
            label4.Text = "Diagnosis Pointers";
            // 
            // DxSearchLabel
            // 
            DxSearchLabel.AutoSize = true;
            DxSearchLabel.Location = new Point(18, 24);
            DxSearchLabel.Margin = new Padding(4, 0, 4, 0);
            DxSearchLabel.Name = "DxSearchLabel";
            DxSearchLabel.Size = new Size(96, 15);
            DxSearchLabel.TabIndex = 3;
            DxSearchLabel.Text = "Diagnosis Search";
            // 
            // DxSearchDataGrid
            // 
            DxSearchDataGrid.AllowUserToAddRows = false;
            DxSearchDataGrid.AllowUserToDeleteRows = false;
            DxSearchDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            DxSearchDataGrid.BackgroundColor = Color.White;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            DxSearchDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            DxSearchDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            DxSearchDataGrid.DefaultCellStyle = dataGridViewCellStyle5;
            DxSearchDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            DxSearchDataGrid.Location = new Point(21, 53);
            DxSearchDataGrid.Margin = new Padding(4, 3, 4, 3);
            DxSearchDataGrid.Name = "DxSearchDataGrid";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            DxSearchDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            DxSearchDataGrid.RowHeadersVisible = false;
            DxSearchDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DxSearchDataGrid.Size = new Size(481, 203);
            DxSearchDataGrid.TabIndex = 2;
            DxSearchDataGrid.CellMouseDoubleClick += DxSearchDataGrid_CellMouseDoubleClick;
            DxSearchDataGrid.DataBindingComplete += DxSearchDataGrid_DataBindingComplete;
            // 
            // SelectedDxLabel
            // 
            SelectedDxLabel.AutoSize = true;
            SelectedDxLabel.Location = new Point(525, 35);
            SelectedDxLabel.Margin = new Padding(4, 0, 4, 0);
            SelectedDxLabel.Name = "SelectedDxLabel";
            SelectedDxLabel.Size = new Size(108, 15);
            SelectedDxLabel.TabIndex = 1;
            SelectedDxLabel.Text = "Selected Diagnoses";
            // 
            // DiagnosisDataGrid
            // 
            DiagnosisDataGrid.AllowUserToAddRows = false;
            DiagnosisDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            DiagnosisDataGrid.BackgroundColor = Color.White;
            dataGridViewCellStyle7.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = SystemColors.Control;
            dataGridViewCellStyle7.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle7.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = DataGridViewTriState.True;
            DiagnosisDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            DiagnosisDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle8.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = SystemColors.Window;
            dataGridViewCellStyle8.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle8.ForeColor = Color.Black;
            dataGridViewCellStyle8.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = DataGridViewTriState.False;
            DiagnosisDataGrid.DefaultCellStyle = dataGridViewCellStyle8;
            DiagnosisDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            DiagnosisDataGrid.Location = new Point(526, 53);
            DiagnosisDataGrid.Margin = new Padding(4, 3, 4, 3);
            DiagnosisDataGrid.Name = "DiagnosisDataGrid";
            dataGridViewCellStyle9.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = SystemColors.Control;
            dataGridViewCellStyle9.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle9.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = DataGridViewTriState.True;
            DiagnosisDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            DiagnosisDataGrid.RowHeadersVisible = false;
            DiagnosisDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            DiagnosisDataGrid.Size = new Size(418, 203);
            DiagnosisDataGrid.TabIndex = 0;
            DiagnosisDataGrid.CellMouseDoubleClick += DiagnosisDataGrid_CellMouseDoubleClick;
            DiagnosisDataGrid.DataBindingComplete += DiagnosisDataGrid_DataBindingComplete;
            DiagnosisDataGrid.RowsAdded += DiagnosisDataGrid_RowsAdded;
            DiagnosisDataGrid.RowsRemoved += DiagnosisDataGrid_RowsRemoved;
            DiagnosisDataGrid.UserDeletingRow += DiagnosisDataGrid_UserDeletingRow;
            // 
            // DxSearchButton
            // 
            DxSearchButton.Image = Properties.Resources.lookup_icon_png_and_vector_for_free_download_pngtree_lookup_png_512_512;
            DxSearchButton.Location = new Point(393, 17);
            DxSearchButton.Margin = new Padding(4, 3, 4, 3);
            DxSearchButton.Name = "DxSearchButton";
            DxSearchButton.Size = new Size(30, 28);
            DxSearchButton.SizeMode = PictureBoxSizeMode.StretchImage;
            DxSearchButton.TabIndex = 5;
            DxSearchButton.TabStop = false;
            DxSearchButton.Click += DxSearchButton_Click;
            // 
            // tabInsPrimary
            // 
            tabInsPrimary.Location = new Point(4, 24);
            tabInsPrimary.Name = "tabInsPrimary";
            tabInsPrimary.Padding = new Padding(3);
            tabInsPrimary.Size = new Size(1344, 568);
            tabInsPrimary.TabIndex = 15;
            tabInsPrimary.Text = "Primary Insurance";
            tabInsPrimary.UseVisualStyleBackColor = true;
            // 
            // tabInsSecondary
            // 
            tabInsSecondary.Location = new Point(4, 24);
            tabInsSecondary.Name = "tabInsSecondary";
            tabInsSecondary.Size = new Size(1344, 568);
            tabInsSecondary.TabIndex = 16;
            tabInsSecondary.Text = "Secondary Insurance";
            tabInsSecondary.UseVisualStyleBackColor = true;
            // 
            // tabInsTertiary
            // 
            tabInsTertiary.Location = new Point(4, 24);
            tabInsTertiary.Name = "tabInsTertiary";
            tabInsTertiary.Size = new Size(1344, 568);
            tabInsTertiary.TabIndex = 17;
            tabInsTertiary.Text = "Tertiary Insurance";
            tabInsTertiary.UseVisualStyleBackColor = true;
            // 
            // tabCharges
            // 
            tabCharges.Location = new Point(4, 24);
            tabCharges.Name = "tabCharges";
            tabCharges.Padding = new Padding(3);
            tabCharges.Size = new Size(1344, 568);
            tabCharges.TabIndex = 14;
            tabCharges.Text = "Charges";
            tabCharges.UseVisualStyleBackColor = true;
            // 
            // tabPayments
            // 
            tabPayments.AutoScroll = true;
            tabPayments.BackColor = Color.White;
            tabPayments.Controls.Add(AddPaymentButton);
            tabPayments.Controls.Add(PmtTotalPmtAdjLabel);
            tabPayments.Controls.Add(TotalPmtAllTextBox);
            tabPayments.Controls.Add(PmtTotalWriteOffLabel);
            tabPayments.Controls.Add(TotalWriteOffTextBox);
            tabPayments.Controls.Add(PmtTotalContractualLabel);
            tabPayments.Controls.Add(TotalContractualTextBox);
            tabPayments.Controls.Add(PmtTotalPaymentLabel);
            tabPayments.Controls.Add(TotalPaymentTextBox);
            tabPayments.Controls.Add(PaymentsDataGrid);
            tabPayments.Location = new Point(4, 24);
            tabPayments.Margin = new Padding(4, 3, 4, 3);
            tabPayments.Name = "tabPayments";
            tabPayments.Size = new Size(1344, 568);
            tabPayments.TabIndex = 7;
            tabPayments.Text = "Payments / Adjustments";
            // 
            // AddPaymentButton
            // 
            AddPaymentButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AddPaymentButton.Location = new Point(1116, 17);
            AddPaymentButton.Margin = new Padding(4, 3, 4, 3);
            AddPaymentButton.Name = "AddPaymentButton";
            AddPaymentButton.Size = new Size(202, 27);
            AddPaymentButton.TabIndex = 9;
            AddPaymentButton.Text = "Add Payment/Adjustment";
            AddPaymentButton.UseVisualStyleBackColor = true;
            AddPaymentButton.Click += AddPaymentButton_Click;
            // 
            // PmtTotalPmtAdjLabel
            // 
            PmtTotalPmtAdjLabel.AutoSize = true;
            PmtTotalPmtAdjLabel.Location = new Point(259, 29);
            PmtTotalPmtAdjLabel.Margin = new Padding(4, 0, 4, 0);
            PmtTotalPmtAdjLabel.Name = "PmtTotalPmtAdjLabel";
            PmtTotalPmtAdjLabel.Size = new Size(140, 15);
            PmtTotalPmtAdjLabel.TabIndex = 8;
            PmtTotalPmtAdjLabel.Text = "Total Pmt && Adjustments";
            // 
            // TotalPmtAllTextBox
            // 
            TotalPmtAllTextBox.Location = new Point(405, 25);
            TotalPmtAllTextBox.Margin = new Padding(4, 3, 4, 3);
            TotalPmtAllTextBox.Name = "TotalPmtAllTextBox";
            TotalPmtAllTextBox.ReadOnly = true;
            TotalPmtAllTextBox.Size = new Size(116, 23);
            TotalPmtAllTextBox.TabIndex = 7;
            TotalPmtAllTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // PmtTotalWriteOffLabel
            // 
            PmtTotalWriteOffLabel.AutoSize = true;
            PmtTotalWriteOffLabel.Location = new Point(28, 89);
            PmtTotalWriteOffLabel.Margin = new Padding(4, 0, 4, 0);
            PmtTotalWriteOffLabel.Name = "PmtTotalWriteOffLabel";
            PmtTotalWriteOffLabel.Size = new Size(83, 15);
            PmtTotalWriteOffLabel.TabIndex = 6;
            PmtTotalWriteOffLabel.Text = "Total Write Off";
            // 
            // TotalWriteOffTextBox
            // 
            TotalWriteOffTextBox.Location = new Point(124, 85);
            TotalWriteOffTextBox.Margin = new Padding(4, 3, 4, 3);
            TotalWriteOffTextBox.Name = "TotalWriteOffTextBox";
            TotalWriteOffTextBox.ReadOnly = true;
            TotalWriteOffTextBox.Size = new Size(116, 23);
            TotalWriteOffTextBox.TabIndex = 5;
            TotalWriteOffTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // PmtTotalContractualLabel
            // 
            PmtTotalContractualLabel.AutoSize = true;
            PmtTotalContractualLabel.Location = new Point(14, 59);
            PmtTotalContractualLabel.Margin = new Padding(4, 0, 4, 0);
            PmtTotalContractualLabel.Name = "PmtTotalContractualLabel";
            PmtTotalContractualLabel.Size = new Size(97, 15);
            PmtTotalContractualLabel.TabIndex = 4;
            PmtTotalContractualLabel.Text = "Total Contractual";
            // 
            // TotalContractualTextBox
            // 
            TotalContractualTextBox.Location = new Point(124, 55);
            TotalContractualTextBox.Margin = new Padding(4, 3, 4, 3);
            TotalContractualTextBox.Name = "TotalContractualTextBox";
            TotalContractualTextBox.ReadOnly = true;
            TotalContractualTextBox.Size = new Size(116, 23);
            TotalContractualTextBox.TabIndex = 3;
            TotalContractualTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // PmtTotalPaymentLabel
            // 
            PmtTotalPaymentLabel.AutoSize = true;
            PmtTotalPaymentLabel.Location = new Point(29, 29);
            PmtTotalPaymentLabel.Margin = new Padding(4, 0, 4, 0);
            PmtTotalPaymentLabel.Name = "PmtTotalPaymentLabel";
            PmtTotalPaymentLabel.Size = new Size(82, 15);
            PmtTotalPaymentLabel.TabIndex = 2;
            PmtTotalPaymentLabel.Text = "Total Payment";
            // 
            // TotalPaymentTextBox
            // 
            TotalPaymentTextBox.Location = new Point(124, 25);
            TotalPaymentTextBox.Margin = new Padding(4, 3, 4, 3);
            TotalPaymentTextBox.Name = "TotalPaymentTextBox";
            TotalPaymentTextBox.ReadOnly = true;
            TotalPaymentTextBox.Size = new Size(116, 23);
            TotalPaymentTextBox.TabIndex = 1;
            TotalPaymentTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // PaymentsDataGrid
            // 
            PaymentsDataGrid.AllowUserToAddRows = false;
            PaymentsDataGrid.AllowUserToDeleteRows = false;
            PaymentsDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dataGridViewCellStyle10.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = SystemColors.Control;
            dataGridViewCellStyle10.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle10.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = DataGridViewTriState.True;
            PaymentsDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            PaymentsDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = SystemColors.Window;
            dataGridViewCellStyle11.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle11.ForeColor = Color.Black;
            dataGridViewCellStyle11.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = DataGridViewTriState.False;
            PaymentsDataGrid.DefaultCellStyle = dataGridViewCellStyle11;
            PaymentsDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            PaymentsDataGrid.Location = new Point(4, 133);
            PaymentsDataGrid.Margin = new Padding(4, 3, 4, 3);
            PaymentsDataGrid.Name = "PaymentsDataGrid";
            PaymentsDataGrid.ReadOnly = true;
            dataGridViewCellStyle12.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = SystemColors.Control;
            dataGridViewCellStyle12.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle12.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = DataGridViewTriState.True;
            PaymentsDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            PaymentsDataGrid.RowHeadersVisible = false;
            PaymentsDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PaymentsDataGrid.Size = new Size(1336, 363);
            PaymentsDataGrid.TabIndex = 0;
            PaymentsDataGrid.CellDoubleClick += PaymentsDataGrid_CellDoubleClick;
            // 
            // tabNotes
            // 
            tabNotes.AutoScroll = true;
            tabNotes.BackColor = Color.White;
            tabNotes.Controls.Add(notesDataGridView);
            tabNotes.Controls.Add(noteAlertCheckBox);
            tabNotes.Controls.Add(AddNoteButton);
            tabNotes.Location = new Point(4, 24);
            tabNotes.Margin = new Padding(4, 3, 4, 3);
            tabNotes.Name = "tabNotes";
            tabNotes.Size = new Size(1344, 568);
            tabNotes.TabIndex = 5;
            tabNotes.Text = "Notes";
            // 
            // notesDataGridView
            // 
            notesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            notesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            notesDataGridView.Location = new Point(3, 46);
            notesDataGridView.Name = "notesDataGridView";
            notesDataGridView.RowTemplate.Height = 25;
            notesDataGridView.Size = new Size(1332, 515);
            notesDataGridView.TabIndex = 3;
            notesDataGridView.CellValueChanged += notesDataGridView_CellValueChanged;
            notesDataGridView.RowHeightChanged += notesDataGridView_RowHeightChanged;
            // 
            // noteAlertCheckBox
            // 
            noteAlertCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            noteAlertCheckBox.AutoSize = true;
            noteAlertCheckBox.Location = new Point(1138, 8);
            noteAlertCheckBox.Margin = new Padding(4, 3, 4, 3);
            noteAlertCheckBox.Name = "noteAlertCheckBox";
            noteAlertCheckBox.Size = new Size(80, 19);
            noteAlertCheckBox.TabIndex = 2;
            noteAlertCheckBox.Text = "Note Alert";
            noteAlertCheckBox.UseVisualStyleBackColor = true;
            noteAlertCheckBox.CheckedChanged += noteAlertCheckBox_CheckedChanged;
            // 
            // AddNoteButton
            // 
            AddNoteButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AddNoteButton.Location = new Point(1247, 3);
            AddNoteButton.Margin = new Padding(4, 3, 4, 3);
            AddNoteButton.Name = "AddNoteButton";
            AddNoteButton.Size = new Size(88, 27);
            AddNoteButton.TabIndex = 1;
            AddNoteButton.Text = "Add Note";
            AddNoteButton.UseVisualStyleBackColor = true;
            AddNoteButton.Click += AddNoteButton_Click;
            // 
            // tabBillingActivity
            // 
            tabBillingActivity.AutoScroll = true;
            tabBillingActivity.BackColor = Color.White;
            tabBillingActivity.Controls.Add(label5);
            tabBillingActivity.Controls.Add(statementHistoryDataGrid);
            tabBillingActivity.Controls.Add(clearClaimStatusButton);
            tabBillingActivity.Controls.Add(readyToBillCheckbox);
            tabBillingActivity.Controls.Add(statementFlagComboBox);
            tabBillingActivity.Controls.Add(label3);
            tabBillingActivity.Controls.Add(label2);
            tabBillingActivity.Controls.Add(label1);
            tabBillingActivity.Controls.Add(lastStmtDateTextBox);
            tabBillingActivity.Controls.Add(firstStmtDateTextBox);
            tabBillingActivity.Controls.Add(statementFlagLabel);
            tabBillingActivity.Controls.Add(GenerateClaimButton);
            tabBillingActivity.Controls.Add(LastValidatedLabel);
            tabBillingActivity.Controls.Add(BillingLastValidatedLabel);
            tabBillingActivity.Controls.Add(ValidationResultsTextBox);
            tabBillingActivity.Controls.Add(ValidateAccountButton);
            tabBillingActivity.Controls.Add(BillActivityDataGrid);
            tabBillingActivity.Controls.Add(minPmtTextBox);
            tabBillingActivity.Location = new Point(4, 24);
            tabBillingActivity.Margin = new Padding(4, 3, 4, 3);
            tabBillingActivity.Name = "tabBillingActivity";
            tabBillingActivity.Padding = new Padding(4, 3, 4, 3);
            tabBillingActivity.Size = new Size(1344, 568);
            tabBillingActivity.TabIndex = 12;
            tabBillingActivity.Text = "Billing Activity";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(10, 357);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(102, 15);
            label5.TabIndex = 15;
            label5.Text = "Statement History";
            // 
            // statementHistoryDataGrid
            // 
            statementHistoryDataGrid.BackgroundColor = Color.White;
            statementHistoryDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            statementHistoryDataGrid.Location = new Point(9, 378);
            statementHistoryDataGrid.Margin = new Padding(4, 3, 4, 3);
            statementHistoryDataGrid.Name = "statementHistoryDataGrid";
            statementHistoryDataGrid.Size = new Size(1069, 130);
            statementHistoryDataGrid.TabIndex = 14;
            // 
            // clearClaimStatusButton
            // 
            clearClaimStatusButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            clearClaimStatusButton.Location = new Point(1198, 107);
            clearClaimStatusButton.Margin = new Padding(4, 3, 4, 3);
            clearClaimStatusButton.Name = "clearClaimStatusButton";
            clearClaimStatusButton.Size = new Size(135, 27);
            clearClaimStatusButton.TabIndex = 13;
            clearClaimStatusButton.Text = "Clear Claim Status";
            clearClaimStatusButton.Click += clearClaimStatusButton_Click;
            // 
            // readyToBillCheckbox
            // 
            readyToBillCheckbox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            readyToBillCheckbox.AutoSize = true;
            readyToBillCheckbox.Location = new Point(1206, 12);
            readyToBillCheckbox.Margin = new Padding(4, 3, 4, 3);
            readyToBillCheckbox.Name = "readyToBillCheckbox";
            readyToBillCheckbox.Size = new Size(91, 19);
            readyToBillCheckbox.TabIndex = 12;
            readyToBillCheckbox.Text = "Ready to Bill";
            readyToBillCheckbox.UseVisualStyleBackColor = true;
            readyToBillCheckbox.CheckedChanged += readyToBillCheckbox_CheckedChanged;
            // 
            // statementFlagComboBox
            // 
            statementFlagComboBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            statementFlagComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            statementFlagComboBox.FormattingEnabled = true;
            statementFlagComboBox.Items.AddRange(new object[] { "N", "Y", "1", "2", "3", "4", "P" });
            statementFlagComboBox.Location = new Point(1198, 382);
            statementFlagComboBox.Margin = new Padding(4, 3, 4, 3);
            statementFlagComboBox.Name = "statementFlagComboBox";
            statementFlagComboBox.Size = new Size(116, 23);
            statementFlagComboBox.TabIndex = 11;
            statementFlagComboBox.SelectionChangeCommitted += statementFlagComboBox_SelectionChangeCommitted;
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(1085, 477);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(110, 15);
            label3.TabIndex = 10;
            label3.Text = "Minimum Payment";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(1101, 447);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(85, 15);
            label2.TabIndex = 8;
            label2.Text = "Last Statement";
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(1101, 417);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(86, 15);
            label1.TabIndex = 8;
            label1.Text = "First Statement";
            // 
            // lastStmtDateTextBox
            // 
            lastStmtDateTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            lastStmtDateTextBox.Location = new Point(1198, 443);
            lastStmtDateTextBox.Margin = new Padding(4, 3, 4, 3);
            lastStmtDateTextBox.Name = "lastStmtDateTextBox";
            lastStmtDateTextBox.ReadOnly = true;
            lastStmtDateTextBox.Size = new Size(116, 23);
            lastStmtDateTextBox.TabIndex = 7;
            // 
            // firstStmtDateTextBox
            // 
            firstStmtDateTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            firstStmtDateTextBox.Location = new Point(1198, 413);
            firstStmtDateTextBox.Margin = new Padding(4, 3, 4, 3);
            firstStmtDateTextBox.Name = "firstStmtDateTextBox";
            firstStmtDateTextBox.ReadOnly = true;
            firstStmtDateTextBox.Size = new Size(116, 23);
            firstStmtDateTextBox.TabIndex = 7;
            // 
            // statementFlagLabel
            // 
            statementFlagLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            statementFlagLabel.AutoSize = true;
            statementFlagLabel.Location = new Point(1100, 387);
            statementFlagLabel.Margin = new Padding(4, 0, 4, 0);
            statementFlagLabel.Name = "statementFlagLabel";
            statementFlagLabel.Size = new Size(86, 15);
            statementFlagLabel.TabIndex = 6;
            statementFlagLabel.Text = "Statement Flag";
            // 
            // GenerateClaimButton
            // 
            GenerateClaimButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GenerateClaimButton.Location = new Point(1198, 40);
            GenerateClaimButton.Margin = new Padding(4, 3, 4, 3);
            GenerateClaimButton.Name = "GenerateClaimButton";
            GenerateClaimButton.Size = new Size(135, 27);
            GenerateClaimButton.TabIndex = 4;
            GenerateClaimButton.Text = "GenerateClaim";
            GenerateClaimButton.Click += GenerateClaimButton_Click;
            // 
            // LastValidatedLabel
            // 
            LastValidatedLabel.AutoSize = true;
            LastValidatedLabel.Location = new Point(122, 15);
            LastValidatedLabel.Margin = new Padding(4, 0, 4, 0);
            LastValidatedLabel.Name = "LastValidatedLabel";
            LastValidatedLabel.Size = new Size(22, 15);
            LastValidatedLabel.TabIndex = 3;
            LastValidatedLabel.Text = ".....";
            // 
            // BillingLastValidatedLabel
            // 
            BillingLastValidatedLabel.AutoSize = true;
            BillingLastValidatedLabel.Location = new Point(9, 12);
            BillingLastValidatedLabel.Margin = new Padding(4, 0, 4, 0);
            BillingLastValidatedLabel.Name = "BillingLastValidatedLabel";
            BillingLastValidatedLabel.Size = new Size(82, 15);
            BillingLastValidatedLabel.TabIndex = 3;
            BillingLastValidatedLabel.Text = "Last Validated:";
            // 
            // ValidationResultsTextBox
            // 
            ValidationResultsTextBox.Location = new Point(9, 192);
            ValidationResultsTextBox.Margin = new Padding(4, 3, 4, 3);
            ValidationResultsTextBox.Multiline = true;
            ValidationResultsTextBox.Name = "ValidationResultsTextBox";
            ValidationResultsTextBox.Size = new Size(1069, 145);
            ValidationResultsTextBox.TabIndex = 2;
            // 
            // ValidateAccountButton
            // 
            ValidateAccountButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ValidateAccountButton.Location = new Point(1198, 74);
            ValidateAccountButton.Margin = new Padding(4, 3, 4, 3);
            ValidateAccountButton.Name = "ValidateAccountButton";
            ValidateAccountButton.Size = new Size(135, 27);
            ValidateAccountButton.TabIndex = 1;
            ValidateAccountButton.Text = "Validate Account";
            ValidateAccountButton.Click += ValidateAccountButton_Click;
            // 
            // BillActivityDataGrid
            // 
            BillActivityDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            BillActivityDataGrid.BackgroundColor = Color.WhiteSmoke;
            dataGridViewCellStyle13.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle13.BackColor = SystemColors.Control;
            dataGridViewCellStyle13.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle13.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle13.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle13.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle13.WrapMode = DataGridViewTriState.True;
            BillActivityDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle13;
            BillActivityDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle14.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle14.BackColor = SystemColors.Window;
            dataGridViewCellStyle14.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle14.ForeColor = Color.Black;
            dataGridViewCellStyle14.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle14.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle14.WrapMode = DataGridViewTriState.False;
            BillActivityDataGrid.DefaultCellStyle = dataGridViewCellStyle14;
            BillActivityDataGrid.Location = new Point(9, 40);
            BillActivityDataGrid.Margin = new Padding(4, 3, 4, 3);
            BillActivityDataGrid.Name = "BillActivityDataGrid";
            dataGridViewCellStyle15.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle15.BackColor = SystemColors.Control;
            dataGridViewCellStyle15.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle15.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle15.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle15.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle15.WrapMode = DataGridViewTriState.True;
            BillActivityDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle15;
            BillActivityDataGrid.Size = new Size(1069, 144);
            BillActivityDataGrid.TabIndex = 0;
            BillActivityDataGrid.MouseDoubleClick += BillActivityDataGrid_MouseDoubleClick;
            // 
            // minPmtTextBox
            // 
            minPmtTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            minPmtTextBox.DollarValue = new decimal(new int[] { 0, 0, 0, 0 });
            minPmtTextBox.Location = new Point(1198, 473);
            minPmtTextBox.Margin = new Padding(4, 3, 4, 3);
            minPmtTextBox.Name = "minPmtTextBox";
            minPmtTextBox.Size = new Size(116, 23);
            minPmtTextBox.TabIndex = 9;
            // 
            // noteTextContextMenu
            // 
            noteTextContextMenu.Items.AddRange(new ToolStripItem[] { copyToolStripMenuItem });
            noteTextContextMenu.Name = "noteTextContextMenu";
            noteTextContextMenu.Size = new Size(103, 26);
            // 
            // copyToolStripMenuItem
            // 
            copyToolStripMenuItem.Name = "copyToolStripMenuItem";
            copyToolStripMenuItem.Size = new Size(102, 22);
            copyToolStripMenuItem.Text = "Copy";
            copyToolStripMenuItem.Click += copyToolStripMenuItem_Click;
            // 
            // menuStrip1
            // 
            menuStrip1.ImageScalingSize = new Size(20, 20);
            menuStrip1.Items.AddRange(new ToolStripItem[] { accountToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Padding = new Padding(7, 2, 0, 2);
            menuStrip1.Size = new Size(1352, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "mainMenuStrip";
            // 
            // accountToolStripMenuItem
            // 
            accountToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { changeDateOfServiceToolStripMenuItem, changeFinancialClassToolStripMenuItem, changeClientToolStripMenuItem, viewAuditInfoToolStripMenuItem, clearHoldStatusToolStripMenuItem, swapInsurancesToolStripMenuItem, moveAllChargesToolStripMenuItem });
            accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            accountToolStripMenuItem.Size = new Size(64, 20);
            accountToolStripMenuItem.Text = "Account";
            // 
            // changeDateOfServiceToolStripMenuItem
            // 
            changeDateOfServiceToolStripMenuItem.Name = "changeDateOfServiceToolStripMenuItem";
            changeDateOfServiceToolStripMenuItem.Size = new Size(196, 22);
            changeDateOfServiceToolStripMenuItem.Text = "Change Date of Service";
            changeDateOfServiceToolStripMenuItem.Click += ChangeDateOfServiceToolStripMenuItem_Click;
            // 
            // changeFinancialClassToolStripMenuItem
            // 
            changeFinancialClassToolStripMenuItem.Name = "changeFinancialClassToolStripMenuItem";
            changeFinancialClassToolStripMenuItem.Size = new Size(196, 22);
            changeFinancialClassToolStripMenuItem.Text = "Change Financial Class";
            changeFinancialClassToolStripMenuItem.Click += ChangeFinancialClassToolStripMenuItem_Click;
            // 
            // changeClientToolStripMenuItem
            // 
            changeClientToolStripMenuItem.Name = "changeClientToolStripMenuItem";
            changeClientToolStripMenuItem.Size = new Size(196, 22);
            changeClientToolStripMenuItem.Text = "Change Client";
            changeClientToolStripMenuItem.Click += ChangeClientToolStripMenuItem_Click;
            // 
            // viewAuditInfoToolStripMenuItem
            // 
            viewAuditInfoToolStripMenuItem.Name = "viewAuditInfoToolStripMenuItem";
            viewAuditInfoToolStripMenuItem.Size = new Size(196, 22);
            viewAuditInfoToolStripMenuItem.Text = "View Audit Info";
            // 
            // clearHoldStatusToolStripMenuItem
            // 
            clearHoldStatusToolStripMenuItem.Name = "clearHoldStatusToolStripMenuItem";
            clearHoldStatusToolStripMenuItem.Size = new Size(196, 22);
            clearHoldStatusToolStripMenuItem.Text = "Clear Hold Status";
            clearHoldStatusToolStripMenuItem.Click += ClearHoldStatusToolStripMenuItem_Click;
            // 
            // swapInsurancesToolStripMenuItem
            // 
            swapInsurancesToolStripMenuItem.Name = "swapInsurancesToolStripMenuItem";
            swapInsurancesToolStripMenuItem.Size = new Size(196, 22);
            swapInsurancesToolStripMenuItem.Text = "Swap Insurances";
            swapInsurancesToolStripMenuItem.Click += swapInsurancesToolStripMenuItem_Click;
            // 
            // moveAllChargesToolStripMenuItem
            // 
            moveAllChargesToolStripMenuItem.Name = "moveAllChargesToolStripMenuItem";
            moveAllChargesToolStripMenuItem.Size = new Size(196, 22);
            moveAllChargesToolStripMenuItem.Text = "Move All Charges";
            moveAllChargesToolStripMenuItem.Click += moveAllChargesToolStripMenuItem_Click;
            // 
            // BannerMRNTextBox
            // 
            BannerMRNTextBox.BackColor = Color.Blue;
            BannerMRNTextBox.BorderStyle = BorderStyle.None;
            BannerMRNTextBox.Font = new Font("Microsoft Sans Serif", 9.75F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BannerMRNTextBox.ForeColor = Color.White;
            BannerMRNTextBox.Location = new Point(791, 20);
            BannerMRNTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerMRNTextBox.Name = "BannerMRNTextBox";
            BannerMRNTextBox.ReadOnly = true;
            BannerMRNTextBox.Size = new Size(110, 15);
            BannerMRNTextBox.TabIndex = 26;
            // 
            // BannerAccountTextBox
            // 
            BannerAccountTextBox.BackColor = Color.Blue;
            BannerAccountTextBox.BorderStyle = BorderStyle.None;
            BannerAccountTextBox.Font = new Font("Microsoft Sans Serif", 14F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BannerAccountTextBox.ForeColor = Color.White;
            BannerAccountTextBox.Location = new Point(675, 13);
            BannerAccountTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerAccountTextBox.Name = "BannerAccountTextBox";
            BannerAccountTextBox.ReadOnly = true;
            BannerAccountTextBox.Size = new Size(110, 22);
            BannerAccountTextBox.TabIndex = 25;
            BannerAccountTextBox.Click += BannerAccountTextBox_Click;
            // 
            // BannerSexTextBox
            // 
            BannerSexTextBox.BackColor = Color.Blue;
            BannerSexTextBox.BorderStyle = BorderStyle.None;
            BannerSexTextBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BannerSexTextBox.ForeColor = Color.White;
            BannerSexTextBox.Location = new Point(516, 15);
            BannerSexTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerSexTextBox.Name = "BannerSexTextBox";
            BannerSexTextBox.ReadOnly = true;
            BannerSexTextBox.Size = new Size(50, 19);
            BannerSexTextBox.TabIndex = 24;
            // 
            // BannerDobTextBox
            // 
            BannerDobTextBox.BackColor = Color.Blue;
            BannerDobTextBox.BorderStyle = BorderStyle.None;
            BannerDobTextBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BannerDobTextBox.ForeColor = Color.White;
            BannerDobTextBox.Location = new Point(405, 15);
            BannerDobTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerDobTextBox.Name = "BannerDobTextBox";
            BannerDobTextBox.ReadOnly = true;
            BannerDobTextBox.Size = new Size(103, 19);
            BannerDobTextBox.TabIndex = 23;
            // 
            // BannerNameTextBox
            // 
            BannerNameTextBox.BackColor = Color.Blue;
            BannerNameTextBox.BorderStyle = BorderStyle.None;
            BannerNameTextBox.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            BannerNameTextBox.ForeColor = Color.White;
            BannerNameTextBox.Location = new Point(5, 15);
            BannerNameTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerNameTextBox.Name = "BannerNameTextBox";
            BannerNameTextBox.ReadOnly = true;
            BannerNameTextBox.Size = new Size(293, 19);
            BannerNameTextBox.TabIndex = 22;
            // 
            // BannerDOBSexLabel
            // 
            BannerDOBSexLabel.AutoSize = true;
            BannerDOBSexLabel.ForeColor = Color.White;
            BannerDOBSexLabel.Location = new Point(305, 19);
            BannerDOBSexLabel.Margin = new Padding(4, 0, 4, 0);
            BannerDOBSexLabel.Name = "BannerDOBSexLabel";
            BannerDOBSexLabel.Size = new Size(57, 15);
            BannerDOBSexLabel.TabIndex = 21;
            BannerDOBSexLabel.Text = "DOB/Sex:";
            // 
            // BannerAccountMrnLabel
            // 
            BannerAccountMrnLabel.AutoSize = true;
            BannerAccountMrnLabel.ForeColor = Color.White;
            BannerAccountMrnLabel.Location = new Point(574, 19);
            BannerAccountMrnLabel.Margin = new Padding(4, 0, 4, 0);
            BannerAccountMrnLabel.Name = "BannerAccountMrnLabel";
            BannerAccountMrnLabel.Size = new Size(87, 15);
            BannerAccountMrnLabel.TabIndex = 20;
            BannerAccountMrnLabel.Text = "Account/MRN:";
            // 
            // BannerClientTextBox
            // 
            BannerClientTextBox.BackColor = Color.Blue;
            BannerClientTextBox.BorderStyle = BorderStyle.None;
            BannerClientTextBox.ForeColor = Color.White;
            BannerClientTextBox.Location = new Point(5, 45);
            BannerClientTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerClientTextBox.Name = "BannerClientTextBox";
            BannerClientTextBox.Size = new Size(293, 16);
            BannerClientTextBox.TabIndex = 29;
            // 
            // BannerFinClassTextBox
            // 
            BannerFinClassTextBox.BackColor = Color.Blue;
            BannerFinClassTextBox.BorderStyle = BorderStyle.None;
            BannerFinClassTextBox.ForeColor = Color.White;
            BannerFinClassTextBox.Location = new Point(403, 42);
            BannerFinClassTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerFinClassTextBox.Name = "BannerFinClassTextBox";
            BannerFinClassTextBox.Size = new Size(117, 16);
            BannerFinClassTextBox.TabIndex = 30;
            // 
            // BannerFinClassLabel
            // 
            BannerFinClassLabel.AutoSize = true;
            BannerFinClassLabel.ForeColor = Color.White;
            BannerFinClassLabel.Location = new Point(305, 42);
            BannerFinClassLabel.Margin = new Padding(4, 0, 4, 0);
            BannerFinClassLabel.Name = "BannerFinClassLabel";
            BannerFinClassLabel.Size = new Size(87, 15);
            BannerFinClassLabel.TabIndex = 31;
            BannerFinClassLabel.Text = "Financial Class:";
            // 
            // BannerTotalPmtLabel
            // 
            BannerTotalPmtLabel.AutoSize = true;
            BannerTotalPmtLabel.ForeColor = Color.White;
            BannerTotalPmtLabel.Location = new Point(915, 31);
            BannerTotalPmtLabel.Margin = new Padding(4, 0, 4, 0);
            BannerTotalPmtLabel.Name = "BannerTotalPmtLabel";
            BannerTotalPmtLabel.Size = new Size(83, 15);
            BannerTotalPmtLabel.TabIndex = 20;
            BannerTotalPmtLabel.Text = "Total Pmt/Adj:";
            // 
            // BannerTotalChargesLabel
            // 
            BannerTotalChargesLabel.AutoSize = true;
            BannerTotalChargesLabel.ForeColor = Color.White;
            BannerTotalChargesLabel.Location = new Point(915, 13);
            BannerTotalChargesLabel.Margin = new Padding(4, 0, 4, 0);
            BannerTotalChargesLabel.Name = "BannerTotalChargesLabel";
            BannerTotalChargesLabel.Size = new Size(81, 15);
            BannerTotalChargesLabel.TabIndex = 20;
            BannerTotalChargesLabel.Text = "Total Charges:";
            // 
            // BannerAccBalanceLabel
            // 
            BannerAccBalanceLabel.AutoSize = true;
            BannerAccBalanceLabel.ForeColor = Color.White;
            BannerAccBalanceLabel.Location = new Point(915, 50);
            BannerAccBalanceLabel.Margin = new Padding(4, 0, 4, 0);
            BannerAccBalanceLabel.Name = "BannerAccBalanceLabel";
            BannerAccBalanceLabel.Size = new Size(99, 15);
            BannerAccBalanceLabel.TabIndex = 20;
            BannerAccBalanceLabel.Text = "Account Balance:";
            // 
            // TotalPmtAdjLabel
            // 
            TotalPmtAdjLabel.ForeColor = Color.White;
            TotalPmtAdjLabel.Location = new Point(1031, 31);
            TotalPmtAdjLabel.Margin = new Padding(4, 0, 4, 0);
            TotalPmtAdjLabel.Name = "TotalPmtAdjLabel";
            TotalPmtAdjLabel.Size = new Size(82, 15);
            TotalPmtAdjLabel.TabIndex = 20;
            TotalPmtAdjLabel.Text = "0.00";
            TotalPmtAdjLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // BalanceLabel
            // 
            BalanceLabel.ForeColor = Color.White;
            BalanceLabel.Location = new Point(1031, 50);
            BalanceLabel.Margin = new Padding(4, 0, 4, 0);
            BalanceLabel.Name = "BalanceLabel";
            BalanceLabel.Size = new Size(82, 15);
            BalanceLabel.TabIndex = 20;
            BalanceLabel.Text = "0.00";
            BalanceLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // TotalChargesLabel
            // 
            TotalChargesLabel.ForeColor = Color.White;
            TotalChargesLabel.Location = new Point(1031, 13);
            TotalChargesLabel.Margin = new Padding(4, 0, 4, 0);
            TotalChargesLabel.Name = "TotalChargesLabel";
            TotalChargesLabel.Size = new Size(82, 15);
            TotalChargesLabel.TabIndex = 20;
            TotalChargesLabel.Text = "0.00";
            TotalChargesLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // BannerBillStatusLabel
            // 
            BannerBillStatusLabel.AutoSize = true;
            BannerBillStatusLabel.ForeColor = Color.White;
            BannerBillStatusLabel.Location = new Point(574, 42);
            BannerBillStatusLabel.Margin = new Padding(4, 0, 4, 0);
            BannerBillStatusLabel.Name = "BannerBillStatusLabel";
            BannerBillStatusLabel.Size = new Size(78, 15);
            BannerBillStatusLabel.TabIndex = 32;
            BannerBillStatusLabel.Text = "Billing Status:";
            // 
            // BannerBillStatusTextBox
            // 
            BannerBillStatusTextBox.BackColor = Color.Blue;
            BannerBillStatusTextBox.BorderStyle = BorderStyle.None;
            BannerBillStatusTextBox.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Regular, GraphicsUnit.Point, 0);
            BannerBillStatusTextBox.ForeColor = Color.White;
            BannerBillStatusTextBox.Location = new Point(675, 39);
            BannerBillStatusTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerBillStatusTextBox.Name = "BannerBillStatusTextBox";
            BannerBillStatusTextBox.ReadOnly = true;
            BannerBillStatusTextBox.Size = new Size(110, 16);
            BannerBillStatusTextBox.TabIndex = 25;
            // 
            // BannerProviderTextBox
            // 
            BannerProviderTextBox.BackColor = Color.Blue;
            BannerProviderTextBox.BorderStyle = BorderStyle.None;
            BannerProviderTextBox.ForeColor = Color.White;
            BannerProviderTextBox.Location = new Point(5, 69);
            BannerProviderTextBox.Margin = new Padding(4, 3, 4, 3);
            BannerProviderTextBox.Name = "BannerProviderTextBox";
            BannerProviderTextBox.Size = new Size(293, 16);
            BannerProviderTextBox.TabIndex = 29;
            // 
            // RefreshButton
            // 
            RefreshButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            RefreshButton.BackColor = Color.LightSteelBlue;
            RefreshButton.Image = Properties.Resources.refresh_icon;
            RefreshButton.Location = new Point(1316, 10);
            RefreshButton.Margin = new Padding(4, 3, 4, 3);
            RefreshButton.Name = "RefreshButton";
            RefreshButton.Size = new Size(23, 25);
            RefreshButton.SizeMode = PictureBoxSizeMode.StretchImage;
            RefreshButton.TabIndex = 28;
            RefreshButton.TabStop = false;
            RefreshButton.Click += RefreshButton_Click;
            // 
            // bannerDateOfServiceTextBox
            // 
            bannerDateOfServiceTextBox.BackColor = Color.Blue;
            bannerDateOfServiceTextBox.BorderStyle = BorderStyle.None;
            bannerDateOfServiceTextBox.ForeColor = Color.White;
            bannerDateOfServiceTextBox.Location = new Point(403, 64);
            bannerDateOfServiceTextBox.Margin = new Padding(4, 3, 4, 3);
            bannerDateOfServiceTextBox.Name = "bannerDateOfServiceTextBox";
            bannerDateOfServiceTextBox.Size = new Size(117, 16);
            bannerDateOfServiceTextBox.TabIndex = 30;
            // 
            // bannerDateOfServiceLabel
            // 
            bannerDateOfServiceLabel.AutoSize = true;
            bannerDateOfServiceLabel.ForeColor = Color.White;
            bannerDateOfServiceLabel.Location = new Point(305, 64);
            bannerDateOfServiceLabel.Margin = new Padding(4, 0, 4, 0);
            bannerDateOfServiceLabel.Name = "bannerDateOfServiceLabel";
            bannerDateOfServiceLabel.Size = new Size(88, 15);
            bannerDateOfServiceLabel.TabIndex = 31;
            bannerDateOfServiceLabel.Text = "Date of Service:";
            // 
            // dxPointerMenuStrip
            // 
            dxPointerMenuStrip.Items.AddRange(new ToolStripItem[] { clearDxPointerToolStripMenuItem });
            dxPointerMenuStrip.Name = "dxPointerMenuStrip";
            dxPointerMenuStrip.Size = new Size(160, 26);
            // 
            // clearDxPointerToolStripMenuItem
            // 
            clearDxPointerToolStripMenuItem.Name = "clearDxPointerToolStripMenuItem";
            clearDxPointerToolStripMenuItem.Size = new Size(159, 22);
            clearDxPointerToolStripMenuItem.Text = "Clear Dx Pointer";
            clearDxPointerToolStripMenuItem.Click += clearDxPointerToolStripMenuItem_Click;
            // 
            // bannerAlertLabel
            // 
            bannerAlertLabel.AutoSize = true;
            bannerAlertLabel.BackColor = Color.White;
            bannerAlertLabel.Font = new Font("Microsoft Sans Serif", 10F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bannerAlertLabel.ForeColor = Color.Red;
            bannerAlertLabel.Location = new Point(578, 66);
            bannerAlertLabel.Margin = new Padding(4, 0, 4, 0);
            bannerAlertLabel.Name = "bannerAlertLabel";
            bannerAlertLabel.Size = new Size(13, 17);
            bannerAlertLabel.TabIndex = 33;
            bannerAlertLabel.Text = ".";
            bannerAlertLabel.Visible = false;
            // 
            // BannerThirdPartyBalLabel
            // 
            BannerThirdPartyBalLabel.AutoSize = true;
            BannerThirdPartyBalLabel.ForeColor = Color.White;
            BannerThirdPartyBalLabel.Location = new Point(1115, 13);
            BannerThirdPartyBalLabel.Margin = new Padding(4, 0, 4, 0);
            BannerThirdPartyBalLabel.Name = "BannerThirdPartyBalLabel";
            BannerThirdPartyBalLabel.Size = new Size(76, 15);
            BannerThirdPartyBalLabel.TabIndex = 20;
            BannerThirdPartyBalLabel.Text = "3rd Party Bal:";
            // 
            // ThirdPartyBalLabel
            // 
            ThirdPartyBalLabel.ForeColor = Color.White;
            ThirdPartyBalLabel.Location = new Point(1224, 13);
            ThirdPartyBalLabel.Margin = new Padding(4, 0, 4, 0);
            ThirdPartyBalLabel.Name = "ThirdPartyBalLabel";
            ThirdPartyBalLabel.Size = new Size(70, 15);
            ThirdPartyBalLabel.TabIndex = 20;
            ThirdPartyBalLabel.Text = "0.00";
            ThirdPartyBalLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // BannerClientBalLabel
            // 
            BannerClientBalLabel.AutoSize = true;
            BannerClientBalLabel.ForeColor = Color.White;
            BannerClientBalLabel.Location = new Point(1115, 31);
            BannerClientBalLabel.Margin = new Padding(4, 0, 4, 0);
            BannerClientBalLabel.Name = "BannerClientBalLabel";
            BannerClientBalLabel.Size = new Size(85, 15);
            BannerClientBalLabel.TabIndex = 20;
            BannerClientBalLabel.Text = "Client Balance:";
            // 
            // ClientBalLabel
            // 
            ClientBalLabel.ForeColor = Color.White;
            ClientBalLabel.Location = new Point(1224, 31);
            ClientBalLabel.Margin = new Padding(4, 0, 4, 0);
            ClientBalLabel.Name = "ClientBalLabel";
            ClientBalLabel.Size = new Size(70, 15);
            ClientBalLabel.TabIndex = 20;
            ClientBalLabel.Text = "0.00";
            ClientBalLabel.TextAlign = ContentAlignment.TopRight;
            // 
            // bannerPanel
            // 
            bannerPanel.BackColor = Color.Blue;
            bannerPanel.Controls.Add(BannerFinClassTextBox);
            bannerPanel.Controls.Add(BannerAccountMrnLabel);
            bannerPanel.Controls.Add(BannerTotalPmtLabel);
            bannerPanel.Controls.Add(BannerAccBalanceLabel);
            bannerPanel.Controls.Add(TotalPmtAdjLabel);
            bannerPanel.Controls.Add(bannerAlertLabel);
            bannerPanel.Controls.Add(BannerThirdPartyBalLabel);
            bannerPanel.Controls.Add(BannerBillStatusLabel);
            bannerPanel.Controls.Add(BalanceLabel);
            bannerPanel.Controls.Add(bannerDateOfServiceLabel);
            bannerPanel.Controls.Add(BannerClientBalLabel);
            bannerPanel.Controls.Add(BannerFinClassLabel);
            bannerPanel.Controls.Add(ThirdPartyBalLabel);
            bannerPanel.Controls.Add(bannerDateOfServiceTextBox);
            bannerPanel.Controls.Add(ClientBalLabel);
            bannerPanel.Controls.Add(BannerTotalChargesLabel);
            bannerPanel.Controls.Add(BannerProviderTextBox);
            bannerPanel.Controls.Add(TotalChargesLabel);
            bannerPanel.Controls.Add(BannerClientTextBox);
            bannerPanel.Controls.Add(BannerDOBSexLabel);
            bannerPanel.Controls.Add(RefreshButton);
            bannerPanel.Controls.Add(BannerNameTextBox);
            bannerPanel.Controls.Add(BannerMRNTextBox);
            bannerPanel.Controls.Add(BannerDobTextBox);
            bannerPanel.Controls.Add(BannerBillStatusTextBox);
            bannerPanel.Controls.Add(BannerSexTextBox);
            bannerPanel.Controls.Add(BannerAccountTextBox);
            bannerPanel.Dock = DockStyle.Top;
            bannerPanel.Location = new Point(0, 24);
            bannerPanel.Name = "bannerPanel";
            bannerPanel.Size = new Size(1352, 100);
            bannerPanel.TabIndex = 34;
            // 
            // AccountForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(1352, 727);
            Controls.Add(bannerPanel);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            ForeColor = Color.Black;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            Margin = new Padding(4, 3, 4, 3);
            Name = "AccountForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Account";
            Activated += AccountForm_Activated;
            FormClosing += AccountForm_FormClosing;
            FormClosed += AccountForm_FormClosed;
            Load += AccountForm_Load;
            tabControl1.ResumeLayout(false);
            summaryTab.ResumeLayout(false);
            tabDemographics.ResumeLayout(false);
            demographicsLayoutPanel.ResumeLayout(false);
            demographicsLayoutPanel.PerformLayout();
            tabDiagnosis.ResumeLayout(false);
            tabDiagnosis.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dxPointerGrid2).EndInit();
            ((System.ComponentModel.ISupportInitialize)DxDeleteButton).EndInit();
            ((System.ComponentModel.ISupportInitialize)DxSearchDataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)DiagnosisDataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)DxSearchButton).EndInit();
            tabPayments.ResumeLayout(false);
            tabPayments.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)PaymentsDataGrid).EndInit();
            tabNotes.ResumeLayout(false);
            tabNotes.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)notesDataGridView).EndInit();
            tabBillingActivity.ResumeLayout(false);
            tabBillingActivity.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)statementHistoryDataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)BillActivityDataGrid).EndInit();
            noteTextContextMenu.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)RefreshButton).EndInit();
            dxPointerMenuStrip.ResumeLayout(false);
            bannerPanel.ResumeLayout(false);
            bannerPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.TableLayoutPanel summaryTable;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem accountToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDateOfServiceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFinancialClassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeClientToolStripMenuItem;
        private System.Windows.Forms.TextBox BannerMRNTextBox;
        private System.Windows.Forms.TextBox BannerAccountTextBox;
        private System.Windows.Forms.TextBox BannerSexTextBox;
        private System.Windows.Forms.TextBox BannerDobTextBox;
        private System.Windows.Forms.TextBox BannerNameTextBox;
        private System.Windows.Forms.Label BannerDOBSexLabel;
        private System.Windows.Forms.Label BannerAccountMrnLabel;
        private System.Windows.Forms.DataGridView PaymentsDataGrid;
        private System.Windows.Forms.DataGridView DiagnosisDataGrid;
        private System.Windows.Forms.PictureBox DxSearchButton;
        private System.Windows.Forms.TextBox txtSearchDx;
        private System.Windows.Forms.Label DxSearchLabel;
        private System.Windows.Forms.DataGridView DxSearchDataGrid;
        private System.Windows.Forms.Label SelectedDxLabel;
        private System.Windows.Forms.TextBox DxQuickAddTextBox;
        private System.Windows.Forms.Label QuickAddLabel;
        private System.Windows.Forms.Button SaveDxButton;
        private System.Windows.Forms.PictureBox DxDeleteButton;
        private System.Windows.Forms.PictureBox RefreshButton;
        private System.Windows.Forms.TextBox TotalPaymentTextBox;
        private System.Windows.Forms.Label PmtTotalWriteOffLabel;
        private System.Windows.Forms.TextBox TotalWriteOffTextBox;
        private System.Windows.Forms.Label PmtTotalContractualLabel;
        private System.Windows.Forms.TextBox TotalContractualTextBox;
        private System.Windows.Forms.Label PmtTotalPaymentLabel;
        private System.Windows.Forms.Label PmtTotalPmtAdjLabel;
        private System.Windows.Forms.TextBox TotalPmtAllTextBox;
        private System.Windows.Forms.Button AddChargeButton;
        private System.Windows.Forms.Button AddNoteButton;
        private System.Windows.Forms.MaskedTextBox ZipcodeTextBox;
        private LabBilling.Library.FlatCombo MaritalStatusComboBox;
        private System.Windows.Forms.Label PatMaritalStatusLabel;
        private System.Windows.Forms.Label PatEmailLabel;
        private System.Windows.Forms.TextBox EmailAddressTextBox;
        private System.Windows.Forms.TextBox SuffixTextBox;
        private System.Windows.Forms.Label PatSuffixLabel;
        private System.Windows.Forms.TextBox MiddleNameTextBox;
        private System.Windows.Forms.TextBox FirstNameTextBox;
        private System.Windows.Forms.Label PatMiddleNameLabel;
        private System.Windows.Forms.Label PatFirstNameLabel;
        private System.Windows.Forms.Label PatZipLabel;
        private System.Windows.Forms.Label PatStateLabel;
        private LabBilling.Library.FlatCombo StateComboBox;
        private System.Windows.Forms.MaskedTextBox SocSecNoTextBox;
        private System.Windows.Forms.Label PatPhoneLabel;
        private System.Windows.Forms.Label PatCityLabel;
        private System.Windows.Forms.Label PatAddressLabel;
        private System.Windows.Forms.TextBox PhoneTextBox;
        private System.Windows.Forms.TextBox CityTextBox;
        private System.Windows.Forms.TextBox Address2TextBox;
        private LabBilling.Library.FlatCombo SexComboBox;
        private System.Windows.Forms.Label PatSexLabel;
        private System.Windows.Forms.Label PatDOBLabel;
        private System.Windows.Forms.Label PatSSNLabel;
        private System.Windows.Forms.Label PatLastNameLabel;
        private System.Windows.Forms.TextBox Address1TextBox;
        private System.Windows.Forms.TextBox LastNameTextBox;
        private System.Windows.Forms.Button SaveDemographics;
        private System.Windows.Forms.ToolStripMenuItem viewAuditInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearHoldStatusToolStripMenuItem;
        private System.Windows.Forms.TextBox BannerClientTextBox;
        private System.Windows.Forms.TextBox BannerFinClassTextBox;
        private System.Windows.Forms.Label BannerFinClassLabel;
        private System.Windows.Forms.DataGridView BillActivityDataGrid;
        private System.Windows.Forms.Button AddPaymentButton;
        private System.Windows.Forms.TextBox DemoStatusMessagesTextBox;
        private System.Windows.Forms.Label BannerTotalPmtLabel;
        private System.Windows.Forms.Label BannerTotalChargesLabel;
        private System.Windows.Forms.Label BannerAccBalanceLabel;
        private System.Windows.Forms.Label TotalPmtAdjLabel;
        private System.Windows.Forms.Label BalanceLabel;
        private System.Windows.Forms.Label TotalChargesLabel;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage summaryTab;
        private System.Windows.Forms.TabPage tabNotes;
        private System.Windows.Forms.TabPage tabCharges;
        private System.Windows.Forms.TabPage tabPayments;
        private System.Windows.Forms.TabPage tabDiagnosis;
        private System.Windows.Forms.TabPage tabDemographics;
        private System.Windows.Forms.TabPage tabBillingActivity;
        private System.Windows.Forms.TextBox ValidationResultsTextBox;
        private System.Windows.Forms.Button ValidateAccountButton;
        private System.Windows.Forms.Label LastValidatedLabel;
        private System.Windows.Forms.Label BillingLastValidatedLabel;
        private System.Windows.Forms.Label BannerBillStatusLabel;
        private System.Windows.Forms.TextBox BannerBillStatusTextBox;
        private System.Windows.Forms.Button GenerateClaimButton;
        private System.Windows.Forms.TextBox BannerProviderTextBox;
        private System.Windows.Forms.Label orderingProviderLabel;
        private System.Windows.Forms.Label statementFlagLabel;
        private System.Windows.Forms.TextBox firstStmtDateTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox lastStmtDateTextBox;
        private System.Windows.Forms.Label label3;
        private UserControls.CurrencyTextBox minPmtTextBox;
        private System.Windows.Forms.ToolStripMenuItem swapInsurancesToolStripMenuItem;
        private UserControls.DateTextBox DateOfBirthTextBox;
        private System.Windows.Forms.ComboBox statementFlagComboBox;
        private System.Windows.Forms.ToolStripMenuItem moveAllChargesToolStripMenuItem;
        private System.Windows.Forms.TextBox bannerDateOfServiceTextBox;
        private System.Windows.Forms.Label bannerDateOfServiceLabel;
        private System.Windows.Forms.DataGridView dxPointerGrid2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox readyToBillCheckbox;
        private System.Windows.Forms.ContextMenuStrip dxPointerMenuStrip;
        private System.Windows.Forms.ToolStripMenuItem clearDxPointerToolStripMenuItem;
        private System.Windows.Forms.Button clearClaimStatusButton;
        private System.Windows.Forms.CheckBox noteAlertCheckBox;
        private System.Windows.Forms.Label bannerAlertLabel;
        private System.Windows.Forms.Label BannerThirdPartyBalLabel;
        private System.Windows.Forms.Label ThirdPartyBalLabel;
        private System.Windows.Forms.Label BannerClientBalLabel;
        private System.Windows.Forms.Label ClientBalLabel;
        private System.Windows.Forms.RadioButton showAllChargeRadioButton;
        private System.Windows.Forms.RadioButton showClientRadioButton;
        private System.Windows.Forms.RadioButton show3rdPartyRadioButton;
        private System.Windows.Forms.DataGridView statementHistoryDataGrid;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ContextMenuStrip noteTextContextMenu;
        private System.Windows.Forms.ToolStripMenuItem copyToolStripMenuItem;
        private System.Windows.Forms.Panel bannerPanel;
        private UserControls.LabDataGridView notesDataGridView;
        private System.Windows.Forms.TabPage tabInsPrimary;
        private System.Windows.Forms.TabPage tabInsSecondary;
        private System.Windows.Forms.TabPage tabInsTertiary;
        private System.Windows.Forms.TableLayoutPanel demographicsLayoutPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label PatRelationLabel;
        private Library.FlatCombo GuarantorRelationComboBox;
        private System.Windows.Forms.LinkLabel GuarCopyPatientLink;
        private System.Windows.Forms.Label GuarLastNameLabel;
        private System.Windows.Forms.TextBox GuarantorLastNameTextBox;
        private System.Windows.Forms.Label GuarFirstNameLabel;
        private System.Windows.Forms.TextBox GuarFirstNameTextBox;
        private System.Windows.Forms.Label GuarMiddleNameLabel;
        private System.Windows.Forms.TextBox GuarMiddleNameTextBox;
        private System.Windows.Forms.Label GuarSuffixLabel;
        private System.Windows.Forms.TextBox GuarSuffixTextBox;
        private System.Windows.Forms.MaskedTextBox GuarZipTextBox;
        private System.Windows.Forms.Label GuarZipCodeLabel;
        private System.Windows.Forms.Label GuarStateLabel;
        private Library.FlatCombo GuarStateComboBox;
        private System.Windows.Forms.TextBox GuarCityTextBox;
        private System.Windows.Forms.Label GuarCityLabel;
        private System.Windows.Forms.TextBox GuarantorAddressTextBox;
        private System.Windows.Forms.Label GuarAddressLabel;
        private System.Windows.Forms.Label GuarPhoneLabel;
        private System.Windows.Forms.TextBox GuarantorPhoneTextBox;
        private System.Windows.Forms.TextBox orderingPhyTextBox;
    }
}

