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
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            accountGrid = new DataGridView();
            accountGridContextMenu = new ContextMenuStrip(components);
            holdToolStripMenuItem = new ToolStripMenuItem();
            changeFinancialClassToolStripMenuItem = new ToolStripMenuItem();
            changeClientToolStripMenuItem = new ToolStripMenuItem();
            changeDateOfServiceToolStripMenuItem = new ToolStripMenuItem();
            changeToYFinancialClassToolStripMenuItem = new ToolStripMenuItem();
            readyToBillToolStripMenuItem = new ToolStripMenuItem();
            workqueues = new TreeView();
            statusStrip1 = new StatusStrip();
            toolStripProgressBar1 = new ToolStripProgressBar();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            panel1 = new Panel();
            panelExpandCollapseButton = new Button();
            filterTextBox = new TextBox();
            label1 = new Label();
            nameFilterRadioBtn = new RadioButton();
            clientFilterRadioBtn = new RadioButton();
            accountFilterRadioBtn = new RadioButton();
            showAccountsWithPmtCheckbox = new CheckBox();
            showReadyToBillCheckbox = new CheckBox();
            insuranceFilterRadioButton = new RadioButton();
            showZeroBalanceCheckBox = new CheckBox();
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
            accountGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            accountGrid.BackgroundColor = Color.FromArgb(255, 255, 255);
            accountGrid.BorderStyle = BorderStyle.None;
            accountGrid.CellBorderStyle = DataGridViewCellBorderStyle.None;
            accountGrid.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle4.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle4.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle4.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            accountGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            accountGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            accountGrid.ContextMenuStrip = accountGridContextMenu;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle5.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = Color.FromArgb(136, 136, 136);
            dataGridViewCellStyle5.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle5.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            accountGrid.DefaultCellStyle = dataGridViewCellStyle5;
            accountGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            accountGrid.EnableHeadersVisualStyles = false;
            accountGrid.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            accountGrid.GridColor = Color.FromArgb(255, 255, 255);
            accountGrid.Location = new Point(236, 47);
            accountGrid.Margin = new Padding(4, 3, 4, 3);
            accountGrid.MultiSelect = false;
            accountGrid.Name = "accountGrid";
            accountGrid.RowHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = Color.FromArgb(0, 174, 219);
            dataGridViewCellStyle6.Font = new Font("Segoe UI", 11F, FontStyle.Regular, GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = Color.FromArgb(255, 255, 255);
            dataGridViewCellStyle6.SelectionBackColor = Color.FromArgb(0, 198, 247);
            dataGridViewCellStyle6.SelectionForeColor = Color.FromArgb(17, 17, 17);
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            accountGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            accountGrid.RowHeadersWidthSizeMode = DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            accountGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            accountGrid.Size = new Size(1111, 473);
            accountGrid.TabIndex = 1;
            accountGrid.CellFormatting += accountGrid_CellFormatting;
            accountGrid.MouseDoubleClick += accountGrid_MouseDoubleClick;
            accountGrid.MouseDown += accountGrid_MouseDown;
            // 
            // accountGridContextMenu
            // 
            accountGridContextMenu.Items.AddRange(new ToolStripItem[] { holdToolStripMenuItem, changeFinancialClassToolStripMenuItem, changeClientToolStripMenuItem, changeDateOfServiceToolStripMenuItem, changeToYFinancialClassToolStripMenuItem, readyToBillToolStripMenuItem });
            accountGridContextMenu.Name = "accountGridContextMenu";
            accountGridContextMenu.Size = new Size(220, 136);
            accountGridContextMenu.Opened += accountGridContextMenu_Opened;
            accountGridContextMenu.VisibleChanged += accountGridContextMenu_VisibleChanged;
            // 
            // holdToolStripMenuItem
            // 
            holdToolStripMenuItem.Name = "holdToolStripMenuItem";
            holdToolStripMenuItem.Size = new Size(219, 22);
            holdToolStripMenuItem.Text = "Hold Claim";
            holdToolStripMenuItem.Click += holdToolStripMenuItem_Click;
            // 
            // changeFinancialClassToolStripMenuItem
            // 
            changeFinancialClassToolStripMenuItem.Name = "changeFinancialClassToolStripMenuItem";
            changeFinancialClassToolStripMenuItem.Size = new Size(219, 22);
            changeFinancialClassToolStripMenuItem.Text = "Change Financial Class";
            changeFinancialClassToolStripMenuItem.Click += changeFinancialClassToolStripMenuItem_Click;
            // 
            // changeClientToolStripMenuItem
            // 
            changeClientToolStripMenuItem.Name = "changeClientToolStripMenuItem";
            changeClientToolStripMenuItem.Size = new Size(219, 22);
            changeClientToolStripMenuItem.Text = "Change Client";
            changeClientToolStripMenuItem.Click += changeClientToolStripMenuItem_Click;
            // 
            // changeDateOfServiceToolStripMenuItem
            // 
            changeDateOfServiceToolStripMenuItem.Name = "changeDateOfServiceToolStripMenuItem";
            changeDateOfServiceToolStripMenuItem.Size = new Size(219, 22);
            changeDateOfServiceToolStripMenuItem.Text = "Change Date of Service";
            changeDateOfServiceToolStripMenuItem.Click += changeDateOfServiceToolStripMenuItem_Click;
            // 
            // changeToYFinancialClassToolStripMenuItem
            // 
            changeToYFinancialClassToolStripMenuItem.Name = "changeToYFinancialClassToolStripMenuItem";
            changeToYFinancialClassToolStripMenuItem.Size = new Size(219, 22);
            changeToYFinancialClassToolStripMenuItem.Text = "Change to Y Financial Class";
            changeToYFinancialClassToolStripMenuItem.Click += changeToYFinancialClassToolStripMenuItem_Click;
            // 
            // readyToBillToolStripMenuItem
            // 
            readyToBillToolStripMenuItem.CheckOnClick = true;
            readyToBillToolStripMenuItem.Name = "readyToBillToolStripMenuItem";
            readyToBillToolStripMenuItem.Size = new Size(219, 22);
            readyToBillToolStripMenuItem.Text = "Ready to Bill";
            readyToBillToolStripMenuItem.Click += readyToBillToolStripMenuItem_Click;
            // 
            // workqueues
            // 
            workqueues.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            workqueues.Location = new Point(4, 3);
            workqueues.Margin = new Padding(4, 3, 4, 3);
            workqueues.Name = "workqueues";
            workqueues.Size = new Size(204, 517);
            workqueues.TabIndex = 9;
            workqueues.NodeMouseDoubleClick += workqueues_NodeMouseDoubleClick;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripProgressBar1, toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 525);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 16, 0);
            statusStrip1.Size = new Size(1360, 24);
            statusStrip1.TabIndex = 11;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            toolStripProgressBar1.Name = "toolStripProgressBar1";
            toolStripProgressBar1.Size = new Size(117, 18);
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(118, 19);
            toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // panel1
            // 
            panel1.Controls.Add(panelExpandCollapseButton);
            panel1.Controls.Add(workqueues);
            panel1.Dock = DockStyle.Left;
            panel1.Location = new Point(0, 0);
            panel1.Margin = new Padding(4, 3, 4, 3);
            panel1.Name = "panel1";
            panel1.Size = new Size(229, 525);
            panel1.TabIndex = 12;
            // 
            // panelExpandCollapseButton
            // 
            panelExpandCollapseButton.Dock = DockStyle.Right;
            panelExpandCollapseButton.Location = new Point(206, 0);
            panelExpandCollapseButton.Margin = new Padding(4, 3, 4, 3);
            panelExpandCollapseButton.Name = "panelExpandCollapseButton";
            panelExpandCollapseButton.Size = new Size(23, 525);
            panelExpandCollapseButton.TabIndex = 10;
            panelExpandCollapseButton.Text = "<";
            panelExpandCollapseButton.UseVisualStyleBackColor = true;
            panelExpandCollapseButton.Click += panelExpandCollapseButton_Click;
            // 
            // filterTextBox
            // 
            filterTextBox.Location = new Point(275, 12);
            filterTextBox.Margin = new Padding(4, 3, 4, 3);
            filterTextBox.Name = "filterTextBox";
            filterTextBox.Size = new Size(265, 23);
            filterTextBox.TabIndex = 13;
            filterTextBox.KeyUp += filterTextBox_KeyUp;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(234, 15);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(33, 15);
            label1.TabIndex = 14;
            label1.Text = "Filter";
            // 
            // nameFilterRadioBtn
            // 
            nameFilterRadioBtn.AutoSize = true;
            nameFilterRadioBtn.Checked = true;
            nameFilterRadioBtn.Location = new Point(551, 15);
            nameFilterRadioBtn.Margin = new Padding(4, 3, 4, 3);
            nameFilterRadioBtn.Name = "nameFilterRadioBtn";
            nameFilterRadioBtn.Size = new Size(57, 19);
            nameFilterRadioBtn.TabIndex = 15;
            nameFilterRadioBtn.TabStop = true;
            nameFilterRadioBtn.Text = "Name";
            nameFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // clientFilterRadioBtn
            // 
            clientFilterRadioBtn.AutoSize = true;
            clientFilterRadioBtn.Location = new Point(620, 15);
            clientFilterRadioBtn.Margin = new Padding(4, 3, 4, 3);
            clientFilterRadioBtn.Name = "clientFilterRadioBtn";
            clientFilterRadioBtn.Size = new Size(56, 19);
            clientFilterRadioBtn.TabIndex = 15;
            clientFilterRadioBtn.TabStop = true;
            clientFilterRadioBtn.Text = "Client";
            clientFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // accountFilterRadioBtn
            // 
            accountFilterRadioBtn.AutoSize = true;
            accountFilterRadioBtn.Location = new Point(686, 15);
            accountFilterRadioBtn.Margin = new Padding(4, 3, 4, 3);
            accountFilterRadioBtn.Name = "accountFilterRadioBtn";
            accountFilterRadioBtn.Size = new Size(70, 19);
            accountFilterRadioBtn.TabIndex = 15;
            accountFilterRadioBtn.TabStop = true;
            accountFilterRadioBtn.Text = "Account";
            accountFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // showAccountsWithPmtCheckbox
            // 
            showAccountsWithPmtCheckbox.AutoSize = true;
            showAccountsWithPmtCheckbox.Checked = true;
            showAccountsWithPmtCheckbox.CheckState = CheckState.Checked;
            showAccountsWithPmtCheckbox.FlatStyle = FlatStyle.Flat;
            showAccountsWithPmtCheckbox.Location = new Point(1011, 14);
            showAccountsWithPmtCheckbox.Margin = new Padding(4, 3, 4, 3);
            showAccountsWithPmtCheckbox.Name = "showAccountsWithPmtCheckbox";
            showAccountsWithPmtCheckbox.Size = new Size(186, 19);
            showAccountsWithPmtCheckbox.TabIndex = 16;
            showAccountsWithPmtCheckbox.Text = "Show Accounts with Payments";
            showAccountsWithPmtCheckbox.UseVisualStyleBackColor = true;
            showAccountsWithPmtCheckbox.CheckedChanged += showAccountsWithPmtCheckbox_CheckedChanged;
            // 
            // showReadyToBillCheckbox
            // 
            showReadyToBillCheckbox.AutoSize = true;
            showReadyToBillCheckbox.FlatStyle = FlatStyle.Flat;
            showReadyToBillCheckbox.Location = new Point(1216, 15);
            showReadyToBillCheckbox.Margin = new Padding(4, 3, 4, 3);
            showReadyToBillCheckbox.Name = "showReadyToBillCheckbox";
            showReadyToBillCheckbox.Size = new Size(120, 19);
            showReadyToBillCheckbox.TabIndex = 16;
            showReadyToBillCheckbox.Text = "Show Ready to Bill";
            showReadyToBillCheckbox.UseVisualStyleBackColor = true;
            showReadyToBillCheckbox.CheckedChanged += showReadyToBillCheckbox_CheckedChanged;
            // 
            // insuranceFilterRadioButton
            // 
            insuranceFilterRadioButton.AutoSize = true;
            insuranceFilterRadioButton.Location = new Point(769, 15);
            insuranceFilterRadioButton.Margin = new Padding(4, 3, 4, 3);
            insuranceFilterRadioButton.Name = "insuranceFilterRadioButton";
            insuranceFilterRadioButton.Size = new Size(76, 19);
            insuranceFilterRadioButton.TabIndex = 15;
            insuranceFilterRadioButton.TabStop = true;
            insuranceFilterRadioButton.Text = "Insurance";
            insuranceFilterRadioButton.UseVisualStyleBackColor = true;
            // 
            // showZeroBalanceCheckBox
            // 
            showZeroBalanceCheckBox.AutoSize = true;
            showZeroBalanceCheckBox.Checked = true;
            showZeroBalanceCheckBox.CheckState = CheckState.Checked;
            showZeroBalanceCheckBox.FlatStyle = FlatStyle.Flat;
            showZeroBalanceCheckBox.Location = new Point(868, 14);
            showZeroBalanceCheckBox.Margin = new Padding(4, 3, 4, 3);
            showZeroBalanceCheckBox.Name = "showZeroBalanceCheckBox";
            showZeroBalanceCheckBox.Size = new Size(123, 19);
            showZeroBalanceCheckBox.TabIndex = 16;
            showZeroBalanceCheckBox.Text = "Show Zero Balance";
            showZeroBalanceCheckBox.UseVisualStyleBackColor = true;
            showZeroBalanceCheckBox.CheckedChanged += showZeroBalanceCheckBox_CheckedChanged;
            // 
            // WorkListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1360, 549);
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
            Margin = new Padding(4, 3, 4, 3);
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