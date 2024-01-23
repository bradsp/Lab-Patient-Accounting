namespace Utilities
{
    partial class ViewerBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewerBase));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tsbAbout = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbLoadGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.tspPrintGrid = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tslServer = new System.Windows.Forms.ToolStripLabel();
            this.tscbServer = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.tslDatabase = new System.Windows.Forms.ToolStripLabel();
            this.tscbDatabase = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.tstbTabs = new System.Windows.Forms.ToolStripTextBox();
            this.ssMain = new System.Windows.Forms.StatusStrip();
            this.m_tcSelect = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tsMain.SuspendLayout();
            this.m_tcSelect.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsbAbout,
            this.toolStripSeparator1,
            this.tsbLoadGrid,
            this.toolStripSeparator2,
            this.tspPrintGrid,
            this.toolStripSeparator3,
            this.tslServer,
            this.tscbServer,
            this.toolStripSeparator4,
            this.tslDatabase,
            this.tscbDatabase,
            this.toolStripSeparator5,
            this.tstbTabs});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(794, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "Main ToolStrip";
            // 
            // tsbAbout
            // 
            this.tsbAbout.Image = ((System.Drawing.Image)(resources.GetObject("tsbAbout.Image")));
            this.tsbAbout.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbAbout.Name = "tsbAbout";
            this.tsbAbout.Size = new System.Drawing.Size(56, 22);
            this.tsbAbout.Text = "About";
            this.tsbAbout.Click += new System.EventHandler(this.tsbAbout_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // tsbLoadGrid
            // 
            this.tsbLoadGrid.Image = ((System.Drawing.Image)(resources.GetObject("tsbLoadGrid.Image")));
            this.tsbLoadGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tsbLoadGrid.Name = "tsbLoadGrid";
            this.tsbLoadGrid.Size = new System.Drawing.Size(72, 22);
            this.tsbLoadGrid.Text = "Load Grid";
            this.tsbLoadGrid.Click += new System.EventHandler(this.tsbLoadGrid_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // tspPrintGrid
            // 
            this.tspPrintGrid.Image = ((System.Drawing.Image)(resources.GetObject("tspPrintGrid.Image")));
            this.tspPrintGrid.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tspPrintGrid.Name = "tspPrintGrid";
            this.tspPrintGrid.Size = new System.Drawing.Size(71, 22);
            this.tspPrintGrid.Text = "Print Grid";
            this.tspPrintGrid.Click += new System.EventHandler(this.tspPrintGrid_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // tslServer
            // 
            this.tslServer.Name = "tslServer";
            this.tslServer.Size = new System.Drawing.Size(56, 22);
            this.tslServer.Text = "Sql Server";
            // 
            // tscbServer
            // 
            this.tscbServer.Name = "tscbServer";
            this.tscbServer.Size = new System.Drawing.Size(121, 25);
            this.tscbServer.SelectedIndexChanged += new System.EventHandler(this.tscbServer_SelectedIndexChanged);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 25);
            // 
            // tslDatabase
            // 
            this.tslDatabase.Name = "tslDatabase";
            this.tslDatabase.Size = new System.Drawing.Size(53, 22);
            this.tslDatabase.Text = "Database";
            // 
            // tscbDatabase
            // 
            this.tscbDatabase.Name = "tscbDatabase";
            this.tscbDatabase.Size = new System.Drawing.Size(121, 25);
            this.tscbDatabase.SelectedIndexChanged += new System.EventHandler(this.tscbDatabase_SelectedIndexChanged);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(6, 25);
            // 
            // tstbTabs
            // 
            this.tstbTabs.Name = "tstbTabs";
            this.tstbTabs.Size = new System.Drawing.Size(25, 25);
            this.tstbTabs.Text = "3";
            this.tstbTabs.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.tstbTabs.KeyUp += new System.Windows.Forms.KeyEventHandler(this.tstbTabs_KeyUp);
            this.tstbTabs.TextChanged += new System.EventHandler(this.tstbTabs_TextChanged);
            // 
            // ssMain
            // 
            this.ssMain.Location = new System.Drawing.Point(0, 244);
            this.ssMain.Name = "ssMain";
            this.ssMain.Size = new System.Drawing.Size(794, 22);
            this.ssMain.TabIndex = 1;
            this.ssMain.Text = "Main Status Strip";
            // 
            // m_tcSelect
            // 
            this.m_tcSelect.Controls.Add(this.tabPage1);
            this.m_tcSelect.Controls.Add(this.tabPage2);
            this.m_tcSelect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.m_tcSelect.Location = new System.Drawing.Point(0, 25);
            this.m_tcSelect.Name = "m_tcSelect";
            this.m_tcSelect.SelectedIndex = 0;
            this.m_tcSelect.Size = new System.Drawing.Size(794, 219);
            this.m_tcSelect.TabIndex = 2;
            // 
            // tabPage1
            // 
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(786, 193);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(786, 193);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ViewerBase
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(794, 266);
            this.Controls.Add(this.m_tcSelect);
            this.Controls.Add(this.ssMain);
            this.Controls.Add(this.tsMain);
            this.Name = "ViewerBase";
            this.Text = "ViewerBase";
            this.Shown += new System.EventHandler(this.ViewerBase_Shown);
            this.Load += new System.EventHandler(this.ViewerBase_InitialFormLoad);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.m_tcSelect.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.ToolStripButton tsbAbout;
        private System.Windows.Forms.StatusStrip ssMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton tsbLoadGrid;
        private System.Windows.Forms.TabControl m_tcSelect;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton tspPrintGrid;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripLabel tslServer;
        private System.Windows.Forms.ToolStripComboBox tscbServer;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel tslDatabase;
        private System.Windows.Forms.ToolStripComboBox tscbDatabase;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripTextBox tstbTabs;
    }
}