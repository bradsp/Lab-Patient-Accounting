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
            this.dashboardLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.arChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.announcementLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.dashboardLayoutPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.arChart)).BeginInit();
            this.SuspendLayout();
            // 
            // dashboardLayoutPanel
            // 
            this.dashboardLayoutPanel.ColumnCount = 3;
            this.dashboardLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 52.331F));
            this.dashboardLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 47.669F));
            this.dashboardLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 138F));
            this.dashboardLayoutPanel.Controls.Add(this.arChart, 0, 0);
            this.dashboardLayoutPanel.Controls.Add(this.tableLayoutPanel1, 1, 1);
            this.dashboardLayoutPanel.Controls.Add(this.pictureBox1, 2, 0);
            this.dashboardLayoutPanel.Controls.Add(this.announcementLayoutPanel, 0, 1);
            this.dashboardLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dashboardLayoutPanel.Location = new System.Drawing.Point(0, 0);
            this.dashboardLayoutPanel.Name = "dashboardLayoutPanel";
            this.dashboardLayoutPanel.RowCount = 2;
            this.dashboardLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dashboardLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.dashboardLayoutPanel.Size = new System.Drawing.Size(997, 490);
            this.dashboardLayoutPanel.TabIndex = 0;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::LabBilling.Properties.Resources.logoicon2;
            this.pictureBox1.Location = new System.Drawing.Point(861, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(128, 128);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // arChart
            // 
            this.arChart.Dock = System.Windows.Forms.DockStyle.Fill;
            this.arChart.Location = new System.Drawing.Point(3, 3);
            this.arChart.Name = "arChart";
            this.arChart.Size = new System.Drawing.Size(443, 239);
            this.arChart.TabIndex = 1;
            this.arChart.Text = "chart1";
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 81.72589F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 18.27411F));
            this.tableLayoutPanel1.Location = new System.Drawing.Point(452, 248);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34.22819F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 65.77181F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(394, 149);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // announcementLayoutPanel
            // 
            this.announcementLayoutPanel.ColumnCount = 1;
            this.announcementLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.announcementLayoutPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.announcementLayoutPanel.Location = new System.Drawing.Point(3, 248);
            this.announcementLayoutPanel.Name = "announcementLayoutPanel";
            this.announcementLayoutPanel.RowCount = 1;
            this.announcementLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.announcementLayoutPanel.Size = new System.Drawing.Size(443, 239);
            this.announcementLayoutPanel.TabIndex = 4;
            // 
            // DashboardForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(997, 490);
            this.Controls.Add(this.dashboardLayoutPanel);
            this.Name = "DashboardForm";
            this.Text = "DashboardForm";
            this.Load += new System.EventHandler(this.DashboardForm_Load);
            this.dashboardLayoutPanel.ResumeLayout(false);
            this.dashboardLayoutPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.arChart)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel dashboardLayoutPanel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataVisualization.Charting.Chart arChart;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel announcementLayoutPanel;
    }
}