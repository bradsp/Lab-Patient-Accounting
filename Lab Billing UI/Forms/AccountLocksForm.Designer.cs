namespace LabBilling.Forms;

partial class AccountLocksForm
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
        accountLockDataGrid = new DataGridView();
        toolStrip1 = new ToolStrip();
        clearLockToolStripButton = new ToolStripButton();
        refreshToolStripButton = new ToolStripButton();
        label1 = new Label();
        ((System.ComponentModel.ISupportInitialize)accountLockDataGrid).BeginInit();
        toolStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // accountLockDataGrid
        // 
        accountLockDataGrid.AllowUserToAddRows = false;
        accountLockDataGrid.AllowUserToDeleteRows = false;
        accountLockDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        accountLockDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        accountLockDataGrid.Location = new Point(12, 55);
        accountLockDataGrid.MultiSelect = false;
        accountLockDataGrid.Name = "accountLockDataGrid";
        accountLockDataGrid.RowTemplate.Height = 25;
        accountLockDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        accountLockDataGrid.ShowEditingIcon = false;
        accountLockDataGrid.Size = new Size(709, 252);
        accountLockDataGrid.TabIndex = 0;
        accountLockDataGrid.SelectionChanged += accountLockDataGrid_SelectionChanged;
        // 
        // toolStrip1
        // 
        toolStrip1.Items.AddRange(new ToolStripItem[] { clearLockToolStripButton, refreshToolStripButton });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(733, 25);
        toolStrip1.TabIndex = 1;
        toolStrip1.Text = "toolStrip1";
        // 
        // clearLockToolStripButton
        // 
        clearLockToolStripButton.Enabled = false;
        clearLockToolStripButton.Image = Properties.Resources.Cancel;
        clearLockToolStripButton.ImageTransparentColor = Color.Magenta;
        clearLockToolStripButton.Name = "clearLockToolStripButton";
        clearLockToolStripButton.Size = new Size(82, 22);
        clearLockToolStripButton.Text = "Clear Lock";
        clearLockToolStripButton.ToolTipText = "Clear Lock";
        clearLockToolStripButton.Click += clearLockToolStripButton_Click;
        // 
        // refreshToolStripButton
        // 
        refreshToolStripButton.Image = Properties.Resources.refresh_icon;
        refreshToolStripButton.ImageTransparentColor = Color.Magenta;
        refreshToolStripButton.Name = "refreshToolStripButton";
        refreshToolStripButton.Size = new Size(66, 22);
        refreshToolStripButton.Text = "Refresh";
        refreshToolStripButton.Click += refreshToolStripButton_Click;
        // 
        // label1
        // 
        label1.AutoSize = true;
        label1.Location = new Point(12, 37);
        label1.Name = "label1";
        label1.Size = new Size(282, 15);
        label1.TabIndex = 2;
        label1.Text = "Highlight row and click Clear Lock to remove a lock.";
        // 
        // AccountLocksForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(733, 319);
        Controls.Add(label1);
        Controls.Add(toolStrip1);
        Controls.Add(accountLockDataGrid);
        Name = "AccountLocksForm";
        Text = "Account Locks";
        Load += AccountLocksForm_Load;
        ((System.ComponentModel.ISupportInitialize)accountLockDataGrid).EndInit();
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private DataGridView accountLockDataGrid;
    private ToolStrip toolStrip1;
    private ToolStripButton clearLockToolStripButton;
    private Label label1;
    private ToolStripButton refreshToolStripButton;
}