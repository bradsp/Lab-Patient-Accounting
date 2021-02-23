namespace LabBilling.Forms
{
    partial class InterfaceMapping
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
            this.CodeSet = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.MappingDGV = new System.Windows.Forms.DataGridView();
            this.SendingSystem = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.LoadGrid = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MappingDGV)).BeginInit();
            this.SuspendLayout();
            // 
            // CodeSet
            // 
            this.CodeSet.FormattingEnabled = true;
            this.CodeSet.Location = new System.Drawing.Point(80, 16);
            this.CodeSet.Name = "CodeSet";
            this.CodeSet.Size = new System.Drawing.Size(172, 21);
            this.CodeSet.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(51, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Code Set";
            // 
            // MappingDGV
            // 
            this.MappingDGV.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MappingDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MappingDGV.Location = new System.Drawing.Point(12, 66);
            this.MappingDGV.Name = "MappingDGV";
            this.MappingDGV.Size = new System.Drawing.Size(776, 372);
            this.MappingDGV.TabIndex = 2;
            // 
            // SendingSystem
            // 
            this.SendingSystem.FormattingEnabled = true;
            this.SendingSystem.Location = new System.Drawing.Point(329, 16);
            this.SendingSystem.Name = "SendingSystem";
            this.SendingSystem.Size = new System.Drawing.Size(121, 21);
            this.SendingSystem.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(272, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "System";
            // 
            // LoadGrid
            // 
            this.LoadGrid.Location = new System.Drawing.Point(474, 14);
            this.LoadGrid.Name = "LoadGrid";
            this.LoadGrid.Size = new System.Drawing.Size(75, 23);
            this.LoadGrid.TabIndex = 5;
            this.LoadGrid.Text = "Load";
            this.LoadGrid.UseVisualStyleBackColor = true;
            this.LoadGrid.Click += new System.EventHandler(this.LoadGrid_Click);
            // 
            // InterfaceMapping
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.LoadGrid);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.SendingSystem);
            this.Controls.Add(this.MappingDGV);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CodeSet);
            this.Name = "InterfaceMapping";
            this.Text = "InterfaceMapping";
            this.Load += new System.EventHandler(this.InterfaceMapping_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MappingDGV)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CodeSet;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView MappingDGV;
        private System.Windows.Forms.ComboBox SendingSystem;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button LoadGrid;
    }
}