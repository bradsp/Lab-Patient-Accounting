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
        remittancesDataGridView = new DataGridView();
        tableLayoutPanel1 = new TableLayoutPanel();
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).BeginInit();
        SuspendLayout();
        // 
        // remittancesDataGridView
        // 
        remittancesDataGridView.AllowUserToAddRows = false;
        remittancesDataGridView.AllowUserToDeleteRows = false;
        remittancesDataGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
        remittancesDataGridView.EditMode = DataGridViewEditMode.EditProgrammatically;
        remittancesDataGridView.Location = new Point(12, 12);
        remittancesDataGridView.Name = "remittancesDataGridView";
        remittancesDataGridView.RowTemplate.Height = 25;
        remittancesDataGridView.Size = new Size(1312, 154);
        remittancesDataGridView.TabIndex = 0;
        remittancesDataGridView.SelectionChanged += remittancesDataGridView_SelectionChanged;
        // 
        // tableLayoutPanel1
        // 
        tableLayoutPanel1.ColumnCount = 4;
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
        tableLayoutPanel1.Location = new Point(12, 181);
        tableLayoutPanel1.Name = "tableLayoutPanel1";
        tableLayoutPanel1.RowCount = 2;
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 48.1481476F));
        tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 51.8518524F));
        tableLayoutPanel1.Size = new Size(646, 54);
        tableLayoutPanel1.TabIndex = 1;
        // 
        // PostRemittanceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1336, 566);
        Controls.Add(tableLayoutPanel1);
        Controls.Add(remittancesDataGridView);
        Name = "PostRemittanceForm";
        Text = "PostRemittanceForm";
        Load += PostRemittanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView remittancesDataGridView;
    private TableLayoutPanel tableLayoutPanel1;
}