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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            showAllChargeRadioButton = new System.Windows.Forms.RadioButton();
            showClientRadioButton = new System.Windows.Forms.RadioButton();
            show3rdPartyRadioButton = new System.Windows.Forms.RadioButton();
            chargeLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            ChargesDataGridLabel = new System.Windows.Forms.Label();
            ChargesDataGrid = new System.Windows.Forms.DataGridView();
            chargesContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            toolStripCreditCharge = new System.Windows.Forms.ToolStripMenuItem();
            moveChargeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeCreditFlagToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            addModifierToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            removeModifierToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            AddChargeButton = new System.Windows.Forms.Button();
            chargeBalRichTextbox = new System.Windows.Forms.RichTextBox();
            TotalChargesTextBox = new System.Windows.Forms.TextBox();
            ChargeTotalChargesLabel = new System.Windows.Forms.Label();
            ShowCreditedChrgCheckBox = new System.Windows.Forms.CheckBox();
            chargeLayoutPanel.SuspendLayout();
            ((ISupportInitialize)ChargesDataGrid).BeginInit();
            chargesContextMenu.SuspendLayout();
            SuspendLayout();
            // 
            // showAllChargeRadioButton
            // 
            showAllChargeRadioButton.AutoSize = true;
            showAllChargeRadioButton.Checked = true;
            showAllChargeRadioButton.Location = new System.Drawing.Point(446, 14);
            showAllChargeRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            showAllChargeRadioButton.Name = "showAllChargeRadioButton";
            showAllChargeRadioButton.Size = new System.Drawing.Size(71, 19);
            showAllChargeRadioButton.TabIndex = 17;
            showAllChargeRadioButton.TabStop = true;
            showAllChargeRadioButton.Text = "Show All";
            showAllChargeRadioButton.UseVisualStyleBackColor = true;
            showAllChargeRadioButton.CheckedChanged += filterRadioButton_CheckedChanged;
            // 
            // showClientRadioButton
            // 
            showClientRadioButton.AutoSize = true;
            showClientRadioButton.BackColor = System.Drawing.Color.PaleGreen;
            showClientRadioButton.Location = new System.Drawing.Point(293, 14);
            showClientRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            showClientRadioButton.Name = "showClientRadioButton";
            showClientRadioButton.Size = new System.Drawing.Size(136, 19);
            showClientRadioButton.TabIndex = 18;
            showClientRadioButton.Text = "Show Client Invoiced";
            showClientRadioButton.UseVisualStyleBackColor = false;
            showClientRadioButton.CheckedChanged += filterRadioButton_CheckedChanged;
            // 
            // show3rdPartyRadioButton
            // 
            show3rdPartyRadioButton.AutoSize = true;
            show3rdPartyRadioButton.BackColor = System.Drawing.Color.Cyan;
            show3rdPartyRadioButton.Location = new System.Drawing.Point(129, 14);
            show3rdPartyRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            show3rdPartyRadioButton.Name = "show3rdPartyRadioButton";
            show3rdPartyRadioButton.Size = new System.Drawing.Size(146, 19);
            show3rdPartyRadioButton.TabIndex = 19;
            show3rdPartyRadioButton.Text = "Show 3rd Party/Patient";
            show3rdPartyRadioButton.UseVisualStyleBackColor = false;
            show3rdPartyRadioButton.CheckedChanged += filterRadioButton_CheckedChanged;
            // 
            // chargeLayoutPanel
            // 
            chargeLayoutPanel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            chargeLayoutPanel.ColumnCount = 2;
            chargeLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            chargeLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 150F));
            chargeLayoutPanel.Controls.Add(ChargesDataGridLabel, 0, 0);
            chargeLayoutPanel.Controls.Add(ChargesDataGrid, 0, 1);
            chargeLayoutPanel.Controls.Add(AddChargeButton, 1, 0);
            chargeLayoutPanel.Controls.Add(chargeBalRichTextbox, 0, 2);
            chargeLayoutPanel.Location = new System.Drawing.Point(8, 50);
            chargeLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chargeLayoutPanel.Name = "chargeLayoutPanel";
            chargeLayoutPanel.RowCount = 3;
            chargeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            chargeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 80F));
            chargeLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            chargeLayoutPanel.Size = new System.Drawing.Size(925, 400);
            chargeLayoutPanel.TabIndex = 16;
            // 
            // ChargesDataGridLabel
            // 
            ChargesDataGridLabel.AutoSize = true;
            ChargesDataGridLabel.Dock = System.Windows.Forms.DockStyle.Bottom;
            ChargesDataGridLabel.Location = new System.Drawing.Point(4, 18);
            ChargesDataGridLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ChargesDataGridLabel.Name = "ChargesDataGridLabel";
            ChargesDataGridLabel.Size = new System.Drawing.Size(767, 15);
            ChargesDataGridLabel.TabIndex = 2;
            ChargesDataGridLabel.Text = "Charges";
            // 
            // ChargesDataGrid
            // 
            ChargesDataGrid.AllowUserToAddRows = false;
            ChargesDataGrid.AllowUserToDeleteRows = false;
            ChargesDataGrid.BackgroundColor = System.Drawing.SystemColors.ControlLightLight;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            ChargesDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            ChargesDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            chargeLayoutPanel.SetColumnSpan(ChargesDataGrid, 2);
            ChargesDataGrid.ContextMenuStrip = chargesContextMenu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            ChargesDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            ChargesDataGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            ChargesDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            ChargesDataGrid.Location = new System.Drawing.Point(4, 36);
            ChargesDataGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ChargesDataGrid.Name = "ChargesDataGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            ChargesDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            ChargesDataGrid.RowHeadersVisible = false;
            ChargesDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            ChargesDataGrid.Size = new System.Drawing.Size(917, 287);
            ChargesDataGrid.TabIndex = 0;
            ChargesDataGrid.CellDoubleClick += DgvCharges_CellDoubleClick;
            ChargesDataGrid.CellFormatting += ChargesDataGrid_CellFormatting;
            ChargesDataGrid.CellMouseClick += ChargesDataGrid_CellMouseClick;
            // 
            // chargesContextMenu
            // 
            chargesContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripCreditCharge, moveChargeToolStripMenuItem, changeCreditFlagToolStripMenuItem, addModifierToolStripMenuItem1, removeModifierToolStripMenuItem1 });
            chargesContextMenu.Name = "menuCharges";
            chargesContextMenu.Size = new System.Drawing.Size(176, 114);
            // 
            // toolStripCreditCharge
            // 
            toolStripCreditCharge.Name = "toolStripCreditCharge";
            toolStripCreditCharge.Size = new System.Drawing.Size(175, 22);
            toolStripCreditCharge.Text = "Credit Charge";
            toolStripCreditCharge.Click += ToolStripCreditCharge_Click;
            // 
            // moveChargeToolStripMenuItem
            // 
            moveChargeToolStripMenuItem.Name = "moveChargeToolStripMenuItem";
            moveChargeToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            moveChargeToolStripMenuItem.Text = "Move Charge";
            moveChargeToolStripMenuItem.Click += moveChargeToolStripMenuItem_Click;
            // 
            // changeCreditFlagToolStripMenuItem
            // 
            changeCreditFlagToolStripMenuItem.Name = "changeCreditFlagToolStripMenuItem";
            changeCreditFlagToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            changeCreditFlagToolStripMenuItem.Text = "Change Credit Flag";
            changeCreditFlagToolStripMenuItem.Click += changeCreditFlagToolStripMenuItem_Click;
            // 
            // addModifierToolStripMenuItem1
            // 
            addModifierToolStripMenuItem1.Name = "addModifierToolStripMenuItem1";
            addModifierToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            addModifierToolStripMenuItem1.Text = "Add Modifier";
            // 
            // removeModifierToolStripMenuItem1
            // 
            removeModifierToolStripMenuItem1.Name = "removeModifierToolStripMenuItem1";
            removeModifierToolStripMenuItem1.Size = new System.Drawing.Size(175, 22);
            removeModifierToolStripMenuItem1.Text = "Remove Modifier";
            // 
            // AddChargeButton
            // 
            AddChargeButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            AddChargeButton.Location = new System.Drawing.Point(821, 3);
            AddChargeButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            AddChargeButton.Name = "AddChargeButton";
            AddChargeButton.Size = new System.Drawing.Size(100, 27);
            AddChargeButton.TabIndex = 7;
            AddChargeButton.Text = "Add Charge";
            AddChargeButton.UseVisualStyleBackColor = true;
            AddChargeButton.Click += AddChargeButton_Click;
            // 
            // chargeBalRichTextbox
            // 
            chargeLayoutPanel.SetColumnSpan(chargeBalRichTextbox, 2);
            chargeBalRichTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            chargeBalRichTextbox.Location = new System.Drawing.Point(4, 329);
            chargeBalRichTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chargeBalRichTextbox.Name = "chargeBalRichTextbox";
            chargeBalRichTextbox.Size = new System.Drawing.Size(917, 68);
            chargeBalRichTextbox.TabIndex = 8;
            chargeBalRichTextbox.Text = "";
            // 
            // TotalChargesTextBox
            // 
            TotalChargesTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            TotalChargesTextBox.Location = new System.Drawing.Point(805, 14);
            TotalChargesTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            TotalChargesTextBox.Name = "TotalChargesTextBox";
            TotalChargesTextBox.ReadOnly = true;
            TotalChargesTextBox.Size = new System.Drawing.Size(124, 23);
            TotalChargesTextBox.TabIndex = 15;
            TotalChargesTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // ChargeTotalChargesLabel
            // 
            ChargeTotalChargesLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ChargeTotalChargesLabel.AutoSize = true;
            ChargeTotalChargesLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            ChargeTotalChargesLabel.Location = new System.Drawing.Point(697, 17);
            ChargeTotalChargesLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            ChargeTotalChargesLabel.Name = "ChargeTotalChargesLabel";
            ChargeTotalChargesLabel.Size = new System.Drawing.Size(86, 13);
            ChargeTotalChargesLabel.TabIndex = 14;
            ChargeTotalChargesLabel.Text = "Total Charges";
            // 
            // ShowCreditedChrgCheckBox
            // 
            ShowCreditedChrgCheckBox.AutoSize = true;
            ShowCreditedChrgCheckBox.ForeColor = System.Drawing.Color.Red;
            ShowCreditedChrgCheckBox.Location = new System.Drawing.Point(8, 14);
            ShowCreditedChrgCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ShowCreditedChrgCheckBox.Name = "ShowCreditedChrgCheckBox";
            ShowCreditedChrgCheckBox.Size = new System.Drawing.Size(95, 19);
            ShowCreditedChrgCheckBox.TabIndex = 13;
            ShowCreditedChrgCheckBox.Text = "Show Credits";
            ShowCreditedChrgCheckBox.UseVisualStyleBackColor = true;
            ShowCreditedChrgCheckBox.CheckedChanged += ShowCreditedChrgCheckBox_CheckedChanged;
            // 
            // ChargeMaintenanceUC
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(showAllChargeRadioButton);
            Controls.Add(showClientRadioButton);
            Controls.Add(show3rdPartyRadioButton);
            Controls.Add(chargeLayoutPanel);
            Controls.Add(TotalChargesTextBox);
            Controls.Add(ChargeTotalChargesLabel);
            Controls.Add(ShowCreditedChrgCheckBox);
            Name = "ChargeMaintenanceUC";
            Size = new System.Drawing.Size(947, 463);
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
