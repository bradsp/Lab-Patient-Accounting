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
            components = new System.ComponentModel.Container();
            fromTextBox = new System.Windows.Forms.TextBox();
            fromLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label6 = new System.Windows.Forms.Label();
            refundCheckBox = new System.Windows.Forms.CheckBox();
            label7 = new System.Windows.Forms.Label();
            label8 = new System.Windows.Forms.Label();
            writeOffCodeComboBox = new System.Windows.Forms.ComboBox();
            label9 = new System.Windows.Forms.Label();
            commentTextBox = new System.Windows.Forms.TextBox();
            postButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            checkNoTextBox = new System.Windows.Forms.TextBox();
            label10 = new System.Windows.Forms.Label();
            insuranceComboBox = new System.Windows.Forms.ComboBox();
            errorProvider1 = new System.Windows.Forms.ErrorProvider(components);
            writeOffDateTextBox = new UserControls.DateTextBox();
            checkDateTextBox = new UserControls.DateTextBox();
            dateReceivedTextBox = new UserControls.DateTextBox();
            paymentAmtTextBox = new UserControls.CurrencyTextBox();
            writeOffAmtTextBox = new UserControls.CurrencyTextBox();
            contractualAmtTextBox = new UserControls.CurrencyTextBox();
            writeOffCodeTextBox = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)errorProvider1).BeginInit();
            SuspendLayout();
            // 
            // fromTextBox
            // 
            fromTextBox.Location = new System.Drawing.Point(154, 28);
            fromTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            fromTextBox.Name = "fromTextBox";
            fromTextBox.Size = new System.Drawing.Size(294, 23);
            fromTextBox.TabIndex = 1;
            // 
            // fromLabel
            // 
            fromLabel.AutoSize = true;
            fromLabel.Location = new System.Drawing.Point(104, 31);
            fromLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            fromLabel.Name = "fromLabel";
            fromLabel.Size = new System.Drawing.Size(35, 15);
            fromLabel.TabIndex = 0;
            fromLabel.Text = "From";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(47, 61);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(81, 15);
            label1.TabIndex = 2;
            label1.Text = "Date Received";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(64, 91);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 15);
            label2.TabIndex = 4;
            label2.Text = "Check Date";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(76, 150);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(58, 15);
            label3.TabIndex = 8;
            label3.Text = "Insurance";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(37, 181);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(101, 15);
            label4.TabIndex = 10;
            label4.Text = "Payment Amount";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(22, 211);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(116, 15);
            label5.TabIndex = 13;
            label5.Text = "Contractual Amount";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(36, 241);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(102, 15);
            label6.TabIndex = 15;
            label6.Text = "Write Off Amount";
            // 
            // refundCheckBox
            // 
            refundCheckBox.AutoSize = true;
            refundCheckBox.Location = new System.Drawing.Point(278, 180);
            refundCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            refundCheckBox.Name = "refundCheckBox";
            refundCheckBox.Size = new System.Drawing.Size(125, 19);
            refundCheckBox.TabIndex = 12;
            refundCheckBox.Text = "Payment Is Refund";
            refundCheckBox.UseVisualStyleBackColor = true;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new System.Drawing.Point(51, 271);
            label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new System.Drawing.Size(82, 15);
            label7.TabIndex = 17;
            label7.Text = "Write Off Date";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new System.Drawing.Point(49, 306);
            label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(86, 15);
            label8.TabIndex = 19;
            label8.Text = "Write Off Code";
            // 
            // writeOffCodeComboBox
            // 
            writeOffCodeComboBox.FormattingEnabled = true;
            writeOffCodeComboBox.Location = new System.Drawing.Point(50, 335);
            writeOffCodeComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            writeOffCodeComboBox.Name = "writeOffCodeComboBox";
            writeOffCodeComboBox.Size = new System.Drawing.Size(398, 23);
            writeOffCodeComboBox.TabIndex = 21;
            writeOffCodeComboBox.SelectedValueChanged += writeOffCodeComboBox_SelectedValueChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new System.Drawing.Point(22, 375);
            label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(61, 15);
            label9.TabIndex = 22;
            label9.Text = "Comment";
            // 
            // commentTextBox
            // 
            commentTextBox.Location = new System.Drawing.Point(26, 393);
            commentTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            commentTextBox.MaxLength = 100;
            commentTextBox.Multiline = true;
            commentTextBox.Name = "commentTextBox";
            commentTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            commentTextBox.Size = new System.Drawing.Size(423, 66);
            commentTextBox.TabIndex = 23;
            // 
            // postButton
            // 
            postButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            postButton.Location = new System.Drawing.Point(126, 490);
            postButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            postButton.Name = "postButton";
            postButton.Size = new System.Drawing.Size(96, 40);
            postButton.TabIndex = 24;
            postButton.Text = "Post";
            postButton.UseVisualStyleBackColor = true;
            postButton.Click += postButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cancelButton.Location = new System.Drawing.Point(254, 490);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 40);
            cancelButton.TabIndex = 25;
            cancelButton.Text = "Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // checkNoTextBox
            // 
            checkNoTextBox.Location = new System.Drawing.Point(154, 115);
            checkNoTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkNoTextBox.Name = "checkNoTextBox";
            checkNoTextBox.Size = new System.Drawing.Size(116, 23);
            checkNoTextBox.TabIndex = 7;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new System.Drawing.Point(75, 121);
            label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new System.Drawing.Size(59, 15);
            label10.TabIndex = 6;
            label10.Text = "Check No";
            // 
            // insuranceComboBox
            // 
            insuranceComboBox.BackColor = System.Drawing.SystemColors.Window;
            insuranceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            insuranceComboBox.FormattingEnabled = true;
            insuranceComboBox.Location = new System.Drawing.Point(154, 145);
            insuranceComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            insuranceComboBox.Name = "insuranceComboBox";
            insuranceComboBox.Size = new System.Drawing.Size(294, 23);
            insuranceComboBox.TabIndex = 9;
            // 
            // errorProvider1
            // 
            errorProvider1.ContainerControl = this;
            // 
            // writeOffDateTextBox
            // 
            writeOffDateTextBox.DateValue = new System.DateTime(0L);
            writeOffDateTextBox.Location = new System.Drawing.Point(154, 268);
            writeOffDateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            writeOffDateTextBox.Name = "writeOffDateTextBox";
            writeOffDateTextBox.Size = new System.Drawing.Size(116, 23);
            writeOffDateTextBox.TabIndex = 18;
            writeOffDateTextBox.Validated += writeOffDateTextBox_Validated;
            // 
            // checkDateTextBox
            // 
            checkDateTextBox.DateValue = new System.DateTime(0L);
            checkDateTextBox.Location = new System.Drawing.Point(154, 88);
            checkDateTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            checkDateTextBox.Name = "checkDateTextBox";
            checkDateTextBox.Size = new System.Drawing.Size(116, 23);
            checkDateTextBox.TabIndex = 5;
            checkDateTextBox.Validated += checkDateTextBox_Validated;
            // 
            // dateReceivedTextBox
            // 
            dateReceivedTextBox.DateValue = new System.DateTime(0L);
            dateReceivedTextBox.Location = new System.Drawing.Point(154, 58);
            dateReceivedTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dateReceivedTextBox.Name = "dateReceivedTextBox";
            dateReceivedTextBox.Size = new System.Drawing.Size(116, 23);
            dateReceivedTextBox.TabIndex = 3;
            dateReceivedTextBox.Validated += dateReceivedTextBox_Validated;
            // 
            // paymentAmtTextBox
            // 
            paymentAmtTextBox.DollarValue = new decimal(new int[] { 0, 0, 0, 0 });
            paymentAmtTextBox.Location = new System.Drawing.Point(154, 177);
            paymentAmtTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            paymentAmtTextBox.Name = "paymentAmtTextBox";
            paymentAmtTextBox.Size = new System.Drawing.Size(116, 23);
            paymentAmtTextBox.TabIndex = 11;
            paymentAmtTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // writeOffAmtTextBox
            // 
            writeOffAmtTextBox.DollarValue = new decimal(new int[] { 0, 0, 0, 0 });
            writeOffAmtTextBox.Location = new System.Drawing.Point(154, 238);
            writeOffAmtTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            writeOffAmtTextBox.Name = "writeOffAmtTextBox";
            writeOffAmtTextBox.Size = new System.Drawing.Size(116, 23);
            writeOffAmtTextBox.TabIndex = 16;
            writeOffAmtTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            writeOffAmtTextBox.Validated += writeOffAmtTextBox_Validated;
            // 
            // contractualAmtTextBox
            // 
            contractualAmtTextBox.DollarValue = new decimal(new int[] { 0, 0, 0, 0 });
            contractualAmtTextBox.Location = new System.Drawing.Point(154, 208);
            contractualAmtTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            contractualAmtTextBox.Name = "contractualAmtTextBox";
            contractualAmtTextBox.Size = new System.Drawing.Size(116, 23);
            contractualAmtTextBox.TabIndex = 14;
            contractualAmtTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // writeOffCodeTextBox
            // 
            writeOffCodeTextBox.Location = new System.Drawing.Point(154, 302);
            writeOffCodeTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            writeOffCodeTextBox.Name = "writeOffCodeTextBox";
            writeOffCodeTextBox.Size = new System.Drawing.Size(116, 23);
            writeOffCodeTextBox.TabIndex = 20;
            writeOffCodeTextBox.TextChanged += writeOffCodeTextBox_TextChanged;
            // 
            // PaymentAdjustmentEntryForm
            // 
            AcceptButton = postButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(495, 552);
            ControlBox = false;
            Controls.Add(writeOffDateTextBox);
            Controls.Add(checkDateTextBox);
            Controls.Add(dateReceivedTextBox);
            Controls.Add(insuranceComboBox);
            Controls.Add(paymentAmtTextBox);
            Controls.Add(cancelButton);
            Controls.Add(postButton);
            Controls.Add(commentTextBox);
            Controls.Add(writeOffCodeComboBox);
            Controls.Add(refundCheckBox);
            Controls.Add(writeOffAmtTextBox);
            Controls.Add(contractualAmtTextBox);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label10);
            Controls.Add(label2);
            Controls.Add(label5);
            Controls.Add(label1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(fromLabel);
            Controls.Add(writeOffCodeTextBox);
            Controls.Add(checkNoTextBox);
            Controls.Add(fromTextBox);
            ForeColor = System.Drawing.Color.Black;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "PaymentAdjustmentEntryForm";
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Enter Payment/Adjustment";
            Load += PaymentAdjustmentEntryForm_Load;
            ((System.ComponentModel.ISupportInitialize)errorProvider1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TextBox fromTextBox;
        private System.Windows.Forms.Label fromLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private UserControls.CurrencyTextBox contractualAmtTextBox;
        private System.Windows.Forms.Label label6;
        private UserControls.CurrencyTextBox writeOffAmtTextBox;
        private System.Windows.Forms.CheckBox refundCheckBox;
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
        private UserControls.DateTextBox dateReceivedTextBox;
        private UserControls.DateTextBox checkDateTextBox;
        private UserControls.DateTextBox writeOffDateTextBox;
        private System.Windows.Forms.ErrorProvider errorProvider1;
        private System.Windows.Forms.TextBox writeOffCodeTextBox;
    }
}