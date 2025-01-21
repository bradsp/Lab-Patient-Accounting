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
            tbBannerMRN = new System.Windows.Forms.TextBox();
            tbBannerAccount = new System.Windows.Forms.TextBox();
            tbBannerName = new System.Windows.Forms.TextBox();
            label37 = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            quantityNumericUpDown = new System.Windows.Forms.NumericUpDown();
            label3 = new System.Windows.Forms.Label();
            commentTextBox = new System.Windows.Forms.TextBox();
            btnSubmit = new System.Windows.Forms.Button();
            btnCancel = new System.Windows.Forms.Button();
            tbDateOfService = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            ReferenceNumberTextBox = new System.Windows.Forms.TextBox();
            cdmTextBox = new System.Windows.Forms.TextBox();
            amountTextBox = new System.Windows.Forms.TextBox();
            amountLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)quantityNumericUpDown).BeginInit();
            SuspendLayout();
            // 
            // tbBannerMRN
            // 
            tbBannerMRN.BackColor = System.Drawing.SystemColors.Control;
            tbBannerMRN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tbBannerMRN.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tbBannerMRN.Location = new System.Drawing.Point(183, 29);
            tbBannerMRN.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbBannerMRN.Name = "tbBannerMRN";
            tbBannerMRN.ReadOnly = true;
            tbBannerMRN.Size = new System.Drawing.Size(201, 19);
            tbBannerMRN.TabIndex = 2;
            tbBannerMRN.TabStop = false;
            // 
            // tbBannerAccount
            // 
            tbBannerAccount.BackColor = System.Drawing.SystemColors.Control;
            tbBannerAccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tbBannerAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tbBannerAccount.Location = new System.Drawing.Point(14, 29);
            tbBannerAccount.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbBannerAccount.Name = "tbBannerAccount";
            tbBannerAccount.ReadOnly = true;
            tbBannerAccount.Size = new System.Drawing.Size(162, 22);
            tbBannerAccount.TabIndex = 1;
            tbBannerAccount.TabStop = false;
            // 
            // tbBannerName
            // 
            tbBannerName.BackColor = System.Drawing.SystemColors.Control;
            tbBannerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tbBannerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tbBannerName.Location = new System.Drawing.Point(14, 65);
            tbBannerName.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbBannerName.Name = "tbBannerName";
            tbBannerName.ReadOnly = true;
            tbBannerName.Size = new System.Drawing.Size(638, 19);
            tbBannerName.TabIndex = 5;
            tbBannerName.TabStop = false;
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Location = new System.Drawing.Point(10, 10);
            label37.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label37.Name = "label37";
            label37.Size = new System.Drawing.Size(87, 15);
            label37.TabIndex = 0;
            label37.Text = "Account/MRN:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(34, 110);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(72, 15);
            label1.TabIndex = 6;
            label1.Text = "Charge Item";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(55, 151);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(53, 15);
            label2.TabIndex = 8;
            label2.Text = "Quantity";
            // 
            // quantityNumericUpDown
            // 
            quantityNumericUpDown.Location = new System.Drawing.Point(117, 149);
            quantityNumericUpDown.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            quantityNumericUpDown.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            quantityNumericUpDown.Name = "quantityNumericUpDown";
            quantityNumericUpDown.Size = new System.Drawing.Size(140, 23);
            quantityNumericUpDown.TabIndex = 9;
            quantityNumericUpDown.Value = new decimal(new int[] { 1, 0, 0, 0 });
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(49, 193);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(61, 15);
            label3.TabIndex = 12;
            label3.Text = "Comment";
            // 
            // commentTextBox
            // 
            commentTextBox.Location = new System.Drawing.Point(117, 189);
            commentTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            commentTextBox.Name = "commentTextBox";
            commentTextBox.Size = new System.Drawing.Size(526, 23);
            commentTextBox.TabIndex = 13;
            // 
            // btnSubmit
            // 
            btnSubmit.Location = new System.Drawing.Point(117, 239);
            btnSubmit.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSubmit.Name = "btnSubmit";
            btnSubmit.Size = new System.Drawing.Size(88, 27);
            btnSubmit.TabIndex = 14;
            btnSubmit.Text = "Submit";
            btnSubmit.UseVisualStyleBackColor = true;
            btnSubmit.Click += btnSubmit_Click;
            // 
            // btnCancel
            // 
            btnCancel.Location = new System.Drawing.Point(225, 239);
            btnCancel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnCancel.Name = "btnCancel";
            btnCancel.Size = new System.Drawing.Size(88, 27);
            btnCancel.TabIndex = 15;
            btnCancel.Text = "Cancel";
            btnCancel.UseVisualStyleBackColor = true;
            btnCancel.Click += btnCancel_Click;
            // 
            // tbDateOfService
            // 
            tbDateOfService.BackColor = System.Drawing.SystemColors.Control;
            tbDateOfService.BorderStyle = System.Windows.Forms.BorderStyle.None;
            tbDateOfService.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            tbDateOfService.Location = new System.Drawing.Point(406, 29);
            tbDateOfService.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tbDateOfService.Name = "tbDateOfService";
            tbDateOfService.ReadOnly = true;
            tbDateOfService.Size = new System.Drawing.Size(246, 22);
            tbDateOfService.TabIndex = 4;
            tbDateOfService.TabStop = false;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(402, 10);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(85, 15);
            label4.TabIndex = 3;
            label4.Text = "Date of Service";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(270, 151);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(69, 15);
            label5.TabIndex = 10;
            label5.Text = "Reference #";
            // 
            // ReferenceNumberTextBox
            // 
            ReferenceNumberTextBox.Location = new System.Drawing.Point(355, 149);
            ReferenceNumberTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ReferenceNumberTextBox.Name = "ReferenceNumberTextBox";
            ReferenceNumberTextBox.Size = new System.Drawing.Size(106, 23);
            ReferenceNumberTextBox.TabIndex = 11;
            // 
            // cdmTextBox
            // 
            cdmTextBox.Location = new System.Drawing.Point(115, 106);
            cdmTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cdmTextBox.Name = "cdmTextBox";
            cdmTextBox.Size = new System.Drawing.Size(526, 23);
            cdmTextBox.TabIndex = 7;
            cdmTextBox.KeyUp += cdmTextBox_KeyUp;
            // 
            // amountTextBox
            // 
            amountTextBox.Location = new System.Drawing.Point(525, 151);
            amountTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            amountTextBox.Name = "amountTextBox";
            amountTextBox.ReadOnly = true;
            amountTextBox.Size = new System.Drawing.Size(116, 23);
            amountTextBox.TabIndex = 17;
            // 
            // amountLabel
            // 
            amountLabel.AutoSize = true;
            amountLabel.Location = new System.Drawing.Point(468, 153);
            amountLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            amountLabel.Name = "amountLabel";
            amountLabel.Size = new System.Drawing.Size(51, 15);
            amountLabel.TabIndex = 16;
            amountLabel.Text = "Amount";
            // 
            // ChargeEntryForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(668, 299);
            Controls.Add(amountTextBox);
            Controls.Add(amountLabel);
            Controls.Add(cdmTextBox);
            Controls.Add(ReferenceNumberTextBox);
            Controls.Add(label5);
            Controls.Add(tbDateOfService);
            Controls.Add(label4);
            Controls.Add(btnCancel);
            Controls.Add(btnSubmit);
            Controls.Add(commentTextBox);
            Controls.Add(label3);
            Controls.Add(quantityNumericUpDown);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(tbBannerMRN);
            Controls.Add(tbBannerAccount);
            Controls.Add(tbBannerName);
            Controls.Add(label37);
            ForeColor = System.Drawing.Color.Black;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "ChargeEntryForm";
            ShowInTaskbar = false;
            SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Add Charge";
            Load += ChargeEntryForm_Load;
            ((System.ComponentModel.ISupportInitialize)quantityNumericUpDown).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox tbBannerMRN;
        private System.Windows.Forms.TextBox tbBannerAccount;
        private System.Windows.Forms.TextBox tbBannerName;
        private System.Windows.Forms.Label label37;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown quantityNumericUpDown;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Button btnSubmit;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TextBox tbDateOfService;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox ReferenceNumberTextBox;
        private System.Windows.Forms.TextBox cdmTextBox;
        private System.Windows.Forms.TextBox amountTextBox;
        private System.Windows.Forms.Label amountLabel;
    }
}