namespace LabBilling.Forms
{
    partial class CdmLookupForm
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
            this.resultsDataGrid = new System.Windows.Forms.DataGridView();
            this.searchTextBox = new System.Windows.Forms.TextBox();
            this.searchLabel = new System.Windows.Forms.Label();
            this.selectButton = new System.Windows.Forms.Button();
            this.cancelButton = new System.Windows.Forms.Button();
            this.cdmDescSearchRadioButton = new System.Windows.Forms.RadioButton();
            this.cptSearchRadioButton = new System.Windows.Forms.RadioButton();
            this.searchByLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // resultsDataGrid
            // 
            this.resultsDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.resultsDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.resultsDataGrid.Location = new System.Drawing.Point(12, 57);
            this.resultsDataGrid.MultiSelect = false;
            this.resultsDataGrid.Name = "resultsDataGrid";
            this.resultsDataGrid.ReadOnly = true;
            this.resultsDataGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.resultsDataGrid.Size = new System.Drawing.Size(726, 267);
            this.resultsDataGrid.TabIndex = 0;
            this.resultsDataGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.resultsDataGrid_CellDoubleClick);
            this.resultsDataGrid.SelectionChanged += new System.EventHandler(this.resultsDataGrid_SelectionChanged);
            // 
            // searchTextBox
            // 
            this.searchTextBox.Location = new System.Drawing.Point(59, 16);
            this.searchTextBox.Name = "searchTextBox";
            this.searchTextBox.Size = new System.Drawing.Size(245, 20);
            this.searchTextBox.TabIndex = 1;
            this.searchTextBox.KeyUp += new System.Windows.Forms.KeyEventHandler(this.searchTextBox_KeyUp);
            // 
            // searchLabel
            // 
            this.searchLabel.AutoSize = true;
            this.searchLabel.Location = new System.Drawing.Point(12, 19);
            this.searchLabel.Name = "searchLabel";
            this.searchLabel.Size = new System.Drawing.Size(41, 13);
            this.searchLabel.TabIndex = 2;
            this.searchLabel.Text = "Search";
            // 
            // selectButton
            // 
            this.selectButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.selectButton.Location = new System.Drawing.Point(12, 336);
            this.selectButton.Name = "selectButton";
            this.selectButton.Size = new System.Drawing.Size(75, 23);
            this.selectButton.TabIndex = 3;
            this.selectButton.Text = "&Select";
            this.selectButton.UseVisualStyleBackColor = true;
            this.selectButton.Click += new System.EventHandler(this.selectButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.cancelButton.Location = new System.Drawing.Point(93, 336);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 3;
            this.cancelButton.Text = "&Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            // 
            // cdmDescSearchRadioButton
            // 
            this.cdmDescSearchRadioButton.AutoSize = true;
            this.cdmDescSearchRadioButton.Checked = true;
            this.cdmDescSearchRadioButton.Location = new System.Drawing.Point(427, 17);
            this.cdmDescSearchRadioButton.Name = "cdmDescSearchRadioButton";
            this.cdmDescSearchRadioButton.Size = new System.Drawing.Size(113, 17);
            this.cdmDescSearchRadioButton.TabIndex = 4;
            this.cdmDescSearchRadioButton.TabStop = true;
            this.cdmDescSearchRadioButton.Text = "CDM / Description";
            this.cdmDescSearchRadioButton.UseVisualStyleBackColor = true;
            this.cdmDescSearchRadioButton.CheckedChanged += new System.EventHandler(this.cdmDescSearchRadioButton_CheckedChanged);
            // 
            // cptSearchRadioButton
            // 
            this.cptSearchRadioButton.AutoSize = true;
            this.cptSearchRadioButton.Location = new System.Drawing.Point(567, 17);
            this.cptSearchRadioButton.Name = "cptSearchRadioButton";
            this.cptSearchRadioButton.Size = new System.Drawing.Size(46, 17);
            this.cptSearchRadioButton.TabIndex = 5;
            this.cptSearchRadioButton.Text = "CPT";
            this.cptSearchRadioButton.UseVisualStyleBackColor = true;
            this.cptSearchRadioButton.CheckedChanged += new System.EventHandler(this.cptSearchRadioButton_CheckedChanged);
            // 
            // searchByLabel
            // 
            this.searchByLabel.AutoSize = true;
            this.searchByLabel.Location = new System.Drawing.Point(343, 19);
            this.searchByLabel.Name = "searchByLabel";
            this.searchByLabel.Size = new System.Drawing.Size(58, 13);
            this.searchByLabel.TabIndex = 6;
            this.searchByLabel.Text = "Search by:";
            // 
            // CdmLookupForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(750, 371);
            this.ControlBox = false;
            this.Controls.Add(this.searchByLabel);
            this.Controls.Add(this.cptSearchRadioButton);
            this.Controls.Add(this.cdmDescSearchRadioButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.selectButton);
            this.Controls.Add(this.searchLabel);
            this.Controls.Add(this.searchTextBox);
            this.Controls.Add(this.resultsDataGrid);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CdmLookupForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CDM Lookup";
            this.Load += new System.EventHandler(this.LookupForm_Load);
            this.Shown += new System.EventHandler(this.LookupForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.resultsDataGrid)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView resultsDataGrid;
        private System.Windows.Forms.TextBox searchTextBox;
        private System.Windows.Forms.Label searchLabel;
        private System.Windows.Forms.Button selectButton;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.RadioButton cdmDescSearchRadioButton;
        private System.Windows.Forms.RadioButton cptSearchRadioButton;
        private System.Windows.Forms.Label searchByLabel;
    }
}