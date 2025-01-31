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
        statusStrip1 = new StatusStrip();
        remittancesDataGridView = new DataGridView();
        contextMenuStrip1 = new ContextMenuStrip(components);
        markRemittancePostedToolStripMenuItem = new ToolStripMenuItem();
        reimportRemittanceToolStripMenuItem = new ToolStripMenuItem();
        viewSourceDataToolStripMenuItem = new ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).BeginInit();
        contextMenuStrip1.SuspendLayout();
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
        remittancesDataGridView.BackgroundColor = SystemColors.ButtonHighlight;
        remittancesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        remittancesDataGridView.ContextMenuStrip = contextMenuStrip1;
        remittancesDataGridView.Dock = DockStyle.Fill;
        remittancesDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        remittancesDataGridView.Location = new Point(0, 0);
        remittancesDataGridView.MultiSelect = false;
        remittancesDataGridView.Name = "remittancesDataGridView";
        remittancesDataGridView.ReadOnly = true;
        remittancesDataGridView.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        remittancesDataGridView.Size = new Size(800, 428);
        remittancesDataGridView.TabIndex = 2;
        remittancesDataGridView.CellMouseDoubleClick += remittancesDataGridView_CellMouseDoubleClick;
        // 
        // contextMenuStrip1
        // 
        contextMenuStrip1.Items.AddRange(new ToolStripItem[] { markRemittancePostedToolStripMenuItem, reimportRemittanceToolStripMenuItem, viewSourceDataToolStripMenuItem });
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
        // viewSourceDataToolStripMenuItem
        // 
        viewSourceDataToolStripMenuItem.Name = "viewSourceDataToolStripMenuItem";
        viewSourceDataToolStripMenuItem.Size = new Size(203, 22);
        viewSourceDataToolStripMenuItem.Text = "View Source Data";
        viewSourceDataToolStripMenuItem.Click += viewSourceDataToolStripMenuItem_Click;
        // 
        // ProcessRemittanceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(800, 450);
        Controls.Add(remittancesDataGridView);
        Controls.Add(statusStrip1);
        Name = "ProcessRemittanceForm";
        Text = "Process Remittances";
        Load += ProcessRemittanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).EndInit();
        contextMenuStrip1.ResumeLayout(false);
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion
    private StatusStrip statusStrip1;
    private DataGridView remittancesDataGridView;
    private ContextMenuStrip contextMenuStrip1;
    private ToolStripMenuItem markRemittancePostedToolStripMenuItem;
    private ToolStripMenuItem reimportRemittanceToolStripMenuItem;
    private ToolStripMenuItem viewSourceDataToolStripMenuItem;
}