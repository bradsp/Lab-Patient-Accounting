namespace LabBilling.Forms;

partial class ProcessRemittanceForm
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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ProcessRemittanceForm));
        statusStrip1 = new StatusStrip();
        remittancesDataGridView = new DataGridView();
        contextMenuStrip1 = new ContextMenuStrip(components);
        markRemittancePostedToolStripMenuItem = new ToolStripMenuItem();
        reimportRemittanceToolStripMenuItem = new ToolStripMenuItem();
        reversePostingToolStripMenuItem = new ToolStripMenuItem();
        toolStrip1 = new ToolStrip();
        importRemittancesToolstripButton = new ToolStripButton();
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).BeginInit();
        contextMenuStrip1.SuspendLayout();
        toolStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // statusStrip1
        // 
        statusStrip1.Location = new Point(0, 428);
        statusStrip1.Name = "statusStrip1";
        statusStrip1.Size = new Size(800, 22);
        statusStrip1.TabIndex = 1;
        statusStrip1.Text = "statusStrip1";
        // 
        // remittancesDataGridView
        // 
        remittancesDataGridView.AllowUserToAddRows = false;
        remittancesDataGridView.AllowUserToDeleteRows = false;
        remittancesDataGridView.AllowUserToResizeRows = false;
        remittancesDataGridView.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        remittancesDataGridView.BackgroundColor = SystemColors.ButtonHighlight;
        remittancesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        remittancesDataGridView.ContextMenuStrip = contextMenuStrip1;
        remittancesDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        remittancesDataGridView.Location = new Point(0, 31);
        remittancesDataGridView.MultiSelect = false;
        remittancesDataGridView.Name = "remittancesDataGridView";
        remittancesDataGridView.ReadOnly = true;
        remittancesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        remittancesDataGridView.Size = new Size(800, 394);
        remittancesDataGridView.TabIndex = 2;
        remittancesDataGridView.CellMouseDoubleClick += remittancesDataGridView_CellMouseDoubleClick;
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Items.AddRange(new ToolStripItem[] { markRemittancePostedToolStripMenuItem, reimportRemittanceToolStripMenuItem, reversePostingToolStripMenuItem });
        contextMenuStrip1.Name = "contextMenuStrip1";
        contextMenuStrip1.Size = new Size(204, 70);
        // 
        // markRemittancePostedToolStripMenuItem
        // 
        markRemittancePostedToolStripMenuItem.Name = "markRemittancePostedToolStripMenuItem";
        markRemittancePostedToolStripMenuItem.Size = new Size(203, 22);
        markRemittancePostedToolStripMenuItem.Text = "Mark Remittance Posted";
        markRemittancePostedToolStripMenuItem.Click += markRemittancePostedToolStripMenuItem_Click;
        // 
        // reimportRemittanceToolStripMenuItem
        // 
        reimportRemittanceToolStripMenuItem.Name = "reimportRemittanceToolStripMenuItem";
        reimportRemittanceToolStripMenuItem.Size = new Size(203, 22);
        reimportRemittanceToolStripMenuItem.Text = "Reimport Remittance";
        reimportRemittanceToolStripMenuItem.Click += reimportRemittanceToolStripMenuItem_Click;
        // 
        // reversePostingToolStripMenuItem
        // 
        reversePostingToolStripMenuItem.Name = "reversePostingToolStripMenuItem";
        reversePostingToolStripMenuItem.Size = new Size(203, 22);
        reversePostingToolStripMenuItem.Text = "Reverse Posting";
        reversePostingToolStripMenuItem.Click += reversePostingToolStripMenuItem_Click;
        // 
        // toolStrip1
        // 
        toolStrip1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        toolStrip1.Dock = DockStyle.None;
        toolStrip1.Items.AddRange(new ToolStripItem[] { importRemittancesToolstripButton });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(162, 28);
        toolStrip1.TabIndex = 3;
        toolStrip1.Text = "toolStrip1";
        // 
        // importRemittancesToolstripButton
        // 
        importRemittancesToolstripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        importRemittancesToolstripButton.Font = new Font("Segoe UI", 12F);
        importRemittancesToolstripButton.Image = (Image)resources.GetObject("importRemittancesToolstripButton.Image");
        importRemittancesToolstripButton.ImageTransparentColor = Color.Magenta;
        importRemittancesToolstripButton.Name = "importRemittancesToolstripButton";
        importRemittancesToolstripButton.Size = new Size(150, 25);
        importRemittancesToolstripButton.Text = "Import Remittances";
        importRemittancesToolstripButton.Click += importRemittancesToolStripMenuItem_Click;
        // 
        // ProcessRemittanceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(toolStrip1);
        Controls.Add(remittancesDataGridView);
        Controls.Add(statusStrip1);
        Name = "ProcessRemittanceForm";
        Text = "ProcessRemittanceForm";
        Load += ProcessRemittanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).EndInit();
        contextMenuStrip1.ResumeLayout(false);
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private StatusStrip statusStrip1;
    private DataGridView remittancesDataGridView;
    private ToolStrip toolStrip1;
    private ToolStripButton importRemittancesToolstripButton;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem markRemittancePostedToolStripMenuItem;
    private ToolStripMenuItem reimportRemittanceToolStripMenuItem;
    private ToolStripMenuItem reversePostingToolStripMenuItem;
}