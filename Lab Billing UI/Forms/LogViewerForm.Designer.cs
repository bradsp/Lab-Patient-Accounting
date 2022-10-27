namespace LabBilling.Forms
{
    partial class LogViewerForm
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
            this.logViewDataGrid = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.logViewDataGrid)).BeginInit();
            this.SuspendLayout();
            // 
            // logViewDataGrid
            // 
            this.logViewDataGrid.AllowUserToAddRows = false;
            this.logViewDataGrid.AllowUserToDeleteRows = false;
            this.logViewDataGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.logViewDataGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.logViewDataGrid.Location = new System.Drawing.Point(12, 57);
            this.logViewDataGrid.Name = "logViewDataGrid";
            this.logViewDataGrid.Size = new System.Drawing.Size(890, 435);
            this.logViewDataGrid.TabIndex = 0;
            // 
            // LogViewerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(914, 504);
            this.Controls.Add(this.logViewDataGrid);
            this.Name = "LogViewerForm";
            this.Text = "LogViewerForm";
            ((System.ComponentModel.ISupportInitialize)(this.logViewDataGrid)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView logViewDataGrid;
    }
}