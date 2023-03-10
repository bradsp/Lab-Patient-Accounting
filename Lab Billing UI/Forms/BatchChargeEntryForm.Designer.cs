namespace LabBilling.Forms
{
    partial class BatchChargeEntryForm
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
            this.chargesDataGridView = new System.Windows.Forms.DataGridView();
            this.postChargesButton = new System.Windows.Forms.Button();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.clearGridButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.chargesDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // chargesDataGridView
            // 
            this.chargesDataGridView.BackgroundColor = System.Drawing.Color.White;
            this.chargesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.chargesDataGridView.Location = new System.Drawing.Point(12, 45);
            this.chargesDataGridView.Name = "chargesDataGridView";
            this.chargesDataGridView.Size = new System.Drawing.Size(776, 393);
            this.chargesDataGridView.TabIndex = 0;
            this.chargesDataGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.chargesDataGridView_CellDoubleClick);
            this.chargesDataGridView.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.chargesDataGridView_CellFormatting);
            this.chargesDataGridView.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.chargesDataGridView_CellValueChanged);
            this.chargesDataGridView.KeyDown += new System.Windows.Forms.KeyEventHandler(this.chargesDataGridView_KeyDown);
            this.chargesDataGridView.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.chargesDataGridView_KeyPress);
            // 
            // postChargesButton
            // 
            this.postChargesButton.Location = new System.Drawing.Point(691, 12);
            this.postChargesButton.Name = "postChargesButton";
            this.postChargesButton.Size = new System.Drawing.Size(97, 23);
            this.postChargesButton.TabIndex = 1;
            this.postChargesButton.Text = "Post Charges";
            this.postChargesButton.UseVisualStyleBackColor = true;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Account";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Name";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Fin Code";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Date of Service";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "CDM";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Qty";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Description";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // clearGridButton
            // 
            this.clearGridButton.Location = new System.Drawing.Point(610, 12);
            this.clearGridButton.Name = "clearGridButton";
            this.clearGridButton.Size = new System.Drawing.Size(75, 23);
            this.clearGridButton.TabIndex = 2;
            this.clearGridButton.Text = "Clear Grid";
            this.clearGridButton.UseVisualStyleBackColor = true;
            this.clearGridButton.Click += new System.EventHandler(this.clearGridButton_Click);
            // 
            // BatchChargeEntryForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.clearGridButton);
            this.Controls.Add(this.postChargesButton);
            this.Controls.Add(this.chargesDataGridView);
            this.Name = "BatchChargeEntryForm";
            this.Text = "Batch Charge Entry";
            ((System.ComponentModel.ISupportInitialize)(this.chargesDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView chargesDataGridView;
        private System.Windows.Forms.Button postChargesButton;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.Button clearGridButton;
    }
}