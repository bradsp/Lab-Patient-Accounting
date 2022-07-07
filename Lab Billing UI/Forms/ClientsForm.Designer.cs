namespace LabBilling.Forms
{
    partial class ClientsForm
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
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.tbClientName = new System.Windows.Forms.TextBox();
            this.tbClientMnem = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbClientCode = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbAddress1 = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbAddress2 = new System.Windows.Forms.TextBox();
            this.tbCity = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.cbState = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cbCounty = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbFax = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cbClientType = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.cbEmrType = new System.Windows.Forms.ComboBox();
            this.label14 = new System.Windows.Forms.Label();
            this.cbCostCenter = new System.Windows.Forms.ComboBox();
            this.label15 = new System.Windows.Forms.Label();
            this.tbZipcode = new System.Windows.Forms.MaskedTextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabClientPreferences = new System.Windows.Forms.TabPage();
            this.chkCOCForms = new System.Windows.Forms.CheckBox();
            this.label18 = new System.Windows.Forms.Label();
            this.cbFeeSched = new System.Windows.Forms.ComboBox();
            this.numDefaultDiscount = new System.Windows.Forms.NumericUpDown();
            this.label17 = new System.Windows.Forms.Label();
            this.chkPrintCPTonBill = new System.Windows.Forms.CheckBox();
            this.chkDoNotBill = new System.Windows.Forms.CheckBox();
            this.chkBillAtDiscount = new System.Windows.Forms.CheckBox();
            this.chkCCReport = new System.Windows.Forms.CheckBox();
            this.chkDateOrder = new System.Windows.Forms.CheckBox();
            this.tabClientMRO = new System.Windows.Forms.TabPage();
            this.tbMROZipcode = new System.Windows.Forms.MaskedTextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.cbMROState = new System.Windows.Forms.ComboBox();
            this.label20 = new System.Windows.Forms.Label();
            this.tbMROCity = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.tbMROAddress = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.tbMROName = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.tabComment = new System.Windows.Forms.TabPage();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tbContact = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabClientPreferences.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultDiscount)).BeginInit();
            this.tabClientMRO.SuspendLayout();
            this.tabComment.SuspendLayout();
            this.SuspendLayout();
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvClients.Location = new System.Drawing.Point(12, 38);
            this.dgvClients.MultiSelect = false;
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.Size = new System.Drawing.Size(468, 582);
            this.dgvClients.TabIndex = 0;
            this.dgvClients.VirtualMode = true;
            this.dgvClients.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvClients_CellMouseClick);
            this.dgvClients.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.dgvClients_CellValueNeeded);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(512, 66);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Client Name";
            // 
            // tbClientName
            // 
            this.tbClientName.Location = new System.Drawing.Point(580, 63);
            this.tbClientName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbClientName.Name = "tbClientName";
            this.tbClientName.Size = new System.Drawing.Size(268, 20);
            this.tbClientName.TabIndex = 2;
            // 
            // tbClientMnem
            // 
            this.tbClientMnem.Location = new System.Drawing.Point(580, 39);
            this.tbClientMnem.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbClientMnem.Name = "tbClientMnem";
            this.tbClientMnem.Size = new System.Drawing.Size(119, 20);
            this.tbClientMnem.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(511, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Client Mnem";
            // 
            // tbClientCode
            // 
            this.tbClientCode.Location = new System.Drawing.Point(774, 40);
            this.tbClientCode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbClientCode.Name = "tbClientCode";
            this.tbClientCode.Size = new System.Drawing.Size(74, 20);
            this.tbClientCode.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(705, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Client Code";
            // 
            // tbAddress1
            // 
            this.tbAddress1.Location = new System.Drawing.Point(580, 85);
            this.tbAddress1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbAddress1.Name = "tbAddress1";
            this.tbAddress1.Size = new System.Drawing.Size(268, 20);
            this.tbAddress1.TabIndex = 8;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(531, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(45, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Address";
            // 
            // tbAddress2
            // 
            this.tbAddress2.Location = new System.Drawing.Point(580, 107);
            this.tbAddress2.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbAddress2.Name = "tbAddress2";
            this.tbAddress2.Size = new System.Drawing.Size(268, 20);
            this.tbAddress2.TabIndex = 10;
            // 
            // tbCity
            // 
            this.tbCity.Location = new System.Drawing.Point(580, 129);
            this.tbCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbCity.Name = "tbCity";
            this.tbCity.Size = new System.Drawing.Size(268, 20);
            this.tbCity.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(552, 132);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(24, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "City";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(544, 153);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(32, 13);
            this.label7.TabIndex = 13;
            this.label7.Text = "State";
            // 
            // cbState
            // 
            this.cbState.FormattingEnabled = true;
            this.cbState.Location = new System.Drawing.Point(580, 152);
            this.cbState.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbState.Name = "cbState";
            this.cbState.Size = new System.Drawing.Size(124, 21);
            this.cbState.TabIndex = 14;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(710, 154);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(46, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Zipcode";
            // 
            // tbPhone
            // 
            this.tbPhone.Location = new System.Drawing.Point(580, 201);
            this.tbPhone.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(231, 20);
            this.tbPhone.TabIndex = 18;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(538, 203);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Phone";
            // 
            // cbCounty
            // 
            this.cbCounty.FormattingEnabled = true;
            this.cbCounty.Location = new System.Drawing.Point(580, 176);
            this.cbCounty.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbCounty.Name = "cbCounty";
            this.cbCounty.Size = new System.Drawing.Size(124, 21);
            this.cbCounty.TabIndex = 20;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(536, 179);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 13);
            this.label10.TabIndex = 19;
            this.label10.Text = "County";
            // 
            // tbFax
            // 
            this.tbFax.Location = new System.Drawing.Point(580, 223);
            this.tbFax.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbFax.Name = "tbFax";
            this.tbFax.Size = new System.Drawing.Size(231, 20);
            this.tbFax.TabIndex = 22;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(552, 225);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(24, 13);
            this.label11.TabIndex = 21;
            this.label11.Text = "Fax";
            // 
            // tbEmail
            // 
            this.tbEmail.Location = new System.Drawing.Point(580, 245);
            this.tbEmail.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(231, 20);
            this.tbEmail.TabIndex = 24;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(544, 247);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(32, 13);
            this.label12.TabIndex = 23;
            this.label12.Text = "Email";
            // 
            // cbClientType
            // 
            this.cbClientType.FormattingEnabled = true;
            this.cbClientType.Location = new System.Drawing.Point(954, 40);
            this.cbClientType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbClientType.Name = "cbClientType";
            this.cbClientType.Size = new System.Drawing.Size(124, 21);
            this.cbClientType.TabIndex = 26;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(884, 43);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(60, 13);
            this.label13.TabIndex = 25;
            this.label13.Text = "Client Type";
            // 
            // cbEmrType
            // 
            this.cbEmrType.FormattingEnabled = true;
            this.cbEmrType.Location = new System.Drawing.Point(954, 65);
            this.cbEmrType.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbEmrType.Name = "cbEmrType";
            this.cbEmrType.Size = new System.Drawing.Size(124, 21);
            this.cbEmrType.TabIndex = 28;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(886, 68);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(58, 13);
            this.label14.TabIndex = 27;
            this.label14.Text = "EMR Type";
            // 
            // cbCostCenter
            // 
            this.cbCostCenter.FormattingEnabled = true;
            this.cbCostCenter.Location = new System.Drawing.Point(954, 87);
            this.cbCostCenter.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbCostCenter.Name = "cbCostCenter";
            this.cbCostCenter.Size = new System.Drawing.Size(124, 21);
            this.cbCostCenter.TabIndex = 30;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(882, 90);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(62, 13);
            this.label15.TabIndex = 29;
            this.label15.Text = "Cost Center";
            // 
            // tbZipcode
            // 
            this.tbZipcode.Location = new System.Drawing.Point(762, 151);
            this.tbZipcode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbZipcode.Name = "tbZipcode";
            this.tbZipcode.Size = new System.Drawing.Size(86, 20);
            this.tbZipcode.TabIndex = 31;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabClientPreferences);
            this.tabControl1.Controls.Add(this.tabClientMRO);
            this.tabControl1.Controls.Add(this.tabComment);
            this.tabControl1.Location = new System.Drawing.Point(514, 442);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(483, 175);
            this.tabControl1.TabIndex = 32;
            // 
            // tabClientPreferences
            // 
            this.tabClientPreferences.Controls.Add(this.chkCOCForms);
            this.tabClientPreferences.Controls.Add(this.label18);
            this.tabClientPreferences.Controls.Add(this.cbFeeSched);
            this.tabClientPreferences.Controls.Add(this.numDefaultDiscount);
            this.tabClientPreferences.Controls.Add(this.label17);
            this.tabClientPreferences.Controls.Add(this.chkPrintCPTonBill);
            this.tabClientPreferences.Controls.Add(this.chkDoNotBill);
            this.tabClientPreferences.Controls.Add(this.chkBillAtDiscount);
            this.tabClientPreferences.Controls.Add(this.chkCCReport);
            this.tabClientPreferences.Controls.Add(this.chkDateOrder);
            this.tabClientPreferences.Location = new System.Drawing.Point(4, 22);
            this.tabClientPreferences.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabClientPreferences.Name = "tabClientPreferences";
            this.tabClientPreferences.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabClientPreferences.Size = new System.Drawing.Size(475, 149);
            this.tabClientPreferences.TabIndex = 0;
            this.tabClientPreferences.Text = "Preferences";
            this.tabClientPreferences.UseVisualStyleBackColor = true;
            // 
            // chkCOCForms
            // 
            this.chkCOCForms.AutoSize = true;
            this.chkCOCForms.Location = new System.Drawing.Point(19, 121);
            this.chkCOCForms.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkCOCForms.Name = "chkCOCForms";
            this.chkCOCForms.Size = new System.Drawing.Size(260, 17);
            this.chkCOCForms.TabIndex = 9;
            this.chkCOCForms.Text = "Include Collection Site on Chain of Custody Forms";
            this.chkCOCForms.UseVisualStyleBackColor = true;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(233, 40);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(73, 13);
            this.label18.TabIndex = 8;
            this.label18.Text = "Fee Schedule";
            // 
            // cbFeeSched
            // 
            this.cbFeeSched.FormattingEnabled = true;
            this.cbFeeSched.Location = new System.Drawing.Point(309, 35);
            this.cbFeeSched.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbFeeSched.Name = "cbFeeSched";
            this.cbFeeSched.Size = new System.Drawing.Size(54, 21);
            this.cbFeeSched.TabIndex = 7;
            // 
            // numDefaultDiscount
            // 
            this.numDefaultDiscount.Location = new System.Drawing.Point(309, 14);
            this.numDefaultDiscount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.numDefaultDiscount.Name = "numDefaultDiscount";
            this.numDefaultDiscount.Size = new System.Drawing.Size(53, 20);
            this.numDefaultDiscount.TabIndex = 6;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(210, 18);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(97, 13);
            this.label17.TabIndex = 5;
            this.label17.Text = "Default Discount %";
            // 
            // chkPrintCPTonBill
            // 
            this.chkPrintCPTonBill.AutoSize = true;
            this.chkPrintCPTonBill.Location = new System.Drawing.Point(19, 100);
            this.chkPrintCPTonBill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkPrintCPTonBill.Name = "chkPrintCPTonBill";
            this.chkPrintCPTonBill.Size = new System.Drawing.Size(124, 17);
            this.chkPrintCPTonBill.TabIndex = 4;
            this.chkPrintCPTonBill.Text = "Print CPT on Invoice";
            this.chkPrintCPTonBill.UseVisualStyleBackColor = true;
            // 
            // chkDoNotBill
            // 
            this.chkDoNotBill.AutoSize = true;
            this.chkDoNotBill.Location = new System.Drawing.Point(19, 79);
            this.chkDoNotBill.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDoNotBill.Name = "chkDoNotBill";
            this.chkDoNotBill.Size = new System.Drawing.Size(130, 17);
            this.chkDoNotBill.TabIndex = 3;
            this.chkDoNotBill.Text = "Do NOT Bill this Client";
            this.chkDoNotBill.UseVisualStyleBackColor = true;
            // 
            // chkBillAtDiscount
            // 
            this.chkBillAtDiscount.AutoSize = true;
            this.chkBillAtDiscount.Location = new System.Drawing.Point(19, 58);
            this.chkBillAtDiscount.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkBillAtDiscount.Name = "chkBillAtDiscount";
            this.chkBillAtDiscount.Size = new System.Drawing.Size(96, 17);
            this.chkBillAtDiscount.TabIndex = 2;
            this.chkBillAtDiscount.Text = "Bill at Discount";
            this.chkBillAtDiscount.UseVisualStyleBackColor = true;
            // 
            // chkCCReport
            // 
            this.chkCCReport.AutoSize = true;
            this.chkCCReport.Location = new System.Drawing.Point(19, 37);
            this.chkCCReport.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkCCReport.Name = "chkCCReport";
            this.chkCCReport.Size = new System.Drawing.Size(176, 17);
            this.chkCCReport.TabIndex = 1;
            this.chkCCReport.Text = "Include on Charge Code Report";
            this.chkCCReport.UseVisualStyleBackColor = true;
            // 
            // chkDateOrder
            // 
            this.chkDateOrder.AutoSize = true;
            this.chkDateOrder.Location = new System.Drawing.Point(19, 15);
            this.chkDateOrder.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.chkDateOrder.Name = "chkDateOrder";
            this.chkDateOrder.Size = new System.Drawing.Size(134, 17);
            this.chkDateOrder.TabIndex = 0;
            this.chkDateOrder.Text = "Print Bills in Date Order";
            this.chkDateOrder.UseVisualStyleBackColor = true;
            // 
            // tabClientMRO
            // 
            this.tabClientMRO.Controls.Add(this.tbMROZipcode);
            this.tabClientMRO.Controls.Add(this.label19);
            this.tabClientMRO.Controls.Add(this.cbMROState);
            this.tabClientMRO.Controls.Add(this.label20);
            this.tabClientMRO.Controls.Add(this.tbMROCity);
            this.tabClientMRO.Controls.Add(this.label21);
            this.tabClientMRO.Controls.Add(this.tbMROAddress);
            this.tabClientMRO.Controls.Add(this.label22);
            this.tabClientMRO.Controls.Add(this.tbMROName);
            this.tabClientMRO.Controls.Add(this.label23);
            this.tabClientMRO.Location = new System.Drawing.Point(4, 22);
            this.tabClientMRO.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabClientMRO.Name = "tabClientMRO";
            this.tabClientMRO.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tabClientMRO.Size = new System.Drawing.Size(475, 149);
            this.tabClientMRO.TabIndex = 1;
            this.tabClientMRO.Text = "Medical Review Officer";
            this.tabClientMRO.UseVisualStyleBackColor = true;
            // 
            // tbMROZipcode
            // 
            this.tbMROZipcode.Location = new System.Drawing.Point(267, 91);
            this.tbMROZipcode.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbMROZipcode.Name = "tbMROZipcode";
            this.tbMROZipcode.Size = new System.Drawing.Size(86, 20);
            this.tbMROZipcode.TabIndex = 41;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(215, 94);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 13);
            this.label19.TabIndex = 40;
            this.label19.Text = "Zipcode";
            // 
            // cbMROState
            // 
            this.cbMROState.FormattingEnabled = true;
            this.cbMROState.Location = new System.Drawing.Point(85, 89);
            this.cbMROState.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.cbMROState.Name = "cbMROState";
            this.cbMROState.Size = new System.Drawing.Size(124, 21);
            this.cbMROState.TabIndex = 39;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(48, 94);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(32, 13);
            this.label20.TabIndex = 38;
            this.label20.Text = "State";
            // 
            // tbMROCity
            // 
            this.tbMROCity.Location = new System.Drawing.Point(85, 66);
            this.tbMROCity.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbMROCity.Name = "tbMROCity";
            this.tbMROCity.Size = new System.Drawing.Size(268, 20);
            this.tbMROCity.TabIndex = 37;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(56, 68);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(24, 13);
            this.label21.TabIndex = 36;
            this.label21.Text = "City";
            // 
            // tbMROAddress
            // 
            this.tbMROAddress.Location = new System.Drawing.Point(85, 44);
            this.tbMROAddress.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbMROAddress.Name = "tbMROAddress";
            this.tbMROAddress.Size = new System.Drawing.Size(268, 20);
            this.tbMROAddress.TabIndex = 35;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(35, 46);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(45, 13);
            this.label22.TabIndex = 34;
            this.label22.Text = "Address";
            // 
            // tbMROName
            // 
            this.tbMROName.Location = new System.Drawing.Point(85, 22);
            this.tbMROName.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbMROName.Name = "tbMROName";
            this.tbMROName.Size = new System.Drawing.Size(268, 20);
            this.tbMROName.TabIndex = 33;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(45, 24);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(35, 13);
            this.label23.TabIndex = 32;
            this.label23.Text = "Name";
            // 
            // tabComment
            // 
            this.tabComment.Controls.Add(this.tbComment);
            this.tabComment.Location = new System.Drawing.Point(4, 22);
            this.tabComment.Name = "tabComment";
            this.tabComment.Padding = new System.Windows.Forms.Padding(3);
            this.tabComment.Size = new System.Drawing.Size(475, 149);
            this.tabComment.TabIndex = 2;
            this.tabComment.Text = "Comments";
            this.tabComment.UseVisualStyleBackColor = true;
            // 
            // tbComment
            // 
            this.tbComment.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbComment.Location = new System.Drawing.Point(3, 3);
            this.tbComment.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbComment.Multiline = true;
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(469, 143);
            this.tbComment.TabIndex = 37;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.PaleGreen;
            this.btnSave.Location = new System.Drawing.Point(1255, 552);
            this.btnSave.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(83, 31);
            this.btnSave.TabIndex = 33;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.Location = new System.Drawing.Point(1255, 587);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(83, 30);
            this.btnCancel.TabIndex = 34;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // tbContact
            // 
            this.tbContact.Location = new System.Drawing.Point(515, 286);
            this.tbContact.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.tbContact.Multiline = true;
            this.tbContact.Name = "tbContact";
            this.tbContact.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.tbContact.Size = new System.Drawing.Size(429, 152);
            this.tbContact.TabIndex = 35;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(515, 271);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 13);
            this.label5.TabIndex = 36;
            this.label5.Text = "Contact";
            // 
            // ClientsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 634);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbContact);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.tbZipcode);
            this.Controls.Add(this.cbCostCenter);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.cbEmrType);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.cbClientType);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.tbEmail);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.tbFax);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.cbCounty);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.tbPhone);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.cbState);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.tbCity);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tbAddress2);
            this.Controls.Add(this.tbAddress1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbClientCode);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbClientMnem);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbClientName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dgvClients);
            this.Name = "ClientsForm";
            this.ShowInTaskbar = false;
            this.Text = "Clients";
            this.Load += new System.EventHandler(this.Clients_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabClientPreferences.ResumeLayout(false);
            this.tabClientPreferences.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numDefaultDiscount)).EndInit();
            this.tabClientMRO.ResumeLayout(false);
            this.tabClientMRO.PerformLayout();
            this.tabComment.ResumeLayout(false);
            this.tabComment.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbClientName;
        private System.Windows.Forms.TextBox tbClientMnem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbClientCode;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbAddress1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbAddress2;
        private System.Windows.Forms.TextBox tbCity;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox cbState;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cbCounty;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbFax;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox cbClientType;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ComboBox cbEmrType;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox cbCostCenter;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.MaskedTextBox tbZipcode;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabClientPreferences;
        private System.Windows.Forms.TabPage tabClientMRO;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbContact;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.NumericUpDown numDefaultDiscount;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox chkPrintCPTonBill;
        private System.Windows.Forms.CheckBox chkDoNotBill;
        private System.Windows.Forms.CheckBox chkBillAtDiscount;
        private System.Windows.Forms.CheckBox chkCCReport;
        private System.Windows.Forms.CheckBox chkDateOrder;
        private System.Windows.Forms.CheckBox chkCOCForms;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.ComboBox cbFeeSched;
        private System.Windows.Forms.MaskedTextBox tbMROZipcode;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.ComboBox cbMROState;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox tbMROCity;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox tbMROAddress;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox tbMROName;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TabPage tabComment;
    }
}