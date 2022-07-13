namespace LabBilling.Forms
{
    partial class BatchChargeEntry
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
            this.dgvBatchEntry = new System.Windows.Forms.DataGridView();
            this.SaveCharges = new System.Windows.Forms.Button();
            this.AccountNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PatientName = new System.Windows.Forms.TextBox();
            this.PatientSSN = new System.Windows.Forms.MaskedTextBox();
            this.PatientDOB = new System.Windows.Forms.MaskedTextBox();
            this.PatientSex = new System.Windows.Forms.ComboBox();
            this.Client = new System.Windows.Forms.ComboBox();
            this.ServiceDate = new System.Windows.Forms.MaskedTextBox();
            this.Comment = new System.Windows.Forms.TextBox();
            this.CDM = new System.Windows.Forms.TextBox();
            this.Qty = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AddChargeToGrid = new System.Windows.Forms.Button();
            this.cdmDesc = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Qty)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBatchEntry
            // 
            this.dgvBatchEntry.AllowUserToAddRows = false;
            this.dgvBatchEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBatchEntry.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvBatchEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatchEntry.Location = new System.Drawing.Point(11, 98);
            this.dgvBatchEntry.Name = "dgvBatchEntry";
            this.dgvBatchEntry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBatchEntry.Size = new System.Drawing.Size(1135, 476);
            this.dgvBatchEntry.TabIndex = 20;
            this.dgvBatchEntry.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellEndEdit);
            this.dgvBatchEntry.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellLeave);
            this.dgvBatchEntry.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellValueChanged);
            // 
            // SaveCharges
            // 
            this.SaveCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveCharges.Location = new System.Drawing.Point(1071, 595);
            this.SaveCharges.Name = "SaveCharges";
            this.SaveCharges.Size = new System.Drawing.Size(75, 23);
            this.SaveCharges.TabIndex = 21;
            this.SaveCharges.Text = "Save";
            this.SaveCharges.UseVisualStyleBackColor = true;
            this.SaveCharges.Click += new System.EventHandler(this.SaveCharges_Click);
            // 
            // AccountNo
            // 
            this.AccountNo.Location = new System.Drawing.Point(10, 24);
            this.AccountNo.Name = "AccountNo";
            this.AccountNo.Size = new System.Drawing.Size(100, 20);
            this.AccountNo.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account No";
            // 
            // PatientName
            // 
            this.PatientName.Location = new System.Drawing.Point(116, 24);
            this.PatientName.Name = "PatientName";
            this.PatientName.Size = new System.Drawing.Size(196, 20);
            this.PatientName.TabIndex = 3;
            // 
            // PatientSSN
            // 
            this.PatientSSN.Location = new System.Drawing.Point(319, 24);
            this.PatientSSN.Mask = "000-00-0000";
            this.PatientSSN.Name = "PatientSSN";
            this.PatientSSN.Size = new System.Drawing.Size(100, 20);
            this.PatientSSN.TabIndex = 5;
            // 
            // PatientDOB
            // 
            this.PatientDOB.Location = new System.Drawing.Point(425, 24);
            this.PatientDOB.Mask = "00/00/0000";
            this.PatientDOB.Name = "PatientDOB";
            this.PatientDOB.Size = new System.Drawing.Size(100, 20);
            this.PatientDOB.TabIndex = 7;
            this.PatientDOB.ValidatingType = typeof(System.DateTime);
            // 
            // PatientSex
            // 
            this.PatientSex.FormattingEnabled = true;
            this.PatientSex.Items.AddRange(new object[] {
            "Male",
            "Female",
            "Unknown"});
            this.PatientSex.Location = new System.Drawing.Point(531, 23);
            this.PatientSex.Name = "PatientSex";
            this.PatientSex.Size = new System.Drawing.Size(82, 21);
            this.PatientSex.TabIndex = 9;
            // 
            // Client
            // 
            this.Client.FormattingEnabled = true;
            this.Client.Location = new System.Drawing.Point(619, 23);
            this.Client.Name = "Client";
            this.Client.Size = new System.Drawing.Size(160, 21);
            this.Client.TabIndex = 11;
            // 
            // ServiceDate
            // 
            this.ServiceDate.Location = new System.Drawing.Point(786, 24);
            this.ServiceDate.Mask = "00/00/0000";
            this.ServiceDate.Name = "ServiceDate";
            this.ServiceDate.Size = new System.Drawing.Size(100, 20);
            this.ServiceDate.TabIndex = 13;
            this.ServiceDate.ValidatingType = typeof(System.DateTime);
            // 
            // Comment
            // 
            this.Comment.Location = new System.Drawing.Point(897, 24);
            this.Comment.Name = "Comment";
            this.Comment.Size = new System.Drawing.Size(249, 20);
            this.Comment.TabIndex = 15;
            // 
            // CDM
            // 
            this.CDM.Location = new System.Drawing.Point(10, 72);
            this.CDM.Name = "CDM";
            this.CDM.Size = new System.Drawing.Size(100, 20);
            this.CDM.TabIndex = 17;
            this.CDM.Leave += new System.EventHandler(this.CDM_Leave);
            // 
            // Qty
            // 
            this.Qty.Location = new System.Drawing.Point(319, 72);
            this.Qty.Name = "Qty";
            this.Qty.Size = new System.Drawing.Size(120, 20);
            this.Qty.TabIndex = 19;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(113, 10);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(71, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Patient Name";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(316, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "SSN";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(422, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Date of Birth";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(528, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(25, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Sex";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(625, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(33, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Client";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(785, 10);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Service Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(894, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Comment";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(7, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "CDM";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(316, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Qty";
            // 
            // AddChargeToGrid
            // 
            this.AddChargeToGrid.Location = new System.Drawing.Point(445, 69);
            this.AddChargeToGrid.Name = "AddChargeToGrid";
            this.AddChargeToGrid.Size = new System.Drawing.Size(75, 23);
            this.AddChargeToGrid.TabIndex = 22;
            this.AddChargeToGrid.Text = "Add Charge";
            this.AddChargeToGrid.UseVisualStyleBackColor = true;
            this.AddChargeToGrid.Click += new System.EventHandler(this.AddChargeToGrid_Click);
            // 
            // cdmDesc
            // 
            this.cdmDesc.Location = new System.Drawing.Point(116, 72);
            this.cdmDesc.Name = "cdmDesc";
            this.cdmDesc.ReadOnly = true;
            this.cdmDesc.Size = new System.Drawing.Size(196, 20);
            this.cdmDesc.TabIndex = 23;
            // 
            // BatchChargeEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1160, 637);
            this.Controls.Add(this.cdmDesc);
            this.Controls.Add(this.AddChargeToGrid);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Qty);
            this.Controls.Add(this.CDM);
            this.Controls.Add(this.Comment);
            this.Controls.Add(this.ServiceDate);
            this.Controls.Add(this.Client);
            this.Controls.Add(this.PatientSex);
            this.Controls.Add(this.PatientDOB);
            this.Controls.Add(this.PatientSSN);
            this.Controls.Add(this.PatientName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AccountNo);
            this.Controls.Add(this.SaveCharges);
            this.Controls.Add(this.dgvBatchEntry);
            this.Name = "BatchChargeEntry";
            this.Text = "BatchChargeEntry";
            this.Load += new System.EventHandler(this.BatchChargeEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchEntry)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Qty)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBatchEntry;
        private System.Windows.Forms.Button SaveCharges;
        private System.Windows.Forms.TextBox AccountNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PatientName;
        private System.Windows.Forms.MaskedTextBox PatientSSN;
        private System.Windows.Forms.MaskedTextBox PatientDOB;
        private System.Windows.Forms.ComboBox PatientSex;
        private System.Windows.Forms.ComboBox Client;
        private System.Windows.Forms.MaskedTextBox ServiceDate;
        private System.Windows.Forms.TextBox Comment;
        private System.Windows.Forms.TextBox CDM;
        private System.Windows.Forms.NumericUpDown Qty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button AddChargeToGrid;
        private System.Windows.Forms.TextBox cdmDesc;
    }
}