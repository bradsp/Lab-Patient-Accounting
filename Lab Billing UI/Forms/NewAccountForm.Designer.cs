
namespace LabBilling.Forms
{
    partial class NewAccountForm
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
            this.accountNoTextBox = new System.Windows.Forms.TextBox();
            this.lastNameTextBox = new System.Windows.Forms.TextBox();
            this.firstNameTextBox = new System.Windows.Forms.TextBox();
            this.middleNameTextBox = new System.Windows.Forms.TextBox();
            this.patientSexComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.label3 = new MetroFramework.Controls.MetroLabel();
            this.label4 = new MetroFramework.Controls.MetroLabel();
            this.label5 = new MetroFramework.Controls.MetroLabel();
            this.label6 = new MetroFramework.Controls.MetroLabel();
            this.AddAccount = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label7 = new MetroFramework.Controls.MetroLabel();
            this.label8 = new MetroFramework.Controls.MetroLabel();
            this.financialClassComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new MetroFramework.Controls.MetroLabel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            this.clientLabel = new MetroFramework.Controls.MetroLabel();
            this.clientTextBox = new System.Windows.Forms.TextBox();
            this.serviceDateTextBox = new LabBilling.UserControls.DateTextBox();
            this.dateOfBirthTextBox = new LabBilling.UserControls.DateTextBox();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // accountNoTextBox
            // 
            this.accountNoTextBox.Location = new System.Drawing.Point(124, 69);
            this.accountNoTextBox.Name = "accountNoTextBox";
            this.accountNoTextBox.ReadOnly = true;
            this.accountNoTextBox.Size = new System.Drawing.Size(121, 20);
            this.accountNoTextBox.TabIndex = 1;
            this.accountNoTextBox.TabStop = false;
            // 
            // lastNameTextBox
            // 
            this.lastNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.lastNameTextBox.Location = new System.Drawing.Point(124, 95);
            this.lastNameTextBox.Name = "lastNameTextBox";
            this.lastNameTextBox.Size = new System.Drawing.Size(253, 20);
            this.lastNameTextBox.TabIndex = 4;
            this.lastNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.LastName_Validating);
            // 
            // firstNameTextBox
            // 
            this.firstNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.firstNameTextBox.Location = new System.Drawing.Point(124, 121);
            this.firstNameTextBox.Name = "firstNameTextBox";
            this.firstNameTextBox.Size = new System.Drawing.Size(253, 20);
            this.firstNameTextBox.TabIndex = 6;
            this.firstNameTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.FirstName_Validating);
            // 
            // middleNameTextBox
            // 
            this.middleNameTextBox.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.middleNameTextBox.Location = new System.Drawing.Point(124, 147);
            this.middleNameTextBox.Name = "middleNameTextBox";
            this.middleNameTextBox.Size = new System.Drawing.Size(253, 20);
            this.middleNameTextBox.TabIndex = 8;
            // 
            // patientSexComboBox
            // 
            this.patientSexComboBox.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.patientSexComboBox.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.patientSexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.patientSexComboBox.FormattingEnabled = true;
            this.patientSexComboBox.Location = new System.Drawing.Point(124, 199);
            this.patientSexComboBox.Name = "patientSexComboBox";
            this.patientSexComboBox.Size = new System.Drawing.Size(121, 21);
            this.patientSexComboBox.TabIndex = 12;
            this.patientSexComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.PatientSex_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 72);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(68, 19);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account #";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(26, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 19);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(26, 124);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(73, 19);
            this.label3.TabIndex = 5;
            this.label3.Text = "First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 19);
            this.label4.TabIndex = 7;
            this.label4.Text = "Middle Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(26, 176);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(84, 19);
            this.label5.TabIndex = 9;
            this.label5.Text = "Date of Birth";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(26, 202);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 19);
            this.label6.TabIndex = 11;
            this.label6.Text = "Sex";
            // 
            // AddAccount
            // 
            this.AddAccount.Location = new System.Drawing.Point(125, 334);
            this.AddAccount.Name = "AddAccount";
            this.AddAccount.Size = new System.Drawing.Size(100, 35);
            this.AddAccount.TabIndex = 19;
            this.AddAccount.Text = "Add Account";
            this.AddAccount.UseVisualStyleBackColor = true;
            this.AddAccount.Click += new System.EventHandler(this.AddAccount_Click);
            // 
            // Cancel
            // 
            this.Cancel.CausesValidation = false;
            this.Cancel.Location = new System.Drawing.Point(232, 334);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 35);
            this.Cancel.TabIndex = 20;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(251, 72);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(130, 19);
            this.label7.TabIndex = 2;
            this.label7.Text = "(blank to assign new)";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(26, 229);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(82, 19);
            this.label8.TabIndex = 13;
            this.label8.Text = "Service Date";
            // 
            // financialClassComboBox
            // 
            this.financialClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.financialClassComboBox.Location = new System.Drawing.Point(124, 253);
            this.financialClassComboBox.Name = "financialClassComboBox";
            this.financialClassComboBox.Size = new System.Drawing.Size(253, 21);
            this.financialClassComboBox.TabIndex = 16;
            this.financialClassComboBox.Validating += new System.ComponentModel.CancelEventHandler(this.FinancialClass_Validating);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(26, 253);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(92, 19);
            this.label9.TabIndex = 15;
            this.label9.Text = "Financial Class";
            // 
            // errorProvider1
            // 
            this.errorProvider1.ContainerControl = this;
            // 
            // clientLabel
            // 
            this.clientLabel.AutoSize = true;
            this.clientLabel.Location = new System.Drawing.Point(26, 283);
            this.clientLabel.Name = "clientLabel";
            this.clientLabel.Size = new System.Drawing.Size(42, 19);
            this.clientLabel.TabIndex = 17;
            this.clientLabel.Text = "Client";
            // 
            // clientTextBox
            // 
            this.clientTextBox.Location = new System.Drawing.Point(125, 283);
            this.clientTextBox.Name = "clientTextBox";
            this.clientTextBox.Size = new System.Drawing.Size(252, 20);
            this.clientTextBox.TabIndex = 18;
            this.clientTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.clientTextBox_KeyUp);
            this.clientTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.clientTextBox_Validating);
            // 
            // serviceDateTextBox
            // 
            this.serviceDateTextBox.DateValue = new System.DateTime(((long)(0)));
            this.serviceDateTextBox.Location = new System.Drawing.Point(124, 226);
            this.serviceDateTextBox.Name = "serviceDateTextBox";
            this.serviceDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.serviceDateTextBox.TabIndex = 14;
            this.serviceDateTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.ServiceDate_Validating);
            // 
            // dateOfBirthTextBox
            // 
            this.dateOfBirthTextBox.DateValue = new System.DateTime(((long)(0)));
            this.dateOfBirthTextBox.Location = new System.Drawing.Point(124, 173);
            this.dateOfBirthTextBox.Name = "dateOfBirthTextBox";
            this.dateOfBirthTextBox.Size = new System.Drawing.Size(100, 20);
            this.dateOfBirthTextBox.TabIndex = 10;
            this.dateOfBirthTextBox.Validating += new System.ComponentModel.CancelEventHandler(this.DateOfBirth_Validating);
            // 
            // NewAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(435, 411);
            this.Controls.Add(this.serviceDateTextBox);
            this.Controls.Add(this.dateOfBirthTextBox);
            this.Controls.Add(this.clientTextBox);
            this.Controls.Add(this.clientLabel);
            this.Controls.Add(this.financialClassComboBox);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.Cancel);
            this.Controls.Add(this.AddAccount);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.patientSexComboBox);
            this.Controls.Add(this.middleNameTextBox);
            this.Controls.Add(this.firstNameTextBox);
            this.Controls.Add(this.lastNameTextBox);
            this.Controls.Add(this.accountNoTextBox);
            this.Location = new System.Drawing.Point(110, 188);
            this.Name = "NewAccountForm";
            this.Text = "New Account";
            this.Load += new System.EventHandler(this.NewAccountForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private MetroFramework.Controls.MetroLabel label1;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroLabel label3;
        private MetroFramework.Controls.MetroLabel label4;
        private MetroFramework.Controls.MetroLabel label5;
        private MetroFramework.Controls.MetroLabel label6;
        private MetroFramework.Controls.MetroLabel label7;
        private MetroFramework.Controls.MetroLabel label8;
        private System.Windows.Forms.ComboBox financialClassComboBox;
        private MetroFramework.Controls.MetroLabel label9;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox accountNoTextBox;
        private System.Windows.Forms.TextBox lastNameTextBox;
        private System.Windows.Forms.TextBox firstNameTextBox;
        private System.Windows.Forms.TextBox middleNameTextBox;
        private System.Windows.Forms.ComboBox patientSexComboBox;
        private System.Windows.Forms.Button AddAccount;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.TextBox clientTextBox;
        private MetroFramework.Controls.MetroLabel clientLabel;
        private UserControls.DateTextBox dateOfBirthTextBox;
        private UserControls.DateTextBox serviceDateTextBox;
    }
}