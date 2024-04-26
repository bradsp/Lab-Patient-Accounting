
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
            components = new System.ComponentModel.Container();
            DataGridViewCellStyle dataGridViewCellStyle1 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle2 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle3 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle4 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle5 = new DataGridViewCellStyle();
            DataGridViewCellStyle dataGridViewCellStyle6 = new DataGridViewCellStyle();
            claimBatchDataGrid = new DataGridView();
            contextMenuStrip1 = new ContextMenuStrip(components);
            clearBatchToolStripMenuItem = new ToolStripMenuItem();
            regenerateClaimFileToolStripMenuItem = new ToolStripMenuItem();
            claimBatchDetailDataGrid = new DataGridView();
            generateClaimsButton = new Krypton.Toolkit.KryptonButton();
            institutionalRadioButton = new Krypton.Toolkit.KryptonRadioButton();
            professionalRadioButton = new Krypton.Toolkit.KryptonRadioButton();
            claimProgress = new ProgressBar();
            claimProgressStatusLabel = new Label();
            cancelButton = new Krypton.Toolkit.KryptonButton();
            groupBox1 = new GroupBox();
            kryptonPanel1 = new Krypton.Toolkit.KryptonPanel();
            splitContainer1 = new SplitContainer();
            kryptonLabel1 = new Krypton.Toolkit.KryptonLabel();
            kryptonLabel2 = new Krypton.Toolkit.KryptonLabel();
            ((System.ComponentModel.ISupportInitialize)claimBatchDataGrid).BeginInit();
            contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)claimBatchDetailDataGrid).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).BeginInit();
            kryptonPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            SuspendLayout();
            // 
            // claimBatchDataGrid
            // 
            claimBatchDataGrid.AllowUserToAddRows = false;
            claimBatchDataGrid.AllowUserToDeleteRows = false;
            claimBatchDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            claimBatchDataGrid.BackgroundColor = Color.White;
            claimBatchDataGrid.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle1.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = SystemColors.Control;
            dataGridViewCellStyle1.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle1.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = DataGridViewTriState.True;
            claimBatchDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            claimBatchDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            claimBatchDataGrid.ContextMenuStrip = contextMenuStrip1;
            dataGridViewCellStyle2.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = SystemColors.Window;
            dataGridViewCellStyle2.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle2.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = DataGridViewTriState.False;
            claimBatchDataGrid.DefaultCellStyle = dataGridViewCellStyle2;
            claimBatchDataGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            claimBatchDataGrid.Location = new Point(13, 40);
            claimBatchDataGrid.Margin = new Padding(4, 3, 4, 3);
            claimBatchDataGrid.MultiSelect = false;
            claimBatchDataGrid.Name = "claimBatchDataGrid";
            claimBatchDataGrid.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = SystemColors.Control;
            dataGridViewCellStyle3.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle3.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = DataGridViewTriState.True;
            claimBatchDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            claimBatchDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            claimBatchDataGrid.Size = new Size(283, 328);
            claimBatchDataGrid.TabIndex = 0;
            claimBatchDataGrid.SelectionChanged += claimBatchDataGrid_SelectionChanged;
            claimBatchDataGrid.MouseDoubleClick += claimBatchDataGrid_MouseDoubleClick;
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.Font = new Font("Segoe UI", 9F);
            contextMenuStrip1.Items.AddRange(new ToolStripItem[] { clearBatchToolStripMenuItem, regenerateClaimFileToolStripMenuItem });
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(189, 48);
            // 
            // clearBatchToolStripMenuItem
            // 
            clearBatchToolStripMenuItem.Name = "clearBatchToolStripMenuItem";
            clearBatchToolStripMenuItem.Size = new Size(188, 22);
            clearBatchToolStripMenuItem.Text = "Clear Batch";
            clearBatchToolStripMenuItem.Click += clearBatchToolStripMenuItem_Click;
            // 
            // regenerateClaimFileToolStripMenuItem
            // 
            regenerateClaimFileToolStripMenuItem.Name = "regenerateClaimFileToolStripMenuItem";
            regenerateClaimFileToolStripMenuItem.Size = new Size(188, 22);
            regenerateClaimFileToolStripMenuItem.Text = "Regenerate Claim File";
            regenerateClaimFileToolStripMenuItem.Click += regenerateClaimFileToolStripMenuItem_Click;
            // 
            // claimBatchDetailDataGrid
            // 
            claimBatchDetailDataGrid.AllowUserToAddRows = false;
            claimBatchDetailDataGrid.AllowUserToDeleteRows = false;
            claimBatchDetailDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            claimBatchDetailDataGrid.BackgroundColor = Color.White;
            claimBatchDetailDataGrid.BorderStyle = BorderStyle.None;
            dataGridViewCellStyle4.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = SystemColors.Control;
            dataGridViewCellStyle4.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle4.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle4.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = DataGridViewTriState.True;
            claimBatchDetailDataGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle4;
            claimBatchDetailDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle5.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle5.BackColor = SystemColors.Window;
            dataGridViewCellStyle5.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle5.ForeColor = SystemColors.ControlText;
            dataGridViewCellStyle5.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle5.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle5.WrapMode = DataGridViewTriState.False;
            claimBatchDetailDataGrid.DefaultCellStyle = dataGridViewCellStyle5;
            claimBatchDetailDataGrid.Location = new Point(4, 40);
            claimBatchDetailDataGrid.Margin = new Padding(4, 3, 4, 3);
            claimBatchDetailDataGrid.Name = "claimBatchDetailDataGrid";
            dataGridViewCellStyle6.Alignment = DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = SystemColors.Control;
            dataGridViewCellStyle6.Font = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 0);
            dataGridViewCellStyle6.ForeColor = SystemColors.WindowText;
            dataGridViewCellStyle6.SelectionBackColor = SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = DataGridViewTriState.True;
            claimBatchDetailDataGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle6;
            claimBatchDetailDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            claimBatchDetailDataGrid.Size = new Size(648, 328);
            claimBatchDetailDataGrid.TabIndex = 1;
            claimBatchDetailDataGrid.MouseDoubleClick += claimBatchDetailDataGrid_MouseDoubleClick;
            // 
            // generateClaimsButton
            // 
            generateClaimsButton.Location = new Point(230, 25);
            generateClaimsButton.Margin = new Padding(4, 3, 4, 3);
            generateClaimsButton.Name = "generateClaimsButton";
            generateClaimsButton.Size = new Size(106, 27);
            generateClaimsButton.TabIndex = 2;
            generateClaimsButton.Values.Text = "Generate Claims";
            generateClaimsButton.Click += generateClaimsButton_Click;
            // 
            // institutionalRadioButton
            // 
            institutionalRadioButton.Location = new Point(20, 29);
            institutionalRadioButton.Margin = new Padding(4, 3, 4, 3);
            institutionalRadioButton.Name = "institutionalRadioButton";
            institutionalRadioButton.Size = new Size(87, 20);
            institutionalRadioButton.TabIndex = 3;
            institutionalRadioButton.Values.Text = "Institutional";
            // 
            // professionalRadioButton
            // 
            professionalRadioButton.Location = new Point(133, 29);
            professionalRadioButton.Margin = new Padding(4, 3, 4, 3);
            professionalRadioButton.Name = "professionalRadioButton";
            professionalRadioButton.Size = new Size(88, 20);
            professionalRadioButton.TabIndex = 3;
            professionalRadioButton.Values.Text = "Professional";
            // 
            // claimProgress
            // 
            claimProgress.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            claimProgress.Location = new Point(440, 25);
            claimProgress.Margin = new Padding(4, 3, 4, 3);
            claimProgress.Name = "claimProgress";
            claimProgress.Size = new Size(497, 27);
            claimProgress.TabIndex = 5;
            // 
            // claimProgressStatusLabel
            // 
            claimProgressStatusLabel.AutoSize = true;
            claimProgressStatusLabel.Location = new Point(230, 55);
            claimProgressStatusLabel.Margin = new Padding(4, 0, 4, 0);
            claimProgressStatusLabel.Name = "claimProgressStatusLabel";
            claimProgressStatusLabel.Size = new Size(38, 15);
            claimProgressStatusLabel.TabIndex = 6;
            claimProgressStatusLabel.Text = "label1";
            // 
            // cancelButton
            // 
            cancelButton.Location = new Point(344, 25);
            cancelButton.Margin = new Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(88, 27);
            cancelButton.TabIndex = 7;
            cancelButton.Values.Text = "Cancel";
            cancelButton.Click += cancelButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox1.BackColor = Color.Transparent;
            groupBox1.Controls.Add(cancelButton);
            groupBox1.Controls.Add(generateClaimsButton);
            groupBox1.Controls.Add(institutionalRadioButton);
            groupBox1.Controls.Add(claimProgressStatusLabel);
            groupBox1.Controls.Add(professionalRadioButton);
            groupBox1.Controls.Add(claimProgress);
            groupBox1.Location = new Point(15, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(953, 84);
            groupBox1.TabIndex = 8;
            groupBox1.TabStop = false;
            groupBox1.Text = "Generate Claims";
            // 
            // kryptonPanel1
            // 
            kryptonPanel1.Controls.Add(groupBox1);
            kryptonPanel1.Dock = DockStyle.Top;
            kryptonPanel1.Location = new Point(0, 0);
            kryptonPanel1.Name = "kryptonPanel1";
            kryptonPanel1.PanelBackStyle = Krypton.Toolkit.PaletteBackStyle.HeaderForm;
            kryptonPanel1.Size = new Size(980, 109);
            kryptonPanel1.TabIndex = 9;
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 109);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(kryptonLabel1);
            splitContainer1.Panel1.Controls.Add(claimBatchDataGrid);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(kryptonLabel2);
            splitContainer1.Panel2.Controls.Add(claimBatchDetailDataGrid);
            splitContainer1.Size = new Size(980, 410);
            splitContainer1.SplitterDistance = 311;
            splitContainer1.TabIndex = 10;
            // 
            // kryptonLabel1
            // 
            kryptonLabel1.LabelStyle = Krypton.Toolkit.LabelStyle.BoldPanel;
            kryptonLabel1.Location = new Point(4, 6);
            kryptonLabel1.Name = "kryptonLabel1";
            kryptonLabel1.Size = new Size(91, 20);
            kryptonLabel1.TabIndex = 1;
            kryptonLabel1.Values.Text = "Claim Batches";
            // 
            // kryptonLabel2
            // 
            kryptonLabel2.LabelStyle = Krypton.Toolkit.LabelStyle.BoldControl;
            kryptonLabel2.Location = new Point(3, 6);
            kryptonLabel2.Name = "kryptonLabel2";
            kryptonLabel2.Size = new Size(81, 20);
            kryptonLabel2.TabIndex = 2;
            kryptonLabel2.Values.Text = "Batch Detail";
            // 
            // ClaimsManagementForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = Color.White;
            ClientSize = new Size(980, 519);
            Controls.Add(splitContainer1);
            Controls.Add(kryptonPanel1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "ClaimsManagementForm";
            Text = "Claim Batch Management";
            Load += ClaimsManagementForm_Load;
            ((System.ComponentModel.ISupportInitialize)claimBatchDataGrid).EndInit();
            contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)claimBatchDetailDataGrid).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)kryptonPanel1).EndInit();
            kryptonPanel1.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel1.PerformLayout();
            splitContainer1.Panel2.ResumeLayout(false);
            splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.DataGridView claimBatchDataGrid;
        private System.Windows.Forms.DataGridView claimBatchDetailDataGrid;
        private Krypton.Toolkit.KryptonButton generateClaimsButton;
        private Krypton.Toolkit.KryptonRadioButton institutionalRadioButton;
        private Krypton.Toolkit.KryptonRadioButton professionalRadioButton;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem clearBatchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem regenerateClaimFileToolStripMenuItem;
        private System.Windows.Forms.ProgressBar claimProgress;
        private System.Windows.Forms.Label claimProgressStatusLabel;
        private Krypton.Toolkit.KryptonButton cancelButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private Krypton.Toolkit.KryptonPanel kryptonPanel1;
        private SplitContainer splitContainer1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel1;
        private Krypton.Toolkit.KryptonLabel kryptonLabel2;
    }
}