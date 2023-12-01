namespace LabBilling.Forms
{
    partial class ChargeMasterEditForm
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
            this.cptTabControl = new System.Windows.Forms.TabControl();
            this.feeSched1tab = new System.Windows.Forms.TabPage();
            this.feeSched1Grid = new System.Windows.Forms.DataGridView();
            this.feeSched2tab = new System.Windows.Forms.TabPage();
            this.feeSched2Grid = new System.Windows.Forms.DataGridView();
            this.feeSched3tab = new System.Windows.Forms.TabPage();
            this.feeSched3Grid = new System.Windows.Forms.DataGridView();
            this.feeSched4tab = new System.Windows.Forms.TabPage();
            this.feeSched4Grid = new System.Windows.Forms.DataGridView();
            this.feeSched5tab = new System.Windows.Forms.TabPage();
            this.feeSched5Grid = new System.Windows.Forms.DataGridView();
            this.chargeIdLabel = new System.Windows.Forms.Label();
            this.descriptionLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.chargeIdTextBox = new System.Windows.Forms.TextBox();
            this.descriptionTextBox = new System.Windows.Forms.TextBox();
            this.lisOrderCodeTextBox = new System.Windows.Forms.TextBox();
            this.notesLabel = new System.Windows.Forms.Label();
            this.notesTextBox = new System.Windows.Forms.TextBox();
            this.thirdPartyGroup = new System.Windows.Forms.GroupBox();
            this.mProfTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.patChargeType = new System.Windows.Forms.ComboBox();
            this.clientGroup = new System.Windows.Forms.GroupBox();
            this.cProfTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.clientChargeType = new System.Windows.Forms.ComboBox();
            this.zGroup = new System.Windows.Forms.GroupBox();
            this.zProfTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.zChargeType = new System.Windows.Forms.ComboBox();
            this.refLabGroup = new System.Windows.Forms.GroupBox();
            this.refLabPayment = new System.Windows.Forms.TextBox();
            this.refLabBillCodeTextBox = new System.Windows.Forms.TextBox();
            this.refLabIdTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.costTextBox = new System.Windows.Forms.TextBox();
            this.orderableCheckBox = new System.Windows.Forms.CheckBox();
            this.deletedCheckBox = new System.Windows.Forms.CheckBox();
            this.cbillDetailCheckBox = new System.Windows.Forms.CheckBox();
            this.saveButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.variablePriceCheckBox = new System.Windows.Forms.CheckBox();
            this.cptTabControl.SuspendLayout();
            this.feeSched1tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feeSched1Grid)).BeginInit();
            this.feeSched2tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feeSched2Grid)).BeginInit();
            this.feeSched3tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feeSched3Grid)).BeginInit();
            this.feeSched4tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feeSched4Grid)).BeginInit();
            this.feeSched5tab.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.feeSched5Grid)).BeginInit();
            this.thirdPartyGroup.SuspendLayout();
            this.clientGroup.SuspendLayout();
            this.zGroup.SuspendLayout();
            this.refLabGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // cptTabControl
            // 
            this.cptTabControl.Controls.Add(this.feeSched1tab);
            this.cptTabControl.Controls.Add(this.feeSched2tab);
            this.cptTabControl.Controls.Add(this.feeSched3tab);
            this.cptTabControl.Controls.Add(this.feeSched4tab);
            this.cptTabControl.Controls.Add(this.feeSched5tab);
            this.cptTabControl.Location = new System.Drawing.Point(12, 285);
            this.cptTabControl.Name = "cptTabControl";
            this.cptTabControl.SelectedIndex = 0;
            this.cptTabControl.Size = new System.Drawing.Size(1070, 235);
            this.cptTabControl.TabIndex = 17;
            this.cptTabControl.SelectedIndexChanged += new System.EventHandler(this.cptTabControl_SelectedIndexChanged);
            // 
            // feeSched1tab
            // 
            this.feeSched1tab.Controls.Add(this.feeSched1Grid);
            this.feeSched1tab.Location = new System.Drawing.Point(4, 22);
            this.feeSched1tab.Name = "feeSched1tab";
            this.feeSched1tab.Size = new System.Drawing.Size(1062, 209);
            this.feeSched1tab.TabIndex = 0;
            this.feeSched1tab.Text = "Fee Schedule 1";
            this.feeSched1tab.UseVisualStyleBackColor = true;
            // 
            // feeSched1Grid
            // 
            this.feeSched1Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.feeSched1Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.feeSched1Grid.Location = new System.Drawing.Point(0, 0);
            this.feeSched1Grid.Name = "feeSched1Grid";
            this.feeSched1Grid.Size = new System.Drawing.Size(1062, 209);
            this.feeSched1Grid.TabIndex = 0;
            // 
            // feeSched2tab
            // 
            this.feeSched2tab.Controls.Add(this.feeSched2Grid);
            this.feeSched2tab.Location = new System.Drawing.Point(4, 22);
            this.feeSched2tab.Name = "feeSched2tab";
            this.feeSched2tab.Size = new System.Drawing.Size(1062, 209);
            this.feeSched2tab.TabIndex = 1;
            this.feeSched2tab.Text = "Fee Schedule 2";
            this.feeSched2tab.UseVisualStyleBackColor = true;
            // 
            // feeSched2Grid
            // 
            this.feeSched2Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.feeSched2Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.feeSched2Grid.Location = new System.Drawing.Point(0, 0);
            this.feeSched2Grid.Name = "feeSched2Grid";
            this.feeSched2Grid.Size = new System.Drawing.Size(1062, 209);
            this.feeSched2Grid.TabIndex = 0;
            // 
            // feeSched3tab
            // 
            this.feeSched3tab.Controls.Add(this.feeSched3Grid);
            this.feeSched3tab.Location = new System.Drawing.Point(4, 22);
            this.feeSched3tab.Name = "feeSched3tab";
            this.feeSched3tab.Size = new System.Drawing.Size(1062, 209);
            this.feeSched3tab.TabIndex = 2;
            this.feeSched3tab.Text = "Fee Schedule 3";
            this.feeSched3tab.UseVisualStyleBackColor = true;
            // 
            // feeSched3Grid
            // 
            this.feeSched3Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.feeSched3Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.feeSched3Grid.Location = new System.Drawing.Point(0, 0);
            this.feeSched3Grid.Name = "feeSched3Grid";
            this.feeSched3Grid.Size = new System.Drawing.Size(1062, 209);
            this.feeSched3Grid.TabIndex = 0;
            // 
            // feeSched4tab
            // 
            this.feeSched4tab.Controls.Add(this.feeSched4Grid);
            this.feeSched4tab.Location = new System.Drawing.Point(4, 22);
            this.feeSched4tab.Name = "feeSched4tab";
            this.feeSched4tab.Size = new System.Drawing.Size(1062, 209);
            this.feeSched4tab.TabIndex = 3;
            this.feeSched4tab.Text = "FeeSchedule4";
            this.feeSched4tab.UseVisualStyleBackColor = true;
            // 
            // feeSched4Grid
            // 
            this.feeSched4Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.feeSched4Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.feeSched4Grid.Location = new System.Drawing.Point(0, 0);
            this.feeSched4Grid.Name = "feeSched4Grid";
            this.feeSched4Grid.Size = new System.Drawing.Size(1062, 209);
            this.feeSched4Grid.TabIndex = 0;
            // 
            // feeSched5tab
            // 
            this.feeSched5tab.Controls.Add(this.feeSched5Grid);
            this.feeSched5tab.Location = new System.Drawing.Point(4, 22);
            this.feeSched5tab.Name = "feeSched5tab";
            this.feeSched5tab.Size = new System.Drawing.Size(1062, 209);
            this.feeSched5tab.TabIndex = 4;
            this.feeSched5tab.Text = "Fee Schedule 5";
            this.feeSched5tab.UseVisualStyleBackColor = true;
            // 
            // feeSched5Grid
            // 
            this.feeSched5Grid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.feeSched5Grid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.feeSched5Grid.Location = new System.Drawing.Point(0, 0);
            this.feeSched5Grid.Name = "feeSched5Grid";
            this.feeSched5Grid.Size = new System.Drawing.Size(1062, 209);
            this.feeSched5Grid.TabIndex = 0;
            // 
            // chargeIdLabel
            // 
            this.chargeIdLabel.AutoSize = true;
            this.chargeIdLabel.Location = new System.Drawing.Point(31, 29);
            this.chargeIdLabel.Name = "chargeIdLabel";
            this.chargeIdLabel.Size = new System.Drawing.Size(31, 13);
            this.chargeIdLabel.TabIndex = 0;
            this.chargeIdLabel.Text = "CDM";
            // 
            // descriptionLabel
            // 
            this.descriptionLabel.AutoSize = true;
            this.descriptionLabel.Location = new System.Drawing.Point(31, 55);
            this.descriptionLabel.Name = "descriptionLabel";
            this.descriptionLabel.Size = new System.Drawing.Size(60, 13);
            this.descriptionLabel.TabIndex = 1;
            this.descriptionLabel.Text = "Description";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 106);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "LIS Order Code";
            // 
            // chargeIdTextBox
            // 
            this.chargeIdTextBox.Location = new System.Drawing.Point(68, 26);
            this.chargeIdTextBox.Name = "chargeIdTextBox";
            this.chargeIdTextBox.Size = new System.Drawing.Size(100, 20);
            this.chargeIdTextBox.TabIndex = 4;
            this.chargeIdTextBox.Leave += new System.EventHandler(this.chargeIdTextBox_Leave);
            // 
            // descriptionTextBox
            // 
            this.descriptionTextBox.Location = new System.Drawing.Point(34, 73);
            this.descriptionTextBox.Name = "descriptionTextBox";
            this.descriptionTextBox.Size = new System.Drawing.Size(306, 20);
            this.descriptionTextBox.TabIndex = 5;
            // 
            // lisOrderCodeTextBox
            // 
            this.lisOrderCodeTextBox.Location = new System.Drawing.Point(117, 103);
            this.lisOrderCodeTextBox.Name = "lisOrderCodeTextBox";
            this.lisOrderCodeTextBox.Size = new System.Drawing.Size(100, 20);
            this.lisOrderCodeTextBox.TabIndex = 6;
            // 
            // notesLabel
            // 
            this.notesLabel.AutoSize = true;
            this.notesLabel.Location = new System.Drawing.Point(343, 184);
            this.notesLabel.Name = "notesLabel";
            this.notesLabel.Size = new System.Drawing.Size(35, 13);
            this.notesLabel.TabIndex = 15;
            this.notesLabel.Text = "Notes";
            // 
            // notesTextBox
            // 
            this.notesTextBox.Location = new System.Drawing.Point(346, 200);
            this.notesTextBox.Multiline = true;
            this.notesTextBox.Name = "notesTextBox";
            this.notesTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.notesTextBox.Size = new System.Drawing.Size(544, 68);
            this.notesTextBox.TabIndex = 16;
            // 
            // thirdPartyGroup
            // 
            this.thirdPartyGroup.Controls.Add(this.mProfTextBox);
            this.thirdPartyGroup.Controls.Add(this.label5);
            this.thirdPartyGroup.Controls.Add(this.label1);
            this.thirdPartyGroup.Controls.Add(this.patChargeType);
            this.thirdPartyGroup.Location = new System.Drawing.Point(346, 25);
            this.thirdPartyGroup.Name = "thirdPartyGroup";
            this.thirdPartyGroup.Size = new System.Drawing.Size(159, 156);
            this.thirdPartyGroup.TabIndex = 11;
            this.thirdPartyGroup.TabStop = false;
            this.thirdPartyGroup.Text = "Third Party/Patient";
            // 
            // mProfTextBox
            // 
            this.mProfTextBox.Location = new System.Drawing.Point(20, 97);
            this.mProfTextBox.Name = "mProfTextBox";
            this.mProfTextBox.Size = new System.Drawing.Size(121, 20);
            this.mProfTextBox.TabIndex = 3;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(17, 81);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(113, 13);
            this.label5.TabIndex = 2;
            this.label5.Text = "Professional Allocation";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Type";
            // 
            // patChargeType
            // 
            this.patChargeType.FormattingEnabled = true;
            this.patChargeType.Items.AddRange(new object[] {
            "NORM",
            "SPLIT",
            "N/A",
            "MISC"});
            this.patChargeType.Location = new System.Drawing.Point(20, 47);
            this.patChargeType.Name = "patChargeType";
            this.patChargeType.Size = new System.Drawing.Size(121, 21);
            this.patChargeType.TabIndex = 1;
            // 
            // clientGroup
            // 
            this.clientGroup.Controls.Add(this.cProfTextBox);
            this.clientGroup.Controls.Add(this.label6);
            this.clientGroup.Controls.Add(this.label2);
            this.clientGroup.Controls.Add(this.clientChargeType);
            this.clientGroup.Location = new System.Drawing.Point(524, 26);
            this.clientGroup.Name = "clientGroup";
            this.clientGroup.Size = new System.Drawing.Size(159, 156);
            this.clientGroup.TabIndex = 12;
            this.clientGroup.TabStop = false;
            this.clientGroup.Text = "Client";
            // 
            // cProfTextBox
            // 
            this.cProfTextBox.Location = new System.Drawing.Point(22, 96);
            this.cProfTextBox.Name = "cProfTextBox";
            this.cProfTextBox.Size = new System.Drawing.Size(121, 20);
            this.cProfTextBox.TabIndex = 3;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(19, 80);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(113, 13);
            this.label6.TabIndex = 2;
            this.label6.Text = "Professional Allocation";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(19, 26);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(31, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Type";
            // 
            // clientChargeType
            // 
            this.clientChargeType.FormattingEnabled = true;
            this.clientChargeType.Items.AddRange(new object[] {
            "NORM",
            "SPLIT",
            "MISC",
            "N/A"});
            this.clientChargeType.Location = new System.Drawing.Point(22, 46);
            this.clientChargeType.Name = "clientChargeType";
            this.clientChargeType.Size = new System.Drawing.Size(121, 21);
            this.clientChargeType.TabIndex = 1;
            // 
            // zGroup
            // 
            this.zGroup.Controls.Add(this.zProfTextBox);
            this.zGroup.Controls.Add(this.label7);
            this.zGroup.Controls.Add(this.label4);
            this.zGroup.Controls.Add(this.zChargeType);
            this.zGroup.Location = new System.Drawing.Point(699, 25);
            this.zGroup.Name = "zGroup";
            this.zGroup.Size = new System.Drawing.Size(159, 156);
            this.zGroup.TabIndex = 13;
            this.zGroup.TabStop = false;
            this.zGroup.Text = "Z";
            // 
            // zProfTextBox
            // 
            this.zProfTextBox.Location = new System.Drawing.Point(22, 97);
            this.zProfTextBox.Name = "zProfTextBox";
            this.zProfTextBox.Size = new System.Drawing.Size(121, 20);
            this.zProfTextBox.TabIndex = 3;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(19, 81);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(113, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "Professional Allocation";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(19, 27);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(31, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Type";
            // 
            // zChargeType
            // 
            this.zChargeType.FormattingEnabled = true;
            this.zChargeType.Items.AddRange(new object[] {
            "NORM",
            "SPLIT",
            "MISC",
            "N/A"});
            this.zChargeType.Location = new System.Drawing.Point(22, 47);
            this.zChargeType.Name = "zChargeType";
            this.zChargeType.Size = new System.Drawing.Size(121, 21);
            this.zChargeType.TabIndex = 1;
            // 
            // refLabGroup
            // 
            this.refLabGroup.Controls.Add(this.refLabPayment);
            this.refLabGroup.Controls.Add(this.refLabBillCodeTextBox);
            this.refLabGroup.Controls.Add(this.refLabIdTextBox);
            this.refLabGroup.Controls.Add(this.label10);
            this.refLabGroup.Controls.Add(this.label9);
            this.refLabGroup.Controls.Add(this.label8);
            this.refLabGroup.Location = new System.Drawing.Point(875, 27);
            this.refLabGroup.Name = "refLabGroup";
            this.refLabGroup.Size = new System.Drawing.Size(157, 165);
            this.refLabGroup.TabIndex = 14;
            this.refLabGroup.TabStop = false;
            this.refLabGroup.Text = "Reference Lab";
            // 
            // refLabPayment
            // 
            this.refLabPayment.Location = new System.Drawing.Point(23, 124);
            this.refLabPayment.Name = "refLabPayment";
            this.refLabPayment.Size = new System.Drawing.Size(115, 20);
            this.refLabPayment.TabIndex = 5;
            // 
            // refLabBillCodeTextBox
            // 
            this.refLabBillCodeTextBox.Location = new System.Drawing.Point(23, 85);
            this.refLabBillCodeTextBox.Name = "refLabBillCodeTextBox";
            this.refLabBillCodeTextBox.Size = new System.Drawing.Size(115, 20);
            this.refLabBillCodeTextBox.TabIndex = 3;
            // 
            // refLabIdTextBox
            // 
            this.refLabIdTextBox.Location = new System.Drawing.Point(23, 44);
            this.refLabIdTextBox.Name = "refLabIdTextBox";
            this.refLabIdTextBox.Size = new System.Drawing.Size(115, 20);
            this.refLabIdTextBox.TabIndex = 1;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(20, 108);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(48, 13);
            this.label10.TabIndex = 4;
            this.label10.Text = "Payment";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(20, 69);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(48, 13);
            this.label9.TabIndex = 2;
            this.label9.Text = "Bill Code";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(20, 28);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(18, 13);
            this.label8.TabIndex = 0;
            this.label8.Text = "ID";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(31, 132);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(28, 13);
            this.label11.TabIndex = 3;
            this.label11.Text = "Cost";
            // 
            // costTextBox
            // 
            this.costTextBox.Location = new System.Drawing.Point(117, 129);
            this.costTextBox.Name = "costTextBox";
            this.costTextBox.Size = new System.Drawing.Size(100, 20);
            this.costTextBox.TabIndex = 7;
            // 
            // orderableCheckBox
            // 
            this.orderableCheckBox.AutoSize = true;
            this.orderableCheckBox.Location = new System.Drawing.Point(12, 202);
            this.orderableCheckBox.Name = "orderableCheckBox";
            this.orderableCheckBox.Size = new System.Drawing.Size(72, 17);
            this.orderableCheckBox.TabIndex = 8;
            this.orderableCheckBox.Text = "Orderable";
            this.orderableCheckBox.UseVisualStyleBackColor = true;
            // 
            // deletedCheckBox
            // 
            this.deletedCheckBox.AutoSize = true;
            this.deletedCheckBox.Location = new System.Drawing.Point(12, 178);
            this.deletedCheckBox.Name = "deletedCheckBox";
            this.deletedCheckBox.Size = new System.Drawing.Size(63, 17);
            this.deletedCheckBox.TabIndex = 9;
            this.deletedCheckBox.Text = "Deleted";
            this.deletedCheckBox.UseVisualStyleBackColor = true;
            // 
            // cbillDetailCheckBox
            // 
            this.cbillDetailCheckBox.AutoSize = true;
            this.cbillDetailCheckBox.Location = new System.Drawing.Point(12, 225);
            this.cbillDetailCheckBox.Name = "cbillDetailCheckBox";
            this.cbillDetailCheckBox.Size = new System.Drawing.Size(120, 17);
            this.cbillDetailCheckBox.TabIndex = 10;
            this.cbillDetailCheckBox.Text = "Client Invoice Detail";
            this.cbillDetailCheckBox.UseVisualStyleBackColor = true;
            // 
            // saveButton
            // 
            this.saveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.saveButton.Location = new System.Drawing.Point(922, 528);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(75, 23);
            this.saveButton.TabIndex = 18;
            this.saveButton.Text = "Save";
            this.saveButton.UseVisualStyleBackColor = true;
            this.saveButton.Click += new System.EventHandler(this.saveButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(1003, 528);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 18;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // variablePriceCheckBox
            // 
            this.variablePriceCheckBox.AutoSize = true;
            this.variablePriceCheckBox.Location = new System.Drawing.Point(11, 248);
            this.variablePriceCheckBox.Name = "variablePriceCheckBox";
            this.variablePriceCheckBox.Size = new System.Drawing.Size(91, 17);
            this.variablePriceCheckBox.TabIndex = 19;
            this.variablePriceCheckBox.Text = "Variable Price";
            this.variablePriceCheckBox.UseVisualStyleBackColor = true;
            // 
            // ChargeMasterEditForm
            // 
            this.AcceptButton = this.saveButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(1094, 563);
            this.Controls.Add(this.variablePriceCheckBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.cbillDetailCheckBox);
            this.Controls.Add(this.deletedCheckBox);
            this.Controls.Add(this.orderableCheckBox);
            this.Controls.Add(this.refLabGroup);
            this.Controls.Add(this.zGroup);
            this.Controls.Add(this.clientGroup);
            this.Controls.Add(this.thirdPartyGroup);
            this.Controls.Add(this.notesTextBox);
            this.Controls.Add(this.costTextBox);
            this.Controls.Add(this.lisOrderCodeTextBox);
            this.Controls.Add(this.descriptionTextBox);
            this.Controls.Add(this.notesLabel);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.chargeIdTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.descriptionLabel);
            this.Controls.Add(this.chargeIdLabel);
            this.Controls.Add(this.cptTabControl);
            this.Name = "ChargeMasterEditForm";
            this.Text = "Edit Charge Master";
            this.Load += new System.EventHandler(this.ChargeMasterEditForm_Load);
            this.cptTabControl.ResumeLayout(false);
            this.feeSched1tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.feeSched1Grid)).EndInit();
            this.feeSched2tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.feeSched2Grid)).EndInit();
            this.feeSched3tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.feeSched3Grid)).EndInit();
            this.feeSched4tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.feeSched4Grid)).EndInit();
            this.feeSched5tab.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.feeSched5Grid)).EndInit();
            this.thirdPartyGroup.ResumeLayout(false);
            this.thirdPartyGroup.PerformLayout();
            this.clientGroup.ResumeLayout(false);
            this.clientGroup.PerformLayout();
            this.zGroup.ResumeLayout(false);
            this.zGroup.PerformLayout();
            this.refLabGroup.ResumeLayout(false);
            this.refLabGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TabControl cptTabControl;
        private System.Windows.Forms.TabPage feeSched1tab;
        private System.Windows.Forms.TabPage feeSched2tab;
        private System.Windows.Forms.TabPage feeSched3tab;
        private System.Windows.Forms.TabPage feeSched4tab;
        private System.Windows.Forms.TabPage feeSched5tab;
        private System.Windows.Forms.DataGridView feeSched1Grid;
        private System.Windows.Forms.DataGridView feeSched2Grid;
        private System.Windows.Forms.DataGridView feeSched3Grid;
        private System.Windows.Forms.DataGridView feeSched4Grid;
        private System.Windows.Forms.DataGridView feeSched5Grid;
        private System.Windows.Forms.Label chargeIdLabel;
        private System.Windows.Forms.Label descriptionLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox chargeIdTextBox;
        private System.Windows.Forms.TextBox descriptionTextBox;
        private System.Windows.Forms.TextBox lisOrderCodeTextBox;
        private System.Windows.Forms.Label notesLabel;
        private System.Windows.Forms.TextBox notesTextBox;
        private System.Windows.Forms.GroupBox thirdPartyGroup;
        private System.Windows.Forms.TextBox mProfTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox patChargeType;
        private System.Windows.Forms.GroupBox clientGroup;
        private System.Windows.Forms.TextBox cProfTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox clientChargeType;
        private System.Windows.Forms.GroupBox zGroup;
        private System.Windows.Forms.TextBox zProfTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox zChargeType;
        private System.Windows.Forms.GroupBox refLabGroup;
        private System.Windows.Forms.TextBox refLabPayment;
        private System.Windows.Forms.TextBox refLabBillCodeTextBox;
        private System.Windows.Forms.TextBox refLabIdTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox costTextBox;
        private System.Windows.Forms.CheckBox orderableCheckBox;
        private System.Windows.Forms.CheckBox deletedCheckBox;
        private System.Windows.Forms.CheckBox cbillDetailCheckBox;
        private System.Windows.Forms.Button saveButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.CheckBox variablePriceCheckBox;
    }
}