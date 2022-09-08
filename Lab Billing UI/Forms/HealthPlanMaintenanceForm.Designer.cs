namespace LabBilling.Forms
{
    partial class HealthPlanMaintenanceForm
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
            this.healthPlanListView = new MetroFramework.Controls.MetroListView();
            this.includeDeletedCheckBox = new MetroFramework.Controls.MetroCheckBox();
            this.insCodeTextBox = new MetroFramework.Controls.MetroTextBox();
            this.insCodeLabel = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox1 = new MetroFramework.Controls.MetroTextBox();
            this.planNameLabel = new MetroFramework.Controls.MetroLabel();
            this.address1TextBox = new MetroFramework.Controls.MetroTextBox();
            this.addressLabel = new MetroFramework.Controls.MetroLabel();
            this.address2TextBox = new MetroFramework.Controls.MetroTextBox();
            this.planCityTextBox = new MetroFramework.Controls.MetroTextBox();
            this.planCityLabel = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox5 = new MetroFramework.Controls.MetroTextBox();
            this.planStateLabel = new MetroFramework.Controls.MetroLabel();
            this.planZipCodeTextBox = new MetroFramework.Controls.MetroTextBox();
            this.planZipCodeLabel = new MetroFramework.Controls.MetroLabel();
            this.claimTypeComboBox = new MetroFramework.Controls.MetroComboBox();
            this.claimTypeLabel = new MetroFramework.Controls.MetroLabel();
            this.providerNoQualifierLabel = new MetroFramework.Controls.MetroLabel();
            this.providerNoQualifierTextBox = new MetroFramework.Controls.MetroTextBox();
            this.providerNoLabel = new MetroFramework.Controls.MetroLabel();
            this.providerNoTextBox = new MetroFramework.Controls.MetroTextBox();
            this.payerNoLabel = new MetroFramework.Controls.MetroLabel();
            this.metroTextBox3 = new MetroFramework.Controls.MetroTextBox();
            this.finCodeComboBox = new MetroFramework.Controls.MetroComboBox();
            this.finCodeLabel = new MetroFramework.Controls.MetroLabel();
            this.commentsTextBox = new MetroFramework.Controls.MetroTextBox();
            this.commentsLabel = new MetroFramework.Controls.MetroLabel();
            this.isMedicareHmoCheckBox = new MetroFramework.Controls.MetroCheckBox();
            this.allowOutpatientBillingCheckBox = new MetroFramework.Controls.MetroCheckBox();
            this.payorCodeLabel = new MetroFramework.Controls.MetroLabel();
            this.payorCodeTextBox = new MetroFramework.Controls.MetroTextBox();
            this.finClassLabel = new MetroFramework.Controls.MetroLabel();
            this.finClassComboBox = new MetroFramework.Controls.MetroComboBox();
            this.billAsJmcghCheckBox = new MetroFramework.Controls.MetroCheckBox();
            this.nthrivePayerNoLabel = new MetroFramework.Controls.MetroLabel();
            this.nThrivePayerNoTextBox = new MetroFramework.Controls.MetroTextBox();
            this.insuranceTypeLabel = new MetroFramework.Controls.MetroLabel();
            this.insuranceTypeComboBox = new MetroFramework.Controls.MetroComboBox();
            this.saveButton = new MetroFramework.Controls.MetroButton();
            this.SuspendLayout();
            // 
            // healthPlanListView
            // 
            this.healthPlanListView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.healthPlanListView.Font = new System.Drawing.Font("Segoe UI", 12F);
            this.healthPlanListView.FullRowSelect = true;
            this.healthPlanListView.Location = new System.Drawing.Point(12, 35);
            this.healthPlanListView.Name = "healthPlanListView";
            this.healthPlanListView.OwnerDraw = true;
            this.healthPlanListView.Size = new System.Drawing.Size(187, 403);
            this.healthPlanListView.TabIndex = 1;
            this.healthPlanListView.UseCompatibleStateImageBehavior = false;
            this.healthPlanListView.UseSelectable = true;
            // 
            // includeDeletedCheckBox
            // 
            this.includeDeletedCheckBox.AutoSize = true;
            this.includeDeletedCheckBox.Location = new System.Drawing.Point(12, 12);
            this.includeDeletedCheckBox.Name = "includeDeletedCheckBox";
            this.includeDeletedCheckBox.Size = new System.Drawing.Size(106, 15);
            this.includeDeletedCheckBox.TabIndex = 0;
            this.includeDeletedCheckBox.Text = "Include Inactive";
            this.includeDeletedCheckBox.UseSelectable = true;
            // 
            // insCodeTextBox
            // 
            // 
            // 
            // 
            this.insCodeTextBox.CustomButton.Image = null;
            this.insCodeTextBox.CustomButton.Location = new System.Drawing.Point(73, 1);
            this.insCodeTextBox.CustomButton.Name = "";
            this.insCodeTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.insCodeTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.insCodeTextBox.CustomButton.TabIndex = 1;
            this.insCodeTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.insCodeTextBox.CustomButton.UseSelectable = true;
            this.insCodeTextBox.CustomButton.Visible = false;
            this.insCodeTextBox.Lines = new string[0];
            this.insCodeTextBox.Location = new System.Drawing.Point(375, 45);
            this.insCodeTextBox.MaxLength = 32767;
            this.insCodeTextBox.Name = "insCodeTextBox";
            this.insCodeTextBox.PasswordChar = '\0';
            this.insCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.insCodeTextBox.SelectedText = "";
            this.insCodeTextBox.SelectionLength = 0;
            this.insCodeTextBox.SelectionStart = 0;
            this.insCodeTextBox.ShortcutsEnabled = true;
            this.insCodeTextBox.Size = new System.Drawing.Size(95, 23);
            this.insCodeTextBox.TabIndex = 3;
            this.insCodeTextBox.UseSelectable = true;
            this.insCodeTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.insCodeTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // insCodeLabel
            // 
            this.insCodeLabel.AutoSize = true;
            this.insCodeLabel.Location = new System.Drawing.Point(220, 49);
            this.insCodeLabel.Name = "insCodeLabel";
            this.insCodeLabel.Size = new System.Drawing.Size(149, 19);
            this.insCodeLabel.TabIndex = 2;
            this.insCodeLabel.Text = "Insurance Code (mnem)";
            // 
            // metroTextBox1
            // 
            // 
            // 
            // 
            this.metroTextBox1.CustomButton.Image = null;
            this.metroTextBox1.CustomButton.Location = new System.Drawing.Point(191, 1);
            this.metroTextBox1.CustomButton.Name = "";
            this.metroTextBox1.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox1.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox1.CustomButton.TabIndex = 1;
            this.metroTextBox1.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox1.CustomButton.UseSelectable = true;
            this.metroTextBox1.CustomButton.Visible = false;
            this.metroTextBox1.Lines = new string[0];
            this.metroTextBox1.Location = new System.Drawing.Point(375, 74);
            this.metroTextBox1.MaxLength = 32767;
            this.metroTextBox1.Name = "metroTextBox1";
            this.metroTextBox1.PasswordChar = '\0';
            this.metroTextBox1.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox1.SelectedText = "";
            this.metroTextBox1.SelectionLength = 0;
            this.metroTextBox1.SelectionStart = 0;
            this.metroTextBox1.ShortcutsEnabled = true;
            this.metroTextBox1.Size = new System.Drawing.Size(213, 23);
            this.metroTextBox1.TabIndex = 5;
            this.metroTextBox1.UseSelectable = true;
            this.metroTextBox1.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox1.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // planNameLabel
            // 
            this.planNameLabel.AutoSize = true;
            this.planNameLabel.Location = new System.Drawing.Point(295, 78);
            this.planNameLabel.Name = "planNameLabel";
            this.planNameLabel.Size = new System.Drawing.Size(74, 19);
            this.planNameLabel.TabIndex = 4;
            this.planNameLabel.Text = "Plan Name";
            // 
            // address1TextBox
            // 
            // 
            // 
            // 
            this.address1TextBox.CustomButton.Image = null;
            this.address1TextBox.CustomButton.Location = new System.Drawing.Point(191, 1);
            this.address1TextBox.CustomButton.Name = "";
            this.address1TextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.address1TextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.address1TextBox.CustomButton.TabIndex = 1;
            this.address1TextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.address1TextBox.CustomButton.UseSelectable = true;
            this.address1TextBox.CustomButton.Visible = false;
            this.address1TextBox.Lines = new string[0];
            this.address1TextBox.Location = new System.Drawing.Point(375, 103);
            this.address1TextBox.MaxLength = 32767;
            this.address1TextBox.Name = "address1TextBox";
            this.address1TextBox.PasswordChar = '\0';
            this.address1TextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.address1TextBox.SelectedText = "";
            this.address1TextBox.SelectionLength = 0;
            this.address1TextBox.SelectionStart = 0;
            this.address1TextBox.ShortcutsEnabled = true;
            this.address1TextBox.Size = new System.Drawing.Size(213, 23);
            this.address1TextBox.TabIndex = 7;
            this.address1TextBox.UseSelectable = true;
            this.address1TextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.address1TextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // addressLabel
            // 
            this.addressLabel.AutoSize = true;
            this.addressLabel.Location = new System.Drawing.Point(313, 107);
            this.addressLabel.Name = "addressLabel";
            this.addressLabel.Size = new System.Drawing.Size(56, 19);
            this.addressLabel.TabIndex = 6;
            this.addressLabel.Text = "Address";
            // 
            // address2TextBox
            // 
            // 
            // 
            // 
            this.address2TextBox.CustomButton.Image = null;
            this.address2TextBox.CustomButton.Location = new System.Drawing.Point(191, 1);
            this.address2TextBox.CustomButton.Name = "";
            this.address2TextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.address2TextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.address2TextBox.CustomButton.TabIndex = 1;
            this.address2TextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.address2TextBox.CustomButton.UseSelectable = true;
            this.address2TextBox.CustomButton.Visible = false;
            this.address2TextBox.Lines = new string[0];
            this.address2TextBox.Location = new System.Drawing.Point(375, 132);
            this.address2TextBox.MaxLength = 32767;
            this.address2TextBox.Name = "address2TextBox";
            this.address2TextBox.PasswordChar = '\0';
            this.address2TextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.address2TextBox.SelectedText = "";
            this.address2TextBox.SelectionLength = 0;
            this.address2TextBox.SelectionStart = 0;
            this.address2TextBox.ShortcutsEnabled = true;
            this.address2TextBox.Size = new System.Drawing.Size(213, 23);
            this.address2TextBox.TabIndex = 8;
            this.address2TextBox.UseSelectable = true;
            this.address2TextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.address2TextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // planCityTextBox
            // 
            // 
            // 
            // 
            this.planCityTextBox.CustomButton.Image = null;
            this.planCityTextBox.CustomButton.Location = new System.Drawing.Point(191, 1);
            this.planCityTextBox.CustomButton.Name = "";
            this.planCityTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.planCityTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.planCityTextBox.CustomButton.TabIndex = 1;
            this.planCityTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.planCityTextBox.CustomButton.UseSelectable = true;
            this.planCityTextBox.CustomButton.Visible = false;
            this.planCityTextBox.Lines = new string[0];
            this.planCityTextBox.Location = new System.Drawing.Point(375, 161);
            this.planCityTextBox.MaxLength = 32767;
            this.planCityTextBox.Name = "planCityTextBox";
            this.planCityTextBox.PasswordChar = '\0';
            this.planCityTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.planCityTextBox.SelectedText = "";
            this.planCityTextBox.SelectionLength = 0;
            this.planCityTextBox.SelectionStart = 0;
            this.planCityTextBox.ShortcutsEnabled = true;
            this.planCityTextBox.Size = new System.Drawing.Size(213, 23);
            this.planCityTextBox.TabIndex = 10;
            this.planCityTextBox.UseSelectable = true;
            this.planCityTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.planCityTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // planCityLabel
            // 
            this.planCityLabel.AutoSize = true;
            this.planCityLabel.Location = new System.Drawing.Point(338, 165);
            this.planCityLabel.Name = "planCityLabel";
            this.planCityLabel.Size = new System.Drawing.Size(31, 19);
            this.planCityLabel.TabIndex = 9;
            this.planCityLabel.Text = "City";
            // 
            // metroTextBox5
            // 
            // 
            // 
            // 
            this.metroTextBox5.CustomButton.Image = null;
            this.metroTextBox5.CustomButton.Location = new System.Drawing.Point(191, 1);
            this.metroTextBox5.CustomButton.Name = "";
            this.metroTextBox5.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox5.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox5.CustomButton.TabIndex = 1;
            this.metroTextBox5.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox5.CustomButton.UseSelectable = true;
            this.metroTextBox5.CustomButton.Visible = false;
            this.metroTextBox5.Lines = new string[0];
            this.metroTextBox5.Location = new System.Drawing.Point(375, 190);
            this.metroTextBox5.MaxLength = 32767;
            this.metroTextBox5.Name = "metroTextBox5";
            this.metroTextBox5.PasswordChar = '\0';
            this.metroTextBox5.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox5.SelectedText = "";
            this.metroTextBox5.SelectionLength = 0;
            this.metroTextBox5.SelectionStart = 0;
            this.metroTextBox5.ShortcutsEnabled = true;
            this.metroTextBox5.Size = new System.Drawing.Size(213, 23);
            this.metroTextBox5.TabIndex = 12;
            this.metroTextBox5.UseSelectable = true;
            this.metroTextBox5.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox5.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // planStateLabel
            // 
            this.planStateLabel.AutoSize = true;
            this.planStateLabel.Location = new System.Drawing.Point(331, 194);
            this.planStateLabel.Name = "planStateLabel";
            this.planStateLabel.Size = new System.Drawing.Size(38, 19);
            this.planStateLabel.TabIndex = 11;
            this.planStateLabel.Text = "State";
            // 
            // planZipCodeTextBox
            // 
            // 
            // 
            // 
            this.planZipCodeTextBox.CustomButton.Image = null;
            this.planZipCodeTextBox.CustomButton.Location = new System.Drawing.Point(101, 1);
            this.planZipCodeTextBox.CustomButton.Name = "";
            this.planZipCodeTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.planZipCodeTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.planZipCodeTextBox.CustomButton.TabIndex = 1;
            this.planZipCodeTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.planZipCodeTextBox.CustomButton.UseSelectable = true;
            this.planZipCodeTextBox.CustomButton.Visible = false;
            this.planZipCodeTextBox.Lines = new string[0];
            this.planZipCodeTextBox.Location = new System.Drawing.Point(375, 219);
            this.planZipCodeTextBox.MaxLength = 32767;
            this.planZipCodeTextBox.Name = "planZipCodeTextBox";
            this.planZipCodeTextBox.PasswordChar = '\0';
            this.planZipCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.planZipCodeTextBox.SelectedText = "";
            this.planZipCodeTextBox.SelectionLength = 0;
            this.planZipCodeTextBox.SelectionStart = 0;
            this.planZipCodeTextBox.ShortcutsEnabled = true;
            this.planZipCodeTextBox.Size = new System.Drawing.Size(123, 23);
            this.planZipCodeTextBox.TabIndex = 14;
            this.planZipCodeTextBox.UseSelectable = true;
            this.planZipCodeTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.planZipCodeTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // planZipCodeLabel
            // 
            this.planZipCodeLabel.AutoSize = true;
            this.planZipCodeLabel.Location = new System.Drawing.Point(305, 223);
            this.planZipCodeLabel.Name = "planZipCodeLabel";
            this.planZipCodeLabel.Size = new System.Drawing.Size(64, 19);
            this.planZipCodeLabel.TabIndex = 13;
            this.planZipCodeLabel.Text = "Zip Code";
            // 
            // claimTypeComboBox
            // 
            this.claimTypeComboBox.FormattingEnabled = true;
            this.claimTypeComboBox.ItemHeight = 23;
            this.claimTypeComboBox.Location = new System.Drawing.Point(787, 101);
            this.claimTypeComboBox.Name = "claimTypeComboBox";
            this.claimTypeComboBox.Size = new System.Drawing.Size(141, 29);
            this.claimTypeComboBox.TabIndex = 22;
            this.claimTypeComboBox.UseSelectable = true;
            // 
            // claimTypeLabel
            // 
            this.claimTypeLabel.AutoSize = true;
            this.claimTypeLabel.Location = new System.Drawing.Point(707, 107);
            this.claimTypeLabel.Name = "claimTypeLabel";
            this.claimTypeLabel.Size = new System.Drawing.Size(74, 19);
            this.claimTypeLabel.TabIndex = 21;
            this.claimTypeLabel.Text = "Claim Type";
            // 
            // providerNoQualifierLabel
            // 
            this.providerNoQualifierLabel.AutoSize = true;
            this.providerNoQualifierLabel.Location = new System.Drawing.Point(646, 140);
            this.providerNoQualifierLabel.Name = "providerNoQualifierLabel";
            this.providerNoQualifierLabel.Size = new System.Drawing.Size(135, 19);
            this.providerNoQualifierLabel.TabIndex = 23;
            this.providerNoQualifierLabel.Text = "Provider No Qualifier";
            // 
            // providerNoQualifierTextBox
            // 
            // 
            // 
            // 
            this.providerNoQualifierTextBox.CustomButton.Image = null;
            this.providerNoQualifierTextBox.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.providerNoQualifierTextBox.CustomButton.Name = "";
            this.providerNoQualifierTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.providerNoQualifierTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.providerNoQualifierTextBox.CustomButton.TabIndex = 1;
            this.providerNoQualifierTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.providerNoQualifierTextBox.CustomButton.UseSelectable = true;
            this.providerNoQualifierTextBox.CustomButton.Visible = false;
            this.providerNoQualifierTextBox.Lines = new string[0];
            this.providerNoQualifierTextBox.Location = new System.Drawing.Point(787, 136);
            this.providerNoQualifierTextBox.MaxLength = 32767;
            this.providerNoQualifierTextBox.Name = "providerNoQualifierTextBox";
            this.providerNoQualifierTextBox.PasswordChar = '\0';
            this.providerNoQualifierTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.providerNoQualifierTextBox.SelectedText = "";
            this.providerNoQualifierTextBox.SelectionLength = 0;
            this.providerNoQualifierTextBox.SelectionStart = 0;
            this.providerNoQualifierTextBox.ShortcutsEnabled = true;
            this.providerNoQualifierTextBox.Size = new System.Drawing.Size(141, 23);
            this.providerNoQualifierTextBox.TabIndex = 24;
            this.providerNoQualifierTextBox.UseSelectable = true;
            this.providerNoQualifierTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.providerNoQualifierTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // providerNoLabel
            // 
            this.providerNoLabel.AutoSize = true;
            this.providerNoLabel.Location = new System.Drawing.Point(700, 169);
            this.providerNoLabel.Name = "providerNoLabel";
            this.providerNoLabel.Size = new System.Drawing.Size(81, 19);
            this.providerNoLabel.TabIndex = 25;
            this.providerNoLabel.Text = "Provider No";
            // 
            // providerNoTextBox
            // 
            // 
            // 
            // 
            this.providerNoTextBox.CustomButton.Image = null;
            this.providerNoTextBox.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.providerNoTextBox.CustomButton.Name = "";
            this.providerNoTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.providerNoTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.providerNoTextBox.CustomButton.TabIndex = 1;
            this.providerNoTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.providerNoTextBox.CustomButton.UseSelectable = true;
            this.providerNoTextBox.CustomButton.Visible = false;
            this.providerNoTextBox.Lines = new string[0];
            this.providerNoTextBox.Location = new System.Drawing.Point(787, 165);
            this.providerNoTextBox.MaxLength = 32767;
            this.providerNoTextBox.Name = "providerNoTextBox";
            this.providerNoTextBox.PasswordChar = '\0';
            this.providerNoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.providerNoTextBox.SelectedText = "";
            this.providerNoTextBox.SelectionLength = 0;
            this.providerNoTextBox.SelectionStart = 0;
            this.providerNoTextBox.ShortcutsEnabled = true;
            this.providerNoTextBox.Size = new System.Drawing.Size(141, 23);
            this.providerNoTextBox.TabIndex = 26;
            this.providerNoTextBox.UseSelectable = true;
            this.providerNoTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.providerNoTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // payerNoLabel
            // 
            this.payerNoLabel.AutoSize = true;
            this.payerNoLabel.Location = new System.Drawing.Point(718, 198);
            this.payerNoLabel.Name = "payerNoLabel";
            this.payerNoLabel.Size = new System.Drawing.Size(63, 19);
            this.payerNoLabel.TabIndex = 27;
            this.payerNoLabel.Text = "Payer No";
            // 
            // metroTextBox3
            // 
            // 
            // 
            // 
            this.metroTextBox3.CustomButton.Image = null;
            this.metroTextBox3.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.metroTextBox3.CustomButton.Name = "";
            this.metroTextBox3.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.metroTextBox3.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.metroTextBox3.CustomButton.TabIndex = 1;
            this.metroTextBox3.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.metroTextBox3.CustomButton.UseSelectable = true;
            this.metroTextBox3.CustomButton.Visible = false;
            this.metroTextBox3.Lines = new string[0];
            this.metroTextBox3.Location = new System.Drawing.Point(787, 194);
            this.metroTextBox3.MaxLength = 32767;
            this.metroTextBox3.Name = "metroTextBox3";
            this.metroTextBox3.PasswordChar = '\0';
            this.metroTextBox3.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.metroTextBox3.SelectedText = "";
            this.metroTextBox3.SelectionLength = 0;
            this.metroTextBox3.SelectionStart = 0;
            this.metroTextBox3.ShortcutsEnabled = true;
            this.metroTextBox3.Size = new System.Drawing.Size(141, 23);
            this.metroTextBox3.TabIndex = 28;
            this.metroTextBox3.UseSelectable = true;
            this.metroTextBox3.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.metroTextBox3.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // finCodeComboBox
            // 
            this.finCodeComboBox.FormattingEnabled = true;
            this.finCodeComboBox.ItemHeight = 23;
            this.finCodeComboBox.Location = new System.Drawing.Point(787, 27);
            this.finCodeComboBox.Name = "finCodeComboBox";
            this.finCodeComboBox.Size = new System.Drawing.Size(141, 29);
            this.finCodeComboBox.TabIndex = 18;
            this.finCodeComboBox.UseSelectable = true;
            // 
            // finCodeLabel
            // 
            this.finCodeLabel.AutoSize = true;
            this.finCodeLabel.Location = new System.Drawing.Point(686, 34);
            this.finCodeLabel.Name = "finCodeLabel";
            this.finCodeLabel.Size = new System.Drawing.Size(95, 19);
            this.finCodeLabel.TabIndex = 17;
            this.finCodeLabel.Text = "Financial Code";
            // 
            // commentsTextBox
            // 
            // 
            // 
            // 
            this.commentsTextBox.CustomButton.Image = null;
            this.commentsTextBox.CustomButton.Location = new System.Drawing.Point(285, 2);
            this.commentsTextBox.CustomButton.Name = "";
            this.commentsTextBox.CustomButton.Size = new System.Drawing.Size(95, 95);
            this.commentsTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.commentsTextBox.CustomButton.TabIndex = 1;
            this.commentsTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.commentsTextBox.CustomButton.UseSelectable = true;
            this.commentsTextBox.CustomButton.Visible = false;
            this.commentsTextBox.Lines = new string[0];
            this.commentsTextBox.Location = new System.Drawing.Point(205, 338);
            this.commentsTextBox.MaxLength = 32767;
            this.commentsTextBox.Multiline = true;
            this.commentsTextBox.Name = "commentsTextBox";
            this.commentsTextBox.PasswordChar = '\0';
            this.commentsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.commentsTextBox.SelectedText = "";
            this.commentsTextBox.SelectionLength = 0;
            this.commentsTextBox.SelectionStart = 0;
            this.commentsTextBox.ShortcutsEnabled = true;
            this.commentsTextBox.Size = new System.Drawing.Size(383, 100);
            this.commentsTextBox.TabIndex = 16;
            this.commentsTextBox.UseSelectable = true;
            this.commentsTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.commentsTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // commentsLabel
            // 
            this.commentsLabel.AutoSize = true;
            this.commentsLabel.Location = new System.Drawing.Point(205, 316);
            this.commentsLabel.Name = "commentsLabel";
            this.commentsLabel.Size = new System.Drawing.Size(73, 19);
            this.commentsLabel.TabIndex = 15;
            this.commentsLabel.Text = "Comments";
            // 
            // isMedicareHmoCheckBox
            // 
            this.isMedicareHmoCheckBox.AutoSize = true;
            this.isMedicareHmoCheckBox.Location = new System.Drawing.Point(787, 322);
            this.isMedicareHmoCheckBox.Name = "isMedicareHmoCheckBox";
            this.isMedicareHmoCheckBox.Size = new System.Drawing.Size(115, 15);
            this.isMedicareHmoCheckBox.TabIndex = 35;
            this.isMedicareHmoCheckBox.Text = "Is Medicare HMO";
            this.isMedicareHmoCheckBox.UseSelectable = true;
            // 
            // allowOutpatientBillingCheckBox
            // 
            this.allowOutpatientBillingCheckBox.AutoSize = true;
            this.allowOutpatientBillingCheckBox.Location = new System.Drawing.Point(787, 345);
            this.allowOutpatientBillingCheckBox.Name = "allowOutpatientBillingCheckBox";
            this.allowOutpatientBillingCheckBox.Size = new System.Drawing.Size(149, 15);
            this.allowOutpatientBillingCheckBox.TabIndex = 36;
            this.allowOutpatientBillingCheckBox.Text = "Allow Outpatient Billing";
            this.allowOutpatientBillingCheckBox.UseSelectable = true;
            // 
            // payorCodeLabel
            // 
            this.payorCodeLabel.AutoSize = true;
            this.payorCodeLabel.Location = new System.Drawing.Point(703, 227);
            this.payorCodeLabel.Name = "payorCodeLabel";
            this.payorCodeLabel.Size = new System.Drawing.Size(78, 19);
            this.payorCodeLabel.TabIndex = 29;
            this.payorCodeLabel.Text = "Payor Code";
            // 
            // payorCodeTextBox
            // 
            // 
            // 
            // 
            this.payorCodeTextBox.CustomButton.Image = null;
            this.payorCodeTextBox.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.payorCodeTextBox.CustomButton.Name = "";
            this.payorCodeTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.payorCodeTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.payorCodeTextBox.CustomButton.TabIndex = 1;
            this.payorCodeTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.payorCodeTextBox.CustomButton.UseSelectable = true;
            this.payorCodeTextBox.CustomButton.Visible = false;
            this.payorCodeTextBox.Lines = new string[0];
            this.payorCodeTextBox.Location = new System.Drawing.Point(787, 223);
            this.payorCodeTextBox.MaxLength = 32767;
            this.payorCodeTextBox.Name = "payorCodeTextBox";
            this.payorCodeTextBox.PasswordChar = '\0';
            this.payorCodeTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.payorCodeTextBox.SelectedText = "";
            this.payorCodeTextBox.SelectionLength = 0;
            this.payorCodeTextBox.SelectionStart = 0;
            this.payorCodeTextBox.ShortcutsEnabled = true;
            this.payorCodeTextBox.Size = new System.Drawing.Size(141, 23);
            this.payorCodeTextBox.TabIndex = 30;
            this.payorCodeTextBox.UseSelectable = true;
            this.payorCodeTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.payorCodeTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // finClassLabel
            // 
            this.finClassLabel.AutoSize = true;
            this.finClassLabel.Location = new System.Drawing.Point(686, 69);
            this.finClassLabel.Name = "finClassLabel";
            this.finClassLabel.Size = new System.Drawing.Size(92, 19);
            this.finClassLabel.TabIndex = 19;
            this.finClassLabel.Text = "Financial Class";
            // 
            // finClassComboBox
            // 
            this.finClassComboBox.FormattingEnabled = true;
            this.finClassComboBox.ItemHeight = 23;
            this.finClassComboBox.Location = new System.Drawing.Point(787, 62);
            this.finClassComboBox.Name = "finClassComboBox";
            this.finClassComboBox.Size = new System.Drawing.Size(141, 29);
            this.finClassComboBox.TabIndex = 20;
            this.finClassComboBox.UseSelectable = true;
            // 
            // billAsJmcghCheckBox
            // 
            this.billAsJmcghCheckBox.AutoSize = true;
            this.billAsJmcghCheckBox.Location = new System.Drawing.Point(787, 368);
            this.billAsJmcghCheckBox.Name = "billAsJmcghCheckBox";
            this.billAsJmcghCheckBox.Size = new System.Drawing.Size(96, 15);
            this.billAsJmcghCheckBox.TabIndex = 37;
            this.billAsJmcghCheckBox.Text = "Bill as JMCGH";
            this.billAsJmcghCheckBox.UseSelectable = true;
            // 
            // nthrivePayerNoLabel
            // 
            this.nthrivePayerNoLabel.AutoSize = true;
            this.nthrivePayerNoLabel.Location = new System.Drawing.Point(669, 256);
            this.nthrivePayerNoLabel.Name = "nthrivePayerNoLabel";
            this.nthrivePayerNoLabel.Size = new System.Drawing.Size(112, 19);
            this.nthrivePayerNoLabel.TabIndex = 31;
            this.nthrivePayerNoLabel.Text = "NThrive Payer No";
            // 
            // nThrivePayerNoTextBox
            // 
            // 
            // 
            // 
            this.nThrivePayerNoTextBox.CustomButton.Image = null;
            this.nThrivePayerNoTextBox.CustomButton.Location = new System.Drawing.Point(119, 1);
            this.nThrivePayerNoTextBox.CustomButton.Name = "";
            this.nThrivePayerNoTextBox.CustomButton.Size = new System.Drawing.Size(21, 21);
            this.nThrivePayerNoTextBox.CustomButton.Style = MetroFramework.MetroColorStyle.Blue;
            this.nThrivePayerNoTextBox.CustomButton.TabIndex = 1;
            this.nThrivePayerNoTextBox.CustomButton.Theme = MetroFramework.MetroThemeStyle.Light;
            this.nThrivePayerNoTextBox.CustomButton.UseSelectable = true;
            this.nThrivePayerNoTextBox.CustomButton.Visible = false;
            this.nThrivePayerNoTextBox.Lines = new string[0];
            this.nThrivePayerNoTextBox.Location = new System.Drawing.Point(787, 252);
            this.nThrivePayerNoTextBox.MaxLength = 32767;
            this.nThrivePayerNoTextBox.Name = "nThrivePayerNoTextBox";
            this.nThrivePayerNoTextBox.PasswordChar = '\0';
            this.nThrivePayerNoTextBox.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.nThrivePayerNoTextBox.SelectedText = "";
            this.nThrivePayerNoTextBox.SelectionLength = 0;
            this.nThrivePayerNoTextBox.SelectionStart = 0;
            this.nThrivePayerNoTextBox.ShortcutsEnabled = true;
            this.nThrivePayerNoTextBox.Size = new System.Drawing.Size(141, 23);
            this.nThrivePayerNoTextBox.TabIndex = 32;
            this.nThrivePayerNoTextBox.UseSelectable = true;
            this.nThrivePayerNoTextBox.WaterMarkColor = System.Drawing.Color.FromArgb(((int)(((byte)(109)))), ((int)(((byte)(109)))), ((int)(((byte)(109)))));
            this.nThrivePayerNoTextBox.WaterMarkFont = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Pixel);
            // 
            // insuranceTypeLabel
            // 
            this.insuranceTypeLabel.AutoSize = true;
            this.insuranceTypeLabel.Location = new System.Drawing.Point(684, 288);
            this.insuranceTypeLabel.Name = "insuranceTypeLabel";
            this.insuranceTypeLabel.Size = new System.Drawing.Size(94, 19);
            this.insuranceTypeLabel.TabIndex = 33;
            this.insuranceTypeLabel.Text = "Insurance Type";
            // 
            // insuranceTypeComboBox
            // 
            this.insuranceTypeComboBox.FormattingEnabled = true;
            this.insuranceTypeComboBox.ItemHeight = 23;
            this.insuranceTypeComboBox.Location = new System.Drawing.Point(787, 281);
            this.insuranceTypeComboBox.Name = "insuranceTypeComboBox";
            this.insuranceTypeComboBox.Size = new System.Drawing.Size(197, 29);
            this.insuranceTypeComboBox.TabIndex = 34;
            this.insuranceTypeComboBox.UseSelectable = true;
            // 
            // saveButton
            // 
            this.saveButton.Location = new System.Drawing.Point(875, 406);
            this.saveButton.Name = "saveButton";
            this.saveButton.Size = new System.Drawing.Size(109, 32);
            this.saveButton.TabIndex = 38;
            this.saveButton.Text = "Save";
            this.saveButton.UseSelectable = true;
            // 
            // HealthPlanMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1023, 453);
            this.Controls.Add(this.saveButton);
            this.Controls.Add(this.billAsJmcghCheckBox);
            this.Controls.Add(this.allowOutpatientBillingCheckBox);
            this.Controls.Add(this.isMedicareHmoCheckBox);
            this.Controls.Add(this.commentsLabel);
            this.Controls.Add(this.commentsTextBox);
            this.Controls.Add(this.insuranceTypeComboBox);
            this.Controls.Add(this.finClassComboBox);
            this.Controls.Add(this.finCodeComboBox);
            this.Controls.Add(this.insuranceTypeLabel);
            this.Controls.Add(this.nThrivePayerNoTextBox);
            this.Controls.Add(this.nthrivePayerNoLabel);
            this.Controls.Add(this.payorCodeTextBox);
            this.Controls.Add(this.payorCodeLabel);
            this.Controls.Add(this.metroTextBox3);
            this.Controls.Add(this.payerNoLabel);
            this.Controls.Add(this.providerNoTextBox);
            this.Controls.Add(this.providerNoLabel);
            this.Controls.Add(this.providerNoQualifierTextBox);
            this.Controls.Add(this.finClassLabel);
            this.Controls.Add(this.providerNoQualifierLabel);
            this.Controls.Add(this.finCodeLabel);
            this.Controls.Add(this.claimTypeLabel);
            this.Controls.Add(this.claimTypeComboBox);
            this.Controls.Add(this.planZipCodeLabel);
            this.Controls.Add(this.planStateLabel);
            this.Controls.Add(this.planCityLabel);
            this.Controls.Add(this.addressLabel);
            this.Controls.Add(this.planNameLabel);
            this.Controls.Add(this.insCodeLabel);
            this.Controls.Add(this.planZipCodeTextBox);
            this.Controls.Add(this.metroTextBox5);
            this.Controls.Add(this.planCityTextBox);
            this.Controls.Add(this.address2TextBox);
            this.Controls.Add(this.address1TextBox);
            this.Controls.Add(this.metroTextBox1);
            this.Controls.Add(this.insCodeTextBox);
            this.Controls.Add(this.includeDeletedCheckBox);
            this.Controls.Add(this.healthPlanListView);
            this.Name = "HealthPlanMaintenanceForm";
            this.Text = "Health Plan Maintenance";
            this.Load += new System.EventHandler(this.HealthPlanMaintenanceForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroListView healthPlanListView;
        private MetroFramework.Controls.MetroCheckBox includeDeletedCheckBox;
        private MetroFramework.Controls.MetroTextBox insCodeTextBox;
        private MetroFramework.Controls.MetroLabel insCodeLabel;
        private MetroFramework.Controls.MetroTextBox metroTextBox1;
        private MetroFramework.Controls.MetroLabel planNameLabel;
        private MetroFramework.Controls.MetroTextBox address1TextBox;
        private MetroFramework.Controls.MetroLabel addressLabel;
        private MetroFramework.Controls.MetroTextBox address2TextBox;
        private MetroFramework.Controls.MetroTextBox planCityTextBox;
        private MetroFramework.Controls.MetroLabel planCityLabel;
        private MetroFramework.Controls.MetroTextBox metroTextBox5;
        private MetroFramework.Controls.MetroLabel planStateLabel;
        private MetroFramework.Controls.MetroTextBox planZipCodeTextBox;
        private MetroFramework.Controls.MetroLabel planZipCodeLabel;
        private MetroFramework.Controls.MetroComboBox claimTypeComboBox;
        private MetroFramework.Controls.MetroLabel claimTypeLabel;
        private MetroFramework.Controls.MetroLabel providerNoQualifierLabel;
        private MetroFramework.Controls.MetroTextBox providerNoQualifierTextBox;
        private MetroFramework.Controls.MetroLabel providerNoLabel;
        private MetroFramework.Controls.MetroTextBox providerNoTextBox;
        private MetroFramework.Controls.MetroLabel payerNoLabel;
        private MetroFramework.Controls.MetroTextBox metroTextBox3;
        private MetroFramework.Controls.MetroComboBox finCodeComboBox;
        private MetroFramework.Controls.MetroLabel finCodeLabel;
        private MetroFramework.Controls.MetroTextBox commentsTextBox;
        private MetroFramework.Controls.MetroLabel commentsLabel;
        private MetroFramework.Controls.MetroCheckBox isMedicareHmoCheckBox;
        private MetroFramework.Controls.MetroCheckBox allowOutpatientBillingCheckBox;
        private MetroFramework.Controls.MetroLabel payorCodeLabel;
        private MetroFramework.Controls.MetroTextBox payorCodeTextBox;
        private MetroFramework.Controls.MetroLabel finClassLabel;
        private MetroFramework.Controls.MetroComboBox finClassComboBox;
        private MetroFramework.Controls.MetroCheckBox billAsJmcghCheckBox;
        private MetroFramework.Controls.MetroLabel nthrivePayerNoLabel;
        private MetroFramework.Controls.MetroTextBox nThrivePayerNoTextBox;
        private MetroFramework.Controls.MetroLabel insuranceTypeLabel;
        private MetroFramework.Controls.MetroComboBox insuranceTypeComboBox;
        private MetroFramework.Controls.MetroButton saveButton;
    }
}