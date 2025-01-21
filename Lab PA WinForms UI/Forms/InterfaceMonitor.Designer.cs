
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
            MessagesGrid = new DataGridView();
            GridContextMenu = new ContextMenuStrip(components);
            ReprocessMessage = new ToolStripMenuItem();
            markDoNotProcessToolStripMenuItem = new ToolStripMenuItem();
            MessageTypeFilterComboBox = new ComboBox();
            label1 = new Label();
            hl7MessageTextBox = new TextBox();
            FromDate = new DateTimePicker();
            ThruDate = new DateTimePicker();
            label2 = new Label();
            label3 = new Label();
            FilterButton = new Button();
            accountFilterTextBox = new TextBox();
            label4 = new Label();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            messageQueueTextBox = new TextBox();
            errorsTextBox = new TextBox();
            label5 = new Label();
            processFlagFilterCombo = new ComboBox();
            showMessagesWithErrorsCheckBox = new CheckBox();
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
            MessagesGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            MessagesGrid.ContextMenuStrip = GridContextMenu;
            MessagesGrid.Dock = DockStyle.Fill;
            MessagesGrid.Location = new Point(0, 0);
            MessagesGrid.Margin = new Padding(4, 3, 4, 3);
            MessagesGrid.Name = "MessagesGrid";
            MessagesGrid.ReadOnly = true;
            MessagesGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            MessagesGrid.Size = new Size(1328, 285);
            MessagesGrid.TabIndex = 0;
            MessagesGrid.CellClick += MessagesGrid_CellClick;
            MessagesGrid.DataError += MessagesGrid_DataError;
            MessagesGrid.SelectionChanged += MessagesGrid_SelectionChanged;
            // 
            // GridContextMenu
            // 
            GridContextMenu.Items.AddRange(new ToolStripItem[] { ReprocessMessage, markDoNotProcessToolStripMenuItem });
            GridContextMenu.Name = "GridContextMenu";
            GridContextMenu.Size = new Size(186, 48);
            // 
            // ReprocessMessage
            // 
            ReprocessMessage.Name = "ReprocessMessage";
            ReprocessMessage.Size = new Size(185, 22);
            ReprocessMessage.Text = "Reprocess Message";
            ReprocessMessage.Click += ReprocessMessage_Click;
            // 
            // markDoNotProcessToolStripMenuItem
            // 
            markDoNotProcessToolStripMenuItem.Name = "markDoNotProcessToolStripMenuItem";
            markDoNotProcessToolStripMenuItem.Size = new Size(185, 22);
            markDoNotProcessToolStripMenuItem.Text = "Mark Do Not Process";
            markDoNotProcessToolStripMenuItem.Click += markDoNotProcessToolStripMenuItem_Click;
            // 
            // MessageTypeFilterComboBox
            // 
            MessageTypeFilterComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MessageTypeFilterComboBox.FormattingEnabled = true;
            MessageTypeFilterComboBox.Items.AddRange(new object[] { "All", "ADT", "DFT", "MFN" });
            MessageTypeFilterComboBox.Location = new Point(454, 42);
            MessageTypeFilterComboBox.Margin = new Padding(4, 3, 4, 3);
            MessageTypeFilterComboBox.Name = "MessageTypeFilterComboBox";
            MessageTypeFilterComboBox.Size = new Size(86, 23);
            MessageTypeFilterComboBox.TabIndex = 2;
            MessageTypeFilterComboBox.SelectedIndexChanged += MessageTypeSelect_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(450, 23);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(80, 15);
            label1.TabIndex = 3;
            label1.Text = "Message Type";
            // 
            // hl7MessageTextBox
            // 
            hl7MessageTextBox.Dock = DockStyle.Fill;
            hl7MessageTextBox.Font = new Font("Courier New", 9F, FontStyle.Regular, GraphicsUnit.Point, 0);
            hl7MessageTextBox.Location = new Point(0, 0);
            hl7MessageTextBox.Margin = new Padding(4, 3, 4, 3);
            hl7MessageTextBox.Multiline = true;
            hl7MessageTextBox.Name = "hl7MessageTextBox";
            hl7MessageTextBox.ScrollBars = ScrollBars.Both;
            hl7MessageTextBox.Size = new Size(1328, 152);
            hl7MessageTextBox.TabIndex = 0;
            hl7MessageTextBox.WordWrap = false;
            // 
            // FromDate
            // 
            FromDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FromDate.CustomFormat = "M/d/yy HH:mm";
            FromDate.Format = DateTimePickerFormat.Custom;
            FromDate.Location = new Point(899, 40);
            FromDate.Margin = new Padding(4, 3, 4, 3);
            FromDate.Name = "FromDate";
            FromDate.Size = new Size(135, 23);
            FromDate.TabIndex = 5;
            // 
            // ThruDate
            // 
            ThruDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            ThruDate.CustomFormat = "M/d/yy HH:mm";
            ThruDate.Format = DateTimePickerFormat.Custom;
            ThruDate.Location = new Point(1068, 40);
            ThruDate.Margin = new Padding(4, 3, 4, 3);
            ThruDate.Name = "ThruDate";
            ThruDate.Size = new Size(132, 23);
            ThruDate.TabIndex = 5;
            // 
            // label2
            // 
            label2.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label2.AutoSize = true;
            label2.Location = new Point(896, 22);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(67, 15);
            label2.TabIndex = 6;
            label2.Text = "Date Range";
            // 
            // label3
            // 
            label3.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label3.AutoSize = true;
            label3.Location = new Point(1042, 45);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(18, 15);
            label3.TabIndex = 7;
            label3.Text = " - ";
            // 
            // FilterButton
            // 
            FilterButton.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            FilterButton.Location = new Point(1208, 39);
            FilterButton.Margin = new Padding(4, 3, 4, 3);
            FilterButton.Name = "FilterButton";
            FilterButton.Size = new Size(147, 27);
            FilterButton.TabIndex = 8;
            FilterButton.Text = "Update Date Range";
            FilterButton.UseVisualStyleBackColor = true;
            FilterButton.Click += FilterButton_Click;
            // 
            // accountFilterTextBox
            // 
            accountFilterTextBox.Location = new Point(246, 42);
            accountFilterTextBox.Margin = new Padding(4, 3, 4, 3);
            accountFilterTextBox.Name = "accountFilterTextBox";
            accountFilterTextBox.Size = new Size(192, 23);
            accountFilterTextBox.TabIndex = 9;
            accountFilterTextBox.KeyDown += accountFilterTextBox_KeyDown;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(243, 23);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(81, 15);
            label4.TabIndex = 10;
            label4.Text = "Filter Account";
            // 
            // splitContainer1
            // 
            splitContainer1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            splitContainer1.Location = new Point(27, 92);
            splitContainer1.Margin = new Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(MessagesGrid);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(splitContainer2);
            splitContainer1.Size = new Size(1328, 573);
            splitContainer1.SplitterDistance = 285;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 11;
            // 
            // splitContainer2
            // 
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            splitContainer2.Orientation = Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(hl7MessageTextBox);
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(messageQueueTextBox);
            splitContainer2.Panel2.Controls.Add(errorsTextBox);
            splitContainer2.Size = new Size(1328, 283);
            splitContainer2.SplitterDistance = 152;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 0;
            // 
            // messageQueueTextBox
            // 
            messageQueueTextBox.Dock = DockStyle.Right;
            messageQueueTextBox.Location = new Point(927, 0);
            messageQueueTextBox.Margin = new Padding(4, 3, 4, 3);
            messageQueueTextBox.Multiline = true;
            messageQueueTextBox.Name = "messageQueueTextBox";
            messageQueueTextBox.ScrollBars = ScrollBars.Vertical;
            messageQueueTextBox.Size = new Size(401, 126);
            messageQueueTextBox.TabIndex = 0;
            // 
            // errorsTextBox
            // 
            errorsTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            errorsTextBox.Location = new Point(0, 0);
            errorsTextBox.Margin = new Padding(4, 3, 4, 3);
            errorsTextBox.Multiline = true;
            errorsTextBox.Name = "errorsTextBox";
            errorsTextBox.ScrollBars = ScrollBars.Vertical;
            errorsTextBox.Size = new Size(919, 126);
            errorsTextBox.TabIndex = 0;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(544, 23);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(72, 15);
            label5.TabIndex = 3;
            label5.Text = "Process Flag";
            // 
            // processFlagFilterCombo
            // 
            processFlagFilterCombo.DropDownStyle = ComboBoxStyle.DropDownList;
            processFlagFilterCombo.FormattingEnabled = true;
            processFlagFilterCombo.Location = new Point(547, 42);
            processFlagFilterCombo.Margin = new Padding(4, 3, 4, 3);
            processFlagFilterCombo.Name = "processFlagFilterCombo";
            processFlagFilterCombo.Size = new Size(126, 23);
            processFlagFilterCombo.TabIndex = 2;
            processFlagFilterCombo.SelectedIndexChanged += processFlagFilterCombo_SelectedIndexChanged;
            // 
            // showMessagesWithErrorsCheckBox
            // 
            showMessagesWithErrorsCheckBox.AutoSize = true;
            showMessagesWithErrorsCheckBox.Location = new Point(681, 44);
            showMessagesWithErrorsCheckBox.Margin = new Padding(4, 3, 4, 3);
            showMessagesWithErrorsCheckBox.Name = "showMessagesWithErrorsCheckBox";
            showMessagesWithErrorsCheckBox.Size = new Size(168, 19);
            showMessagesWithErrorsCheckBox.TabIndex = 12;
            showMessagesWithErrorsCheckBox.Text = "Show Messages with Errors";
            showMessagesWithErrorsCheckBox.UseVisualStyleBackColor = true;
            showMessagesWithErrorsCheckBox.CheckedChanged += showMessagesWithErrorsCheckBox_CheckedChanged;
            // 
            // InterfaceMonitor
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1381, 692);
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
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(1106, 686);
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
        private TextBox messageQueueTextBox;
    }
}