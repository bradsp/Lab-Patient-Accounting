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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle13 = new System.Windows.Forms.DataGridViewCellStyle();
            this.dgvPayments = new System.Windows.Forms.DataGridView();
            this.Account = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.PatientName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Balance = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckNo = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CheckDate = new LabBilling.Library.DataGridViewDateColumn();
            this.DateReceived = new LabBilling.Library.DataGridViewDateColumn();
            this.PaymentSource = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.AmountPaid = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Contractual = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteOff = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.WriteOffDate = new LabBilling.Library.DataGridViewDateColumn();
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
            this.SaveBatchButton = new System.Windows.Forms.Button();
            this.SubmitPaymentsButton = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.EntryMode = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.DeleteBatchButton = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDateColumn1 = new LabBilling.Library.DataGridViewDateColumn();
            this.dataGridViewDateColumn2 = new LabBilling.Library.DataGridViewDateColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewDateColumn3 = new LabBilling.Library.DataGridViewDateColumn();
            this.dataGridViewTextBoxColumn9 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OpenBatch = new MultiColumnCombo.MultiColumnComboBox();
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPayments
            // 
            this.dgvPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPayments.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvPayments.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Account,
            this.PatientName,
            this.Balance,
            this.CheckNo,
            this.CheckDate,
            this.DateReceived,
            this.PaymentSource,
            this.AmountPaid,
            this.Contractual,
            this.WriteOff,
            this.WriteOffDate,
            this.WriteOffCode,
            this.Comment});
            this.dgvPayments.Location = new System.Drawing.Point(12, 39);
            this.dgvPayments.Name = "dgvPayments";
            this.dgvPayments.Size = new System.Drawing.Size(939, 406);
            this.dgvPayments.TabIndex = 0;
            this.dgvPayments.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellContentDoubleClick);
            this.dgvPayments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellDoubleClick);
            this.dgvPayments.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellEndEdit);
            this.dgvPayments.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellEnter);
            this.dgvPayments.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellLeave);
            this.dgvPayments.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPayments_CellPainting);
            this.dgvPayments.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvPayments_CellValidating);
            this.dgvPayments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellValueChanged);
            this.dgvPayments.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_RowEnter);
            this.dgvPayments.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_RowLeave);
            this.dgvPayments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPayments_RowsAdded);
            // 
            // Account
            // 
            this.Account.HeaderText = "Account";
            this.Account.Name = "Account";
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
            this.CheckDate.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // DateReceived
            // 
            dataGridViewCellStyle2.Format = "d";
            dataGridViewCellStyle2.NullValue = null;
            this.DateReceived.DefaultCellStyle = dataGridViewCellStyle2;
            this.DateReceived.HeaderText = "DateReceived";
            this.DateReceived.Name = "DateReceived";
            this.DateReceived.Resizable = System.Windows.Forms.DataGridViewTriState.True;
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
            // WriteOffDate
            // 
            this.WriteOffDate.HeaderText = "Write Off Date";
            this.WriteOffDate.Name = "WriteOffDate";
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
            this.AmountTotal.Location = new System.Drawing.Point(57, 484);
            this.AmountTotal.Name = "AmountTotal";
            this.AmountTotal.ReadOnly = true;
            this.AmountTotal.Size = new System.Drawing.Size(100, 20);
            this.AmountTotal.TabIndex = 1;
            // 
            // ContractualTotal
            // 
            this.ContractualTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.ContractualTotal.Location = new System.Drawing.Point(163, 484);
            this.ContractualTotal.Name = "ContractualTotal";
            this.ContractualTotal.ReadOnly = true;
            this.ContractualTotal.Size = new System.Drawing.Size(100, 20);
            this.ContractualTotal.TabIndex = 1;
            // 
            // WriteoffTotal
            // 
            this.WriteoffTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.WriteoffTotal.Location = new System.Drawing.Point(269, 484);
            this.WriteoffTotal.Name = "WriteoffTotal";
            this.WriteoffTotal.ReadOnly = true;
            this.WriteoffTotal.Size = new System.Drawing.Size(100, 20);
            this.WriteoffTotal.TabIndex = 1;
            // 
            // GrandTotal
            // 
            this.GrandTotal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.GrandTotal.Location = new System.Drawing.Point(375, 484);
            this.GrandTotal.Name = "GrandTotal";
            this.GrandTotal.ReadOnly = true;
            this.GrandTotal.Size = new System.Drawing.Size(100, 20);
            this.GrandTotal.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(54, 468);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(67, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Amount Paid";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(160, 468);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Contractual";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(266, 468);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "WriteOff";
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(372, 468);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Grand Total";
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 491);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(36, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Totals";
            // 
            // SaveBatchButton
            // 
            this.SaveBatchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveBatchButton.BackColor = System.Drawing.Color.Yellow;
            this.SaveBatchButton.Location = new System.Drawing.Point(775, 461);
            this.SaveBatchButton.Name = "SaveBatchButton";
            this.SaveBatchButton.Size = new System.Drawing.Size(75, 43);
            this.SaveBatchButton.TabIndex = 4;
            this.SaveBatchButton.Text = "Save Batch for Later";
            this.SaveBatchButton.UseVisualStyleBackColor = false;
            this.SaveBatchButton.Click += new System.EventHandler(this.SaveBatchButton_Click);
            // 
            // SubmitPaymentsButton
            // 
            this.SubmitPaymentsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SubmitPaymentsButton.BackColor = System.Drawing.Color.LightGreen;
            this.SubmitPaymentsButton.Location = new System.Drawing.Point(856, 461);
            this.SubmitPaymentsButton.Name = "SubmitPaymentsButton";
            this.SubmitPaymentsButton.Size = new System.Drawing.Size(95, 43);
            this.SubmitPaymentsButton.TabIndex = 5;
            this.SubmitPaymentsButton.Text = "Submit Payments";
            this.SubmitPaymentsButton.UseVisualStyleBackColor = false;
            this.SubmitPaymentsButton.Click += new System.EventHandler(this.SubmitPayments_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 15);
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
            this.EntryMode.Location = new System.Drawing.Point(547, 12);
            this.EntryMode.Name = "EntryMode";
            this.EntryMode.Size = new System.Drawing.Size(142, 21);
            this.EntryMode.TabIndex = 8;
            this.EntryMode.SelectedIndexChanged += new System.EventHandler(this.EntryMode_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(480, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 9;
            this.label7.Text = "Entry Mode";
            // 
            // DeleteBatchButton
            // 
            this.DeleteBatchButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.DeleteBatchButton.BackColor = System.Drawing.Color.Red;
            this.DeleteBatchButton.Location = new System.Drawing.Point(694, 461);
            this.DeleteBatchButton.Name = "DeleteBatchButton";
            this.DeleteBatchButton.Size = new System.Drawing.Size(75, 43);
            this.DeleteBatchButton.TabIndex = 11;
            this.DeleteBatchButton.Text = "Delete Batch";
            this.DeleteBatchButton.UseVisualStyleBackColor = false;
            this.DeleteBatchButton.Click += new System.EventHandler(this.DeleteBatch_Click);
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Account";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Patient Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Balance";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Check No";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            // 
            // dataGridViewDateColumn1
            // 
            dataGridViewCellStyle6.Format = "d";
            dataGridViewCellStyle6.NullValue = null;
            this.dataGridViewDateColumn1.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewDateColumn1.HeaderText = "Check Date";
            this.dataGridViewDateColumn1.Name = "dataGridViewDateColumn1";
            this.dataGridViewDateColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewDateColumn2
            // 
            dataGridViewCellStyle7.Format = "d";
            dataGridViewCellStyle7.NullValue = null;
            this.dataGridViewDateColumn2.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewDateColumn2.HeaderText = "DateReceived";
            this.dataGridViewDateColumn2.Name = "dataGridViewDateColumn2";
            this.dataGridViewDateColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle8.Format = "d";
            dataGridViewCellStyle8.NullValue = null;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle8;
            this.dataGridViewTextBoxColumn5.HeaderText = "Check Date";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle9.Format = "d";
            dataGridViewCellStyle9.NullValue = null;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn6.HeaderText = "DateReceived";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = "0.00";
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle10;
            this.dataGridViewTextBoxColumn7.HeaderText = "Payment Source";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle11.Format = "N2";
            dataGridViewCellStyle11.NullValue = "0.00";
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle11;
            this.dataGridViewTextBoxColumn8.HeaderText = "Amount Paid";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            // 
            // dataGridViewDateColumn3
            // 
            this.dataGridViewDateColumn3.HeaderText = "Write Off Date";
            this.dataGridViewDateColumn3.Name = "dataGridViewDateColumn3";
            // 
            // dataGridViewTextBoxColumn9
            // 
            dataGridViewCellStyle12.Format = "N2";
            dataGridViewCellStyle12.NullValue = "0.00";
            this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle12;
            this.dataGridViewTextBoxColumn9.HeaderText = "Contractual";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // OpenBatch
            // 
            this.OpenBatch.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.OpenBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.OpenBatch.Location = new System.Drawing.Point(120, 12);
            this.OpenBatch.Name = "OpenBatch";
            this.OpenBatch.Size = new System.Drawing.Size(354, 21);
            this.OpenBatch.TabIndex = 12;
            this.OpenBatch.SelectionChangeCommitted += new System.EventHandler(this.OpenBatch_SelectionChangeCommitted);
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewCellStyle13.Format = "N2";
            dataGridViewCellStyle13.NullValue = "0.00";
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle13;
            this.dataGridViewTextBoxColumn10.HeaderText = "Write Off";
            this.dataGridViewTextBoxColumn10.Name = "dataGridViewTextBoxColumn10";
            // 
            // dataGridViewTextBoxColumn11
            // 
            this.dataGridViewTextBoxColumn11.HeaderText = "Comment";
            this.dataGridViewTextBoxColumn11.Name = "dataGridViewTextBoxColumn11";
            this.dataGridViewTextBoxColumn11.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn11.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // BatchRemittance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(963, 527);
            this.Controls.Add(this.DeleteBatchButton);
            this.Controls.Add(this.OpenBatch);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.EntryMode);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.SubmitPaymentsButton);
            this.Controls.Add(this.SaveBatchButton);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn Account;
        private System.Windows.Forms.DataGridViewTextBoxColumn PatientName;
        private System.Windows.Forms.DataGridViewTextBoxColumn Balance;
        private System.Windows.Forms.DataGridViewTextBoxColumn CheckNo;
        private Library.DataGridViewDateColumn CheckDate;
        private Library.DataGridViewDateColumn DateReceived;
        private System.Windows.Forms.DataGridViewTextBoxColumn PaymentSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn AmountPaid;
        private System.Windows.Forms.DataGridViewTextBoxColumn Contractual;
        private System.Windows.Forms.DataGridViewTextBoxColumn WriteOff;
        private Library.DataGridViewDateColumn WriteOffDate;
        private System.Windows.Forms.DataGridViewComboBoxColumn WriteOffCode;
        private System.Windows.Forms.DataGridViewTextBoxColumn Comment;
        private Library.DataGridViewDateColumn dataGridViewDateColumn1;
        private Library.DataGridViewDateColumn dataGridViewDateColumn2;
        private Library.DataGridViewDateColumn dataGridViewDateColumn3;
    }
}