namespace LabBilling.Forms;

partial class LoginForm
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
        txtUsername = new TextBox();
        txtPassword = new TextBox();
        btnLogin = new Button();
        pictureBox1 = new PictureBox();
        label1 = new Label();
        showPasswordCheckBox = new CheckBox();
        pictureBox2 = new PictureBox();
        label2 = new Label();
        ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
        SuspendLayout();
        // 
        // txtUsername
        // 
        txtUsername.Font = new Font("Segoe UI", 12F);
        txtUsername.Location = new Point(306, 124);
        txtUsername.Name = "txtUsername";
        txtUsername.PlaceholderText = "Username";
        txtUsername.Size = new Size(294, 29);
        txtUsername.TabIndex = 1;
        // 
        // txtPassword
        // 
        txtPassword.Font = new Font("Segoe UI", 12F);
        txtPassword.Location = new Point(306, 159);
        txtPassword.Name = "txtPassword";
        txtPassword.PasswordChar = '*';
        txtPassword.PlaceholderText = "Password";
        txtPassword.Size = new Size(294, 29);
        txtPassword.TabIndex = 3;
        // 
        // btnLogin
        // 
        btnLogin.BackColor = Color.DeepSkyBlue;
        btnLogin.FlatStyle = FlatStyle.Flat;
        btnLogin.Font = new Font("Segoe UI", 12F);
        btnLogin.ForeColor = SystemColors.ButtonHighlight;
        btnLogin.Location = new Point(306, 243);
        btnLogin.Name = "btnLogin";
        btnLogin.Size = new Size(294, 32);
        btnLogin.TabIndex = 4;
        btnLogin.Text = "Login";
        btnLogin.UseVisualStyleBackColor = false;
        btnLogin.Click += btnLogin_Click;
        // 
        // pictureBox1
        // 
        pictureBox1.Image = Properties.Resources.logoicon2;
        pictureBox1.Location = new Point(62, 34);
        pictureBox1.Name = "pictureBox1";
        pictureBox1.Padding = new Padding(5);
        pictureBox1.Size = new Size(152, 154);
        pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        pictureBox1.TabIndex = 6;
        pictureBox1.TabStop = false;
        // 
        // label1
        // 
        label1.FlatStyle = FlatStyle.Flat;
        label1.Font = new Font("Berlin Sans FB", 18F);
        label1.Location = new Point(62, 191);
        label1.Name = "label1";
        label1.Size = new Size(152, 76);
        label1.TabIndex = 7;
        label1.Text = "Lab Patient Accounting";
        label1.TextAlign = ContentAlignment.MiddleCenter;
        // 
        // showPasswordCheckBox
        // 
        showPasswordCheckBox.AutoSize = true;
        showPasswordCheckBox.Location = new Point(492, 194);
        showPasswordCheckBox.Name = "showPasswordCheckBox";
        showPasswordCheckBox.Size = new Size(108, 19);
        showPasswordCheckBox.TabIndex = 8;
        showPasswordCheckBox.Text = "Show Password";
        showPasswordCheckBox.UseVisualStyleBackColor = true;
        showPasswordCheckBox.CheckedChanged += showPasswordCheckBox_CheckedChanged;
        // 
        // pictureBox2
        // 
        pictureBox2.Image = Properties.Resources.Cancel;
        pictureBox2.Location = new Point(612, 12);
        pictureBox2.Name = "pictureBox2";
        pictureBox2.Size = new Size(27, 26);
        pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
        pictureBox2.TabIndex = 9;
        pictureBox2.TabStop = false;
        // 
        // label2
        // 
        label2.AutoSize = true;
        label2.Font = new Font("Segoe UI", 14F);
        label2.Location = new Point(419, 67);
        label2.Name = "label2";
        label2.Size = new Size(66, 25);
        label2.TabIndex = 10;
        label2.Text = "LOGIN";
        // 
        // LoginForm
        // 
        AutoScaleDimensions = new SizeF(7F, 15F);
        AutoScaleMode = AutoScaleMode.Font;
        BackColor = Color.White;
        ClientSize = new Size(651, 321);
        Controls.Add(label2);
        Controls.Add(pictureBox2);
        Controls.Add(showPasswordCheckBox);
        Controls.Add(label1);
        Controls.Add(pictureBox1);
        Controls.Add(txtUsername);
        Controls.Add(txtPassword);
        Controls.Add(btnLogin);
        Font = new Font("Segoe UI", 9F);
        FormBorderStyle = FormBorderStyle.None;
        MaximizeBox = false;
        MinimizeBox = false;
        Name = "LoginForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "Login";
        ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
        ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
        ResumeLayout(false);
        PerformLayout();
    }

    private TextBox txtUsername;
    private TextBox txtPassword;
    private Button btnLogin;

    #endregion

    private PictureBox pictureBox1;
    private Label label1;
    private CheckBox showPasswordCheckBox;
    private PictureBox pictureBox2;
    private Label label2;
}