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
            this.ValidateButton = new MetroFramework.Controls.MetroButton();
            this.PostButton = new MetroFramework.Controls.MetroButton();
            this.progressBar = new MetroFramework.Controls.MetroProgressBar();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.statusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.workqueues = new System.Windows.Forms.TreeView();
            this.CancelValidationButton = new MetroFramework.Controls.MetroButton();
            ((System.ComponentModel.ISupportInitialize)(this.accountGrid)).BeginInit();
            this.accountGridContextMenu.SuspendLayout();
            this.statusStrip.SuspendLayout();
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
            this.accountGrid.Location = new System.Drawing.Point(213, 68);
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
            this.accountGrid.Size = new System.Drawing.Size(941, 383);
            this.accountGrid.TabIndex = 1;
            this.accountGrid.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.accountGrid_CellFormatting);
            this.accountGrid.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.accountGrid_RowHeaderMouseDoubleClick);
            this.accountGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.accountGrid_MouseDoubleClick);
            this.accountGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.accountGrid_MouseDown);
            // 
            // accountGridContextMenu
            // 
            this.accountGridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.holdToolStripMenuItem});
            this.accountGridContextMenu.Name = "accountGridContextMenu";
            this.accountGridContextMenu.Size = new System.Drawing.Size(101, 26);
            // 
            // holdToolStripMenuItem
            // 
            this.holdToolStripMenuItem.Name = "holdToolStripMenuItem";
            this.holdToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.holdToolStripMenuItem.Text = "Hold";
            this.holdToolStripMenuItem.Click += new System.EventHandler(this.holdToolStripMenuItem_Click);
            // 
            // ValidateButton
            // 
            this.ValidateButton.Location = new System.Drawing.Point(13, 13);
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
            this.PostButton.Location = new System.Drawing.Point(251, 13);
            this.PostButton.Name = "PostButton";
            this.PostButton.Size = new System.Drawing.Size(94, 23);
            this.PostButton.TabIndex = 3;
            this.PostButton.Text = "Submit Claims";
            this.PostButton.UseSelectable = true;
            this.PostButton.Visible = false;
            this.PostButton.Click += new System.EventHandler(this.PostButton_Click);
            // 
            // progressBar
            // 
            this.progressBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar.Location = new System.Drawing.Point(213, 48);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(941, 14);
            this.progressBar.TabIndex = 7;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel1,
            this.statusLabel2});
            this.statusStrip.Location = new System.Drawing.Point(0, 454);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1166, 22);
            this.statusStrip.TabIndex = 8;
            this.statusStrip.Text = "Worklist Status";
            // 
            // statusLabel1
            // 
            this.statusLabel1.Name = "statusLabel1";
            this.statusLabel1.Size = new System.Drawing.Size(0, 17);
            // 
            // statusLabel2
            // 
            this.statusLabel2.Name = "statusLabel2";
            this.statusLabel2.Size = new System.Drawing.Size(0, 17);
            // 
            // workqueues
            // 
            this.workqueues.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.workqueues.Location = new System.Drawing.Point(13, 48);
            this.workqueues.Name = "workqueues";
            this.workqueues.Size = new System.Drawing.Size(194, 403);
            this.workqueues.TabIndex = 9;
            this.workqueues.NodeMouseDoubleClick += new System.Windows.Forms.TreeNodeMouseClickEventHandler(this.workqueues_NodeMouseDoubleClick);
            // 
            // CancelValidationButton
            // 
            this.CancelValidationButton.Location = new System.Drawing.Point(132, 13);
            this.CancelValidationButton.Name = "CancelValidationButton";
            this.CancelValidationButton.Size = new System.Drawing.Size(113, 23);
            this.CancelValidationButton.TabIndex = 10;
            this.CancelValidationButton.Text = "Cancel Validation";
            this.CancelValidationButton.UseSelectable = true;
            this.CancelValidationButton.Click += new System.EventHandler(this.CancelValidationButton_Click);
            // 
            // WorkListForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1166, 476);
            this.Controls.Add(this.CancelValidationButton);
            this.Controls.Add(this.workqueues);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.PostButton);
            this.Controls.Add(this.ValidateButton);
            this.Controls.Add(this.accountGrid);
            this.Name = "WorkListForm";
            this.Text = "Work List";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WorkListForm_FormClosing);
            this.Load += new System.EventHandler(this.WorkListForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.accountGrid)).EndInit();
            this.accountGridContextMenu.ResumeLayout(false);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroGrid accountGrid;
        private MetroFramework.Controls.MetroButton ValidateButton;
        private MetroFramework.Controls.MetroButton PostButton;
        private MetroFramework.Controls.MetroProgressBar progressBar;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel2;
        private System.Windows.Forms.TreeView workqueues;
        private MetroFramework.Controls.MetroButton CancelValidationButton;
        private System.Windows.Forms.ContextMenuStrip accountGridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem holdToolStripMenuItem;
    }
}