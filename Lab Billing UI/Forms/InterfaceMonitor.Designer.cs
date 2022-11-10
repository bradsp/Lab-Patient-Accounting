
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
            this.MessageTypeFilterComboBox = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.hl7MessageTextBox = new System.Windows.Forms.TextBox();
            this.FromDate = new System.Windows.Forms.DateTimePicker();
            this.ThruDate = new System.Windows.Forms.DateTimePicker();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FilterButton = new System.Windows.Forms.Button();
            this.accountFilterTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.errorsTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.processFlagFilterCombo = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.MessagesGrid)).BeginInit();
            this.GridContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
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
            this.MessagesGrid.Size = new System.Drawing.Size(1138, 248);
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
            // MessageTypeFilterComboBox
            // 
            this.MessageTypeFilterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.MessageTypeFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MessageTypeFilterComboBox.FormattingEnabled = true;
            this.MessageTypeFilterComboBox.Items.AddRange(new object[] {
            "All",
            "ADT",
            "DFT",
            "MFN"});
            this.MessageTypeFilterComboBox.Location = new System.Drawing.Point(434, 35);
            this.MessageTypeFilterComboBox.Name = "MessageTypeFilterComboBox";
            this.MessageTypeFilterComboBox.Size = new System.Drawing.Size(121, 21);
            this.MessageTypeFilterComboBox.TabIndex = 2;
            this.MessageTypeFilterComboBox.SelectedIndexChanged += new System.EventHandler(this.MessageTypeSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(431, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Message Type";
            // 
            // hl7MessageTextBox
            // 
            this.hl7MessageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.hl7MessageTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.hl7MessageTextBox.Location = new System.Drawing.Point(0, 0);
            this.hl7MessageTextBox.Multiline = true;
            this.hl7MessageTextBox.Name = "hl7MessageTextBox";
            this.hl7MessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.hl7MessageTextBox.Size = new System.Drawing.Size(1138, 132);
            this.hl7MessageTextBox.TabIndex = 0;
            this.hl7MessageTextBox.WordWrap = false;
            // 
            // FromDate
            // 
            this.FromDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FromDate.CustomFormat = "M/d/yy HH:mm";
            this.FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.FromDate.Location = new System.Drawing.Point(749, 36);
            this.FromDate.Name = "FromDate";
            this.FromDate.Size = new System.Drawing.Size(116, 20);
            this.FromDate.TabIndex = 5;
            // 
            // ThruDate
            // 
            this.ThruDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.ThruDate.CustomFormat = "M/d/yy HH:mm";
            this.ThruDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.ThruDate.Location = new System.Drawing.Point(893, 36);
            this.ThruDate.Name = "ThruDate";
            this.ThruDate.Size = new System.Drawing.Size(114, 20);
            this.ThruDate.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(746, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Date Range";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(871, 38);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(16, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = " - ";
            // 
            // FilterButton
            // 
            this.FilterButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.FilterButton.Location = new System.Drawing.Point(1013, 34);
            this.FilterButton.Name = "FilterButton";
            this.FilterButton.Size = new System.Drawing.Size(148, 23);
            this.FilterButton.TabIndex = 8;
            this.FilterButton.Text = "Update Date Range";
            this.FilterButton.UseVisualStyleBackColor = true;
            this.FilterButton.Click += new System.EventHandler(this.FilterButton_Click);
            // 
            // accountFilterTextBox
            // 
            this.accountFilterTextBox.Location = new System.Drawing.Point(211, 36);
            this.accountFilterTextBox.Name = "accountFilterTextBox";
            this.accountFilterTextBox.Size = new System.Drawing.Size(217, 20);
            this.accountFilterTextBox.TabIndex = 9;
            this.accountFilterTextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.accountFilterTextBox_KeyDown);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(208, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Filter Account";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(23, 80);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.MessagesGrid);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1138, 497);
            this.splitContainer1.SplitterDistance = 248;
            this.splitContainer1.TabIndex = 11;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.hl7MessageTextBox);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.errorsTextBox);
            this.splitContainer2.Size = new System.Drawing.Size(1138, 245);
            this.splitContainer2.SplitterDistance = 132;
            this.splitContainer2.TabIndex = 0;
            // 
            // errorsTextBox
            // 
            this.errorsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.errorsTextBox.Location = new System.Drawing.Point(0, 0);
            this.errorsTextBox.Multiline = true;
            this.errorsTextBox.Name = "errorsTextBox";
            this.errorsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.errorsTextBox.Size = new System.Drawing.Size(1138, 109);
            this.errorsTextBox.TabIndex = 0;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(558, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(68, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Process Flag";
            // 
            // processFlagFilterCombo
            // 
            this.processFlagFilterCombo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.processFlagFilterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processFlagFilterCombo.FormattingEnabled = true;
            this.processFlagFilterCombo.Location = new System.Drawing.Point(561, 35);
            this.processFlagFilterCombo.Name = "processFlagFilterCombo";
            this.processFlagFilterCombo.Size = new System.Drawing.Size(121, 21);
            this.processFlagFilterCombo.TabIndex = 2;
            this.processFlagFilterCombo.SelectedIndexChanged += new System.EventHandler(this.processFlagFilterCombo_SelectedIndexChanged);
            // 
            // InterfaceMonitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1184, 600);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.accountFilterTextBox);
            this.Controls.Add(this.FilterButton);
            this.Controls.Add(this.ThruDate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.FromDate);
            this.Controls.Add(this.processFlagFilterCombo);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.MessageTypeFilterComboBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.MinimumSize = new System.Drawing.Size(950, 600);
            this.Name = "InterfaceMonitor";
            this.Text = "InterfaceMonitor";
            this.Load += new System.EventHandler(this.InterfaceMonitor_Load);
            ((System.ComponentModel.ISupportInitialize)(this.MessagesGrid)).EndInit();
            this.GridContextMenu.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView MessagesGrid;
        private System.Windows.Forms.ComboBox MessageTypeFilterComboBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox hl7MessageTextBox;
        private System.Windows.Forms.ContextMenuStrip GridContextMenu;
        private System.Windows.Forms.ToolStripMenuItem ReprocessMessage;
        private System.Windows.Forms.DateTimePicker FromDate;
        private System.Windows.Forms.DateTimePicker ThruDate;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button FilterButton;
        private System.Windows.Forms.TextBox accountFilterTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox errorsTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox processFlagFilterCombo;
    }
}