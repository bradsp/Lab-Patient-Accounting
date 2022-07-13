
namespace LabBilling.Forms
{
    partial class InterfaceMonitor
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
            this.components = new System.ComponentModel.Container();
            this.MessagesGrid = new System.Windows.Forms.DataGridView();
            this.GridContextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ReprocessMessage = new System.Windows.Forms.ToolStripMenuItem();
            this.MessageTypeSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.hl7Message = new System.Windows.Forms.TextBox();
            this.FromDate = new System.Windows.Forms.DateTimePicker();
            this.ThruDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FilterButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.MessagesGrid)).BeginInit();
            this.GridContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MessagesGrid
            // 
            this.MessagesGrid.AllowUserToAddRows = false;
            this.MessagesGrid.AllowUserToDeleteRows = false;
            this.MessagesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.MessagesGrid.ContextMenuStrip = this.GridContextMenu;
            this.MessagesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MessagesGrid.Location = new System.Drawing.Point(0, 0);
            this.MessagesGrid.Name = "MessagesGrid";
            this.MessagesGrid.ReadOnly = true;
            this.MessagesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MessagesGrid.Size = new System.Drawing.Size(1187, 409);
            this.MessagesGrid.TabIndex = 0;
            this.MessagesGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MessagesGrid_CellClick);
            this.MessagesGrid.SelectionChanged += new System.EventHandler(this.MessagesGrid_SelectionChanged);
            // 
            // GridContextMenu
            // 
            this.GridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ReprocessMessage});
            this.GridContextMenu.Name = "GridContextMenu";
            this.GridContextMenu.Size = new System.Drawing.Size(177, 26);
            // 
            // ReprocessMessage
            // 
            this.ReprocessMessage.Name = "ReprocessMessage";
            this.ReprocessMessage.Size = new System.Drawing.Size(176, 22);
            this.ReprocessMessage.Text = "Reprocess Message";
            this.ReprocessMessage.Click += new System.EventHandler(this.ReprocessMessage_Click);
            // 
            // MessageTypeSelect
            // 
            this.MessageTypeSelect.FormattingEnabled = true;
            this.MessageTypeSelect.Items.AddRange(new object[] {
            "ADT",
            "DFT",
            "MFN"});
            this.MessageTypeSelect.Location = new System.Drawing.Point(94, 12);
            this.MessageTypeSelect.Name = "MessageTypeSelect";
            this.MessageTypeSelect.Size = new System.Drawing.Size(121, 21);
            this.MessageTypeSelect.TabIndex = 2;
            this.MessageTypeSelect.SelectedIndexChanged += new System.EventHandler(this.MessageTypeSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Message Type";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(14, 50);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MessagesGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.hl7Message);
            this.splitContainer1.Size = new System.Drawing.Size(1187, 645);
            this.splitContainer1.SplitterDistance = 409;
            this.splitContainer1.TabIndex = 4;
            // 
            // hl7Message
            // 
            this.hl7Message.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hl7Message.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hl7Message.Location = new System.Drawing.Point(0, 0);
            this.hl7Message.Multiline = true;
            this.hl7Message.Name = "hl7Message";
            this.hl7Message.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.hl7Message.Size = new System.Drawing.Size(1187, 232);
            this.hl7Message.TabIndex = 0;
            this.hl7Message.WordWrap = false;
            // 
            // FromDate
            // 
            this.FromDate.CustomFormat = "M/d/yy HH:mm";
            this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FromDate.Location = new System.Drawing.Point(378, 13);
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(145, 20);
            this.FromDate.TabIndex = 5;
            // 
            // ThruDate
            // 
            this.ThruDate.CustomFormat = "M/d/yy HH:mm";
            this.ThruDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ThruDate.Location = new System.Drawing.Point(551, 13);
            this.ThruDate.Name = "ThruDate";
            this.ThruDate.Size = new System.Drawing.Size(142, 20);
            this.ThruDate.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(307, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Date Range";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(529, 16);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = " - ";
            // 
            // FilterButton
            // 
            this.FilterButton.Location = new System.Drawing.Point(699, 13);
            this.FilterButton.Name = "FilterButton";
            this.FilterButton.Size = new System.Drawing.Size(75, 23);
            this.FilterButton.TabIndex = 8;
            this.FilterButton.Text = "Filter";
            this.FilterButton.UseVisualStyleBackColor = true;
            this.FilterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // InterfaceMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1213, 707);
            this.Controls.Add(this.FilterButton);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ThruDate);
            this.Controls.Add(this.FromDate);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.MessageTypeSelect);
            this.Name = "InterfaceMonitor";
            this.Text = "InterfaceMonitor";
            this.Load += new System.EventHandler(this.InterfaceMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MessagesGrid)).EndInit();
            this.GridContextMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView MessagesGrid;
        private System.Windows.Forms.ComboBox MessageTypeSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox hl7Message;
        private System.Windows.Forms.ContextMenuStrip GridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ReprocessMessage;
        private System.Windows.Forms.DateTimePicker FromDate;
        private System.Windows.Forms.DateTimePicker ThruDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FilterButton;
    }
}