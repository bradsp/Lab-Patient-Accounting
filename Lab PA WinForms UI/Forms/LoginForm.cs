using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LabBilling.Forms;
public partial class LoginForm : Form
{
    public string Username => txtUsername.Text;
    public string Password => txtPassword.Text;

    public LoginForm()
    {
        InitializeComponent();
    }

    private void btnLogin_Click(object sender, EventArgs e)
    {
        // Perform any necessary validation here

        // Set the DialogResult to OK to indicate successful input
        this.DialogResult = DialogResult.OK;
        this.Close();
    }

    private void btnCancel_Click(object sender, EventArgs e)
    {
        // Set the DialogResult to Cancel to indicate cancellation
        this.DialogResult = DialogResult.Cancel;
        this.Close();
    }

    private void showPasswordCheckBox_CheckedChanged(object sender, EventArgs e)
    {
        if (showPasswordCheckBox.Checked)
        {
            txtPassword.PasswordChar = '\0';
        }
        else
        {
            txtPassword.PasswordChar = '*';
        }
    }

    private void txtPassword_TextChanged(object sender, EventArgs e)
    {

    }
}
