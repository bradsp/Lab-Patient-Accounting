namespace LabBilling.Forms
{
    partial class ClientMaintenanceForm
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
            this.dgvClients = new System.Windows.Forms.DataGridView();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.filterLabel = new System.Windows.Forms.Label();
            this.includeDeletedCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToResizeRows = false;
            this.dgvClients.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvClients.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            this.dgvClients.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvClients.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvClients.Location = new System.Drawing.Point(12, 39);
            this.dgvClients.MultiSelect = false;
            this.dgvClients.Name = "dgvClients";
            this.dgvClients.ReadOnly = true;
            this.dgvClients.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvClients.Size = new System.Drawing.Size(866, 518);
            this.dgvClients.TabIndex = 0;
            this.dgvClients.VirtualMode = true;
            this.dgvClients.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvClients_CellMouseDoubleClick);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(67, 13);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(276, 20);
            this.filterTextBox.TabIndex = 1;
            // 
            // filterLabel
            // 
            this.filterLabel.AutoSize = true;
            this.filterLabel.Location = new System.Drawing.Point(32, 16);
            this.filterLabel.Name = "filterLabel";
            this.filterLabel.Size = new System.Drawing.Size(29, 13);
            this.filterLabel.TabIndex = 2;
            this.filterLabel.Text = "Filter";
            // 
            // includeDeletedCheckBox
            // 
            this.includeDeletedCheckBox.AutoSize = true;
            this.includeDeletedCheckBox.Location = new System.Drawing.Point(358, 15);
            this.includeDeletedCheckBox.Name = "includeDeletedCheckBox";
            this.includeDeletedCheckBox.Size = new System.Drawing.Size(101, 17);
            this.includeDeletedCheckBox.TabIndex = 3;
            this.includeDeletedCheckBox.Text = "Include Deleted";
            this.includeDeletedCheckBox.UseVisualStyleBackColor = true;
            this.includeDeletedCheckBox.CheckedChanged += new System.EventHandler(this.includeDeletedCheckBox_CheckedChanged);
            // 
            // ClientMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(890, 571);
            this.Controls.Add(this.includeDeletedCheckBox);
            this.Controls.Add(this.filterLabel);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.dgvClients);
            this.Name = "ClientMaintenanceForm";
            this.ShowInTaskbar = false;
            this.Text = "Clients";
            this.Load += new System.EventHandler(this.Clients_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dgvClients;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label filterLabel;
        private System.Windows.Forms.CheckBox includeDeletedCheckBox;
    }
}