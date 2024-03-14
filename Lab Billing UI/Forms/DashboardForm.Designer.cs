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
            dashboardLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            announcementLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            dashboardLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // dashboardLayoutPanel
            // 
            dashboardLayoutPanel.ColumnCount = 3;
            dashboardLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.331F));
            dashboardLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.669F));
            dashboardLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 162F));
            dashboardLayoutPanel.Controls.Add(pictureBox1, 2, 0);
            dashboardLayoutPanel.Controls.Add(announcementLayoutPanel, 0, 1);
            dashboardLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            dashboardLayoutPanel.Location = new System.Drawing.Point(0, 0);
            dashboardLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            dashboardLayoutPanel.Name = "dashboardLayoutPanel";
            dashboardLayoutPanel.RowCount = 2;
            dashboardLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            dashboardLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            dashboardLayoutPanel.Size = new System.Drawing.Size(1163, 565);
            dashboardLayoutPanel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.logoicon2;
            pictureBox1.Location = new System.Drawing.Point(1004, 3);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(128, 128);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            pictureBox1.TabIndex = 0;
            pictureBox1.TabStop = false;
            // 
            // announcementLayoutPanel
            // 
            announcementLayoutPanel.ColumnCount = 1;
            announcementLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            announcementLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            announcementLayoutPanel.Location = new System.Drawing.Point(4, 285);
            announcementLayoutPanel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            announcementLayoutPanel.Name = "announcementLayoutPanel";
            announcementLayoutPanel.RowCount = 1;
            announcementLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            announcementLayoutPanel.Size = new System.Drawing.Size(515, 277);
            announcementLayoutPanel.TabIndex = 4;
            // 
            // DashboardForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.ControlLightLight;
            ClientSize = new System.Drawing.Size(1163, 565);
            Controls.Add(dashboardLayoutPanel);
            ForeColor = System.Drawing.Color.Black;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
        private System.Windows.Forms.TableLayoutPanel announcementLayoutPanel;
    }
}