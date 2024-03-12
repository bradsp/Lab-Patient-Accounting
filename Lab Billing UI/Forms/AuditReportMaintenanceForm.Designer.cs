namespace LabBilling.Forms;

partial class AuditReportMaintenanceForm
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
        reportListDataGrid = new System.Windows.Forms.DataGridView();
        reportNameLabel = new System.Windows.Forms.Label();
        reportNameTextbox = new System.Windows.Forms.TextBox();
        reportTitleLabel = new System.Windows.Forms.Label();
        reportTitleTextbox = new System.Windows.Forms.TextBox();
        buttonLabel = new System.Windows.Forms.Label();
        buttonTextBox = new System.Windows.Forms.TextBox();
        reportCodeLabel = new System.Windows.Forms.Label();
        reportCodeTextbox = new System.Windows.Forms.TextBox();
        isChildButtonCheckBox = new System.Windows.Forms.CheckBox();
        commentLabel = new System.Windows.Forms.Label();
        commentsTextbox = new System.Windows.Forms.TextBox();
        saveButton = new System.Windows.Forms.Button();
        cancelButton = new System.Windows.Forms.Button();
        menuStrip1 = new System.Windows.Forms.MenuStrip();
        addReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
        ((System.ComponentModel.ISupportInitialize)reportListDataGrid).BeginInit();
        menuStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // reportListDataGrid
        // 
        reportListDataGrid.AllowUserToAddRows = false;
        reportListDataGrid.AllowUserToDeleteRows = false;
        reportListDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        reportListDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        reportListDataGrid.Location = new System.Drawing.Point(17, 35);
        reportListDataGrid.MultiSelect = false;
        reportListDataGrid.Name = "reportListDataGrid";
        reportListDataGrid.ReadOnly = true;
        reportListDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
        reportListDataGrid.Size = new System.Drawing.Size(988, 150);
        reportListDataGrid.TabIndex = 0;
        reportListDataGrid.SelectionChanged += reportListDataGrid_SelectionChanged;
        // 
        // reportNameLabel
        // 
        reportNameLabel.AutoSize = true;
        reportNameLabel.Location = new System.Drawing.Point(17, 198);
        reportNameLabel.Name = "reportNameLabel";
        reportNameLabel.Size = new System.Drawing.Size(77, 15);
        reportNameLabel.TabIndex = 1;
        reportNameLabel.Text = "Report Name";
        // 
        // reportNameTextbox
        // 
        reportNameTextbox.Location = new System.Drawing.Point(17, 216);
        reportNameTextbox.Name = "reportNameTextbox";
        reportNameTextbox.Size = new System.Drawing.Size(266, 23);
        reportNameTextbox.TabIndex = 2;
        // 
        // reportTitleLabel
        // 
        reportTitleLabel.AutoSize = true;
        reportTitleLabel.Location = new System.Drawing.Point(289, 198);
        reportTitleLabel.Name = "reportTitleLabel";
        reportTitleLabel.Size = new System.Drawing.Size(67, 15);
        reportTitleLabel.TabIndex = 1;
        reportTitleLabel.Text = "Report Title";
        // 
        // reportTitleTextbox
        // 
        reportTitleTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        reportTitleTextbox.Location = new System.Drawing.Point(289, 216);
        reportTitleTextbox.Name = "reportTitleTextbox";
        reportTitleTextbox.Size = new System.Drawing.Size(331, 23);
        reportTitleTextbox.TabIndex = 2;
        // 
        // buttonLabel
        // 
        buttonLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        buttonLabel.AutoSize = true;
        buttonLabel.Location = new System.Drawing.Point(626, 198);
        buttonLabel.Name = "buttonLabel";
        buttonLabel.Size = new System.Drawing.Size(43, 15);
        buttonLabel.TabIndex = 1;
        buttonLabel.Text = "Button";
        // 
        // buttonTextBox
        // 
        buttonTextBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        buttonTextBox.Location = new System.Drawing.Point(626, 216);
        buttonTextBox.Name = "buttonTextBox";
        buttonTextBox.Size = new System.Drawing.Size(266, 23);
        buttonTextBox.TabIndex = 2;
        // 
        // reportCodeLabel
        // 
        reportCodeLabel.AutoSize = true;
        reportCodeLabel.Location = new System.Drawing.Point(17, 247);
        reportCodeLabel.Name = "reportCodeLabel";
        reportCodeLabel.Size = new System.Drawing.Size(73, 15);
        reportCodeLabel.TabIndex = 1;
        reportCodeLabel.Text = "Report Code";
        // 
        // reportCodeTextbox
        // 
        reportCodeTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
        reportCodeTextbox.Location = new System.Drawing.Point(17, 265);
        reportCodeTextbox.Multiline = true;
        reportCodeTextbox.Name = "reportCodeTextbox";
        reportCodeTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
        reportCodeTextbox.Size = new System.Drawing.Size(716, 257);
        reportCodeTextbox.TabIndex = 2;
        // 
        // isChildButtonCheckBox
        // 
        isChildButtonCheckBox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        isChildButtonCheckBox.AutoSize = true;
        isChildButtonCheckBox.Location = new System.Drawing.Point(901, 216);
        isChildButtonCheckBox.Name = "isChildButtonCheckBox";
        isChildButtonCheckBox.Size = new System.Drawing.Size(104, 19);
        isChildButtonCheckBox.TabIndex = 3;
        isChildButtonCheckBox.Text = "Is Child Button";
        isChildButtonCheckBox.UseVisualStyleBackColor = true;
        // 
        // commentLabel
        // 
        commentLabel.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
        commentLabel.AutoSize = true;
        commentLabel.Location = new System.Drawing.Point(739, 247);
        commentLabel.Name = "commentLabel";
        commentLabel.Size = new System.Drawing.Size(38, 15);
        commentLabel.TabIndex = 1;
        commentLabel.Text = "Notes";
        // 
        // commentsTextbox
        // 
        commentsTextbox.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        commentsTextbox.Location = new System.Drawing.Point(739, 265);
        commentsTextbox.Multiline = true;
        commentsTextbox.Name = "commentsTextbox";
        commentsTextbox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
        commentsTextbox.Size = new System.Drawing.Size(266, 257);
        commentsTextbox.TabIndex = 2;
        // 
        // saveButton
        // 
        saveButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        saveButton.Location = new System.Drawing.Point(849, 528);
        saveButton.Name = "saveButton";
        saveButton.Size = new System.Drawing.Size(75, 23);
        saveButton.TabIndex = 4;
        saveButton.Text = "Save";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click += saveButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
        cancelButton.Location = new System.Drawing.Point(930, 528);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new System.Drawing.Size(75, 23);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // menuStrip1
        // 
        menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { addReportToolStripMenuItem });
        menuStrip1.Location = new System.Drawing.Point(0, 0);
        menuStrip1.Name = "menuStrip1";
        menuStrip1.Size = new System.Drawing.Size(1014, 24);
        menuStrip1.TabIndex = 5;
        menuStrip1.Text = "menuStrip1";
        // 
        // addReportToolStripMenuItem
        // 
        addReportToolStripMenuItem.Name = "addReportToolStripMenuItem";
        addReportToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
        addReportToolStripMenuItem.Text = "Add Report";
        // 
        // AuditReportMaintenanceForm
        // 
        AcceptButton = saveButton;
        AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
        AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new System.Drawing.Size(1014, 562);
        Controls.Add(cancelButton);
        Controls.Add(saveButton);
        Controls.Add(isChildButtonCheckBox);
        Controls.Add(reportTitleTextbox);
        Controls.Add(reportTitleLabel);
        Controls.Add(commentsTextbox);
        Controls.Add(commentLabel);
        Controls.Add(reportCodeTextbox);
        Controls.Add(reportCodeLabel);
        Controls.Add(buttonTextBox);
        Controls.Add(buttonLabel);
        Controls.Add(reportNameTextbox);
        Controls.Add(reportNameLabel);
        Controls.Add(reportListDataGrid);
        Controls.Add(menuStrip1);
        MainMenuStrip = menuStrip1;
        Name = "AuditReportMaintenanceForm";
        Text = "Audit Report Maintenance";
        Load += AuditReportMaintenanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)reportListDataGrid).EndInit();
        menuStrip1.ResumeLayout(false);
        menuStrip1.PerformLayout();
        ResumeLayout(false);
        PerformLayout();
    }

    #endregion

    private System.Windows.Forms.DataGridView reportListDataGrid;
    private System.Windows.Forms.Label reportNameLabel;
    private System.Windows.Forms.TextBox reportNameTextbox;
    private System.Windows.Forms.Label reportTitleLabel;
    private System.Windows.Forms.TextBox reportTitleTextbox;
    private System.Windows.Forms.Label buttonLabel;
    private System.Windows.Forms.TextBox buttonTextBox;
    private System.Windows.Forms.Label reportCodeLabel;
    private System.Windows.Forms.TextBox reportCodeTextbox;
    private System.Windows.Forms.CheckBox isChildButtonCheckBox;
    private System.Windows.Forms.Label commentLabel;
    private System.Windows.Forms.TextBox commentsTextbox;
    private System.Windows.Forms.Button saveButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.MenuStrip menuStrip1;
    private System.Windows.Forms.ToolStripMenuItem addReportToolStripMenuItem;
}