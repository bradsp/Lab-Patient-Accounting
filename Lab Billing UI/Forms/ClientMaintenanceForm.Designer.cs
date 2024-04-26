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
            this.filterTextBox = new Krypton.Toolkit.KryptonTextBox();
            this.filterLabel = new Krypton.Toolkit.KryptonLabel();
            this.includeDeletedCheckBox = new Krypton.Toolkit.KryptonCheckBox();
            this.newClientButton = new Krypton.Toolkit.KryptonButton();
            this.helpProvider1 = new System.Windows.Forms.HelpProvider();
            ((System.ComponentModel.ISupportInitialize)(this.dgvClients)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvClients
            // 
            this.dgvClients.AllowUserToAddRows = false;
            this.dgvClients.AllowUserToDeleteRows = false;
            this.dgvClients.AllowUserToOrderColumns = true;
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
            this.dgvClients.RowPrePaint += new System.Windows.Forms.DataGridViewRowPrePaintEventHandler(this.dgvClients_RowPrePaint);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(67, 13);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(276, 20);
            this.filterTextBox.TabIndex = 1;
            this.filterTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterTextBox_KeyUp);
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
            this.includeDeletedCheckBox.CheckedChanged += new System.EventHandler(this.includeDeletedCheckBox_CheckedChanged);
            // 
            // newClientButton
            // 
            this.newClientButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.newClientButton.Location = new System.Drawing.Point(803, 6);
            this.newClientButton.Name = "newClientButton";
            this.newClientButton.Size = new System.Drawing.Size(75, 23);
            this.newClientButton.TabIndex = 4;
            this.newClientButton.Text = "New Client";
            this.newClientButton.Click += new System.EventHandler(this.newClientButton_Click);
            // 
            // ClientMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(890, 571);
            this.Controls.Add(this.newClientButton);
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
        private Krypton.Toolkit.KryptonTextBox filterTextBox;
        private Krypton.Toolkit.KryptonLabel filterLabel;
        private Krypton.Toolkit.KryptonCheckBox includeDeletedCheckBox;
        private Krypton.Toolkit.KryptonButton newClientButton;
        private System.Windows.Forms.HelpProvider helpProvider1;
    }
}