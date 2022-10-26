namespace LabBilling.Forms
{
    partial class PaymentAdjustmentEntryForm
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
            this.fromTextBox = new System.Windows.Forms.TextBox();
            this.fromLabel = new System.Windows.Forms.Label();
            this.dateReceivedTextBox = new System.Windows.Forms.MaskedTextBox();
            this.checkDateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.refundCheckBox = new System.Windows.Forms.CheckBox();
            this.writeOffDateTextBox = new System.Windows.Forms.MaskedTextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.writeOffCodeComboBox = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.commentTextBox = new System.Windows.Forms.TextBox();
            this.postButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.checkNoTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.insuranceComboBox = new System.Windows.Forms.ComboBox();
            this.paymentAmtTextBox = new LabBilling.UserControls.CurrencyTextBox();
            this.writeOffAmtTextBox = new LabBilling.UserControls.CurrencyTextBox();
            this.contractualAmtTextBox = new LabBilling.UserControls.CurrencyTextBox();
            this.SuspendLayout();
            // 
            // fromTextBox
            // 
            this.fromTextBox.Location = new System.Drawing.Point(132, 24);
            this.fromTextBox.Name = "fromTextBox";
            this.fromTextBox.Size = new System.Drawing.Size(253, 20);
            this.fromTextBox.TabIndex = 1;
            // 
            // fromLabel
            // 
            this.fromLabel.AutoSize = true;
            this.fromLabel.Location = new System.Drawing.Point(89, 27);
            this.fromLabel.Name = "fromLabel";
            this.fromLabel.Size = new System.Drawing.Size(30, 13);
            this.fromLabel.TabIndex = 0;
            this.fromLabel.Text = "From";
            // 
            // dateReceivedTextBox
            // 
            this.dateReceivedTextBox.Location = new System.Drawing.Point(132, 50);
            this.dateReceivedTextBox.Mask = "00/00/0000";
            this.dateReceivedTextBox.Name = "dateReceivedTextBox";
            this.dateReceivedTextBox.Size = new System.Drawing.Size(144, 20);
            this.dateReceivedTextBox.TabIndex = 3;
            this.dateReceivedTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // checkDateTextBox
            // 
            this.checkDateTextBox.Location = new System.Drawing.Point(132, 76);
            this.checkDateTextBox.Mask = "00/00/0000";
            this.checkDateTextBox.Name = "checkDateTextBox";
            this.checkDateTextBox.Size = new System.Drawing.Size(144, 20);
            this.checkDateTextBox.TabIndex = 5;
            this.checkDateTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 53);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Date Received";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Check Date";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(65, 130);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Insurance";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(32, 157);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(87, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Payment Amount";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 183);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(100, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Contractual Amount";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 209);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(88, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Write Off Amount";
            // 
            // refundCheckBox
            // 
            this.refundCheckBox.AutoSize = true;
            this.refundCheckBox.Location = new System.Drawing.Point(238, 156);
            this.refundCheckBox.Name = "refundCheckBox";
            this.refundCheckBox.Size = new System.Drawing.Size(116, 17);
            this.refundCheckBox.TabIndex = 12;
            this.refundCheckBox.Text = "Payment Is Refund";
            this.refundCheckBox.UseVisualStyleBackColor = true;
            // 
            // writeOffDateTextBox
            // 
            this.writeOffDateTextBox.Location = new System.Drawing.Point(132, 232);
            this.writeOffDateTextBox.Mask = "00/00/0000";
            this.writeOffDateTextBox.Name = "writeOffDateTextBox";
            this.writeOffDateTextBox.Size = new System.Drawing.Size(100, 20);
            this.writeOffDateTextBox.TabIndex = 18;
            this.writeOffDateTextBox.ValidatingType = typeof(System.DateTime);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 235);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 13);
            this.label7.TabIndex = 17;
            this.label7.Text = "Write Off Date";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(42, 265);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 19;
            this.label8.Text = "Write Off Code";
            // 
            // writeOffCodeComboBox
            // 
            this.writeOffCodeComboBox.FormattingEnabled = true;
            this.writeOffCodeComboBox.Location = new System.Drawing.Point(132, 262);
            this.writeOffCodeComboBox.Name = "writeOffCodeComboBox";
            this.writeOffCodeComboBox.Size = new System.Drawing.Size(253, 21);
            this.writeOffCodeComboBox.TabIndex = 20;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(19, 303);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(51, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Comment";
            // 
            // commentTextBox
            // 
            this.commentTextBox.Location = new System.Drawing.Point(22, 319);
            this.commentTextBox.Multiline = true;
            this.commentTextBox.Name = "commentTextBox";
            this.commentTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.commentTextBox.Size = new System.Drawing.Size(363, 58);
            this.commentTextBox.TabIndex = 22;
            // 
            // postButton
            // 
            this.postButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.postButton.Location = new System.Drawing.Point(108, 403);
            this.postButton.Name = "postButton";
            this.postButton.Size = new System.Drawing.Size(82, 35);
            this.postButton.TabIndex = 23;
            this.postButton.Text = "Post";
            this.postButton.UseVisualStyleBackColor = true;
            this.postButton.Click += new System.EventHandler(this.postButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(218, 403);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 35);
            this.cancelButton.TabIndex = 24;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // checkNoTextBox
            // 
            this.checkNoTextBox.Location = new System.Drawing.Point(132, 102);
            this.checkNoTextBox.Name = "checkNoTextBox";
            this.checkNoTextBox.Size = new System.Drawing.Size(144, 20);
            this.checkNoTextBox.TabIndex = 7;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(64, 105);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 13);
            this.label10.TabIndex = 6;
            this.label10.Text = "Check No";
            // 
            // insuranceComboBox
            // 
            this.insuranceComboBox.BackColor = System.Drawing.SystemColors.Window;
            this.insuranceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.insuranceComboBox.FormattingEnabled = true;
            this.insuranceComboBox.Location = new System.Drawing.Point(132, 126);
            this.insuranceComboBox.Name = "insuranceComboBox";
            this.insuranceComboBox.Size = new System.Drawing.Size(253, 21);
            this.insuranceComboBox.TabIndex = 9;
            // 
            // paymentAmtTextBox
            // 
            this.paymentAmtTextBox.DollarValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.paymentAmtTextBox.Location = new System.Drawing.Point(132, 153);
            this.paymentAmtTextBox.Name = "paymentAmtTextBox";
            this.paymentAmtTextBox.Size = new System.Drawing.Size(100, 20);
            this.paymentAmtTextBox.TabIndex = 11;
            this.paymentAmtTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // writeOffAmtTextBox
            // 
            this.writeOffAmtTextBox.DollarValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.writeOffAmtTextBox.Location = new System.Drawing.Point(132, 206);
            this.writeOffAmtTextBox.Name = "writeOffAmtTextBox";
            this.writeOffAmtTextBox.Size = new System.Drawing.Size(100, 20);
            this.writeOffAmtTextBox.TabIndex = 16;
            this.writeOffAmtTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // contractualAmtTextBox
            // 
            this.contractualAmtTextBox.DollarValue = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.contractualAmtTextBox.Location = new System.Drawing.Point(132, 180);
            this.contractualAmtTextBox.Name = "contractualAmtTextBox";
            this.contractualAmtTextBox.Size = new System.Drawing.Size(100, 20);
            this.contractualAmtTextBox.TabIndex = 14;
            this.contractualAmtTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // PaymentAdjustmentEntryForm
            // 
            this.AcceptButton = this.postButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(424, 478);
            this.ControlBox = false;
            this.Controls.Add(this.insuranceComboBox);
            this.Controls.Add(this.paymentAmtTextBox);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.postButton);
            this.Controls.Add(this.commentTextBox);
            this.Controls.Add(this.writeOffCodeComboBox);
            this.Controls.Add(this.refundCheckBox);
            this.Controls.Add(this.writeOffAmtTextBox);
            this.Controls.Add(this.contractualAmtTextBox);
            this.Controls.Add(this.checkDateTextBox);
            this.Controls.Add(this.writeOffDateTextBox);
            this.Controls.Add(this.dateReceivedTextBox);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.fromLabel);
            this.Controls.Add(this.checkNoTextBox);
            this.Controls.Add(this.fromTextBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "PaymentAdjustmentEntryForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Enter Payment/Adjustment";
            this.Load += new System.EventHandler(this.PaymentAdjustmentEntryForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox fromTextBox;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.MaskedTextBox dateReceivedTextBox;
        private System.Windows.Forms.MaskedTextBox checkDateTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private UserControls.CurrencyTextBox contractualAmtTextBox;
        private System.Windows.Forms.Label label6;
        private UserControls.CurrencyTextBox writeOffAmtTextBox;
        private System.Windows.Forms.CheckBox refundCheckBox;
        private System.Windows.Forms.MaskedTextBox writeOffDateTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox writeOffCodeComboBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox commentTextBox;
        private System.Windows.Forms.Button postButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.TextBox checkNoTextBox;
        private System.Windows.Forms.Label label10;
        private UserControls.CurrencyTextBox paymentAmtTextBox;
        private System.Windows.Forms.ComboBox insuranceComboBox;
    }
}