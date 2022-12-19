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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.accountGrid = new MetroFramework.Controls.MetroGrid();
            this.accountGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.holdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFinancialClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDateOfServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeToYFinancialClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ValidateButton = new MetroFramework.Controls.MetroButton();
            this.workqueues = new System.Windows.Forms.TreeView();
            this.CancelValidationButton = new MetroFramework.Controls.MetroButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelExpandCollapseButton = new System.Windows.Forms.Button();
            this.filterTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nameFilterRadioBtn = new System.Windows.Forms.RadioButton();
            this.radioButton1 = new System.Windows.Forms.RadioButton();
            this.clientFilterRadioBtn = new System.Windows.Forms.RadioButton();
            this.accountFilterRadioBtn = new System.Windows.Forms.RadioButton();
            this.showAccountsWithPmtCheckbox = new System.Windows.Forms.CheckBox();
            this.readyToBillToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.accountGrid)).BeginInit();
            this.accountGridContextMenu.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // accountGrid
            // 
            this.accountGrid.AllowUserToAddRows = false;
            this.accountGrid.AllowUserToDeleteRows = false;
            this.accountGrid.AllowUserToOrderColumns = true;
            this.accountGrid.AllowUserToResizeRows = false;
            this.accountGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.accountGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountGrid.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.accountGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.accountGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.accountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accountGrid.ContextMenuStrip = this.accountGridContextMenu;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.accountGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.accountGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.accountGrid.EnableHeadersVisualStyles = false;
            this.accountGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.accountGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountGrid.Location = new System.Drawing.Point(202, 41);
            this.accountGrid.MultiSelect = false;
            this.accountGrid.Name = "accountGrid";
            this.accountGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.accountGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.accountGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.accountGrid.Size = new System.Drawing.Size(952, 410);
            this.accountGrid.TabIndex = 1;
            this.accountGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.accountGrid_CellFormatting);
            this.accountGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.accountGrid_MouseDoubleClick);
            this.accountGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.accountGrid_MouseDown);
            // 
            // accountGridContextMenu
            // 
            this.accountGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.holdToolStripMenuItem,
            this.changeFinancialClassToolStripMenuItem,
            this.changeClientToolStripMenuItem,
            this.changeDateOfServiceToolStripMenuItem,
            this.changeToYFinancialClassToolStripMenuItem,
            this.readyToBillToolStripMenuItem});
            this.accountGridContextMenu.Name = "accountGridContextMenu";
            this.accountGridContextMenu.Size = new System.Drawing.Size(220, 158);
            this.accountGridContextMenu.Opened += new System.EventHandler(this.accountGridContextMenu_Opened);
            this.accountGridContextMenu.VisibleChanged += new System.EventHandler(this.accountGridContextMenu_VisibleChanged);
            // 
            // holdToolStripMenuItem
            // 
            this.holdToolStripMenuItem.Name = "holdToolStripMenuItem";
            this.holdToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.holdToolStripMenuItem.Text = "Hold Claim";
            this.holdToolStripMenuItem.Click += new System.EventHandler(this.holdToolStripMenuItem_Click);
            // 
            // changeFinancialClassToolStripMenuItem
            // 
            this.changeFinancialClassToolStripMenuItem.Name = "changeFinancialClassToolStripMenuItem";
            this.changeFinancialClassToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.changeFinancialClassToolStripMenuItem.Text = "Change Financial Class";
            this.changeFinancialClassToolStripMenuItem.Click += new System.EventHandler(this.changeFinancialClassToolStripMenuItem_Click);
            // 
            // changeClientToolStripMenuItem
            // 
            this.changeClientToolStripMenuItem.Name = "changeClientToolStripMenuItem";
            this.changeClientToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.changeClientToolStripMenuItem.Text = "Change Client";
            this.changeClientToolStripMenuItem.Click += new System.EventHandler(this.changeClientToolStripMenuItem_Click);
            // 
            // changeDateOfServiceToolStripMenuItem
            // 
            this.changeDateOfServiceToolStripMenuItem.Name = "changeDateOfServiceToolStripMenuItem";
            this.changeDateOfServiceToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.changeDateOfServiceToolStripMenuItem.Text = "Change Date of Service";
            this.changeDateOfServiceToolStripMenuItem.Click += new System.EventHandler(this.changeDateOfServiceToolStripMenuItem_Click);
            // 
            // changeToYFinancialClassToolStripMenuItem
            // 
            this.changeToYFinancialClassToolStripMenuItem.Name = "changeToYFinancialClassToolStripMenuItem";
            this.changeToYFinancialClassToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.changeToYFinancialClassToolStripMenuItem.Text = "Change to Y Financial Class";
            this.changeToYFinancialClassToolStripMenuItem.Click += new System.EventHandler(this.changeToYFinancialClassToolStripMenuItem_Click);
            // 
            // ValidateButton
            // 
            this.ValidateButton.Location = new System.Drawing.Point(202, 12);
            this.ValidateButton.Name = "ValidateButton";
            this.ValidateButton.Size = new System.Drawing.Size(113, 23);
            this.ValidateButton.TabIndex = 2;
            this.ValidateButton.Text = "Validate Accounts";
            this.ValidateButton.UseSelectable = true;
            this.ValidateButton.Click += new System.EventHandler(this.ValidateButton_Click);
            // 
            // workqueues
            // 
            this.workqueues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.workqueues.Location = new System.Drawing.Point(3, 3);
            this.workqueues.Name = "workqueues";
            this.workqueues.Size = new System.Drawing.Size(175, 448);
            this.workqueues.TabIndex = 9;
            this.workqueues.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.workqueues_NodeMouseDoubleClick);
            // 
            // CancelValidationButton
            // 
            this.CancelValidationButton.Location = new System.Drawing.Point(321, 12);
            this.CancelValidationButton.Name = "CancelValidationButton";
            this.CancelValidationButton.Size = new System.Drawing.Size(113, 23);
            this.CancelValidationButton.TabIndex = 10;
            this.CancelValidationButton.Text = "Cancel Validation";
            this.CancelValidationButton.UseSelectable = true;
            this.CancelValidationButton.Click += new System.EventHandler(this.CancelValidationButton_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar1,
            this.toolStripStatusLabel1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 454);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1166, 22);
            this.statusStrip1.TabIndex = 11;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(118, 17);
            this.toolStripStatusLabel1.Text = "toolStripStatusLabel1";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panelExpandCollapseButton);
            this.panel1.Controls.Add(this.workqueues);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(196, 454);
            this.panel1.TabIndex = 12;
            // 
            // panelExpandCollapseButton
            // 
            this.panelExpandCollapseButton.Dock = System.Windows.Forms.DockStyle.Right;
            this.panelExpandCollapseButton.Location = new System.Drawing.Point(176, 0);
            this.panelExpandCollapseButton.Name = "panelExpandCollapseButton";
            this.panelExpandCollapseButton.Size = new System.Drawing.Size(20, 454);
            this.panelExpandCollapseButton.TabIndex = 10;
            this.panelExpandCollapseButton.Text = "<";
            this.panelExpandCollapseButton.UseVisualStyleBackColor = true;
            this.panelExpandCollapseButton.Click += new System.EventHandler(this.panelExpandCollapseButton_Click);
            // 
            // filterTextBox
            // 
            this.filterTextBox.Location = new System.Drawing.Point(492, 15);
            this.filterTextBox.Name = "filterTextBox";
            this.filterTextBox.Size = new System.Drawing.Size(253, 20);
            this.filterTextBox.TabIndex = 13;
            this.filterTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.filterTextBox_KeyUp);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(457, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(29, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Filter";
            // 
            // nameFilterRadioBtn
            // 
            this.nameFilterRadioBtn.AutoSize = true;
            this.nameFilterRadioBtn.Checked = true;
            this.nameFilterRadioBtn.Location = new System.Drawing.Point(760, 16);
            this.nameFilterRadioBtn.Name = "nameFilterRadioBtn";
            this.nameFilterRadioBtn.Size = new System.Drawing.Size(53, 17);
            this.nameFilterRadioBtn.TabIndex = 15;
            this.nameFilterRadioBtn.TabStop = true;
            this.nameFilterRadioBtn.Text = "Name";
            this.nameFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // radioButton1
            // 
            this.radioButton1.AutoSize = true;
            this.radioButton1.Location = new System.Drawing.Point(819, 15);
            this.radioButton1.Name = "radioButton1";
            this.radioButton1.Size = new System.Drawing.Size(53, 17);
            this.radioButton1.TabIndex = 15;
            this.radioButton1.TabStop = true;
            this.radioButton1.Text = "Name";
            this.radioButton1.UseVisualStyleBackColor = true;
            // 
            // clientFilterRadioBtn
            // 
            this.clientFilterRadioBtn.AutoSize = true;
            this.clientFilterRadioBtn.Location = new System.Drawing.Point(819, 16);
            this.clientFilterRadioBtn.Name = "clientFilterRadioBtn";
            this.clientFilterRadioBtn.Size = new System.Drawing.Size(51, 17);
            this.clientFilterRadioBtn.TabIndex = 15;
            this.clientFilterRadioBtn.TabStop = true;
            this.clientFilterRadioBtn.Text = "Client";
            this.clientFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // accountFilterRadioBtn
            // 
            this.accountFilterRadioBtn.AutoSize = true;
            this.accountFilterRadioBtn.Location = new System.Drawing.Point(876, 16);
            this.accountFilterRadioBtn.Name = "accountFilterRadioBtn";
            this.accountFilterRadioBtn.Size = new System.Drawing.Size(65, 17);
            this.accountFilterRadioBtn.TabIndex = 15;
            this.accountFilterRadioBtn.TabStop = true;
            this.accountFilterRadioBtn.Text = "Account";
            this.accountFilterRadioBtn.UseVisualStyleBackColor = true;
            // 
            // showAccountsWithPmtCheckbox
            // 
            this.showAccountsWithPmtCheckbox.AutoSize = true;
            this.showAccountsWithPmtCheckbox.Location = new System.Drawing.Point(982, 15);
            this.showAccountsWithPmtCheckbox.Name = "showAccountsWithPmtCheckbox";
            this.showAccountsWithPmtCheckbox.Size = new System.Drawing.Size(172, 17);
            this.showAccountsWithPmtCheckbox.TabIndex = 16;
            this.showAccountsWithPmtCheckbox.Text = "Show Accounts with Payments";
            this.showAccountsWithPmtCheckbox.UseVisualStyleBackColor = true;
            this.showAccountsWithPmtCheckbox.CheckedChanged += new System.EventHandler(this.showAccountsWithPmtCheckbox_CheckedChanged);
            // 
            // readyToBillToolStripMenuItem
            // 
            this.readyToBillToolStripMenuItem.CheckOnClick = true;
            this.readyToBillToolStripMenuItem.Name = "readyToBillToolStripMenuItem";
            this.readyToBillToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.readyToBillToolStripMenuItem.Text = "Ready to Bill";
            this.readyToBillToolStripMenuItem.Click += new System.EventHandler(this.readyToBillToolStripMenuItem_Click);
            // 
            // WorkListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 476);
            this.Controls.Add(this.showAccountsWithPmtCheckbox);
            this.Controls.Add(this.accountFilterRadioBtn);
            this.Controls.Add(this.clientFilterRadioBtn);
            this.Controls.Add(this.radioButton1);
            this.Controls.Add(this.nameFilterRadioBtn);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.filterTextBox);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CancelValidationButton);
            this.Controls.Add(this.ValidateButton);
            this.Controls.Add(this.accountGrid);
            this.Name = "WorkListForm";
            this.Text = "Work List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkListForm_FormClosing);
            this.Load += new System.EventHandler(this.WorkListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accountGrid)).EndInit();
            this.accountGridContextMenu.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroGrid accountGrid;
        private MetroFramework.Controls.MetroButton ValidateButton;
        private System.Windows.Forms.TreeView workqueues;
        private MetroFramework.Controls.MetroButton CancelValidationButton;
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
        private System.Windows.Forms.RadioButton radioButton1;
        private System.Windows.Forms.RadioButton clientFilterRadioBtn;
        private System.Windows.Forms.RadioButton accountFilterRadioBtn;
        private System.Windows.Forms.CheckBox showAccountsWithPmtCheckbox;
        private System.Windows.Forms.ToolStripMenuItem changeToYFinancialClassToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem readyToBillToolStripMenuItem;
    }
}