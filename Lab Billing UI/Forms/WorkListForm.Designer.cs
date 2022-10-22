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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            this.accountGrid = new MetroFramework.Controls.MetroGrid();
            this.accountGridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.holdToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeFinancialClassToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeClientToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeDateOfServiceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ValidateButton = new MetroFramework.Controls.MetroButton();
            this.PostButton = new MetroFramework.Controls.MetroButton();
            this.workqueues = new System.Windows.Forms.TreeView();
            this.CancelValidationButton = new MetroFramework.Controls.MetroButton();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panelExpandCollapseButton = new System.Windows.Forms.Button();
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
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.accountGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.accountGrid.ContextMenuStrip = this.accountGridContextMenu;
            dataGridViewCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle5.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(136)))), ((int)(((byte)(136)))), ((int)(((byte)(136)))));
            dataGridViewCellStyle5.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle5.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.accountGrid.DefaultCellStyle = dataGridViewCellStyle5;
            this.accountGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.accountGrid.EnableHeadersVisualStyles = false;
            this.accountGrid.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.accountGrid.GridColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.accountGrid.Location = new System.Drawing.Point(202, 41);
            this.accountGrid.MultiSelect = false;
            this.accountGrid.Name = "accountGrid";
            this.accountGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(174)))), ((int)(((byte)(219)))));
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            dataGridViewCellStyle6.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(198)))), ((int)(((byte)(247)))));
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(17)))), ((int)(((byte)(17)))), ((int)(((byte)(17)))));
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.accountGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.accountGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.accountGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.accountGrid.Size = new System.Drawing.Size(952, 410);
            this.accountGrid.TabIndex = 1;
            this.accountGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.accountGrid_CellFormatting);
            this.accountGrid.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.accountGrid_RowHeaderMouseDoubleClick);
            this.accountGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.accountGrid_MouseDoubleClick);
            this.accountGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.accountGrid_MouseDown);
            // 
            // accountGridContextMenu
            // 
            this.accountGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.holdToolStripMenuItem,
            this.changeFinancialClassToolStripMenuItem,
            this.changeClientToolStripMenuItem,
            this.changeDateOfServiceToolStripMenuItem});
            this.accountGridContextMenu.Name = "accountGridContextMenu";
            this.accountGridContextMenu.Size = new System.Drawing.Size(197, 92);
            // 
            // holdToolStripMenuItem
            // 
            this.holdToolStripMenuItem.Name = "holdToolStripMenuItem";
            this.holdToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.holdToolStripMenuItem.Text = "Hold Claim";
            this.holdToolStripMenuItem.Click += new System.EventHandler(this.holdToolStripMenuItem_Click);
            // 
            // changeFinancialClassToolStripMenuItem
            // 
            this.changeFinancialClassToolStripMenuItem.Name = "changeFinancialClassToolStripMenuItem";
            this.changeFinancialClassToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changeFinancialClassToolStripMenuItem.Text = "Change Financial Class";
            this.changeFinancialClassToolStripMenuItem.Click += new System.EventHandler(this.changeFinancialClassToolStripMenuItem_Click);
            // 
            // changeClientToolStripMenuItem
            // 
            this.changeClientToolStripMenuItem.Name = "changeClientToolStripMenuItem";
            this.changeClientToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changeClientToolStripMenuItem.Text = "Change Client";
            this.changeClientToolStripMenuItem.Click += new System.EventHandler(this.changeClientToolStripMenuItem_Click);
            // 
            // changeDateOfServiceToolStripMenuItem
            // 
            this.changeDateOfServiceToolStripMenuItem.Name = "changeDateOfServiceToolStripMenuItem";
            this.changeDateOfServiceToolStripMenuItem.Size = new System.Drawing.Size(196, 22);
            this.changeDateOfServiceToolStripMenuItem.Text = "Change Date of Service";
            this.changeDateOfServiceToolStripMenuItem.Click += new System.EventHandler(this.changeDateOfServiceToolStripMenuItem_Click);
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
            // PostButton
            // 
            this.PostButton.Enabled = false;
            this.PostButton.Location = new System.Drawing.Point(440, 12);
            this.PostButton.Name = "PostButton";
            this.PostButton.Size = new System.Drawing.Size(94, 23);
            this.PostButton.TabIndex = 3;
            this.PostButton.Text = "Submit Claims";
            this.PostButton.UseSelectable = true;
            this.PostButton.Visible = false;
            this.PostButton.Click += new System.EventHandler(this.PostButton_Click);
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
            // WorkListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 476);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.CancelValidationButton);
            this.Controls.Add(this.PostButton);
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
        private MetroFramework.Controls.MetroButton PostButton;
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
    }
}