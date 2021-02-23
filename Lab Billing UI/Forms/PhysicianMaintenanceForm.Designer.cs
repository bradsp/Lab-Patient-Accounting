
namespace LabBilling.Forms
{
    partial class PhysicianMaintenanceForm
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
            this.PhysicianDGV = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.PhysicianDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // PhysicianDGV
            // 
            this.PhysicianDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PhysicianDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.PhysicianDGV.Location = new System.Drawing.Point(266, 48);
            this.PhysicianDGV.Name = "PhysicianDGV";
            this.PhysicianDGV.Size = new System.Drawing.Size(699, 415);
            this.PhysicianDGV.TabIndex = 0;
            // 
            // PhysicianMaintenanceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(977, 521);
            this.Controls.Add(this.PhysicianDGV);
            this.Name = "PhysicianMaintenanceForm";
            this.Text = "PhysicianMaintenanceForm";
            this.Load += new System.EventHandler(this.PhysicianMaintenanceForm_Load);
            ((System.ComponentModel.ISupportInitialize)(this.PhysicianDGV)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView PhysicianDGV;
    }
}