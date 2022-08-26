namespace LabBilling.Forms
{
    partial class WorkListForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.accountGrid = new MetroFramework.Controls.MetroGrid();
            this.ValidateButton = new MetroFramework.Controls.MetroButton();
            this.PostButton = new MetroFramework.Controls.MetroButton();
            this.ClaimFilter = new MetroFramework.Controls.MetroComboBox();
            this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
            this.OutpatientBilling = new MetroFramework.Controls.MetroCheckBox();
            this.progressBar = new MetroFramework.Controls.MetroProgressBar();
            ((System.ComponentModel.ISupportInitialize)(this.accountGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // accountGrid
            // 
            this.accountGrid.AllowUserToResizeRows = false;
            this.accountGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.accountGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.accountGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.accountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.accountGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.accountGrid.EnableHeadersVisualStyles = false;
            this.accountGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.accountGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountGrid.Location = new System.Drawing.Point(12, 68);
            this.accountGrid.Name = "accountGrid";
            this.accountGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.accountGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.accountGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.accountGrid.Size = new System.Drawing.Size(776, 396);
            this.accountGrid.TabIndex = 1;
            this.accountGrid.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.accountGrid_RowHeaderMouseDoubleClick);
            // 
            // ValidateButton
            // 
            this.ValidateButton.Location = new System.Drawing.Point(13, 13);
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(113, 23);
            this.ValidateButton.TabIndex = 2;
            this.ValidateButton.Text = "Validate Accounts";
            this.ValidateButton.UseSelectable = true;
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // PostButton
            // 
            this.PostButton.Location = new System.Drawing.Point(132, 13);
            this.PostButton.Name = "PostButton";
            this.PostButton.Size = new System.Drawing.Size(94, 23);
            this.PostButton.TabIndex = 3;
            this.PostButton.Text = "Submit Claims";
            this.PostButton.UseSelectable = true;
            this.PostButton.Click += new System.EventHandler(this.PostButton_Click);
            // 
            // ClaimFilter
            // 
            this.ClaimFilter.FormattingEnabled = true;
            this.ClaimFilter.ItemHeight = 23;
            this.ClaimFilter.Location = new System.Drawing.Point(281, 7);
            this.ClaimFilter.Name = "ClaimFilter";
            this.ClaimFilter.Size = new System.Drawing.Size(139, 29);
            this.ClaimFilter.TabIndex = 4;
            this.ClaimFilter.UseSelectable = true;
            this.ClaimFilter.SelectionChangeCommitted += new System.EventHandler(this.ClaimFilter_SelectionChangeCommitted);
            // 
            // metroLabel1
            // 
            this.metroLabel1.AutoSize = true;
            this.metroLabel1.Location = new System.Drawing.Point(237, 13);
            this.metroLabel1.Name = "metroLabel1";
            this.metroLabel1.Size = new System.Drawing.Size(38, 19);
            this.metroLabel1.TabIndex = 5;
            this.metroLabel1.Text = "Filter";
            // 
            // OutpatientBilling
            // 
            this.OutpatientBilling.AutoSize = true;
            this.OutpatientBilling.Location = new System.Drawing.Point(638, 17);
            this.OutpatientBilling.Name = "OutpatientBilling";
            this.OutpatientBilling.Size = new System.Drawing.Size(150, 15);
            this.OutpatientBilling.TabIndex = 6;
            this.OutpatientBilling.Text = "Select Outpatient Billing";
            this.OutpatientBilling.UseSelectable = true;
            this.OutpatientBilling.CheckedChanged += new System.EventHandler(this.OutpatientBilling_CheckedChanged);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(12, 48);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(776, 14);
            this.progressBar.TabIndex = 7;
            // 
            // WorkListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 476);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.OutpatientBilling);
            this.Controls.Add(this.metroLabel1);
            this.Controls.Add(this.ClaimFilter);
            this.Controls.Add(this.PostButton);
            this.Controls.Add(this.ValidateButton);
            this.Controls.Add(this.accountGrid);
            this.Name = "WorkListForm";
            this.Text = "WorkListForm";
            this.Load += new System.EventHandler(this.WorkListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accountGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroGrid accountGrid;
        private MetroFramework.Controls.MetroButton ValidateButton;
        private MetroFramework.Controls.MetroButton PostButton;
        private MetroFramework.Controls.MetroComboBox ClaimFilter;
        private MetroFramework.Controls.MetroLabel metroLabel1;
        private MetroFramework.Controls.MetroCheckBox OutpatientBilling;
        private MetroFramework.Controls.MetroProgressBar progressBar;
    }
}