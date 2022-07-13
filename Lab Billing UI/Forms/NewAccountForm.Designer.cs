
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.AddAccount = new System.Windows.Forms.Button();
            this.Cancel = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.ServiceDate = new System.Windows.Forms.MaskedTextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.FinancialClass = new MultiColumnCombo.MultiColumnComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.errorProvider1 = new System.Windows.Forms.ErrorProvider(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider1)).BeginInit();
            this.SuspendLayout();
            // 
            // AccountNo
            // 
            this.AccountNo.Location = new System.Drawing.Point(110, 28);
            this.AccountNo.Name = "AccountNo";
            this.AccountNo.Size = new System.Drawing.Size(121, 20);
            this.AccountNo.TabIndex = 1;
            // 
            // LastName
            // 
            this.LastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.LastName.Location = new System.Drawing.Point(110, 54);
            this.LastName.Name = "LastName";
            this.LastName.Size = new System.Drawing.Size(253, 20);
            this.LastName.TabIndex = 4;
            this.LastName.Validating += new System.ComponentModel.CancelEventHandler(this.LastName_Validating);
            // 
            // FirstName
            // 
            this.FirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.FirstName.Location = new System.Drawing.Point(110, 80);
            this.FirstName.Name = "FirstName";
            this.FirstName.Size = new System.Drawing.Size(253, 20);
            this.FirstName.TabIndex = 6;
            this.FirstName.Validating += new System.ComponentModel.CancelEventHandler(this.FirstName_Validating);
            // 
            // MiddleName
            // 
            this.MiddleName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MiddleName.Location = new System.Drawing.Point(110, 106);
            this.MiddleName.Name = "MiddleName";
            this.MiddleName.Size = new System.Drawing.Size(253, 20);
            this.MiddleName.TabIndex = 8;
            // 
            // DateOfBirth
            // 
            this.DateOfBirth.Location = new System.Drawing.Point(110, 132);
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
            this.PatientSex.Location = new System.Drawing.Point(110, 158);
            this.PatientSex.Name = "PatientSex";
            this.PatientSex.Size = new System.Drawing.Size(121, 21);
            this.PatientSex.TabIndex = 12;
            this.PatientSex.Validating += new System.ComponentModel.CancelEventHandler(this.PatientSex_Validating);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account #";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 57);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(58, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Last Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 83);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "First Name";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Middle Name";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 135);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Date of Birth";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 161);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(25, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Sex";
            // 
            // AddAccount
            // 
            this.AddAccount.Location = new System.Drawing.Point(110, 280);
            this.AddAccount.Name = "AddAccount";
            this.AddAccount.Size = new System.Drawing.Size(75, 23);
            this.AddAccount.TabIndex = 17;
            this.AddAccount.Text = "Add Account";
            this.AddAccount.UseVisualStyleBackColor = true;
            this.AddAccount.Click += new System.EventHandler(this.AddAccount_Click);
            // 
            // Cancel
            // 
            this.Cancel.CausesValidation = false;
            this.Cancel.Location = new System.Drawing.Point(191, 280);
            this.Cancel.Name = "Cancel";
            this.Cancel.Size = new System.Drawing.Size(75, 23);
            this.Cancel.TabIndex = 18;
            this.Cancel.Text = "Cancel";
            this.Cancel.UseVisualStyleBackColor = true;
            this.Cancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(237, 31);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(107, 13);
            this.label7.TabIndex = 2;
            this.label7.Text = "(blank to assign new)";
            // 
            // ServiceDate
            // 
            this.ServiceDate.Location = new System.Drawing.Point(110, 185);
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
            this.label8.Location = new System.Drawing.Point(12, 188);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(69, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "Service Date";
            // 
            // FinancialClass
            // 
            this.FinancialClass.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.FinancialClass.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FinancialClass.Location = new System.Drawing.Point(110, 212);
            this.FinancialClass.Name = "FinancialClass";
            this.FinancialClass.Size = new System.Drawing.Size(253, 21);
            this.FinancialClass.TabIndex = 0;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 212);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 13);
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
            this.ClientSize = new System.Drawing.Size(378, 344);
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

        private System.Windows.Forms.TextBox AccountNo;
        private System.Windows.Forms.TextBox LastName;
        private System.Windows.Forms.TextBox FirstName;
        private System.Windows.Forms.TextBox MiddleName;
        private System.Windows.Forms.MaskedTextBox DateOfBirth;
        private System.Windows.Forms.ComboBox PatientSex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button AddAccount;
        private System.Windows.Forms.Button Cancel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.MaskedTextBox ServiceDate;
        private System.Windows.Forms.Label label8;
        private MultiColumnCombo.MultiColumnComboBox FinancialClass;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ErrorProvider errorProvider1;
    }
}