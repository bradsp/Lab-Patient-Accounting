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
            PersonAccountResults = new DataGridView();
            label2 = new Label();
            label3 = new Label();
            txtLastName = new TextBox();
            label4 = new Label();
            mrnSearchText = new TextBox();
            label5 = new Label();
            ssnSearchText = new TextBox();
            label6 = new Label();
            label7 = new Label();
            cbSexSearch = new ComboBox();
            label8 = new Label();
            accountSearchText = new TextBox();
            SearchButton = new Button();
            ClearButton = new Button();
            dobSearchText = new MaskedTextBox();
            SelectButton = new Button();
            CancelBtn = new Button();
            txtFirstName = new TextBox();
            label1 = new Label();
            AddAccount = new Button();
            ((System.ComponentModel.ISupportInitialize)PersonAccountResults).BeginInit();
            SuspendLayout();
            // 
            // PersonAccountResults
            // 
            PersonAccountResults.AllowUserToAddRows = false;
            PersonAccountResults.AllowUserToDeleteRows = false;
            PersonAccountResults.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            PersonAccountResults.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PersonAccountResults.EditMode = DataGridViewEditMode.EditProgrammatically;
            PersonAccountResults.Location = new Point(227, 39);
            PersonAccountResults.Margin = new Padding(4, 3, 4, 3);
            PersonAccountResults.MultiSelect = false;
            PersonAccountResults.Name = "PersonAccountResults";
            PersonAccountResults.ReadOnly = true;
            PersonAccountResults.RowHeadersVisible = false;
            PersonAccountResults.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PersonAccountResults.Size = new Size(1007, 483);
            PersonAccountResults.TabIndex = 16;
            PersonAccountResults.DoubleClick += PersonAccountResults_DoubleClick;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(227, 14);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(57, 15);
            label2.TabIndex = 14;
            label2.Text = "Accounts";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 84);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(63, 15);
            label3.TabIndex = 2;
            label3.Text = "Last Name";
            // 
            // txtLastName
            // 
            txtLastName.CharacterCasing = CharacterCasing.Upper;
            txtLastName.Location = new Point(14, 102);
            txtLastName.Margin = new Padding(4, 3, 4, 3);
            txtLastName.Name = "txtLastName";
            txtLastName.Size = new Size(205, 23);
            txtLastName.TabIndex = 3;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 190);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(34, 15);
            label4.TabIndex = 6;
            label4.Text = "MRN";
            // 
            // mrnSearchText
            // 
            mrnSearchText.Location = new Point(14, 208);
            mrnSearchText.Margin = new Padding(4, 3, 4, 3);
            mrnSearchText.Name = "mrnSearchText";
            mrnSearchText.Size = new Size(206, 23);
            mrnSearchText.TabIndex = 7;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 242);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(28, 15);
            label5.TabIndex = 8;
            label5.Text = "SSN";
            // 
            // ssnSearchText
            // 
            ssnSearchText.Location = new Point(13, 260);
            ssnSearchText.Margin = new Padding(4, 3, 4, 3);
            ssnSearchText.Name = "ssnSearchText";
            ssnSearchText.Size = new Size(206, 23);
            ssnSearchText.TabIndex = 9;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(14, 288);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(73, 15);
            label6.TabIndex = 10;
            label6.Text = "Date of Birth";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(14, 340);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(25, 15);
            label7.TabIndex = 12;
            label7.Text = "Sex";
            // 
            // cbSexSearch
            // 
            cbSexSearch.FormattingEnabled = true;
            cbSexSearch.Location = new Point(14, 358);
            cbSexSearch.Margin = new Padding(4, 3, 4, 3);
            cbSexSearch.Name = "cbSexSearch";
            cbSexSearch.Size = new Size(206, 23);
            cbSexSearch.TabIndex = 13;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(13, 31);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(52, 15);
            label8.TabIndex = 0;
            label8.Text = "Account";
            // 
            // accountSearchText
            // 
            accountSearchText.CharacterCasing = CharacterCasing.Upper;
            accountSearchText.Location = new Point(14, 49);
            accountSearchText.Margin = new Padding(4, 3, 4, 3);
            accountSearchText.Name = "accountSearchText";
            accountSearchText.Size = new Size(206, 23);
            accountSearchText.TabIndex = 1;
            // 
            // SearchButton
            // 
            SearchButton.Location = new Point(13, 426);
            SearchButton.Margin = new Padding(4, 3, 4, 3);
            SearchButton.Name = "SearchButton";
            SearchButton.Size = new Size(206, 42);
            SearchButton.TabIndex = 14;
            SearchButton.Text = "Search";
            SearchButton.UseVisualStyleBackColor = true;
            SearchButton.Click += SearchButton_Click;
            // 
            // ClearButton
            // 
            ClearButton.Location = new Point(14, 480);
            ClearButton.Margin = new Padding(4, 3, 4, 3);
            ClearButton.Name = "ClearButton";
            ClearButton.Size = new Size(206, 42);
            ClearButton.TabIndex = 15;
            ClearButton.Text = "Clear";
            ClearButton.UseVisualStyleBackColor = true;
            ClearButton.Click += ClearButton_Click;
            // 
            // dobSearchText
            // 
            dobSearchText.Location = new Point(14, 306);
            dobSearchText.Margin = new Padding(4, 3, 4, 3);
            dobSearchText.Mask = "00/00/0000";
            dobSearchText.Name = "dobSearchText";
            dobSearchText.Size = new Size(206, 23);
            dobSearchText.TabIndex = 11;
            dobSearchText.ValidatingType = typeof(DateTime);
            // 
            // SelectButton
            // 
            SelectButton.DialogResult = DialogResult.OK;
            SelectButton.Location = new Point(989, 529);
            SelectButton.Margin = new Padding(4, 3, 4, 3);
            SelectButton.Name = "SelectButton";
            SelectButton.Size = new Size(110, 39);
            SelectButton.TabIndex = 17;
            SelectButton.Text = "Select";
            SelectButton.UseVisualStyleBackColor = true;
            SelectButton.Click += SelectButton_Click;
            // 
            // CancelBtn
            // 
            CancelBtn.DialogResult = DialogResult.Cancel;
            CancelBtn.Location = new Point(1123, 529);
            CancelBtn.Margin = new Padding(4, 3, 4, 3);
            CancelBtn.Name = "CancelBtn";
            CancelBtn.Size = new Size(110, 39);
            CancelBtn.TabIndex = 18;
            CancelBtn.Text = "Cancel";
            CancelBtn.UseVisualStyleBackColor = true;
            // 
            // txtFirstName
            // 
            txtFirstName.CharacterCasing = CharacterCasing.Upper;
            txtFirstName.Location = new Point(14, 157);
            txtFirstName.Margin = new Padding(4, 3, 4, 3);
            txtFirstName.Name = "txtFirstName";
            txtFirstName.Size = new Size(205, 23);
            txtFirstName.TabIndex = 5;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(14, 139);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(64, 15);
            label1.TabIndex = 4;
            label1.Text = "First Name";
            // 
            // AddAccount
            // 
            AddAccount.Location = new Point(855, 529);
            AddAccount.Margin = new Padding(4, 3, 4, 3);
            AddAccount.Name = "AddAccount";
            AddAccount.Size = new Size(110, 39);
            AddAccount.TabIndex = 19;
            AddAccount.Text = "New Account";
            AddAccount.UseVisualStyleBackColor = true;
            AddAccount.Click += AddAccount_Click;
            // 
            // PersonSearchForm
            // 
            AcceptButton = SearchButton;
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1247, 583);
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
            Margin = new Padding(4, 3, 4, 3);
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