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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            AmountTotal = new System.Windows.Forms.TextBox();
            ContractualTotal = new System.Windows.Forms.TextBox();
            WriteoffTotal = new System.Windows.Forms.TextBox();
            GrandTotal = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            SaveBatchButton = new System.Windows.Forms.Button();
            SubmitPaymentsButton = new System.Windows.Forms.Button();
            label6 = new System.Windows.Forms.Label();
            EntryMode = new System.Windows.Forms.ComboBox();
            label7 = new System.Windows.Forms.Label();
            DeleteBatchButton = new System.Windows.Forms.Button();
            NewBatchButton = new System.Windows.Forms.Button();
            OpenBatch = new MultiColumnCombo.MultiColumnComboBox();
            dgvPayments = new UserControls.LabDataGridView();
            dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewDateColumn1 = new Library.DataGridViewDateColumn();
            dataGridViewDateColumn2 = new Library.DataGridViewDateColumn();
            dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewDateColumn3 = new Library.DataGridViewDateColumn();
            dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)dgvPayments).BeginInit();
            SuspendLayout();
            // 
            // AmountTotal
            // 
            AmountTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            AmountTotal.Location = new System.Drawing.Point(66, 558);
            AmountTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AmountTotal.Name = "AmountTotal";
            AmountTotal.ReadOnly = true;
            AmountTotal.Size = new System.Drawing.Size(116, 23);
            AmountTotal.TabIndex = 1;
            // 
            // ContractualTotal
            // 
            ContractualTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            ContractualTotal.Location = new System.Drawing.Point(190, 558);
            ContractualTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ContractualTotal.Name = "ContractualTotal";
            ContractualTotal.ReadOnly = true;
            ContractualTotal.Size = new System.Drawing.Size(116, 23);
            ContractualTotal.TabIndex = 1;
            // 
            // WriteoffTotal
            // 
            WriteoffTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            WriteoffTotal.Location = new System.Drawing.Point(314, 558);
            WriteoffTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            WriteoffTotal.Name = "WriteoffTotal";
            WriteoffTotal.ReadOnly = true;
            WriteoffTotal.Size = new System.Drawing.Size(116, 23);
            WriteoffTotal.TabIndex = 1;
            // 
            // GrandTotal
            // 
            GrandTotal.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            GrandTotal.Location = new System.Drawing.Point(438, 558);
            GrandTotal.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            GrandTotal.Name = "GrandTotal";
            GrandTotal.ReadOnly = true;
            GrandTotal.Size = new System.Drawing.Size(116, 23);
            GrandTotal.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(63, 540);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(77, 15);
            label1.TabIndex = 2;
            label1.Text = "Amount Paid";
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(187, 540);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(69, 15);
            label2.TabIndex = 2;
            label2.Text = "Contractual";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(310, 540);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(52, 15);
            label3.TabIndex = 2;
            label3.Text = "WriteOff";
            // 
            // label4
            // 
            label4.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(434, 540);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(67, 15);
            label4.TabIndex = 2;
            label4.Text = "Grand Total";
            // 
            // label5
            // 
            label5.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(18, 567);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(37, 15);
            label5.TabIndex = 3;
            label5.Text = "Totals";
            // 
            // SaveBatchButton
            // 
            SaveBatchButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            SaveBatchButton.BackColor = System.Drawing.Color.Yellow;
            SaveBatchButton.Location = new System.Drawing.Point(904, 532);
            SaveBatchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SaveBatchButton.Name = "SaveBatchButton";
            SaveBatchButton.Size = new System.Drawing.Size(88, 50);
            SaveBatchButton.TabIndex = 4;
            SaveBatchButton.Text = "Save Batch for Later";
            SaveBatchButton.UseVisualStyleBackColor = false;
            SaveBatchButton.Visible = false;
            SaveBatchButton.Click += SaveBatchButton_Click;
            // 
            // SubmitPaymentsButton
            // 
            SubmitPaymentsButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            SubmitPaymentsButton.BackColor = System.Drawing.Color.LightGreen;
            SubmitPaymentsButton.Location = new System.Drawing.Point(999, 532);
            SubmitPaymentsButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            SubmitPaymentsButton.Name = "SubmitPaymentsButton";
            SubmitPaymentsButton.Size = new System.Drawing.Size(111, 50);
            SubmitPaymentsButton.TabIndex = 5;
            SubmitPaymentsButton.Text = "Submit Payments";
            SubmitPaymentsButton.UseVisualStyleBackColor = false;
            SubmitPaymentsButton.Click += SubmitPayments_Click;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(19, 17);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(103, 15);
            label6.TabIndex = 7;
            label6.Text = "Open Saved Batch";
            // 
            // EntryMode
            // 
            EntryMode.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            EntryMode.FormattingEnabled = true;
            EntryMode.Items.AddRange(new object[] { "Standard", "Patient", "Commercial", "Amount Paid", "Contractual", "Write Off", "Refunds" });
            EntryMode.Location = new System.Drawing.Point(944, 14);
            EntryMode.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            EntryMode.Name = "EntryMode";
            EntryMode.Size = new System.Drawing.Size(165, 23);
            EntryMode.TabIndex = 8;
            EntryMode.SelectedIndexChanged += EntryMode_SelectedIndexChanged;
            // 
            // label7
            // 
            label7.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(866, 17);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(68, 15);
            label7.TabIndex = 9;
            label7.Text = "Entry Mode";
            // 
            // DeleteBatchButton
            // 
            DeleteBatchButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            DeleteBatchButton.BackColor = System.Drawing.Color.Red;
            DeleteBatchButton.Location = new System.Drawing.Point(810, 532);
            DeleteBatchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            DeleteBatchButton.Name = "DeleteBatchButton";
            DeleteBatchButton.Size = new System.Drawing.Size(88, 50);
            DeleteBatchButton.TabIndex = 11;
            DeleteBatchButton.Text = "Delete Batch";
            DeleteBatchButton.UseVisualStyleBackColor = false;
            DeleteBatchButton.Click += DeleteBatch_Click;
            // 
            // NewBatchButton
            // 
            NewBatchButton.Location = new System.Drawing.Point(560, 12);
            NewBatchButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            NewBatchButton.Name = "NewBatchButton";
            NewBatchButton.Size = new System.Drawing.Size(88, 27);
            NewBatchButton.TabIndex = 13;
            NewBatchButton.Text = "New Batch";
            NewBatchButton.UseVisualStyleBackColor = true;
            NewBatchButton.Click += NewBatchButton_Click;
            // 
            // OpenBatch
            // 
            OpenBatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            OpenBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            OpenBatch.Location = new System.Drawing.Point(140, 14);
            OpenBatch.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            OpenBatch.Name = "OpenBatch";
            OpenBatch.Size = new System.Drawing.Size(412, 24);
            OpenBatch.TabIndex = 12;
            OpenBatch.SelectionChangeCommitted += OpenBatch_SelectionChangeCommitted;
            // 
            // dgvPayments
            // 
            dgvPayments.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            dgvPayments.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            dgvPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            dgvPayments.DefaultCellStyle = dataGridViewCellStyle2;
            dgvPayments.Location = new System.Drawing.Point(14, 45);
            dgvPayments.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dgvPayments.MultiSelect = false;
            dgvPayments.Name = "dgvPayments";
            dgvPayments.Size = new System.Drawing.Size(1096, 468);
            dgvPayments.TabIndex = 0;
            dgvPayments.CellDoubleClick += dgvPayments_CellDoubleClick;
            dgvPayments.CellEndEdit += dgvPayments_CellEndEdit;
            dgvPayments.CellEnter += dgvPayments_CellEnter;
            dgvPayments.CellFormatting += dgvPayments_CellFormatting;
            dgvPayments.CellPainting += dgvPayments_CellPainting;
            dgvPayments.CellValidating += dgvPayments_CellValidating;
            dgvPayments.CellValueChanged += dgvPayments_CellValueChanged;
            dgvPayments.DataError += dgvPayments_DataError;
            dgvPayments.DefaultValuesNeeded += dgvPayments_DefaultValuesNeeded;
            dgvPayments.EditingControlShowing += dgvPayments_EditingControlShowing;
            dgvPayments.RowEnter += dgvPayments_RowEnter;
            dgvPayments.RowLeave += dgvPayments_RowLeave;
            dgvPayments.RowsAdded += dgvPayments_RowsAdded;
            dgvPayments.RowsRemoved += dgvPayments_RowsRemoved;
            dgvPayments.UserDeletedRow += dgvPayments_UserDeletedRow;
            dgvPayments.UserDeletingRow += dgvPayments_UserDeletingRow;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Account";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Patient Name";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Balance";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Check No";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewDateColumn1
            // 
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            dataGridViewDateColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            dataGridViewDateColumn1.HeaderText = "Check Date";
            dataGridViewDateColumn1.Name = "dataGridViewDateColumn1";
            dataGridViewDateColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewDateColumn2
            // 
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewDateColumn2.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewDateColumn2.HeaderText = "DateReceived";
            dataGridViewDateColumn2.Name = "dataGridViewDateColumn2";
            dataGridViewDateColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewTextBoxColumn5.HeaderText = "Check Date";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle6.Format = "d";
            dataGridViewCellStyle6.NullValue = null;
            dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle6;
            dataGridViewTextBoxColumn6.HeaderText = "DateReceived";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0.00";
            dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle7;
            dataGridViewTextBoxColumn7.HeaderText = "Payment Source";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = "0.00";
            dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle8;
            dataGridViewTextBoxColumn8.HeaderText = "Amount Paid";
            dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewDateColumn3
            // 
            dataGridViewDateColumn3.HeaderText = "Write Off Date";
            dataGridViewDateColumn3.Name = "dataGridViewDateColumn3";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = "0.00";
            dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle9;
            dataGridViewTextBoxColumn9.HeaderText = "Contractual";
            dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = "0.00";
            dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle10;
            dataGridViewTextBoxColumn10.HeaderText = "Write Off";
            dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            dataGridViewTextBoxColumn11.HeaderText = "Comment";
            dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BatchRemittance
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(1124, 608);
            Controls.Add(NewBatchButton);
            Controls.Add(DeleteBatchButton);
            Controls.Add(OpenBatch);
            Controls.Add(label7);
            Controls.Add(EntryMode);
            Controls.Add(label6);
            Controls.Add(SubmitPaymentsButton);
            Controls.Add(SaveBatchButton);
            Controls.Add(label5);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(GrandTotal);
            Controls.Add(WriteoffTotal);
            Controls.Add(ContractualTotal);
            Controls.Add(AmountTotal);
            Controls.Add(dgvPayments);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BatchRemittance";
            Text = "Batch Remittance";
            Load += BatchRemittance_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPayments).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private LabBilling.UserControls.LabDataGridView dgvPayments;
        private System.Windows.Forms.TextBox AmountTotal;
        private System.Windows.Forms.TextBox ContractualTotal;
        private System.Windows.Forms.TextBox WriteoffTotal;
        private System.Windows.Forms.TextBox GrandTotal;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button SaveBatchButton;
        private System.Windows.Forms.Button SubmitPaymentsButton;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox EntryMode;
        private System.Windows.Forms.Label label7;
        private MultiColumnCombo.MultiColumnComboBox OpenBatch;
        private System.Windows.Forms.Button DeleteBatchButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn9;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn10;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn11;
        private Library.DataGridViewDateColumn dataGridViewDateColumn1;
        private Library.DataGridViewDateColumn dataGridViewDateColumn2;
        private Library.DataGridViewDateColumn dataGridViewDateColumn3;
        private System.Windows.Forms.Button NewBatchButton;
    }
}