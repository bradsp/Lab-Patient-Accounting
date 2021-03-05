namespace LabBilling.Forms
{
    partial class BatchRemittance
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchRemittance));
            this.dgvPayments = new System.Windows.Forms.DataGridView();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AccountSearch = new System.Windows.Forms.DataGridViewButtonColumn();
            this.PatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DateReceived = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PaymentSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contractual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteOff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteOffCode = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.Comment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountTotal = new System.Windows.Forms.TextBox();
            this.ContractualTotal = new System.Windows.Forms.TextBox();
            this.WriteoffTotal = new System.Windows.Forms.TextBox();
            this.GrandTotal = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SaveBatch = new System.Windows.Forms.Button();
            this.SubmitPayments = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.EntryMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.OpenBatch = new MTGCComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPayments
            // 
            this.dgvPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Account,
            this.AccountSearch,
            this.PatientName,
            this.Balance,
            this.CheckNo,
            this.CheckDate,
            this.DateReceived,
            this.PaymentSource,
            this.AmountPaid,
            this.Contractual,
            this.WriteOff,
            this.WriteOffCode,
            this.Comment});
            this.dgvPayments.Location = new System.Drawing.Point(12, 33);
            this.dgvPayments.Name = "dgvPayments";
            this.dgvPayments.Size = new System.Drawing.Size(939, 427);
            this.dgvPayments.TabIndex = 0;
            this.dgvPayments.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellContentClick);
            this.dgvPayments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellValueChanged);
            this.dgvPayments.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_RowLeave);
            this.dgvPayments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPayments_RowsAdded);
            // 
            // Account
            // 
            this.Account.HeaderText = "Account";
            this.Account.Name = "Account";
            // 
            // AccountSearch
            // 
            this.AccountSearch.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.AccountSearch.HeaderText = "Search";
            this.AccountSearch.Name = "AccountSearch";
            this.AccountSearch.Text = "Search";
            this.AccountSearch.ToolTipText = "Account Search";
            this.AccountSearch.UseColumnTextForButtonValue = true;
            this.AccountSearch.Width = 47;
            // 
            // PatientName
            // 
            this.PatientName.HeaderText = "Patient Name";
            this.PatientName.Name = "PatientName";
            // 
            // Balance
            // 
            this.Balance.HeaderText = "Balance";
            this.Balance.Name = "Balance";
            // 
            // CheckNo
            // 
            this.CheckNo.HeaderText = "Check No";
            this.CheckNo.Name = "CheckNo";
            // 
            // CheckDate
            // 
            dataGridViewCellStyle1.Format = "d";
            dataGridViewCellStyle1.NullValue = null;
            this.CheckDate.DefaultCellStyle = dataGridViewCellStyle1;
            this.CheckDate.HeaderText = "Check Date";
            this.CheckDate.Name = "CheckDate";
            // 
            // DateReceived
            // 
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.DateReceived.DefaultCellStyle = dataGridViewCellStyle2;
            this.DateReceived.HeaderText = "DateReceived";
            this.DateReceived.Name = "DateReceived";
            // 
            // PaymentSource
            // 
            this.PaymentSource.HeaderText = "Payment Source";
            this.PaymentSource.Name = "PaymentSource";
            // 
            // AmountPaid
            // 
            dataGridViewCellStyle3.Format = "N2";
            dataGridViewCellStyle3.NullValue = "0.00";
            this.AmountPaid.DefaultCellStyle = dataGridViewCellStyle3;
            this.AmountPaid.HeaderText = "Amount Paid";
            this.AmountPaid.Name = "AmountPaid";
            // 
            // Contractual
            // 
            dataGridViewCellStyle4.Format = "N2";
            dataGridViewCellStyle4.NullValue = "0.00";
            this.Contractual.DefaultCellStyle = dataGridViewCellStyle4;
            this.Contractual.HeaderText = "Contractual";
            this.Contractual.Name = "Contractual";
            // 
            // WriteOff
            // 
            dataGridViewCellStyle5.Format = "N2";
            dataGridViewCellStyle5.NullValue = "0.00";
            this.WriteOff.DefaultCellStyle = dataGridViewCellStyle5;
            this.WriteOff.HeaderText = "Write Off";
            this.WriteOff.Name = "WriteOff";
            // 
            // WriteOffCode
            // 
            this.WriteOffCode.HeaderText = "Write Off Code";
            this.WriteOffCode.Name = "WriteOffCode";
            this.WriteOffCode.Width = 75;
            // 
            // Comment
            // 
            this.Comment.HeaderText = "Comment";
            this.Comment.Name = "Comment";
            this.Comment.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.Comment.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // AmountTotal
            // 
            this.AmountTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.AmountTotal.Location = new System.Drawing.Point(57, 502);
            this.AmountTotal.Name = "AmountTotal";
            this.AmountTotal.ReadOnly = true;
            this.AmountTotal.Size = new System.Drawing.Size(100, 20);
            this.AmountTotal.TabIndex = 1;
            // 
            // ContractualTotal
            // 
            this.ContractualTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ContractualTotal.Location = new System.Drawing.Point(163, 502);
            this.ContractualTotal.Name = "ContractualTotal";
            this.ContractualTotal.ReadOnly = true;
            this.ContractualTotal.Size = new System.Drawing.Size(100, 20);
            this.ContractualTotal.TabIndex = 1;
            // 
            // WriteoffTotal
            // 
            this.WriteoffTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WriteoffTotal.Location = new System.Drawing.Point(269, 502);
            this.WriteoffTotal.Name = "WriteoffTotal";
            this.WriteoffTotal.ReadOnly = true;
            this.WriteoffTotal.Size = new System.Drawing.Size(100, 20);
            this.WriteoffTotal.TabIndex = 1;
            // 
            // GrandTotal
            // 
            this.GrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GrandTotal.Location = new System.Drawing.Point(375, 502);
            this.GrandTotal.Name = "GrandTotal";
            this.GrandTotal.ReadOnly = true;
            this.GrandTotal.Size = new System.Drawing.Size(100, 20);
            this.GrandTotal.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 486);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Amount Paid";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 486);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Contractual";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 486);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "WriteOff";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(372, 486);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Grand Total";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 509);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Totals";
            // 
            // SaveBatch
            // 
            this.SaveBatch.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBatch.BackColor = System.Drawing.Color.Yellow;
            this.SaveBatch.Location = new System.Drawing.Point(775, 479);
            this.SaveBatch.Name = "SaveBatch";
            this.SaveBatch.Size = new System.Drawing.Size(75, 43);
            this.SaveBatch.TabIndex = 4;
            this.SaveBatch.Text = "Save Batch for Later";
            this.SaveBatch.UseVisualStyleBackColor = false;
            this.SaveBatch.Click += new System.EventHandler(this.SaveBatch_Click);
            // 
            // SubmitPayments
            // 
            this.SubmitPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SubmitPayments.BackColor = System.Drawing.Color.LightGreen;
            this.SubmitPayments.Location = new System.Drawing.Point(856, 479);
            this.SubmitPayments.Name = "SubmitPayments";
            this.SubmitPayments.Size = new System.Drawing.Size(95, 43);
            this.SubmitPayments.TabIndex = 5;
            this.SubmitPayments.Text = "Submit Payments";
            this.SubmitPayments.UseVisualStyleBackColor = false;
            this.SubmitPayments.Click += new System.EventHandler(this.SubmitPayments_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 9);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(98, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Open Saved Batch";
            // 
            // EntryMode
            // 
            this.EntryMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.EntryMode.FormattingEnabled = true;
            this.EntryMode.Items.AddRange(new object[] {
            "Standard",
            "Patient",
            "Commercial",
            "Amount Paid",
            "Contractual",
            "Write Off"});
            this.EntryMode.Location = new System.Drawing.Point(809, 6);
            this.EntryMode.Name = "EntryMode";
            this.EntryMode.Size = new System.Drawing.Size(142, 21);
            this.EntryMode.TabIndex = 8;
            this.EntryMode.SelectedIndexChanged += new System.EventHandler(this.EntryMode_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(742, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Entry Mode";
            // 
            // OpenBatch
            // 
            this.OpenBatch.ArrowBoxColor = System.Drawing.SystemColors.Control;
            this.OpenBatch.ArrowColor = System.Drawing.Color.Black;
            this.OpenBatch.BindedControl = ((MTGCComboBox.ControlloAssociato)(resources.GetObject("OpenBatch.BindedControl")));
            this.OpenBatch.BorderStyle = MTGCComboBox.TipiBordi.FlatXP;
            this.OpenBatch.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.OpenBatch.ColumnNum = 1;
            this.OpenBatch.ColumnWidth = "121";
            this.OpenBatch.DisabledArrowBoxColor = System.Drawing.SystemColors.Control;
            this.OpenBatch.DisabledArrowColor = System.Drawing.Color.LightGray;
            this.OpenBatch.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.OpenBatch.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.OpenBatch.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.OpenBatch.DisplayMember = "Text";
            this.OpenBatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.OpenBatch.DropDownArrowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(169)))), ((int)(((byte)(223)))));
            this.OpenBatch.DropDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(210)))), ((int)(((byte)(238)))));
            this.OpenBatch.DropDownForeColor = System.Drawing.Color.Black;
            this.OpenBatch.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown;
            this.OpenBatch.DropDownWidth = 141;
            this.OpenBatch.GridLineColor = System.Drawing.Color.LightGray;
            this.OpenBatch.GridLineHorizontal = false;
            this.OpenBatch.GridLineVertical = false;
            this.OpenBatch.HighlightBorderColor = System.Drawing.Color.Blue;
            this.OpenBatch.HighlightBorderOnMouseEvents = true;
            this.OpenBatch.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            this.OpenBatch.Location = new System.Drawing.Point(121, 6);
            this.OpenBatch.ManagingFastMouseMoving = true;
            this.OpenBatch.ManagingFastMouseMovingInterval = 30;
            this.OpenBatch.Name = "OpenBatch";
            this.OpenBatch.NormalBorderColor = System.Drawing.Color.Black;
            this.OpenBatch.SelectedItem = null;
            this.OpenBatch.SelectedValue = null;
            this.OpenBatch.Size = new System.Drawing.Size(187, 21);
            this.OpenBatch.TabIndex = 10;
            this.OpenBatch.SelectedIndexChanged += new System.EventHandler(this.OpenBatch_SelectedIndexChanged);
            // 
            // BatchRemittance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(963, 534);
            this.Controls.Add(this.OpenBatch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.EntryMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SubmitPayments);
            this.Controls.Add(this.SaveBatch);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.GrandTotal);
            this.Controls.Add(this.WriteoffTotal);
            this.Controls.Add(this.ContractualTotal);
            this.Controls.Add(this.AmountTotal);
            this.Controls.Add(this.dgvPayments);
            this.Name = "BatchRemittance";
            this.Text = "BatchRemittance";
            this.Load += new System.EventHandler(this.BatchRemittance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvPayments;
        private System.Windows.Forms.TextBox AmountTotal;
        private System.Windows.Forms.TextBox ContractualTotal;
        private System.Windows.Forms.TextBox WriteoffTotal;
        private System.Windows.Forms.TextBox GrandTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button SaveBatch;
        private System.Windows.Forms.Button SubmitPayments;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox EntryMode;
        private System.Windows.Forms.Label label7;
        private MTGCComboBox OpenBatch;
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewButtonColumn AccountSearch;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckNo;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn DateReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contractual;
        private System.Windows.Forms.DataGridViewTextBoxColumn WriteOff;
        private System.Windows.Forms.DataGridViewComboBoxColumn WriteOffCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
    }
}