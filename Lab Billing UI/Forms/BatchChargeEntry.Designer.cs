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
            this.AccountNoTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.PatientNameTextBox = new System.Windows.Forms.TextBox();
            this.PatientSSNTextBox = new System.Windows.Forms.MaskedTextBox();
            this.PatientDOBTextBox = new System.Windows.Forms.MaskedTextBox();
            this.ServiceDateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.CommentTextBox = new System.Windows.Forms.TextBox();
            this.Qty = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.AddChargeButton = new System.Windows.Forms.Button();
            this.cdmDescTextBox = new System.Windows.Forms.TextBox();
            this.patientSearchButton = new System.Windows.Forms.Button();
            this.cdmTextBox = new System.Windows.Forms.TextBox();
            this.clientTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchEntry)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Qty)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBatchEntry
            // 
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
            // AccountNoTextBox
            // 
            this.AccountNoTextBox.BackColor = System.Drawing.Color.White;
            this.AccountNoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.AccountNoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.AccountNoTextBox.Location = new System.Drawing.Point(637, 24);
            this.AccountNoTextBox.Name = "AccountNoTextBox";
            this.AccountNoTextBox.ReadOnly = true;
            this.AccountNoTextBox.Size = new System.Drawing.Size(100, 16);
            this.AccountNoTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(633, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account No";
            // 
            // PatientNameTextBox
            // 
            this.PatientNameTextBox.BackColor = System.Drawing.Color.White;
            this.PatientNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PatientNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatientNameTextBox.Location = new System.Drawing.Point(116, 24);
            this.PatientNameTextBox.Name = "PatientNameTextBox";
            this.PatientNameTextBox.ReadOnly = true;
            this.PatientNameTextBox.Size = new System.Drawing.Size(196, 16);
            this.PatientNameTextBox.TabIndex = 3;
            // 
            // PatientSSNTextBox
            // 
            this.PatientSSNTextBox.BackColor = System.Drawing.Color.White;
            this.PatientSSNTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PatientSSNTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatientSSNTextBox.Location = new System.Drawing.Point(319, 24);
            this.PatientSSNTextBox.Mask = "000-00-0000";
            this.PatientSSNTextBox.Name = "PatientSSNTextBox";
            this.PatientSSNTextBox.ReadOnly = true;
            this.PatientSSNTextBox.Size = new System.Drawing.Size(100, 16);
            this.PatientSSNTextBox.TabIndex = 5;
            // 
            // PatientDOBTextBox
            // 
            this.PatientDOBTextBox.BackColor = System.Drawing.Color.White;
            this.PatientDOBTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.PatientDOBTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PatientDOBTextBox.Location = new System.Drawing.Point(425, 24);
            this.PatientDOBTextBox.Mask = "00/00/0000";
            this.PatientDOBTextBox.Name = "PatientDOBTextBox";
            this.PatientDOBTextBox.ReadOnly = true;
            this.PatientDOBTextBox.Size = new System.Drawing.Size(100, 16);
            this.PatientDOBTextBox.TabIndex = 7;
            this.PatientDOBTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // ServiceDateTextBox
            // 
            this.ServiceDateTextBox.BackColor = System.Drawing.Color.White;
            this.ServiceDateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.ServiceDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ServiceDateTextBox.Location = new System.Drawing.Point(531, 24);
            this.ServiceDateTextBox.Mask = "00/00/0000";
            this.ServiceDateTextBox.Name = "ServiceDateTextBox";
            this.ServiceDateTextBox.ReadOnly = true;
            this.ServiceDateTextBox.Size = new System.Drawing.Size(100, 16);
            this.ServiceDateTextBox.TabIndex = 13;
            this.ServiceDateTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // CommentTextBox
            // 
            this.CommentTextBox.Location = new System.Drawing.Point(551, 72);
            this.CommentTextBox.Name = "CommentTextBox";
            this.CommentTextBox.ReadOnly = true;
            this.CommentTextBox.Size = new System.Drawing.Size(249, 20);
            this.CommentTextBox.TabIndex = 15;
            // 
            // Qty
            // 
            this.Qty.Location = new System.Drawing.Point(425, 72);
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
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(528, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Service Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(548, 58);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "Comment";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(113, 56);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(31, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "CDM";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(422, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(23, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Qty";
            // 
            // AddChargeButton
            // 
            this.AddChargeButton.Location = new System.Drawing.Point(1071, 69);
            this.AddChargeButton.Name = "AddChargeButton";
            this.AddChargeButton.Size = new System.Drawing.Size(75, 23);
            this.AddChargeButton.TabIndex = 22;
            this.AddChargeButton.Text = "Add Charge";
            this.AddChargeButton.UseVisualStyleBackColor = true;
            this.AddChargeButton.Click += new System.EventHandler(this.AddChargeToGrid_Click);
            // 
            // cdmDescTextBox
            // 
            this.cdmDescTextBox.Location = new System.Drawing.Point(222, 72);
            this.cdmDescTextBox.Name = "cdmDescTextBox";
            this.cdmDescTextBox.ReadOnly = true;
            this.cdmDescTextBox.Size = new System.Drawing.Size(196, 20);
            this.cdmDescTextBox.TabIndex = 23;
            // 
            // patientSearchButton
            // 
            this.patientSearchButton.Location = new System.Drawing.Point(12, 12);
            this.patientSearchButton.Name = "patientSearchButton";
            this.patientSearchButton.Size = new System.Drawing.Size(75, 48);
            this.patientSearchButton.TabIndex = 24;
            this.patientSearchButton.Text = "Select Patient";
            this.patientSearchButton.UseVisualStyleBackColor = true;
            this.patientSearchButton.Click += new System.EventHandler(this.patientSearchButton_Click);
            // 
            // cdmTextBox
            // 
            this.cdmTextBox.Location = new System.Drawing.Point(116, 72);
            this.cdmTextBox.Name = "cdmTextBox";
            this.cdmTextBox.Size = new System.Drawing.Size(100, 20);
            this.cdmTextBox.TabIndex = 17;
            this.cdmTextBox.Leave += new System.EventHandler(this.CDM_Leave);
            // 
            // clientTextBox
            // 
            this.clientTextBox.BackColor = System.Drawing.Color.White;
            this.clientTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clientTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientTextBox.Location = new System.Drawing.Point(743, 24);
            this.clientTextBox.Name = "clientTextBox";
            this.clientTextBox.ReadOnly = true;
            this.clientTextBox.Size = new System.Drawing.Size(212, 16);
            this.clientTextBox.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(740, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Client";
            // 
            // BatchChargeEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1160, 637);
            this.Controls.Add(this.clientTextBox);
            this.Controls.Add(this.patientSearchButton);
            this.Controls.Add(this.cdmDescTextBox);
            this.Controls.Add(this.AddChargeButton);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.Qty);
            this.Controls.Add(this.cdmTextBox);
            this.Controls.Add(this.CommentTextBox);
            this.Controls.Add(this.ServiceDateTextBox);
            this.Controls.Add(this.PatientDOBTextBox);
            this.Controls.Add(this.PatientSSNTextBox);
            this.Controls.Add(this.PatientNameTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.AccountNoTextBox);
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
        private System.Windows.Forms.TextBox AccountNoTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox PatientNameTextBox;
        private System.Windows.Forms.MaskedTextBox PatientSSNTextBox;
        private System.Windows.Forms.MaskedTextBox PatientDOBTextBox;
        private System.Windows.Forms.MaskedTextBox ServiceDateTextBox;
        private System.Windows.Forms.TextBox CommentTextBox;
        private System.Windows.Forms.NumericUpDown Qty;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button AddChargeButton;
        private System.Windows.Forms.TextBox cdmDescTextBox;
        private System.Windows.Forms.Button patientSearchButton;
        private System.Windows.Forms.TextBox cdmTextBox;
        private System.Windows.Forms.TextBox clientTextBox;
        private System.Windows.Forms.Label label5;
    }
}