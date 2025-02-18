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
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AuditReportMaintenanceForm));
        reportListDataGrid = new DataGridView();
        reportNameLabel = new Label();
        reportNameTextbox = new TextBox();
        reportTitleLabel = new Label();
        reportTitleTextbox = new TextBox();
        buttonLabel = new Label();
        buttonTextBox = new TextBox();
        reportCodeLabel = new Label();
        reportCodeTextbox = new TextBox();
        isChildButtonCheckBox = new CheckBox();
        commentLabel = new Label();
        commentsTextbox = new TextBox();
        saveButton = new Button();
        cancelButton = new Button();
        toolStrip1 = new ToolStrip();
        newReportToolStripButton = new ToolStripButton();
        deleteToolStripButton = new ToolStripButton();
        menuSelectionComboBox = new ToolStripComboBox();
        idLabel = new Label();
        validationErrorsLabel = new Label();
        validationErrorsTextBox = new TextBox();
        fixSqlCodeButton = new Button();
        ((System.ComponentModel.ISupportInitialize)reportListDataGrid).BeginInit();
        toolStrip1.SuspendLayout();
        SuspendLayout();
        // 
        // reportListDataGrid
        // 
        reportListDataGrid.AllowUserToAddRows = false;
        reportListDataGrid.AllowUserToDeleteRows = false;
        reportListDataGrid.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        reportListDataGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        reportListDataGrid.Location = new Point(17, 35);
        reportListDataGrid.MultiSelect = false;
        reportListDataGrid.Name = "reportListDataGrid";
        reportListDataGrid.ReadOnly = true;
        reportListDataGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        reportListDataGrid.Size = new Size(988, 150);
        reportListDataGrid.TabIndex = 0;
        reportListDataGrid.SelectionChanged += reportListDataGrid_SelectionChanged;
        // 
        // reportNameLabel
        // 
        reportNameLabel.AutoSize = true;
        reportNameLabel.Location = new Point(17, 198);
        reportNameLabel.Name = "reportNameLabel";
        reportNameLabel.Size = new Size(77, 15);
        reportNameLabel.TabIndex = 1;
        reportNameLabel.Text = "Report Name";
        // 
        // reportNameTextbox
        // 
        reportNameTextbox.Location = new Point(17, 216);
        reportNameTextbox.Name = "reportNameTextbox";
        reportNameTextbox.Size = new Size(266, 23);
        reportNameTextbox.TabIndex = 2;
        // 
        // reportTitleLabel
        // 
        reportTitleLabel.AutoSize = true;
        reportTitleLabel.Location = new Point(289, 198);
        reportTitleLabel.Name = "reportTitleLabel";
        reportTitleLabel.Size = new Size(67, 15);
        reportTitleLabel.TabIndex = 1;
        reportTitleLabel.Text = "Report Title";
        // 
        // reportTitleTextbox
        // 
        reportTitleTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
        reportTitleTextbox.Location = new Point(289, 216);
        reportTitleTextbox.Name = "reportTitleTextbox";
        reportTitleTextbox.Size = new Size(331, 23);
        reportTitleTextbox.TabIndex = 2;
        // 
        // buttonLabel
        // 
        buttonLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        buttonLabel.AutoSize = true;
        buttonLabel.Location = new Point(626, 198);
        buttonLabel.Name = "buttonLabel";
        buttonLabel.Size = new Size(43, 15);
        buttonLabel.TabIndex = 1;
        buttonLabel.Text = "Button";
        // 
        // buttonTextBox
        // 
        buttonTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        buttonTextBox.Location = new Point(626, 216);
        buttonTextBox.Name = "buttonTextBox";
        buttonTextBox.Size = new Size(266, 23);
        buttonTextBox.TabIndex = 2;
        // 
        // reportCodeLabel
        // 
        reportCodeLabel.AutoSize = true;
        reportCodeLabel.Location = new Point(17, 247);
        reportCodeLabel.Name = "reportCodeLabel";
        reportCodeLabel.Size = new Size(73, 15);
        reportCodeLabel.TabIndex = 1;
        reportCodeLabel.Text = "Report Code";
        // 
        // reportCodeTextbox
        // 
        reportCodeTextbox.AcceptsReturn = true;
        reportCodeTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
        reportCodeTextbox.Location = new Point(17, 265);
        reportCodeTextbox.Multiline = true;
        reportCodeTextbox.Name = "reportCodeTextbox";
        reportCodeTextbox.ScrollBars = ScrollBars.Both;
        reportCodeTextbox.Size = new Size(629, 257);
        reportCodeTextbox.TabIndex = 2;
        // 
        // isChildButtonCheckBox
        // 
        isChildButtonCheckBox.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        isChildButtonCheckBox.AutoSize = true;
        isChildButtonCheckBox.Location = new Point(901, 216);
        isChildButtonCheckBox.Name = "isChildButtonCheckBox";
        isChildButtonCheckBox.Size = new Size(104, 19);
        isChildButtonCheckBox.TabIndex = 3;
        isChildButtonCheckBox.Text = "Is Child Button";
        isChildButtonCheckBox.UseVisualStyleBackColor = true;
        // 
        // commentLabel
        // 
        commentLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
        commentLabel.AutoSize = true;
        commentLabel.Location = new Point(652, 247);
        commentLabel.Name = "commentLabel";
        commentLabel.Size = new Size(38, 15);
        commentLabel.TabIndex = 1;
        commentLabel.Text = "Notes";
        // 
        // commentsTextbox
        // 
        commentsTextbox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
        commentsTextbox.Location = new Point(652, 265);
        commentsTextbox.Multiline = true;
        commentsTextbox.Name = "commentsTextbox";
        commentsTextbox.ScrollBars = ScrollBars.Vertical;
        commentsTextbox.Size = new Size(353, 118);
        commentsTextbox.TabIndex = 2;
        // 
        // saveButton
        // 
        saveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        saveButton.Location = new Point(849, 528);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(75, 23);
        saveButton.TabIndex = 4;
        saveButton.Text = "Save";
        saveButton.UseVisualStyleBackColor = true;
        saveButton.Click += saveButton_Click;
        // 
        // cancelButton
        // 
        cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        cancelButton.Location = new Point(930, 528);
        cancelButton.Name = "cancelButton";
        cancelButton.Size = new Size(75, 23);
        cancelButton.TabIndex = 4;
        cancelButton.Text = "Cancel";
        cancelButton.UseVisualStyleBackColor = true;
        cancelButton.Click += cancelButton_Click;
        // 
        // toolStrip1
        // 
        toolStrip1.Items.AddRange(new ToolStripItem[] { newReportToolStripButton, deleteToolStripButton, menuSelectionComboBox });
        toolStrip1.Location = new Point(0, 0);
        toolStrip1.Name = "toolStrip1";
        toolStrip1.Size = new Size(1014, 25);
        toolStrip1.TabIndex = 5;
        toolStrip1.Text = "toolStrip1";
        // 
        // newReportToolStripButton
        // 
        newReportToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        newReportToolStripButton.Image = (Image)resources.GetObject("newReportToolStripButton.Image");
        newReportToolStripButton.ImageTransparentColor = Color.Magenta;
        newReportToolStripButton.Name = "newReportToolStripButton";
        newReportToolStripButton.Size = new Size(73, 22);
        newReportToolStripButton.Text = "New Report";
        // 
        // deleteToolStripButton
        // 
        deleteToolStripButton.DisplayStyle = ToolStripItemDisplayStyle.Text;
        deleteToolStripButton.Image = (Image)resources.GetObject("deleteToolStripButton.Image");
        deleteToolStripButton.ImageTransparentColor = Color.Magenta;
        deleteToolStripButton.Name = "deleteToolStripButton";
        deleteToolStripButton.Size = new Size(82, 22);
        deleteToolStripButton.Text = "Delete Report";
        deleteToolStripButton.Click += deleteToolStripButton_Click;
        // 
        // menuSelectionComboBox
        // 
        menuSelectionComboBox.Name = "menuSelectionComboBox";
        menuSelectionComboBox.Size = new Size(121, 25);
        // 
        // idLabel
        // 
        idLabel.AutoSize = true;
        idLabel.Location = new Point(245, 198);
        idLabel.Name = "idLabel";
        idLabel.Size = new Size(16, 15);
        idLabel.TabIndex = 6;
        idLabel.Text = "...";
        // 
        // validationErrorsLabel
        // 
        validationErrorsLabel.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        validationErrorsLabel.AutoSize = true;
        validationErrorsLabel.Location = new Point(652, 386);
        validationErrorsLabel.Name = "validationErrorsLabel";
        validationErrorsLabel.Size = new Size(92, 15);
        validationErrorsLabel.TabIndex = 7;
        validationErrorsLabel.Text = "Validation Errors";
        // 
        // validationErrorsTextBox
        // 
        validationErrorsTextBox.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
        validationErrorsTextBox.Location = new Point(652, 404);
        validationErrorsTextBox.Multiline = true;
        validationErrorsTextBox.Name = "validationErrorsTextBox";
        validationErrorsTextBox.Size = new Size(350, 118);
        validationErrorsTextBox.TabIndex = 8;
        // 
        // fixSqlCodeButton
        // 
        fixSqlCodeButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
        fixSqlCodeButton.Location = new Point(17, 528);
        fixSqlCodeButton.Name = "fixSqlCodeButton";
        fixSqlCodeButton.Size = new Size(75, 23);
        fixSqlCodeButton.TabIndex = 9;
        fixSqlCodeButton.Text = "Fix SQL Code";
        fixSqlCodeButton.UseVisualStyleBackColor = true;
        fixSqlCodeButton.Click += fixSqlCodeButton_Click;
        // 
        // AuditReportMaintenanceForm
        // 
        AcceptButton = saveButton;
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        CancelButton = cancelButton;
        ClientSize = new Size(1014, 562);
        Controls.Add(fixSqlCodeButton);
        Controls.Add(validationErrorsTextBox);
        Controls.Add(validationErrorsLabel);
        Controls.Add(idLabel);
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
        Controls.Add(toolStrip1);
        Name = "AuditReportMaintenanceForm";
        Text = "Audit Report Maintenance";
        Load += AuditReportMaintenanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)reportListDataGrid).EndInit();
        toolStrip1.ResumeLayout(false);
        toolStrip1.PerformLayout();
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
    private System.Windows.Forms.ToolStrip toolStrip1;
    private System.Windows.Forms.ToolStripButton newReportToolStripButton;
    private System.Windows.Forms.Label idLabel;
    private System.Windows.Forms.ToolStripButton deleteToolStripButton;
    private ToolStripComboBox menuSelectionComboBox;
    private Label validationErrorsLabel;
    private TextBox validationErrorsTextBox;
    private Button fixSqlCodeButton;
}