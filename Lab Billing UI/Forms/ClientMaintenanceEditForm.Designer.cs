namespace LabBilling.Forms
{
    partial class ClientMaintenanceEditForm
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
            label5 = new Krypton.Toolkit.KryptonLabel();
            tbContact = new Krypton.Toolkit.KryptonTextBox();
            tabControl1 = new TabControl();
            tabClientPreferences = new TabPage();
            chkCOCForms = new Krypton.Toolkit.KryptonCheckBox();
            label18 = new Krypton.Toolkit.KryptonLabel();
            cbFeeSched = new Krypton.Toolkit.KryptonComboBox();
            numDefaultDiscount = new NumericUpDown();
            label17 = new Krypton.Toolkit.KryptonLabel();
            chkPrintCPTonBill = new Krypton.Toolkit.KryptonCheckBox();
            chkDoNotBill = new Krypton.Toolkit.KryptonCheckBox();
            chkBillAtDiscount = new Krypton.Toolkit.KryptonCheckBox();
            chkCCReport = new Krypton.Toolkit.KryptonCheckBox();
            chkDateOrder = new Krypton.Toolkit.KryptonCheckBox();
            tabClientMRO = new TabPage();
            tbMROZipcode = new Krypton.Toolkit.KryptonMaskedTextBox();
            label19 = new Krypton.Toolkit.KryptonLabel();
            cbMROState = new Krypton.Toolkit.KryptonComboBox();
            label20 = new Krypton.Toolkit.KryptonLabel();
            tbMROCity = new Krypton.Toolkit.KryptonTextBox();
            label21 = new Krypton.Toolkit.KryptonLabel();
            MROAddress2TextBox = new Krypton.Toolkit.KryptonTextBox();
            tbMROAddress = new Krypton.Toolkit.KryptonTextBox();
            label22 = new Krypton.Toolkit.KryptonLabel();
            tbMROName = new Krypton.Toolkit.KryptonTextBox();
            label23 = new Krypton.Toolkit.KryptonLabel();
            tabComment = new TabPage();
            tbComment = new Krypton.Toolkit.KryptonTextBox();
            DiscountsTab = new TabPage();
            clientDiscountDataGrid = new DataGridView();
            tabInterface = new TabPage();
            interfaceMappingDataGrid = new DataGridView();
            tbZipcode = new Krypton.Toolkit.KryptonMaskedTextBox();
            cbCostCenter = new Krypton.Toolkit.KryptonComboBox();
            label15 = new Krypton.Toolkit.KryptonLabel();
            cbEmrType = new Krypton.Toolkit.KryptonComboBox();
            label14 = new Krypton.Toolkit.KryptonLabel();
            cbClientType = new Krypton.Toolkit.KryptonComboBox();
            label13 = new Krypton.Toolkit.KryptonLabel();
            tbEmail = new Krypton.Toolkit.KryptonTextBox();
            label12 = new Krypton.Toolkit.KryptonLabel();
            tbFax = new Krypton.Toolkit.KryptonTextBox();
            label11 = new Krypton.Toolkit.KryptonLabel();
            cbCounty = new Krypton.Toolkit.KryptonComboBox();
            label10 = new Krypton.Toolkit.KryptonLabel();
            tbPhone = new Krypton.Toolkit.KryptonTextBox();
            label9 = new Krypton.Toolkit.KryptonLabel();
            label8 = new Krypton.Toolkit.KryptonLabel();
            cbState = new Krypton.Toolkit.KryptonComboBox();
            label7 = new Krypton.Toolkit.KryptonLabel();
            tbCity = new Krypton.Toolkit.KryptonTextBox();
            label6 = new Krypton.Toolkit.KryptonLabel();
            tbAddress2 = new Krypton.Toolkit.KryptonTextBox();
            tbAddress1 = new Krypton.Toolkit.KryptonTextBox();
            label4 = new Krypton.Toolkit.KryptonLabel();
            tbClientCode = new Krypton.Toolkit.KryptonTextBox();
            label3 = new Krypton.Toolkit.KryptonLabel();
            ClientMnemTextBox = new Krypton.Toolkit.KryptonTextBox();
            label2 = new Krypton.Toolkit.KryptonLabel();
            ClientNameTextBox = new Krypton.Toolkit.KryptonTextBox();
            label1 = new Krypton.Toolkit.KryptonLabel();
            CancelButton = new Krypton.Toolkit.KryptonButton();
            SaveButton = new Krypton.Toolkit.KryptonButton();
            label16 = new Krypton.Toolkit.KryptonLabel();
            billMethodComboBox = new Krypton.Toolkit.KryptonComboBox();
            activeCheckBox = new Krypton.Toolkit.KryptonCheckBox();
            errorProvider1 = new ErrorProvider(components);
            tabControl1.SuspendLayout();
            tabClientPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbFeeSched).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numDefaultDiscount).BeginInit();
            tabClientMRO.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)cbMROState).BeginInit();
            tabComment.SuspendLayout();
            DiscountsTab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)clientDiscountDataGrid).BeginInit();
            tabInterface.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)interfaceMappingDataGrid).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbCostCenter).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbEmrType).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbClientType).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbCounty).BeginInit();
            ((System.ComponentModel.ISupportInitialize)cbState).BeginInit();
            ((System.ComponentModel.ISupportInitialize)billMethodComboBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // label5
            // 
            label5.Location = new Point(43, 299);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(53, 20);
            label5.TabIndex = 31;
            label5.Values.Text = "Contact";
            // 
            // tbContact
            // 
            tbContact.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            tbContact.Location = new Point(43, 321);
            tbContact.Margin = new Padding(4, 2, 4, 2);
            tbContact.Multiline = true;
            tbContact.Name = "tbContact";
            tbContact.ScrollBars = ScrollBars.Vertical;
            tbContact.Size = new Size(672, 175);
            tbContact.TabIndex = 32;
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabClientPreferences);
            tabControl1.Controls.Add(tabClientMRO);
            tabControl1.Controls.Add(tabComment);
            tabControl1.Controls.Add(DiscountsTab);
            tabControl1.Controls.Add(tabInterface);
            tabControl1.Location = new Point(42, 501);
            tabControl1.Margin = new Padding(4, 2, 4, 2);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(674, 202);
            tabControl1.TabIndex = 33;
            // 
            // tabClientPreferences
            // 
            tabClientPreferences.Controls.Add(chkCOCForms);
            tabClientPreferences.Controls.Add(label18);
            tabClientPreferences.Controls.Add(cbFeeSched);
            tabClientPreferences.Controls.Add(numDefaultDiscount);
            tabClientPreferences.Controls.Add(label17);
            tabClientPreferences.Controls.Add(chkPrintCPTonBill);
            tabClientPreferences.Controls.Add(chkDoNotBill);
            tabClientPreferences.Controls.Add(chkBillAtDiscount);
            tabClientPreferences.Controls.Add(chkCCReport);
            tabClientPreferences.Controls.Add(chkDateOrder);
            tabClientPreferences.Location = new Point(4, 24);
            tabClientPreferences.Margin = new Padding(4, 2, 4, 2);
            tabClientPreferences.Name = "tabClientPreferences";
            tabClientPreferences.Padding = new Padding(4, 2, 4, 2);
            tabClientPreferences.Size = new Size(666, 174);
            tabClientPreferences.TabIndex = 0;
            tabClientPreferences.Text = "Preferences";
            tabClientPreferences.UseVisualStyleBackColor = true;
            // 
            // chkCOCForms
            // 
            chkCOCForms.Location = new Point(22, 140);
            chkCOCForms.Margin = new Padding(4, 2, 4, 2);
            chkCOCForms.Name = "chkCOCForms";
            chkCOCForms.Size = new Size(296, 20);
            chkCOCForms.TabIndex = 5;
            chkCOCForms.Values.Text = "Include Collection Site on Chain of Custody Forms";
            // 
            // label18
            // 
            label18.Location = new Point(272, 46);
            label18.Margin = new Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new Size(82, 20);
            label18.TabIndex = 8;
            label18.Values.Text = "Fee Schedule";
            // 
            // cbFeeSched
            // 
            cbFeeSched.DropDownWidth = 62;
            cbFeeSched.FormattingEnabled = true;
            cbFeeSched.IntegralHeight = false;
            cbFeeSched.Location = new Point(360, 40);
            cbFeeSched.Margin = new Padding(4, 2, 4, 2);
            cbFeeSched.Name = "cbFeeSched";
            cbFeeSched.Size = new Size(62, 21);
            cbFeeSched.TabIndex = 9;
            cbFeeSched.SelectedIndexChanged += cbFeeSched_SelectedIndexChanged;
            cbFeeSched.Validating += cbFeeSched_Validating;
            // 
            // numDefaultDiscount
            // 
            numDefaultDiscount.Location = new Point(360, 16);
            numDefaultDiscount.Margin = new Padding(4, 2, 4, 2);
            numDefaultDiscount.Name = "numDefaultDiscount";
            numDefaultDiscount.Size = new Size(62, 23);
            numDefaultDiscount.TabIndex = 7;
            // 
            // label17
            // 
            label17.Location = new Point(245, 21);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(115, 20);
            label17.TabIndex = 6;
            label17.Values.Text = "Default Discount %";
            // 
            // chkPrintCPTonBill
            // 
            chkPrintCPTonBill.Location = new Point(22, 115);
            chkPrintCPTonBill.Margin = new Padding(4, 2, 4, 2);
            chkPrintCPTonBill.Name = "chkPrintCPTonBill";
            chkPrintCPTonBill.Size = new Size(133, 20);
            chkPrintCPTonBill.TabIndex = 4;
            chkPrintCPTonBill.Values.Text = "Print CPT on Invoice";
            // 
            // chkDoNotBill
            // 
            chkDoNotBill.Location = new Point(22, 91);
            chkDoNotBill.Margin = new Padding(4, 2, 4, 2);
            chkDoNotBill.Name = "chkDoNotBill";
            chkDoNotBill.Size = new Size(145, 20);
            chkDoNotBill.TabIndex = 3;
            chkDoNotBill.Values.Text = "Do NOT Bill this Client";
            // 
            // chkBillAtDiscount
            // 
            chkBillAtDiscount.Location = new Point(22, 67);
            chkBillAtDiscount.Margin = new Padding(4, 2, 4, 2);
            chkBillAtDiscount.Name = "chkBillAtDiscount";
            chkBillAtDiscount.Size = new Size(105, 20);
            chkBillAtDiscount.TabIndex = 2;
            chkBillAtDiscount.Values.Text = "Bill at Discount";
            // 
            // chkCCReport
            // 
            chkCCReport.Location = new Point(22, 43);
            chkCCReport.Margin = new Padding(4, 2, 4, 2);
            chkCCReport.Name = "chkCCReport";
            chkCCReport.Size = new Size(195, 20);
            chkCCReport.TabIndex = 1;
            chkCCReport.Values.Text = "Include on Charge Code Report";
            // 
            // chkDateOrder
            // 
            chkDateOrder.Location = new Point(22, 17);
            chkDateOrder.Margin = new Padding(4, 2, 4, 2);
            chkDateOrder.Name = "chkDateOrder";
            chkDateOrder.Size = new Size(151, 20);
            chkDateOrder.TabIndex = 0;
            chkDateOrder.Values.Text = "Print Bills in Date Order";
            // 
            // tabClientMRO
            // 
            tabClientMRO.Controls.Add(tbMROZipcode);
            tabClientMRO.Controls.Add(label19);
            tabClientMRO.Controls.Add(cbMROState);
            tabClientMRO.Controls.Add(label20);
            tabClientMRO.Controls.Add(tbMROCity);
            tabClientMRO.Controls.Add(label21);
            tabClientMRO.Controls.Add(MROAddress2TextBox);
            tabClientMRO.Controls.Add(tbMROAddress);
            tabClientMRO.Controls.Add(label22);
            tabClientMRO.Controls.Add(tbMROName);
            tabClientMRO.Controls.Add(label23);
            tabClientMRO.Location = new Point(4, 24);
            tabClientMRO.Margin = new Padding(4, 2, 4, 2);
            tabClientMRO.Name = "tabClientMRO";
            tabClientMRO.Padding = new Padding(4, 2, 4, 2);
            tabClientMRO.Size = new Size(666, 174);
            tabClientMRO.TabIndex = 1;
            tabClientMRO.Text = "Medical Review Officer";
            tabClientMRO.UseVisualStyleBackColor = true;
            // 
            // tbMROZipcode
            // 
            tbMROZipcode.Location = new Point(312, 134);
            tbMROZipcode.Margin = new Padding(4, 2, 4, 2);
            tbMROZipcode.Name = "tbMROZipcode";
            tbMROZipcode.Size = new Size(100, 23);
            tbMROZipcode.TabIndex = 10;
            // 
            // label19
            // 
            label19.Location = new Point(251, 137);
            label19.Margin = new Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new Size(54, 20);
            label19.TabIndex = 9;
            label19.Values.Text = "Zipcode";
            // 
            // cbMROState
            // 
            cbMROState.DropDownStyle = ComboBoxStyle.DropDownList;
            cbMROState.DropDownWidth = 144;
            cbMROState.FormattingEnabled = true;
            cbMROState.IntegralHeight = false;
            cbMROState.Location = new Point(99, 132);
            cbMROState.Margin = new Padding(4, 2, 4, 2);
            cbMROState.Name = "cbMROState";
            cbMROState.Size = new Size(144, 21);
            cbMROState.TabIndex = 8;
            // 
            // label20
            // 
            label20.Location = new Point(56, 137);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(38, 20);
            label20.TabIndex = 7;
            label20.Values.Text = "State";
            // 
            // tbMROCity
            // 
            tbMROCity.Location = new Point(99, 105);
            tbMROCity.Margin = new Padding(4, 2, 4, 2);
            tbMROCity.Name = "tbMROCity";
            tbMROCity.Size = new Size(312, 23);
            tbMROCity.TabIndex = 6;
            // 
            // label21
            // 
            label21.Location = new Point(65, 107);
            label21.Margin = new Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new Size(31, 20);
            label21.TabIndex = 5;
            label21.Values.Text = "City";
            // 
            // MROAddress2TextBox
            // 
            MROAddress2TextBox.Location = new Point(99, 77);
            MROAddress2TextBox.Margin = new Padding(4, 2, 4, 2);
            MROAddress2TextBox.Name = "MROAddress2TextBox";
            MROAddress2TextBox.Size = new Size(312, 23);
            MROAddress2TextBox.TabIndex = 4;
            // 
            // tbMROAddress
            // 
            tbMROAddress.Location = new Point(99, 51);
            tbMROAddress.Margin = new Padding(4, 2, 4, 2);
            tbMROAddress.Name = "tbMROAddress";
            tbMROAddress.Size = new Size(312, 23);
            tbMROAddress.TabIndex = 3;
            // 
            // label22
            // 
            label22.Location = new Point(41, 53);
            label22.Margin = new Padding(4, 0, 4, 0);
            label22.Name = "label22";
            label22.Size = new Size(54, 20);
            label22.TabIndex = 2;
            label22.Values.Text = "Address";
            // 
            // tbMROName
            // 
            tbMROName.Location = new Point(99, 25);
            tbMROName.Margin = new Padding(4, 2, 4, 2);
            tbMROName.Name = "tbMROName";
            tbMROName.Size = new Size(312, 23);
            tbMROName.TabIndex = 1;
            // 
            // label23
            // 
            label23.Location = new Point(52, 28);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(43, 20);
            label23.TabIndex = 0;
            label23.Values.Text = "Name";
            // 
            // tabComment
            // 
            tabComment.Controls.Add(tbComment);
            tabComment.Location = new Point(4, 24);
            tabComment.Margin = new Padding(4, 3, 4, 3);
            tabComment.Name = "tabComment";
            tabComment.Padding = new Padding(4, 3, 4, 3);
            tabComment.Size = new Size(666, 174);
            tabComment.TabIndex = 2;
            tabComment.Text = "Comments";
            tabComment.UseVisualStyleBackColor = true;
            // 
            // tbComment
            // 
            tbComment.Dock = DockStyle.Fill;
            tbComment.Location = new Point(4, 3);
            tbComment.Margin = new Padding(4, 2, 4, 2);
            tbComment.Multiline = true;
            tbComment.Name = "tbComment";
            tbComment.Size = new Size(658, 168);
            tbComment.TabIndex = 37;
            // 
            // DiscountsTab
            // 
            DiscountsTab.Controls.Add(clientDiscountDataGrid);
            DiscountsTab.Location = new Point(4, 24);
            DiscountsTab.Margin = new Padding(4, 3, 4, 3);
            DiscountsTab.Name = "DiscountsTab";
            DiscountsTab.Padding = new Padding(4, 3, 4, 3);
            DiscountsTab.Size = new Size(666, 174);
            DiscountsTab.TabIndex = 3;
            DiscountsTab.Text = "Discounts";
            DiscountsTab.UseVisualStyleBackColor = true;
            // 
            // clientDiscountDataGrid
            // 
            clientDiscountDataGrid.BackgroundColor = Color.White;
            clientDiscountDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            clientDiscountDataGrid.Dock = DockStyle.Fill;
            clientDiscountDataGrid.Location = new Point(4, 3);
            clientDiscountDataGrid.Margin = new Padding(4, 3, 4, 3);
            clientDiscountDataGrid.Name = "clientDiscountDataGrid";
            clientDiscountDataGrid.Size = new Size(658, 168);
            clientDiscountDataGrid.TabIndex = 0;
            clientDiscountDataGrid.CellEndEdit += clientDiscountDataGrid_CellEndEdit;
            clientDiscountDataGrid.CellValueChanged += clientDiscountDataGrid_CellValueChanged;
            clientDiscountDataGrid.RowEnter += clientDiscountDataGrid_RowEnter;
            clientDiscountDataGrid.RowLeave += clientDiscountDataGrid_RowLeave;
            // 
            // tabInterface
            // 
            tabInterface.Controls.Add(interfaceMappingDataGrid);
            tabInterface.Location = new Point(4, 24);
            tabInterface.Margin = new Padding(4, 3, 4, 3);
            tabInterface.Name = "tabInterface";
            tabInterface.Padding = new Padding(4, 3, 4, 3);
            tabInterface.Size = new Size(666, 174);
            tabInterface.TabIndex = 4;
            tabInterface.Text = "Interface";
            tabInterface.UseVisualStyleBackColor = true;
            // 
            // interfaceMappingDataGrid
            // 
            interfaceMappingDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            interfaceMappingDataGrid.Dock = DockStyle.Fill;
            interfaceMappingDataGrid.Location = new Point(4, 3);
            interfaceMappingDataGrid.Margin = new Padding(4, 3, 4, 3);
            interfaceMappingDataGrid.Name = "interfaceMappingDataGrid";
            interfaceMappingDataGrid.Size = new Size(658, 168);
            interfaceMappingDataGrid.TabIndex = 0;
            // 
            // tbZipcode
            // 
            tbZipcode.Location = new Point(331, 165);
            tbZipcode.Margin = new Padding(4, 2, 4, 2);
            tbZipcode.Name = "tbZipcode";
            tbZipcode.Size = new Size(100, 23);
            tbZipcode.TabIndex = 14;
            // 
            // cbCostCenter
            // 
            cbCostCenter.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCostCenter.DropDownWidth = 144;
            cbCostCenter.FormattingEnabled = true;
            cbCostCenter.IntegralHeight = false;
            cbCostCenter.Location = new Point(555, 91);
            cbCostCenter.Margin = new Padding(4, 2, 4, 2);
            cbCostCenter.Name = "cbCostCenter";
            cbCostCenter.Size = new Size(144, 21);
            cbCostCenter.TabIndex = 28;
            cbCostCenter.Validating += cbCostCenter_Validating;
            // 
            // label15
            // 
            label15.Location = new Point(471, 95);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(74, 20);
            label15.TabIndex = 27;
            label15.Values.Text = "Cost Center";
            // 
            // cbEmrType
            // 
            cbEmrType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbEmrType.DropDownWidth = 144;
            cbEmrType.FormattingEnabled = true;
            cbEmrType.IntegralHeight = false;
            cbEmrType.Location = new Point(555, 63);
            cbEmrType.Margin = new Padding(4, 2, 4, 2);
            cbEmrType.Name = "cbEmrType";
            cbEmrType.Size = new Size(144, 21);
            cbEmrType.TabIndex = 26;
            // 
            // label14
            // 
            label14.Location = new Point(476, 69);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(65, 20);
            label14.TabIndex = 25;
            label14.Values.Text = "EMR Type";
            // 
            // cbClientType
            // 
            cbClientType.DropDownStyle = ComboBoxStyle.DropDownList;
            cbClientType.DropDownWidth = 144;
            cbClientType.FormattingEnabled = true;
            cbClientType.IntegralHeight = false;
            cbClientType.Location = new Point(555, 36);
            cbClientType.Margin = new Padding(4, 2, 4, 2);
            cbClientType.Name = "cbClientType";
            cbClientType.Size = new Size(144, 21);
            cbClientType.TabIndex = 24;
            cbClientType.Validating += cbClientType_Validating;
            // 
            // label13
            // 
            label13.Location = new Point(474, 40);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(71, 20);
            label13.TabIndex = 23;
            label13.Values.Text = "Client Type";
            // 
            // tbEmail
            // 
            tbEmail.Location = new Point(119, 273);
            tbEmail.Margin = new Padding(4, 2, 4, 2);
            tbEmail.Name = "tbEmail";
            tbEmail.Size = new Size(269, 23);
            tbEmail.TabIndex = 22;
            // 
            // label12
            // 
            label12.Location = new Point(77, 276);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(40, 20);
            label12.TabIndex = 21;
            label12.Values.Text = "Email";
            // 
            // tbFax
            // 
            tbFax.Location = new Point(119, 248);
            tbFax.Margin = new Padding(4, 2, 4, 2);
            tbFax.Name = "tbFax";
            tbFax.Size = new Size(269, 23);
            tbFax.TabIndex = 20;
            // 
            // label11
            // 
            label11.Location = new Point(86, 250);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(28, 20);
            label11.TabIndex = 19;
            label11.Values.Text = "Fax";
            // 
            // cbCounty
            // 
            cbCounty.DropDownStyle = ComboBoxStyle.DropDownList;
            cbCounty.DropDownWidth = 144;
            cbCounty.FormattingEnabled = true;
            cbCounty.IntegralHeight = false;
            cbCounty.Location = new Point(119, 194);
            cbCounty.Margin = new Padding(4, 2, 4, 2);
            cbCounty.Name = "cbCounty";
            cbCounty.Size = new Size(144, 21);
            cbCounty.TabIndex = 16;
            // 
            // label10
            // 
            label10.Location = new Point(68, 197);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(50, 20);
            label10.TabIndex = 15;
            label10.Values.Text = "County";
            // 
            // tbPhone
            // 
            tbPhone.Location = new Point(119, 223);
            tbPhone.Margin = new Padding(4, 2, 4, 2);
            tbPhone.Name = "tbPhone";
            tbPhone.Size = new Size(269, 23);
            tbPhone.TabIndex = 18;
            // 
            // label9
            // 
            label9.Location = new Point(70, 225);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(45, 20);
            label9.TabIndex = 17;
            label9.Values.Text = "Phone";
            // 
            // label8
            // 
            label8.Location = new Point(271, 168);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(54, 20);
            label8.TabIndex = 13;
            label8.Values.Text = "Zipcode";
            // 
            // cbState
            // 
            cbState.DropDownStyle = ComboBoxStyle.DropDownList;
            cbState.DropDownWidth = 144;
            cbState.FormattingEnabled = true;
            cbState.IntegralHeight = false;
            cbState.Location = new Point(119, 166);
            cbState.Margin = new Padding(4, 2, 4, 2);
            cbState.Name = "cbState";
            cbState.Size = new Size(144, 21);
            cbState.TabIndex = 12;
            // 
            // label7
            // 
            label7.Location = new Point(77, 167);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(38, 20);
            label7.TabIndex = 11;
            label7.Values.Text = "State";
            // 
            // tbCity
            // 
            tbCity.Location = new Point(119, 140);
            tbCity.Margin = new Padding(4, 2, 4, 2);
            tbCity.Name = "tbCity";
            tbCity.Size = new Size(312, 23);
            tbCity.TabIndex = 10;
            // 
            // label6
            // 
            label6.Location = new Point(86, 143);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(31, 20);
            label6.TabIndex = 9;
            label6.Values.Text = "City";
            // 
            // tbAddress2
            // 
            tbAddress2.Location = new Point(119, 114);
            tbAddress2.Margin = new Padding(4, 2, 4, 2);
            tbAddress2.Name = "tbAddress2";
            tbAddress2.Size = new Size(312, 23);
            tbAddress2.TabIndex = 8;
            // 
            // tbAddress1
            // 
            tbAddress1.Location = new Point(119, 89);
            tbAddress1.Margin = new Padding(4, 2, 4, 2);
            tbAddress1.Name = "tbAddress1";
            tbAddress1.Size = new Size(312, 23);
            tbAddress1.TabIndex = 7;
            // 
            // label4
            // 
            label4.Location = new Point(62, 92);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(54, 20);
            label4.TabIndex = 6;
            label4.Values.Text = "Address";
            // 
            // tbClientCode
            // 
            tbClientCode.Location = new Point(345, 37);
            tbClientCode.Margin = new Padding(4, 2, 4, 2);
            tbClientCode.Name = "tbClientCode";
            tbClientCode.Size = new Size(86, 23);
            tbClientCode.TabIndex = 3;
            // 
            // label3
            // 
            label3.Location = new Point(265, 40);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(74, 20);
            label3.TabIndex = 2;
            label3.Values.Text = "Client Code";
            // 
            // ClientMnemTextBox
            // 
            ClientMnemTextBox.Location = new Point(119, 36);
            ClientMnemTextBox.Margin = new Padding(4, 2, 4, 2);
            ClientMnemTextBox.Name = "ClientMnemTextBox";
            ClientMnemTextBox.Size = new Size(138, 23);
            ClientMnemTextBox.TabIndex = 1;
            ClientMnemTextBox.Leave += tbClientMnem_Leave;
            ClientMnemTextBox.Validating += ClientMnemTextBox_Validating;
            // 
            // label2
            // 
            label2.Location = new Point(38, 40);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(80, 20);
            label2.TabIndex = 0;
            label2.Values.Text = "Client Mnem";
            // 
            // ClientNameTextBox
            // 
            ClientNameTextBox.Location = new Point(119, 63);
            ClientNameTextBox.Margin = new Padding(4, 2, 4, 2);
            ClientNameTextBox.Name = "ClientNameTextBox";
            ClientNameTextBox.Size = new Size(312, 23);
            ClientNameTextBox.TabIndex = 5;
            // 
            // label1
            // 
            label1.Location = new Point(40, 67);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(78, 20);
            label1.TabIndex = 4;
            label1.Values.Text = "Client Name";
            // 
            // CancelButton
            // 
            CancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            CancelButton.CausesValidation = false;
            CancelButton.Location = new Point(161, 735);
            CancelButton.Margin = new Padding(4, 2, 4, 2);
            CancelButton.Name = "CancelButton";
            CancelButton.Size = new Size(97, 35);
            CancelButton.TabIndex = 33;
            CancelButton.Values.Text = "Cancel";
            CancelButton.Click += btnCancel_Click;
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            SaveButton.BackColor = Color.PaleGreen;
            SaveButton.Location = new Point(43, 734);
            SaveButton.Margin = new Padding(4, 2, 4, 2);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(97, 36);
            SaveButton.TabIndex = 32;
            SaveButton.Values.Text = "Save";
            SaveButton.Click += btnSave_Click;
            // 
            // label16
            // 
            label16.Location = new Point(475, 123);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(73, 20);
            label16.TabIndex = 29;
            label16.Values.Text = "Bill Method";
            // 
            // billMethodComboBox
            // 
            billMethodComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            billMethodComboBox.DropDownWidth = 144;
            billMethodComboBox.FormattingEnabled = true;
            billMethodComboBox.IntegralHeight = false;
            billMethodComboBox.Items.AddRange(new object[] { "INVOICE", "PATIENT", "PER ACCOUNT" });
            billMethodComboBox.Location = new Point(555, 120);
            billMethodComboBox.Margin = new Padding(4, 2, 4, 2);
            billMethodComboBox.Name = "billMethodComboBox";
            billMethodComboBox.Size = new Size(144, 21);
            billMethodComboBox.TabIndex = 30;
            billMethodComboBox.Validating += billMethodComboBox_Validating;
            // 
            // activeCheckBox
            // 
            activeCheckBox.Checked = true;
            activeCheckBox.CheckState = CheckState.Checked;
            activeCheckBox.Location = new Point(119, 14);
            activeCheckBox.Margin = new Padding(4, 3, 4, 3);
            activeCheckBox.Name = "activeCheckBox";
            activeCheckBox.Size = new Size(57, 20);
            activeCheckBox.TabIndex = 34;
            activeCheckBox.Values.Text = "Active";
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // ClientMaintenanceEditForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.WhiteSmoke;
            ClientSize = new Size(738, 796);
            Controls.Add(activeCheckBox);
            Controls.Add(CancelButton);
            Controls.Add(SaveButton);
            Controls.Add(label5);
            Controls.Add(tbContact);
            Controls.Add(tabControl1);
            Controls.Add(tbZipcode);
            Controls.Add(billMethodComboBox);
            Controls.Add(label16);
            Controls.Add(cbCostCenter);
            Controls.Add(label15);
            Controls.Add(cbEmrType);
            Controls.Add(label14);
            Controls.Add(cbClientType);
            Controls.Add(label13);
            Controls.Add(tbEmail);
            Controls.Add(label12);
            Controls.Add(tbFax);
            Controls.Add(label11);
            Controls.Add(cbCounty);
            Controls.Add(label10);
            Controls.Add(tbPhone);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(cbState);
            Controls.Add(label7);
            Controls.Add(tbCity);
            Controls.Add(label6);
            Controls.Add(tbAddress2);
            Controls.Add(tbAddress1);
            Controls.Add(label4);
            Controls.Add(tbClientCode);
            Controls.Add(label3);
            Controls.Add(ClientMnemTextBox);
            Controls.Add(label2);
            Controls.Add(ClientNameTextBox);
            Controls.Add(label1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ClientMaintenanceEditForm";
            Text = "Client Maintenance";
            Load += ClientMaintenanceEditForm_Load;
            tabControl1.ResumeLayout(false);
            tabClientPreferences.ResumeLayout(false);
            tabClientPreferences.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cbFeeSched).EndInit();
            ((System.ComponentModel.ISupportInitialize)numDefaultDiscount).EndInit();
            tabClientMRO.ResumeLayout(false);
            tabClientMRO.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)cbMROState).EndInit();
            tabComment.ResumeLayout(false);
            tabComment.PerformLayout();
            DiscountsTab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)clientDiscountDataGrid).EndInit();
            tabInterface.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)interfaceMappingDataGrid).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbCostCenter).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbEmrType).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbClientType).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbCounty).EndInit();
            ((System.ComponentModel.ISupportInitialize)cbState).EndInit();
            ((System.ComponentModel.ISupportInitialize)billMethodComboBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Krypton.Toolkit.KryptonLabel label5;
        private Krypton.Toolkit.KryptonTextBox tbContact;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabClientPreferences;
        private Krypton.Toolkit.KryptonCheckBox chkCOCForms;
        private Krypton.Toolkit.KryptonLabel label18;
        private Krypton.Toolkit.KryptonComboBox cbFeeSched;
        private System.Windows.Forms.NumericUpDown numDefaultDiscount;
        private Krypton.Toolkit.KryptonLabel label17;
        private Krypton.Toolkit.KryptonCheckBox chkPrintCPTonBill;
        private Krypton.Toolkit.KryptonCheckBox chkDoNotBill;
        private Krypton.Toolkit.KryptonCheckBox chkBillAtDiscount;
        private Krypton.Toolkit.KryptonCheckBox chkCCReport;
        private Krypton.Toolkit.KryptonCheckBox chkDateOrder;
        private System.Windows.Forms.TabPage tabClientMRO;
        private Krypton.Toolkit.KryptonMaskedTextBox tbMROZipcode;
        private Krypton.Toolkit.KryptonLabel label19;
        private Krypton.Toolkit.KryptonComboBox cbMROState;
        private Krypton.Toolkit.KryptonLabel label20;
        private Krypton.Toolkit.KryptonTextBox tbMROCity;
        private Krypton.Toolkit.KryptonLabel label21;
        private Krypton.Toolkit.KryptonTextBox tbMROAddress;
        private Krypton.Toolkit.KryptonLabel label22;
        private Krypton.Toolkit.KryptonTextBox tbMROName;
        private Krypton.Toolkit.KryptonLabel label23;
        private System.Windows.Forms.TabPage tabComment;
        private Krypton.Toolkit.KryptonTextBox tbComment;
        private Krypton.Toolkit.KryptonMaskedTextBox tbZipcode;
        private Krypton.Toolkit.KryptonComboBox cbCostCenter;
        private Krypton.Toolkit.KryptonLabel label15;
        private Krypton.Toolkit.KryptonComboBox cbEmrType;
        private Krypton.Toolkit.KryptonLabel label14;
        private Krypton.Toolkit.KryptonComboBox cbClientType;
        private Krypton.Toolkit.KryptonLabel label13;
        private Krypton.Toolkit.KryptonTextBox tbEmail;
        private Krypton.Toolkit.KryptonLabel label12;
        private Krypton.Toolkit.KryptonTextBox tbFax;
        private Krypton.Toolkit.KryptonLabel label11;
        private Krypton.Toolkit.KryptonComboBox cbCounty;
        private Krypton.Toolkit.KryptonLabel label10;
        private Krypton.Toolkit.KryptonTextBox tbPhone;
        private Krypton.Toolkit.KryptonLabel label9;
        private Krypton.Toolkit.KryptonLabel label8;
        private Krypton.Toolkit.KryptonComboBox cbState;
        private Krypton.Toolkit.KryptonLabel label7;
        private Krypton.Toolkit.KryptonTextBox tbCity;
        private Krypton.Toolkit.KryptonLabel label6;
        private Krypton.Toolkit.KryptonTextBox tbAddress2;
        private Krypton.Toolkit.KryptonTextBox tbAddress1;
        private Krypton.Toolkit.KryptonLabel label4;
        private Krypton.Toolkit.KryptonTextBox tbClientCode;
        private Krypton.Toolkit.KryptonLabel label3;
        private Krypton.Toolkit.KryptonTextBox ClientMnemTextBox;
        private Krypton.Toolkit.KryptonLabel label2;
        private Krypton.Toolkit.KryptonTextBox ClientNameTextBox;
        private Krypton.Toolkit.KryptonLabel label1;
        private new Krypton.Toolkit.KryptonButton CancelButton;
        private Krypton.Toolkit.KryptonButton SaveButton;
        private Krypton.Toolkit.KryptonTextBox MROAddress2TextBox;
        private System.Windows.Forms.TabPage DiscountsTab;
        private System.Windows.Forms.DataGridView clientDiscountDataGrid;
        private Krypton.Toolkit.KryptonLabel label16;
        private Krypton.Toolkit.KryptonComboBox billMethodComboBox;
        private Krypton.Toolkit.KryptonCheckBox activeCheckBox;
        private System.Windows.Forms.TabPage tabInterface;
        private System.Windows.Forms.DataGridView interfaceMappingDataGrid;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}