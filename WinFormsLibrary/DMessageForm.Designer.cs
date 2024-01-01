namespace RFClassLibrary
{
    partial class DMessageForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DMessageForm));
            this.tsMain = new System.Windows.Forms.ToolStrip();
            this.tssbPrint = new System.Windows.Forms.ToolStripSplitButton();
            this.tsmiPrintMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.formToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rtbError = new System.Windows.Forms.RichTextBox();
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.tsMain.SuspendLayout();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // tsMain
            // 
            this.tsMain.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.tsMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssbPrint});
            this.tsMain.Location = new System.Drawing.Point(0, 0);
            this.tsMain.Name = "tsMain";
            this.tsMain.Size = new System.Drawing.Size(292, 25);
            this.tsMain.TabIndex = 0;
            this.tsMain.Text = "toolStrip1";
            // 
            // tssbPrint
            // 
            this.tssbPrint.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tssbPrint.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiPrintMessage,
            this.formToolStripMenuItem});
            this.tssbPrint.Image = ((System.Drawing.Image)(resources.GetObject("tssbPrint.Image")));
            this.tssbPrint.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.tssbPrint.Name = "tssbPrint";
            this.tssbPrint.Size = new System.Drawing.Size(72, 22);
            this.tssbPrint.Text = "Print Error";
            // 
            // tsmiPrintMessage
            // 
            this.tsmiPrintMessage.CheckOnClick = true;
            this.tsmiPrintMessage.Name = "tsmiPrintMessage";
            this.tsmiPrintMessage.Size = new System.Drawing.Size(152, 22);
            this.tsmiPrintMessage.Text = "Message";
            this.tsmiPrintMessage.Click += new System.EventHandler(this.tsmiPrintMessage_Click);
            // 
            // formToolStripMenuItem
            // 
            this.formToolStripMenuItem.Enabled = false;
            this.formToolStripMenuItem.Name = "formToolStripMenuItem";
            this.formToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.formToolStripMenuItem.Text = "Form";
            // 
            // rtbError
            // 
            this.rtbError.BackColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.rtbError.BulletIndent = 3;
            this.rtbError.DetectUrls = false;
            this.rtbError.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbError.Location = new System.Drawing.Point(0, 0);
            this.rtbError.Name = "rtbError";
            this.rtbError.ReadOnly = true;
            this.rtbError.ShortcutsEnabled = false;
            this.rtbError.Size = new System.Drawing.Size(292, 162);
            this.rtbError.TabIndex = 1;
            this.rtbError.Text = global::RFClassLibrary.Properties.Resources.String1;
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 25);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.rtbError);
            this.scMain.Panel1MinSize = 60;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.btnYes);
            this.scMain.Panel2.Controls.Add(this.btnNo);
            this.scMain.Size = new System.Drawing.Size(292, 241);
            this.scMain.SplitterDistance = 162;
            this.scMain.TabIndex = 2;
            // 
            // btnYes
            // 
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(28, 38);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(75, 23);
            this.btnYes.TabIndex = 1;
            this.btnYes.Text = "YES";
            this.btnYes.UseVisualStyleBackColor = true;
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(142, 38);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(75, 23);
            this.btnNo.TabIndex = 0;
            this.btnNo.Text = "NO";
            this.btnNo.UseVisualStyleBackColor = true;
            // 
            // DMessageForm
            // 
            this.AcceptButton = this.btnNo;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(292, 266);
            this.ControlBox = false;
            this.Controls.Add(this.scMain);
            this.Controls.Add(this.tsMain);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DMessageForm";
            this.ShowInTaskbar = false;
            this.Text = "DMessageForm";
            this.Load += new System.EventHandler(this.DMessageForm_Load);
            this.tsMain.ResumeLayout(false);
            this.tsMain.PerformLayout();
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            this.scMain.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip tsMain;
        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.Button btnNo;
        /// <summary>
        /// 
        /// </summary>
        public System.Windows.Forms.RichTextBox rtbError;
        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.ToolStripSplitButton tssbPrint;
        private System.Windows.Forms.ToolStripMenuItem tsmiPrintMessage;
        private System.Windows.Forms.ToolStripMenuItem formToolStripMenuItem;
    }
}