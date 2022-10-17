namespace LabBilling
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.panel1 = new System.Windows.Forms.Panel();
            this.impersonateUserComboBox = new System.Windows.Forms.ComboBox();
            this.impersonateUserLabel = new System.Windows.Forms.Label();
            this.IntegratedAuthentication = new System.Windows.Forms.CheckBox();
            this.domain = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.statustext = new System.Windows.Forms.TextBox();
            this.password = new System.Windows.Forms.TextBox();
            this.LoginButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.LoginTitle = new System.Windows.Forms.Label();
            this.username = new System.Windows.Forms.TextBox();
            this.setupImage = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setupImage)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.setupImage);
            this.panel1.Controls.Add(this.impersonateUserComboBox);
            this.panel1.Controls.Add(this.impersonateUserLabel);
            this.panel1.Controls.Add(this.IntegratedAuthentication);
            this.panel1.Controls.Add(this.domain);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.pictureBox2);
            this.panel1.Controls.Add(this.statustext);
            this.panel1.Controls.Add(this.password);
            this.panel1.Controls.Add(this.LoginButton);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.LoginTitle);
            this.panel1.Controls.Add(this.username);
            this.panel1.Location = new System.Drawing.Point(10, 10);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(483, 330);
            this.panel1.TabIndex = 0;
            // 
            // impersonateUserComboBox
            // 
            this.impersonateUserComboBox.FormattingEnabled = true;
            this.impersonateUserComboBox.Location = new System.Drawing.Point(12, 270);
            this.impersonateUserComboBox.Name = "impersonateUserComboBox";
            this.impersonateUserComboBox.Size = new System.Drawing.Size(151, 21);
            this.impersonateUserComboBox.TabIndex = 13;
            this.impersonateUserComboBox.Visible = false;
            this.impersonateUserComboBox.SelectedValueChanged += new System.EventHandler(this.impersonateUserComboBox_SelectedValueChanged);
            // 
            // impersonateUserLabel
            // 
            this.impersonateUserLabel.AutoSize = true;
            this.impersonateUserLabel.Location = new System.Drawing.Point(9, 248);
            this.impersonateUserLabel.Name = "impersonateUserLabel";
            this.impersonateUserLabel.Size = new System.Drawing.Size(90, 13);
            this.impersonateUserLabel.TabIndex = 12;
            this.impersonateUserLabel.Text = "Impersonate User";
            this.impersonateUserLabel.Visible = false;
            // 
            // IntegratedAuthentication
            // 
            this.IntegratedAuthentication.AutoSize = true;
            this.IntegratedAuthentication.Checked = true;
            this.IntegratedAuthentication.CheckState = System.Windows.Forms.CheckState.Checked;
            this.IntegratedAuthentication.Enabled = false;
            this.IntegratedAuthentication.Location = new System.Drawing.Point(12, 220);
            this.IntegratedAuthentication.Name = "IntegratedAuthentication";
            this.IntegratedAuthentication.Size = new System.Drawing.Size(145, 17);
            this.IntegratedAuthentication.TabIndex = 11;
            this.IntegratedAuthentication.Text = "Integrated Authentication";
            this.IntegratedAuthentication.UseVisualStyleBackColor = true;
            this.IntegratedAuthentication.CheckedChanged += new System.EventHandler(this.IntegratedAuthentication_CheckedChanged);
            // 
            // domain
            // 
            this.domain.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.domain.Location = new System.Drawing.Point(204, 172);
            this.domain.Name = "domain";
            this.domain.Size = new System.Drawing.Size(258, 26);
            this.domain.TabIndex = 6;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(202, 156);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(43, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Domain";
            // 
            // statustext
            // 
            this.statustext.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.statustext.Location = new System.Drawing.Point(202, 213);
            this.statustext.Multiline = true;
            this.statustext.Name = "statustext";
            this.statustext.ReadOnly = true;
            this.statustext.Size = new System.Drawing.Size(256, 43);
            this.statustext.TabIndex = 7;
            this.statustext.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // password
            // 
            this.password.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.password.Location = new System.Drawing.Point(204, 119);
            this.password.Name = "password";
            this.password.Size = new System.Drawing.Size(258, 26);
            this.password.TabIndex = 4;
            this.password.UseSystemPasswordChar = true;
            // 
            // LoginButton
            // 
            this.LoginButton.BackColor = System.Drawing.Color.Cyan;
            this.LoginButton.FlatAppearance.BorderColor = System.Drawing.Color.White;
            this.LoginButton.FlatAppearance.BorderSize = 0;
            this.LoginButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.LoginButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginButton.Location = new System.Drawing.Point(204, 270);
            this.LoginButton.Name = "LoginButton";
            this.LoginButton.Size = new System.Drawing.Size(254, 38);
            this.LoginButton.TabIndex = 8;
            this.LoginButton.Text = "Login";
            this.LoginButton.UseVisualStyleBackColor = false;
            this.LoginButton.Click += new System.EventHandler(this.LoginButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(202, 103);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Password";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(203, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Username";
            // 
            // LoginTitle
            // 
            this.LoginTitle.AutoSize = true;
            this.LoginTitle.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.LoginTitle.Location = new System.Drawing.Point(8, 13);
            this.LoginTitle.Name = "LoginTitle";
            this.LoginTitle.Size = new System.Drawing.Size(294, 25);
            this.LoginTitle.TabIndex = 0;
            this.LoginTitle.Text = "Lab Patient Accounting Login";
            // 
            // username
            // 
            this.username.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.username.Location = new System.Drawing.Point(204, 68);
            this.username.Name = "username";
            this.username.Size = new System.Drawing.Size(258, 26);
            this.username.TabIndex = 2;
            // 
            // setupImage
            // 
            this.setupImage.Image = global::LabBilling.Properties.Resources.kisspng_gear_spanners_computer_icons_gear_5abf330af2b385_0314791715224798829941;
            this.setupImage.Location = new System.Drawing.Point(12, 297);
            this.setupImage.Name = "setupImage";
            this.setupImage.Size = new System.Drawing.Size(32, 30);
            this.setupImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.setupImage.TabIndex = 14;
            this.setupImage.TabStop = false;
            this.setupImage.Click += new System.EventHandler(this.setupImage_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(441, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(17, 17);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.Image = global::LabBilling.Properties.Resources.client_login_icon_4;
            this.pictureBox2.Location = new System.Drawing.Point(13, 68);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(157, 146);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 6;
            this.pictureBox2.TabStop = false;
            // 
            // Login
            // 
            this.AcceptButton = this.LoginButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 352);
            this.Controls.Add(this.panel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.setupImage)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.TextBox password;
        private System.Windows.Forms.Button LoginButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label LoginTitle;
        private System.Windows.Forms.TextBox username;
        private System.Windows.Forms.TextBox statustext;
        private System.Windows.Forms.TextBox domain;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox IntegratedAuthentication;
        private System.Windows.Forms.ComboBox impersonateUserComboBox;
        private System.Windows.Forms.Label impersonateUserLabel;
        private System.Windows.Forms.PictureBox setupImage;
    }
}