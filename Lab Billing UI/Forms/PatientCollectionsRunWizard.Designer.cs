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
            introTabPage = new System.Windows.Forms.TabPage();
            label6 = new System.Windows.Forms.Label();
            richTextBox1 = new System.Windows.Forms.RichTextBox();
            sendCollectionsTabPage = new System.Windows.Forms.TabPage();
            sendToCollectionsTextbox = new System.Windows.Forms.TextBox();
            sendToCollectionsProgressBar = new System.Windows.Forms.ProgressBar();
            sendToCollectionsStartButton = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            compileStatementsTabPage = new System.Windows.Forms.TabPage();
            batchNoLabel = new System.Windows.Forms.Label();
            throughDateLabel = new System.Windows.Forms.Label();
            label5 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            compileStmtsStartButton = new System.Windows.Forms.Button();
            compileStatementsTextBox = new System.Windows.Forms.TextBox();
            compileStatementsProgressBar = new System.Windows.Forms.ProgressBar();
            label2 = new System.Windows.Forms.Label();
            createStmtFileTabPage = new System.Windows.Forms.TabPage();
            createStmtFileStartButton = new System.Windows.Forms.Button();
            createStmtFileTextBox = new System.Windows.Forms.TextBox();
            createStmtFileProgressBar = new System.Windows.Forms.ProgressBar();
            label3 = new System.Windows.Forms.Label();
            cancelButton = new System.Windows.Forms.Button();
            backButton = new System.Windows.Forms.Button();
            nextButton = new System.Windows.Forms.Button();
            finishButton = new System.Windows.Forms.Button();
            bannerLabel = new System.Windows.Forms.Label();
            tabControl1.SuspendLayout();
            introTabPage.SuspendLayout();
            sendCollectionsTabPage.SuspendLayout();
            compileStatementsTabPage.SuspendLayout();
            createStmtFileTabPage.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            tabControl1.Appearance = System.Windows.Forms.TabAppearance.FlatButtons;
            tabControl1.Controls.Add(introTabPage);
            tabControl1.Controls.Add(sendCollectionsTabPage);
            tabControl1.Controls.Add(compileStatementsTabPage);
            tabControl1.Controls.Add(createStmtFileTabPage);
            tabControl1.Location = new System.Drawing.Point(14, 37);
            tabControl1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new System.Drawing.Size(905, 434);
            tabControl1.TabIndex = 0;
            tabControl1.SelectedIndexChanged += tabControl1_SelectedIndexChanged;
            // 
            // introTabPage
            // 
            introTabPage.BackColor = System.Drawing.Color.White;
            introTabPage.Controls.Add(label6);
            introTabPage.Controls.Add(richTextBox1);
            introTabPage.Location = new System.Drawing.Point(4, 27);
            introTabPage.Name = "introTabPage";
            introTabPage.Padding = new System.Windows.Forms.Padding(3);
            introTabPage.Size = new System.Drawing.Size(897, 403);
            introTabPage.TabIndex = 3;
            introTabPage.Text = "Intro";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(28, 27);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(477, 15);
            label6.TabIndex = 1;
            label6.Text = "This process will generate patient statements and send qualifying accounts to collections.";
            // 
            // richTextBox1
            // 
            richTextBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            richTextBox1.Location = new System.Drawing.Point(28, 106);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new System.Drawing.Size(840, 250);
            richTextBox1.TabIndex = 0;
            richTextBox1.Text = resources.GetString("richTextBox1.Text");
            // 
            // sendCollectionsTabPage
            // 
            sendCollectionsTabPage.BackColor = System.Drawing.Color.White;
            sendCollectionsTabPage.Controls.Add(sendToCollectionsTextbox);
            sendCollectionsTabPage.Controls.Add(sendToCollectionsProgressBar);
            sendCollectionsTabPage.Controls.Add(sendToCollectionsStartButton);
            sendCollectionsTabPage.Controls.Add(label1);
            sendCollectionsTabPage.Location = new System.Drawing.Point(4, 27);
            sendCollectionsTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            sendCollectionsTabPage.Name = "sendCollectionsTabPage";
            sendCollectionsTabPage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            sendCollectionsTabPage.Size = new System.Drawing.Size(897, 403);
            sendCollectionsTabPage.TabIndex = 0;
            sendCollectionsTabPage.Text = "Send to Collections";
            // 
            // sendToCollectionsTextbox
            // 
            sendToCollectionsTextbox.Location = new System.Drawing.Point(46, 140);
            sendToCollectionsTextbox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            sendToCollectionsTextbox.Multiline = true;
            sendToCollectionsTextbox.Name = "sendToCollectionsTextbox";
            sendToCollectionsTextbox.Size = new System.Drawing.Size(618, 217);
            sendToCollectionsTextbox.TabIndex = 3;
            // 
            // sendToCollectionsProgressBar
            // 
            sendToCollectionsProgressBar.Location = new System.Drawing.Point(46, 106);
            sendToCollectionsProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            sendToCollectionsProgressBar.Name = "sendToCollectionsProgressBar";
            sendToCollectionsProgressBar.Size = new System.Drawing.Size(618, 27);
            sendToCollectionsProgressBar.TabIndex = 2;
            // 
            // sendToCollectionsStartButton
            // 
            sendToCollectionsStartButton.Location = new System.Drawing.Point(691, 106);
            sendToCollectionsStartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            sendToCollectionsStartButton.Name = "sendToCollectionsStartButton";
            sendToCollectionsStartButton.Size = new System.Drawing.Size(88, 27);
            sendToCollectionsStartButton.TabIndex = 1;
            sendToCollectionsStartButton.Text = "Start";
            sendToCollectionsStartButton.UseVisualStyleBackColor = true;
            sendToCollectionsStartButton.Click += sendToCollectionsStartButton_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new System.Drawing.Point(48, 47);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(591, 15);
            label1.TabIndex = 0;
            label1.Text = "This step will generate the collections file to send to MSCB, and write off those accounts. Click Start to proceed.";
            // 
            // compileStatementsTabPage
            // 
            compileStatementsTabPage.BackColor = System.Drawing.Color.White;
            compileStatementsTabPage.Controls.Add(batchNoLabel);
            compileStatementsTabPage.Controls.Add(throughDateLabel);
            compileStatementsTabPage.Controls.Add(label5);
            compileStatementsTabPage.Controls.Add(label4);
            compileStatementsTabPage.Controls.Add(compileStmtsStartButton);
            compileStatementsTabPage.Controls.Add(compileStatementsTextBox);
            compileStatementsTabPage.Controls.Add(compileStatementsProgressBar);
            compileStatementsTabPage.Controls.Add(label2);
            compileStatementsTabPage.Location = new System.Drawing.Point(4, 27);
            compileStatementsTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            compileStatementsTabPage.Name = "compileStatementsTabPage";
            compileStatementsTabPage.Padding = new System.Windows.Forms.Padding(4, 3, 4, 3);
            compileStatementsTabPage.Size = new System.Drawing.Size(897, 403);
            compileStatementsTabPage.TabIndex = 1;
            compileStatementsTabPage.Text = "Compile Statements";
            // 
            // batchNoLabel
            // 
            batchNoLabel.AutoSize = true;
            batchNoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            batchNoLabel.Location = new System.Drawing.Point(168, 78);
            batchNoLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            batchNoLabel.Name = "batchNoLabel";
            batchNoLabel.Size = new System.Drawing.Size(91, 18);
            batchNoLabel.TabIndex = 5;
            batchNoLabel.Text = "<batchNo>";
            // 
            // throughDateLabel
            // 
            throughDateLabel.AutoSize = true;
            throughDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            throughDateLabel.Location = new System.Drawing.Point(168, 45);
            throughDateLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            throughDateLabel.Name = "throughDateLabel";
            throughDateLabel.Size = new System.Drawing.Size(122, 18);
            throughDateLabel.TabIndex = 5;
            throughDateLabel.Text = "<through date>";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new System.Drawing.Point(72, 83);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(59, 15);
            label5.TabIndex = 4;
            label5.Text = "Batch No:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new System.Drawing.Point(72, 50);
            label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(82, 15);
            label4.TabIndex = 4;
            label4.Text = "Through Date:";
            // 
            // compileStmtsStartButton
            // 
            compileStmtsStartButton.Location = new System.Drawing.Point(542, 126);
            compileStmtsStartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            compileStmtsStartButton.Name = "compileStmtsStartButton";
            compileStmtsStartButton.Size = new System.Drawing.Size(88, 27);
            compileStmtsStartButton.TabIndex = 3;
            compileStmtsStartButton.Text = "Start";
            compileStmtsStartButton.UseVisualStyleBackColor = true;
            compileStmtsStartButton.Click += compileStmtsStartButton_Click;
            // 
            // compileStatementsTextBox
            // 
            compileStatementsTextBox.Location = new System.Drawing.Point(76, 173);
            compileStatementsTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            compileStatementsTextBox.Multiline = true;
            compileStatementsTextBox.Name = "compileStatementsTextBox";
            compileStatementsTextBox.Size = new System.Drawing.Size(436, 202);
            compileStatementsTextBox.TabIndex = 2;
            // 
            // compileStatementsProgressBar
            // 
            compileStatementsProgressBar.Location = new System.Drawing.Point(76, 126);
            compileStatementsProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            compileStatementsProgressBar.Name = "compileStatementsProgressBar";
            compileStatementsProgressBar.Size = new System.Drawing.Size(436, 27);
            compileStatementsProgressBar.TabIndex = 1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(72, 20);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(419, 15);
            label2.TabIndex = 0;
            label2.Text = "This step will compile the records to receive statements. Click Start to proceed.";
            // 
            // createStmtFileTabPage
            // 
            createStmtFileTabPage.BackColor = System.Drawing.Color.White;
            createStmtFileTabPage.Controls.Add(createStmtFileStartButton);
            createStmtFileTabPage.Controls.Add(createStmtFileTextBox);
            createStmtFileTabPage.Controls.Add(createStmtFileProgressBar);
            createStmtFileTabPage.Controls.Add(label3);
            createStmtFileTabPage.Location = new System.Drawing.Point(4, 27);
            createStmtFileTabPage.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            createStmtFileTabPage.Name = "createStmtFileTabPage";
            createStmtFileTabPage.Size = new System.Drawing.Size(897, 403);
            createStmtFileTabPage.TabIndex = 2;
            createStmtFileTabPage.Text = "Create Statement File";
            // 
            // createStmtFileStartButton
            // 
            createStmtFileStartButton.Location = new System.Drawing.Point(561, 82);
            createStmtFileStartButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            createStmtFileStartButton.Name = "createStmtFileStartButton";
            createStmtFileStartButton.Size = new System.Drawing.Size(88, 27);
            createStmtFileStartButton.TabIndex = 3;
            createStmtFileStartButton.Text = "Start";
            createStmtFileStartButton.UseVisualStyleBackColor = true;
            createStmtFileStartButton.Click += createStmtFileStartButton_Click;
            // 
            // createStmtFileTextBox
            // 
            createStmtFileTextBox.Location = new System.Drawing.Point(80, 115);
            createStmtFileTextBox.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            createStmtFileTextBox.Multiline = true;
            createStmtFileTextBox.Name = "createStmtFileTextBox";
            createStmtFileTextBox.Size = new System.Drawing.Size(442, 245);
            createStmtFileTextBox.TabIndex = 2;
            // 
            // createStmtFileProgressBar
            // 
            createStmtFileProgressBar.Location = new System.Drawing.Point(80, 82);
            createStmtFileProgressBar.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            createStmtFileProgressBar.Name = "createStmtFileProgressBar";
            createStmtFileProgressBar.Size = new System.Drawing.Size(442, 27);
            createStmtFileProgressBar.TabIndex = 1;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(68, 48);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(432, 15);
            label3.TabIndex = 0;
            label3.Text = "This step will generate the statement file to be sent to DNI. Click Start to proceed.";
            // 
            // cancelButton
            // 
            cancelButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            cancelButton.Location = new System.Drawing.Point(827, 479);
            cancelButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            cancelButton.Name = "cancelButton";
            cancelButton.Size = new System.Drawing.Size(88, 27);
            cancelButton.TabIndex = 1;
            cancelButton.Text = "Close";
            cancelButton.UseVisualStyleBackColor = true;
            cancelButton.Click += cancelButton_Click;
            // 
            // backButton
            // 
            backButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            backButton.Location = new System.Drawing.Point(544, 479);
            backButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            backButton.Name = "backButton";
            backButton.Size = new System.Drawing.Size(88, 27);
            backButton.TabIndex = 1;
            backButton.Text = "< Back";
            backButton.UseVisualStyleBackColor = true;
            backButton.Visible = false;
            // 
            // nextButton
            // 
            nextButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            nextButton.Location = new System.Drawing.Point(638, 479);
            nextButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            nextButton.Name = "nextButton";
            nextButton.Size = new System.Drawing.Size(88, 27);
            nextButton.TabIndex = 1;
            nextButton.Text = "Next >";
            nextButton.UseVisualStyleBackColor = true;
            nextButton.Click += nextButton_Click;
            // 
            // finishButton
            // 
            finishButton.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            finishButton.Location = new System.Drawing.Point(733, 479);
            finishButton.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            finishButton.Name = "finishButton";
            finishButton.Size = new System.Drawing.Size(88, 27);
            finishButton.TabIndex = 1;
            finishButton.Text = "Finish";
            finishButton.UseVisualStyleBackColor = true;
            finishButton.Visible = false;
            // 
            // bannerLabel
            // 
            bannerLabel.AutoSize = true;
            bannerLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, 0);
            bannerLabel.Location = new System.Drawing.Point(15, 10);
            bannerLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            bannerLabel.Name = "bannerLabel";
            bannerLabel.Size = new System.Drawing.Size(119, 20);
            bannerLabel.TabIndex = 2;
            bannerLabel.Text = "<bannerText>";
            // 
            // PatientCollectionsRunWizard
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(933, 519);
            Controls.Add(bannerLabel);
            Controls.Add(finishButton);
            Controls.Add(nextButton);
            Controls.Add(backButton);
            Controls.Add(cancelButton);
            Controls.Add(tabControl1);
            ForeColor = System.Drawing.Color.Black;
            HelpButton = true;
            Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
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
    }
}