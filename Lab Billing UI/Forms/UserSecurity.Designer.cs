namespace LabBilling
{
    partial class UserSecurity
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.UserListDGV = new System.Windows.Forms.DataGridView();
            this.UserNameLabel = new System.Windows.Forms.Label();
            this.UserName = new System.Windows.Forms.TextBox();
            this.Password = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.AccessLevelLabel = new System.Windows.Forms.Label();
            this.AccessLevelCombo = new System.Windows.Forms.ComboBox();
            this.CanEditDictionaries = new System.Windows.Forms.CheckBox();
            this.CanEditBadDebt = new System.Windows.Forms.CheckBox();
            this.CanSubmitBilling = new System.Windows.Forms.CheckBox();
            this.CanChangeAccountFinCode = new System.Windows.Forms.CheckBox();
            this.CanAddCharges = new System.Windows.Forms.CheckBox();
            this.CanAddAccountAdjustments = new System.Windows.Forms.CheckBox();
            this.IsAdministrator = new System.Windows.Forms.CheckBox();
            this.Reserved5 = new System.Windows.Forms.CheckBox();
            this.CanAddPayments = new System.Windows.Forms.CheckBox();
            this.Reserved6 = new System.Windows.Forms.CheckBox();
            this.FullName = new System.Windows.Forms.TextBox();
            this.FullNameLabel = new System.Windows.Forms.Label();
            this.ModUser = new System.Windows.Forms.TextBox();
            this.ModProgram = new System.Windows.Forms.TextBox();
            this.ModDateTime = new System.Windows.Forms.TextBox();
            this.LastUpdatedGroup = new System.Windows.Forms.GroupBox();
            this.AddUserButton = new System.Windows.Forms.Button();
            this.SaveButton = new System.Windows.Forms.Button();
            this.ShowInactive = new System.Windows.Forms.CheckBox();
            this.ResetPassword = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.UserListDGV)).BeginInit();
            this.LastUpdatedGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // UserListDGV
            // 
            this.UserListDGV.AllowUserToAddRows = false;
            this.UserListDGV.AllowUserToDeleteRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.UserListDGV.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.UserListDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserListDGV.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.UserListDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.UserListDGV.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UserListDGV.Location = new System.Drawing.Point(378, 33);
            this.UserListDGV.MultiSelect = false;
            this.UserListDGV.Name = "UserListDGV";
            this.UserListDGV.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.UserListDGV.ShowEditingIcon = false;
            this.UserListDGV.Size = new System.Drawing.Size(430, 428);
            this.UserListDGV.TabIndex = 21;
            this.UserListDGV.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.UserListDGV_CellMouseClick);
            // 
            // UserNameLabel
            // 
            this.UserNameLabel.AutoSize = true;
            this.UserNameLabel.Location = new System.Drawing.Point(31, 59);
            this.UserNameLabel.Name = "UserNameLabel";
            this.UserNameLabel.Size = new System.Drawing.Size(55, 13);
            this.UserNameLabel.TabIndex = 0;
            this.UserNameLabel.Text = "Username";
            // 
            // UserName
            // 
            this.UserName.Location = new System.Drawing.Point(92, 57);
            this.UserName.Name = "UserName";
            this.UserName.ReadOnly = true;
            this.UserName.Size = new System.Drawing.Size(253, 20);
            this.UserName.TabIndex = 1;
            // 
            // Password
            // 
            this.Password.Location = new System.Drawing.Point(92, 115);
            this.Password.Name = "Password";
            this.Password.ReadOnly = true;
            this.Password.Size = new System.Drawing.Size(188, 20);
            this.Password.TabIndex = 5;
            this.Password.UseSystemPasswordChar = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 118);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Password";
            // 
            // AccessLevelLabel
            // 
            this.AccessLevelLabel.AutoSize = true;
            this.AccessLevelLabel.Location = new System.Drawing.Point(15, 144);
            this.AccessLevelLabel.Name = "AccessLevelLabel";
            this.AccessLevelLabel.Size = new System.Drawing.Size(71, 13);
            this.AccessLevelLabel.TabIndex = 6;
            this.AccessLevelLabel.Text = "Access Level";
            // 
            // AccessLevelCombo
            // 
            this.AccessLevelCombo.FormattingEnabled = true;
            this.AccessLevelCombo.Items.AddRange(new object[] {
            "NONE",
            "VIEW",
            "ENTER/EDIT"});
            this.AccessLevelCombo.Location = new System.Drawing.Point(92, 141);
            this.AccessLevelCombo.Name = "AccessLevelCombo";
            this.AccessLevelCombo.Size = new System.Drawing.Size(253, 21);
            this.AccessLevelCombo.TabIndex = 7;
            // 
            // CanEditDictionaries
            // 
            this.CanEditDictionaries.AutoSize = true;
            this.CanEditDictionaries.Location = new System.Drawing.Point(15, 180);
            this.CanEditDictionaries.Name = "CanEditDictionaries";
            this.CanEditDictionaries.Size = new System.Drawing.Size(124, 17);
            this.CanEditDictionaries.TabIndex = 8;
            this.CanEditDictionaries.Text = "Can Edit Dictionaries";
            this.CanEditDictionaries.UseVisualStyleBackColor = true;
            // 
            // CanEditBadDebt
            // 
            this.CanEditBadDebt.AutoSize = true;
            this.CanEditBadDebt.Location = new System.Drawing.Point(15, 202);
            this.CanEditBadDebt.Name = "CanEditBadDebt";
            this.CanEditBadDebt.Size = new System.Drawing.Size(114, 17);
            this.CanEditBadDebt.TabIndex = 9;
            this.CanEditBadDebt.Text = "Can Edit Bad Debt";
            this.CanEditBadDebt.UseVisualStyleBackColor = true;
            // 
            // CanSubmitBilling
            // 
            this.CanSubmitBilling.AutoSize = true;
            this.CanSubmitBilling.Location = new System.Drawing.Point(15, 224);
            this.CanSubmitBilling.Name = "CanSubmitBilling";
            this.CanSubmitBilling.Size = new System.Drawing.Size(110, 17);
            this.CanSubmitBilling.TabIndex = 10;
            this.CanSubmitBilling.Text = "Can Submit Billing";
            this.CanSubmitBilling.UseVisualStyleBackColor = true;
            // 
            // CanChangeAccountFinCode
            // 
            this.CanChangeAccountFinCode.AutoSize = true;
            this.CanChangeAccountFinCode.Location = new System.Drawing.Point(15, 247);
            this.CanChangeAccountFinCode.Name = "CanChangeAccountFinCode";
            this.CanChangeAccountFinCode.Size = new System.Drawing.Size(170, 17);
            this.CanChangeAccountFinCode.TabIndex = 11;
            this.CanChangeAccountFinCode.Text = "Can Change Account FinCode";
            this.CanChangeAccountFinCode.UseVisualStyleBackColor = true;
            // 
            // CanAddCharges
            // 
            this.CanAddCharges.AutoSize = true;
            this.CanAddCharges.Location = new System.Drawing.Point(15, 270);
            this.CanAddCharges.Name = "CanAddCharges";
            this.CanAddCharges.Size = new System.Drawing.Size(109, 17);
            this.CanAddCharges.TabIndex = 12;
            this.CanAddCharges.Text = "Can Add Charges";
            this.CanAddCharges.UseVisualStyleBackColor = true;
            // 
            // CanAddAccountAdjustments
            // 
            this.CanAddAccountAdjustments.AutoSize = true;
            this.CanAddAccountAdjustments.Location = new System.Drawing.Point(15, 292);
            this.CanAddAccountAdjustments.Name = "CanAddAccountAdjustments";
            this.CanAddAccountAdjustments.Size = new System.Drawing.Size(170, 17);
            this.CanAddAccountAdjustments.TabIndex = 13;
            this.CanAddAccountAdjustments.Text = "Can Add Account Adjustments";
            this.CanAddAccountAdjustments.UseVisualStyleBackColor = true;
            // 
            // IsAdministrator
            // 
            this.IsAdministrator.AutoSize = true;
            this.IsAdministrator.Location = new System.Drawing.Point(15, 338);
            this.IsAdministrator.Name = "IsAdministrator";
            this.IsAdministrator.Size = new System.Drawing.Size(97, 17);
            this.IsAdministrator.TabIndex = 15;
            this.IsAdministrator.Text = "Is Administrator";
            this.IsAdministrator.UseVisualStyleBackColor = true;
            // 
            // Reserved5
            // 
            this.Reserved5.AutoSize = true;
            this.Reserved5.Location = new System.Drawing.Point(203, 338);
            this.Reserved5.Name = "Reserved5";
            this.Reserved5.Size = new System.Drawing.Size(142, 17);
            this.Reserved5.TabIndex = 17;
            this.Reserved5.Text = "Reserved for Future Use";
            this.Reserved5.UseVisualStyleBackColor = true;
            this.Reserved5.Visible = false;
            // 
            // CanAddPayments
            // 
            this.CanAddPayments.AutoSize = true;
            this.CanAddPayments.Location = new System.Drawing.Point(15, 315);
            this.CanAddPayments.Name = "CanAddPayments";
            this.CanAddPayments.Size = new System.Drawing.Size(116, 17);
            this.CanAddPayments.TabIndex = 14;
            this.CanAddPayments.Text = "Can Add Payments";
            this.CanAddPayments.UseVisualStyleBackColor = true;
            // 
            // Reserved6
            // 
            this.Reserved6.AutoSize = true;
            this.Reserved6.Location = new System.Drawing.Point(203, 315);
            this.Reserved6.Name = "Reserved6";
            this.Reserved6.Size = new System.Drawing.Size(142, 17);
            this.Reserved6.TabIndex = 16;
            this.Reserved6.Text = "Reserved for Future Use";
            this.Reserved6.UseVisualStyleBackColor = true;
            this.Reserved6.Visible = false;
            // 
            // FullName
            // 
            this.FullName.Location = new System.Drawing.Point(92, 88);
            this.FullName.Name = "FullName";
            this.FullName.Size = new System.Drawing.Size(253, 20);
            this.FullName.TabIndex = 3;
            // 
            // FullNameLabel
            // 
            this.FullNameLabel.AutoSize = true;
            this.FullNameLabel.Location = new System.Drawing.Point(32, 90);
            this.FullNameLabel.Name = "FullNameLabel";
            this.FullNameLabel.Size = new System.Drawing.Size(54, 13);
            this.FullNameLabel.TabIndex = 2;
            this.FullNameLabel.Text = "Full Name";
            // 
            // ModUser
            // 
            this.ModUser.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModUser.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModUser.Location = new System.Drawing.Point(22, 32);
            this.ModUser.Name = "ModUser";
            this.ModUser.ReadOnly = true;
            this.ModUser.Size = new System.Drawing.Size(265, 13);
            this.ModUser.TabIndex = 0;
            // 
            // ModProgram
            // 
            this.ModProgram.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModProgram.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModProgram.Location = new System.Drawing.Point(22, 50);
            this.ModProgram.Name = "ModProgram";
            this.ModProgram.ReadOnly = true;
            this.ModProgram.Size = new System.Drawing.Size(265, 13);
            this.ModProgram.TabIndex = 1;
            // 
            // ModDateTime
            // 
            this.ModDateTime.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.ModDateTime.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ModDateTime.Location = new System.Drawing.Point(22, 67);
            this.ModDateTime.Name = "ModDateTime";
            this.ModDateTime.ReadOnly = true;
            this.ModDateTime.Size = new System.Drawing.Size(265, 13);
            this.ModDateTime.TabIndex = 2;
            // 
            // LastUpdatedGroup
            // 
            this.LastUpdatedGroup.Controls.Add(this.ModDateTime);
            this.LastUpdatedGroup.Controls.Add(this.ModProgram);
            this.LastUpdatedGroup.Controls.Add(this.ModUser);
            this.LastUpdatedGroup.Location = new System.Drawing.Point(15, 369);
            this.LastUpdatedGroup.Name = "LastUpdatedGroup";
            this.LastUpdatedGroup.Size = new System.Drawing.Size(307, 94);
            this.LastUpdatedGroup.TabIndex = 18;
            this.LastUpdatedGroup.TabStop = false;
            this.LastUpdatedGroup.Text = "Last Updated";
            // 
            // AddUserButton
            // 
            this.AddUserButton.BackColor = System.Drawing.Color.SpringGreen;
            this.AddUserButton.FlatAppearance.BorderSize = 0;
            this.AddUserButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddUserButton.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AddUserButton.Location = new System.Drawing.Point(92, 7);
            this.AddUserButton.Name = "AddUserButton";
            this.AddUserButton.Size = new System.Drawing.Size(99, 32);
            this.AddUserButton.TabIndex = 19;
            this.AddUserButton.Text = "New User";
            this.AddUserButton.UseVisualStyleBackColor = false;
            this.AddUserButton.Click += new System.EventHandler(this.AddUserButton_Click);
            // 
            // SaveButton
            // 
            this.SaveButton.BackColor = System.Drawing.Color.RoyalBlue;
            this.SaveButton.FlatAppearance.BorderSize = 0;
            this.SaveButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.SaveButton.Font = new System.Drawing.Font("Century Gothic", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SaveButton.ForeColor = System.Drawing.Color.White;
            this.SaveButton.Location = new System.Drawing.Point(223, 7);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(99, 32);
            this.SaveButton.TabIndex = 20;
            this.SaveButton.Text = "Save";
            this.SaveButton.UseVisualStyleBackColor = false;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // ShowInactive
            // 
            this.ShowInactive.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ShowInactive.AutoSize = true;
            this.ShowInactive.Location = new System.Drawing.Point(683, 7);
            this.ShowInactive.Name = "ShowInactive";
            this.ShowInactive.Size = new System.Drawing.Size(124, 17);
            this.ShowInactive.TabIndex = 22;
            this.ShowInactive.Text = "Show Inactive Users";
            this.ShowInactive.UseVisualStyleBackColor = true;
            this.ShowInactive.CheckedChanged += new System.EventHandler(this.ShowInactive_CheckedChanged);
            // 
            // ResetPassword
            // 
            this.ResetPassword.Location = new System.Drawing.Point(286, 114);
            this.ResetPassword.Name = "ResetPassword";
            this.ResetPassword.Size = new System.Drawing.Size(59, 21);
            this.ResetPassword.TabIndex = 23;
            this.ResetPassword.Text = "Reset";
            this.ResetPassword.UseVisualStyleBackColor = true;
            this.ResetPassword.Click += new System.EventHandler(this.ResetPassword_Click);
            // 
            // UserSecurity
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(819, 477);
            this.Controls.Add(this.ResetPassword);
            this.Controls.Add(this.ShowInactive);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.AddUserButton);
            this.Controls.Add(this.FullName);
            this.Controls.Add(this.FullNameLabel);
            this.Controls.Add(this.Reserved6);
            this.Controls.Add(this.CanAddPayments);
            this.Controls.Add(this.Reserved5);
            this.Controls.Add(this.IsAdministrator);
            this.Controls.Add(this.CanAddAccountAdjustments);
            this.Controls.Add(this.CanAddCharges);
            this.Controls.Add(this.CanChangeAccountFinCode);
            this.Controls.Add(this.CanSubmitBilling);
            this.Controls.Add(this.CanEditBadDebt);
            this.Controls.Add(this.CanEditDictionaries);
            this.Controls.Add(this.AccessLevelCombo);
            this.Controls.Add(this.AccessLevelLabel);
            this.Controls.Add(this.Password);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.UserName);
            this.Controls.Add(this.UserNameLabel);
            this.Controls.Add(this.UserListDGV);
            this.Controls.Add(this.LastUpdatedGroup);
            this.Name = "UserSecurity";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Security";
            this.Load += new System.EventHandler(this.UserSecurity_Load);
            ((System.ComponentModel.ISupportInitialize)(this.UserListDGV)).EndInit();
            this.LastUpdatedGroup.ResumeLayout(false);
            this.LastUpdatedGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView UserListDGV;
        private System.Windows.Forms.Label UserNameLabel;
        private System.Windows.Forms.TextBox UserName;
        private System.Windows.Forms.TextBox Password;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label AccessLevelLabel;
        private System.Windows.Forms.ComboBox AccessLevelCombo;
        private System.Windows.Forms.CheckBox CanEditDictionaries;
        private System.Windows.Forms.CheckBox CanEditBadDebt;
        private System.Windows.Forms.CheckBox CanSubmitBilling;
        private System.Windows.Forms.CheckBox CanChangeAccountFinCode;
        private System.Windows.Forms.CheckBox CanAddCharges;
        private System.Windows.Forms.CheckBox CanAddAccountAdjustments;
        private System.Windows.Forms.CheckBox IsAdministrator;
        private System.Windows.Forms.CheckBox Reserved5;
        private System.Windows.Forms.CheckBox CanAddPayments;
        private System.Windows.Forms.CheckBox Reserved6;
        private System.Windows.Forms.TextBox FullName;
        private System.Windows.Forms.Label FullNameLabel;
        private System.Windows.Forms.TextBox ModUser;
        private System.Windows.Forms.TextBox ModProgram;
        private System.Windows.Forms.TextBox ModDateTime;
        private System.Windows.Forms.GroupBox LastUpdatedGroup;
        private System.Windows.Forms.Button AddUserButton;
        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.CheckBox ShowInactive;
        private System.Windows.Forms.Button ResetPassword;
    }
}