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
            this.tbDateOfService = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.ReferenceNumber = new System.Windows.Forms.TextBox();
            this.cdmTextBox = new System.Windows.Forms.TextBox();
            this.amountTextBox = new System.Windows.Forms.TextBox();
            this.amountLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nQty)).BeginInit();
            this.SuspendLayout();
            // 
            // tbBannerMRN
            // 
            this.tbBannerMRN.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerMRN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerMRN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerMRN.Location = new System.Drawing.Point(157, 25);
            this.tbBannerMRN.Name = "tbBannerMRN";
            this.tbBannerMRN.ReadOnly = true;
            this.tbBannerMRN.Size = new System.Drawing.Size(172, 19);
            this.tbBannerMRN.TabIndex = 2;
            this.tbBannerMRN.TabStop = false;
            // 
            // tbBannerAccount
            // 
            this.tbBannerAccount.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerAccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerAccount.Location = new System.Drawing.Point(12, 25);
            this.tbBannerAccount.Name = "tbBannerAccount";
            this.tbBannerAccount.ReadOnly = true;
            this.tbBannerAccount.Size = new System.Drawing.Size(139, 22);
            this.tbBannerAccount.TabIndex = 1;
            this.tbBannerAccount.TabStop = false;
            // 
            // tbBannerName
            // 
            this.tbBannerName.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerName.Location = new System.Drawing.Point(12, 56);
            this.tbBannerName.Name = "tbBannerName";
            this.tbBannerName.ReadOnly = true;
            this.tbBannerName.Size = new System.Drawing.Size(547, 19);
            this.tbBannerName.TabIndex = 5;
            this.tbBannerName.TabStop = false;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(9, 9);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(80, 13);
            this.label37.TabIndex = 0;
            this.label37.Text = "Account/MRN:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 95);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Charge Item";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(47, 131);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "Quantity";
            // 
            // nQty
            // 
            this.nQty.Location = new System.Drawing.Point(100, 129);
            this.nQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nQty.Name = "nQty";
            this.nQty.Size = new System.Drawing.Size(120, 20);
            this.nQty.TabIndex = 9;
            this.nQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(42, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Comment";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(100, 164);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(451, 20);
            this.tbComment.TabIndex = 13;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(100, 207);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 14;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(193, 207);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 15;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // tbDateOfService
            // 
            this.tbDateOfService.BackColor = System.Drawing.SystemColors.Control;
            this.tbDateOfService.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDateOfService.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDateOfService.Location = new System.Drawing.Point(348, 25);
            this.tbDateOfService.Name = "tbDateOfService";
            this.tbDateOfService.ReadOnly = true;
            this.tbDateOfService.Size = new System.Drawing.Size(211, 22);
            this.tbDateOfService.TabIndex = 4;
            this.tbDateOfService.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(345, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 3;
            this.label4.Text = "Date of Service";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(231, 131);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Reference #";
            // 
            // ReferenceNumber
            // 
            this.ReferenceNumber.Location = new System.Drawing.Point(304, 129);
            this.ReferenceNumber.Name = "ReferenceNumber";
            this.ReferenceNumber.Size = new System.Drawing.Size(91, 20);
            this.ReferenceNumber.TabIndex = 11;
            // 
            // cdmTextBox
            // 
            this.cdmTextBox.Location = new System.Drawing.Point(99, 92);
            this.cdmTextBox.Name = "cdmTextBox";
            this.cdmTextBox.Size = new System.Drawing.Size(451, 20);
            this.cdmTextBox.TabIndex = 7;
            this.cdmTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.cdmTextBox_KeyUp);
            // 
            // amountTextBox
            // 
            this.amountTextBox.Location = new System.Drawing.Point(450, 131);
            this.amountTextBox.Name = "amountTextBox";
            this.amountTextBox.ReadOnly = true;
            this.amountTextBox.Size = new System.Drawing.Size(100, 20);
            this.amountTextBox.TabIndex = 17;
            // 
            // amountLabel
            // 
            this.amountLabel.AutoSize = true;
            this.amountLabel.Location = new System.Drawing.Point(401, 133);
            this.amountLabel.Name = "amountLabel";
            this.amountLabel.Size = new System.Drawing.Size(43, 13);
            this.amountLabel.TabIndex = 16;
            this.amountLabel.Text = "Amount";
            // 
            // ChargeEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(571, 259);
            this.Controls.Add(this.amountTextBox);
            this.Controls.Add(this.amountLabel);
            this.Controls.Add(this.cdmTextBox);
            this.Controls.Add(this.ReferenceNumber);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbDateOfService);
            this.Controls.Add(this.label4);
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
        private System.Windows.Forms.TextBox tbDateOfService;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ReferenceNumber;
        private System.Windows.Forms.TextBox cdmTextBox;
        private System.Windows.Forms.TextBox amountTextBox;
        private System.Windows.Forms.Label amountLabel;
    }
}