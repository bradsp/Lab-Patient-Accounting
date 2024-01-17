namespace LabBilling.LookupForms
{
    partial class ProviderLookupForm
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
            cancelButton = new System.Windows.Forms.Button();
            selectButton = new System.Windows.Forms.Button();
            searchLabel = new System.Windows.Forms.Label();
            searchTextBox = new System.Windows.Forms.TextBox();
            resultsDataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)resultsDataGrid).BeginInit();
            SuspendLayout();
            // 
            // cancelButton
            // 
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cancelButton.Location = new System.Drawing.Point(107, 394);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 27);
            cancelButton.TabIndex = 9;
            cancelButton.Text = "&Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // selectButton
            // 
            selectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            selectButton.Location = new System.Drawing.Point(13, 394);
            selectButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            selectButton.Name = "selectButton";
            selectButton.Size = new System.Drawing.Size(88, 27);
            selectButton.TabIndex = 8;
            selectButton.Text = "&Select";
            selectButton.UseVisualStyleBackColor = true;
            selectButton.Click += selectButton_Click;
            // 
            // searchLabel
            // 
            searchLabel.AutoSize = true;
            searchLabel.Location = new System.Drawing.Point(13, 28);
            searchLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            searchLabel.Name = "searchLabel";
            searchLabel.Size = new System.Drawing.Size(42, 15);
            searchLabel.TabIndex = 6;
            searchLabel.Text = "Search";
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new System.Drawing.Point(68, 24);
            searchTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new System.Drawing.Size(285, 23);
            searchTextBox.TabIndex = 5;
            searchTextBox.KeyUp += searchTextBox_KeyUp;
            // 
            // resultsDataGrid
            // 
            resultsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resultsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            resultsDataGrid.Location = new System.Drawing.Point(13, 72);
            resultsDataGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            resultsDataGrid.MultiSelect = false;
            resultsDataGrid.Name = "resultsDataGrid";
            resultsDataGrid.ReadOnly = true;
            resultsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            resultsDataGrid.Size = new System.Drawing.Size(847, 308);
            resultsDataGrid.TabIndex = 7;
            resultsDataGrid.CellDoubleClick += resultsDataGrid_CellDoubleClick;
            resultsDataGrid.SelectionChanged += resultsDataGrid_SelectionChanged;
            // 
            // ProviderLookupForm
            // 
            AcceptButton = selectButton;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(875, 450);
            Controls.Add(cancelButton);
            Controls.Add(selectButton);
            Controls.Add(searchLabel);
            Controls.Add(searchTextBox);
            Controls.Add(resultsDataGrid);
            Name = "ProviderLookupForm";
            Text = "ProviderLookupForm";
            Load += LookupForm_Load;
            Shown += LookupForm_Shown;
            ((System.ComponentModel.ISupportInitialize)resultsDataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.DataGridView resultsDataGrid;
    }
}