namespace LabBilling.Forms
{
    partial class InsCompanyLookupForm
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
            resultsDataGrid = new System.Windows.Forms.DataGridView();
            searchTextBox = new System.Windows.Forms.TextBox();
            searchLabel = new System.Windows.Forms.Label();
            selectButton = new System.Windows.Forms.Button();
            cancelButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)resultsDataGrid).BeginInit();
            SuspendLayout();
            // 
            // resultsDataGrid
            // 
            resultsDataGrid.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            resultsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            resultsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            resultsDataGrid.Location = new System.Drawing.Point(14, 66);
            resultsDataGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            resultsDataGrid.MultiSelect = false;
            resultsDataGrid.Name = "resultsDataGrid";
            resultsDataGrid.ReadOnly = true;
            resultsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            resultsDataGrid.Size = new System.Drawing.Size(851, 311);
            resultsDataGrid.TabIndex = 2;
            resultsDataGrid.CellDoubleClick += resultsDataGrid_CellDoubleClick;
            resultsDataGrid.SelectionChanged += resultsDataGrid_SelectionChanged;
            // 
            // searchTextBox
            // 
            searchTextBox.Location = new System.Drawing.Point(69, 18);
            searchTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            searchTextBox.Name = "searchTextBox";
            searchTextBox.Size = new System.Drawing.Size(285, 23);
            searchTextBox.TabIndex = 0;
            searchTextBox.KeyUp += searchTextBox_KeyUp;
            // 
            // searchLabel
            // 
            searchLabel.AutoSize = true;
            searchLabel.Location = new System.Drawing.Point(14, 22);
            searchLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            searchLabel.Name = "searchLabel";
            searchLabel.Size = new System.Drawing.Size(42, 15);
            searchLabel.TabIndex = 1;
            searchLabel.Text = "Search";
            // 
            // selectButton
            // 
            selectButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            selectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            selectButton.Location = new System.Drawing.Point(14, 391);
            selectButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            selectButton.Name = "selectButton";
            selectButton.Size = new System.Drawing.Size(88, 27);
            selectButton.TabIndex = 3;
            selectButton.Text = "&Select";
            selectButton.UseVisualStyleBackColor = true;
            selectButton.Click += selectButton_Click;
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left;
            cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            cancelButton.Location = new System.Drawing.Point(108, 391);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 27);
            cancelButton.TabIndex = 4;
            cancelButton.Text = "&Cancel";
            cancelButton.UseVisualStyleBackColor = true;
            // 
            // InsCompanyLookupForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.Color.White;
            CancelButton = cancelButton;
            ClientSize = new System.Drawing.Size(879, 434);
            ControlBox = false;
            Controls.Add(cancelButton);
            Controls.Add(selectButton);
            Controls.Add(searchLabel);
            Controls.Add(searchTextBox);
            Controls.Add(resultsDataGrid);
            ForeColor = System.Drawing.Color.Black;
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "InsCompanyLookupForm";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Insurance Plan Lookup";
            Load += LookupForm_Load;
            Shown += LookupForm_Shown;
            ((System.ComponentModel.ISupportInitialize)resultsDataGrid).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView resultsDataGrid;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button cancelButton;
    }
}