namespace LabBilling.UserControls
{
    partial class ChargeTreeListView
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.chargeGrid = new System.Windows.Forms.DataGridView();
            this.chargeContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.creditChargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.moveChargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.chargeDetailGrid = new System.Windows.Forms.DataGridView();
            this.chargeDetailContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addModifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeModifierToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addChargeButton = new MetroFramework.Controls.MetroButton();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.chargeStatus1 = new System.Windows.Forms.RichTextBox();
            this.chargeStatus2 = new System.Windows.Forms.RichTextBox();
            this.chargesLabel = new MetroFramework.Controls.MetroLabel();
            this.chargeDetailsLabel = new MetroFramework.Controls.MetroLabel();
            this.showCreditsCheckbox = new MetroFramework.Controls.MetroCheckBox();
            this.showThirdPartyRadioButton = new MetroFramework.Controls.MetroRadioButton();
            this.showClientRadioButton = new MetroFramework.Controls.MetroRadioButton();
            this.showAllRadioButton = new MetroFramework.Controls.MetroRadioButton();
            ((System.ComponentModel.ISupportInitialize)(this.chargeGrid)).BeginInit();
            this.chargeContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chargeDetailGrid)).BeginInit();
            this.chargeDetailContextMenu.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // chargeGrid
            // 
            this.chargeGrid.AllowUserToAddRows = false;
            this.chargeGrid.AllowUserToDeleteRows = false;
            this.chargeGrid.AllowUserToResizeRows = false;
            this.chargeGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chargeGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chargeGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.chargeGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.chargeGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.chargeGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chargeGrid.ContextMenuStrip = this.chargeContextMenu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.chargeGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.chargeGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chargeGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.chargeGrid.EnableHeadersVisualStyles = false;
            this.chargeGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chargeGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chargeGrid.Location = new System.Drawing.Point(3, 43);
            this.chargeGrid.Name = "chargeGrid";
            this.chargeGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.chargeGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.chargeGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.chargeGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.chargeGrid.Size = new System.Drawing.Size(940, 139);
            this.chargeGrid.TabIndex = 0;
            this.chargeGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.chargeGrid_CellDoubleClick);
            this.chargeGrid.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.chargeGrid_CellMouseClick);
            // 
            // chargeContextMenu
            // 
            this.chargeContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.creditChargeToolStripMenuItem,
            this.moveChargeToolStripMenuItem});
            this.chargeContextMenu.Name = "chargeContextMenu";
            this.chargeContextMenu.Size = new System.Drawing.Size(148, 48);
            // 
            // creditChargeToolStripMenuItem
            // 
            this.creditChargeToolStripMenuItem.Name = "creditChargeToolStripMenuItem";
            this.creditChargeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.creditChargeToolStripMenuItem.Text = "Credit Charge";
            this.creditChargeToolStripMenuItem.Click += new System.EventHandler(this.creditChargeToolStripMenuItem_Click);
            // 
            // moveChargeToolStripMenuItem
            // 
            this.moveChargeToolStripMenuItem.Name = "moveChargeToolStripMenuItem";
            this.moveChargeToolStripMenuItem.Size = new System.Drawing.Size(147, 22);
            this.moveChargeToolStripMenuItem.Text = "Move Charge";
            this.moveChargeToolStripMenuItem.Click += new System.EventHandler(this.moveChargeToolStripMenuItem_Click);
            // 
            // chargeDetailGrid
            // 
            this.chargeDetailGrid.AllowUserToAddRows = false;
            this.chargeDetailGrid.AllowUserToDeleteRows = false;
            this.chargeDetailGrid.AllowUserToResizeRows = false;
            this.chargeDetailGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chargeDetailGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.chargeDetailGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.chargeDetailGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.chargeDetailGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.chargeDetailGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chargeDetailGrid.ContextMenuStrip = this.chargeDetailContextMenu;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.chargeDetailGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.chargeDetailGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chargeDetailGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.chargeDetailGrid.EnableHeadersVisualStyles = false;
            this.chargeDetailGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.chargeDetailGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.chargeDetailGrid.Location = new System.Drawing.Point(3, 228);
            this.chargeDetailGrid.Name = "chargeDetailGrid";
            this.chargeDetailGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.chargeDetailGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.chargeDetailGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.chargeDetailGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.chargeDetailGrid.Size = new System.Drawing.Size(940, 282);
            this.chargeDetailGrid.TabIndex = 0;
            this.chargeDetailGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.chargeDetailGrid_CellDoubleClick);
            this.chargeDetailGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.chargeDetailGrid_CellFormatting);
            this.chargeDetailGrid.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.chargeDetailGrid_CellPainting);
            // 
            // chargeDetailContextMenu
            // 
            this.chargeDetailContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addModifierToolStripMenuItem,
            this.removeModifierToolStripMenuItem});
            this.chargeDetailContextMenu.Name = "chargeDetailContextMenu";
            this.chargeDetailContextMenu.Size = new System.Drawing.Size(181, 70);
            // 
            // addModifierToolStripMenuItem
            // 
            this.addModifierToolStripMenuItem.Name = "addModifierToolStripMenuItem";
            this.addModifierToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.addModifierToolStripMenuItem.Text = "Add Modifier";
            // 
            // removeModifierToolStripMenuItem
            // 
            this.removeModifierToolStripMenuItem.Name = "removeModifierToolStripMenuItem";
            this.removeModifierToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.removeModifierToolStripMenuItem.Text = "Remove Modifier";
            this.removeModifierToolStripMenuItem.Click += new System.EventHandler(this.removeModifierToolStripMenuItem_Click);
            // 
            // addChargeButton
            // 
            this.addChargeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.addChargeButton.Location = new System.Drawing.Point(1057, 16);
            this.addChargeButton.Name = "addChargeButton";
            this.addChargeButton.Size = new System.Drawing.Size(75, 23);
            this.addChargeButton.TabIndex = 1;
            this.addChargeButton.Text = "Add Charge";
            this.addChargeButton.UseSelectable = true;
            this.addChargeButton.Click += new System.EventHandler(this.addChargeButton_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 84.38894F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 15.61106F));
            this.tableLayoutPanel1.Controls.Add(this.chargeGrid, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.chargeStatus1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.chargeDetailGrid, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.chargeStatus2, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.chargesLabel, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.chargeDetailsLabel, 0, 2);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(14, 54);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.55704F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 40F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 66.44296F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(1121, 513);
            this.tableLayoutPanel1.TabIndex = 2;
            // 
            // chargeStatus1
            // 
            this.chargeStatus1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chargeStatus1.Location = new System.Drawing.Point(949, 43);
            this.chargeStatus1.MaxLength = 32767;
            this.chargeStatus1.Name = "chargeStatus1";
            this.chargeStatus1.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.chargeStatus1.Size = new System.Drawing.Size(169, 139);
            this.chargeStatus1.TabIndex = 2;
            this.chargeStatus1.Text = "";
            // 
            // chargeStatus2
            // 
            this.chargeStatus2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chargeStatus2.Location = new System.Drawing.Point(949, 228);
            this.chargeStatus2.MaxLength = 32767;
            this.chargeStatus2.Name = "chargeStatus2";
            this.chargeStatus2.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.None;
            this.chargeStatus2.Size = new System.Drawing.Size(169, 282);
            this.chargeStatus2.TabIndex = 3;
            this.chargeStatus2.Text = "";
            // 
            // chargesLabel
            // 
            this.chargesLabel.AutoSize = true;
            this.chargesLabel.Location = new System.Drawing.Point(3, 0);
            this.chargesLabel.Name = "chargesLabel";
            this.chargesLabel.Size = new System.Drawing.Size(50, 19);
            this.chargesLabel.TabIndex = 4;
            this.chargesLabel.Text = "Orders";
            // 
            // chargeDetailsLabel
            // 
            this.chargeDetailsLabel.AutoSize = true;
            this.chargeDetailsLabel.Location = new System.Drawing.Point(3, 185);
            this.chargeDetailsLabel.Name = "chargeDetailsLabel";
            this.chargeDetailsLabel.Size = new System.Drawing.Size(57, 19);
            this.chargeDetailsLabel.TabIndex = 4;
            this.chargeDetailsLabel.Text = "Charges";
            // 
            // showCreditsCheckbox
            // 
            this.showCreditsCheckbox.AutoSize = true;
            this.showCreditsCheckbox.Location = new System.Drawing.Point(14, 16);
            this.showCreditsCheckbox.Name = "showCreditsCheckbox";
            this.showCreditsCheckbox.Size = new System.Drawing.Size(92, 15);
            this.showCreditsCheckbox.TabIndex = 4;
            this.showCreditsCheckbox.Text = "Show Credits";
            this.showCreditsCheckbox.UseSelectable = true;
            // 
            // showThirdPartyRadioButton
            // 
            this.showThirdPartyRadioButton.AutoSize = true;
            this.showThirdPartyRadioButton.Location = new System.Drawing.Point(153, 16);
            this.showThirdPartyRadioButton.Name = "showThirdPartyRadioButton";
            this.showThirdPartyRadioButton.Size = new System.Drawing.Size(144, 15);
            this.showThirdPartyRadioButton.TabIndex = 5;
            this.showThirdPartyRadioButton.Text = "Show Patient/3rd Party";
            this.showThirdPartyRadioButton.UseSelectable = true;
            // 
            // showClientRadioButton
            // 
            this.showClientRadioButton.AutoSize = true;
            this.showClientRadioButton.Location = new System.Drawing.Point(318, 16);
            this.showClientRadioButton.Name = "showClientRadioButton";
            this.showClientRadioButton.Size = new System.Drawing.Size(132, 15);
            this.showClientRadioButton.TabIndex = 5;
            this.showClientRadioButton.Text = "Show Client Charges";
            this.showClientRadioButton.UseSelectable = true;
            // 
            // showAllRadioButton
            // 
            this.showAllRadioButton.AutoSize = true;
            this.showAllRadioButton.Checked = true;
            this.showAllRadioButton.Location = new System.Drawing.Point(469, 16);
            this.showAllRadioButton.Name = "showAllRadioButton";
            this.showAllRadioButton.Size = new System.Drawing.Size(69, 15);
            this.showAllRadioButton.TabIndex = 5;
            this.showAllRadioButton.TabStop = true;
            this.showAllRadioButton.Text = "Show All";
            this.showAllRadioButton.UseSelectable = true;
            // 
            // ChargeTreeListView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.showAllRadioButton);
            this.Controls.Add(this.showClientRadioButton);
            this.Controls.Add(this.showThirdPartyRadioButton);
            this.Controls.Add(this.showCreditsCheckbox);
            this.Controls.Add(this.addChargeButton);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "ChargeTreeListView";
            this.Size = new System.Drawing.Size(1153, 592);
            this.Load += new System.EventHandler(this.ChargeTreeListView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chargeGrid)).EndInit();
            this.chargeContextMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chargeDetailGrid)).EndInit();
            this.chargeDetailContextMenu.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView chargeGrid;
        private System.Windows.Forms.DataGridView chargeDetailGrid;
        private MetroFramework.Controls.MetroButton addChargeButton;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.RichTextBox chargeStatus1;
        private System.Windows.Forms.RichTextBox chargeStatus2;
        private MetroFramework.Controls.MetroLabel chargesLabel;
        private MetroFramework.Controls.MetroLabel chargeDetailsLabel;
        private System.Windows.Forms.ContextMenuStrip chargeContextMenu;
        private System.Windows.Forms.ToolStripMenuItem creditChargeToolStripMenuItem;
        private MetroFramework.Controls.MetroCheckBox showCreditsCheckbox;
        private MetroFramework.Controls.MetroRadioButton showThirdPartyRadioButton;
        private MetroFramework.Controls.MetroRadioButton showClientRadioButton;
        private MetroFramework.Controls.MetroRadioButton showAllRadioButton;
        private System.Windows.Forms.ToolStripMenuItem moveChargeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip chargeDetailContextMenu;
        private System.Windows.Forms.ToolStripMenuItem addModifierToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeModifierToolStripMenuItem;
    }
}
