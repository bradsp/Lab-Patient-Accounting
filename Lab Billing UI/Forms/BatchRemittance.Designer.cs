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
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle7 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle8 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle9 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle10 = new DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchRemittance));
            AmountTotal = new TextBox();
            ContractualTotal = new TextBox();
            WriteoffTotal = new TextBox();
            GrandTotal = new TextBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            label5 = new Label();
            label6 = new Label();
            OpenBatch = new MultiColumnCombo.MultiColumnComboBox();
            dgvPayments = new UserControls.LabDataGridView();
            dataGridViewTextBoxColumn1 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new DataGridViewTextBoxColumn();
            dataGridViewDateColumn1 = new Library.DataGridViewDateColumn();
            dataGridViewDateColumn2 = new Library.DataGridViewDateColumn();
            dataGridViewTextBoxColumn5 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn8 = new DataGridViewTextBoxColumn();
            dataGridViewDateColumn3 = new Library.DataGridViewDateColumn();
            dataGridViewTextBoxColumn9 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn10 = new DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn11 = new DataGridViewTextBoxColumn();
            toolStrip2 = new ToolStrip();
            newBatchToolStripButton = new ToolStripButton();
            saveBatchToolStripButton = new ToolStripButton();
            submitPaymentsToolStripButton = new ToolStripButton();
            deleteBatchToolStripButton = new ToolStripButton();
            toolStripSeparator1 = new ToolStripSeparator();
            entryModeToolStripLabel = new ToolStripLabel();
            entryModeToolStripComboBox = new ToolStripComboBox();
            ((System.ComponentModel.ISupportInitialize)dgvPayments).BeginInit();
            toolStrip2.SuspendLayout();
            SuspendLayout();
            // 
            // AmountTotal
            // 
            AmountTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AmountTotal.Location = new Point(624, 55);
            AmountTotal.Margin = new Padding(4, 3, 4, 3);
            AmountTotal.Name = "AmountTotal";
            AmountTotal.ReadOnly = true;
            AmountTotal.Size = new Size(116, 23);
            AmountTotal.TabIndex = 1;
            // 
            // ContractualTotal
            // 
            ContractualTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ContractualTotal.Location = new Point(748, 55);
            ContractualTotal.Margin = new Padding(4, 3, 4, 3);
            ContractualTotal.Name = "ContractualTotal";
            ContractualTotal.ReadOnly = true;
            ContractualTotal.Size = new Size(116, 23);
            ContractualTotal.TabIndex = 1;
            // 
            // WriteoffTotal
            // 
            WriteoffTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            WriteoffTotal.Location = new Point(872, 55);
            WriteoffTotal.Margin = new Padding(4, 3, 4, 3);
            WriteoffTotal.Name = "WriteoffTotal";
            WriteoffTotal.ReadOnly = true;
            WriteoffTotal.Size = new Size(116, 23);
            WriteoffTotal.TabIndex = 1;
            // 
            // GrandTotal
            // 
            GrandTotal.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            GrandTotal.Location = new Point(996, 55);
            GrandTotal.Margin = new Padding(4, 3, 4, 3);
            GrandTotal.Name = "GrandTotal";
            GrandTotal.ReadOnly = true;
            GrandTotal.Size = new Size(116, 23);
            GrandTotal.TabIndex = 1;
            // 
            // label1
            // 
            label1.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label1.AutoSize = true;
            label1.Location = new Point(621, 37);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(77, 15);
            label1.TabIndex = 2;
            label1.Text = "Amount Paid";
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(745, 37);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 2;
            label2.Text = "Contractual";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(868, 37);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(52, 15);
            label3.TabIndex = 2;
            label3.Text = "WriteOff";
            // 
            // label4
            // 
            label4.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label4.AutoSize = true;
            label4.Location = new Point(992, 37);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(67, 15);
            label4.TabIndex = 2;
            label4.Text = "Grand Total";
            // 
            // label5
            // 
            label5.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label5.AutoSize = true;
            label5.Location = new Point(579, 58);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 3;
            label5.Text = "Totals";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(16, 37);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(103, 15);
            label6.TabIndex = 7;
            label6.Text = "Open Saved Batch";
            // 
            // OpenBatch
            // 
            OpenBatch.DrawMode = DrawMode.OwnerDrawVariable;
            OpenBatch.DropDownStyle = ComboBoxStyle.DropDownList;
            OpenBatch.Location = new Point(16, 54);
            OpenBatch.Margin = new Padding(4, 3, 4, 3);
            OpenBatch.Name = "OpenBatch";
            OpenBatch.Size = new Size(343, 24);
            OpenBatch.TabIndex = 12;
            OpenBatch.SelectionChangeCommitted += OpenBatch_SelectionChangeCommitted;
            // 
            // dgvPayments
            // 
            dgvPayments.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            dgvPayments.BackgroundColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            dgvPayments.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            dgvPayments.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            dgvPayments.DefaultCellStyle = dataGridViewCellStyle2;
            dgvPayments.Location = new Point(18, 84);
            dgvPayments.Margin = new Padding(4, 3, 4, 3);
            dgvPayments.MultiSelect = false;
            dgvPayments.Name = "dgvPayments";
            dgvPayments.Size = new Size(1096, 512);
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
            dataGridViewDateColumn1.Resizable = DataGridViewTriState.True;
            // 
            // dataGridViewDateColumn2
            // 
            dataGridViewCellStyle4.Format = "d";
            dataGridViewCellStyle4.NullValue = null;
            dataGridViewDateColumn2.DefaultCellStyle = dataGridViewCellStyle4;
            dataGridViewDateColumn2.HeaderText = "DateReceived";
            dataGridViewDateColumn2.Name = "dataGridViewDateColumn2";
            dataGridViewDateColumn2.Resizable = DataGridViewTriState.True;
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
            dataGridViewTextBoxColumn9.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn9.SortMode = DataGridViewColumnSortMode.NotSortable;
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
            dataGridViewTextBoxColumn11.Resizable = DataGridViewTriState.True;
            dataGridViewTextBoxColumn11.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // toolStrip2
            // 
            toolStrip2.Font = new Font("Segoe UI", 9F);
            toolStrip2.Items.AddRange(new ToolStripItem[] { newBatchToolStripButton, saveBatchToolStripButton, submitPaymentsToolStripButton, deleteBatchToolStripButton, toolStripSeparator1, entryModeToolStripLabel, entryModeToolStripComboBox });
            toolStrip2.Location = new Point(0, 0);
            toolStrip2.Name = "toolStrip2";
            toolStrip2.Size = new Size(1124, 25);
            toolStrip2.TabIndex = 15;
            toolStrip2.Text = "toolStrip2";
            // 
            // newBatchToolStripButton
            // 
            newBatchToolStripButton.Image = (Image)resources.GetObject("newBatchToolStripButton.Image");
            newBatchToolStripButton.ImageTransparentColor = Color.Magenta;
            newBatchToolStripButton.Name = "newBatchToolStripButton";
            newBatchToolStripButton.Size = new Size(121, 22);
            newBatchToolStripButton.Text = "Create New Batch";
            newBatchToolStripButton.Click += NewBatchButton_Click;
            // 
            // saveBatchToolStripButton
            // 
            saveBatchToolStripButton.Image = (Image)resources.GetObject("saveBatchToolStripButton.Image");
            saveBatchToolStripButton.ImageTransparentColor = Color.Magenta;
            saveBatchToolStripButton.Name = "saveBatchToolStripButton";
            saveBatchToolStripButton.Size = new Size(191, 22);
            saveBatchToolStripButton.Text = "Save Batch for Later Processing";
            saveBatchToolStripButton.Click += saveBatchToolStripButton_Click;
            // 
            // submitPaymentsToolStripButton
            // 
            submitPaymentsToolStripButton.Image = (Image)resources.GetObject("submitPaymentsToolStripButton.Image");
            submitPaymentsToolStripButton.ImageTransparentColor = Color.Magenta;
            submitPaymentsToolStripButton.Name = "submitPaymentsToolStripButton";
            submitPaymentsToolStripButton.Size = new Size(120, 22);
            submitPaymentsToolStripButton.Text = "Submit Payments";
            submitPaymentsToolStripButton.Click += submitPaymentsToolStripButton_Click;
            // 
            // deleteBatchToolStripButton
            // 
            deleteBatchToolStripButton.Image = Properties.Resources.hiclipart_com_id_dbhyp;
            deleteBatchToolStripButton.ImageTransparentColor = Color.Magenta;
            deleteBatchToolStripButton.Name = "deleteBatchToolStripButton";
            deleteBatchToolStripButton.Size = new Size(93, 22);
            deleteBatchToolStripButton.Text = "Delete Batch";
            deleteBatchToolStripButton.Click += deleteBatchToolStripButton_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(6, 25);
            // 
            // entryModeToolStripLabel
            // 
            entryModeToolStripLabel.Name = "entryModeToolStripLabel";
            entryModeToolStripLabel.Size = new Size(68, 22);
            entryModeToolStripLabel.Text = "Entry Mode";
            // 
            // entryModeToolStripComboBox
            // 
            entryModeToolStripComboBox.Items.AddRange(new object[] { "Standard", "Patient", "Commercial", "Amount Paid", "Contractual", "Write Off", "Refunds" });
            entryModeToolStripComboBox.Name = "entryModeToolStripComboBox";
            entryModeToolStripComboBox.Size = new Size(121, 25);
            entryModeToolStripComboBox.SelectedIndexChanged += EntryMode_SelectedIndexChanged;
            // 
            // BatchRemittance
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1124, 608);
            Controls.Add(toolStrip2);
            Controls.Add(OpenBatch);
            Controls.Add(label6);
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
            Margin = new Padding(4, 3, 4, 3);
            Name = "BatchRemittance";
            Text = "Batch Remittance";
            Load += BatchRemittance_Load;
            ((System.ComponentModel.ISupportInitialize)dgvPayments).EndInit();
            toolStrip2.ResumeLayout(false);
            toolStrip2.PerformLayout();
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
        private System.Windows.Forms.Label label6;
        private MultiColumnCombo.MultiColumnComboBox OpenBatch;
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
        private ToolStrip toolStrip2;
        private ToolStripButton saveBatchToolStripButton;
        private ToolStripButton submitPaymentsToolStripButton;
        private ToolStripButton deleteBatchToolStripButton;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripLabel entryModeToolStripLabel;
        private ToolStripComboBox entryModeToolStripComboBox;
        private ToolStripButton newBatchToolStripButton;
    }
}