namespace LabBilling.Forms
{
    partial class SystemParametersForm
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
            this.SystemParmDGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.SystemParmDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // SystemParmDGV
            // 
            this.SystemParmDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.SystemParmDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.SystemParmDGV.Location = new System.Drawing.Point(9, 10);
            this.SystemParmDGV.Margin = new System.Windows.Forms.Padding(2);
            this.SystemParmDGV.Name = "SystemParmDGV";
            this.SystemParmDGV.RowTemplate.Height = 24;
            this.SystemParmDGV.Size = new System.Drawing.Size(708, 546);
            this.SystemParmDGV.TabIndex = 0;
            // 
            // SystemParametersForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(833, 566);
            this.Controls.Add(this.SystemParmDGV);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "SystemParametersForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SystemParametersForm";
            this.Load += new System.EventHandler(this.SystemParametersForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.SystemParmDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView SystemParmDGV;
    }
}