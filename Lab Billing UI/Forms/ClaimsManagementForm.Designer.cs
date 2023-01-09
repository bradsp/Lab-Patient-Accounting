namespace LabBilling.Forms
{
    partial class ClaimsManagementForm
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle9 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.claimBatchDataGrid = new System.Windows.Forms.DataGridView();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.clearBatchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.regenerateClaimFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.claimBatchDetailDataGrid = new System.Windows.Forms.DataGridView();
            this.generateClaimsButton = new System.Windows.Forms.Button();
            this.institutionalRadioButton = new System.Windows.Forms.RadioButton();
            this.professionalRadioButton = new System.Windows.Forms.RadioButton();
            this.claimProgress = new System.Windows.Forms.ProgressBar();
            this.claimProgressStatusLabel = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.claimBatchDataGrid)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.claimBatchDetailDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // claimBatchDataGrid
            // 
            this.claimBatchDataGrid.AllowUserToAddRows = false;
            this.claimBatchDataGrid.AllowUserToDeleteRows = false;
            this.claimBatchDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimBatchDataGrid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.claimBatchDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle7;
            this.claimBatchDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.claimBatchDataGrid.ContextMenuStrip = this.contextMenuStrip1;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.claimBatchDataGrid.DefaultCellStyle = dataGridViewCellStyle8;
            this.claimBatchDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.claimBatchDataGrid.Location = new System.Drawing.Point(12, 8);
            this.claimBatchDataGrid.MultiSelect = false;
            this.claimBatchDataGrid.Name = "claimBatchDataGrid";
            this.claimBatchDataGrid.ReadOnly = true;
            dataGridViewCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle9.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle9.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.claimBatchDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle9;
            this.claimBatchDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.claimBatchDataGrid.Size = new System.Drawing.Size(356, 118);
            this.claimBatchDataGrid.TabIndex = 0;
            this.claimBatchDataGrid.SelectionChanged += new System.EventHandler(this.claimBatchDataGrid_SelectionChanged);
            this.claimBatchDataGrid.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.claimBatchDataGrid_MouseDoubleClick);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.clearBatchToolStripMenuItem,
            this.regenerateClaimFileToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(189, 48);
            // 
            // clearBatchToolStripMenuItem
            // 
            this.clearBatchToolStripMenuItem.Name = "clearBatchToolStripMenuItem";
            this.clearBatchToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.clearBatchToolStripMenuItem.Text = "Clear Batch";
            this.clearBatchToolStripMenuItem.Click += new System.EventHandler(this.clearBatchToolStripMenuItem_Click);
            // 
            // regenerateClaimFileToolStripMenuItem
            // 
            this.regenerateClaimFileToolStripMenuItem.Name = "regenerateClaimFileToolStripMenuItem";
            this.regenerateClaimFileToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.regenerateClaimFileToolStripMenuItem.Text = "Regenerate Claim File";
            this.regenerateClaimFileToolStripMenuItem.Click += new System.EventHandler(this.regenerateClaimFileToolStripMenuItem_Click);
            // 
            // claimBatchDetailDataGrid
            // 
            this.claimBatchDetailDataGrid.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.claimBatchDetailDataGrid.BackgroundColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.claimBatchDetailDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.claimBatchDetailDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.claimBatchDetailDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            this.claimBatchDetailDataGrid.Location = new System.Drawing.Point(12, 146);
            this.claimBatchDetailDataGrid.Name = "claimBatchDetailDataGrid";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.claimBatchDetailDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.claimBatchDetailDataGrid.Size = new System.Drawing.Size(1075, 276);
            this.claimBatchDetailDataGrid.TabIndex = 1;
            // 
            // generateClaimsButton
            // 
            this.generateClaimsButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.generateClaimsButton.Location = new System.Drawing.Point(790, 8);
            this.generateClaimsButton.Name = "generateClaimsButton";
            this.generateClaimsButton.Size = new System.Drawing.Size(116, 23);
            this.generateClaimsButton.TabIndex = 2;
            this.generateClaimsButton.Text = "Generate Claims";
            this.generateClaimsButton.UseVisualStyleBackColor = true;
            this.generateClaimsButton.Click += new System.EventHandler(this.generateClaimsButton_Click);
            // 
            // institutionalRadioButton
            // 
            this.institutionalRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.institutionalRadioButton.AutoSize = true;
            this.institutionalRadioButton.Location = new System.Drawing.Point(870, 37);
            this.institutionalRadioButton.Name = "institutionalRadioButton";
            this.institutionalRadioButton.Size = new System.Drawing.Size(78, 17);
            this.institutionalRadioButton.TabIndex = 3;
            this.institutionalRadioButton.TabStop = true;
            this.institutionalRadioButton.Text = "Institutional";
            this.institutionalRadioButton.UseVisualStyleBackColor = true;
            // 
            // professionalRadioButton
            // 
            this.professionalRadioButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.professionalRadioButton.AutoSize = true;
            this.professionalRadioButton.Location = new System.Drawing.Point(961, 37);
            this.professionalRadioButton.Name = "professionalRadioButton";
            this.professionalRadioButton.Size = new System.Drawing.Size(82, 17);
            this.professionalRadioButton.TabIndex = 3;
            this.professionalRadioButton.TabStop = true;
            this.professionalRadioButton.Text = "Professional";
            this.professionalRadioButton.UseVisualStyleBackColor = true;
            // 
            // claimProgress
            // 
            this.claimProgress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.claimProgress.Location = new System.Drawing.Point(871, 60);
            this.claimProgress.Name = "claimProgress";
            this.claimProgress.Size = new System.Drawing.Size(216, 23);
            this.claimProgress.TabIndex = 5;
            // 
            // claimProgressStatusLabel
            // 
            this.claimProgressStatusLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.claimProgressStatusLabel.AutoSize = true;
            this.claimProgressStatusLabel.Location = new System.Drawing.Point(868, 86);
            this.claimProgressStatusLabel.Name = "claimProgressStatusLabel";
            this.claimProgressStatusLabel.Size = new System.Drawing.Size(35, 13);
            this.claimProgressStatusLabel.TabIndex = 6;
            this.claimProgressStatusLabel.Text = "label1";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(790, 60);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // ClaimsManagementForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1099, 450);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.claimProgressStatusLabel);
            this.Controls.Add(this.claimProgress);
            this.Controls.Add(this.professionalRadioButton);
            this.Controls.Add(this.institutionalRadioButton);
            this.Controls.Add(this.generateClaimsButton);
            this.Controls.Add(this.claimBatchDetailDataGrid);
            this.Controls.Add(this.claimBatchDataGrid);
            this.Name = "ClaimsManagementForm";
            this.Text = "Claim Batch Management";
            this.Load += new System.EventHandler(this.ClaimsManagementForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.claimBatchDataGrid)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.claimBatchDetailDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView claimBatchDataGrid;
        private System.Windows.Forms.DataGridView claimBatchDetailDataGrid;
        private System.Windows.Forms.Button generateClaimsButton;
        private System.Windows.Forms.RadioButton institutionalRadioButton;
        private System.Windows.Forms.RadioButton professionalRadioButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearBatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regenerateClaimFileToolStripMenuItem;
        private System.Windows.Forms.ProgressBar claimProgress;
        private System.Windows.Forms.Label claimProgressStatusLabel;
        private System.Windows.Forms.Button cancelButton;
    }
}