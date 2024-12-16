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
        // PostRemittanceForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        ClientSize = new Size(1336, 566);
        Controls.Add(remittancesDataGridView);
        Name = "PostRemittanceForm";
        Text = "PostRemittanceForm";
        Load += PostRemittanceForm_Load;
        ((System.ComponentModel.ISupportInitialize)remittancesDataGridView).EndInit();
        ResumeLayout(false);
    }

    #endregion

    private DataGridView remittancesDataGridView;
}