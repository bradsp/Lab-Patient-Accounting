
namespace LabBilling.Forms
{
    partial class ChargeMasterMaintenance
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
            this.cdmGrid = new MetroFramework.Controls.MetroGrid();
            this.includeInactiveCheckBox = new System.Windows.Forms.CheckBox();
            this.addCdmButton = new System.Windows.Forms.Button();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.filterLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.cdmGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // cdmGrid
            // 
            this.cdmGrid.AllowUserToOrderColumns = true;
            this.cdmGrid.AllowUserToResizeRows = false;
            this.cdmGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cdmGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cdmGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.cdmGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.cdmGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cdmGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.cdmGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.cdmGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.cdmGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.cdmGrid.EnableHeadersVisualStyles = false;
            this.cdmGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cdmGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.cdmGrid.Location = new System.Drawing.Point(12, 39);
            this.cdmGrid.Name = "cdmGrid";
            this.cdmGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.cdmGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.cdmGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.cdmGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.cdmGrid.Size = new System.Drawing.Size(776, 399);
            this.cdmGrid.TabIndex = 0;
            this.cdmGrid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.cdmGrid_RowPrePaint);
            // 
            // includeInactiveCheckBox
            // 
            this.includeInactiveCheckBox.AutoSize = true;
            this.includeInactiveCheckBox.Location = new System.Drawing.Point(12, 12);
            this.includeInactiveCheckBox.Name = "includeInactiveCheckBox";
            this.includeInactiveCheckBox.Size = new System.Drawing.Size(102, 17);
            this.includeInactiveCheckBox.TabIndex = 1;
            this.includeInactiveCheckBox.Text = "Include Inactive";
            this.includeInactiveCheckBox.UseVisualStyleBackColor = true;
            this.includeInactiveCheckBox.CheckedChanged += new System.EventHandler(this.includeInactiveCheckBox_CheckedChanged);
            // 
            // addCdmButton
            // 
            this.addCdmButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.addCdmButton.Location = new System.Drawing.Point(120, 8);
            this.addCdmButton.Name = "addCdmButton";
            this.addCdmButton.Size = new System.Drawing.Size(75, 23);
            this.addCdmButton.TabIndex = 2;
            this.addCdmButton.Text = "Add New";
            this.addCdmButton.UseVisualStyleBackColor = true;
            this.addCdmButton.Click += new System.EventHandler(this.addCdmButton_Click);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(355, 9);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(234, 20);
            this.filterTextBox.TabIndex = 3;
            this.filterTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterTextBox_KeyUp);
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(314, 12);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(29, 13);
            this.filterLabel.TabIndex = 4;
            this.filterLabel.Text = "Filter";
            // 
            // ChargeMasterMaintenance
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.addCdmButton);
            this.Controls.Add(this.includeInactiveCheckBox);
            this.Controls.Add(this.cdmGrid);
            this.Name = "ChargeMasterMaintenance";
            this.Text = "Charge Master Maintenance";
            this.Load += new System.EventHandler(this.ChargeMasterMaintenance_Load);
            ((System.ComponentModel.ISupportInitialize)(this.cdmGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroGrid cdmGrid;
        private System.Windows.Forms.CheckBox includeInactiveCheckBox;
        private System.Windows.Forms.Button addCdmButton;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label filterLabel;
    }
}