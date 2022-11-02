namespace LabBilling.Forms
{
    partial class LogViewerForm
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
            this.logViewDataGrid = new System.Windows.Forms.DataGridView();
            this.statusFilterComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fromDate = new System.Windows.Forms.DateTimePicker();
            this.thruDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.fromTime = new System.Windows.Forms.DateTimePicker();
            this.thruTime = new System.Windows.Forms.DateTimePicker();
            this.userFilterTextBox = new System.Windows.Forms.TextBox();
            this.hostFilterTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.logViewDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // logViewDataGrid
            // 
            this.logViewDataGrid.AllowUserToAddRows = false;
            this.logViewDataGrid.AllowUserToDeleteRows = false;
            this.logViewDataGrid.AllowUserToOrderColumns = true;
            this.logViewDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.logViewDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logViewDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.logViewDataGrid.Location = new System.Drawing.Point(12, 57);
            this.logViewDataGrid.Name = "logViewDataGrid";
            this.logViewDataGrid.ReadOnly = true;
            this.logViewDataGrid.Size = new System.Drawing.Size(982, 435);
            this.logViewDataGrid.TabIndex = 0;
            // 
            // statusFilterComboBox
            // 
            this.statusFilterComboBox.FormattingEnabled = true;
            this.statusFilterComboBox.Items.AddRange(new object[] {
            "Trace",
            "Debug",
            "Warn",
            "Error",
            "Fatal"});
            this.statusFilterComboBox.Location = new System.Drawing.Point(13, 25);
            this.statusFilterComboBox.Name = "statusFilterComboBox";
            this.statusFilterComboBox.Size = new System.Drawing.Size(121, 21);
            this.statusFilterComboBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(37, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Status";
            // 
            // fromDate
            // 
            this.fromDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.fromDate.Location = new System.Drawing.Point(174, 26);
            this.fromDate.Name = "fromDate";
            this.fromDate.Size = new System.Drawing.Size(102, 20);
            this.fromDate.TabIndex = 3;
            // 
            // thruDate
            // 
            this.thruDate.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.thruDate.Location = new System.Drawing.Point(390, 26);
            this.thruDate.Name = "thruDate";
            this.thruDate.Size = new System.Drawing.Size(106, 20);
            this.thruDate.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(171, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(84, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "From Date/Time";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(387, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(83, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Thru Date/Time";
            // 
            // fromTime
            // 
            this.fromTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.fromTime.Location = new System.Drawing.Point(282, 26);
            this.fromTime.Name = "fromTime";
            this.fromTime.Size = new System.Drawing.Size(88, 20);
            this.fromTime.TabIndex = 3;
            // 
            // thruTime
            // 
            this.thruTime.Format = System.Windows.Forms.DateTimePickerFormat.Time;
            this.thruTime.Location = new System.Drawing.Point(502, 26);
            this.thruTime.Name = "thruTime";
            this.thruTime.Size = new System.Drawing.Size(106, 20);
            this.thruTime.TabIndex = 4;
            // 
            // userFilterTextBox
            // 
            this.userFilterTextBox.Location = new System.Drawing.Point(653, 26);
            this.userFilterTextBox.Name = "userFilterTextBox";
            this.userFilterTextBox.Size = new System.Drawing.Size(149, 20);
            this.userFilterTextBox.TabIndex = 6;
            // 
            // hostFilterTextBox
            // 
            this.hostFilterTextBox.Location = new System.Drawing.Point(808, 26);
            this.hostFilterTextBox.Name = "hostFilterTextBox";
            this.hostFilterTextBox.Size = new System.Drawing.Size(149, 20);
            this.hostFilterTextBox.TabIndex = 6;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(650, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "User Filter";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(805, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(54, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Host Filter";
            // 
            // LogViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1006, 504);
            this.Controls.Add(this.hostFilterTextBox);
            this.Controls.Add(this.userFilterTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.thruTime);
            this.Controls.Add(this.thruDate);
            this.Controls.Add(this.fromTime);
            this.Controls.Add(this.fromDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.statusFilterComboBox);
            this.Controls.Add(this.logViewDataGrid);
            this.Name = "LogViewerForm";
            this.Text = "LogViewerForm";
            this.Load += new System.EventHandler(this.LogViewerForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.logViewDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView logViewDataGrid;
        private System.Windows.Forms.ComboBox statusFilterComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DateTimePicker fromDate;
        private System.Windows.Forms.DateTimePicker thruDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DateTimePicker fromTime;
        private System.Windows.Forms.DateTimePicker thruTime;
        private System.Windows.Forms.TextBox userFilterTextBox;
        private System.Windows.Forms.TextBox hostFilterTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
    }
}