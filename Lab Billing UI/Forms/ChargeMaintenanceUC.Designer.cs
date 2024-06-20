using System;
using System.ComponentModel;

namespace LabBilling.Forms
{
    partial class ChargeMaintenanceUC
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components;

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
            components = new Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            showAllChargeRadioButton = new RadioButton();
            showClientRadioButton = new RadioButton();
            show3rdPartyRadioButton = new RadioButton();
            chargeLayoutPanel = new TableLayoutPanel();
            ChargesDataGridLabel = new Label();
            ChargesDataGrid = new DataGridView();
            chargesContextMenu = new ContextMenuStrip(components);
            toolStripCreditCharge = new ToolStripMenuItem();
            moveChargeToolStripMenuItem = new ToolStripMenuItem();
            changeCreditFlagToolStripMenuItem = new ToolStripMenuItem();
            addModifierToolStripMenuItem1 = new ToolStripMenuItem();
            removeModifierToolStripMenuItem1 = new ToolStripMenuItem();
            AddChargeButton = new Button();
            chargeBalRichTextbox = new RichTextBox();
            TotalChargesTextBox = new TextBox();
            ChargeTotalChargesLabel = new Label();
            ShowCreditedChrgCheckBox = new CheckBox();
            chargeLayoutPanel.SuspendLayout();
            ((ISupportInitialize)ChargesDataGrid).BeginInit();
            chargesContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // showAllChargeRadioButton
            // 
            showAllChargeRadioButton.AutoSize = true;
            showAllChargeRadioButton.Checked = true;
            showAllChargeRadioButton.Location = new Point(446, 14);
            showAllChargeRadioButton.Margin = new Padding(4, 3, 4, 3);
            showAllChargeRadioButton.Name = "showAllChargeRadioButton";
            showAllChargeRadioButton.Size = new Size(71, 19);
            showAllChargeRadioButton.TabIndex = 17;
            showAllChargeRadioButton.TabStop = true;
            showAllChargeRadioButton.Text = "Show All";
            showAllChargeRadioButton.UseVisualStyleBackColor = true;
            showAllChargeRadioButton.CheckedChanged += filterRadioButton_CheckedChanged;
            // 
            // showClientRadioButton
            // 
            showClientRadioButton.AutoSize = true;
            showClientRadioButton.BackColor = Color.PaleGreen;
            showClientRadioButton.Location = new Point(293, 14);
            showClientRadioButton.Margin = new Padding(4, 3, 4, 3);
            showClientRadioButton.Name = "showClientRadioButton";
            showClientRadioButton.Size = new Size(136, 19);
            showClientRadioButton.TabIndex = 18;
            showClientRadioButton.Text = "Show Client Invoiced";
            showClientRadioButton.UseVisualStyleBackColor = false;
            showClientRadioButton.CheckedChanged += filterRadioButton_CheckedChanged;
            // 
            // show3rdPartyRadioButton
            // 
            show3rdPartyRadioButton.AutoSize = true;
            show3rdPartyRadioButton.BackColor = Color.Cyan;
            show3rdPartyRadioButton.Location = new Point(129, 14);
            show3rdPartyRadioButton.Margin = new Padding(4, 3, 4, 3);
            show3rdPartyRadioButton.Name = "show3rdPartyRadioButton";
            show3rdPartyRadioButton.Size = new Size(146, 19);
            show3rdPartyRadioButton.TabIndex = 19;
            show3rdPartyRadioButton.Text = "Show 3rd Party/Patient";
            show3rdPartyRadioButton.UseVisualStyleBackColor = false;
            show3rdPartyRadioButton.CheckedChanged += filterRadioButton_CheckedChanged;
            // 
            // chargeLayoutPanel
            // 
            chargeLayoutPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            chargeLayoutPanel.ColumnCount = 2;
            chargeLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            chargeLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
            chargeLayoutPanel.Controls.Add(ChargesDataGridLabel, 0, 0);
            chargeLayoutPanel.Controls.Add(ChargesDataGrid, 0, 1);
            chargeLayoutPanel.Controls.Add(AddChargeButton, 1, 0);
            chargeLayoutPanel.Controls.Add(chargeBalRichTextbox, 0, 2);
            chargeLayoutPanel.Location = new Point(8, 50);
            chargeLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            chargeLayoutPanel.Name = "chargeLayoutPanel";
            chargeLayoutPanel.RowCount = 3;
            chargeLayoutPanel.RowStyles.Add(new RowStyle());
            chargeLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 80F));
            chargeLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            chargeLayoutPanel.Size = new Size(925, 400);
            chargeLayoutPanel.TabIndex = 16;
            // 
            // ChargesDataGridLabel
            // 
            ChargesDataGridLabel.AutoSize = true;
            ChargesDataGridLabel.Dock = DockStyle.Bottom;
            ChargesDataGridLabel.Location = new Point(4, 18);
            ChargesDataGridLabel.Margin = new Padding(4, 0, 4, 0);
            ChargesDataGridLabel.Name = "ChargesDataGridLabel";
            ChargesDataGridLabel.Size = new Size(767, 15);
            ChargesDataGridLabel.TabIndex = 2;
            ChargesDataGridLabel.Text = "Charges";
            // 
            // ChargesDataGrid
            // 
            ChargesDataGrid.AllowUserToAddRows = false;
            ChargesDataGrid.AllowUserToDeleteRows = false;
            ChargesDataGrid.BackgroundColor = SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            ChargesDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ChargesDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            chargeLayoutPanel.SetColumnSpan(ChargesDataGrid, 2);
            ChargesDataGrid.ContextMenuStrip = chargesContextMenu;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            ChargesDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            ChargesDataGrid.Dock = DockStyle.Fill;
            ChargesDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            ChargesDataGrid.Location = new Point(4, 36);
            ChargesDataGrid.Margin = new Padding(4, 3, 4, 3);
            ChargesDataGrid.Name = "ChargesDataGrid";
            ChargesDataGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            ChargesDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            ChargesDataGrid.RowHeadersVisible = false;
            ChargesDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            ChargesDataGrid.Size = new Size(917, 287);
            ChargesDataGrid.TabIndex = 0;
            ChargesDataGrid.CellDoubleClick += DgvCharges_CellDoubleClick;
            ChargesDataGrid.CellFormatting += ChargesDataGrid_CellFormatting;
            ChargesDataGrid.CellMouseClick += ChargesDataGrid_CellMouseClick;
            ChargesDataGrid.ColumnSortModeChanged += ChargesDataGrid_ColumnSortModeChanged;
            // 
            // chargesContextMenu
            // 
            chargesContextMenu.Items.AddRange(new ToolStripItem[] { toolStripCreditCharge, moveChargeToolStripMenuItem, changeCreditFlagToolStripMenuItem, addModifierToolStripMenuItem1, removeModifierToolStripMenuItem1 });
            chargesContextMenu.Name = "menuCharges";
            chargesContextMenu.Size = new Size(181, 136);
            chargesContextMenu.Opening += chargesContextMenu_Opening;
            // 
            // toolStripCreditCharge
            // 
            toolStripCreditCharge.Name = "toolStripCreditCharge";
            toolStripCreditCharge.Size = new Size(180, 22);
            toolStripCreditCharge.Text = "Credit Charge";
            toolStripCreditCharge.Click += creditChargeToolStrip_Click;
            // 
            // moveChargeToolStripMenuItem
            // 
            moveChargeToolStripMenuItem.Name = "moveChargeToolStripMenuItem";
            moveChargeToolStripMenuItem.Size = new Size(180, 22);
            moveChargeToolStripMenuItem.Text = "Move Charge";
            moveChargeToolStripMenuItem.Click += moveChargeToolStripMenuItem_Click;
            // 
            // changeCreditFlagToolStripMenuItem
            // 
            changeCreditFlagToolStripMenuItem.Name = "changeCreditFlagToolStripMenuItem";
            changeCreditFlagToolStripMenuItem.Size = new Size(180, 22);
            changeCreditFlagToolStripMenuItem.Text = "Change Credit Flag";
            changeCreditFlagToolStripMenuItem.Click += changeCreditFlagToolStripMenuItem_Click;
            // 
            // addModifierToolStripMenuItem1
            // 
            addModifierToolStripMenuItem1.Name = "addModifierToolStripMenuItem1";
            addModifierToolStripMenuItem1.Size = new Size(180, 22);
            addModifierToolStripMenuItem1.Text = "Add Modifier";
            // 
            // removeModifierToolStripMenuItem1
            // 
            removeModifierToolStripMenuItem1.Name = "removeModifierToolStripMenuItem1";
            removeModifierToolStripMenuItem1.Size = new Size(180, 22);
            removeModifierToolStripMenuItem1.Text = "Remove Modifier";
            // 
            // AddChargeButton
            // 
            AddChargeButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            AddChargeButton.Location = new Point(821, 3);
            AddChargeButton.Margin = new Padding(4, 3, 4, 3);
            AddChargeButton.Name = "AddChargeButton";
            AddChargeButton.Size = new Size(100, 27);
            AddChargeButton.TabIndex = 7;
            AddChargeButton.Text = "Add Charge";
            AddChargeButton.UseVisualStyleBackColor = true;
            AddChargeButton.Click += AddChargeButton_Click;
            // 
            // chargeBalRichTextbox
            // 
            chargeLayoutPanel.SetColumnSpan(chargeBalRichTextbox, 2);
            chargeBalRichTextbox.Dock = DockStyle.Fill;
            chargeBalRichTextbox.Location = new Point(4, 329);
            chargeBalRichTextbox.Margin = new Padding(4, 3, 4, 3);
            chargeBalRichTextbox.Name = "chargeBalRichTextbox";
            chargeBalRichTextbox.Size = new Size(917, 68);
            chargeBalRichTextbox.TabIndex = 8;
            chargeBalRichTextbox.Text = "";
            // 
            // TotalChargesTextBox
            // 
            TotalChargesTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            TotalChargesTextBox.Location = new Point(805, 14);
            TotalChargesTextBox.Margin = new Padding(4, 3, 4, 3);
            TotalChargesTextBox.Name = "TotalChargesTextBox";
            TotalChargesTextBox.ReadOnly = true;
            TotalChargesTextBox.Size = new Size(124, 23);
            TotalChargesTextBox.TabIndex = 15;
            TotalChargesTextBox.TextAlign = HorizontalAlignment.Right;
            // 
            // ChargeTotalChargesLabel
            // 
            ChargeTotalChargesLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ChargeTotalChargesLabel.AutoSize = true;
            ChargeTotalChargesLabel.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            ChargeTotalChargesLabel.Location = new Point(697, 17);
            ChargeTotalChargesLabel.Margin = new Padding(4, 0, 4, 0);
            ChargeTotalChargesLabel.Name = "ChargeTotalChargesLabel";
            ChargeTotalChargesLabel.Size = new Size(86, 13);
            ChargeTotalChargesLabel.TabIndex = 14;
            ChargeTotalChargesLabel.Text = "Total Charges";
            // 
            // ShowCreditedChrgCheckBox
            // 
            ShowCreditedChrgCheckBox.AutoSize = true;
            ShowCreditedChrgCheckBox.ForeColor = Color.Red;
            ShowCreditedChrgCheckBox.Location = new Point(8, 14);
            ShowCreditedChrgCheckBox.Margin = new Padding(4, 3, 4, 3);
            ShowCreditedChrgCheckBox.Name = "ShowCreditedChrgCheckBox";
            ShowCreditedChrgCheckBox.Size = new Size(95, 19);
            ShowCreditedChrgCheckBox.TabIndex = 13;
            ShowCreditedChrgCheckBox.Text = "Show Credits";
            ShowCreditedChrgCheckBox.UseVisualStyleBackColor = true;
            ShowCreditedChrgCheckBox.CheckedChanged += ShowCreditedChrgCheckBox_CheckedChanged;
            // 
            // ChargeMaintenanceUC
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(showAllChargeRadioButton);
            Controls.Add(showClientRadioButton);
            Controls.Add(show3rdPartyRadioButton);
            Controls.Add(chargeLayoutPanel);
            Controls.Add(TotalChargesTextBox);
            Controls.Add(ChargeTotalChargesLabel);
            Controls.Add(ShowCreditedChrgCheckBox);
            Name = "ChargeMaintenanceUC";
            Size = new Size(947, 463);
            Load += ChargeMaintenanceUC_Load;
            chargeLayoutPanel.ResumeLayout(false);
            chargeLayoutPanel.PerformLayout();
            ((ISupportInitialize)ChargesDataGrid).EndInit();
            chargesContextMenu.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }
        #endregion

        private System.Windows.Forms.ToolStripMenuItem changeCreditFlagToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolStripCreditCharge;
        private System.Windows.Forms.ToolStripMenuItem moveChargeToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip chargesContextMenu;
        private System.Windows.Forms.RadioButton showAllChargeRadioButton;
        private System.Windows.Forms.RadioButton showClientRadioButton;
        private System.Windows.Forms.RadioButton show3rdPartyRadioButton;
        private System.Windows.Forms.TableLayoutPanel chargeLayoutPanel;
        private System.Windows.Forms.Label ChargesDataGridLabel;
        private System.Windows.Forms.DataGridView ChargesDataGrid;
        private System.Windows.Forms.Button AddChargeButton;
        private System.Windows.Forms.RichTextBox chargeBalRichTextbox;
        private System.Windows.Forms.TextBox TotalChargesTextBox;
        private System.Windows.Forms.Label ChargeTotalChargesLabel;
        private System.Windows.Forms.CheckBox ShowCreditedChrgCheckBox;
        private System.Windows.Forms.ToolStripMenuItem addModifierToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem removeModifierToolStripMenuItem1;
    }
}
