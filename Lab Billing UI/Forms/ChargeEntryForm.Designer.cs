namespace LabBilling.Forms
{
    partial class ChargeEntryForm
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
            this.tbBannerMRN = new System.Windows.Forms.TextBox();
            this.tbBannerAccount = new System.Windows.Forms.TextBox();
            this.tbBannerName = new System.Windows.Forms.TextBox();
            this.label37 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.nQty = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tbComment = new System.Windows.Forms.TextBox();
            this.btnSubmit = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.cbChargeItem = new MultiColumnCombo.MultiColumnComboBox();
            this.tbDateOfService = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SearchByCdm = new System.Windows.Forms.RadioButton();
            this.SearchByDescription = new System.Windows.Forms.RadioButton();
            this.SearchBy = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.ReferenceNumber = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nQty)).BeginInit();
            this.SearchBy.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbBannerMRN
            // 
            this.tbBannerMRN.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerMRN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerMRN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerMRN.Location = new System.Drawing.Point(233, 13);
            this.tbBannerMRN.Name = "tbBannerMRN";
            this.tbBannerMRN.ReadOnly = true;
            this.tbBannerMRN.Size = new System.Drawing.Size(94, 22);
            this.tbBannerMRN.TabIndex = 2;
            this.tbBannerMRN.TabStop = false;
            // 
            // tbBannerAccount
            // 
            this.tbBannerAccount.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerAccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerAccount.Location = new System.Drawing.Point(99, 13);
            this.tbBannerAccount.Name = "tbBannerAccount";
            this.tbBannerAccount.ReadOnly = true;
            this.tbBannerAccount.Size = new System.Drawing.Size(128, 22);
            this.tbBannerAccount.TabIndex = 1;
            this.tbBannerAccount.TabStop = false;
            // 
            // tbBannerName
            // 
            this.tbBannerName.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerName.Location = new System.Drawing.Point(12, 41);
            this.tbBannerName.Name = "tbBannerName";
            this.tbBannerName.ReadOnly = true;
            this.tbBannerName.Size = new System.Drawing.Size(547, 19);
            this.tbBannerName.TabIndex = 5;
            this.tbBannerName.TabStop = false;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(13, 18);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(80, 13);
            this.label37.TabIndex = 0;
            this.label37.Text = "Account/MRN:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(28, 154);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Charge Item";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(46, 190);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Quantity";
            // 
            // nQty
            // 
            this.nQty.Location = new System.Drawing.Point(99, 188);
            this.nQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nQty.Name = "nQty";
            this.nQty.Size = new System.Drawing.Size(120, 20);
            this.nQty.TabIndex = 10;
            this.nQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(41, 226);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Comment";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(99, 223);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(451, 20);
            this.tbComment.TabIndex = 14;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(101, 274);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 15;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(194, 274);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 16;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbChargeItem
            // 
            this.cbChargeItem.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.cbChargeItem.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbChargeItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.cbChargeItem.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbChargeItem.Location = new System.Drawing.Point(99, 154);
            this.cbChargeItem.Name = "cbChargeItem";
            this.cbChargeItem.Size = new System.Drawing.Size(451, 21);
            this.cbChargeItem.TabIndex = 13;
            // 
            // tbDateOfService
            // 
            this.tbDateOfService.BackColor = System.Drawing.SystemColors.Control;
            this.tbDateOfService.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDateOfService.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDateOfService.Location = new System.Drawing.Point(431, 13);
            this.tbDateOfService.Name = "tbDateOfService";
            this.tbDateOfService.ReadOnly = true;
            this.tbDateOfService.Size = new System.Drawing.Size(128, 22);
            this.tbDateOfService.TabIndex = 4;
            this.tbDateOfService.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(345, 18);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Date of Service";
            // 
            // SearchByCdm
            // 
            this.SearchByCdm.AutoSize = true;
            this.SearchByCdm.Checked = true;
            this.SearchByCdm.Location = new System.Drawing.Point(8, 23);
            this.SearchByCdm.Name = "SearchByCdm";
            this.SearchByCdm.Size = new System.Drawing.Size(89, 17);
            this.SearchByCdm.TabIndex = 0;
            this.SearchByCdm.TabStop = true;
            this.SearchByCdm.Text = "CDM Number";
            this.SearchByCdm.UseVisualStyleBackColor = true;
            this.SearchByCdm.CheckedChanged += new System.EventHandler(this.SearchByCheckChanged);
            // 
            // SearchByDescription
            // 
            this.SearchByDescription.AutoSize = true;
            this.SearchByDescription.Location = new System.Drawing.Point(103, 23);
            this.SearchByDescription.Name = "SearchByDescription";
            this.SearchByDescription.Size = new System.Drawing.Size(78, 17);
            this.SearchByDescription.TabIndex = 1;
            this.SearchByDescription.TabStop = true;
            this.SearchByDescription.Text = "Description";
            this.SearchByDescription.UseVisualStyleBackColor = true;
            this.SearchByDescription.CheckedChanged += new System.EventHandler(this.SearchByCheckChanged);
            // 
            // SearchBy
            // 
            this.SearchBy.Controls.Add(this.SearchByCdm);
            this.SearchBy.Controls.Add(this.SearchByDescription);
            this.SearchBy.Location = new System.Drawing.Point(99, 95);
            this.SearchBy.Name = "SearchBy";
            this.SearchBy.Size = new System.Drawing.Size(188, 49);
            this.SearchBy.TabIndex = 6;
            this.SearchBy.TabStop = false;
            this.SearchBy.Text = "Search By";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(230, 190);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Reference #";
            // 
            // ReferenceNumber
            // 
            this.ReferenceNumber.Location = new System.Drawing.Point(303, 188);
            this.ReferenceNumber.Name = "ReferenceNumber";
            this.ReferenceNumber.Size = new System.Drawing.Size(100, 20);
            this.ReferenceNumber.TabIndex = 12;
            // 
            // ChargeEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(571, 349);
            this.Controls.Add(this.ReferenceNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SearchBy);
            this.Controls.Add(this.tbDateOfService);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cbChargeItem);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnSubmit);
            this.Controls.Add(this.tbComment);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nQty);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbBannerMRN);
            this.Controls.Add(this.tbBannerAccount);
            this.Controls.Add(this.tbBannerName);
            this.Controls.Add(this.label37);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ChargeEntryForm";
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add Charge";
            this.Load += new System.EventHandler(this.ChargeEntryForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nQty)).EndInit();
            this.SearchBy.ResumeLayout(false);
            this.SearchBy.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox tbBannerMRN;
        private System.Windows.Forms.TextBox tbBannerAccount;
        private System.Windows.Forms.TextBox tbBannerName;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nQty;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbComment;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private MultiColumnCombo.MultiColumnComboBox cbChargeItem;
        private System.Windows.Forms.TextBox tbDateOfService;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.RadioButton SearchByCdm;
        private System.Windows.Forms.RadioButton SearchByDescription;
        private System.Windows.Forms.GroupBox SearchBy;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ReferenceNumber;
    }
}