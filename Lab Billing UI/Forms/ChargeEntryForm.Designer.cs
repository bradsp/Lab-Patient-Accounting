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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChargeEntryForm));
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
            this.cbChargeItem = new MTGCComboBox();
            this.tbDateOfService = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.nQty)).BeginInit();
            this.SuspendLayout();
            // 
            // tbBannerMRN
            // 
            this.tbBannerMRN.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerMRN.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerMRN.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerMRN.Location = new System.Drawing.Point(233, 37);
            this.tbBannerMRN.Name = "tbBannerMRN";
            this.tbBannerMRN.ReadOnly = true;
            this.tbBannerMRN.Size = new System.Drawing.Size(94, 22);
            this.tbBannerMRN.TabIndex = 30;
            this.tbBannerMRN.TabStop = false;
            // 
            // tbBannerAccount
            // 
            this.tbBannerAccount.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerAccount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerAccount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerAccount.Location = new System.Drawing.Point(99, 37);
            this.tbBannerAccount.Name = "tbBannerAccount";
            this.tbBannerAccount.ReadOnly = true;
            this.tbBannerAccount.Size = new System.Drawing.Size(128, 22);
            this.tbBannerAccount.TabIndex = 29;
            this.tbBannerAccount.TabStop = false;
            // 
            // tbBannerName
            // 
            this.tbBannerName.BackColor = System.Drawing.SystemColors.Control;
            this.tbBannerName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbBannerName.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbBannerName.Location = new System.Drawing.Point(12, 12);
            this.tbBannerName.Name = "tbBannerName";
            this.tbBannerName.ReadOnly = true;
            this.tbBannerName.Size = new System.Drawing.Size(573, 19);
            this.tbBannerName.TabIndex = 28;
            this.tbBannerName.TabStop = false;
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(13, 42);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(80, 13);
            this.label37.TabIndex = 27;
            this.label37.Text = "Account/MRN:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 111);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Charge Item";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(55, 147);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(46, 13);
            this.label2.TabIndex = 33;
            this.label2.Text = "Quantity";
            // 
            // nQty
            // 
            this.nQty.Location = new System.Drawing.Point(108, 145);
            this.nQty.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nQty.Name = "nQty";
            this.nQty.Size = new System.Drawing.Size(120, 20);
            this.nQty.TabIndex = 1;
            this.nQty.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 183);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(51, 13);
            this.label3.TabIndex = 35;
            this.label3.Text = "Comment";
            // 
            // tbComment
            // 
            this.tbComment.Location = new System.Drawing.Point(108, 180);
            this.tbComment.Name = "tbComment";
            this.tbComment.Size = new System.Drawing.Size(451, 20);
            this.tbComment.TabIndex = 2;
            // 
            // btnSubmit
            // 
            this.btnSubmit.Location = new System.Drawing.Point(108, 229);
            this.btnSubmit.Name = "btnSubmit";
            this.btnSubmit.Size = new System.Drawing.Size(75, 23);
            this.btnSubmit.TabIndex = 3;
            this.btnSubmit.Text = "Submit";
            this.btnSubmit.UseVisualStyleBackColor = true;
            this.btnSubmit.Click += new System.EventHandler(this.btnSubmit_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(201, 229);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // cbChargeItem
            // 
            this.cbChargeItem.ArrowBoxColor = System.Drawing.SystemColors.Control;
            this.cbChargeItem.ArrowColor = System.Drawing.Color.Black;
            this.cbChargeItem.BindedControl = ((MTGCComboBox.ControlloAssociato)(resources.GetObject("cbChargeItem.BindedControl")));
            this.cbChargeItem.BorderStyle = MTGCComboBox.TipiBordi.FlatXP;
            this.cbChargeItem.CharacterCasing = System.Windows.Forms.CharacterCasing.Normal;
            this.cbChargeItem.ColumnNum = 1;
            this.cbChargeItem.ColumnWidth = "121";
            this.cbChargeItem.DisabledArrowBoxColor = System.Drawing.SystemColors.Control;
            this.cbChargeItem.DisabledArrowColor = System.Drawing.Color.LightGray;
            this.cbChargeItem.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.cbChargeItem.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.cbChargeItem.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.cbChargeItem.DisplayMember = "Text";
            this.cbChargeItem.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbChargeItem.DropDownArrowBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(169)))), ((int)(((byte)(223)))));
            this.cbChargeItem.DropDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(193)))), ((int)(((byte)(210)))), ((int)(((byte)(238)))));
            this.cbChargeItem.DropDownForeColor = System.Drawing.Color.Black;
            this.cbChargeItem.DropDownStyle = MTGCComboBox.CustomDropDownStyle.DropDown;
            this.cbChargeItem.GridLineColor = System.Drawing.Color.LightGray;
            this.cbChargeItem.GridLineHorizontal = false;
            this.cbChargeItem.GridLineVertical = false;
            this.cbChargeItem.HighlightBorderColor = System.Drawing.Color.Blue;
            this.cbChargeItem.HighlightBorderOnMouseEvents = true;
            this.cbChargeItem.LoadingType = MTGCComboBox.CaricamentoCombo.ComboBoxItem;
            this.cbChargeItem.Location = new System.Drawing.Point(108, 107);
            this.cbChargeItem.ManagingFastMouseMoving = true;
            this.cbChargeItem.ManagingFastMouseMovingInterval = 30;
            this.cbChargeItem.Name = "cbChargeItem";
            this.cbChargeItem.NormalBorderColor = System.Drawing.Color.Black;
            this.cbChargeItem.SelectedItem = null;
            this.cbChargeItem.SelectedValue = null;
            this.cbChargeItem.Size = new System.Drawing.Size(273, 21);
            this.cbChargeItem.TabIndex = 0;
            // 
            // tbDateOfService
            // 
            this.tbDateOfService.BackColor = System.Drawing.SystemColors.Control;
            this.tbDateOfService.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.tbDateOfService.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbDateOfService.Location = new System.Drawing.Point(457, 37);
            this.tbDateOfService.Name = "tbDateOfService";
            this.tbDateOfService.ReadOnly = true;
            this.tbDateOfService.Size = new System.Drawing.Size(128, 22);
            this.tbDateOfService.TabIndex = 41;
            this.tbDateOfService.TabStop = false;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(371, 42);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 13);
            this.label4.TabIndex = 40;
            this.label4.Text = "Date of Service";
            // 
            // ChargeEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(597, 264);
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
            this.Name = "ChargeEntryForm";
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
        private MTGCComboBox cbChargeItem;
        private System.Windows.Forms.TextBox tbDateOfService;
        private System.Windows.Forms.Label label4;
    }
}