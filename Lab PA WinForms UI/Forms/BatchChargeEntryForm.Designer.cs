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
            chargesDataGridView = new System.Windows.Forms.DataGridView();
            postChargesButton = new System.Windows.Forms.Button();
            dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            clearGridButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)chargesDataGridView).BeginInit();
            SuspendLayout();
            // 
            // chargesDataGridView
            // 
            chargesDataGridView.BackgroundColor = System.Drawing.Color.White;
            chargesDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            chargesDataGridView.Location = new System.Drawing.Point(14, 52);
            chargesDataGridView.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            chargesDataGridView.Name = "chargesDataGridView";
            chargesDataGridView.Size = new System.Drawing.Size(905, 453);
            chargesDataGridView.TabIndex = 0;
            chargesDataGridView.CellDoubleClick += chargesDataGridView_CellDoubleClick;
            chargesDataGridView.CellFormatting += chargesDataGridView_CellFormatting;
            chargesDataGridView.CellValueChanged += chargesDataGridView_CellValueChanged;
            chargesDataGridView.KeyDown += chargesDataGridView_KeyDown;
            chargesDataGridView.KeyPress += chargesDataGridView_KeyPress;
            // 
            // postChargesButton
            // 
            postChargesButton.Location = new System.Drawing.Point(806, 14);
            postChargesButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            postChargesButton.Name = "postChargesButton";
            postChargesButton.Size = new System.Drawing.Size(113, 27);
            postChargesButton.TabIndex = 1;
            postChargesButton.Text = "Post Charges";
            postChargesButton.UseVisualStyleBackColor = true;
            postChargesButton.Click += postChargesButton_Click;
            // 
            // dataGridViewTextBoxColumn1
            // 
            dataGridViewTextBoxColumn1.HeaderText = "Account";
            dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            // 
            // dataGridViewTextBoxColumn2
            // 
            dataGridViewTextBoxColumn2.HeaderText = "Name";
            dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            dataGridViewTextBoxColumn3.HeaderText = "Fin Code";
            dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            dataGridViewTextBoxColumn4.HeaderText = "Date of Service";
            dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn5
            // 
            dataGridViewTextBoxColumn5.HeaderText = "CDM";
            dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            // 
            // dataGridViewTextBoxColumn6
            // 
            dataGridViewTextBoxColumn6.HeaderText = "Qty";
            dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            // 
            // dataGridViewTextBoxColumn7
            // 
            dataGridViewTextBoxColumn7.HeaderText = "Description";
            dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // clearGridButton
            // 
            clearGridButton.Location = new System.Drawing.Point(712, 14);
            clearGridButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            clearGridButton.Name = "clearGridButton";
            clearGridButton.Size = new System.Drawing.Size(88, 27);
            clearGridButton.TabIndex = 2;
            clearGridButton.Text = "Clear Grid";
            clearGridButton.UseVisualStyleBackColor = true;
            clearGridButton.Click += clearGridButton_Click;
            // 
            // BatchChargeEntryForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(clearGridButton);
            Controls.Add(postChargesButton);
            Controls.Add(chargesDataGridView);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            Name = "BatchChargeEntryForm";
            Text = "Batch Charge Entry";
            ((System.ComponentModel.ISupportInitialize)chargesDataGridView).EndInit();
            ResumeLayout(false);
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