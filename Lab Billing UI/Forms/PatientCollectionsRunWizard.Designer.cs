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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.sendCollectionsTabPage = new System.Windows.Forms.TabPage();
            this.sendToCollectionsTextbox = new System.Windows.Forms.TextBox();
            this.sendToCollectionsProgressBar = new System.Windows.Forms.ProgressBar();
            this.sendToCollectionsStartButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.compileStatementsTabPage = new System.Windows.Forms.TabPage();
            this.compileStmtsStartButton = new System.Windows.Forms.Button();
            this.compileStatementsTextBox = new System.Windows.Forms.TextBox();
            this.compileStatementsProgressBar = new System.Windows.Forms.ProgressBar();
            this.label2 = new System.Windows.Forms.Label();
            this.createStmtFileTabPage = new System.Windows.Forms.TabPage();
            this.createStmtFileStartButton = new System.Windows.Forms.Button();
            this.createStmtFileTextBox = new System.Windows.Forms.TextBox();
            this.createStmtFileProgressBar = new System.Windows.Forms.ProgressBar();
            this.label3 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.backButton = new System.Windows.Forms.Button();
            this.nextButton = new System.Windows.Forms.Button();
            this.finishButton = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.throughDateLabel = new System.Windows.Forms.Label();
            this.batchNoLabel = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.sendCollectionsTabPage.SuspendLayout();
            this.compileStatementsTabPage.SuspendLayout();
            this.createStmtFileTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.sendCollectionsTabPage);
            this.tabControl1.Controls.Add(this.compileStatementsTabPage);
            this.tabControl1.Controls.Add(this.createStmtFileTabPage);
            this.tabControl1.Location = new System.Drawing.Point(12, 32);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(776, 376);
            this.tabControl1.TabIndex = 0;
            // 
            // sendCollectionsTabPage
            // 
            this.sendCollectionsTabPage.Controls.Add(this.sendToCollectionsTextbox);
            this.sendCollectionsTabPage.Controls.Add(this.sendToCollectionsProgressBar);
            this.sendCollectionsTabPage.Controls.Add(this.sendToCollectionsStartButton);
            this.sendCollectionsTabPage.Controls.Add(this.label1);
            this.sendCollectionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.sendCollectionsTabPage.Name = "sendCollectionsTabPage";
            this.sendCollectionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.sendCollectionsTabPage.Size = new System.Drawing.Size(768, 350);
            this.sendCollectionsTabPage.TabIndex = 0;
            this.sendCollectionsTabPage.Text = "Send to Collections";
            this.sendCollectionsTabPage.UseVisualStyleBackColor = true;
            // 
            // sendToCollectionsTextbox
            // 
            this.sendToCollectionsTextbox.Location = new System.Drawing.Point(39, 121);
            this.sendToCollectionsTextbox.Multiline = true;
            this.sendToCollectionsTextbox.Name = "sendToCollectionsTextbox";
            this.sendToCollectionsTextbox.Size = new System.Drawing.Size(530, 189);
            this.sendToCollectionsTextbox.TabIndex = 3;
            // 
            // sendToCollectionsProgressBar
            // 
            this.sendToCollectionsProgressBar.Location = new System.Drawing.Point(39, 92);
            this.sendToCollectionsProgressBar.Name = "sendToCollectionsProgressBar";
            this.sendToCollectionsProgressBar.Size = new System.Drawing.Size(530, 23);
            this.sendToCollectionsProgressBar.TabIndex = 2;
            // 
            // sendToCollectionsStartButton
            // 
            this.sendToCollectionsStartButton.Location = new System.Drawing.Point(592, 92);
            this.sendToCollectionsStartButton.Name = "sendToCollectionsStartButton";
            this.sendToCollectionsStartButton.Size = new System.Drawing.Size(75, 23);
            this.sendToCollectionsStartButton.TabIndex = 1;
            this.sendToCollectionsStartButton.Text = "Start";
            this.sendToCollectionsStartButton.UseVisualStyleBackColor = true;
            this.sendToCollectionsStartButton.Click += new System.EventHandler(this.sendToCollectionsStartButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(533, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "This step will generate the collections file to send to MSCB, and write off those" +
    " accounts. Click Start to proceed.";
            // 
            // compileStatementsTabPage
            // 
            this.compileStatementsTabPage.Controls.Add(this.batchNoLabel);
            this.compileStatementsTabPage.Controls.Add(this.throughDateLabel);
            this.compileStatementsTabPage.Controls.Add(this.label5);
            this.compileStatementsTabPage.Controls.Add(this.label4);
            this.compileStatementsTabPage.Controls.Add(this.compileStmtsStartButton);
            this.compileStatementsTabPage.Controls.Add(this.compileStatementsTextBox);
            this.compileStatementsTabPage.Controls.Add(this.compileStatementsProgressBar);
            this.compileStatementsTabPage.Controls.Add(this.label2);
            this.compileStatementsTabPage.Location = new System.Drawing.Point(4, 22);
            this.compileStatementsTabPage.Name = "compileStatementsTabPage";
            this.compileStatementsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.compileStatementsTabPage.Size = new System.Drawing.Size(768, 350);
            this.compileStatementsTabPage.TabIndex = 1;
            this.compileStatementsTabPage.Text = "Compile Statements";
            this.compileStatementsTabPage.UseVisualStyleBackColor = true;
            // 
            // compileStmtsStartButton
            // 
            this.compileStmtsStartButton.Location = new System.Drawing.Point(465, 109);
            this.compileStmtsStartButton.Name = "compileStmtsStartButton";
            this.compileStmtsStartButton.Size = new System.Drawing.Size(75, 23);
            this.compileStmtsStartButton.TabIndex = 3;
            this.compileStmtsStartButton.Text = "Start";
            this.compileStmtsStartButton.UseVisualStyleBackColor = true;
            this.compileStmtsStartButton.Click += new System.EventHandler(this.compileStmtsStartButton_Click);
            // 
            // compileStatementsTextBox
            // 
            this.compileStatementsTextBox.Location = new System.Drawing.Point(65, 150);
            this.compileStatementsTextBox.Multiline = true;
            this.compileStatementsTextBox.Name = "compileStatementsTextBox";
            this.compileStatementsTextBox.Size = new System.Drawing.Size(374, 176);
            this.compileStatementsTextBox.TabIndex = 2;
            // 
            // compileStatementsProgressBar
            // 
            this.compileStatementsProgressBar.Location = new System.Drawing.Point(65, 109);
            this.compileStatementsProgressBar.Name = "compileStatementsProgressBar";
            this.compileStatementsProgressBar.Size = new System.Drawing.Size(374, 23);
            this.compileStatementsProgressBar.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(62, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(377, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "This step will compile the records to receive statements. Click Start to proceed." +
    "";
            // 
            // createStmtFileTabPage
            // 
            this.createStmtFileTabPage.Controls.Add(this.createStmtFileStartButton);
            this.createStmtFileTabPage.Controls.Add(this.createStmtFileTextBox);
            this.createStmtFileTabPage.Controls.Add(this.createStmtFileProgressBar);
            this.createStmtFileTabPage.Controls.Add(this.label3);
            this.createStmtFileTabPage.Location = new System.Drawing.Point(4, 22);
            this.createStmtFileTabPage.Name = "createStmtFileTabPage";
            this.createStmtFileTabPage.Size = new System.Drawing.Size(768, 350);
            this.createStmtFileTabPage.TabIndex = 2;
            this.createStmtFileTabPage.Text = "Create Statement File";
            this.createStmtFileTabPage.UseVisualStyleBackColor = true;
            // 
            // createStmtFileStartButton
            // 
            this.createStmtFileStartButton.Location = new System.Drawing.Point(481, 71);
            this.createStmtFileStartButton.Name = "createStmtFileStartButton";
            this.createStmtFileStartButton.Size = new System.Drawing.Size(75, 23);
            this.createStmtFileStartButton.TabIndex = 3;
            this.createStmtFileStartButton.Text = "Start";
            this.createStmtFileStartButton.UseVisualStyleBackColor = true;
            this.createStmtFileStartButton.Click += new System.EventHandler(this.createStmtFileStartButton_Click);
            // 
            // createStmtFileTextBox
            // 
            this.createStmtFileTextBox.Location = new System.Drawing.Point(69, 100);
            this.createStmtFileTextBox.Multiline = true;
            this.createStmtFileTextBox.Name = "createStmtFileTextBox";
            this.createStmtFileTextBox.Size = new System.Drawing.Size(379, 153);
            this.createStmtFileTextBox.TabIndex = 2;
            // 
            // createStmtFileProgressBar
            // 
            this.createStmtFileProgressBar.Location = new System.Drawing.Point(69, 71);
            this.createStmtFileProgressBar.Name = "createStmtFileProgressBar";
            this.createStmtFileProgressBar.Size = new System.Drawing.Size(379, 23);
            this.createStmtFileProgressBar.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(58, 42);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(390, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "This step will generate the statement file to be sent to DNI. Click Start to proc" +
    "eed.";
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.Location = new System.Drawing.Point(434, 415);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 1;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // backButton
            // 
            this.backButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.backButton.Location = new System.Drawing.Point(515, 415);
            this.backButton.Name = "backButton";
            this.backButton.Size = new System.Drawing.Size(75, 23);
            this.backButton.TabIndex = 1;
            this.backButton.Text = "< Back";
            this.backButton.UseVisualStyleBackColor = true;
            // 
            // nextButton
            // 
            this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nextButton.Location = new System.Drawing.Point(596, 415);
            this.nextButton.Name = "nextButton";
            this.nextButton.Size = new System.Drawing.Size(75, 23);
            this.nextButton.TabIndex = 1;
            this.nextButton.Text = "Next >";
            this.nextButton.UseVisualStyleBackColor = true;
            // 
            // finishButton
            // 
            this.finishButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.finishButton.Location = new System.Drawing.Point(677, 415);
            this.finishButton.Name = "finishButton";
            this.finishButton.Size = new System.Drawing.Size(75, 23);
            this.finishButton.TabIndex = 1;
            this.finishButton.Text = "Finish";
            this.finishButton.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(62, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Through Date:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(62, 72);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(55, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Batch No:";
            // 
            // throughDateLabel
            // 
            this.throughDateLabel.AutoSize = true;
            this.throughDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.throughDateLabel.Location = new System.Drawing.Point(144, 39);
            this.throughDateLabel.Name = "throughDateLabel";
            this.throughDateLabel.Size = new System.Drawing.Size(122, 18);
            this.throughDateLabel.TabIndex = 5;
            this.throughDateLabel.Text = "<through date>";
            // 
            // batchNoLabel
            // 
            this.batchNoLabel.AutoSize = true;
            this.batchNoLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.batchNoLabel.Location = new System.Drawing.Point(144, 68);
            this.batchNoLabel.Name = "batchNoLabel";
            this.batchNoLabel.Size = new System.Drawing.Size(91, 18);
            this.batchNoLabel.TabIndex = 5;
            this.batchNoLabel.Text = "<batchNo>";
            // 
            // PatientCollectionsRunWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.finishButton);
            this.Controls.Add(this.nextButton);
            this.Controls.Add(this.backButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.tabControl1);
            this.Name = "PatientCollectionsRunWizard";
            this.Text = "Patient Collections Run";
            this.Load += new System.EventHandler(this.PatientCollectionsRunWizard_Load);
            this.tabControl1.ResumeLayout(false);
            this.sendCollectionsTabPage.ResumeLayout(false);
            this.sendCollectionsTabPage.PerformLayout();
            this.compileStatementsTabPage.ResumeLayout(false);
            this.compileStatementsTabPage.PerformLayout();
            this.createStmtFileTabPage.ResumeLayout(false);
            this.createStmtFileTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
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
    }
}