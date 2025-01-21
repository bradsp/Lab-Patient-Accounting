namespace LabBilling.Forms
{
    partial class DashboardForm
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
            dashboardLayoutPanel = new TableLayoutPanel();
            pictureBox1 = new PictureBox();
            dashboardLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dashboardLayoutPanel
            // 
            dashboardLayoutPanel.ColumnCount = 3;
            dashboardLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 52.331F));
            dashboardLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 47.669F));
            dashboardLayoutPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 162F));
            dashboardLayoutPanel.Controls.Add(pictureBox1, 2, 0);
            dashboardLayoutPanel.Dock = DockStyle.Fill;
            dashboardLayoutPanel.Location = new Point(0, 0);
            dashboardLayoutPanel.Margin = new Padding(4, 3, 4, 3);
            dashboardLayoutPanel.Name = "dashboardLayoutPanel";
            dashboardLayoutPanel.RowCount = 2;
            dashboardLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            dashboardLayoutPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            dashboardLayoutPanel.Size = new Size(1163, 565);
            dashboardLayoutPanel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logoicon2;
            pictureBox1.Location = new Point(1004, 3);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(128, 128);
            pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ControlLightLight;
            ClientSize = new Size(1163, 565);
            Controls.Add(dashboardLayoutPanel);
            ForeColor = Color.Black;
            Margin = new Padding(4, 3, 4, 3);
            Name = "DashboardForm";
            Text = "Dashboard";
            Load += DashboardForm_Load;
            dashboardLayoutPanel.ResumeLayout(false);
            dashboardLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel dashboardLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}