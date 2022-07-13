
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
            this.AccountNo = new System.Windows.Forms.TextBox();
            this.LastName = new System.Windows.Forms.TextBox();
            this.FirstName = new System.Windows.Forms.TextBox();
            this.MiddleName = new System.Windows.Forms.TextBox();
            this.DateOfBirth = new System.Windows.Forms.MaskedTextBox();
            this.PatientSex = new System.Windows.Forms.ComboBox();
            this.label1 = new MetroFramework.Controls.MetroLabel();
            this.label2 = new MetroFramework.Controls.MetroLabel();
            this.label3 = new MetroFramework.Controls.MetroLabel();
            this.label4 = new MetroFramework.Controls.MetroLabel();
            this.label5 = new MetroFramework.Controls.MetroLabel();
            this.label6 = new MetroFramework.Controls.MetroLabel();
            this.AddAccount = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label7 = new MetroFramework.Controls.MetroLabel();
            this.ServiceDate = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new MetroFramework.Controls.MetroLabel();
            this.FinancialClass = new MultiColumnCombo.MultiColumnComboBox();
            this.label9 = new MetroFramework.Controls.MetroLabel();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountNo
            // 
            this.AccountNo.Location = new System.Drawing.Point(124, 69);
            this.AccountNo.Name = "AccountNo";
            this.AccountNo.Size = new System.Drawing.Size(121, 20);
            this.AccountNo.TabIndex = 1;
            // 
            // LastName
            // 
            this.LastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LastName.Location = new System.Drawing.Point(124, 95);
            this.LastName.Name = "LastName";
            this.LastName.Size = new System.Drawing.Size(253, 20);
            this.LastName.TabIndex = 4;
            this.LastName.Validating += new System.ComponentModel.CancelEventHandler(this.LastName_Validating);
            // 
            // FirstName
            // 
            this.FirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.FirstName.Location = new System.Drawing.Point(124, 121);
            this.FirstName.Name = "FirstName";
            this.FirstName.Size = new System.Drawing.Size(253, 20);
            this.FirstName.TabIndex = 6;
            this.FirstName.Validating += new System.ComponentModel.CancelEventHandler(this.FirstName_Validating);
            // 
            // MiddleName
            // 
            this.MiddleName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MiddleName.Location = new System.Drawing.Point(124, 147);
            this.MiddleName.Name = "MiddleName";
            this.MiddleName.Size = new System.Drawing.Size(253, 20);
            this.MiddleName.TabIndex = 8;
            // 
            // DateOfBirth
            // 
            this.DateOfBirth.Location = new System.Drawing.Point(124, 173);
            this.DateOfBirth.Mask = "00/00/0000";
            this.DateOfBirth.Name = "DateOfBirth";
            this.DateOfBirth.Size = new System.Drawing.Size(100, 20);
            this.DateOfBirth.TabIndex = 10;
            this.DateOfBirth.ValidatingType = typeof(System.DateTime);
            this.DateOfBirth.Validating += new System.ComponentModel.CancelEventHandler(this.DateOfBirth_Validating);
            // 
            // PatientSex
            // 
            this.PatientSex.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.PatientSex.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.PatientSex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.PatientSex.FormattingEnabled = true;
            this.PatientSex.Location = new System.Drawing.Point(124, 199);
            this.PatientSex.Name = "PatientSex";
            this.PatientSex.Size = new System.Drawing.Size(121, 21);
            this.PatientSex.TabIndex = 12;
            this.PatientSex.Validating += new System.ComponentModel.CancelEventHandler(this.PatientSex_Validating);
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
            this.AddAccount.Location = new System.Drawing.Point(124, 291);
            this.AddAccount.Name = "AddAccount";
            this.AddAccount.Size = new System.Drawing.Size(100, 35);
            this.AddAccount.TabIndex = 17;
            this.AddAccount.Text = "Add Account";
            this.AddAccount.UseVisualStyleBackColor = true;
            this.AddAccount.Click += new System.EventHandler(this.AddAccount_Click);
            // 
            // Cancel
            // 
            this.Cancel.CausesValidation = false;
            this.Cancel.Location = new System.Drawing.Point(231, 291);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(100, 35);
            this.Cancel.TabIndex = 18;
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
            // ServiceDate
            // 
            this.ServiceDate.Location = new System.Drawing.Point(124, 226);
            this.ServiceDate.Mask = "00/00/0000";
            this.ServiceDate.Name = "ServiceDate";
            this.ServiceDate.Size = new System.Drawing.Size(100, 20);
            this.ServiceDate.TabIndex = 14;
            this.ServiceDate.ValidatingType = typeof(System.DateTime);
            this.ServiceDate.Validating += new System.ComponentModel.CancelEventHandler(this.ServiceDate_Validating);
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
            // FinancialClass
            // 
            this.FinancialClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.FinancialClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FinancialClass.Location = new System.Drawing.Point(124, 253);
            this.FinancialClass.Name = "FinancialClass";
            this.FinancialClass.Size = new System.Drawing.Size(253, 21);
            this.FinancialClass.TabIndex = 0;
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
            // NewAccountForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(392, 364);
            this.Controls.Add(this.FinancialClass);
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
            this.Controls.Add(this.PatientSex);
            this.Controls.Add(this.ServiceDate);
            this.Controls.Add(this.DateOfBirth);
            this.Controls.Add(this.MiddleName);
            this.Controls.Add(this.FirstName);
            this.Controls.Add(this.LastName);
            this.Controls.Add(this.AccountNo);
            this.Location = new System.Drawing.Point(110, 188);
            this.Name = "NewAccountForm";
            this.Text = "New Account";
            this.Load += new System.EventHandler(this.NewAccountForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MaskedTextBox DateOfBirth;
        private MetroFramework.Controls.MetroLabel label1;
        private MetroFramework.Controls.MetroLabel label2;
        private MetroFramework.Controls.MetroLabel label3;
        private MetroFramework.Controls.MetroLabel label4;
        private MetroFramework.Controls.MetroLabel label5;
        private MetroFramework.Controls.MetroLabel label6;
        private MetroFramework.Controls.MetroLabel label7;
        private System.Windows.Forms.MaskedTextBox ServiceDate;
        private MetroFramework.Controls.MetroLabel label8;
        private MultiColumnCombo.MultiColumnComboBox FinancialClass;
        private MetroFramework.Controls.MetroLabel label9;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox AccountNo;
        private System.Windows.Forms.TextBox LastName;
        private System.Windows.Forms.TextBox FirstName;
        private System.Windows.Forms.TextBox MiddleName;
        private System.Windows.Forms.ComboBox PatientSex;
        private System.Windows.Forms.Button AddAccount;
        private System.Windows.Forms.Button Cancel;
    }
}