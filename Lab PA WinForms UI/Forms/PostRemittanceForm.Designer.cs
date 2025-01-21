namespace LabBilling.Forms;

partial class PostRemittanceForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PostRemittanceForm));
        claimDataGridView = new DataGridView();
        toolStrip1 = new ToolStrip();
        postRemittanceToolButton = new ToolStripButton();
        printRemittanceToolStripButton = new ToolStripButton();
        statusStrip1 = new StatusStrip();
        splitContainer1 = new SplitContainer();
        ((System.ComponentModel.ISupportInitialize)claimDataGridView).BeginInit();
        toolStrip1.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
        splitContainer1.Panel2.SuspendLayout();
        splitContainer1.SuspendLayout();
        SuspendLayout();
        // 
        // claimDataGridView
        // 
        claimDataGridView.AllowUserToAddRows = false;
        claimDataGridView.AllowUserToDeleteRows = false;
        claimDataGridView.AllowUserToResizeRows = false;
        claimDataGridView.BackgroundColor = SystemColors.ButtonHighlight;
        claimDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        claimDataGridView.Dock = DockStyle.Fill;
        claimDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        claimDataGridView.Location = new Point(0, 0);
        claimDataGridView.MultiSelect = false;
        claimDataGridView.Name = "claimDataGridView";
        claimDataGridView.RowTemplate.Height = 25;
        claimDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        claimDataGridView.Size = new Size(1026, 171);
        claimDataGridView.TabIndex = 2;
        claimDataGridView.CellMouseDoubleClick += claimDataGridView_CellMouseDoubleClick;
        // 
        // toolStrip1
        // 
        toolStrip1.Items.AddRange(new ToolStripItem[] { postRemittanceToolButton, printRemittanceToolStripButton });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(1026, 48);
        toolStrip1.TabIndex = 3;
        toolStrip1.Text = "toolStrip1";
        // 
        // postRemittanceToolButton
        // 
        postRemittanceToolButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        postRemittanceToolButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        postRemittanceToolButton.Image = (Image)resources.GetObject("postRemittanceToolButton.Image");
        postRemittanceToolButton.ImageTransparentColor = Color.Magenta;
        postRemittanceToolButton.Name = "postRemittanceToolButton";
        postRemittanceToolButton.Padding = new Padding(10);
        postRemittanceToolButton.Size = new Size(145, 45);
        postRemittanceToolButton.Text = "Post Remittance";
        postRemittanceToolButton.ToolTipText = "Post Selected Remittance";
        postRemittanceToolButton.Click += postRemittanceToolButton_Click;
        // 
        // printRemittanceToolStripButton
        // 
        printRemittanceToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        printRemittanceToolStripButton.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
        printRemittanceToolStripButton.Image = (Image)resources.GetObject("printRemittanceToolStripButton.Image");
        printRemittanceToolStripButton.ImageTransparentColor = Color.Magenta;
        printRemittanceToolStripButton.Name = "printRemittanceToolStripButton";
        printRemittanceToolStripButton.Size = new Size(129, 45);
        printRemittanceToolStripButton.Text = "Print Remittance";
        printRemittanceToolStripButton.Click += printRemittanceToolStripButton_Click;
        // 
        // statusStrip1
        // 
        statusStrip1.Location = new Point(0, 544);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(1026, 22);
        statusStrip1.TabIndex = 4;
        statusStrip1.Text = "statusStrip1";
        // 
        // splitContainer1
        // 
        splitContainer1.Dock = DockStyle.Fill;
        splitContainer1.Location = new Point(0, 48);
        splitContainer1.Name = "splitContainer1";
        splitContainer1.Orientation = Orientation.Horizontal;
        // 
        // splitContainer1.Panel2
        // 
        splitContainer1.Panel2.Controls.Add(claimDataGridView);
        splitContainer1.Size = new Size(1026, 496);
        splitContainer1.SplitterDistance = 321;
        splitContainer1.TabIndex = 6;
        // 
        // PostRemittanceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1026, 566);
        Controls.Add(splitContainer1);
        Controls.Add(statusStrip1);
        Controls.Add(toolStrip1);
        Name = "PostRemittanceForm";
        Text = "Post Remittance";
        Load += PostRemittanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)claimDataGridView).EndInit();
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        splitContainer1.Panel2.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
        splitContainer1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private DataGridView claimDataGridView;
    private ToolStrip toolStrip1;
    private ToolStripButton postRemittanceToolButton;
    private StatusStrip statusStrip1;
    private ToolStripButton printRemittanceToolStripButton;
    private SplitContainer splitContainer1;
}