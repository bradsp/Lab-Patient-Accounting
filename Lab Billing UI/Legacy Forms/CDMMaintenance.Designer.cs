namespace LabBilling.Legacy
{
    partial class frmCDM
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCDM));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle10 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle11 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle12 = new System.Windows.Forms.DataGridViewCellStyle();
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbLoadCDM = new System.Windows.Forms.ToolStripButton();
            this.tscbCDM = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbUpdate = new System.Windows.Forms.ToolStripButton();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tabFeeSched2 = new System.Windows.Forms.TabPage();
            this.dgvFS2 = new System.Windows.Forms.DataGridView();
            this.tabFeeSched3 = new System.Windows.Forms.TabPage();
            this.dgvFS3 = new System.Windows.Forms.DataGridView();
            this.dgvCDM = new System.Windows.Forms.DataGridView();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.tabFeeSched1 = new System.Windows.Forms.TabPage();
            this.tsMain.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tabFeeSched2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFS2)).BeginInit();
            this.tabFeeSched3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvFS3)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCDM)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbLoadCDM,
            this.tscbCDM,
            this.toolStripSeparator1,
            this.tsbUpdate});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(1262, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tsbLoadCDM
            // 
            this.tsbLoadCDM.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbLoadCDM.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadCDM.Image")));
            this.tsbLoadCDM.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadCDM.Name = "tsbLoadCDM";
            this.tsbLoadCDM.Size = new System.Drawing.Size(72, 22);
            this.tsbLoadCDM.Text = "LOAD CDM";
            this.tsbLoadCDM.Click += new System.EventHandler(this.tsbLoadCDM_Click);
            // 
            // tscbCDM
            // 
            this.tscbCDM.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.tscbCDM.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.tscbCDM.Name = "tscbCDM";
            this.tscbCDM.Size = new System.Drawing.Size(121, 25);
            this.tscbCDM.ToolTipText = "Master CDM";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbUpdate
            // 
            this.tsbUpdate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsbUpdate.Image = ((System.Drawing.Image)(resources.GetObject("tsbUpdate.Image")));
            this.tsbUpdate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbUpdate.Name = "tsbUpdate";
            this.tsbUpdate.Size = new System.Drawing.Size(53, 22);
            this.tsbUpdate.Text = "UPDATE";
            this.tsbUpdate.Click += new System.EventHandler(this.tsbUpdate_Click);
            // 
            // ssMain
            // 
            this.ssMain.Location = new System.Drawing.Point(0, 706);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(1262, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "statusStrip1";
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tabFeeSched1);
            this.tcMain.Controls.Add(this.tabFeeSched2);
            this.tcMain.Controls.Add(this.tabFeeSched3);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(1262, 376);
            this.tcMain.TabIndex = 0;
            // 
            // tabFeeSched2
            // 
            this.tabFeeSched2.Controls.Add(this.dgvFS2);
            this.tabFeeSched2.Location = new System.Drawing.Point(4, 22);
            this.tabFeeSched2.Name = "tabFeeSched2";
            this.tabFeeSched2.Padding = new System.Windows.Forms.Padding(3);
            this.tabFeeSched2.Size = new System.Drawing.Size(1254, 350);
            this.tabFeeSched2.TabIndex = 0;
            this.tabFeeSched2.Text = "Fee Schedule 2";
            this.tabFeeSched2.UseVisualStyleBackColor = true;
            // 
            // dgvFS2
            // 
            this.dgvFS2.AllowDrop = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(224)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.Black;
            this.dgvFS2.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFS2.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.dgvFS2.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFS2.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvFS2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFS2.Location = new System.Drawing.Point(3, 3);
            this.dgvFS2.MultiSelect = false;
            this.dgvFS2.Name = "dgvFS2";
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFS2.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvFS2.Size = new System.Drawing.Size(1248, 344);
            this.dgvFS2.TabIndex = 0;
            this.dgvFS2.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_DragDrop);
            this.dgvFS2.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_DragEnter);
            // 
            // tabFeeSched3
            // 
            this.tabFeeSched3.Controls.Add(this.dgvFS3);
            this.tabFeeSched3.Location = new System.Drawing.Point(4, 22);
            this.tabFeeSched3.Name = "tabFeeSched3";
            this.tabFeeSched3.Padding = new System.Windows.Forms.Padding(3);
            this.tabFeeSched3.Size = new System.Drawing.Size(1254, 350);
            this.tabFeeSched3.TabIndex = 1;
            this.tabFeeSched3.Text = "Fee Schedule 3";
            this.tabFeeSched3.UseVisualStyleBackColor = true;
            // 
            // dgvFS3
            // 
            this.dgvFS3.AllowDrop = true;
            dataGridViewCellStyle5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.dgvFS3.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle5;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFS3.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle6;
            this.dgvFS3.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvFS3.DefaultCellStyle = dataGridViewCellStyle7;
            this.dgvFS3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvFS3.Location = new System.Drawing.Point(3, 3);
            this.dgvFS3.Name = "dgvFS3";
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvFS3.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.dgvFS3.Size = new System.Drawing.Size(1248, 344);
            this.dgvFS3.TabIndex = 0;
            this.dgvFS3.DragDrop += new System.Windows.Forms.DragEventHandler(this.dgv_DragDrop);
            this.dgvFS3.DragEnter += new System.Windows.Forms.DragEventHandler(this.dgv_DragEnter);
            // 
            // dgvCDM
            // 
            this.dgvCDM.AllowUserToAddRows = false;
            this.dgvCDM.AllowUserToDeleteRows = false;
            this.dgvCDM.AllowUserToOrderColumns = true;
            dataGridViewCellStyle9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            dataGridViewCellStyle9.ForeColor = System.Drawing.Color.Black;
            this.dgvCDM.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle9;
            this.dgvCDM.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            dataGridViewCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle10.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle10.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle10.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle10.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle10.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCDM.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle10;
            this.dgvCDM.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle11.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle11.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle11.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvCDM.DefaultCellStyle = dataGridViewCellStyle11;
            this.dgvCDM.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCDM.Location = new System.Drawing.Point(0, 0);
            this.dgvCDM.Name = "dgvCDM";
            dataGridViewCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle12.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle12.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle12.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle12.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle12.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvCDM.RowHeadersDefaultCellStyle = dataGridViewCellStyle12;
            this.dgvCDM.Size = new System.Drawing.Size(1262, 301);
            this.dgvCDM.TabIndex = 0;
            this.dgvCDM.DragLeave += new System.EventHandler(this.dgvCDM_DragLeave);
            this.dgvCDM.MouseDown += new System.Windows.Forms.MouseEventHandler(this.dgvCDM_MouseDown);
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.dgvCDM);
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tcMain);
            this.scMain.Size = new System.Drawing.Size(1262, 681);
            this.scMain.SplitterDistance = 301;
            this.scMain.TabIndex = 2;
            // 
            // tabFeeSched1
            // 
            this.tabFeeSched1.Location = new System.Drawing.Point(4, 22);
            this.tabFeeSched1.Name = "tabFeeSched1";
            this.tabFeeSched1.Padding = new System.Windows.Forms.Padding(3);
            this.tabFeeSched1.Size = new System.Drawing.Size(1254, 350);
            this.tabFeeSched1.TabIndex = 2;
            this.tabFeeSched1.Text = "Fee Schedule 1";
            this.tabFeeSched1.UseVisualStyleBackColor = true;
            // 
            // frmCDM
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1262, 728);
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Name = "frmCDM";
            this.Text = "FEE SCHEDULE MAINTENANCE";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmCDM_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.tcMain.ResumeLayout(false);
            this.tabFeeSched2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFS2)).EndInit();
            this.tabFeeSched3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvFS3)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvCDM)).EndInit();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripComboBox tscbCDM;
        private System.Windows.Forms.ToolStripButton tsbLoadCDM;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbUpdate;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tabFeeSched2;
        private System.Windows.Forms.DataGridView dgvFS2;
        private System.Windows.Forms.TabPage tabFeeSched3;
        private System.Windows.Forms.DataGridView dgvFS3;
        private System.Windows.Forms.DataGridView dgvCDM;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TabPage tabFeeSched1;
    }
}

