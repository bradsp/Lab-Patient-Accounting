namespace LabBilling.Forms
{
    partial class HealthPlanMaintenanceForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.includeDeletedCheckBox = new System.Windows.Forms.CheckBox();
            this.healthPlanGrid = new MetroFramework.Controls.MetroGrid();
            this.AddPlanButton = new System.Windows.Forms.Button();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.filterLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.healthPlanGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // includeDeletedCheckBox
            // 
            this.includeDeletedCheckBox.AutoSize = true;
            this.includeDeletedCheckBox.ForeColor = System.Drawing.Color.Black;
            this.includeDeletedCheckBox.Location = new System.Drawing.Point(12, 12);
            this.includeDeletedCheckBox.Name = "includeDeletedCheckBox";
            this.includeDeletedCheckBox.Size = new System.Drawing.Size(106, 15);
            this.includeDeletedCheckBox.TabIndex = 0;
            this.includeDeletedCheckBox.Text = "Include Inactive";
            this.includeDeletedCheckBox.CheckedChanged += new System.EventHandler(this.includeDeletedCheckBox_CheckedChanged);
            // 
            // healthPlanGrid
            // 
            this.healthPlanGrid.AllowUserToOrderColumns = true;
            this.healthPlanGrid.AllowUserToResizeRows = false;
            this.healthPlanGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.healthPlanGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.healthPlanGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.healthPlanGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.healthPlanGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.healthPlanGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.healthPlanGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.healthPlanGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.healthPlanGrid.EnableHeadersVisualStyles = false;
            this.healthPlanGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.healthPlanGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.healthPlanGrid.Location = new System.Drawing.Point(12, 54);
            this.healthPlanGrid.Name = "healthPlanGrid";
            this.healthPlanGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.healthPlanGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.healthPlanGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.healthPlanGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.healthPlanGrid.Size = new System.Drawing.Size(1157, 555);
            this.healthPlanGrid.TabIndex = 1;
            this.healthPlanGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.healthPlanGrid_CellMouseDoubleClick);
            this.healthPlanGrid.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.healthPlanGrid_RowPrePaint);
            // 
            // AddPlanButton
            // 
            this.AddPlanButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.AddPlanButton.Location = new System.Drawing.Point(156, 4);
            this.AddPlanButton.Name = "AddPlanButton";
            this.AddPlanButton.Size = new System.Drawing.Size(75, 23);
            this.AddPlanButton.TabIndex = 2;
            this.AddPlanButton.Text = "Add Plan";
            this.AddPlanButton.UseVisualStyleBackColor = true;
            this.AddPlanButton.Click += new System.EventHandler(this.AddPlanButton_Click);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(369, 7);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(265, 20);
            this.filterTextBox.TabIndex = 3;
            this.filterTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterTextBox_KeyUp);
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(328, 10);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(29, 13);
            this.filterLabel.TabIndex = 4;
            this.filterLabel.Text = "Filter";
            // 
            // HealthPlanMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1181, 621);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.AddPlanButton);
            this.Controls.Add(this.healthPlanGrid);
            this.Controls.Add(this.includeDeletedCheckBox);
            this.Name = "HealthPlanMaintenanceForm";
            this.Text = "Health Plan Maintenance";
            this.Load += new System.EventHandler(this.HealthPlanMaintenanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.healthPlanGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox includeDeletedCheckBox;
        private MetroFramework.Controls.MetroGrid healthPlanGrid;
        private System.Windows.Forms.Button AddPlanButton;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label filterLabel;
    }
}