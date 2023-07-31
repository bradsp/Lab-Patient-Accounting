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
            this.NewBatchButton = new System.Windows.Forms.Button();
            this.OpenBatch = new MultiColumnCombo.MultiColumnComboBox();
            this.dgvPayments = new LabBilling.UserControls.LabDataGridView();
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
            this.dataGridViewTextBoxColumn10 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn11 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).BeginInit();
            this.SuspendLayout();
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
            this.SaveBatchButton.Visible = false;
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
            this.EntryMode.Location = new System.Drawing.Point(809, 12);
            this.EntryMode.Name = "EntryMode";
            this.EntryMode.Size = new System.Drawing.Size(142, 21);
            this.EntryMode.TabIndex = 8;
            this.EntryMode.SelectedIndexChanged += new System.EventHandler(this.EntryMode_SelectedIndexChanged);
            // 
            // label7
            // 
            this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(742, 15);
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
            // NewBatchButton
            // 
            this.NewBatchButton.Location = new System.Drawing.Point(480, 10);
            this.NewBatchButton.Name = "NewBatchButton";
            this.NewBatchButton.Size = new System.Drawing.Size(75, 23);
            this.NewBatchButton.TabIndex = 13;
            this.NewBatchButton.Text = "New Batch";
            this.NewBatchButton.UseVisualStyleBackColor = true;
            this.NewBatchButton.Click += new System.EventHandler(this.NewBatchButton_Click);
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
            // dgvPayments
            // 
            this.dgvPayments.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvPayments.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvPayments.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvPayments.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvPayments.Location = new System.Drawing.Point(12, 39);
            this.dgvPayments.MultiSelect = false;
            this.dgvPayments.Name = "dgvPayments";
            this.dgvPayments.Size = new System.Drawing.Size(939, 406);
            this.dgvPayments.TabIndex = 0;
            this.dgvPayments.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellDoubleClick);
            this.dgvPayments.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellEndEdit);
            this.dgvPayments.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellEnter);
            this.dgvPayments.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.dgvPayments_CellFormatting);
            this.dgvPayments.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvPayments_CellPainting);
            this.dgvPayments.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.dgvPayments_CellValidating);
            this.dgvPayments.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_CellValueChanged);
            this.dgvPayments.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.dgvPayments_DataError);
            this.dgvPayments.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvPayments_DefaultValuesNeeded);
            this.dgvPayments.EditingControlShowing += new System.Windows.Forms.DataGridViewEditingControlShowingEventHandler(this.dgvPayments_EditingControlShowing);
            this.dgvPayments.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_RowEnter);
            this.dgvPayments.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvPayments_RowLeave);
            this.dgvPayments.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvPayments_RowsAdded);
            this.dgvPayments.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.dgvPayments_RowsRemoved);
            this.dgvPayments.UserDeletedRow += new System.Windows.Forms.DataGridViewRowEventHandler(this.dgvPayments_UserDeletedRow);
            this.dgvPayments.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.dgvPayments_UserDeletingRow);
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
            dataGridViewCellStyle3.Format = "d";
            dataGridViewCellStyle3.NullValue = null;
            this.dataGridViewDateColumn1.DefaultCellStyle = dataGridViewCellStyle3;
            this.dataGridViewDateColumn1.HeaderText = "Check Date";
            this.dataGridViewDateColumn1.Name = "dataGridViewDateColumn1";
            this.dataGridViewDateColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewDateColumn2
            // 
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            this.dataGridViewDateColumn2.DefaultCellStyle = dataGridViewCellStyle4;
            this.dataGridViewDateColumn2.HeaderText = "DateReceived";
            this.dataGridViewDateColumn2.Name = "dataGridViewDateColumn2";
            this.dataGridViewDateColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewCellStyle5.Format = "d";
            dataGridViewCellStyle5.NullValue = null;
            this.dataGridViewTextBoxColumn5.DefaultCellStyle = dataGridViewCellStyle5;
            this.dataGridViewTextBoxColumn5.HeaderText = "Check Date";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewCellStyle6.Format = "d";
            dataGridViewCellStyle6.NullValue = null;
            this.dataGridViewTextBoxColumn6.DefaultCellStyle = dataGridViewCellStyle6;
            this.dataGridViewTextBoxColumn6.HeaderText = "DateReceived";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewCellStyle7.Format = "N2";
            dataGridViewCellStyle7.NullValue = "0.00";
            this.dataGridViewTextBoxColumn7.DefaultCellStyle = dataGridViewCellStyle7;
            this.dataGridViewTextBoxColumn7.HeaderText = "Payment Source";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            // 
            // dataGridViewTextBoxColumn8
            // 
            dataGridViewCellStyle8.Format = "N2";
            dataGridViewCellStyle8.NullValue = "0.00";
            this.dataGridViewTextBoxColumn8.DefaultCellStyle = dataGridViewCellStyle8;
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
            dataGridViewCellStyle9.Format = "N2";
            dataGridViewCellStyle9.NullValue = "0.00";
            this.dataGridViewTextBoxColumn9.DefaultCellStyle = dataGridViewCellStyle9;
            this.dataGridViewTextBoxColumn9.HeaderText = "Contractual";
            this.dataGridViewTextBoxColumn9.Name = "dataGridViewTextBoxColumn9";
            this.dataGridViewTextBoxColumn9.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewTextBoxColumn9.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // dataGridViewTextBoxColumn10
            // 
            dataGridViewCellStyle10.Format = "N2";
            dataGridViewCellStyle10.NullValue = "0.00";
            this.dataGridViewTextBoxColumn10.DefaultCellStyle = dataGridViewCellStyle10;
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
            this.Controls.Add(this.NewBatchButton);
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
            this.Text = "Batch Remittance";
            this.Load += new System.EventHandler(this.BatchRemittance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPayments)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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