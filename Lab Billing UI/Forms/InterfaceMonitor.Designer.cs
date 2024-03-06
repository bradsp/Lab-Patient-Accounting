
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
            components = new System.ComponentModel.Container();
            MessagesGrid = new System.Windows.Forms.DataGridView();
            GridContextMenu = new System.Windows.Forms.ContextMenuStrip(components);
            ReprocessMessage = new System.Windows.Forms.ToolStripMenuItem();
            markDoNotProcessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            MessageTypeFilterComboBox = new System.Windows.Forms.ComboBox();
            label1 = new System.Windows.Forms.Label();
            hl7MessageTextBox = new System.Windows.Forms.TextBox();
            FromDate = new System.Windows.Forms.DateTimePicker();
            ThruDate = new System.Windows.Forms.DateTimePicker();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            FilterButton = new System.Windows.Forms.Button();
            accountFilterTextBox = new System.Windows.Forms.TextBox();
            label4 = new System.Windows.Forms.Label();
            splitContainer1 = new System.Windows.Forms.SplitContainer();
            splitContainer2 = new System.Windows.Forms.SplitContainer();
            errorsTextBox = new System.Windows.Forms.TextBox();
            label5 = new System.Windows.Forms.Label();
            processFlagFilterCombo = new System.Windows.Forms.ComboBox();
            showMessagesWithErrorsCheckBox = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)MessagesGrid).BeginInit();
            GridContextMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            SuspendLayout();
            // 
            // MessagesGrid
            // 
            MessagesGrid.AllowUserToAddRows = false;
            MessagesGrid.AllowUserToDeleteRows = false;
            MessagesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MessagesGrid.ContextMenuStrip = GridContextMenu;
            MessagesGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            MessagesGrid.Location = new System.Drawing.Point(0, 0);
            MessagesGrid.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MessagesGrid.Name = "MessagesGrid";
            MessagesGrid.ReadOnly = true;
            MessagesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            MessagesGrid.Size = new System.Drawing.Size(1328, 285);
            MessagesGrid.TabIndex = 0;
            MessagesGrid.CellClick += MessagesGrid_CellClick;
            MessagesGrid.DataError += MessagesGrid_DataError;
            MessagesGrid.SelectionChanged += MessagesGrid_SelectionChanged;
            // 
            // GridContextMenu
            // 
            GridContextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { ReprocessMessage, markDoNotProcessToolStripMenuItem });
            GridContextMenu.Name = "GridContextMenu";
            GridContextMenu.Size = new System.Drawing.Size(186, 48);
            // 
            // ReprocessMessage
            // 
            ReprocessMessage.Name = "ReprocessMessage";
            ReprocessMessage.Size = new System.Drawing.Size(185, 22);
            ReprocessMessage.Text = "Reprocess Message";
            ReprocessMessage.Click += ReprocessMessage_Click;
            // 
            // markDoNotProcessToolStripMenuItem
            // 
            markDoNotProcessToolStripMenuItem.Name = "markDoNotProcessToolStripMenuItem";
            markDoNotProcessToolStripMenuItem.Size = new System.Drawing.Size(185, 22);
            markDoNotProcessToolStripMenuItem.Text = "Mark Do Not Process";
            markDoNotProcessToolStripMenuItem.Click += markDoNotProcessToolStripMenuItem_Click;
            // 
            // MessageTypeFilterComboBox
            // 
            MessageTypeFilterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            MessageTypeFilterComboBox.FormattingEnabled = true;
            MessageTypeFilterComboBox.Items.AddRange(new object[] { "All", "ADT", "DFT", "MFN" });
            MessageTypeFilterComboBox.Location = new System.Drawing.Point(454, 42);
            MessageTypeFilterComboBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MessageTypeFilterComboBox.Name = "MessageTypeFilterComboBox";
            MessageTypeFilterComboBox.Size = new System.Drawing.Size(86, 23);
            MessageTypeFilterComboBox.TabIndex = 2;
            MessageTypeFilterComboBox.SelectedIndexChanged += MessageTypeSelect_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(450, 23);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(80, 15);
            label1.TabIndex = 3;
            label1.Text = "Message Type";
            // 
            // hl7MessageTextBox
            // 
            hl7MessageTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            hl7MessageTextBox.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 0);
            hl7MessageTextBox.Location = new System.Drawing.Point(0, 0);
            hl7MessageTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            hl7MessageTextBox.Multiline = true;
            hl7MessageTextBox.Name = "hl7MessageTextBox";
            hl7MessageTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            hl7MessageTextBox.Size = new System.Drawing.Size(1328, 152);
            hl7MessageTextBox.TabIndex = 0;
            hl7MessageTextBox.WordWrap = false;
            // 
            // FromDate
            // 
            FromDate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            FromDate.CustomFormat = "M/d/yy HH:mm";
            FromDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            FromDate.Location = new System.Drawing.Point(899, 40);
            FromDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FromDate.Name = "FromDate";
            FromDate.Size = new System.Drawing.Size(135, 23);
            FromDate.TabIndex = 5;
            // 
            // ThruDate
            // 
            ThruDate.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            ThruDate.CustomFormat = "M/d/yy HH:mm";
            ThruDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            ThruDate.Location = new System.Drawing.Point(1068, 40);
            ThruDate.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            ThruDate.Name = "ThruDate";
            ThruDate.Size = new System.Drawing.Size(132, 23);
            ThruDate.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(896, 22);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(67, 15);
            label2.TabIndex = 6;
            label2.Text = "Date Range";
            // 
            // label3
            // 
            label3.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(1042, 45);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(18, 15);
            label3.TabIndex = 7;
            label3.Text = " - ";
            // 
            // FilterButton
            // 
            FilterButton.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            FilterButton.Location = new System.Drawing.Point(1208, 39);
            FilterButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            FilterButton.Name = "FilterButton";
            FilterButton.Size = new System.Drawing.Size(147, 27);
            FilterButton.TabIndex = 8;
            FilterButton.Text = "Update Date Range";
            FilterButton.UseVisualStyleBackColor = true;
            FilterButton.Click += FilterButton_Click;
            // 
            // accountFilterTextBox
            // 
            accountFilterTextBox.Location = new System.Drawing.Point(246, 42);
            accountFilterTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            accountFilterTextBox.Name = "accountFilterTextBox";
            accountFilterTextBox.Size = new System.Drawing.Size(192, 23);
            accountFilterTextBox.TabIndex = 9;
            accountFilterTextBox.KeyDown += accountFilterTextBox_KeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(243, 23);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(81, 15);
            label4.TabIndex = 10;
            label4.Text = "Filter Account";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            splitContainer1.Location = new System.Drawing.Point(27, 92);
            splitContainer1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(MessagesGrid);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new System.Drawing.Size(1328, 573);
            splitContainer1.SplitterDistance = 285;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 11;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            splitContainer2.Location = new System.Drawing.Point(0, 0);
            splitContainer2.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(hl7MessageTextBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(errorsTextBox);
            splitContainer2.Size = new System.Drawing.Size(1328, 283);
            splitContainer2.SplitterDistance = 152;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 0;
            // 
            // errorsTextBox
            // 
            errorsTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            errorsTextBox.Location = new System.Drawing.Point(0, 0);
            errorsTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            errorsTextBox.Multiline = true;
            errorsTextBox.Name = "errorsTextBox";
            errorsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            errorsTextBox.Size = new System.Drawing.Size(1328, 126);
            errorsTextBox.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(544, 23);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(72, 15);
            label5.TabIndex = 3;
            label5.Text = "Process Flag";
            // 
            // processFlagFilterCombo
            // 
            processFlagFilterCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            processFlagFilterCombo.FormattingEnabled = true;
            processFlagFilterCombo.Location = new System.Drawing.Point(547, 42);
            processFlagFilterCombo.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            processFlagFilterCombo.Name = "processFlagFilterCombo";
            processFlagFilterCombo.Size = new System.Drawing.Size(126, 23);
            processFlagFilterCombo.TabIndex = 2;
            processFlagFilterCombo.SelectedIndexChanged += processFlagFilterCombo_SelectedIndexChanged;
            // 
            // showMessagesWithErrorsCheckBox
            // 
            showMessagesWithErrorsCheckBox.AutoSize = true;
            showMessagesWithErrorsCheckBox.Location = new System.Drawing.Point(681, 44);
            showMessagesWithErrorsCheckBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            showMessagesWithErrorsCheckBox.Name = "showMessagesWithErrorsCheckBox";
            showMessagesWithErrorsCheckBox.Size = new System.Drawing.Size(168, 19);
            showMessagesWithErrorsCheckBox.TabIndex = 12;
            showMessagesWithErrorsCheckBox.Text = "Show Messages with Errors";
            showMessagesWithErrorsCheckBox.UseVisualStyleBackColor = true;
            showMessagesWithErrorsCheckBox.CheckedChanged += showMessagesWithErrorsCheckBox_CheckedChanged;
            // 
            // InterfaceMonitor
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1381, 692);
            Controls.Add(showMessagesWithErrorsCheckBox);
            Controls.Add(splitContainer1);
            Controls.Add(label4);
            Controls.Add(accountFilterTextBox);
            Controls.Add(FilterButton);
            Controls.Add(ThruDate);
            Controls.Add(label3);
            Controls.Add(FromDate);
            Controls.Add(processFlagFilterCombo);
            Controls.Add(label5);
            Controls.Add(MessageTypeFilterComboBox);
            Controls.Add(label1);
            Controls.Add(label2);
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            MinimumSize = new System.Drawing.Size(1106, 686);
            Name = "InterfaceMonitor";
            Text = "InterfaceMonitor";
            Load += InterfaceMonitor_Load;
            ((System.ComponentModel.ISupportInitialize)MessagesGrid).EndInit();
            GridContextMenu.ResumeLayout(false);
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem markDoNotProcessToolStripMenuItem;
        private System.Windows.Forms.CheckBox showMessagesWithErrorsCheckBox;
    }
}