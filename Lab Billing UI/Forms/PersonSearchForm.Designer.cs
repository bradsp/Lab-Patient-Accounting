namespace LabBilling.Forms
{
    partial class PersonSearchForm
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
            PersonAccountResults = new System.Windows.Forms.DataGridView();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            txtLastName = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            mrnSearchText = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            ssnSearchText = new System.Windows.Forms.TextBox();
            label6 = new System.Windows.Forms.Label();
            label7 = new System.Windows.Forms.Label();
            cbSexSearch = new System.Windows.Forms.ComboBox();
            label8 = new System.Windows.Forms.Label();
            accountSearchText = new System.Windows.Forms.TextBox();
            SearchButton = new System.Windows.Forms.Button();
            ClearButton = new System.Windows.Forms.Button();
            dobSearchText = new System.Windows.Forms.MaskedTextBox();
            SelectButton = new System.Windows.Forms.Button();
            CancelBtn = new System.Windows.Forms.Button();
            txtFirstName = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            AddAccount = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)PersonAccountResults).BeginInit();
            SuspendLayout();
            // 
            // PersonAccountResults
            // 
            PersonAccountResults.AllowUserToAddRows = false;
            PersonAccountResults.AllowUserToDeleteRows = false;
            PersonAccountResults.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            PersonAccountResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PersonAccountResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            PersonAccountResults.Location = new System.Drawing.Point(227, 39);
            PersonAccountResults.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            PersonAccountResults.MultiSelect = false;
            PersonAccountResults.Name = "PersonAccountResults";
            PersonAccountResults.ReadOnly = true;
            PersonAccountResults.RowHeadersVisible = false;
            PersonAccountResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            PersonAccountResults.Size = new System.Drawing.Size(1007, 483);
            PersonAccountResults.TabIndex = 16;
            PersonAccountResults.DoubleClick += PersonAccountResults_DoubleClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(227, 14);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(57, 15);
            label2.TabIndex = 14;
            label2.Text = "Accounts";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 74);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(63, 15);
            label3.TabIndex = 2;
            label3.Text = "Last Name";
            // 
            // txtLastName
            // 
            txtLastName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            txtLastName.Location = new System.Drawing.Point(14, 102);
            txtLastName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new System.Drawing.Size(205, 23);
            txtLastName.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(14, 184);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(34, 15);
            label4.TabIndex = 6;
            label4.Text = "MRN";
            // 
            // mrnSearchText
            // 
            mrnSearchText.Location = new System.Drawing.Point(14, 208);
            mrnSearchText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            mrnSearchText.Name = "mrnSearchText";
            mrnSearchText.Size = new System.Drawing.Size(206, 23);
            mrnSearchText.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(14, 234);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(28, 15);
            label5.TabIndex = 8;
            label5.Text = "SSN";
            // 
            // ssnSearchText
            // 
            ssnSearchText.Location = new System.Drawing.Point(13, 260);
            ssnSearchText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ssnSearchText.Name = "ssnSearchText";
            ssnSearchText.Size = new System.Drawing.Size(206, 23);
            ssnSearchText.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(14, 283);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(73, 15);
            label6.TabIndex = 10;
            label6.Text = "Date of Birth";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(14, 333);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(25, 15);
            label7.TabIndex = 12;
            label7.Text = "Sex";
            // 
            // cbSexSearch
            // 
            cbSexSearch.FormattingEnabled = true;
            cbSexSearch.Location = new System.Drawing.Point(14, 358);
            cbSexSearch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cbSexSearch.Name = "cbSexSearch";
            cbSexSearch.Size = new System.Drawing.Size(206, 23);
            cbSexSearch.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(14, 23);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(52, 15);
            label8.TabIndex = 0;
            label8.Text = "Account";
            // 
            // accountSearchText
            // 
            accountSearchText.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            accountSearchText.Location = new System.Drawing.Point(14, 49);
            accountSearchText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accountSearchText.Name = "accountSearchText";
            accountSearchText.Size = new System.Drawing.Size(206, 23);
            accountSearchText.TabIndex = 1;
            // 
            // SearchButton
            // 
            SearchButton.Location = new System.Drawing.Point(13, 426);
            SearchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new System.Drawing.Size(206, 42);
            SearchButton.TabIndex = 14;
            SearchButton.Text = "Search";
            SearchButton.UseVisualStyleBackColor = true;
            SearchButton.Click += SearchButton_Click;
            // 
            // ClearButton
            // 
            ClearButton.Location = new System.Drawing.Point(14, 480);
            ClearButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new System.Drawing.Size(206, 42);
            ClearButton.TabIndex = 15;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // dobSearchText
            // 
            dobSearchText.Location = new System.Drawing.Point(14, 306);
            dobSearchText.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dobSearchText.Mask = "00/00/0000";
            dobSearchText.Name = "dobSearchText";
            dobSearchText.Size = new System.Drawing.Size(206, 23);
            dobSearchText.TabIndex = 11;
            dobSearchText.ValidatingType = typeof(System.DateTime);
            // 
            // SelectButton
            // 
            SelectButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            SelectButton.Location = new System.Drawing.Point(989, 529);
            SelectButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SelectButton.Name = "SelectButton";
            SelectButton.Size = new System.Drawing.Size(110, 39);
            SelectButton.TabIndex = 17;
            SelectButton.Text = "Select";
            SelectButton.UseVisualStyleBackColor = true;
            SelectButton.Click += SelectButton_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            CancelBtn.Location = new System.Drawing.Point(1123, 529);
            CancelBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new System.Drawing.Size(110, 39);
            CancelBtn.TabIndex = 18;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            // 
            // txtFirstName
            // 
            txtFirstName.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            txtFirstName.Location = new System.Drawing.Point(14, 157);
            txtFirstName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new System.Drawing.Size(205, 23);
            txtFirstName.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(14, 128);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(64, 15);
            label1.TabIndex = 4;
            label1.Text = "First Name";
            // 
            // AddAccount
            // 
            AddAccount.Location = new System.Drawing.Point(855, 529);
            AddAccount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AddAccount.Name = "AddAccount";
            AddAccount.Size = new System.Drawing.Size(110, 39);
            AddAccount.TabIndex = 19;
            AddAccount.Text = "New Account";
            AddAccount.UseVisualStyleBackColor = true;
            AddAccount.Click += AddAccount_Click;
            // 
            // PersonSearchForm
            // 
            AcceptButton = SearchButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1247, 583);
            Controls.Add(AddAccount);
            Controls.Add(txtFirstName);
            Controls.Add(label1);
            Controls.Add(CancelBtn);
            Controls.Add(SelectButton);
            Controls.Add(dobSearchText);
            Controls.Add(ClearButton);
            Controls.Add(SearchButton);
            Controls.Add(accountSearchText);
            Controls.Add(label8);
            Controls.Add(cbSexSearch);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(ssnSearchText);
            Controls.Add(label5);
            Controls.Add(mrnSearchText);
            Controls.Add(label4);
            Controls.Add(txtLastName);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(PersonAccountResults);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "PersonSearchForm";
            Text = "Person Search";
            Load += PersonSearchForm_Load;
            ((System.ComponentModel.ISupportInitialize)PersonAccountResults).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.MaskedTextBox dobSearchText;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView PersonAccountResults;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox mrnSearchText;
        private System.Windows.Forms.TextBox ssnSearchText;
        private System.Windows.Forms.ComboBox cbSexSearch;
        private System.Windows.Forms.TextBox accountSearchText;
        private System.Windows.Forms.Button SearchButton;
        private System.Windows.Forms.Button ClearButton;
        private System.Windows.Forms.Button SelectButton;
        private System.Windows.Forms.Button CancelBtn;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Button AddAccount;
    }
}