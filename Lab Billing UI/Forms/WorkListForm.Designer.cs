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
            components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            accountGrid = new System.Windows.Forms.DataGridView();
            accountGridContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            holdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeFinancialClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeDateOfServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            changeToYFinancialClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            readyToBillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            workqueues = new System.Windows.Forms.TreeView();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            panel1 = new System.Windows.Forms.Panel();
            panelExpandCollapseButton = new System.Windows.Forms.Button();
            filterTextBox = new System.Windows.Forms.TextBox();
            label1 = new System.Windows.Forms.Label();
            nameFilterRadioBtn = new System.Windows.Forms.RadioButton();
            clientFilterRadioBtn = new System.Windows.Forms.RadioButton();
            accountFilterRadioBtn = new System.Windows.Forms.RadioButton();
            showAccountsWithPmtCheckbox = new System.Windows.Forms.CheckBox();
            showReadyToBillCheckbox = new System.Windows.Forms.CheckBox();
            insuranceFilterRadioButton = new System.Windows.Forms.RadioButton();
            showZeroBalanceCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)accountGrid).BeginInit();
            accountGridContextMenu.SuspendLayout();
            statusStrip1.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // accountGrid
            // 
            accountGrid.AllowUserToAddRows = false;
            accountGrid.AllowUserToDeleteRows = false;
            accountGrid.AllowUserToOrderColumns = true;
            accountGrid.AllowUserToResizeRows = false;
            accountGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            accountGrid.BackgroundColor = System.Drawing.Color.FromArgb(255, 255, 255);
            accountGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            accountGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            accountGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            accountGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            accountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            accountGrid.ContextMenuStrip = accountGridContextMenu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            accountGrid.DefaultCellStyle = dataGridViewCellStyle2;
            accountGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            accountGrid.EnableHeadersVisualStyles = false;
            accountGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            accountGrid.GridColor = System.Drawing.Color.FromArgb(255, 255, 255);
            accountGrid.Location = new System.Drawing.Point(236, 47);
            accountGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accountGrid.MultiSelect = false;
            accountGrid.Name = "accountGrid";
            accountGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            accountGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            accountGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            accountGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            accountGrid.Size = new System.Drawing.Size(1111, 473);
            accountGrid.TabIndex = 1;
            accountGrid.CellFormatting += accountGrid_CellFormatting;
            accountGrid.MouseDoubleClick += accountGrid_MouseDoubleClick;
            accountGrid.MouseDown += accountGrid_MouseDown;
            // 
            // accountGridContextMenu
            // 
            accountGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { holdToolStripMenuItem, changeFinancialClassToolStripMenuItem, changeClientToolStripMenuItem, changeDateOfServiceToolStripMenuItem, changeToYFinancialClassToolStripMenuItem, readyToBillToolStripMenuItem });
            accountGridContextMenu.Name = "accountGridContextMenu";
            accountGridContextMenu.Size = new System.Drawing.Size(220, 136);
            accountGridContextMenu.Opened += accountGridContextMenu_Opened;
            accountGridContextMenu.VisibleChanged += accountGridContextMenu_VisibleChanged;
            // 
            // holdToolStripMenuItem
            // 
            holdToolStripMenuItem.Name = "holdToolStripMenuItem";
            holdToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            holdToolStripMenuItem.Text = "Hold Claim";
            holdToolStripMenuItem.Click += holdToolStripMenuItem_Click;
            // 
            // changeFinancialClassToolStripMenuItem
            // 
            changeFinancialClassToolStripMenuItem.Name = "changeFinancialClassToolStripMenuItem";
            changeFinancialClassToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            changeFinancialClassToolStripMenuItem.Text = "Change Financial Class";
            changeFinancialClassToolStripMenuItem.Click += changeFinancialClassToolStripMenuItem_Click;
            // 
            // changeClientToolStripMenuItem
            // 
            changeClientToolStripMenuItem.Name = "changeClientToolStripMenuItem";
            changeClientToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            changeClientToolStripMenuItem.Text = "Change Client";
            changeClientToolStripMenuItem.Click += changeClientToolStripMenuItem_Click;
            // 
            // changeDateOfServiceToolStripMenuItem
            // 
            changeDateOfServiceToolStripMenuItem.Name = "changeDateOfServiceToolStripMenuItem";
            changeDateOfServiceToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            changeDateOfServiceToolStripMenuItem.Text = "Change Date of Service";
            changeDateOfServiceToolStripMenuItem.Click += changeDateOfServiceToolStripMenuItem_Click;
            // 
            // changeToYFinancialClassToolStripMenuItem
            // 
            changeToYFinancialClassToolStripMenuItem.Name = "changeToYFinancialClassToolStripMenuItem";
            changeToYFinancialClassToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            changeToYFinancialClassToolStripMenuItem.Text = "Change to Y Financial Class";
            changeToYFinancialClassToolStripMenuItem.Click += changeToYFinancialClassToolStripMenuItem_Click;
            // 
            // readyToBillToolStripMenuItem
            // 
            readyToBillToolStripMenuItem.CheckOnClick = true;
            readyToBillToolStripMenuItem.Name = "readyToBillToolStripMenuItem";
            readyToBillToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            readyToBillToolStripMenuItem.Text = "Ready to Bill";
            readyToBillToolStripMenuItem.Click += readyToBillToolStripMenuItem_Click;
            // 
            // workqueues
            // 
            workqueues.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            workqueues.Location = new System.Drawing.Point(4, 3);
            workqueues.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            workqueues.Name = "workqueues";
            workqueues.Size = new System.Drawing.Size(204, 517);
            workqueues.TabIndex = 9;
            workqueues.NodeMouseDoubleClick += workqueues_NodeMouseDoubleClick;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabel1 });
            statusStrip1.Location = new System.Drawing.Point(0, 525);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new System.Windows.Forms.Padding(1, 0, 16, 0);
            statusStrip1.Size = new System.Drawing.Size(1360, 24);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new System.Drawing.Size(117, 18);
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new System.Drawing.Size(118, 19);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // panel1
            // 
            panel1.Controls.Add(panelExpandCollapseButton);
            panel1.Controls.Add(workqueues);
            panel1.Dock = System.Windows.Forms.DockStyle.Left;
            panel1.Location = new System.Drawing.Point(0, 0);
            panel1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new System.Drawing.Size(229, 525);
            panel1.TabIndex = 12;
            // 
            // panelExpandCollapseButton
            // 
            panelExpandCollapseButton.Dock = System.Windows.Forms.DockStyle.Right;
            panelExpandCollapseButton.Location = new System.Drawing.Point(206, 0);
            panelExpandCollapseButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            panelExpandCollapseButton.Name = "panelExpandCollapseButton";
            panelExpandCollapseButton.Size = new System.Drawing.Size(23, 525);
            panelExpandCollapseButton.TabIndex = 10;
            panelExpandCollapseButton.Text = "<";
            panelExpandCollapseButton.UseVisualStyleBackColor = true;
            panelExpandCollapseButton.Click += panelExpandCollapseButton_Click;
            // 
            // filterTextBox
            // 
            filterTextBox.Location = new System.Drawing.Point(275, 12);
            filterTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            filterTextBox.Name = "filterTextBox";
            filterTextBox.Size = new System.Drawing.Size(265, 23);
            filterTextBox.TabIndex = 13;
            filterTextBox.KeyUp += filterTextBox_KeyUp;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(234, 15);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(33, 15);
            label1.TabIndex = 14;
            label1.Text = "Filter";
            // 
            // nameFilterRadioBtn
            // 
            nameFilterRadioBtn.AutoSize = true;
            nameFilterRadioBtn.Checked = true;
            nameFilterRadioBtn.Location = new System.Drawing.Point(551, 15);
            nameFilterRadioBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nameFilterRadioBtn.Name = "nameFilterRadioBtn";
            nameFilterRadioBtn.Size = new System.Drawing.Size(57, 19);
            nameFilterRadioBtn.TabIndex = 15;
            nameFilterRadioBtn.TabStop = true;
            nameFilterRadioBtn.Text = "Name";
            nameFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // clientFilterRadioBtn
            // 
            clientFilterRadioBtn.AutoSize = true;
            clientFilterRadioBtn.Location = new System.Drawing.Point(620, 15);
            clientFilterRadioBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            clientFilterRadioBtn.Name = "clientFilterRadioBtn";
            clientFilterRadioBtn.Size = new System.Drawing.Size(56, 19);
            clientFilterRadioBtn.TabIndex = 15;
            clientFilterRadioBtn.TabStop = true;
            clientFilterRadioBtn.Text = "Client";
            clientFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // accountFilterRadioBtn
            // 
            accountFilterRadioBtn.AutoSize = true;
            accountFilterRadioBtn.Location = new System.Drawing.Point(686, 15);
            accountFilterRadioBtn.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accountFilterRadioBtn.Name = "accountFilterRadioBtn";
            accountFilterRadioBtn.Size = new System.Drawing.Size(70, 19);
            accountFilterRadioBtn.TabIndex = 15;
            accountFilterRadioBtn.TabStop = true;
            accountFilterRadioBtn.Text = "Account";
            accountFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // showAccountsWithPmtCheckbox
            // 
            showAccountsWithPmtCheckbox.AutoSize = true;
            showAccountsWithPmtCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            showAccountsWithPmtCheckbox.Location = new System.Drawing.Point(1011, 14);
            showAccountsWithPmtCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            showAccountsWithPmtCheckbox.Name = "showAccountsWithPmtCheckbox";
            showAccountsWithPmtCheckbox.Size = new System.Drawing.Size(186, 19);
            showAccountsWithPmtCheckbox.TabIndex = 16;
            showAccountsWithPmtCheckbox.Text = "Show Accounts with Payments";
            showAccountsWithPmtCheckbox.UseVisualStyleBackColor = true;
            showAccountsWithPmtCheckbox.CheckedChanged += showAccountsWithPmtCheckbox_CheckedChanged;
            // 
            // showReadyToBillCheckbox
            // 
            showReadyToBillCheckbox.AutoSize = true;
            showReadyToBillCheckbox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            showReadyToBillCheckbox.Location = new System.Drawing.Point(1216, 15);
            showReadyToBillCheckbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            showReadyToBillCheckbox.Name = "showReadyToBillCheckbox";
            showReadyToBillCheckbox.Size = new System.Drawing.Size(120, 19);
            showReadyToBillCheckbox.TabIndex = 16;
            showReadyToBillCheckbox.Text = "Show Ready to Bill";
            showReadyToBillCheckbox.UseVisualStyleBackColor = true;
            showReadyToBillCheckbox.CheckedChanged += showReadyToBillCheckbox_CheckedChanged;
            // 
            // insuranceFilterRadioButton
            // 
            insuranceFilterRadioButton.AutoSize = true;
            insuranceFilterRadioButton.Location = new System.Drawing.Point(769, 15);
            insuranceFilterRadioButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            insuranceFilterRadioButton.Name = "insuranceFilterRadioButton";
            insuranceFilterRadioButton.Size = new System.Drawing.Size(76, 19);
            insuranceFilterRadioButton.TabIndex = 15;
            insuranceFilterRadioButton.TabStop = true;
            insuranceFilterRadioButton.Text = "Insurance";
            insuranceFilterRadioButton.UseVisualStyleBackColor = true;
            // 
            // showZeroBalanceCheckBox
            // 
            showZeroBalanceCheckBox.AutoSize = true;
            showZeroBalanceCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            showZeroBalanceCheckBox.Location = new System.Drawing.Point(868, 14);
            showZeroBalanceCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            showZeroBalanceCheckBox.Name = "showZeroBalanceCheckBox";
            showZeroBalanceCheckBox.Size = new System.Drawing.Size(123, 19);
            showZeroBalanceCheckBox.TabIndex = 16;
            showZeroBalanceCheckBox.Text = "Show Zero Balance";
            showZeroBalanceCheckBox.UseVisualStyleBackColor = true;
            showZeroBalanceCheckBox.CheckedChanged += showZeroBalanceCheckBox_CheckedChanged;
            // 
            // WorkListForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1360, 549);
            Controls.Add(showReadyToBillCheckbox);
            Controls.Add(showZeroBalanceCheckBox);
            Controls.Add(showAccountsWithPmtCheckbox);
            Controls.Add(insuranceFilterRadioButton);
            Controls.Add(accountFilterRadioBtn);
            Controls.Add(clientFilterRadioBtn);
            Controls.Add(nameFilterRadioBtn);
            Controls.Add(label1);
            Controls.Add(filterTextBox);
            Controls.Add(panel1);
            Controls.Add(statusStrip1);
            Controls.Add(accountGrid);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "WorkListForm";
            Text = "Work List";
            Activated += WorkListForm_Activated;
            FormClosing += WorkListForm_FormClosing;
            Load += WorkListForm_Load;
            Enter += WorkListForm_Enter;
            ((System.ComponentModel.ISupportInitialize)accountGrid).EndInit();
            accountGridContextMenu.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            panel1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView accountGrid;
        private System.Windows.Forms.TreeView workqueues;
        private System.Windows.Forms.ContextMenuStrip accountGridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem holdToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeFinancialClassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeClientToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeDateOfServiceToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button panelExpandCollapseButton;
        private System.Windows.Forms.TextBox filterTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton nameFilterRadioBtn;
        private System.Windows.Forms.RadioButton clientFilterRadioBtn;
        private System.Windows.Forms.RadioButton accountFilterRadioBtn;
        private System.Windows.Forms.CheckBox showAccountsWithPmtCheckbox;
        private System.Windows.Forms.ToolStripMenuItem changeToYFinancialClassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readyToBillToolStripMenuItem;
        private System.Windows.Forms.CheckBox showReadyToBillCheckbox;
        private System.Windows.Forms.RadioButton insuranceFilterRadioButton;
        private System.Windows.Forms.CheckBox showZeroBalanceCheckBox;
    }
}