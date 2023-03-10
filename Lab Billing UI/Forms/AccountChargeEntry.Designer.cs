namespace LabBilling.Forms
{
    partial class AccountChargeEntry
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
            this.PostCharges = new System.Windows.Forms.Button();
            this.accountNoTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.patientNameTextBox = new System.Windows.Forms.TextBox();
            this.patientDOBTextBox = new System.Windows.Forms.MaskedTextBox();
            this.serviceDateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.patientSearchButton = new System.Windows.Forms.Button();
            this.clientTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.fincodeLabel = new System.Windows.Forms.Label();
            this.fincodeTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchEntry)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvBatchEntry
            // 
            this.dgvBatchEntry.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvBatchEntry.BackgroundColor = System.Drawing.SystemColors.ControlLight;
            this.dgvBatchEntry.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvBatchEntry.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.dgvBatchEntry.Location = new System.Drawing.Point(11, 98);
            this.dgvBatchEntry.Name = "dgvBatchEntry";
            this.dgvBatchEntry.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvBatchEntry.Size = new System.Drawing.Size(1137, 476);
            this.dgvBatchEntry.TabIndex = 20;
            this.dgvBatchEntry.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellEndEdit);
            this.dgvBatchEntry.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellEnter);
            this.dgvBatchEntry.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellLeave);
            this.dgvBatchEntry.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_CellValueChanged);
            this.dgvBatchEntry.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvBatchEntry_RowEnter);
            this.dgvBatchEntry.RowsAdded += new System.Windows.Forms.DataGridViewRowsAddedEventHandler(this.dgvBatchEntry_RowsAdded);
            // 
            // PostCharges
            // 
            this.PostCharges.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.PostCharges.Location = new System.Drawing.Point(1046, 69);
            this.PostCharges.Name = "PostCharges";
            this.PostCharges.Size = new System.Drawing.Size(102, 23);
            this.PostCharges.TabIndex = 21;
            this.PostCharges.Text = "&Post Charges";
            this.PostCharges.UseVisualStyleBackColor = true;
            this.PostCharges.Click += new System.EventHandler(this.PostCharges_Click);
            // 
            // accountNoTextBox
            // 
            this.accountNoTextBox.BackColor = System.Drawing.Color.White;
            this.accountNoTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.accountNoTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.accountNoTextBox.Location = new System.Drawing.Point(559, 24);
            this.accountNoTextBox.Name = "accountNoTextBox";
            this.accountNoTextBox.ReadOnly = true;
            this.accountNoTextBox.Size = new System.Drawing.Size(100, 16);
            this.accountNoTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(555, 8);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Account No";
            // 
            // patientNameTextBox
            // 
            this.patientNameTextBox.BackColor = System.Drawing.Color.White;
            this.patientNameTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.patientNameTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patientNameTextBox.Location = new System.Drawing.Point(116, 24);
            this.patientNameTextBox.Name = "patientNameTextBox";
            this.patientNameTextBox.ReadOnly = true;
            this.patientNameTextBox.Size = new System.Drawing.Size(196, 16);
            this.patientNameTextBox.TabIndex = 3;
            // 
            // patientDOBTextBox
            // 
            this.patientDOBTextBox.BackColor = System.Drawing.Color.White;
            this.patientDOBTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.patientDOBTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.patientDOBTextBox.Location = new System.Drawing.Point(347, 24);
            this.patientDOBTextBox.Name = "patientDOBTextBox";
            this.patientDOBTextBox.ReadOnly = true;
            this.patientDOBTextBox.Size = new System.Drawing.Size(100, 16);
            this.patientDOBTextBox.TabIndex = 7;
            this.patientDOBTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // serviceDateTextBox
            // 
            this.serviceDateTextBox.BackColor = System.Drawing.Color.White;
            this.serviceDateTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.serviceDateTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.serviceDateTextBox.Location = new System.Drawing.Point(453, 24);
            this.serviceDateTextBox.Name = "serviceDateTextBox";
            this.serviceDateTextBox.ReadOnly = true;
            this.serviceDateTextBox.Size = new System.Drawing.Size(100, 16);
            this.serviceDateTextBox.TabIndex = 13;
            this.serviceDateTextBox.ValidatingType = typeof(System.DateTime);
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
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(344, 10);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Date of Birth";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(450, 8);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(69, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Service Date";
            // 
            // patientSearchButton
            // 
            this.patientSearchButton.Location = new System.Drawing.Point(12, 12);
            this.patientSearchButton.Name = "patientSearchButton";
            this.patientSearchButton.Size = new System.Drawing.Size(75, 48);
            this.patientSearchButton.TabIndex = 24;
            this.patientSearchButton.Text = "&Select Patient";
            this.patientSearchButton.UseVisualStyleBackColor = true;
            this.patientSearchButton.Click += new System.EventHandler(this.patientSearchButton_Click);
            // 
            // clientTextBox
            // 
            this.clientTextBox.BackColor = System.Drawing.Color.White;
            this.clientTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.clientTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.clientTextBox.Location = new System.Drawing.Point(665, 24);
            this.clientTextBox.Name = "clientTextBox";
            this.clientTextBox.ReadOnly = true;
            this.clientTextBox.Size = new System.Drawing.Size(212, 16);
            this.clientTextBox.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(662, 8);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(33, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Client";
            // 
            // fincodeLabel
            // 
            this.fincodeLabel.AutoSize = true;
            this.fincodeLabel.Location = new System.Drawing.Point(907, 8);
            this.fincodeLabel.Name = "fincodeLabel";
            this.fincodeLabel.Size = new System.Drawing.Size(77, 13);
            this.fincodeLabel.TabIndex = 26;
            this.fincodeLabel.Text = "Financial Code";
            // 
            // fincodeTextBox
            // 
            this.fincodeTextBox.BackColor = System.Drawing.Color.White;
            this.fincodeTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.fincodeTextBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fincodeTextBox.Location = new System.Drawing.Point(910, 24);
            this.fincodeTextBox.Name = "fincodeTextBox";
            this.fincodeTextBox.ReadOnly = true;
            this.fincodeTextBox.Size = new System.Drawing.Size(142, 16);
            this.fincodeTextBox.TabIndex = 25;
            // 
            // AccountChargeEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(1160, 637);
            this.Controls.Add(this.fincodeLabel);
            this.Controls.Add(this.fincodeTextBox);
            this.Controls.Add(this.clientTextBox);
            this.Controls.Add(this.patientSearchButton);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.serviceDateTextBox);
            this.Controls.Add(this.patientDOBTextBox);
            this.Controls.Add(this.patientNameTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.accountNoTextBox);
            this.Controls.Add(this.PostCharges);
            this.Controls.Add(this.dgvBatchEntry);
            this.Name = "AccountChargeEntry";
            this.Text = "Account Charge Entry";
            this.Load += new System.EventHandler(this.BatchChargeEntry_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvBatchEntry)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvBatchEntry;
        private System.Windows.Forms.Button PostCharges;
        private System.Windows.Forms.TextBox accountNoTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox patientNameTextBox;
        private System.Windows.Forms.MaskedTextBox patientDOBTextBox;
        private System.Windows.Forms.MaskedTextBox serviceDateTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button patientSearchButton;
        private System.Windows.Forms.TextBox clientTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label fincodeLabel;
        private System.Windows.Forms.TextBox fincodeTextBox;
    }
}