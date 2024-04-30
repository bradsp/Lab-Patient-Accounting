namespace LabBilling.Forms
{
    partial class PatientCollectionsRunWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PatientCollectionsRunWizard));
            tabControl1 = new UserControls.WizardPages();
            introTabPage = new TabPage();
            label6 = new Label();
            richTextBox1 = new RichTextBox();
            sendCollectionsTabPage = new TabPage();
            skipSendCollectionsCheckBox = new CheckBox();
            sendToCollectionsTextbox = new TextBox();
            sendToCollectionsProgressBar = new ProgressBar();
            sendToCollectionsStartButton = new Button();
            label1 = new Label();
            compileStatementsTabPage = new TabPage();
            batchNoLabel = new Label();
            throughDateLabel = new Label();
            label5 = new Label();
            label4 = new Label();
            compileStmtsStartButton = new Button();
            compileStatementsTextBox = new TextBox();
            compileStatementsProgressBar = new ProgressBar();
            label2 = new Label();
            createStmtFileTabPage = new TabPage();
            createStmtFileStartButton = new Button();
            createStmtFileTextBox = new TextBox();
            createStmtFileProgressBar = new ProgressBar();
            label3 = new Label();
            cancelButton = new Button();
            backButton = new Button();
            nextButton = new Button();
            finishButton = new Button();
            bannerLabel = new Label();
            tabControl1.SuspendLayout();
            introTabPage.SuspendLayout();
            sendCollectionsTabPage.SuspendLayout();
            compileStatementsTabPage.SuspendLayout();
            createStmtFileTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Appearance = TabAppearance.FlatButtons;
            tabControl1.Controls.Add(introTabPage);
            tabControl1.Controls.Add(sendCollectionsTabPage);
            tabControl1.Controls.Add(compileStatementsTabPage);
            tabControl1.Controls.Add(createStmtFileTabPage);
            tabControl1.Location = new Point(14, 37);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(905, 434);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // introTabPage
            // 
            introTabPage.BackColor = Color.White;
            introTabPage.Controls.Add(label6);
            introTabPage.Controls.Add(richTextBox1);
            introTabPage.Location = new Point(4, 27);
            introTabPage.Name = "introTabPage";
            introTabPage.Padding = new Padding(3);
            introTabPage.Size = new Size(897, 403);
            introTabPage.TabIndex = 3;
            introTabPage.Text = "Intro";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(28, 27);
            label6.Name = "label6";
            label6.Size = new Size(477, 15);
            label6.TabIndex = 1;
            label6.Text = "This process will generate patient statements and send qualifying accounts to collections.";
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = BorderStyle.None;
            richTextBox1.Location = new Point(28, 106);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(840, 250);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // sendCollectionsTabPage
            // 
            sendCollectionsTabPage.BackColor = Color.White;
            sendCollectionsTabPage.Controls.Add(skipSendCollectionsCheckBox);
            sendCollectionsTabPage.Controls.Add(sendToCollectionsTextbox);
            sendCollectionsTabPage.Controls.Add(sendToCollectionsProgressBar);
            sendCollectionsTabPage.Controls.Add(sendToCollectionsStartButton);
            sendCollectionsTabPage.Controls.Add(label1);
            sendCollectionsTabPage.Location = new Point(4, 27);
            sendCollectionsTabPage.Margin = new Padding(4, 3, 4, 3);
            sendCollectionsTabPage.Name = "sendCollectionsTabPage";
            sendCollectionsTabPage.Padding = new Padding(4, 3, 4, 3);
            sendCollectionsTabPage.Size = new Size(897, 403);
            sendCollectionsTabPage.TabIndex = 0;
            sendCollectionsTabPage.Text = "Send to Collections";
            // 
            // skipSendCollectionsCheckBox
            // 
            skipSendCollectionsCheckBox.AutoSize = true;
            skipSendCollectionsCheckBox.Location = new Point(695, 150);
            skipSendCollectionsCheckBox.Name = "skipSendCollectionsCheckBox";
            skipSendCollectionsCheckBox.Size = new Size(156, 19);
            skipSendCollectionsCheckBox.TabIndex = 4;
            skipSendCollectionsCheckBox.Text = "Skip Sending Collections";
            skipSendCollectionsCheckBox.UseVisualStyleBackColor = true;
            skipSendCollectionsCheckBox.CheckedChanged += skipSendCollectionsCheckBox_CheckedChanged;
            // 
            // sendToCollectionsTextbox
            // 
            sendToCollectionsTextbox.BackColor = Color.White;
            sendToCollectionsTextbox.BorderStyle = BorderStyle.None;
            sendToCollectionsTextbox.Location = new Point(46, 140);
            sendToCollectionsTextbox.Margin = new Padding(4, 3, 4, 3);
            sendToCollectionsTextbox.Multiline = true;
            sendToCollectionsTextbox.Name = "sendToCollectionsTextbox";
            sendToCollectionsTextbox.ReadOnly = true;
            sendToCollectionsTextbox.ScrollBars = ScrollBars.Vertical;
            sendToCollectionsTextbox.Size = new Size(618, 217);
            sendToCollectionsTextbox.TabIndex = 3;
            // 
            // sendToCollectionsProgressBar
            // 
            sendToCollectionsProgressBar.Location = new Point(46, 106);
            sendToCollectionsProgressBar.Margin = new Padding(4, 3, 4, 3);
            sendToCollectionsProgressBar.Name = "sendToCollectionsProgressBar";
            sendToCollectionsProgressBar.Size = new Size(618, 27);
            sendToCollectionsProgressBar.TabIndex = 2;
            // 
            // sendToCollectionsStartButton
            // 
            sendToCollectionsStartButton.Location = new Point(691, 106);
            sendToCollectionsStartButton.Margin = new Padding(4, 3, 4, 3);
            sendToCollectionsStartButton.Name = "sendToCollectionsStartButton";
            sendToCollectionsStartButton.Size = new Size(88, 27);
            sendToCollectionsStartButton.TabIndex = 1;
            sendToCollectionsStartButton.Text = "Start";
            sendToCollectionsStartButton.UseVisualStyleBackColor = true;
            sendToCollectionsStartButton.Click += sendToCollectionsStartButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(48, 47);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(591, 15);
            label1.TabIndex = 0;
            label1.Text = "This step will generate the collections file to send to MSCB, and write off those accounts. Click Start to proceed.";
            // 
            // compileStatementsTabPage
            // 
            compileStatementsTabPage.BackColor = Color.White;
            compileStatementsTabPage.Controls.Add(batchNoLabel);
            compileStatementsTabPage.Controls.Add(throughDateLabel);
            compileStatementsTabPage.Controls.Add(label5);
            compileStatementsTabPage.Controls.Add(label4);
            compileStatementsTabPage.Controls.Add(compileStmtsStartButton);
            compileStatementsTabPage.Controls.Add(compileStatementsTextBox);
            compileStatementsTabPage.Controls.Add(compileStatementsProgressBar);
            compileStatementsTabPage.Controls.Add(label2);
            compileStatementsTabPage.Location = new Point(4, 27);
            compileStatementsTabPage.Margin = new Padding(4, 3, 4, 3);
            compileStatementsTabPage.Name = "compileStatementsTabPage";
            compileStatementsTabPage.Padding = new Padding(4, 3, 4, 3);
            compileStatementsTabPage.Size = new Size(897, 403);
            compileStatementsTabPage.TabIndex = 1;
            compileStatementsTabPage.Text = "Compile Statements";
            // 
            // batchNoLabel
            // 
            batchNoLabel.AutoSize = true;
            batchNoLabel.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            batchNoLabel.Location = new Point(168, 78);
            batchNoLabel.Margin = new Padding(4, 0, 4, 0);
            batchNoLabel.Name = "batchNoLabel";
            batchNoLabel.Size = new Size(91, 18);
            batchNoLabel.TabIndex = 5;
            batchNoLabel.Text = "<batchNo>";
            // 
            // throughDateLabel
            // 
            throughDateLabel.AutoSize = true;
            throughDateLabel.Font = new Font("Microsoft Sans Serif", 11.25F, FontStyle.Bold, GraphicsUnit.Point, 0);
            throughDateLabel.Location = new Point(168, 45);
            throughDateLabel.Margin = new Padding(4, 0, 4, 0);
            throughDateLabel.Name = "throughDateLabel";
            throughDateLabel.Size = new Size(122, 18);
            throughDateLabel.TabIndex = 5;
            throughDateLabel.Text = "<through date>";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(72, 83);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(59, 15);
            label5.TabIndex = 4;
            label5.Text = "Batch No:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(72, 50);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(82, 15);
            label4.TabIndex = 4;
            label4.Text = "Through Date:";
            // 
            // compileStmtsStartButton
            // 
            compileStmtsStartButton.Location = new Point(542, 126);
            compileStmtsStartButton.Margin = new Padding(4, 3, 4, 3);
            compileStmtsStartButton.Name = "compileStmtsStartButton";
            compileStmtsStartButton.Size = new Size(88, 27);
            compileStmtsStartButton.TabIndex = 3;
            compileStmtsStartButton.Text = "Start";
            compileStmtsStartButton.UseVisualStyleBackColor = true;
            compileStmtsStartButton.Click += compileStmtsStartButton_Click;
            // 
            // compileStatementsTextBox
            // 
            compileStatementsTextBox.Location = new Point(76, 173);
            compileStatementsTextBox.Margin = new Padding(4, 3, 4, 3);
            compileStatementsTextBox.Multiline = true;
            compileStatementsTextBox.Name = "compileStatementsTextBox";
            compileStatementsTextBox.Size = new Size(436, 202);
            compileStatementsTextBox.TabIndex = 2;
            // 
            // compileStatementsProgressBar
            // 
            compileStatementsProgressBar.Location = new Point(76, 126);
            compileStatementsProgressBar.Margin = new Padding(4, 3, 4, 3);
            compileStatementsProgressBar.Name = "compileStatementsProgressBar";
            compileStatementsProgressBar.Size = new Size(436, 27);
            compileStatementsProgressBar.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(72, 20);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(419, 15);
            label2.TabIndex = 0;
            label2.Text = "This step will compile the records to receive statements. Click Start to proceed.";
            // 
            // createStmtFileTabPage
            // 
            createStmtFileTabPage.BackColor = Color.White;
            createStmtFileTabPage.Controls.Add(createStmtFileStartButton);
            createStmtFileTabPage.Controls.Add(createStmtFileTextBox);
            createStmtFileTabPage.Controls.Add(createStmtFileProgressBar);
            createStmtFileTabPage.Controls.Add(label3);
            createStmtFileTabPage.Location = new Point(4, 27);
            createStmtFileTabPage.Margin = new Padding(4, 3, 4, 3);
            createStmtFileTabPage.Name = "createStmtFileTabPage";
            createStmtFileTabPage.Size = new Size(897, 403);
            createStmtFileTabPage.TabIndex = 2;
            createStmtFileTabPage.Text = "Create Statement File";
            // 
            // createStmtFileStartButton
            // 
            createStmtFileStartButton.Location = new Point(561, 82);
            createStmtFileStartButton.Margin = new Padding(4, 3, 4, 3);
            createStmtFileStartButton.Name = "createStmtFileStartButton";
            createStmtFileStartButton.Size = new Size(88, 27);
            createStmtFileStartButton.TabIndex = 3;
            createStmtFileStartButton.Text = "Start";
            createStmtFileStartButton.UseVisualStyleBackColor = true;
            createStmtFileStartButton.Click += createStmtFileStartButton_Click;
            // 
            // createStmtFileTextBox
            // 
            createStmtFileTextBox.Location = new Point(80, 115);
            createStmtFileTextBox.Margin = new Padding(4, 3, 4, 3);
            createStmtFileTextBox.Multiline = true;
            createStmtFileTextBox.Name = "createStmtFileTextBox";
            createStmtFileTextBox.Size = new Size(442, 245);
            createStmtFileTextBox.TabIndex = 2;
            // 
            // createStmtFileProgressBar
            // 
            createStmtFileProgressBar.Location = new Point(80, 82);
            createStmtFileProgressBar.Margin = new Padding(4, 3, 4, 3);
            createStmtFileProgressBar.Name = "createStmtFileProgressBar";
            createStmtFileProgressBar.Size = new Size(442, 27);
            createStmtFileProgressBar.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(68, 48);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(432, 15);
            label3.TabIndex = 0;
            label3.Text = "This step will generate the statement file to be sent to DNI. Click Start to proceed.";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            cancelButton.Location = new Point(827, 479);
            cancelButton.Margin = new Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new Size(88, 27);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Close";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // backButton
            // 
            backButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            backButton.Location = new Point(544, 479);
            backButton.Margin = new Padding(4, 3, 4, 3);
            backButton.Name = "backButton";
            backButton.Size = new Size(88, 27);
            backButton.TabIndex = 1;
            backButton.Text = "< Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Visible = false;
            // 
            // nextButton
            // 
            nextButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            nextButton.Location = new Point(638, 479);
            nextButton.Margin = new Padding(4, 3, 4, 3);
            nextButton.Name = "nextButton";
            nextButton.Size = new Size(88, 27);
            nextButton.TabIndex = 1;
            nextButton.Text = "Next >";
            nextButton.UseVisualStyleBackColor = true;
            nextButton.Click += nextButton_Click;
            // 
            // finishButton
            // 
            finishButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            finishButton.Location = new Point(733, 479);
            finishButton.Margin = new Padding(4, 3, 4, 3);
            finishButton.Name = "finishButton";
            finishButton.Size = new Size(88, 27);
            finishButton.TabIndex = 1;
            finishButton.Text = "Finish";
            finishButton.UseVisualStyleBackColor = true;
            finishButton.Visible = false;
            // 
            // bannerLabel
            // 
            bannerLabel.AutoSize = true;
            bannerLabel.Font = new Font("Microsoft Sans Serif", 12F, FontStyle.Bold, GraphicsUnit.Point, 0);
            bannerLabel.Location = new Point(15, 10);
            bannerLabel.Margin = new Padding(4, 0, 4, 0);
            bannerLabel.Name = "bannerLabel";
            bannerLabel.Size = new Size(119, 20);
            bannerLabel.TabIndex = 2;
            bannerLabel.Text = "<bannerText>";
            // 
            // PatientCollectionsRunWizard
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(933, 519);
            Controls.Add(bannerLabel);
            Controls.Add(finishButton);
            Controls.Add(nextButton);
            Controls.Add(backButton);
            Controls.Add(cancelButton);
            Controls.Add(tabControl1);
            ForeColor = Color.Black;
            HelpButton = true;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PatientCollectionsRunWizard";
            Text = "Patient Collections Run";
            HelpButtonClicked += PatientCollectionsRunWizard_HelpButtonClicked;
            Load += PatientCollectionsRunWizard_Load;
            HelpRequested += PatientCollectionsRunWizard_HelpRequested;
            tabControl1.ResumeLayout(false);
            introTabPage.ResumeLayout(false);
            introTabPage.PerformLayout();
            sendCollectionsTabPage.ResumeLayout(false);
            sendCollectionsTabPage.PerformLayout();
            compileStatementsTabPage.ResumeLayout(false);
            compileStatementsTabPage.PerformLayout();
            createStmtFileTabPage.ResumeLayout(false);
            createStmtFileTabPage.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private LabBilling.UserControls.WizardPages tabControl1;
        private System.Windows.Forms.TabPage sendCollectionsTabPage;
        private System.Windows.Forms.TabPage compileStatementsTabPage;
        private System.Windows.Forms.Button cancelButton;
        private System.Windows.Forms.Button backButton;
        private System.Windows.Forms.Button nextButton;
        private System.Windows.Forms.Button finishButton;
        private System.Windows.Forms.Button sendToCollectionsStartButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage createStmtFileTabPage;
        private System.Windows.Forms.TextBox sendToCollectionsTextbox;
        private System.Windows.Forms.ProgressBar sendToCollectionsProgressBar;
        private System.Windows.Forms.Button compileStmtsStartButton;
        private System.Windows.Forms.TextBox compileStatementsTextBox;
        private System.Windows.Forms.ProgressBar compileStatementsProgressBar;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox createStmtFileTextBox;
        private System.Windows.Forms.ProgressBar createStmtFileProgressBar;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button createStmtFileStartButton;
        private System.Windows.Forms.Label batchNoLabel;
        private System.Windows.Forms.Label throughDateLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label bannerLabel;
        private System.Windows.Forms.TabPage introTabPage;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox skipSendCollectionsCheckBox;
    }
}